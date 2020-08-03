using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.BLL.Electric
{
    public class PntInTower
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public string UnitType { get; set; }

        public PntInTower()
        {
            X = 0;
            Y = 0;
            Z = 0;
            UnitType = "米";
        }
    }
}
