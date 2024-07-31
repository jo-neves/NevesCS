using NevesCS.Abstractions.Interfaces;
using NevesCS.Abstractions.Services;
using NevesCS.NonStatic.Patterns;
using NevesCS.Static.Utils;

using Solnet.Rpc;
using Solnet.Wallet;

namespace NevesCS.NonStatic.Clients.Web3.SolanaJupiterHttpApi.Factories
{
    public sealed class SolanaJupiterV6PrivateRestClientManagedFactory
        : IManagedServiceFactory<SolanaJupiterV6PrivateRestClient>
    {
        private readonly Wallet Wallet;

        private readonly ICachedServiceFactory<HttpClient> HttpClientCachedFactory;

        private readonly ICachedServiceFactory<IRpcClient> CachedRpcClientFactory;

        private readonly IJsonParser JsonParser;

        public SolanaJupiterV6PrivateRestClientManagedFactory(
            Wallet wallet,
            ICachedServiceFactory<HttpClient> httpClientCachedFactory,
            ICachedServiceFactory<IRpcClient> cachedRpcClientFactory,
            IJsonParser jsonParser)
        {
            Wallet = ObjectUtils.ThrowIfNull(wallet, nameof(wallet));
            HttpClientCachedFactory = ObjectUtils.ThrowIfNull(httpClientCachedFactory, nameof(httpClientCachedFactory));
            CachedRpcClientFactory = ObjectUtils.ThrowIfNull(cachedRpcClientFactory, nameof(cachedRpcClientFactory));
            JsonParser = ObjectUtils.ThrowIfNull(jsonParser, nameof(jsonParser));
        }

        public SolanaJupiterV6PrivateRestClient Create(string key)
        {
            return new SolanaJupiterV6PrivateRestClient(
                Wallet,
                CachedRpcClientFactory.Create(key),
                HttpClientCachedFactory.Create(key),
                JsonParser);
        }

        public static CachedServiceFactoryManager<SolanaJupiterV6PrivateRestClient> CreateNewManager(
            CachedFactoryOptions options,
            Wallet wallet,
            ICachedServiceFactory<HttpClient> httpClientCachedFactory,
            ICachedServiceFactory<IRpcClient> rpcClientCachedFactory,
            IJsonParser jsonParser,
            CancellationToken cancellationToken = default)
        {
            return new CachedServiceFactoryManager<SolanaJupiterV6PrivateRestClient>(
                options,
                new SolanaJupiterV6PrivateRestClientManagedFactory(
                    wallet,
                    httpClientCachedFactory,
                    rpcClientCachedFactory,
                    jsonParser),
                cancellationToken);
        }
    }
}
