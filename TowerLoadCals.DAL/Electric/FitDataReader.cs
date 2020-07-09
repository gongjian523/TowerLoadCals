using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using TowerLoadCals.Common;
using TowerLoadCals.Mode;

namespace TowerLoadCals.DAL
{
    public  class FitDataReader
    {
        public static List<FitDataCollection> Read(string path)
        {
            if (!File.Exists(path))
                return new List<FitDataCollection>();

            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            XmlNode rootNode = doc.GetElementsByTagName("Root")[0];
            if (rootNode == null)
                return new List<FitDataCollection>();

            List<FitDataCollection> list = new List<FitDataCollection>();

            foreach (XmlNode typeNode in rootNode.ChildNodes)
            {
                FitDataCollection collectionItem = new FitDataCollection()
                {
                    Type = typeNode.Attributes["Type"].Value.ToString(),
                    FitDatas = new List<FitData>()
                };

                foreach (XmlNode node in typeNode.ChildNodes)
                {
                    FitData fitData = new FitData();

                    if (node.Attributes["Model"] != null)
                        fitData.Model = node.Attributes["Model"].Value.ToString();
                    if (node.Attributes["Name"] != null)
                        fitData.Name = node.Attributes["Name"].Value.ToString();
                    if (node.Attributes["Weight"] != null)
                        fitData.Weight = Convert.ToInt16(node.Attributes["Weight"].Value.ToString());
                    if (node.Attributes["Voltage"] != null)
                        fitData.Voltage = Convert.ToInt16(node.Attributes["Voltage"].Value.ToString());
                    if (node.Attributes["SecWind"] != null)
                        fitData.SecWind = Convert.ToInt16(node.Attributes["SecWind"].Value.ToString());
                 
                    collectionItem.FitDatas.Add(fitData);
                }

                list.Add(collectionItem);
            }

            return list;
        }

        public static void Save(string path, List<FitDataCollection> infos)
        {
            if (File.Exists(path))
                File.Delete(path);

            XmlUtils.Save(path, infos);
        }
    }
}
