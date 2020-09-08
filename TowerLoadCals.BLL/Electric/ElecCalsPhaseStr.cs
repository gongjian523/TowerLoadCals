using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TowerLoadCals.Common;
using TowerLoadCals.Common.Utils;
using TowerLoadCals.Mode;
using TowerLoadCals.Mode.Electric;

namespace TowerLoadCals.BLL.Electric
{
    public class ElecCalsPhaseStr
    {
        public virtual List<string>  WireWorkConditionNames { get; set; }

        /// <summary>
        /// 相位置标记
        /// </summary>
        public int PhasePosType { get; set; }

        /// <summary>
        /// 前侧导线挂线串
        /// </summary>
        public ElecCalsStrData HangStr { get; set; }

        /// <summary>
        /// 跳线配置
        /// </summary>
        public ElecCalsStrData JumpStr { get; set; }

        /// <summary>
        /// 回路ID
        /// </summary>
        public int LoopID { get; set; }

        /// <summary>
        ///  相ID
        /// </summary>
        public int PhaseID { get; set; }

        /// <summary>
        /// 串长
        /// </summary>
        public double BaseStringEquLength { get; set; }

        /// <summary>
        /// 空间位置数据
        /// </summary>
        public ElecCalsPhaseSpaceStr SpaceStr { get; set; } = new ElecCalsPhaseSpaceStr();

        /// <summary>
        ///  线风压系数
        /// </summary>
        public double WireWindPara { get; set; }

        /// <summary>
        /// 绝缘子串风压系数
        /// </summary>
        public double StrWindPara { get; set; }

        /// <summary>
        ///  跳线风压系数
        /// </summary>
        public double JmWindPara { get; set; }

        /// <summary>
        /// 支撑管风压系数
        /// </summary>
        public double PropUpWindPara { get; set; }

        /// <summary>
        ///  线计算数据
        /// </summary>
        public ElecCalsWire WireData { get; set; }

        /// <summary>
        /// 跳线计算数据
        /// </summary>
        public ElecCalsWire JmWireData { get; set; }

        /// <summary>
        /// 事故断线工况张力差(中间计算过程临时变量)
        /// </summary>
        public double BreakTenMaxTemp { get; set; }
        /// <summary>
        /// 事故断线工况张力差(最终结果)
        /// </summary>
        public double BreakTenMax { get; set; }

        /// <summary>
        /// 不均匀冰工况张力差(中间计算过程临时变量)
        /// </summary>
        public double UnbaIceTenMaxTemp { get; set; }
        /// <summary>
        /// 不均匀冰工况张力差(最终结果)
        /// </summary>
        public double UnbaIceTenMax { get; set; }


        public List<LoadThrDe> LoadList { get; set; } = new List<LoadThrDe>();

        /// <summary>
        /// 绝缘子串的荷载
        /// </summary>
        [XmlIgnore]
        public Dictionary<string, StrLoadResult> StrLoad { get; set; } = new Dictionary<string, StrLoadResult>();

        /// <summary>
        /// 受风面积
        /// </summary>
        public double WindArea { get; set; }

        /// <summary>
        /// 跳线绝缘子串的风荷载
        /// </summary>
        [XmlIgnore]
        public Dictionary<string, JumpStrLoadResult> JumpStrLoad { get; set; } = new Dictionary<string, JumpStrLoadResult>();


        /// <summary>
        /// 断线工况张力差
        /// </summary>
        public double BreakTenDiff { get; set; }
        public string BreakTenDiffStr { get; set; }

        /// <summary>
        /// 不均匀冰工况张力差
        /// </summary>
        public double UnbaIceTenDiff { get; set; }
        public string UnbaIceTenDiffStr { get; set; }

        /// <summary>
        /// 另一侧断线张力差
        /// </summary>
        public double AnSideBreakTenDiff { get; set; }

        /// <summary>
        /// 两侧侧断线张力差
        /// </summary>
        public string BothSidesBreakTenDiff { get; set; }

        /// <summary>
        /// 另一侧断线张力差(验算)
        /// </summary>
        public double AnSideBreakTenDiffCheck { get; set; }

        /// <summary>
        /// 两侧侧断线张力差（验算 中间过程）
        /// </summary>
        public string BothSidesBreakTenDiffCheckTemp { get; set; }

