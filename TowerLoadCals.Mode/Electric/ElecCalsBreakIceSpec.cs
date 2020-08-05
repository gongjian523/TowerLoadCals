using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TowerLoadCals.Mode.Electric
{
    public class ElecCalsBreakIceSpec
    {
        [XmlAttribute("杆塔类型")]
        public string  TowerType { get; set;  }

        /// <summary>
        /// 冰区类型
        /// </summary>
        [XmlAttribute("冰区类型")]
        public string IceArea { get; set; }

        /// <summary>
        /// 冰厚
        /// </summary>
        [XmlAttribute("冰厚")]
        public int IceThickness { get; set; }

        /// <summary>
        /// 分类
        /// </summary>
        [XmlAttribute("分类")]
        public string Category { get; set; }

        /// <summary>
        /// 种类
        /// </summary>
        [XmlAttribute("种类")]
        public string Type { get; set; }

        /// <summary>
        /// 覆冰率
        /// </summary>
        [XmlAttribute("覆冰率")]
        public int Percent { get; set; }
    }
}
