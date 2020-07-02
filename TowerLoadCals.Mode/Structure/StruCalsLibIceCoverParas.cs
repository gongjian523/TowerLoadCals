using System.Xml.Serialization;

namespace TowerLoadCals.Mode
{
    public class StruCalsLibIceCoverParas
    {
        [XmlAttribute("序号")]
        public int Index { get; set; }

        [XmlAttribute("覆冰厚度")]
        public float IceThickness { get; set; }

        [XmlAttribute("塔身风荷载增大系数")]
        public float TowerWindLoadAmplifyCoef { get; set; }

        [XmlAttribute("塔身垂荷增大系数")]
        public float TowerGravityLoadAmplifyCoef { get; set; }
    }
}
