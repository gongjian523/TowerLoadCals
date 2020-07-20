using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace TowerLoadCals.Mode.Internet
{
    /// <summary>
    /// kb_wire导线库表 
    /// </summary>
    [SugarTable("kb_wire")]
    public class Wire
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
        [SugarColumn(ColumnName = "Category")]
        public string WireType { get; set; }

        /// <summary>
        /// 型号规格 db:型号
        /// </summary>
        [SugarColumn(ColumnName = "name")]
        public string ModelSpecification { get; set; }


        /// <summary>
        /// 截面积(mm²) db:截面(mm2)-合计
        /// </summary>

        [SugarColumn(ColumnName = "TotCroSection")]
        public double SectionArea { get; set; }

        /// <summary>
        /// 外径(mm)
        /// </summary>
        [SugarColumn(ColumnName = "TotDiaConductor")]
        public double ExternalDiameter { get; set; }

        /// <summary>
        /// 单位长度质量(kg/km)
        /// </summary>
        [SugarColumn(ColumnName = "ConWeight")]
        public string UnitLengthMass { get; set; }


        /// <summary>
        /// 20℃时直流电阻(Ω/km)
        /// </summary>
        [SugarColumn(ColumnName = "DCRes")]
        public string DCResistor { get; set; }

        /// <summary>
        /// 额定拉断力(kN)
        /// </summary>
        [SugarColumn(ColumnName = "UltTenStrength")]
        public string RatedBreakingForce { get; set; }

        /// <summary>
        /// 弹性模量(Gpa)
        /// </summary>
        [SugarColumn(ColumnName = "ModElastioity")]
        public double ModulusElasticity { get; set; }

        /// <summary>
        /// 线膨胀系数(1/℃)
        /// </summary>
        [SugarColumn(ColumnName = "CoeExpansion")]
        public string LineCoefficient { get; set; }
    }
}
