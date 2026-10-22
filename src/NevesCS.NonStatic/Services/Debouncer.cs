using System.Diagnostics;

using NevesCS.Abstractions.Services;

namespace NevesCS.NonStatic.Services;

/// <inheritdoc/>
public sealed class Debouncer<TInAction> : IDebouncer<TInAction>, IDisposable
{
    private readonly object _lock = new();

    private bool _didInit;
    private bool _disposed;

    private Action<TInAction> _action = null!;
    private int _millisecondDelay;
    private Timer _timer = null!;
    private TInAction _latestValue = default!;

    public Debouncer()
    {
    }

    public Debouncer(Action<TInAction> action, int millisecondDelay)
    {
        Initialize(action, millisecondDelay);
    }

    public void Initialize(Action<TInAction> action, int millisecondDelay)
    {
        lock (_lock)
        {
            ThrowIfDisposed();

            if (_didInit)
            {
                throw new InvalidOperationException("Instance already initialized.");
            }

            _action = action;
            _millisecondDelay = millisecondDelay;
            _timer = new Timer(OnTimerElapsed, null, Timeout.Infinite, Timeout.Infinite);
            _didInit = true;
        }
    }

    public void Trigger(TInAction value)
    {
        lock (_lock)
        {
            ThrowIfDisposed();
            ThrowIfNotInitialized();

            _latestValue = value;
            _timer.Change(_millisecondDelay, Timeout.Infinite);
        }
    }

    private void OnTimerElapsed(object? state)
    {
        TInAction snapshot;

        lock (_lock)
        {
            if (!_didInit || _disposed)
            {
                return;
            }

            snapshot = _latestValue;
        }

        _action.Invoke(snapshot);
    }

    #region IDisposable

    public void Dispose()
    {
        lock (_lock)
        {
            if (_disposed)
            {
                return;
            }

            _timer?.Dispose();
            _disposed = true;
        }

        GC.SuppressFinalize(this);
    }

    #endregion IDisposable

    private void ThrowIfDisposed()
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(Debouncer<TInAction>));
    }

    private void ThrowIfNotInitialized()
    {
        Debug.Assert(_didInit, ".Initialize() must be called first to register an action.");
    }
}
