using System.Diagnostics;
using NevesCS.Abstractions.Services;

namespace NevesCS.NonStatic.Services;

/// <inheritdoc/>
public sealed class BatchAccumulator<T> : IBatchAccumulator<T>
{
    private readonly object _lock = new();
    private readonly List<T> _buffer = [];

    private bool _didInit;
    private bool _disposed;

    private Action<IReadOnlyList<T>> _action;
    private int _millisecondsDelay;
    private Timer _timer;

    public BatchAccumulator()
    {
    }

    public BatchAccumulator(Action<IReadOnlyList<T>> action, int millisecondsDelay)
    {
        Initialize(action, millisecondsDelay);
    }

    public void Initialize(Action<IReadOnlyList<T>> action, int millisecondsDelay)
    {
        lock (_lock)
        {
            ThrowIfDisposed();

            if (_didInit)
            {
                throw new InvalidOperationException("Instance already initialized.");
            }

            _action = action;
            _millisecondsDelay = millisecondsDelay;
            _timer = new Timer(OnTimerElapsed, null, Timeout.Infinite, Timeout.Infinite);
            _didInit = true;
        }
    }

    /// <inheritdoc/>
    public void Add(T item)
    {
        lock (_lock)
        {
            ThrowIfDisposed();
            ThrowIfNotInitialized();

            if (_disposed)
            {
                return;
            }

            _buffer.Add(item);
            _timer.Change(_millisecondsDelay, Timeout.Infinite);
        }
    }

    private void OnTimerElapsed(object? state)
    {
        IReadOnlyList<T> snapshot;

        lock (_lock)
        {
            ThrowIfDisposed();
            ThrowIfNotInitialized();

            if (_disposed)
            {
                return;
            }

            snapshot = _buffer.ToArray();
            _buffer.Clear();
        }

        if (snapshot.Count > 0)
        {
            _action.Invoke(snapshot);
        }
    }

    #region IDisposable

    ~BatchAccumulator()
    {
        Dispose();
    }

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
        ObjectDisposedException.ThrowIf(_disposed, nameof(BatchAccumulator<T>));
    }

    private void ThrowIfNotInitialized()
    {
        Debug.Assert(_didInit, ".Initialize() must be called first to register an action.");
    }
}
