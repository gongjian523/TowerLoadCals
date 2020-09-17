using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TowerLoadCals.Mode.Structure
{
    public class WireElecLoadLine
    {
        [XmlAttribute]
        public string WireType { get; set; }

        public List<ElecLoadLine> ElecLoad { get; set; }
    }
}
