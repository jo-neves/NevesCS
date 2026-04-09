using System.Collections.Concurrent;

using NevesCS.Abstractions.Interfaces;
using NevesCS.NonStatic.Clients;
using NevesCS.NonStatic.Collections;
using NevesCS.Static.Utils;

namespace NevesCS.NonStatic.Patterns;

public class ThreadSafeCachedServiceFactoryManager<TService> : IThreadSafeCachedServiceFactory<TService>, IDisposable
{
    private readonly CachedServiceFactoryManager<TService> CachedServiceFactoryManager;

    private readonly CancellationToken CancellationToken;

    private readonly ConcurrentDictionary<string, SemaphoreSlim> _KeyLocks = new();

    public ThreadSafeCachedServiceFactoryManager(
        CachedFactoryOptions options,
        ICachedServiceFactory<TService> serviceFactory,
        CancellationToken cancellationToken = default
    )
    {
        CancellationToken = ObjectUtils.AssertNotNull(cancellationToken, nameof(cancellationToken));

        CachedServiceFactoryManager = new CachedServiceFactoryManager<TService>(
            options,
            serviceFactory,
            cancellationToken
        );
    }

    public void Dispose()
    {
        CachedServiceFactoryManager.Dispose();

        foreach (var semaphore in _KeyLocks)
        {
            semaphore.Value.Dispose();
        }

        GC.SuppressFinalize(this);
    }

    public async Task<TOut> CreateAsync<TOut>(string key, Func<TService, TOut> inUse)
    {
        var semaphore = _KeyLocks.GetOrAdd(key, _ => new SemaphoreSlim(1, 1));
        await semaphore.WaitAsync(CancellationToken).ConfigureAwait(false);

        try
        {
            return inUse(CachedServiceFactoryManager.Create(key));
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            semaphore.Release();
        }
    }

    public async Task<TOut> CreateAsync<TOut>(string key, Func<TService, Task<TOut>> inUseAsync)
    {
        var semaphore = _KeyLocks.GetOrAdd(key, _ => new SemaphoreSlim(1, 1));
        await semaphore.WaitAsync(CancellationToken).ConfigureAwait(false);

        try
        {
            return await inUseAsync(CachedServiceFactoryManager.Create(key));
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            semaphore.Release();
        }
    }
}
