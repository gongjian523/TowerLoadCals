using System;
using System.Collections.Generic;

namespace TowerLoadCals.Mode
{
    /// <summary>
    /// 绝缘子串
    /// </summary>
    public class StrDataCollection
    {
        /// <summary>
        /// 类型 
        /// </summary>
        public String Type { get; set; }

        /// <summary>
        /// 重量
        /// </summary>
        public List<StrData> StrDatas { get; set; }

    }
}
