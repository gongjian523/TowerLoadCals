using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using TowerLoadCals.Common;
using TowerLoadCals.DAL;
using TowerLoadCals.Mode;
using TowerLoadCals.ModulesViewModels;

namespace TowerLoadCals.Modules
{
    public class StruCalsResultViewModel : ViewModelBase, IStruCalsBaseViewModel, INotifyPropertyChanged
    {
        protected ObservableCollection<StruCalsResult> _results = new ObservableCollection<StruCalsResult>();
        public ObservableCollection<StruCalsResult> Results
        {
            get
            {
                return _results;
            }
            set
            {
                _results = value;
                RaisePropertyChanged("Results");
            }
        }

        public TowerTemplate Template { get; set; }

        public FormulaParas BaseParas { get; set; }

        public StruCalsResultViewModel()
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

            Template = globalInfo.StruCalsParas[index].Template;

            BaseParas = globalInfo.StruCalsParas[index].BaseParas;



        }

        public string GetTowerType()
        {
            return Template.TowerType;
        }

        public void Save()
        {
            var sss = Results;
        }

        public void UpDateView(string para1, string para2 = "")
        {
            throw new NotImplementedException();
        }

        public void DelSubItem(string itemName)
        {
            throw new NotImplementedException();
        }
    }
}
