using System.Xml.Serialization;

namespace TowerLoadCals.Mode
{
    public class StruCalsLibStandardRelatedBaseParas
    {
        [XmlAttribute("恒荷载分项系数-不利")]
        public float RGBad { get; set; }

        [XmlIgnore]
        public string RGBadName { get { return "恒荷载分项系数-不利"; } }

        [XmlAttribute("恒荷载分项系数-有利")]
        public float RGGood { get; set; }

        [XmlIgnore]
        public string RGGoodName { get { return "恒荷载分项系数-有利"; } }

        [XmlAttribute("活荷载分项系数")]
        public float RQ { get; set; }

        [XmlIgnore]
        public string RQName { get { return "活荷载分项系数"; } }

        [XmlAttribute("可变荷载组合系数-安装")]
        public float VcFInstall { get; set; }

        [XmlIgnore]
        public string VcFInstallName { get { return "可变荷载组合系数-安装"; } }

        [XmlAttribute("可变荷载组合系数-断线")]
        public float VcFBroken { get; set; }

        [XmlIgnore]
        public string VcFBrokenName { get { return "可变荷载组合系数-断线"; } }

        [XmlAttribute("可变荷载组合系数-不均匀冰")]
        public float VcFUnevenIce { get; set; }

        [XmlIgnore]
        public string VcFUnevenIceName { get { return "可变荷载组合系数-不均匀冰"; } }

    }

    public class StruCalsLibGB50545BaseParas:StruCalsLibStandardRelatedBaseParas
    {
        [XmlAttribute("可变荷载组合系数-运行")]
        public float VcFNormal { get; set; }

        [XmlIgnore]
        public string VcFNormalName { get { return "可变荷载组合系数-运行"; } }


        [XmlAttribute("可变荷载组合系数-验算")]
        public float VcFCheck { get; set; }

        [XmlIgnore]
        public string VcFCheckName { get { return "可变荷载组合系数-验算"; } }
    }

    public class StruCalsLibDLT5551BaseParas : StruCalsLibStandardRelatedBaseParas
    {
        [XmlAttribute("恒荷载分项系数-抗倾覆")]
        public float RGOverturn { get; set; }

        [XmlIgnore]
        public string RGOverturnName { get { return "恒荷载分项系数-抗倾覆"; } }

        [XmlAttribute("可变荷载组合系数-大风")]
        public float VcFNormal { get; set; }

        [XmlIgnore]
        public string VcFNormalName { get { return "可变荷载组合系数-大风"; } }

        [XmlAttribute("可变荷载组合系数-覆冰")]
        public float VcFIce { get; set; }

        [XmlIgnore]
        public string VcFIceName { get { return "可变荷载组合系数-覆冰"; } }

        [XmlAttribute("可变荷载组合系数-低温")]
        public float VcFCold { get; set; }

        [XmlIgnore]
        public string VcFColdName { get { return "可变荷载组合系数-低温"; } }


    }
}
