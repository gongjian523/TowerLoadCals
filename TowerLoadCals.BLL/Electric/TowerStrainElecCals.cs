using System;
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
        /// 更新风压系数
        /// </summary>
        public void UpdateWindPara()
        {
            PhaseTraList[0].WireWindPara = CalsWireWindPara(FrontSideRes.CommParas.WireWindPara, AbsUpSideHei, BackIndWindHc, FrontSideRes.CommParas.IndAveHei);
            PhaseTraList[0].StrWindPara = CalsStrWindPara(AbsUpSideHei, FrontSideRes.CommParas.IndAveHei);
            PhaseTraList[0].JmWindPara = CalsJumpStrWindPara(FrontSideRes.CommParas.JmpWindPara, AbsUpJumHei, FrontSideRes.CommParas.IndAveHei, FrontSideRes.CommParas.JumpStrLen);
            PhaseTraList[0].PropUpWindPara = CalsPropUpWindPara(FrontSideRes.CommParas.JmpWindPara, AbsUpJumHei, FrontSideRes.CommParas.IndAveHei, FrontSideRes.CommParas.JumpStrLen);

            PhaseTraList[1].WireWindPara = CalsWireWindPara(FrontSideRes.CommParas.WireWindPara, AbsMidHei, BackIndWindHc, FrontSideRes.CommParas.IndAveHei);
            PhaseTraList[1].StrWindPara = CalsStrWindPara(AbsMidHei, FrontSideRes.CommParas.IndAveHei);
            PhaseTraList[1].JmWindPara = CalsJumpStrWindPara(FrontSideRes.CommParas.JmpWindPara, AbsMidJumHei, FrontSideRes.CommParas.IndAveHei, FrontSideRes.CommParas.JumpStrLen);
            PhaseTraList[1].PropUpWindPara = CalsPropUpWindPara(FrontSideRes.CommParas.JmpWindPara, AbsMidJumHei, FrontSideRes.CommParas.IndAveHei, FrontSideRes.CommParas.JumpStrLen);

            PhaseTraList[2].WireWindPara = CalsWireWindPara(FrontSideRes.CommParas.WireWindPara, AbsDownSideHei, BackIndWindHc, FrontSideRes.CommParas.IndAveHei);
            PhaseTraList[2].StrWindPara = CalsStrWindPara(AbsDownSideHei, FrontSideRes.CommParas.IndAveHei);
            PhaseTraList[2].JmWindPara = CalsJumpStrWindPara(FrontSideRes.CommParas.JmpWindPara, AbsDownJumHei, FrontSideRes.CommParas.IndAveHei, FrontSideRes.CommParas.JumpStrLen);
            PhaseTraList[2].PropUpWindPara = CalsPropUpWindPara(FrontSideRes.CommParas.JmpWindPara, AbsDownJumHei, FrontSideRes.CommParas.IndAveHei, FrontSideRes.CommParas.JumpStrLen);

            PhaseTraList[3].WireWindPara = CalsWireWindPara(FrontSideRes.CommParas.WireWindPara, AbsGrdHei, BackGrdWindHc, FrontSideRes.CommParas.GrdAveHei);
            PhaseTraList[3].StrWindPara = CalsStrWindPara(AbsGrdHei, FrontSideRes.CommParas.GrdAveHei);

            PhaseTraList[4].WireWindPara = CalsWireWindPara(FrontSideRes.CommParas.WireWindPara, AbsGrdHei, BackGrdWindHc, FrontSideRes.CommParas.GrdAveHei);
            PhaseTraList[4].StrWindPara = CalsStrWindPara(AbsGrdHei, FrontSideRes.CommParas.GrdAveHei);

            PhaseTraList[5].WireWindPara = CalsWireWindPara(BackSideRes.CommParas.WireWindPara, AbsUpSideHei, FrontIndWindHc, BackSideRes.CommParas.IndAveHei);
            PhaseTraList[5].StrWindPara = CalsStrWindPara(AbsUpSideHei, BackSideRes.CommParas.IndAveHei);
            PhaseTraList[5].JmWindPara = CalsJumpStrWindPara(BackSideRes.CommParas.JmpWindPara, AbsUpJumHei, BackSideRes.CommParas.IndAveHei, BackSideRes.CommParas.JumpStrLen);
            PhaseTraList[5].PropUpWindPara = CalsPropUpWindPara(BackSideRes.CommParas.JmpWindPara, AbsUpJumHei, BackSideRes.CommParas.IndAveHei, BackSideRes.CommParas.JumpStrLen);

            PhaseTraList[6].WireWindPara = CalsWireWindPara(BackSideRes.CommParas.WireWindPara, AbsMidHei, FrontIndWindHc, BackSideRes.CommParas.IndAveHei);
            PhaseTraList[6].StrWindPara = CalsStrWindPara(AbsMidHei, BackSideRes.CommParas.IndAveHei);
            PhaseTraList[6].JmWindPara = CalsJumpStrWindPara(BackSideRes.CommParas.JmpWindPara, AbsMidJumHei, BackSideRes.CommParas.IndAveHei, BackSideRes.CommParas.JumpStrLen);
            PhaseTraList[6].PropUpWindPara = CalsPropUpWindPara(BackSideRes.CommParas.JmpWindPara, AbsMidJumHei, BackSideRes.CommParas.IndAveHei, BackSideRes.CommParas.JumpStrLen);

            PhaseTraList[7].WireWindPara = CalsWireWindPara(BackSideRes.CommParas.WireWindPara, AbsDownSideHei, FrontIndWindHc, BackSideRes.CommParas.IndAveHei);
            PhaseTraList[7].StrWindPara = CalsStrWindPara(AbsDownSideHei, BackSideRes.CommParas.IndAveHei);
            PhaseTraList[7].JmWindPara = CalsJumpStrWindPara(BackSideRes.CommParas.JmpWindPara, AbsDownJumHei, BackSideRes.CommParas.IndAveHei, BackSideRes.CommParas.JumpStrLen);
            PhaseTraList[7].PropUpWindPara = CalsPropUpWindPara(BackSideRes.CommParas.JmpWindPara, AbsDownJumHei, BackSideRes.CommParas.IndAveHei, BackSideRes.CommParas.JumpStrLen);

            PhaseTraList[8].WireWindPara = CalsWireWindPara(BackSideRes.CommParas.WireWindPara, AbsGrdHei, FrontGrdWindHc, BackSideRes.CommParas.GrdAveHei);
            PhaseTraList[8].StrWindPara = CalsStrWindPara(AbsGrdHei, BackSideRes.CommParas.GrdAveHei);

            PhaseTraList[9].WireWindPara = CalsWireWindPara(BackSideRes.CommParas.WireWindPara, AbsGrdHei, FrontGrdWindHc, BackSideRes.CommParas.GrdAveHei);
            PhaseTraList[9].StrWindPara = CalsStrWindPara(AbsGrdHei, BackSideRes.CommParas.GrdAveHei);
        }


        /// <summary>
        /// 计算线高空风压系数
        /// </summary>
        /// <param name="wireWindPara">计算方式 1：线平均高 2:按照下相挂点高反算 </param>
        /// <param name="wireHei">导线挂点高</param>
        /// <param name="wireWindVerSag">大风垂直方向弧垂</param>
        /// <param name="avaHei">导线计算平均高</param>
        /// <returns></returns>
        public double CalsWireWindPara(int wireWindPara, double wireHei, double wireWindVerSag, double avaHei)
        {
            //1：线平均高 2:按照下相挂点高反算
            if (wireWindPara == 1)
            {
                return ((double)(int)(Math.Pow(wireHei / avaHei, 0.32) * 1000 + 0.5)) / 1000;
            }
            else
            {
                return ((double)(int)(Math.Pow((wireHei - 2d / 3 * wireWindVerSag) / avaHei, 0.32) * 1000 + 0.5)) / 1000;
            }
        }

        /// <summary>
        /// 计算绝缘子串高空风压系数
        /// </summary>
        /// <param name="wireHei">挂点高</param>
        /// <param name="avaHei">导线计算平均高</param>
        /// <returns></returns>
        protected double CalsStrWindPara(double wireHei, double avaHei)
        {
            return Math.Round(Math.Pow((wireHei / avaHei), 0.32), 3);
        }


        /// <summary>
        /// 计算跳串高空风压系数
        /// </summary>
        /// <param name="wireWindPara">跳串高空风压系数</param>
        /// <param name="strHei"></param>
        /// <param name="avaHei">导线计算平均高</param>
        /// <param name="jumpStrLen">跳线绝缘子串长</param>
        /// <returns></returns>
        protected double CalsJumpStrWindPara(int jumpWindPara, double strHei, double avaHei, double jumpStrLen)
        {
            //1：挂线高，2：按照跳线中点高度，硬跳线按照实际高度
            if (jumpWindPara == 1)
            {
                //按挂点高
                return Math.Round(Math.Pow(strHei / avaHei, 0.32), 3);
            }
            else
            {
                //按平均高
                return Math.Round(Math.Pow((strHei - jumpStrLen / 2) / avaHei, 0.32), 3);
            }
        }

        /// <summary>
        /// 计算支撑管高空风压系数
        /// </summary>
        /// <param name="wireWindPara">跳串高空风压系数</param>
        /// <param name="strHei"></param>
        /// <param name="avaHei">导线计算平均高</param>
        /// <param name="jumpStrLen">跳线绝缘子串长</param>
        /// <returns></returns>
        protected double CalsPropUpWindPara(int jumpWindPara, double strHei, double avaHei, double jumpStrLen)
        {
            //1：挂线高，2：按照跳线中点高度，硬跳线按照实际高度
            if (jumpWindPara == 1)
            {
                //按挂点高
                return Math.Round(Math.Pow(strHei / avaHei, 0.32), 3);
            }
            else
            {
                //按平均高
                return Math.Round(Math.Pow((strHei - jumpStrLen) / avaHei, 0.32), 3);
            }
        }



        /// <summary>
        /// 计算各个工况的垂直档距,，耐张塔分为前后侧计算
        /// </summary>
        public void Cals()
        {
            for(int i = 0; i <= 4; i++)
            {
                PhaseTraList[i].UpdateVertialSpan();

                var spanFit = BackSideRes.SpanFit;

                double diaInc, weiInc, secInc, weiFZC, insErrorPara, constrErrorPara, wireExtend;
                int numFZC;

                if (PhaseTraList[i].WireData.bGrd == 0){
                    diaInc = BackSideRes.CommParas.DiaIndInc;
                    weiInc = BackSideRes.CommParas.WeiIndInc;
                    numFZC = spanFit.NumInFZC;
                    weiFZC = spanFit.WeiInFZC;
                    secInc = BackSideRes.CommParas.SecIndInc;
                }
                else if (PhaseTraList[i].WireData.bGrd == 1){
                    diaInc = BackSideRes.CommParas.DiaGrdInc;
                    weiInc = BackSideRes.CommParas.WeiGrdInc;
                    numFZC = spanFit.NumGrFZC;
                    weiFZC = spanFit.WeiGrFZC;
                    secInc = BackSideRes.CommParas.SecGrdInc;
                }
                else
                {
                    diaInc = BackSideRes.CommParas.DiaOPGWInc;
                    weiInc = BackSideRes.CommParas.WeiOPGWInc;
                    numFZC = spanFit.NumGrFZC;
                    weiFZC = spanFit.WeiGrFZC;
                    secInc = BackSideRes.CommParas.SecOPGWInc;
                }

                insErrorPara = FrontSideRes.SideParas.InsErrorPara;
                constrErrorPara  = FrontSideRes.SideParas.ConstruErrorPara;
                wireExtend = PhaseTraList[i].WireData.bGrd == 0 ? BackSideRes.SideParas.IndExtendPara : BackSideRes.SideParas.GrdExtendPara;

                PhaseTraList[i].UpdateHoriLoad(diaInc);
                PhaseTraList[i].UpdateVerLoad(weiInc, spanFit.NumJGB, spanFit.WeiJGB, numFZC, weiFZC);
                PhaseTraList[i].UpdateTension(secInc, constrErrorPara, insErrorPara, wireExtend);

                if(PhaseTraList[i].WireData.bGrd == 0)
                {
                    PhaseTraList[i].UpdateJumpStrHorLoad();
                    PhaseTraList[i].UpdateJumpStrVerLoad(spanFit.WeiJGB, PhaseTraList[i+5].JmWireData.WeatherParas.WeathComm);
                }
            }

            for (int i = 5; i <= 9; i++)
            {
                PhaseTraList[i].UpdateVertialSpan();

                var spanFit = FrontSideRes.SpanFit;

                double diaInc, weiInc, secInc, weiFZC, insErrorPara, constrErrorPara, wireExtend;
                int numFZC;

                if (PhaseTraList[i].WireData.bGrd == 0)
                {
                    diaInc = FrontSideRes.CommParas.DiaIndInc;
                    weiInc = FrontSideRes.CommParas.WeiIndInc;
                    numFZC = spanFit.NumInFZC;
                    weiFZC = spanFit.WeiInFZC;
                    secInc = FrontSideRes.CommParas.SecIndInc;
                }
                else if (PhaseTraList[i].WireData.bGrd == 1)
                {
                    diaInc = FrontSideRes.CommParas.DiaGrdInc;
                    weiInc = FrontSideRes.CommParas.WeiGrdInc;
                    numFZC = spanFit.NumGrFZC;
                    weiFZC = spanFit.WeiGrFZC;
                    secInc = FrontSideRes.CommParas.SecGrdInc;
                }
                else
                {
                    diaInc = FrontSideRes.CommParas.DiaOPGWInc;
                    weiInc = FrontSideRes.CommParas.WeiOPGWInc;
                    numFZC = spanFit.NumGrFZC;
                    weiFZC = spanFit.WeiGrFZC;
                    secInc = FrontSideRes.CommParas.SecOPGWInc;
                }

                insErrorPara = FrontSideRes.SideParas.InsErrorPara;
                constrErrorPara = FrontSideRes.SideParas.ConstruErrorPara;
                wireExtend = PhaseTraList[i].WireData.bGrd == 0 ? BackSideRes.SideParas.IndExtendPara : BackSideRes.SideParas.GrdExtendPara;

                PhaseTraList[i].UpdateHoriLoad(diaInc);
                PhaseTraList[i].UpdateVerLoad(weiInc, spanFit.NumJGB, spanFit.WeiJGB, numFZC, weiFZC);
                PhaseTraList[i].UpdateTension(secInc, constrErrorPara, insErrorPara, wireExtend);

                if (PhaseTraList[i].WireData.bGrd == 0)
                {
                    PhaseTraList[i].UpdateJumpStrHorLoad();
                    PhaseTraList[i].UpdateJumpStrVerLoad(spanFit.NumJGB, PhaseTraList[i - 5].JmWireData.WeatherParas.WeathComm);
                }
            }
        }


        public void CalsTensionMax()
        {
            PhaseTraList[0].UpdateBreakTensionMax(BackSideRes.IndBreakTensionDiff);
            PhaseTraList[0].UpdateUnbaIceTensionMax(BackSideRes.IndUnbaIceTensionDiff);

            PhaseTraList[3].UpdateBreakTensionMax(BackSideRes.GrdBreakTensionDiff);
            PhaseTraList[3].UpdateUnbaIceTensionMax(BackSideRes.GrdUnbaIceTensionDiff);

            PhaseTraList[4].UpdateBreakTensionMax(BackSideRes.OPGWBreakTensionDiff);
            PhaseTraList[4].UpdateUnbaIceTensionMax(BackSideRes.OPGWUnbaIceTensionDiff);

            PhaseTraList[5].UpdateBreakTensionMax(FrontSideRes.IndBreakTensionDiff);
            PhaseTraList[5].UpdateUnbaIceTensionMax(FrontSideRes.IndUnbaIceTensionDiff);

            PhaseTraList[8].UpdateBreakTensionMax(FrontSideRes.GrdBreakTensionDiff);
            PhaseTraList[8].UpdateUnbaIceTensionMax(FrontSideRes.GrdUnbaIceTensionDiff);

            PhaseTraList[9].UpdateBreakTensionMax(FrontSideRes.OPGWBreakTensionDiff);
            PhaseTraList[9].UpdateUnbaIceTensionMax(FrontSideRes.OPGWUnbaIceTensionDiff);
        }

        public List<string> PrintCalsReslt()
        {
            List<string> relt = new List<string>();

            relt.Add("\n垂直档距计算：");
            for (int i = 0; i <= 4; i++)
            {
                relt.Add(FileUtils.PadRightEx("小号侧"+i, 118) + FileUtils.PadRightEx("大号侧"+i, 118));
                relt.Add(FileUtils.PadRightEx("工况", 26) + FileUtils.PadRightEx("计算过程：", 80) + FileUtils.PadRightEx("计算值：", 12)
                    + FileUtils.PadRightEx("工况", 26) + FileUtils.PadRightEx("计算过程：", 80) + FileUtils.PadRightEx("计算值：", 12));

                var phase = PhaseTraList[i];
                var phaseAn = PhaseTraList[i+5];

                foreach(var name in phase.WireData.WorkCdtNames)
                {
                    relt.Add(FileUtils.PadRightEx(name, 26) + FileUtils.PadRightEx(phase.VerSpanStrDic[name], 80) + FileUtils.PadRightEx(phase.VerSpanDic[name].ToString("0.###"), 12)
                        + FileUtils.PadRightEx(name, 26) + FileUtils.PadRightEx(phaseAn.VerSpanStrDic[name], 80) + FileUtils.PadRightEx(phaseAn.VerSpanDic[name].ToString("0.###"), 12));
                }
            }

            relt.Add("\n水平荷载计算：");
            for (int i = 0; i <= 4; i++)
            {
                relt.Add(FileUtils.PadRightEx("小号侧" + i, 118) + FileUtils.PadRightEx("大号侧" + i, 118));
                relt.Add(FileUtils.PadRightEx("工况", 26) + FileUtils.PadRightEx("计算过程：", 80) + FileUtils.PadRightEx("计算值：", 12)
                    + FileUtils.PadRightEx("工况", 26) + FileUtils.PadRightEx("计算过程：", 80) + FileUtils.PadRightEx("计算值：", 12));

                var phase = PhaseTraList[i];
                var phaseAn = PhaseTraList[i + 5];

                foreach (var name in phase.WireData.WorkCdtNames)
                {
                    relt.Add(FileUtils.PadRightEx(name, 26) + FileUtils.PadRightEx(phase.HoriLoadStrDic[name], 80) + FileUtils.PadRightEx(phase.HoriLoadDic[name].ToString("0.###"), 12)
                        + FileUtils.PadRightEx(name, 26) + FileUtils.PadRightEx(phaseAn.HoriLoadStrDic[name], 80) + FileUtils.PadRightEx(phaseAn.HoriLoadDic[name].ToString("0.###"), 12));
                }
            }


            relt.Add("\n垂直荷载计算：");
            for (int i = 0; i <= 4; i++)
            {
                relt.Add(FileUtils.PadRightEx("小号侧" + i, 118) + FileUtils.PadRightEx("大号侧" + i, 118));
                relt.Add(FileUtils.PadRightEx("工况", 26) + FileUtils.PadRightEx("计算过程：", 80) + FileUtils.PadRightEx("计算值：", 12)
                    + FileUtils.PadRightEx("工况", 26) + FileUtils.PadRightEx("计算过程：", 80) + FileUtils.PadRightEx("计算值：", 12));

                var phase = PhaseTraList[i];
                var phaseAn = PhaseTraList[i + 5];

                foreach (var name in phase.WireData.WorkCdtNames)
                {
                    relt.Add(FileUtils.PadRightEx(name, 26) + FileUtils.PadRightEx(phase.VerLoadStrDic[name], 80) + FileUtils.PadRightEx(phase.VerLoadDic[name].ToString("0.###"), 12)
                        + FileUtils.PadRightEx(name, 26) + FileUtils.PadRightEx(phaseAn.VerLoadStrDic[name], 80) + FileUtils.PadRightEx(phaseAn.VerLoadDic[name].ToString("0.###"), 12));
                }
            }

            relt.Add("\n线条张力：");
            for (int i = 0; i <= 4; i++)
            {
                relt.Add(FileUtils.PadRightEx("小号侧" + i, 118) + FileUtils.PadRightEx("大号侧" + i, 118));
                relt.Add(FileUtils.PadRightEx("工况", 26) + FileUtils.PadRightEx("计算过程：", 80) + FileUtils.PadRightEx("计算值：", 12)
                    + FileUtils.PadRightEx("工况", 26) + FileUtils.PadRightEx("计算过程：", 80) + FileUtils.PadRightEx("计算值：", 12));

                var phase = PhaseTraList[i];
                var phaseAn = PhaseTraList[i + 5];

                foreach (var name in phase.WireData.WorkCdtNames)
                {
                    relt.Add(FileUtils.PadRightEx(name, 26) + FileUtils.PadRightEx(phase.TensionStrDic[name], 80) + FileUtils.PadRightEx(phase.TensionDic[name].ToString("0.###"), 12)
                        + FileUtils.PadRightEx(name, 26) + FileUtils.PadRightEx(phaseAn.TensionStrDic[name], 80) + FileUtils.PadRightEx(phaseAn.TensionDic[name].ToString("0.###"), 12));
                }
            }

            relt.Add("\n跳线串水平荷载：");
            for (int i = 0; i <= 2; i++)
            {
                relt.Add(FileUtils.PadRightEx("小号侧" + i, 118) + FileUtils.PadRightEx("大号侧" + i, 118));
                relt.Add(FileUtils.PadRightEx("工况", 26) + FileUtils.PadRightEx("计算过程：", 80) + FileUtils.PadRightEx("计算值：", 12)
                    + FileUtils.PadRightEx("工况", 26) + FileUtils.PadRightEx("计算过程：", 80) + FileUtils.PadRightEx("计算值：", 12));

                var phase = PhaseTraList[i];
                var phaseAn = PhaseTraList[i + 5];

                foreach (var name in phase.WireData.WorkCdtNames)
                {
                    relt.Add(FileUtils.PadRightEx(name, 26) + FileUtils.PadRightEx(phase.JumpStrHorLoadStrDic[name], 80) + FileUtils.PadRightEx(phase.JumpStrHorLoadDic[name].ToString("0.###"), 12)
                        + FileUtils.PadRightEx(name, 26) + FileUtils.PadRightEx(phaseAn.JumpStrHorLoadStrDic[name], 80) + FileUtils.PadRightEx(phaseAn.JumpStrHorLoadDic[name].ToString("0.###"), 12));
                }
            }

            relt.Add("\n跳线串垂直荷载：");
            for (int i = 0; i <= 2; i++)
            {
                relt.Add(FileUtils.PadRightEx("小号侧" + i, 148) + FileUtils.PadRightEx("大号侧" + i, 148));
                relt.Add(FileUtils.PadRightEx("工况", 26) + FileUtils.PadRightEx("计算过程：", 110) + FileUtils.PadRightEx("计算值：", 12)
                    + FileUtils.PadRightEx("工况", 26) + FileUtils.PadRightEx("计算过程：", 110) + FileUtils.PadRightEx("计算值：", 12));

                var phase = PhaseTraList[i];
                var phaseAn = PhaseTraList[i + 5];

                foreach (var name in phase.WireData.WorkCdtNames)
                {
                    relt.Add(FileUtils.PadRightEx(name, 26) + FileUtils.PadRightEx(phase.JumpStrVerLoadStrDic[name], 110) + FileUtils.PadRightEx(phase.JumpStrVerLoadDic[name].ToString("0.###"), 12)
                        + FileUtils.PadRightEx(name, 26) + FileUtils.PadRightEx(phaseAn.JumpStrVerLoadStrDic[name], 110) + FileUtils.PadRightEx(phaseAn.JumpStrVerLoadDic[name].ToString("0.###"), 12));
                }
            }

            return relt;
        }

        public List<string> PrintTensionDiff()
        {
            List<string> relt = new List<string>();

            relt.Add("\n张力差：");
            relt.Add(FileUtils.PadRightEx("小号侧", 26) + FileUtils.PadRightEx("计算过程：", 50) + FileUtils.PadRightEx("计算值：", 16) 
                + FileUtils.PadRightEx("大号侧", 26) + FileUtils.PadRightEx("计算过程：", 50) + FileUtils.PadRightEx("计算值：", 16) + FileUtils.PadRightEx("确定Max：", 32) + FileUtils.PadRightEx("最终Max：", 32));
            relt.Add(FileUtils.PadRightEx("导线事故断线工况", 26) + FileUtils.PadRightEx(BackSideRes.IndBreakTensionDiffStr, 50) + FileUtils.PadRightEx(BackSideRes.IndBreakTensionDiff.ToString("0.##"), 16)
                + FileUtils.PadRightEx("导线事故断线工况", 26) + FileUtils.PadRightEx(FrontSideRes.IndBreakTensionDiffStr, 50) + FileUtils.PadRightEx(FrontSideRes.IndBreakTensionDiff.ToString("0.##"), 16)
                + FileUtils.PadRightEx(PhaseTraList[0].BreakTensionMaxO.ToString("0.##"), 20) + FileUtils.PadRightEx(PhaseTraList[5].BreakTensionMaxO.ToString("0.##"), 16)
                + FileUtils.PadRightEx(PhaseTraList[0].BreakTensionMax.ToString("0.##"), 20) + FileUtils.PadRightEx(PhaseTraList[5].BreakTensionMax.ToString("0.##"), 16));
            relt.Add(FileUtils.PadRightEx("地线事故断线工况", 26) + FileUtils.PadRightEx(BackSideRes.GrdBreakTensionDiffStr, 50) + FileUtils.PadRightEx(BackSideRes.GrdBreakTensionDiff.ToString("0.##"), 16)
                + FileUtils.PadRightEx("地线事故断线工况", 26) + FileUtils.PadRightEx(FrontSideRes.GrdBreakTensionDiffStr, 50) + FileUtils.PadRightEx(FrontSideRes.GrdBreakTensionDiff.ToString("0.##"), 16)
                + FileUtils.PadRightEx(PhaseTraList[3].BreakTensionMaxO.ToString("0.##"), 20) + FileUtils.PadRightEx(PhaseTraList[8].BreakTensionMaxO.ToString("0.##"), 16)
                + FileUtils.PadRightEx(PhaseTraList[3].BreakTensionMax.ToString("0.##"), 20) + FileUtils.PadRightEx(PhaseTraList[8].BreakTensionMax.ToString("0.##"), 16));
            relt.Add(FileUtils.PadRightEx("地线事故断线工况", 26) + FileUtils.PadRightEx(BackSideRes.OPGWBreakTensionDiffStr, 50) + FileUtils.PadRightEx(BackSideRes.OPGWBreakTensionDiff.ToString("0.##"), 16)
                + FileUtils.PadRightEx("导线事故断线工况", 26) + FileUtils.PadRightEx(FrontSideRes.OPGWBreakTensionDiffStr, 50) + FileUtils.PadRightEx(FrontSideRes.OPGWBreakTensionDiff.ToString("0.##"), 16)
                + FileUtils.PadRightEx(PhaseTraList[4].BreakTensionMaxO.ToString("0.##"), 20) + FileUtils.PadRightEx(PhaseTraList[9].BreakTensionMaxO.ToString("0.##"), 16)
                + FileUtils.PadRightEx(PhaseTraList[4].BreakTensionMax.ToString("0.##"), 20) + FileUtils.PadRightEx(PhaseTraList[9].BreakTensionMax.ToString("0.##"), 16));
            relt.Add(FileUtils.PadRightEx("导线事故断线工况", 26) + FileUtils.PadRightEx(BackSideRes.IndUnbaIceTensionDiffStr, 50) + FileUtils.PadRightEx(BackSideRes.IndUnbaIceTensionDiff.ToString("0.##"), 16)
                + FileUtils.PadRightEx("导线事故断线工况", 26) + FileUtils.PadRightEx(FrontSideRes.IndUnbaIceTensionDiffStr, 50) + FileUtils.PadRightEx(FrontSideRes.IndUnbaIceTensionDiff.ToString("0.##"), 16)
                + FileUtils.PadRightEx(PhaseTraList[0].UnbaIceTensionMax0.ToString("0.##"), 20) + FileUtils.PadRightEx(PhaseTraList[5].UnbaIceTensionMax0.ToString("0.##"), 16)
                + FileUtils.PadRightEx(PhaseTraList[0].UnbaIceTensionMax.ToString("0.##"), 20) + FileUtils.PadRightEx(PhaseTraList[5].UnbaIceTensionMax.ToString("0.##"), 16));
            relt.Add(FileUtils.PadRightEx("导线事故断线工况", 26) + FileUtils.PadRightEx(BackSideRes.GrdUnbaIceTensionDiffStr, 50) + FileUtils.PadRightEx(BackSideRes.GrdUnbaIceTensionDiff.ToString("0.##"), 16)
                + FileUtils.PadRightEx("导线事故断线工况", 26) + FileUtils.PadRightEx(FrontSideRes.GrdUnbaIceTensionDiffStr, 50) + FileUtils.PadRightEx(FrontSideRes.GrdUnbaIceTensionDiff.ToString("0.##"), 16)
                + FileUtils.PadRightEx(PhaseTraList[3].UnbaIceTensionMax0.ToString("0.##"), 20) + FileUtils.PadRightEx(PhaseTraList[8].UnbaIceTensionMax0.ToString("0.##"), 16)
                + FileUtils.PadRightEx(PhaseTraList[3].UnbaIceTensionMax.ToString("0.##"), 20) + FileUtils.PadRightEx(PhaseTraList[8].UnbaIceTensionMax.ToString("0.##"), 16));
            relt.Add(FileUtils.PadRightEx("导线事故断线工况", 26) + FileUtils.PadRightEx(BackSideRes.OPGWUnbaIceTensionDiffStr, 50) + FileUtils.PadRightEx(BackSideRes.OPGWUnbaIceTensionDiff.ToString("0.##"), 16)
                + FileUtils.PadRightEx("导线事故断线工况", 26) + FileUtils.PadRightEx(FrontSideRes.OPGWUnbaIceTensionDiffStr, 50) + FileUtils.PadRightEx(FrontSideRes.OPGWUnbaIceTensionDiff.ToString("0.##"), 16)
                + FileUtils.PadRightEx(PhaseTraList[4].UnbaIceTensionMax0.ToString("0.##"), 20) + FileUtils.PadRightEx(PhaseTraList[8].UnbaIceTensionMax0.ToString("0.##"), 16)
                + FileUtils.PadRightEx(PhaseTraList[4].UnbaIceTensionMax.ToString("0.##"), 20) + FileUtils.PadRightEx(PhaseTraList[8].UnbaIceTensionMax.ToString("0.##"), 16));

            return relt;
        }

        double BackIndWindHc { get; set; } = 22.10;
        double FrontIndWindHc { get; set; } = 7.62;

        double BackGrdWindHc { get; set; } = 13.04;
        double FrontGrdWindHc { get; set; } = 6.86;

        double BackOPGWWindHc { get; set; } = 13.63;
        double FrontOPGWWindHc { get; set; } = 7.13;

        /// <summary>
        /// 计算大风工况下垂直方向的应用弧垂
        /// </summary>
        void CalDFCure()
        {
            //这部分计算很复杂，但是原理性不足
            //计算导线的大风应用弧垂，先后侧，先前侧
            double GdBackHc = TowerAppre.DnSideInHei - BackSideRes.CommParas.GrdAveHei;
            //double GdBackHc = DownSideHei - BackSideRes.CommParas.GrdCl;

            double KBackHot = BackSideRes.IndWire.BzDic["最高气温"].VerBizai / 8 / BackSideRes.IndWire.BzDic["最高气温"].Stress;
            double KBackIce = BackSideRes.IndWire.BzDic["覆冰无风"].VerBizai / 8 / BackSideRes.IndWire.BzDic["覆冰无风"].Stress;

            double KMaxHc = Math.Max(KBackHot, KBackIce) * Math.Pow(BackPosRes.Span, 2);
            double MaxIndBackHc;

            //最大弧垂应用值
            if (KBackHot > KBackIce)
            {
                MaxIndBackHc = Math.Min(GdBackHc, KMaxHc) / BackSideRes.IndWire.BzDic["最高气温"].g2 * BackSideRes.IndWire.YLTable["最高气温"] * BackSideRes.IndWire.BzDic["最大风速"].g2 / BackSideRes.IndWire.YLTable["最大风速"];
            }
            else
            {
                MaxIndBackHc = Math.Min(GdBackHc, KMaxHc) / BackSideRes.IndWire.BzDic["覆冰无风"].g2 * BackSideRes.IndWire.YLTable["覆冰无风"] * BackSideRes.IndWire.BzDic["最大风速"].g2 / BackSideRes.IndWire.YLTable["最大风速"];
            }
            BackIndWindHc = MaxIndBackHc * BackSideRes.IndWire.BzDic["最大风速"].g2 / BackSideRes.IndWire.BzDic["最大风速"].g7;

            //计算前侧的大风应用弧垂
            //对地距离控制最大弧垂
            double GdFrontHc = TowerAppre.DnSideInHei - FrontSideRes.CommParas.GrdAveHei;
            double KFrontHot = FrontSideRes.IndWire.BzDic["最高气温"].g2 / 8 / FrontSideRes.IndWire.YLTable["最高气温"];
            double KFrontIce = FrontSideRes.IndWire.BzDic["覆冰无风"].g2 / 8 / FrontSideRes.IndWire.YLTable["覆冰无风"];
            // 按照K值控制的最大弧垂
            KMaxHc = Math.Max(KFrontHot, KFrontIce) * Math.Pow(FrontPosRes.Span, 2); 
        
            double MaxIndFrontHc; 
            //最大弧垂应用值
            if (KFrontHot > KFrontIce)
            {
                MaxIndFrontHc = Math.Min(GdFrontHc, KMaxHc) / FrontSideRes.IndWire.BzDic["最高气温"].g2 * FrontSideRes.IndWire.YLTable["最高气温"] * FrontSideRes.IndWire.BzDic["最大风速"].g2 / FrontSideRes.IndWire.YLTable["最大风速"];
            }
            else
            {
                MaxIndFrontHc = Math.Min(GdFrontHc, KMaxHc) / FrontSideRes.IndWire.BzDic["覆冰无风"].g2 * FrontSideRes.IndWire.YLTable["覆冰无风"] * FrontSideRes.IndWire.BzDic["最大风速"].g2 / FrontSideRes.IndWire.YLTable["最大风速"];
            }

            //计算前侧大风应用弧垂
            FrontIndWindHc = MaxIndFrontHc * FrontSideRes.IndWire.BzDic["最大风速"].g2 / FrontSideRes.IndWire.BzDic["最大风速"].g7;

            //计算地线的大风弧垂应用值,先普通地线，再OPGW
            //这部分代码Excel表格有问题，为保持一致，还是采用原来的方法
            double KHot = BackSideRes.GrdWire.BzDic["最高气温"].g2 / 8 / BackSideRes.GrdWire.YLTable["最高气温"];
            double KIce = BackSideRes.GrdWire.BzDic["覆冰无风"].g2 / 8 / BackSideRes.GrdWire.YLTable["覆冰无风"];
            BackGrdWindHc = MaxIndFrontHc * Math.Max(KHot, KIce) / Math.Max(KBackHot, KBackIce);
            KHot = BackSideRes.OPGWWire.BzDic["最高气温"].g2 / 8 / BackSideRes.OPGWWire.YLTable["最高气温"];
            KIce = BackSideRes.OPGWWire.BzDic["覆冰无风"].g2 / 8 / BackSideRes.OPGWWire.YLTable["覆冰无风"];
            BackOPGWWindHc = MaxIndFrontHc * Math.Max(KHot, KIce) / Math.Max(KBackHot, KBackIce);

            //前侧计算
            KHot = FrontSideRes.GrdWire.BzDic["最高气温"].g2 / 8 / FrontSideRes.GrdWire.YLTable["最高气温"];
            KIce = FrontSideRes.GrdWire.BzDic["覆冰无风"].g2 / 8 / FrontSideRes.GrdWire.YLTable["覆冰无风"];
            FrontGrdWindHc = MaxIndFrontHc * Math.Max(KHot, KIce) / Math.Max(KFrontHot, KFrontIce);
            KHot = FrontSideRes.OPGWWire.BzDic["最高气温"].g2 / 8 / FrontSideRes.OPGWWire.YLTable["最高气温"];
            KIce = FrontSideRes.OPGWWire.BzDic["覆冰无风"].g2 / 8 / FrontSideRes.OPGWWire.YLTable["覆冰无风"];
            FrontOPGWWindHc = MaxIndFrontHc * Math.Max(KHot, KIce) / Math.Max(KFrontHot, KFrontIce);
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

        public List<string> PrintWindPara()
        {
            List<string> rslt = new List<string>();

            rslt.Add("小号侧");
            rslt.Add(FileUtils.PadRightEx("上相导线高差", 17) + PhaseTraList[0].SpaceStr.SubHei.ToString("0.0").PadRight(8) 
                + FileUtils.PadRightEx("中相导线高差", 17) + PhaseTraList[1].SpaceStr.SubHei.ToString("0.0").PadRight(8)
                + FileUtils.PadRightEx("下相导线高差", 17) + PhaseTraList[2].SpaceStr.SubHei.ToString("0.0").PadRight(8)
                + FileUtils.PadRightEx("地线高差", 17) + PhaseTraList[3].SpaceStr.SubHei.ToString("0.000").PadRight(8));
            rslt.Add(FileUtils.PadRightEx("上相导线μz", 18) + PhaseTraList[0].WireWindPara.ToString("0.000").PadRight(8)
                + FileUtils.PadRightEx("中相导线μz", 18) + PhaseTraList[1].WireWindPara.ToString("0.000").PadRight(8)
                + FileUtils.PadRightEx("下相导线μz", 18) + PhaseTraList[2].WireWindPara.ToString("0.000").PadRight(8)
                + FileUtils.PadRightEx("地线μz", 18) + PhaseTraList[3].WireWindPara.ToString("0.000").PadRight(8));
            rslt.Add(FileUtils.PadRightEx("上相串μz", 18) + PhaseTraList[0].StrWindPara.ToString("0.000").PadRight(8)
                + FileUtils.PadRightEx("中相串μz", 18) + PhaseTraList[1].StrWindPara.ToString("0.000").PadRight(8)
                + FileUtils.PadRightEx("下相串μz", 18) + PhaseTraList[2].StrWindPara.ToString("0.000").PadRight(8)
                + FileUtils.PadRightEx("地线串μz", 18) + PhaseTraList[3].StrWindPara.ToString("0.00").PadRight(8));
            rslt.Add(FileUtils.PadRightEx("上相跳线绝缘子μz", 18) + PhaseTraList[0].JmWindPara.ToString("0.000").PadRight(8)
                + FileUtils.PadRightEx("中相跳线绝缘子μz", 18) + PhaseTraList[1].JmWindPara.ToString("0.000").PadRight(8)
                + FileUtils.PadRightEx("下相跳线绝缘子μz", 18) + PhaseTraList[2].JmWindPara.ToString("0.000").PadRight(8));
            rslt.Add(FileUtils.PadRightEx("上相跳线支撑管μz", 18) + PhaseTraList[0].PropUpWindPara.ToString("0.000").PadRight(8)
                + FileUtils.PadRightEx("中相跳线支撑管μz", 18) + PhaseTraList[1].PropUpWindPara.ToString("0.000").PadRight(8)
                + FileUtils.PadRightEx("下相跳线支撑管μz", 18) + PhaseTraList[2].PropUpWindPara.ToString("0.000").PadRight(8));

            rslt.Add("\n大号侧");
            rslt.Add(FileUtils.PadRightEx("上相导线高差", 17) + PhaseTraList[5].SpaceStr.SubHei.ToString("0.0").PadRight(8) 
                + FileUtils.PadRightEx("中相导线高差", 17) + PhaseTraList[6].SpaceStr.SubHei.ToString("0.0").PadRight(8)
                + FileUtils.PadRightEx("下相导线高差", 17) + PhaseTraList[7].SpaceStr.SubHei.ToString("0.0").PadRight(8)
                + FileUtils.PadRightEx("地线高差", 17) + PhaseTraList[8].SpaceStr.SubHei.ToString("0.0").PadRight(8));
            rslt.Add(FileUtils.PadRightEx("上相导线μz", 18) + PhaseTraList[5].WireWindPara.ToString("0.000").PadRight(8)
                + FileUtils.PadRightEx("中相导线μz", 18) + PhaseTraList[6].WireWindPara.ToString("0.000").PadRight(8)
                + FileUtils.PadRightEx("下相导线μz", 18) + PhaseTraList[7].WireWindPara.ToString("0.000").PadRight(8)
                + FileUtils.PadRightEx("地线μz", 18) + PhaseTraList[8].WireWindPara.ToString("0.000").PadRight(8));
            rslt.Add(FileUtils.PadRightEx("上相串μz", 18) + PhaseTraList[5].StrWindPara.ToString("0.000").PadRight(8)
                + FileUtils.PadRightEx("中相串μz", 18) + PhaseTraList[6].StrWindPara.ToString("0.000").PadRight(8)
                + FileUtils.PadRightEx("下相串μz", 18) + PhaseTraList[7].StrWindPara.ToString("0.000").PadRight(8)
                + FileUtils.PadRightEx("地线串μz", 18) + PhaseTraList[8].StrWindPara.ToString("0.000").PadRight(8));
            rslt.Add(FileUtils.PadRightEx("上相跳线绝缘子μz", 18) + PhaseTraList[5].JmWindPara.ToString("0.000").PadRight(8)
                + FileUtils.PadRightEx("中相跳线绝缘子μz", 18) + PhaseTraList[6].JmWindPara.ToString("0.000").PadRight(8)
                + FileUtils.PadRightEx("下相跳线绝缘子μz", 18) + PhaseTraList[7].JmWindPara.ToString("0.000").PadRight(8));
            rslt.Add(FileUtils.PadRightEx("上相跳线支撑管μz", 18) + PhaseTraList[5].PropUpWindPara.ToString("0.000").PadRight(8)
                + FileUtils.PadRightEx("中相跳线支撑管μz", 18) + PhaseTraList[6].PropUpWindPara.ToString("0.000").PadRight(8)
                + FileUtils.PadRightEx("下相跳线支撑管μz", 18) + PhaseTraList[7].PropUpWindPara.ToString("0.000").PadRight(8));

            return rslt;
        }



    }
}
