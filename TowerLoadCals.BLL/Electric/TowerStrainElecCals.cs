using TowerLoadCals.Common;

namespace TowerLoadCals.BLL.Electric
{
    public class TowerStrainElecCals:TowerElecCals
    {
        public TowerStrainElecCals()
        {
            
        }

        /// <summary>
        /// 拷贝前后计算资源进入计算模型，并更新导线、地线数据，前体是铁塔本体数据已更新
        /// </summary>
        /// <param name="BackSideResSor"></param>
        /// <param name="FrontSideResSor"></param>
        void GetAndUpdateSideRes(ElecCalsRes BackSideResSor, ElecCalsRes FrontSideResSor)
        {
            BackSideRes = XmlUtils.Clone(BackSideResSor);
            FrontSideRes = XmlUtils.Clone(FrontSideResSor);
            BackSideRes.FlashWireData(BackPosRes.DRepresentSpan);
            FrontSideRes.FlashWireData(FrontPosRes.DRepresentSpan);
            PhaseTraList[0].WrieData = PhaseTraList[1].WrieData = PhaseTraList[2].WrieData = BackSideRes.IndWire;
            PhaseTraList[5].WrieData = PhaseTraList[6].WrieData = PhaseTraList[7].WrieData = FrontSideRes.IndWire;
            PhaseTraList[3].WrieData = BackSideRes.GrdWire;
            PhaseTraList[4].WrieData = BackSideRes.OPGWWire;
            PhaseTraList[8].WrieData = FrontSideRes.GrdWire;
            PhaseTraList[9].WrieData = FrontSideRes.OPGWWire;
            PhaseTraList[0].JmWrieData = PhaseTraList[1].JmWrieData = PhaseTraList[2].JmWrieData = BackSideRes.JumWire;
            PhaseTraList[5].JmWrieData = PhaseTraList[6].JmWrieData = PhaseTraList[7].JmWrieData = FrontSideRes.JumWire;
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

    }
}
