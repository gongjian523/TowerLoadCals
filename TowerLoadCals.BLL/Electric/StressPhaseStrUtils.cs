using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.BLL.Electric
{
    public class StressPhaseStrUtils
    {
        /// <summary>
        /// 相位置标记
        /// </summary>
        public int PhasePosType { get; set; }

        /// <summary>
        /// 前侧导线挂线串
        /// </summary>
        public StrDataUtils HangStr { get; set; }

        /// <summary>
        /// 跳线配置
        /// </summary>
        public StrDataUtils JumpStr { get; set; }

        /// <summary>
        /// 回路ID
        /// </summary>
        public int LoopID { get; set; }

        /// <summary>
        ///  相ID
        /// </summary>
        public int PhaseID { get; set; }

        /// <summary>
        /// 荷载列表，包括工况名称
        /// </summary>
        public List<string> LoadList { get; set; }

        /// <summary>
        /// 串长
        /// </summary>
        public float BaseStringEquLength { get; set; }


        /// <summary>
        /// 空间位置数据
        /// </summary>
        public PhaseSpaceStrUtils SpaceStr { get; set; }

        /// <summary>
        ///  线风压系数
        /// </summary>
        public float WireWindPara { get; set; }

        /// <summary>
        /// 绝缘子串风压系数
        /// </summary>
        public float StrWindPara { get; set; }

        /// <summary>
        ///  跳线风压系数
        /// </summary>
        public float JmWindPara { get; set; }

        /// <summary>
        /// 支撑管风压系数
        /// </summary>
        public float PropUpWindPara { get; set; }

        /// <summary>
        ///  线计算数据
        /// </summary>
        public WireUtils WrieData { get; set; }

        /// <summary>
        /// 跳线计算数据
        /// </summary>
        public WireUtils JmWrieData { get; set; }
    }
}
