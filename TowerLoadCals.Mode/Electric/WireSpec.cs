namespace TowerLoadCals
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
}
