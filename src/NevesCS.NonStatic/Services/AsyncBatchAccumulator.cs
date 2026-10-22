using System.Diagnostics;

using NevesCS.Abstractions.Services;

namespace NevesCS.NonStatic.Services;

/// <inheritdoc/>
public sealed class AsyncBatchAccumulator<T> : IAsyncBatchAccumulator<T>
{
    private readonly object _lock = new();
    private readonly List<T> _buffer = [];
    private readonly SemaphoreSlim _runGate = new(1, 1);
    private readonly CancellationTokenSource _disposeCts = new();

    private bool _didInit;
    private bool _disposed;

    private Func<IReadOnlyList<T>, CancellationToken, Task> _action = null!;
    private int _millisecondsDelay;
    private Timer _timer = null!;

    public AsyncBatchAccumulator()
    {
    }

    public AsyncBatchAccumulator(Func<IReadOnlyList<T>, CancellationToken, Task> action, int millisecondsDelay)
    {
        Initialize(action, millisecondsDelay);
    }

    public void Initialize(Func<IReadOnlyList<T>, CancellationToken, Task> action, int millisecondsDelay)
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
            if (_disposed)
            {
                return;
            }

            ThrowIfNotInitialized();

            _buffer.Add(item);
            _timer.Change(_millisecondsDelay, Timeout.Infinite);
        }
    }

    private void OnTimerElapsed(object? state)
    {
        IReadOnlyList<T> snapshot;

        lock (_lock)
        {
            if (!_didInit || _disposed)
            {
                return;
            }

            if (_buffer.Count == 0)
            {
                return;
            }

            snapshot = _buffer.ToArray();
            _buffer.Clear();
        }

        // RunAsync handles its own failures, so the task is never left faulted.
        _ = RunAsync(snapshot);
    }

    private async Task RunAsync(IReadOnlyList<T> snapshot)
    {
        var cancellationToken = _disposeCts.Token;

        try
        {
            await _runGate.WaitAsync(cancellationToken).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            return;
        }

        try
        {
            await _action(snapshot, cancellationToken).ConfigureAwait(false);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            // Disposed while running.
        }
        catch (Exception)
        {
            // A failing action must not stop later invocations nor surface as an unobserved task exception.
        }
        finally
        {
            _runGate.Release();
        }
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

        // Only cancelled, not disposed: an in-flight action may still be using the token or the gate.
        _disposeCts.Cancel();
    }

    #endregion IDisposable

    private void ThrowIfDisposed()
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(AsyncBatchAccumulator<T>));
    }

    private void ThrowIfNotInitialized()
    {
        Debug.Assert(_didInit, ".Initialize() must be called first to register an action.");
    }
}
