using DevExpress.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
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

        public void EditTemplate(string name)
        {
            var temp = ProjectUtils.GetInstance().GetProjectTowerTemplate().Where(item => item.Name == name).First();

            if (temp == null)
            {
                MessageBox.Show("无法获取模板详情");
                return;
            }

            ShowTemplateEditWindow(temp, false);
        }

        public void DeleteTemplate(string name)
        {
            var proInst = ProjectUtils.GetInstance();
            var temp = proInst.GetProjectTowerTemplate().Where(item => item.Name == name).First();

            if (temp == null)
            {
                MessageBox.Show("无法获取模板详情");
                return;
            }

            if(MessageBox.Show("确认删除此模板?", "警告", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
            {
                return;
            }

            string path = proInst.GetProjectlTowerTemplatePath(temp.Name, temp.TowerType);
            if (File.Exists(path))
                File.Delete(path);
            proInst.DeleteProjectTowerTemplate(temp);
            TowerTemplates.Remove(TowerTemplates.First(item => item.Name == name));
        }

        public void NewTemplate()
        {
            ShowTemplateEditWindow(null, false);
        }

    }
}
