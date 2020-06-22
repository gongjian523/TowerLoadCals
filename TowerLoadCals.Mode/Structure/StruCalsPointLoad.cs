using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.Mode
{
    public class StruCalsPointLoad
    {
        public int Name { get; set; }

        public string Wire { get; set; }

        public int WorkConditionId { get; set; }

        public string WorkCondition { get; set; }

        public string Orientation { get; set; }

        public float Proportion { get; set; }

        public float Load { get; set; }

        public string HPSettingName { get; set; }
    }
}
