using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.Mode.Internet
{

    /// <summary>
    /// 覆冰参数库
    /// </summary>
    [SugarTable("kb_struextraparas")]
    public class StruCalsLibIceCover
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
        /// Index 
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 覆冰厚度
        /// </summary>
        public double IceThickness { get; set; }
        /// <summary>
        /// 塔身风荷载增大系数 
        /// </summary>
        public double TowerWindLoadAmplifyCoef { get; set; }

        /// <summary>
        /// 塔身垂荷增大系数
        /// </summary>
        public double TowerGravityLoadAmplifyCoef { get; set; }
    }
}
