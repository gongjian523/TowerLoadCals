using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.Mode.Electric
{
    public class BZResult
    {
        public float g1 { get; set; }

        public float g2 { get; set; }

        public float g3 { get; set; }

        public float g4 { get; set; }

        public float g5 { get; set; }

        public float g6 { get; set; }

        public float g7 { get; set; }

        public float gi { get; set; }

        /// <summary>
        /// 比载
        /// </summary>
        public float BiZai { get; set;}

        /// <summary>
        /// 垂直比载
        /// </summary>
        public float VerBizai { get; set; }

        /// <summary>
        /// 横向比载
        /// </summary>
        public float HorBizai { get; set; }

        /// <summary>
        /// 应力
        /// </summary>
        public float Stress { get; set; }

        /// <summary>
        /// 垂直荷载
        /// </summary>
        public float VerHezai { get; set; }

        /// <summary>
        /// 风荷载
        /// </summary>
        public float WindHezai { get; set; }
    }
}
