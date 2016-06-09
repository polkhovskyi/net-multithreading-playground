using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreadSafeCollections
{
    abstract class SimpleQueue<T>
    {
        public abstract int Count { get; }

        public abstract T Dequeue();

        public abstract void Enqueue(T item);
    }
}
