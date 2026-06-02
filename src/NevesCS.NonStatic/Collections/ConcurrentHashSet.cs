using NevesCS.Abstractions.Traits;

namespace NevesCS.NonStatic.Collections
{
    public class ConcurrentHashSet<T> : ICountable, IDisposable
    {
        private readonly HashSet<T> InternalHashSet = [];

        private readonly ReaderWriterLockSlim Lock = new(LockRecursionPolicy.SupportsRecursion);

        private bool disposedValue;

        public int Count()
        {
            Lock.EnterReadLock();

            try
            {
                return InternalHashSet.Count;
            }
            finally
            {
                if (Lock.IsReadLockHeld)
                {
                    Lock.ExitReadLock();
                }
            }
        }

        public bool Contains(T item)
        {
            Lock.EnterReadLock();

            try
            {
                return InternalHashSet.Contains(item);
            }
            catch
            {
                return false;
            }
            finally
            {
                if (Lock.IsReadLockHeld)
                {
                    Lock.ExitReadLock();
                }
            }
        }

        public bool Add(T item)
        {
            Lock.EnterWriteLock();

            try
            {
                return InternalHashSet.Add(item);
            }
            catch
            {
                return false;
            }
            finally
            {
                if (Lock.IsWriteLockHeld)
                {
                    Lock.ExitWriteLock();
                }
            }
        }

        public bool Remove(T item)
        {
            Lock.EnterWriteLock();

            try
            {
                return InternalHashSet.Remove(item);
            }
            catch
            {
                return false;
            }
            finally
            {
                if (Lock.IsWriteLockHeld)
                {
                    Lock.ExitWriteLock();
                }
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Lock?.Dispose();
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
