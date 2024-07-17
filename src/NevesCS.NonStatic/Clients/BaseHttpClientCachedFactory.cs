using NevesCS.Abstractions.Interfaces;
using NevesCS.Static.Utils;

using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace NevesCS.NonStatic.Clients
{
    public abstract class BaseHttpClientCachedFactory<TService> : ICachedServiceFactory<TService>
    {
        private readonly HttpClientCachedFactoryOptions Options;

        private readonly IHttpClientFactory HttpClientFactory;

        private readonly ConcurrentDictionary<string, CacheItem> CachedHttpClients = [];

        protected BaseHttpClientCachedFactory(HttpClientCachedFactoryOptions options, IHttpClientFactory httpClientFactory)
        {
            Options = ObjectUtils.ThrowIfNull(options, nameof(options));
            HttpClientFactory = ObjectUtils.ThrowIfNull(httpClientFactory, nameof(httpClientFactory));

            LastExpiredCacheItemsCheck = DateTimeOffset.UtcNow;
        }

        protected abstract TService CreateNewInstance(string key, HttpClient httpClient);

        public TService Create(string key)
        {
            DeleteCacheItemIfExpired(key, null);
            CheckAndDeleteExpiredCacheItems();

            return CreateNewInstance(
                key,
                DictionaryUtils.GetOrCreate(
                    CachedHttpClients,
                    key,
                    () => new CacheItem(HttpClientFactory.CreateClient(key)))
                .HttpClient);
        }

        private DateTimeOffset LastExpiredCacheItemsCheck;

        private void CheckAndDeleteExpiredCacheItems()
        {
            var now = DateTimeOffset.UtcNow;

            if ((now - LastExpiredCacheItemsCheck) < Options.TimeBetweenExpiredCacheItemsChecks)
            {
                return;
            }

            foreach (var item in CachedHttpClients)
            {
                DeleteCacheItemIfExpired(item.Key, item.Value);
            }

            LastExpiredCacheItemsCheck = DateTimeOffset.UtcNow;
        }

        private void DeleteCacheItemIfExpired(string key, CacheItem? cacheItem)
        {
            cacheItem ??= CachedHttpClients.GetValueOrDefault(key);

            if (ObjectUtils.IsNull(cacheItem))
            {
                return;
            }

            if ((DateTimeOffset.UtcNow - cacheItem.Value.CreatedAt) > Options.MaxLifetime)
            {
                cacheItem.Value.HttpClient.Dispose();
                CachedHttpClients.Remove(key, out _);
            }
        }

        private readonly struct CacheItem
        {
            [SetsRequiredMembers]
            public CacheItem(HttpClient httpClient)
            {
                HttpClient = httpClient;
            }

            public required readonly HttpClient HttpClient { get; init; }

            public readonly DateTimeOffset CreatedAt { get; } = DateTimeOffset.UtcNow;
        }
    }
}
