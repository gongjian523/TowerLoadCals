﻿using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace TowerLoadCals.Mode.Internet
{
    /// <summary>
    /// 导线库表 
    /// </summary>
    [SugarTable("kb_linedaoxian")]
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
        /// 型号规格 db:型号
        /// </summary>
        [SugarColumn(ColumnName = "name")]
        public string ModelSpecification { get; set; }

        /// <summary>
        /// 线类型 db:分类
        /// </summary>
        [SugarColumn(ColumnName = "category")]
        public string WireType { get; set; }


        /// <summary>
        /// 截面积(mm²) db:截面(mm2)-合计
        /// </summary>

        [SugarColumn(ColumnName = "JieMianTotal")]
        public double SectionArea { get; set; }

        /// <summary>
        /// 外径(mm)
        /// </summary>
        [SugarColumn(ColumnName = "WaiJing")]
        public double ExternalDiameter { get; set; }

        /// <summary>
        /// 单位长度质量(kg/km)
        /// </summary>
        [SugarColumn(ColumnName = "Weight")]
        public string UnitLengthMass { get; set; }


        /// <summary>
        /// 20℃时直流电阻(Ω/km)
        /// </summary>
        [SugarColumn(ColumnName = "ZhiLiuDianZu")]
        public string DCResistor { get; set; }

        /// <summary>
        /// 额定拉断力(kN)
        /// </summary>
        [SugarColumn(ColumnName = "LaDuanLi")]
        public string RatedBreakingForce { get; set; }

        /// <summary>
        /// 弹性模量(Gpa)
        /// </summary>
        [SugarColumn(ColumnName = "TanXingMoLiang")]
        public double ModulusElasticity { get; set; }

        /// <summary>
        /// 线膨胀系数(1/℃)
        /// </summary>
        [SugarColumn(ColumnName = "PengZhangXiShu")]
        public string LineCoefficient { get; set; }

        /*
        /// <summary>
        /// 来源
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 规格
        /// </summary>
        public string Specs { get; set; }

        /// <summary>
        /// 预览图
        /// </summary>
        public string BaseImage { get; set; }

        /// <summary>
        /// 3D模型
        /// </summary>
        public string ModelFile { get; set; }
        /// <summary>
        /// STL模型
        /// </summary>
        public string STLFile { get; set; }

        /// <summary>
        /// 结构(根/直径)(mm)-铝
        /// </summary>
        public string ZhiJingLv { get; set; }

        /// <summary>
        /// 结构(根/直径)(mm)-铝包钢
        /// </summary>
        public string ZhiJingLvBaoGang { get; set; }

        /// <summary>
        /// 截面(mm2)-铝
        /// </summary>
        public double JieMianLv { get; set; }

        /// <summary>
        /// 截面(mm2)-铝包钢
        /// </summary>
        public double JieMianLvBaoGang { get; set; }

        /// <summary>
        /// 录入人员
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        [SugarColumn(ColumnName = "operator")]
        public string OperatorID { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public int CreateTime { get; set; }
        */

    }
}
