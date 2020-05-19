using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TowerLoadCals.Mode;
using TowerLoadCals.Modules;
using TowerLoadCals.ModulesViewModels;

namespace TowerLoadCals
{
    public partial class MainWindowViewModel
    {
        private ModuleInfo IniStruCalsModule()
        {
            ModuleInfo baseDataMudule = new ModuleInfo("StruCalsModule", this, "结构计算");
            baseDataMudule.SetIcon("FolderList_32x32.png");

            var menuItem = new List<MenuItemVM>() { };

            var weatherMenu = new MenuItemVM("BaseAndLineParasModule", this, "结构计算参数", (e) => { OnSelectedStruCalsSubModuleChanged(e); }, Visibility.Visible, Visibility.Visible);
            menuItem.Add(weatherMenu);

            var towerMenu = new  MenuItemVM("WorkConditionComboModule", this, "工况组合", (e) => { OnSelectedStruCalsSubModuleChanged(e); }, Visibility.Visible, Visibility.Visible);
            menuItem.Add(towerMenu);


            baseDataMudule.MenuItems = menuItem;

            return baseDataMudule;
        }

        private void OnSelectedStruCalsSubModuleChanged(MenuItemVM vm)
        {
            if (!UpdateSubModule(vm))
                return;
        }


    }
}
