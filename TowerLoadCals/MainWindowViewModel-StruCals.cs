using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using TowerLoadCals.BLL;
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
            NewTowerSequenceTowerBtnVisibity = Visibility.Collapsed;

            //清除以前的结构计算模块下的子菜单，
            if (mv.MenuItems != null)
                mv.MenuItems.Clear();

            //根据当前铁塔序列加载其下所选择的塔位或者单独增加的塔位
            LaodTowerSubMenu(_curTowerSeqence == "" ? true :false, mv);

            StruCalsTowerSingleBtnVisibity = _curTowerSeqence == "" ? Visibility.Visible : Visibility.Collapsed;
            StruCalsTowerSerialBtnVisibity = _curTowerSeqence != "" ? Visibility.Visible : Visibility.Collapsed;
        }

        protected void AddStruClasTowerSubMenu(SubMenuBase menuVm)
        {
            var subMenus = new List<SubMenuBase>() { };

            var elecLoadMenu = new SubMenuBase("StruCalsElecLoadModule", this, "  电气荷载", (e) => { OnSelectedStruCalsSubModuleChanged(e); });
            elecLoadMenu.ParentNode = menuVm;
            subMenus.Add(elecLoadMenu);

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

            var fullStressRstMenu = new SubMenuBase("TowerMemberModule", this, "  满应力分析结果", (e) => { OnSelectedStruCalsSubModuleChanged(e); });
            fullStressRstMenu.ParentNode = menuVm;
            subMenus.Add(fullStressRstMenu);

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
                //杆塔序列中的塔需要将序列名字也传进去
                if(((StrCalsModuleSubMenu)menuVm.ParentNode).Sequence == null || ((StrCalsModuleSubMenu)menuVm.ParentNode).Sequence == "")
                    menuVm.Show(menuVm.ParentNode.Title.Trim());
                else
                    menuVm.Show(menuVm.ParentNode.Title.Trim() + "*" + ((StrCalsModuleSubMenu)menuVm.ParentNode).Sequence);
                curSubModule = menuVm.Title;
            }
        }
        
        private void OnSelectedStruCalsTowersChanged(SubMenuBase menuVm)
        {
            IStruCalsBaseViewModel viewModel = NavigationService.Current as IStruCalsBaseViewModel;
            if (viewModel == null || (viewModel != null && viewModel.GetTowerName() != menuVm.Title))
            {
                //杆塔序列中的塔需要将序列名字也传进去
                if (((StrCalsModuleSubMenu)menuVm).Sequence == null || ((StrCalsModuleSubMenu)menuVm).Sequence == "")
                    menuVm.ChildItems[0].Show(menuVm.Title.Trim());
                else
                    menuVm.ChildItems[0].Show(menuVm.Title.Trim() + "*" + ((StrCalsModuleSubMenu)menuVm).Sequence);
                curSubModule = menuVm.Title;
            }
        }

        #region 新增塔位
        NewStruCalsTowerWindow newStruCalsTowerWindow;
        public void ShowNewStruCalsTowerWindow()
        {
            newStruCalsTowerWindow = new NewStruCalsTowerWindow();
            ((NewStruCalsTowerViewModel)(newStruCalsTowerWindow.DataContext)).CloseStruCalsTowerDetailWindowEvent += NewStruCalsTowerWindowClosed;
            newStruCalsTowerWindow.ShowDialog();
        }

        public void NewStruCalsTowerWindowClosed(object sender, string newTowerName)
        {
            NewStruCalsTowerViewModel model = (NewStruCalsTowerViewModel)sender;
            model.CloseStruCalsTowerDetailWindowEvent -= NewStruCalsTowerWindowClosed;
            if (newStruCalsTowerWindow != null) newStruCalsTowerWindow.Close();
            newStruCalsTowerWindow = null;

            if (newTowerName == null || newTowerName == "")
            {
                return;
            }

            StrCalsModuleSubMenu newTowerMenu = new StrCalsModuleSubMenu("", this, newTowerName, "", (e) => { OnSelectedStruCalsTowersChanged(e); });

            SelectedModuleInfo.MenuItems.Add(newTowerMenu);

            NewTowerSubMenuItem(newTowerMenu);

            MenuItems = new ObservableCollection<SubMenuBase>(SelectedModuleInfo.MenuItems);
        }
        #endregion

        #region 塔位编辑
        public void ShowEidtStruCalsTowerWindow(string towerNmae)
        {
            newStruCalsTowerWindow = new NewStruCalsTowerWindow();
            ((NewStruCalsTowerViewModel)(newStruCalsTowerWindow.DataContext)).CloseStruCalsTowerDetailWindowEvent += NewStruCalsTowerWindowClosed;
            newStruCalsTowerWindow.ShowDialog();
        }

        public void EidtStruCalsTowerWindowClosed(object sender, string newTowerName)
        {
            NewStruCalsTowerViewModel model = (NewStruCalsTowerViewModel)sender;
            model.CloseStruCalsTowerDetailWindowEvent -= NewStruCalsTowerWindowClosed;
            if (newStruCalsTowerWindow != null) newStruCalsTowerWindow.Close();
            newStruCalsTowerWindow = null;

            if (newTowerName == null || newTowerName == "")
            {
                return;
            }

            StrCalsModuleSubMenu newTowerMenu = new StrCalsModuleSubMenu("", this, newTowerName, "", (e) => { OnSelectedStruCalsTowersChanged(e); });

            SelectedModuleInfo.MenuItems.Add(newTowerMenu);

            NewTowerSubMenuItem(newTowerMenu);

            MenuItems = new ObservableCollection<SubMenuBase>(SelectedModuleInfo.MenuItems);
        }
        #endregion

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

        protected Visibility _struCalsTowerSerialBtnVisibity = Visibility.Collapsed;
        public Visibility StruCalsTowerSerialBtnVisibity
        {
            set
            {
                _struCalsTowerSerialBtnVisibity = value;
                RaisePropertyChanged("StruCalsTowerSerialBtnVisibity");
            }
            get
            {
                return _struCalsTowerSerialBtnVisibity;
            }
        }

        protected Visibility _struCalsTowerSingleBtnVisibity = Visibility.Visible;
        public Visibility StruCalsTowerSingleBtnVisibity
        {
            set
            {
                _struCalsTowerSingleBtnVisibity = value;
                RaisePropertyChanged("StruCalsTowerSingleBtnVisibity");
            }
            get
            {
                return _struCalsTowerSingleBtnVisibity;
            }
        }

        protected string _curTowerSeqence = "";

        public void StruCalsTowerSerialShow()
        {
            StruCalsTowerSingleBtnVisibity = Visibility.Collapsed;
            StruCalsTowerSerialBtnVisibity = Visibility.Visible;

            var struCalsMenu = Modules.Where(item => item.Title.Trim() == "结构计算").FirstOrDefault();
            if(struCalsMenu != null)
                LaodTowerSubMenu(false, struCalsMenu);
        }

        public void StruCalsTowerSingleShow()
        {
            StruCalsTowerSingleBtnVisibity = Visibility.Visible;
            StruCalsTowerSerialBtnVisibity = Visibility.Collapsed;

            var struCalsMenu = Modules.Where(item => item.Title.Trim() == "结构计算").FirstOrDefault();
            if (struCalsMenu != null)
                LaodTowerSubMenu(true, struCalsMenu);
        }


        protected void LaodTowerSubMenu(bool isSingle, ModuleMenu mv)
        {
            var menuItems = new List<SubMenuBase>() { };

            var towers = isSingle ? projectUtils.GetAllStrucTowerNames() : GlobalInfo.GetInstance().GetSelecedTowerNamesInSequence(_curTowerSeqence);

            string seq = isSingle ? "" : _curTowerSeqence;

            foreach (var tower in towers)
            {
                StrCalsModuleSubMenu menu = new StrCalsModuleSubMenu("", this, tower, seq, (e) => { OnSelectedStruCalsTowersChanged(e); });
                AddStruClasTowerSubMenu(menu);
                menuItems.Add(menu);
            }

            mv.MenuItems = menuItems;

            MenuItems = new ObservableCollection<SubMenuBase>(SelectedModuleInfo.MenuItems);
        }

    }
}


