using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TowerLoadCals.Mode;

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

            XmlNode nodeBaseData = doc.CreateElement(ConstVar.DataBaseStr);
            xmlNode.AppendChild(nodeBaseData);

            XmlNode generalTemplateNode = doc.CreateElement(ConstVar.GeneralStruTemplateStr);
            nodeBaseData.AppendChild(generalTemplateNode);

            XmlNode projectTemplateNode = doc.CreateElement(ConstVar.ProjectStruTemplateStr);
            nodeBaseData.AppendChild(projectTemplateNode);

            XmlNode nodeStruCals = doc.CreateElement(ConstVar.StruCalsStr);
            xmlNode.AppendChild(nodeStruCals);

            doc.Save(path);

            return true;
        }

        #region 结构计算塔位信息操作函数
        public static List<string> GetAllStrucTowerNames(string path)
        {
            List<string> rstList = new List<string>();

            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            XmlNode struCalsNode = doc.GetElementsByTagName(ConstVar.StruCalsStr)[0];
            if (struCalsNode == null)
                return rstList;

            foreach (XmlNode towerNode in struCalsNode.ChildNodes)
            {
                rstList.Add(towerNode.Attributes["Name"].Value.ToString());
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
        #endregion


        #region 结构计算塔库模板操作函数
        public static List<TowerTemplateStorageInfo> GetAllTowerTemplates(string path, bool isGeneralTemplate = true)
        {
            List<TowerTemplateStorageInfo> rstList = new List<TowerTemplateStorageInfo>();

            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            XmlNode templateNode = doc.GetElementsByTagName(isGeneralTemplate ? ConstVar.GeneralStruTemplateStr : ConstVar.ProjectStruTemplateStr)[0];
            if (templateNode == null)
                return rstList;

            foreach (XmlNode subNode in templateNode.ChildNodes)
            {
                rstList.Add( new TowerTemplateStorageInfo() {
                    Name = subNode.Attributes["Name"].Value.ToString(),
                    TowerType = subNode.Attributes[ConstVar.TowerTypeStr].Value.ToString(),
                });
            }

            return rstList;
        }

        public static bool InsertTowerTemplates(string path, List<TowerTemplateStorageInfo> templates, bool isGeneralTemplate = true)
        {
            List<string> rstList = new List<string>();

            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(path);

                XmlNode templatesNode = doc.GetElementsByTagName(isGeneralTemplate ? ConstVar.GeneralStruTemplateStr : ConstVar.ProjectStruTemplateStr)[0];
                if (templatesNode == null)
                    return false;

                foreach (var item in templates)
                {
                    XmlAttribute nameAttribute = doc.CreateAttribute("Name");
                    nameAttribute.Value = item.Name;

                    XmlAttribute towerTypeAttribute = doc.CreateAttribute(ConstVar.TowerTypeStr);
                    towerTypeAttribute.Value = item.TowerType;

                    XmlNode templateNode = doc.CreateElement(ConstVar.TowerTemplateStr);
                    templateNode.Attributes.Append(nameAttribute);
                    templateNode.Attributes.Append(towerTypeAttribute);

                    templatesNode.AppendChild(templateNode);
                }

                doc.Save(path);
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public static bool DeleteTowerTemplates(string path, List<TowerTemplateStorageInfo> templates, bool isGeneralTemplate = true)
        {
            List<string> rstList = new List<string>();

            XmlDocument doc = new XmlDocument();

            try
            {
                doc.Load(path);

                XmlNode templatesNode = doc.GetElementsByTagName(isGeneralTemplate ? ConstVar.GeneralStruTemplateStr : ConstVar.ProjectStruTemplateStr)[0];
                if (templatesNode == null)
                    return false;

                foreach (XmlNode subNode in templatesNode.ChildNodes)
                {
                    if (subNode.Attributes["Name"] != null && templates.Where(item => item.Name == subNode.Attributes["Name"].Value.ToString()).Count() > 0)
                    {
                        templatesNode.RemoveChild(subNode);
                    }
                }

                doc.Save(path);
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public static bool UpdateTowerTemplateName(string path, string oldName, string oldType, string newName, string newType, bool isGeneralTemplate = true)
        {
            List<string> rstList = new List<string>();

            XmlDocument doc = new XmlDocument();

            try
            {
                doc.Load(path);

                XmlNode templatesNode = doc.GetElementsByTagName(isGeneralTemplate ? ConstVar.GeneralStruTemplateStr : ConstVar.ProjectStruTemplateStr)[0];
                if (templatesNode == null)
                    return false;

                foreach (XmlNode subNode in templatesNode.ChildNodes)
                {
                    if (subNode.Attributes["Name"] != null && subNode.Attributes["Name"].Value.ToString() == oldName 
                        && subNode.Attributes["TowerType"] != null && subNode.Attributes["TowerType"].Value.ToString() == oldType)
                    { 
                        XmlElement subXe = (XmlElement)subNode;
                        subXe.SetAttribute("Name", newName);
                        subXe.SetAttribute("TowerType", newType);
                        break;
                    }
                }

                doc.Save(path);
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
        #endregion

    }
}
