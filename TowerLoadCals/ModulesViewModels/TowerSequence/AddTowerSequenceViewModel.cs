using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TowerLoadCals.Common.Utils;
using System.Windows.Input;
using TowerLoadCals.BLL;
using TowerLoadCals.DAL.Common;
using TowerLoadCals.Mode;
using System.IO;
using Microsoft.Win32;



/// <summary>
/// created by : glj
/// </summary>


namespace TowerLoadCals.ModulesViewModels.TowerSequence
{
    public class AddTowerSequenceViewModel : ViewModelBase
    {
        protected ProjectUtils projectUtils;
        public AddTowerSequenceViewModel()
        {
            projectUtils = ProjectUtils.GetInstance();
            //ShowMessageCommand = new DelegateCommand<string>(ShowMessage);
        }
        protected string _fullName;
        public string FullName
        {
            get
            {
                return _fullName;
            }
            set
            {
                _fullName = value;
                RaisePropertyChanged("ConfirmCanExecute");
            }
        }

        protected string _filePath;
        public string FilePath
        {
            get
            {
                return _filePath;
            }
            set
            {
                _filePath = value;
                 RaisePropertyChanged(() => FilePath);
                RaisePropertyChanged("ConfirmCanExecute");
            }
        }

        public bool ConfirmCanExecute
        {
            get
            {
                return (FullName != null && FullName.Trim() != "") && (FilePath != null && FilePath.Trim() != "");
            }
        }

        /// <summary>
        /// 上传文件方法
        /// </summary>
        public void UpLoadFile()
        {
            var openFileDialog = new OpenFileDialog()
            {
                DefaultExt = ".TA",
                Filter = "Text documents (.ta)|*.ta"
            };

            if (openFileDialog.ShowDialog() == true)
                FilePath = openFileDialog.FileName;
        }


        public delegate void CloseWindowHandler(object sender, string strNewTowerName);
        public event CloseWindowHandler CloseWindowEvent;


        public virtual void onConfirm()
        {
            string savePath = "";//保存文件夹
            try
            {
                var list = projectUtils.GetAllTowerSequenceNames();
                if (list.Contains(FullName.Trim()))
                {
                    MessageBox.Show("序列名称已经存在，请修改后重新保存！");
                    return;
                }
                savePath = projectUtils.ProjectPath + "\\" + ConstVar.TowerSequenceStr + "\\" + FullName.Trim();

                //读取并计算DA文件
                List<TowerSerial> seriallist = TowerSerialReader.ReadTa(FilePath);
                //保存源文件数据
                TowerSerialReader.CopySourceFile( FullName, FilePath, savePath);

                //保存计算后的杆塔序列文件
                TowerSerialReader.SaveDT(seriallist, savePath);

                projectUtils.InsertTowerSequenceName(FullName);//新增序列节点

                close(FullName);
            }
            catch (Exception ex)
            {
                if (Directory.Exists(savePath))
                {
                    Directory.Delete(savePath,true);
                }
                MessageBox.Show("保存过程中出错，错误信息如下：" + ex.Message);
            }
        }

        public virtual void onConcel()
        {
            close("");
        }

        protected virtual void close(string TowerSequenceName)
        {
            if (CloseWindowEvent != null)
                CloseWindowEvent(this, TowerSequenceName);
        }

    }
}
