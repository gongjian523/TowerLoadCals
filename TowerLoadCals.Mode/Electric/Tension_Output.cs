using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.Mode.Electric
{
    /// <summary>
    /// 电气基础数据-断线张力取值 实体
    /// </summary>
    public class Tension_Output
    {
        /// <summary>
        /// 冰厚
        /// </summary>
        public int IceThickness { get; set; }
        /// <summary>
        /// 等级
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 地形
        /// </summary>
        public string Terrain { get; set; }
        /// <summary>
        /// 悬垂型杆塔 单导线
        /// </summary>
        public int X_DWires { get; set; }
        /// <summary>
        /// 悬垂型杆塔 双分裂导线
        /// </summary>
        public int X_SWires { get; set; }
        /// <summary>
        /// 悬垂型杆塔 双分裂导线
        /// </summary>
        public int X_SFWires { get; set; }
        /// <summary>
        /// 悬垂型杆塔 地线
        /// </summary>
        public int X_GroundWires { get; set; }

        /// <summary>
        /// 耐张型杆塔 单导线
        /// </summary>
        public int N_DWires { get; set; }
        /// <summary>
        /// 耐张型杆塔 双分裂及以上导线
        /// </summary>
        public int N_SFWires { get; set; }
        /// <summary>
        /// 耐张型杆塔 地线
        /// </summary>
        public int N_GroundWires { get; set; }

        /// <summary>
        /// 悬垂型杆塔 一类
        /// </summary>
        public int X_FCategory { get; set; }
        /// <summary>
        /// 悬垂型杆塔 二类
        /// </summary>
        public int X_SCategory { get; set; }
        /// <summary>
        /// 悬垂型杆塔 三类
        /// </summary>
        public int X_TCategory { get; set; }

        /// <summary>
        /// 耐张型杆塔 一类
        /// </summary>
        public int N_FCategory { get; set; }
        /// <summary>
        /// 耐张型杆塔 二类
        /// </summary>
        public int N_SCategory { get; set; }
        /// <summary>
        /// 耐张型杆塔 三类
        /// </summary>
        public int N_TCategory { get; set; }

    }
}
