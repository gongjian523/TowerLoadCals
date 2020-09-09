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
                ElecCalsPhaseStrHang smallSize = (ElecCalsPhaseStrHang)PhaseTraList[i];
                ElecCalsPhaseStrHang bigSize = (ElecCalsPhaseStrHang)PhaseTraList[i+5];

                //断线计算 
                //直线塔计算项
                //Max=最大张力
                smallSize.TenBreakMaxTension = Math.Round(smallSize.TenBreakMax, 0) - Math.Round(smallSize.BreakTenDiff, 0);
                bigSize.TenBreakMaxTension = Math.Round(bigSize.TenBreakMax, 0);
                //Max=覆冰100%断线工况
                if(smallSize.WireData.bGrd == 0)
                {
                    smallSize.TenBreakMaxIceCov100 = Math.Round(smallSize.TenBreakIceCov100, 0) - Math.Round(smallSize.BreakTenDiff, 0);
                    bigSize.TenBreakMaxIceCov100 = Math.Round(bigSize.TenBreakIceCov100, 0);
                }
                else
                {
                    double temp = Math.Round(smallSize.TenBreakIceCov100, 0) - Math.Round(smallSize.BreakTenDiff, 0);
                    smallSize.TenBreakMaxIceCov100 = temp < 0 ? 0 : temp;
                    bigSize.TenBreakMaxIceCov100 = smallSize.TenBreakMaxIceCov100 + Math.Round(bigSize.BreakTenDiff, 0);
                }
                //最大张力取值
                smallSize.BreakTenMax = smallSize.WireData.CommParas.BreakMaxPara == 1 ? smallSize.TenBreakMaxTension:smallSize.TenBreakMaxIceCov100; 
                bigSize.BreakTenMax = bigSize.WireData.CommParas.BreakMaxPara == 1 ? bigSize.TenBreakMaxTension : bigSize.TenBreakMaxIceCov100;
                //直线塔
                //O与张力差(在excel中有一个与是否增加5mm的比较过程，但是里面没有填写任何值，故计算过程简化)
                smallSize.TenBreak = smallSize.WireData.CommParas.BreakInPara == 1 ? 0 : smallSize.BreakTenMax;
                bigSize.TenBreak = bigSize.WireData.CommParas.BreakInPara == 1 ? Math.Round(bigSize.BreakTenDiff, 0) : bigSize.BreakTenMax;

                //开断
                if(smallSize.WireData.bGrd != 0)
                {
                    //直线塔计算项
                    //Max=最大张力
                    smallSize.TenBreakMaxTensionBreak = Math.Round(smallSize.TenBreakMaxBreak, 0) - Math.Round(smallSize.BreakTenDiff, 0);
                    bigSize.TenBreakMaxTensionBreak = Math.Round(bigSize.TenBreakMaxBreak, 0);

                    //Excel中写的是大号塔
                    double temp = Math.Round(bigSize.TenBreakIceCov100, 0) - Math.Round(smallSize.BreakTenDiff, 0);
                    smallSize.TenBreakMaxIceCov100Break = temp < 0 ? 0 : temp;
                    bigSize.TenBreakMaxIceCov100Break = smallSize.TenBreakMaxIceCov100 + Math.Round(bigSize.BreakTenDiff, 0);


                }




                //不均匀冰计算 
                //直线塔计算项
                //Max=最大张力
                smallSize.TenUnbaIceMaxTension = Math.Round(smallSize.TenUnbaIceMax, 0) - Math.Round(smallSize.UnbaIceTenDiff, 0);
                bigSize.TenUnbaIceMaxTension = Math.Round(bigSize.TenUnbaIceMax, 0);
                //Max=覆冰100%断线工况
                if (smallSize.WireData.bGrd == 0)
                {
                    smallSize.TenUnbaIceMaxIceCov100 = Math.Round(smallSize.TenUnbaIceIceCov100, 0) - Math.Round(smallSize.UnbaIceTenDiff, 0);
                    bigSize.TenUnbaIceMaxIceCov100 = Math.Round(bigSize.TenUnbaIceIceCov100, 0);
                }
                else
                {
                    double temp = Math.Round(smallSize.TenUnbaIceIceCov100, 0) - Math.Round(smallSize.UnbaIceTenDiff, 0);
                    smallSize.TenUnbaIceMaxIceCov100 = temp < 0 ? 0 : temp;
                    bigSize.TenUnbaIceMaxIceCov100 = smallSize.TenUnbaIceMaxIceCov100 + Math.Round(bigSize.UnbaIceTenDiff, 0);
                }
                //最大张力取值
                smallSize.UnbaIceTenMax = smallSize.WireData.CommParas.UnbaMaxPara == 1 ? smallSize.TenUnbaIceMaxTension : smallSize.TenUnbaIceMaxIceCov100;
                bigSize.UnbaIceTenMax = bigSize.WireData.CommParas.UnbaMaxPara == 1 ? bigSize.TenUnbaIceMaxTension : bigSize.TenUnbaIceMaxIceCov100;

                //直线塔
                //O与张力差(在excel中有一个与是否增加5mm的比较过程，但是里面没有填写任何值，故计算过程简化)
                smallSize.TenUnbaIce = smallSize.WireData.CommParas.UnbaInPara == 1 ? 0 : smallSize.UnbaIceTenMax;
                bigSize.TenUnbaIce = bigSize.WireData.CommParas.UnbaInPara == 1 ? Math.Round(smallSize.UnbaIceTenDiff, 0) : bigSize.UnbaIceTenMax;

                if (smallSize.WireData.bGrd != 0)
                {
                    //直线塔计算项
                    //Max=最大张力
                    smallSize.TenUnbaIceMaxTensionBreak = Math.Round(smallSize.TenUnbaIceMaxBreak, 0) - Math.Round(smallSize.UnbaIceTenDiff, 0);
                    bigSize.TenUnbaIceMaxTensionBreak = Math.Round(bigSize.TenUnbaIceMaxBreak, 0);

                    //Excel中写的是大号塔
                    double temp = Math.Round(bigSize.TenUnbaIceIceCov100, 0) - Math.Round(smallSize.UnbaIceTenDiff, 0);
                    smallSize.TenUnbaIceMaxIceCov100Break = temp < 0 ? 0 : temp;
                    bigSize.TenUnbaIceMaxIceCov100Break = smallSize.TenUnbaIceMaxIceCov100 + Math.Round(bigSize.UnbaIceTenDiff, 0);
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
                + FileUtils.PadRightEx("导线", 16) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[0]).BreakTenMax.ToString("0.##"), 16) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[0]).TenBreakIceCov100.ToString("0.##"), 16)
                + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[5]).TenBreakIceCov100.ToString("0.##"), 16));
            rslt.Add(FileUtils.PadRightEx("地线事故断线工况", 26) + FileUtils.PadRightEx(PhaseTraList[3].BreakTenDiffStr, 50) + FileUtils.PadRightEx(PhaseTraList[3].BreakTenDiff.ToString("0.##"), 16)
                + FileUtils.PadRightEx("地线事故断线工况", 26) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[3]).BreakTenDiffGrdBreStr, 50) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[3]).BreakTenDiffGrdBre.ToString("0.##"), 16)
                + FileUtils.PadRightEx("地线1", 16) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[3]).BreakTenMax.ToString("0.##"), 16) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[3]).TenBreakIceCov100.ToString("0.##"), 16)
                + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[8]).TenBreakIceCov100.ToString("0.##"), 16));
            rslt.Add(FileUtils.PadRightEx("地线事故断线工况", 26) + FileUtils.PadRightEx(PhaseTraList[4].BreakTenDiffStr, 50) + FileUtils.PadRightEx(PhaseTraList[4].BreakTenDiff.ToString("0.##"), 16)
                + FileUtils.PadRightEx("地线事故断线工况", 26) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[4]).BreakTenDiffGrdBreStr, 50) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[4]).BreakTenDiffGrdBre.ToString("0.##"), 16)
                + FileUtils.PadRightEx("地线2", 16) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[4]).BreakTenMax.ToString("0.##"), 16) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[4]).TenBreakIceCov100.ToString("0.##"), 16)
                + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[9]).TenBreakIceCov100.ToString("0.##"), 16));

            rslt.Add(FileUtils.PadRightEx("\n直线塔计算：", 20) + FileUtils.PadRightEx("Max=最大张力", 24) + FileUtils.PadRightEx("Max=覆冰100%断线", 24) + FileUtils.PadRightEx("最大张力取值", 24));
            rslt.Add(FileUtils.PadRightEx("导线小号侧：", 20) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[0]).TenBreakMaxTension.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[0]).TenBreakMaxIceCov100.ToString(), 24) 
                + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[0]).BreakTenMax.ToString(), 24));
            rslt.Add(FileUtils.PadRightEx("导线大号侧：", 20) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[5]).TenBreakMaxTension.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[5]).TenBreakMaxIceCov100.ToString(), 24)
                + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[5]).BreakTenMax.ToString(), 24));
            rslt.Add(FileUtils.PadRightEx("地线1小号侧：", 20) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[3]).TenBreakMaxTension.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[3]).TenBreakMaxIceCov100.ToString(), 24)
                + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[3]).BreakTenMax.ToString(), 24));
            rslt.Add(FileUtils.PadRightEx("地线1大号侧：", 20) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[8]).TenBreakMaxTension.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[8]).TenBreakMaxIceCov100.ToString(), 24)
                + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[8]).BreakTenMax.ToString(), 24));
            rslt.Add(FileUtils.PadRightEx("地线2小号侧：", 20) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[4]).TenBreakMaxTension.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[4]).TenBreakMaxIceCov100.ToString(), 24)
                + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[4]).BreakTenMax.ToString(), 24));
            rslt.Add(FileUtils.PadRightEx("地线2大号侧：", 20) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[9]).TenBreakMaxTension.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[9]).TenBreakMaxIceCov100.ToString(), 24)
                + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[9]).BreakTenMax.ToString(), 24));

            rslt.Add(FileUtils.PadRightEx("\n直线塔：", 20) + FileUtils.PadRightEx("最终值", 24));
            rslt.Add(FileUtils.PadRightEx("导线小号侧：", 20) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[0]).TenBreak.ToString(), 24));
            rslt.Add(FileUtils.PadRightEx("导线大号侧：", 20) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[5]).TenBreak.ToString(), 24));
            rslt.Add(FileUtils.PadRightEx("地线1小号侧：", 20) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[3]).TenBreak.ToString(), 24));
            rslt.Add(FileUtils.PadRightEx("地线1大号侧：", 20) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[8]).TenBreak.ToString(), 24));
            rslt.Add(FileUtils.PadRightEx("地线2小号侧：", 20) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[4]).TenBreak.ToString(), 24));
            rslt.Add(FileUtils.PadRightEx("地线2大号侧：", 20) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[9]).TenBreak.ToString(), 24));

            rslt.Add(FileUtils.PadRightEx("\n开断：", 20) + FileUtils.PadRightEx("Max=最大张力", 24) + FileUtils.PadRightEx("Max=覆冰100%断线", 24) + FileUtils.PadRightEx("最大张力取值", 24));
            rslt.Add(FileUtils.PadRightEx("地线1小号侧：", 20) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[3]).TenBreakMaxTensionBreak.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[3]).TenBreakMaxIceCov100Break.ToString(), 24)
                + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[3]).BreakTenMaxBreak.ToString(), 24));
            rslt.Add(FileUtils.PadRightEx("地线1大号侧：", 20) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[8]).TenBreakMaxTensionBreak.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[8]).TenBreakMaxIceCov100Break.ToString(), 24)
                + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[8]).BreakTenMax.ToString(), 24));
            rslt.Add(FileUtils.PadRightEx("地线2小号侧：", 20) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[4]).TenBreakMaxTensionBreak.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[4]).TenBreakMaxIceCov100Break.ToString(), 24)
                + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[4]).BreakTenMax.ToString(), 24));
            rslt.Add(FileUtils.PadRightEx("地线2大号侧：", 20) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[9]).TenBreakMaxTensionBreak.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[9]).TenBreakMaxIceCov100Break.ToString(), 24)
                + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[9]).BreakTenMax.ToString(), 24));

            rslt.Add(FileUtils.PadRightEx("\n不均匀冰计算：", 184) + FileUtils.PadRightEx("Max", 16) + FileUtils.PadRightEx("最大张力", 16) + FileUtils.PadRightEx("覆冰断线1", 16) + FileUtils.PadRightEx("覆冰断线2", 16));
            rslt.Add(FileUtils.PadRightEx("导线不均匀冰工况", 26) + FileUtils.PadRightEx(PhaseTraList[0].UnbaIceTenDiffStr, 50) + FileUtils.PadRightEx(PhaseTraList[0].UnbaIceTenDiff.ToString("0.##"), 16)
                + FileUtils.PadRightEx("开断情况", 92)
                + FileUtils.PadRightEx("导线", 16) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[0]).UnbaIceTenMax.ToString("0.##"), 16) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[0]).TenUnbaIceIceCov100.ToString("0.##"), 16)
                + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[5]).TenUnbaIceIceCov100.ToString("0.##"), 16));
            rslt.Add(FileUtils.PadRightEx("地线不均匀冰工况", 26) + FileUtils.PadRightEx(PhaseTraList[3].UnbaIceTenDiffStr, 50) + FileUtils.PadRightEx(PhaseTraList[3].UnbaIceTenDiff.ToString("0.##"), 16)
                + FileUtils.PadRightEx("地线不均匀冰工况", 26) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[3]).UnbaIceTenDiffGrdBreStr, 50) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[3]).UnbaIceTenDiffGrdBre.ToString("0.##"), 16)
                + FileUtils.PadRightEx("地线1", 16) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[3]).UnbaIceTenMax.ToString("0.##"), 16) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[3]).TenUnbaIceIceCov100.ToString("0.##"), 16)
                + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[8]).TenUnbaIceIceCov100.ToString("0.##"), 16));
            rslt.Add(FileUtils.PadRightEx("地线不均匀冰工况", 26) + FileUtils.PadRightEx(PhaseTraList[4].UnbaIceTenDiffStr, 50) + FileUtils.PadRightEx(PhaseTraList[4].UnbaIceTenDiff.ToString("0.##"), 16)
                + FileUtils.PadRightEx("地线不均匀冰工况", 26) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[4]).UnbaIceTenDiffGrdBreStr, 50) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[4]).UnbaIceTenDiffGrdBre.ToString("0.##"), 16)
                + FileUtils.PadRightEx("地线2", 16) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[4]).UnbaIceTenMax.ToString("0.##"), 16) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[4]).TenUnbaIceIceCov100.ToString("0.##"), 16)
                + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[9]).TenUnbaIceIceCov100.ToString("0.##"), 16));

            rslt.Add(FileUtils.PadRightEx("\n直线塔计算：", 20) + FileUtils.PadRightEx("Max=最大张力", 24) + FileUtils.PadRightEx("Max=覆冰100%断线", 24) + FileUtils.PadRightEx("最大张力取值", 24));
            rslt.Add(FileUtils.PadRightEx("导线小号侧：", 20) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[0]).TenUnbaIceMaxTension.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[0]).TenUnbaIceMaxIceCov100.ToString(), 24)
                + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[0]).UnbaIceTenMax.ToString(), 24));
            rslt.Add(FileUtils.PadRightEx("导线大号侧：", 20) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[5]).TenUnbaIceMaxTension.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[5]).TenUnbaIceMaxIceCov100.ToString(), 24)
                + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[5]).UnbaIceTenMax.ToString(), 24));
            rslt.Add(FileUtils.PadRightEx("地线1小号侧：", 20) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[3]).TenUnbaIceMaxTension.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[3]).TenUnbaIceMaxIceCov100.ToString(), 24)
                + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[3]).UnbaIceTenMax.ToString(), 24));
            rslt.Add(FileUtils.PadRightEx("地线1大号侧：", 20) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[8]).TenUnbaIceMaxTension.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[8]).TenUnbaIceMaxIceCov100.ToString(), 24)
                + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[8]).UnbaIceTenMax.ToString(), 24));
            rslt.Add(FileUtils.PadRightEx("地线2小号侧：", 20) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[4]).TenUnbaIceMaxTension.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[4]).TenUnbaIceMaxIceCov100.ToString(), 24)
                + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[4]).UnbaIceTenMax.ToString(), 24));
            rslt.Add(FileUtils.PadRightEx("地线2大号侧：", 20) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[9]).TenUnbaIceMaxTension.ToString(), 24) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[9]).TenUnbaIceMaxIceCov100.ToString(), 24)
                + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[9]).UnbaIceTenMax.ToString(), 24));

            rslt.Add(FileUtils.PadRightEx("\n直线塔：", 20) + FileUtils.PadRightEx("最终值", 24));
            rslt.Add(FileUtils.PadRightEx("导线小号侧：", 20) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[0]).TenUnbaIce.ToString(), 24));
            rslt.Add(FileUtils.PadRightEx("导线大号侧：", 20) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[5]).TenUnbaIce.ToString(), 24));
            rslt.Add(FileUtils.PadRightEx("地线1小号侧：", 20) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[3]).TenUnbaIce.ToString(), 24));
            rslt.Add(FileUtils.PadRightEx("地线1大号侧：", 20) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[8]).TenUnbaIce.ToString(), 24));
            rslt.Add(FileUtils.PadRightEx("地线2小号侧：", 20) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[4]).TenUnbaIce.ToString(), 24));
            rslt.Add(FileUtils.PadRightEx("地线2大号侧：", 20) + FileUtils.PadRightEx(((ElecCalsPhaseStrHang)PhaseTraList[9]).TenUnbaIce.ToString(), 24));

            return rslt;
        }
    }
}
