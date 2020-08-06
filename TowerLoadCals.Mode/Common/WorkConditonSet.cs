using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.Mode.Common
{
    /// <summary>
    /// 工况设置对应表
    /// </summary>
    public class WorkConditonSet
    {
        public WorkConditonSet()
        {
            LineTower = new Dictionary<string, string>();
            CornerTower = new Dictionary<string, string>();
        }
        /// <summary>
        /// 直线塔、直转塔
        /// </summary>
        public Dictionary<string, string> LineTower { get; set; }

        /// <summary>
        /// 终端塔、分支塔、转角塔
        /// </summary>
        public Dictionary<string, string> CornerTower { get; set; }


    }
}
