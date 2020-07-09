using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.Mode.Internet
{
    /// <summary>
    /// kb_linedixian    地线库表 
    /// </summary>
    [SugarTable("kb_linedixian")]
    public class EarthWire
    {

        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 型号
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 分类
        /// </summary>
        public string Category { get; set; }

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
        /// 截面(mm2)-合计
        /// </summary>
        public double JieMianTotal { get; set; }

        /// <summary>
        /// 外径(mm)
        /// </summary>
        public double WaiJing { get; set; }

        /// <summary>
        /// 单位长度质量(kg/km)
        /// </summary>
        public string Weight { get; set; }

        /// <summary>
        /// 额定拉断力(kN)
        /// </summary>
        public string LaDuanLi { get; set; }


        /// <summary>
        /// 20℃时直流电阻(Ω/km)
        /// </summary>
        public string ZhiLiuDianZu { get; set; }

        /// <summary>
        /// 线膨胀系数(1/℃)
        /// </summary>
        public string PengZhangXiShu { get; set; }

        /// <summary>
        /// 弹性模量(Gpa)
        /// </summary>
        public double TanXingMoLiang { get; set; }

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

    }
}
