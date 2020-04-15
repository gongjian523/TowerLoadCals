using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.Mode
{
    /// <summary>
    /// 绝缘子串
    /// </summary>
    public class InsulatorString
    {
        /// <summary>
        /// 名字 
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// 类型 
        /// </summary>
        public String Type { get; set; }

        /// <summary>
        /// 长度 
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// 重量
        /// </summary>
        public int Weight { get; set; }

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
        public int JointNum { get; set; }

    }
}
