using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace TowerLoadCals.Readers
{
    public class TaStructureReader
    {
        public static TaStructure Read(string path)
        {
            TaStructure taStructure = new TaStructure();

            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            //解析XML节点
            XmlNode xmlNode = doc.GetElementsByTagName("XML")[0];
            if (xmlNode == null)
                return null;

            taStructure.Name = xmlNode.Attributes["Name"].Value.ToString();
            taStructure.CircuitNum  = Convert.ToInt16(xmlNode.Attributes["Circuit"].Value.ToString());
            taStructure.Type = Convert.ToInt16(xmlNode.Attributes["Type"].Value.ToString());
            taStructure.Category = xmlNode.Attributes["Category"].Value.ToString();
            taStructure.AppearanceType = xmlNode.Attributes["AppearanceType"].Value.ToString();

            //解析CircuitSet节点
            XmlNode circuitSetNode = doc.GetElementsByTagName("CircuitSet")[0];
            if (circuitSetNode == null)
                return null;

            taStructure.CircuitSet = new List<Circuit>();
            foreach (XmlNode node in circuitSetNode.ChildNodes)
            {
                Circuit cs = new Circuit();
                cs.Name = node.Attributes["Name"].Value.ToString();
                cs.Id = Convert.ToInt16(node.Attributes["Id"].Value.ToString());



            }








            return taStructure;
        }
        
    }
}