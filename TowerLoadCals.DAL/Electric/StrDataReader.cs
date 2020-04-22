using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TowerLoadCals.Common;
using TowerLoadCals.Mode;

namespace TowerLoadCals.DAL.Electric
{
    public class StrDataReader
    {
        public static List<StrData> Read(string path)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            XmlNode rootNode = doc.GetElementsByTagName("Root")[0];
            if (rootNode == null)
                return new List<StrData>();

            List<StrData> list = new List<StrData>();

            foreach (XmlNode node in rootNode.ChildNodes)
            {
                StrData str = new StrData()
                {
                    ID = Convert.ToInt16(node.Attributes["ID"].Value.ToString()),
                    Name = node.Attributes["Name"].Value.ToString(),
                    Type = node.Attributes["Type"].Value.ToString(),
                    Weight = Convert.ToInt16(node.Attributes["Weight"].Value.ToString()),
                    FitLength = Convert.ToInt16(node.Attributes["FitLength"].Value.ToString()),
                    PieceLength = Convert.ToInt16(node.Attributes["PieceLength"].Value.ToString()),
                    PieceNum = Convert.ToInt16(node.Attributes["PieceNum"].Value.ToString()),
                    GoldPieceNum = Convert.ToInt16(node.Attributes["GoldPieceNum"].Value.ToString()),
                    LNum = Convert.ToInt16(node.Attributes["LNum"].Value.ToString()),
                    DampLength = Convert.ToInt16(node.Attributes["DampLength"].Value.ToString()),
                    SuTubleLen = Convert.ToInt16(node.Attributes["SuTubleLen"].Value.ToString()),
                    SoftLineLen = Convert.ToInt16(node.Attributes["SoftLineLen"].Value.ToString()),
                };
                list.Add(str);
            }

            return list;
        }

        public static void Save(string path, List<StrData> infos)
        {
            XmlUtils.Save(path, infos);
        }

    }
}
