using System.Diagnostics;

using FluentAssertions;

using NevesCS.NonStatic.Services;

namespace NevesCS.Tests.NonStatic.Services;

public class AsyncDebouncerTests
{
    private const int MillisecondDelay = 60;

    private const int SettleMilliseconds = 2000;


    [Fact]
    public async Task Trigger__WithSeveralCallsInsideTheWindow__InvokesActionOnceWithTheLatestValue()
    {
        var sink = new InvocationSink<int>();
        using var sut = new AsyncDebouncer<int>(sink.RecordAsync, MillisecondDelay);

        for (int i = 1; i <= 50; ++i)
        {
            sut.Trigger(i);
        }

        await sink.WaitForCountAsync(1);
        await Task.Delay(MillisecondDelay * 3);

        sink.Count.Should().Be(1);
        sink.LastValue.Should().Be(50);
    }

    [Fact]
    public async Task Trigger__WithGapsLongerThanTheDelay__InvokesActionForEachSettledValue()
    {
        var sink = new InvocationSink<int>();
        using var sut = new AsyncDebouncer<int>(sink.RecordAsync, MillisecondDelay);

        sut.Trigger(1);
        await sink.WaitForCountAsync(1);

        sut.Trigger(2);
        await sink.WaitForCountAsync(2);

        sink.Count.Should().Be(2);
        sink.LastValue.Should().Be(2);
    }

    [Fact]
    public async Task Trigger__WhileTheWindowKeepsSliding__DoesNotInvokeEarly()
    {
        var sink = new InvocationSink<int>();
        using var sut = new AsyncDebouncer<int>(sink.RecordAsync, MillisecondDelay);

        for (int i = 0; i < 5; ++i)
        {
            sut.Trigger(i);
            await Task.Delay(MillisecondDelay / 3);
        }

        sink.Count.Should().Be(0);

        await sink.WaitForCountAsync(1);
        sink.LastValue.Should().Be(4);
    }

    [Fact]
    public void Trigger__AfterDispose__ThrowsObjectDisposedException()
    {
        var sut = new AsyncDebouncer<int>((_, _) => Task.CompletedTask, MillisecondDelay);
        sut.Dispose();

        sut.Invoking(x => x.Trigger(1)).Should().Throw<ObjectDisposedException>();
    }

    [Fact]
    public async Task Dispose__BeforeThePendingInvocation__CancelsIt()
    {
        var sink = new InvocationSink<int>();
        var sut = new AsyncDebouncer<int>(sink.RecordAsync, MillisecondDelay);

        sut.Trigger(1);
        sut.Dispose();

        await Task.Delay(SettleMilliseconds);

        sink.Count.Should().Be(0);
    }

    [Fact]
    public async Task Dispose__WhileTheActionIsRunning__CancelsTheTokenWithoutLoggingAnError()
    {
        var started = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var finished = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
        var sut = new AsyncDebouncer<int>(
            async (_, cancellationToken) =>
            {
                started.SetResult();

                try
                {
                    await Task.Delay(Timeout.Infinite, cancellationToken);
                }
                finally
                {
                    finished.SetResult(cancellationToken.IsCancellationRequested);
                }
            },
            MillisecondDelay);

        sut.Trigger(1);
        await started.Task.WaitAsync(TimeSpan.FromMilliseconds(SettleMilliseconds));

        sut.Dispose();

        (await finished.Task.WaitAsync(TimeSpan.FromMilliseconds(SettleMilliseconds))).Should().BeTrue();
    }

    [Fact]
    public void Dispose__CalledRepeatedly__DoesNotThrow()
    {
        var sut = new AsyncDebouncer<int>((_, _) => Task.CompletedTask, MillisecondDelay);

        sut.Invoking(x =>
        {
            x.Dispose();
            x.Dispose();
        }).Should().NotThrow();
    }

    [Fact]
    public void Dispose__OnAnInstanceThatWasNeverInitialized__DoesNotThrow()
    {
        var sut = new AsyncDebouncer<int>();

        sut.Invoking(x => x.Dispose()).Should().NotThrow();
    }

