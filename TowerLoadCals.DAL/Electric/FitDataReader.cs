using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TowerLoadCals.Common;
using TowerLoadCals.Mode.Electric;

namespace TowerLoadCals.DAL.Electric
{
    public class FitDataReader
    {
        public static List<FitData> Read(string path)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            XmlNode rootNode = doc.GetElementsByTagName("Root")[0];
            if (rootNode == null)
                return new List<FitData>();

            List<FitData> list = new List<FitData>();

            foreach (XmlNode node in rootNode.ChildNodes)
            {
                FitData str = new FitData()
                {
                    ID = Convert.ToInt16(node.Attributes["ID"].Value.ToString()),
                    Name = node.Attributes["Name"].Value.ToString(),
                    Type = node.Attributes["Type"].Value.ToString(),
                    Weight = Convert.ToInt16(node.Attributes["Weight"].Value.ToString()),
                    SecWind = Convert.ToInt16(node.Attributes["SecWind"].Value.ToString()),
                };
                list.Add(str);
            }

            return list;
        }

        public static void Save(string path, List<FitData> infos)
        {
            XmlUtils.Save(path, infos);
        }
    }
}
