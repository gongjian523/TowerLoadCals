using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.ModulesViewModels
{
    public class BaseDataViewModel : ViewModelBase
    {
        public BaseDataViewModel()
        {

            //MetadataLocator.Default = MetadataLocator.Create().AddMetadata<PrefixEnumWithExternalMetadata>();

            //Modules = new List<SubModuleInfo>();
            //LoadModules();
        }

        public IEnumerable<SubModuleInfo> Modules { get; protected set; }

        public SubModuleInfo SelectedModuleInfo { get; set; }

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

            SelectedModuleInfo.IsSelected = true;
            SelectedModuleInfo.Show();
        }

        protected void LoadModules()
        {
            Modules = new List<SubModuleInfo>() {
                ViewModelSource.Create(() => new SubModuleInfo("WeatherConditionModule", this, "气象条件")),
                ViewModelSource.Create(() => new SubModuleInfo("WireModule", this, "导地线")),
                ViewModelSource.Create(() => new SubModuleInfo("TowerModule", this, "杆塔")),
                ViewModelSource.Create(() => new SubModuleInfo("StrDataModule", this, "绝缘子串")),
                ViewModelSource.Create(() => new SubModuleInfo("FitDataModule", this, "其他金具")),
            };
            OnModulesLoaded();
        }
    }

    public class SubModuleInfo
    {
        ISupportServices parent;

        public SubModuleInfo(string _type, object parent, string _title)
        {
            Type = _type;
            this.parent = (ISupportServices)parent;
            Title = _title;
        }
        public string Type { get; private set; }
        public virtual bool IsSelected { get; set; }
        public string Title { get; private set; }


        public void Show(object parameter = null)
        {
            INavigationService navigationService = parent.ServiceContainer.GetRequiredService<INavigationService>();
            navigationService.Navigate(Type, parameter, parent);
        }
    }

}
