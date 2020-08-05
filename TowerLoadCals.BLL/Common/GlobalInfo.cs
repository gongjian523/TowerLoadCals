using System.Collections.Generic;
using TowerLoadCals.DAL;
using TowerLoadCals.Mode;
using TowerLoadCals.Mode.Electric;

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
            StruCalsParas = new List<StruCalsParasCompose>();
        }

        public string ProjectPath { get; set; }

        public string ProjectName { get; set; }

        public List<StruCalsParasCompose> StruCalsParas { get; set; }

        protected StruCalsLib _struCalsLibParas;
        public StruCalsLib GetStruCalsLibParas()
        {
            if (_struCalsLibParas == null)
            {
                _struCalsLibParas = ProjectUtils.GetInstance().ReadStruCalsLibParas();
            }

            return _struCalsLibParas;
        }

        public ElecCalsSpec _elecCalsSpecParas;
        public ElecCalsSpec GetElecCalsSpecParas()
        {
            if (_elecCalsSpecParas == null)
            {
                _elecCalsSpecParas = ProjectUtils.GetInstance().ReadElecCalsSpecParas();
            }

            return _elecCalsSpecParas;
        }


        public string SmartTowerPath { get; set; }

        public string GetSmartTowerPath()
        {
            if (SmartTowerPath == null || SmartTowerPath == "")
            {
                SmartTowerPath = ProjectUtils.GetInstance().ReadSmartTowerPath();
            }

            return SmartTowerPath;
        }

        public int SmartTowerMode { get; set; }

        public int GetSmartTowerMode()
        {
            SmartTowerMode = ProjectUtils.GetInstance().ReadSmartTowerMode();
            return SmartTowerMode;
        }
        
    }
}