    [Fact]
    public void Initialize__WhenAlreadyInitialized__ThrowsInvalidOperationException()
    {
        using var sut = new AsyncDebouncer<int>((_, _) => Task.CompletedTask, MillisecondDelay);

        sut.Invoking(x => x.Initialize((_, _) => Task.CompletedTask, MillisecondDelay))
            .Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public async Task Initialize__OnAnInstanceBuiltWithTheParameterlessConstructor__EnablesTriggering()
    {
        var sink = new InvocationSink<int>();
        using var sut = new AsyncDebouncer<int>();

        sut.Initialize(sink.RecordAsync, MillisecondDelay);

        sut.Trigger(11);
        sut.Trigger(22);

        await sink.WaitForCountAsync(1);

        sink.Count.Should().Be(1);
        sink.LastValue.Should().Be(22);
    }

    [Fact]
    public async Task Trigger__WhilePreviousActionIsStillRunning__WaitsForItBeforeInvokingAgain()
    {
        var firstRelease = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var sink = new InvocationSink<int>();
        var concurrent = 0;
        var maxConcurrent = 0;

        using var sut = new AsyncDebouncer<int>(
            async (value, cancellationToken) =>
            {
                var now = Interlocked.Increment(ref concurrent);
                InterlockedMax(ref maxConcurrent, now);

                try
                {
                    if (value == 1)
                    {
                        await firstRelease.Task.WaitAsync(cancellationToken);
                    }

                    await sink.RecordAsync(value, cancellationToken);
                }
                finally
                {
                    Interlocked.Decrement(ref concurrent);
                }
            },
            MillisecondDelay);

        sut.Trigger(1);
        await Task.Delay(MillisecondDelay * 3);

        sut.Trigger(2);
        await Task.Delay(MillisecondDelay * 3);

        sink.Count.Should().Be(0, "the first action is still blocked and the second must wait for it");

        firstRelease.SetResult();
        await sink.WaitForCountAsync(2);

        maxConcurrent.Should().Be(1);
        sink.LastValue.Should().Be(2);
    }

    [Fact]
    public async Task Trigger__WhenTheActionThrows__KeepsWorking()
    {
        var sink = new InvocationSink<int>();
        using var sut = new AsyncDebouncer<int>(
            (value, cancellationToken) =>
            {
                if (value == 1)
                {
                    throw new InvalidOperationException("boom");
                }

                return sink.RecordAsync(value, cancellationToken);
            },
            MillisecondDelay);

        sut.Trigger(1);
        await Task.Delay(MillisecondDelay * 3);

        sut.Trigger(2);
        await sink.WaitForCountAsync(1);

        sink.LastValue.Should().Be(2);
    }

    [Fact]
    public async Task Trigger__FromManyThreadsConcurrently__NeverThrowsAndSettlesOnATriggeredValue()
    {
        const int threads = 8;
        const int triggersPerThread = 2000;

        var sink = new InvocationSink<int>();
        using var sut = new AsyncDebouncer<int>(sink.RecordAsync, MillisecondDelay);

        await Task.WhenAll(Enumerable.Range(1, threads).Select(threadId => Task.Run(() =>
        {
            for (int i = 0; i < triggersPerThread; ++i)
            {
                sut.Trigger(threadId);
            }
        })));

        await sink.WaitForCountAsync(1);
        await Task.Delay(MillisecondDelay * 3);

        sink.Count.Should().Be(1, "every trigger inside the window collapses into a single invocation");
        sink.LastValue.Should().BeInRange(1, threads);
    }

    [Fact]
    public async Task Trigger__RacingWithDispose__OnlyEverThrowsObjectDisposedException()
    {
        var sink = new InvocationSink<int>();
        var sut = new AsyncDebouncer<int>(sink.RecordAsync, MillisecondDelay);

        var caught = new List<Exception>();

        var spinning = Task.Run(() =>
        {
            for (int i = 0; i < 200_000; ++i)
            {
                try
                {
                    sut.Trigger(i);
                }
                catch (ObjectDisposedException)
                {
                    return;
                }
                catch (Exception ex)
                {
                    caught.Add(ex);
                    return;
                }
            }
        });

        await Task.Delay(5);
        sut.Dispose();
        await spinning;

        caught.Should().BeEmpty("the lock makes the Trigger/Dispose race well-defined");
    }

    private static void InterlockedMax(ref int target, int candidate)
    {
        int current;

        do
        {
            current = Volatile.Read(ref target);

            if (candidate <= current)
            {
                return;
            }
        }
        while (Interlocked.CompareExchange(ref target, candidate, current) != current);
    }

    private static async Task WaitUntilAsync(Func<bool> condition)
    {
        var timeout = TimeSpan.FromMilliseconds(SettleMilliseconds);
        var start = Stopwatch.GetTimestamp();

        while (Stopwatch.GetElapsedTime(start) < timeout)
        {
            if (condition())
            {
                return;
            }

            await Task.Delay(10);
        }

        throw new TimeoutException($"Condition not met within {timeout.TotalMilliseconds}ms.");
    }

    private sealed class InvocationSink<T>
    {
        private readonly object _gate = new();

        private int _count;

        private T _lastValue = default!;

        public int Count
        {
            get
            {
                lock (_gate)
                {
                    return _count;
                }
            }
        }

        public T LastValue
        {
            get
            {
                lock (_gate)
                {
                    return _lastValue;
                }
            }
        }

        public Task RecordAsync(T value, CancellationToken cancellationToken)
        {
            lock (_gate)
            {
                _count++;
                _lastValue = value;
            }

            return Task.CompletedTask;
        }

        public Task WaitForCountAsync(int expected)
        {
            return WaitUntilAsync(() => Count >= expected);
        }
    }
}
