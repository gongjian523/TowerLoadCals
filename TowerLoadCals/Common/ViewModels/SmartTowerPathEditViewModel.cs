using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TowerLoadCals.BLL;
using TowerLoadCals.Mode;

namespace TowerLoadCals.Common.ViewModels
{
    public class SmartTowerPathEditViewModel: ViewModelBase
    {
        protected string _path = "";
        public string Path
        {
            get
            {
                return _path;
            }
            set
            {
                _path = value;
                RaisePropertyChanged("Path");
                RaisePropertyChanged("ConfirmCanExecute");
            }
        }

        public bool ConfirmCanExecute
        {
            get
            {
                return Path.EndsWith(ConstVar.SmartTowerQTName);
            }
        }

        protected string oldPath;

        public SmartTowerPathEditViewModel()
        {
            Path = GlobalInfo.GetInstance().GetSmartTowerPath();
            oldPath = Path;
        }

        public delegate void SmartTowerPathEditCloseHandler(object sender, string e);
        public event SmartTowerPathEditCloseHandler SmartTowerPathEditCloseEvent;


        public void ChooseSmartTowerConsole()
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "SmartTower (*.exe)|*.exe"
            };

            if (openFileDialog.ShowDialog() != true)
                return;

            Path = openFileDialog.FileName;
        }

        public void onConfirm()
        {
            if (Path.Contains(" "))
            {
                MessageBox.Show("路径中不能包含空格\" \"，请重置SmartTower的路径！");
                return;
            }

            string dirPath = Path.Substring(0, Path.LastIndexOf("\\"));
            string scePath = Directory.GetCurrentDirectory() + "\\" + ConstVar.SmartTowerConsoleName;
            string destPath = dirPath + ConstVar.SmartTowerConsoleName;

            if (Path != oldPath)
            {
                ProjectUtils.GetInstance().SaveSmartTowerPath(Path);
                GlobalInfo.GetInstance().SmartTowerPath = Path;

                //无论在指定目录下是否有后台程序
                //将后台程序复制到SmartTower的路径下
                if (File.Exists(destPath))
                    File.Delete(destPath);
                File.Copy(scePath, destPath);
            }
            else
            {
                //路径没有改变，在指定路径下存在后台程序
                //将不会复制程序
                if (!File.Exists(destPath))
                    File.Copy(scePath, destPath);
            }

            close("");
        }

        public void onConcel()
        {
            close("");
        }

        protected void close(string e)
        {
            if (SmartTowerPathEditCloseEvent != null)
                SmartTowerPathEditCloseEvent(this, null);
        }
    }
}
