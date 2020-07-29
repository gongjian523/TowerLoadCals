using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Grid;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using TowerLoadCals.Common;
using TowerLoadCals.DAL;
using TowerLoadCals.Mode;
using TowerLoadCals.ModulesViewModels;
using TextBox = System.Windows.Controls.TextBox;

namespace TowerLoadCals.Modules
{
    public class WeatherConditionViewModel: DaseDataBaseViewModel<WorkCondition, List<Weather>>
    {
        public ObservableCollection<WeatherCollection> WeatherCollections { get; set; }

        public DelegateCommand AddItemCommand { get; private set; }

        protected WeatherXmlReader _weatherXmlReader = new WeatherXmlReader();

        protected string curName;

        protected override void InitializeItemsSource()
        {
            base.InitializeItemsSource();

            filePath = globalInfo.ProjectPath + "\\BaseData\\WeatherCondition.xml";

            AddItemCommand = new DelegateCommand(AddItem);
        }

        protected override void InitializeData()
        {
            BaseData = _weatherXmlReader.ReadLocal(filePath);

            UpdateCurrentWeatherCondition(BaseData.Count == 0 ? "" : BaseData[0].Name);
        }

        public override void Save()
        {
            UpdateLastSelectedWeather();

            _weatherXmlReader.Save(filePath, BaseData);
        }


        public void AddItem()
        {
            ;
        }


        protected void UpdateLastSelectedWeather()
        {
            int index= BaseData.FindIndex(item => item.Name == curName);

            //这种情况只能是curName为空的情况
            if (index == -1)
                return;

            BaseData[index].WorkConditions = SelectedItems.ToList();
        }


        protected void UpdateCurrentWeatherCondition(string name)
        {
            curName = name;

            if (BaseData.Where(item => item.Name == curName).Count() == 0)
            {
                SelectedItems.Clear(); 
            }
            else
            {
                SelectedItems = new ObservableCollection<WorkCondition>(BaseData.Where(item => item.Name == curName).First().WorkConditions);
            }
        }

        public override void UpDateView(string para1, string para2 = "")
        {
            UpdateCurrentWeatherCondition(para1);
        }

        public override void DelSubItem(string itemName)
        {
            BaseData.Remove(BaseData.Where(item  => item.Name == itemName).First());

            UpdateCurrentWeatherCondition(BaseData.Count == 0 ? "" : BaseData.First().Name);
        }
    }
}
