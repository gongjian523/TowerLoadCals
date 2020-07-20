using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using TowerLoadCals.BLL;
using TowerLoadCals.Mode;

namespace TowerLoadCals.Modules
{
    public class StruTemplateLibGeneralViewModel: StruTemplateLibBaseViewModel
    {

        protected override List<TowerTemplateStorageInfo> GetTemplate()
        {
            return ProjectUtils.GetInstance().GetGeneralTowerTemplate();
        }

        public void EidtTemplate(string name)
        {
            //ShowTemplateEditWindow(name, true);
        }


    }

}
