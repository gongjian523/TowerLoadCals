using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.Mode
{
    public class ProInfo
    {
        /// <summary>
        /// 电压
        /// </summary>
        public int Volt { get; set; }

        /// <summary>
        /// 工程名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 工程编号
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 设计阶段
        /// </summary>
        public string Stage { get; set; }

        /// <summary>
        /// 提资单文件编号
        /// </summary>
        public int FilesID { get; set; }
    }
}
