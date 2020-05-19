using System.Collections.Generic;

namespace TowerLoadCals.Mode
{
    public class WorkConditionComboBase
    {
        /// <summary>
        /// 选择与否
        /// </summary>
        public bool IsCalculate { get; set; }

        /// <summary>
        /// 工况代号        
        /// </summary>
        public string WorkConditionCode { get; set; }

        /// <summary>
        /// 张力角
        /// </summary>
        public string TensionAngleCode { get; set; }

        /// <summary>
        /// 垂直载荷
        /// </summary>
        public string VertialLoadCode { get; set; }

        /// <summary>
        /// 风向
        /// </summary>
        public int WindDirectionCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string WorkCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        //public List<int> WirdIndexCodes { get; set; }

        /// <summary>
        /// 注释
        /// </summary>
        public string WorkComment { get; set; }
    }
}
