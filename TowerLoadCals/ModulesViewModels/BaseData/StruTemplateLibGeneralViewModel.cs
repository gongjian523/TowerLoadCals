using DevExpress.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using TowerLoadCals.BLL;
using TowerLoadCals.Mode;

namespace TowerLoadCals.Modules
{
    public class StruTemplateLibGeneralViewModel
    {
        public ObservableCollection<TowerTemplateStorageInfoViewMode> TowerTemplates { get; set; }

        public StruTemplateLibGeneralViewModel()
        {
            List <TowerTemplateStorageInfoViewMode> list = new List<TowerTemplateStorageInfoViewMode>();

            var templates = ProjectUtils.GetInstance().GetGeneralTowerTemplate();

            foreach(var item in templates)
            {
                list.Add(new TowerTemplateStorageInfoViewMode()
                {
                    Name = item.Name,
                    TowerType = item.TowerType
                });
            }

            TowerTemplates = new ObservableCollection<TowerTemplateStorageInfoViewMode>(list);
        }

        public void DetailTemplate()
        {

        }

    }


    public class TowerTemplateStorageInfoViewMode: TowerTemplateStorageInfo
    {
        public void DetailTemplate()
        {

        }
    }
}
