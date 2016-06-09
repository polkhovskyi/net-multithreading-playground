using System;
using System.Collections.Generic;
using System.Threading;

namespace ThreadSafeCollections
{
    class SafeDictionaryTest
    {
        static IDictionaryWriter<int, string> producerConsumer;

        public static void Run(IDictionaryWriter<int,string> writer)
        {
            producerConsumer = writer;
            new Thread(new ThreadStart(ConsumerJob)).Start();
            new Thread(new ThreadStart(ProducerJob)).Start();
            Random rng = new Random(0);
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("Producing {0}", i);
                producerConsumer.Produce(i, "Main Thread " + i);
                Thread.Sleep(rng.Next(1000));
            }
        }

        static void ProducerJob()
        {
            Random rng = new Random(0);
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("Producing {0}", i);
                producerConsumer.Produce(i, "Producer Job " + i);
                Thread.Sleep(rng.Next(1000));
            }
        }

        static void ConsumerJob()
        {
            Random rng = new Random(1);
            for (int i = 0; i < 10; i++)
            {
                object o = producerConsumer.Consume(i);
                Console.WriteLine("\t\t\t\tConsuming {0}", o);
                Thread.Sleep(rng.Next(1000));
            }
        }
    }

    public interface IDictionaryWriter<T, V>
    {
        void Produce(T key, V value);
        V Consume(T key);
    }

    public class DictionaryWriter<T, V> : IDictionaryWriter<T, V>
    {
        private Dictionary<T, V> dict = new Dictionary<T, V>();

        public void Produce(T key, V value)
        {
            dict.Add(key, value);
        }

        public V Consume(T key)
        {
            return dict[key];
        }
    }


    public class SafeDictionaryWriter<T, V> : IDictionaryWriter<T, V>
    {
        private SafeDictionary<T, V> dict = new SafeDictionary<T, V>();

        public void Produce(T key, V value)
        {
            dict.Add(key, value);
        }

        public V Consume(T key)
        {
            return dict.Get(key);
        }
    }
}
