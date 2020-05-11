
using System;
using System.Collections.Generic;
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

using System.Windows.Shapes;
using DevExpress.Xpf.WindowsUI;
using DevExpress.Xpf.WindowsUI.Navigation;

namespace TowerLoadCals.Modules
{
    /// <summary>
    /// BaseDataModule.xaml 的交互逻辑
    /// </summary>
    public partial class BaseDataModule : UserControl
    {
        public BaseDataModule()
        {
            InitializeComponent();
        }

        void OnDocumentFrameNavigating(object sender, NavigatingEventArgs e)
        {
            if (e.Cancel) return;

        }

        void OnDocumentFrameNavigated(object sender, NavigationEventArgs e)
        {

        }
    }
}
