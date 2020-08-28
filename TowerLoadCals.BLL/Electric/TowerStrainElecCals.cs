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
            PhaseTraList[0].WireWindPara = CalsWireWindPara(BackSideRes.CommParas.WireWindPara, AbsUpSideHei, FrontIndWindHc, BackSideRes.CommParas.IndAveHei);
            PhaseTraList[0].StrWindPara = CalsStrWindPara(AbsUpSideHei, BackSideRes.CommParas.IndAveHei);
            PhaseTraList[0].JmWindPara = CalsJumpStrWindPara(BackSideRes.CommParas.JmpWindPara, AbsUpJumHei, BackSideRes.CommParas.IndAveHei, BackSideRes.CommParas.JumpStrLen);
            PhaseTraList[0].PropUpWindPara = CalsPropUpWindPara(BackSideRes.CommParas.JmpWindPara, AbsUpJumHei, BackSideRes.CommParas.IndAveHei, BackSideRes.CommParas.JumpStrLen);

            PhaseTraList[1].WireWindPara = CalsWireWindPara(BackSideRes.CommParas.WireWindPara, AbsMidHei, FrontIndWindHc, BackSideRes.CommParas.IndAveHei);
            PhaseTraList[1].StrWindPara = CalsStrWindPara(AbsMidHei, BackSideRes.CommParas.IndAveHei);
            PhaseTraList[1].JmWindPara = CalsJumpStrWindPara(BackSideRes.CommParas.JmpWindPara, AbsMidJumHei, BackSideRes.CommParas.IndAveHei, BackSideRes.CommParas.JumpStrLen);
            PhaseTraList[1].PropUpWindPara = CalsPropUpWindPara(BackSideRes.CommParas.JmpWindPara, AbsMidJumHei, BackSideRes.CommParas.IndAveHei, BackSideRes.CommParas.JumpStrLen);

            PhaseTraList[2].WireWindPara = CalsWireWindPara(BackSideRes.CommParas.WireWindPara, AbsDownSideHei, FrontIndWindHc, BackSideRes.CommParas.IndAveHei);
            PhaseTraList[2].StrWindPara = CalsStrWindPara(AbsDownSideHei, BackSideRes.CommParas.IndAveHei);
            PhaseTraList[2].JmWindPara = CalsJumpStrWindPara(BackSideRes.CommParas.JmpWindPara, AbsDownJumHei, BackSideRes.CommParas.IndAveHei, BackSideRes.CommParas.JumpStrLen);
            PhaseTraList[2].PropUpWindPara = CalsPropUpWindPara(BackSideRes.CommParas.JmpWindPara, AbsDownJumHei, BackSideRes.CommParas.IndAveHei, BackSideRes.CommParas.JumpStrLen);

            PhaseTraList[3].WireWindPara = CalsWireWindPara(BackSideRes.CommParas.WireWindPara, AbsGrdHei, FrontGrdWindHc, BackSideRes.CommParas.GrdAveHei);
            PhaseTraList[3].StrWindPara = CalsStrWindPara(AbsGrdHei, BackSideRes.CommParas.GrdAveHei);

            PhaseTraList[4].WireWindPara = CalsWireWindPara(BackSideRes.CommParas.WireWindPara, AbsGrdHei, FrontGrdWindHc, BackSideRes.CommParas.GrdAveHei);
            PhaseTraList[4].StrWindPara = CalsStrWindPara(AbsGrdHei, BackSideRes.CommParas.GrdAveHei);

            PhaseTraList[5].WireWindPara = CalsWireWindPara(FrontSideRes.CommParas.WireWindPara, AbsUpSideHei, BackIndWindHc, FrontSideRes.CommParas.IndAveHei);
            PhaseTraList[5].StrWindPara = CalsStrWindPara(AbsUpSideHei, FrontSideRes.CommParas.IndAveHei);
            PhaseTraList[5].JmWindPara = CalsJumpStrWindPara(FrontSideRes.CommParas.JmpWindPara, AbsUpJumHei, FrontSideRes.CommParas.IndAveHei, FrontSideRes.CommParas.JumpStrLen);
            PhaseTraList[5].PropUpWindPara = CalsPropUpWindPara(FrontSideRes.CommParas.JmpWindPara, AbsUpJumHei, FrontSideRes.CommParas.IndAveHei, FrontSideRes.CommParas.JumpStrLen);

            PhaseTraList[6].WireWindPara = CalsWireWindPara(FrontSideRes.CommParas.WireWindPara, AbsMidHei, BackIndWindHc, FrontSideRes.CommParas.IndAveHei);
            PhaseTraList[6].StrWindPara = CalsStrWindPara(AbsMidHei, FrontSideRes.CommParas.IndAveHei);
            PhaseTraList[6].JmWindPara = CalsJumpStrWindPara(FrontSideRes.CommParas.JmpWindPara, AbsMidJumHei, FrontSideRes.CommParas.IndAveHei, FrontSideRes.CommParas.JumpStrLen);
            PhaseTraList[6].PropUpWindPara = CalsPropUpWindPara(FrontSideRes.CommParas.JmpWindPara, AbsMidJumHei, FrontSideRes.CommParas.IndAveHei, FrontSideRes.CommParas.JumpStrLen);

            PhaseTraList[7].WireWindPara = CalsWireWindPara(FrontSideRes.CommParas.WireWindPara, AbsDownSideHei, BackIndWindHc, FrontSideRes.CommParas.IndAveHei);
            PhaseTraList[7].StrWindPara = CalsStrWindPara(AbsDownSideHei, FrontSideRes.CommParas.IndAveHei);
            PhaseTraList[7].JmWindPara = CalsJumpStrWindPara(FrontSideRes.CommParas.JmpWindPara, AbsDownJumHei, FrontSideRes.CommParas.IndAveHei, FrontSideRes.CommParas.JumpStrLen);
            PhaseTraList[7].PropUpWindPara = CalsPropUpWindPara(FrontSideRes.CommParas.JmpWindPara, AbsDownJumHei, FrontSideRes.CommParas.IndAveHei, FrontSideRes.CommParas.JumpStrLen);

            PhaseTraList[8].WireWindPara = CalsWireWindPara(FrontSideRes.CommParas.WireWindPara, AbsGrdHei, BackGrdWindHc, FrontSideRes.CommParas.GrdAveHei);
            PhaseTraList[8].StrWindPara = CalsStrWindPara(AbsGrdHei, FrontSideRes.CommParas.GrdAveHei);

            PhaseTraList[9].WireWindPara = CalsWireWindPara(FrontSideRes.CommParas.WireWindPara, AbsGrdHei, BackGrdWindHc, FrontSideRes.CommParas.GrdAveHei);
            PhaseTraList[9].StrWindPara = CalsStrWindPara(AbsGrdHei, FrontSideRes.CommParas.GrdAveHei);
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
                wireExtend = PhaseTraList[i].WireData.bGrd == 0 ? FrontSideRes.SideParas.IndExtendPara : FrontSideRes.SideParas.GrdExtendPara;

                PhaseTraList[i].UpdateHorFor(diaInc);
                PhaseTraList[i].UpdateVerWei(weiInc, spanFit.NumJGB, spanFit.WeiJGB, numFZC, weiFZC);
                PhaseTraList[i].UpdateLoStr(secInc, constrErrorPara, insErrorPara, wireExtend);

                if(PhaseTraList[i].WireData.bGrd == 0)
                {
                    PhaseTraList[i].UpdateJumpHorFor();
                    PhaseTraList[i].UpdateJumpVerWei(spanFit.WeiJGB, PhaseTraList[i+5].JmWireData.WeatherParas.WeathComm);
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
                wireExtend = PhaseTraList[i].WireData.bGrd == 0 ? FrontSideRes.SideParas.IndExtendPara : FrontSideRes.SideParas.GrdExtendPara;

                PhaseTraList[i].UpdateHorFor(diaInc);
                PhaseTraList[i].UpdateVerWei(weiInc, spanFit.NumJGB, spanFit.WeiJGB, numFZC, weiFZC);
                PhaseTraList[i].UpdateLoStr(secInc, constrErrorPara, insErrorPara, wireExtend);

                if (PhaseTraList[i].WireData.bGrd == 0)
                {
                    PhaseTraList[i].UpdateJumpHorFor();
                    PhaseTraList[i].UpdateJumpVerWei(spanFit.WeiJGB, PhaseTraList[i - 5].JmWireData.WeatherParas.WeathComm);
                }
            }

            for (int i = 0; i <=4; i++)
            {
                PhaseTraList[i].CheckLoStr(PhaseTraList[i+5].LoadList, FrontSideRes.SideParas, BackSideRes.SideParas);
                PhaseTraList[i+5].CheckLoStr(PhaseTraList[i].LoadList, FrontSideRes.SideParas, BackSideRes.SideParas);
            }
        }

        public void Cals1()
        {
            for (int i = 0; i <= 4; i++)
            {
                var spanFit = BackSideRes.SpanFit;

                double diaInc, weiInc, secInc, weiFZC, insErrorPara, constrErrorPara, wireExtend;
                int numFZC;

                if (PhaseTraList[i].WireData.bGrd == 0)
                {
                    diaInc = BackSideRes.CommParas.DiaIndInc;
                    weiInc = BackSideRes.CommParas.WeiIndInc;
                    numFZC = spanFit.NumInFZC;
                    weiFZC = spanFit.WeiInFZC;
                    secInc = BackSideRes.CommParas.SecIndInc;
                }
                else if (PhaseTraList[i].WireData.bGrd == 1)
                {
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
                constrErrorPara = FrontSideRes.SideParas.ConstruErrorPara;
                wireExtend = PhaseTraList[i].WireData.bGrd == 0 ? FrontSideRes.SideParas.IndExtendPara : FrontSideRes.SideParas.GrdExtendPara;

                foreach(var nameGk in PhaseTraList[i].WireData.WorkCdtNames)
                {
                    double verSpan = PhaseTraList[i].UpdateVertialSpan(nameGk, out string verSpanStr);
                    double horFor = PhaseTraList[i].UpdateHorFor(nameGk, out string horForStr, diaInc);
                    double verWei = PhaseTraList[i].UpdateVerWei(nameGk, out string verWeiStr, weiInc, verSpan, spanFit.NumJGB, spanFit.WeiJGB, numFZC, weiFZC);

                    double jumpHorFor = 0, jumpVerWei = 0;
                    string jumpHorForStr = "", jumpVerWeiStr = "";

                    if (PhaseTraList[i].WireData.bGrd == 0)
                    {
                        jumpHorFor = PhaseTraList[i].UpdateJumpHorFor(nameGk, out jumpHorForStr);
                        jumpVerWei = PhaseTraList[i].UpdateJumpVerWei(nameGk, out jumpVerWeiStr, spanFit.WeiJGB, PhaseTraList[i + 5].JmWireData.WeatherParas.WeathComm);
                    }

                    int index = PhaseTraList[i].LoadList.FindIndex(item => item.GKName == nameGk);
                    if (index < 0)
                    {
                        PhaseTraList[i].LoadList.Add(new LoadThrDe()
                        {
                            GKName = nameGk,
                            VetiSpan = verSpan,
                            VetiSpanStr = verSpanStr,
                            HorFor = horFor,
                            HorForStr = horForStr,
                            VerWei = verWei,
                            VerWeiStr = verWeiStr,
                            JumpHorFor = jumpHorFor,
                            JumpHorForStr = jumpHorForStr,
                            JumpVerWei = jumpVerWei,
                            JumpVerWeiStr = jumpVerWeiStr,
                        });
                    }
                    else
                    {
                        PhaseTraList[i].LoadList[index].VetiSpan = verSpan;
                        PhaseTraList[i].LoadList[index].VetiSpanStr = verSpanStr;
                        PhaseTraList[i].LoadList[index].HorFor = horFor;
                        PhaseTraList[i].LoadList[index].HorForStr = horForStr;
                        PhaseTraList[i].LoadList[index].VerWei = verWei;
                        PhaseTraList[i].LoadList[index].VerWeiStr = verWeiStr;
                        PhaseTraList[i].LoadList[index].JumpHorFor = jumpHorFor;
                        PhaseTraList[i].LoadList[index].JumpHorForStr = jumpHorForStr;
                        PhaseTraList[i].LoadList[index].JumpVerWei = jumpVerWei;
                        PhaseTraList[i].LoadList[index].JumpVerWeiStr = jumpVerWeiStr;
                    }
                }
                
                PhaseTraList[i].UpdateLoStr(secInc, constrErrorPara, insErrorPara, wireExtend);
            }

            for (int i = 5; i <= 9; i++)
            {
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
                wireExtend = PhaseTraList[i].WireData.bGrd == 0 ? FrontSideRes.SideParas.IndExtendPara : FrontSideRes.SideParas.GrdExtendPara;

                foreach (var nameGk in PhaseTraList[i].WireData.WorkCdtNames)
                {
                    double verSpan = PhaseTraList[i].UpdateVertialSpan(nameGk, out string verSpanStr);
                    double horFor = PhaseTraList[i].UpdateHorFor(nameGk, out string horForStr, diaInc);
                    double verWei = PhaseTraList[i].UpdateVerWei(nameGk, out string verWeiStr, weiInc, verSpan, spanFit.NumJGB, spanFit.WeiJGB, numFZC, weiFZC);

                    double jumpHorFor = 0, jumpVerWei = 0;
                    string jumpHorForStr = "", jumpVerWeiStr = "";

                    if (PhaseTraList[i].WireData.bGrd == 0)
                    {
                        jumpHorFor = PhaseTraList[i].UpdateJumpHorFor(nameGk, out jumpHorForStr);
                        jumpVerWei = PhaseTraList[i].UpdateJumpVerWei(nameGk, out jumpVerWeiStr, spanFit.WeiJGB, PhaseTraList[i - 5].JmWireData.WeatherParas.WeathComm);
                    }

                    int index = PhaseTraList[i].LoadList.FindIndex(item => item.GKName == nameGk);
                    if (index < 0)
                    {
                        PhaseTraList[i].LoadList.Add(new LoadThrDe()
                        {
                            GKName = nameGk,
                            VetiSpan = verSpan,
                            VetiSpanStr = verSpanStr,
                            HorFor = horFor,
                            HorForStr = horForStr,
                            VerWei = verWei,
                            VerWeiStr = verWeiStr,
                            JumpHorFor = jumpHorFor,
                            JumpHorForStr = jumpHorForStr,
                            JumpVerWei = jumpVerWei,
                            JumpVerWeiStr = jumpVerWeiStr,
                        });
                    }
                    else
                    {
                        PhaseTraList[i].LoadList[index].VetiSpan = verSpan;
                        PhaseTraList[i].LoadList[index].VetiSpanStr = verSpanStr;
                        PhaseTraList[i].LoadList[index].HorFor = horFor;
                        PhaseTraList[i].LoadList[index].HorForStr = horForStr;
                        PhaseTraList[i].LoadList[index].VerWei = verWei;
                        PhaseTraList[i].LoadList[index].VerWeiStr = verWeiStr;
                        PhaseTraList[i].LoadList[index].JumpHorFor = jumpHorFor;
                        PhaseTraList[i].LoadList[index].JumpHorForStr = jumpHorForStr;
                        PhaseTraList[i].LoadList[index].JumpVerWei = jumpVerWei;
                        PhaseTraList[i].LoadList[index].JumpVerWeiStr = jumpVerWeiStr;
                    }
                }

                PhaseTraList[i].UpdateLoStr(secInc, constrErrorPara, insErrorPara, wireExtend);
            }
        }

        public void UpdateTensionDiff()
        {
            for(int i = 0; i <=4; i++)
            {
                double secInc, effectPara, savePara;
                
                if(PhaseTraList[i].WireData.bGrd == 0)
                {
                    secInc = BackSideRes.CommParas.SecIndInc;
                    effectPara = BackSideRes.SideParas.IndEffectPara;
                    savePara = BackSideRes.SideParas.IndSafePara;
                }
                else if (PhaseTraList[i].WireData.bGrd == 1)
                {
                    secInc = BackSideRes.CommParas.SecGrdInc;
                    effectPara = BackSideRes.SideParas.GrdEffectPara;
                    savePara = BackSideRes.SideParas.GrdSafePara;
                }
                else
                {
                    secInc = BackSideRes.CommParas.SecOPGWInc;
                    effectPara = BackSideRes.SideParas.OPGWEffectPara;
                    savePara = BackSideRes.SideParas.OPGWSafePara;
                }

                PhaseTraList[i].UpdateTensionDiff(secInc, effectPara, savePara);
            }

            for (int i = 5; i <= 9; i++)
            {
                double secInc, effectPara, savePara;

                if (PhaseTraList[i].WireData.bGrd == 0)
                {
                    secInc = FrontSideRes.CommParas.SecIndInc;
                    effectPara = FrontSideRes.SideParas.IndEffectPara;
                    savePara = FrontSideRes.SideParas.IndSafePara;
                }
                else if (PhaseTraList[i].WireData.bGrd == 1)
                {
                    secInc = FrontSideRes.CommParas.SecGrdInc;
                    effectPara = FrontSideRes.SideParas.GrdEffectPara;
                    savePara = FrontSideRes.SideParas.GrdSafePara;
                }
                else
                {
                    secInc = FrontSideRes.CommParas.SecOPGWInc;
                    effectPara = FrontSideRes.SideParas.OPGWEffectPara;
                    savePara = FrontSideRes.SideParas.OPGWSafePara;
                }

                PhaseTraList[i].UpdateTensionDiff(secInc, effectPara, savePara);
            }
        }

        public void UpateAnchor()
        {
            var loadInd = PhaseTraList[0].LoadList.Where(item => item.GKName == "安装情况").FirstOrDefault();
            double noIceTensionInd1 = loadInd == null ? 0 : loadInd.LoStr;

            var loadIndLowTemp = PhaseTraList[0].LoadList.Where(item => item.GKName == "安装情况降温").FirstOrDefault();
            double lowTempTensionInd1 = loadIndLowTemp == null ? 0 : loadIndLowTemp.LoStr;

            var loadInd2 = PhaseTraList[5].LoadList.Where(item => item.GKName == "安装情况").FirstOrDefault();
            double noIceTensionInd2 = loadInd2 == null ? 0 : loadInd2.LoStr;

            var loadIndLowTemp2 = PhaseTraList[5].LoadList.Where(item => item.GKName == "安装情况降温").FirstOrDefault();
            double lowTempTensionInd2 = loadIndLowTemp2 == null ? 0 : loadIndLowTemp2.LoStr;
            AnchorTensionInd = AnchorTension(noIceTensionInd1, noIceTensionInd2, lowTempTensionInd1, lowTempTensionInd2);

            var loadGrd = PhaseTraList[3].LoadList.Where(item => item.GKName == "安装情况").FirstOrDefault();
            double noIceTensionGrd1 = loadGrd == null ? 0 : loadGrd.LoStr;

            var loadGrdLowTemp = PhaseTraList[3].LoadList.Where(item => item.GKName == "安装情况降温").FirstOrDefault();
            double lowTempTensionGrd1 = loadGrdLowTemp == null ? 0 : loadGrdLowTemp.LoStr;

            var loadGrd2 = PhaseTraList[8].LoadList.Where(item => item.GKName == "安装情况").FirstOrDefault();
            double noIceTensionGrd2 = loadGrd2 == null ? 0 : loadGrd2.LoStr;

            var loadGrdLowTemp2 = PhaseTraList[8].LoadList.Where(item => item.GKName == "安装情况降温").FirstOrDefault();
            double lowTempTensionGrd2 = loadGrdLowTemp2 == null ? 0 : loadGrdLowTemp2.LoStr;
            AnchorTensionGrd = AnchorTension(noIceTensionGrd1, noIceTensionGrd2, lowTempTensionGrd1, lowTempTensionGrd2);

            var loadOPGW = PhaseTraList[4].LoadList.Where(item => item.GKName == "安装情况").FirstOrDefault();
            double noIceTensionOPGW1 = loadOPGW == null ? 0 : loadOPGW.LoStr;

            var loadOPGWLowTemp = PhaseTraList[4].LoadList.Where(item => item.GKName == "安装情况降温").FirstOrDefault();
            double lowTempTensionOPGW1 = loadOPGWLowTemp == null ? 0 : loadOPGWLowTemp.LoStr;

            var loadOPGW2 = PhaseTraList[9].LoadList.Where(item => item.GKName == "安装情况").FirstOrDefault();
            double noIceTensionOPGW2 = loadOPGW2 == null ? 0 : loadOPGW2.LoStr;

            var loadOPGWLowTemp2 = PhaseTraList[9].LoadList.Where(item => item.GKName == "安装情况降温").FirstOrDefault();
            double lowTempTensionOPGW2 = loadOPGWLowTemp2 == null ? 0 : loadOPGWLowTemp2.LoStr;
            AnchorTensionOPGW = AnchorTension(noIceTensionOPGW1, noIceTensionOPGW2, lowTempTensionOPGW1, lowTempTensionOPGW2);
        }

        public void CalsTensionChcek()
        {
            for (int i = 0; i <= 4; i++)
            {
                var backPhase = PhaseTraList[i];
                var frontPhase = PhaseTraList[i + 5];

                //计算断线张力差 
                var laodBreakBk = backPhase.LoadList.Where(item => item.GKName == "断线").FirstOrDefault();
                double tenMaxBreakBk = laodBreakBk == null ? 0 : laodBreakBk.LoStrCheck2;

                var laodBreakFrt = frontPhase.LoadList.Where(item => item.GKName == "断线").FirstOrDefault();
                double tenMaxBreakFrt = laodBreakFrt == null ? 0 : laodBreakFrt.LoStrCheck2;


                if (backPhase.WireData.bGrd == 0)
                {
                    backPhase.AnSideBreakTenDiff = backPhase.WireData.CommParas.BreakIceCoverPara == 2 ? 0 : tenMaxBreakFrt;
                    frontPhase.AnSideBreakTenDiff = frontPhase.WireData.CommParas.BreakIceCoverPara == 2 ? 0 : tenMaxBreakBk;
                }
                else
                {
                    var laodBreakAdd5Bk = backPhase.LoadList.Where(item => item.GKName == "断线(导线+5mm)").FirstOrDefault();
                    double tenMaxBreakAdd5Bk = laodBreakAdd5Bk == null ? 0 : laodBreakAdd5Bk.LoStrCheck2;

                    var laodBreakAdd5Frt = frontPhase.LoadList.Where(item => item.GKName == "断线(导线+5mm)").FirstOrDefault();
                    double tenMaxBreakAdd5Frt = laodBreakAdd5Frt == null ? 0 : laodBreakAdd5Frt.LoStrCheck2;

                    backPhase.AnSideBreakTenDiff = backPhase.WireData.CommParas.BreakIceCoverPara == 2 ? 0 : (backPhase.WireData.CommParas.GrdIceUnbaPara == 1 ? tenMaxBreakFrt : tenMaxBreakAdd5Frt);
                    frontPhase.AnSideBreakTenDiff = frontPhase.WireData.CommParas.BreakIceCoverPara == 2 ? 0 : (frontPhase.WireData.CommParas.GrdIceUnbaPara == 1 ? tenMaxBreakBk : tenMaxBreakAdd5Bk);

                }

                backPhase.BothSidesBreakTenDiff = "0/" + Math.Round(frontPhase.AnSideBreakTenDiff, 0).ToString();
                frontPhase.BothSidesBreakTenDiff = (Math.Round(backPhase.AnSideBreakTenDiff, 0).ToString() + "/0");

                //验证断线张力差 
                backPhase.AnSideBreakTenDiffCheck = frontPhase.BreakTenDiff;
                frontPhase.AnSideBreakTenDiffCheck = backPhase.BreakTenDiff;

                backPhase.BothSidesBreakTenDiffCheckTemp = backPhase.WireData.CommParas.BreakInPara == 1 ? "0/" + Math.Round(frontPhase.AnSideBreakTenDiffCheck, 0).ToString()
                    : Math.Round(frontPhase.BreakTenMax - backPhase.AnSideBreakTenDiffCheck, 0).ToString() + "/" + Math.Round(backPhase.BreakTenMax, 0).ToString();

                frontPhase.BothSidesBreakTenDiffCheckTemp = frontPhase.WireData.CommParas.BreakInPara == 1 ? Math.Round(backPhase.AnSideBreakTenDiffCheck, 0).ToString() + "/0"
                    : Math.Round(frontPhase.BreakTenMax, 0).ToString() + "/" + Math.Round(frontPhase.BreakTenMax - frontPhase.AnSideBreakTenDiffCheck, 0).ToString();

                backPhase.BothSidesBreakTenDiffCheck = backPhase.AnSideBreakTenDiffCheck > backPhase.AnSideBreakTenDiff ? backPhase.BothSidesBreakTenDiffCheckTemp : backPhase.BothSidesBreakTenDiff;
                frontPhase.BothSidesBreakTenDiffCheck = frontPhase.AnSideBreakTenDiffCheck > frontPhase.AnSideBreakTenDiff ? frontPhase.BothSidesBreakTenDiffCheckTemp : frontPhase.BothSidesBreakTenDiff;

                //计算不均匀冰张立差
                var loadUnabIceIBk = backPhase.LoadList.Where(item => item.GKName == "不均匀冰I").FirstOrDefault();
                double tenMaxUnabIceIBk = loadUnabIceIBk == null ? 0 : loadUnabIceIBk.LoStrCheck2;
                var loadUnabIceIIBk = backPhase.LoadList.Where(item => item.GKName == "不均匀冰II").FirstOrDefault();
                double tenMaxUnabIceIIBk = loadUnabIceIIBk == null ? 0 : loadUnabIceIIBk.LoStrCheck2;

                var loadUnabIceIFrt = frontPhase.LoadList.Where(item => item.GKName == "不均匀冰I").FirstOrDefault();
                double tenMaxUnabIceIFrt = loadUnabIceIFrt == null ? 0 : loadUnabIceIFrt.LoStrCheck2;
                var loadUnabIceIIFrt = frontPhase.LoadList.Where(item => item.GKName == "不均匀冰II").FirstOrDefault();
                double tenMaxUnabIceIIFrt = loadUnabIceIIFrt == null ? 0 : loadUnabIceIIFrt.LoStrCheck2;

                if (backPhase.WireData.bGrd == 0)
                {
                    backPhase.UnbaIceTenIDiffBothSids = Math.Abs(tenMaxUnabIceIBk - tenMaxUnabIceIFrt);
                    frontPhase.UnbaIceTenIDiffBothSids = Math.Abs(tenMaxUnabIceIBk - tenMaxUnabIceIFrt);

                    backPhase.UnbaIceTenIDiff = Math.Round(tenMaxUnabIceIBk, 0);
                    frontPhase.UnbaIceTenIDiff = Math.Round(tenMaxUnabIceIFrt, 0);

                    backPhase.UnbaIceTenIIDiffBothSids = Math.Abs(tenMaxUnabIceIIBk - tenMaxUnabIceIIFrt);
                    frontPhase.UnbaIceTenIIDiffBothSids = Math.Abs(tenMaxUnabIceIIBk - tenMaxUnabIceIIFrt);

                    backPhase.UnbaIceTenIIDiff = Math.Round(tenMaxUnabIceIIBk, 0);
                    frontPhase.UnbaIceTenIIDiff = Math.Round(tenMaxUnabIceIIFrt, 0);
                }
                else
                {
                    var loadUnabIceIAdd5Bk = backPhase.LoadList.Where(item => item.GKName == "不均匀冰I(导线+5mm)").FirstOrDefault();
                    double tenMaxUnabIceIAdd5Bk = loadUnabIceIAdd5Bk == null ? 0 : loadUnabIceIAdd5Bk.LoStrCheck2;
                    var loadUnabIceIIAdd5Bk = backPhase.LoadList.Where(item => item.GKName == "不均匀冰II(导线+5mm)").FirstOrDefault();
                    double tenMaxUnabIceIIAdd5Bk = loadUnabIceIIAdd5Bk == null ? 0 : loadUnabIceIIAdd5Bk.LoStrCheck2;

                    var loadUnabIceIAdd5Frt = frontPhase.LoadList.Where(item => item.GKName == "不均匀冰I(导线+5mm)").FirstOrDefault();
                    double tenMaxUnabIceIAdd5Frt = loadUnabIceIAdd5Frt == null ? 0 : loadUnabIceIAdd5Frt.LoStrCheck2;
                    var loadUnabIceIIAdd5Frt = frontPhase.LoadList.Where(item => item.GKName == "不均匀冰II(导线+5mm)").FirstOrDefault();
                    double tenMaxUnabIceIIAdd5Frt = loadUnabIceIIAdd5Frt == null ? 0 : loadUnabIceIIAdd5Frt.LoStrCheck2;

                    backPhase.UnbaIceTenIDiffBothSids = backPhase.WireData.CommParas.GrdIceUnbaPara == 1 ? Math.Abs(tenMaxUnabIceIBk - tenMaxUnabIceIFrt) : Math.Abs(tenMaxUnabIceIAdd5Bk - tenMaxUnabIceIAdd5Frt);
                    frontPhase.UnbaIceTenIDiffBothSids = frontPhase.WireData.CommParas.GrdIceUnbaPara == 1 ? Math.Abs(tenMaxUnabIceIBk - tenMaxUnabIceIFrt) : Math.Abs(tenMaxUnabIceIAdd5Bk - tenMaxUnabIceIAdd5Frt);

                    backPhase.UnbaIceTenIDiff = backPhase.WireData.CommParas.GrdIceUnbaPara == 1 ? Math.Round(tenMaxUnabIceIBk, 0) : Math.Round(tenMaxUnabIceIAdd5Bk, 0);
                    frontPhase.UnbaIceTenIDiff = frontPhase.WireData.CommParas.GrdIceUnbaPara == 1 ? Math.Round(tenMaxUnabIceIFrt, 0) : Math.Round(tenMaxUnabIceIAdd5Frt, 0);

                    backPhase.UnbaIceTenIIDiffBothSids = backPhase.WireData.CommParas.GrdIceUnbaPara == 1 ? Math.Abs(tenMaxUnabIceIIBk - tenMaxUnabIceIIFrt) : Math.Abs(tenMaxUnabIceIIAdd5Bk - tenMaxUnabIceIIAdd5Frt);
                    frontPhase.UnbaIceTenIIDiffBothSids = frontPhase.WireData.CommParas.GrdIceUnbaPara == 1 ? Math.Abs(tenMaxUnabIceIIBk - tenMaxUnabIceIIFrt) : Math.Abs(tenMaxUnabIceIIAdd5Bk - tenMaxUnabIceIIAdd5Frt);

                    backPhase.UnbaIceTenIIDiff = backPhase.WireData.CommParas.GrdIceUnbaPara == 1 ? Math.Round(tenMaxUnabIceIIBk, 0) : Math.Round(tenMaxUnabIceIIAdd5Bk, 0);
                    frontPhase.UnbaIceTenIIDiff = frontPhase.WireData.CommParas.GrdIceUnbaPara == 1 ? Math.Round(tenMaxUnabIceIIFrt, 0) : Math.Round(tenMaxUnabIceIIAdd5Frt, 0);
                }

                //验证不平衡张力差 
                //两次不均匀冰I这个值都是使用后侧的数据
                backPhase.UnbaIceTenIDiffBothSidsCheck = backPhase.UnbaIceTenDiff;
                frontPhase.UnbaIceTenIDiffBothSidsCheck = backPhase.UnbaIceTenDiff;

                backPhase.UnbaIceTenIDiffCheckTemp = backPhase.WireData.CommParas.UnbaInPara == 1 ? Math.Round(backPhase.UnbaIceTenIDiffBothSidsCheck, 0) : Math.Round(backPhase.UnbaIceTenMax, 0);
                frontPhase.UnbaIceTenIDiffCheckTemp = frontPhase.WireData.CommParas.UnbaInPara == 1 ? 0 : Math.Round(frontPhase.UnbaIceTenMax - frontPhase.UnbaIceTenIDiffBothSidsCheck, 0);

                backPhase.UnbaIceTenIDiffCheck = backPhase.UnbaIceTenIDiffBothSids >= backPhase.UnbaIceTenIDiffBothSidsCheck ? backPhase.UnbaIceTenIDiff : backPhase.UnbaIceTenIDiffCheckTemp;
                frontPhase.UnbaIceTenIDiffCheck = frontPhase.UnbaIceTenIDiffBothSids >= frontPhase.UnbaIceTenIDiffBothSidsCheck ? frontPhase.UnbaIceTenIDiff : frontPhase.UnbaIceTenIDiffCheckTemp;

                //两次不均匀冰II这个值都是使用后侧的数据
                backPhase.UnbaIceTenIIDiffBothSidsCheck = frontPhase.UnbaIceTenDiff;
                frontPhase.UnbaIceTenIIDiffBothSidsCheck = frontPhase.UnbaIceTenDiff;

                backPhase.UnbaIceTenIIDiffCheckTemp = backPhase.WireData.CommParas.UnbaInPara == 1 ? 0 : Math.Round(backPhase.UnbaIceTenMax - backPhase.UnbaIceTenIIDiffBothSidsCheck, 0);    
                frontPhase.UnbaIceTenIIDiffCheckTemp = frontPhase.WireData.CommParas.UnbaInPara == 1 ? Math.Round(frontPhase.UnbaIceTenIIDiffBothSidsCheck, 0) : Math.Round(frontPhase.UnbaIceTenMax, 0);

                backPhase.UnbaIceTenIIDiffCheck = backPhase.UnbaIceTenIIDiffBothSids >= backPhase.UnbaIceTenIIDiffBothSidsCheck ? backPhase.UnbaIceTenIIDiff : backPhase.UnbaIceTenIIDiffCheckTemp;
                frontPhase.UnbaIceTenIIDiffCheck = frontPhase.UnbaIceTenIIDiffBothSids >= frontPhase.UnbaIceTenIIDiffBothSidsCheck ? frontPhase.UnbaIceTenIIDiff : frontPhase.UnbaIceTenIIDiffCheckTemp;

            }
        }


        double BackIndWindHc { get; set; } = 22.10;
        double FrontIndWindHc { get; set; } = 7.62;

        double BackGrdWindHc { get; set; } = 13.04;
        double FrontGrdWindHc { get; set; } = 6.86;

        double BackOPGWWindHc { get; set; } = 13.63;
        double FrontOPGWWindHc { get; set; } = 7.13;

        double AnchorTensionInd { get; set; }
        double AnchorTensionGrd { get; set; }
        double AnchorTensionOPGW { get; set; }

        /// <summary>
        /// 计算大风工况下垂直方向的应用弧垂
        /// </summary>
        public void CalDFCurePY()
        {
            //这部分计算很复杂，但是原理性不足
            //计算导线的大风应用弧垂，先后侧，先前侧
            GdBackHc = AbsDownSideHei - BackSideRes.CommParas.GrdCl;

            double KBackHot = BackSideRes.IndWire.BzDic["最高气温"].VerBizai / 8 / BackSideRes.IndWire.YLTableXls ["最高气温"];
            double KBackIce = BackSideRes.IndWire.BzDic["覆冰无风"].VerBizai / 8 / BackSideRes.IndWire.YLTableXls["覆冰无风"];

            double KMaxHc = Math.Max(KBackHot, KBackIce) * Math.Pow(BackPosRes.Span, 2);
            double MaxIndBackHc;

            //最大弧垂应用值
            if (KBackHot > KBackIce)
            {
                MaxIndBackHc = Math.Min(GdBackHc, KMaxHc) / BackSideRes.IndWire.BzDic["最高气温"].VerBizai * BackSideRes.IndWire.YLTableXls["最高气温"] * BackSideRes.IndWire.BzDic["最大风速"].VerBizai / BackSideRes.IndWire.YLTableXls["最大风速"];
            }
            else
            {
                MaxIndBackHc = Math.Min(GdBackHc, KMaxHc) / BackSideRes.IndWire.BzDic["覆冰无风"].VerBizai * BackSideRes.IndWire.YLTableXls["覆冰无风"] * BackSideRes.IndWire.BzDic["最大风速"].VerBizai / BackSideRes.IndWire.YLTableXls["最大风速"];
            }
            BackIndWindHc = MaxIndBackHc * BackSideRes.IndWire.BzDic["最大风速"].VerBizai / BackSideRes.IndWire.BzDic["最大风速"].g7;

            //计算前侧的大风应用弧垂
            //对地距离控制最大弧垂
            GdFrontHc = TowerAppre.DnSideInHei - FrontSideRes.CommParas.GrdAveHei;
            double KFrontHot = FrontSideRes.IndWire.BzDic["最高气温"].VerBizai / 8 / FrontSideRes.IndWire.YLTableXls["最高气温"];
            double KFrontIce = FrontSideRes.IndWire.BzDic["覆冰无风"].VerBizai / 8 / FrontSideRes.IndWire.YLTableXls["覆冰无风"];
            // 按照K值控制的最大弧垂
            KMaxHc = Math.Max(KFrontHot, KFrontIce) * Math.Pow(FrontPosRes.Span, 2); 
        
            double MaxIndFrontHc; 
            //最大弧垂应用值
            if (KFrontHot > KFrontIce)
            {
                MaxIndFrontHc = Math.Min(GdFrontHc, KMaxHc) / FrontSideRes.IndWire.BzDic["最高气温"].VerBizai * FrontSideRes.IndWire.YLTableXls["最高气温"] * FrontSideRes.IndWire.BzDic["最大风速"].g2 / FrontSideRes.IndWire.YLTableXls["最大风速"];
            }
            else
            {
                MaxIndFrontHc = Math.Min(GdFrontHc, KMaxHc) / FrontSideRes.IndWire.BzDic["覆冰无风"].VerBizai * FrontSideRes.IndWire.YLTableXls["覆冰无风"] * FrontSideRes.IndWire.BzDic["最大风速"].g2 / FrontSideRes.IndWire.YLTableXls["最大风速"];
            }

            //计算前侧大风应用弧垂
            FrontIndWindHc = MaxIndFrontHc * FrontSideRes.IndWire.BzDic["最大风速"].VerBizai / FrontSideRes.IndWire.YLTableXls["最大风速"];

            //计算地线的大风弧垂应用值,先普通地线，再OPGW
            //这部分代码Excel表格有问题，为保持一致，还是采用原来的方法
            double KHot = BackSideRes.GrdWire.BzDic["最高气温"].VerBizai / 8 / BackSideRes.GrdWire.YLTable["最高气温"];
            double KIce = BackSideRes.GrdWire.BzDic["覆冰无风"].VerBizai / 8 / BackSideRes.GrdWire.YLTable["覆冰无风"];
            BackGrdWindHc = MaxIndFrontHc * Math.Max(KHot, KIce) / Math.Max(KBackHot, KBackIce);
            KHot = BackSideRes.OPGWWire.BzDic["最高气温"].VerBizai / 8 / BackSideRes.OPGWWire.YLTable["最高气温"];
            KIce = BackSideRes.OPGWWire.BzDic["覆冰无风"].VerBizai / 8 / BackSideRes.OPGWWire.YLTable["覆冰无风"];
            BackOPGWWindHc = MaxIndFrontHc * Math.Max(KHot, KIce) / Math.Max(KBackHot, KBackIce);

            //前侧计算
            KHot = FrontSideRes.GrdWire.BzDic["最高气温"].VerBizai / 8 / FrontSideRes.GrdWire.YLTable["最高气温"];
            KIce = FrontSideRes.GrdWire.BzDic["覆冰无风"].VerBizai / 8 / FrontSideRes.GrdWire.YLTable["覆冰无风"];
            FrontGrdWindHc = MaxIndFrontHc * Math.Max(KHot, KIce) / Math.Max(KFrontHot, KFrontIce);
            KHot = FrontSideRes.OPGWWire.BzDic["最高气温"].VerBizai / 8 / FrontSideRes.OPGWWire.YLTable["最高气温"];
            KIce = FrontSideRes.OPGWWire.BzDic["覆冰无风"].VerBizai / 8 / FrontSideRes.OPGWWire.YLTable["覆冰无风"];
            FrontOPGWWindHc = MaxIndFrontHc * Math.Max(KHot, KIce) / Math.Max(KFrontHot, KFrontIce);
        }

        double BackIndWindHc1 { get; set; } = 22.10;
        double FrontIndWindHc1 { get; set; } = 7.62;

        double BackGrdWindHc1 { get; set; } = 13.04;
        double FrontGrdWindHc1 { get; set; } = 6.86;

        double BackOPGWWindHc1 { get; set; } = 13.63;
        double FrontOPGWWindHc1 { get; set; } = 7.13;

        double KBackHotInd { get; set; }
        double KBackIceInd { get; set; }

        double KBackHotGrd { get; set; }
        double KBackIceGrd { get; set; }

        double KBackHotOPGW { get; set; }
        double KBackIceOPGW { get; set; }

        double KFrontHotInd { get; set; }
        double KFrontIceInd { get; set; }

        double KFrontHotGrd { get; set; }
        double KFrontIceGrd { get; set; }

        double KFrontHotOPGW { get; set; }
        double KFrontIceOPGW { get; set; }

        double GdBackHc { get; set; }
        double KBackHc { get; set; }

        double GdFrontHc { get; set; }
        double KFrontHc { get; set; }

        double MaxIndBackHc { get; set; }
        double MaxIndFrontHc { get; set; }

        double MaxGrdBackHc { get; set; }
        double MaxGrdFrontHc { get; set; }

        double MaxOPGWBackHc { get; set; }
        double MaxOPGWFrontHc { get; set; }

        public void CalDFCure()
        {
            //这部分计算很复杂，但是原理性不足
            //计算导线的大风应用弧垂，先后侧，先前侧

            KBackHotInd = BackSideRes.IndWire.BzDic["最高气温"].VerBizai / 8 / BackSideRes.IndWire.YLTableXls["最高气温"];
            KBackIceInd = BackSideRes.IndWire.BzDic["覆冰无风"].VerBizai / 8 / BackSideRes.IndWire.YLTableXls["覆冰无风"];

            GdBackHc = AbsDownSideHei - BackSideRes.CommParas.GrdCl;
            KBackHc = Math.Max(KBackHotInd, KBackIceInd) * Math.Pow(BackPosRes.Span, 2);

            //最大弧垂应用值
            if (KBackHotInd > KBackIceInd)
            {
                MaxIndBackHc = Math.Min(GdBackHc, KBackHc) / BackSideRes.IndWire.BzDic["最高气温"].VerBizai * BackSideRes.IndWire.YLTableXls["最高气温"] * BackSideRes.IndWire.BzDic["换算最大风速"].VerBizai / BackSideRes.IndWire.YLTableXls["换算最大风速"];
            }
            else
            {
                MaxIndBackHc = Math.Min(GdBackHc, KBackHc) / BackSideRes.IndWire.BzDic["最大覆冰"].VerBizai * BackSideRes.IndWire.YLTableXls["最大覆冰"] * BackSideRes.IndWire.BzDic["换算最大风速"].VerBizai / BackSideRes.IndWire.YLTableXls["换算最大风速"];
            }
            //-----------------------------------------------------------------------就这儿用的的垂直荷载
            BackIndWindHc1 = MaxIndBackHc * BackSideRes.IndWire.BzDic["换算最大风速"].VerBizai / BackSideRes.IndWire.BzDic["换算最大风速"].BiZai;

            //计算前侧的大风应用弧垂
            KFrontHotInd = FrontSideRes.IndWire.BzDic["最高气温"].VerBizai / 8 / FrontSideRes.IndWire.YLTableXls["最高气温"];
            KFrontIceInd = FrontSideRes.IndWire.BzDic["覆冰无风"].VerBizai / 8 / FrontSideRes.IndWire.YLTableXls["覆冰无风"];
           
            //对地距离控制最大弧垂
            GdFrontHc = AbsDownSideHei - FrontSideRes.CommParas.GrdCl;
            KFrontHc = FrontSideRes.IndWire.BzDic["换算最大风速"].VerBizai / FrontSideRes.IndWire.YLTableXls["换算最大风速"] / 8 * Math.Pow(FrontPosRes.Span, 2);
            MaxIndFrontHc = Math.Min(GdFrontHc, KFrontHc);
            //计算前侧大风应用弧垂
            FrontIndWindHc1 = MaxIndFrontHc * FrontSideRes.IndWire.BzDic["换算最大风速"].HorBizai / FrontSideRes.IndWire.BzDic["换算最大风速"].BiZai;

            //计算地线的大风弧垂应用值,先普通地线，再OPGW
            //这部分代码Excel表格有问题，为保持一致，还是采用原来的方法
            KBackHotGrd = BackSideRes.GrdWire.BzDic["最高气温"].VerBizai / 8 / BackSideRes.GrdWire.YLTableXls["最高气温"];
            KBackIceGrd = BackSideRes.GrdWire.BzDic["最大覆冰"].VerBizai / 8 / BackSideRes.GrdWire.YLTableXls["最大覆冰"];
            MaxGrdBackHc = MaxIndBackHc / Math.Max(KBackHotInd, KBackIceInd) * Math.Max(KBackHotGrd, KBackIceGrd);
            BackGrdWindHc1 = MaxGrdBackHc * BackSideRes.GrdWire.BzDic["换算最大风速"].HorBizai / BackSideRes.GrdWire.BzDic["换算最大风速"].BiZai;

            KBackHotOPGW = BackSideRes.OPGWWire.BzDic["最高气温"].VerBizai / 8 / BackSideRes.OPGWWire.YLTableXls["最高气温"];
            KBackIceOPGW = BackSideRes.OPGWWire.BzDic["最大覆冰"].VerBizai / 8 / BackSideRes.OPGWWire.YLTableXls["覆冰无风"];
            MaxOPGWBackHc = MaxIndBackHc / Math.Max(KBackHotInd, KBackIceInd) * Math.Max(KBackHotOPGW, KBackIceOPGW);
            BackOPGWWindHc1 = MaxOPGWBackHc * BackSideRes.GrdWire.BzDic["换算最大风速"].HorBizai / BackSideRes.GrdWire.BzDic["换算最大风速"].BiZai;

            //前侧计算
            KFrontHotGrd = FrontSideRes.GrdWire.BzDic["最高气温"].VerBizai / 8 / FrontSideRes.GrdWire.YLTableXls["最高气温"];
            KFrontIceGrd = FrontSideRes.GrdWire.BzDic["覆冰无风"].VerBizai / 8 / FrontSideRes.GrdWire.YLTableXls["覆冰无风"];
            MaxGrdFrontHc = MaxIndFrontHc / Math.Max(KFrontHotInd, KFrontIceInd) * Math.Max(KFrontHotGrd, KFrontIceGrd);
            FrontGrdWindHc1 = MaxGrdFrontHc * FrontSideRes.GrdWire.BzDic["换算最大风速"].HorBizai / FrontSideRes.GrdWire.BzDic["换算最大风速"].BiZai;

            KFrontHotOPGW = FrontSideRes.OPGWWire.BzDic["最高气温"].VerBizai / 8 / FrontSideRes.OPGWWire.YLTableXls["最高气温"];
            KFrontIceOPGW = FrontSideRes.OPGWWire.BzDic["覆冰无风"].VerBizai / 8 / FrontSideRes.OPGWWire.YLTableXls["覆冰无风"];
            MaxOPGWFrontHc = MaxIndFrontHc / Math.Max(KFrontHotInd, KFrontIceInd) * Math.Max(KFrontHotOPGW, KFrontIceOPGW);
            FrontOPGWWindHc1 = MaxOPGWFrontHc * FrontSideRes.OPGWWire.BzDic["换算最大风速"].HorBizai / FrontSideRes.OPGWWire.BzDic["换算最大风速"].BiZai;
        }



        #region 内部计算函数

        /// <summary>
        /// 计算线高空风压系数
        /// </summary>
        /// <param name="wireWindPara">计算方式 1：线平均高 2:按照下相挂点高反算 </param>
        /// <param name="wireHei">导线挂点高</param>
        /// <param name="wireWindVerSag">大风垂直方向弧垂</param>
        /// <param name="avaHei">导线计算平均高</param>
        /// <returns></returns>
        protected double CalsWireWindPara(int wireWindPara, double wireHei, double wireWindVerSag, double avaHei)
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
        /// 锚线张力
        /// </summary>
        /// <param name="noIceTension1">小号侧 安装情况（无冰）线条张力</param>
        /// <param name="noIceTension2">大号侧 安装情况（无冰）线条张力</param>
        /// <param name="lowTempTension1">小号侧 安装情况（降温）线条张力<</param>
        /// <param name="lowTempTension2">小号侧 安装情况（降温）线条张力<</param>
        /// <returns></returns>
        protected double AnchorTension(double noIceTension1, double noIceTension2, double lowTempTension1, double lowTempTension2)
        {
            //"系数法"
            if (FrontSideRes.CommParas.HandForcePara == 2)
            {
                return Math.Max(noIceTension1, noIceTension2);
            }
            //降温法
            else if (FrontSideRes.CommParas.HandForcePara == 3)
            {
                return Math.Max(lowTempTension1, lowTempTension2);
            }
            //取两者最大值
            else
            {
                return Math.Max(Math.Max(noIceTension1, noIceTension2), Math.Max(lowTempTension1, lowTempTension2));
            }
        }
        #endregion

        #region 用于打印输出的函数
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

        public List<string> PrintCalsReslt()
        {
            List<string> relt = new List<string>();

            relt.Add("\n垂直档距计算：");
            for (int i = 0; i <= 4; i++)
            {
                relt.Add(FileUtils.PadRightEx("小号侧" + i, 118) + FileUtils.PadRightEx("大号侧" + i, 118));
                relt.Add(FileUtils.PadRightEx("工况", 26) + FileUtils.PadRightEx("计算过程：", 80) + FileUtils.PadRightEx("计算值：", 12)
                    + FileUtils.PadRightEx("工况", 26) + FileUtils.PadRightEx("计算过程：", 80) + FileUtils.PadRightEx("计算值：", 12));

                var phase = PhaseTraList[i];
                var phaseAn = PhaseTraList[i + 5];

                foreach (var name in phase.WireData.WorkCdtNames)
                {
                    var laod = phase.LoadList.Where(item => item.GKName == name).FirstOrDefault();
                    double verSpan = laod == null ? 0 : laod.VetiSpan;
                    string verSpanStr = laod == null ? "" : laod.VetiSpanStr;

                    var laodAn = phaseAn.LoadList.Where(item => item.GKName == name).FirstOrDefault();
                    double verSpanAn = phaseAn == null ? 0 : laodAn.VetiSpan;
                    string verSpanStrAn = phaseAn == null ? "" : laodAn.VetiSpanStr;

                    //relt.Add(FileUtils.PadRightEx(name, 26) + FileUtils.PadRightEx(phase.VerSpanStrDic[name], 80) + FileUtils.PadRightEx(phase.VerSpanDic[name].ToString("0.###"), 12)
                    //    + FileUtils.PadRightEx(name, 26) + FileUtils.PadRightEx(phaseAn.VerSpanStrDic[name], 80) + FileUtils.PadRightEx(phaseAn.VerSpanDic[name].ToString("0.###"), 12));

                    relt.Add(FileUtils.PadRightEx(name, 26) + FileUtils.PadRightEx(verSpanStr, 80) + FileUtils.PadRightEx(verSpan.ToString("0.###"), 12)
                        + FileUtils.PadRightEx(name, 26) + FileUtils.PadRightEx(verSpanStrAn, 80) + FileUtils.PadRightEx(verSpanAn.ToString("0.###"), 12));
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
                    var laod = phase.LoadList.Where(item => item.GKName == name).FirstOrDefault();
                    double horFor = laod == null ? 0 : laod.HorFor;
                    string horForStr = laod == null ? "" : laod.HorForStr;

                    var laodAn = phaseAn.LoadList.Where(item => item.GKName == name).FirstOrDefault();
                    double horForAn = phaseAn == null ? 0 : laodAn.HorFor;
                    string horForStrAn = phaseAn == null ? "" : laodAn.HorForStr;

                    //relt.Add(FileUtils.PadRightEx(name, 26) + FileUtils.PadRightEx(phase.HoriLoadStrDic[name], 80) + FileUtils.PadRightEx(phase.HoriLoadDic[name].ToString("0.###"), 12)
                    //    + FileUtils.PadRightEx(name, 26) + FileUtils.PadRightEx(phaseAn.HoriLoadStrDic[name], 80) + FileUtils.PadRightEx(phaseAn.HoriLoadDic[name].ToString("0.###"), 12));

                    relt.Add(FileUtils.PadRightEx(name, 26) + FileUtils.PadRightEx(horForStr, 80) + FileUtils.PadRightEx(horFor.ToString("0.###"), 12)
                        + FileUtils.PadRightEx(name, 26) + FileUtils.PadRightEx(horForStrAn, 80) + FileUtils.PadRightEx(horForAn.ToString("0.###"), 12));
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
                    var laod = phase.LoadList.Where(item => item.GKName == name).FirstOrDefault();
                    double verWei = laod == null ? 0 : laod.VerWei;
                    string verWeiStr = laod == null ? "" : laod.VerWeiStr;

                    var laodAn = phaseAn.LoadList.Where(item => item.GKName == name).FirstOrDefault();
                    double verWeiAn = phaseAn == null ? 0 : laodAn.VerWei;
                    string verWeiStrAn = phaseAn == null ? "" : laodAn.VerWeiStr;

                    //relt.Add(FileUtils.PadRightEx(name, 26) + FileUtils.PadRightEx(phase.VerLoadStrDic[name], 80) + FileUtils.PadRightEx(phase.VerLoadDic[name].ToString("0.###"), 12)
                    //    + FileUtils.PadRightEx(name, 26) + FileUtils.PadRightEx(phaseAn.VerLoadStrDic[name], 80) + FileUtils.PadRightEx(phaseAn.VerLoadDic[name].ToString("0.###"), 12));

                    relt.Add(FileUtils.PadRightEx(name, 26) + FileUtils.PadRightEx(verWeiStr, 80) + FileUtils.PadRightEx(verWei.ToString("0.###"), 12)
                        + FileUtils.PadRightEx(name, 26) + FileUtils.PadRightEx(verWeiStrAn, 80) + FileUtils.PadRightEx(verWeiAn.ToString("0.###"), 12));
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
                    var laod = phase.LoadList.Where(item => item.GKName == name).FirstOrDefault();
                    double loStr = laod == null ? 0 : laod.LoStr;
                    string loStrStr = laod == null ? "" : laod.LoStrStr;
                    string loStrCheckStr = laod == null ? "" : laod.LoStrCheck;

                    var laodAn = phaseAn.LoadList.Where(item => item.GKName == name).FirstOrDefault();
                    double loStrAn = phaseAn == null ? 0 : laodAn.LoStr;
                    string loStrStrAn = phaseAn == null ? "" : laodAn.LoStrStr;
                    string loStrCheckStrAn = phaseAn == null ? "" : laodAn.LoStrCheck;

                    //relt.Add(FileUtils.PadRightEx(name, 26) + FileUtils.PadRightEx(phase.TensionStrDic[name], 80) + FileUtils.PadRightEx(phase.TensionDic[name].ToString("0.###"), 12)
                    //    + FileUtils.PadRightEx(name, 26) + FileUtils.PadRightEx(phaseAn.TensionStrDic[name], 80) + FileUtils.PadRightEx(phaseAn.TensionDic[name].ToString("0.###"), 12));

                    string str = FileUtils.PadRightEx(name, 26) + FileUtils.PadRightEx(loStrStr, 80) + FileUtils.PadRightEx(loStr.ToString("0.###"), 12)
                        + FileUtils.PadRightEx(name, 26) + FileUtils.PadRightEx(loStrStrAn, 80) + FileUtils.PadRightEx(loStrAn.ToString("0.###"), 12)
                        + FileUtils.PadRightEx(loStrCheckStr, 20) + FileUtils.PadRightEx(loStrCheckStrAn, 20);

                    if (name == "断线" || name == "不均匀冰I" || name == "不均匀冰II" || name == "断线(导线+5mm)" || name == "不均匀冰I(导线+5mm)" || name == "不均匀冰II(导线+5mm)")
                    {
                        double loStrCheck2Str = laod == null ? 0 : laod.LoStrCheck2;
                        double loStrCheck2StrAn = phaseAn == null ? 0 : laodAn.LoStrCheck2;

                        str += FileUtils.PadRightEx(loStrCheck2Str.ToString(), 10) + FileUtils.PadRightEx(loStrCheck2StrAn.ToString(), 10);
                    }


                    relt.Add(str);
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
                    var laod = phase.LoadList.Where(item => item.GKName == name).FirstOrDefault();
                    double jumpHorFor = laod == null ? 0 : laod.JumpHorFor;
                    string jumpHorForStr = laod == null ? "" : laod.JumpHorForStr;

                    var laodAn = phaseAn.LoadList.Where(item => item.GKName == name).FirstOrDefault();
                    double jumpHorForAn = phaseAn == null ? 0 : laodAn.JumpHorFor;
                    string jumpHorForStrAn = phaseAn == null ? "" : laodAn.JumpHorForStr;

                    //relt.Add(FileUtils.PadRightEx(name, 26) + FileUtils.PadRightEx(phase.JumpStrHorLoadStrDic[name], 80) + FileUtils.PadRightEx(phase.JumpStrHorLoadDic[name].ToString("0.###"), 12)
                    //    + FileUtils.PadRightEx(name, 26) + FileUtils.PadRightEx(phaseAn.JumpStrHorLoadStrDic[name], 80) + FileUtils.PadRightEx(phaseAn.JumpStrHorLoadDic[name].ToString("0.###"), 12));

                    relt.Add(FileUtils.PadRightEx(name, 26) + FileUtils.PadRightEx(jumpHorForStr, 80) + FileUtils.PadRightEx(jumpHorFor.ToString("0.###"), 12)
                        + FileUtils.PadRightEx(name, 26) + FileUtils.PadRightEx(jumpHorForStrAn, 80) + FileUtils.PadRightEx(jumpHorForAn.ToString("0.###"), 12));
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
                    var laod = phase.LoadList.Where(item => item.GKName == name).FirstOrDefault();
                    double jumpVerWei = laod == null ? 0 : laod.JumpVerWei;
                    string jumpVerWeiStr = laod == null ? "" : laod.JumpVerWeiStr;

                    var laodAn = phaseAn.LoadList.Where(item => item.GKName == name).FirstOrDefault();
                    double jumpVerWeiAn = phaseAn == null ? 0 : laodAn.JumpVerWei;
                    string jumpVerWeiStrAn = phaseAn == null ? "" : laodAn.JumpVerWeiStr;

                    //relt.Add(FileUtils.PadRightEx(name, 26) + FileUtils.PadRightEx(phase.JumpStrVerLoadStrDic[name], 110) + FileUtils.PadRightEx(phase.JumpStrVerLoadDic[name].ToString("0.###"), 12)
                    //    + FileUtils.PadRightEx(name, 26) + FileUtils.PadRightEx(phaseAn.JumpStrVerLoadStrDic[name], 110) + FileUtils.PadRightEx(phaseAn.JumpStrVerLoadDic[name].ToString("0.###"), 12));
                    relt.Add(FileUtils.PadRightEx(name, 26) + FileUtils.PadRightEx(jumpVerWeiStr, 110) + FileUtils.PadRightEx(jumpVerWei.ToString("0.###"), 12)
                        + FileUtils.PadRightEx(name, 26) + FileUtils.PadRightEx(jumpVerWeiStrAn, 110) + FileUtils.PadRightEx(jumpVerWeiAn.ToString("0.###"), 12));
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
            relt.Add(FileUtils.PadRightEx("导线事故断线工况", 26) + FileUtils.PadRightEx(PhaseTraList[0].BreakTenDiffStr, 50) + FileUtils.PadRightEx(PhaseTraList[0].BreakTenDiff.ToString("0.##"), 16)
                + FileUtils.PadRightEx("导线事故断线工况", 26) + FileUtils.PadRightEx(PhaseTraList[5].BreakTenDiffStr, 50) + FileUtils.PadRightEx(PhaseTraList[5].BreakTenDiff.ToString("0.##"), 16)
                + FileUtils.PadRightEx(PhaseTraList[0].BreakTenMaxTemp.ToString("0.##"), 20) + FileUtils.PadRightEx(PhaseTraList[5].BreakTenMaxTemp.ToString("0.##"), 16)
                + FileUtils.PadRightEx(PhaseTraList[0].BreakTenMax.ToString("0.##"), 20) + FileUtils.PadRightEx(PhaseTraList[5].BreakTenMax.ToString("0.##"), 16));
            relt.Add(FileUtils.PadRightEx("地线事故断线工况", 26) + FileUtils.PadRightEx(PhaseTraList[3].BreakTenDiffStr, 50) + FileUtils.PadRightEx(PhaseTraList[3].BreakTenDiff.ToString("0.##"), 16)
                + FileUtils.PadRightEx("地线事故断线工况", 26) + FileUtils.PadRightEx(PhaseTraList[8].BreakTenDiffStr, 50) + FileUtils.PadRightEx(PhaseTraList[8].BreakTenDiff.ToString("0.##"), 16)
                + FileUtils.PadRightEx(PhaseTraList[3].BreakTenMaxTemp.ToString("0.##"), 20) + FileUtils.PadRightEx(PhaseTraList[8].BreakTenMaxTemp.ToString("0.##"), 16)
                + FileUtils.PadRightEx(PhaseTraList[3].BreakTenMax.ToString("0.##"), 20) + FileUtils.PadRightEx(PhaseTraList[8].BreakTenMax.ToString("0.##"), 16));
            relt.Add(FileUtils.PadRightEx("地线事故断线工况", 26) + FileUtils.PadRightEx(PhaseTraList[4].BreakTenDiffStr, 50) + FileUtils.PadRightEx(PhaseTraList[4].BreakTenDiff.ToString("0.##"), 16)
                + FileUtils.PadRightEx("导线事故断线工况", 26) + FileUtils.PadRightEx(PhaseTraList[9].BreakTenDiffStr, 50) + FileUtils.PadRightEx(PhaseTraList[9].BreakTenDiff.ToString("0.##"), 16)
                + FileUtils.PadRightEx(PhaseTraList[4].BreakTenMaxTemp.ToString("0.##"), 20) + FileUtils.PadRightEx(PhaseTraList[9].BreakTenMaxTemp.ToString("0.##"), 16)
                + FileUtils.PadRightEx(PhaseTraList[4].BreakTenMax.ToString("0.##"), 20) + FileUtils.PadRightEx(PhaseTraList[9].BreakTenMax.ToString("0.##"), 16));

            relt.Add(FileUtils.PadRightEx("导线不均匀冰工况", 26) + FileUtils.PadRightEx(PhaseTraList[0].UnbaIceTenDiffStr, 50) + FileUtils.PadRightEx(PhaseTraList[0].UnbaIceTenDiff.ToString("0.##"), 16)
                + FileUtils.PadRightEx("导线不均匀冰工况", 26) + FileUtils.PadRightEx(PhaseTraList[5].UnbaIceTenDiffStr, 50) + FileUtils.PadRightEx(PhaseTraList[5].UnbaIceTenDiff.ToString("0.##"), 16)
                + FileUtils.PadRightEx(PhaseTraList[0].UnbaIceTenMaxTemp.ToString("0.##"), 20) + FileUtils.PadRightEx(PhaseTraList[5].UnbaIceTenMaxTemp.ToString("0.##"), 16)
                + FileUtils.PadRightEx(PhaseTraList[0].UnbaIceTenMax.ToString("0.##"), 20) + FileUtils.PadRightEx(PhaseTraList[5].UnbaIceTenMax.ToString("0.##"), 16));
            relt.Add(FileUtils.PadRightEx("地线不均匀冰工况", 26) + FileUtils.PadRightEx(PhaseTraList[3].UnbaIceTenDiffStr, 50) + FileUtils.PadRightEx(PhaseTraList[3].UnbaIceTenDiff.ToString("0.##"), 16)
                + FileUtils.PadRightEx("地线不均匀冰工况", 26) + FileUtils.PadRightEx(PhaseTraList[8].UnbaIceTenDiffStr, 50) + FileUtils.PadRightEx(PhaseTraList[8].UnbaIceTenDiff.ToString("0.##"), 16)
                + FileUtils.PadRightEx(PhaseTraList[3].UnbaIceTenMaxTemp.ToString("0.##"), 20) + FileUtils.PadRightEx(PhaseTraList[8].UnbaIceTenMaxTemp.ToString("0.##"), 16)
                + FileUtils.PadRightEx(PhaseTraList[3].UnbaIceTenMax.ToString("0.##"), 20) + FileUtils.PadRightEx(PhaseTraList[8].UnbaIceTenMax.ToString("0.##"), 16));
            relt.Add(FileUtils.PadRightEx("地线不均匀冰工况", 26) + FileUtils.PadRightEx(PhaseTraList[4].UnbaIceTenDiffStr, 50) + FileUtils.PadRightEx(PhaseTraList[4].UnbaIceTenDiff.ToString("0.##"), 16)
                + FileUtils.PadRightEx("地线不均匀冰工况", 26) + FileUtils.PadRightEx(PhaseTraList[9].UnbaIceTenDiffStr, 50) + FileUtils.PadRightEx(PhaseTraList[9].UnbaIceTenDiff.ToString("0.##"), 16)
                + FileUtils.PadRightEx(PhaseTraList[4].UnbaIceTenMaxTemp.ToString("0.##"), 20) + FileUtils.PadRightEx(PhaseTraList[9].UnbaIceTenMaxTemp.ToString("0.##"), 16)
                + FileUtils.PadRightEx(PhaseTraList[4].UnbaIceTenMax.ToString("0.##"), 20) + FileUtils.PadRightEx(PhaseTraList[9].UnbaIceTenMax.ToString("0.##"), 16));

            return relt;
        }

        public List <string> PrintBreakUnabCheck()
        {
            List<string> rslt = new List<string>();

            rslt.Add("\n断线、不均匀冰验算：");
            rslt.Add(FileUtils.PadRightEx("断线张力差", 20) + FileUtils.PadRightEx("小号侧断线大号侧张力", 26) + FileUtils.PadRightEx("大号侧断线小号侧张力", 26) + FileUtils.PadRightEx("小号侧断线", 26) + FileUtils.PadRightEx("大号侧断线", 26));
            rslt.Add(FileUtils.PadRightEx("导线", 20) + FileUtils.PadRightEx(PhaseTraList[0].AnSideBreakTenDiff.ToString("0.00"), 26) + FileUtils.PadRightEx(PhaseTraList[5].AnSideBreakTenDiff.ToString("0.00"), 26) 
                + FileUtils.PadRightEx(PhaseTraList[0].BothSidesBreakTenDiff, 26) + FileUtils.PadRightEx(PhaseTraList[5].BothSidesBreakTenDiff, 26));
            rslt.Add(FileUtils.PadRightEx("地线1", 20) + FileUtils.PadRightEx(PhaseTraList[3].AnSideBreakTenDiff.ToString("0.00"), 26) + FileUtils.PadRightEx(PhaseTraList[8].AnSideBreakTenDiff.ToString("0.00"), 26)
                + FileUtils.PadRightEx(PhaseTraList[3].BothSidesBreakTenDiff, 26) + FileUtils.PadRightEx(PhaseTraList[8].BothSidesBreakTenDiff, 26));
            rslt.Add(FileUtils.PadRightEx("地线2", 20) + FileUtils.PadRightEx(PhaseTraList[4].AnSideBreakTenDiff.ToString("0.00"), 26) + FileUtils.PadRightEx(PhaseTraList[9].AnSideBreakTenDiff.ToString("0.00"), 26)
                + FileUtils.PadRightEx(PhaseTraList[4].BothSidesBreakTenDiff, 26) + FileUtils.PadRightEx(PhaseTraList[9].BothSidesBreakTenDiff, 26));

            rslt.Add(FileUtils.PadRightEx("断线张力差验算", 20) + FileUtils.PadRightEx("小号侧断线大号侧张力", 26) + FileUtils.PadRightEx("大号侧断线小号侧张力", 26) + FileUtils.PadRightEx("小号侧断线", 26) + FileUtils.PadRightEx("大号侧断线", 26)
                + FileUtils.PadRightEx("小号侧断线最终值", 26) + FileUtils.PadRightEx("大号侧断线最终值", 26));

            rslt.Add(FileUtils.PadRightEx("导线", 20) + FileUtils.PadRightEx(PhaseTraList[0].AnSideBreakTenDiffCheck.ToString("0.00"), 26) + FileUtils.PadRightEx(PhaseTraList[5].AnSideBreakTenDiffCheck.ToString("0.00"), 26) 
                + FileUtils.PadRightEx(PhaseTraList[0].BothSidesBreakTenDiffCheckTemp, 26) + FileUtils.PadRightEx(PhaseTraList[5].BothSidesBreakTenDiffCheckTemp, 26)
                + FileUtils.PadRightEx(PhaseTraList[0].BothSidesBreakTenDiffCheck, 26) + FileUtils.PadRightEx(PhaseTraList[5].BothSidesBreakTenDiffCheck, 26));
            rslt.Add(FileUtils.PadRightEx("地线1", 20) + FileUtils.PadRightEx(PhaseTraList[3].AnSideBreakTenDiffCheck.ToString("0.00"), 26) + FileUtils.PadRightEx(PhaseTraList[8].AnSideBreakTenDiffCheck.ToString("0.00"), 26)
                + FileUtils.PadRightEx(PhaseTraList[3].BothSidesBreakTenDiffCheckTemp, 26) + FileUtils.PadRightEx(PhaseTraList[8].BothSidesBreakTenDiffCheckTemp, 26)
                + FileUtils.PadRightEx(PhaseTraList[3].BothSidesBreakTenDiffCheck, 26) + FileUtils.PadRightEx(PhaseTraList[8].BothSidesBreakTenDiffCheck, 26));
            rslt.Add(FileUtils.PadRightEx("地线2", 20) + FileUtils.PadRightEx(PhaseTraList[4].AnSideBreakTenDiffCheck.ToString("0.00"), 26) + FileUtils.PadRightEx(PhaseTraList[9].AnSideBreakTenDiffCheck.ToString("0.00"), 26)
                + FileUtils.PadRightEx(PhaseTraList[4].BothSidesBreakTenDiffCheckTemp, 26) + FileUtils.PadRightEx(PhaseTraList[9].BothSidesBreakTenDiffCheckTemp, 26)
                + FileUtils.PadRightEx(PhaseTraList[4].BothSidesBreakTenDiffCheck, 26) + FileUtils.PadRightEx(PhaseTraList[9].BothSidesBreakTenDiffCheck, 26));

            rslt.Add(FileUtils.PadRightEx("断线张力差", 20) + FileUtils.PadRightEx("工况", 20) + FileUtils.PadRightEx("张力差", 20) + FileUtils.PadRightEx("小号侧", 20) + FileUtils.PadRightEx("大号侧", 20));
            rslt.Add(FileUtils.PadRightEx("导线", 20) + FileUtils.PadRightEx("不均匀覆冰I", 20) + FileUtils.PadRightEx(PhaseTraList[0].UnbaIceTenIDiffBothSids.ToString("0.00"), 20) + FileUtils.PadRightEx(PhaseTraList[0].UnbaIceTenIDiff.ToString("0.00"), 20) + FileUtils.PadRightEx(PhaseTraList[5].UnbaIceTenIDiff.ToString("0.00"), 20));
            rslt.Add(FileUtils.PadRightEx("导线", 20) + FileUtils.PadRightEx("不均匀覆冰II", 20) + FileUtils.PadRightEx(PhaseTraList[0].UnbaIceTenIIDiffBothSids.ToString("0.00"), 20) + FileUtils.PadRightEx(PhaseTraList[0].UnbaIceTenIIDiff.ToString("0.00"), 20) + FileUtils.PadRightEx(PhaseTraList[5].UnbaIceTenIIDiff.ToString("0.00"), 20));
            rslt.Add(FileUtils.PadRightEx("地线1", 20) + FileUtils.PadRightEx("不均匀覆冰I", 20) + FileUtils.PadRightEx(PhaseTraList[3].UnbaIceTenIDiffBothSids.ToString("0.00"), 20) + FileUtils.PadRightEx(PhaseTraList[0].UnbaIceTenIDiff.ToString("0.00"), 20) + FileUtils.PadRightEx(PhaseTraList[8].UnbaIceTenIDiff.ToString("0.00"), 20));
            rslt.Add(FileUtils.PadRightEx("地线1", 20) + FileUtils.PadRightEx("不均匀覆冰II", 20) + FileUtils.PadRightEx(PhaseTraList[3].UnbaIceTenIIDiffBothSids.ToString("0.00"), 20) + FileUtils.PadRightEx(PhaseTraList[0].UnbaIceTenIIDiff.ToString("0.00"), 20) + FileUtils.PadRightEx(PhaseTraList[8].UnbaIceTenIIDiff.ToString("0.00"), 20));
            rslt.Add(FileUtils.PadRightEx("地线2", 20) + FileUtils.PadRightEx("不均匀覆冰I", 20) + FileUtils.PadRightEx(PhaseTraList[4].UnbaIceTenIDiffBothSids.ToString("0.00"), 20) + FileUtils.PadRightEx(PhaseTraList[0].UnbaIceTenIDiff.ToString("0.00"), 20) + FileUtils.PadRightEx(PhaseTraList[9].UnbaIceTenIDiff.ToString("0.00"), 20));
            rslt.Add(FileUtils.PadRightEx("地线2", 20) + FileUtils.PadRightEx("不均匀覆冰II", 20) + FileUtils.PadRightEx(PhaseTraList[4].UnbaIceTenIIDiffBothSids.ToString("0.00"), 20) + FileUtils.PadRightEx(PhaseTraList[0].UnbaIceTenIIDiff.ToString("0.00"), 20) + FileUtils.PadRightEx(PhaseTraList[9].UnbaIceTenIIDiff.ToString("0.00"), 20));

            rslt.Add(FileUtils.PadRightEx("断线张力验算", 20) + FileUtils.PadRightEx("工况", 20) + FileUtils.PadRightEx("张力差", 20) + FileUtils.PadRightEx("小号侧", 20) + FileUtils.PadRightEx("大号侧", 20) + FileUtils.PadRightEx("小号侧最终值", 20) + FileUtils.PadRightEx("大号侧最终值", 20));
            rslt.Add(FileUtils.PadRightEx("导线", 20) + FileUtils.PadRightEx("不均匀覆冰I", 20) + FileUtils.PadRightEx(PhaseTraList[0].UnbaIceTenIDiffBothSidsCheck.ToString("0.00"), 20) + FileUtils.PadRightEx(PhaseTraList[0].UnbaIceTenIDiffCheckTemp.ToString("0.00"), 20)
                + FileUtils.PadRightEx(PhaseTraList[5].UnbaIceTenIDiffCheckTemp.ToString("0.00"), 20) + FileUtils.PadRightEx(PhaseTraList[0].UnbaIceTenIDiffCheck.ToString("0.00"), 20) + FileUtils.PadRightEx(PhaseTraList[5].UnbaIceTenIDiffCheck.ToString("0.00"), 20));
            rslt.Add(FileUtils.PadRightEx("导线", 20) + FileUtils.PadRightEx("不均匀覆冰II", 20) + FileUtils.PadRightEx(PhaseTraList[0].UnbaIceTenIIDiffBothSidsCheck.ToString("0.00"), 20)+ FileUtils.PadRightEx(PhaseTraList[0].UnbaIceTenIIDiffCheckTemp.ToString("0.00"), 20)
                + FileUtils.PadRightEx(PhaseTraList[5].UnbaIceTenIIDiffCheckTemp.ToString("0.00"), 20) + FileUtils.PadRightEx(PhaseTraList[0].UnbaIceTenIIDiffCheck.ToString("0.00"), 20) + FileUtils.PadRightEx(PhaseTraList[5].UnbaIceTenIIDiffCheck.ToString("0.00"), 20));
            
            rslt.Add(FileUtils.PadRightEx("地线1", 20) + FileUtils.PadRightEx("不均匀覆冰I", 20) + FileUtils.PadRightEx(PhaseTraList[3].UnbaIceTenIDiffBothSidsCheck.ToString("0.00"), 20)  + FileUtils.PadRightEx(PhaseTraList[3].UnbaIceTenIDiffCheckTemp.ToString("0.00"), 20)
                + FileUtils.PadRightEx(PhaseTraList[8].UnbaIceTenIDiffCheckTemp.ToString("0.00"), 20) + FileUtils.PadRightEx(PhaseTraList[3].UnbaIceTenIDiffCheck.ToString("0.00"), 20) + FileUtils.PadRightEx(PhaseTraList[8].UnbaIceTenIDiffCheck.ToString("0.00"), 20));
            rslt.Add(FileUtils.PadRightEx("地线1", 20) + FileUtils.PadRightEx("不均匀覆冰II", 20) + FileUtils.PadRightEx(PhaseTraList[3].UnbaIceTenIIDiffBothSidsCheck.ToString("0.00"), 20) + FileUtils.PadRightEx(PhaseTraList[3].UnbaIceTenIIDiffCheckTemp.ToString("0.00"), 20)
                + FileUtils.PadRightEx(PhaseTraList[8].UnbaIceTenIIDiffCheckTemp.ToString("0.00"), 20) + FileUtils.PadRightEx(PhaseTraList[3].UnbaIceTenIIDiffCheck.ToString("0.00"), 20) + FileUtils.PadRightEx(PhaseTraList[8].UnbaIceTenIIDiffCheck.ToString("0.00"), 20));
            rslt.Add(FileUtils.PadRightEx("地线2", 20) + FileUtils.PadRightEx("不均匀覆冰I", 20) + FileUtils.PadRightEx(PhaseTraList[4].UnbaIceTenIDiffBothSidsCheck.ToString("0.00"), 20) + FileUtils.PadRightEx(PhaseTraList[4].UnbaIceTenIDiffCheckTemp.ToString("0.00"), 20)
                + FileUtils.PadRightEx(PhaseTraList[9].UnbaIceTenIDiffCheckTemp.ToString("0.00"), 20) + FileUtils.PadRightEx(PhaseTraList[4].UnbaIceTenIDiffCheck.ToString("0.00"), 20) + FileUtils.PadRightEx(PhaseTraList[9].UnbaIceTenIDiffCheck.ToString("0.00"), 20));
            rslt.Add(FileUtils.PadRightEx("地线2", 20) + FileUtils.PadRightEx("不均匀覆冰II", 20) + FileUtils.PadRightEx(PhaseTraList[4].UnbaIceTenIIDiffBothSidsCheck.ToString("0.00"), 20) + FileUtils.PadRightEx(PhaseTraList[4].UnbaIceTenIIDiffCheckTemp.ToString("0.00"), 20)
                + FileUtils.PadRightEx(PhaseTraList[9].UnbaIceTenIIDiffCheckTemp.ToString("0.00"), 20) + FileUtils.PadRightEx(PhaseTraList[4].UnbaIceTenIIDiffCheck.ToString("0.00"), 20) + FileUtils.PadRightEx(PhaseTraList[9].UnbaIceTenIIDiffCheck.ToString("0.00"), 20));
            return rslt;

        }

        public List<string> PrintAnchor()
        {
            List<string> rslt = new List<string>();
            rslt.Add(FileUtils.PadRightEx("\n锚线张力：", 12) + FileUtils.PadRightEx(AnchorTensionInd.ToString("0"), 10));
            rslt.Add(FileUtils.PadRightEx("一边地线：", 12) + FileUtils.PadRightEx(AnchorTensionGrd.ToString("0"), 10) + FileUtils.PadRightEx("另一边地线：", 12) + FileUtils.PadRightEx(AnchorTensionOPGW.ToString("0"), 10));
            return rslt;
        }

        public List<string> PrintDFCure()
        {
            List<string> rslt = new List<string>();
            rslt.Add(FileUtils.PadRightEx("\n弧垂", 16) +  FileUtils.PadRightEx("小号侧", 36) + FileUtils.PadRightEx("大号侧", 36));
            rslt.Add(FileUtils.PadRightEx("导线：", 28) + FileUtils.PadRightEx("地线：", 12) + FileUtils.PadRightEx("OPGW：", 12) + FileUtils.PadRightEx("导线：", 12) + FileUtils.PadRightEx("地线：", 12) + FileUtils.PadRightEx("OPGW：", 12));
            rslt.Add(FileUtils.PadRightEx("高温K值：", 16) + FileUtils.PadRightEx(KBackHotInd.ToString("e2"), 12) + FileUtils.PadRightEx(KBackHotGrd.ToString("e2"), 12) + FileUtils.PadRightEx(KBackHotOPGW.ToString("e2"), 12)
                + FileUtils.PadRightEx(KFrontHotInd.ToString("e2"), 12) + FileUtils.PadRightEx(KFrontHotGrd.ToString("e2"), 12) + FileUtils.PadRightEx(KFrontHotOPGW.ToString("e2"), 12));
            rslt.Add(FileUtils.PadRightEx("有冰无风K值：", 16) + FileUtils.PadRightEx(KBackIceInd.ToString("e2"), 12) + FileUtils.PadRightEx(KBackIceGrd.ToString("e2"), 12) + FileUtils.PadRightEx(KBackIceOPGW.ToString("e2"), 12)
                 + FileUtils.PadRightEx(KFrontIceInd.ToString("e2"), 12) + FileUtils.PadRightEx(KFrontIceGrd.ToString("e2"), 12) + FileUtils.PadRightEx(KFrontIceOPGW.ToString("e2"), 12));
            rslt.Add(FileUtils.PadRightEx("最大弧垂对地：", 16) + FileUtils.PadRightEx(GdBackHc.ToString("0.00"), 36) + FileUtils.PadRightEx(GdFrontHc.ToString("0.00"), 36));
            rslt.Add(FileUtils.PadRightEx("最大弧垂K：", 16) + FileUtils.PadRightEx(KBackHc.ToString("0.00"), 36) + FileUtils.PadRightEx(KFrontHc.ToString("0.00"), 36));
            rslt.Add(FileUtils.PadRightEx("最大弧垂：", 16) + FileUtils.PadRightEx(MaxIndBackHc.ToString("0.00"), 12) + FileUtils.PadRightEx(MaxGrdBackHc.ToString("0.00"), 12) + FileUtils.PadRightEx(MaxOPGWBackHc.ToString("0.00"), 12)
                + FileUtils.PadRightEx(MaxIndFrontHc.ToString("0.00"), 12) + FileUtils.PadRightEx(MaxGrdFrontHc.ToString("0.00"), 12) + FileUtils.PadRightEx(MaxOPGWFrontHc.ToString("0.00"), 12));
            rslt.Add(FileUtils.PadRightEx("大风垂直弧垂：", 16) + FileUtils.PadRightEx(BackIndWindHc1.ToString("0.00"), 12) + FileUtils.PadRightEx(BackGrdWindHc1.ToString("0.00"), 12) + FileUtils.PadRightEx(BackOPGWWindHc1.ToString("0.00"), 12)
                + FileUtils.PadRightEx(FrontIndWindHc1.ToString("0.00"), 12) + FileUtils.PadRightEx(FrontGrdWindHc1.ToString("0.00"), 12) + FileUtils.PadRightEx(FrontOPGWWindHc1.ToString("0.00"), 12));
            return rslt;
        }

        public List<string> PrintDFCurePY()
        {
            List<string> rslt = new List<string>();
            rslt.Add(FileUtils.PadRightEx("\n弧垂", 16) + FileUtils.PadRightEx("小号侧", 36) + FileUtils.PadRightEx("大号侧", 36));
            rslt.Add(FileUtils.PadRightEx("导线：", 28) + FileUtils.PadRightEx("地线：", 12) + FileUtils.PadRightEx("OPGW：", 12) + FileUtils.PadRightEx("导线：", 12) + FileUtils.PadRightEx("地线：", 12) + FileUtils.PadRightEx("OPGW：", 12));
            rslt.Add(FileUtils.PadRightEx("大风垂直弧垂：", 16) + FileUtils.PadRightEx(BackIndWindHc.ToString("0.00"), 12) + FileUtils.PadRightEx(BackGrdWindHc.ToString("0.00"), 12) + FileUtils.PadRightEx(BackOPGWWindHc.ToString("0.00"), 12)
                + FileUtils.PadRightEx(FrontIndWindHc.ToString("0.00"), 12) + FileUtils.PadRightEx(FrontGrdWindHc.ToString("0.00"), 12) + FileUtils.PadRightEx(FrontOPGWWindHc.ToString("0.00"), 12));
            return rslt;
        }

        #endregion

    }
}
