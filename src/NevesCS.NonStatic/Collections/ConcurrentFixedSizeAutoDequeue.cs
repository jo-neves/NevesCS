using System.Collections;
using System.Collections.Concurrent;

namespace NevesCS.NonStatic.Collections
{
    public class ConcurrentFixedSizeAutoDequeue<T> : IEnumerable<T>
    {
        private readonly uint FixedSize;

        private readonly ConcurrentQueue<T> Data;

        private readonly object _EnqueueLock = new();

        public ConcurrentFixedSizeAutoDequeue(uint fixedSize)
        {
            FixedSize = fixedSize;
            Data = new();
        }

        public ConcurrentFixedSizeAutoDequeue(uint fixedSize, IEnumerable<T> initValues)
        {
            if (initValues.Count() > fixedSize)
            {
                throw new ArgumentOutOfRangeException(nameof(initValues));
            }

            Data = new(initValues);
        }

        public int Count()
        {
            return Data.Count;
        }

        public void Enqueue(T newItem)
        {
            lock (_EnqueueLock)
            {
                if (Data.Count == FixedSize)
                {
                    Data.TryDequeue(out T? _);
                }

                Data.Enqueue(newItem);
            }
        }

        public void Clear()
        {
            Data.Clear();
        }

        public T? Peek()
        {
            Data.TryPeek(out T? item);

            return item;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)Data).GetEnumerator();
        }
    }
}
