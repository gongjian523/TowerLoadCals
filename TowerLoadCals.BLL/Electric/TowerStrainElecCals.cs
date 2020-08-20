using System.Collections.Generic;
using System.Linq;
using TowerLoadCals.Common;
using TowerLoadCals.Mode.Electric;

namespace TowerLoadCals.BLL.Electric
{
    public class TowerStrainElecCals:TowerElecCals
    {
        public override string TowerType { get { return "耐张塔"; } }

        public TowerStrainElecCals()
        {
            
        }

        /// <summary>
        /// 拷贝前后计算资源进入计算模型，并更新导线、地线数据，前体是铁塔本体数据已更新
        /// </summary>
        /// <param name="BackSideResSor"></param>
        /// <param name="FrontSideResSor"></param>
        public void GetAndUpdateSideRes(ElecCalsRes BackSideResSor, ElecCalsRes FrontSideResSor)
        {
            BackSideRes = XmlUtils.Clone(BackSideResSor);
            FrontSideRes = XmlUtils.Clone(FrontSideResSor);

            BackSideRes.FlashWireData(TowerType, BackPosRes.DRepresentSpan, AngelofApplication);
            FrontSideRes.FlashWireData(TowerType, FrontPosRes.DRepresentSpan, AngelofApplication);

            BackSideRes.FlashJumWireData(TowerType, AngelofApplication);
            FrontSideRes.FlashJumWireData(TowerType, AngelofApplication);

            PhaseTraList[0].WireData = PhaseTraList[1].WireData = PhaseTraList[2].WireData = BackSideRes.IndWire;
            PhaseTraList[5].WireData = PhaseTraList[6].WireData = PhaseTraList[7].WireData = FrontSideRes.IndWire;
            PhaseTraList[3].WireData = BackSideRes.GrdWire;
            PhaseTraList[4].WireData = BackSideRes.OPGWWire;
            PhaseTraList[8].WireData = FrontSideRes.GrdWire;
            PhaseTraList[9].WireData = FrontSideRes.OPGWWire;
            PhaseTraList[0].JmWireData = PhaseTraList[1].JmWireData = PhaseTraList[2].JmWireData = BackSideRes.JumWire;
            PhaseTraList[5].JmWireData = PhaseTraList[6].JmWireData = PhaseTraList[7].JmWireData = FrontSideRes.JumWire;
        }

        /// <summary>
        /// 更新绝缘子串参数
        /// </summary>
        /// <param name="indStr"></param>
        /// <param name="grdStr"></param>
        /// <param name="jumpStr"></param>
        public void GetAndUpdateStrData(ElecCalsStrData indStr, ElecCalsStrData grdStr, ElecCalsStrData jumpStr)
        {
            PhaseTraList[0].HangStr = PhaseTraList[1].HangStr = PhaseTraList[2].HangStr = indStr;
            PhaseTraList[5].HangStr = PhaseTraList[6].HangStr = PhaseTraList[7].HangStr = indStr;
            PhaseTraList[3].HangStr = grdStr;
            PhaseTraList[4].HangStr = grdStr;
            PhaseTraList[8].HangStr = grdStr;
            PhaseTraList[9].HangStr = grdStr;
            PhaseTraList[0].JumpStr = PhaseTraList[1].JumpStr = PhaseTraList[2].JumpStr = jumpStr;
            PhaseTraList[5].JumpStr = PhaseTraList[6].JumpStr = PhaseTraList[7].JumpStr = jumpStr;
        }

        /// <summary>
        /// 更新相后侧高差，档距等数据，传统模式  本塔-后侧铁塔
        /// 有5个相位
        /// </summary>
        /// <param name="PntTower"></param>
        public void FlashBackHeiSub(TowerElecCals pntTower)
        {
            PhaseTraList[0].SpaceStr.SubHei = AtiUpSide - pntTower.AtiUpSide;
            PhaseTraList[0].SpaceStr.GDHei = AbsUpSideHei;
            PhaseTraList[0].SpaceStr.Span = BackPosRes.Span;
            PhaseTraList[1].SpaceStr.SubHei = AtiMid - pntTower.AtiMid;
            PhaseTraList[1].SpaceStr.GDHei = AbsMidHei;
            PhaseTraList[1].SpaceStr.Span = BackPosRes.Span;
            PhaseTraList[2].SpaceStr.SubHei = AtiDownd - pntTower.AtiDownd;
            PhaseTraList[2].SpaceStr.GDHei = AbsDownSideHei;
            PhaseTraList[2].SpaceStr.Span = BackPosRes.Span;
            PhaseTraList[3].SpaceStr.SubHei = AtiGrd - pntTower.AtiGrd;
            PhaseTraList[3].SpaceStr.GDHei = AbsGrdHei;
            PhaseTraList[3].SpaceStr.Span = BackPosRes.Span;
            PhaseTraList[4].SpaceStr.SubHei = AtiGrd - pntTower.AtiGrd;
            PhaseTraList[4].SpaceStr.GDHei = AbsGrdHei;
            PhaseTraList[4].SpaceStr.Span = BackPosRes.Span;
        }

