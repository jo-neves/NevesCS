using System.Diagnostics;

using NevesCS.Abstractions.Services;

namespace NevesCS.NonStatic.Services;

/// <inheritdoc/>
public sealed class AsyncDebouncer<TInAction> : IAsyncDebouncer<TInAction>
{
    private readonly object _lock = new();
    private readonly SemaphoreSlim _runGate = new(1, 1);
    private readonly CancellationTokenSource _disposeCts = new();

    private bool _didInit;
    private bool _disposed;

    private Func<TInAction, CancellationToken, Task> _action = null!;
    private int _millisecondDelay;
    private Timer _timer = null!;
    private TInAction _latestValue = default!;

    public AsyncDebouncer()
    {
    }

    public AsyncDebouncer(Func<TInAction, CancellationToken, Task> action, int millisecondDelay)
    {
        Initialize(action, millisecondDelay);
    }

    public void Initialize(Func<TInAction, CancellationToken, Task> action, int millisecondDelay)
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

        // RunAsync handles its own failures, so the task is never left faulted.
        _ = RunAsync(snapshot);
    }

    private async Task RunAsync(TInAction snapshot)
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
        ObjectDisposedException.ThrowIf(_disposed, nameof(AsyncDebouncer<TInAction>));
    }

    private void ThrowIfNotInitialized()
    {
        Debug.Assert(_didInit, ".Initialize() must be called first to register an action.");
    }
}
