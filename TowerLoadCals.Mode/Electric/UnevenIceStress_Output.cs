using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.Mode.Electric
{

    /// <summary>
    /// 电气基础数据-不均匀冰不平衡张力 实体
    /// </summary>
    public class UnevenIceStress_Output
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
        /// 悬垂型杆塔 单导线
        /// </summary>
        public int X_Wires { get; set; }
        /// <summary>
        /// 悬垂型杆塔 双分裂导线
        /// </summary>
        public int X_GroundWires { get; set; }
        /// <summary>
        /// 耐张塔 双分裂导线
        /// </summary>
        public int N_Wires { get; set; }
        /// <summary>
        /// 耐张塔 双分裂导线
        /// </summary>
        public int N_GroundWires { get; set; }
    }
}
