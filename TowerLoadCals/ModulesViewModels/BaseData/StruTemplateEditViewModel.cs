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
using System.Reflection;

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
                RaisePropertyChanged("IsTensionAngleVisible");
                RaisePropertyChanged("IsVertialLoadVisible");
            }
        }

        public bool IsTensionAngleVisible
        {
            get
            {
                return _template.TowerType != "直线塔" ? true : false;
            }
        }

        public bool IsVertialLoadVisible
        {
            get
            {
                return (_template.TowerType == "转角塔" || _template.TowerType == "分支塔" || _template.TowerType == "终端塔") ? true : false;
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

        //public ObservableCollection<Column> WorkConditionComboColumns { get; private set; }
        //public ObservableCollection<WorkConditionComboSpec> WorkConditionCombos { get; set; }
        public ObservableCollection<WorkConditionCombo> WorkConditionCombos { get; set; }

        

        public StruTemplateEditViewModel(TowerTemplate template,  bool isReadOnly = true)
        {
            oldName =  template.Name;

            _template = template;
            IsReadOnly = isReadOnly;
            

            #region 导地线的初始化
            TemplateWire wire = new TemplateWire();
            wire.Wire = new string[16];

            List<Column> wireColumns = new List<Column>() {
                new HeaderColumn() {
                    Settings = SettingsType.Binding,
                    FieldName = "Name",
                    Header = "序号",
                    AllowEditing = (!isReadOnly).ToString(),
                },
            };

            if (template.Wires != null)
            {
                for(int i = 0; i < template.Wires.Count; i++)
                {
                    WireNum++;
                    wire.Wire[i] = template.Wires[i];

                    wireColumns.Add(new HeaderColumn() {
                        Settings = SettingsType.Binding,
                        FieldName = "Wire[" + i.ToString() + "]",
                        Header = (i + 1).ToString(),
                        AllowEditing = (!isReadOnly).ToString(),
                    });

                    SetWireVisbility(WireNum);
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
                new HeaderColumn() {
                    Settings = SettingsType.Binding,
                    FieldName = "Name",
                    Header = "序号",
                    AllowEditing = (!isReadOnly).ToString(),
                },
            };

            if (template.WorkConditongs != null)
            {
                for (int i = 0; i < template.WorkConditongs.Count; i++)
                {
                    WorkConditionNum++;
                    workCondition.WorkCondition[i] = template.WorkConditongs[i+1];

                    workConditionColumns.Add(new HeaderColumn()
                    {
                        Settings = SettingsType.Binding,
                        FieldName = "WorkCondition[" + i.ToString() + "]",
                        Header = (i + 1).ToString(),
                        AllowEditing = (!isReadOnly).ToString(),
                    });
                }
            }

            WorkConditions = new ObservableCollection<TemplateWorkCondition>();
            WorkConditions.Add(workCondition);

            WorkConditionColumns = new ObservableCollection<Column>(workConditionColumns);

            #endregion


            #region 工况的初始化

            //WorkConditionCombos = new ObservableCollection<WorkConditionComboSpec>(StruCalsParasCompose.ConvertTemplateToSpec(template));

            WorkConditionCombos = new ObservableCollection<WorkConditionCombo>(template.WorkConditionCombos);


            //List<Column> workConditionComboColumns = new List<Column>();
            //workConditionComboColumns.Add(new HeaderColumn() {
            //    Settings = SettingsType.Binding,
            //    FieldName = "Index",
            //    Header = "序号",
            //    Width = "*",
            //    AllowEditing = (!isReadOnly).ToString(),
            //});
            //workConditionComboColumns.Add(new HeaderColumn() {
            //    Settings = SettingsType.Binding,
            //    FieldName = "IsCalculate",
            //    Header = "选择与否",
            //    Width = "2*",
            //    AllowEditing = (!isReadOnly).ToString(),
            //});
            //workConditionComboColumns.Add(new HeaderColumn() {
            //    Settings = SettingsType.Binding,
            //    FieldName = "WorkConditionCode",
            //    Header = "工况",
            //    Width = "*",
            //    AllowEditing = (!isReadOnly).ToString(),

            //});
            //workConditionComboColumns.Add(new HeaderColumn() {
            //    Settings = SettingsType.Binding,
            //    FieldName = "TensionAngleCode",
            //    Header = "张力角",
            //    Width = "*",
            //    AllowEditing = (!isReadOnly).ToString(),
            //    Visible = "{Binding IsTensionAngleVisible}"
            //});
            //workConditionComboColumns.Add(new HeaderColumn() {
            //    Settings = SettingsType.Binding,
            //    FieldName = "VertialLoadCode",
            //    Header = "垂直载荷",
            //    Width = "*",
            //    AllowEditing = (!isReadOnly).ToString(),
            //    Visible = "{Binding IsVertialLoadCodeVisible}"
            //});
            //workConditionComboColumns.Add(new HeaderColumn() {
            //    Settings = SettingsType.Binding,
            //    FieldName = "WindDirectionCode",
            //    Header = "风向",
            //    Width = "*",
            //    AllowEditing = (!isReadOnly).ToString(),
            //});
            //for (int i = 0; i < template.Wires.Count; i++)
            //{
            //    workConditionComboColumns.Add(new HeaderColumn() {
            //        Settings = SettingsType.Binding,
            //        FieldName = "Wire" + (i + 1).ToString(),
            //        Header = template.Wires[i],
            //        Width = "*",
            //        AllowEditing = (!isReadOnly).ToString(),
            //    });
            //}
            //workConditionComboColumns.Add(new HeaderColumn() {
            //    Settings = SettingsType.Binding,
            //    FieldName = "WorkCode",
            //    Header = "工况代码",
            //    Width = "*",
            //    AllowEditing = (!isReadOnly).ToString(),
            //});
            //workConditionComboColumns.Add(new HeaderColumn() {
            //    Settings = SettingsType.Binding,
            //    FieldName = "WorkComment",
            //    Header = "注释",
            //    Width = "6*",
            //    AllowEditing = (!isReadOnly).ToString(),
            //});

            //WorkConditionComboColumns = new ObservableCollection<Column>(workConditionComboColumns);

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
                Header = (++WireNum).ToString() }
            );
            _template.Wires.Add("");

            SetWireVisbility(WireNum);

            //WorkConditionComboColumns.Insert((6 + WireNum), new HeaderColumn()
            //{
            //    Settings = SettingsType.Binding,
            //    Header = _template.Wires[WireNum],
            //    FieldName = "Wire" + (++WireNum).ToString(),
            //    Width = "*"
            //});

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
            int index = WorkConditionCombos.Count + 1;
            //WorkConditionCombos.Add(new WorkConditionComboSpec() {  Index = index });
            WorkConditionCombos.Add(new WorkConditionCombo() { Index = index });
        }

        public void WiresGridChanged(string index)
        {
            RaisePropertyChanged("Wire"+ index + "Name");
        }

        protected void SetWireVisbility(int index)
        {
            Type vwType = GetType();
            PropertyInfo vwPro = vwType.GetProperty("Wire" + index.ToString() + "Visible", BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public);
            if (vwPro != null)
                vwPro.SetValue(this, true);
        }

        //导地线的名字和可见性 
        //在xml中绑定无法使用数组，还无法找到更简洁和通用的写法，
        //按时使用罗列的办法，最多支持16根导线
        #region Wire
        public String Wire1Name
        {
            get { return Wires[0].Wire[0]; }
        }

        protected bool _wire1Visible = false;
        public bool Wire1Visible
        {
            get{ return _wire1Visible; }
            set{
                _wire1Visible = value;
                RaisePropertyChanged("Wire1Visible"); }
        }

        public String Wire2Name
        {
            get{ return Wires[0].Wire[1]; }
        }

        protected bool _wire2Visible = false;
        public bool Wire2Visible
        {
            get { return _wire2Visible; } 
            set {
                _wire2Visible = value;
                RaisePropertyChanged("Wire2Visible");}
        }

        public String Wire3Name
        {
            get{ return Wires[0].Wire[2];}
        }

        protected bool _wire3Visible = false;
        public bool Wire3Visible
        {
            get {  return _wire3Visible; }
            set
            {
                _wire3Visible = value;
                RaisePropertyChanged("Wire3Visible");}
        }

        public String Wire4Name
        {
            get { return Wires[0].Wire[3]; }
        }

        protected bool _wire4Visible = false;
        public bool Wire4Visible
        {
            get { return _wire4Visible; }
            set
            {
                _wire4Visible = value;
                RaisePropertyChanged("Wire4Visible");
            }
        }

        public String Wire5Name
        {
            get { return Wires[0].Wire[4]; }
        }

        protected bool _wire5Visible = false;
        public bool Wire5Visible
        {
            get { return _wire5Visible; }
            set
            {
                _wire5Visible = value;
                RaisePropertyChanged("Wire5Visible");
            }
        }

        public String Wire6Name
        {
            get { return Wires[0].Wire[5]; }
        }

        protected bool _wire6Visible = false;
        public bool Wire6Visible
        {
            get { return _wire6Visible; }
            set
            {
                _wire6Visible = value;
                RaisePropertyChanged("Wire6Visible");
            }
        }

        public String Wire7Name
        {
            get { return Wires[0].Wire[6]; }
        }

        protected bool _wire7Visible = false;
        public bool Wire7Visible
        {
            get { return _wire7Visible; }
            set
            {
                _wire7Visible = value;
                RaisePropertyChanged("Wire7Visible");
            }
        }

        public String Wire8Name
        {
            get { return Wires[0].Wire[7]; }
        }

        protected bool _wire8Visible = false;
        public bool Wire8Visible
        {
            get { return _wire8Visible; }
            set
            {
                _wire8Visible = value;
                RaisePropertyChanged("Wire8Visible");
            }
        }

        public String Wire9Name
        {
            get { return Wires[0].Wire[8]; }
        }

        protected bool _wire9Visible = false;
        public bool Wire9Visible
        {
            get { return _wire9Visible; }
            set
            {
                _wire9Visible = value;
                RaisePropertyChanged("Wire9Visible");
            }
        }

        public String Wire10Name
        {
            get { return Wires[0].Wire[9]; }
        }

        protected bool _wire10Visible = false;
        public bool Wire10Visible
        {
            get { return _wire10Visible; }
            set
            {
                _wire10Visible = value;
                RaisePropertyChanged("Wire10Visible");
            }
        }

        public String Wire11Name
        {
            get { return Wires[0].Wire[10]; }
        }

        protected bool _wire11Visible = false;
        public bool Wire11Visible
        {
            get { return _wire11Visible; }
            set
            {
                _wire11Visible = value;
                RaisePropertyChanged("Wire11Visible");
            }
        }

        public String Wire12Name
        {
            get { return Wires[0].Wire[11]; }
        }

        protected bool _wire12Visible = false;
        public bool Wire12Visible
        {
            get { return _wire12Visible; }
            set
            {
                _wire12Visible = value;
                RaisePropertyChanged("Wire12Visible");
            }
        }

        public String Wire13Name
        {
            get { return Wires[0].Wire[12]; }
        }

        protected bool _wire13Visible = false;
        public bool Wire13Visible
        {
            get { return _wire13Visible; }
            set
            {
                _wire13Visible = value;
                RaisePropertyChanged("Wire13Visible");
            }
        }

        public String Wire14Name
        {
            get { return Wires[0].Wire[13]; }
        }

        protected bool _wire14Visible = false;
        public bool Wire14Visible
        {
            get { return _wire14Visible; }
            set
            {
                _wire14Visible = value;
                RaisePropertyChanged("Wire14Visible");
            }
        }

        public String Wire15Name
        {
            get { return Wires[0].Wire[14]; }
        }

        protected bool _wire15Visible = false;
        public bool Wire15Visible
        {
            get { return _wire15Visible; }
            set
            {
                _wire15Visible = value;
                RaisePropertyChanged("Wire15Visible");
            }
        }

        public String Wire16Name
        {
            get { return Wires[0].Wire[15]; }
        }

        protected bool _wire16Visible = false;
        public bool Wire16Visible
        {
            get { return _wire16Visible; }
            set
            {
                _wire16Visible = value;
                RaisePropertyChanged("Wire16Visible");
            }
        }


        #endregion
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
