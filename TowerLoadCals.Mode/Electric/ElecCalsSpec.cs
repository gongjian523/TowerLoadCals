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

        [XmlArray("轻冰区断张力")]
        [XmlArrayItem("断张力")]
        public List<ElecCalsBreakWireSpec> LightIceBreakWireStress { get; set; }

        [XmlArray("中冰区断张力")]
        [XmlArrayItem("断张力")]
        public List<ElecCalsBreakWireSpec> MiddleIceBreakWireStress { get; set; }

        [XmlArray("重冰区断张力")]
        [XmlArrayItem("断张力")]
        public List<ElecCalsBreakWireSpec> HeavyIceBreakWireStress { get; set; }

        [XmlArray("轻冰区不平衡张力")]
        [XmlArrayItem("不平衡张力")]
        public List<ElecCalsUnevenIceSpec> LightIceStress { get; set; }

        [XmlArray("中冰区不平衡张力")]
        [XmlArrayItem("不平衡张力")]
        public List<ElecCalsUnevenIceSpec> MiddleIceStress { get; set; }

        [XmlArray("重冰区不平衡张力")]
        [XmlArrayItem("不平衡张力")]
        public List<ElecCalsUnevenIceSpec> HeavyIceStress { get; set; }

        [XmlArray("重冰不均匀覆冰不平衡张力")]
        [XmlArrayItem("不平衡张力")]
        public List<ElecCalsUnevenIceSpec> HeavyUnevenIceStress { get; set; }

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
