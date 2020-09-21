using System.Xml.Serialization;

namespace TowerLoadCals.Mode.Structure
{
    public class StruCalsTension
    {
        [XmlAttribute]
        public string WireType { get; set; }

        [XmlAttribute]
        public string DirectF { get { return "45 - α/ 2"; } }

        [XmlAttribute]
        public string DirectB{ get { return "45 + α/ 2"; } }

        [XmlAttribute]
        public float AngleWinTenDF { get; set; }

        [XmlAttribute]
        public float AngleWinTenDB { get; set; }

        [XmlAttribute]
        public float AngleWinTenXF { get; set; }

        [XmlAttribute]
        public float AngleWinTenXB { get; set; }

        [XmlAttribute]
        public float AnchorTen { get; set; }

        [XmlAttribute]
        public float TemporaryTen { get; set; }

        [XmlAttribute]
        public float PulleyTen { get; set; }
    }
}
