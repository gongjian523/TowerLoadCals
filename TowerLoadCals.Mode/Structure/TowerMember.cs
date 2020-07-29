using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.Mode.Structure
{
    /// <summary>
    /// 结果解析文件
    /// </summary>
    public class TowerMember
    {
        /// <summary>
        /// 杆件编号
        /// </summary>
        public string Member { get; set; }


        /// <summary>
        /// 截面
        /// </summary>
        public string Section { get; set; }


        /// <summary>
        /// 材质
        /// </summary>
        public string Material { get; set; }


        /// <summary>
        /// 长度
        /// </summary>
        public string LEN { get; set; }


        /// <summary>
        /// 计算长度
        /// </summary>
        public string ULEN { get; set; }


        /// <summary>
        /// 回转半径
        /// </summary>
        public string GR { get; set; }


        /// <summary>
        /// 长细比
        /// </summary>
        public string SR { get; set; }


        /// <summary>
        /// 允许长细比
        /// </summary>
        public string ASR { get; set; }

        /// <summary>
        /// 稳定系数
        /// </summary>
        public string GSFACR { get; set; }

        /// <summary>
        /// 拉力
        /// </summary>
        public string Tens { get; set; }
        /// <summary>
        /// 受拉工况
        /// </summary>
        public string TensCase { get; set; }
        /// <summary>
        ///  压力
        /// </summary>
        public string Comp { get; set; }
        /// <summary>
        ///  受压工况
        /// </summary>
        public string CompCase { get; set; }

        /// <summary>
        ///  折减系数
        /// </summary>
        public string WFAC { get; set; }

        /// <summary>
        ///  最大应力
        /// </summary>
        public string WSTR { get; set; }
        /// <summary>
        ///  效率
        /// </summary>
        public double EFFIC { get; set; }
        /// <summary>
        ///  螺栓
        /// </summary>
        public string Bolt { get; set; }
        /// <summary>
        ///  螺栓个数
        /// </summary>
        public string BoltNum { get; set; }
        /// <summary>
        ///  减孔
        /// </summary>
        public string ReducingBoltNum { get; set; }
    }
}
