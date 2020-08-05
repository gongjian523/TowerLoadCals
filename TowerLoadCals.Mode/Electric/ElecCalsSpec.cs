using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TowerLoadCals.Mode.Electric
{
    [XmlRoot("电气计算规范")]
    public class ElecCalsSpec
    {
        [XmlArray("电气分类")]
        [XmlArrayItem("分类信息")]
        public List<ElecCalsCateSpec> Category { get; set; }

        [XmlArray("断张力")]
        [XmlArrayItem("断张力详情")]
        public List<ElecCalsBreakWireSpec> BreakWireStress { get; set; }

        [XmlArray("断线覆冰")]
        [XmlArrayItem("断线覆冰率")]
        public List<ElecCalsBreakIceSpec> BreakIceRate { get; set; }

        [XmlArray("不平衡张力")]
        [XmlArrayItem("不平衡张力详情")]
        public List<ElecCalsUnevenIceSpec> UnevenIceStress { get; set; }

        [XmlArray("不均匀冰覆冰")]
        [XmlArrayItem("不均匀冰覆冰率")]
        public List<ElecCalsUnevenIceCoverSpec> UnevenIceRate { get; set; }

        [XmlArray("跳线不均匀系数")]
        [XmlArrayItem("系数")]
        public List<ElecCalsJumpWireSpec> JumpWireWind { get; set; }

        [XmlArray("垂直荷载")]
        [XmlArrayItem("荷载")]
        public List<ElecCalsVertLoadSpec> VerticalLoad { get; set; }

        [XmlArray("风压系数")]
        [XmlArrayItem("系数")]
        public List<ElecCalsWindSpec> WindCoef { get; set; }

    }
}