        /// <summary>
        /// 更新前侧高差，传统模式  本塔-前侧铁塔
        /// 有5个相位
        /// </summary>
        /// <param name="pntTower"></param>
        public void FlashFrontHeiSub(TowerElecCals pntTower)
        {
            PhaseTraList[5].SpaceStr.SubHei = AtiUpSide - pntTower.AtiUpSide;
            PhaseTraList[5].SpaceStr.GDHei = AbsUpSideHei;
            PhaseTraList[5].SpaceStr.Span = FrontPosRes.Span;
            PhaseTraList[6].SpaceStr.SubHei = AtiMid - pntTower.AtiMid;
            PhaseTraList[6].SpaceStr.GDHei = AbsMidHei;
            PhaseTraList[6].SpaceStr.Span = FrontPosRes.Span;
            PhaseTraList[7].SpaceStr.SubHei = AtiDownd - pntTower.AtiDownd;
            PhaseTraList[7].SpaceStr.GDHei = AbsDownSideHei;
            PhaseTraList[7].SpaceStr.Span = FrontPosRes.Span;
            PhaseTraList[8].SpaceStr.SubHei = AtiGrd - pntTower.AtiGrd;
            PhaseTraList[8].SpaceStr.GDHei = AbsGrdHei;
            PhaseTraList[8].SpaceStr.Span = FrontPosRes.Span;
            PhaseTraList[9].SpaceStr.SubHei = AtiGrd - pntTower.AtiGrd;
            PhaseTraList[9].SpaceStr.GDHei = AbsGrdHei;
            PhaseTraList[9].SpaceStr.Span = FrontPosRes.Span;
        }

        /// <summary>
        /// 计算各个工况的垂直档距，耐张塔分为前后侧计算
        /// </summary>
        void CalHoriVetValue()
        {

        }

        /// <summary>
        /// 计算大风工况下垂直方向的应用弧垂
        /// </summary>
        void CalDFCure()
        {

        }

        public List<string> PrintJumpStrLoad()
        {
            List<string> rslt1 = new List<string>();
            List<string> rslt2 = new List<string>();
            List<string> rslt3 = new List<string>();

            var wkCdtNames = PhaseTraList[0].WireData.WeatherParas.NameOfWkCalsInd;

            foreach (var name in wkCdtNames)
            {
                var load1 = PhaseTraList[0].JumpStrLoad[name] == null ? new JumpStrLoadResult() : PhaseTraList[0].JumpStrLoad[name];
                var load2 = PhaseTraList[1].JumpStrLoad[name] == null ? new JumpStrLoadResult() : PhaseTraList[1].JumpStrLoad[name];
                var load3 = PhaseTraList[2].JumpStrLoad[name] == null ? new JumpStrLoadResult() : PhaseTraList[2].JumpStrLoad[name];

                string strValue1 = FileUtils.PadRightEx(name, 26) + FileUtils.PadRightEx(load1.Temperature.ToString(), 8) + FileUtils.PadRightEx(load1.WindSpeed.ToString(), 8) 
                    + FileUtils.PadRightEx(load1.IceThickness.ToString(), 8) + FileUtils.PadRightEx(load1.BaseWindSpeed.ToString(), 12) + FileUtils.PadRightEx(load1.JumpStrWindLoad.ToString("0.##"), 12) 
                    + FileUtils.PadRightEx(load2.JumpStrWindLoad.ToString("0.##"), 12) + FileUtils.PadRightEx(load3.JumpStrWindLoad.ToString("0.##"), 12);
                rslt1.Add(strValue1);

                string strValue2 = FileUtils.PadRightEx(name, 26) + FileUtils.PadRightEx(load1.Temperature.ToString(), 8) + FileUtils.PadRightEx(load1.WindSpeed.ToString(), 8)
                    + FileUtils.PadRightEx(load1.IceThickness.ToString(), 8) + FileUtils.PadRightEx(load1.BaseWindSpeed.ToString(), 12) + FileUtils.PadRightEx(load1.JumpWindLoad.ToString("0.##"), 12)
                    + FileUtils.PadRightEx(load2.JumpWindLoad.ToString("0.##"), 12) + FileUtils.PadRightEx(load3.JumpWindLoad.ToString("0.##"), 12);
                rslt2.Add(strValue2);

                string strValue3 = FileUtils.PadRightEx(name, 26) + FileUtils.PadRightEx(load1.Temperature.ToString(), 8) + FileUtils.PadRightEx(load1.WindSpeed.ToString(), 8)
                    + FileUtils.PadRightEx(load1.IceThickness.ToString(), 8) + FileUtils.PadRightEx(load1.BaseWindSpeed.ToString(), 12) + FileUtils.PadRightEx(load1.SuTubleWindLoad.ToString("0.##"), 12)
                    + FileUtils.PadRightEx(load2.SuTubleWindLoad.ToString("0.##"), 12) + FileUtils.PadRightEx(load3.SuTubleWindLoad.ToString("0.##"), 12);
                rslt3.Add(strValue3);
            }

            string strTitle = FileUtils.PadRightEx("气象条件", 26) + FileUtils.PadRightEx("温度：", 8) + FileUtils.PadRightEx("风速：", 8) + FileUtils.PadRightEx("覆冰：", 8)
                + FileUtils.PadRightEx("基本风速：", 12) + FileUtils.PadRightEx("中相：", 12) + FileUtils.PadRightEx("边相：", 12) + FileUtils.PadRightEx("边相：", 12);

            List<string> rslt = new List<string>();
            rslt.Add("跳线绝缘子串风荷载");
            rslt.Add(strTitle);
            rslt.AddRange(rslt1);

            rslt.Add("\n跳线风荷载");
            rslt.Add(strTitle);
            rslt.AddRange(rslt2);

            rslt.Add("\n支撑管线风荷载");
            rslt.Add(strTitle);
            rslt.AddRange(rslt3);

            return rslt;
        }



    }
}
