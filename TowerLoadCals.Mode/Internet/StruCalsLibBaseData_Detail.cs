using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.Mode.Internet
{
    /// <summary>
    /// kb_strubaseparas 详细信息
    /// </summary>
    [SugarTable("kb_strubaseparasdetail")]
    public class StruCalsLibBaseData_Detail
    {
        /// <summary>
        /// id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// ParentId
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// 基础类别
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// 恒荷载分项系数-不利
        /// </summary>
        public double RGBad { get; set; }


        /// <summary>
        /// 恒荷载分项系数-有利
        /// </summary>
        public double RGGood { get; set; }


        /// <summary>
        /// 活荷载分项系数
        /// </summary>
        public double RQ { get; set; }

        /// <summary>
        /// 可变荷载组合系数-安装
        /// </summary>
        public double VcFInstall { get; set; }


        /// <summary>
        /// 可变荷载组合系数-断线
        /// </summary>
        public double VcFBroken { get; set; }

        /// <summary>
        /// 可变荷载组合系数-不均匀冰
        /// </summary>
        public double VcFUnevenIce { get; set; }

        /// <summary>
        /// 可变荷载组合系数-运行
        /// </summary>
        public double VcFNormal { get; set; }


        /// <summary>
        /// 可变荷载组合系数-验算
        /// </summary>
        public double VcFCheck { get; set; }

        /// <summary>
        /// 恒荷载分项系数-抗倾覆
        /// </summary>
        public double RGOverturn { get; set; }

        /// <summary>
        /// 可变荷载组合系数-大风
        /// </summary>
        public double VcGNormal { get; set; }

        /// <summary>
        /// 可变荷载组合系数-覆冰
        /// </summary>
        public double VcFIce { get; set; }

        /// <summary>
        /// 可变荷载组合系数-低温
        /// </summary>
        public double VcFCold { get; set; }
    }
}
