using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.Mode
{
    public class TowerStrData
    {
        /// <summary>
        /// 
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 电压等级
        /// </summary>
        public double VoltageLevel { get; set; }

        /// <summary>
        /// 杆塔型号
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 类型，1，直线塔；2，转角塔 源文件格式
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 类型，1，直线塔；2，转角塔 转换值
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 杆塔回路数
        /// </summary>
        public int CirNum { get; set; }

        /// <summary>
        /// 交直流杆塔
        /// </summary>
        public int CurType { get; set; }

        /// <summary>
        /// 计算高度
        /// </summary>
        public double CalHeight { get; set; }

        /// <summary>
        /// 最小呼高
        /// </summary>
        public double MinHeight { get; set; }

        /// <summary>
        /// 最大呼高
        /// </summary>
        public double MaxHeight { get; set; }

        /// <summary>
        /// 设计水平档距
        /// </summary>
        public double AllowedHorSpan { get; set; }

        /// <summary>
        /// 单侧最小水平档距
        /// </summary>
        public double OneSideMinHorSpan { get; set; }

        /// <summary>
        /// 单侧最大水平档距
        /// </summary>
        public double OneSideMaxHorSpan { get; set; }

        /// <summary>
        /// 最大垂直档距
        /// </summary>
        public double AllowedVerSpan { get; set; }

        /// <summary>
        /// 单侧最小垂直档距
        /// </summary>
        public double OneSideMinVerSpan { get; set; }

        /// <summary>
        /// 单侧最大垂直档距
        /// </summary>
        public double OneSideMaxVerSpan { get; set; }

        /// <summary>
        /// 单侧上拔最小档距
        /// </summary>
        public double OneSideUpVerSpanMin { get; set; }

        /// <summary>
        /// 单侧上拔最大档距
        /// </summary>
        public double OneSideUpVerSpanMax { get; set; }

        /// <summary>
        /// 最小转角
        /// </summary>
        public double MinAngel { get; set; }

        /// <summary>
        /// 最大转角
        /// </summary>
        public double MaxAngel { get; set; }

        /// <summary>
        /// 最小代表档距
        /// </summary>
        public double DRepresentSpanMin { get; set; }

        /// <summary>
        /// 最大代表档距
        /// </summary>
        public double DRepresentSpanMax { get; set; }

        /// <summary>
        /// 直线塔呼高序列
        /// </summary>
        public double[] HeightSer { get; set; }

        /// <summary>
        /// 直线塔呼高序列字符串
        /// </summary>
        public string StrHeightSer { get; set; }

        /// <summary>
        /// 直线塔档距序列
        /// </summary>
        public double[] AllowHorSpan { get; set; }

        /// <summary>
        /// 直线塔档距序列字符串
        /// </summary>
        public string StrAllowHorSpan { get; set; }

        /// <summary>
        /// 耐张塔角度折档距
        /// </summary>
        public double AngelToHorSpan { get; set; }

        /// <summary>
        /// 耐张塔最大应用水平档距
        /// </summary>
        public double MaxAngHorSpan { get; set; }


        /// <summary>
        /// 上相导线高差(上相（中相）与下横担高差)
        /// </summary>
        public double UpSideInHei { get; set; }

        /// <summary>
        /// 中相导线高差(中相（边相）与下横担高差)
        /// </summary>
        public double MidInHei { get; set; }

        /// <summary>
        /// 下相导线高差(下相（边相）与下横担高差)
        /// </summary>
        public double DnSideInHei { get; set; }

        /// <summary>
        /// 地线高差(地线与下横担高差)
        /// </summary>
        public double GrDHei { get; set; }

        /// <summary>
        /// 上相跳线高差(上相（中相）跳线挂点与下横担高差)
        /// </summary>
        public double UpSideJuHei { get; set; }

        /// <summary>
        ///  中相跳线高差(中相（边相）跳线挂点与下横担高差)
        /// </summary>
        public double MidJuHei { get; set; }

        /// <summary>
        /// 下相跳线高差(下相（边相）跳线挂点与下横担高差)
        /// </summary>
        public double DnSideJuHei { get; set; }

        /// <summary>
        /// 结构计算模板
        /// </summary>
        public string TempletName { get; set; }

        /// <summary>
        /// 结构计算模型
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// 结构计算模型 扩展名 年月日
        /// </summary>
        public string ModelFileExtension { get; set; }


        /// <summary>
        /// 挂点文件
        /// </summary>
        public string HangPointName { get; set; }

        /// <summary>
        /// 挂点文件 扩展名 年月日
        /// </summary>
        public string HangPointFileExtension { get; set; }

        /// <summary>
        /// 是否被修改
        /// </summary>
        public bool IsEdit { get; set; }
    }
}
