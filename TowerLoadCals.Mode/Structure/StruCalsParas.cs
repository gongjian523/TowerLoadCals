using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.Mode
{
    public class StruCalsParas
    {

        public StruCalsParas(string name, string tablePath, TowerTemplate template)
        {
            TowerName = name;

            TablePath = tablePath;

            Template = template;

            WorkConditions = ConvertTemplateToSpec(template);

            BaseParas = new FormulaParas() {
                SelectedStandard = "GB50545-2010",
                Type = TowerTypeStringConvert.TowerStringToType(template.TowerType),
                IsMethod1Selected = true
            };

            List<StruLineParas> lineParas = new List<StruLineParas>();
            for (int i = 0; i < template.Wires.Count; i++)
            {
                lineParas.Add(new StruLineParas { Index = i + 1, WireType = template.Wires[i] });
            }
            LineParas = lineParas;

            HPSettingsParas = new List<HangingPointSettingParas>();
            NewHangingPointSetting();

            ResultPointLoad = new List<StruCalsPointLoad>();
        }

        public String TowerName { get; set; }

        public String TablePath { get; set; }

        public TowerTemplate Template { get; set; }

        // 从Template转换而来，用于WorkConditionComboModule
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

            HangingPointParas[] installPoint = new HangingPointParas[arrList.Count * numDW];


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
                    installPoint[j * numDW + i - numGW] = new HangingPointParas {
                        Index = j * numDW + i - numGW + 1,
                        Array = "第" + arrList[j] + "组",
                        WireType = Template.Wires[i]
                    };
                }

                paras.TurningPoints.Add(new HangingPointParas { Index = i - numGW + 1, WireType = Template.Wires[i] });
            }

            paras.InstallXYPoints = installPoint.ToList();
            paras.InstallZPoints = installPoint.ToList();

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

        

    }
}

