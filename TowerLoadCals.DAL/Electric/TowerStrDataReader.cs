﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using TowerLoadCals.Common;
using TowerLoadCals.Mode;


namespace TowerLoadCals.DAL
{
    public class TowerStrDataReader
    {
        public static List<TowerStrCollection> Read(string path)
        {
            if (!File.Exists(path))
                return new List<TowerStrCollection>();

            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            XmlNode rootNode = doc.GetElementsByTagName("Root")[0];
            if (rootNode == null)
                return new List<TowerStrCollection>();

            List<TowerStrData> list = new List<TowerStrData>();

            List<TowerStrCollection> collectionList = new List<TowerStrCollection>();

            foreach (XmlNode colNode in rootNode.ChildNodes)
            {
                TowerStrCollection colItem = new TowerStrCollection()
                {
                    Name = colNode.Attributes["Name"].Value.ToString(),
                    Types = new List<TowerStrType>()
                };

                foreach(XmlNode typeNode in colNode.ChildNodes)
                {
                    TowerStrType typeItem = new TowerStrType()
                    {
                        Type = typeNode.Attributes["Type"].Value.ToString(),
                        TowerStrs = new List<TowerStrData>()
                    };

                    foreach(XmlNode node in typeNode.ChildNodes)
                    {
                        TowerStrData item = new TowerStrData();

                        if (node.Attributes["ID"] != null)
                            item.ID = Convert.ToInt32(node.Attributes["ID"].Value.ToString());
                        if (node.Attributes["Name"] != null)
                            item.Name = node.Attributes["Name"].Value.ToString();
                        if (node.Attributes["Type"] != null)
                            item.Type = int.Parse(node.Attributes["Type"].Value.ToString());
                        if (node.Attributes["TypeName"] != null)
                            item.TypeName = node.Attributes["TypeName"].Value.ToString();
                        if (node.Attributes["CirNum"] != null)
                            item.CirNum = Convert.ToInt32(node.Attributes["CirNum"].Value.ToString());
                        if (node.Attributes["CurType"] != null)
                            item.CurType = Convert.ToInt32(node.Attributes["CurType"].Value.ToString());
                        if (node.Attributes["CalHeight"] != null)
                            item.CalHeight = Convert.ToInt32(node.Attributes["CalHeight"].Value.ToString());
                        if (node.Attributes["MinHeight"] != null)
                            item.MinHeight = Convert.ToInt32(node.Attributes["MinHeight"].Value.ToString());
                        if (node.Attributes["MaxHeight"] != null)
                            item.MaxHeight = Convert.ToInt32(node.Attributes["MaxHeight"].Value.ToString());
                        if (node.Attributes["AllowedHorSpan"] != null)
                            item.AllowedHorSpan = Convert.ToInt32(node.Attributes["AllowedHorSpan"].Value.ToString());
                        if (node.Attributes["OneSideMinHorSpan"] != null)
                            item.OneSideMinHorSpan = Convert.ToInt32(node.Attributes["OneSideMinHorSpan"].Value.ToString());
                        if (node.Attributes["OneSideMaxHorSpan"] != null)
                            item.OneSideMaxHorSpan = Convert.ToInt32(node.Attributes["OneSideMaxHorSpan"].Value.ToString());
                        if (node.Attributes["AllowedVerSpan"] != null)
                            item.AllowedVerSpan = Convert.ToInt32(node.Attributes["AllowedVerSpan"].Value.ToString());
                        if (node.Attributes["OneSideMinVerSpan"] != null)
                            item.OneSideMinVerSpan = Convert.ToInt32(node.Attributes["OneSideMinVerSpan"].Value.ToString());
                        if (node.Attributes["OneSideMaxVerSpan"] != null)
                            item.OneSideMaxVerSpan = Convert.ToInt32(node.Attributes["OneSideMaxVerSpan"].Value.ToString());
                        if (node.Attributes["OneSideUpVerSpanMin"] != null)
                            item.OneSideUpVerSpanMin = Convert.ToInt32(node.Attributes["OneSideUpVerSpanMin"].Value.ToString());
                        if (node.Attributes["OneSideUpVerSpanMax"] != null)
                            item.OneSideUpVerSpanMax = Convert.ToInt32(node.Attributes["OneSideUpVerSpanMax"].Value.ToString());
                        if (node.Attributes["MinAngel"] != null)
                            item.MinAngel = Convert.ToInt32(node.Attributes["MinAngel"].Value.ToString());
                        if (node.Attributes["MaxAngel"] != null)
                            item.MaxAngel = Convert.ToInt32(node.Attributes["MaxAngel"].Value.ToString());
                        if (node.Attributes["DRepresentSpanMin"] != null)
                            item.DRepresentSpanMin = Convert.ToInt32(node.Attributes["DRepresentSpanMin"].Value.ToString());
                        if (node.Attributes["DRepresentSpanMax"] != null)
                            item.DRepresentSpanMax = Convert.ToInt32(node.Attributes["DRepresentSpanMax"].Value.ToString());
                        if (node.Attributes["StrHeightSer"] != null)
                            item.StrHeightSer = node.Attributes["StrHeightSer"].Value.ToString();
                        if (node.Attributes["StrAllowHorSpan"] != null)
                            item.StrAllowHorSpan = node.Attributes["StrAllowHorSpan"].Value.ToString();
                        if (node.Attributes["AngelToHorSpan"] != null)
                            item.AngelToHorSpan = Convert.ToInt32(node.Attributes["AngelToHorSpan"].Value.ToString());
                        if (node.Attributes["MaxAngHorSpan"] != null)
                            item.MaxAngHorSpan = Convert.ToInt32(node.Attributes["MaxAngHorSpan"].Value.ToString());

                        typeItem.TowerStrs.Add(item);
                    }

                    colItem.Types.Add(typeItem);

                }

                collectionList.Add(colItem);
            }

            return collectionList; 
        }

