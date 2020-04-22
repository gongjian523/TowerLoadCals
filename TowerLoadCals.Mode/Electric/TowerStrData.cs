using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.Mode.Electric
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
        public int VoltageLevel { get; set; }

        /// <summary>
        /// 杆塔型号
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 类型，1，直线塔；2，转角塔
        /// </summary>
        public int Type { get; set; }

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
        public int CalHeight { get; set; }

        /// <summary>
        /// 最小呼高
        /// </summary>
        public int MinHeight { get; set; }

        /// <summary>
        /// 最大呼高
        /// </summary>
        public int MaxHeight { get; set; }

        /// <summary>
        /// 设计水平档距
        /// </summary>
        public int AllowedHorSpan { get; set; }

        /// <summary>
        /// 单侧最小水平档距
        /// </summary>
        public int OneSideMinHorSpan { get; set; }

        /// <summary>
        /// 单侧最大水平档距
        /// </summary>
        public int OneSideMaxHorSpan { get; set; }

        /// <summary>
        /// 最大垂直档距
        /// </summary>
        public int AllowedVerSpan { get; set; }

        /// <summary>
        /// 单侧最小垂直档距
        /// </summary>
        public int OneSideMinVerSpan { get; set; }

        /// <summary>
        /// 单侧最大垂直档距
        /// </summary>
        public int OneSideMaxVerSpan { get; set; }

        /// <summary>
        /// 单侧上拔最小档距
        /// </summary>
        public int OneSideUpVerSpanMin { get; set; }

        /// <summary>
        /// 单侧上拔最大档距
        /// </summary>
        public int OneSideUpVerSpanMax { get; set; }

        /// <summary>
        /// 最小转角
        /// </summary>
        public int MinAngel { get; set; }

        /// <summary>
        /// 最大转角
        /// </summary>
        public int MaxAngel { get; set; }

        /// <summary>
        /// 最小代表档距
        /// </summary>
        public int DRepresentSpanMin { get; set; }

        /// <summary>
        /// 最大代表档距
        /// </summary>
        public int DRepresentSpanMax { get; set; }

        /// <summary>
        /// 直线塔呼高序列
        /// </summary>
        public int[] HeightSer { get; set; }

        /// <summary>
        /// 直线塔呼高序列字符串
        /// </summary>
        public string StrHeightSer { get; set; }

        /// <summary>
        /// 直线塔档距序列
        /// </summary>
        public int[] AllowHorSpan { get; set; }

        /// <summary>
        /// 直线塔档距序列字符串
        /// </summary>
        public string StrAllowHorSpan { get; set; }

        /// <summary>
        /// 耐张塔角度折档距
        /// </summary>
        public int AngelToHorSpan { get; set; }

        /// <summary>
        /// 耐张塔最大应用水平档距
        /// </summary>
        public int MaxAngHorSpan { get; set; }
    }
}
