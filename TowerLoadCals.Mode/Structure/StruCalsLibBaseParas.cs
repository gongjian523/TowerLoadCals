﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TowerLoadCals.Mode
{
    public class StruCalsLibBaseParas
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

        [XmlAttribute("可变荷载组合系数-运行")]
        public float VcFNormal { get; set; }

        [XmlIgnore]
        public string VcFNormalName { get { return "可变荷载组合系数-运行"; } }

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

        [XmlAttribute("可变荷载组合系数-验算")]
        public float VcFCheck { get; set; }

        [XmlIgnore]
        public string VcFCheckName { get { return "可变荷载组合系数-验算"; } }

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
    }
}
