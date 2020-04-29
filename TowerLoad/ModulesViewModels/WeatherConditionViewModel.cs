using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TowerLoadCals.DAL;
using TowerLoadCals.Mode;

namespace TowerLoadCals.Modules
{

                         
    public class WeatherConditionViewModel
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
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedWeatherCondition"));
            }
        }

        public ObservableCollection<BaseDataNameTreeItem> NameTree { get; set; }

        protected WeatherXmlReader _weatherXmlReader;


        public ICommand<MouseEventArgs> SelectedCommand { get; private set; }

        public WeatherConditionViewModel()
        {
            
            SelectedCommand = new DelegateCommand<MouseEventArgs>(UpdateWeatherCondition);

            _weatherXmlReader = new WeatherXmlReader();

            Weathers = _weatherXmlReader.ReadLocal("D:\\00-项目\\P-200325-杆塔负荷程序\\数据资源示例\\test-weather.xml");

            if(Weathers.Count == 0)
            {
                SelectedWeatherCondition = new ObservableCollection<WorkCondition>();
            }
            else
            {
                SelectedWeatherCondition = new ObservableCollection<WorkCondition>(Weathers[0].WorkConditions);
            }


            int id = 0;
            NameTree = new ObservableCollection<BaseDataNameTreeItem>();
            NameTree.Add(new BaseDataNameTreeItem
            {
                ID = (++id),
                ParentID = 0,
                Name = "气象条件"
            });

            foreach(var item in Weathers)
            {
                NameTree.Add(new BaseDataNameTreeItem
                {
                    ID = (++id),
                    ParentID = 1,
                    Name = item.Name
                });
            }

        }

        int i = 0;

        public void UpdateWeatherCondition(MouseEventArgs arg)
        {
            if (i == 2)
                i = 0;
            else
                i++;

            SelectedWeatherCondition = new ObservableCollection<WorkCondition>(Weathers[i].WorkConditions);
        }


        public event PropertyChangedEventHandler PropertyChanged;
    }
}
