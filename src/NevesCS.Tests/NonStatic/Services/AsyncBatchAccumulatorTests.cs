using System.Diagnostics;

using FluentAssertions;

using NevesCS.NonStatic.Services;

namespace NevesCS.Tests.NonStatic.Services;

public class AsyncBatchAccumulatorTests
{
    private const int MillisecondsDelay = 60;

    private const int SettleMilliseconds = 2000;


    [Fact]
    public async Task Add__AfterQuietPeriod__InvokesActionOnceWithEveryAccumulatedItem()
    {
        var batches = new BatchCollector<int>();
        using var sut = new AsyncBatchAccumulator<int>(batches.CollectAsync, MillisecondsDelay);

        sut.Add(1);
        sut.Add(2);
        sut.Add(3);

        await batches.WaitForBatchCountAsync(1);

        batches.Batches.Should().ContainSingle()
            .Which.Should().Equal(1, 2, 3);
    }

    [Fact]
    public async Task Add__WhileItemsKeepArrivingWithinTheWindow__DoesNotFlushEarly()
    {
        var batches = new BatchCollector<int>();
        using var sut = new AsyncBatchAccumulator<int>(batches.CollectAsync, MillisecondsDelay);

        for (int i = 0; i < 5; ++i)
        {
            sut.Add(i);
            await Task.Delay(MillisecondsDelay / 3);
        }

        batches.Batches.Should().BeEmpty("the sliding window is reset by every Add");

        await batches.WaitForBatchCountAsync(1);

        batches.Batches.Should().ContainSingle()
            .Which.Should().Equal(0, 1, 2, 3, 4);
    }

    [Fact]
    public async Task Add__AfterAPreviousFlush__StartsAFreshBatch()
    {
        var batches = new BatchCollector<int>();
        using var sut = new AsyncBatchAccumulator<int>(batches.CollectAsync, MillisecondsDelay);

        sut.Add(1);
        sut.Add(2);
        await batches.WaitForBatchCountAsync(1);

        sut.Add(3);
        sut.Add(4);
        await batches.WaitForBatchCountAsync(2);

        batches.Batches.Should().HaveCount(2);
        batches.Batches[0].Should().Equal(1, 2);
        batches.Batches[1].Should().Equal(3, 4);
    }

    [Fact]
    public async Task Add__AfterDispose__IsIgnoredAndDoesNotThrow()
    {
        var batches = new BatchCollector<int>();
        var sut = new AsyncBatchAccumulator<int>(batches.CollectAsync, MillisecondsDelay);

        sut.Dispose();

        sut.Invoking(x => x.Add(1)).Should().NotThrow();

        await Task.Delay(SettleMilliseconds);

        batches.Batches.Should().BeEmpty();
    }

    [Fact]
    public async Task Dispose__BeforeThePendingFlush__CancelsIt()
    {
        var batches = new BatchCollector<int>();
        var sut = new AsyncBatchAccumulator<int>(batches.CollectAsync, MillisecondsDelay);

        sut.Add(1);
        sut.Dispose();

        await Task.Delay(SettleMilliseconds);

        batches.Batches.Should().BeEmpty();
    }

    [Fact]
    public async Task Dispose__WhileTheActionIsRunning__CancelsTheTokenWithoutLoggingAnError()
    {
        var started = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var finished = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
        var sut = new AsyncBatchAccumulator<int>(
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
            MillisecondsDelay);

        sut.Add(1);
        await started.Task.WaitAsync(TimeSpan.FromMilliseconds(SettleMilliseconds));

        sut.Dispose();

        (await finished.Task.WaitAsync(TimeSpan.FromMilliseconds(SettleMilliseconds))).Should().BeTrue();
    }

    [Fact]
    public void Dispose__CalledRepeatedly__DoesNotThrow()
    {
        var sut = new AsyncBatchAccumulator<int>((_, _) => Task.CompletedTask, MillisecondsDelay);

        sut.Invoking(x =>
        {
            x.Dispose();
            x.Dispose();
        }).Should().NotThrow();
    }

