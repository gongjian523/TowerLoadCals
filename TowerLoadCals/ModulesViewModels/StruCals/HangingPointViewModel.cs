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
using TowerLoadCals.Common;
using TowerLoadCals.DAL;
using TowerLoadCals.Mode;
using TowerLoadCals.ModulesViewModels;

namespace TowerLoadCals.Modules
{
    public class HangingPointViewModel: ViewModelBase, IStruCalsBaseViewModel, INotifyPropertyChanged
    {
        protected int num = 0;
        protected TowerTemplate template;
        protected string towerType;
        protected StruCalsParas struCalsParas;

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

        private void InitializeData(string towerType)
        {
            var globalInfo = GlobalInfo.GetInstance();
            int index = globalInfo.StruCalsParas.FindIndex(para => para.TowerName == towerType);

            if (index < 0)
                return;

            struCalsParas = globalInfo.StruCalsParas[index];

            HangingPoints = new ObservableCollection<HangingPoint>();
            for (num = 1; num < 2; num++)
            {
                HangingPoints.Add(HangingPoint.Create("挂点方案" + num.ToString(), "直线塔", struCalsParas));
            }
        }


        public HangingPointViewModel(string type)
        {

            var globalInfo = GlobalInfo.GetInstance();
            int index = globalInfo.StruCalsParas.FindIndex(para => para.TowerName == towerType);

            if (index < 0)
                return;

            template = globalInfo.StruCalsParas[index].Template;
            towerType = type;

            HangingPoints = new ObservableCollection<HangingPoint>();
            for (num = 1; num < 2; num++)
            {
                HangingPoints.Add(HangingPoint.Create("挂点方案" + num.ToString(), towerType, struCalsParas));
            }
        }

        public virtual ObservableCollection<HangingPoint> HangingPoints { get; protected set; }
        public void AddNewTab(TabControlTabAddingEventArgs e)
        {
            e.Item = HangingPoint.Create("挂点方案" + num.ToString(), towerType, struCalsParas);
            num++;
        }

        string IStruCalsBaseViewModel.GetTowerType()
        {
            return towerType;
        }

        void IBaseViewModel.Save()
        {
            throw new NotImplementedException();
        }

        void IBaseViewModel.UpDateView(string para1, string para2)
        {
            throw new NotImplementedException();
        }

        void IBaseViewModel.DelSubItem(string itemName)
        {
            throw new NotImplementedException();
        }
    }

