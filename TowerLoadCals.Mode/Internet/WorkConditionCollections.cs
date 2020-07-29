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
    public class WorkConditionCollections
    {
        /// <summary>
        /// IsChosen
        /// </summary>
        public bool IsSelected { get; set; }

        /// <summary>
        /// ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///冰区类别
        /// </summary>
        public int  CategoryId { get; set; }

        /// <summary>
        ///冰区类别名称
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// 工况名称
        /// </summary>
        public string SWorkConditionName { get; set; }

        /// <summary>
        /// 风速
        /// </summary>
        public double SWindSpeed { get; set; }

        /// <summary>
        /// 气温
        /// </summary>
        public double STemperature { get; set; }

        /// <summary>
        /// 冰厚
        /// </summary>
        public double SIceThickness { get; set; }
    }
}
