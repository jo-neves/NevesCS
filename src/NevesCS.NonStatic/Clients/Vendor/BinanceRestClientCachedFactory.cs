using Binance.Net.Clients;
using CryptoExchange.Net.Authentication;

using NevesCS.Abstractions.Interfaces;
using NevesCS.NonStatic.Clients.Models;
using NevesCS.NonStatic.Patterns;
using NevesCS.Static.Utils;

namespace NevesCS.NonStatic.Clients;

public class BinanceRestClientCachedFactory : ICachedServiceFactory<BinanceRestClient>
{
    private readonly BinanceRestClientCachedFactoryOptions Options;

    public BinanceRestClientCachedFactory(BinanceRestClientCachedFactoryOptions options)
    {
        Options = ObjectUtils.AssertNotNull(options, nameof(options));
    }

    public BinanceRestClient Create(string key)
    {
        return new BinanceRestClient(
            o => o.ApiCredentials = new ApiCredentials(Options.ApiKey, Options.ApiSecret));
    }

    public static ICachedServiceFactory<BinanceRestClient> CreateNewManager(
            CachedFactoryOptions cacheOptions,
            BinanceRestClientCachedFactoryOptions serviceOptions,
            CancellationToken cancellationToken)
    {
        return new CachedServiceFactoryManager<BinanceRestClient>(
            cacheOptions,
            new BinanceRestClientCachedFactory(serviceOptions),
            cancellationToken);
    }
}
