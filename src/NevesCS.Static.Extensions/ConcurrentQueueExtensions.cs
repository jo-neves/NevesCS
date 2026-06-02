using NevesCS.Static.Utils;

using System.Collections.Concurrent;

namespace NevesCS.Static.Extensions
{
    public static class ConcurrentQueueExtensions
    {
        public static async Task HandleAllAsync<T>(
            this ConcurrentQueue<T> target,
            Func<T, CancellationToken, Task> asyncHandler,
            CancellationToken cancellationToken = default)
        {
            await ConcurrentQueueUtils.HandleAllAsync(target, asyncHandler, cancellationToken);
        }

        public static IEnumerable<T> DequeueAll<T>(this ConcurrentQueue<T> target)
        {
            return ConcurrentQueueUtils.DequeueAll(target);
        }
    }
}
