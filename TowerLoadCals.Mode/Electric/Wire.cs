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
        public double SectionArea { get; set; }

        /// <summary>
        /// 外径
        /// </summary>
        public double ExternalDiameter { get; set; }

        /// <summary>
        /// 单位长度质量
        /// </summary>
        public double UnitLengthMass { get; set; }

        /// <summary>
        /// 直流电阻
        /// </summary>
        public double DCResistor { get; set; }

        /// <summary>
        /// 额定拉断力
        /// </summary>
        public double RatedBreakingForce { get; set; }

        /// <summary>
        /// 弹性模量
        /// </summary>
        public double ModulusElasticity { get; set; }

        /// <summary>
        /// 线膨胀系数
        /// </summary>
        public double LineCoefficient { get; set; }
    }
}
