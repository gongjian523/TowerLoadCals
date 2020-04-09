using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.DataMaterials
{
    public class WorkCondition
    {
        /// <summary>
        /// 工况名称
        /// </summary>
        public string SWorkConditionName { get; set; }

        /// <summary>
        /// 风速
        /// </summary>
        public string SWindSpeed { get; set; }

        /// <summary>
        /// 气温
        /// </summary>
        public string STemperature { get; set; }

        /// <summary>
        /// 冰厚
        /// </summary>
        public string SIceThickness { get; set; }
    }

    public class Weather
    {
        /// <summary>
        /// 气象区对象
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 工况
        /// </summary>
        public List<WorkCondition> WorkConditions { get; set; }
    }

    public class WeatherCollection
    {
        public string Name { get; set; }

        public List<Weather> Weathers { get; set; }
    }
}
