using NevesCS.Abstractions.Clients.Web3.DexTools;
using NevesCS.Abstractions.Clients.Web3.DexTools.Models;
using NevesCS.Abstractions.Services;
using NevesCS.NonStatic.Clients.Web3.DexToolsClient.Extensions;
using NevesCS.NonStatic.Services.ThreadRateLimiters;
using NevesCS.Static.Utils;

namespace NevesCS.NonStatic.Clients.Web3.DexToolsClient;

// NOTE: >1d candles (2d,3d,...) are made from 1d candle data.
/// <summary>
/// This is a reverse engineered DexTools.io API client.
/// Use it at your own risk and pray that they implement and respect SemVer or other convention. <br/>
/// Not thread-safe due to the <see cref="HttpClient"/>, which is also not disposed.
///
/// </summary>
public sealed class DexToolsV1HttpClient : IDexToolsClient
{
    private const byte ApiVersion = 1;
    private const string BaseCoreApiUrl = "https://core-api.dextools.io";
    private const string BaseWebsiteUrl = "https://www.dextools.io";

    private readonly HttpClient HttpClient;

    private readonly IThreadRateLimiter ThreadRateLimiter;

    public DexToolsV1HttpClient(HttpClient httpClient)
    {
        HttpClient = httpClient;
        SetDefaultRequestHeaders();

        ThreadRateLimiter = new NoOpThreadRateLimiter();
    }

    public DexToolsV1HttpClient(HttpClient httpClient, IThreadRateLimiter rateLimiter)
    {
        HttpClient = httpClient;
        SetDefaultRequestHeaders();

        ThreadRateLimiter = rateLimiter;
    }

    private bool UserAgentWasUpdated;

    private void SetDefaultRequestHeaders()
    {
        HttpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        HttpClient.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
        HttpClient.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "cors");
        HttpClient.DefaultRequestHeaders.Add("Sec-Fetch-Site", "same-site");
        HttpClient.DefaultRequestHeaders.Add("User-Agent", HttpUserAgentUtils.DefaultUserAgent);

        HttpClient.DefaultRequestHeaders.Add("Origin", BaseWebsiteUrl);
        HttpClient.DefaultRequestHeaders.Add("Referer", BaseWebsiteUrl);
        HttpClient.DefaultRequestHeaders.Add("X-Api-Version", ApiVersion.ToString());
    }

    // NOTE: >1d candles (2d,3d,...) are made from 1d candle data.
    /// <summary>
    /// Not rate limited. <br/>
    ///
    /// <!-- https://core-api.dextools.io/pool/candles/solana/EGZ7tiLeH62TPV1gL8WwbXGzEPa9zmcpVnnkPKKnrE2U/usd/1h/month/latest?ts=1719946784&tz=0 -->
    /// </summary>
    public async Task<DexToolsV1GetPoolCandlesResponse> GetLatestPoolUsdCandlesAsync(
        DexToolsV1GetLatestPoolCandlesRequest request,
        CancellationToken cancellationToken = default)
    {
        if (!UserAgentWasUpdated && request.UseRandomUserAgent)
        {
            HttpClient.DefaultRequestHeaders.Remove("User-Agent");
            HttpClient.DefaultRequestHeaders.Add("User-Agent", HttpUserAgentUtils.GetNewRandomUserAgent());
            UserAgentWasUpdated = true;
        }

        return await DexToolsHttpClientUtils.TryGetOrThrowAsync<DexToolsV1GetPoolCandlesResponse>(
            HttpClient,
            $"{BaseCoreApiUrl}/pool/candles/{EnumUtils.GetDescription(request.Chain)}/{request.DexPoolAddress}/usd"
            + $"/{EnumUtils.GetDescription(request.CandleSize)}/{request.CandleSize.GetTimeSpanSize()}"
            + $"/latest?ts={DateTimeOffset.UtcNow.ToUnixTimeSeconds()}&tz=0",
            cancellationToken);
    }

    /// <summary>
    /// Rate limited if one is provided. <br/>
    ///
    /// <!-- https://core-api.dextools.io/pool/candles/solana/EGZ7tiLeH62TPV1gL8WwbXGzEPa9zmcpVnnkPKKnrE2U/usd/1h/350/amount?ts=1719921600&tz=1 -->
    /// </summary>
    public async Task<IList<DexToolsV1GetPoolCandlesResponseDataCandles>> GetPoolUsdCandlesAsync(
        DexToolsV1GetPoolCandlesRequest request,
        CancellationToken cancellationToken = default)
    {
        var latestCandles = await GetLatestPoolUsdCandlesAsync(
            new DexToolsV1GetLatestPoolCandlesRequest()
            {
                CandleSize = request.CandleSize,
                Chain = request.Chain,
                DexPoolAddress = request.DexPoolAddress,
                UseRandomUserAgent = request.UseRandomUserAgent,
            },
            cancellationToken);

        var allResponseCandles = new List<DexToolsV1GetPoolCandlesResponseDataCandles>(latestCandles.Data.Candles);

        var targetDate = request.FromUtcDate.ToUnixTimeSeconds();
        var nextTimestamp = latestCandles.Data.Next.Ts;
        var latestTimestamp = latestCandles.Data.Candles.LastOrDefault()?.LastTimestamp ?? 0;

        while (!cancellationToken.IsCancellationRequested && nextTimestamp > targetDate && latestTimestamp > targetDate)
        {
            await ThreadRateLimiter.WaitAsync(cancellationToken);

            var response = await DexToolsHttpClientUtils.TryGetOrThrowAsync<DexToolsV1GetPoolCandlesResponse>(
                HttpClient,
                $"{BaseCoreApiUrl}/pool/candles/{EnumUtils.GetDescription(request.Chain)}/{request.DexPoolAddress}/usd" +
                $"/{EnumUtils.GetDescription(request.CandleSize)}/{request.CandleSize.GetTotalNumberOfCandles()}" +
                $"/amount?ts={nextTimestamp}&tz=0",
                cancellationToken);

            if (response.Data.Total == 0)
            {
                break;
            }

            nextTimestamp = response.Data.Next.Ts;
            latestTimestamp = response.Data.Candles.LastOrDefault()?.LastTimestamp ?? 0;
            allResponseCandles.AddRange(response.Data.Candles);
        }

        return allResponseCandles;
    }
}
