using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TowerLoadCals.DataMaterials
{
    public class WireSpec
    {
        /// <summary>
        /// 型号规格
        /// </summary>
        public string ModelSpecification { get; set; }

        /// <summary>
        /// 截面积
        /// </summary>
        public string SectionArea { get; set; }

        /// <summary>
        /// 外径
        /// </summary>
        public string ExternalDiameter { get; set; }

        /// <summary>
        /// 单位长度质量
        /// </summary>
        public string UnitLengthMass { get; set; }

        /// <summary>
        /// 直流电阻
        /// </summary>
        public string DCResistor { get; set; }

        /// <summary>
        /// 额定拉断力
        /// </summary>
        public string RatedBreakingForce { get; set; }

        /// <summary>
        /// 弹性模量
        /// </summary>
        public string ModulusElasticity { get; set; }

        /// <summary>
        /// 线膨胀系数
        /// </summary>
        public string LineCoefficient { get; set; }
    }

    public class Wire
    {
        public string Name { get; set; }

        public List<WireSpec>  Specs { get; set; } 
    }


    public class WireType
    {
        public string Type { get; set; }

        public List<Wire> Wire { get; set; }
    }

    public class WireLib
    {
        public string Lib { get; set; }

        public List<WireType> Types { get; set; }
    }

    public class WireCollection
    {
        public string Name { get; set; }

        public List<WireLib> Libs { get; set; }
    }
}