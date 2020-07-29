using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.Mode.Internet
{
    /// <summary>
    /// 绝缘子串表(硬跳线)
    /// </summary>
    [SugarTable("kb_rigidjumperinsulator")]
    public class RigidJumperInsulator
    {

        /// <summary>
        /// 页面是否选中
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public bool IsSelected { get; set; }

        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 串类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 重量
        /// </summary>
        public double Weight { get; set; }

        /// <summary>
        /// 长度
        /// </summary>
        public double FitLength { get; set; }

        /// <summary>
        /// 单片绝缘子长度--
        /// </summary>
        public double PieceLength { get; set; }

        /// <summary>
        /// 片数
        /// </summary>
        public int PieceNum { get; set; }

        /// <summary>
        /// 金具换算片数--
        /// </summary>
        public int GoldPieceNum { get; set; }

        /// <summary>
        /// 联数
        /// </summary>
        public int LNum { get; set; }

        /// <summary>
        /// 阻尼线长度--
        /// </summary>
        public double DampLength { get; set; }
    }
}
