using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TowerLoadCals.Mode.Structure
{
    public class StruCalsWind45Tension
    {
        [XmlAttribute]
        public int Id { get; set; }

        [XmlAttribute]
        public string WireType { get; set; }

        [XmlAttribute]
        public string Direct { get; set; }

        [XmlAttribute]
        public float AngleTensionX { get; set; }

        [XmlAttribute]
        public int AngleTensionD { get; set; }
    }
}