    public class HangingPoint:ViewModelBase,INotifyPropertyChanged
    {
        public static HangingPoint Create(string title, string towerType, StruCalsParas calsParas)
        {
            return ViewModelSource.Create(() => new HangingPoint(title, towerType, calsParas));
        }
        protected HangingPoint(string title,  string towerType, StruCalsParas calsParas)
        {
            Title = title;

            //if (towerType == "直线塔")
            //{
            //    Type = TowerType.LineTower;
            //}
            //else if (towerType == "直转塔")
            //{
            //    Type = TowerType.LineCornerTower;
            //}
            //else if (towerType == "转角塔")
            //{
            //    Type = TowerType.CornerTower;
            //}
            //else if (towerType == "分支塔")
            //{
            //    Type = TowerType.BranchTower;
            //}
            //else
            //{
            //    Type = TowerType.TerminalTower;
            //}
            //Type = TowerType.LineTower;

            VStrings = new ObservableCollection<VStringParas>();

            Template = calsParas.Template;
            RatioParas = calsParas.RatioParas;
            BaseParas = calsParas.BaseParas;

            NormalXYPoints = new ObservableCollection<HangingPointParas>();
            NormalZPoints = new ObservableCollection<HangingPointParas>();
            InstallXYPoints = new ObservableCollection<HangingPointParas>();
            InstallZPoints = new ObservableCollection<HangingPointParas>();
            TurningPoints = new ObservableCollection<HangingPointParas>();

            for (int i = 0; i < Template.Wires.Count; i++)
            {
                NormalXYPoints.Add(new HangingPointParas { Index = i + 1, WireType = Template.Wires[i] });
                NormalZPoints.Add(new HangingPointParas { Index = i + 1, WireType = Template.Wires[i] });
                InstallXYPoints.Add(new HangingPointParas { Index = i + 1, WireType = Template.Wires[i] });
                InstallZPoints.Add(new HangingPointParas { Index = i + 1, WireType = Template.Wires[i] });
                TurningPoints.Add(new HangingPointParas { Index = i + 1, WireType = Template.Wires[i] });
            }

            List<string> normalSource = new List<string> { "悬臂", "V1", "V2", "V3"};
            List<Column> normalXYColumns = new List<Column>() {
                new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "Index", Header = "序号" },
                new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "WireType", Header = "项目" },
                new ComboColumn() { Settings = SettingsType.Combo, FieldName = "StringType", Header = "串型",Source = normalSource },
                new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "Point1", Header = "点1" }
            };
            NormalXYColumns = new ObservableCollection<Column>(normalXYColumns);
            List<Column> normalZColumns = new List<Column>() {
                new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "Index", Header = "序号" },
                new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "WireType", Header = "项目" },
                new ComboColumn() { Settings = SettingsType.Combo, FieldName = "StringType", Header = "串型",Source = normalSource },
                new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "Point1", Header = "点1" }
            };
            NormalZColumns = new ObservableCollection<Column>(normalZColumns);

            List<string> installSource = new List<string> { "组a", "组b", "组c", "组d" };
            List<Column> installXYColumns = new List<Column>() {
                new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "Index", Header = "序号" },
                new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "WireType", Header = "项目" },
                new ComboColumn() { Settings = SettingsType.Combo, FieldName = "Array", Header = "组数",Source = installSource },
                new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "Point1", Header = "点1" }
            };
            InstallXYColumns = new ObservableCollection<Column>(installXYColumns);
            List<Column> intallZColumns = new List<Column>() {
                new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "Index", Header = "序号" },
                new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "WireType", Header = "项目" },
                new ComboColumn() { Settings = SettingsType.Combo, FieldName = "Array", Header = "组数",Source = installSource },
                new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "Point1", Header = "点1" }
            };
            InstallZColumns = new ObservableCollection<Column>(intallZColumns);

            List<Column> turningColumns = new List<Column>() {
                new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "Index", Header = "序号" },
                new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "WireType", Header = "项目" },
                new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "Angle", Header = "方向角"},
                new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "Point1", Header = "点1" }
            };
            TurningColumns = new ObservableCollection<Column>(turningColumns);
        }

        protected TowerTemplate Template;

        protected StruRatioParas _ratioParas;
        public StruRatioParas RatioParas
        {
            get
            {
                return _ratioParas;
            }
            set
            {
                _ratioParas = value;
                RaisePropertyChanged("RatioParas");
            }
        }

        protected FormulaParas _baseParas;
        public FormulaParas BaseParas
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

        //public TowerType Type { get; set; }

        //public bool IsLineTower
        //{
        //    get
        //    {
        //        return Type == TowerType.LineTower;
        //    }
        //}

        //public bool IsTensionTower
        //{
        //    get
        //    {
        //        return(Type == TowerType.CornerTower || Type ==  TowerType.BranchTower || Type == TowerType.TerminalTower);
        //    }
        //}

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

        private bool _isTensionHPCheck = false;
        public bool IsTurnHPChecked
        {
            get
            {
                return _isTensionHPCheck;
            }
            set
            {
                _isTensionHPCheck = value;
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

        public ObservableCollection<VStringParas> VStrings { get;  set; }

        protected int normalXYPointNum = 1;
        public ObservableCollection<Column> NormalXYColumns { get; private set; }
        public ObservableCollection<HangingPointParas> NormalXYPoints { get; set; }

        protected int normalZPointNum = 1;
        public ObservableCollection<Column> NormalZColumns { get; private set; }
        public ObservableCollection<HangingPointParas> NormalZPoints { get; set; }

        protected int installXYPointNum = 1;
        public ObservableCollection<Column> InstallXYColumns { get; private set; }
        public ObservableCollection<HangingPointParas> InstallXYPoints { get; set; }

        protected int installZPointNum = 1;
        public ObservableCollection<Column> InstallZColumns { get; private set; }
        public ObservableCollection<HangingPointParas> InstallZPoints { get; set; }

        protected int turningPointNum = 1;
        public ObservableCollection<Column> TurningColumns { get; private set; }
        public ObservableCollection<HangingPointParas> TurningPoints { get; set; }

        public void AddNormalXYPoint()
        {
            normalXYPointNum++;
            NormalXYColumns.Add(new HeaderColumn() { 
                Settings = SettingsType.Binding, 
                FieldName = "Point" + normalXYPointNum.ToString(), 
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
                FieldName = "Point" + normalZPointNum.ToString(),
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
                FieldName = "Point" + installXYPointNum.ToString(),
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
                FieldName = "Point" + installZPointNum.ToString(),
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
                FieldName = "Point" + turningPointNum.ToString(),
                Header = "点" + turningPointNum.ToString()
            }
            );
        }

    }


}
