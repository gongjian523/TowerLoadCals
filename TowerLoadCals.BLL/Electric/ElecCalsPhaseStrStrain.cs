using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using TowerLoadCals.Common;
using TowerLoadCals.Common.Utils;
using TowerLoadCals.Mode.Electric;

namespace TowerLoadCals.BLL.Electric
{
    public class ElecCalsPhaseStrStrain : ElecCalsPhaseStr
    {
        public override List<string> WireWorkConditionNames
        {
            get
            {
                return WireData.WorkCdtNamesStrain;
            }
        }


        public override void UpdateHorFor(double diaInc)
        {
            foreach (var nameWd in WireWorkConditionNames)
            {
                double rslt = HorFor(diaInc, WireData.DevideNum, SpaceStr.Span/2, HangStr.DampLength, WireData.BzDic[nameWd].WindHezai, StrLoad[nameWd].WindLoad, out string str);

                int index = LoadList.FindIndex(item => item.GKName == nameWd);
                if (index < 0)
                {
                    LoadList.Add(new LoadThrDe()
                    {
                        GKName = nameWd,
                        HorFor = rslt,
                        HorForStr = str,
                    });
                }
                else
                {
                    LoadList[index].HorFor = rslt;
                    LoadList[index].HorForStr = str;
                }
            }
        }

        public override double UpdateHorFor(string nameGK, out string str, double diaInc)
        {
            double rslt = HorFor(diaInc, WireData.DevideNum, SpaceStr.Span/2, HangStr.DampLength, WireData.BzDic[nameGK].WindHezai, StrLoad[nameGK].WindLoad, out str);
            return rslt;
        }

        public override void CheckLoStr(List<LoadThrDe> anLoads, ElecCalsCommRes commPara)
        {
            foreach (var nameWd in WireWorkConditionNames)
            {
                int index = LoadList.FindIndex(item => item.GKName == nameWd);
                if (index < 0)
                    continue;
                double loStr = LoadList[index].LoStr;

                var loadAn = anLoads.Where(item => item.GKName == nameWd).FirstOrDefault();
                double loStrAd = loadAn == null ? 0 : loadAn.LoStr;

                if (loStr > loStrAd)
                {
                    LoadList[index].LoStrCheckStr = Math.Round(loStr, 0).ToString();
                }
                else
                {
                    double loStrCheck;
                    if (nameWd == "安装情况")
                    {
                        if (WireData.bGrd == 0)
                        {
                            loStrCheck = Math.Round(loStr / commPara.InstMaxPara / commPara.IndExMaxPara * commPara.InstMinPara * commPara.IndExMinPara, 0);
                        }
                        else
                        {
                            loStrCheck = Math.Round(loStr / commPara.InstMaxPara / commPara.GrdExMaxPara * commPara.InstMinPara * commPara.GrdExMinPara, 0);
                        }
                    }
                    else
                    {
                        loStrCheck = Math.Round(loStr / commPara.BuildMaxPara * commPara.BuildMinPara);
                    }
                    LoadList[index].LoStrCheckStr = Math.Round(loStr, 0).ToString() + "/(" + loStrCheck.ToString() + ")";
                }

                if (nameWd == "断线" || nameWd == "不均匀冰I" || nameWd == "不均匀冰II" || nameWd == "断线(导线+5mm)" || nameWd == "不均匀冰I(导线+5mm)" || nameWd == "不均匀冰II(导线+5mm)")
                {
                    if (loStr > loStrAd)
                    {
                        LoadList[index].LoStrCheck2 = Math.Round(loStr, 0);
                    }
                    else
                    {
                        LoadList[index].LoStrCheck2 = Math.Round(loStr / commPara.BuildMaxPara * commPara.BuildMinPara);
                    }
                }
            }
        }


        public override void UpdateTensionDiff(double secInc, double effectPara, double safePara)
        {
            BreakTenDiff = TensionDiff(out string breakTensionDiffStr, WireData.Fore, secInc, effectPara, safePara, WireData.BreakTensionPara, WireData.DevideNum);
            BreakTenDiffStr = breakTensionDiffStr;

            UnbaIceTenDiff = TensionDiff(out string unbaIceTensionDiffStr, WireData.Fore, secInc, effectPara, safePara, WireData.UnbaTensionPara, WireData.DevideNum);
            UnbaIceTenDiffStr = unbaIceTensionDiffStr;

            BreakTenMaxTemp = BreakTensionMax(secInc, effectPara, safePara);
            BreakTenMax = Math.Max(BreakTenMaxTemp, BreakTenDiff);
            UnbaIceTenMaxTemp = UnbaIceTensionMax(secInc, effectPara, safePara);
            UnbaIceTenMax = Math.Max(UnbaIceTenMaxTemp, UnbaIceTenDiff);
        }

        protected double BreakTensionMax(double secInc, double effectPara, double safePara)
        {
            if (WireData.CommParas.BreakMaxPara == 1)
            {
                return Math.Round(secInc * WireData.Fore * effectPara / 9.80665 / safePara * WireData.DevideNum, 2);
            }
            else
            {
                var load = LoadList.Where(item => item.GKName == "覆冰无风").FirstOrDefault();
                double loStr = load == null ? 0 : load.LoStr;

                var loadAdd5 = LoadList.Where(item => item.GKName == "覆冰无风+5").FirstOrDefault();
                double loStrAdd5 = loadAdd5 == null ? 0 : loadAdd5.LoStr;

                return WireData.bGrd == 0 ? loStr : (WireData.CommParas.GrdIceUnbaPara == 1 ? loStr : loStrAdd5);
            }
        }

        protected double UnbaIceTensionMax(double secInc, double effectPara, double safePara)
        {
            if (WireData.CommParas.UnbaMaxPara == 1)
            {
                return Math.Round(secInc * WireData.Fore * effectPara / 9.80665 / safePara * WireData.DevideNum, 2);
            }
            else
            {
                var load = LoadList.Where(item => item.GKName == "最大覆冰").FirstOrDefault();
                double loStr = load == null ? 0 : load.LoStr;

                var loadGrdIce = LoadList.Where(item => item.GKName == "地线覆冰").FirstOrDefault();
                double loStrGrdIce = loadGrdIce == null ? 0 : loadGrdIce.LoStr;

                return WireData.bGrd == 0 ? loStr : (WireData.CommParas.GrdIceUnbaPara == 1 ? loStr : loStrGrdIce);
            }
        }

    }
}
