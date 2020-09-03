using TowerLoadCals.Common;

namespace TowerLoadCals.BLL.Electric
{
    public class TowerHangElecCals : TowerElecCals
    {
        public override  string TowerType { get { return "悬垂塔"; } }

        public ElecCalsRes SideRes { get; set; }

        public TowerHangElecCals()
        {
            for (int i = 0; i < 10; i++)
            {
                PhaseTraList.Add(new ElecCalsPhaseStrHang());
            }
        }

        /// <summary>
        /// 按照传统计算模式更新铁塔的本身各个挂点高度
        /// </summary>
        //public override void UpdataTowerTraHei()
        //{
        //    AbsUpSideHei = Height + UpSideInHei - RepStrIndLen;
        //    AbsMidHei = Height + MidInHei - RepStrIndLen;
        //    AbsDownSideHei = Height + DownSideHei - RepStrIndLen;
        //    AbsGrdHei = Height + GrDHei - RepStrGrdLen;
        //    AbsDownJumHei = Height + DnSideJuHei;
        //    AbsMidJumHei = Height + MidJuHei;
        //    AbsUpJumHei = Height + UpSideJuHei;
        //}

        public void GetAndUpdateSideRes(ElecCalsRes SideResSor)
        {
            SideRes = XmlUtils.Clone(SideResSor);

            SideRes.FlashWireData(TowerType, BackPosRes.DRepresentSpan, AngelofApplication);

            SideRes.FlashJumWireData(TowerType, AngelofApplication);

            PhaseTraList[0].WireData = PhaseTraList[1].WireData = PhaseTraList[2].WireData = SideResSor.IndWire;
            PhaseTraList[5].WireData = PhaseTraList[6].WireData = PhaseTraList[7].WireData = SideResSor.IndWire;
            PhaseTraList[3].WireData = SideResSor.GrdWire;
            PhaseTraList[4].WireData = SideResSor.OPGWWire;
            PhaseTraList[8].WireData = SideResSor.GrdWire;
            PhaseTraList[9].WireData = SideResSor.OPGWWire;
            PhaseTraList[0].JmWireData = PhaseTraList[1].JmWireData = PhaseTraList[2].JmWireData = SideResSor.JumWire;
            PhaseTraList[5].JmWireData = PhaseTraList[6].JmWireData = PhaseTraList[7].JmWireData = SideResSor.JumWire;
        }
    }
}
