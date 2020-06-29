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


        public BaseAndLineParasViewModel()
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

            var template = globalInfo.StruCalsParas[index].Template;
            LineParas = new ObservableCollection<StruLineParas>(globalInfo.StruCalsParas[index].LineParas);

            BaseParas = globalInfo.StruCalsParas[index].BaseParas;
            SelectedStandard = BaseParas.SelectedStandard;
        }

        public void Save()
        {
            var ssss = BaseParas.IsMethod1Selected;
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
            return TowerTypeStringConvert.TowerTypeToString(BaseParas.Type);
        }
    }
}
