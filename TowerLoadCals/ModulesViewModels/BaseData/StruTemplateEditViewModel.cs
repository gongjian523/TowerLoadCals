using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TowerLoadCals.Common.Utils;
using System.Windows.Input;
using TowerLoadCals.BLL;
using TowerLoadCals.Mode;
using TowerLoadCals.DAL;
using System.IO;

namespace TowerLoadCals.Modules
{
    public class StruTemplateEditViewModel : ViewModelBase
    {
        //这个参数不仅表示模块是否可以编辑
        //还表示模板的类型是通用模板还是工程模板
        //通用模板不可编辑，工程模板可以编辑
        protected bool _isReadOnly;
        public bool IsReadOnly
        {
            get
            {
                return _isReadOnly;
            }
            set
            {
                _isReadOnly = value;
                RaisePropertyChanged("IsReadOnly");
            }
        }

        protected TowerTemplate _template;

        protected string _towerType;
        public string TowerType
         {
            get
            {
                return _template.TowerType;
            }
            set
            {
                _template.TowerType = value;
                RaisePropertyChanged("TowerType");
            }
        }

        protected List<String> _towerTypes = new List<string>() { "直线塔", "直转塔", "转角塔",  "分支塔", "终端塔"};
        public List<String> TowerTypes
        {
            get
            {
                return _towerTypes;
            }
            set
            {
                _towerTypes = value;
            }
        }

        protected string oldName;

        public StruTemplateEditViewModel(TowerTemplate template,  bool isReadOnly = true)
        {
            oldName =  template.Name;

            _template = template;
            _isReadOnly = isReadOnly;
        }


        public delegate void CloseEditTemplateWindowHandler(object sender, bool isSave);
        public event CloseEditTemplateWindowHandler CloseEditTemplateWindowEvent;

        //只有工程模板才会调用这个函数
        public void onConfirm()
        {
            var proUtils = ProjectUtils.GetInstance();

            //新增模板
            if (oldName == null || oldName == "")
            {
                proUtils.InsertProjectTowerTemplate(new TowerTemplateStorageInfo() {
                    Name = _template.Name,
                    //lcp文件中towerType是中文
                    TowerType = _template.TowerType
                });
            }
            else
            {
                //旧模板改了新名字
                if (oldName != _template.Name)
                {
                    proUtils.UpdateProjectTowerTemplateName(oldName, _template.Name);
                }
                //删除旧模板
                string oldTemplatePath = proUtils.GetProjectlTowerTemplatePath(_template.Name, _template.TowerType);
                if(File.Exists(oldTemplatePath))
                {
                    File.Delete(oldTemplatePath);
                }
            }

            //保存模板
            string newTemplatePath = proUtils.GetProjectlTowerTemplatePath(_template.Name, _template.TowerType);
            NewTowerTemplateReader templateReader = new NewTowerTemplateReader(TowerTypeStringConvert.TowerStringToType(_template.TowerType));
            templateReader.Save(newTemplatePath, _template);

            close(true);
        }

        public void onConcel()
        {
            close(false);
        }

        protected void close(bool isSave)
        {
            if(CloseEditTemplateWindowEvent != null)
                CloseEditTemplateWindowEvent(this, isSave);
        }

        public void AddWire()
        {
            
        }

        public void AddWorkCondition()
        {

        }

        public void AddWorkConditionCombo()
        {
            
        }

    }
}
