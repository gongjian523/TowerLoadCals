using System.Xml.Serialization;

namespace TowerLoadCals.Mode.Electric
{
    public class ElecCalsWorkConditionBase
    {
        /// <summary>
        /// 序号
        /// </summary>
        [XmlAttribute]
        public int Id { get; set; }

        /// <summary>
        /// 工况名称
        /// </summary>
        [XmlAttribute]
        public string Name { get; set; }

        /// <summary>
        /// 风速
        /// </summary>
        [XmlAttribute]
        public double WindSpeed { get; set; }

        /// <summary>
        /// 气温
        /// </summary>
        [XmlAttribute]
        public double Temperature { get; set; }

        /// <summary>
        /// 冰厚
        /// </summary>
        [XmlAttribute]
        public double IceThickness { get; set; }

        /// <summary>
        /// 基本风速
        /// </summary>
        [XmlAttribute]
        public double BaseWindSpeed { get; set; }

        /// <summary>
        ///在电气荷载模板中的位置
        /// </summary>
        [XmlIgnore]
        public int Pos { get; set; }
    }
}
