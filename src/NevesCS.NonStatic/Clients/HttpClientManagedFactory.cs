using NevesCS.Abstractions.Interfaces;
using NevesCS.NonStatic.Patterns;
using NevesCS.Static.Utils;

namespace NevesCS.NonStatic.Clients
{
    public sealed class HttpClientManagedFactory : IManagedServiceFactory<HttpClient>
    {
        private readonly IHttpClientFactory HttpClientFactory;

        public HttpClientManagedFactory(IHttpClientFactory httpClientFactory)
        {
            HttpClientFactory = ObjectUtils.ThrowIfNull(httpClientFactory, nameof(httpClientFactory));
        }

        public HttpClient Create(string key)
        {
            return HttpClientFactory.CreateClient(key);
        }

        public static CachedServiceFactoryManager<HttpClient> CreateNewManager(
            CachedFactoryOptions options,
            IHttpClientFactory httpClientFactory,
            CancellationToken cancellationToken = default)
        {
            return new CachedServiceFactoryManager<HttpClient>(
                options,
                new HttpClientManagedFactory(httpClientFactory),
                cancellationToken);
        }
    }
}
