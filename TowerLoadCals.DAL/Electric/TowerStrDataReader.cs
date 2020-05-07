using System;
using System.Collections.Generic;
using System.Xml;
using TowerLoadCals.Common;
using TowerLoadCals.Mode.Electric;

namespace TowerLoadCals.DAL
{
    public class TowerStrDataReader
    {
        public static List<TowerStrData> Read(string path)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            XmlNode rootNode = doc.GetElementsByTagName("Root")[0];
            if (rootNode == null)
                return new List<TowerStrData>();

            List<TowerStrData> list = new List<TowerStrData>();

            foreach (XmlNode node in rootNode.ChildNodes)
            {
                TowerStrData strData = new TowerStrData()
                {
                    ID = Convert.ToInt16(node.Attributes["ID"].Value.ToString()),
                    Name = node.Attributes["Name"].Value.ToString(),
                    Type = Convert.ToInt16(node.Attributes["Type"].Value.ToString()),
                    CirNum = Convert.ToInt16(node.Attributes["CirNum"].Value.ToString()),
                    CurType = Convert.ToInt16(node.Attributes["CurType"].Value.ToString()),
                    CalHeight = Convert.ToInt16(node.Attributes["CalHeight"].Value.ToString()),
                    MinHeight = Convert.ToInt16(node.Attributes["MinHeight"].Value.ToString()),
                    MaxHeight = Convert.ToInt16(node.Attributes["MaxHeight"].Value.ToString()),
                    AllowedHorSpan = Convert.ToInt16(node.Attributes["AllowedHorSpan"].Value.ToString()),
                    OneSideMinHorSpan = Convert.ToInt16(node.Attributes["OneSideMinHorSpan"].Value.ToString()),
                    OneSideMaxHorSpan = Convert.ToInt16(node.Attributes["OneSideMaxHorSpan"].Value.ToString()),
                    AllowedVerSpan = Convert.ToInt16(node.Attributes["AllowedVerSpan"].Value.ToString()),
                    OneSideMinVerSpan = Convert.ToInt16(node.Attributes["OneSideMinVerSpan"].Value.ToString()),
                    OneSideMaxVerSpan = Convert.ToInt16(node.Attributes["OneSideMaxVerSpan"].Value.ToString()),
                    OneSideUpVerSpanMin = Convert.ToInt16(node.Attributes["OneSideUpVerSpanMin"].Value.ToString()),
                    OneSideUpVerSpanMax = Convert.ToInt16(node.Attributes["OneSideUpVerSpanMax"].Value.ToString()),
                    MinAngel = Convert.ToInt16(node.Attributes["MinAngel"].Value.ToString()),
                    MaxAngel = Convert.ToInt16(node.Attributes["MaxAngel"].Value.ToString()),
                    DRepresentSpanMin = Convert.ToInt16(node.Attributes["DRepresentSpanMin"].Value.ToString()),
                    DRepresentSpanMax = Convert.ToInt16(node.Attributes["DRepresentSpanMax"].Value.ToString()),
                    StrHeightSer = node.Attributes["StrHeightSer"].Value.ToString(),
                    StrAllowHorSpan = node.Attributes["StrAllowHorSpan"].Value.ToString(),
                    AngelToHorSpan = Convert.ToInt16(node.Attributes["AngelToHorSpan"].Value.ToString()),
                    MaxAngHorSpan = Convert.ToInt16(node.Attributes["MaxAngHorSpan"].Value.ToString()),
                };
                list.Add(strData);
            }

            return list;
        }

        public static void Save(string path, List<TowerStrData> infos)
        {
            XmlUtils.Save(path, infos);
        }
    }
}
