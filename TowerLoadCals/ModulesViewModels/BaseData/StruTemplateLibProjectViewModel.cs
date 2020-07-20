using DevExpress.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using TowerLoadCals.BLL;
using TowerLoadCals.Mode;

namespace TowerLoadCals.Modules
{
    public class StruTemplateLibProjectViewModel:StruTemplateLibBaseViewModel
    {
        protected override List<TowerTemplateStorageInfo> GetTemplate()
        {
            return ProjectUtils.GetInstance().GetProjectTowerTemplate();
        }

        public void EidtTemplate(string name)
        {
            //ShowTemplateEditWindow(template, false);
        }

        public void NewTemplate()
        {
            ShowTemplateEditWindow(null, false);
        }

    }
}
