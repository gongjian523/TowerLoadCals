using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.DataMaterials
{
    public class SLXian
    {
        /// <summary>
        /// 电线型号
        /// </summary>
        public string WireType { get; set; }

        /// <summary>
        /// 截面MM2
        /// </summary>
        public string SectionMM2 { get; set; }

        /// <summary>
        /// 外径MM
        /// </summary>
        public string OuterDiameterMM { get; set; }

        /// <summary>
        /// 重量KG每KM
        /// </summary>
        public string WeightKG { get; set; }

        /// <summary>
        /// 弹性系数KG
        /// </summary>
        public string ElasticityCoefficientKG { get; set; }

        /// <summary>
        /// 弹性系数N
        /// </summary>
        public string ElasticityCoefficientN { get; set; }

        /// <summary>
        /// 线膨胀系数
        /// </summary>
        public string LinearExpansionCoefficient { get; set; }

        /// <summary>
        /// 断应力KGMM
        /// </summary>
        public string BreakingStressKGMM { get; set; }

        /// <summary>
        /// 断拉力N
        /// </summary>
        public string BreakingForceN { get; set; }

        /// <summary>
        /// 导线1地线2
        /// </summary>
        public string Wire1EarthWire2 { get; set; }

        /// <summary>
        /// 标称截面
        /// </summary>
        public string NominalCrossSection { get; set; }
    }
}
