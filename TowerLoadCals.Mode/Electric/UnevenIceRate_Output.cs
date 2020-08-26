using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.Mode.Electric
{

    /// <summary>
    /// 电气基础数据-线路等级覆冰率 实体
    /// </summary>
    public class UnevenIceRate_Output
    {
        /// <summary>
        /// 分类
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// 悬垂型杆塔 一侧
        /// </summary>
        public int X_Side { get; set; }
        /// <summary>
        /// 悬垂型杆塔 另一侧
        /// </summary>
        public int X_OtherSide { get; set; }
        /// <summary>
        /// 耐张塔 一侧
        /// </summary>
        public int N_Side { get; set; }
        /// <summary>
        /// 耐张塔 另一侧
        /// </summary>
        public int N_OtherSide { get; set; }
    }

}
