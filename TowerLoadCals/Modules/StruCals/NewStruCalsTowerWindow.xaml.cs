using DevExpress.Xpf.Core;
using System;

namespace TowerLoadCals.Modules
{
    /// <summary>
    /// NewStruCalsTowerWindow.xaml 的交互逻辑
    /// </summary>
    public partial class NewStruCalsTowerWindow : ThemedWindow
    {
        public NewStruCalsTowerWindow()
        {
            InitializeComponent();
        }

        void ThemedWindow_Closed(object sender, EventArgs e)
        {
            ((NewStruCalsTowerViewModel)DataContext).onConcel();
        }
    }
}
