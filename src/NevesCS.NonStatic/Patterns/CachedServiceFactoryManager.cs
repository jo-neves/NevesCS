using NevesCS.Abstractions.Interfaces;
using NevesCS.NonStatic.Clients;
using NevesCS.Static.Utils;

using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace NevesCS.NonStatic.Patterns
{
    /// <summary>
    /// This is to be used only in cases where the lifetimes of objects needs to be optimized.
    /// Otherwise, the creation should be manged through DI.
    ///
    /// </summary>
    public sealed class CachedServiceFactoryManager<TService> : ICachedServiceFactory<TService>, IDisposable
    {
        private readonly CachedFactoryOptions Options;

        private readonly CancellationTokenSource InternalCancellationTokenSource;
        private readonly CancellationToken CancellationToken;

        private readonly IManagedServiceFactory<TService> ServiceFactory;

        private readonly ConcurrentDictionary<string, CacheItem> CachedItems = [];

        private readonly Task? BackgroundJob;

        public CachedServiceFactoryManager(
            CachedFactoryOptions options,
            IManagedServiceFactory<TService> serviceFactory,
            CancellationToken cancellationToken = default)
        {
            Options = ObjectUtils.ThrowIfNull(options, nameof(options));
            InternalCancellationTokenSource = new CancellationTokenSource();
            ServiceFactory = ObjectUtils.ThrowIfNull(serviceFactory, nameof(serviceFactory));

            CancellationToken = CancellationTokenSource
                .CreateLinkedTokenSource(
                    InternalCancellationTokenSource.Token,
                    cancellationToken)
                .Token;

            BackgroundJob = StartBackgroudJob();
        }

        public TService Create(string key)
        {
            DeleteCacheItemIfExpired(key, null);

            return CachedItems.GetOrAdd(key, k => new CacheItem(ServiceFactory.Create(k))).Service;
        }

        private Task StartBackgroudJob()
        {
            if (Options.CheckExpiredCacheItemsEvery == TimeSpan.Zero)
            {
                return Task.CompletedTask;
            }

            return Task.Run(async () =>
            {
                while (!CancellationToken.IsCancellationRequested)
                {
                    await Task.Delay(Options.CheckExpiredCacheItemsEvery, CancellationToken);

                    if (CancellationToken.IsCancellationRequested)
                    {
                        break;
                    }

                    CheckAndDeleteExpiredCacheItems();
                }
            });
        }

        private void CheckAndDeleteExpiredCacheItems()
        {
            foreach (var item in CachedItems)
            {
                DeleteCacheItemIfExpired(item.Key, item.Value);
            }
        }

        private void DeleteCacheItemIfExpired(string key, CacheItem? cacheItem)
        {
            cacheItem ??= CachedItems.GetValueOrDefault(key);

            if (ObjectUtils.IsNull(cacheItem))
            {
                return;
            }

            if ((DateTimeOffset.UtcNow - cacheItem.Value.CreatedAt) <= Options.MaxLifetime)
            {
                return;
            }

            if (cacheItem.Value.Service is IDisposable disposableService)
            {
                disposableService.Dispose();
            }

            CachedItems.Remove(key, out _);
        }

        public void Dispose()
        {
            InternalCancellationTokenSource.Cancel();

            try
            {
                BackgroundJob?.Wait();
            }
            catch (AggregateException ex) when (ex.InnerException is TaskCanceledException)
            {
                // Task was canceled, swallow the exception.
            }

            foreach (var cachedItem in CachedItems.Values)
            {
                if (cachedItem.Service is IDisposable disposableService)
                {
                    disposableService.Dispose();
                }
            }

            InternalCancellationTokenSource.Dispose();
        }

        private readonly struct CacheItem
        {
            [SetsRequiredMembers]
            public CacheItem(TService service)
            {
                Service = service;
            }

            public required readonly TService Service { get; init; }

            public readonly DateTimeOffset CreatedAt { get; } = DateTimeOffset.UtcNow;
        }
    }
}
