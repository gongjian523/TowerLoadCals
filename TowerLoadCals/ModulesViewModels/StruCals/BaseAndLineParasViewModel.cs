using DevExpress.Mvvm;
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
using static TowerLoadCals.DAL.TowerTemplateReader;

namespace TowerLoadCals.Modules
{
    public class BaseAndLineParasViewModel: ViewModelBase, IStruCalsBaseViewModel, INotifyPropertyChanged
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
        }
        
        
        public BaseAndLineParasViewModel(string towerType)
        {
            if (towerType == "直线塔")
            {
                Type = TowerType.LineTower;
            }
            else if (towerType == "直转塔")
            {
                Type = TowerType.LineCornerTower;
            }
            else if (towerType == "转角塔")
            {
                Type = TowerType.CornerTower;
            }
            else if (towerType == "分支塔")
            {
                Type = TowerType.BranchTower;
            }
            else
            {
                Type = TowerType.TerminalTower;
            }

            var globalInfo = GlobalInfo.GetInstance();
            int index = globalInfo.StruCalsParas.FindIndex(para => para.TowerName == towerType);

            if (index < 0)
                return;

            var template = globalInfo.StruCalsParas[index].Template;
            LineParas = new ObservableCollection<StruLineParas>(globalInfo.StruCalsParas[index].LineParas);
        }

        public void Save()
        {
            
        }

        public void UpDateView(string para1, string para2)
        {
            throw new NotImplementedException();
        }

        public void DelSubItem(string itemName)
        {
            throw new NotImplementedException();
        }

        public string GetTowerType()
        {
            if (Type == TowerType.LineTower)
                return "直线塔";
            else if (Type == TowerType.LineCornerTower)
                return "直转塔";
            else if (Type == TowerType.CornerTower)
                return "转角塔";
            else if (Type == TowerType.BranchTower)
                return "分支塔";
            else
                return "终端塔"; 
        }
    }
}
