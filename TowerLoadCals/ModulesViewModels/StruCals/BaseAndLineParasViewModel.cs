using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using TowerLoadCals.Common;
using TowerLoadCals.Mode;
using TowerLoadCals.ModulesViewModels;


namespace TowerLoadCals.Modules
{
    public class BaseAndLineParasViewModel: ViewModelBase, IStruCalsBaseViewModel, INotifyPropertyChanged
    {
        //protected string _selectedStandard = "GB50545-2010";
        public String SelectedStandard {
            get
            {
                
                return BaseParas == null ? null : BaseParas.SelectedStandard;
            }
            set
            {
                if (BaseParas == null)
                    return;
                BaseParas.SelectedStandard = value;
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

        protected ObservableCollection<StruLineParas> _lineParas = new ObservableCollection<StruLineParas>();
        public ObservableCollection<StruLineParas> LineParas {
            get
            {
                return _lineParas;
            }
            set
            {
                _lineParas = value;
                RaisePropertyChanged("LineParas");
            }
        }

        public bool IsMethod1Selected { get; set; }
        public bool IsMethod2Selected { get; set; }

        public string  SelectedMothed { get; set; }

        public BaseAndLineParasViewModel()
        {
        }

        protected override void OnParameterChanged(object parameter)
        {
            InitializeData((string)parameter);
        }

        private void InitializeData(string towerType)
        {
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

            var globalInfo = GlobalInfo.GetInstance();
            int index = globalInfo.StruCalsParas.FindIndex(para => para.TowerName == towerType);

            if (index < 0)
                return;

            var template = globalInfo.StruCalsParas[index].Template;
            LineParas = new ObservableCollection<StruLineParas>(globalInfo.StruCalsParas[index].LineParas);

            BaseParas = globalInfo.StruCalsParas[index].BaseParas;
            SelectedStandard = BaseParas.SelectedStandard;
        }

        public void Save()
        {
            var sss  = LineParas; 
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
            if (BaseParas.Type == TowerType.LineTower)
                return "直线塔";
            else if (BaseParas.Type == TowerType.LineCornerTower)
                return "直转塔";
            else if (BaseParas.Type == TowerType.CornerTower)
                return "转角塔";
            else if (BaseParas.Type == TowerType.BranchTower)
                return "分支塔";
            else
                return "终端塔"; 
        }
    }
}
