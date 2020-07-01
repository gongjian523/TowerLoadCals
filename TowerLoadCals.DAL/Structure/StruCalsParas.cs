using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using TowerLoadCals.Common;
using TowerLoadCals.Mode;

namespace TowerLoadCals.DAL
{
    public class StruCalsParas
    {
        public StruCalsParas()
        {

        }
        
        //此构造函数用于单元测试
        public StruCalsParas(FormulaParas baseParas, List<StruLineParas> lineParas, List<HangingPointSettingParas> hpSettingParas)
        {
            BaseParas = baseParas;
            LineParas = lineParas;
            HPSettingsParas = hpSettingParas;
        }


        //此构造函数用于从配置文件中获取已经保存的塔位参数，所有参数都来做保存文件
        public StruCalsParas(string towerName, string electricalLaodFilePath, string templatePath, StruCalsParas temp)
        {
            TowerName = towerName;
            ElectricalLoadFilePath = electricalLaodFilePath;

            BaseParas = temp.BaseParas;
            LineParas = temp.LineParas;
            HPSettingsParas = temp.HPSettingsParas;

            DecodeTemplate(BaseParas.Type, templatePath);
        }

        //此构造函数用于新增塔位，线条相关的初始化信息主要来自于Template，
        public StruCalsParas(string towerName, string towerType,  string templatePath, string electricalLaodFilePath, out string decodeTemolateStr)
        {
            decodeTemolateStr = "";

            TowerName = towerName;

            TemplatePath = templatePath;
            TemplateName = templatePath.Substring(templatePath.LastIndexOf('\\')+1);

            ElectricalLoadFilePath = electricalLaodFilePath;

            BaseParas = new FormulaParas() {
                SelectedStandard = "GB50545-2010",
                Type = TowerTypeStringConvert.TowerStringToType(towerType),
                IsMethod1Selected = true
            };

            if(!DecodeTemplate(BaseParas.Type, templatePath))
            {
                decodeTemolateStr = "解码模块错误！";
                return;
            }


            List<StruLineParas> lineParas = new List<StruLineParas>();
            for (int i = 0; i < Template.Wires.Count; i++)
            {
                lineParas.Add(new StruLineParas { Index = i + 1, WireType = Template.Wires[i] });
            }
            LineParas = lineParas;

            HPSettingsParas = new List<HangingPointSettingParas>();
            NewHangingPointSetting();

            ResultPointLoad = new List<StruCalsPointLoad>();
        }

        [XmlAttribute]
        public string TowerName { get; set; }

        /// <summary>
        /// 工况模块的路径，只是在新加入塔位，保存不在工程目录下的模块路径
        /// </summary>
        [XmlIgnore]
        public string TemplatePath { get; set; }

        [XmlAttribute]
        public string TemplateName { get; set; }

        [XmlIgnore]
        public string ElectricalLoadFilePath { get; set; }

        [XmlIgnore]
        public TowerTemplate Template { get; set; }

        // 从Template转换而来，用于WorkConditionComboModule
        [XmlIgnore]
        public List<WorkConditionComboSpec> WorkConditions { get; set;}

        //基础参数，来自BaseAndLineParasModule
        public FormulaParas BaseParas { get; set; }

        //线参数，来自BaseAndLineParasModule
        public List<StruLineParas> LineParas { get; set; }

        //挂点参数，来自HangingPointModule
        public List<HangingPointSettingParas> HPSettingsParas { get; set; }

        protected List<WorkConditionComboSpec> ConvertTemplateToSpec(TowerTemplate template)
        {
            List<WorkConditionComboSpec> listSpec = new List<WorkConditionComboSpec>();

            if (template == null)
                return listSpec;

            foreach (var item in template.WorkConditionCombos)
            {
                WorkConditionComboSpec spec = new WorkConditionComboSpec();

                spec.Index = item.Index;
                spec.IsCalculate = item.IsCalculate;
                spec.WorkConditionCode = item.WorkConditionCode;
                if (item.TensionAngleCode != null)
                    spec.TensionAngleCode = item.TensionAngleCode;
                if (item.VertialLoadCode != null)
                    spec.VertialLoadCode = item.VertialLoadCode;
                spec.WindDirectionCode = item.WindDirectionCode;
                spec.WorkCode = item.WorkCode;


                for (int i = 1; i <= item.WireIndexCodes.Count; i++)
                {
                    Type specType = spec.GetType();
                    PropertyInfo specPro = specType.GetProperty("Wire" + i.ToString(), BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public);
                    if (specPro != null)
                        specPro.SetValue(spec, item.WireIndexCodes[i - 1]);
                }

                spec.WorkComment = item.WorkComment;

                listSpec.Add(spec);
            }

            return listSpec;
        }

