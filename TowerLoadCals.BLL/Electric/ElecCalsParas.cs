using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerLoadCals.Mode.Electric;

namespace TowerLoadCals.BLL.Electric
{
    public class ElecCalsParas
    {
        //
        //public ProjectInfo  Project { get; set; }

        public ElecCalsWireData  WirePara { get; set; }

        public ElecCalsWeatherData WeatherData { get; set; }

        public ElecCalsCommRes CommParas { get; set;}

        public ElecCalsSideData SideParas { get; set; }

        public ElecCalsAppreData AppreData { get; set; }

        public ElecCalsSpanFitData SpanFitData { get; set; }

        public ElecCalsStrDatas StrDatas { get; set; }

        public ElecCalsTowerData TowerData { get; set; }
    }

    public class ElecCalsWireData
    {
        public ElecCalsWireBase Ind { get; set; }
        public ElecCalsWireBase Grd { get; set; }
        public ElecCalsWireBase OPGW { get; set; }
    }

    public class ElecCalsWeatherData
    {
        public List<ElecCalsWorkCondition> OneWeather { get; set; }
        public List<ElecCalsWorkCondition> AnWeather { get; set; }
    }

    public class ElecCalsSideData
    {
        public ElecCalsSideRes SidePara { get; set; }
        public ElecCalsSideRes AnSidePara { get; set; }
    }

    public class ElecCalsAppreData
    {
        public ElecCalsTowerAppre BackAppre { get; set; }
        public ElecCalsTowerAppre CalsAppre { get; set; }
        public ElecCalsTowerAppre FrontAppre { get; set; }
    }

    public class ElecCalsSpanFitData
    {
        public ElecCalsSpanFit BackSpanFit { get; set; }
        public ElecCalsSpanFit FrontSpanFit { get; set; }
    }

    public class ElecCalsStrDatas
    {
        public ElecCalsStrData IndStr { get; set; }
        public ElecCalsStrData GrdStr { get; set; }
        public ElecCalsStrData JumpStr { get; set; }
    }

    public class ElecCalsTowerData
    {
        public TowerElecCals BackTower { get; set; }
        public TowerElecCals CalsTower { get; set; }
        public TowerElecCals FrontTower { get; set; }
    }
}
