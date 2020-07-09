using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using TowerLoadCals.Mode;
using TowerLoadCals.Modules;
using TowerLoadCals.ModulesViewModels;

namespace TowerLoadCals
{
    public partial class MainWindowViewModel
    {
        private ModuleMenu IniBaseDataModule()
        {
            ModuleMenu baseDataMudule = new ModuleMenu("BaseDataModule", this, "基础数据", (e) => { OnSelectedModuleChanged(e); });
            baseDataMudule.SetIcon("FolderList_32x32.png");

            //左侧基础菜单列表
            var menuItem = new List<SubMenuBase>() { };
            GetMenuList(menuItem,true);
            baseDataMudule.MenuItems = menuItem;

            //右侧网络菜单列表
            //var InternetMenuItem = new List<SubMenuBase>() { };
            //GetMenuList(InternetMenuItem,false);
            //baseDataMudule.InternetMenuItems = InternetMenuItem;

            var rightMemuItem = new List<SubMenuBase> { };
            GetMenuList(rightMemuItem, false);
            InternetMenuItems = new ObservableCollection<SubMenuBase>(rightMemuItem);

            return baseDataMudule;
        }

        #region 菜单列表
        /// <summary>
        /// 菜单列表
        /// </summary>
        /// <param name="menuItem"></param>
        private void GetMenuList(List<SubMenuBase> menuItem, bool IsBaseMenu)
        {
            if (IsBaseMenu)//左侧主菜单
            {
                var towerMenu = new SubMenuBase("TowerModule", this, "杆塔", (e) => { OnSelectedBaseDataSubModuleChanged(e); });
                towerMenu.SetIcon("Menu_tower.png");
                menuItem.Add(towerMenu);
            }

            var weatherMenu = new SubMenuBase(IsBaseMenu ? "WeatherConditionModule" : "WeatherConditionModule_Internet", this, "气象条件", (e) => { OnSelectedBaseDataSubModuleChanged(e); });
            weatherMenu.SetIcon("Menu_weather.png");
            menuItem.Add(weatherMenu);

            var wireMenu = new SubMenuBase(IsBaseMenu ? "WireModule" : "WireModule_Internet", this, "导地线", (e) => { OnSelectedBaseDataSubModuleChanged(e); });
            wireMenu.SetIcon("Menu_wire.png");
            var wireSubList = new List<SubMenuBase>() { };
            var wireSubMenu1 = new SubMenuBase("", this, "  导线", (e) => { OnSelectedSubModuleItemChanged(e); });
            wireSubMenu1.ParentNode = wireMenu;
            wireSubList.Add(wireSubMenu1);
            var wireSubMenu2 = new SubMenuBase("", this, "  地线", (e) => { OnSelectedSubModuleItemChanged(e); });
            wireSubMenu2.ParentNode = wireMenu;
            wireSubList.Add(wireSubMenu2);
            wireMenu.ChildItems = wireSubList;
            menuItem.Add(wireMenu);

            var strDataMenu = new SubMenuBase(IsBaseMenu ? "StrDataModule" : "StrDataModule_Internet", this, "绝缘子串", (e) => { OnSelectedBaseDataSubModuleChanged(e); });
            strDataMenu.SetIcon("Menu_subString.png");
            var strDataSubList = new List<SubMenuBase>() { };
            var strDataSubMenu1 = new SubMenuBase("", this, "  一般子串", (e) => { OnSelectedSubModuleItemChanged(e); });
            strDataSubMenu1.ParentNode = strDataMenu;
            strDataSubList.Add(strDataSubMenu1);
            var strDataSubMenu2 = new SubMenuBase("", this, "  硬跳线", (e) => { OnSelectedSubModuleItemChanged(e); });
            strDataSubMenu2.ParentNode = strDataMenu;
            strDataSubList.Add(strDataSubMenu2);
            strDataMenu.ChildItems = strDataSubList;
            menuItem.Add(strDataMenu);

            var fitDataMenu = new SubMenuBase(IsBaseMenu ? "FitDataModule" : "FitDataModule_Internet", this, "其他金具", (e) => { OnSelectedBaseDataSubModuleChanged(e); });
            fitDataMenu.SetIcon("Menu_tool.png");
            var fitDataSubList = new List<SubMenuBase>() { };
            var fitDataSubMenu1 = new SubMenuBase("", this, "  防震锤", (e) => { OnSelectedSubModuleItemChanged(e); });
            fitDataSubMenu1.ParentNode = fitDataMenu;
            fitDataSubList.Add(fitDataSubMenu1);
            var fitDataSubMenu2 = new SubMenuBase("", this, "  间隔棒", (e) => { OnSelectedSubModuleItemChanged(e); });
            fitDataSubMenu2.ParentNode = fitDataMenu;
            fitDataSubList.Add(fitDataSubMenu2);
            var fitDataSubMenu3 = new SubMenuBase("", this, "  警示装置", (e) => { OnSelectedSubModuleItemChanged(e); });
            fitDataSubMenu3.ParentNode = fitDataMenu;
            fitDataSubList.Add(fitDataSubMenu3);
            var fitDataSubMenu4 = new SubMenuBase("", this, "  其他装置", (e) => { OnSelectedSubModuleItemChanged(e); });
            fitDataSubMenu4.ParentNode = fitDataMenu;
            fitDataSubList.Add(fitDataSubMenu4);
            fitDataMenu.ChildItems = fitDataSubList;
            menuItem.Add(fitDataMenu);

            if (IsBaseMenu)//左侧主菜单
            {
                var struCalsLibMenu = new SubMenuBase("", this, "结构计算库", (e) => { OnSelectedBaseDataSubModuleChanged(e); });
                struCalsLibMenu.SetIcon("Menu_para.png");
                var struCalsLibSubList = new List<SubMenuBase>() { };
                var baseDataLibMenu = new SubMenuBase("StruCalsLibBaseDataModule", this, "  基本参数库", (e) => { OnSelectedBaseDataSubModuleChanged(e); });
                baseDataLibMenu.ParentNode = struCalsLibMenu;
                struCalsLibSubList.Add(baseDataLibMenu);
                var extralLoadLibMenu = new SubMenuBase("StruCalsLibExtralLoadModule", this, "  附加荷载库", (e) => { OnSelectedBaseDataSubModuleChanged(e); });
                extralLoadLibMenu.ParentNode = struCalsLibMenu;
                struCalsLibSubList.Add(extralLoadLibMenu);
                var IceCoverLibModule = new SubMenuBase("StruCalsLibIceCoverModule", this, "  覆冰参数库", (e) => { OnSelectedBaseDataSubModuleChanged(e); });
                IceCoverLibModule.ParentNode = struCalsLibMenu;
                struCalsLibSubList.Add(IceCoverLibModule);
                struCalsLibMenu.ChildItems = struCalsLibSubList;
                menuItem.Add(struCalsLibMenu);
            }
        } 
        #endregion

