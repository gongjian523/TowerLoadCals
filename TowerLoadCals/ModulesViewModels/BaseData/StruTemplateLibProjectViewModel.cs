using DevExpress.Mvvm;
using System.Collections.ObjectModel;
using System.ComponentModel;
using TowerLoadCals.BLL;
using TowerLoadCals.Mode;

namespace TowerLoadCals.Modules
{
    public class StruTemplateLibProjectViewModel: ViewModelBase
    {
        protected ObservableCollection<TowerTemplateStorageInfo> _towerTemplates = new ObservableCollection<TowerTemplateStorageInfo>();
        public ObservableCollection<TowerTemplateStorageInfo> TowerTemplates
        {
            get
            {
                return _towerTemplates;
            }
            set
            {
                _towerTemplates = value;
                RaisePropertyChanged(() => "TowerTemplates");
            }
        }

        public StruTemplateLibProjectViewModel()
        {
            TowerTemplates = new ObservableCollection<TowerTemplateStorageInfo>( ProjectUtils.GetInstance().GetProjectTowerTemplate());
        }

    }
}
