using Microsoft.Win32;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using TowerLoadCals.Common;
using TowerLoadCals.Mode;

namespace TowerLoadCals.BLL
{
    public class ProjectUtils
    {
        private static ProjectUtils singleton;

        private static readonly object locker = new object();

        public string ProjectPath { get; set; }
        public string ProjectName { get; set; }
        protected string ConfigFilePath { get; set; }

        protected GlobalInfo globalInfo = GlobalInfo.GetInstance();

        public static ProjectUtils GetInstance()
        {
            if (singleton == null)
            {
                lock (locker)
                {
                    if (singleton == null)
                    {
                        singleton = new ProjectUtils();
                    }
                }
            }
            return singleton;
        }

        private ProjectUtils()
        {

        }

        public bool CreateProject()
        {
            var saveFileDialog = new SaveFileDialog()
            {
                Filter = "Dat Files (*.lcp)|*.lcp",
            };

            var result = saveFileDialog.ShowDialog();

            if (result != true)
                return false;

            String strDir = saveFileDialog.FileName.Substring(0, saveFileDialog.FileName.Length - 1 - saveFileDialog.SafeFileName.Length);
            string prejectName = saveFileDialog.SafeFileName.Substring(0, saveFileDialog.SafeFileName.Length - 4);

            Directory.CreateDirectory(strDir);
            Directory.CreateDirectory(strDir + "\\" + prejectName);
            Directory.CreateDirectory(strDir + "\\" + prejectName + "\\" + ConstVar.DataBaseStr);

            Directory.CreateDirectory(strDir + "\\" + prejectName + "\\" + ConstVar.DataBaseStr + "\\" + ConstVar.GeneralStruTemplateStr);
            Directory.CreateDirectory(strDir + "\\" + prejectName + "\\" + ConstVar.DataBaseStr + "\\" + ConstVar.GeneralStruTemplateStr + "\\" + ConstVar.LineTowerStr);
            Directory.CreateDirectory(strDir + "\\" + prejectName + "\\" + ConstVar.DataBaseStr + "\\" + ConstVar.GeneralStruTemplateStr + "\\" + ConstVar.LineCornerTowerStr);
            Directory.CreateDirectory(strDir + "\\" + prejectName + "\\" + ConstVar.DataBaseStr + "\\" + ConstVar.GeneralStruTemplateStr + "\\" + ConstVar.CornerTowerStr);
            Directory.CreateDirectory(strDir + "\\" + prejectName + "\\" + ConstVar.DataBaseStr + "\\" + ConstVar.GeneralStruTemplateStr + "\\" + ConstVar.BranchTowerStr);
            Directory.CreateDirectory(strDir + "\\" + prejectName + "\\" + ConstVar.DataBaseStr + "\\" + ConstVar.GeneralStruTemplateStr + "\\" + ConstVar.TerminalTowerStr);

            Directory.CreateDirectory(strDir + "\\" + prejectName + "\\" + ConstVar.DataBaseStr + "\\" + ConstVar.ProjectStruTemplateStr);
            Directory.CreateDirectory(strDir + "\\" + prejectName + "\\" + ConstVar.DataBaseStr + "\\" + ConstVar.ProjectStruTemplateStr + "\\" + ConstVar.LineTowerStr);
            Directory.CreateDirectory(strDir + "\\" + prejectName + "\\" + ConstVar.DataBaseStr + "\\" + ConstVar.ProjectStruTemplateStr + "\\" + ConstVar.LineCornerTowerStr);
            Directory.CreateDirectory(strDir + "\\" + prejectName + "\\" + ConstVar.DataBaseStr + "\\" + ConstVar.ProjectStruTemplateStr + "\\" + ConstVar.CornerTowerStr);
            Directory.CreateDirectory(strDir + "\\" + prejectName + "\\" + ConstVar.DataBaseStr + "\\" + ConstVar.ProjectStruTemplateStr + "\\" + ConstVar.BranchTowerStr);
            Directory.CreateDirectory(strDir + "\\" + prejectName + "\\" + ConstVar.DataBaseStr + "\\" + ConstVar.ProjectStruTemplateStr + "\\" + ConstVar.TerminalTowerStr);

            Directory.CreateDirectory(strDir + "\\" + prejectName + "\\" + ConstVar.StruCalsStr);
            
            if (!ConfigFileUtils.CreateProjetcFile(strDir + "\\" + prejectName + "\\" + saveFileDialog.SafeFileName))
            {
                System.Windows.Forms.MessageBox.Show("新建工程Lcp文件失败");
                Directory.Delete(strDir, true);

                return false;
            }

            ProjectPath = strDir + "\\" + prejectName;
            ProjectName = prejectName;

            ConfigFilePath = strDir + "\\" + prejectName + "\\" + saveFileDialog.SafeFileName;

            globalInfo.ProjectPath = ProjectPath;
            globalInfo.ProjectName = prejectName;

            return true;
        }