        /// <summary>
        /// 两侧侧断线张力差（验算 最终结果）
        /// </summary>
        public string BothSidesBreakTenDiffCheck { get; set; }

        /// <summary>
        /// 不均匀冰I张力差 两侧都有
        /// </summary>
        public double UnbaIceTenIDiffBothSids { get; set; }

        /// <summary>
        /// 不均匀冰I张力差 
        /// </summary>
        public double UnbaIceTenIDiff { get; set; }

        /// <summary>
        /// 不均匀冰I张力差 两侧都有（验算）
        /// </summary>
        public double UnbaIceTenIDiffBothSidsCheck { get; set; }

        /// <summary>
        /// 不均匀冰I张力差 （验算， 中间过程）
        /// </summary>
        public double UnbaIceTenIDiffCheckTemp { get; set; }

        /// <summary>
        /// 不均匀冰I张力差 （验算， 最终结果）
        /// </summary>
        public double UnbaIceTenIDiffCheck { get; set; }

        /// <summary>
        /// 不均匀冰II张力差 两侧都有
        /// </summary>
        public double UnbaIceTenIIDiffBothSids { get; set; }

        /// <summary>
        /// 不均匀冰II张力差 
        /// </summary>
        public double UnbaIceTenIIDiff { get; set; }

        /// <summary>
        /// 不均匀冰II张力差 两侧都有（验算）
        /// </summary>
        public double UnbaIceTenIIDiffBothSidsCheck { get; set; }

        /// <summary>
        /// 不均匀冰II张力差 （验算， 中间过程）
        /// </summary>
        public double UnbaIceTenIIDiffCheckTemp { get; set; }

        /// <summary>
        /// 不均匀冰II张力差 （验算， 最终结果）
        /// </summary>
        public double UnbaIceTenIIDiffCheck { get; set; }


        public ElecCalsPhaseStr()
        {
        }


        public virtual void CalDFCure(double span, double repStrLen, double cl, char terrainType, double wireHei)
        {

        }

        /// <summary>
        /// 计算绝缘子串的风荷载和垂直荷载
        /// </summary>
        public void CalsStrLoad()
        {
            foreach (var weaItem in WireData.WeatherParas.WeathComm)
            {
                double wload = Math.Round(ElecCalsToolBox2.StringWind(HangStr.PieceNum, HangStr.LNum, HangStr.GoldPieceNum, weaItem.IceThickness, weaItem.WindSpeed, weaItem.BaseWindSpeed, out string str1), 3);
                double vload = HangStr.Weight + WeightIceIn(weaItem.IceThickness) * (HangStr.PieceNum * HangStr.LNum + HangStr.GoldPieceNum);

                StrLoad.Add(weaItem.Name, new StrLoadResult
                {
                    WindLoad = wload,
                    VerLoad = vload,
                });
            }

            WindArea = 0.04 * (HangStr.PieceNum * HangStr.LNum + HangStr.GoldPieceNum);
        }

        /// <summary>
        /// 计算跳线绝缘子串的风荷载
        /// </summary>
        /// <param name="volt">电压</param>
        /// <param name="anSideWkCdtList">另一侧的工况</param>
        public void CalsWindLoad(string volt, List<ElecCalsWorkCondition>  anSideWkCdtList)
        {
            foreach (var weaItem in WireData.WeatherParas.WeathComm)
            {
                var anWea = anSideWkCdtList.Where(wea => wea.Name == weaItem.Name).FirstOrDefault();

                JumpStrLoadResult rslt = new JumpStrLoadResult();

                rslt.Temperature = weaItem.Temperature;

                //冰厚，风速和基本风速需要比较大号侧和小号侧相应的工况，取其中的较大值
                rslt.IceThickness = (anWea != null && anWea.IceThickness > weaItem.IceThickness) ? anWea.IceThickness : weaItem.IceThickness;
                rslt.WindSpeed = (anWea != null && anWea.WindSpeed > weaItem.WindSpeed) ? anWea.WindSpeed : weaItem.WindSpeed;
                rslt.BaseWindSpeed = (anWea != null && anWea.BaseWindSpeed > weaItem.BaseWindSpeed) ? anWea.BaseWindSpeed : weaItem.BaseWindSpeed;

                rslt.JumpStrWindLoad = Math.Round(ElecCalsToolBox2.StringWind(JumpStr.PieceNum, JumpStr.LNum, JumpStr.GoldPieceNum, rslt.IceThickness, rslt.WindSpeed, rslt.BaseWindSpeed, out string str1), 3);
                rslt.JumpWindLoad = Math.Round(ElecCalsToolBox2.WindPaT(volt, JmWireData.Dia, rslt.IceThickness, rslt.WindSpeed, rslt.BaseWindSpeed, out string str2) /1000, 3) * JumpStr.SoftLineLen;
                rslt.SuTubleWindLoad = Math.Round(ElecCalsToolBox2.WindPaT(volt, JumpStr.SuTubleDia, rslt.IceThickness, rslt.WindSpeed, rslt.BaseWindSpeed, out string str3) /1000, 3) * JumpStr.SuTubleLen;

                JumpStrLoad.Add(weaItem.Name, rslt);
            }
        }

