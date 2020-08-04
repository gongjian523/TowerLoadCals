using System.Xml.Serialization;

namespace TowerLoadCals.Mode.Electric
{
    public class ElecCalsJumpWireSpec
    {
        [XmlAttribute("电压等级")]
        public string Voltage { get; set; }

        [XmlAttribute("系数取值")]
        public int Coef { get; set; }
    }
}