        public void NewHangingPointSetting()
        {
            HangingPointSettingParas paras = new HangingPointSettingParas();

            paras.HangingPointSettingName = "挂点方案" + (HPSettingsParas.Count + 1).ToString();

            //地线数
            int numGW = Template.Wires.Where(item => item.Contains("地")).Count();
            //导线数
            int numDW = Template.Wires.Count - numGW;

            List<string> arrList = GetArrayList();

            paras.InstallXYPoints = new List<HangingPointParas>(new HangingPointParas[arrList.Count * numDW]);
            paras.InstallZPoints = new List<HangingPointParas>(new HangingPointParas[arrList.Count * numDW]); 

            for (int i = 0; i < numGW; i++)
            {
                paras.NormalXYPoints.Add(new HangingPointParas { Index = i + 1, WireType = Template.Wires[i] });
                paras.NormalZPoints.Add(new HangingPointParas { Index = i + 1, WireType = Template.Wires[i] });
            }

            for (int i = numGW; i < Template.Wires.Count; i++)
            {
                paras.NormalXYPoints.Add(new HangingPointParas { Index = i + 1, WireType = Template.Wires[i] });
                paras.NormalZPoints.Add(new HangingPointParas { Index = i + 1, WireType = Template.Wires[i] });

                for(int j = 0; j  < arrList.Count; j++)
                {
                    paras.InstallXYPoints[j * numDW + i - numGW] = new HangingPointParas
                    {
                        Index = j * numDW + i - numGW + 1,
                        Array = "第" + arrList[j] + "组",
                        WireType = Template.Wires[i]
                    };
                    paras.InstallZPoints[j * numDW + i - numGW] = new HangingPointParas
                    {
                        Index = j * numDW + i - numGW + 1,
                        Array = "第" + arrList[j] + "组",
                        WireType = Template.Wires[i]
                    };
                }

                paras.TurningPoints.Add(new HangingPointParas { Index = i - numGW + 1, WireType = Template.Wires[i] });
            }

            HPSettingsParas.Add(paras);
        }

        //挂点的荷载计算结果，用于StruCalsResultModule
        public List<StruCalsPointLoad>  ResultPointLoad { get; set; }

        protected List<string> GetArrayList()
        {
            List<string> arr = new List<string>();

            foreach(var item in Template.WorkConditionCombos)
            {
                if (item.WorkConditionCode.EndsWith("a"))
                    arr.Add("a");
                else if (item.WorkConditionCode.EndsWith("b"))
                    arr.Add("b");
                else if (item.WorkConditionCode.EndsWith("c"))
                    arr.Add("c");
                else if (item.WorkConditionCode.EndsWith("d"))
                    arr.Add("d");
                else if (item.WorkConditionCode.EndsWith("e"))
                    arr.Add("e");
                else if (item.WorkConditionCode.EndsWith("f"))
                    arr.Add("f");
                else if (item.WorkConditionCode.EndsWith("g"))
                    arr.Add("g");
                else if (item.WorkConditionCode.EndsWith("h"))
                    arr.Add("h");
            }

            var result =  arr.Distinct().ToList();
            result.Sort();

            return result;
        }

        public bool DecodeTemplate(TowerType towerType, string templatesPath)
        {
            string file = templatesPath.Substring(templatesPath.Length - 3) + "dat";
            DES.DesDecrypt(templatesPath, file, "12345678");

            TowerTemplateReader templateReader = new TowerTemplateReader(towerType);
            Template = templateReader.Read(file);

            WorkConditions = ConvertTemplateToSpec(Template);

            return true;
        }

    }
}

