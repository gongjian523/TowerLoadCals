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
        [SugarColumn(ColumnName = "type")]
        public string StrType { get; set; }

        /// <summary>
        /// 重量
        /// </summary>
        public string Weight { get; set; }

        /// <summary>
        /// 长度
        /// </summary>
        public string FitLength { get; set; }

        /// <summary>
        /// 单片绝缘子长度--
        /// </summary>
        public string PieceLength { get; set; }

        /// <summary>
        /// 片数
        /// </summary>
        public string PieceNum { get; set; }

        /// <summary>
        /// 金具换算片数--
        /// </summary>
        public string GoldPieceNum { get; set; }

        /// <summary>
        /// 联数
        /// </summary>
        public string LNum { get; set; }

        /// <summary>
        /// 阻尼线长度--
        /// </summary>
        public string DampLength { get; set; }
    }
}
