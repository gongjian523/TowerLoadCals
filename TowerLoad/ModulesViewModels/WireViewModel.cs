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

    public class WireViewModel: ViewModelBase, IBaseViewModel
    {
        public List<WireType> WireTypes { get; set; }

        public ObservableCollection<Wire> SelectedWire{ get; set; }

        public DelegateCommand<object> SetSelectedItemCommand { get; private set; }

        private GlobalInfo globalInfo;

        protected string filePath;

        protected WeatherXmlReader _weatherXmlReader = new WeatherXmlReader();

        public WireViewModel()
        {
            globalInfo = GlobalInfo.GetInstance();

            //WireTypes = WireReader.Read(filePath);
            WireTypes = WireReader.Read("D:\\智菲\\P-200325-杆塔负荷程序\\数据资源示例\\3.xml");


            SetSelectedItemCommand = new DelegateCommand<object>(SelectedItemChanged);

        }

        protected void SelectedItemChanged(object para)
        {
            if (((TreeViewItem)para).Header.ToString() == "导底线")
                return;

        }


        public void Save()
        {
            
        }

    }
}
