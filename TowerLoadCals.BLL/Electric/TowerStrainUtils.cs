using TowerLoadCals.Common;

namespace TowerLoadCals.BLL.Electric
{
    public class TowerStrainUtils:TowerUtils
    {
        public TowerStrainUtils()
        {
            
        }

        /// <summary>
        /// 拷贝前后计算资源进入计算模型，并更新导线、地线数据，前期是铁塔本体数据已更新
        /// </summary>
        /// <param name="BackSideResSor"></param>
        /// <param name="FrontSideResSor"></param>
        void GetAndUpdateSideRes(ElectrialCals BackSideResSor, ElectrialCals FrontSideResSor)
        {
            BackSideRes = XmlUtils.Clone(BackSideResSor);
            FrontSideRes = XmlUtils.Clone(FrontSideResSor);
            BackSideRes.FlashWireData(BackPosRes.DRepresentSpan);
            FrontSideRes.FlashWireData(FrontPosRes.DRepresentSpan);
            //PhaseTraList[0].WrieData = PhaseTraList[1].WrieData = PhaseTraList[2].WrieData = BackPosRes.IndWire;
            //PhaseTraList[5].WrieData = PhaseTraList[6].WrieData = PhaseTraList[7].WrieData = FrontPosRes.IndWire;
            //PhaseTraList[3].WrieData = BackPosRes.GrdWire;
            //PhaseTraList[4].WrieData = BackPosRes.OPGWWire;
            //PhaseTraList[8].WrieData = BackPosRes.GrdWire;
            //PhaseTraList[9].WrieData = BackPosRes.OPGWWire;
            //PhaseTraList[0].JmWrieData = PhaseTraList[1].JmWrieData = PhaseTraList[2].JmWrieData = BackPosRes.JumWire;
            //PhaseTraList[5].JmWrieData = PhaseTraList[6].JmWrieData = PhaseTraList[7].JmWrieData = FrontPosRes.JumWire;
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
