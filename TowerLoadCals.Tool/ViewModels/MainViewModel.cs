using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using DevExpress.Mvvm;
using TowerLoadCals.Common;
using TowerLoadCals.DAL;
using TowerLoadCals.Mode;

namespace TowerLoadCals.Tool.ViewModels
{
    public class MainViewModel : ViewModelBase
    {

        protected List<String> _towerTypes = new List<string>() { "直线塔", "直转塔", "转角塔", "分支塔", "终端塔" };
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

        protected String _towerType = "直线塔";
        public String TowerType
        {
            get
            {
                return _towerType;
            }
            set
            {
                _towerType = value;
            }
        }

        protected ObservableCollection<TemplateInfo> _templates = new ObservableCollection<TemplateInfo>();
        public ObservableCollection<TemplateInfo> Templates
        {
            get
            {
                return _templates;
            }
            set
            {
                _templates = value;
                RaisePropertyChanged("Templates");
            }
        }


        public void ChooseWorkConditionTemplate()
        {
            var openTemplateDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "模板文件 (*.dll)|*.dll",
                Multiselect = true
            };

            if (openTemplateDialog.ShowDialog() != true)
                return;

            var list = new List<TemplateInfo>();

            int num = Templates.Count;

            foreach(var item in openTemplateDialog.FileNames)
            {
                Templates.Add(new TemplateInfo()
                {
                    Index = (++num),
                    Name = item,
                    TowerType = TowerType
                });
            }
        }


        public void ConvertTemplate()
        {
            foreach (var item in Templates)
            {
                string datPath = item.Name.Substring(0, item.Name.Length - 3) + "dat";
                DES.DesDecrypt(item.Name, datPath, "12345678");

                TowerTemplateReader templateReader = new TowerTemplateReader(TowerTypeStringConvert.TowerStringToType(item.TowerType));
                TowerTemplate template = templateReader.Read(datPath);

                string dirPath = datPath.Substring(0, datPath.LastIndexOf('\\'));
                string templateName = datPath.Substring(datPath.LastIndexOf('\\')+1);

                string newDirPath = dirPath + "\\新模板\\";

                if (!Directory.Exists(newDirPath))
                    Directory.CreateDirectory(newDirPath);

                string newTemplatePath = newDirPath + templateName;

                NewTowerTemplateReader newTemplateReader = new NewTowerTemplateReader(TowerTypeStringConvert.TowerStringToType(item.TowerType));
                newTemplateReader.Save(newTemplatePath, template);
            }
        }


        public void ClearTemplate()
        {
            Templates.Clear();
        }
    }


    public class TemplateInfo
    {
        public int Index { get; set; }

        public string Name { get; set; }
        
        public string TowerType { get; set; }
    }
}