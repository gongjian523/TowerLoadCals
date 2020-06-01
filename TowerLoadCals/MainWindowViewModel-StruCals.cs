using DevExpress.Mvvm;
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

            menuItem.Add(NewTowrSubMenuItem("直线塔"));
            menuItem.Add(NewTowrSubMenuItem("直转塔"));

            baseDataMudule.MenuItems = menuItem;

            return baseDataMudule;
        }

        private MenuItemVM NewTowrSubMenuItem(string towerName)
        {
            var subMenus = new List<MenuItemVM>() { };
            var weatherMenu = new MenuItemVM("BaseAndLineParasModule", this, "结构计算参数", (e) => { OnSelectedStruCalsSubModuleChanged(e); });
            subMenus.Add(weatherMenu);

            var towerMenu = new MenuItemVM("WorkConditionComboModule", this, "工况组合", (e) => { OnSelectedStruCalsSubModuleChanged(e); });
            subMenus.Add(towerMenu);

            var hungingPointMenu = new MenuItemVM("HangingPointModule", this, "挂点设置", (e) => { OnSelectedStruCalsSubModuleChanged(e); });
            subMenus.Add(hungingPointMenu);

            MenuItemVM menu = new MenuItemVM("", this, towerName, (e) => { OnSelectedStruCalsTowersChanged(e); }, Visibility.Visible, Visibility.Collapsed, Visibility.Collapsed, Visibility.Collapsed, subMenus);
            menu.CalsBtnVisible = Visibility.Visible;
            menu.LoadBtnVisible = Visibility.Visible;

            return menu;
        }

        private void OnSelectedStruCalsSubModuleChanged(MenuItemVM vm)
        {
            object mv = new object();
            if(vm.Type == "BaseAndLineParasModule")
            {
                mv = new BaseAndLineParasViewModel(vm.Title);
            }
            

            if (curSubModule != vm.Title)
            {
                vm.Show(mv);
                curSubModule = vm.Title;
            }
            else
            {
                subVm.Show();
                curSubModule = subVm.Title;

                return true;
            }
        }

        private void OnSelectedStruCalsTowersChanged(MenuItemVM vm)
        {
            
        }
    }
}
