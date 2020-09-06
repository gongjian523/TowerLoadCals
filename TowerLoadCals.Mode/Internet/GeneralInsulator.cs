using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// created by :glj
/// </summary>

namespace TowerLoadCals.Mode.Internet
{
    /// <summary>
    /// 绝缘子串表(一般子串)
    /// </summary>
    [SugarTable("kb_insulator")]
    public class GeneralInsulator
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
        [SugarColumn(ColumnName = "type")]
        public string Name { get; set; }

        /// <summary>
        /// 串类型
        /// </summary>
        [SugarColumn(ColumnName = "settype")]
        public string StrType { get; set; }

        /// <summary>
        /// 重量
        /// </summary>
        public double Weight { get; set; }

        /// <summary>
        /// 长度
        /// </summary>
        [SugarColumn(ColumnName = "length")]
        public double FitLength { get; set; }

        /// <summary>
        /// 单片绝缘子长度
        /// </summary>
        public double PieceLength { get; set; }

        /// <summary>
        /// 片数
        /// </summary>
        [SugarColumn(ColumnName = "insulatornumber")]
        public int PieceNum { get; set; }

        /// <summary>
        /// 金具换算片数
        /// </summary>
        public int GoldPieceNum { get; set; }

        /// <summary>
        /// 联数
        /// </summary>
        [SugarColumn(ColumnName = "stringnumber")]
        public int LNum { get; set; }

        /// <summary>
        /// 阻尼线长度
        /// </summary>
        public double DampLength { get; set; }


    }
}
