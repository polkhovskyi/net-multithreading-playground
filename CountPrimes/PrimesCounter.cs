using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CountPrimes
{
    class PrimesCounter
    {
        private static Int32 total = 0;
        /// <summary>
        /// Implement this.
        /// Method will block execution until calculation is done.
        /// </summary>
        /// <param name="max">Search for primes up to this number</param>
        /// <param name="threads">Number of threads to use</param>
        /// <returns>Number of primes</returns>
        public int CountUpTo(int max, int threads)
        {
            total = 0;

            ManualResetEvent[] events = new ManualResetEvent[threads];
            var itemsPerThread = max / threads;

            for (int i = 0; i < events.Length; i++)
            {
                var lower = i * itemsPerThread;
                var upper = (i == events.Length - 1) ? max : lower + itemsPerThread;
                events[i] = new ManualResetEvent(false);
                CountPart r = new CountPart(events[i], lower, upper);
                new Thread(new ThreadStart(r.Run)).Start();
            }

            WaitHandle.WaitAll(events);
            return total;
        }

        private class CountPart
        {
            ManualResetEvent _manualResetEvent;
            private int _start;
            private int _end;
            internal CountPart(ManualResetEvent ev, int start, int end)
            {
                _manualResetEvent = ev;
                _start = start;
                _end = end;
            }

            private static bool IsPrime(int number)
            {
                if ((number & 1) == 0)
                    return (number != 2) ? false : true;

                for (int i = 3; i * i <= number; i += 2)
                {
                    if (number % i == 0)
                        return false;
                }
                return number != 1;
            }

            internal void Run()
            {
                var count = 0;
                for (int i = _start; i < _end; i++)
                {
                    if (IsPrime(i))
                    {
                        count++;
                    }
                }

                Interlocked.Add(ref total, count);
                _manualResetEvent.Set();
            }
        }
    }
}
