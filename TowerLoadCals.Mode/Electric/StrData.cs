using System;

namespace TowerLoadCals.Mode
{
    /// <summary>
    /// 绝缘子串
    /// </summary>
    public class StrData
    {
        /// <summary>
        /// ID 
        /// </summary>
        //public int ID { get; set; }

        /// <summary>
        /// 名字 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 串类型 
        /// </summary>
        public string StrType { get; set; }

        /// <summary>
        /// 重量
        /// </summary>
        public double Weight { get; set; }

        /// <summary>
        /// 长度 
        /// </summary>
        public double FitLength { get; set; }

        /// <summary>
        /// 单片绝缘子长度 
        /// </summary>
        public double PieceLength { get; set; }

        /// <summary>
        ///片数
        /// </summary>
        public int PieceNum { get; set; }

        /// <summary>
        ///金具换算片数
        /// </summary>
        public int GoldPieceNum { get; set; }

        /// <summary>
        ///联数
        /// </summary>
        public int LNum { get; set; }

        /// <summary>
        ///阻尼线长度
        /// </summary>
        public double DampLength { get; set; }

        /// <summary>
        ///硬跳线参数，支撑管长度
        /// </summary>
        public double SuTubleLen { get; set; }

        /// <summary>
        ///硬跳线参数，软跳线长度
        /// </summary>
        public double SoftLineLen { get; set; }


        /// <summary>
        ///硬跳线参数，软跳线长度
        /// </summary>
        public int JGBNum { get; set; }
    }
}
