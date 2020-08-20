namespace TowerLoadCals.Mode.Electric
{
    
    public class JumpStrLoadResult:ElecCalsWorkCondition
    {
        /// <summary>
        /// 跳线绝缘子串的风荷载
        /// </summary>
        public double JumpStrWindLoad { get; set;}

        /// <summary>
        /// 跳线的风荷载
        /// </summary>
        public double JumpWindLoad { get; set; }

        /// <summary>
        /// 支撑管线的风荷载
        /// </summary>
        public double SuTubleWindLoad { get; set; }
    }
}
