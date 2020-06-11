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

        public string WireType { get; set; }

        public string Type { get; set; }

        public string Link { get; set; }

        public List<StruCalsDicOption> Options { get; set;}
    }
}
