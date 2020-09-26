using System.Collections.Generic;
using System.IO;
using System.Linq;
using TowerLoadCals.BLL.Electric;
using TowerLoadCals.DAL;
using TowerLoadCals.DAL.Common;
using TowerLoadCals.Mode;
using TowerLoadCals.Mode.Common;
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

        #region 基本数据

        protected List<Weather> _localWeathers;
        public List<Weather> GetLocalWeathers()
        {
            if (_localWeathers == null || _localWeathers.Count == 0)
            {
                _localWeathers = ProjectUtils.GetInstance().ReadLocalWeathers();
            }
            return _localWeathers;
        }

        public List<string> GetLocalWeatherNames()
        {
            var waes = GetLocalWeathers();
            if (waes == null || waes.Count == 0)
                return new List<string>();
            else
                return waes.Select(it => it.Name).ToList();
        }

        public Weather GetLocalWeatherByName(string name)
        {
            if (name == null)
                return new Weather();
            var waes = GetLocalWeathers();

            if (waes == null && waes.Count == 0)
                return new Weather();
            else
            {
                var wea = waes.Where(item => item.Name == name).FirstOrDefault();
                return wea == null ? new Weather() : wea;
            }
        }


        protected List<WireType> _localWires;
        public List<Wire> GetLocalWires(bool isInd)
        {
            if (_localWires == null || _localWires.Count == 0)
            {
                _localWires = ProjectUtils.GetInstance().ReadLocalWires();
            }
            var type = _localWires.Where(item => item.Type == (isInd ? "导线" : "地线")).FirstOrDefault();
            return type == null ? new List<Wire>() : type.Wires;
        }

        public List<string> GetLocalWireNames(bool isInd)
        {
            var wires = GetLocalWires(isInd);
            if (wires == null || wires.Count == 0)
                return new List<string>();
            else
                return wires.Select(it => it.ModelSpecification).ToList();
        }

        public Wire GetLocalWireByName(bool isInd, string name)
        {
            if (name == null)
                return new Wire();

            var wires = GetLocalWires(isInd);

            if (wires == null && wires.Count == 0)
                return new Wire();
            else
            {
                var wire = wires.Where(item => item.ModelSpecification == name).FirstOrDefault();
                return wire == null ? new Wire() : wire;
            }
        }


        protected List<FitDataCollection> _localFitDatas;
        public List<FitData> GetLocalFitDatas(string type)
        {
            if (_localFitDatas == null || _localFitDatas.Count == 0)
            {
                _localFitDatas = ProjectUtils.GetInstance().ReadLocalFitDatas();
            }
            var fitDatas = _localFitDatas.Where(item => item.Type == type).FirstOrDefault();
            return fitDatas == null ? new List<FitData>() : fitDatas.FitDatas;
        }

        public List<string> GetLocalFitDataNames(string type)
        {
            var fitDatas = GetLocalFitDatas(type);
            if (fitDatas == null || fitDatas.Count == 0)
                return new List<string>();
            else
                return fitDatas.Select(it => it.Name).ToList();
        }

        public FitData GetLocalFitDataByName(string type, string name)
        {
            if (name == null)
                return new FitData();

            var fitDatas = GetLocalFitDatas(type);

            if (fitDatas == null && fitDatas.Count == 0)
                return new FitData();
            else
            {
                var fitData = fitDatas.Where(item => item.Name == name).FirstOrDefault();
                return fitData == null ? new FitData() : fitData;
            }
        }

        protected List<StrDataCollection> _localStrDatas;
        public List<StrData> GetLocalStrDatas(string type)
        {
            if (_localStrDatas == null || _localStrDatas.Count == 0)
            {
                _localStrDatas = ProjectUtils.GetInstance().ReadLocalStrDatas();
            }
            var strDatas = _localStrDatas.Where(item => item.Type == type).FirstOrDefault();
            return strDatas == null ? new List<StrData>() : strDatas.StrDatas;
        }

        public List<string> GetLocalStrDataNames(string type)
        {
            var strDatas = GetLocalStrDatas(type);
            if (strDatas == null || strDatas.Count == 0)
                return new List<string>();
            else
                return strDatas.Select(it => it.Name).ToList();
        }

        public StrData GetLocalStrDataByName(string type, string name)
        {
            if (name == null)
                return new StrData();

            var strDatas = GetLocalStrDatas(type);

            if (strDatas == null && strDatas.Count == 0)
                return new StrData();
            else
            {
                var strData = strDatas.Where(item => item.Name == name).FirstOrDefault();
                return strData == null ? new StrData() : strData;
            }
        }

        protected List<TowerStrData> _localTowerStrs;
        public List<TowerStrData> GetLocalTowerStrs()
        {
            if (_localTowerStrs == null || _localTowerStrs.Count == 0)
            {
                string filePath = ProjectPath + "\\BaseData\\TowerStr.xml";
                _localTowerStrs = TowerStrDataReader.ReadLoadFile(filePath);
            }
            return _localTowerStrs;
        }

        public List<string> GetLocalTowerStrNames()
        {
            var waes = GetLocalTowerStrs();
            if (waes == null || waes.Count == 0)
                return new List<string>();
            else
                return waes.Select(it => it.Name).ToList();
        }

        public TowerStrData GetLocalTowerStrByName(string name)
        {
            if (name == null)
                return new TowerStrData();
            var towerSerial = GetLocalTowerStrs();

            if (towerSerial == null && towerSerial.Count == 0)
                return new TowerStrData();
            else
            {
                var tower = towerSerial.Where(item => item.Name == name).FirstOrDefault();
                return tower == null ? new TowerStrData() : tower;
            }
        }

        #endregion

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

        protected ElecCalsSpec _elecCalsSpecParas;
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

        protected List<ElecCalsCommRes> _elecCalsCommParasList;
        public List<ElecCalsCommRes> GetElecCalsCommParasList()
        {
            if (_elecCalsCommParasList == null)
            {
                _elecCalsCommParasList = ProjectUtils.GetInstance().ReadElecCalsCommParas();
            }
            return _elecCalsCommParasList;
        }
        public List<string> GetElecCalsCommParaNames()
        {
            var commParas = GetElecCalsCommParasList();
            if (commParas == null || commParas.Count == 0)
                return new List<string>();
            else
                return commParas.Select(it => it.Name).ToList();
        }
        public void SaveElecCalsCommParasList()
        {
            if (_elecCalsCommParasList != null && _elecCalsCommParasList.Count != 0)
            {
                ProjectUtils.GetInstance().SaveElecCalsCommParas(_elecCalsCommParasList);
            }
        }

        public ElecCalsCommRes GetElecCalsCommParasByName(string name)
        {
            if (name == null)
                return new ElecCalsCommRes();

            var commParas = GetElecCalsCommParasList();
            if (commParas == null || commParas.Count == 0)
                return new ElecCalsCommRes();
            else
            {
                var commPara = _elecCalsCommParasList.Where(item => item.Name == name).FirstOrDefault();
                return commPara == null ? new ElecCalsCommRes() : commPara;
            }
        }

        protected List<ElecCalsSideRes> _elecCalsSideParasList;
        public List<ElecCalsSideRes> GetElecCalsSideParasList()
        {

            if (_elecCalsSideParasList == null)
            {
                _elecCalsSideParasList = ProjectUtils.GetInstance().ReadElecCalsSideParas();
            }
            return _elecCalsSideParasList;
        }
        public List<string> GetElecCalsSideParaNames()
        {
            var sideParas = GetElecCalsSideParasList();
            if (sideParas == null || sideParas.Count == 0)
                return new List<string>();
            else
                return sideParas.Select(it => it.Name).ToList();
        }
        public void SaveElecCalsSideParasList()
        {
            if (_elecCalsSideParasList != null && _elecCalsSideParasList.Count != 0)
            {
                ProjectUtils.GetInstance().SaveElecCalsSideParas(_elecCalsSideParasList);
            }
        }

        public ElecCalsSideRes GetElecCalsSideParasByName(string name)
        {
            if (name == null)
                return new ElecCalsSideRes();

            var sideParas = GetElecCalsSideParasList();
            if (sideParas == null || sideParas.Count == 0)
                return new ElecCalsSideRes();
            else
            {
                var sidePara = sideParas.Where(item => item.Name == name).FirstOrDefault();
                return sidePara == null ? new ElecCalsSideRes() : sidePara;
            }
        }


        protected List<ElecCalsTowerRes> _elecCalsTowerParasList;
        public List<ElecCalsTowerRes> GetElecCalsTowerParasList()
        {
            if (_elecCalsTowerParasList == null)
            {
                _elecCalsTowerParasList = ProjectUtils.GetInstance().ReadElecCalsTowerParas();
            }
            return _elecCalsTowerParasList;
        }
        public List<string> GetElecCalsTowerParaNames()
        {
            var towerParas = GetElecCalsTowerParasList();
            if (towerParas == null || towerParas.Count == 0)
                return new List<string>();
            else
                return towerParas.Select(it => it.Name).ToList();
        }
        public void SaveElecCalsTowerParasList()
        {
            if (_elecCalsTowerParasList != null && _elecCalsTowerParasList.Count != 0)
            {
                ProjectUtils.GetInstance().SaveElecCalsTowerParas(_elecCalsTowerParasList);
            }
        }

        public ElecCalsTowerRes GetElecCalsTowerParasByName(string name)
        {
            if (name == null)
                return new ElecCalsTowerRes();

            var towerParas = GetElecCalsTowerParasList();
            if (towerParas == null || towerParas.Count == 0)
                return new ElecCalsTowerRes();
            else
            {
                var towerPara = towerParas.Where(item => item.Name == name).FirstOrDefault();
                return towerPara == null ? new ElecCalsTowerRes() : towerPara;
            }
        }

        protected List<TowerSeq> _TowerSeqList;
        public TowerSeq GetTowerSequenceBySequenceName(string seqName)
        {
            TowerSeq towerSeq = new TowerSeq();

            if (_TowerSeqList == null || _TowerSeqList.Where(item => item.Name == seqName).Count() == 0)
            {
                string filePath = ProjectPath + "\\" + ConstVar.TowerSequenceStr + "\\" + seqName + "\\TowerSequenceStr.xml";
                if (File.Exists(filePath))
                {
                    var towerList = TowerSerialReader.ReadXml(ProjectPath, seqName);

                    towerSeq.Name = seqName;
                    towerSeq.Towers = towerList;
                }
            }
            else
            {
                towerSeq = _TowerSeqList.Where(item => item.Name == seqName).FirstOrDefault();
            }

            return towerSeq;
        }

        public List<string> GetSelecedTowerNamesInSequence(string seqName)
        {
            TowerSeq towerSeq = GetTowerSequenceBySequenceName(seqName);

            if (towerSeq == null || towerSeq.Towers == null)
                return new List<string>();
            else
            {
                return towerSeq.Towers.Where(item => item.IsChecking == true).Select(item => item.TowerName).ToList();
            }
        }
    }
}
