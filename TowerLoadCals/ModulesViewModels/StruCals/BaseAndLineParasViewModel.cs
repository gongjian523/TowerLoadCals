using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerLoadCals.DAL;
using TowerLoadCals.Mode;
using TowerLoadCals.ModulesViewModels;
using static TowerLoadCals.DAL.TowerTemplateReader;

namespace TowerLoadCals.Modules
{
    public class BaseAndLineParasViewModel: ViewModelBase,IBaseViewModel, INotifyPropertyChanged
    {
        protected string _selectedStandard = "GB50545-2010";
        public String SelectedStandard {
            get
            {
                return _selectedStandard;
            }
            set
            {
                _selectedStandard = value;
                RaisePropertyChanged("SelectedStandard");
            }
        }

        protected List<String> _standards = new List<string>() { "GB50545-2010", "DL/T5551-2018" };
        public List<String>  Standards
        {
            get {
                return _standards;
            }
            set
            {
                _standards = value;
            }
        }

       protected FormulaParas _baseParas = new FormulaParas();


        public FormulaParas BaseParas
        {
            get
            {
                return _baseParas;
            }
            set
            {
                _baseParas = value;
            }
        }
        public ObservableCollection<StruLineParas> LineParas { get; set; }

        public TowerType  Type { get; set; }

        public bool IsLineTower { get
            {
                return Type == TowerType.LineTower;
            }
        }

        public bool IsCornerTower
        {
            get
            {
                return Type == TowerType.CornerTower;
            }
        }

        public bool IsLineCornerTower
        {
            get
            {
                return Type == TowerType.LineCornerTower;
            }
        }

        public bool IsBranchTower
        {
            get
            {
                return Type == TowerType.BranchTower;
            }
        }

        public bool IsTerminalTower
        {
            get
            {
                return Type == TowerType.TerminalTower;
            }
        }

        public bool IsOtherParasAngleVisible
        {
            get
            {
                return Type == TowerType.LineTower;
            }
        }

        public bool IsOtherParasJumpVisible
        {
            get
            {
                return (Type == TowerType.CornerTower || Type == TowerType.TerminalTower || Type == TowerType.BranchTower);
            }
        }

        public bool IsMethod1Selected { get; set; }
        public bool IsMethod2Selected { get; set; }

        public string  SelectedMothed { get; set; }

        public BaseAndLineParasViewModel()
        {
            Type = TowerType.BranchTower;

            BaseParas.R1Install = 1;

            IsMethod1Selected = true;
            IsMethod2Selected = false;

            LineParas = new ObservableCollection<StruLineParas>();
            TowerTemplateReader templateReader = new TowerTemplateReader(TowerType.LineTower);
            //TowerTemplate template = templateReader.Read("D:\\00-项目\\P-200325-杆塔负荷程序\\数据资源示例\\塔库\\双回交流重冰区.dat");
            TowerTemplate template = templateReader.Read("D:\\智菲\\P-200325-杆塔负荷程序\\双回交流重冰区.dat");

            for (int i = 0; i < template.Wires.Count; i++)
            {
                LineParas.Add(new StruLineParas { Index = i+1, WireType  = template.Wires[i]});
            }
        }

        void IBaseViewModel.Save()
        {
            var itme = BaseParas;
            var ti = SelectedStandard;

            var aaa = IsMethod1Selected ;
            var sd = SelectedMothed;
            var sss = LineParas;
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
}
