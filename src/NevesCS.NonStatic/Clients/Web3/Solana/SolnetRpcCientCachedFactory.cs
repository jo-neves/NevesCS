using NevesCS.Abstractions.Interfaces;
using NevesCS.Static.Utils;

using Solnet.Rpc;

using System.Diagnostics.CodeAnalysis;

namespace NevesCS.NonStatic.Clients.Web3.Solana
{
    public sealed class SolnetRpcCientCachedFactory : ICachedServiceFactory<IRpcClient>
    {
        private readonly RpcClientCachedFactoryOptions Options;

        private readonly Cluster RpcClusterType;

        private readonly IHttpClientFactory HttpClientFactory;

        private readonly Dictionary<string, CacheItem> CachedRpcClients = [];

        public SolnetRpcCientCachedFactory(
            RpcClientCachedFactoryOptions options,
            Cluster rpcClusterType,
            IHttpClientFactory httpClientFactory)
        {
            Options = ObjectUtils.ThrowIfNull(options, nameof(options));
            RpcClusterType = ObjectUtils.ThrowIfNull(rpcClusterType, nameof(rpcClusterType));
            HttpClientFactory = ObjectUtils.ThrowIfNull(httpClientFactory, nameof(httpClientFactory));

            LastExpiredCacheItemsCheck = DateTimeOffset.UtcNow;
        }

        public IRpcClient Create(string key)
        {
            DeleteCacheItemIfExpired(key, null);
            CheckAndDeleteExpiredCacheItems();

            return DictionaryUtils
                .GetOrCreate(
                    CachedRpcClients,
                    key,
                    () => new CacheItem(ClientFactory.GetClient(
                        cluster: RpcClusterType,
                        httpClient: HttpClientFactory.CreateClient($"{nameof(IRpcClient)}_{key}"))))
                .RpcClient;
        }

        private DateTimeOffset LastExpiredCacheItemsCheck;

        private void CheckAndDeleteExpiredCacheItems()
        {
            var now = DateTimeOffset.UtcNow;

            if ((now - LastExpiredCacheItemsCheck) < Options.TimeBetweenExpiredCacheItemsChecks)
            {
                return;
            }

            foreach (var item in CachedRpcClients)
            {
                DeleteCacheItemIfExpired(item.Key, item.Value);
            }

            LastExpiredCacheItemsCheck = DateTimeOffset.UtcNow;
        }

        private void DeleteCacheItemIfExpired(string key, CacheItem? cacheItem)
        {
            cacheItem ??= CachedRpcClients.GetValueOrDefault(key);

            if (ObjectUtils.IsNull(cacheItem))
            {
                return;
            }

            if ((DateTimeOffset.UtcNow - cacheItem.Value.CreatedAt) > Options.MaxLifetime)
            {
                CachedRpcClients.Remove(key, out _);
            }
        }

        private readonly struct CacheItem
        {
            [SetsRequiredMembers]
            public CacheItem(IRpcClient rpcClient)
            {
                RpcClient = rpcClient;
            }

            public required readonly IRpcClient RpcClient { get; init; }

            public readonly DateTimeOffset CreatedAt { get; } = DateTimeOffset.UtcNow;
        }
    }
}
