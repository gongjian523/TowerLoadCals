﻿using System;
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

        public ElecCalsTowerRes TowerParas { get; set; }


        public ElecCalsSpanFit SpanFit { get; set; }


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
            IndWire.UpdataPara(Weather, CommParas, SideParas, towerType);
            IndWire.UpdateWeaForCals(IsBackSide, angle, towerType, IndWire.Dia, GrdWire.Dia);
            IndWire.CalBZ();
            IndWire.SaveYLTabel(spanVal);

            GrdWire.UpdataPara(Weather, CommParas, SideParas, towerType);
            GrdWire.UpdateWeaForCals(IsBackSide, angle, towerType, IndWire.Dia, GrdWire.Dia);
            GrdWire.CalBZ();
            GrdWire.SaveYLTabel(spanVal);

            OPGWWire.UpdataPara(Weather, CommParas, SideParas, towerType);
            OPGWWire.UpdateWeaForCals(IsBackSide, angle, towerType, IndWire.Dia, GrdWire.Dia);
            OPGWWire.CalBZ();
            OPGWWire.SaveYLTabel(spanVal);
        }

        /// <summary>
        ///  刷新跳线计算
        /// </summary>
        public void FlashJumWireData(string towerType, double angle)
        {
            JumWire.UpdataPara(Weather, CommParas, SideParas, towerType);
            JumWire.UpdateWeaForCals(IsBackSide, angle, towerType, IndWire.Dia, GrdWire.Dia);
            
            //JumWire.CalBZ();
            //JumWire.SaveYLTabel(spanVal);
        }

        public List<string> PrintBzAndYL(string towerType)
        {
            List<string> logStrs = new List<string>();

            logStrs.Add("导线：");
            logStrs.AddRange(PrintSpecLoadAndStress(IndWire, towerType));

            logStrs.Add("\n地线：");
            logStrs.AddRange(PrintSpecLoadAndStress(GrdWire, towerType));

            logStrs.Add("\nOPGW：");
            logStrs.AddRange(PrintSpecLoadAndStress(OPGWWire, towerType));

            return logStrs;
        }

        protected List<string> PrintSpecLoadAndStress(ElecCalsWire wire, string towerType)
        {
            List<string> rslt = new List<string>();

            string str1 = FileUtils.PadRightEx("最大使用应力:" + wire.CtrlStress.ToString("0.###"), 24)+ FileUtils.PadRightEx("最大年均应力:" + wire.AvaStress.ToString("0.###"), 24) 
                + FileUtils.PadRightEx("控制工况温度:" +  wire.CtrlGk.Temperature.ToString("0.###"), 20);
            rslt.Add(str1);

            string str11 = FileUtils.PadRightEx("最大使用应力py: " + wire.MaxPerFor.ToString("0.###"), 24) + FileUtils.PadRightEx("最大年均应力py: " + wire.AvePerFor.ToString("0.###"), 24) 
                + FileUtils.PadRightEx("控制工况py: " + wire.CtrNaSave, 24);
            rslt.Add(str11);

            string str2 = FileUtils.PadRightEx("控制工况:" + wire.CtrlGkName, 24) + FileUtils.PadRightEx("控制工况比载:" + wire.BzDic[wire.CtrlGkName].BiZai.ToString("e3"),24) 
                + FileUtils.PadRightEx("控制工况应力:" +  wire.CtrlGkStress.ToString("0.###"), 20) ;
            rslt.Add(str2);

            string str22 = FileUtils.PadRightEx("控制工况py:" + wire.CtrNaSave, 24) + FileUtils.PadRightEx("控制工况应力py:" + wire.CtrYLSave.ToString("0.###"), 20);
            rslt.Add(str22);

            string strTitle = FileUtils.PadRightEx("气象条件", 26) + FileUtils.PadRightEx("温度：", 8) + FileUtils.PadRightEx("风速：", 8) + FileUtils.PadRightEx("覆冰：", 8)
                + FileUtils.PadRightEx("基本风速：", 12) + FileUtils.PadRightEx("比载：", 12) + FileUtils.PadRightEx("比载g7：", 12)
                + FileUtils.PadRightEx("垂直比载：", 12) + FileUtils.PadRightEx("垂直比载g3：", 12) + FileUtils.PadRightEx("横向比载：", 12) + FileUtils.PadRightEx("横向比载g5：", 12)
                + FileUtils.PadRightEx("垂直荷载：", 12) + FileUtils.PadRightEx("风荷载：", 12) + FileUtils.PadRightEx("应力：", 12) + FileUtils.PadRightEx("应力g：", 12);
            rslt.Add(strTitle);

            List<string> wkCdtList = towerType == "悬垂塔" ? wire.WorkCdtNamesHang : wire.WorkCdtNamesStrain;

            foreach (var name in wkCdtList)
            {
                if (wire.WeatherParas.WeathComm.Where(item => item.Name == name).Count() <= 0)
                    continue;

                var wea = wire.WeatherParas.WeathComm.Where(item => item.Name == name).First();

                string str = FileUtils.PadRightEx(name, 26) + FileUtils.PadRightEx(wea.Temperature.ToString(), 8) + FileUtils.PadRightEx(wea.WindSpeed.ToString(), 8) + FileUtils.PadRightEx(wea.IceThickness.ToString(), 8)
                    + FileUtils.PadRightEx(wea.BaseWindSpeed.ToString(), 12) + FileUtils.PadRightEx(wire.BzDic[name].BiZai.ToString("e3"), 12) + FileUtils.PadRightEx((wire.BzDic[name].g7 / CommParas.GraAcc).ToString("e3"), 12)
                    + FileUtils.PadRightEx(wire.BzDic[name].VerBizai.ToString("e3"), 12) + FileUtils.PadRightEx((wire.BzDic[name].g3 / CommParas.GraAcc).ToString("e3"), 12) + FileUtils.PadRightEx(wire.BzDic[name].HorBizai.ToString("e3"), 12) + FileUtils.PadRightEx((wire.BzDic[name].g5/CommParas.GraAcc).ToString("e3"), 12)
                    + FileUtils.PadRightEx(wire.BzDic[name].VerHezai.ToString("0.000"), 12) + FileUtils.PadRightEx(wire.BzDic[name].WindHezai.ToString("0.000"), 12) + FileUtils.PadRightEx(wire.YLTableXls[name].ToString("0.000"), 12) + FileUtils.PadRightEx((wire.YLTable[name]/CommParas.GraAcc).ToString("0.000"), 12);
                rslt.Add(str);
            }
            
            return rslt;
        }

        public List<string> PrintTension(string towerType)
        {
            List<string> logStrs = new List<string>();

            string str = FileUtils.PadRightEx(" ", 6) + FileUtils.PadRightEx("断线张力系数 ", 16);
            if(towerType == "悬垂塔"){
                str += FileUtils.PadRightEx("地线开断 ", 16);
            }
            str += FileUtils.PadRightEx("不平衡冰张力系数 ", 20);
            if (towerType == "悬垂塔"){
                str += FileUtils.PadRightEx("地线开断 ", 16);
            }
            logStrs.Add(str);

            string strInd = FileUtils.PadRightEx("导线 ", 6) + FileUtils.PadRightEx(IndWire.BreakTensionPara.ToString("0.00"), 16);
            if (towerType == "悬垂塔"){
                strInd += FileUtils.PadRightEx("  ", 16);
            }
            strInd += FileUtils.PadRightEx(IndWire.UnbaTensionPara.ToString("0.00"), 20);
            logStrs.Add(strInd);

            string strGrd = FileUtils.PadRightEx("地线 ", 6) + FileUtils.PadRightEx(GrdWire.BreakTensionPara.ToString("0.00"), 16);
            if (towerType == "悬垂塔")
            {
                strGrd += FileUtils.PadRightEx(GrdWire.BreakTensionGrdBrePara.ToString("0.00"), 16);
            }
            strGrd += FileUtils.PadRightEx(GrdWire.UnbaTensionPara.ToString("0.00"), 20);
            if (towerType == "悬垂塔")
            {
                strGrd += FileUtils.PadRightEx(GrdWire.UnbaTensionGrdBrePara.ToString("0.00"), 16);
            }

            logStrs.Add(strGrd);

            string strOPGW = FileUtils.PadRightEx("OPGW ", 6) + FileUtils.PadRightEx(OPGWWire.BreakTensionPara.ToString("0.00"), 16);
            if (towerType == "悬垂塔")
            {
                strOPGW += FileUtils.PadRightEx(OPGWWire.BreakTensionGrdBrePara.ToString("0.00"), 16);
            }
            strOPGW += FileUtils.PadRightEx(OPGWWire.UnbaTensionPara.ToString("0.00"), 20);
            if (towerType == "悬垂塔")
            {
                strOPGW += FileUtils.PadRightEx(OPGWWire.UnbaTensionGrdBrePara.ToString("0.00"), 16);
            }
            logStrs.Add(strOPGW);

            return logStrs;
        }

    }
}
