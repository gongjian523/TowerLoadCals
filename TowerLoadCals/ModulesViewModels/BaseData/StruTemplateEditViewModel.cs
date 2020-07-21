using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TowerLoadCals.Common.Utils;
using System.Windows.Input;
using TowerLoadCals.BLL;
using TowerLoadCals.Mode;
using TowerLoadCals.DAL;
using System.IO;
using System.Collections.ObjectModel;

namespace TowerLoadCals.Modules
{
    public class StruTemplateEditViewModel : ViewModelBase
    {
        //这个参数不仅表示模块是否可以编辑
        //还表示模板的类型是通用模板还是工程模板
        //通用模板不可编辑，工程模板可以编辑
        protected bool _isReadOnly;
        public bool IsReadOnly
        {
            get
            {
                return _isReadOnly;
            }
            set
            {
                _isReadOnly = value;
                RaisePropertyChanged("IsReadOnly");
            }
        }

        protected TowerTemplate _template;

        protected string _towerName;
        public string TowerName
        {
            get
            {
                return _template.Name;
            }
            set
            {
                _template.Name = value;
                RaisePropertyChanged("TowerName");
                RaisePropertyChanged("SaveCanExecute");
            }
        }

        public string TowerType
         {
            get
            {
                return _template.TowerType;
            }
            set
            {
                _template.TowerType = value;
                RaisePropertyChanged("TowerType");
                RaisePropertyChanged("SaveCanExecute");
            }
        }

        protected List<String> _towerTypes = new List<string>() { "直线塔", "直转塔", "转角塔",  "分支塔", "终端塔"};
        public List<String> TowerTypes
        {
            get
            {
                return _towerTypes;
            }
            set
            {
                _towerTypes = value;
            }
        }

        public bool SaveCanExecute
        {
            get
            {
                return (TowerName != null && TowerName.Trim() != "") && (TowerType != null && TowerType.Trim() != "");
            }
        }

        protected string oldName;

        protected int WireNum = 0;
        public ObservableCollection<Column> WireColumns { get; private set; }
        public ObservableCollection<TemplateWire> Wires { get; set; }

        protected int WorkConditionNum = 0;
        public ObservableCollection<Column> WorkConditionColumns { get; private set; }
        public ObservableCollection<TemplateWorkCondition> WorkConditions { get; set; }

        public ObservableCollection<Column> WorkConditionComboColumns { get; private set; }
        public ObservableCollection<WorkConditionComboSpec> WorkConditionCombos { get; set; }

        public StruTemplateEditViewModel(TowerTemplate template,  bool isReadOnly = true)
        {
            oldName =  template.Name;

            _template = template;
            IsReadOnly = isReadOnly;
            

            #region 导地线的初始化
            TemplateWire wire = new TemplateWire();
            wire.Wire = new string[16];

            List<Column> wireColumns = new List<Column>() {
                new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "Name", Header = "序号" },
            };

            if (template.Wires != null)
            {
                for(int i = 0; i < template.Wires.Count; i++)
                {
                    WireNum++;
                    wire.Wire[i] = template.Wires[i];

                    wireColumns.Add(new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "Wire[" + i.ToString() + "]", Header = (i + 1).ToString() });
                }
            }

            Wires = new ObservableCollection<TemplateWire>();
            Wires.Add(wire);

            WireColumns = new ObservableCollection<Column>(wireColumns);
            #endregion


            #region 工况的初始化
            TemplateWorkCondition workCondition = new TemplateWorkCondition();
            workCondition.WorkCondition = new string[16];

