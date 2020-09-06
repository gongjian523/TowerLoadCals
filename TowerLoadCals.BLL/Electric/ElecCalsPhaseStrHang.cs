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
    }
}
