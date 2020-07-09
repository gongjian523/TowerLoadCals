using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.Mode.Internet
{
    /// <summary>
    /// 气象条件
    /// </summary>
    public class WorkCondition
    {
        /// <summary>
        /// ID
        /// </summary>
        public string Id { get; set; }

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
