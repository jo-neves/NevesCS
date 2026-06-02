using System.Collections.Concurrent;

namespace NevesCS.Static.Utils
{
    public static class ConcurrentQueueUtils
    {
        public static async Task HandleAllAsync<T>(
            ConcurrentQueue<T> target,
            Func<T, CancellationToken, Task> asyncHandler,
            CancellationToken cancellationToken = default)
        {
            while (!cancellationToken.IsCancellationRequested && target.TryDequeue(out T? item))
            {
                if (ObjectUtils.IsNullOrDefault(item))
                {
                    break;
                }

                await asyncHandler(item, cancellationToken);
            }
        }

        public static IEnumerable<T> DequeueAll<T>(ConcurrentQueue<T> target)
        {
            while (target.TryDequeue(out T? item))
            {
                if (ObjectUtils.IsNullOrDefault(item))
                {
                    yield break;
                }

                yield return item;
            }
        }
    }
}
