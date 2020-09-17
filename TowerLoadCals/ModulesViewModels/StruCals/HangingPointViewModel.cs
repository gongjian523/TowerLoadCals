using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using DevExpress.Xpf.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerLoadCals.BLL;
using TowerLoadCals.Common;
using TowerLoadCals.DAL;
using TowerLoadCals.Mode;
using TowerLoadCals.ModulesViewModels;

namespace TowerLoadCals.Modules
{
    public class HangingPointViewModel: StruCalsBaseViewModel
    {
        protected int num = 0;

        public static HangingPointViewModel Create()
        {
            return ViewModelSource.Create(() => new HangingPointViewModel());
        }
        protected HangingPointViewModel()
        {
        }

        protected override void OnParameterChanged(object parameter)
        {
            InitializeData((string)parameter);
        }

        protected override void InitializeData(string towerName)
        {
            base.InitializeData(towerName);

            HangingPoints = new ObservableCollection<HangingPoint>();
            for (num = 1; num < 2; num++)
            {
                HangingPoints.Add(HangingPoint.Create("挂点方案" + num.ToString(), struCalsParas));
            }
        }

        public virtual ObservableCollection<HangingPoint> HangingPoints { get; protected set; }
        public void AddNewTab(TabControlTabAddingEventArgs e)
        {
            struCalsParas.NewHangingPointSetting();
            e.Item = HangingPoint.Create("挂点方案" + num.ToString(), struCalsParas);
            num++;
        }

    }

    public class HangingPoint:ViewModelBase,INotifyPropertyChanged
    {
        public static HangingPoint Create(string title, StruCalsParasCompose calsParas)
        {
            return ViewModelSource.Create(() => new HangingPoint(title, calsParas));
        }
        protected HangingPoint(string title,  StruCalsParasCompose calsParas)
        {
            Title = title;
            Template = calsParas.Template;

            HPSetitingParas = calsParas.HPSettingsParas.Where(item => item.HangingPointSettingName == title).First();
            BaseParas = calsParas.BaseParas;

            NormalXYPoints = new ObservableCollection<HangingPointParas>(HPSetitingParas.NormalXYPoints);
            NormalZPoints = new ObservableCollection<HangingPointParas>(HPSetitingParas.NormalZPoints);
            InstallXYPoints = new ObservableCollection<HangingPointParas>(HPSetitingParas.InstallXYPoints);
            InstallZPoints = new ObservableCollection<HangingPointParas>(HPSetitingParas.InstallZPoints);
            TurningPoints = new ObservableCollection<HangingPointParas>(HPSetitingParas.TurningPoints);
            VStrings = new ObservableCollection<VStringParas>(HPSetitingParas.VStrings);

            List<string> normalSource = new List<string> { "无跳线", "常规", "悬臂",  "I串", "V1", "V2", "V3"};
            List<Column> normalXYColumns = new List<Column>() {
                new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "Index", Header = "序号" },
                new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "WireType", Header = "项目" },
                new ComboColumn() { Settings = SettingsType.Combo, FieldName = "StringType", Header = "串型", Source = normalSource }
            };
            NormalXYColumns = new ObservableCollection<Column>(normalXYColumns);
            int hpNum = GetMaxHPNum(HPSetitingParas.NormalXYPoints);
            if (hpNum == 0)
                hpNum = 1;
            for(int i = 0; i < hpNum; i++)
            {
                AddNormalXYPoint();
            }

