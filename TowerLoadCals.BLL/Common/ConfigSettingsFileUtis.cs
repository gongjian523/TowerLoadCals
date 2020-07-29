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
    /// 此类用来对ConfigSettings.xml文件进行读写操作
    /// </summary>
    public class ConfigSettingsFileUtis
    {
        public static string GetSmartTowerSetting(string path, string att)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            XmlNode smartTowerNode = doc.GetElementsByTagName(ConstVar.SmartTowerStr)[0];
            if (smartTowerNode == null)
                return "";

            return smartTowerNode.Attributes[att].Value.ToString();
        }

        public static void SaveSmartTowerSetting(string path, string att, string value)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            XmlElement smartTowerNode = (XmlElement)doc.GetElementsByTagName(ConstVar.SmartTowerStr)[0];
            if (smartTowerNode == null)
                return ;

            smartTowerNode.SetAttribute(att, value);

            doc.Save(path);
        }
    }
}