        public static void Save(string path, List<TowerStrCollection> infos)
        {
            if (File.Exists(path))
                File.Delete(path);

            XmlUtils.Save(path, infos);
        }


        /// <summary>
        /// 读取本地文件值
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<TowerStrData> ReadLoadFile(string path)
        {
            if (!File.Exists(path))
                return new List<TowerStrData>();

            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            XmlNode rootNode = doc.GetElementsByTagName("Root")[0];
            if (rootNode == null)
                return new List<TowerStrData>();

            List<TowerStrData> list = new List<TowerStrData>();

            foreach (XmlNode node in rootNode.ChildNodes)
            {
                TowerStrData item = new TowerStrData();

                if (node.Attributes["ID"] != null)
                    item.ID = Convert.ToInt32(node.Attributes["ID"].Value.ToString());
                if (node.Attributes["Name"] != null)
                    item.Name = node.Attributes["Name"].Value.ToString();
                if (node.Attributes["VoltageLevel"] != null)
                    item.VoltageLevel = Convert.ToInt32(node.Attributes["VoltageLevel"].Value.ToString());
                if (node.Attributes["Type"] != null)
                    item.Type = Convert.ToInt32(node.Attributes["Type"].Value.ToString());
                if (node.Attributes["TypeName"] != null)
                    item.TypeName =node.Attributes["TypeName"].Value.ToString();
                if (node.Attributes["CirNum"] != null)
                    item.CirNum = Convert.ToInt32(node.Attributes["CirNum"].Value.ToString());
                if (node.Attributes["CurType"] != null)
                    item.CurType = Convert.ToInt32(node.Attributes["CurType"].Value.ToString());
                if (node.Attributes["CalHeight"] != null)
                    item.CalHeight = Convert.ToInt32(node.Attributes["CalHeight"].Value.ToString());
                if (node.Attributes["MinHeight"] != null)
                    item.MinHeight = Convert.ToInt32(node.Attributes["MinHeight"].Value.ToString());
                if (node.Attributes["MaxHeight"] != null)
                    item.MaxHeight = Convert.ToInt32(node.Attributes["MaxHeight"].Value.ToString());
                if (node.Attributes["AllowedHorSpan"] != null)
                    item.AllowedHorSpan = Convert.ToInt32(node.Attributes["AllowedHorSpan"].Value.ToString());
                if (node.Attributes["OneSideMinHorSpan"] != null)
                    item.OneSideMinHorSpan = Convert.ToInt32(node.Attributes["OneSideMinHorSpan"].Value.ToString());
                if (node.Attributes["OneSideMaxHorSpan"] != null)
                    item.OneSideMaxHorSpan = Convert.ToInt32(node.Attributes["OneSideMaxHorSpan"].Value.ToString());
                if (node.Attributes["AllowedVerSpan"] != null)
                    item.AllowedVerSpan = Convert.ToInt32(node.Attributes["AllowedVerSpan"].Value.ToString());
                if (node.Attributes["OneSideMinVerSpan"] != null)
                    item.OneSideMinVerSpan = Convert.ToInt32(node.Attributes["OneSideMinVerSpan"].Value.ToString());
                if (node.Attributes["OneSideMaxVerSpan"] != null)
                    item.OneSideMaxVerSpan = Convert.ToInt32(node.Attributes["OneSideMaxVerSpan"].Value.ToString());
                if (node.Attributes["OneSideUpVerSpanMin"] != null)
                    item.OneSideUpVerSpanMin = Convert.ToInt32(node.Attributes["OneSideUpVerSpanMin"].Value.ToString());
                if (node.Attributes["OneSideUpVerSpanMax"] != null)
                    item.OneSideUpVerSpanMax = Convert.ToInt32(node.Attributes["OneSideUpVerSpanMax"].Value.ToString());
                if (node.Attributes["MinAngel"] != null)
                    item.MinAngel = Convert.ToInt32(node.Attributes["MinAngel"].Value.ToString());
                if (node.Attributes["MaxAngel"] != null)
                    item.MaxAngel = Convert.ToInt32(node.Attributes["MaxAngel"].Value.ToString());
                if (node.Attributes["DRepresentSpanMin"] != null)
                    item.DRepresentSpanMin = Convert.ToInt32(node.Attributes["DRepresentSpanMin"].Value.ToString());
                if (node.Attributes["DRepresentSpanMax"] != null)
                    item.DRepresentSpanMax = Convert.ToInt32(node.Attributes["DRepresentSpanMax"].Value.ToString());
                if (node.Attributes["StrHeightSer"] != null)
                    item.StrHeightSer = node.Attributes["StrHeightSer"].Value.ToString();
                if (node.Attributes["StrAllowHorSpan"] != null)
                    item.StrAllowHorSpan = node.Attributes["StrAllowHorSpan"].Value.ToString();
                if (node.Attributes["AngelToHorSpan"] != null)
                    item.AngelToHorSpan = Convert.ToInt32(node.Attributes["AngelToHorSpan"].Value.ToString());
                if (node.Attributes["MaxAngHorSpan"] != null)
                    item.MaxAngHorSpan = Convert.ToInt32(node.Attributes["MaxAngHorSpan"].Value.ToString());

                if (node.Attributes["UpSideInHei"] != null)
                    item.UpSideInHei = Convert.ToDouble(node.Attributes["UpSideInHei"].Value.ToString());
                if (node.Attributes["MidInHei"] != null)
                    item.MidInHei = Convert.ToDouble(node.Attributes["MidInHei"].Value.ToString());
                if (node.Attributes["DnSideInHei"] != null)
                    item.DnSideInHei = Convert.ToDouble(node.Attributes["DnSideInHei"].Value.ToString());
                if (node.Attributes["GrDHei"] != null)
                    item.GrDHei = Convert.ToDouble(node.Attributes["GrDHei"].Value.ToString());
                if (node.Attributes["UpSideJuHei"] != null)
                    item.UpSideJuHei = Convert.ToDouble(node.Attributes["UpSideJuHei"].Value.ToString());
                if (node.Attributes["MidJuHei"] != null)
                    item.MidJuHei = Convert.ToDouble(node.Attributes["MidJuHei"].Value.ToString());
                if (node.Attributes["DnSideJuHei"] != null)
                    item.DnSideJuHei = Convert.ToDouble(node.Attributes["DnSideJuHei"].Value.ToString());

                if (node.Attributes["TempletName"] != null)//结构计算模板
                    item.TempletName = node.Attributes["TempletName"].Value.ToString();
                if (node.Attributes["ModelName"] != null)//结构计算模型
                    item.ModelName = node.Attributes["ModelName"].Value.ToString();
                if (node.Attributes["ModelFileExtension"] != null)//结构计算模型 扩展名 年月日
                    item.ModelFileExtension = node.Attributes["ModelFileExtension"].Value.ToString();
                if (node.Attributes["HangPointName"] != null)//挂点文件
                    item.HangPointName =node.Attributes["HangPointName"].Value.ToString();
                if (node.Attributes["HangPointFileExtension"] != null)//挂点文件 扩展名 年月日
                    item.HangPointFileExtension =node.Attributes["HangPointFileExtension"].Value.ToString();

                list.Add(item);
            }

            return list;
        }


