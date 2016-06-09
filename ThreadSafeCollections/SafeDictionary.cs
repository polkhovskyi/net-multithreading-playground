using System.Collections.Generic;
using System.Threading;

namespace ThreadSafeCollections
{
    class SafeDictionary<TKey, TValue>
    {
        private Dictionary<TKey, TValue> dict = new Dictionary<TKey, TValue>();
        readonly object listLock = new object();

        public TValue Get(TKey key)
        {
            lock (listLock)
            {
                while (!dict.ContainsKey(key))
                {
                    Monitor.Wait(listLock);
                }

                return dict[key];
            }
        }

        public void Add(TKey key, TValue value)
        {
            lock (listLock)
            {
                dict[key] = value;
                Monitor.Pulse(listLock);
            }
        }
    }
}
