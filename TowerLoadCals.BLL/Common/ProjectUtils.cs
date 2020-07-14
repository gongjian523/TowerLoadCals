using Microsoft.Win32;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using TowerLoadCals.DAL;
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
        public static bool NewStruCalsTower(string towerName, string towerType, string templatePath, string electricalLoadFilePath)
        {
            var struCalsParas = GlobalInfo.GetInstance().StruCalsParas;
            
            if(struCalsParas.Where(item=>item.TowerName == towerName).Count() > 0)
            {
                System.Windows.Forms.MessageBox.Show(towerName + "已经存在！");
                return false;
            }

            StruCalsParasCompose paras = new StruCalsParasCompose(towerName, towerType, templatePath, electricalLoadFilePath, out string decodeTemplateStr);

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
            string dirPath = ProjectPath + "\\" + ConstVar.StruCalsStr + "\\" + name + "\\";

            string electricalLaodFilePath = dirPath + ConstVar.StruCalsElecLoadFileName;

            string parasSavedFilePath = dirPath + ConstVar.StruCalsParasFileName;
            StruCalsParasCompose temp = XmlUtils.Deserializer<StruCalsParasCompose>(parasSavedFilePath);

            if (temp == null || temp == default(StruCalsParasCompose))
                return;


            string templatePath = dirPath + temp.TemplateName;

            StruCalsParasCompose paras = new StruCalsParasCompose(name, electricalLaodFilePath, templatePath, temp);

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
                string dirPath = ProjectPath + "\\" + ConstVar.StruCalsStr + "\\" + item.TowerName;

                //配置文件中没有保存新建的塔位：
                //1 把模板和电气荷载文件复制到工程文件下
                //2 把塔位加入到配置文件中
                if (!savedToewer.Contains(item.TowerName))
                {
                    string templatePath = dirPath + "\\" + item.TemplateName;
                    string elecLoadFilePath = dirPath + "\\" + ConstVar.StruCalsElecLoadFileName;

                    if (!Directory.Exists(dirPath))
                        Directory.CreateDirectory(dirPath);

                    if (File.Exists(templatePath))
                        File.Delete(templatePath);
                    File.Copy(item.TemplatePath, templatePath);
                    if (File.Exists(elecLoadFilePath))
                        File.Delete(elecLoadFilePath);
                    File.Copy(item.ElectricalLoadFilePath, elecLoadFilePath);

                    InsertStrucTowerName(item.TowerName);
                }

                XmlUtils.Serializer(dirPath + "\\" + ConstVar.StruCalsParasFileName, item);
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

        #endregion
    }
}
