using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerLoadCals.Common;

namespace TowerLoadCals.BLL.Electric
{
    /// <summary>
    /// 电力计算资资源
    /// </summary>
    public class ElecCalsRes
    {
        /// <summary>
        /// 气象条件
        /// </summary>
        public ElecCalsWeaRes Weather;

        /// <summary>
        /// 导线
        /// </summary>
        public ElecCalsWire IndWire { get; set; }

        /// <summary>
        /// 地线
        /// </summary>
        public ElecCalsWire GrdWire { get; set; }

        /// <summary>
        /// OPGW
        /// </summary>
        public ElecCalsWire OPGWWire { get; set; }

        /// <summary>
        /// 跳线导线
        /// </summary>
        public ElecCalsWire JumWire { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ElecCalsSideRes SideParas { get; set; }


        public ElecCalsCommRes CommParas { get; set; }


        public ElecCalsSpanFit SpanFit { get; set; }



        /// <summary>
        /// 冰区类型：轻冰区，中冰区，重冰区
        /// </summary>
        public string IceArea { get; set;}

        //true 小号侧，后侧; fasle 大号侧，前侧
        public bool IsBackSide { get; set; }

        /// <summary>
        /// 配置计算数据,并刷新导线相关参数等
        /// </summary>
        /// <param name="WeathSor"></param>
        /// <param name="IndWireSor"></param>
        /// <param name="GrdWireSor"></param>
        /// <param name="OPGWWrieSor"></param>
        /// <param name="JumWireSor"></param>
        /// <param name="SideParaSor"></param>
        /// <param name="ComParaSor"></param>
        public void UpdataSor(ElecCalsWeaRes WeathSor, ElecCalsWire IndWireSor, ElecCalsWire GrdWireSor, ElecCalsWire OPGWWrieSor, 
            ElecCalsWire JumWireSor, ElecCalsSideRes SideParaSor, ElecCalsCommRes ComParaSor)
        {
            Weather = XmlUtils.Clone(WeathSor);
            IndWire = XmlUtils.Clone(IndWireSor);
            GrdWire = XmlUtils.Clone(GrdWireSor);
            OPGWWire = XmlUtils.Clone(OPGWWrieSor);
            JumWire = XmlUtils.Clone(JumWireSor);
            SideParas = SideParaSor;
            CommParas = ComParaSor;
        }

        /// <summary>
        /// 刷新导地线计算
        /// </summary>
        /// <param name="spanVal"></param>
        public void FlashWireData(string towerType, double spanVal, double angle)
        {
            IndWire.UpdataPara(Weather, CommParas, SideParas, towerType, IceArea);
            IndWire.UpdateWeaForCals(IsBackSide, angle);
            IndWire.CalBZ();
            IndWire.SaveYLTabel(spanVal);

            GrdWire.UpdataPara(Weather, CommParas, SideParas, towerType, IceArea);
            GrdWire.UpdateWeaForCals(IsBackSide, angle);
            GrdWire.CalBZ();
            GrdWire.SaveYLTabel(spanVal);

            OPGWWire.UpdataPara(Weather, CommParas, SideParas, towerType, IceArea);
            OPGWWire.UpdateWeaForCals(IsBackSide, angle);
            OPGWWire.CalBZ();
            OPGWWire.SaveYLTabel(spanVal);
        }

        /// <summary>
        ///  刷新跳线计算
        /// </summary>
        public void FlashJumWireData(string towerType, double angle)
        {
            JumWire.UpdataPara(Weather, CommParas, SideParas, towerType, IceArea);
            JumWire.UpdateWeaForCals(IsBackSide, angle);
            
            //JumWire.CalBZ();
            //JumWire.SaveYLTabel(spanVal);
        }

        public List<string> PrintBzAndYL()
        {
            List<string> logStrs = new List<string>();

            logStrs.Add("导线：");
            logStrs.AddRange(PrintSpecLoadAndStress(IndWire));

            logStrs.Add("\n地线：");
            logStrs.AddRange(PrintSpecLoadAndStress(GrdWire));

            logStrs.Add("\nOPGW：");
            logStrs.AddRange(PrintSpecLoadAndStress(OPGWWire));

            return logStrs;
        }

        protected List<string> PrintSpecLoadAndStress(ElecCalsWire wire)
        {
            List<string> rslt = new List<string>();

            string str1 = FileUtils.PadRightEx("最大使用应力:" + wire.CtrlStress.ToString("0.###"), 24)+ FileUtils.PadRightEx("最大年均应力:" + wire.AvaStress.ToString("0.###"), 24) 
                + FileUtils.PadRightEx("控制工况温度:" +  wire.CtrlGk.Temperature.ToString("0.###"), 20);
            rslt.Add(str1);

            string str2 = FileUtils.PadRightEx("控制工况:" + wire.CtrlGkName, 24) + FileUtils.PadRightEx("控制工况比载:" + wire.BzDic[wire.CtrlGkName].BiZai.ToString("e3"),24) 
                + FileUtils.PadRightEx("控制工况应力:" +  wire.CtrlGkStress.ToString("0.###"), 20) ;
            rslt.Add(str2);

            string strTitle = FileUtils.PadRightEx("气象条件", 26) + FileUtils.PadRightEx("温度：", 8) + FileUtils.PadRightEx("风速：", 8) + FileUtils.PadRightEx("覆冰：", 8)
                + FileUtils.PadRightEx("基本风速：", 12) + FileUtils.PadRightEx("比载：", 12) + FileUtils.PadRightEx("比载g7：", 12)
                + FileUtils.PadRightEx("垂直比载：", 12) + FileUtils.PadRightEx("垂直比载g3：", 12) + FileUtils.PadRightEx("横向比载：", 12) + FileUtils.PadRightEx("横向比载g5：", 12)
                + FileUtils.PadRightEx("垂直荷载：", 12) + FileUtils.PadRightEx("风荷载：", 12) + FileUtils.PadRightEx("应力：", 12) + FileUtils.PadRightEx("应力g：", 12);
            rslt.Add(strTitle);

            foreach (var name in wire.WorkCdtNames)
            {
                if (wire.WeatherParas.WeathComm.Where(item => item.Name == name).Count() <= 0)
                    continue;

                var wea = wire.WeatherParas.WeathComm.Where(item => item.Name == name).First();

                string str = FileUtils.PadRightEx(name, 26) + FileUtils.PadRightEx(wea.Temperature.ToString(), 8) + FileUtils.PadRightEx(wea.WindSpeed.ToString(), 8) + FileUtils.PadRightEx(wea.IceThickness.ToString(), 8)
                    + FileUtils.PadRightEx(wea.BaseWindSpeed.ToString(), 12) + FileUtils.PadRightEx(wire.BzDic[name].BiZai.ToString("e3"), 12) + FileUtils.PadRightEx((wire.BzDic[name].g7 / 9.8).ToString("e3"), 12)
                    + FileUtils.PadRightEx(wire.BzDic[name].VerBizai.ToString("e3"), 12) + FileUtils.PadRightEx((wire.BzDic[name].g3 / 9.8).ToString("e3"), 12) + FileUtils.PadRightEx(wire.BzDic[name].HorBizai.ToString("e3"), 12) + FileUtils.PadRightEx((wire.BzDic[name].g5/9.8).ToString("e3"), 12)
                    + FileUtils.PadRightEx(wire.BzDic[name].VerHezai.ToString("0.000"), 12) + FileUtils.PadRightEx(wire.BzDic[name].WindHezai.ToString("0.000"), 12) + FileUtils.PadRightEx(wire.YLTable2[name].ToString("0.000"), 12) + FileUtils.PadRightEx(wire.YLTable[name].ToString("0.000"), 12);
                rslt.Add(str);
            }
            
            return rslt;
        }

        public List<string> PrintTension()
        {
            List<string> logStrs = new List<string>();

            string str = FileUtils.PadRightEx(" ", 6) + FileUtils.PadRightEx("断线张力系数 ", 16) + FileUtils.PadRightEx("不平衡冰张力系数 ", 16);
            logStrs.Add(str);

            string strInd = FileUtils.PadRightEx("导线 ", 6) + FileUtils.PadRightEx(IndWire.BreakTensionPara.ToString("0.00"), 16) + FileUtils.PadRightEx(IndWire.UnbaTensionPara.ToString("0.00"), 16);
            logStrs.Add(strInd);

            string strGrd = FileUtils.PadRightEx("地线 ", 6) + FileUtils.PadRightEx(GrdWire.BreakTensionPara.ToString("0.00"), 16) + FileUtils.PadRightEx(GrdWire.UnbaTensionPara.ToString("0.00"), 16);
            logStrs.Add(strGrd);

            string strOPGW = FileUtils.PadRightEx("OPGW ", 6) + FileUtils.PadRightEx(OPGWWire.BreakTensionPara.ToString("0.00"), 16) + FileUtils.PadRightEx(OPGWWire.UnbaTensionPara.ToString("0.00"), 16);
            logStrs.Add(strOPGW);

            return logStrs;
        }


        public double IndBreakTensionDiff { get; set; }
        public double GrdBreakTensionDiff { get; set; }
        public double OPGWBreakTensionDiff { get; set; }

        public double IndUnbaIceTensionDiff { get; set; }
        public double GrdUnbaIceTensionDiff { get; set; }
        public double OPGWUnbaIceTensionDiff { get; set; }

        public string IndBreakTensionDiffStr;
        public string GrdBreakTensionDiffStr;
        public string OPGWBreakTensionDiffStr;

        public string IndUnbaIceTensionDiffStr;
        public string GrdUnbaIceTensionDiffStr;
        public string OPGWUnbaIceTensionDiffStr;

        public void UpdateTensionDiff()
        {
            IndBreakTensionDiff = TensinDiff(out IndBreakTensionDiffStr, IndWire.Fore, CommParas.SecIndInc, SideParas.IndEffectPara, SideParas.IndSafePara, IndWire.BreakTensionPara, IndWire.DevideNum);
            GrdBreakTensionDiff = TensinDiff(out GrdBreakTensionDiffStr, GrdWire.Fore, CommParas.SecGrdInc, SideParas.GrdEffectPara, SideParas.GrdSafePara, GrdWire.BreakTensionPara);
            OPGWBreakTensionDiff = TensinDiff(out OPGWBreakTensionDiffStr, OPGWWire.Fore, CommParas.SecOPGWInc, SideParas.OPGWEffectPara, SideParas.OPGWSafePara, OPGWWire.BreakTensionPara);

            IndUnbaIceTensionDiff = TensinDiff(out IndUnbaIceTensionDiffStr, IndWire.Fore, CommParas.SecIndInc, SideParas.IndEffectPara, SideParas.IndSafePara, IndWire.UnbaTensionPara, IndWire.DevideNum);
            GrdUnbaIceTensionDiff = TensinDiff(out GrdUnbaIceTensionDiffStr, GrdWire.Fore, CommParas.SecGrdInc, SideParas.GrdEffectPara, SideParas.GrdSafePara, GrdWire.UnbaTensionPara);
            OPGWUnbaIceTensionDiff = TensinDiff(out OPGWUnbaIceTensionDiffStr, OPGWWire.Fore, CommParas.SecOPGWInc, SideParas.OPGWEffectPara, SideParas.OPGWSafePara, OPGWWire.UnbaTensionPara);

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
        protected double TensinDiff(out string str, double fore, double secInc, double effectPara, double savePara, double tensionCoef, int devideNum = 1)
        {
            double rslt =  secInc * Math.Round(fore * effectPara / 9.8665, 2) / savePara * devideNum * tensionCoef;
            str = secInc.ToString("0.##") + "*" + fore.ToString("0.##") + "*" + effectPara.ToString("0.##") + "/ 9.8665/" +  savePara.ToString("0.##") + "*" + devideNum.ToString("0.##") + "*" + tensionCoef.ToString("0.##") + "=" + rslt.ToString("0.###");
            return rslt;
        }

    }
}
