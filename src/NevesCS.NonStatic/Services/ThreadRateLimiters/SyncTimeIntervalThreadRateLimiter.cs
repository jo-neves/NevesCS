using NevesCS.Abstractions.Services;
using NevesCS.Static.Utils;

namespace NevesCS.NonStatic.Services.ThreadRateLimiters
{
    /// <summary>
    /// Thread safe. Can be used between multiple threads to synchronize them.
    ///
    /// </summary>
    public sealed class SyncTimeIntervalThreadRateLimiter : IThreadRateLimiter, IDisposable
    {
        private readonly TimeSpan IntervalInBetween;
        private readonly bool ShouldLock;

        private readonly IClock Clock;

        private readonly SemaphoreSlim _Semaphore = new(1, 1);

        /// <summary>
        /// Throws if <paramref name="intervalInBetween"/> is `null`.
        ///
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public SyncTimeIntervalThreadRateLimiter(TimeSpan intervalInBetween)
        {
            IntervalInBetween = ObjectUtils.ThrowIfNull(intervalInBetween, nameof(intervalInBetween));
            ShouldLock = IntervalInBetween.Ticks > 0;

            Clock = new UtcClock();
            Reset();
        }

        /// <summary>
        /// Throws if <paramref name="intervalInBetween"/> is `null`.
        ///
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public SyncTimeIntervalThreadRateLimiter(TimeSpan intervalInBetween, IClock clock)
        {
            IntervalInBetween = ObjectUtils.ThrowIfNull(intervalInBetween, nameof(intervalInBetween));
            ShouldLock = IntervalInBetween.Ticks > 0;

            Clock = ObjectUtils.ThrowIfNull(clock, nameof(clock));
            Reset();
        }

        private long LastReleaseTicks;

        private bool disposedValue;

        public void Reset()
        {
            LastReleaseTicks = Clock.GetTime().Ticks;
        }

        public async Task WaitAsync(CancellationToken cancellationToken = default)
        {
            if (!ShouldLock)
            {
                await Task.Delay(IntervalInBetween, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                await _Semaphore.WaitAsync(cancellationToken).ConfigureAwait(false);

                var ticksSinceLastRelease = (DateTimeOffset.UtcNow.Ticks - LastReleaseTicks);
                var timeToWait = TimeSpan.FromTicks(Math.Max(0, IntervalInBetween.Ticks - ticksSinceLastRelease));
                await Task.Delay(timeToWait, cancellationToken).ConfigureAwait(false);

                Reset();
                _Semaphore.Release();
            }
        }

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _Semaphore.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
