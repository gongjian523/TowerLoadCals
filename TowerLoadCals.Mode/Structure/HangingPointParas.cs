using System.Xml.Serialization;

namespace TowerLoadCals.Mode
{
    public class HangingPointParas
    {
        public HangingPointParas()
        {
            Points = new string[8]; 
        }

        [XmlAttribute]
        public int Index { get; set; }

        [XmlAttribute]
        public string WireType { get; set; }

        [XmlAttribute]
        public string StringType { get; set; }

        [XmlAttribute]
        public string Array { get; set; }

        [XmlAttribute]
        public int Angle { get; set; }

        [XmlAttribute]
        public string[] Points { get; set; } 
    }
}
