using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TowerLoadCals.Common;
using TowerLoadCals.DAL;
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

            var menuItems = new List<MenuItemVM>() { };

            MenuItemVM menu1 = new MenuItemVM("", this, "直线塔", (e) => { OnSelectedStruCalsTowersChanged(e); }, Visibility.Visible);
            menu1.SetIcon("Menu_tower.png");
            menu1.CalsBtnVisible = Visibility.Visible;
            menu1.LoadBtnVisible = Visibility.Visible;
            menuItems.Add(menu1);

            MenuItemVM menu2 = new MenuItemVM("", this, "直转塔", (e) => { OnSelectedStruCalsTowersChanged(e); }, Visibility.Visible);
            menu2.SetIcon("Menu_tower.png");
            menu2.CalsBtnVisible = Visibility.Visible;
            menu2.LoadBtnVisible = Visibility.Visible;
            menuItems.Add(menu2);

            MenuItemVM menu3 = new MenuItemVM("", this, "转角塔", (e) => { OnSelectedStruCalsTowersChanged(e); }, Visibility.Visible);
            menu3.SetIcon("Menu_tower.png");
            menu3.CalsBtnVisible = Visibility.Visible;
            menu3.LoadBtnVisible = Visibility.Visible;
            menuItems.Add(menu3);

            MenuItemVM menu4 = new MenuItemVM("", this, "分支塔", (e) => { OnSelectedStruCalsTowersChanged(e); }, Visibility.Visible);
            menu4.SetIcon("Menu_tower.png");
            menu4.CalsBtnVisible = Visibility.Visible;
            menu4.LoadBtnVisible = Visibility.Visible;
            menuItems.Add(menu4);

            MenuItemVM menu5 = new MenuItemVM("", this, "终端塔", (e) => { OnSelectedStruCalsTowersChanged(e); }, Visibility.Visible);
            menu5.SetIcon("Menu_tower.png");
            menu5.CalsBtnVisible = Visibility.Visible;
            menu5.LoadBtnVisible = Visibility.Visible;
            menuItems.Add(menu5);

            baseDataMudule.MenuItems = menuItems;

            return baseDataMudule;
        }

        public void NewTowerSubMenuItem(MenuItemVM menuVm)
        {
            var subMenus = new List<MenuItemVM>() { };
            var weatherMenu = new MenuItemVM("BaseAndLineParasModule", this, "  计算参数", (e) => { OnSelectedStruCalsSubModuleChanged(e); });
            weatherMenu.ParentNode = menuVm;
            subMenus.Add(weatherMenu);

            var towerMenu = new MenuItemVM("WorkConditionComboModule", this, "  工况组合", (e) => { OnSelectedStruCalsSubModuleChanged(e); });
            towerMenu.ParentNode = menuVm;
            subMenus.Add(towerMenu);

            var hangingPointMenu = new MenuItemVM("HangingPointModule", this, "  挂点设置", (e) => { OnSelectedStruCalsSubModuleChanged(e); });
            hangingPointMenu.ParentNode = menuVm;
            subMenus.Add(hangingPointMenu);

            var struCalsResultMenu = new MenuItemVM("StruCalsResultModule", this, "  计算结果", (e) => { OnSelectedStruCalsSubModuleChanged(e); });
            struCalsResultMenu.ParentNode = menuVm;
            subMenus.Add(struCalsResultMenu);

            menuVm.ChildItems = subMenus;

            MenuItems = new ObservableCollection<MenuItemVM>(SelectedModuleInfo.MenuItems);

            weatherMenu.Show(menuVm.Title);

        }

        private void OnSelectedStruCalsSubModuleChanged(MenuItemVM menuVm)
        {
            IStruCalsBaseViewModel viewModel = NavigationService.Current as IStruCalsBaseViewModel;
            if (curSubModule != menuVm.Title || (viewModel != null && viewModel.GetTowerType() != menuVm.ParentNode.Title))
            {
                menuVm.Show(menuVm.ParentNode.Title);
                curSubModule = menuVm.Title;
            }
        }

        private void OnSelectedStruCalsTowersChanged(MenuItemVM menuVm)
        {
         
            
        }

        


    }
}
