using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ProducerConsumer
{
    class IOProcessor : BaseProcessor
    {
        public override void Process()
        {
            Thread.Sleep(1000);
        }
    }
}