        private void OnSelectedModuleChanged(ModuleMenu mv)
        {
            NewStruCalsTowerBtnVisibity = (mv != null && mv.Title == "结构计算") ? Visibility.Visible : Visibility.Collapsed;
        }

        private void OnSelectedBaseDataSubModuleChanged(SubMenuBase vm)
        {
            if (vm.Title == "结构计算库")
                return;

            if (!UpdateSubModule(vm))
                return;

            curViewMode = NavigationService.Current as IBaseViewModel;
             
            List<SubMenuBase> listMenu = new List<SubMenuBase>();

            //以前加载过这个子模块的导航栏菜单后，这次就不用继续加载了
            if (vm.ChildItems != null && vm.ChildItems.Count > 0)
                return;

            if (vm.Title == "气象条件")
            {
                List<Weather> baseData = ((WeatherConditionViewModel)curViewMode).BaseData;

                foreach(var item in baseData)
                {
                    var subMenu = new SubMenuBase("", this, item.Name, (e) => { OnSelectedSubModuleItemChanged(e); });
                    subMenu.ParentNode = vm;
                    listMenu.Add(subMenu);
                }
                
            }
            else if (vm.Title == "杆塔")
            {
                List<TowerStrCollection> baseData = ((TowerViewModel)curViewMode).BaseData;

                foreach (var item in baseData)
                {
                    List<SubMenuBase> typeMenu = new List<SubMenuBase>();

                    var subMenu = new SubMenuBase("", this, item.Name, (e) => { OnSelectedSubModuleItemChanged(e); });
                    subMenu.ParentNode = vm;
                    var subList = new List<SubMenuBase>() { };

                    foreach (var type in item.Types)
                    {
                        var typeItem = new SubMenuBase("", this, type.Type, (e) => { OnSelectedSubModuleSubTypeChanged(e); });
                        typeItem.ParentNode = subMenu;
                        subList.Add(subMenu);
                    }

                    subMenu.ChildItems = subList;
                    listMenu.Add(subMenu);
                }
                
            }

            if(vm.ChildItems == null)
            {
                vm.ChildItems = new List<SubMenuBase>();
            }
            else
            {
                vm.ChildItems.Clear();
            }
            
            vm.ChildItems = listMenu;

            MenuItems = new ObservableCollection<SubMenuBase>(SelectedModuleInfo.MenuItems);
        }

        private void OnSelectedSubModuleItemChanged(SubMenuBase vm)
        {
            //先判断是否需要加载新子模块
            UpdateSubModule(vm.ParentNode);

            var subVm = GetCurSubModuleVM();

            if (subVm == null)
                return;

            subVm.UpDateView(vm.Title.Trim());
        }

        private void OnSelectedSubModuleSubTypeChanged(SubMenuBase vm)
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
