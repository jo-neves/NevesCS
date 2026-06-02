using System.Collections;

namespace NevesCS.NonStatic.Collections
{
    public class FixedSizeAutoDequeue<T> : IEnumerable<T>
    {
        private readonly uint FixedSize;

        private readonly Queue<T> Data = new();

        public FixedSizeAutoDequeue(uint fixedSize)
        {
            FixedSize = fixedSize;
        }

        public FixedSizeAutoDequeue(uint fixedSize, IEnumerable<T> initValues)
        {
            FixedSize = fixedSize;

            foreach (var value in initValues)
            {
                Enqueue(value);
            }
        }

        public int Count()
        {
            return Data.Count;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)Data).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)Data).GetEnumerator();
        }

        public IEnumerable<T> AsEnumerable()
        {
            return Data.AsEnumerable();
        }

        public void Enqueue(T newItem)
        {
            if (Data.Count == FixedSize)
            {
                Data.Dequeue();
            }

            Data.Enqueue(newItem);
        }

        public void Clear()
        {
            Data.Clear();
        }

        public T? Peek()
        {
            return Data.Peek();
        }
    }
}
