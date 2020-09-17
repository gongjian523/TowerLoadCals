using DevExpress.Xpf.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using TowerLoadCals.Mode.Internet;
using TowerLoadCals.Modules.Login;
using TowerLoadCals.Service.Login;



/// <summary>
/// created by : glj
/// </summary>


namespace TowerLoadCals
{
    /// <summary>
    /// LoginPage.xaml 的交互逻辑
    /// </summary>
    public partial class LoginPage : Window
    {
        Thread multi;

        BackgroundWorker bgWork = new BackgroundWorker();//后台线程

        public LoginPage()
        {
            InitializeComponent();
            AutoLogin();//是否已经保存cookie 自动登录到主界面   
            pb_import.Maximum = 100;
            pb_import.Value = 0;

            bgWork.WorkerReportsProgress = true;//报告进度更新
            bgWork.WorkerSupportsCancellation = true;//支持异步取消

            bgWork.DoWork += DoWork_Handler;
            bgWork.ProgressChanged += ProgressChanged_Handler;
            bgWork.RunWorkerCompleted += RunWorkCompleted_Handler;

        }
        void ProgressChanged_Handler(object s, ProgressChangedEventArgs e)
        {
            pb_import.Value = e.ProgressPercentage;
        }

        public void DoWork_Handler(object s, DoWorkEventArgs e)
        {
            BackgroundWorker worker = s as BackgroundWorker;
            for (int i = 0; i <= 10; i++)
            {
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    worker.ReportProgress(i * 10);
                    Thread.Sleep(500);
                }
            }
        }

        void RunWorkCompleted_Handler(object s, RunWorkerCompletedEventArgs e)
        {
            pb_import.Value = 0;
            if (e.Cancelled)
                MessageBox.Show("进度被取消了!");
            else
                MessageBox.Show("进度完成鸟!");
        }

        /// <summary>
        /// 判断是否自动登录
        /// </summary>
        public void AutoLogin()
        {
            txtUsername.Text = LoginHelpers.GetSettingString("userName");
            txtPassword.Password = LoginHelpers.GetSettingString("password");

            if (!string.IsNullOrEmpty(txtUsername.Text) && LoginHelpers.GetSettingString("isRemember") == "true")
            {
                ckbRemember.IsChecked = true;

                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            else
            {
                ckbRemember.IsChecked = false;
            }
        }
        private void btn_login_Click(object sender, RoutedEventArgs e)
        {


            //判断用户名不能为空
            if (string.IsNullOrEmpty(txtUsername.Text))
            {
                MessageBox.Show("用户名不能为空！");
                return;
            }
            //判断密码不能为空
            if (string.IsNullOrEmpty(txtPassword.Password))
            {
                MessageBox.Show("密码不能为空！");
                return;
            }
            beginImport();//进度条显示

            UserInfo info = new UserInfo();
            info.UserName = txtUsername.Text;//用户名
            info.Password = txtPassword.Password;//密码
            bool IsSaveLoginInfo = ckbRemember.IsChecked == true;//是否保存登陆账号密码
            //验证用户名和密码
            bool result = new LoginService().doLogin(info);
            if (result)
            {
                LoginHelpers.UpdateSettingString("nickName", info.NickName);
                //判断是否记住用户信息
                if (Convert.ToBoolean(ckbRemember.IsChecked))
                {
                    LoginHelpers.UpdateSettingString("userName", info.UserName);
                    LoginHelpers.UpdateSettingString("password", info.Password);
                    LoginHelpers.UpdateSettingString("isRemember", "true");
                }
                else
                {
                    LoginHelpers.UpdateSettingString("userName", "");
                    LoginHelpers.UpdateSettingString("password", "");
                    LoginHelpers.UpdateSettingString("isRemember", "");
                }
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("用户名或密码错误，请重新输入！");
                pb_import.Visibility = Visibility.Hidden;
                return;
            }
        }
        private void btn_exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            Environment.Exit(0);
        }

        private void beginImport()
        {
            pb_import.Visibility = Visibility.Visible;
            for (int i = 0; i <= 1000; i++)
            {
                double value = i * 10.0 / 1000;
                pb_import.Dispatcher.Invoke(new Action<System.Windows.DependencyProperty, object>(pb_import.SetValue), System.Windows.Threading.DispatcherPriority.Background, ProgressBar.ValueProperty, value);
            }

        }
    }
}
