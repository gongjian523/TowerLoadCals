using Microsoft.Win32;
using System;
using System.Linq;
using System.Xml;
using TowerLoadCals.DAL;

namespace TowerLoadCals.Common.Utils
{
    public class ProjectUtils
    {
        public string ProjectPath { get; set; }

        protected GlobalInfo globalInfo;

        public ProjectUtils()
        {
            globalInfo = GlobalInfo.GetInstance();
        }

        public bool CreateProject()
        {
            var saveFileDialog = new SaveFileDialog()
            {
                Filter = "Dat Files (*.dat)|*.dat",
            };

            var result = saveFileDialog.ShowDialog();

            if (result != true)
                return false;

            String strDir = saveFileDialog.FileName.Substring(0, saveFileDialog.FileName.Length - 1 - saveFileDialog.SafeFileName.Length);
            string prejectName = saveFileDialog.SafeFileName.Substring(0, saveFileDialog.SafeFileName.Length - 4);

            System.IO.Directory.CreateDirectory(strDir);
            System.IO.Directory.CreateDirectory(strDir + "//" + prejectName);
            System.IO.Directory.CreateDirectory(strDir + "//" + prejectName + "//BaseData");
            System.IO.Directory.CreateDirectory(strDir + "//" + prejectName + "//StruCals");

            if (!CreateProjetcFile(strDir + "//" + prejectName + "//" + saveFileDialog.SafeFileName))
            {
                System.Windows.Forms.MessageBox.Show("新建工程Lcp文件失败");
                return false;
            }

            globalInfo.ProjectPath = strDir + "//" + prejectName;
            globalInfo.ProjectName = prejectName;

            return true;
        }

        protected bool CreateProjetcFile(string path)
        {
            XmlDocument doc = new XmlDocument();

            XmlNode decNode = doc.CreateXmlDeclaration("1.0", "UTF-8", "");
            doc.AppendChild(decNode);

            XmlNode xmlNode = doc.CreateElement("Project");
            doc.AppendChild(xmlNode);

            XmlNode nodeBaseData = doc.CreateElement("BaseData");
            xmlNode.AppendChild(nodeBaseData);

            XmlNode nodeStruCals = doc.CreateElement("StruCals");
            xmlNode.AppendChild(nodeStruCals);

            doc.Save(path);

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

            String strDir = openFileDialog.FileName.Substring(0, openFileDialog.FileName.Length - 1 - openFileDialog.SafeFileName.Length);

            string prejectName = openFileDialog.SafeFileName.Substring(0, openFileDialog.SafeFileName.Length - 4);

            globalInfo.ProjectPath = strDir;
            globalInfo.ProjectName = prejectName;

            return true;
        }

        public static bool NewStruCalsTower(string towerName, string towerType, string templatePath, string electricalLoadFilePath)
        {
            var struCalsParas = GlobalInfo.GetInstance().StruCalsParas;
            
            if(struCalsParas.Where(item=>item.TowerName == towerName).Count() > 0)
            {
                System.Windows.Forms.MessageBox.Show(towerName + "已经存在！");
                return false;
            }

            StruCalsParas paras = new StruCalsParas(towerName, towerType, templatePath, electricalLoadFilePath, out string decodeTemplateStr);

            if(decodeTemplateStr != "")
            {
                System.Windows.Forms.MessageBox.Show(towerName + decodeTemplateStr);
                return false;
            }

            struCalsParas.Add(paras);

            return true;
        }

    }
}
