﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using TowerLoadCals.Common;
using TowerLoadCals.DAL;
using TowerLoadCals.DAL.Common;
using TowerLoadCals.Mode;
using TowerLoadCals.Mode.Common;
using TowerLoadCals.Mode.Electric;
using TowerLoadCals.Mode.Structure;

namespace TowerLoadCals.BLL
{
    public class StruCalsParasCompose: StruCalsParas
    {
       static WorkConditonSet conditionSet = WorkConditonSetReader.Read();//读取配置
        public StruCalsParasCompose()
        {

        }
        
        //此构造函数用于单元测试
        public StruCalsParasCompose(StruCalseBaseParas baseParas, List<StruLineParas> lineParas, List<HangingPointSettingParas> hpSettingParas)
        {
            BaseParas = baseParas;
            LineParas = lineParas;
            HPSettingsParas = hpSettingParas;
        }


        //此构造函数用于从配置文件中获取已经保存的塔位参数，所有参数都来做保存文件
        public StruCalsParasCompose(string electricalLaodFilePath, string templatePath, List<string> fullStressTemplatePaths, StruCalsParasCompose temp)
        {
            TowerName = temp.TowerName;
            TemplateName = temp.TemplateName;
            
            ElectricalLoadFilePath = electricalLaodFilePath;

            FullStressTemplateNames = temp.FullStressTemplateNames;
            FullStressTemplatePaths = fullStressTemplatePaths;

            BaseParas = temp.BaseParas;
            LineParas = temp.LineParas;
            HPSettingsParas = temp.HPSettingsParas;

            DecodeTemplate(BaseParas.Type, templatePath);

            ResultPointLoad = new List<StruCalsPointLoad>();

            ElecLoad = new StruCalsElecLoad()
            {
                CornerElecLoads = new List<WireElecLoadCorner>(),
                LineCornerElecLoads = new List<WireElecLoadLineCorner>(),
                LineElecLoads = new List<WireElecLoadLine>(),
                ExtraLoad = new List<StruCalsTension>(),
                WorkCondition = new List<ElecCalsWorkConditionBase>(),
            };
        }

        //此构造函数用于不是杆塔序列中新增塔位， 线条相关的初始化信息主要来自于Template，
        public StruCalsParasCompose(string towerName, string towerType, float voltage, string templatePath, string electricalLaodFilePath, List<string> fullStressTemplatePaths,  out string decodeTemolateStr)
        {
            decodeTemolateStr = "";

            TowerName = towerName;
            SequenceName = "";

            TemplatePath = templatePath;
            TemplateName = templatePath.Substring(templatePath.LastIndexOf('\\')+1);

            FullStressTemplatePaths = fullStressTemplatePaths;
            FullStressTemplateNames = new List<string>();
            foreach(var path in FullStressTemplatePaths)
            {
                FullStressTemplateNames.Add(path.Substring(path.LastIndexOf('\\') + 1));
            }

            TowerTypeEnum type = TowerTypeStringConvert.TowerStringToType(towerType);

            ElectricalLoadFilePath = electricalLaodFilePath;

            if(!DecodeTemplate(type, templatePath))
            {
                decodeTemolateStr = "解码模块错误！";
                return;
            }

            LineParas = new List<StruLineParas>();

            SetDefaultValue(type);
            BaseParas.Voltage = voltage;

            HPSettingsParas = new List<HangingPointSettingParas>();
            NewHangingPointSetting();

            ResultPointLoad = new List<StruCalsPointLoad>();

            ElecLoad = new StruCalsElecLoad()
            {
                CornerElecLoads = new List<WireElecLoadCorner>(),
                LineCornerElecLoads = new List<WireElecLoadLineCorner>(),
                LineElecLoads = new List<WireElecLoadLine>(),
                ExtraLoad = new List<StruCalsTension>(),
                WorkCondition = new List<ElecCalsWorkConditionBase>(),
            };
        }


        //此构造函数用于杆塔序列中新增塔位，
        public StruCalsParasCompose(string towerName, string sequence, string towerType,  string excelPath,  float voltage, string templatePath, List<string> fullStressTemplatePaths, List<HangingPointSettingParas> hpSettingsParas, out string decodeTemolateStr)
        {
            decodeTemolateStr = "";

            TowerName = towerName;
            SequenceName = sequence;

            TemplatePath = templatePath;
            TemplateName = templatePath.Substring(templatePath.LastIndexOf('\\') + 1);

            FullStressTemplatePaths = fullStressTemplatePaths;
            FullStressTemplateNames = new List<string>();
            foreach (var path in FullStressTemplatePaths)
            {
                FullStressTemplateNames.Add(path.Substring(path.LastIndexOf('\\') + 1));
            }

            TowerTypeEnum type = TowerTypeStringConvert.TowerStringToType(towerType);

            //在杆塔序列中新增的塔位，保存的是电气计算中结果的Excel
            ElectricalLoadFilePath = excelPath;

            if (!DecodeEncryTemplate(type, templatePath))
            {
                decodeTemolateStr = "解码模块错误！";
                return;
            }

            LineParas = new List<StruLineParas>();

            SetDefaultValue(type);
            BaseParas.Voltage = voltage;

            HPSettingsParas = hpSettingsParas;

            ResultPointLoad = new List<StruCalsPointLoad>();

            ElecLoad = new StruCalsElecLoad()
            {
                CornerElecLoads = new List<WireElecLoadCorner>(),
                LineCornerElecLoads = new List<WireElecLoadLineCorner>(),
                LineElecLoads = new List<WireElecLoadLine>(),
                ExtraLoad = new List<StruCalsTension>(),
                WorkCondition = new List<ElecCalsWorkConditionBase>(),
            };
        }

