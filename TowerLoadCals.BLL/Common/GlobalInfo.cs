﻿using System.Collections.Generic;
using TowerLoadCals.DAL;
using TowerLoadCals.Mode;

namespace TowerLoadCals.BLL
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

        private GlobalInfo()
        {
            StruCalsParas = new List<StruCalsParas>();
        }

        public string ProjectPath { get; set; }

        public string ProjectName { get; set; }

        public List<StruCalsParas> StruCalsParas { get; set; }

        public StruCalsLib StruCalsLibParas { get; set; }

        public StruCalsLib GetStruCalsLibParas()
        {
            if( StruCalsLibParas == null )
            {
                ProjectUtils.GetInstance().ReadStruCalsLibParas();
            }

            return StruCalsLibParas;
        }
    }
}
