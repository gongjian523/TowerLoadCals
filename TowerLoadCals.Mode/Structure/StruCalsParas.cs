using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TowerLoadCals.Mode
{
    public class StruCalsParas
    {
        [XmlAttribute]
        public string SequenceName { get; set; } = "";

        [XmlAttribute]
        public string TowerName { get; set; }

        [XmlAttribute]
        public string TowerType { get; set; }

        [XmlAttribute]
        public string Volt { get; set; }

        /// <summary>
        /// 杆塔型号
        /// </summary>
        [XmlAttribute]
        public string ToweMode { get; set; }

        /// <summary>
        /// 工况模块的路径，只是在新加入塔位，保存不在工程目录下的模块路径
        /// </summary>
        [XmlIgnore]
        public string TemplatePath { get; set; }

        [XmlAttribute]
        public string TemplateName { get; set; }

        [XmlIgnore]
        public List<string> FullStressTemplatePaths { get; set; }

        [XmlAttribute]
        public List<string> FullStressTemplateNames { get; set; }

        [XmlIgnore]
        public string ElectricalLoadFilePath { get; set; }

        [XmlIgnore]
        public TowerTemplate Template { get; set; }

        // 从Template转换而来，用于WorkConditionComboModule
        [XmlIgnore]
        public List<WorkConditionComboSpec> WorkConditions { get; set; }

        //基础参数，来自BaseAndLineParasModule
        public StruCalseBaseParas BaseParas { get; set; }

        //线参数，来自BaseAndLineParasModule
        public List<StruLineParas> LineParas { get; set; }

        //挂点参数，来自HangingPointModule
        public List<HangingPointSettingParas> HPSettingsParas { get; set; }

        public StruCalsElecLoad ElecLoad { get; set; }


    }
}
