using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TowerLoadCals.Mode;
using TowerLoadCals.Mode.Electric;

namespace TowerLoadCals.BLL.Electric
{
    public class ElecCalsWorkCondition: ElecCalsWorkConditionBase
    {
        public ElecCalsWorkCondition()
        {

        }

        public ElecCalsWorkCondition(WorkCondition wkCdt)
        {
            Name = wkCdt.SWorkConditionName;

            WindSpeed = Convert.ToDouble(wkCdt.SWindSpeed);
            BaseWindSpeed = 0;
            Temperature = Convert.ToDouble(wkCdt.STemperature);
            IceThickness = Convert.ToDouble(wkCdt.SIceThickness);
        }
    }
}
