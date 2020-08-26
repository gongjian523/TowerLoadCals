using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using TowerLoadCals.Common;
using TowerLoadCals.Mode;
using TowerLoadCals.Mode.Electric;

namespace TowerLoadCals.BLL.Electric
{
    public class ElecCalsWire
    {

        public int ID {get; set;}

        /// <summary>
        /// 导线型号
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 截面积 mm2
        /// </summary>
        public double Sec { get; set; }

        /// <summary>
        /// 直径 mm
        /// </summary>
        public double Dia { get; set; }

        /// <summary>
        /// 单位质量  Kg/Km,默认换算为kg/m
        /// </summary>
        public double Wei { get; set; }

        /// <summary>
        /// 综合弹性系数 Gpa 10^6 N/m2 
        /// 是否就是弹性模块
        /// </summary>
        public double Elas { get; set; }

        /// <summary>
        /// 是否就是弹性模量
        /// </summary>
        public double ElasM { get; set; }

        /// <summary>
        /// 线性膨胀系数 10^-6/℃
        /// </summary>
        public double Coef { get; set; }

        /// <summary>
        /// 计算拉断力 N//计算破断力?
        /// </summary>
        public double Fore { get; set; }

        /// <summary>
        /// #0：导线；1：普通地线；2：OPGW
        /// </summary>
        public int bGrd { get; set; }

        /// <summary>
        /// #降温值
        /// </summary>
        public double DecrTem { get; set; }

        /// <summary>
        /// 导线分裂数
        /// </summary>
        public int DevideNum { get; set; }

        /// <summary>
        /// 控制应力
        /// </summary>
        public double CtrlStress { get; set; }

        /// <summary>
        /// 平均应力
        /// </summary>
        public double AvaStress { get; set; }

        /// <summary>
        /// 控制工况名字，中间变量，主要为了做输出
        /// </summary>
        public string CtrlGkName { get; set; }

        /// <summary>
        /// 控制工况，中间变量，主要为了做输出
        /// </summary>
        public ElecCalsWorkCondition CtrlGk { get; set; }

        /// <summary>
        /// 控制工况应力，中间变量，主要为了做输出
        /// </summary>
        public double CtrlGkStress { get; set; }

        /// <summary>
        /// 比载字典
        /// </summary>
        [XmlIgnore]
        public Dictionary<string, BZResult> BzDic { get; set; }

        /// <summary>
        /// 应力字典
        /// </summary>
        [XmlIgnore]
        public Dictionary<string, double> ForSpanDic { get; set; }