        public void  AddFileToProject(string module,string file)
        {
            
        }

        public bool OpenProject()
        {
            var openFileDialog = new OpenFileDialog()
            {
                Filter = "Lcp Files (*.lcp)|*.lcp"
            };

            var result = openFileDialog.ShowDialog();

            if (result != true)
                return false;

            ConfigFilePath = openFileDialog.FileName;

            ProjectPath = openFileDialog.FileName.Substring(0, openFileDialog.FileName.Length - 1 - openFileDialog.SafeFileName.Length);
            ProjectName = openFileDialog.SafeFileName.Substring(0, openFileDialog.SafeFileName.Length - 4);

            globalInfo.ProjectPath = ProjectPath;
            globalInfo.ProjectName = ProjectName;

            return true;
        }


        #region 结构计算 塔位相关函数

        /// <summary>
        /// 在结构计算的参数库的保存文件读取参数，并保存在GlobalInfo中
        /// </summary>
        /// <param name="name"></param>
        public void ReadStruCalsLibParas()
        {
            string path = Directory.GetCurrentDirectory() + "\\" + ConstVar.UserDataStr + "\\" + ConstVar.StruCalsLibFileName;

            StruCalsLib paras = XmlUtils.Deserializer<StruCalsLib>(path);

            if (paras == null || paras == default(StruCalsLib))
                return;

            GlobalInfo.GetInstance().StruCalsLibParas = paras;
        }

        /// <summary>
        /// 将新增的塔位参数添加的GlobalInfo中
        /// </summary>
        /// <param name="towerName"></param>
        /// <param name="towerType"></param>
        /// <param name="templatePath"></param>
        /// <param name="electricalLoadFilePath"></param>
        /// <returns></returns>
        public static bool NewStruCalsTower(string towerName, string towerType, float voltage, string templatePath, string electricalLoadFilePath,  List<string> fullStressTemplatePaths)
        {
            var struCalsParas = GlobalInfo.GetInstance().StruCalsParas;
            
            if(struCalsParas.Where(item=>item.TowerName == towerName).Count() > 0)
            {
                System.Windows.Forms.MessageBox.Show(towerName + "已经存在！");
                return false;
            }

            StruCalsParasCompose paras = new StruCalsParasCompose(towerName, towerType, voltage, templatePath, electricalLoadFilePath, fullStressTemplatePaths, out string decodeTemplateStr);

            if(decodeTemplateStr != "")
            {
                System.Windows.Forms.MessageBox.Show(towerName + decodeTemplateStr);
                return false;
            }

            struCalsParas.Add(paras);

            return true;
        }

        /// <summary>
        /// 在结构计算的参数的保存文件读取参数，并保存在GlobalInfo中
        /// </summary>
        /// <param name="name"></param>
        public void ReadStruCalsTowerParas(string name)
        {
            string struCalsDirPath = StruCalsDirForTower(name) + "\\";
            string fullStessDirPath = FullStressDirForTower(name) + "\\";
            string electricalLaodFilePath = struCalsDirPath + ConstVar.StruCalsElecLoadFileName;

            string parasSavedFilePath = struCalsDirPath + ConstVar.StruCalsParasFileName;
            StruCalsParasCompose temp = XmlUtils.Deserializer<StruCalsParasCompose>(parasSavedFilePath);

            if (temp == null || temp == default(StruCalsParasCompose))
                return;

            string templatePath = struCalsDirPath + temp.TemplateName;

            List<string> fullStressTemplatePaths = new List<string>();
            foreach(var tempName in temp.FullStressTemplateNames)
            {
                fullStressTemplatePaths.Add(fullStessDirPath + tempName);
            }

            StruCalsParasCompose paras = new StruCalsParasCompose(electricalLaodFilePath, templatePath, fullStressTemplatePaths, temp);

            GlobalInfo.GetInstance().StruCalsParas.Add(paras);
        }

