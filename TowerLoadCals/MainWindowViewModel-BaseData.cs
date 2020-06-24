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
        private ModuleInfo IniBaseDataModule()
        {
            ModuleInfo baseDataMudule = new ModuleInfo("BaseDataModule", this, "基础数据");
            baseDataMudule.SetIcon("FolderList_32x32.png");

            var menuItem = new List<MenuItemVM>() { };

            var weatherMenu = new MenuItemVM("WeatherConditionModule", this, "气象条件", (e) => { OnSelectedBaseDataSubModuleChanged(e); }, Visibility.Visible, Visibility.Visible);
            weatherMenu.SetIcon("Menu_weather.png");
            menuItem.Add(weatherMenu);

            var towerMenu = new  MenuItemVM("TowerModule", this, "杆塔", (e) => { OnSelectedBaseDataSubModuleChanged(e); }, Visibility.Visible, Visibility.Visible);
            towerMenu.SetIcon("Menu_tower.png");
            menuItem.Add(towerMenu);

            var wireMenu = new MenuItemVM("WireModule", this, "导地线", (e) => { OnSelectedBaseDataSubModuleChanged(e); });
            wireMenu.SetIcon("Menu_wire.png");
            var wireSubList = new List<MenuItemVM>() { };
            var wireSubMenu1 = new MenuItemVM("", this, "  导线", (e) => { OnSelectedSubModuleItemChanged(e); });
            wireSubMenu1.ParentNode = wireMenu;
            wireSubList.Add(wireSubMenu1);
            var wireSubMenu2 = new MenuItemVM("", this, "  地线", (e) => { OnSelectedSubModuleItemChanged(e); });
            wireSubMenu2.ParentNode = wireMenu;
            wireSubList.Add(wireSubMenu2);
            wireMenu.ChildItems = wireSubList;
            menuItem.Add(wireMenu);

            var strDataMenu = new MenuItemVM("StrDataModule", this, "绝缘子串", (e) => { OnSelectedBaseDataSubModuleChanged(e); });
            strDataMenu.SetIcon("Menu_subString.png");
            var strDataSubList = new List<MenuItemVM>() { };
            var strDataSubMenu1 = new MenuItemVM("", this, "  一般子串", (e) => { OnSelectedSubModuleItemChanged(e); });
            strDataSubMenu1.ParentNode = strDataMenu;
            strDataSubList.Add(strDataSubMenu1);
            var strDataSubMenu2 = new MenuItemVM("", this, "  硬跳线", (e) => { OnSelectedSubModuleItemChanged(e); });
            strDataSubMenu2.ParentNode = strDataMenu;
            strDataSubList.Add(strDataSubMenu2);
            strDataMenu.ChildItems = strDataSubList;
            menuItem.Add(strDataMenu);

            var fitDataMenu = new MenuItemVM("FitDataModule", this, "其他金具", (e) => { OnSelectedBaseDataSubModuleChanged(e); });
            fitDataMenu.SetIcon("Menu_tool.png");
            var fitDataSubList = new List<MenuItemVM>() { };
            var fitDataSubMenu1 = new MenuItemVM("", this, "  防震锤", (e) => { OnSelectedSubModuleItemChanged(e); });
            fitDataSubMenu1.ParentNode = fitDataMenu;
            fitDataSubList.Add(fitDataSubMenu1);
            var fitDataSubMenu2 = new MenuItemVM("", this, "  间隔棒", (e) => { OnSelectedSubModuleItemChanged(e); });
            fitDataSubMenu2.ParentNode = fitDataMenu;
            fitDataSubList.Add(fitDataSubMenu2);
            var fitDataSubMenu3 = new MenuItemVM("", this, "  警示装置", (e) => { OnSelectedSubModuleItemChanged(e); });
            fitDataSubMenu3.ParentNode = fitDataMenu;
            fitDataSubList.Add(fitDataSubMenu3);
            var fitDataSubMenu4 = new MenuItemVM("", this, "  其他装置", (e) => { OnSelectedSubModuleItemChanged(e); });
            fitDataSubMenu4.ParentNode = fitDataMenu;
            fitDataSubList.Add(fitDataSubMenu4);
            fitDataMenu.ChildItems = fitDataSubList;
            menuItem.Add(fitDataMenu);

            var struCalsLibMenu = new MenuItemVM("", this, "结构计算库", (e) => { OnSelectedBaseDataSubModuleChanged(e); });
            struCalsLibMenu.SetIcon("Menu_para.png");
            var struCalsLibSubList = new List<MenuItemVM>() { };
            var baseDataLibMenu = new MenuItemVM("BaseDataLibModule", this, "  基本参数库", (e) => { OnSelectedBaseDataSubModuleChanged(e); });
            baseDataLibMenu.ParentNode = struCalsLibMenu;
            struCalsLibSubList.Add(baseDataLibMenu);
            var extralLoadLibMenu = new MenuItemVM("ExtralLoadLibModule", this, "  附加荷载库", (e) => { OnSelectedBaseDataSubModuleChanged(e); });
            extralLoadLibMenu.ParentNode = struCalsLibMenu;
            struCalsLibSubList.Add(extralLoadLibMenu);
            var IceCoverLibModule = new MenuItemVM("IceCoverLibModule", this, "  覆冰参数库", (e) => { OnSelectedBaseDataSubModuleChanged(e); });
            IceCoverLibModule.ParentNode = struCalsLibMenu;
            struCalsLibSubList.Add(IceCoverLibModule);
            struCalsLibMenu.ChildItems = struCalsLibSubList;
            menuItem.Add(struCalsLibMenu);

            baseDataMudule.MenuItems = menuItem;

            return baseDataMudule;
        }

        private void OnSelectedBaseDataSubModuleChanged(MenuItemVM vm)
        {
            if (vm.Title == "结构计算库")
                return;

            if (!UpdateSubModule(vm))
                return;

            curViewMode = NavigationService.Current as IBaseViewModel;
             
            List<MenuItemVM> listMenu = new List<MenuItemVM>();

            //以前加载过这个子模块的导航栏菜单后，这次就不用继续加载了
            if (vm.ChildItems != null && vm.ChildItems.Count > 0)
                return;

            if (vm.Title == "气象条件")
            {
                List<Weather> baseData = ((WeatherConditionViewModel)curViewMode).BaseData;

                foreach(var item in baseData)
                {
                    var subMenu = new MenuItemVM("", this, item.Name, (e) => { OnSelectedSubModuleItemChanged(e); }, Visibility.Visible, Visibility.Collapsed, Visibility.Visible, Visibility.Visible);
                    subMenu.ParentNode = vm;
                    listMenu.Add(subMenu);
                }
                
            }
            else if (vm.Title == "杆塔")
            {
                List<TowerStrCollection> baseData = ((TowerViewModel)curViewMode).BaseData;

                foreach (var item in baseData)
                {
                    List<MenuItemVM> typeMenu = new List<MenuItemVM>();

                    var subMenu = new MenuItemVM("", this, item.Name, (e) => { OnSelectedSubModuleItemChanged(e); }, Visibility.Visible, Visibility.Collapsed, Visibility.Visible, Visibility.Visible);
                    subMenu.ParentNode = vm;
                    var subList = new List<MenuItemVM>() { };

                    foreach (var type in item.Types)
                    {
                        var typeItem = new MenuItemVM("", this, type.Type, (e) => { OnSelectedSubModuleSubTypeChanged(e); });
                        typeItem.ParentNode = subMenu;
                        subList.Add(subMenu);
                    }

                    subMenu.ChildItems = subList;
                    listMenu.Add(subMenu);
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

        private void OnSelectedSubModuleItemChanged(MenuItemVM vm)
        {
            //先判断是否需要加载新子模块
            UpdateSubModule(vm.ParentNode);

            var subVm = GetCurSubModuleVM();

            if (subVm == null)
                return;

            subVm.UpDateView(vm.Title.Trim());
        }

        private void OnSelectedSubModuleSubTypeChanged(MenuItemVM vm)
        {
            //先判断是否需要加载新子模块
            UpdateSubModule(vm.ParentNode.ParentNode);

            var subVm = GetCurSubModuleVM();

            if (subVm == null)
                return;

            subVm.UpDateView(vm.ParentNode.Title.Trim(), vm.Title.Trim());
        }
    }
}
