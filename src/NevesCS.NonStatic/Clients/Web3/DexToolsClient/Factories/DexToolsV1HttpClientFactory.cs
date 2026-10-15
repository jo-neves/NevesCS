using NevesCS.Abstractions.Clients.Web3.DexTools;
using NevesCS.Abstractions.Interfaces;
using NevesCS.Abstractions.Services;
using NevesCS.Static.Utils;

namespace NevesCS.NonStatic.Clients.Web3.DexToolsClient.Factories
{
    public sealed class DexToolsV1HttpClientFactory(
        IHttpClientFactory httpClientFactory,
        IThreadRateLimiter? rateLimiter)

        : IServiceFactory<IDexToolsClient>
    {
        private readonly IThreadRateLimiter? ThreadRateLimiter = rateLimiter;

        private readonly IHttpClientFactory HttpClientFactory = ObjectUtils.ThrowIfNull(httpClientFactory, nameof(httpClientFactory));

        public DexToolsV1HttpClientFactory(IHttpClientFactory httpClientFactory)
            : this(httpClientFactory, null)
        {
        }

        public IDexToolsClient Create()
        {
            return new DexToolsV1HttpClient(HttpClientFactory.CreateClient(), ThreadRateLimiter);
        }
    }
}
