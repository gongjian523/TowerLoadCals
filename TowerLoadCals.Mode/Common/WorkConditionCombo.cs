using System.Collections.Generic;

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
        public int TensionAngleCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string VertialLoadCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int WindDirectionCode { get; set; }

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
