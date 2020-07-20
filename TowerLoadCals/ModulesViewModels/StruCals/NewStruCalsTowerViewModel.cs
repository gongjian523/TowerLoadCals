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

namespace TowerLoadCals.Modules
{
    public class NewStruCalsTowerViewModel : ViewModelBase
    {
        protected string _towerName;
        public string TowerName
        {
            get
            {
                return _towerName;
            }
            set
            {
                _towerName = value;
                RaisePropertyChanged("ConfirmCanExecute");
            }
        }

        protected string _templatePath;
        public string TemplatePath
        {
            get
            {
                return _templatePath;
            }
            set
            {
                _templatePath = value;
                RaisePropertyChanged("TemplatePath");
                RaisePropertyChanged("ConfirmCanExecute");
            }
        }

        protected string _electricalLoadFilePath;
        public string ElectricalLoadFilePath
        {
            get
            {
                return _electricalLoadFilePath;
            }
            set
            {
                _electricalLoadFilePath = value;
                RaisePropertyChanged("ElectricalLoadFilePath");
                RaisePropertyChanged("ConfirmCanExecute");
            }
        }

        protected string _towerType;
        public string TowerType
         {
            get
            {
                return _towerType;
            }
            set
            {
                _towerType = value;
                RaisePropertyChanged("ConfirmCanExecute");
            }
        }

        protected List<String> _towerTypes = new List<string>() { "直线塔", "直转塔", "转角塔",  "分支塔", "终端塔"};
        public List<String> TowerTypes
        {
            get
            {
                return _towerTypes;
            }
            set
            {
                _towerTypes = value;
            }
        }

        public bool ConfirmCanExecute
        {
            get
            {
                return (TowerName != null && TowerName.Trim() != "") && (TemplatePath != null && TemplatePath.Trim() != "")
                    && (TowerType != null && TowerType.Trim() != "") && (ElectricalLoadFilePath != null && ElectricalLoadFilePath.Trim() != "");
            }
        }

        //public ICommand ShowMessageCommand { get; private set; }

        public NewStruCalsTowerViewModel()
        {
            //ShowMessageCommand = new DelegateCommand<string>(ShowMessage);
        }
        
        public void ChooseWorkConditionTemplate()
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "DLL Files (*.dll)|*.dll"
            };

            if (openFileDialog.ShowDialog() != true)
                return;

            TemplatePath = openFileDialog.FileName;
        }

        public void ChooseElectricalLoadFile()
        {
            var openTemplateDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "Excel Files (*.xlsx)|*.xlsx"
            };

            if (openTemplateDialog.ShowDialog() != true)
                return;

            ElectricalLoadFilePath = openTemplateDialog.FileName;
        }

        //public event EventHandler Closed;

        public delegate void NewStruCalsTowerHandler(object sender, string strNewTowerName);
        public event NewStruCalsTowerHandler NewStruCalsTowerEvent;

        public void onConfirm()
        {
            if(ProjectUtils.NewStruCalsTower(TowerName, TowerType, TemplatePath, ElectricalLoadFilePath))
            {
                close(TowerName);
            }
        }

        public void onConcel()
        {
            close("");
        }

        protected void close(string strNewTowerName)
        {
            if (NewStruCalsTowerEvent != null)
                NewStruCalsTowerEvent(this, strNewTowerName);
        }

    }
}
