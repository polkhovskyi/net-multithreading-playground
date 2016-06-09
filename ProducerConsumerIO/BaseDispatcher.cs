using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProducerConsumer
{
    abstract class BaseDispatcher
    {
            public ConcurrentEnv Env { get; set; }

            public abstract void OnReceived(object msg);

            public abstract void Initialize(Action<object> process);
    }
}
