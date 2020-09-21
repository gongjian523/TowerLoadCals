using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerLoadCals.Mode.Electric;
using TowerLoadCals.Mode.Structure;

namespace TowerLoadCals.Mode
{
    //结构计算中的电气荷载
    //正常情况下是从电气计算结果转换过来的，
    //用了替代最原始的结构计算中 从电气荷载Excel的文件
    public class StruCalsElecLoad
    {
        public List<ElecCalsWorkConditionBase> WorkCondition { get; set;}

        public List<WireElecLoadLine> LineElecLoads { get; set; }

        public List<WireElecLoadLineCorner> LineCornerElecLoads { get; set; }

        public List<WireElecLoadCorner> CornerElecLoads { get; set; }

        /// <summary>
        /// 锚线张力、过滑车张力差和45°风
        /// </summary>
        public List<StruCalsTension> Tension { get; set; }
    }
}
