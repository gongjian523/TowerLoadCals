using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using System;
using System.Windows;
using TowerLoadCals.ModulesViewModels.TowerSequence;

namespace TowerLoadCals.Modules.TowerSequence
{
    /// <summary>
    /// CommSideParaSettingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CommSideParaSettingWindow : ThemedWindow
    {
        public CommSideParaSettingWindow()
        {
            InitializeComponent();
        }

        void ThemedWindow_Closed(object sender, EventArgs e)
        {
            ((CommSideParaSettingViewModel)DataContext).onConcel();
        }

    }
}
