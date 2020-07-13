using DevExpress.Xpf.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
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
using TowerLoadCals.Mode.Internet;
using TowerLoadCals.Modules.Login;
using TowerLoadCals.Service.Login;

namespace TowerLoadCals
{
    /// <summary>
    /// LoginPage.xaml 的交互逻辑
    /// </summary>
    public partial class LoginPage : ThemedWindow
    {
        public LoginPage()
        {
            InitializeComponent();
            AutoLogin();//是否已经保存cookie 自动登录到主界面
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
            }
            //判断密码不能为空
            if (string.IsNullOrEmpty(txtPassword.Password))
            {
                MessageBox.Show("密码不能为空！");
            }

            UserInfo info = new UserInfo();
            info.UserName = txtUsername.Text;//用户名
            info.Password = txtPassword.Password;//密码
            bool IsSaveLoginInfo = ckbRemember.IsChecked == true;//是否保存登陆账号密码
            //验证用户名和密码
            bool result = new LoginService().doLogin(info);
            if (result)
            {
                //判断是否记住用户信息
                if (Convert.ToBoolean(ckbRemember.IsChecked))
                {
                    LoginHelpers.UpdateSettingString("userName", info.UserName);
                    LoginHelpers.UpdateSettingString("nickName", info.NickName);
                    LoginHelpers.UpdateSettingString("password", info.Password);
                    LoginHelpers.UpdateSettingString("isRemember", "true");
                }
                else
                {
                    LoginHelpers.UpdateSettingString("userName", "");
                    LoginHelpers.UpdateSettingString("nickName", "");
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
            }
        }
        private void btn_exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            Environment.Exit(0);
        }


    }
}
