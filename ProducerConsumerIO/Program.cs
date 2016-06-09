using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ProducerConsumer
{
    class IOTaskDispatcher : BaseDispatcher
    {
        Action<object> _action;
        ConcurrentQueue<object> queue;
        /// <summary>
        /// Called before recieving messages, can execute for any time
        /// </summary>
        public override void Initialize(Action<object> action)
        {
            _action = action;
            queue = new ConcurrentQueue<object>();
            new Thread(() => Run()).Start();
        }

        private void Run()
        {
            while(true)
            {
                if(queue.Count > 0)
                {
                    object outObject;
                    if (queue.TryDequeue(out outObject))
                    {
                        //ThreadPool.QueueUserWorkItem((e) => _action(outObject));
                        new Thread(() => _action(outObject)).Start();
                        //_action(outObject);
                    }
                }
            }
        }
        /// <summary>
        /// This code is executed everytime message is received
        /// This method can't execute too long, so actual processing
        /// should take place somewhere else
        /// </summary>
        /// <param name="msg">Call Env.Process for this object</param>
        public override void OnReceived(object msg)
        {
            queue.Enqueue(msg);
            //new Thread(() => _action(msg)).Start();
        }
    }

    class CPUTaskDispatcher : BaseDispatcher
    {
        Action<object> _action;
        ConcurrentQueue<object> queue;
        /// <summary>
        /// Called before recieving messages, can execute for any time
        /// </summary>
        public override void Initialize(Action<object> action)
        {
            _action = action;
            queue = new ConcurrentQueue<object>();
            new Thread(() => Run()).Start();
        }

        private void Run()
        {
            while (true)
            {
                if (queue.Count > 0)
                {
                    object outObject;
                    if (queue.TryDequeue(out outObject))
                    {
                        ThreadPool.QueueUserWorkItem((e) => _action(outObject));
                        //new Thread(() => _action(outObject)).Start();
                    }
                }
            }
        }
        /// <summary>
        /// This code is executed everytime message is received
        /// This method can't execute too long, so actual processing
        /// should take place somewhere else
        /// </summary>
        /// <param name="msg">Call Env.Process for this object</param>
        public override void OnReceived(object msg)
        {
            queue.Enqueue(msg);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            ConcurrentEnv env = new ConcurrentEnv();
            env.Start<IOTaskDispatcher, IOProcessor>(5);
            Console.ReadLine();

            env.Start<IOTaskDispatcher, IOProcessor>(10);
            Console.ReadLine();

            env.Start<IOTaskDispatcher, IOProcessor>(100);
            Console.ReadLine();

            env.Start<CPUTaskDispatcher, CPUProcessor>(5);
            Console.ReadLine();

            env.Start<CPUTaskDispatcher, CPUProcessor>(10);
            Console.ReadLine();

            env.Start<CPUTaskDispatcher, CPUProcessor>(100);
            Console.ReadLine();
        }
    }
}
