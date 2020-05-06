namespace TowerLoadCals
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
        public int RatedBreakingForce { get; set; }

        /// <summary>
        /// 弹性模量
        /// </summary>
        public int ModulusElasticity { get; set; }

        /// <summary>
        /// 线膨胀系数
        /// </summary>
        public decimal LineCoefficient { get; set; }
    }

    public class WireCh
    {
        /// <summary>
        /// 型号规格
        /// </summary>
        public string 型号规格 { get; set; }

        /// <summary>
        /// 线类型
        /// </summary>
        public string 线类型 { get; set; }

        /// <summary>
        /// 截面积 mm2
        /// </summary>
        public decimal 截面积 { get; set; }

        /// <summary>
        /// 外径 mm
        /// </summary>
        public decimal 外径 { get; set; }

        /// <summary>
        /// 单位长度质量  Kg/Km
        /// </summary>
        public decimal 单位长度质量 { get; set; }

        /// <summary>
        /// 直流电阻(Ω)
        /// </summary>
        public decimal 直流电阻 { get; set; }

        /// <summary>
        /// 额定拉断力 N
        /// </summary>
        public int 额定拉断力 { get; set; }

        /// <summary>
        /// 综合弹性系数 Gpa 10^6 N/m2
        /// </summary>
        public int 弹性模量 { get; set; }

        /// <summary>
        /// 线性膨胀系数 10^-6/℃
        /// </summary>
        public decimal 线性膨胀系数 { get; set; }
    }
}
