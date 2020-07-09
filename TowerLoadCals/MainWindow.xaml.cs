using System;
using System.Windows;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Ribbon;
using DevExpress.Xpf.WindowsUI;
using DevExpress.Xpf.WindowsUI.Navigation;
using System.Windows.Controls;
using TowerLoadCals.Modules.Login;

namespace TowerLoadCals
{
    public partial class MainWindow : ThemedWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            if (Height > SystemParameters.VirtualScreenHeight || Width > SystemParameters.VirtualScreenWidth)
                WindowState = WindowState.Maximized;
            DevExpress.Utils.About.UAlgo.Default.DoEventObject(DevExpress.Utils.About.UAlgo.kDemo, DevExpress.Utils.About.UAlgo.pWPF, this);

            this.txt_UserName.Content = "欢迎您："+ LoginHelpers.GetSettingString("userName");
         
        }

        void OnDocumentFrameNavigating(object sender, NavigatingEventArgs e)
        {
            if (e.Cancel) return;
            NavigationFrame frame = (NavigationFrame)sender;
            FrameworkElement oldContent = (FrameworkElement)frame.Content;
            if (oldContent != null)
            {
                RibbonMergingHelper.SetMergeWith(oldContent, null);
                RibbonMergingHelper.SetMergeStatusBarWith(oldContent, null);
            }
        }

        void OnDocumentFrameNavigated(object sender, NavigationEventArgs e)
        {
            FrameworkElement newContent = (FrameworkElement)e.Content;
            if (newContent != null)
            {
                RibbonMergingHelper.SetMergeWith(newContent, RibbonControl);
                RibbonMergingHelper.SetMergeStatusBarWith(newContent, statusBar);
            }
        }

        /// <summary>
        /// 退出系统 事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_exit_Click(object sender, RoutedEventArgs e)
        {
            LoginHelpers.UpdateSettingString("userName", "");
            LoginHelpers.UpdateSettingString("password", "");
            LoginHelpers.UpdateSettingString("isRemember", "");

            this.Close();
            LoginPage loginPage = new LoginPage();
            loginPage.Show();
        }
    }
}
