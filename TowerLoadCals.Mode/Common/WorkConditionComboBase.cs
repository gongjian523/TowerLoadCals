using System.Collections.Generic;

namespace TowerLoadCals.Mode
{
    public class WorkConditionComboBase
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int Index { get; set; }

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
        /// 也是一个工况代号
        /// 表明用的是Template.WorkConditongs的那个工况
        /// 使用的工况详情（温度，冰厚和风向在电气荷载文件中读出）
        /// </summary>
        public int WorkCode { get; set; }

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
