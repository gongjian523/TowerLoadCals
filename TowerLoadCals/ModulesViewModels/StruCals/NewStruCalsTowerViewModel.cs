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

        protected List<string> _fullStressTemplatePaths = new List<string>();
        public string FullStressTemplatePath
        {
            get
            {
                string str = "";
                foreach (string path in _fullStressTemplatePaths)
                    str += path;
                return str;
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
        }

        protected string _voltage;
        public string Voltage
        {
            get
            {
                return _voltage;
            }
            set
            {
                _voltage = value;
                RaisePropertyChanged("Voltage");
                RaisePropertyChanged("ConfirmCanExecute");
            }
        }

        protected List<String> _voltages = new List<string>() { "110KV", "220KV", "330KV", "500KV", "750KV", "800KV", "1000KV", "1100KV" };
        public List<String> Voltages
        {
            get
            {
                return _voltages;
            }
        }

        public bool ConfirmCanExecute
        {
            get
            {
                return (TowerName != null && TowerName.Trim() != "") && (TemplatePath != null && TemplatePath.Trim() != "")
                    && (TowerType != null && TowerType.Trim() != "") && (ElectricalLoadFilePath != null && ElectricalLoadFilePath.Trim() != "")
                    && (Voltage != null && Voltage.Trim() != "");
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

        public void ChooseFullStressTemplate()
        {
            var openTemplateDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "Template Files (*.dat)|*.dat",
                Multiselect = true,
            };

            if (openTemplateDialog.ShowDialog() != true)
                return;

            _fullStressTemplatePaths = openTemplateDialog.FileNames.ToList();
            RaisePropertyChanged("FullStressTemplatePath");
        }

        public delegate void CloseStruCalsTowerDetailWindowHandler(object sender, string strNewTowerName);
        public event CloseStruCalsTowerDetailWindowHandler CloseStruCalsTowerDetailWindowEvent;

        public virtual void onConfirm()
        {
            float vol  = (float)Convert.ToDecimal(Voltage.Substring(0, Voltage.Length-2));
            if(ProjectUtils.NewStruCalsTower(TowerName, TowerType, vol, TemplatePath, ElectricalLoadFilePath, _fullStressTemplatePaths))
            {
                close(TowerName);
            }
        }

        public virtual void onConcel()
        {
            close("");
        }

        protected  virtual void close(string strNewTowerName)
        {
            if (CloseStruCalsTowerDetailWindowEvent != null)
                CloseStruCalsTowerDetailWindowEvent(this, strNewTowerName);
        }

    }
}
