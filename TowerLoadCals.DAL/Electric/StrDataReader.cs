using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using TowerLoadCals.Common;
using TowerLoadCals.Mode;

namespace TowerLoadCals.DAL
{
    public class StrDataReader
    {
        public static List<StrDataCollection> Read(string path)
        {
            if (!File.Exists(path))
                return new List<StrDataCollection>();

            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            XmlNode rootNode = doc.GetElementsByTagName("Root")[0];
            if (rootNode == null)
                return new List<StrDataCollection>();

            List<StrDataCollection> list = new List<StrDataCollection>();

            foreach (XmlNode typeNode in rootNode.ChildNodes)
            {
                StrDataCollection collectionItem = new StrDataCollection()
                {
                    Type = typeNode.Attributes["Type"].Value.ToString(),
                    StrDatas = new List<StrData>()
                };

                foreach (XmlNode node in typeNode.ChildNodes)
                {
                    StrData fitData = new StrData();

                    if (node.Attributes["Name"] != null)
                        fitData.Name = node.Attributes["Name"].Value.ToString();
                    if (node.Attributes["StrType"] != null)
                        fitData.StrType = node.Attributes["StrType"].Value.ToString();
                    if (node.Attributes["Weight"] != null)
                        fitData.Weight = Convert.ToDecimal(node.Attributes["Weight"].Value.ToString());
                    if (node.Attributes["FitLength"] != null)
                        fitData.FitLength = Convert.ToDecimal(node.Attributes["FitLength"].Value.ToString());
                    if (node.Attributes["PieceLength"] != null)
                        fitData.PieceLength = Convert.ToDecimal(node.Attributes["PieceLength"].Value.ToString());
                    if (node.Attributes["PieceNum"] != null)
                        fitData.PieceNum = Convert.ToDecimal(node.Attributes["PieceNum"].Value.ToString());
                    if (node.Attributes["GoldPieceNum"] != null)
                        fitData.GoldPieceNum = Convert.ToDecimal(node.Attributes["GoldPieceNum"].Value.ToString());
                    if (node.Attributes["LNum"] != null)
                        fitData.LNum = Convert.ToDecimal(node.Attributes["LNum"].Value.ToString());
                    if (node.Attributes["DampLength"] != null)
                        fitData.DampLength = Convert.ToDecimal(node.Attributes["DampLength"].Value.ToString());
                    if (node.Attributes["SuTubleLen"] != null)
                        fitData.SuTubleLen = Convert.ToDecimal(node.Attributes["SuTubleLen"].Value.ToString());
                    if (node.Attributes["SoftLineLen"] != null)
                        fitData.SoftLineLen = Convert.ToDecimal(node.Attributes["SoftLineLen"].Value.ToString());

                    collectionItem.StrDatas.Add(fitData);
                }
                list.Add(collectionItem);
            }

            return list;
        }

        public static void Save(string path, List<StrDataCollection> infos)
        {
            if (File.Exists(path))
                File.Delete(path);

            XmlUtils.Save(path, infos);
        }

    }
}
