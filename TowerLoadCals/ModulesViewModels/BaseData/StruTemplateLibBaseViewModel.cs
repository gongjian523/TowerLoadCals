using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerLoadCals.BLL;
using TowerLoadCals.DAL;
using TowerLoadCals.Mode;

namespace TowerLoadCals.Modules
{
    public class StruTemplateLibBaseViewModel:ViewModelBase
    {
        public ObservableCollection<TowerTemplateStorageInfo> _towerTemplates = new ObservableCollection<TowerTemplateStorageInfo>();
        public ObservableCollection<TowerTemplateStorageInfo> TowerTemplates
        {
            get
            {
                return _towerTemplates;
            }
            set
            {
                _towerTemplates = value;
                RaisePropertyChanged("TowerTemplates");
            }
        }


        public StruTemplateLibBaseViewModel()
        {
            InitializeData();
        }


        protected void InitializeData()
        {
            var templates = GetTemplate();
            TowerTemplates = new ObservableCollection<TowerTemplateStorageInfo>(templates);
        }

        protected  virtual List<TowerTemplateStorageInfo> GetTemplate() { return new List<TowerTemplateStorageInfo>(); }

        protected StruTemplateEditWindow editWindow;

        protected void ShowTemplateEditWindow(TowerTemplateStorageInfo templateInfo, bool isReadOnly)
        {
            TowerTemplate template = new TowerTemplate();

            //templateInfo 不是null，需要从project中读取模板
            if (templateInfo != null)
            {
                var proInstance = ProjectUtils.GetInstance();

                string path = isReadOnly ? proInstance.GetGeneralTowerTemplatePath(templateInfo.Name, templateInfo.TowerType) 
                    : proInstance.GetProjectlTowerTemplatePath(templateInfo.Name, templateInfo.TowerType);

                NewTowerTemplateReader newTemplateReader = new NewTowerTemplateReader( TowerTypeStringConvert.TowerStringToType(templateInfo.TowerType));
                template = newTemplateReader.Read(path);
            }

            StruTemplateEditViewModel model = ViewModelSource.Create(() => new StruTemplateEditViewModel(template, isReadOnly));
            model.CloseEditTemplateWindowEvent += CloseTemplateEditWindow;
            editWindow = new StruTemplateEditWindow();
            editWindow.DataContext = model;
            editWindow.ShowDialog();
        }

        protected void CloseTemplateEditWindow(object sender, bool isSave)
        {
            StruTemplateEditViewModel model = (StruTemplateEditViewModel)sender;
            model.CloseEditTemplateWindowEvent -= CloseTemplateEditWindow;
            if (editWindow != null) editWindow.Close();
            editWindow = null;

            if (isSave)
            {
                InitializeData();
            }
        }
    }
}
