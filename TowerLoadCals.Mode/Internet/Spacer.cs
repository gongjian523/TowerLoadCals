using MySqlX.XDevAPI.Relational;
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
    /// 间隔棒
    /// </summary>
    [SugarTable("kb_jinjufanghujgb")]
    public class Spacer
    {

        /// <summary>
        /// 页面是否选中
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public bool IsSelected { get; set; }


        /// <summary>
        /// ID 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 型号 
        /// </summary>
        public string CategorySub { get; set; }

        /// <summary>
        /// 重量
        /// </summary>
        public double Weight { get; set; }

        /// <summary>
        /// 电压等级
        /// </summary>
        public double Voltage { get; set; }

        /// <summary>
        /// 受风面积 
        /// </summary>
        public double SecWind { get; set; }
    }
}
