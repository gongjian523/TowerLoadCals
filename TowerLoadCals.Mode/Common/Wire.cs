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
        /// 截面积 mm2
        /// </summary>
        public int Sec { get; set; }

        /// <summary>
        /// 直径 mm
        /// </summary>
        public int Dia { get; set; }

        /// <summary>
        /// 单位质量  Kg/Km
        /// </summary>
        public int Wei { get; set; }

        /// <summary>
        /// 综合弹性系数 Gpa 10^6 N/m2
        /// </summary>
        public int Elas { get; set; }

        /// <summary>
        /// 线性膨胀系数 10^-6/℃
        /// </summary>
        public int Coef { get; set; }

        /// <summary>
        /// 计算拉断力 N
        /// </summary>
        public int Fore { get; set; }


        //public List<WireSpec>  Specs { get; set; } 
    }

    //public class WireCollection
    //{
    //    public string Name { get; set; }

    //    public List<WireLib> Libs { get; set; }
    //}
}