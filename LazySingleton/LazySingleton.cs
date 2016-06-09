using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LazySingleton
{
    class LazySingleton
    {
        private static volatile LazySingleton instance;
        private static object syncRoot = new Object();

        private LazySingleton() { }

        public static LazySingleton Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new LazySingleton();
                    }
                }

                return instance;
            }
        }
    }
}