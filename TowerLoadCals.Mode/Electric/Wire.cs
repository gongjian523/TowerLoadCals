namespace TowerLoadCals.Mode
{
    public class Wire
    {
        /// <summary>
        /// 型号规格
        /// </summary>
        public string ModelSpecification { get; set; }

        /// <summary>
        /// 线类型
        /// </summary>
        public string WireType { get; set; }

        /// <summary>
        /// 截面积
        /// </summary>
        public decimal SectionArea { get; set; }

        /// <summary>
        /// 外径
        /// </summary>
        public decimal ExternalDiameter { get; set; }

        /// <summary>
        /// 单位长度质量
        /// </summary>
        public decimal UnitLengthMass { get; set; }

        /// <summary>
        /// 直流电阻
        /// </summary>
        public decimal DCResistor { get; set; }

        /// <summary>
        /// 额定拉断力
        /// </summary>
        public decimal RatedBreakingForce { get; set; }

        /// <summary>
        /// 弹性模量
        /// </summary>
        public decimal ModulusElasticity { get; set; }

        /// <summary>
        /// 线膨胀系数
        /// </summary>
        public decimal LineCoefficient { get; set; }
    }
}
