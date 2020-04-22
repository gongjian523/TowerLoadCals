using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.Mode.Electric
{
    public class FitData
    {
        /// <summary>
        /// ID 
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 名字 
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// 类型 
        /// </summary>
        public String Type { get; set; }

        /// <summary>
        /// 重量
        /// </summary>
        public int Weight { get; set; }

        /// <summary>
        /// 受风面积 
        /// </summary>
        public int SecWind { get; set; }
    }
}
