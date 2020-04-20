using System.Collections.Generic;

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