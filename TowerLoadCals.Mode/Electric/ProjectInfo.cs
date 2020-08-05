using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.Mode.Electric
{
    public class ProjectInfo
    {
        /// <summary>
        /// 电压
        /// </summary>
        public int Volt { get; set; }

        /// <summary>
        /// 交流还是直流
        /// 默认是交流线路 0
        /// </summary>
        public int ACorDC { get; set; }

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

        public ProjectInfo(string name, string stage)
        {
            Name = name;
            Stage = stage;
        }
    }
}