    [Fact]
    public void Initialize__WhenAlreadyInitialized__ThrowsInvalidOperationException()
    {
        using var sut = new AsyncBatchAccumulator<int>((_, _) => Task.CompletedTask, MillisecondsDelay);

        sut.Invoking(x => x.Initialize((_, _) => Task.CompletedTask, MillisecondsDelay))
            .Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public async Task Initialize__OnAnInstanceBuiltWithTheParameterlessConstructor__EnablesAccumulation()
    {
        var batches = new BatchCollector<int>();
        using var sut = new AsyncBatchAccumulator<int>();

        sut.Initialize(batches.CollectAsync, MillisecondsDelay);

        sut.Add(7);
        sut.Add(8);

        await batches.WaitForBatchCountAsync(1);

        batches.Batches.Should().ContainSingle()
            .Which.Should().Equal(7, 8);
    }

    [Fact]
    public async Task Add__WhilePreviousFlushIsStillRunning__DeliversNextBatchAfterItInOrder()
    {
        var firstRelease = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var batches = new BatchCollector<int>();
        var concurrent = 0;
        var maxConcurrent = 0;

        using var sut = new AsyncBatchAccumulator<int>(
            async (batch, cancellationToken) =>
            {
                var now = Interlocked.Increment(ref concurrent);
                InterlockedMax(ref maxConcurrent, now);

                try
                {
                    if (batch[0] == 1)
                    {
                        await firstRelease.Task.WaitAsync(cancellationToken);
                    }

                    await batches.CollectAsync(batch, cancellationToken);
                }
                finally
                {
                    Interlocked.Decrement(ref concurrent);
                }
            },
            MillisecondsDelay);

        sut.Add(1);
        await Task.Delay(MillisecondsDelay * 3);

        sut.Add(2);
        await Task.Delay(MillisecondsDelay * 3);

        batches.Batches.Should().BeEmpty("the first flush is still blocked and the second must wait for it");

        firstRelease.SetResult();
        await batches.WaitForBatchCountAsync(2);

        maxConcurrent.Should().Be(1);
        batches.Batches[0].Should().Equal(1);
        batches.Batches[1].Should().Equal(2);
    }

    [Fact]
    public async Task Add__WhenTheActionThrows__KeepsWorking()
    {
        var batches = new BatchCollector<int>();
        using var sut = new AsyncBatchAccumulator<int>(
            (batch, cancellationToken) =>
            {
                if (batch[0] == 1)
                {
                    throw new InvalidOperationException("boom");
                }

                return batches.CollectAsync(batch, cancellationToken);
            },
            MillisecondsDelay);

        sut.Add(1);
        await Task.Delay(MillisecondsDelay * 3);

        sut.Add(2);
        await batches.WaitForBatchCountAsync(1);

        batches.Batches.Should().ContainSingle()
            .Which.Should().Equal(2);
    }

    [Fact]
    public async Task Add__FromManyThreadsConcurrently__DeliversEveryItemExactlyOnce()
    {
        const int threads = 8;
        const int itemsPerThread = 500;

        var batches = new BatchCollector<int>();
        using var sut = new AsyncBatchAccumulator<int>(batches.CollectAsync, MillisecondsDelay);

        await Task.WhenAll(Enumerable.Range(0, threads).Select(threadIndex => Task.Run(() =>
        {
            for (int i = 0; i < itemsPerThread; ++i)
            {
                sut.Add((threadIndex * itemsPerThread) + i);
            }
        })));

        await batches.WaitForNoActivityAsync(TimeSpan.FromMilliseconds(MillisecondsDelay * 4));

        var delivered = batches.Batches.SelectMany(batch => batch).ToArray();

        delivered.Should().HaveCount(threads * itemsPerThread);
        delivered.Should().OnlyHaveUniqueItems();
        delivered.Should().BeEquivalentTo(Enumerable.Range(0, threads * itemsPerThread));
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

    private sealed class BatchCollector<T>
    {
        private readonly object _gate = new();

        private readonly List<IReadOnlyList<T>> _batches = [];

        private long _lastActivityTimestamp = Stopwatch.GetTimestamp();

        public IReadOnlyList<IReadOnlyList<T>> Batches
        {
            get
            {
                lock (_gate)
                {
                    return _batches.ToArray();
                }
            }
        }

        public Task CollectAsync(IReadOnlyList<T> batch, CancellationToken cancellationToken)
        {
            lock (_gate)
            {
                _batches.Add(batch);
                _lastActivityTimestamp = Stopwatch.GetTimestamp();
            }

            return Task.CompletedTask;
        }

        public Task WaitForBatchCountAsync(int expectedCount)
        {
            return WaitUntilAsync(() => Batches.Count >= expectedCount);
        }

        public async Task WaitForNoActivityAsync(TimeSpan quietFor)
        {
            while (true)
            {
                TimeSpan sinceLastActivity;

                lock (_gate)
                {
                    sinceLastActivity = Stopwatch.GetElapsedTime(_lastActivityTimestamp);
                }

                if (sinceLastActivity >= quietFor)
                {
                    return;
                }

                await Task.Delay(10);
            }
        }
    }
}
