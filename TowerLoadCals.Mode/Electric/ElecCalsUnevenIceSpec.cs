using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TowerLoadCals.Mode.Electric
{
    public class ElecCalsUnevenIceSpec
    {
        [XmlAttribute("杆塔类型")]
        public string TowerType { get; set; }

        /// <summary>
        /// 冰厚
        /// </summary>
        [XmlAttribute("冰厚")]
        public int IceThickness { get; set; }

        [XmlAttribute("导地线")]
        public string WireType { get; set; }

        /// <summary>
        /// 张力
        /// </summary>
        [XmlAttribute("张力")]
        public int Stress { get; set; }

        /// <summary>
        /// 最大使用张力百分数
        /// </summary>
        [XmlAttribute("最大使用百分比")]
        public int Percent { get; set; }

        /// <summary>
        /// 种类
        /// </summary>
        [XmlAttribute("种类")]
        public string Type { get; set; }

    }
}
