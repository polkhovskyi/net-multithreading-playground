using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProducerConsumer
{
    class CPUProcessor : BaseProcessor
    {
        public override void Process()
        {
            double fakeRes;
            for (int i = 0; i < int.MaxValue >> 2; i++)
                fakeRes = Math.Sqrt((double)i);
        }
    }
}