        /// <summary>
        /// 计算垂直档距
        /// </summary>
        /// 
        public void UpdateVertialSpan()
        {
            foreach (var nameWd in WireWorkConditionNames)
            {
                //这儿比载excel中用的是孤立档应力，用的是普通档应力
                double rslt = VerticalSpan(SpaceStr.Span, SpaceStr.SubHei, WireData.YLTableXls[nameWd], WireData.BzDic[nameWd].BiZai, out string str);

                int index = LoadList.FindIndex(item => item.GKName == nameWd);
                if(index < 0)
                {
                    LoadList.Add(new LoadThrDe()
                    {
                        GKName = nameWd,
                        VetiSpan = rslt,
                        VetiSpanStr = str,
                    });
                }
                else
                {
                    LoadList[index].VetiSpan = rslt;
                    LoadList[index].VetiSpanStr = str;
                }
            }
        }

        public double  UpdateVertialSpan(string nameGK, out string str)
        {
            //这儿比载excel中用的是孤立档应力，用的是普通档应力
            double rslt = VerticalSpan(SpaceStr.Span, SpaceStr.SubHei, WireData.YLTableXls[nameGK], WireData.BzDic[nameGK].BiZai, out str);
            return rslt;
        }

        public virtual void  UpdateHorFor(double diaInc)
        {

        }

        public virtual double UpdateHorFor(string nameGK, out string str, double diaInc)
        {
            str = "";
            return 0; 
        }

        public void UpdateVerWei(double weiInc, int numJGB, double weiJGB, int numFZ, double weiFZ)
        {
            foreach (var nameWd in WireWorkConditionNames)
            {
                var wea = WireData.WeatherParas.WeathComm.Where(item => item.Name == nameWd).FirstOrDefault();
                double iceThick = wea == null ? 0 : wea.IceThickness;

                var laod = LoadList.Where(item => item.GKName == nameWd).FirstOrDefault();
                double verSpan = laod == null ? 0 : laod.VetiSpan;

                double rslt = VerWei(weiInc, WireData.DevideNum, WireData.BzDic[nameWd].VerHezai, verSpan, StrLoad[nameWd].VerLoad, numJGB, weiJGB, weiFZ, iceThick,
                    numFZ,HangStr.DampLength, WireData.bGrd,  out string str);

                int index = LoadList.FindIndex(item => item.GKName == nameWd);
                if (index < 0)
                {
                    LoadList.Add(new LoadThrDe()
                    {
                        GKName = nameWd,
                        VerWei = rslt,
                        VerWeiStr = str,
                    });
                }
                else
                {
                    LoadList[index].VerWei = rslt;
                    LoadList[index].VerWeiStr = str;
                }
            }
        }

        public double UpdateVerWei(string nameGK, out string str, double weiInc, double verSpan, int numJGB, double weiJGB, int numFZ, double weiFZ)
        {
            var wea = WireData.WeatherParas.WeathComm.Where(item => item.Name == nameGK).FirstOrDefault();
            double iceThick = wea == null ? 0 : wea.IceThickness;

            double rslt = VerWei(weiInc, WireData.DevideNum, WireData.BzDic[nameGK].VerHezai, verSpan, StrLoad[nameGK].VerLoad, numJGB, weiJGB, weiFZ, iceThick,
                numFZ, HangStr.DampLength, WireData.bGrd, out  str);

            return rslt;
        }

