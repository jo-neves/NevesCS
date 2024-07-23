using NevesCS.Abstractions.Interfaces;
using NevesCS.NonStatic.Patterns;
using NevesCS.Static.Utils;

using Solnet.Rpc;

namespace NevesCS.NonStatic.Clients.Web3.Solana
{
    public sealed class SolnetRpcCientManagedFactory : IManagedServiceFactory<IRpcClient>
    {
        private readonly Cluster RpcClusterType;

        private readonly IHttpClientFactory HttpClientFactory;

        public SolnetRpcCientManagedFactory(IHttpClientFactory httpClientFactory, Cluster rpcClusterType)
        {
            HttpClientFactory = ObjectUtils.ThrowIfNull(httpClientFactory, nameof(httpClientFactory));
            RpcClusterType = ObjectUtils.ThrowIfNull(rpcClusterType, nameof(rpcClusterType));
        }

        public IRpcClient Create(string key)
        {
            return ClientFactory.GetClient(
                cluster: RpcClusterType,
                httpClient: HttpClientFactory.CreateClient($"{nameof(IRpcClient)}_{key}"));
        }

        public static ICachedServiceFactory<IRpcClient> CreateNewManager(
            CachedFactoryOptions options,
            IHttpClientFactory httpClientFactory,
            Cluster clusterType,
            CancellationToken cancellationToken = default)
        {
            return new CachedServiceFactoryManager<IRpcClient>(
                options,
                new SolnetRpcCientManagedFactory(httpClientFactory, clusterType),
                cancellationToken);
        }
    }
}
