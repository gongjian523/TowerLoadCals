using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using TowerLoadCals.BLL;
using TowerLoadCals.Common;
using TowerLoadCals.DAL;
using TowerLoadCals.Mode;
using TowerLoadCals.ModulesViewModels;

namespace TowerLoadCals.Modules
{
    public class WorkConditionComboViewModel: StruCalsBaseViewModel
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

        public StruCalseBaseParas BaseParas { get; set; }

        public WorkConditionComboViewModel()
        {

        }

        protected override void OnParameterChanged(object parameter)
        {
            InitializeData((string)parameter);
        }

        protected override void InitializeData(string towerName)
        {
            base.InitializeData(towerName);

            Template = struCalsParas.Template;

            BaseParas = struCalsParas.BaseParas;

            WorkConditions = new ObservableCollection<WorkConditionComboSpec>(struCalsParas.WorkConditions);

            List<Column> columns = new List<Column>();
            columns.Add(new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "Index", Header = "序号", Width = "*"});
            columns.Add(new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "IsCalculate", Header = "选择与否", Width = "2*" });
            columns.Add(new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "WorkConditionCode", Header = "工况", Width = "*" });
            if (Template.TowerType != "直线塔")
                columns.Add(new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "TensionAngleCode", Header = "张力角", Width = "*" });
            if (Template.TowerType == "转角塔" || Template.TowerType == "分支塔" || Template.TowerType == "终端塔")
                columns.Add(new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "VertialLoadCode", Header = "垂直载荷", Width = "*" });
            columns.Add(new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "WindDirectionCode", Header = "风向", Width = "*" });

            for (int i = 0; i < Template.Wires.Count; i++)
            {
                columns.Add(new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "Wire" + (i + 1).ToString(), Header = Template.Wires[i], Width = "*" });
            }

            columns.Add(new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "WorkComment", Header = "注释", Width = "6*" });

            Columns = new ObservableCollection<Column>(columns);

        }
    }
}
