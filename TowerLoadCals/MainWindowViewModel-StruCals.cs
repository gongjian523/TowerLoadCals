using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
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
        private ModuleMenu IniStruCalsModule()
        {
            ModuleMenu struCalsMudule = new ModuleMenu("StruCalsModule", this, "结构计算", (e) => { OnStruCalsModuleSeleced(e); });
            struCalsMudule.SetIcon("FolderList_32x32.png");

            return struCalsMudule;
        }

        /// <summary>
        /// 点击结构计算按钮时的操作
        /// </summary>
        /// <param name="mv"></param>
        private void OnStruCalsModuleSeleced(ModuleMenu mv)
        {
            NewStruCalsTowerBtnVisibity = Visibility.Visible;

            //以前没有加载过子菜单，从配置文件中读出所有塔位名称
            if(mv.MenuItems == null || mv.MenuItems.Count() == 0)
            {
                var menuItems = new List<SubMenuBase>() { };

                var towers = projectUtils.GetAllStrucTowerNames();

                foreach (var tower in towers)
                {
                    StrCalsModuleSubMenu menu = new StrCalsModuleSubMenu("", this, tower, (e) => { OnSelectedStruCalsTowersChanged(e); });
                    AddStruClasTowerSubMenu(menu);
                    menuItems.Add(menu);
                }

                mv.MenuItems = menuItems;
            }

            MenuItems = new ObservableCollection<SubMenuBase>(SelectedModuleInfo.MenuItems);
        }

        protected void AddStruClasTowerSubMenu(SubMenuBase menuVm)
        {
            var subMenus = new List<SubMenuBase>() { };
            var paraMenu = new SubMenuBase("BaseAndLineParasModule", this, "  计算参数", (e) => { OnSelectedStruCalsSubModuleChanged(e); });
            paraMenu.ParentNode = menuVm;
            subMenus.Add(paraMenu);

            var towerMenu = new SubMenuBase("WorkConditionComboModule", this, "  工况组合", (e) => { OnSelectedStruCalsSubModuleChanged(e); });
            towerMenu.ParentNode = menuVm;
            subMenus.Add(towerMenu);

            var hangingPointMenu = new SubMenuBase("HangingPointModule", this, "  挂点设置", (e) => { OnSelectedStruCalsSubModuleChanged(e); });
            hangingPointMenu.ParentNode = menuVm;
            subMenus.Add(hangingPointMenu);

            var struCalsResultMenu = new SubMenuBase("StruCalsResultModule", this, "  计算结果", (e) => { OnSelectedStruCalsSubModuleChanged(e); });
            struCalsResultMenu.ParentNode = menuVm;
            subMenus.Add(struCalsResultMenu);

            menuVm.ChildItems = subMenus;
        }

        public void NewTowerSubMenuItem(SubMenuBase menuVm)
        {
            AddStruClasTowerSubMenu(menuVm);
            MenuItems = new ObservableCollection<SubMenuBase>(SelectedModuleInfo.MenuItems);
            menuVm.ChildItems[0].Show(menuVm.Title);
        }

        private void OnSelectedStruCalsSubModuleChanged(SubMenuBase menuVm)
        {
            IStruCalsBaseViewModel viewModel = NavigationService.Current as IStruCalsBaseViewModel;
            if (curSubModule != menuVm.Title || (viewModel != null && viewModel.GetTowerName() != menuVm.ParentNode.Title))
            {
                menuVm.Show(menuVm.ParentNode.Title);
                curSubModule = menuVm.Title;
            }
        }

        private void OnSelectedStruCalsTowersChanged(SubMenuBase menuVm)
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

            StrCalsModuleSubMenu newTowerMenu = new StrCalsModuleSubMenu("", this, newTowerName, (e) => { OnSelectedStruCalsTowersChanged(e); });

            SelectedModuleInfo.MenuItems.Add(newTowerMenu);

            NewTowerSubMenuItem(newTowerMenu);

            MenuItems = new ObservableCollection<SubMenuBase>(SelectedModuleInfo.MenuItems);
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
