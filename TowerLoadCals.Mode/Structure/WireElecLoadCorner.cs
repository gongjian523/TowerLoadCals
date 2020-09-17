using System.Collections.Generic;
using System.Xml.Serialization;

namespace TowerLoadCals.Mode.Structure
{
    public class WireElecLoadCorner
    {
        [XmlAttribute]
        public string WireType { get; set; }

        public List<ElecLoadCorner> ElecLoad { get; set; }
    }
}