            List<Column> workConditionColumns = new List<Column>() {
                new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "Name", Header = "序号" },
            };

            if (template.WorkConditongs != null)
            {
                for (int i = 0; i < template.WorkConditongs.Count; i++)
                {
                    WorkConditionNum++;
                    workCondition.WorkCondition[i] = template.WorkConditongs[i+1];

                    workConditionColumns.Add(new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "WorkCondition[" + i.ToString() + "]", Header = (i + 1).ToString() });
                }
            }

            WorkConditions = new ObservableCollection<TemplateWorkCondition>();
            WorkConditions.Add(workCondition);

            WorkConditionColumns = new ObservableCollection<Column>(workConditionColumns);

            #endregion


            #region 工况的初始化

            WorkConditionCombos = new ObservableCollection<WorkConditionComboSpec>(StruCalsParasCompose.ConvertTemplateToSpec(template));

            List<Column> workConditionComboColumns = new List<Column>();
            workConditionComboColumns.Add(new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "Index", Header = "序号", Width = "*" });
            workConditionComboColumns.Add(new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "IsCalculate", Header = "选择与否", Width = "2*" });
            workConditionComboColumns.Add(new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "WorkConditionCode", Header = "工况", Width = "*" });
            workConditionComboColumns.Add(new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "TensionAngleCode", Header = "张力角", Width = "*" });
            workConditionComboColumns.Add(new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "VertialLoadCode", Header = "垂直载荷", Width = "*" });
            workConditionComboColumns.Add(new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "WindDirectionCode", Header = "风向", Width = "*" });
            for (int i = 0; i < template.Wires.Count; i++)
            {
                workConditionComboColumns.Add(new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "Wire" + (i + 1).ToString(), Header = template.Wires[i], Width = "*" });
            }
            workConditionComboColumns.Add(new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "WorkCode", Header = "工况代码", Width = "*" });
            workConditionComboColumns.Add(new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "WorkComment", Header = "注释", Width = "6*" });

            WorkConditionComboColumns = new ObservableCollection<Column>(workConditionComboColumns);

            #endregion
        }


        public delegate void CloseEditTemplateWindowHandler(object sender, bool isSave);
        public event CloseEditTemplateWindowHandler CloseEditTemplateWindowEvent;

        //只有工程模板才会调用这个函数
        public void onConfirm()
        {
            var proUtils = ProjectUtils.GetInstance();

            //新增模板
            if (oldName == null || oldName == "")
            {
                proUtils.InsertProjectTowerTemplate(new TowerTemplateStorageInfo() {
                    Name = _template.Name,
                    //lcp文件中towerType是中文
                    TowerType = _template.TowerType
                });
            }
            else
            {
                //旧模板改了新名字
                if (oldName != _template.Name)
                {
                    proUtils.UpdateProjectTowerTemplateName(oldName, _template.Name);
                }
                //删除旧模板
                string oldTemplatePath = proUtils.GetProjectlTowerTemplatePath(_template.Name, _template.TowerType);
                if(File.Exists(oldTemplatePath))
                {
                    File.Delete(oldTemplatePath);
                }
            }

            //保存模板
            string newTemplatePath = proUtils.GetProjectlTowerTemplatePath(_template.Name, _template.TowerType);
            NewTowerTemplateReader templateReader = new NewTowerTemplateReader(TowerTypeStringConvert.TowerStringToType(_template.TowerType));
            templateReader.Save(newTemplatePath, _template);

            close(true);
        }

        public void onConcel()
        {
            close(false);
        }

        protected void close(bool isSave)
        {
            if(CloseEditTemplateWindowEvent != null)
                CloseEditTemplateWindowEvent(this, isSave);
        }

        public void AddWire()
        {
            WireColumns.Add(new HeaderColumn() {
                Settings = SettingsType.Binding,
                FieldName = "Wire[" + WireNum.ToString() + "]",
                Header = (WireNum + 1).ToString() }
            );
            _template.Wires.Add("");

            WireNum++;

            WorkConditionComboColumns.Add(new HeaderColumn()
            {
                Settings = SettingsType.Binding,
                FieldName = "Wire" + (WireNum - 1).ToString(),
                Header = _template.Wires[WireNum-1],
                Width = "*"
            });
        }

        public void AddWorkCondition()
        {
            WorkConditionColumns.Add(new HeaderColumn() {
                Settings = SettingsType.Binding,
                FieldName = "WorkCondition[" + WorkConditionNum.ToString() + "]",
                Header = (WorkConditionNum + 1).ToString() }
            );
            WorkConditionNum++;
        }

        public void AddWorkConditionCombo()
        {
            WorkConditionCombos.Add(new WorkConditionComboSpec());
        }

    }


    public class TemplateWire
    {
        public string  Name { get { return "导地线"; } }

        public string[] Wire { get; set; }
    }

    public class TemplateWorkCondition
    {
        public string Name { get { return "工况"; } }

        public string[] WorkCondition{ get; set; }
    }

}
