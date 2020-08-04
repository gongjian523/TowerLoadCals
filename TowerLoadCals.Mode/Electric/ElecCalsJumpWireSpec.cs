using System.Xml.Serialization;

namespace TowerLoadCals.Mode.Electric
{
    public class ElecCalsJumpWireSpec
    {
        [XmlAttribute("电压等级")]
        public string Voltage { get; set; }

        [XmlAttribute("系数")]
        public float Coef { get; set; }
    }
}
