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
    public class WireViewModel: ViewModelBase, IBaseViewModel
    {
        private GlobalInfo globalInfo;
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

        public ObservableCollection<BaseDataNameTreeItem> NameTree { get; set; }

        protected WeatherXmlReader _weatherXmlReader = new WeatherXmlReader();


        public ICommand<MouseButtonEventArgs> SelectedCommand { get; private set; }


        public WireViewModel()
        {
            globalInfo = GlobalInfo.GetInstance();

            SelectedCommand = new DelegateCommand<MouseButtonEventArgs>(UpdateWeatherCondition);

            Messenger.Default.Register<string>(this, OnMessage);

            Weathers = _weatherXmlReader.ReadLocal("D:\\智菲\\P-200325-杆塔负荷程序\\数据资源示例\\3.xml");
            //Weathers = _weatherXmlReader.ReadLocal("D:\\00-项目\\P-200325-杆塔负荷程序\\数据资源示例\\test-weather.xml");
       
            if (Weathers.Count == 0)
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

        void OnMessage(string message)
        {
            System.Windows.MessageBox.Show("Wire");
        }

        public void UpdateWeatherCondition(MouseButtonEventArgs arg)
        {

            TreeListControl parentElement = (TreeListControl)arg.Source;
            DependencyObject clickedElement = (DependencyObject)arg.OriginalSource;

            TextBox clickedItem =
                LayoutTreeHelper.GetVisualParents(child: clickedElement, stopNode: parentElement)
                .OfType<TextBox>()
                .FirstOrDefault();

            //var clickedItem = LayoutTreeHelper.GetVisualParents(child: clickedElement, stopNode: parentElement);

            if (clickedItem == null)
                return;
            //BaseDataNameTreeItem treeItem = (BaseDataNameTreeItem)clickedItem.DataContext;

            if (Weathers.Where(item => item.Name == clickedItem.Text).ToList().Count == 0)
                return;

            Weather selectedWd = Weathers.Where(item => item.Name == clickedItem.Text).First();

            SelectedWeatherCondition = new ObservableCollection<WorkCondition>(selectedWd.WorkConditions);
        }

        public void Save()
        {
            
        }

    }
}
