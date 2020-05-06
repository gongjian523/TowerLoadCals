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
    public class WeatherConditionViewModel: ViewModelBase, IBaseViewModel,INotifyPropertyChanged
    {
        public List<Weather> Weathers { get; set; }

        private ObservableCollection<WorkCondition> _SelectedWeatherCondition = new ObservableCollection<WorkCondition>();
        public ObservableCollection<WorkCondition> SelectedWeatherCondition
        {
            get
            {
                return _SelectedWeatherCondition;
            }

            private set
            {
                _SelectedWeatherCondition = value;
                RaisePropertyChanged("SelectedWeatherCondition");
            }
        }

        public ObservableCollection<WeatherCollection> WeatherCollections { get; set; }

        public DelegateCommand AddItemCommand { get; private set; }

        public DelegateCommand<object> SetSelectedItemCommand { get; private set; }

        private GlobalInfo globalInfo;

        protected string filePath;

        protected WeatherXmlReader _weatherXmlReader = new WeatherXmlReader();

        public WeatherConditionViewModel()
        {
            globalInfo = GlobalInfo.GetInstance();
            filePath = globalInfo.ProjectPath + "\\BaseData\\WeatherCondition.xml";

            AddItemCommand = new DelegateCommand(AddItem);
            SetSelectedItemCommand = new DelegateCommand<object>(SelectedItemChanged);

            //Weathers = _weatherXmlReader.ReadLocal(filePath);
            //Weathers = _weatherXmlReader.ReadLocal("D:\\00-项目\\P-200325-杆塔负荷程序\\数据资源示例\\test-weather.xml");
            Weathers = _weatherXmlReader.ReadLocal("D:\\智菲\\P-200325-杆塔负荷程序\\数据资源示例\\3.xml");

            WeatherCollections = new ObservableCollection<WeatherCollection>();
            WeatherCollections.Add(new WeatherCollection
            {
                Name = "气象条件",
                Weathers = Weathers
            });

            if (Weathers.Count == 0)
            {
                SelectedWeatherCondition = new ObservableCollection<WorkCondition>();
            }
            else
            {
                SelectedWeatherCondition = new ObservableCollection<WorkCondition>(Weathers[0].WorkConditions);
            }
        }

        protected void SelectedItemChanged(object para)
        {
            if (para.GetType().Name != "Weather")
                return;

            if (Weathers.Where(item => item.Name == ((Weather)para).Name).ToList().Count == 0)
                return;

            Weather selectedWd = Weathers.Where(item => item.Name == ((Weather)para).Name).First();
            SelectedWeatherCondition = new ObservableCollection<WorkCondition>(selectedWd.WorkConditions);
        }

        public void Save()
        {
            ;
        }

        public void AddItem()
        {
            ;
        }
    }
}
