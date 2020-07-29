using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.Mode.Internet
{
    /// <summary>
    /// 绝缘子串表
    /// </summary>
    [SugarTable("kb_workconditioncategory")]
    public class WorkConditionCategory
    {

        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 冰区类型
        /// </summary>
        public string Name { get; set; }

    }
}
