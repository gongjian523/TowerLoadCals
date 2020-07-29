using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TowerLoadCals.BLL;
using TowerLoadCals.Converter;
using TowerLoadCals.Mode;

namespace TowerLoadCals.Common.ViewModels
{
    public class SmartTowerModeEditViewModel: ViewModelBase
    {
        protected List<String> _modes = new List<string>() { ConstVar.SmartTowerMode1Str, ConstVar.SmartTowerMode2Str, ConstVar.SmartTowerMode3Str };
        public List<String> Modes
        {
            get
            {
                return _modes;
            }
        }

        //protected List<int> _modes = new List<int>() { 0, 1, 2 };
        //public List<int> Modes
        //{
        //    get
        //    {
        //        return _modes;
        //    }
        //}

        protected string _mode;
        public string Mode
        {
            get
            {
                return _mode;
            }
            set
            {
                _mode = value;
                RaisePropertyChanged("Mode");
            }
        }

        protected string oldMode;

        public SmartTowerModeEditViewModel()
        {
            Mode = SmartTowerModeStringConvert(GlobalInfo.GetInstance().GetSmartTowerMode());
            oldMode = Mode;
        }

        public delegate void SmartTowerModeEditCloseHandler(object sender, string e);
        public event SmartTowerModeEditCloseHandler SmartTowerModeEditCloseEvent;

        public void onConfirm()
        {
            if (Mode != oldMode)
            {
                var mod = SmartTowerModeStringConvertBack(Mode);

                ProjectUtils.GetInstance().SaveSmartTowerMode(mod);
                GlobalInfo.GetInstance().SmartTowerMode = mod;
            }

            close("");
        }

        public void onConcel()
        {
            close("");
        }

        protected void close(string e)
        {
            if (SmartTowerModeEditCloseEvent != null)
                SmartTowerModeEditCloseEvent(this, null);
        }


        protected string SmartTowerModeStringConvert(int mode)
        {
            if (mode == 0)
                return ConstVar.SmartTowerMode1Str;
            else if (mode == 1)
                return ConstVar.SmartTowerMode2Str;
            else
                return ConstVar.SmartTowerMode3Str;
        }

        protected int SmartTowerModeStringConvertBack(string modeStr)
        {
            if (modeStr == ConstVar.SmartTowerMode1Str)
                return 0;
            else if (modeStr == ConstVar.SmartTowerMode2Str)
                return 1;
            else
                return 2;
        }
    }
}
