using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace ProducerConsumer
{
    class ConcurrentEnv
    {
        private HashSet<object> pending;
        AutoResetEvent canStop = new AutoResetEvent(false);
        private BaseProcessor proc;

        public void Start<T, P>(int number) 
            where T : BaseDispatcher, new()
            where P : BaseProcessor, new() 
        {
            Console.WriteLine("Initializing");
            proc = new P();
            BaseDispatcher disp = new T();
            disp.Env = this;
            disp.Initialize(Process);
            Console.WriteLine("Sending {0} messages", number);

            var messages = Enumerable.Range(0, number).Select(x => new object()).ToList();
            pending = new HashSet<object>(messages);
            Stopwatch sw = new Stopwatch();
            Stopwatch swTotal = new Stopwatch();
            swTotal.Start();
            int i = 0;
            foreach (object o in messages)
            {
                sw.Reset();
                sw.Start();
                disp.OnReceived(o);
                sw.Stop();
                if (sw.ElapsedMilliseconds >= 1000)
                    throw new ApplicationException("Dispatcher must accept messages faster");
                i++;
                Console.WriteLine("Received: {0}", i);
            }
            canStop.WaitOne();
            swTotal.Stop();
            Console.WriteLine("Processing finished. Total msec: {0}", swTotal.ElapsedMilliseconds);
        }

        public void Process(object msg)
        {
            int left;
            lock (pending)
            {
                if (!pending.Remove(msg))
                    throw new ApplicationException("Attempt to process message more than once");
                left = pending.Count;
            }
            proc.Process();
            Console.WriteLine("Remaining: {0}", left);
            if (left == 0)
                canStop.Set();
        }
    }

}
