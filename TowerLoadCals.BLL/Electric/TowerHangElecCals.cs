using System;
using System.Collections.Generic;
using System.Linq;
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

        public void Cals()
        {
            UpdateVertialSpan();

            for (int i = 0; i <= 9; i++)
            {
                var spanFit = SideRes.SpanFit;

                double diaInc, weiInc, secInc, weiFZC, wireExtend;
                int numFZC;

                if (PhaseTraList[i].WireData.bGrd == 0)
                {
                    diaInc = SideRes.CommParas.DiaIndInc;
                    weiInc = SideRes.CommParas.WeiIndInc;
                    numFZC = spanFit.NumInFZC;
                    weiFZC = spanFit.WeiInFZC;
                    secInc = SideRes.CommParas.SecIndInc;
                }
                else if (PhaseTraList[i].WireData.bGrd == 1)
                {
                    diaInc = SideRes.CommParas.DiaGrdInc;
                    weiInc = SideRes.CommParas.WeiGrdInc;
                    numFZC = spanFit.NumGrFZC;
                    weiFZC = spanFit.WeiGrFZC;
                    secInc = SideRes.CommParas.SecGrdInc;
                }
                else
                {
                    diaInc = SideRes.CommParas.DiaOPGWInc;
                    weiInc = SideRes.CommParas.WeiOPGWInc;
                    numFZC = spanFit.NumGrFZC;
                    weiFZC = spanFit.WeiGrFZC;
                    secInc = SideRes.CommParas.SecOPGWInc;
                }

                wireExtend = PhaseTraList[i].WireData.bGrd == 0 ? SideRes.CommParas.IndExMaxPara : SideRes.CommParas.GrdExMaxPara;

                PhaseTraList[i].UpdateHorFor(diaInc);
                PhaseTraList[i].UpdateVerWei(weiInc, spanFit.NumJGB, spanFit.WeiJGB, numFZC, weiFZC);
                PhaseTraList[i].UpdateLoStr(secInc, SideRes.CommParas.BuildMaxPara, SideRes.CommParas.InstMaxPara, wireExtend);
            }
        }

        //直线塔的垂直档距需要将前后侧相加
        protected void UpdateVertialSpan()
        {
            for (int i = 0; i <= 4; i++)
            {
                foreach (var nameWd in PhaseTraList[i].WireWorkConditionNames)
                {
                    double verSpan = PhaseTraList[i].UpdateVertialSpan(nameWd, out string rstStr);
                    double verSpanAn = PhaseTraList[i+5].UpdateVertialSpan(nameWd, out string rstStrAn);
                    int index = PhaseTraList[i].LoadList.FindIndex(item => item.GKName == nameWd);
                    int indexAn = PhaseTraList[i+5].LoadList.FindIndex(item => item.GKName == nameWd);

                    if (index > 0)
                    {
                        PhaseTraList[i].LoadList[index].VetiSpan = verSpan + verSpanAn;
                        PhaseTraList[i].LoadList[index].VetiSpanStr = rstStr;
                    }
                    else
                    {
                        PhaseTraList[i].LoadList.Add(new LoadThrDe()
                        {
                            GKName = nameWd,
                            VetiSpan = verSpan + verSpanAn,
                            VetiSpanStr = rstStr,
                        });
                    }

                    if (indexAn > 0)
                    {
                        PhaseTraList[i+5].LoadList[index].VetiSpan = verSpan + verSpanAn;
                        PhaseTraList[i+5].LoadList[index].VetiSpanStr = rstStrAn;
                    }
                    else
                    {
                        PhaseTraList[i+5].LoadList.Add(new LoadThrDe()
                        {
                            GKName = nameWd,
                            VetiSpan = verSpan + verSpanAn,
                            VetiSpanStr = rstStrAn,
                        });
                    }
                }
            }
        }

        public void UpdateTensionDiff()
        {
            for (int i = 0; i <= 9; i++)
            {
                double secInc, effectPara, savePara;

                if (PhaseTraList[i].WireData.bGrd == 0)
                {
                    secInc = SideRes.CommParas.SecIndInc;
                    effectPara = SideRes.SideParas.IndEffectPara;
                    savePara = SideRes.SideParas.IndSafePara;
                }
                else if (PhaseTraList[i].WireData.bGrd == 1)
                {
                    secInc = SideRes.CommParas.SecGrdInc;
                    effectPara = SideRes.SideParas.GrdEffectPara;
                    savePara = SideRes.SideParas.GrdSafePara;
                }
                else
                {
                    secInc = SideRes.CommParas.SecOPGWInc;
                    effectPara = SideRes.SideParas.OPGWEffectPara;
                    savePara = SideRes.SideParas.OPGWSafePara;
                }

                PhaseTraList[i].UpdateTensionDiff(secInc, effectPara, savePara);
            }

            for(int i = 0; i  <= 4; i++)
            {
                ElecCalsPhaseStrHang smallSide = (ElecCalsPhaseStrHang)PhaseTraList[i];
                ElecCalsPhaseStrHang largeSide = (ElecCalsPhaseStrHang)PhaseTraList[i+5];

                //断线计算 
                //直线塔计算项
                //Max=最大张力
                smallSide.TenBreakMaxTensionS = Math.Round(smallSide.TenBreakMax, 0) - Math.Round(smallSide.BreakTenDiff, 0);
                smallSide.TenBreakMaxTensionL = Math.Round(smallSide.TenBreakMax, 0);
                largeSide.TenBreakMaxTensionS = Math.Round(largeSide.TenBreakMax, 0);
                largeSide.TenBreakMaxTensionL = Math.Round(largeSide.TenBreakMax, 0) - Math.Round(largeSide.BreakTenDiff, 0);

                //Max=覆冰100%断线工况
                if (smallSide.WireData.bGrd == 0)
                {
                    smallSide.TenBreakMaxIceCov100S = Math.Round(smallSide.TenBreakIceCov100, 0) - Math.Round(smallSide.BreakTenDiff, 0);
                    smallSide.TenBreakMaxIceCov100L = Math.Round(smallSide.TenBreakIceCov100, 0);
                    largeSide.TenBreakMaxIceCov100S = Math.Round(largeSide.TenBreakIceCov100, 0);
                    largeSide.TenBreakMaxIceCov100L = Math.Round(largeSide.TenBreakIceCov100, 0) - Math.Round(largeSide.BreakTenDiff, 0);
                }
                else
                {
                    double tempS = Math.Round(smallSide.TenBreakIceCov100, 0) - Math.Round(smallSide.BreakTenDiff, 0);
                    smallSide.TenBreakMaxIceCov100S = tempS < 0 ? 0 : tempS;
                    largeSide.TenBreakMaxIceCov100S = smallSide.TenBreakMaxIceCov100S + Math.Round(largeSide.BreakTenDiff, 0);

                    //这儿为什么全部用小号侧数据
                    double tempL = Math.Round(smallSide.TenBreakIceCov100, 0) - Math.Round(smallSide.BreakTenDiff, 0);
                    largeSide.TenBreakMaxIceCov100L = tempL < 0 ? 0 : tempL;
                    smallSide.TenBreakMaxIceCov100L = largeSide.TenBreakMaxIceCov100L + Math.Round(smallSide.BreakTenDiff, 0);
                }
                //最大张力取值
                smallSide.BreakTenMaxS = smallSide.WireData.CommParas.BreakMaxPara == 1 ? smallSide.TenBreakMaxTensionS : smallSide.TenBreakMaxIceCov100S;
                smallSide.BreakTenMaxL = smallSide.WireData.CommParas.BreakMaxPara == 1 ? smallSide.TenBreakMaxTensionL : smallSide.TenBreakMaxIceCov100L;
                largeSide.BreakTenMaxS = largeSide.WireData.CommParas.BreakMaxPara == 1 ? largeSide.TenBreakMaxTensionS : largeSide.TenBreakMaxIceCov100S;
                largeSide.BreakTenMaxL = largeSide.WireData.CommParas.BreakMaxPara == 1 ? largeSide.TenBreakMaxTensionL : largeSide.TenBreakMaxIceCov100L;

                //直线塔
                //O与张力差(在excel中有一个与是否增加5mm的比较过程，但是里面没有填写任何值，故计算过程简化)
                smallSide.TenBreakS = smallSide.WireData.CommParas.BreakInPara == 1 ? 0 : smallSide.BreakTenMaxS;
                smallSide.TenBreakL = smallSide.WireData.CommParas.BreakInPara == 1 ? Math.Round(smallSide.BreakTenDiff, 0) : smallSide.BreakTenMaxL;
                largeSide.TenBreakS = largeSide.WireData.CommParas.BreakInPara == 1 ? Math.Round(largeSide.BreakTenDiff, 0) : largeSide.BreakTenMaxS;
                largeSide.TenBreakL = smallSide.WireData.CommParas.BreakInPara == 1 ? 0 : largeSide.BreakTenMaxL;

                //开断
                if (smallSide.WireData.bGrd != 0)
                {
                    //直线塔计算项
                    //Max=最大张力
                    smallSide.TenBreakMaxTensionBreakS = Math.Round(smallSide.TenBreakMax, 0) - Math.Round(smallSide.BreakTenDiffGrdBre, 0);
                    smallSide.TenBreakMaxTensionBreakL = Math.Round(smallSide.TenBreakMax, 0);
                    largeSide.TenBreakMaxTensionBreakS = Math.Round(largeSide.TenBreakMax, 0);
                    largeSide.TenBreakMaxTensionBreakL = Math.Round(largeSide.TenBreakMax, 0) - Math.Round(largeSide.BreakTenDiffGrdBre, 0);

                    //Excel中写的是大号塔
                    double tempS = Math.Round(largeSide.TenBreakIceCov100, 0) - Math.Round(smallSide.BreakTenDiffGrdBre, 0);
                    smallSide.TenBreakMaxIceCov100BreakS = tempS < 0 ? 0 : tempS;
                    largeSide.TenBreakMaxIceCov100BreakS = smallSide.TenBreakMaxIceCov100BreakS + Math.Round(largeSide.BreakTenDiffGrdBre, 0);

                    double tempL = Math.Round(smallSide.TenBreakIceCov100, 0) - Math.Round(largeSide.BreakTenDiffGrdBre, 0);
                    largeSide.TenBreakMaxIceCov100BreakL = tempL < 0 ? 0 : tempL;
                    smallSide.TenBreakMaxIceCov100BreakL = largeSide.TenBreakMaxIceCov100BreakL + Math.Round(smallSide.BreakTenDiffGrdBre, 0);

                    //最大张力取值
                    smallSide.BreakTenMaxBreakS = smallSide.WireData.CommParas.BreakMaxPara == 1 ? smallSide.TenBreakMaxTensionBreakS : smallSide.TenBreakMaxIceCov100BreakS;
                    largeSide.BreakTenMaxBreakS = largeSide.WireData.CommParas.BreakMaxPara == 1 ? largeSide.TenBreakMaxTensionBreakS : largeSide.TenBreakMaxIceCov100BreakS;

                    smallSide.BreakTenMaxBreakL = smallSide.WireData.CommParas.BreakMaxPara == 1 ? smallSide.TenBreakMaxTensionBreakL : smallSide.TenBreakMaxIceCov100BreakL;
                    largeSide.BreakTenMaxBreakL = largeSide.WireData.CommParas.BreakMaxPara == 1 ? largeSide.TenBreakMaxTensionBreakL : largeSide.TenBreakMaxIceCov100BreakL;

                    var breakItemS = smallSide.LoadList.Where(item => item.GKName == "断线开断").FirstOrDefault();
                    double breakLoStrS = breakItemS == null ? 0 : breakItemS.LoStr / SideRes.CommParas.BuildMaxPara ;
                    var breakA5ItemS = smallSide.LoadList.Where(item => item.GKName == "断线开断(导线+5mm)").FirstOrDefault();
                    double breaA5kLoStrS = breakA5ItemS == null ? 0 : breakA5ItemS.LoStr / SideRes.CommParas.BuildMaxPara;

                    smallSide.BreakTenAdd5mmBreakS = 0;
                    smallSide.BreakTenAdd5mmBreakL = smallSide.WireData.CommParas.GrdIceUnbaPara == 1 ? Math.Round(breakLoStrS, 0) : Math.Round(breaA5kLoStrS, 0);

                    var breakItemL = largeSide.LoadList.Where(item => item.GKName == "断线开断").FirstOrDefault();
                    double breakLoStrL = breakItemL == null ? 0 : breakItemL.LoStr / SideRes.CommParas.BuildMaxPara;
                    var breakA5ItemL = largeSide.LoadList.Where(item => item.GKName == "断线开断(导线+5mm)").FirstOrDefault();
                    double breaA5kLoStrL = breakA5ItemL == null ? 0 : breakA5ItemL.LoStr / SideRes.CommParas.BuildMaxPara;

                    largeSide.BreakTenAdd5mmBreakS = largeSide.WireData.CommParas.GrdIceUnbaPara == 1 ? Math.Round(breakLoStrL, 0) : Math.Round(breaA5kLoStrL, 0);
                    largeSide.BreakTenAdd5mmBreakL = 0;

                    //断线的最终结果
                    smallSide.BreakTenBreakS = Math.Abs(smallSide.TenBreakMaxIceCov100BreakS - largeSide.TenBreakMaxIceCov100BreakS) > Math.Abs(smallSide.BreakTenAdd5mmBreakS 
                        - largeSide.BreakTenAdd5mmBreakS) ? smallSide.TenBreakMaxIceCov100BreakS : smallSide.BreakTenAdd5mmBreakS;
                    smallSide.BreakTenBreakL = Math.Abs(smallSide.TenBreakMaxIceCov100BreakL - largeSide.TenBreakMaxIceCov100BreakL) > Math.Abs(smallSide.BreakTenAdd5mmBreakL
                        - largeSide.BreakTenAdd5mmBreakL) ? smallSide.TenBreakMaxIceCov100BreakL : smallSide.BreakTenAdd5mmBreakL;

                    largeSide.BreakTenBreakS = Math.Abs(smallSide.TenBreakMaxIceCov100BreakS - largeSide.TenBreakMaxIceCov100BreakS) > Math.Abs(smallSide.BreakTenAdd5mmBreakS
                        - largeSide.BreakTenAdd5mmBreakS) ? largeSide.TenBreakMaxIceCov100BreakS : largeSide.BreakTenAdd5mmBreakS;
                    largeSide.BreakTenBreakL = Math.Abs(smallSide.TenBreakMaxIceCov100BreakL - largeSide.TenBreakMaxIceCov100BreakL) > Math.Abs(smallSide.BreakTenAdd5mmBreakL
                        - largeSide.BreakTenAdd5mmBreakL) ? largeSide.TenBreakMaxIceCov100BreakL : largeSide.BreakTenAdd5mmBreakL;
                }

                //不均匀冰计算 
                //直线塔计算项
                //Max=最大张力
                smallSide.TenUnbaIceMaxTensionS = Math.Round(smallSide.TenUnbaIceMax, 0) - Math.Round(smallSide.UnbaIceTenDiff, 0);
                largeSide.TenUnbaIceMaxTensionL = Math.Round(largeSide.TenUnbaIceMax, 0) - Math.Round(largeSide.UnbaIceTenDiff, 0);
                largeSide.TenUnbaIceMaxTensionS = smallSide.TenUnbaIceMaxTensionS + Math.Round(smallSide.UnbaIceTenDiff, 0);
                smallSide.TenUnbaIceMaxTensionL = largeSide.TenUnbaIceMaxTensionL + Math.Round(largeSide.UnbaIceTenDiff, 0);

                //Max=覆冰100%断线工况
                if (smallSide.WireData.bGrd == 0)
                {
                    smallSide.TenUnbaIceMaxIceCov100S = Math.Round(smallSide.TenUnbaIceIceCov100, 0) - Math.Round(smallSide.UnbaIceTenDiff, 0);
                    smallSide.TenUnbaIceMaxIceCov100L = Math.Round(smallSide.TenUnbaIceIceCov100, 0);
                    largeSide.TenUnbaIceMaxIceCov100S = Math.Round(largeSide.TenUnbaIceIceCov100, 0);
                    largeSide.TenUnbaIceMaxIceCov100L = Math.Round(largeSide.TenUnbaIceIceCov100, 0) - Math.Round(largeSide.UnbaIceTenDiff, 0);
                }
                else
                {
                    double tempS = Math.Round(smallSide.TenUnbaIceIceCov100, 0) - Math.Round(smallSide.UnbaIceTenDiff, 0);
                    smallSide.TenUnbaIceMaxIceCov100S = tempS < 0 ? 0 : tempS;
                    largeSide.TenUnbaIceMaxIceCov100S = smallSide.TenUnbaIceMaxIceCov100S + Math.Round(largeSide.UnbaIceTenDiff, 0);

                    double tempL = Math.Round(largeSide.TenUnbaIceIceCov100, 0) - Math.Round(largeSide.UnbaIceTenDiff, 0);
                    largeSide.TenUnbaIceMaxIceCov100L = tempL < 0 ? 0 : tempL;
                    smallSide.TenUnbaIceMaxIceCov100L = largeSide.TenUnbaIceMaxIceCov100L + Math.Round(smallSide.UnbaIceTenDiff, 0);

                }
                //最大张力取值
                smallSide.UnbaIceTenMaxS = smallSide.WireData.CommParas.UnbaMaxPara == 1 ? smallSide.TenUnbaIceMaxTensionS : smallSide.TenUnbaIceMaxIceCov100S;
                smallSide.UnbaIceTenMaxL = smallSide.WireData.CommParas.UnbaMaxPara == 1 ? smallSide.TenUnbaIceMaxTensionL : smallSide.TenUnbaIceMaxIceCov100L;
                largeSide.UnbaIceTenMaxS = largeSide.WireData.CommParas.UnbaMaxPara == 1 ? largeSide.TenUnbaIceMaxTensionS : largeSide.TenUnbaIceMaxIceCov100S;
                largeSide.UnbaIceTenMaxL = largeSide.WireData.CommParas.UnbaMaxPara == 1 ? largeSide.TenUnbaIceMaxTensionL : largeSide.TenUnbaIceMaxIceCov100L;

                //直线塔
                //O与张力差(在excel中有一个与是否增加5mm的比较过程，但是里面没有填写任何值，故计算过程简化)
                smallSide.TenUnbaIceS = smallSide.WireData.CommParas.UnbaInPara == 1 ? 0 : smallSide.UnbaIceTenMaxS;
                smallSide.TenUnbaIceL = smallSide.WireData.CommParas.UnbaInPara == 1 ? Math.Round(smallSide.UnbaIceTenDiff, 0) : smallSide.UnbaIceTenMaxL;
                largeSide.TenUnbaIceS = largeSide.WireData.CommParas.UnbaInPara == 1 ? Math.Round(smallSide.UnbaIceTenDiff, 0) : largeSide.UnbaIceTenMaxS;
                largeSide.TenUnbaIceL = smallSide.WireData.CommParas.UnbaInPara == 1 ? 0 : largeSide.UnbaIceTenMaxL;

                if (smallSide.WireData.bGrd != 0)
                {
                    //直线塔计算项
                    //Max=最大张力
                    smallSide.TenUnbaIceMaxTensionBreakS = Math.Round(smallSide.TenUnbaIceMax, 0) - Math.Round(smallSide.UnbaIceTenDiffGrdBre, 0);
                    smallSide.TenUnbaIceMaxTensionBreakL = Math.Round(smallSide.TenUnbaIceMax, 0);
                    largeSide.TenUnbaIceMaxTensionBreakS = Math.Round(largeSide.TenUnbaIceMax, 0);
                    largeSide.TenUnbaIceMaxTensionBreakL = Math.Round(largeSide.TenUnbaIceMax, 0) - Math.Round(largeSide.UnbaIceTenDiffGrdBre, 0);

                    //Excel中写的是大号塔
                    double tempS = Math.Round(largeSide.TenUnbaIceIceCov100, 0) - Math.Round(smallSide.UnbaIceTenDiffGrdBre, 0);
                    smallSide.TenUnbaIceMaxIceCov100BreakS = tempS < 0 ? 0 : tempS;
                    largeSide.TenUnbaIceMaxIceCov100BreakS = smallSide.TenUnbaIceMaxIceCov100BreakS + Math.Round(largeSide.UnbaIceTenDiffGrdBre, 0);

                    double tempL = Math.Round(smallSide.TenUnbaIceIceCov100, 0) - Math.Round(largeSide.UnbaIceTenDiffGrdBre, 0);
                    largeSide.TenUnbaIceMaxIceCov100BreakL = tempL < 0 ? 0 : tempL;
                    smallSide.TenUnbaIceMaxIceCov100BreakL = largeSide.TenUnbaIceMaxIceCov100BreakL + Math.Round(smallSide.UnbaIceTenDiffGrdBre, 0);

                    smallSide.UnbaIceTenMaxBreakS = smallSide.WireData.CommParas.UnbaMaxPara == 1 ? smallSide.TenUnbaIceMaxTensionBreakS : smallSide.TenUnbaIceMaxIceCov100BreakS;
                    largeSide.UnbaIceTenMaxBreakS = largeSide.WireData.CommParas.UnbaMaxPara == 1 ? largeSide.TenUnbaIceMaxTensionBreakS : largeSide.TenUnbaIceMaxIceCov100BreakS;

                    smallSide.UnbaIceTenMaxBreakL = smallSide.WireData.CommParas.UnbaMaxPara == 1 ? smallSide.TenUnbaIceMaxTensionBreakL : smallSide.TenUnbaIceMaxIceCov100BreakL;
                    largeSide.UnbaIceTenMaxBreakL = largeSide.WireData.CommParas.UnbaMaxPara == 1 ? largeSide.TenUnbaIceMaxTensionBreakL : largeSide.TenUnbaIceMaxIceCov100BreakL;

                    var breakItemS = smallSide.LoadList.Where(item => item.GKName == "不均匀冰开断").FirstOrDefault();
                    double breakLoStrS = breakItemS == null ? 0 : breakItemS.LoStr / SideRes.CommParas.BuildMaxPara;
                    var breakA5ItemS = smallSide.LoadList.Where(item => item.GKName == "不均匀冰开断(导线+5mm)").FirstOrDefault();
                    double breakA5kLoStrS = breakA5ItemS == null ? 0 : breakA5ItemS.LoStr / SideRes.CommParas.BuildMaxPara;
                    smallSide.UnbaIceTenAdd5mmBreakS = smallSide.WireData.CommParas.GrdIceUnbaPara == 1 ? Math.Round(breakLoStrS, 0) : Math.Round(breakA5kLoStrS, 0);

                    var fullIceItemS = smallSide.LoadList.Where(item => item.GKName == "不均匀冰满冰").FirstOrDefault();
                    double fullIceLoStrS = fullIceItemS == null ? 0 : fullIceItemS.LoStr / SideRes.CommParas.BuildMaxPara;
                    var fullIceA5ItemS = smallSide.LoadList.Where(item => item.GKName == "不均匀冰满冰(导线+5mm)").FirstOrDefault();
                    double fullIceA5kLoStrS = fullIceA5ItemS == null ? 0 : fullIceA5ItemS.LoStr / SideRes.CommParas.BuildMaxPara;
                    smallSide.UnbaIceTenAdd5mmBreakL = smallSide.WireData.CommParas.GrdIceUnbaPara == 1 ? Math.Round(fullIceLoStrS, 0) : Math.Round(fullIceA5kLoStrS, 0);

                    var breakItemL = largeSide.LoadList.Where(item => item.GKName == "不均匀冰开断").FirstOrDefault();
                    double breakLoStrL = breakItemL == null ? 0 : breakItemL.LoStr / SideRes.CommParas.BuildMaxPara;
                    var breakA5ItemL = largeSide.LoadList.Where(item => item.GKName == "不均匀冰开断(导线+5mm)").FirstOrDefault();
                    double breaA5kLoStrL = breakA5ItemL == null ? 0 : breakA5ItemL.LoStr / SideRes.CommParas.BuildMaxPara;
                    largeSide.UnbaIceTenAdd5mmBreakL = largeSide.WireData.CommParas.GrdIceUnbaPara == 1 ? Math.Round(breakLoStrL, 0) : Math.Round(breaA5kLoStrL, 0);

                    var fullIceItemL = largeSide.LoadList.Where(item => item.GKName == "不均匀冰满冰").FirstOrDefault();
                    double fullIceLoStrL = fullIceItemL == null ? 0 : fullIceItemL.LoStr / SideRes.CommParas.BuildMaxPara;
                    var fullIceA5ItemL = largeSide.LoadList.Where(item => item.GKName == "不均匀冰满冰(导线+5mm)").FirstOrDefault();
                    double fullIceA5kLoStrL = fullIceA5ItemL == null ? 0 : fullIceA5ItemL.LoStr / SideRes.CommParas.BuildMaxPara;
                    largeSide.UnbaIceTenAdd5mmBreakS = largeSide.WireData.CommParas.GrdIceUnbaPara == 1 ? Math.Round(fullIceLoStrS, 0) : Math.Round(fullIceA5kLoStrS, 0);

                    //断线的最终结果
                    smallSide.UnbaIceTenBreakS = Math.Abs(smallSide.TenUnbaIceMaxIceCov100BreakS - largeSide.TenUnbaIceMaxIceCov100BreakS) > Math.Abs(smallSide.UnbaIceTenAdd5mmBreakS
                        - largeSide.UnbaIceTenAdd5mmBreakS) ? smallSide.TenUnbaIceMaxIceCov100BreakS : smallSide.UnbaIceTenAdd5mmBreakS;
                    smallSide.UnbaIceTenBreakL = Math.Abs(smallSide.TenUnbaIceMaxIceCov100BreakL - largeSide.TenUnbaIceMaxIceCov100BreakL) > Math.Abs(smallSide.UnbaIceTenAdd5mmBreakL
                        - largeSide.UnbaIceTenAdd5mmBreakL) ? smallSide.TenUnbaIceMaxIceCov100BreakL : smallSide.UnbaIceTenAdd5mmBreakL;

                    largeSide.UnbaIceTenBreakS = Math.Abs(smallSide.TenUnbaIceMaxIceCov100BreakS - largeSide.TenUnbaIceMaxIceCov100BreakS) > Math.Abs(smallSide.UnbaIceTenAdd5mmBreakS
                        - largeSide.UnbaIceTenAdd5mmBreakS) ? largeSide.TenUnbaIceMaxIceCov100BreakS : largeSide.UnbaIceTenAdd5mmBreakS;
                    largeSide.UnbaIceTenBreakL = Math.Abs(smallSide.TenUnbaIceMaxIceCov100BreakL - largeSide.TenUnbaIceMaxIceCov100BreakL) > Math.Abs(smallSide.UnbaIceTenAdd5mmBreakL
                        - largeSide.UnbaIceTenAdd5mmBreakL) ? largeSide.TenUnbaIceMaxIceCov100BreakL : largeSide.UnbaIceTenAdd5mmBreakL;
                }
            }
        }

        public List<string> PrintCalsReslt()
        {
            List<string> rslt = new List<string>();

            rslt.Add("\n垂直档距计算：");
            for (int i = 0; i <= 4; i++)
            {
                rslt.Add(FileUtils.PadRightEx("小号侧" + i, 118) + FileUtils.PadRightEx("大号侧" + i, 118));
                string strVerSpanTitle = FileUtils.PadRightEx("工况", 26) + FileUtils.PadRightEx("计算过程：", 80) + FileUtils.PadRightEx("计算值：", 12);
                strVerSpanTitle += FileUtils.PadRightEx("工况", 26) + FileUtils.PadRightEx("计算过程：", 80) + FileUtils.PadRightEx("计算值：", 12);
                rslt.Add(strVerSpanTitle);

                var phase = PhaseTraList[i];
                var phaseAn = PhaseTraList[i + 5];

                foreach (var name in phase.WireData.WorkCdtNamesHang)
                {
                    var laod = phase.LoadList.Where(item => item.GKName == name).FirstOrDefault();
                    double verSpan = laod == null ? 0 : laod.VetiSpan;
                    string verSpanStr = laod == null ? "" : laod.VetiSpanStr;

                    var laodAn = phaseAn.LoadList.Where(item => item.GKName == name).FirstOrDefault();
                    double verSpanAn = phaseAn == null ? 0 : laodAn.VetiSpan;
                    string verSpanStrAn = phaseAn == null ? "" : laodAn.VetiSpanStr;

                    string strVerSpan = FileUtils.PadRightEx(name, 26) + FileUtils.PadRightEx(verSpanStr, 80) + FileUtils.PadRightEx(verSpan.ToString("0.###"), 12)
                        + FileUtils.PadRightEx(name, 26);
                    strVerSpan += FileUtils.PadRightEx(verSpanStrAn, 80) + FileUtils.PadRightEx(verSpanAn.ToString("0.###"), 12);
                    rslt.Add(strVerSpan);
                }
            }

            rslt.Add("\n水平荷载计算：");
            for (int i = 0; i <= 4; i++)
            {
                rslt.Add(FileUtils.PadRightEx("小号侧" + i, 118) + FileUtils.PadRightEx("大号侧" + i, 118));
                string strHorForTitle = FileUtils.PadRightEx("工况", 26) + FileUtils.PadRightEx("计算过程：", 80) + FileUtils.PadRightEx("计算值：", 12);
                strHorForTitle += FileUtils.PadRightEx("工况", 26) + FileUtils.PadRightEx("计算过程：", 80) + FileUtils.PadRightEx("计算值：", 12);
                rslt.Add(strHorForTitle);

                var phase = PhaseTraList[i];
                var phaseAn = PhaseTraList[i + 5];

                foreach (var name in phase.WireData.WorkCdtNamesHang)
                {
                    var laod = phase.LoadList.Where(item => item.GKName == name).FirstOrDefault();
                    double horFor = laod == null ? 0 : laod.HorFor;
                    string horForStr = laod == null ? "" : laod.HorForStr;

                    var laodAn = phaseAn.LoadList.Where(item => item.GKName == name).FirstOrDefault();
                    double horForAn = phaseAn == null ? 0 : laodAn.HorFor;
                    string horForStrAn = phaseAn == null ? "" : laodAn.HorForStr;

                    string strHorFor = FileUtils.PadRightEx(name, 26) + FileUtils.PadRightEx(horForStr, 80) + FileUtils.PadRightEx(horFor.ToString("0.###"), 12)
                        + FileUtils.PadRightEx(name, 26);
                    strHorFor += FileUtils.PadRightEx(horForStrAn, 80) + FileUtils.PadRightEx(horForAn.ToString("0.###"), 12);
                    rslt.Add(strHorFor);
                }
            }


            rslt.Add("\n垂直荷载计算：");
            for (int i = 0; i <= 4; i++)
            {
                rslt.Add(FileUtils.PadRightEx("小号侧" + i, 118) + FileUtils.PadRightEx("大号侧" + i, 118));
                string strVerWeiStrTitle = FileUtils.PadRightEx("工况", 26) + FileUtils.PadRightEx("计算过程：", 80) + FileUtils.PadRightEx("计算值：", 12); 
                strVerWeiStrTitle  += FileUtils.PadRightEx("工况", 26) + FileUtils.PadRightEx("计算过程：", 80) + FileUtils.PadRightEx("计算值：", 12);
                rslt.Add(strVerWeiStrTitle);

                var phase = PhaseTraList[i];
                var phaseAn = PhaseTraList[i + 5];

                foreach (var name in phase.WireData.WorkCdtNamesHang)
                {
                    var laod = phase.LoadList.Where(item => item.GKName == name).FirstOrDefault();
                    double verWei = laod == null ? 0 : laod.VerWei;
                    string verWeiStr = laod == null ? "" : laod.VerWeiStr;

                    var laodAn = phaseAn.LoadList.Where(item => item.GKName == name).FirstOrDefault();
                    double verWeiAn = phaseAn == null ? 0 : laodAn.VerWei;
                    string verWeiStrAn = phaseAn == null ? "" : laodAn.VerWeiStr;

                    string strVerWeiStr = FileUtils.PadRightEx(name, 26) + FileUtils.PadRightEx(verWeiStr, 80) + FileUtils.PadRightEx(verWei.ToString("0.###"), 12);
                    strVerWeiStr += FileUtils.PadRightEx(name, 26) + FileUtils.PadRightEx(verWeiStrAn, 80) + FileUtils.PadRightEx(verWeiAn.ToString("0.###"), 12);
                    rslt.Add(strVerWeiStr);
                }
            }

            rslt.Add("\n线条张力：");
            for (int i = 0; i <= 4; i++)
            {
                rslt.Add(FileUtils.PadRightEx("小号侧" + i, 118) + FileUtils.PadRightEx("大号侧" + i, 118));
                rslt.Add(FileUtils.PadRightEx("工况", 26) + FileUtils.PadRightEx("计算过程：", 80) + FileUtils.PadRightEx("计算值：", 12)
                    + FileUtils.PadRightEx("工况", 26) + FileUtils.PadRightEx("计算过程：", 80) + FileUtils.PadRightEx("计算值：", 12));

                var phase = PhaseTraList[i];
                var phaseAn = PhaseTraList[i + 5];

                foreach (var name in phase.WireData.WorkCdtNamesHang)
                {
                    var laod = phase.LoadList.Where(item => item.GKName == name).FirstOrDefault();
                    double loStr = laod == null ? 0 : laod.LoStr;
                    string loStrStr = laod == null ? "" : laod.LoStrStr;
                    string loStrCheckStr = laod == null ? "" : laod.LoStrCheckStr;

                    var laodAn = phaseAn.LoadList.Where(item => item.GKName == name).FirstOrDefault();
                    double loStrAn = phaseAn == null ? 0 : laodAn.LoStr;
                    string loStrStrAn = phaseAn == null ? "" : laodAn.LoStrStr;
                    string loStrCheckStrAn = phaseAn == null ? "" : laodAn.LoStrCheckStr;
                   
                    string str = FileUtils.PadRightEx(name, 26) + FileUtils.PadRightEx(loStrStr, 80) + FileUtils.PadRightEx(loStr.ToString("0.###"), 12)
                        + FileUtils.PadRightEx(name, 26) + FileUtils.PadRightEx(loStrStrAn, 80) + FileUtils.PadRightEx(loStrAn.ToString("0.###"), 12)
                        + FileUtils.PadRightEx(loStrCheckStr, 20) + FileUtils.PadRightEx(loStrCheckStrAn, 20);

                    if (name == "断线" || name == "不均匀冰I" || name == "不均匀冰II" || name == "断线(导线+5mm)" || name == "不均匀冰I(导线+5mm)" || name == "不均匀冰II(导线+5mm)")
                    {
                        double loStrCheck2Str = laod == null ? 0 : laod.LoStrCheck2;
                        double loStrCheck2StrAn = phaseAn == null ? 0 : laodAn.LoStrCheck2;

                        str += FileUtils.PadRightEx(loStrCheck2Str.ToString(), 10) + FileUtils.PadRightEx(loStrCheck2StrAn.ToString(), 10);
                    }

                    rslt.Add(str);
                }
            }

            return rslt;
        }


        public List<string> PrintTensionDiff()
        {
            List<string> rslt = new List<string>();

            rslt.Add(FileUtils.PadRightEx("\n断线计算：", 184) + FileUtils.PadRightEx("Max", 16) + FileUtils.PadRightEx("最大张力", 16) + FileUtils.PadRightEx("覆冰断线1", 16) + FileUtils.PadRightEx("覆冰断线2", 16));
            rslt.Add(FileUtils.PadRightEx("导线事故断线工况", 26) + FileUtils.PadRightEx(PhaseTraList[0].BreakTenDiffStr, 50) + FileUtils.PadRightEx(PhaseTraList[0].BreakTenDiff.ToString("0.##"), 16)
                + FileUtils.PadRightEx("开断情况", 92)
                + FileUtils.PadRightEx("导线", 16) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[0]).TenBreakMax.ToString("0.##"), 16) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[0]).TenBreakIceCov100.ToString("0.##"), 16)
                + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[5]).TenBreakIceCov100.ToString("0.##"), 16));
            rslt.Add(FileUtils.PadRightEx("地线事故断线工况", 26) + FileUtils.PadRightEx(PhaseTraList[3].BreakTenDiffStr, 50) + FileUtils.PadRightEx(PhaseTraList[3].BreakTenDiff.ToString("0.##"), 16)
                + FileUtils.PadRightEx("地线事故断线工况", 26) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[3]).BreakTenDiffGrdBreStr, 50) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[3]).BreakTenDiffGrdBre.ToString("0.##"), 16)
                + FileUtils.PadRightEx("地线1", 16) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[3]).TenBreakMax.ToString("0.##"), 16) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[3]).TenBreakIceCov100.ToString("0.##"), 16)
                + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[8]).TenBreakIceCov100.ToString("0.##"), 16));
            rslt.Add(FileUtils.PadRightEx("地线事故断线工况", 26) + FileUtils.PadRightEx(PhaseTraList[4].BreakTenDiffStr, 50) + FileUtils.PadRightEx(PhaseTraList[4].BreakTenDiff.ToString("0.##"), 16)
                + FileUtils.PadRightEx("地线事故断线工况", 26) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[4]).BreakTenDiffGrdBreStr, 50) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[4]).BreakTenDiffGrdBre.ToString("0.##"), 16)
                + FileUtils.PadRightEx("地线2", 16) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[4]).TenBreakMax.ToString("0.##"), 16) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[4]).TenBreakIceCov100.ToString("0.##"), 16)
                + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[9]).TenBreakIceCov100.ToString("0.##"), 16));

            rslt.Add(FileUtils.PadRightEx("\n直线塔计算：", 20) + FileUtils.PadRightEx("Max=最大张力", 48) + FileUtils.PadRightEx("Max=覆冰100%断线", 48) + FileUtils.PadRightEx("最大张力取值", 48));
            rslt.Add(FileUtils.PadRightEx(" ", 20) + FileUtils.PadRightEx("小号侧", 24) + FileUtils.PadRightEx("大号侧", 24) + FileUtils.PadRightEx("小号侧", 24) + FileUtils.PadRightEx("大号侧", 24) + FileUtils.PadRightEx("小号侧", 24) + FileUtils.PadRightEx("大号侧", 24));
            rslt.Add(FileUtils.PadRightEx("导线小号侧：", 20) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[0]).TenBreakMaxTensionS.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[0]).TenBreakMaxTensionL.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[0]).TenBreakMaxIceCov100S.ToString(), 24)
                + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[0]).TenBreakMaxIceCov100L.ToString(), 24)  + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[0]).BreakTenMaxS.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[0]).BreakTenMaxL.ToString(), 24));
            rslt.Add(FileUtils.PadRightEx("导线大号侧：", 20) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[5]).TenBreakMaxTensionS.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[5]).TenBreakMaxTensionL.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[5]).TenBreakMaxIceCov100S.ToString(), 24)
                + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[5]).TenBreakMaxIceCov100L.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[5]).BreakTenMaxS.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[5]).BreakTenMaxL.ToString(), 24));
            rslt.Add(FileUtils.PadRightEx("地线1小号侧：", 20) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[3]).TenBreakMaxTensionS.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[3]).TenBreakMaxTensionL.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[3]).TenBreakMaxIceCov100S.ToString(), 24)
                + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[3]).TenBreakMaxIceCov100L.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[3]).BreakTenMaxS.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[3]).BreakTenMaxL.ToString(), 24));
            rslt.Add(FileUtils.PadRightEx("地线1大号侧：", 20) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[8]).TenBreakMaxTensionS.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[8]).TenBreakMaxTensionL.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[8]).TenBreakMaxIceCov100S.ToString(), 24)
                + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[8]).TenBreakMaxIceCov100L.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[8]).BreakTenMaxS.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[8]).BreakTenMaxL.ToString(), 24));
            rslt.Add(FileUtils.PadRightEx("地线2小号侧：", 20) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[4]).TenBreakMaxTensionS.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[4]).TenBreakMaxTensionL.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[4]).TenBreakMaxIceCov100S.ToString(), 24)
                + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[4]).TenBreakMaxIceCov100L.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[4]).BreakTenMaxS.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[4]).BreakTenMaxL.ToString(), 24));
            rslt.Add(FileUtils.PadRightEx("地线2大号侧：", 20) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[9]).TenBreakMaxTensionS.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[9]).TenBreakMaxTensionL.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[9]).TenBreakMaxIceCov100S.ToString(), 24)
                + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[9]).TenBreakMaxIceCov100L.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[9]).BreakTenMaxS.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[9]).BreakTenMaxL.ToString(), 24));

            rslt.Add(FileUtils.PadRightEx("\n直线塔：", 20) + FileUtils.PadRightEx("最终值小号侧", 24) + FileUtils.PadRightEx("最终值大号侧", 24));
            rslt.Add(FileUtils.PadRightEx("导线小号侧：", 20) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[0]).TenBreakS.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[0]).TenBreakL.ToString(), 24));
            rslt.Add(FileUtils.PadRightEx("导线大号侧：", 20) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[5]).TenBreakS.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[5]).TenBreakL.ToString(), 24));
            rslt.Add(FileUtils.PadRightEx("地线1小号侧：", 20) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[3]).TenBreakS.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[3]).TenBreakL.ToString(), 24));
            rslt.Add(FileUtils.PadRightEx("地线1大号侧：", 20) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[8]).TenBreakS.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[8]).TenBreakL.ToString(), 24));
            rslt.Add(FileUtils.PadRightEx("地线2小号侧：", 20) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[4]).TenBreakS.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[4]).TenBreakL.ToString(), 24));
            rslt.Add(FileUtils.PadRightEx("地线2大号侧：", 20) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[9]).TenBreakS.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[9]).TenBreakL.ToString(), 24));

            rslt.Add(FileUtils.PadRightEx("\n开断：", 20) + FileUtils.PadRightEx("Max=最大张力", 48) + FileUtils.PadRightEx("Max=覆冰100%断线", 48) + FileUtils.PadRightEx("最大张力取值", 48));
            rslt.Add(FileUtils.PadRightEx(" ", 20) + FileUtils.PadRightEx("小号侧", 24) + FileUtils.PadRightEx("大号侧", 24) + FileUtils.PadRightEx("小号侧", 24) + FileUtils.PadRightEx("大号侧", 24) + FileUtils.PadRightEx("小号侧", 24) + FileUtils.PadRightEx("大号侧", 24));
            rslt.Add(FileUtils.PadRightEx("地线1小号侧：", 20) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[3]).TenBreakMaxTensionBreakS.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[3]).TenBreakMaxTensionBreakL.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[3]).TenBreakMaxIceCov100BreakS.ToString(), 24)
                + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[3]).TenBreakMaxIceCov100BreakL.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[3]).BreakTenMaxBreakS.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[3]).BreakTenMaxBreakL.ToString(), 24));
            rslt.Add(FileUtils.PadRightEx("地线1大号侧：", 20) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[8]).TenBreakMaxTensionBreakS.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[8]).TenBreakMaxTensionBreakL.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[8]).TenBreakMaxIceCov100BreakS.ToString(), 24)
                + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[8]).TenBreakMaxIceCov100BreakL.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[8]).BreakTenMaxBreakS.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[8]).BreakTenMaxBreakL.ToString(), 24));
            rslt.Add(FileUtils.PadRightEx("地线2小号侧：", 20) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[4]).TenBreakMaxTensionBreakS.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[4]).TenBreakMaxTensionBreakL.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[4]).TenBreakMaxIceCov100BreakS.ToString(), 24)
                + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[4]).TenBreakMaxIceCov100BreakL.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[4]).BreakTenMaxBreakS.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[4]).BreakTenMaxBreakL.ToString(), 24));
            rslt.Add(FileUtils.PadRightEx("地线2大号侧：", 20) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[9]).TenBreakMaxTensionBreakS.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[9]).TenBreakMaxTensionBreakL.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[9]).TenBreakMaxIceCov100BreakS.ToString(), 24)
                + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[9]).TenBreakMaxIceCov100BreakL.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[9]).BreakTenMaxBreakS.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[9]).BreakTenMaxBreakL.ToString(), 24));

            rslt.Add(FileUtils.PadRightEx("\n直线开断：", 20) + FileUtils.PadRightEx("是否加5mm", 48) + FileUtils.PadRightEx("开断最终", 48));
            rslt.Add(FileUtils.PadRightEx("地线1小号侧：", 20) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[3]).BreakTenAdd5mmBreakS.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[3]).BreakTenAdd5mmBreakL.ToString(), 24)
                + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[3]).BreakTenBreakS.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[3]).BreakTenBreakL.ToString(), 24));
            rslt.Add(FileUtils.PadRightEx("地线1大号侧：", 20) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[8]).BreakTenAdd5mmBreakS.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[8]).BreakTenAdd5mmBreakL.ToString(), 24)
                + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[8]).BreakTenBreakS.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[8]).BreakTenBreakL.ToString(), 24));
            rslt.Add(FileUtils.PadRightEx("地线2小号侧：", 20) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[4]).BreakTenAdd5mmBreakS.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[4]).BreakTenAdd5mmBreakL.ToString(), 24)
                + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[4]).BreakTenBreakS.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[4]).BreakTenBreakL.ToString(), 24));
            rslt.Add(FileUtils.PadRightEx("地线2大号侧：", 20) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[9]).BreakTenAdd5mmBreakS.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[9]).BreakTenAdd5mmBreakL.ToString(), 24)
                + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[9]).BreakTenBreakS.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[9]).BreakTenBreakL.ToString(), 24));

            rslt.Add(FileUtils.PadRightEx("\n不均匀冰计算：", 184) + FileUtils.PadRightEx("Max", 16) + FileUtils.PadRightEx("最大张力", 16) + FileUtils.PadRightEx("覆冰断线1", 16) + FileUtils.PadRightEx("覆冰断线2", 16));
            rslt.Add(FileUtils.PadRightEx("导线不均匀冰工况", 26) + FileUtils.PadRightEx(PhaseTraList[0].UnbaIceTenDiffStr, 50) + FileUtils.PadRightEx(PhaseTraList[0].UnbaIceTenDiff.ToString("0.##"), 16)
                + FileUtils.PadRightEx("开断情况", 92)
                + FileUtils.PadRightEx("导线", 16) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[0]).TenUnbaIceMax.ToString("0.##"), 16) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[0]).TenUnbaIceIceCov100.ToString("0.##"), 16)
                + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[5]).TenUnbaIceIceCov100.ToString("0.##"), 16));
            rslt.Add(FileUtils.PadRightEx("地线不均匀冰工况", 26) + FileUtils.PadRightEx(PhaseTraList[3].UnbaIceTenDiffStr, 50) + FileUtils.PadRightEx(PhaseTraList[3].UnbaIceTenDiff.ToString("0.##"), 16)
                + FileUtils.PadRightEx("地线不均匀冰工况", 26) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[3]).UnbaIceTenDiffGrdBreStr, 50) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[3]).UnbaIceTenDiffGrdBre.ToString("0.##"), 16)
                + FileUtils.PadRightEx("地线1", 16) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[3]).TenUnbaIceMax.ToString("0.##"), 16) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[3]).TenUnbaIceIceCov100.ToString("0.##"), 16)
                + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[8]).TenUnbaIceIceCov100.ToString("0.##"), 16));
            rslt.Add(FileUtils.PadRightEx("地线不均匀冰工况", 26) + FileUtils.PadRightEx(PhaseTraList[4].UnbaIceTenDiffStr, 50) + FileUtils.PadRightEx(PhaseTraList[4].UnbaIceTenDiff.ToString("0.##"), 16)
                + FileUtils.PadRightEx("地线不均匀冰工况", 26) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[4]).UnbaIceTenDiffGrdBreStr, 50) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[4]).UnbaIceTenDiffGrdBre.ToString("0.##"), 16)
                + FileUtils.PadRightEx("地线2", 16) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[4]).TenUnbaIceMax.ToString("0.##"), 16) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[4]).TenUnbaIceIceCov100.ToString("0.##"), 16)
                + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[9]).TenUnbaIceIceCov100.ToString("0.##"), 16));

            rslt.Add(FileUtils.PadRightEx("\n直线塔计算：", 20) + FileUtils.PadRightEx("Max=最大张力", 48) + FileUtils.PadRightEx("Max=覆冰100%断线", 48) + FileUtils.PadRightEx("最大张力取值", 48));
            rslt.Add(FileUtils.PadRightEx(" ", 20) + FileUtils.PadRightEx("小号侧", 24) + FileUtils.PadRightEx("大号侧", 24) + FileUtils.PadRightEx("小号侧", 24) + FileUtils.PadRightEx("大号侧", 24) + FileUtils.PadRightEx("小号侧", 24) + FileUtils.PadRightEx("大号侧", 24));
            rslt.Add(FileUtils.PadRightEx("导线小号侧：", 20) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[0]).TenUnbaIceMaxTensionS.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[0]).TenUnbaIceMaxTensionL.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[0]).TenUnbaIceMaxIceCov100S.ToString(), 24)
                + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[0]).TenUnbaIceMaxIceCov100L.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[0]).UnbaIceTenMaxS.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[0]).UnbaIceTenMaxL.ToString(), 24));
            rslt.Add(FileUtils.PadRightEx("导线大号侧：", 20) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[5]).TenUnbaIceMaxTensionS.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[5]).TenUnbaIceMaxTensionL.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[5]).TenUnbaIceMaxIceCov100S.ToString(), 24)
                + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[5]).TenUnbaIceMaxIceCov100L.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[5]).UnbaIceTenMaxS.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[5]).UnbaIceTenMaxL.ToString(), 24));
            rslt.Add(FileUtils.PadRightEx("地线1小号侧：", 20) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[3]).TenUnbaIceMaxTensionS.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[3]).TenUnbaIceMaxTensionL.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[3]).TenUnbaIceMaxIceCov100S.ToString(), 24)
                + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[3]).TenUnbaIceMaxIceCov100L.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[3]).UnbaIceTenMaxS.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[3]).UnbaIceTenMaxL.ToString(), 24));
            rslt.Add(FileUtils.PadRightEx("地线1大号侧：", 20) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[8]).TenUnbaIceMaxTensionS.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[8]).TenUnbaIceMaxTensionL.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[8]).TenUnbaIceMaxIceCov100S.ToString(), 24)
                + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[8]).TenUnbaIceMaxIceCov100L.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[8]).UnbaIceTenMaxS.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[8]).UnbaIceTenMaxL.ToString(), 24));
            rslt.Add(FileUtils.PadRightEx("地线2小号侧：", 20) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[4]).TenUnbaIceMaxTensionS.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[4]).TenUnbaIceMaxTensionL.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[4]).TenUnbaIceMaxIceCov100S.ToString(), 24)
                + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[4]).TenUnbaIceMaxIceCov100L.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[4]).UnbaIceTenMaxS.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[4]).UnbaIceTenMaxL.ToString(), 24));
            rslt.Add(FileUtils.PadRightEx("地线2大号侧：", 20) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[9]).TenUnbaIceMaxTensionS.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[9]).TenUnbaIceMaxTensionL.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[9]).TenUnbaIceMaxIceCov100S.ToString(), 24)
                + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[9]).TenUnbaIceMaxIceCov100L.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[9]).UnbaIceTenMaxS.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[9]).UnbaIceTenMaxL.ToString(), 24));

            rslt.Add(FileUtils.PadRightEx("\n直线塔：", 20) + FileUtils.PadRightEx("最终值小号侧", 24) + FileUtils.PadRightEx("最终值大号侧", 24));
            rslt.Add(FileUtils.PadRightEx("导线小号侧：", 20) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[0]).TenUnbaIceS.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[0]).TenUnbaIceL.ToString(), 24));
            rslt.Add(FileUtils.PadRightEx("导线大号侧：", 20) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[5]).TenUnbaIceS.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[5]).TenUnbaIceL.ToString(), 24));
            rslt.Add(FileUtils.PadRightEx("地线1小号侧：", 20) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[3]).TenUnbaIceS.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[3]).TenUnbaIceL.ToString(), 24));
            rslt.Add(FileUtils.PadRightEx("地线1大号侧：", 20) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[8]).TenUnbaIceS.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[8]).TenUnbaIceL.ToString(), 24));
            rslt.Add(FileUtils.PadRightEx("地线2小号侧：", 20) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[4]).TenUnbaIceS.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[4]).TenUnbaIceL.ToString(), 24));
            rslt.Add(FileUtils.PadRightEx("地线2大号侧：", 20) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[9]).TenUnbaIceS.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[9]).TenUnbaIceL.ToString(), 24));

            rslt.Add(FileUtils.PadRightEx("\n开断：", 20) + FileUtils.PadRightEx("Max=最大张力", 48) + FileUtils.PadRightEx("Max=覆冰100%断线", 48) + FileUtils.PadRightEx("最大张力取值", 48));
            rslt.Add(FileUtils.PadRightEx(" ", 20) + FileUtils.PadRightEx("小号侧", 24) + FileUtils.PadRightEx("大号侧", 24) + FileUtils.PadRightEx("小号侧", 24) + FileUtils.PadRightEx("大号侧", 24) + FileUtils.PadRightEx("小号侧", 24) + FileUtils.PadRightEx("大号侧", 24));
            rslt.Add(FileUtils.PadRightEx("地线1小号侧：", 20) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[3]).TenUnbaIceMaxTensionBreakS.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[3]).TenUnbaIceMaxTensionBreakL.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[3]).TenUnbaIceMaxIceCov100BreakS.ToString(), 24)
                + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[3]).TenUnbaIceMaxIceCov100BreakL.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[3]).UnbaIceTenMaxBreakS.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[3]).UnbaIceTenMaxBreakL.ToString(), 24));
            rslt.Add(FileUtils.PadRightEx("地线1大号侧：", 20) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[8]).TenUnbaIceMaxTensionBreakS.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[8]).TenUnbaIceMaxTensionBreakL.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[8]).TenUnbaIceMaxIceCov100BreakS.ToString(), 24)
                + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[8]).TenUnbaIceMaxIceCov100BreakL.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[8]).UnbaIceTenMaxBreakS.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[8]).UnbaIceTenMaxBreakL.ToString(), 24));
            rslt.Add(FileUtils.PadRightEx("地线2小号侧：", 20) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[4]).TenUnbaIceMaxTensionBreakS.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[4]).TenUnbaIceMaxTensionBreakL.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[4]).TenUnbaIceMaxIceCov100BreakS.ToString(), 24)
                + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[4]).TenUnbaIceMaxIceCov100BreakL.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[4]).UnbaIceTenMaxBreakS.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[4]).UnbaIceTenMaxBreakL.ToString(), 24));
            rslt.Add(FileUtils.PadRightEx("地线2大号侧：", 20) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[9]).TenUnbaIceMaxTensionBreakS.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[9]).TenUnbaIceMaxTensionBreakL.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[9]).TenUnbaIceMaxIceCov100BreakS.ToString(), 24)
                + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[9]).TenUnbaIceMaxIceCov100BreakL.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[9]).UnbaIceTenMaxBreakS.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[9]).UnbaIceTenMaxBreakL.ToString(), 24));


            rslt.Add(FileUtils.PadRightEx("\n直线开断：", 20) + FileUtils.PadRightEx("是否加5mm", 48) + FileUtils.PadRightEx("开断最终", 48));
            rslt.Add(FileUtils.PadRightEx("地线1小号侧：", 20) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[3]).UnbaIceTenAdd5mmBreakS.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[3]).UnbaIceTenAdd5mmBreakL.ToString(), 24) 
                + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[3]).UnbaIceTenBreakS.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[3]).UnbaIceTenBreakL.ToString(), 24));
            rslt.Add(FileUtils.PadRightEx("地线1大号侧：", 20) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[8]).UnbaIceTenAdd5mmBreakS.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[8]).UnbaIceTenAdd5mmBreakL.ToString(), 24)
                + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[8]).UnbaIceTenBreakS.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[8]).UnbaIceTenBreakL.ToString(), 24));
            rslt.Add(FileUtils.PadRightEx("地线2小号侧：", 20) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[4]).UnbaIceTenAdd5mmBreakS.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[4]).UnbaIceTenAdd5mmBreakL.ToString(), 24)
                + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[4]).UnbaIceTenBreakS.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[4]).UnbaIceTenBreakL.ToString(), 24));
            rslt.Add(FileUtils.PadRightEx("地线2大号侧：", 20) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[9]).UnbaIceTenAdd5mmBreakS.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[9]).UnbaIceTenAdd5mmBreakL.ToString(), 24)
                + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[9]).UnbaIceTenBreakS.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[9]).UnbaIceTenBreakL.ToString(), 24));

            return rslt;
        }
    }
}
