using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace CountPrimes
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 64; i > 0; i--)
            {
                var threadsCount = i;
                Run(int.MaxValue/10, threadsCount);
                //Run(10000000, threadsCount);
                //Run(100000000, threadsCount);
                //Run(1000000000, threadsCount);
            }


            Console.ReadLine();
        }

        private static void Run(int upTo, int threads)
        {
            Stopwatch sw = new Stopwatch();
            PrimesCounter counter = new PrimesCounter();
            sw.Start();
            int primes = counter.CountUpTo(upTo, threads);
            sw.Stop();
            Console.WriteLine("Primes({2}): {0} in {1} msec using {3} threads", primes, sw.ElapsedMilliseconds, upTo, threads);

        }
    }

}
