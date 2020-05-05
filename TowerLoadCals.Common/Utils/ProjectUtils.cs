using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;

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
                Filter = "Lcp Files (*.lcp)|*.lcp"
            };

            var result = saveFileDialog.ShowDialog();

            if (result != true)
                return false;

            String strDir = saveFileDialog.FileName.Substring(0, saveFileDialog.FileName.Length - 1 - saveFileDialog.SafeFileName.Length);
            string prejectName = saveFileDialog.SafeFileName.Substring(0, saveFileDialog.SafeFileName.Length - 4);

            System.IO.Directory.CreateDirectory(strDir);
            System.IO.Directory.CreateDirectory(strDir + "//" + prejectName);

            if (!CreateProjetcFile(strDir + "//" + prejectName + "//" + saveFileDialog.SafeFileName))
            {
                MessageBox.Show("新建工程Lcp文件失败");
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

    }
}
