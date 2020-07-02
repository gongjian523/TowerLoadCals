using System.Xml.Serialization;

namespace TowerLoadCals.Mode
{
    public class StruCalsLibWireExtraLoadParas
    {
        [XmlAttribute("序号")]
        public int Index { get; set; }

        [XmlAttribute("电压等级")]
        public int Voltage { get; set; }

        [XmlAttribute("铁塔安装重要性系数")]
        public float InstallImportanceCoef { get; set; }

        [XmlAttribute("铁塔其他重要性系数")]
        public float OtherImportanceCoef { get; set; }

        [XmlAttribute("悬垂塔地线附加荷载")]
        public float OverhangingTowerEarthWireExtraLoad { get; set; }

        [XmlAttribute("悬垂塔导线附加荷载")]
        public float OverhangingTowerWireExtraLoad { get; set; }

        [XmlAttribute("耐张塔地线附加荷载")]
        public float TensionTowerEarthWireExtraLoad { get; set; }

        [XmlAttribute("耐张塔导线附加荷载")]
        public float TensionTowerWireExtraLoad { get; set; }

        [XmlAttribute("耐张塔跳线附加荷载")]
        public float TensionTowerJumperWireExtraLoad { get; set; }
    }
}
