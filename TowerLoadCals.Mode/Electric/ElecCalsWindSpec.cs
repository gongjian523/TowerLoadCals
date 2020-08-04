using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TowerLoadCals.Mode.Electric
{
    public class ElecCalsWindSpec
    {
        [XmlAttribute("电压等级")]
        public string Voltage { get; set; }

        [XmlAttribute("分类")]
        public int Category { get; set; }
    }
}
