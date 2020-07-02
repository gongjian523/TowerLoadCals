using System.Collections.Generic;
using System.Xml.Serialization;

namespace TowerLoadCals.Mode
{
    [XmlRoot("结构计算参数")]
    public class StruCalsLib
    {
        [XmlElement("悬垂塔基础参数")]
        public StruCalsLibBaseParas OverhangingTowerBaseParas { get; set;}

        [XmlElement("耐张塔基础参数")]
        public StruCalsLibBaseParas TensionTowerBaseParas { get; set; }

        [XmlArray("附加荷载参数列表")]
        [XmlArrayItem("附加荷载参数")]
        public List<StruCalsLibWireExtraLoadParas> WireExtraLoadParas { get; set; }

        [XmlArray("覆冰参数列表")]
        [XmlArrayItem("覆冰参数")]
        public List<StruCalsLibIceCoverParas> IceCoverParas { get; set; }
    }
}
