using Microsoft.Extensions.Options;

using NevesCS.Abstractions.Services;
using NevesCS.AspNetCore.Abstractions.Interfaces;
using NevesCS.AspNetCore.Abstractions.Models.Configuration;
using NevesCS.Static.Extensions;
using NevesCS.Static.Utils.Vendor;
using Polly;

namespace NevesCS.AspNetCore.Adapters
{
    public sealed class DotnetHttpClient : IHttpClientAdapter
    {
        private readonly HttpClient _httpClient;

        private readonly IJsonParser _jsonClient;

        private readonly TimeSpan[] _sleepDurationPolicy;

        public DotnetHttpClient(
            IOptions<HttpConfig> appConfig,
            HttpClient httpClient,
            IJsonParser jsonClient)
        {
            _httpClient = httpClient.ThrowIfNull(nameof(httpClient));
            _jsonClient = jsonClient.ThrowIfNull(nameof(jsonClient));

            _sleepDurationPolicy = [.. PollyRetryPolicyUtils.GetSleepDurationPolicy(
                !appConfig.IsNull() ? appConfig.Value.NumberOfHttpRetries : 3,
                startMillisecondsDuration: 500)];
        }

        public async Task<TResponse?> GetAsync<TResponse>(string endpoint, CancellationToken cancellationToken = default)
        {
            endpoint.ThrowIfNull(nameof(endpoint));

            var pollyResponse = await Policy
                .Handle<HttpRequestException>()
                .WaitAndRetryAsync(_sleepDurationPolicy)
                .ExecuteAndCaptureAsync(
                    async (cancelToken) => await _httpClient.GetStreamAsync(endpoint, cancelToken),
                    cancellationToken);

            if (pollyResponse.FinalException is not null)
            {
                throw pollyResponse.FinalException;
            }

            var result = _jsonClient.DeserializeStream<TResponse?>(pollyResponse.Result);
            await pollyResponse.Result.DisposeAsync();

            return result;
        }
    }
}
