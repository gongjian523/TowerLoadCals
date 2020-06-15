using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.Mode
{
    public class StruCalsDicGroup
    {
        public string Group { get; set; }

        public string Name { get; set; }

        public string Wire { get; set; }

        public string FixedType { get; set; }

        public string ForceDirection { get; set; }

        public string Link { get; set; }

        public List<StruCalsDicOption> Options { get; set;}
    }
}
