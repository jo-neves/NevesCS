using NevesCS.Abstractions.Interfaces;
using NevesCS.Abstractions.Services;
using NevesCS.NonStatic.Patterns;

using Solnet.Rpc;

namespace NevesCS.NonStatic.Clients.Web3.Solana.Factories
{
    public sealed class HttpSolanaClientManagedFactory : IManagedServiceFactory<HttpSolanaClient>
    {
        private readonly HttpSolanaClientOptions Options;

        private readonly ICachedServiceFactory<IRpcClient> RpcClientFactory;

        private readonly IJsonParser JsonParser;

        private readonly CancellationToken CancellationToken;

        public HttpSolanaClientManagedFactory(
            HttpSolanaClientOptions options,
            ICachedServiceFactory<IRpcClient> rpcClientFactory,
            IJsonParser jsonParser,
            CancellationToken cancellationToken)
        {
            Options = options;
            RpcClientFactory = rpcClientFactory;
            JsonParser = jsonParser;
            CancellationToken = cancellationToken;
        }

        public HttpSolanaClient Create(string key)
        {
            return new HttpSolanaClient(Options, RpcClientFactory.Create(key), JsonParser, CancellationToken);
        }

        public static ICachedServiceFactory<HttpSolanaClient> CreateNewManager(
            CachedFactoryOptions cachedFactoryOptions,
            HttpSolanaClientOptions httpSolanaClientOptions,
            ICachedServiceFactory<IRpcClient> rpcClientFactory,
            IJsonParser jsonParser,
            CancellationToken cancellationToken = default)
        {
            return new CachedServiceFactoryManager<HttpSolanaClient>(
                cachedFactoryOptions,
                new HttpSolanaClientManagedFactory(httpSolanaClientOptions, rpcClientFactory, jsonParser, cancellationToken),
                cancellationToken);
        }
    }
}
