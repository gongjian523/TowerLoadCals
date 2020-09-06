using System.Collections.Generic;
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

            PhaseTraList[0].WireData = PhaseTraList[1].WireData = PhaseTraList[2].WireData = SideRes.IndWire;
            PhaseTraList[5].WireData = PhaseTraList[6].WireData = PhaseTraList[7].WireData = SideRes.IndWire;
            PhaseTraList[3].WireData = SideRes.GrdWire;
            PhaseTraList[4].WireData = SideRes.OPGWWire;
            PhaseTraList[8].WireData = SideRes.GrdWire;
            PhaseTraList[9].WireData = SideRes.OPGWWire;
        }


        public void  UpdateDFCure()
        {
            for(int i = 0; i  < 9; i++)
            {
                double span = (BackPosRes.Span + FrontPosRes.Span) /2 ;
                double aveHei = PhaseTraList[i].WireData.bGrd == 0 ? SideRes.CommParas.IndAveHei : SideRes.CommParas.GrdAveHei;
                double repLen = PhaseTraList[i].WireData.bGrd == 0 ? RepStrIndLen : RepStrGrdLen;
                PhaseTraList[i].CalDFCure(span, repLen, SideRes.CommParas.GrdCl, SideRes.CommParas.TerType, aveHei);
            }
        }


        public List<string> PrintDFCure()
        {
            List<string> rslt = new List<string>();

            rslt.Add(FileUtils.PadRightEx("\n高度系数", 26) + FileUtils.PadRightEx("导线中相", 12) + FileUtils.PadRightEx("导线左边相", 12) + FileUtils.PadRightEx("导线右边相", 12) + FileUtils.PadRightEx("地线", 12) + FileUtils.PadRightEx("地线", 12));

            string str1 = FileUtils.PadRightEx("挂点高", 26);
            string str2 = FileUtils.PadRightEx("弧垂1", 26);
            string str3 = FileUtils.PadRightEx("弧垂2", 26);
            string str4 = FileUtils.PadRightEx("计算弧垂", 26);
            string str5 = FileUtils.PadRightEx("大风风偏角", 26);
            string str6 = FileUtils.PadRightEx("计算高度", 26);
            string str7 = FileUtils.PadRightEx("线风压系数", 26);
            string str8 = FileUtils.PadRightEx("串风压系数", 26);

            for(int i = 0; i < 5; i++)
            {
                str1 += FileUtils.PadRightEx(PhaseTraList[i].SpaceStr.GDHei.ToString("0.#"), 12);
                str2 += FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[i]).HC1.ToString("0.#"), 12);
                str3 += FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[i]).HC2.ToString("0.#"), 12);
                str4 += FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[i]).HC.ToString("0.#"), 12);
                str5 += FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[i]).WindAngle.ToString("0.##"), 12);
                str6 += FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[i]).CalHei.ToString("0.##"), 12);
                str7 += FileUtils.PadRightEx(PhaseTraList[i].WireWindPara.ToString("0.###"), 12);
                str8 += FileUtils.PadRightEx(PhaseTraList[i].StrWindPara.ToString("0.###"), 12);
            }

            rslt.Add(str1);
            rslt.Add(str2);
            rslt.Add(str3);
            rslt.Add(str4);
            rslt.Add(str5);
            rslt.Add(str6);
            rslt.Add(str7);
            rslt.Add(str8);

            return rslt;
        }
    }
}
