using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerLoadCals.BLL.Electric;
using TowerLoadCals.Mode;
using TowerLoadCals.Mode.Common;
using TowerLoadCals.Mode.Electric;
using TowerLoadCals.Mode.Structure;

namespace TowerLoadCals.BLL.Structure
{
    public class EelcRstConvStruInput
    {
        protected TowerElecCals ElecRst;
        protected TowerTemplate StruTemplate;

        /// <summary>
        /// 电气计算的结果转转换成结构计算的输入
        /// </summary>
        /// <param name="elecCals"></param>
        /// <param name="struTemp"></param>
        public EelcRstConvStruInput(TowerElecCals elecCals, TowerTemplate struTemp)
        {
            ElecRst = elecCals;
            StruTemplate = struTemp;
        }



        public StruCalsElecLoad ConvertLoadHang()
        {
            StruCalsElecLoad elecLoad = new StruCalsElecLoad();
            List<ElecCalsWorkConditionBase> wkCdtList = new List<ElecCalsWorkConditionBase>();
            List<WireElecLoadLine> LineElecLoads = new List<WireElecLoadLine>();

            
            for (int i = 1; i <= StruTemplate.WorkConditongs.Count; i++)
            {
                string wkNameS = StruTemplate.WorkConditongs[i];
                if (wkNameS == null)
                    continue;
                string wkNameE = WorkConditionMaps(wkNameS);

                //查找工况
                GetWorkCondition(wkNameE, out ElecCalsWorkConditionBase wkCdt);
                wkCdt.Id = i;
                wkCdt.Name = wkNameS;
                wkCdtList.Add(wkCdt);

                for(int j = 0; j < StruTemplate.Wires.Count; j++)
                {
                    string wire = StruTemplate.Wires[j];
                    
                    //GetWindLoad();
                }
            }

            return elecLoad;
        }


        public StruCalsElecLoad ConvertLoadStrain()
        {
            StruCalsElecLoad elecLoad = new StruCalsElecLoad();
            List<ElecCalsWorkConditionBase> wkCdtList = new List<ElecCalsWorkConditionBase>();

            for (int i = 1; i <= StruTemplate.WorkConditongs.Count; i++)
            {
                string wkNameS = StruTemplate.WorkConditongs[i];
                if (wkNameS == null)
                    continue;
                string wkNameE = WorkConditionMaps(wkNameS);

                //查找工况
                GetWorkCondition(wkNameE, out ElecCalsWorkConditionBase wkCdt);
                wkCdt.Id = i;
                wkCdt.Name = wkNameS;
                wkCdtList.Add(wkCdt);
            }

            return elecLoad;
        }


        //
        private void GetWorkCondition(string wkCdtName, out ElecCalsWorkConditionBase wkCdt)
        {
            //在工况这里需要增加对 
            wkCdt = new ElecCalsWorkConditionBase();
            if (ElecRst.TowerType == "悬垂塔")
            {
                //悬垂塔只需要一侧的工况
                //这儿用的是导线中的工况数据，事实上导地线中任何一个的工况都是可行的，它们计算方式是一样的。
                var wkCdtDes = ((TowerHangElecCals)ElecRst).SideRes.IndWire.WeatherParas.WeathComm.Where(item => item.Name == wkCdtName).FirstOrDefault();
                if (wkCdtDes == null)
                    return;
                wkCdt.Temperature = wkCdtDes.Temperature;
                wkCdt.WindSpeed = wkCdtDes.WindSpeed;
                wkCdt.IceThickness = wkCdtDes.IceThickness;
            }
            else
            {
                //耐张塔需要两侧的工况
                //采用导线的工况数据的理由和悬垂塔一样
                var wkCdtDesB = ((TowerStrainElecCals)ElecRst).BackSideRes.IndWire.WeatherParas.WeathComm.Where(item => item.Name == wkCdtName).FirstOrDefault();
                if (wkCdtDesB == null)
                    wkCdtDesB = new ElecCalsWorkCondition();

                var wkCdtDesF = ((TowerStrainElecCals)ElecRst).FrontSideRes.IndWire.WeatherParas.WeathComm.Where(item => item.Name == wkCdtName).FirstOrDefault();
                if (wkCdtDesF == null)
                    wkCdtDesF = new ElecCalsWorkCondition();

                wkCdt.Temperature = Math.Max(wkCdtDesB.Temperature, wkCdtDesF.Temperature);
                wkCdt.WindSpeed = Math.Max(wkCdtDesB.WindSpeed, wkCdtDesF.WindSpeed);  
                wkCdt.IceThickness = Math.Max(wkCdtDesB.IceThickness, wkCdtDesF.IceThickness);
            }
        }


        //private double GetWindHang(string wkCdtName, string wireType)
        //{
        //    //地线
        //    if(wireType.Contains("地"))
        //    {
        //        var load = ElecRst.PhaseTraList[3].LoadList.Where(item => item.GKName == wkCdtName).FirstOrDefault();
        //        if (load == null)
        //            load = new LoadThrDe();
        //        var loadAn = ElecRst.PhaseTraList[4].LoadList.Where(item => item.GKName == wkCdtName).FirstOrDefault();
        //        if (loadAn == null)
        //            loadAn = new LoadThrDe();
        //        int maxIdx = load.HorFor > loadAn.HorFor ? 3:4;
        //        if(wkCdtName == )
        //    }
        //}

        protected string WorkConditionMaps(string wkCdtName, bool isStruToElec= true)
        {
            string wkCdtNameRst = "";
            if (isStruToElec)
            {
                //根据结构的工况名字查找电气工况名称
                if (wkCdtName == WkCdtNameStru.Wind)
                    return WkCdtNameElec.Wind;
            }
            else
            {
                //根据电气的工况名字查找结构工况名称
                if (wkCdtName == WkCdtNameElec.Wind)
                    return WkCdtNameStru.Wind;
            }

            return wkCdtNameRst;
        }

    }
}
