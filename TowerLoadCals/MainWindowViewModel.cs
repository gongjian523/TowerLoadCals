using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.Mvvm;
using System.ComponentModel.DataAnnotations;
using TowerLoadCals.ModulesViewModels;
using System.Collections.ObjectModel;
using TowerLoadCals.BLL;
using System.Windows;
using TowerLoadCals.Service.Helpers;

namespace TowerLoadCals
{
    public partial class MainWindowViewModel : ViewModelBase, INotifyPropertyChanged
    {
        protected ProjectUtils projectUtils;

        //记录当前的子模块，用于判断是否要加载新的模块
        public string curSubModule;
        protected IBaseViewModel curViewMode;

        public MainWindowViewModel()
        {
            SplashScreenType = typeof(SplashScreenWindow);

            projectUtils = ProjectUtils.GetInstance();
       
            //MetadataLocator.Default = MetadataLocator.Create().AddMetadata<PrefixEnumWithExternalMetadata>();
       
            Modules = new List<ModuleMenu>();

            CreateProjectCommand = new DelegateCommand(CreateProject);
            OpenProjectCommand = new DelegateCommand(OpenProject);
            CloseProjectCommand = new DelegateCommand(CloseProject);
            SaveProjectCommand = new DelegateCommand(SaveProject);


            NewItemCommand = new DelegateCommand<object>(NewMenuItem);
            EditItemCommand = new DelegateCommand<object>(EditMenuItem);
            DelItemCommand = new DelegateCommand<object>(DelMenuItem);

            var rightMemuItem = new List<SubMenuBase> { };
            GetRightMenuList(rightMemuItem);
            InternetMenuItems = new ObservableCollection<SubMenuBase>(rightMemuItem);

            //InternetLinkEnabled = InternetLinkHelper.GetInternetLink();

        }

        public virtual IEnumerable<ModuleMenu> Modules { get; protected set; }
        public virtual ModuleMenu SelectedModuleInfo { get; set; }


        private ObservableCollection<SubMenuBase> _menuItems = new ObservableCollection<SubMenuBase> ();
        /// <summary>
        /// 保存每个模块下面的的子按钮
        /// </summary>
        public ObservableCollection<SubMenuBase> MenuItems
        {
            get
            {
                return _menuItems;
            }

            set
            {
                _menuItems = value;
                RaisePropertyChanged("MenuItems");
            }
        }


        private ObservableCollection<SubMenuBase> _internetMenuItems = new ObservableCollection<SubMenuBase>();
        /// <summary>
        /// 保存每个模块下面的的子按钮
        /// </summary>
        public ObservableCollection<SubMenuBase> InternetMenuItems
        {
            get
            {
                return _internetMenuItems;
            }

            protected set
            {
                _internetMenuItems = value;
                RaisePropertyChanged("InternetMenuItems");
            }
        }

        public virtual SubMenuBase SelectedMenuItem { get; set; }

        public virtual Type SplashScreenType { get; set; }
        public virtual int DefaultBackstatgeIndex { get; set; }
        public virtual bool HasPrinting { get; set; }
        public virtual bool IsBackstageOpen { get; set; }

        public virtual bool InternetLinkEnabled { get; set; }
        public void Exit()
        {
            CurrentWindowService.Close();
        }
        public void OnModulesLoaded()
        {
            if (Modules.Count() == 0)
                return;

            if (SelectedModuleInfo == null)
            {
                SelectedModuleInfo = Modules.First();
                SelectedModuleInfo.IsSelected = true;
            }

            //SplashScreenType = typeof(ProgressWindow);
            //ApplicationJumpListService.Items.AddOrReplace("New Task", NewTaskIcon, ShowGridTasksModuleNewItemWindow);
            //ApplicationJumpListService.Apply();
        }
        [Required]
        protected virtual ICurrentWindowService CurrentWindowService { get { return null; } }
        [Required]
        protected virtual IApplicationJumpListService ApplicationJumpListService { get { return null; } }
        [Required]
        protected virtual INavigationService NavigationService { get { return null; } }
        protected virtual void OnSelectedModuleInfoChanged()
        {
            if (SelectedModuleInfo == null)
                return;

            if (!allowSelectedModuleInfoChanged)
                return;

            //SelectedModuleInfo.IsSelected = true;
            if (SelectedModuleInfo.MenuItems == null)
            {
                SelectedModuleInfo.MenuItems = new List<SubMenuBase>();
            }

            MenuItems = new ObservableCollection<SubMenuBase>(SelectedModuleInfo.MenuItems);

            if (MenuItems == null || MenuItems.Count() == 0)
                return;

            SelectedMenuItem = MenuItems.ToList().First();
            SelectedMenuItem.IsSelected = true;
            //SelectedMenuItem.Show();
            OnSelectedBaseDataSubModuleChanged(SelectedMenuItem);

            curSubModule = SelectedMenuItem.Title;
        }

