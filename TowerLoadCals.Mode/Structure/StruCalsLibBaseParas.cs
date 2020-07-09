using System.Xml.Serialization;

namespace TowerLoadCals.Mode
{
    public class StruCalsLibBaseParas
    {
        [XmlAttribute("大风线条风压调整系数")]
        public float WindAdjustFactor { get; set; }

        [XmlIgnore]
        public string WindAdjustFactorName { get { return "大风线条风压调整系数"; } }

        [XmlAttribute("其他情况线条风压调整系数")]
        public float OtherWindAdjustFactor { get; set; }

        [XmlIgnore]
        public string OtherWindAdjustFactorName { get { return "其他情况线条风压调整系数"; } }

        [XmlAttribute("安装动力系数")]
        public float DynamicCoef { get; set; }

        [XmlIgnore]
        public string DynamicCoefName { get { return "安装动力系数"; } }

        [XmlAttribute("过牵引系数")]
        public float DrawingCoef { get; set; }

        [XmlIgnore]
        public string DrawingCoefName { get { return "过牵引系数"; } }

        [XmlAttribute("锚线风荷系数")]
        public float AnchorWindCoef { get; set; }

        [XmlIgnore]
        public string AnchorWindCoefName { get { return "锚线风荷系数"; } }

        [XmlAttribute("锚线垂荷系数")]
        public float AnchorGravityCoef { get; set; }

        [XmlIgnore]
        public string AnchorGravityCoefName { get { return "锚线垂荷系数"; } }

        [XmlAttribute("锚角")]
        public float AnchorAngle { get; set; }

        [XmlIgnore]
        public string AnchorAngleName { get { return "锚角（°）"; } }

        [XmlAttribute("跳线吊装系数")]
        public float LiftCoefJumper { get; set; }

        [XmlIgnore]
        public string LiftCoefJumperName { get { return "跳线吊装系数"; } }

        [XmlAttribute("临时拉线对地夹角")]
        public float TempStayWireAngle { get; set; }

        [XmlIgnore]
        public string TempStayWireAngleName { get { return "临时拉线对地夹角（°）"; } }

        [XmlAttribute("牵引角度")]
        public float TractionAgnle { get; set; }

        [XmlIgnore]
        public string TractionAgnleName { get { return "牵引角度（°）"; } }

        [XmlElement("GB50545-2010")]
        public StruCalsLibGB50545BaseParas BaseParasGB50545 { get; set; }
            
        [XmlElement("DLT5551-2018")]
        public StruCalsLibDLT5551BaseParas BaseParasDLT5551 { get; set; }
    }
}
