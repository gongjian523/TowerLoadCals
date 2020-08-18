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
        public SideCalUtils SideParas { get; set; }


        public ElecCalsCommRes CommParas { get; set; }

        /// <summary>
        /// 冰区类型：轻冰区，中冰区，重冰区
        /// </summary>
        public string IceArea { get; set;}

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
            ElecCalsWire JumWireSor, SideCalUtils SideParaSor, ElecCalsCommRes ComParaSor)
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
            IndWire.UpdateWeaForCals(angle);
            IndWire.CalBZ();
            IndWire.SaveYLTabel(spanVal);

            GrdWire.UpdataPara(Weather, CommParas, SideParas, towerType, IceArea);
            GrdWire.UpdateWeaForCals(angle);
            GrdWire.CalBZ();
            GrdWire.SaveYLTabel(spanVal);

            OPGWWire.UpdataPara(Weather, CommParas, SideParas, towerType, IceArea);
            OPGWWire.UpdateWeaForCals(angle);
            OPGWWire.CalBZ();
            OPGWWire.SaveYLTabel(spanVal);
        }

        /// <summary>
        ///  刷新跳线计算
        /// </summary>
        /// <param name="spanVal"></param>
        public void FlashJumWireData(string towerType, double spanVal, double angle)
        {
            JumWire.UpdataPara(Weather, CommParas, SideParas, towerType, IceArea);
            JumWire.UpdateWeaForCals(angle);
            JumWire.CalBZ();
            JumWire.SaveYLTabel(spanVal);
        }

        public List<string> PrintBzAndYL()
        {
            List<string> logStrs = new List<string>();

            logStrs.Add("导线：");
            logStrs.AddRange(PrintBzAndYL(IndWire, Weather.NameOfWkCalsInd));

            logStrs.Add("\n地线：");
            logStrs.AddRange(PrintBzAndYL(GrdWire, Weather.NameOfWkCalsGrd));

            logStrs.Add("\nOPGW：");
            logStrs.AddRange(PrintBzAndYL(OPGWWire, Weather.NameOfWkCalsGrd));

            return logStrs;
        }

        protected List<string> PrintBzAndYL(ElecCalsWire wire, List<string> gkList)
        {
            List<string> rslt = new List<string>();
            
            foreach(var gk in gkList)
            {
                if (wire.WeatherParas.WeathComm.Where(item => item.Name == gk).Count() <= 0)
                    continue;

                var wea = wire.WeatherParas.WeathComm.Where(item => item.Name == gk).First();

                string str = FileUtils.PadRightEx(gk, 26) + "温度：" + wea.Temperature.ToString().PadRight(4) + "风速：" + wea.WindSpeed.ToString().PadRight(8)
                     + "覆冰：" + wea.IceThickness.ToString().PadRight(4) + "基本风速：" + wea.BaseWindSpeed.ToString().PadRight(8)
                     + "比载：" + wire.BzDic[gk].BiZai.ToString("e3").PadRight(12) + "比载g7：" + wire.BzDic[gk].g7.ToString("e3").PadRight(12)
                     + "垂直比载：" + wire.BzDic[gk].VerBizai.ToString("e3").PadRight(12) + "垂直比载g3：" + wire.BzDic[gk].VerBizai.ToString("e3").PadRight(12)
                     + "横向比载：" + wire.BzDic[gk].HorBizai.ToString("e3").PadRight(12) + "横向比载g5：" + wire.BzDic[gk].VerBizai.ToString("e3").PadRight(12)
                     + "垂直荷载：" + wire.BzDic[gk].VerHezai.ToString("e3").PadRight(12) + "风荷载：" + wire.BzDic[gk].WindHezai.ToString("e3").PadRight(12)
                     + "应力：" + wire.YLTable[gk].ToString("e3").PadRight(12) + "应力g：" + wire.YLTable2[gk].ToString("e3").PadRight(12);
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

    }
}
