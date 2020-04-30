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

        public ProjectUtils()
        {
            
        }

        public bool CreateProject(out string msgStr)
        {
            var saveFileDialog = new OpenFileDialog()
            {
                Filter = "Lcp Files (*.lcp)|*.lcp"
            };

            var result = saveFileDialog.ShowDialog();

            if (result != true)
            {
                msgStr = "";
                return true;
            }

            String strDir = saveFileDialog.FileName.Substring(0, saveFileDialog.FileName.Length - 1 - saveFileDialog.SafeFileName.Length);

            string prejectName = saveFileDialog.SafeFileName.Substring(0, saveFileDialog.SafeFileName.Length - 4);

            System.IO.Directory.CreateDirectory(strDir);
            System.IO.Directory.CreateDirectory(strDir + "//" + prejectName);

            if (!CreateProjetcFile(strDir + "//" + prejectName + "//" + saveFileDialog.SafeFileName))
            {
                msgStr = "新建工程Lcp文件失败";
                return false;
            }

            msgStr = "";
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

    }
}