        public virtual void UpdateLoStr(double secInc,  double constrError, double isntallError, double extendPara)
        {
            foreach (var nameWd in WireWorkConditionNames)
            {
                double rslt;
                string str;

                if (nameWd != "安装情况")
                {
                    rslt = LoStr(out str, secInc, WireData.DevideNum, WireData.YLTableXls[nameWd], WireData.Sec, constrError);
                }
                else
                {
                    rslt = LoStr(out str, secInc, WireData.DevideNum, WireData.YLTableXls[nameWd], WireData.Sec, isntallError, extendPara);
                }

                int index = LoadList.FindIndex(item => item.GKName == nameWd);
                if (index < 0)
                {
                    LoadList.Add(new LoadThrDe()
                    {
                        GKName = nameWd,
                        LoStr = rslt,
                        LoStrStr = str,
                    });
                }
                else
                {
                    LoadList[index].LoStr = rslt;
                    LoadList[index].LoStrStr = str;
                }
            }
        }

        public virtual double UpdateLoStr(string nameGK, out string str, double secInc, double constrError, double isntallError, double extendPara)
        {
            if (nameGK != "安装情况")
            {
                return LoStr(out str, secInc, WireData.DevideNum, WireData.YLTableXls[nameGK], WireData.Sec, constrError);
            }
            else
            {
                return LoStr(out str, secInc, WireData.DevideNum, WireData.YLTableXls[nameGK], WireData.Sec, isntallError, extendPara);
            }
        }

        public virtual void CheckLoStr(List<LoadThrDe> anLoads, ElecCalsCommRes commPara)
        {
            
        }

        public void UpdateJumpHorFor()
        {
            foreach (var nameWd in WireWorkConditionNames)
            {
                double rslt = JumpHorFor(JumpStrLoad[nameWd].JumpStrWindLoad, JumpStrLoad[nameWd].JumpWindLoad, JumpStrLoad[nameWd].SuTubleWindLoad, JmWireData.DevideNum, out string str);

                int index = LoadList.FindIndex(item => item.GKName == nameWd);
                if (index < 0)
                {
                    LoadList.Add(new LoadThrDe()
                    {
                        GKName = nameWd,
                        JumpHorFor = rslt,
                        JumpHorForStr = str,
                    });
                }
                else
                {
                    LoadList[index].JumpHorFor = rslt;
                    LoadList[index].JumpHorForStr = str;
                }
            }
        }

        public double UpdateJumpHorFor(string nameGK, out string str)
        {
            double rslt = JumpHorFor(JumpStrLoad[nameGK].JumpStrWindLoad, JumpStrLoad[nameGK].JumpWindLoad, JumpStrLoad[nameGK].SuTubleWindLoad, JmWireData.DevideNum, out str);
            return rslt;
        }

        public void UpdateJumpVerWei(double jgbWei, List<ElecCalsWorkCondition> anWeathComm)
        {
            foreach (var nameWd in WireWorkConditionNames)
            {
                var wea = JmWireData.WeatherParas.WeathComm.Where(item => item.Name == nameWd).FirstOrDefault();
                var anWea = anWeathComm.Where(item => item.Name == nameWd).FirstOrDefault();

                var iceThick = Math.Max(wea == null ? 0 : wea.IceThickness, anWea == null ? 0 : anWea.IceThickness);

                double rslt = JumpVerWei(JumpStr.Weight, JumpStr.PieceNum, JumpStr.LNum, JumpStr.GoldPieceNum, JumpStr.SuTubleLen, JumpStr.SoftLineLen, JumpStr.JGBNum, iceThick,
                    JmWireData.Wei, JmWireData.Dia, jgbWei, JmWireData.DevideNum, JumpStr.SuTubleWei, JumpStr.SuTubleDia, out string str);

                int index = LoadList.FindIndex(item => item.GKName == nameWd);
                if (index < 0)
                {
                    LoadList.Add(new LoadThrDe()
                    {
                        GKName = nameWd,
                        JumpVerWei = rslt,
                        JumpVerWeiStr = str,
                    });
                }
                else
                {
                    LoadList[index].JumpVerWei = rslt;
                    LoadList[index].JumpVerWeiStr = str;
                }
            }
        }

