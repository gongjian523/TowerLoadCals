using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TowerLoadCals.Mode.Electric
{
    public class ElecCalsUnevenIceCoverSpec
    {
        [XmlAttribute("杆塔类型")]
        public string TowerType { get; set; }

        /// <summary>
        /// 边
        /// </summary>
        [XmlAttribute("边")]
        public string Side { get; set; }

        /// <summary>
        /// 分类
        /// </summary>
        [XmlAttribute("分类")]
        public string Category { get; set; }

        /// <summary>
        /// 覆冰率
        /// </summary>
        [XmlAttribute("覆冰率")]
        public int Percent { get; set; }


    }
}
