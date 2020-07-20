using DevExpress.Xpf.Core;
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
        }

        void ThemedWindow_Closed(object sender, EventArgs e)
        {
            ((StruTemplateEditViewModel)DataContext).onConcel();
            
        }


    }
}
