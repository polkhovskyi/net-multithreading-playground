using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadSafeCollections
{
    public class QueueTests
    {
        static IQueueWriter<object> producerConsumer;

        public static void Run(IQueueWriter<object> writer)
        {
            producerConsumer = writer;
            new Thread(new ThreadStart(ConsumerJob)).Start();
            new Thread(new ThreadStart(ProducerJob)).Start();
            Random rng = new Random(0);
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("Producing {0}", i);
                producerConsumer.Produce(i);
                Thread.Sleep(rng.Next(1000));
            }
        }

        static void ProducerJob()
        {
            Random rng = new Random(0);
            for (int i = 10; i < 20; i++)
            {
                Console.WriteLine("Producing {0}", i);
                producerConsumer.Produce(i);
                Thread.Sleep(rng.Next(1000));
            }
        }

        static void ConsumerJob()
        {
            Random rng = new Random(1);
            for (int i = 0; i < 20; i++)
            {
                object o = producerConsumer.Consume();
                Console.WriteLine("\t\t\t\tConsuming {0}", o);
                Thread.Sleep(rng.Next(1000));
            }
        }
    }


    public interface IQueueWriter<T>
    {
        void Produce(T value);
        T Consume();
    }

    public class BlockingQueueWriter<T> : IQueueWriter<T>
    {
        BlockingQueue<T> queue = new BlockingQueue<T>();

        public void Produce(T o)
        {

            queue.Enqueue(o);

        }

        public T Consume()
        {

            return queue.Dequeue();
        }
    }

    public class QueueWriter<T> : IQueueWriter<T>
    {
        Queue<T> queue = new Queue<T>();

        public void Produce(T o)
        {

            queue.Enqueue(o);

        }

        public T Consume()
        {

            return queue.Dequeue();
        }
    }

    public class SafeQueueWriter<T> : IQueueWriter<T>
    {
        SafeQueue<T> queue = new SafeQueue<T>();

        public void Produce(T o)
        {

            queue.Enqueue(o);

        }

        public T Consume()
        {
            try
            {
                return queue.Dequeue();
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("! queue is empty !");
            }

            return default(T);
        }
    }


}

