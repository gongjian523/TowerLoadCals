using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using TowerLoadCals.Common;
using TowerLoadCals.DAL;
using TowerLoadCals.Mode;
using TowerLoadCals.ModulesViewModels;

namespace TowerLoadCals.Modules
{
    public class StruCalsResultViewModel : ViewModelBase, IStruCalsBaseViewModel, INotifyPropertyChanged
    {
        //protected ObservableCollection<StruCalsResult> _results = new ObservableCollection<StruCalsResult>();
        //public ObservableCollection<StruCalsResult> Results
        //{
        //    get
        //    {
        //        return _results;
        //    }
        //    set
        //    {
        //        _results = value;
        //        RaisePropertyChanged("Results");
        //    }
        //}

        public List<StruCalsResult> Results { get; set; }
        public ObservableCollection<Band> Bands { get; set; }

        public List<int> Points { get; set; }

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

            var hpSettingsParas = globalInfo.StruCalsParas[index].HPSettingsParas;

            List<StruCalsPointLoad> pointLoads = globalInfo.StruCalsParas[index].ResultPointLoad;
            List<int> points = pointLoads.Select(p => p.Name).Distinct().ToList();

            points.Sort();

            Points = points;

            Results = new List<StruCalsResult>();

            foreach (var point in points)
            {
                for (int j = 0; j < Template.WorkConditionCombos.Count; j++)
                {
                    StruCalsResult resultItem = new StruCalsResult()
                    {
                        Index = j + 1,
                        PointNum = point,
                        WorkCondition = Template.WorkConditionCombos[j].WorkComment,
                    };

                    for (int k = 0; k < hpSettingsParas.Count; k++)
                    {
                        resultItem.Fx[k] = pointLoads.Where(p => p.Name == point && p.WorkConditionId == j && p.Orientation == "X" 
                            && p.HPSettingName == hpSettingsParas[k].HangingPointSettingName).Sum(p => p.Load);
                        resultItem.Fy[k] = pointLoads.Where(p => p.Name == point && p.WorkConditionId == j && p.Orientation == "Y"
                            && p.HPSettingName == hpSettingsParas[k].HangingPointSettingName).Sum(p => p.Load);
                        resultItem.Fz[k] = pointLoads.Where(p => p.Name == point && p.WorkConditionId == j && p.Orientation == "Z"
                            && p.HPSettingName == hpSettingsParas[k].HangingPointSettingName).Sum(p => p.Load);
                    }

                    Results.Add(resultItem);
                }
            }



            Bands = new ObservableCollection<Band>() {
                new Band() {
                    Header = " ",
                    ChildColumns = new ObservableCollection<Column>() {
                        new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "PointNum", Header = "挂点" },
                        new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "Index", Header = "序号" },
                        new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "WorkCondition", Header = "工况" },
                    }
                }
            };

            for(int i = 0; i < hpSettingsParas.Count;  i++)
            {
                Bands.Add(new Band()
                {
                    Header = hpSettingsParas[i].HangingPointSettingName,
                    ChildColumns = new ObservableCollection<Column>() {
                        new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "Fx[" + (i+1).ToString() + "]", Header = "Fx" },
                        new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "Fy[" + (i+1).ToString() + "]", Header = "Fy" },
                        new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "Fz[" + (i+1).ToString() + "]", Header = "Fz" },
                    }
                });
            }
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
