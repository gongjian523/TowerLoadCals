﻿using System.Xml.Serialization;

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
        
        /// <summary>
        /// 风速
        /// </summary>
        [XmlIgnore]
        public float WindSpeed { get; set; }

        /// <summary>
        /// 气温
        /// </summary>
        [XmlIgnore]
        public float Temperature { get; set; }

        /// <summary>
        /// 冰厚
        /// </summary>
        [XmlIgnore]
        public float IceThickness { get; set; }


        public float BaseWindSpeed { get; set; }
    }
}
