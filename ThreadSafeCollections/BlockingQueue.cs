using System.Collections.Generic;
using System.Threading;

namespace ThreadSafeCollections
{
    class BlockingQueue<T> : SimpleQueue<T>
    {
        readonly object listLock = new object();

        private Queue<T> queue = new Queue<T>();

        public override T Dequeue()
        {
            lock (listLock)
            {
                while (queue.Count == 0)
                {
                    Monitor.Wait(listLock);
                }

                return queue.Dequeue();
            }
        }

        public override void Enqueue(T item)
        {
            lock (listLock)
            {
                queue.Enqueue(item);
                Monitor.Pulse(listLock);
            }
        }

        public override int Count
        {
            get
            {
                lock (listLock)
                {
                    return queue.Count;
                }
            }
        }
    }
}
