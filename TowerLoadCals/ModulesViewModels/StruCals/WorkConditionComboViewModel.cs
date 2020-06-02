using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using TowerLoadCals.Common;
using TowerLoadCals.DAL;
using TowerLoadCals.Mode;
using TowerLoadCals.ModulesViewModels;

namespace TowerLoadCals.Modules
{
    public class WorkConditionComboViewModel: ViewModelBase, IStruCalsBaseViewModel, INotifyPropertyChanged
    {
        protected ObservableCollection<WorkConditionComboSpec> _workConditions = new ObservableCollection<WorkConditionComboSpec>();
        public ObservableCollection<WorkConditionComboSpec> WorkConditions
        {
            get
            {
                return _workConditions;
            }
            set
            {
                _workConditions = value;
                RaisePropertyChanged("WorkConditions");
            }
        }

        protected ObservableCollection<Column> _columns = new ObservableCollection<Column>();
        public ObservableCollection<Column> Columns
        {
            get
            {
                return _columns;
            }
            set
            {
                _columns = value;
                RaisePropertyChanged("Columns");
            }
        }

        public TowerTemplate Template { get; set; }

        public FormulaParas BaseParas { get; set; }

        public WorkConditionComboViewModel()
        {
            //TowerTemplateReader templateReader = new TowerTemplateReader(TowerType.LineTower);
            //TowerTemplate template = templateReader.Read("D:\\00-项目\\P-200325-杆塔负荷程序\\数据资源示例\\塔库\\双回交流重冰区.dat");
            ////TowerTemplate template = templateReader.Read("D:\\智菲\\P-200325-杆塔负荷程序\\双回交流重冰区.dat");

            //WorkConditions = templateReader.ConvertTemplateToSpec(template);

            //List<Column> columns = new List<Column>();
            //columns.Add(new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "Index", Header = "序号" });
            //columns.Add(new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "IsCalculate", Header = "选择与否" });
            //columns.Add(new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "WorkConditionCode", Header = "工况" });
            //if(template.TowerType != "直线塔")
            //    columns.Add(new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "TensionAngleCode", Header = "张力角" });
            //if (template.TowerType == "转角塔" || template.TowerType == "分支塔" || template.TowerType == "终端塔")
            //    columns.Add(new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "VertialLoadCode", Header = "垂直载荷" });
            //columns.Add(new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "WindDirectionCode", Header = "风向" });

            //for(int i = 0; i< template.Wires.Count; i++)
            //{
            //    columns.Add(new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "Wire"+ (i+1).ToString(), Header = template.Wires[i] });
            //}

            //columns.Add(new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "WorkComment", Header = "注释" });

            //Columns = new ObservableCollection<Column>(columns);
        }

        protected override void OnParameterChanged(object parameter)
        {
            InitializeData((string)parameter);
        }

        private void InitializeData(string towerType)
        {
            var globalInfo = GlobalInfo.GetInstance();
            int index = globalInfo.StruCalsParas.FindIndex(para => para.TowerName == towerType);

            if (index < 0)
                return;

            Template = globalInfo.StruCalsParas[index].Template;

            BaseParas = globalInfo.StruCalsParas[index].BaseParas;

            TowerTemplateReader templateReader = new TowerTemplateReader(BaseParas.Type);

            WorkConditions = new ObservableCollection<WorkConditionComboSpec>(templateReader.ConvertTemplateToSpec(Template));

            List<Column> columns = new List<Column>();
            columns.Add(new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "Index", Header = "序号" });
            columns.Add(new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "IsCalculate", Header = "选择与否" });
            columns.Add(new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "WorkConditionCode", Header = "工况" });
            if (Template.TowerType != "直线塔")
                columns.Add(new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "TensionAngleCode", Header = "张力角" });
            if (Template.TowerType == "转角塔" || Template.TowerType == "分支塔" || Template.TowerType == "终端塔")
                columns.Add(new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "VertialLoadCode", Header = "垂直载荷" });
            columns.Add(new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "WindDirectionCode", Header = "风向" });

            for (int i = 0; i < Template.Wires.Count; i++)
            {
                columns.Add(new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "Wire" + (i + 1).ToString(), Header = Template.Wires[i] });
            }

            columns.Add(new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "WorkComment", Header = "注释" });

            Columns = new ObservableCollection<Column>(columns);

        }

        public string GetTowerType()
        {
            return Template.TowerType;
        }

        public void Save()
        {
            var sss = WorkConditions;
        }

        public void UpDateView(string para1, string para2 = "")
        {
            throw new NotImplementedException();
        }

        public void DelSubItem(string itemName)
        {
            throw new NotImplementedException();
        }
    }
}
