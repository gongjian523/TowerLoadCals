using System.Collections.Generic;
using System.Xml.Serialization;

namespace TowerLoadCals.Mode.Structure
{
    public class WireElecLoadLineCorner
    {
        [XmlAttribute]
        public string WireType { get; set; }

        public List<ElecLoadLineCorner> ElecLoad { get; set; }
    }
}
