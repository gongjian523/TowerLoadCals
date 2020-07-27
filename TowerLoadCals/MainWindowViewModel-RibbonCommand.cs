using DevExpress.Mvvm.POCO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using TowerLoadCals.Common.Modules;
using TowerLoadCals.Common.ViewModels;
using TowerLoadCals.Mode;
using TowerLoadCals.Mode.Internet;
using TowerLoadCals.Modules;
using TowerLoadCals.ModulesViewModels;
using TowerLoadCals.ModulesViewModels.Internet;

namespace TowerLoadCals
{
    //此文件中处理Ribbon菜单中按钮对应的命令和它们相关的函数
    public partial class MainWindowViewModel
    {

        protected SmartTowerPathEditWindow stPathEditWindow;
        public void ShowSmartTowerPathEditWindow()
        {
            SmartTowerPathEditViewModel model = ViewModelSource.Create(() => new SmartTowerPathEditViewModel());
            model.SmartTowerPathEditCloseEvent += CloseSmartTowerPathEditWindow;
            stPathEditWindow = new SmartTowerPathEditWindow();
            stPathEditWindow.DataContext = model;
            stPathEditWindow.ShowDialog();
        }

        public void CloseSmartTowerPathEditWindow(object sender, string e)
        {
            SmartTowerPathEditViewModel model = (SmartTowerPathEditViewModel)sender;
            model.SmartTowerPathEditCloseEvent -= CloseSmartTowerPathEditWindow;
            if (stPathEditWindow != null) stPathEditWindow.Close();
            stPathEditWindow = null;
        }

        protected SmartTowerModeEditWindow stModeEditWindow;
        public void ShowSmartTowerModeEditWindow()
        {
            SmartTowerModeEditViewModel model = ViewModelSource.Create(() => new SmartTowerModeEditViewModel());
            model.SmartTowerModeEditCloseEvent += CloseSmartTowerModeEditWindow;
            stModeEditWindow = new SmartTowerModeEditWindow();
            stModeEditWindow.DataContext = model;
            stModeEditWindow.ShowDialog();
        }

        public void CloseSmartTowerModeEditWindow(object sender, string e)
        {
            SmartTowerModeEditViewModel model = (SmartTowerModeEditViewModel)sender;
            model.SmartTowerModeEditCloseEvent -= CloseSmartTowerModeEditWindow;
            if (stModeEditWindow != null) stModeEditWindow.Close();
            stModeEditWindow = null;
        }

    }
}
