﻿namespace TowerLoadCals.Mode.Electric
{
    
    public class JumpStrLoadResult
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

        /// <summary>
        /// 工况名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 风速
        /// </summary>
        public double WindSpeed { get; set; }

        /// <summary>
        /// 气温
        /// </summary>
        public double Temperature { get; set; }

        /// <summary>
        /// 冰厚
        /// </summary>
        public double IceThickness { get; set; }

        /// <summary>
        /// 基本风速
        /// </summary>
        public double BaseWindSpeed { get; set; }
    }
}
