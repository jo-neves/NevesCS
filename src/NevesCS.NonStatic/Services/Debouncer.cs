using System.Diagnostics;

using NevesCS.Abstractions.Services;

namespace NevesCS.NonStatic.Services;

/// <inheritdoc/>
public sealed class Debouncer<TInAction> : IDebouncer<TInAction>, IDisposable
{
    public Debouncer()
    {
    }

    public Debouncer(Action<TInAction> action, int millisecondDelay)
    {
        Initialize(action, millisecondDelay);
    }

    private Action<TInAction> action;

    private int millisecondDelay;

    private Timer timer;

    private bool initialized;

    private TInAction latestValue;

    public void Initialize(Action<TInAction> action, int millisecondDelay)
    {
        this.action = action;
        this.millisecondDelay = millisecondDelay;
        timer = new Timer(OnTimerElapsed, null, Timeout.Infinite, Timeout.Infinite);

        initialized = true;
    }

    #region IDisposable

    private bool disposed;

    public void Dispose()
    {
        if (disposed)
        {
            return;
        }

        timer.Dispose();
        disposed = true;
        GC.SuppressFinalize(this);
    }

    #endregion IDisposable

    public void Trigger(TInAction value)
    {
        ThrowIfNotInitialized();
        ThrowIfDisposed();

        latestValue = value;
        timer.Change(millisecondDelay, Timeout.Infinite);
    }

    private void OnTimerElapsed(object state)
    {
        ThrowIfNotInitialized();
        ThrowIfDisposed();

        if (disposed)
        {
            return;
        }

        action.Invoke(latestValue);
    }

    private void ThrowIfDisposed()
    {
        if (disposed)
        {
            throw new ObjectDisposedException($"{nameof(Debouncer<>)} was already disposed.");
        }
    }

    private void ThrowIfNotInitialized()
    {
        Debug.Assert(initialized);
    }
}
