using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.Mode
{
    public class StruCalsResult
    {
        /// <summary>
        /// 点位号
        /// </summary>
        public int PointNum { get; set;}

        /// <summary>
        /// 序号
        /// </summary>
        public int Index  { get; set; }

        /// <summary>
        /// 工况
        /// </summary>
        public string WorkCondition { get; set; }

        /// <summary>
        /// fx
        /// </summary>
        public float[] Fx { get; set; }

        /// <summary>
        /// fy
        /// </summary>
        public float[] Fy { get; set; }

        /// <summary>
        /// Fz
        /// </summary>
        public float[] Fz { get; set; }
    }
}
