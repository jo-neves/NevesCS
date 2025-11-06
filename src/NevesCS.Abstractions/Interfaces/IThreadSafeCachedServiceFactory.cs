namespace NevesCS.Abstractions.Interfaces;

public interface IThreadSafeCachedServiceFactory<TService>
{
    public Task CreateAsync(string key, Action<TService> inUse);

    public Task CreateAsync(string key, Func<TService, Task<bool>> inUseAsync);
}
