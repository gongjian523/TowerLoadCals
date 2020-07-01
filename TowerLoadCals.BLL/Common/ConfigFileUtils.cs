using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TowerLoadCals.BLL
{
    /// <summary>
    /// 此类用来对lcp文件进行读写操作
    /// </summary>
    public class ConfigFileUtils
    {
        public static bool CreateProjetcFile(string path)
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

        public static List<string> GetAllStrucTowerNames(string path)
        {
            List<string> rstList = new List<string>();

            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            XmlNode struCalsNode = doc.GetElementsByTagName("StruCals")[0];
            if (struCalsNode == null)
                return rstList;

            foreach (XmlNode towerNode in struCalsNode.ChildNodes)
            {
                rstList.Add(towerNode.Name);
            }

            return rstList;
        }

        public static bool InsertStrucTowerNames(string path, List<string> towerNames)
        {
            List<string> rstList = new List<string>();

            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(path);

                XmlNode struCalsNode = doc.GetElementsByTagName("StruCals")[0];
                if (struCalsNode == null)
                    return false;

                foreach (string tower in towerNames)
                {
                    XmlAttribute nameAttribute = doc.CreateAttribute("Name");
                    nameAttribute.Value = tower;

                    XmlNode towerNode = doc.CreateElement("Tower");
                    towerNode.Attributes.Append(nameAttribute);

                    struCalsNode.AppendChild(towerNode);
                }

                doc.Save(path);
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public static bool DeleteStrucTowerNames(string path, List<string> towerNames)
        {
            List<string> rstList = new List<string>();

            XmlDocument doc = new XmlDocument();

            try
            {
                doc.Load(path);

                XmlNode struCalsNode = doc.GetElementsByTagName("StruCals")[0];
                if (struCalsNode == null)
                    return false;

                foreach (XmlNode subNode in struCalsNode.ChildNodes)
                {
                    if(subNode.Attributes["Name"] != null && towerNames.Contains(subNode.Attributes["Name"].Value.ToString()))
                    {
                        struCalsNode.RemoveChild(subNode);
                    }
                }

                doc.Save(path);
            }
            catch(Exception ex)
            {
                return false;
            }

            return true;
        }

    }
}
