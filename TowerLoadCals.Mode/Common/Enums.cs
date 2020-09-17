using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.Mode.Common
{
    public class Enums
    {
        public enum AllotShelfStatusEnum
        {
            /// <summary>
            /// 待上架
            /// </summary>
            [Description("待上架")]
            待上架 = 0,

            /// <summary>
            /// 已完成
            /// </summary>
            [Description("已完成")]
            已完成 = 1,

            /// <summary>
            /// 异常
            /// </summary>
            [Description("异常")]

            异常 = 3,

            /// <summary>
            /// 进行中
            /// </summary>
            [Description("进行中")]
            进行中 = 4
        }
    }
}
