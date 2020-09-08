using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.BLL.Electric
{
    public class LoadThrDe
    {
        /// <summary>
        /// 水平档距
        /// </summary>
        public double HoriSpan { get; set; }

        /// <summary>
        /// 水平荷载
        /// </summary>
        public double HorFor { get; set; }


        /// <summary>
        /// 水平荷载表达式
        /// </summary>
        public string HorForStr { get; set; }

        /// <summary>
        /// 垂直档距
        /// </summary>
        public double VetiSpan { get; set; }

        /// <summary>
        /// 垂直档距的计算表达式
        /// </summary>
        public string VetiSpanStr { get; set; }

        /// <summary>
        /// 垂直荷载
        /// </summary>
        public double VerWei { get; set; }

        /// <summary>
        /// 垂直荷载
        /// </summary>
        public string VerWeiStr { get; set; }

        /// <summary>
        ///  纵向荷载 (张力)
        /// </summary>
        public double LoStr { get; set; }

        /// <summary>
        ///  纵向荷载 (张力) 验算 (只用于耐张塔)
        /// </summary>
        public string LoStrCheckStr { get; set; }

        /// <summary>
        ///  纵向荷载 (张力) 验算 (只用于悬垂塔)
        /// </summary>
        public double LoStrCheck { get; set; }

        /// <summary>
        ///  纵向荷载 (张力) 验算
        ///  在耐张塔中只有断线和不均匀冰工况才有
        ///  在悬垂塔中每种工况都要计算
        /// </summary>
        public double LoStrCheck2 { get; set; }

        /// <summary>
        ///  纵向荷载 (张力)的计算表达式
        /// </summary>
        public string LoStrStr { get; set; }

        /// <summary>
        /// 跳线水平荷载
        /// </summary>
        public double JumpHorFor { get; set; }

        /// <summary>
        /// 跳线水平荷载计算表达式
        /// </summary>
        public string JumpHorForStr { get; set; }

        /// <summary>
        /// 跳线垂直荷载
        /// </summary>
        public double JumpVerWei { get; set; }

        /// <summary>
        /// 跳线垂直荷载计算表达式
        /// </summary>
        public string JumpVerWeiStr { get; set; }

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

        public LoadThrDe()
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
