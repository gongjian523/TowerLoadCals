using System.Xml.Serialization;

namespace TowerLoadCals.Mode.Electric
{
    public class ElecCalsCateSpec
    {
        [XmlAttribute("电压等级")]
        public string Voltage { get; set; }

        [XmlAttribute("分类")]
        public string Category { get; set; }
    }
}
