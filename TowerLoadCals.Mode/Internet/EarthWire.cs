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
    /// kb_earthwire  地线库表 
    /// </summary>
    [SugarTable("kb_earthwire")]
    public class EarthWire
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
        /// 线类型 db:分类
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// 型号规格 db:型号
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// 截面积(mm²) db:截面(mm2)-合计
        /// </summary>

        public double TotCroSection { get; set; }

        /// <summary>
        /// 外径(mm)
        /// </summary>
        public double TotDiaConductor { get; set; }

        /// <summary>
        /// 单位长度质量(kg/km)
        /// </summary>
        public string ConWeight { get; set; }


        /// <summary>
        /// 20℃时直流电阻(Ω/km)
        /// </summary>
        public string DCRes { get; set; }

        /// <summary>
        /// 额定拉断力(kN)
        /// </summary>
        public string UltTenStrength { get; set; }

        /// <summary>
        /// 弹性模量(Gpa)
        /// </summary>
        public double ModElastioity { get; set; }

        /// <summary>
        /// 线膨胀系数(1/℃)
        /// </summary>
        public string CoeExpansion { get; set; }
    }
}
