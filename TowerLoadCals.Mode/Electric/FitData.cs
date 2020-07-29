using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.Mode
{
    public class FitData
    {
        /// <summary>
        /// ID 
        /// </summary>
        //public int ID { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 型号 
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// 重量
        /// </summary>
        public double Weight { get; set; }

        /// <summary>
        /// 电压等级
        /// </summary>
        public double Voltage { get; set; }

        /// <summary>
        /// 受风面积 
        /// </summary>
        public double SecWind { get; set; }
    }
}
