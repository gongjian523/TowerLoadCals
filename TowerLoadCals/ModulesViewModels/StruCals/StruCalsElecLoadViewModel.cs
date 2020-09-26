using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using TowerLoadCals.BLL;
using TowerLoadCals.Mode;
using TowerLoadCals.Mode.Structure;
using TowerLoadCals.ModulesViewModels;


namespace TowerLoadCals.Modules
{
    public class StruCalsElecLoadViewModel : StruCalsBaseViewModel
    {
        protected StruCalsElecLoadModule View;

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

        protected ObservableCollection<StruCalsTension> _wind45TenParas = new ObservableCollection<StruCalsTension>();
        public ObservableCollection<StruCalsTension> Wind45TenParas
        {
            get
            {
                return _wind45TenParas;
            }
            set
            {
                _wind45TenParas = value;
                RaisePropertyChanged("Wind45TenParas");
            }
        }

        public StruCalsElecLoad ElecLoad { get; set; }


        public StruCalsElecLoadViewModel(StruCalsElecLoadModule view)
        {
            View = view;
        }

        protected override void OnParameterChanged(object parameter)
        {
            InitializeData((string)parameter);
            View.InitElecLoadSheet(BaseParas.Type, ElecLoad);
        }

        protected override void  InitializeData(string towerName)
        {
            base.InitializeData(towerName);

            BaseParas = struCalsParas.BaseParas;
            ElecLoad = struCalsParas.ElecLoad;
            Wind45TenParas = new ObservableCollection<StruCalsTension>(struCalsParas.ElecLoad.ExtraLoad); 
        }


        public override void Save()
        {
            //从WorkSheet获取最新的数据
            View.CollectElecLoadFromWorkSheet();
            ProjectUtils.GetInstance().SaveStruCalsTower();
        }
    }
}
