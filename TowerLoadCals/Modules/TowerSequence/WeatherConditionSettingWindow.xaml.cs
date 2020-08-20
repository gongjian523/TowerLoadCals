using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using System;
using System.Windows;
using TowerLoadCals.ModulesViewModels.TowerSequence;

namespace TowerLoadCals.Modules.TowerSequence
{
    /// <summary>
    /// AddTowerSequenceWindow.xaml 的交互逻辑
    /// </summary>
    public partial class WeatherConditionSettingWindow : ThemedWindow
    {
        public WeatherConditionSettingWindow()
        {
            InitializeComponent();
        }

        void ThemedWindow_Closed(object sender, EventArgs e)
        {
            ((WeatherConditionSettingWindowViewModel)DataContext).onConcel();
        }

    }
}
