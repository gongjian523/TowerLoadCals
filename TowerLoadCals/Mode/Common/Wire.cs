using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TowerLoadCals.Mode
{
    public class Wire
    {
        public string Name { get; set; }

        public List<WireSpec>  Specs { get; set; } 
    }

    public class WireCollection
    {
        public string Name { get; set; }

        public List<WireLib> Libs { get; set; }
    }
}