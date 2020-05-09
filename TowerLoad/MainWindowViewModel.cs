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

namespace TowerLoadCals
{
    public class MainWindowViewModel: ViewModelBase
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
                SelectedModuleInfo.Show();
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
            //PrintingService.PrintableControlLink = null;
            SelectedModuleInfo.IsSelected = true;
            SelectedModuleInfo.Show();
        }
        protected virtual void OnIsBackstageOpenChanged()
        {
            //HasPrinting = PrintingService.HasPrinting;
            //if (!HasPrinting && DefaultBackstatgeIndex == 1)
            //    DefaultBackstatgeIndex = 0
            if (DefaultBackstatgeIndex == 1)
                DefaultBackstatgeIndex = 0;
        }
        BitmapImage NewTaskIcon
        {
            get { return new BitmapImage(AssemblyHelper.GetResourceUri(typeof(DXImages).Assembly, "Images/Tasks/NewTask_16x16.png")); }
        }
        BitmapImage NewContactIcon
        {
            get { return new BitmapImage(AssemblyHelper.GetResourceUri(typeof(DXImages).Assembly, "Images/Mail/NewContact_16x16.png")); }
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
            Modules = new List<ModuleInfo>() {
                ViewModelSource.Create(() => new ModuleInfo("WeatherConditionModule", this, "气象条件")),
                ViewModelSource.Create(() => new ModuleInfo("WireModule", this, "导地线")),
                ViewModelSource.Create(() => new ModuleInfo("StrDataModule", this, "绝缘子串")),
                ViewModelSource.Create(() => new ModuleInfo("FitDataModule", this, "其他金具")),
            };
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
        ISupportServices parent;

        public ModuleInfo(string _type, object parent, string _title)
        {
            Type = _type;
            this.parent = (ISupportServices)parent;
            Title = _title;
        }
        public string Type { get; private set; }
        public virtual bool IsSelected { get; set; }
        public string Title { get; private set; }
        public virtual ImageSource Icon { get; set; }
        //public ModuleInfo SetIcon(string icon)
        //{
        //    Icon = new Uri("D:\\智菲\\P - 200325 - 杆塔负荷程序\\数据资源示例\\ModuleIcon.png");
        //    //this.Icon = AssemblyHelper.GetResourceUri(typeof(ModuleInfo).Assembly, string.Format("Images/{0}.png", icon));
        //    return this;
        //}

        public ModuleInfo SetIcon(string icon)
        {

            
            var extension = new SvgImageSourceExtension() { Uri = AssemblyHelper.GetResourceUri(typeof(DXImages).Assembly, "Images/BandedReports.png") };
            //var extension = new SvgImageSourceExtension() { Uri = new Uri(string.Format(@"pack://application:,,,/TowerLoadCals;component/Images/{0}.png", icon), UriKind.RelativeOrAbsolute) };
            this.Icon = (ImageSource)extension.ProvideValue(null);
            return this;
        }
        public void Show(object parameter = null)
        {
            INavigationService navigationService = parent.ServiceContainer.GetRequiredService<INavigationService>();
            navigationService.Navigate(Type, parameter, parent);
        }
    }
    public class PrefixEnumWithExternalMetadata
    {
        
    }
}
