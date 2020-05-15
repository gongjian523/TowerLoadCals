﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using DevExpress.Images;
using DevExpress.Mvvm;
using DevExpress.Mvvm.Native;
using DevExpress.Mvvm.POCO;
using DevExpress.Mvvm.UI;
using DevExpress.Utils;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using TowerLoadCals.Modules;
using System.ComponentModel.DataAnnotations;
using DevExpress.Mvvm.DataAnnotations;
using TowerLoadCals.Common;
using TowerLoadCals.ModulesViewModels;
using System.Windows.Media;
using TowerLoadCals.Common.Utils;
using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.Accordion;
using TowerLoadCals.Mode;
using System.Windows.Input;
using System.Collections.ObjectModel;

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

            projectUtils = new ProjectUtils();

            //MetadataLocator.Default = MetadataLocator.Create().AddMetadata<PrefixEnumWithExternalMetadata>();

            Modules = new List<ModuleInfo>();

            CreateProjectCommand = new DelegateCommand(CreateProject);
            OpenProjectCommand = new DelegateCommand(OpenProject);
            CloseProjectCommand = new DelegateCommand(CloseProject);
            SaveProjectCommand = new DelegateCommand(SaveProject);


            NewItemCommand = new DelegateCommand<object>(NewMenuItem);
            EditItemCommand = new DelegateCommand<object>(EditMenuItem);
            DelItemCommand = new DelegateCommand<object>(DelMenuItem);
        }

        public virtual IEnumerable<ModuleInfo> Modules { get; protected set; }
        public virtual ModuleInfo SelectedModuleInfo { get; set; }

        private ObservableCollection<MenuItemVM> _menuItems = new ObservableCollection<MenuItemVM>();
        public ObservableCollection<MenuItemVM> MenuItems
        {
            get
            {
                return _menuItems;
            }

            protected set
            {
                _menuItems = value;
                RaisePropertyChanged("MenuItems");
            }
        }

        public virtual MenuItemVM SelectedMenuItem { get; set; }

        public virtual Type SplashScreenType { get; set; }
        public virtual int DefaultBackstatgeIndex { get; set; }
        public virtual bool HasPrinting { get; set; }
        public virtual bool IsBackstageOpen { get; set; }
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
                SelectedModuleInfo.MenuItems = new List<MenuItemVM>();
            }

            MenuItems = new ObservableCollection<MenuItemVM>(SelectedModuleInfo.MenuItems);

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

            Modules = new List<ModuleInfo>();

            ModuleInfo blankModule = new ModuleInfo("BlankModule", this, "空模板");
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
            List<ModuleInfo> moduleList = new List<ModuleInfo>() { };

            moduleList.Add(IniBaseDataModule());

            ModuleInfo towerMudule = new ModuleInfo("TowersModule", this, "塔杆排位");
            towerMudule.SetIcon("FolderList_32x32.png");
            moduleList.Add(towerMudule);

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
            curViewMode.DelSubItem(((MenuItemVM)menu).Title);
        }

        protected IBaseViewModel GetCurSubModuleVM()
        {
            if (NavigationService == null)
                return null;

            return (NavigationService.Current as IBaseViewModel);
        }

        //更新子模块，返回true； 不更新子模块，返回false
        protected bool UpdateSubModule(MenuItemVM subVm)
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
            MenuItems = new ObservableCollection<MenuItemVM>(SelectedModuleInfo.MenuItems);
        }
    }

    public class ModuleInfo
    {
        protected ISupportServices parent;
        protected MainWindowViewModel parentVm;

        public ModuleInfo(string _type, object parent, string _title)
        {
            Type = _type;
            this.parent = (ISupportServices)parent;
            parentVm = (MainWindowViewModel)parent;
            Title = _title;
        }
        public string Type { get; protected set; }
        public virtual bool IsSelected { get; set; }
        public string Title { get; protected set; }
        public ImageSource Icon { get; set; }

        public void SetIcon(string icon)
        {
            //一定要把image文件加入到工程中
            Icon = ImageSourceHelper.GetImageSource(AssemblyHelper.GetResourceUri(typeof(ModuleInfo).Assembly, string.Format("Images/{0}", icon)));
        }

        public virtual void Show(object parameter = null)
        {
            INavigationService navigationService = parent.ServiceContainer.GetRequiredService<INavigationService>();
            navigationService.Navigate(Type, parameter, parent);
        }
        public virtual IEnumerable<MenuItemVM> MenuItems { get; set; }

        public virtual MenuItemVM SelectedMenuItem { get; set; }

    }

    public class MenuItemVM : ModuleInfo
    {
        public ICommand Command { get; set; }

        public MenuItemVM ParentNode { get; set; }

        public IList<MenuItemVM> ChildItems { get; set; }

        public Visibility ContextVisible { get; set; }

        public Visibility NewBtnVisible { get; set; }

        public Visibility EditBtnVisible { get; set; }

        public Visibility DelBtnVisible { get; set; }



        public MenuItemVM(string _type, 
                        object parent, 
                        string _title, 
                        Action<MenuItemVM> func,
                        Visibility contextVisible = Visibility.Collapsed,
                        Visibility bNewBtnVisible = Visibility.Collapsed,
                        Visibility bEditBtnVisible = Visibility.Collapsed,
                        Visibility bDelBtnVisible = Visibility.Collapsed, 
                        IList<MenuItemVM> children = null) 
                        : base(_type, parent, _title)
        {
            Type = _type;
            Title = _title;
            this.parent = (ISupportServices)parent;

            Command = new DelegateCommand<MenuItemVM>(func);

            ContextVisible = contextVisible;
            NewBtnVisible = bNewBtnVisible;
            EditBtnVisible = bEditBtnVisible;
            DelBtnVisible = bDelBtnVisible;

            ChildItems = children;

            DelItemCommand = new DelegateCommand<object>(DelMenuItem);
        }

        public override void Show(object parameter = null)
        {
            INavigationService navigationService = parent.ServiceContainer.GetRequiredService<INavigationService>();
            navigationService.Navigate(Type, parameter, parent);
        }

        public DelegateCommand<object> DelItemCommand { get; private set; }
        void DelMenuItem(object menu)
        {
            INavigationService navigationService = parent.ServiceContainer.GetRequiredService<INavigationService>();
            IBaseViewModel curViewMode = navigationService.Current as IBaseViewModel;
            if (curViewMode == null)
                return;

            ((MenuItemVM)menu).ParentNode.ChildItems.Remove((MenuItemVM)menu);
            parentVm.UpdateNavigationBar();

            curViewMode.DelSubItem(((MenuItemVM)menu).Title);
        }

    }

    public class PrefixEnumWithExternalMetadata
    {
        
    }
}
