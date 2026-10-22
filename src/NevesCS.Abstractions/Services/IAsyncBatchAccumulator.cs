namespace NevesCS.Abstractions.Services;

/// <summary>
/// Async variant of <see cref="IBatchAccumulator{T}"/>. Accumulates items and flushes them as a single
/// batch once a sliding inactivity window elapses.
/// Flushes never overlap and are delivered in order: if a previous flush is still running when the next
/// window elapses, the next one waits for it to finish.
/// Disposing cancels the <see cref="CancellationToken"/> handed to the action.
///
/// </summary>
public interface IAsyncBatchAccumulator<T> : IDisposable
{
    public void Initialize(Func<IReadOnlyList<T>, CancellationToken, Task> action, int millisecondsDelay);

    /// <summary>
    /// Adds an item to the current batch and (re)starts the inactivity window.
    ///
    /// </summary>
    public void Add(T item);
}
