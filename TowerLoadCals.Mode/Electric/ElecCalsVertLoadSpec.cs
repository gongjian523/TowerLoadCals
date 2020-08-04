using System.Xml.Serialization;

namespace TowerLoadCals.Mode.Electric
{
    public class ElecCalsVertLoadSpec
    {
        /// <summary>
        /// 工况
        /// </summary>
        [XmlAttribute("工况")]
        public string WorkCondition { get; set; }

        /// <summary>
        /// 设计冰荷载的百分数
        /// </summary>
        [XmlAttribute("设计冰荷载的百分数")]
        public int Percent { get; set; }
    }
}
