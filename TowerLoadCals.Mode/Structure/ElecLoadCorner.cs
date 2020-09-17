using System.Xml.Serialization;

namespace TowerLoadCals.Mode.Structure
{
    public class ElecLoadCorner
    {
        [XmlAttribute]
        public int WorkConditionId { get; set; }

        [XmlIgnore]
        public string WorkConditionName { get; set; }

        [XmlAttribute]
        public float WindDF { get; set; }

        [XmlAttribute]
        public float WindDB { get; set; }

        [XmlAttribute]
        public float WindXF { get; set; }

        [XmlAttribute]
        public float WindXB { get; set; }

        [XmlAttribute]
        public float GMaxF { get; set; }

        [XmlAttribute]
        public float GMaxB { get; set; }

        [XmlAttribute]
        public float GMinF { get; set; }

        [XmlAttribute]
        public float GMinB { get; set; }

        [XmlAttribute]
        public float TensionD { get; set; }

        [XmlAttribute]
        public float TensionX { get; set; }

        [XmlAttribute]
        public float WindTX { get; set; }

        [XmlAttribute]
        public float GTX { get; set; }

    }
}
