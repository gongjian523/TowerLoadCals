using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TowerLoadCals.Mode;
using TowerLoadCals.Modules;

namespace TowerLoadCals
{
    public partial class MainWindowViewModel
    {
        private ModuleInfo IniBaseDataModule()
        {
            var menuItem = new List<MenuItemVM>() {
                new MenuItemVM("WeatherConditionModule", this, "气象条件", (e) => {OnSelectedBaseDataSubModuleChanged(e);}, Visibility.Visible, Visibility.Visible),
                new MenuItemVM("TowerModule", this, "杆塔", (e) => {OnSelectedBaseDataSubModuleChanged(e);}, Visibility.Visible, Visibility.Visible),
                new MenuItemVM("WireModule", this, "导地线", (e) => {OnSelectedBaseDataSubModuleChanged(e);}, Visibility.Collapsed, Visibility.Collapsed, Visibility.Collapsed, Visibility.Collapsed, new List<MenuItemVM>(){
                    new MenuItemVM("", this, "导线", (e) => { OnSelectedWeatherItemChanged(e);}),
                    new MenuItemVM("", this, "地线", (e) => { OnSelectedWeatherItemChanged(e);})
                    }),
                new MenuItemVM("StrDataModule", this, "绝缘子串", (e) => {OnSelectedBaseDataSubModuleChanged(e);}, Visibility.Collapsed, Visibility.Collapsed, Visibility.Collapsed, Visibility.Collapsed, new List<MenuItemVM>(){
                    new MenuItemVM("", this, "一般子串", (e) => { OnSelectedWeatherItemChanged(e);}),
                    new MenuItemVM("", this, "硬跳线", (e) => { OnSelectedWeatherItemChanged(e);})
                }),
                new MenuItemVM("FitDataModule", this, "其他金具", (e) => {OnSelectedBaseDataSubModuleChanged(e);}, Visibility.Collapsed, Visibility.Collapsed, Visibility.Collapsed, Visibility.Collapsed, new List<MenuItemVM>(){
                    new MenuItemVM("", this, "防震锤", (e) => { OnSelectedWeatherItemChanged(e);}),
                    new MenuItemVM("", this, "间隔棒", (e) => { OnSelectedWeatherItemChanged(e);}),
                    new MenuItemVM("", this, "警示装置", (e) => { OnSelectedWeatherItemChanged(e);}),
                    new MenuItemVM("", this, "其他装置", (e) => { OnSelectedWeatherItemChanged(e);})
                }),
            };

            ModuleInfo baseDataMudule = new ModuleInfo("BaseDataModule", this, "基础数据");
            baseDataMudule.SetIcon("FolderList_32x32.png");
            baseDataMudule.MenuItems = menuItem;

            return baseDataMudule;
        }

        private void OnSelectedBaseDataSubModuleChanged(MenuItemVM vm)
        {
            vm.Show();
            var viewModel = NavigationService.Current;
            List<MenuItemVM> listMenu = new List<MenuItemVM>();

            if (vm.ChildItems != null && vm.ChildItems.Count > 0)
                return;

            if (vm.Title == "气象条件")
            {
                List<Weather> baseData = ((WeatherConditionViewModel)viewModel).BaseData;

                foreach(var item in baseData)
                {
                    listMenu.Add(new MenuItemVM("", this, item.Name, (e) => { OnSelectedWeatherItemChanged(e); }, Visibility.Visible, Visibility.Collapsed, Visibility.Visible, Visibility.Visible));
                }
                
            }
            else if (vm.Title == "杆塔")
            {
                List<TowerStrCollection> baseData = ((TowerViewModel)viewModel).BaseData;

                foreach (var item in baseData)
                {
                    List<MenuItemVM> typeMenu = new List<MenuItemVM>();
                    foreach (var type in item.Types)
                    {
                        var typeItem = new MenuItemVM("", this, type.Type, (e) => { OnSelectedTowerItemTypeChanged(e); });
                        typeItem.ParentNode = vm;

                        typeMenu.Add(typeItem);
                    }

                    listMenu.Add(new MenuItemVM("", this, item.Name, (e) => { OnSelectedTowerItemChanged(e); }, Visibility.Visible, Visibility.Collapsed, Visibility.Visible, Visibility.Visible));
                }
                
            }

            if(vm.ChildItems == null)
            {
                vm.ChildItems = new List<MenuItemVM>();
            }
            else
            {
                vm.ChildItems.Clear();
            }
            
            vm.ChildItems = listMenu;

            MenuItems = new ObservableCollection<MenuItemVM>(SelectedModuleInfo.MenuItems);
        }

        private void OnSelectedWeatherItemChanged(MenuItemVM vm)
        {

        }

        private void OnSelectedTowerItemChanged(MenuItemVM vm)
        {

        }

        private void OnSelectedTowerItemTypeChanged(MenuItemVM vm)
        {

        }

        private void OnSelectedWireItemChanged(MenuItemVM vm)
        {

        }

        private void OnSelectedStrDataItemChanged(MenuItemVM vm)
        {

        }

        private void OnSelectedFitDataItemChanged(MenuItemVM vm)
        {

        }
    }
}
