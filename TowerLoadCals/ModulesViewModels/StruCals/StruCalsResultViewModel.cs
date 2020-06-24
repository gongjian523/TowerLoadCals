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

        protected ObservableCollection<Band> _bands = new ObservableCollection<Band>();
        public ObservableCollection<Band> Bands
        {
            get
            {
                return _bands;
            }
            set
            {
                _bands = value;
                RaisePropertyChanged("Bands");
            }
        }

        protected ObservableCollection<AccordionItem> _points = new ObservableCollection<AccordionItem>();
        public ObservableCollection<AccordionItem> Points
        {
            get
            {
                return _points;
            }
            set
            {
                _points = value;
                RaisePropertyChanged("Points");
            }
        }


        public TowerTemplate Template { get; set; }

        public FormulaParas BaseParas { get; set; }

        protected List<StruCalsResult> pointlist = new List<StruCalsResult>();

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

            foreach (var point in points)
            {
                for (int j = 0; j < Template.WorkConditionCombos.Count; j++)
                {
                    StruCalsResult resultItem = new StruCalsResult()
                    {
                        Index = j + 1,
                        PointNum = point,
                        WorkCondition = Template.WorkConditionCombos[j].WorkComment,
                        Fx = new float[hpSettingsParas.Count],
                        Fy = new float[hpSettingsParas.Count],
                        Fz = new float[hpSettingsParas.Count],
                    };

                    for (int k = 0; k < hpSettingsParas.Count; k++)
                    {
                        resultItem.Fx[k] = pointLoads.Where(p => p.Name == point && p.WorkConditionId == j && p.Orientation == "X" 
                            && p.HPSettingName == hpSettingsParas[k].HangingPointSettingName).Sum(p => p.Load);
                        resultItem.Fx[k] = (float)Math.Round(resultItem.Fx[k], 2);
                        resultItem.Fy[k] = pointLoads.Where(p => p.Name == point && p.WorkConditionId == j && p.Orientation == "Y"
                            && p.HPSettingName == hpSettingsParas[k].HangingPointSettingName).Sum(p => p.Load);
                        resultItem.Fy[k] = (float)Math.Round(resultItem.Fy[k], 2);
                        resultItem.Fz[k] = pointLoads.Where(p => p.Name == point && p.WorkConditionId == j && p.Orientation == "Z"
                            && p.HPSettingName == hpSettingsParas[k].HangingPointSettingName).Sum(p => p.Load);
                        resultItem.Fz[k] = (float)Math.Round(resultItem.Fz[k], 2);
                    }

                    pointlist.Add(resultItem);
                }

                _points.Add(new AccordionItem(point.ToString(), (e) => { SeletedPointChanged(e); }));
            }

            if(Points.Count > 0)
                SeletedPointChanged(Points[0]);

            Bands = new ObservableCollection<Band>() {
                new Band() {
                    Header = " ",
                    ChildColumns = new ObservableCollection<HeaderColumn>() {
                        new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "Index", Header = "序号" },
                        new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "WorkCondition", Header = "工况" },
                    }
                }
            };

            for (int i = 0; i < hpSettingsParas.Count;  i++)
            {
                Bands.Add(new Band()
                {
                    Header = hpSettingsParas[i].HangingPointSettingName,
                    ChildColumns = new ObservableCollection<HeaderColumn>() {
                        new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "Fx[" + i.ToString() + "]", Header = "Fx" },
                        new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "Fy[" + i.ToString() + "]", Header = "Fy" },
                        new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "Fz[" + i.ToString() + "]", Header = "Fz" },
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

        protected void SeletedPointChanged(AccordionItem point)
        {
            Results = new ObservableCollection<StruCalsResult>(pointlist.Where(item=> item.PointNum.ToString() == point.Title));
        }
             
    }
    
}
