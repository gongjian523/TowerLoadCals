using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
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
            var temp = ProjectUtils.GetInstance().GetGeneralTowerTemplate().Where(item => item.Name == name).First();

            if (temp == null)
            {
                MessageBox.Show("无法获取模板详情");
                return;
            }

            ShowTemplateEditWindow(temp, true);
        }

    }

}
