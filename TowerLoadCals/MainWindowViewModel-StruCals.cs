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
using static TowerLoadCals.DAL.TowerTemplateReader;

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
            menu1.CalsBtnVisible = Visibility.Visible;
            menu1.LoadBtnVisible = Visibility.Visible;
            menuItems.Add(menu1);

            MenuItemVM menu2 = new MenuItemVM("", this, "直转塔", (e) => { OnSelectedStruCalsTowersChanged(e); }, Visibility.Visible);
            menu2.CalsBtnVisible = Visibility.Visible;
            menu2.LoadBtnVisible = Visibility.Visible;
            menuItems.Add(menu2);
            
            baseDataMudule.MenuItems = menuItems;

            return baseDataMudule;
        }

        public void NewTowerSubMenuItem(MenuItemVM menuVm)
        {
            var subMenus = new List<MenuItemVM>() { };
            var weatherMenu = new MenuItemVM("BaseAndLineParasModule", this, "结构计算参数", (e) => { OnSelectedStruCalsSubModuleChanged(e); });
            weatherMenu.ParentNode = menuVm;
            subMenus.Add(weatherMenu);

            var towerMenu = new MenuItemVM("WorkConditionComboModule", this, "工况组合", (e) => { OnSelectedStruCalsSubModuleChanged(e); });
            towerMenu.ParentNode = menuVm;
            subMenus.Add(towerMenu);

            var hungingPointMenu = new MenuItemVM("HangingPointModule", this, "挂点设置", (e) => { OnSelectedStruCalsSubModuleChanged(e); });
            hungingPointMenu.ParentNode = menuVm;
            subMenus.Add(hungingPointMenu);

            menuVm.ChildItems = subMenus;

            MenuItems = new ObservableCollection<MenuItemVM>(SelectedModuleInfo.MenuItems);

            weatherMenu.Show(menuVm.Title);

        }

        private void OnSelectedStruCalsSubModuleChanged(MenuItemVM menuVm)
        {
            IStruCalsBaseViewModel viewModel = NavigationService.Current as IStruCalsBaseViewModel;
            if (curSubModule != menuVm.Title || (viewModel != null && viewModel.GetTowerType() != menuVm.Title))
            {
                menuVm.Show(viewModel.GetTowerType());
                curSubModule = menuVm.Title;
            }
        }

        private void OnSelectedStruCalsTowersChanged(MenuItemVM menuVm)
        {
         
            
        }

        


    }
}