        /// <summary>
        /// 垂直档距
        /// </summary>
        [XmlIgnore]
        public Dictionary<string, double> VerSpanDic { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ElecCalsWeaRes  WeatherParas { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ElecCalsCommRes CommParas { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ElecCalsSideRes SideParas { get; set; }

        [XmlIgnore]
        public Dictionary<string, double>  YLTable { get; set; }

        [XmlIgnore]
        public Dictionary<string, double> YLTableXls { get; set; }

        public string CtrNaSave { get; set; }

        public double Comg1 { get; set; }

        public double EffectPara { get; set; }
        public double SafePara { get; set; }
        public double AvePara { get; set; }
        public double OPGWEffectPara { get; set; }
        public double OPGWSafePara { get; set; }
        public double OPGWAnPara { get; set; }


        /// <summary>
        ///断线张力系数
        /// </summary>
        public double BreakTensionPara { get; set; }

        /// <summary>
        ///不均匀冰张力系数
        /// </summary>
        public double UnbaTensionPara { get; set; }

        public List<string> WorkCdtNames
        {
            get { return WeatherParas == null ?  new List<string>() : (bGrd == 0 ? WeatherParas.NameOfWkCalsInd : WeatherParas.NameOfWkCalsGrd); }      
        }

        public ElecCalsWire()
        {
        }

        public ElecCalsWire(string name = "", int id = 0, double sec = 0, double dia = 0, double wei = 0, double elas = 0, double elasM = 0,
            double coef = 0, double fore = 0, int grd = 0, double decrTem = 0, int devide = 1)
        {
            ID = id;
            Name = name;
            Sec = sec;
            Dia = dia;
            Wei = wei / 1000;
            Elas = elas;
            ElasM = elasM;
            Coef = coef * 1e-6;
            Fore = fore;
            bGrd = grd;
            DecrTem = decrTem;
            DevideNum = devide;
            BzDic = new Dictionary<string, BZResult>();
            ForSpanDic = new Dictionary<string, double>();
        }

        /// <summary>
        /// 更新参数
        /// </summary>
        /// <param name="weaData"></param>
        /// <param name="commPara">通用系数</param>
        /// <param name="wirePara">导线参数</param>
        public void UpdataPara(ElecCalsWeaRes weaData, ElecCalsCommRes commPara, ElecCalsSideRes sideParas, string towerType, string iceArea)
        {
            WeatherParas = XmlUtils.Clone(weaData);
            CommParas = commPara;
            SideParas = sideParas;
            UpdateTensinPara(towerType, iceArea);
        }

        protected void UpdateTensinPara(string towerType, string iceArea)
        {
            if(WeatherParas.WeathComm.Where(item => item.Name == "最大覆冰").Count() <= 0)
            {
                BreakTensionPara = 0;
                UnbaTensionPara = 0;
                return;
            }

            double iceThick = WeatherParas.WeathComm.Where(item => item.Name == "最大覆冰").First().IceThickness;

            //导线
            if (bGrd == 0)
            {
                BreakTensionPara = ElecCalsToolBox.UBlanceK(towerType, iceArea, iceThick, CommParas.Terrain, "导线", DevideNum);
                UnbaTensionPara = ElecCalsToolBox.IBlanceK(towerType, iceArea, iceThick, "导线");
        }
            //地线
            else
            {
                BreakTensionPara = ElecCalsToolBox.UBlanceK(towerType, iceArea, iceThick, CommParas.Terrain, "地线");
                UnbaTensionPara = ElecCalsToolBox.IBlanceK(towerType, iceArea, iceThick, "地线");
            }
        }



        /// <summary>
        /// 计算指定档距下应力比载并存入对象中,一般采用这个接口计算
        /// </summary>
        /// <param name="span"></param>
        public void SaveYLTabel(double span)
        {
            YLTable = CalFTable(span);
        }

        /// <summary>
        /// #计算导线比载,默认采用老规范计算，计算参数取1，新规范取2
        /// </summary>
        /// <param name="CalType"></param>
        public void CalBZ(int CalType = 1)
        {

            //计算比载
            var graAcc = CommParas.GraAcc;

            BzDic = new Dictionary<string, BZResult>();

            //换算最大风速值
            var maxWindCon = WeatherParas.WeathComm.Where(item => item.Name == "换算最大风速").First().WindSpeed;          
            var maxWindSt = WeatherParas.WeathComm.Where(item => item.Name == "最大风速").First().WindSpeed;

            double maxUpbaWindSt = 0, maxUpbaWindCon = 0;

            //如果存在不均匀风工况
            if (WeatherParas.WeathComm.Where(item => item.Name == "不均匀风").Count()> 0)
            {
                maxUpbaWindSt = WeatherParas.WeathComm.Where(item => item.Name == "不均匀风").First().WindSpeed;
                maxUpbaWindCon = WeatherParas.WeathComm.Where(item => item.Name == "换算不均匀风").First().WindSpeed;
            }

            //g1为统一值
            Comg1 = Wei * graAcc / Sec;

            foreach(var weaItem in WeatherParas.WeathComm)
            {

                if (CalType == 1)
                {
                    BZResult bz = new BZResult();

                    //温度
                    var temVal = weaItem.Temperature;
                    double winVal;

                    // 大风工况采用换算后的数值计算
                    if (weaItem.Name == "最大风速")
                    {
                        winVal = maxWindCon;
                    }
                    else if (weaItem.Name == "不均匀风")
                    {
                        winVal = maxUpbaWindCon;
                    }
                    else
                    {
                        winVal = weaItem.WindSpeed;
                    }

                    double iceVal = weaItem.IceThickness;

                    double alpha = ElecCalsToolBox.WindAlpha(winVal, iceVal, 1);
                    bz.g1 = Comg1;

                    double g2 = graAcc * 0.9 * Math.PI * iceVal * (iceVal + Dia) * 1e-3 / Sec;
                    bz.g2 = g2;

                    double g3 = Comg1 + g2;
                    bz.g3 = g3;

                    // Usc系数不区分覆冰与不覆冰
                    double usc = ElecCalsToolBox.WindEpson(iceVal, Dia);
                    //横向比载
                    double g4 = 0.625 * Math.Pow(winVal, 2) * Dia * alpha * usc * 1e-3 / Sec;
                    bz.g4 = g4;

                    //在覆冰计算中，计入覆冰增大系数
                    double bex = ElecCalsToolBox.WindLoadEnlargeCoe(iceVal);
                    double g5 = 0.625 * Math.Pow(winVal, 2) * (Dia + 2 * iceVal) * alpha * usc * bex * 1e-3 / Sec;
                    bz.g5 = g5;

                    double g6 = Math.Sqrt(Math.Pow(Comg1,2) + Math.Pow(g4, 2));
                    bz.g6 = g6;

                    double g7 = Math.Sqrt(Math.Pow(g3, 2) + Math.Pow(g5, 2));
                    bz.g7 = g7;

                    //按照Excel的算法
                    bz.BiZai = ElecCalsToolBox2.BiZai(Wei * 1000, Dia, Sec, weaItem.IceThickness, weaItem.WindSpeed, weaItem.BaseWindSpeed) / 1000;
                    bz.HorBizai = ElecCalsToolBox2.BiZaiH(Dia, weaItem.IceThickness, weaItem.WindSpeed, weaItem.BaseWindSpeed, Sec) / 1000;
                    bz.VerHezai = ElecCalsToolBox2.Weight(Wei * 1000, Dia, weaItem.IceThickness) /1000;
                    bz.VerBizai = bz.VerHezai / Sec;
                    bz.WindHezai = ElecCalsToolBox2.WindPa(CommParas.VoltStr,Dia, weaItem.IceThickness, weaItem.WindSpeed, weaItem.BaseWindSpeed) /1000;

                    BzDic.Add(weaItem.Name, bz);
                }
            }
        }


        /// <summary>
        /// 计算给定档距下各个工况的应力
        /// </summary>
        /// <param name="span"></param>
        /// <returns></returns>
        protected Dictionary<string, double> CalFTable(double span)
        {
            if (bGrd == 0)
            {
                EffectPara = SideParas.IndEffectPara;
                SafePara = SideParas.IndSafePara;
                AvePara =SideParas.IndAnPara;
            }
            else if(bGrd == 1)
            {
                EffectPara = SideParas.GrdEffectPara;
                SafePara = SideParas.GrdSafePara;
                AvePara = SideParas.GrdAnPara;
            }
            else
            {
                OPGWEffectPara = SideParas.OPGWEffectPara;
                //OPGW安全系数
                OPGWSafePara = SideParas.OPGWSafePara;
                //OPGW年均系数 
                OPGWAnPara = SideParas.OPGWAnPara;

                EffectPara = SideParas.OPGWEffectPara;
                //OPGW安全系数
                SafePara = SideParas.OPGWSafePara;
                //OPGW年均系数 
                AvePara = SideParas.OPGWAnPara;
            }



            #region 按照Excel的算法
            var maxWindWkCdt = WeatherParas.WeathComm.Where(item => item.Name == "换算最大风速").First();
            var minTempWkCdt = WeatherParas.WeathComm.Where(item => item.Name == "最低气温").First();
            var maxIceWkCdt = WeatherParas.WeathComm.Where(item => item.Name == "最大覆冰").First();
            var aveTempWkCdt = WeatherParas.WeathComm.Where(item => item.Name == "平均气温").First();

            List<ElecCalsWorkCondition> wkCdtList = new List<ElecCalsWorkCondition>();
            wkCdtList.Add(maxWindWkCdt);
            wkCdtList.Add(minTempWkCdt);
            wkCdtList.Add(maxIceWkCdt);
            wkCdtList.Add(aveTempWkCdt);

            CtrlStress = Math.Round(Fore / Sec / 9.80665 * EffectPara / SafePara, 3);
            AvaStress = Math.Round(Fore / Sec / 9.80665 * EffectPara * AvePara / 100, 3);

            CtrlGkName = ElecCalsToolBox2.GetCtrlWorkConditionName(BzDic, wkCdtList,span, ElasM * 9.8065, CtrlStress, AvaStress, Coef);
            CtrlGkStress = CtrlGkName == "平均气温" ? AvaStress : CtrlStress;
            CtrlGk = WeatherParas.WeathComm.Where(item => item.Name == CtrlGkName).First();

            YLTableXls = new Dictionary<string, double>();
            foreach (var wd in WeatherParas.WeathComm)
            {
                double stress = ElecCalsToolBox2.StressNew(CtrlGkStress, BzDic[CtrlGkName].BiZai, CtrlGk.Temperature,ElasM, Coef, span, BzDic[wd.Name].BiZai, wd.Temperature);
                YLTableXls.Add(wd.Name, stress);
            }
            #endregion

            //最大允许应力   
            var maxPerFor = Fore * CommParas.NewPerPara / SafePara / Sec;
            //年均应力
            var avePerFor = Fore * CommParas.NewPerPara * AvePara / Sec;

            //控制工况计算

            //存储中间计算结果
            Dictionary<string, double> YLCalDic = new Dictionary<string, double>();
            Dictionary<string, double> ForCalDic = new Dictionary<string, double>();

            foreach (var nameWd in WeatherParas.NameOfCtrWkCdt)
            {
                double calFor = nameWd != "平均气温" ? maxPerFor : avePerFor;

                //均采用综合比载计算
                double calBz = BzDic[nameWd].g6;
                double temVal = WeatherParas.WeathComm.Where(item => item.Name == nameWd).First().Temperature;
                double fm = (Elas * Math.Pow(calBz, 2) * Math.Pow(span, 2) / 24 / Math.Pow(calFor, 2)) - (calFor + Coef * Elas * temVal);
                YLCalDic.Add(nameWd, fm);
                //存储每个工况初始控制应力取值
                ForCalDic.Add(nameWd, calFor);
            }

            //确定控制工况
            //存储各个工况应力,采用字典模式
            Dictionary<string, double> ForDic = new Dictionary<string, double>();

            //CtrName sorted(YLCalDic.items(), key = lambda kv: kv[1], reverse = True)[0]
            string CtrName = "";
            double CtrFm = 0;

            ForDic.Add(CtrName, CtrFm);

            //将控制工况存储下来
            CtrNaSave = CtrName;

            foreach(var wd in WeatherParas.WeathComm)
            {
                if (wd.Name == CtrName)
                    continue;

                double calBz = BzDic[wd.Name].g6;
                double temVal = WeatherParas.WeathComm.Where(item => item.Name == wd.Name).First().Temperature;
                double temA = CtrFm + Coef * Elas * temVal;
                double temB = Elas * Math.Pow(calBz, 2) * Math.Pow(span, 2) / 24;
                double calFor = ElecCalsToolBox.caculateCurDelta(temA, temB);
                ForDic.Add(wd.Name, calFor);
            }

            return ForDic;
        }

        public void UpdateWeaForCals(bool isBackSide, double angle)
        {
            double aveHei = bGrd == 0 ? CommParas.IndAveHei : CommParas.GrdAveHei;
            
            //两种不同计算方式，一个是从按照Excel转换，一个从python转换，结果相同，采用从python转换的结果
            //WeatherParas.ConverWind(aveHei, CommParas.TerrainPara);
            WeatherParas.ConverWind(aveHei, CommParas.TerType);
            
            WeatherParas.ConverWind45(isBackSide, angle);
            WeatherParas.AddBreakGK("断线", bGrd, CommParas.GrdIceUnbaPara, Dia, CommParas.BreakIceCoverPer);
            WeatherParas.AddUnevenIceGK(false, Dia, isBackSide, CommParas.UnbaIceCoverPerI, CommParas.UnbaIceCoverPerII);

            WeatherParas.AddOtherGk();
            WeatherParas.AddInstallColdGk(DecrTem);

            if (bGrd > 0)
            {
                //增加地线覆冰工况
                WeatherParas.AddGrdWeath();

                //下面的三种工况是各自工况的导线+5mm后的工况
                WeatherParas.AddBreakGK("断线(导线+5mm)", bGrd, CommParas.GrdIceUnbaPara, Dia, CommParas.BreakIceCoverPer);
                WeatherParas.AddUnevenIceGK(true, Dia, isBackSide, CommParas.UnbaIceCoverPerI, CommParas.UnbaIceCoverPerII);
                WeatherParas.AddCkeckGKIcr5mm();
            }

        }
    }
}
