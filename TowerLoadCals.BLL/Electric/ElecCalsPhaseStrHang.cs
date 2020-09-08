using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using TowerLoadCals.Common;
using TowerLoadCals.Common.Utils;
using TowerLoadCals.Mode.Electric;

namespace TowerLoadCals.BLL.Electric
{
    public class ElecCalsPhaseStrHang: ElecCalsPhaseStr
    {
        public override List<string> WireWorkConditionNames
        {
            get
            {
                return WireData.WorkCdtNamesHang;
            }
        }

        /// <summary>
        /// 1弧垂值（m）
        /// </summary>
        public double HC1 { get; set; }

        /// <summary>
        /// 2弧垂值（m）
        /// </summary>
        public double HC2 { get; set; }

        /// <summary>
        /// 2弧垂值（m）
        /// </summary>
        public double HC { get; set; }

        /// <summary>
        /// 大风风偏角
        /// </summary>
        public double WindAngle { get; set;}

        /// <summary>
        /// 计算高度
        /// </summary>
        public double CalHei { get; set; }


        /// <summary>
        /// 断线工况张力差-地线开断情况
        /// </summary>
        public double BreakTenDiffGrdBre { get; set; }
        public string BreakTenDiffGrdBreStr { get; set; }

        /// <summary>
        /// 不均匀冰工况张力差-地线开断情况
        /// </summary>
        public double UnbaIceTenDiffGrdBre { get; set; }
        public string UnbaIceTenDiffGrdBreStr { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="span"></param>
        public override void CalDFCure(double span, double repStrLen, double cl, char terrainType, double wireHei)
        {
            double bzHighTemp = WireData.BzDic["最高气温"].BiZai;
            double ylHighTemp = WireData.YLTableXls["最高气温"];

            HC1 = Math.Round(bzHighTemp/ylHighTemp / 8 * Math.Pow(span, 2), 1);
            HC2 = SpaceStr.GDHei - repStrLen - cl;
            HC = Math.Min(HC1, HC2);

            double windHBizai = WireData.BzDic["换算最大风速"].HorBizai;
            double windVBizai = WireData.BzDic["换算最大风速"].VerBizai;
            WindAngle = Math.Atan(windHBizai / windVBizai) /Math.PI * 180;

            CalHei = SpaceStr.GDHei - (repStrLen + 2d / 3 * HC) * Math.Cos(WindAngle / 180 * Math.PI);

            WireWindPara = ElecCalsToolBox2.UZFunction(CalHei, terrainType, wireHei);

            double terrainPara = ElecCalsToolBox2.TerrainValue(terrainType);
            StrWindPara = Math.Round(Math.Pow(SpaceStr.GDHei/ wireHei, terrainPara * 2) , 3);
        }



        public override void UpdateHorFor(double diaInc)
        {
            foreach (var nameWd in WireWorkConditionNames)
            {
                double rslt = HorFor(diaInc, WireData.DevideNum, SpaceStr.HorSpan, HangStr.DampLength, WireData.BzDic[nameWd].WindHezai, StrLoad[nameWd].WindLoad, out string str);

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
            double rslt = HorFor(diaInc, WireData.DevideNum, SpaceStr.HorSpan, HangStr.DampLength, WireData.BzDic[nameGK].WindHezai, StrLoad[nameGK].WindLoad, out str);
            return rslt;
        }


        public override void UpdateTensionDiff(double secInc, double effectPara, double safePara)
        {
            BreakTenDiff = TensionDiff(out string breakTensionDiffStr, WireData.Fore, secInc, effectPara, safePara, WireData.BreakTensionPara, WireData.DevideNum);
            BreakTenDiffStr = breakTensionDiffStr;

            UnbaIceTenDiff = TensionDiff(out string unbaIceTensionDiffStr, WireData.Fore, secInc, effectPara, safePara, WireData.UnbaTensionPara, WireData.DevideNum);
            UnbaIceTenDiffStr = unbaIceTensionDiffStr;

            if(WireData.bGrd != 0)
            {
                BreakTenDiffGrdBre = TensionDiff(out string breakTenDiffGrdBreStr, WireData.Fore, secInc, effectPara, safePara, WireData.BreakTensionGrdBrePara, WireData.DevideNum);
                BreakTenDiffGrdBreStr = breakTenDiffGrdBreStr;

                UnbaIceTenDiffGrdBre = TensionDiff(out string unbaIceTenDiffGrdBreStr, WireData.Fore, secInc, effectPara, safePara, WireData.UnbaTensionGrdBrePara, WireData.DevideNum);
                UnbaIceTenDiffGrdBreStr = unbaIceTenDiffGrdBreStr;
            }

            BreakTenMax = BreakTensionMax(secInc, effectPara, safePara);
            UnbaIceTenMax = UnbaIceTensionMax(secInc, effectPara, safePara);
        }
    }
}
