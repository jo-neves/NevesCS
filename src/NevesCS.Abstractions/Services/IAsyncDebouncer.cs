namespace NevesCS.Abstractions.Services;

/// <summary>
/// Async variant of <see cref="IDebouncer{TInAction}"/>. Ensures the registered async action is only
/// called once after a period of inactivity, with the latest triggered value.
/// Invocations never overlap: if a previous invocation is still running when the next window elapses,
/// the next one waits for it to finish.
/// Disposing cancels the <see cref="CancellationToken"/> handed to the action.
///
/// </summary>
public interface IAsyncDebouncer<TInAction> : IDisposable
{
    public void Initialize(Func<TInAction, CancellationToken, Task> action, int millisecondDelay);

    public void Trigger(TInAction value);
}
