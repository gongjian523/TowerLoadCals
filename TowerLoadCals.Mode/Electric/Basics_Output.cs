using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.Mode.Electric
{
    /// <summary>
    /// 电气基础数据 基础系数 实体类
    /// </summary>
    public class Basics_Output
    {
        /// <summary>
        /// 电压
        /// </summary>
        public string Voltage { get; set; }

        /// <summary>
        /// 分类
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// 风压系数
        /// </summary>
        public int WindCategory { get; set; }

        /// <summary>
        /// 跳线风压不均匀系数
        /// </summary>
        public float Coef { get; set; }
    }

}
