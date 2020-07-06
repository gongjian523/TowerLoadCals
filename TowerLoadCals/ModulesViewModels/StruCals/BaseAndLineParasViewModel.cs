using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using TowerLoadCals.BLL;
using TowerLoadCals.Mode;
using TowerLoadCals.ModulesViewModels;


namespace TowerLoadCals.Modules
{
    public class BaseAndLineParasViewModel: StruCalsBaseViewModel
    {
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

        protected override void  InitializeData(string towerName)
        {
            base.InitializeData(towerName);

            var template = struCalsParas.Template;
            LineParas = new ObservableCollection<StruLineParas>(struCalsParas.LineParas);

            BaseParas = struCalsParas.BaseParas;
            SelectedStandard = BaseParas.SelectedStandard;
        }


    }
}
