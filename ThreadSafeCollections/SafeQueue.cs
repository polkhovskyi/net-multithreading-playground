using System.Collections.Generic;

namespace ThreadSafeCollections
{
    class SafeQueue<T> : SimpleQueue<T>
    {
        readonly object listLock = new object();

        private Queue<T> queue = new Queue<T>();

        public override T Dequeue()
        {
            lock (listLock)
            {                
                return queue.Dequeue();
            }
        }

        public override void Enqueue(T item)
        {
            lock (listLock)
            {
                queue.Enqueue(item);
            }
        }

        public override int Count
        {
            get { return queue.Count; }
        }
    }
}
