using System.Collections.Generic;

namespace TowerLoadCals.Mode
{
    public class LoadDic
    {
        public string Wire { get; set; }

        public List<string> WorkConditionCode { get; set; }

        public int WireIndexCodesMin { get; set; }

        public int WireIndexCodesMax { get; set; }

        public string Group { get; set; }

        public string LinkXY { get; set; }

        public string LinkZ { get; set; }

        public string PointXY { get; set; }

        public string PointZ { get; set; }

        public bool IsGeneral { get; set; }
    }
}
