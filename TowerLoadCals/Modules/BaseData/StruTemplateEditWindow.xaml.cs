using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using System;

namespace TowerLoadCals.Modules
{
    /// <summary>
    /// StruTemplateEditWindow.xaml 的交互逻辑
    /// </summary>
    public partial class StruTemplateEditWindow : ThemedWindow
    {
        public StruTemplateEditWindow()
        {
            InitializeComponent();

            //WireGird.CellValueChanged += WireGird_CellValueChanged;
        }

        void ThemedWindow_Closed(object sender, EventArgs e)
        {
            ((StruTemplateEditViewModel)DataContext).onConcel();
            
        }

        void WireGird_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            ((StruTemplateEditViewModel)DataContext).WiresGridChanged(e.Column.Header.ToString());
        }

    }
}
