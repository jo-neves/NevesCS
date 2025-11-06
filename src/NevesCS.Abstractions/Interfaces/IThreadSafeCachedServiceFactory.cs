namespace NevesCS.Abstractions.Interfaces;

public interface IThreadSafeCachedServiceFactory<TService>
{
    public Task<TOut> CreateAsync<TOut>(string key, Func<TService, TOut> inUse);

    public Task<TOut> CreateAsync<TOut>(string key, Func<TService, Task<TOut>> inUseAsync);
}