        public double UpdateJumpVerWei(string nameGK, out string str, double jgbWei, List<ElecCalsWorkCondition> anWeathComm)
        {
            var wea = JmWireData.WeatherParas.WeathComm.Where(item => item.Name == nameGK).FirstOrDefault();
            var anWea = anWeathComm.Where(item => item.Name == nameGK).FirstOrDefault();

            var iceThick = Math.Max(wea == null ? 0 : wea.IceThickness, anWea == null ? 0 : anWea.IceThickness);

            double rslt = JumpVerWei(JumpStr.Weight, JumpStr.PieceNum, JumpStr.LNum, JumpStr.GoldPieceNum, JumpStr.SuTubleLen, JumpStr.SoftLineLen, JumpStr.JGBNum, iceThick,
                JmWireData.Wei, JmWireData.Dia, jgbWei, JmWireData.DevideNum, JumpStr.SuTubleWei, JumpStr.SuTubleDia, out str);

            return rslt;
        }

        public virtual void UpdateTensionDiff(double secInc, double effectPara, double safePara)
        {
            //BreakTenDiff = TensionDiff(out string breakTensionDiffStr, WireData.Fore, secInc, effectPara, safePara, WireData.BreakTensionPara, WireData.DevideNum);
            //BreakTenDiffStr = breakTensionDiffStr;

            //UnbaIceTenDiff = TensionDiff(out string unbaIceTensionDiffStr, WireData.Fore, secInc, effectPara, safePara, WireData.UnbaTensionPara, WireData.DevideNum);
            //UnbaIceTenDiffStr = unbaIceTensionDiffStr;

            //BreakTenMaxTemp = BreakTensionMax(secInc, effectPara, safePara);
            //BreakTenMax = Math.Max(BreakTenMaxTemp, BreakTenDiff);

            //UnbaIceTenMaxTemp = UnbaIceTensionMax(secInc, effectPara, safePara);
            //UnbaIceTenMax = Math.Max(UnbaIceTenMaxTemp, UnbaIceTenDiff);
        }

        #region 内部计算函数

        protected double WeightIceIn(double iceThick)
        {
            return (iceThick / 5);
        }

        /// <summary>
        /// 垂直档距
        /// </summary>
        /// <param name="span">档距</param>
        /// <param name="wireHei">高差</param>
        /// <param name="stress">孤立档应力</param>
        /// <param name="specLoad">比载</param>
        /// <returns></returns>
        protected double VerticalSpan(double span, double wireHei, double stress, double specLoad, out string str)
        {
            double rslt = Math.Round(span / 2 + stress / specLoad * Calc.Asinh(specLoad * wireHei / (2 * stress * Math.Sinh(specLoad * span / 2 / stress))), 2);
            str = span.ToString() + "/2+" + stress.ToString("0.000") + "/" + specLoad.ToString("0.000") + "*ASINH(" + specLoad.ToString("0.000") + "*" + wireHei.ToString("0.0")
                + "/(2*" + stress.ToString("0.000") + "*SINH(" + specLoad.ToString("0.000") + "*" + span + "/2/" + stress.ToString("0.000") + "))=" + rslt.ToString("0.##");
            return rslt;
        }

        /// <summary>
        /// 水平荷载
        /// </summary>
        /// <param name="diaInc">直径增大系数</param>
        /// <param name="devideNum"> 分裂系数</param>
        /// <param name="span">档距</param>
        /// <param name="dampLength">阻尼线长</param>
        /// <param name="windLoad">风荷载, 来源用wire,Bizai中的风荷载</param>
        /// <param name="strWindLaod">绝缘子串的风荷载</param>
        /// <returns></returns>
        protected double HorFor(double diaInc, double devideNum, double span, double dampLength, double windLoad, double strWindLaod, out string str)
        {
            //为了将悬垂塔和耐张塔在统一，将span/2提出去
            //double rslt = Math.Round(diaInc * WireWindPara * devideNum * (span / 2 + dampLength) * windLoad + StrWindPara * strWindLaod, 2);
            //str = diaInc.ToString() + "*" + WireWindPara.ToString("0.###") + "*" + devideNum.ToString() + "*(" + (span / 2).ToString("0.###") + "+"
            //    + dampLength.ToString("0.000") + ")*" + windLoad.ToString("0.000") + "+" + StrWindPara.ToString("0.000") + "*" + strWindLaod.ToString("0.000") + "= " + rslt.ToString("0.000");

            double rslt = Math.Round(diaInc * WireWindPara * devideNum * (span + dampLength) * windLoad + StrWindPara * strWindLaod, 2);
            str = diaInc.ToString() + "*" + WireWindPara.ToString("0.###") + "*" + devideNum.ToString() + "*(" + (span).ToString("0.###") + "+"
                + dampLength.ToString("0.000") + ")*" + windLoad.ToString("0.000") + "+" + StrWindPara.ToString("0.000") + "*" + strWindLaod.ToString("0.000") + "= " + rslt.ToString("0.000");
            return rslt;
        }