        protected virtual void OnIsBackstageOpenChanged()
        {
            //HasPrinting = PrintingService.HasPrinting;
            //if (!HasPrinting && DefaultBackstatgeIndex == 1)
            //    DefaultBackstatgeIndex = 0
            if (DefaultBackstatgeIndex == 1)
                DefaultBackstatgeIndex = 0;
        }

        bool allowSelectedModuleInfoChanged = true;

        public DelegateCommand CreateProjectCommand { get; private set; }

        protected void CreateProject()
        {
            if (!projectUtils.CreateProject())
                return;

            LoadModules();
        }

        public DelegateCommand OpenProjectCommand { get; private set; }
        protected void OpenProject()
        {
            if (!projectUtils.OpenProject())
                return;

            LoadModules();
        }

        public DelegateCommand CloseProjectCommand { get; private set; }
        void CloseProject()
        {
            SaveCurrentModule();

            Modules = new List<ModuleMenu>();

            MenuItems = new ObservableCollection<SubMenuBase>();

            NewStruCalsTowerBtnVisibity = Visibility.Collapsed;

            ModuleMenu blankModule = new ModuleMenu("BlankModule", this, "空模板" , (e) => { OnSelectedModuleChanged(e); });
            blankModule.Show();
        }

        public DelegateCommand SaveProjectCommand { get; private set; }
        void SaveProject()
        {
            SaveCurrentModule();
        }

        public DelegateCommand SaveProjectAsCommand { get; private set; }
        void SaveProjectAs()
        {

        }

        protected void LoadModules()
        {
            List<ModuleMenu> moduleList = new List<ModuleMenu>() { };

            moduleList.Add(IniBaseDataModule());

            ModuleMenu towerMudule = new ModuleMenu("TowersModule", this, "塔杆序列", (e) => { OnSelectedModuleChanged(e); });
            towerMudule.SetIcon("FolderList_32x32.png");
            moduleList.Add(towerMudule);

            ModuleMenu elecCalsMudule = new ModuleMenu("ElecCalsModule", this, "电气计算", (e) => { OnSelectedModuleChanged(e); });
            elecCalsMudule.SetIcon("FolderList_32x32.png");
            moduleList.Add(elecCalsMudule);

            moduleList.Add(IniStruCalsModule());

            ModuleMenu resultMudule = new ModuleMenu("ResultModule", this, "成功输出", (e) => { OnSelectedModuleChanged(e); });
            resultMudule.SetIcon("FolderList_32x32.png");
            moduleList.Add(resultMudule);

            Modules = moduleList;

            OnModulesLoaded();
        }


        protected void SaveCurrentModule()
        {
            IBaseViewModel viewModel = GetCurSubModuleVM();
            if (viewModel == null)
                return;
            viewModel.Save();
        }

        public DelegateCommand<object> NewItemCommand { get; private set; }
        void NewMenuItem(object menu)
        {
            IBaseViewModel viewModel = GetCurSubModuleVM();
            if (viewModel == null)
                return;
          
        }

        public DelegateCommand<object> EditItemCommand { get; private set; }
        void EditMenuItem(object menu)
        {
            ;
        }

        public DelegateCommand<object> DelItemCommand { get; private set; }
        void DelMenuItem(object menu)
        {
            IBaseViewModel viewModel = NavigationService.Current as IBaseViewModel;

            if (curViewMode == null)
                return;
            curViewMode.DelSubItem(((SubMenuBase)menu).Title);
        }

        protected IBaseViewModel GetCurSubModuleVM()
        {
            if (NavigationService == null)
                return null;

            return (NavigationService.Current as IBaseViewModel);
        }

        //更新子模块，返回true； 不更新子模块，返回false
        protected bool UpdateSubModule(SubMenuBase subVm)
        {
            if (curSubModule == subVm.Title)
                return false;
            else
            {
                subVm.Show();
                curSubModule = subVm.Title;

                return true;
            }
        }

        public void UpdateNavigationBar()
        {
            MenuItems = new ObservableCollection<SubMenuBase>(SelectedModuleInfo.MenuItems);
        }
    }



    public class PrefixEnumWithExternalMetadata
    {
        
    }
}
