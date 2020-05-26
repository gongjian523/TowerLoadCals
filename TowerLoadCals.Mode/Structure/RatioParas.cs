using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.Mode
{
    public class RatioParas
    {
        /// <summary>
        /// 常规情况跳线前比例
        /// </summary>
        public float BLTQ { get; set; }

        /// <summary>
        /// 常规情况跳线中比例
        /// </summary>
        public float BLTZ { get; set; }

        /// <summary>
        /// 常规情况跳线后比例
        /// </summary>
        public float BLTH { get; set; }

        /// <summary>
        /// 吊装情况跳线前比例
        /// </summary>
        public float BLDZTQ { get; set; }

        /// <summary>
        /// 吊装情况跳线中比例
        /// </summary>
        public float BLDZTZ { get; set; }

        /// <summary>
        /// 吊装情况跳线后比例
        /// </summary>
        public float BLDZTH { get; set; }
    }
}
