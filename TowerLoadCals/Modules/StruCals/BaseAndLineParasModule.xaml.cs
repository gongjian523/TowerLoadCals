using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Grid;

namespace TowerLoadCals.Modules
{
    /// <summary>
    /// BaseAndLineParasModule.xaml 的交互逻辑
    /// </summary>
    public partial class BaseAndLineParasModule 
    {
        protected TableView SeletedTableView;

        public BaseAndLineParasModule()
        {
            InitializeComponent();

            LineTowerGrid.PreviewMouseDown += new MouseButtonEventHandler(lineTowerGrid_PreviewMouseDown);
            TensionTowerGrid.PreviewMouseDown += new MouseButtonEventHandler(tensionTowerGrid_PreviewMouseDown);
        }

        void lineTowerGrid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            SeletedTableView = LineTowerView;
        }
        void tensionTowerGrid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            SeletedTableView = TensionTowerView;
        }

        private void copyCellData_ItemClick(object sender, ItemClickEventArgs e)
        {
            SeletedTableView.CopySelectedCellsToClipboard();
        }

        private void pasteCellData_ItemClick(object sender, ItemClickEventArgs e)
        {
            SeletedTableView.OnPaste();
        }
    }
}
