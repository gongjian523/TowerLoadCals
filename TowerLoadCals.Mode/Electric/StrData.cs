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
        public String Name { get; set; }

        /// <summary>
        /// 串类型 
        /// </summary>
        public String StrType { get; set; }

        /// <summary>
        /// 重量
        /// </summary>
        public decimal Weight { get; set; }

        /// <summary>
        /// 长度 
        /// </summary>
        public decimal FitLength { get; set; }

        /// <summary>
        /// 单片绝缘子长度 
        /// </summary>
        public decimal PieceLength { get; set; }

        /// <summary>
        ///片数
        /// </summary>
        public decimal PieceNum { get; set; }

        /// <summary>
        ///金具换算片数
        /// </summary>
        public decimal GoldPieceNum { get; set; }

        /// <summary>
        ///联数
        /// </summary>
        public decimal LNum { get; set; }

        /// <summary>
        ///阻尼线长度
        /// </summary>
        public decimal DampLength { get; set; }

        /// <summary>
        ///硬跳线参数，支撑管长度
        /// </summary>
        public decimal SuTubleLen { get; set; }

        /// <summary>
        ///硬跳线参数，软跳线长度
        /// </summary>
        public decimal SoftLineLen { get; set; }
    }
}
