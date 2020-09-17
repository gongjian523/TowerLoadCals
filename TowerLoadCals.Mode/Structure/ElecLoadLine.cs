using System.Xml.Serialization;

namespace TowerLoadCals.Mode.Structure
{
    public class ElecLoadLine
    {
        [XmlAttribute]
        public int WorkConditionId { get; set; }

        [XmlIgnore]
        public string WorkConditionName { get; set; }

        [XmlAttribute]
        public float Wind { get; set; }

        [XmlAttribute]
        public float GMax { get; set; }

        [XmlAttribute]
        public float GMin { get; set; }

        [XmlAttribute]
        public float TensionMax { get; set; }

        [XmlAttribute]
        public float TensionMin { get; set; }
    }
}
