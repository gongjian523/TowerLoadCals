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
        /// <summary>
        /// 打开或者新建工程时，初始化的结构计算按钮
        /// 为了节省操作工程的时间，不初始下这个按钮的子菜单
        /// </summary>
        /// <returns></returns>
        private ModuleInfo IniStruCalsModule()
        {
            ModuleInfo struCalsMudule = new ModuleInfo("StruCalsModule", this, "结构计算", (e) => { OnStruCalsModuleSeleced(e); });
            struCalsMudule.SetIcon("FolderList_32x32.png");

            return struCalsMudule;
        }

        /// <summary>
        /// 点击结构计算按钮时的操作
        /// </summary>
        /// <param name="mv"></param>
        private void OnStruCalsModuleSeleced(ModuleInfo mv)
        {
            NewStruCalsTowerBtnVisibity = Visibility.Visible;

            //以前没有加载过子菜单，从配置文件中读出所有塔位名称
            if(mv.MenuItems == null || mv.MenuItems.Count() == 0)
            {
                var menuItems = new List<MenuItemVM>() { };

                var towers = projectUtils.GetAllStrucTowerNames();

                foreach (var tower in towers)
                {
                    MenuItemVM menu = new MenuItemVM("", this, tower, (e) => { OnSelectedStruCalsTowersChanged(e); }, Visibility.Visible);
                    menu.SetIcon("Menu_tower.png");
                    menu.CalsBtnVisible = Visibility.Visible;
                    menu.LoadBtnVisible = Visibility.Visible;

                    AddStruClasTowerSubMenu(menu);

                    menuItems.Add(menu);
                }

                mv.MenuItems = menuItems;
            }

            MenuItems = new ObservableCollection<MenuItemVM>(SelectedModuleInfo.MenuItems);
        }

        protected void AddStruClasTowerSubMenu(MenuItemVM menuVm)
        {
            var subMenus = new List<MenuItemVM>() { };
            var paraMenu = new MenuItemVM("BaseAndLineParasModule", this, "  计算参数", (e) => { OnSelectedStruCalsSubModuleChanged(e); });
            paraMenu.ParentNode = menuVm;
            subMenus.Add(paraMenu);

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
        }

        public void NewTowerSubMenuItem(MenuItemVM menuVm)
        {
            AddStruClasTowerSubMenu(menuVm);
            MenuItems = new ObservableCollection<MenuItemVM>(SelectedModuleInfo.MenuItems);
            menuVm.ChildItems[0].Show(menuVm.Title);

            //paraMenu.Show(menuVm.Title);
        }

        private void OnSelectedStruCalsSubModuleChanged(MenuItemVM menuVm)
        {
            IStruCalsBaseViewModel viewModel = NavigationService.Current as IStruCalsBaseViewModel;
            if (curSubModule != menuVm.Title || (viewModel != null && viewModel.GetTowerName() != menuVm.ParentNode.Title))
            {
                menuVm.Show(menuVm.ParentNode.Title);
                curSubModule = menuVm.Title;
            }
        }

        private void OnSelectedStruCalsTowersChanged(MenuItemVM menuVm)
        {
         
            
        }

        NewStruCalsTowerWindow newStruCalsTowerWindow;
        public void ShowNewStruCalsTowerWindow()
        {
            newStruCalsTowerWindow = new NewStruCalsTowerWindow();
            ((NewStruCalsTowerViewModel)(newStruCalsTowerWindow.DataContext)).NewStruCalsTowerEvent += NewStruCalsTowerWindowClosed;
            newStruCalsTowerWindow.ShowDialog();
        }

        public void NewStruCalsTowerWindowClosed(object sender, string newTowerName)
        {
            NewStruCalsTowerViewModel model = (NewStruCalsTowerViewModel)sender;
            model.NewStruCalsTowerEvent -= NewStruCalsTowerWindowClosed;
            if (newStruCalsTowerWindow != null) newStruCalsTowerWindow.Close();
            newStruCalsTowerWindow = null;

            if(newTowerName == null || newTowerName == "")
            {
                return;
            }

            MenuItemVM newTowerMenu = new MenuItemVM("", this, newTowerName, (e) => { OnSelectedStruCalsTowersChanged(e); }, Visibility.Visible);
            newTowerMenu.SetIcon("Menu_tower.png");
            newTowerMenu.CalsBtnVisible = Visibility.Visible;
            newTowerMenu.LoadBtnVisible = Visibility.Visible;

            SelectedModuleInfo.MenuItems.Add(newTowerMenu);

            NewTowerSubMenuItem(newTowerMenu);

            MenuItems = new ObservableCollection<MenuItemVM>(SelectedModuleInfo.MenuItems);
        }

        protected Visibility _newStruCalsTowerBtnVisibity = Visibility.Collapsed;
        public Visibility NewStruCalsTowerBtnVisibity 
        {
            set
            {
                _newStruCalsTowerBtnVisibity = value;
                RaisePropertyChanged("NewStruCalsTowerBtnVisibity");
            }
            get
            {
                return _newStruCalsTowerBtnVisibity;
            }
        }

    }
}
