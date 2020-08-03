using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.BLL.Electric
{
    public class LoadThrDeUtils
    {
        /// <summary>
        /// 水平档距
        /// </summary>
        public float HoriSpan { get; set; }

        /// <summary>
        /// 水平荷载
        /// </summary>
        public float HorFor { get; set; }

        /// <summary>
        /// 垂直档距
        /// </summary>
        public float VetiSpan { get; set; }

        /// <summary>
        /// 垂直荷载
        /// </summary>
        public float VerWei { get; set; }

        /// <summary>
        ///  纵向荷载
        /// </summary>
        public float LoStr { get; set; }

        /// <summary>
        /// 跳线水平荷载
        /// </summary>
        public float JumpHorFor { get; set; }

        /// <summary>
        /// 跳线垂直荷载
        /// </summary>
        public float JumpVerWei { get; set; }

        /// <summary>
        /// 工况名称
        /// </summary>
        public string GKName { get; set; }

        /// <summary>
        /// 气象条件列表
        /// </summary>
        public List<string> WeathList { get; set; }

        /// <summary>
        ///  默认是后侧，前侧为1
        /// </summary>
        public int Side { get; set; }

        public LoadThrDeUtils()
        {
            HoriSpan = 0;
            HorFor = 0;
            VetiSpan = 0;
            VerWei = 0;
            LoStr = 0;
            JumpHorFor = 0;
            JumpVerWei = 0;
            GKName = "";
            WeathList = new List<string>();
            Side = 0;
        }

    }
}
