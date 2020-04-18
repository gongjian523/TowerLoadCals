﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TowerLoadCals.Mode
{
    public class Weather
    {
        /// <summary>
        /// 气象区对象
        /// </summary>
        [XmlAttribute("Name")]
        public string Name { get; set; }

        /// <summary>
        /// 工况
        /// </summary>
        public List<WorkCondition> WorkConditions { get; set; }
    }
}
