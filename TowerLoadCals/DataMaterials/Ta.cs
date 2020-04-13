using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.DataMaterials
{


    public class Ta
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 塔位号
        /// </summary>
        public string Num { get; set; }

        /// <summary>
        ///塔位点=塔位名字+塔位偏差
        /// <summary>
        public string Pos { get; set; }

        /// <summary>
        /// 塔位名字
        /// </summary>
        public string PosName { get; set; }

        /// <summary>
        /// 塔位偏差
        /// </summary>
        public string PosOffset { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 塔型=塔名+"-"+呼高
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// 塔名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 呼高
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        /// 塔库ID
        /// </summary>
        public string ResID { get; set; }

        /// <summary>
        /// 塔位高程
        /// </summary>
        public double Elevation { get; set; }

        /// <summary>
        /// 定位高差（基面下降）
        /// </summary>
        public double SubOfElv { get; set; }

        /// <summary>
        /// 累距
        /// </summary>
        public double TotalSpan { get; set; }

        /// <summary>
        /// 前侧档距 = 前一个塔的累距-自己的累距
        /// </summary>
        public double FrontSpan { get; set; }

        /// <summary>
        /// 水平档距:
        /// 后侧档距 = 自己的累距-前一个塔的累距
        /// 水平档距 = (前侧档距 + 后侧档距)/2
        /// </summary>
        public double HorizontalSpan { get; set; }

        /// <summary>
        /// 前侧水平档距
        /// </summary>
        //public double FrontHorizontalSpan { get; set; }

        /// <summary>
        /// 后侧水平档距
        /// </summary>
        //public double BackHorizontalSpan { get; set; }

        /// <summary>
        /// 垂直档距 
        /// </summary>
        public string VerticalSpan { get; set; }

        /// <summary>
        /// 导线K值
        /// </summary>
        public double WireK { get; set; }

        /// <summary>
        /// 绝缘串长
        /// </summary>
        public double StringLength { get; set; }

        /// <summary>
        /// 垂直档距 中间计算参数
        /// </summary>
        public double guadg { get; set; }

        /// <summary>
        /// 中间部分
        /// </summary>
        public double sec { get; set; }

        /// <summary>
        /// 前侧垂直档距
        /// </summary>
        public double FrontVerticalSpan { get; set; }

        /// <summary>
        /// 后侧垂直档距
        /// </summary>
        public double BackVerticalSpan { get; set; }

        /// <summary>
        /// 转角
        /// </summary>
        public double AngelofApplication { get; set; }

        /// <summary>
        /// 后侧/前侧设计气象条件 = 后侧设计气象条件 + "/" + 前侧设计气象条件
        /// </summary>
        public string WeatherID { get; set; }

        /// <summary>
        /// 前侧设计气象条件
        /// </summary>
        public string FrontWeatherID { get; set; }

        /// <summary>
        /// 后侧设计气象条件
        /// </summary>
        public string BackWeatherID { get; set; }

    }
}
