using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.Mode
{
    public class StrCalsResult
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int Index { get; set;}

        /// <summary>
        /// 点位号
        /// </summary>
        public string TowerPosition { get; set; }

        /// <summary>
        /// 工况
        /// </summary>
        public string WorkCondition { get; set; }

        /// <summary>
        /// 气温（0C）
        /// </summary>
        public string Temperature { get; set; }

        /// <summary>
        /// 风速（m/s）
        /// </summary>
        public string WindSpeed { get; set; }

        /// <summary>
        /// 冰厚（mm）
        /// </summary>
        public string IceThickness { get; set; }

        /// <summary>
        /// fx
        /// </summary>
        public string Fx { get; set; }

        /// <summary>
        /// fy
        /// </summary>
        public string Fy { get; set; }

        /// <summary>
        /// Fz
        /// </summary>
        public string Fz { get; set; }
    }
}
