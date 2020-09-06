using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// created by :glj
/// </summary>

namespace TowerLoadCals.Mode.Internet
{
    /// <summary>
    /// 基本参数库
    /// </summary>
    [SugarTable("kb_strubaseparas")]
    public class StruCalsLibBaseData
    {
        /// <summary>
        /// id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 类别
        /// </summary>
        public string BaseCategory { get; set; }

        /// <summary>
        /// 大风线条风压调整系数
        /// </summary>
        public double WindAdjustFactor { get; set; }


        /// <summary>
        /// 其他情况线条风压调整系数
        /// </summary>
        public double OtherWindAdjustFactor { get; set; }


        /// <summary>
        /// 安装动力系数
        /// </summary>
        public double DynamicCoef { get; set; }

        /// <summary>
        /// 过牵引系数
        /// </summary>
        public double DrawingCoef { get; set; }


        /// <summary>
        /// 锚线风荷系数
        /// </summary>
        public double AnchorWindCoef { get; set; }

        /// <summary>
        /// 锚线垂荷系数
        /// </summary>
        public double AnchorGravityCoef { get; set; }

        /// <summary>
        /// 锚角
        /// </summary>
        public double AnchorAngle { get; set; }


        /// <summary>
        /// 跳线吊装系数
        /// </summary>
        public double LiftCoefJumper { get; set; }


        /// <summary>
        /// 临时拉线对地夹角
        /// </summary>
        public double TempStayWireAngle { get; set; }

        /// <summary>
        /// 牵引角度
        /// </summary>
        public double TractionAgnle { get; set; }

    }
}
