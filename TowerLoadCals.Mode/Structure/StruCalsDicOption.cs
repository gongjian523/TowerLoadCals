using System;
using System.Collections.Generic;

namespace TowerLoadCals.Mode
{
    public class StruCalsDicOption
    {
        public int Num { get; set; }

        public String[] LeftPoints { get; set; }

        public String[] RightPoints { get; set; }

        public String[] FrontPoints { get; set; }

        public String[] CentralPoints { get; set; }

        public String[] BackPoints { get; set; }

        public List<StruCalsDicComposeInfo> ComposrInfos { get; set; }
    }
}
