using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.Mode.Internet
{
    /// <summary>
    /// 绝缘子串表
    /// </summary>
    [SugarTable("kb_insulator")]
    public class Insulator
    {

        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 模块型号
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 电压等级
        /// </summary>
        public string Voltage { get; set; }

        /// <summary>
        /// 预览图
        /// </summary>
        public string BaseImage { get; set; }

        /// <summary>
        /// 3D模型
        /// </summary>
        public string ModelFile { get; set; }

        /// <summary>
        /// IPT模型
        /// </summary>
        public string IPTFile { get; set; }

        /// <summary>
        /// 绝缘子型号
        /// </summary>
        public string InsulatorType { get; set; }

        /// <summary>
        /// 串类型
        /// </summary>
        public string SetType { get; set; }

        /// <summary>
        /// 串用途
        /// </summary>
        public string ApplicationUse { get; set; }

        /// <summary>
        /// 绝缘子材质
        /// </summary>
        public string Materialofinsulator { get; set; }

        /// <summary>
        /// 绝缘子串受风面积(m2)
        /// </summary>
        public double WindArea { get; set; }

        /// <summary>
        /// V串夹角
        /// </summary>
        public double Anglert { get; set; }

        /// <summary>
        /// V串夹角(左)
        /// </summary>
        public double AngleL { get; set; }

        /// <summary>
        /// V串夹角(右)
        /// </summary>
        public double AngleR { get; set; }

        /// <summary>
        /// 串长(m)
        /// </summary>
        public double Length { get; set; }

        /// <summary>
        /// 重量(kg)
        /// </summary>
        public double Weight { get; set; }

        /// <summary>
        /// 绝缘子片数
        /// </summary>
        public int InsulatorNumber { get; set; }

        /// <summary>
        /// 绝缘子片数(左)
        /// </summary>
        public int InsulatorCountL { get; set; }

        /// <summary>
        /// 绝缘子片数(右)
        /// </summary>
        public int InsulatorCountR { get; set; }

        /// <summary>
        /// 导线分裂数
        /// </summary>
        public int BundleNumber { get; set; }

        /// <summary>
        /// 分裂排列方式
        /// </summary>
        public string SubType { get; set; }

        /// <summary>
        /// 分裂间距(mm)
        /// </summary>
        public double SubDistance { get; set; }

        /// <summary>
        /// 联数
        /// </summary>
        public int StringNumber { get; set; }

        /// <summary>
        /// 缠绕物类型
        /// </summary>
        public double EntanglementType { get; set; }

        /// <summary>
        /// 适用导线型号
        /// </summary>
        public double MatchedconductorType { get; set; }

        /// <summary>
        /// 是否为预绞丝悬垂线夹
        /// </summary>
        public double IsEntanglement { get; set; }

        /// <summary>
        /// U串连接长度(mm)
        /// </summary>
        public double HorizontalLength { get; set; }

        /// <summary>
        /// 金具长度(左上)(mm)
        /// </summary>
        public double FittingLengthLU { get; set; }

        /// <summary>
        /// 金具长度(右上)(mm)
        /// </summary>
        public double FittingLengthRU { get; set; }

        /// <summary>
        /// 金具长度(左下)(mm)
        /// </summary>
        public double FittingLengthLD { get; set; }

        /// <summary>
        /// 金具长度(右下)(mm)
        /// </summary>
        public double FittingLengthRD { get; set; }

        /// <summary>
        /// 绝缘子半径(mm)
        /// </summary>
        public double Radius { get; set; }

        /// <summary>
        /// 绝缘子结构高度(mm)
        /// </summary>
        public double InsulatorHigh { get; set; }

        /// <summary>
        /// 均压环个数
        /// </summary>
        public int GradingRingCount { get; set; }

        /// <summary>
        /// 均压环安装位置(mm)
        /// </summary>
        public double GradingRingPos { get; set; }

        /// <summary>
        /// 均压环高度(mm)
        /// </summary>
        public double GradingRingHigh { get; set; }

        /// <summary>
        /// 均压环半径(mm)
        /// </summary>
        public double GradingRingRad { get; set; }

        /// <summary>
        /// 物资编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 安装相别/极别
        /// </summary>
        public string AnzhuangLevel { get; set; }

        /// <summary>
        /// 接线点信息
        /// </summary>
        public string JieXianPoint { get; set; }

        /// <summary>
        /// 连塔挂点数
        /// </summary>
        public int GuaDianNumber { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [SugarColumn(ColumnName = "operator")]
        public string OperatorId { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public int CreateTime { get; set; }


    }
}
