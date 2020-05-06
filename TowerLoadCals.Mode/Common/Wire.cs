using System.Collections.Generic;

namespace TowerLoadCals.Mode
{
    public class Wire
    {
        /// <summary>
        /// 
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 导线型号
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 线类型
        /// </summary>
        public string WireType { get; set; }

        /// <summary>
        /// 截面积 mm2
        /// </summary>
        public float Sec { get; set; }

        /// <summary>
        /// 外径 mm
        /// </summary>
        public float Dia { get; set; }

        /// <summary>
        /// 单位长度质量  Kg/Km
        /// </summary>
        public float Wei { get; set; }

        /// <summary>
        /// 直流电阻(Ω)
        /// </summary>
        public float Resistance { get; set; }

        /// <summary>
        /// 额定拉断力 N
        /// </summary>
        public int Fore { get; set; }

        /// <summary>
        /// 综合弹性系数 Gpa 10^6 N/m2
        /// </summary>
        public int Elas { get; set; }

        /// <summary>
        /// 线性膨胀系数 10^-6/℃
        /// </summary>
        public float Coef { get; set; }
    }


    public class WireChinese
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
        public float 截面积 { get; set; }

        /// <summary>
        /// 外径 mm
        /// </summary>
        public float 外径 { get; set; }

        /// <summary>
        /// 单位长度质量  Kg/Km
        /// </summary>
        public float 单位长度质量 { get; set; }

        /// <summary>
        /// 直流电阻(Ω)
        /// </summary>
        public float 直流电阻 { get; set; }

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
        public float 线性膨胀系数 { get; set; }
    }
}