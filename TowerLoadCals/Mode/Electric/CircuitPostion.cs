using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.Mode
{
    public class CircuitPostion
    {
        /// <summary>
        /// 线的类型
        /// </summary>
        public string FunctionType { get; set; }

        /// <summary>
        /// Z轴的位置
        /// </summary>
        public int Pz { get; set; }

        /// <summary>
        /// Y轴的位置
        /// </summary>
        public int Py { get; set; }

        /// <summary>
        /// X轴的位置
        /// </summary>
        public int Px { get; set; }
    }
}
