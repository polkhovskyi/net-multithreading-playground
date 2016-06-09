using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MultithreadingTasks
{
    class Program
    {
        static Dictionary<int, object> dict = new Dictionary<int, object>();

        static void meth()
        {
            while (true)
            {
                for (int i = 0; i < 100; i++)
                {
                    dict.Add(i, DBNull.Value);
                }
                for (int i = 0; i < 100; i++)
                {
                    dict.Remove(i);
                }
            }
        }

        static void meth2()
        {
            while (true)
            {
                for (int i = 100; i < 200; i++)
                {
                    dict.Add(i, DBNull.Value);
                }
                for (int i = 100; i < 200; i++)
                {
                    dict.Remove(i);
                }
            }
        }

        static void Main(string[] args)
        {
            Thread t1 = new Thread(meth);
            Thread t2 = new Thread(meth2);
            t1.Start();
            t2.Start();
            t1.Join();
            t2.Join();
        }
    }
}
