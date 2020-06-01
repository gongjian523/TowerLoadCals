using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.Mode
{
    public class StruCalsParas
    {
        public String TowerName { get; set; }

        public String TablePath { get; set; }

        public TowerTemplate Template { get; set; }

        public List<StruLineParas> LineParas { get; set; }
    }
}
