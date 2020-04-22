using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TowerLoadCals.Common;
using TowerLoadCals.Mode;

namespace TowerLoadCals.DAL
{
    public class ProInfoReader
    {
        public static List<ProInfo> Read(string path)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            XmlNode rootNode = doc.GetElementsByTagName("Root")[0];
            if (rootNode == null)
                return new List<ProInfo>();

            List<ProInfo> list = new List<ProInfo>();

            foreach (XmlNode node in rootNode.ChildNodes)
            {
                ProInfo info = new ProInfo()
                {
                    Name = node.Attributes["Name"].Value.ToString(),
                    Volt = Convert.ToInt16(node.Attributes["Volt"].Value.ToString()),
                    ID = Convert.ToInt16(node.Attributes["ID"].Value.ToString()),
                    Stage = node.Attributes["Stage"].Value.ToString(),
                    FilesID = Convert.ToInt16(node.Attributes["FilesID"].Value.ToString())
                };
                list.Add(info);
            }

            return list;
        }

        public static void Save(string path,  List<ProInfo> infos)
        {
            XmlUtils.Save(path, infos);
        }

    }
}
