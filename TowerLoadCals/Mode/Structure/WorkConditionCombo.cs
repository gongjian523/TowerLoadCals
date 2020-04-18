using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.Mode
{
    public class WorkConditionCombo
    {
        /// <summary>
        /// 
        /// </summary>
        public bool IsCalculate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string WorkConditionCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TensionAngleCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string VertialLoadCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string WindDirectionCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string WorkCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<int> WirdIndexCodes { get; set; }

        /// <summary>
        /// 注释
        /// </summary>
        public string WorkComment { get; set; }
    }
}