            List<Column> normalZColumns = new List<Column>() {
                new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "Index", Header = "序号" },
                new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "WireType", Header = "项目" },
                new ComboColumn() { Settings = SettingsType.Combo, FieldName = "StringType", Header = "串型",Source = normalSource  }
            };
            NormalZColumns = new ObservableCollection<Column>(normalZColumns);
            hpNum = GetMaxHPNum(HPSetitingParas.NormalZPoints);
            if (hpNum == 0)
                hpNum = 1;
            for (int i = 0; i < hpNum; i++)
            {
                AddNormalZPoint();
            }
            
            List<Column> installXYColumns = new List<Column>() {
                new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "Index", Header = "序号" },
                new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "WireType", Header = "项目" },
                new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "Array", Header = "组数" }
            };
            InstallXYColumns = new ObservableCollection<Column>(installXYColumns);
            hpNum = GetMaxHPNum(HPSetitingParas.InstallXYPoints);
            if (hpNum == 0)
                hpNum = 1;
            for (int i = 0; i < hpNum; i++)
            {
                AddInstallXYPoint();
            }

            List<Column> intallZColumns = new List<Column>() {
                new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "Index", Header = "序号" },
                new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "WireType", Header = "项目" },
                new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "Array", Header = "组数" }
            };
            InstallZColumns = new ObservableCollection<Column>(intallZColumns);
            hpNum = GetMaxHPNum(HPSetitingParas.InstallZPoints);
            if (hpNum == 0)
                hpNum = 1;
            for (int i = 0; i < hpNum; i++)
            {
                AddInstallZPoint();
            }

            List<Column> turningColumns = new List<Column>() {
                new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "Index", Header = "序号" },
                new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "WireType", Header = "项目" },
                new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "Angle", Header = "方向角"}
            };
            TurningColumns = new ObservableCollection<Column>(turningColumns);
            hpNum = GetMaxHPNum(HPSetitingParas.TurningPoints);
            if (hpNum == 0)
                hpNum = 1;
            for (int i = 0; i < hpNum; i++)
            {
                AddTurningPoint();
            }
            
        }

        protected TowerTemplate Template;

        protected HangingPointSettingParas _hpSettingParas;
        public HangingPointSettingParas HPSetitingParas
        {
            get
            {
                return _hpSettingParas;
            }
            set
            {
                _hpSettingParas = value;
                RaisePropertyChanged("HPSetitingParas");
            }
        }

        protected StruCalseBaseParas _baseParas;
        public StruCalseBaseParas BaseParas
        {
            get
            {
                return _baseParas;
            }
            set
            {
                _baseParas = value;
                RaisePropertyChanged("BaseParas");
            }
        }

        public string Title { get; private set; }

        public string HPColumnNXYName
        {
            get
            {
                return BaseParas.IsTensionTower ? "常规导线挂点" : "常规挂点 XY向";
            }
        }

        public string HPColumnNZName
        {
            get
            {
                return BaseParas.IsTensionTower ? "常规跳线挂点" : "常规挂点 Z向";
            }
        }

        public string HPColumnXYName
        {
            get
            {
                return BaseParas.IsTensionTower ? "跳线吊装挂点 XY向" : "吊装挂点 XY向";
            }
        }

        public string HPColumnZName
        {
            get
            {
                return BaseParas.IsTensionTower ? "跳线吊装挂点 Z向" : "吊装挂点 Z向";
            }
        }

        public string HPCheckBoxName
        {
            get
            {
                return BaseParas.IsTensionTower ? "过滑车挂架挂点" : "转向挂点";
            }
        }

        //private bool _isTensionHPCheck = false;
        public bool IsTurnHPChecked
        {
            get
            {
                return HPSetitingParas.IsTuringPointSeleced;
            }
            set
            {
                HPSetitingParas.IsTuringPointSeleced = value;
                RaisePropertyChanged("IsTurnHPChecked");
            }
        }

        private bool _isNormalHangingPointVisible = true;
        public bool IsNormalHangingPointVisible
        {
            get
            {
                return _isNormalHangingPointVisible;
            }
            set
            {
                _isNormalHangingPointVisible = value;
                RaisePropertyChanged("IsNormalHangingPointVisible");
            }
        }

        public void NormalHangingPointVisible(bool isNormalVisible)
        {
            IsNormalHangingPointVisible = isNormalVisible;
        }

        protected ObservableCollection<VStringParas> _vStrings { get; set; }
        public ObservableCollection<VStringParas> VStrings 
        {
            get
            {
                return _vStrings;
            }
            set
            {
                _vStrings = value;
                RaisePropertyChanged("VStrings");
            }
        }

        protected int normalXYPointNum;
        public ObservableCollection<Column> NormalXYColumns { get; private set; }
        public ObservableCollection<HangingPointParas> NormalXYPoints { get; set; }

        protected int normalZPointNum;
        public ObservableCollection<Column> NormalZColumns { get; private set; }
        public ObservableCollection<HangingPointParas> NormalZPoints { get; set; }

        protected int installXYPointNum;
        public ObservableCollection<Column> InstallXYColumns { get; private set; }
        public ObservableCollection<HangingPointParas> InstallXYPoints { get; set; }

        protected int installZPointNum;
        public ObservableCollection<Column> InstallZColumns { get; private set; }
        public ObservableCollection<HangingPointParas> InstallZPoints { get; set; }

        protected int turningPointNum;
        public ObservableCollection<Column> TurningColumns { get; private set; }
        public ObservableCollection<HangingPointParas> TurningPoints { get; set; }

        public void AddVString()
        {
            int num = HPSetitingParas.VStrings.Count;

            string newName = "V" + (++num).ToString();

            while(HPSetitingParas.VStrings.Where(item => item.Index == newName).Count() > 0)
            {
                newName = "V" + (++num).ToString();
            }

            HPSetitingParas.VStrings.Add(new VStringParas() { Index = newName });
            VStrings = new ObservableCollection<VStringParas>(HPSetitingParas.VStrings);
        }

        public void AddNormalXYPoint()
        {
            normalXYPointNum++;
            NormalXYColumns.Add(new HeaderColumn() { 
                Settings = SettingsType.Binding,
                FieldName = "Points[" + (normalXYPointNum-1).ToString() + "]", 
                Header = "点" + normalXYPointNum.ToString()
            }
            );
        }

        public void AddNormalZPoint()
        {
            normalZPointNum++;
            NormalZColumns.Add(new HeaderColumn()
            {
                Settings = SettingsType.Binding,
                FieldName = "Points[" + (normalZPointNum - 1).ToString() + "]",
                Header = "点" + normalZPointNum.ToString()
            }
            );
        }

        public void AddInstallXYPoint()
        {
            installXYPointNum++;
            InstallXYColumns.Add(new HeaderColumn()
            {
                Settings = SettingsType.Binding,
                FieldName = "Points[" + (installXYPointNum - 1).ToString() + "]",
                Header = "点" + installXYPointNum.ToString()
            }
            );
        }

        public void AddInstallZPoint()
        {
            installZPointNum++;
            InstallZColumns.Add(new HeaderColumn()
            {
                Settings = SettingsType.Binding,
                FieldName = "Points[" + (installZPointNum - 1).ToString() + "]",
                Header = "点" + installZPointNum.ToString()
            }
            );
        }

        public void AddTurningPoint()
        {
            turningPointNum++;
            TurningColumns.Add(new HeaderColumn()
            {
                Settings = SettingsType.Binding,
                FieldName = "Points[" + (turningPointNum - 1).ToString() + "]",
                Header = "点" + turningPointNum.ToString()
            }
            );
        }

        protected int GetMaxHPNum(List<HangingPointParas> hpParas)
        {
            int max = 0;

            foreach(var item in hpParas)
            {
                max = Math.Max(max, item.PointNum);
            }

            return max;
        }
    }


}
