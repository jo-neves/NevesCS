using NevesCS.Abstractions.Clients.Web3.SolanaJupiterHttpApi;
using NevesCS.Abstractions.Interfaces;
using NevesCS.NonStatic.Patterns;
using NevesCS.Static.Utils;

namespace NevesCS.NonStatic.Clients.Web3.SolanaJupiterHttpApi.Factories
{
    public sealed class SolanaJupiterV6PublicRestClientManagedFactory
        : IManagedServiceFactory<ISolanaJupiterV6PublicClient>
    {
        private readonly ICachedServiceFactory<HttpClient> HttpClientCachedFactory;

        public SolanaJupiterV6PublicRestClientManagedFactory(ICachedServiceFactory<HttpClient> httpClientCachedFactory)
        {
            HttpClientCachedFactory = ObjectUtils.ThrowIfNull(httpClientCachedFactory, nameof(httpClientCachedFactory));
        }

        public ISolanaJupiterV6PublicClient Create(string key)
        {
            return new SolanaJupiterV6PublicRestClient(HttpClientCachedFactory.Create(key));
        }

        public static ICachedServiceFactory<ISolanaJupiterV6PublicClient> CreateNewManager(
            CachedFactoryOptions options,
            ICachedServiceFactory<HttpClient> httpClientCachedFactory,
            CancellationToken cancellationToken = default)
        {
            return new CachedServiceFactoryManager<ISolanaJupiterV6PublicClient>(
                options,
                new SolanaJupiterV6PublicRestClientManagedFactory(httpClientCachedFactory),
                cancellationToken);
        }
    }
}