        //此构造函数用于从配置文件中获取已经保存的塔位参数()，所有参数都来做保存文件
        public StruCalsParasCompose(string templatePath, List<string> fullStressTemplatePaths, StruCalsParasCompose temp)
        {
            TowerName = temp.TowerName;
            TemplateName = temp.TemplateName;

            ElectricalLoadFilePath = temp.ElectricalLoadFilePath;

            FullStressTemplateNames = temp.FullStressTemplateNames;
            FullStressTemplatePaths = fullStressTemplatePaths;

            BaseParas = temp.BaseParas;
            LineParas = temp.LineParas;
            HPSettingsParas = temp.HPSettingsParas;

            DecodeEncryTemplate(BaseParas.Type, templatePath);

            ResultPointLoad = new List<StruCalsPointLoad>();

            ElecLoad = new StruCalsElecLoad()
            {
                CornerElecLoads = new List<WireElecLoadCorner>(),
                LineCornerElecLoads = new List<WireElecLoadLineCorner>(),
                LineElecLoads = new List<WireElecLoadLine>(),
                ExtraLoad = new List<StruCalsTension>(),
                WorkCondition = new List<ElecCalsWorkConditionBase>(),
            };
        }



        public static List<WorkConditionComboSpec> ConvertTemplateToSpec(TowerTemplate template, bool isCalculation = false)
        {
            List<WorkConditionComboSpec> listSpec = new List<WorkConditionComboSpec>();

            if (template == null || template.WorkConditionCombos == null)
                return listSpec;

            foreach (var item in template.WorkConditionCombos)
            {
                WorkConditionComboSpec spec = new WorkConditionComboSpec();

                spec.Index = item.Index;
                spec.IsCalculate = item.IsCalculate;
                spec.WorkConditionCode = isCalculation ? item.WorkConditionCode: WorkConditonSetReader.ConvertWorkConditionCode(template.TowerType, item.WorkConditionCode, conditionSet);//
                
                if (item.TensionAngleCode != null)
                    spec.TensionAngleCode = isCalculation ? item.TensionAngleCode: WorkConditonSetReader.ConvertTensionAngleCode(item.TensionAngleCode);
                if (item.VertialLoadCode != null)
                    spec.VertialLoadCode = isCalculation ? item.VertialLoadCode : WorkConditonSetReader.ConvertVertialLoadCode(item.VertialLoadCode);
                spec.WindDirectionCode = item.WindDirectionCode;
                spec.WorkCode = item.WorkCode;

                for (int i = 1; i <= item.WireIndexCodes.Count; i++)
                {
                    Type specType = spec.GetType();
                    PropertyInfo specPro = specType.GetProperty("Wire" + i.ToString(), BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public);
                    if (specPro != null)
                    {
                        if(isCalculation)
                            specPro.SetValue(spec, item.WireIndexCodes[i - 1].ToString());
                        else
                        {
                            string value = WorkConditonSetReader.ConvertWireCodes(template.TowerType, i, item.WorkConditionCode, item.WireIndexCodes, template.WorkConditongs);
                            specPro.SetValue(spec, value);
                        }

                    }
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


            //配置默认参数
            if(BaseParas.Type == TowerTypeEnum.LineTower)
            {
                paras.GCQ = 0.5f;
                paras.GCH = 0.5f;
                paras.GXW = 2;
                paras.GXN = -1;

                paras.DQWQ = 0.7f;
                paras.DQWH = 0.3f;
                paras.DQCQ = 0.7f;
                paras.DQCH = 0.3f;

                paras.DDWQ = 0.7f;
                paras.DDWH = 0.3f;
                paras.DDCQ = 0.7f;
                paras.DDCH = 0.3f;

                paras.DMWQ = 0.7f;
                paras.DMWH = 0.3f;
                paras.DMCQ = 0.7f;
                paras.DMCH = 0.3f;
            }
            else if(BaseParas.Type == TowerTypeEnum.LineCornerTower)
            {
                paras.GCQ = 0.5f;
                paras.GCH = 0.5f;
                paras.GXW = 1;
                paras.GXN = 0;

                paras.DQWQ = 0.5f;
                paras.DQWH = 0.5f;
                paras.DQCQ = 0.5f;
                paras.DQCH = 0.5f;

                paras.DDWQ = 0.5f;
                paras.DDWH = 0.5f;
                paras.DDCQ = 0.5f;
                paras.DDCH = 0.5f;
            }
            else
            {
                paras.BLTQ = 0.5f;
                paras.BLTH = 0.5f;
                paras.BLTZ = 0;

                paras.BLDZTQ = 0.5f;
                paras.BLDZTH = 0.5f;
                paras.BLDZTZ = 0;
            }


            HPSettingsParas.Add(paras);
        }

        //挂点的荷载计算结果，用于StruCalsResultModule
        [XmlIgnore]
        public List<StruCalsPointLoad>  ResultPointLoad { get; set; }

        //满应力分析结果，用于TowerMemberModule
        [XmlIgnore]
        public List<TowerMember> ResultFullStess { get; set; }

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

        //解码加密的结构计算模板
        public bool DecodeTemplate(TowerTypeEnum towerType, string templatesPath)
        {
            string file = templatesPath.Substring(0,templatesPath.Length - 3) + "dat";
            DES.DesDecrypt(templatesPath, file, "12345678");

            TowerTemplateReader templateReader = new TowerTemplateReader(towerType);
            Template = templateReader.Read(file);

            WorkConditions = ConvertTemplateToSpec(Template);

            return true;
        }

        //解码没有加密的结构计算模板
        public bool DecodeEncryTemplate(TowerTypeEnum towerType, string templatesPath)
        {
            TowerTemplateReader templateReader = new TowerTemplateReader(towerType);
            Template = templateReader.Read(templatesPath);

            WorkConditions = ConvertTemplateToSpec(Template);

            return true;
        }

        /// <summary>
        /// 从配置文件中获取默认参数
        /// </summary>
        protected void SetDefaultValue(TowerTypeEnum towerType)
        {
            var libParas = GlobalInfo.GetInstance().GetStruCalsLibParas();

            //有配置文件就从配置文件中读出
            if (libParas != null)
            {
                var config = new MapperConfiguration(x => x.CreateMap<StruCalsLibBaseParas, StruCalseBaseParas>().ForMember(des=>des.Type, item =>item.Ignore()));
                IMapper mapper = new Mapper(config);

                StruCalsLibBaseParas libBaseParas = (towerType == TowerTypeEnum.LineTower || towerType == TowerTypeEnum.LineCornerTower) ? libParas.OverhangingTowerBaseParas : libParas.TensionTowerBaseParas;

                BaseParas = mapper.Map<StruCalseBaseParas>(libBaseParas);

                //这种初始化有点问题，当两种标准的共同参数的值取不一样时会有错误
                //正确的做法是应该是在加入库时，就选择标准。
                //但这种做法两种标准切换时，两种标准共同参数取值不一样时，仍然无法切换到正确的值，
                //故维持现在的做法。
                BaseParas.RGBad = libBaseParas.BaseParasGB50545.RGBad;
                BaseParas.RGGood = libBaseParas.BaseParasGB50545.RGGood;
                BaseParas.RQ = libBaseParas.BaseParasGB50545.RQ;
                BaseParas.VcFInstall = libBaseParas.BaseParasGB50545.VcFInstall;
                BaseParas.VcFBroken = libBaseParas.BaseParasGB50545.VcFBroken;
                BaseParas.VcFUnevenIce = libBaseParas.BaseParasGB50545.VcFUnevenIce;
                BaseParas.VcFNormal = libBaseParas.BaseParasGB50545.VcFNormal;
                BaseParas.VcFCheck = libBaseParas.BaseParasGB50545.VcFCheck;

                BaseParas.RGOverturn = libBaseParas.BaseParasDLT5551.RGOverturn;
                BaseParas.VcFIce = libBaseParas.BaseParasDLT5551.VcFIce;
                BaseParas.VcFCold = libBaseParas.BaseParasDLT5551.VcFCold;

                BaseParas.LoadRatio = 1;
                BaseParas.R1Install = 1;
                BaseParas.R0Normal = 1;

                List<StruLineParas> lineParas = new List<StruLineParas>();
                for (int i = 0; i < Template.Wires.Count; i++)
                {
                    lineParas.Add(new StruLineParas()
                    {
                        Index = i + 1,
                        WireType = Template.Wires[i],
                        DrawingCoef = ((towerType == TowerTypeEnum.LineTower || towerType == TowerTypeEnum.LineCornerTower) ? libParas.OverhangingTowerBaseParas.DrawingCoef
                        : libParas.TensionTowerBaseParas.DrawingCoef)
                    });
                }
                LineParas = lineParas;
            }
            //没有默认文件就默认为0
            else
            {
                List<StruLineParas> lineParas = new List<StruLineParas>();
                for (int i = 0; i < Template.Wires.Count; i++)
                {
                    lineParas.Add(new StruLineParas()
                    {
                        Index = i + 1,
                        WireType = Template.Wires[i],
                    });
                }
                LineParas = lineParas;
            }

            //BaseParas下列参数中在映射后赋值的原因在于：
            //如果先复制，在前面的映射过程中，会导致某些值会改变
            BaseParas.SelectedStandard = "GB50545-2010";
            BaseParas.Type = towerType;
            BaseParas.IsMethod1Selected = true;

        }
    }
}

