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

    //[POCOViewModel]
    public class WeatherConditionViewModel: DaseDataBaseViewModel<WorkCondition, List<Weather>>
    {
        //public List<Weather> Weathers { get; set; }

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
            //Weathers = _weatherXmlReader.ReadLocal(filePath);
            //Weathers = _weatherXmlReader.ReadLocal("D:\\00-项目\\P-200325-杆塔负荷程序\\数据资源示例\\test-weather.xml");
            BaseData = _weatherXmlReader.ReadLocal("D:\\智菲\\P-200325-杆塔负荷程序\\数据资源示例\\3.xml");


            //WeatherCollections = new ObservableCollection<WeatherCollection>();
            //WeatherCollections.Add(new WeatherCollection
            //{
            //    Name = "气象条件",
            //    Weathers = Weathers
            //});

            //if (Weathers.Count == 0)
            //{
            //    curName = "";
            //    SelectedItems = new ObservableCollection<WorkCondition>();
            //}
            //else
            //{
            //    curName = Weathers[0].Name;
            //    SelectedItems = new ObservableCollection<WorkCondition>(Weathers[0].WorkConditions);
            //}
        }

        protected override void SelectedItemChanged(object para)
        {
            if (para.GetType().Name != "Weather")
                return;

            if (BaseData.Where(item => item.Name == ((Weather)para).Name).ToList().Count == 0)
                return;
            
            UpdateLastSelectedWeather();

            curName = ((Weather)para).Name;
            Weather selectedWd = BaseData.Where(item => item.Name == ((Weather)para).Name).First();
            SelectedItems = new ObservableCollection<WorkCondition>(selectedWd.WorkConditions);
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
    }
}
