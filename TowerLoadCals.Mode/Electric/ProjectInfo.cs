using System.Xml.Serialization;

namespace TowerLoadCals.Mode.Electric
{
    public class ProjectInfo
    {
        /// <summary>
        /// 电压
        /// </summary>
        [XmlAttribute]
        public int Volt { get; set; }

        /// <summary>
        /// 电压的字符串
        /// </summary>
        [XmlAttribute]
        public string VoltStr { get; set; }

        /// <summary>
        /// 交流还是直流
        /// 默认是交流线路 0
        /// </summary>
        [XmlAttribute]
        public int ACorDC { get; set; }

        /// <summary>
        /// 工程名称
        /// </summary>
        [XmlAttribute]
        public string Name { get; set; }

        /// <summary>
        /// 工程编号
        /// </summary>
        [XmlAttribute]
        public int ID { get; set; }

        /// <summary>
        /// 设计阶段
        /// </summary>
        [XmlAttribute]
        public string Stage { get; set; }

        /// <summary>
        /// 提资单文件编号
        /// </summary>
        [XmlAttribute]
        public int FilesID { get; set; }

        public ProjectInfo()
        {
        }

        public ProjectInfo(string name, string stage)
        {
            Name = name;
            Stage = stage;
        }
    }
}
