using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using System;
using System.Windows;
using TowerLoadCals.ModulesViewModels.TowerSequence;

namespace TowerLoadCals.Modules.TowerSequence
{
    /// <summary>
    /// TowerParaSettingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TowerParaSettingWindow : ThemedWindow
    {
        public TowerParaSettingWindow()
        {
            InitializeComponent();
        }

        void ThemedWindow_Closed(object sender, EventArgs e)
        {
            ((TowerParaSettingViewModel)DataContext).onConcel();
        }

    }
}
