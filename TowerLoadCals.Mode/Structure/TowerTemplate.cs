using System.Collections.Generic;

namespace TowerLoadCals.Mode
{
    public class TowerTemplate
    {
        /// <summary>
        /// 塔模板名字
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 塔型：直线塔、直线转角塔、转角塔、终端塔、分支塔
        /// </summary>
        public string TowerType { get; set; }

        /// <summary>
        /// 线名
        /// </summary>
        public List<string>  Wires { get; set; }

        /// <summary>
        /// 工况名列表
        /// </summary>
        public Dictionary<int, string>  WorkConditongs { get; set; }

        /// <summary>
        /// 工况组合列表
        /// </summary>
        public List<WorkConditionCombo>  WorkConditionCombos { get; set; }
    }






}