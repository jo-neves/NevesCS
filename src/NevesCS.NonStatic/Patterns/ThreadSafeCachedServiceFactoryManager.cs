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

    private readonly ConcurrentHashSet<string> ThreadsInUse = new();

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

        ThreadsInUse.Dispose();

        GC.SuppressFinalize(this);
    }

    public async Task CreateAsync(string key, Action<TService> inUse)
    {
        var semaphore = _KeyLocks.GetOrAdd(key, _ => new SemaphoreSlim(1, 1));
        await semaphore.WaitAsync(CancellationToken).ConfigureAwait(false);

        try
        {
            inUse(CachedServiceFactoryManager.Create(key));
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

    public async Task CreateAsync(string key, Func<TService, Task<bool>> inUseAsync)
    {
        var semaphore = _KeyLocks.GetOrAdd(key, _ => new SemaphoreSlim(1, 1));
        await semaphore.WaitAsync(CancellationToken).ConfigureAwait(false);

        try
        {
            await inUseAsync(CachedServiceFactoryManager.Create(key));
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
