using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Deadlocks
{
    class Program
    {
        static object a = new object();
        static object b = new object();

        static void ThreadAB()
        {
            while (true)
            {
                if (Monitor.TryEnter(a, 0))
                {
                    Console.WriteLine("ThreadAB locked a");
                    Thread.Sleep(0);
                    if (Monitor.TryEnter(b, 0))
                    {
                        Console.WriteLine("ThreadAB locked b, reached");
                        Console.WriteLine("Readched");
                        Thread.Sleep(0);
                        Monitor.Exit(b);
                        Console.WriteLine("ThreadAB unlocked b");
                    }
                    else
                    {
                        //b is locked
                        //we unlock it and try to go futher.
                        Monitor.Exit(a);
                        Console.WriteLine("ThreadAB unlocked a");
                        continue;
                        
                    }

                    Monitor.Exit(a);
                    Console.WriteLine("ThreadAB unlocked a");
                }
                else
                {
                    //Console.WriteLine("aaa");
                    //Monitor.Exit(b);
                    //Monitor.Exit(a);
                    //a is locked
                }

                Thread.Sleep(0);

                //lock (a)
                //{
                //    Thread.Sleep(0);
                //    //Your code here
                //    lock (b)
                //    {
                //        //
                //        Console.WriteLine("Readched");
                //        //Your code here
                //        Thread.Sleep(0);
                //    }
                //    //Your code here
                //}
                //Thread.Sleep(0);
            }
        }

        static void ThreadBA()
        {
            while (true)
            {
                lock (b)
                {
                    Console.WriteLine("ThreadBA locked b");
                    Thread.Sleep(0);
                    lock (a)
                    {
                        Console.WriteLine("ThreadBA locked a");
                        Thread.Sleep(0);
                    }

                    Console.WriteLine("ThreadBA unlocked a");

                }
                Console.WriteLine("ThreadBA unlocked b");

                Thread.Sleep(0);
            }
        }

        static void Main(string[] args)
        {
            Thread ab = new Thread(ThreadAB);
            Thread ba = new Thread(ThreadBA);
            ab.Start();
            ba.Start();
            Thread.Sleep(1000);
            while (true)
            {
                while (ab.ThreadState != ThreadState.WaitSleepJoin && ba.ThreadState != ThreadState.WaitSleepJoin)
                {
                    Thread.Sleep(0);
                }
                
                Console.WriteLine("Deadlock!");
                Thread.Sleep(1000);
            }
        }
    }
}