        public void SaveStruCalsTower(List<string> towers = null)
        {
            List<StruCalsParasCompose> towerParas;
            
            if(towers == null || towers.Count == 0)
            {
                towerParas = globalInfo.StruCalsParas;
            }
            else
            {
                towerParas = globalInfo.StruCalsParas.Where(item => towers.Contains(item.TowerName)).ToList();
            }

            List<string> savedToewer = GetAllStrucTowerNames();

            foreach(var item in towerParas)
            {
                string dirStruCalsPath = StruCalsDirForTower(item.TowerName);
                string dirFullStressPath = FullStressDirForTower(item.TowerName);

                //配置文件中没有保存新建的塔位：
                //1 把模板和电气荷载文件复制到工程文件下
                //2 把塔位加入到配置文件中
                //3 把满应力分析模板和复制到工程文件目录下
                if (!savedToewer.Contains(item.TowerName))
                {
                    #region 复制结构计算的文件

                    string templatePath = dirStruCalsPath + "\\" + item.TemplateName;
                    string elecLoadFilePath = dirStruCalsPath + "\\" + ConstVar.StruCalsElecLoadFileName;

                    if (!Directory.Exists(dirStruCalsPath))
                        Directory.CreateDirectory(dirStruCalsPath);

                    if (File.Exists(templatePath))
                        File.Delete(templatePath);
                    File.Copy(item.TemplatePath, templatePath);
                    if (File.Exists(elecLoadFilePath))
                        File.Delete(elecLoadFilePath);
                    File.Copy(item.ElectricalLoadFilePath, elecLoadFilePath);

                    InsertStrucTowerName(item.TowerName);
                    #endregion

                    #region 复制满应力分析的文件
                    if (item.FullStressTemplatePaths!=null&&item.FullStressTemplatePaths.Count!=0)
                    { 
                        if (!Directory.Exists(dirFullStressPath))
                        Directory.CreateDirectory(dirFullStressPath);

                    string soureDir = item.FullStressTemplatePaths[0].Substring(0, item.FullStressTemplatePaths[0].LastIndexOf("\\"));

                    foreach (var path in item.FullStressTemplatePaths)
                    {
                        string name = path.Substring(path.LastIndexOf("\\")+1);
                        File.Copy(path, dirFullStressPath + "\\" + name);
                    }

                    File.Copy(soureDir + "\\" + ConstVar.SmartTowerIntFileName, dirFullStressPath + "\\" + ConstVar.SmartTowerIntFileName);
                    File.Copy(soureDir + "\\" + ConstVar.SmartTowerIntCHFileName, dirFullStressPath + "\\" + ConstVar.SmartTowerIntCHFileName);
                    }
                    #endregion
                }

                XmlUtils.Serializer(dirStruCalsPath + "\\" + ConstVar.StruCalsParasFileName, item);
            }
        }


        public List<string> GetAllStrucTowerNames()
        {
            return ConfigFileUtils.GetAllStrucTowerNames(ConfigFilePath);
        }

        public bool InsertStrucTowerName(string towerName)
        {
            return ConfigFileUtils.InsertStrucTowerNames(ConfigFilePath, new List<String> { towerName });
        }

        public bool InsertStrucTowerNames(List<string> towerNames)
        {
            return ConfigFileUtils.InsertStrucTowerNames(ConfigFilePath, towerNames);
        }

        public bool DeleteStrucTowerName(string towerName)
        {
            return ConfigFileUtils.DeleteStrucTowerNames(ConfigFilePath, new List<String> { towerName });
        }

        public bool DeleteStrucTowerNames(List<string> towerNames)
        {
            return ConfigFileUtils.DeleteStrucTowerNames(ConfigFilePath, towerNames);
        }

        public string StruCalsDirForTower(string towerName)
        {
            return  ProjectPath + "\\" + ConstVar.StruCalsStr + "\\" + towerName + "\\" + ConstVar.StruCalsStr;
        }

        public string FullStressDirForTower(string towerName)
        {
            return ProjectPath + "\\" + ConstVar.StruCalsStr + "\\" + towerName + "\\" + ConstVar.FullStressStr;
        }

        //默认这两个文件和满应力模板在同一个目录下
        public void SmartToweIniPath(string templatePath, out string iniPath, out string iniCHPath)
        {
            string dir = templatePath.Substring(0, templatePath.LastIndexOf("\\"));

            iniPath = dir + ConstVar.SmartTowerIntFileName;
            iniCHPath = dir + ConstVar.SmartTowerIntCHFileName;

            return;
        }