        public static bool SaveLocalFile(string filePath, List<TowerStrData> towerStrDatas, out string warning)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(filePath);

                XmlNode rootNode = doc.GetElementsByTagName("Root")[0];
                rootNode.RemoveAll();//移除所有节点，全部新增

                foreach (TowerStrData item in towerStrDatas)
                {
                    XmlElement row = doc.CreateElement("Tower");
                    row.SetAttribute("ID", item.ID.ToString());
                    row.SetAttribute("Name", item.Name == null ? "" : item.Name);
                    row.SetAttribute("Type", item.Type.ToString());
                    row.SetAttribute("TypeName", item.TypeName.ToString());
                    row.SetAttribute("VoltageLevel", item.VoltageLevel.ToString());
                    row.SetAttribute("CirNum", item.CirNum.ToString());
                    row.SetAttribute("CurType", item.CurType.ToString());
                    row.SetAttribute("MinAngel", item.MinAngel.ToString());
                    row.SetAttribute("MaxAngel", item.MaxAngel.ToString());
                    row.SetAttribute("CalHeight", item.CalHeight.ToString());
                    row.SetAttribute("MinHeight", item.MinHeight.ToString());
                    row.SetAttribute("MaxHeight", item.MaxHeight.ToString());
                    row.SetAttribute("AllowedHorSpan", item.AllowedHorSpan.ToString());
                    row.SetAttribute("OneSideMinHorSpan", item.OneSideMinHorSpan.ToString());
                    row.SetAttribute("OneSideMaxHorSpan", item.OneSideMaxHorSpan.ToString());
                    row.SetAttribute("AllowedVerSpan", item.AllowedVerSpan.ToString());
                    row.SetAttribute("OneSideMinVerSpan", item.OneSideMinVerSpan.ToString());
                    row.SetAttribute("OneSideMaxVerSpan", item.OneSideMaxVerSpan.ToString());
                    row.SetAttribute("OneSideUpVerSpanMin", item.OneSideUpVerSpanMin.ToString());
                    row.SetAttribute("OneSideUpVerSpanMax", item.OneSideUpVerSpanMax.ToString());
                    row.SetAttribute("DRepresentSpanMin", item.DRepresentSpanMin.ToString());
                    row.SetAttribute("DRepresentSpanMax", item.DRepresentSpanMax.ToString());
                    row.SetAttribute("StrHeightSer", item.StrHeightSer == null ? "" : item.StrHeightSer.ToString());
                    row.SetAttribute("StrAllowHorSpan", item.StrAllowHorSpan == null ? "" : item.StrAllowHorSpan.ToString());
                    row.SetAttribute("AngelToHorSpan", item.AngelToHorSpan.ToString());
                    row.SetAttribute("MaxAngHorSpan", item.MaxAngHorSpan.ToString());

                    row.SetAttribute("UpSideInHei", item.UpSideInHei.ToString());
                    row.SetAttribute("MidInHei", item.MidInHei.ToString());
                    row.SetAttribute("DnSideInHei", item.DnSideInHei.ToString());
                    row.SetAttribute("GrDHei", item.GrDHei.ToString());
                    row.SetAttribute("UpSideJuHei", item.UpSideJuHei.ToString());
                    row.SetAttribute("MidJuHei", item.MidJuHei.ToString());
                    row.SetAttribute("DnSideJuHei", item.DnSideJuHei.ToString());

                    rootNode.AppendChild(row);

                }
                doc.Save(filePath);

                warning = "";
                return true;
            }
            catch (Exception ex)
            {
                warning = ex.Message;
                return false;
            }
        }
    }
}
