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
        public string  Index { get; set; }

        /// <summary>
        /// 塔位号
        /// </summary>
        public string TowerNum { get; set; }

        /// <summary>
        /// 塔位点
        /// </summary>
        public string TowerLocation { get; set; }

        /// <summary>
        /// 塔型
        /// </summary>
        public string TowerType { get; set; }

        /// <summary>
        /// 塔位高程
        /// </summary>
        public string TowerDistance { get; set; }

        /// <summary>
        /// 定位高差
        /// </summary>
        public string HeigtDifference { get; set; }

        /// <summary>
        /// 前侧档距
        /// </summary>
        public string FrontDistance { get; set; }

        /// <summary>
        /// 水平档距
        /// </summary>
        public string HorizontalDistance { get; set; }

        /// <summary>
        /// 垂直档距
        /// </summary>
        public string VerticalDistance { get; set; }

        /// <summary>
        /// 后侧/前侧设计气象条件
        /// </summary>
        public string DesignWeatherCondition { get; set; }

    }
}