        #endregion

        #region 满应力分析相关函数
        public string ReadSmartTowerPath()
        {
            return ConfigSettingsFileUtis.GetSmartTowerSetting(ConfigSettingsFilePath, "Path");
        }

        public int ReadSmartTowerMode()
        {
            return Convert.ToInt32(ConfigSettingsFileUtis.GetSmartTowerSetting(ConfigSettingsFilePath, "Mode"));
        }

        public void SaveSmartTowerPath(string path)
        {
            ConfigSettingsFileUtis.SaveSmartTowerSetting(ConfigSettingsFilePath, "Path", path);
        }

        public void SaveSmartTowerMode(int mode)
        {
            ConfigSettingsFileUtis.SaveSmartTowerSetting(ConfigSettingsFilePath, "Mode", mode.ToString());
        }

        public string ConfigSettingsFilePath
        {
            get { return Directory.GetCurrentDirectory() + "\\" + ConstVar.UserDataStr + "\\" + ConstVar.ConfigSettingsFileName; }
        }

        #endregion


        #region 基本参数 结构模板库相关

        public List<TowerTemplateStorageInfo> GetGeneralTowerTemplate()
        {
            return ConfigFileUtils.GetAllTowerTemplates(ConfigFilePath);
        }

        public List<TowerTemplateStorageInfo> GetProjectTowerTemplate()
        {
            return ConfigFileUtils.GetAllTowerTemplates(ConfigFilePath, false);
        }

        public bool InsertGeneralTowerTemplate(TowerTemplateStorageInfo template)
        {
            return ConfigFileUtils.InsertTowerTemplates(ConfigFilePath, new List<TowerTemplateStorageInfo> { template });
        }

        public bool InsertGeneralTowerTemplates(List<TowerTemplateStorageInfo> templates)
        {
            return ConfigFileUtils.InsertTowerTemplates(ConfigFilePath, templates);
        }

        public bool InsertProjectTowerTemplate(TowerTemplateStorageInfo template)
        {
            return ConfigFileUtils.InsertTowerTemplates(ConfigFilePath, new List<TowerTemplateStorageInfo> { template }, false);
        }

        public bool InsertProjectTowerTemplates(List<TowerTemplateStorageInfo> templates)
        {
            return ConfigFileUtils.InsertTowerTemplates(ConfigFilePath, templates, false);
        }

        public bool DeleteGeneralTowerTemplate(TowerTemplateStorageInfo template)
        {
            return ConfigFileUtils.DeleteTowerTemplates(ConfigFilePath, new List<TowerTemplateStorageInfo> { template });
        }

        public bool DeleteGeneralTowerTemplates(List<TowerTemplateStorageInfo> templates)
        {
            return ConfigFileUtils.DeleteTowerTemplates(ConfigFilePath, templates);
        }

        public bool DeleteProjectTowerTemplate(TowerTemplateStorageInfo template)
        {
            return ConfigFileUtils.DeleteTowerTemplates(ConfigFilePath, new List<TowerTemplateStorageInfo> { template }, false);
        }

        public bool DeleteProjectTowerTemplates(List<TowerTemplateStorageInfo> templates)
        {
            return ConfigFileUtils.DeleteTowerTemplates(ConfigFilePath, templates, false);
        }

        public bool UpdateGeneralTowerTemplateName(string oldName, string oldType, string newName, string newType)
        {
            return ConfigFileUtils.UpdateTowerTemplateName(ConfigFilePath, oldName, oldType, newName, newType, true);
        }

        public bool UpdateProjectTowerTemplateName(string oldName, string oldType, string newName, string newType)
        {
            return ConfigFileUtils.UpdateTowerTemplateName(ConfigFilePath, oldName, oldType, newName, newType, false);
        }

        public string GetGeneralTowerTemplatePath(string name, string towerType)
        {
            return ProjectPath + "\\" + ConstVar.DataBaseStr + "\\" + ConstVar.GeneralStruTemplateStr + "\\" + TowerCHENStringConvert.CH2EN(towerType) + "\\" + name + ".dat";
        }

        public string GetProjectlTowerTemplatePath(string name, string towerType)
        {
            return ProjectPath + "\\" + ConstVar.DataBaseStr + "\\" + ConstVar.ProjectStruTemplateStr + "\\" + TowerCHENStringConvert.CH2EN(towerType) + "\\" + name + ".dat";
        }



        #endregion
    }
}
