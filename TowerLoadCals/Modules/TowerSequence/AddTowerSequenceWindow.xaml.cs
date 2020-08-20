using DevExpress.Xpf.Core;
using System;
using TowerLoadCals.ModulesViewModels.TowerSequence;

namespace TowerLoadCals.Modules.TowerSequence
{
    /// <summary>
    /// AddTowerSequenceWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AddTowerSequenceWindow : ThemedWindow
    {
        public AddTowerSequenceWindow()
        {
            InitializeComponent();
        }

        void ThemedWindow_Closed(object sender, EventArgs e)
        {
            ((AddTowerSequenceViewModel)DataContext).onConcel();
        }


    }
}
