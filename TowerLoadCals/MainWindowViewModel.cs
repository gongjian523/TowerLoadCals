using System;
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

namespace TowerLoadCals
{
    public partial class  MainWindowViewModel: ViewModelBase
    {
        protected ProjectUtils projectUtils;

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
            
        }
        public virtual IEnumerable<ModuleInfo> Modules { get; protected set; }

        public virtual ModuleInfo SelectedModuleInfo { get; set; }
        public virtual IEnumerable<SubModuleInfo> SubModules { get; set; }
        public virtual SubModuleInfo SelectedSubModuleInfo { get; set; }

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

            SubModules = SelectedModuleInfo.SubModules;

            if (SubModules == null || SubModules.Count() == 0)
                return;

            SelectedSubModuleInfo = SubModules.ToList().First();
            SelectedSubModuleInfo.IsSelected = true;
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

        public void OnSelectedSubModuleChanged(AccordionSelectedItemChangedEventArgs e)
        {
            if(SelectedSubModuleInfo != null)
                SelectedSubModuleInfo.Show();
        }

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
            List<ModuleInfo> moduleList = new List<ModuleInfo>() {};

            var baseDataSubModules = new List<SubModuleInfo>() {
                ViewModelSource.Create(() => new SubModuleInfo("WeatherConditionModule", this, "气象条件")),
                ViewModelSource.Create(() => new SubModuleInfo("WireModule", this, "导地线")),
                ViewModelSource.Create(() => new SubModuleInfo("TowerModule", this, "杆塔")),
                ViewModelSource.Create(() => new SubModuleInfo("StrDataModule", this, "绝缘子串")),
                ViewModelSource.Create(() => new SubModuleInfo("FitDataModule", this, "其他金具")),
            };

            ModuleInfo baseDataMudule = new ModuleInfo("BaseDataModule", this, "基础数据");
            baseDataMudule.SetIcon("FolderList_32x32.png");
            baseDataMudule.SubModules = baseDataSubModules;
            moduleList.Add(baseDataMudule);

            ModuleInfo towerMudule = new ModuleInfo("TowersModule", this, "塔杆排位");
            towerMudule.SetIcon("FolderList_32x32.png");
            moduleList.Add(towerMudule);

            Modules = moduleList;

            OnModulesLoaded();
        }

        protected void SaveCurrentModule()
        {
            IBaseViewModel viewModel = NavigationService.Current as IBaseViewModel;
            if (viewModel == null)
                return;
            viewModel.Save();
        }

    }

    public class ModuleInfo
    {
        protected ISupportServices parent;

        public ModuleInfo(string _type, object parent, string _title)
        {
            Type = _type;
            this.parent = (ISupportServices)parent;
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

        public virtual IEnumerable<SubModuleInfo> SubModules { get; set; }

        public virtual SubModuleInfo SelectedSubModuleInfo { get; set; }


    }

    public class SubModuleInfo: ModuleInfo
    {

        public SubModuleInfo(string _type, object parent, string _title):base(_type, parent, _title)
        {
            Type = _type;
            Title = _title;
        }

        public  override void  Show(object parameter = null)
        {
            INavigationService navigationService = parent.ServiceContainer.GetRequiredService<INavigationService>();
            navigationService.Navigate(Type, parameter, parent);
        }
    }


    public class PrefixEnumWithExternalMetadata
    {
        
    }
}
