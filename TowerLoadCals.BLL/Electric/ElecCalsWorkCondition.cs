using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerLoadCals.Mode;

namespace TowerLoadCals.BLL.Electric
{
    public class ElecCalsWorkCondition
    {
        public ElecCalsWorkCondition()
        {

        }

        public ElecCalsWorkCondition(WorkCondition wkCdt)
        {
            Name = wkCdt.SWorkConditionName;

            WindSpeed = Convert.ToDouble(wkCdt.SWindSpeed);
            BaseWindSpeed = 0;
            Temperature = Convert.ToDouble(wkCdt.STemperature);
            IceThickness = Convert.ToDouble(wkCdt.SIceThickness);
        }

        /// <summary>
        /// 工况名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 风速
        /// </summary>
        public double WindSpeed { get; set; }

        /// <summary>
        /// 气温
        /// </summary>
        public double Temperature { get; set; }

        /// <summary>
        /// 冰厚
        /// </summary>
        public double IceThickness { get; set; }

        /// <summary>
        /// 基本风速
        /// </summary>
        public double BaseWindSpeed { get; set; }
    }
}
