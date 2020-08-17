using System.Xml.Serialization;

namespace TowerLoadCals.Mode
{
    [XmlType("KNode")]
    public class WorkCondition
    {
        /// <summary>
        /// 工况名称
        /// </summary>
        [XmlAttribute("SWorkConditionName")]
        public string SWorkConditionName { get; set; }

        /// <summary>
        /// 风速
        /// </summary>
        [XmlAttribute("SWindSpeed")]
        public string SWindSpeed { get; set; }

        /// <summary>
        /// 气温
        /// </summary>
        [XmlAttribute("STemperature")]
        public string STemperature { get; set; }

        /// <summary>
        /// 冰厚
        /// </summary>
        [XmlAttribute("SIceThickness")]
        public string SIceThickness { get; set; }
    }


    public class ElecCalsWorkCondition
    {
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
