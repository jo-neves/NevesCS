using NevesCS.Abstractions.Clients.Web3.DexTools;
using NevesCS.Abstractions.Interfaces;
using NevesCS.Abstractions.Services;
using NevesCS.NonStatic.Patterns;
using NevesCS.Static.Utils;

namespace NevesCS.NonStatic.Clients.Web3.DexToolsClient.Factories
{
    public sealed class DexToolsV1HttpClientCachedFactory : ICachedServiceFactory<IDexToolsClient>
    {
        private readonly IThreadRateLimiter? ThreadRateLimiter;

        private readonly ICachedServiceFactory<HttpClient> HttpClientCachedFactory;

        public DexToolsV1HttpClientCachedFactory(ICachedServiceFactory<HttpClient> httpClientCachedFactory)
        {
            HttpClientCachedFactory = ObjectUtils.ThrowIfNull(httpClientCachedFactory, nameof(httpClientCachedFactory));
        }

        public DexToolsV1HttpClientCachedFactory(
            ICachedServiceFactory<HttpClient> httpClientCachedFactory,
            IThreadRateLimiter rateLimiter)
        {
            HttpClientCachedFactory = ObjectUtils.ThrowIfNull(httpClientCachedFactory, nameof(httpClientCachedFactory));
            ThreadRateLimiter = ObjectUtils.ThrowIfNull(rateLimiter, nameof(rateLimiter));
        }

        public IDexToolsClient Create(string key)
        {
            return ThreadRateLimiter == null
                ? new DexToolsV1HttpClient(HttpClientCachedFactory.Create(key))
                : new DexToolsV1HttpClient(HttpClientCachedFactory.Create(key), ThreadRateLimiter);
        }

        public static CachedServiceFactoryManager<IDexToolsClient> CreateNewManager(
            CachedFactoryOptions options,
            ICachedServiceFactory<HttpClient> httpClientCachedFactory,
            CancellationToken cancellationToken = default)
        {
            return new CachedServiceFactoryManager<IDexToolsClient>(
                options,
                new DexToolsV1HttpClientCachedFactory(httpClientCachedFactory),
                cancellationToken);
        }

        public static CachedServiceFactoryManager<IDexToolsClient> CreateNewManager(
            CachedFactoryOptions options,
            ICachedServiceFactory<HttpClient> httpClientCachedFactory,
            IThreadRateLimiter rateLimiter,
            CancellationToken cancellationToken = default)
        {
            return new CachedServiceFactoryManager<IDexToolsClient>(
                options,
                new DexToolsV1HttpClientCachedFactory(httpClientCachedFactory, rateLimiter),
                cancellationToken);
        }
    }
}
