using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.Common
{
    public class GlobalInfo
    {
        private static GlobalInfo singleton;

        private static readonly object locker = new object();

        public static GlobalInfo GetInstance()
        {
            if (singleton == null)
            {
                lock (locker)
                {
                    if (singleton == null)
                    {
                        singleton = new GlobalInfo();
                    }
                }
            }
            return singleton;
        }

        public string ProjectPath { get; set; }

        public string ProjectName { get; set; }
    }
}