        /// <summary>
        /// 垂直荷载
        /// </summary>
        /// <param name="weightInc">重量增大 来自公共参数</param>
        /// <param name="devideNum">分裂系数</param>
        /// <param name="verLoad">垂直荷载 来源用wire,Bizai中的垂直荷载</param>
        /// <param name="verSpan">垂直档距，就是前面算的</param>
        /// <param name="strVerLoad">绝缘子串的垂直荷载</param>
        /// <param name="numJGB">每相导线间隔棒数</param>
        /// <param name="weiJGB">导线间隔棒重</param>
        /// <param name="weiFZ">导线防振锤重</param>
        /// <param name="iceThick">覆冰厚度 来源用wire.wea</param>
        /// <param name="numFZum">每相导线防震锤</param>
        /// <param name="dampLength">阻尼线长</param>
        /// <returns></returns>
        protected double VerWei(double weightInc, int devideNum, double verLoad, double verSpan, double strVerLoad, int numJGB,
            double weiJGB, double weiFZ, double iceThick, int numFZum, double dampLength, int bGrd, out string str)
        {
            if (bGrd == 0)
            {
                double rslt = weightInc * devideNum * verLoad * verSpan + strVerLoad + numJGB * (weiJGB + iceThick / 5)
                    + numFZum * (weiFZ + iceThick / 5) + verLoad * dampLength;
                str = weightInc.ToString("0.###") + "*" + devideNum.ToString("0.###") + "*" + verLoad.ToString("0.###") + "*" + verSpan.ToString("0.###") + "+" + strVerLoad.ToString("0.###") + "+" + numJGB.ToString("0.###")
                    + "*(" + weiJGB.ToString("0.###") + "+" + (iceThick / 5).ToString("0.###") + ")+" + numFZum.ToString("0.###") + "*(" + weiFZ.ToString("0.###") + "+" + (iceThick / 5).ToString("0.###") + ")+"
                    + verLoad.ToString("0.###") + "*" + dampLength.ToString("0.###") + "=" + rslt.ToString("0.###");
                return rslt;
            }
            else
            {
                double rslt = weightInc * devideNum * verLoad * verSpan + strVerLoad + numFZum * (weiFZ + iceThick / 5) + verLoad * dampLength;
                str = weightInc.ToString("0.###") + "*" + devideNum.ToString("0.###") + "*" + verLoad.ToString("0.###") + "*" + verSpan.ToString("0.###") + "+" + strVerLoad.ToString("0.###") + "+" + numFZum.ToString("0.###")
                    + "*(" + weiFZ.ToString("0.###") + "+" + (iceThick / 5).ToString("0.###") + ")+" + verLoad.ToString("0.###") + "*" + dampLength.ToString("0.###") + "=" + rslt.ToString("0.###");
                return rslt;
            }

        }

        /// <summary>
        /// 跳线串水平荷载
        /// </summary>
        /// <param name="jumpStrWindLoad">跳线绝缘子串风荷载</param>
        /// <param name="jumpWinLoad">跳线风荷载</param>
        /// <param name="jumpDeviedeNum">跳线分裂系数</param>
        /// <param name="PropUpWindLoad">支撑管风荷载</param>
        /// <returns></returns>
        protected double JumpHorFor(double jumpStrWindLoad, double jumpWinLoad, double PropUpWindLoad, int jumpDeviedeNum, out string str)
        {
            double rslt = jumpStrWindLoad * JmWindPara + jumpWinLoad * JmWindPara * jumpDeviedeNum + PropUpWindLoad * PropUpWindPara;
            str = jumpStrWindLoad.ToString("0.###") + "*" + JmWindPara.ToString("0.###") + "+" + jumpWinLoad.ToString("0.###") + "*" + JmWindPara.ToString("0.###") + "*" + jumpDeviedeNum.ToString("0.###") + "+" + PropUpWindLoad.ToString("0.###") + "*" + PropUpWindPara.ToString("0.###") + "=" + rslt.ToString("0.00");
            return rslt;
        }

