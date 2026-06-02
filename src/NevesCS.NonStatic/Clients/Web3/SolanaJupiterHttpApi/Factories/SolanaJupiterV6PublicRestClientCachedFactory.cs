using NevesCS.Abstractions.Interfaces;
using NevesCS.NonStatic.Patterns;
using NevesCS.Static.Utils;

namespace NevesCS.NonStatic.Clients.Web3.SolanaJupiterHttpApi.Factories
{
    public sealed class SolanaJupiterV6PublicRestClientCachedFactory
        : ICachedServiceFactory<SolanaJupiterV6PublicRestClient>
    {
        private readonly ICachedServiceFactory<HttpClient> HttpClientCachedFactory;

        public SolanaJupiterV6PublicRestClientCachedFactory(ICachedServiceFactory<HttpClient> httpClientCachedFactory)
        {
            HttpClientCachedFactory = ObjectUtils.ThrowIfNull(httpClientCachedFactory, nameof(httpClientCachedFactory));
        }

        public SolanaJupiterV6PublicRestClient Create(string key)
        {
            return new SolanaJupiterV6PublicRestClient(HttpClientCachedFactory.Create(key));
        }

        public static CachedServiceFactoryManager<SolanaJupiterV6PublicRestClient> CreateNewManager(
            CachedFactoryOptions options,
            ICachedServiceFactory<HttpClient> httpClientCachedFactory,
            CancellationToken cancellationToken = default)
        {
            return new CachedServiceFactoryManager<SolanaJupiterV6PublicRestClient>(
                options,
                new SolanaJupiterV6PublicRestClientCachedFactory(httpClientCachedFactory),
                cancellationToken);
        }
    }
}