        /// <summary>
        /// 跳线串垂直荷载
        /// </summary>
        /// <param name="jumpStrWei">跳线串重 B44 </param>
        /// <param name="pieceNum">绝缘子片数 D44 </param>
        /// <param name="lNum">联数 C44 </param>
        /// <param name="goldPieceNum">导地线金具折片数 E44 </param>
        /// <param name="suTubleLen">支撑管长度 F44 </param>
        /// <param name="jgbNum">间隔棒数 H44</param>
        /// <param name="softJumpLen">单相单根软跳线长 G44</param>
        /// <param name="jgbWei">导线间隔棒重 A48  </param>
        /// <param name="jumpDivideNum">跳线分裂数 E48 </param>
        /// <param name="suTubleWei">支撑管单重 H48</param>
        /// <param name="suTubleDia">支撑管直径 G48</param>
        /// <param name="unitWei">跳线的单位重量 F139 </param>
        /// <param name="dia">跳线的直径 C139</param>
        /// <param name="iceThick">此工况对应的冰厚 D141 </param>
        /// <returns></returns>
        public double JumpVerWei(double jumpStrWei, double pieceNum, double lNum, double goldPieceNum, double suTubleLen, double softJumpLen, double jgbNum, double iceThick,
            double unitWei, double dia, double jgbWei, int jumpDivideNum, double suTubleWei, double suTubleDia, out string str)
        {
            double rslt = jumpStrWei + (pieceNum * lNum + goldPieceNum) * WeightIceIn(iceThick) + (unitWei + 2.8274334 * iceThick * (dia + iceThick) / 1000) * softJumpLen * jumpDivideNum
                + (suTubleWei + 2.82743334 * iceThick * (suTubleDia + iceThick) / 1000) * suTubleLen + jgbNum * (jgbWei + iceThick / 5);
            str = jumpStrWei.ToString("0.###") + "+(" + pieceNum.ToString("0.###") + "*" + lNum.ToString("0.###") + "+" + goldPieceNum.ToString("0.###") + ")*" + (WeightIceIn(iceThick)).ToString("0.###") + "+(" + unitWei.ToString("0.###")
                 + "+2.8274334*" + iceThick.ToString("0.###") + "*(" + dia.ToString("0.###") + "+" + iceThick.ToString("0.###") + ")/1000)*" + softJumpLen.ToString("0.###") + "*" + jumpDivideNum.ToString("0.###") + "+(" + suTubleWei.ToString("0.###")
                 + "+2.8274334*" + iceThick.ToString("0.###") + "*(" + suTubleDia.ToString("0.###") + "+" + iceThick.ToString("0.###") + ")/1000)*" + suTubleLen.ToString("0.###") + "+" + jgbNum.ToString("0.###") + "*(" + jgbWei.ToString("0.###")
                 + "+" + iceThick.ToString("0.###") + "/5)=" + rslt.ToString("0.##");
            return rslt;
        }

        /// <summary>
        /// 纵向荷载（线张力）
        /// </summary>
        /// <param name="secInc">截面增大系数</param>
        /// <param name="devideNum">导线分裂数，地线不用</param>
        /// <param name="stress">孤立档应力，Bizai.Stress</param>
        /// <param name="sec">截面积</param>
        /// <param name="errorCoef">施工误差系数，全部用的大张力侧？ </param>
        /// <returns></returns>
        protected double LoStr(out string str, double secInc, int devideNum, double stress, double sec, double errorCoef, double extendPara = double.MinValue)
        {
            double rslt;
            if (extendPara == double.MinValue)
            {
                rslt = secInc * devideNum * stress * sec * errorCoef;
                str = secInc.ToString("0.###") + "*" + devideNum.ToString("0.###") + "*" + stress.ToString("0.###") + "*" + sec.ToString("0.###") + "*" + errorCoef.ToString("0.###") + "=" + rslt.ToString("0.###");
            }
            else
            {
                rslt = secInc * devideNum * stress * sec * errorCoef * extendPara;
                str = secInc.ToString("0.###") + "*" + devideNum.ToString("0.###") + "*" + stress.ToString("0.###") + "*" + sec.ToString("0.###") + "*" + errorCoef.ToString("0.###") + "*" + extendPara.ToString("0.###") + "=" + rslt.ToString("0.###");
            }
            return rslt;
        }

        /// <summary>
        /// 张力差
        /// </summary>
        /// <param name="secInc">截面增大系数</param>
        /// <param name="effectPara"> 有效系数</param>
        /// <param name="savePara">安全系数</param>
        /// <param name="tensionCoef">断线张力系数/不均匀冰张力系数？ 为什么全部用的小号侧</param>
        /// <param name="devideNum">导线分裂数，地线不用</param>
        /// <returns></returns>
        protected double TensionDiff(out string str, double fore, double secInc, double effectPara, double savePara, double tensionCoef, int devideNum = 1)
        {
            double rslt = secInc * Math.Round(fore * effectPara / 9.80665, 2) / savePara * devideNum * tensionCoef;
            str = secInc.ToString("0.##") + "*" + fore.ToString("0.##") + "*" + effectPara.ToString("0.##") + "/9.80665/" + savePara.ToString("0.##") + "*" + devideNum.ToString("0.##") + "*" + tensionCoef.ToString("0.##") + "=" + rslt.ToString("0.###");
            return rslt;
        }

        protected double BreakTensionMax(double secInc, double effectPara, double safePara)
        {
            if (WireData.CommParas.BreakMaxPara == 1)
            {
                return Math.Round(secInc * WireData.Fore * effectPara / 9.80665 / safePara * WireData.DevideNum, 2);
            }
            else
            {
                var load = LoadList.Where(item => item.GKName == "覆冰无风").FirstOrDefault();
                double loStr = load == null ? 0 : load.LoStr;

                var loadAdd5 = LoadList.Where(item => item.GKName == "覆冰无风+5").FirstOrDefault();
                double loStrAdd5 = loadAdd5 == null ? 0 : loadAdd5.LoStr;

                return WireData.bGrd == 0 ? loStr : (WireData.CommParas.GrdIceUnbaPara == 1 ? loStr : loStrAdd5);
            }
        }

        protected double UnbaIceTensionMax(double secInc, double effectPara, double safePara)
        {
            if (WireData.CommParas.UnbaMaxPara == 1)
            {
                return Math.Round(secInc * WireData.Fore * effectPara / 9.80665 / safePara * WireData.DevideNum, 2);
            }
            else
            {
                var load = LoadList.Where(item => item.GKName == "最大覆冰").FirstOrDefault();
                double loStr = load == null ? 0 : load.LoStr;

                var loadGrdIce = LoadList.Where(item => item.GKName == "地线覆冰").FirstOrDefault();
                double loStrGrdIce = loadGrdIce == null ? 0 : loadGrdIce.LoStr;

                return WireData.bGrd == 0 ? loStr : (WireData.CommParas.GrdIceUnbaPara == 1 ? loStr : loStrGrdIce);
            }
        }

        #endregion

        #region 打印函数
        public List<string> PrintStrLoad(string towerType)
        {
            List<string> rslt = new List<string>();

            string str = FileUtils.PadRightEx("受风面积:" + WindArea.ToString("0.##"), 16);
            rslt.Add(str);

            string strTitle = FileUtils.PadRightEx("气象条件", 26) + FileUtils.PadRightEx("温度：", 8) + FileUtils.PadRightEx("风速：", 8) + FileUtils.PadRightEx("覆冰：", 8)
                + FileUtils.PadRightEx("基本风速：", 12) + FileUtils.PadRightEx("风荷载：", 12) + FileUtils.PadRightEx("垂直荷载：", 12);
            rslt.Add(strTitle);

            List<string> wkCdtList = towerType == "悬垂塔" ? WireData.WorkCdtNamesHang : WireData.WorkCdtNamesStrain;

            foreach (var name in wkCdtList)
            {
                var wea = WireData.WeatherParas.WeathComm.Where(item => item.Name == name).FirstOrDefault();

                if (wea == null)
                    continue;

                string strValue = FileUtils.PadRightEx(name, 26) + FileUtils.PadRightEx(wea.Temperature.ToString(), 8) + FileUtils.PadRightEx(wea.WindSpeed.ToString(), 8) + FileUtils.PadRightEx(wea.IceThickness.ToString(), 8)
                    + FileUtils.PadRightEx(wea.BaseWindSpeed.ToString(), 12) + FileUtils.PadRightEx(StrLoad[name].WindLoad.ToString("0.###"), 12) + FileUtils.PadRightEx(StrLoad[name].VerLoad.ToString("0.###"), 12);
                rslt.Add(strValue);
            }

            return rslt;
        }
        #endregion

    }
}
