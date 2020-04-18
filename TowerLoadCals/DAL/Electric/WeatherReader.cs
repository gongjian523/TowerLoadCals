using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using TowerLoadCals.Mode;
using TowerLoadCals.Utils;

namespace TowerLoadCals.Readers
{
    public abstract class WeatherReader
    {
        public abstract List<Weather> Read(string path);
        public abstract List<Weather> Read(string dir, string tableName);

        public abstract List<Weather> ReadLocal(string path);
        public abstract void Save(string path, List<Weather> weathers);
    }

    public class WeatherXmlReader : WeatherReader
    {
        public override List<Weather> Read(string path)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            XmlNode rootNode = doc.FirstChild;
            if (rootNode == null)
                return new List<Weather>();

            XmlNode weatherNameTitleNode = rootNode.FirstChild;
            if (weatherNameTitleNode == null)
                return new List<Weather>();

            List<Weather> list = new List<Weather>();

            foreach (XmlNode node in weatherNameTitleNode.ChildNodes)
            {
                List<WorkCondition> listWd = new List<WorkCondition>();
                foreach (XmlNode nodeWd in node.ChildNodes)
                {
                    WorkCondition wd = new WorkCondition()
                    {
                        SWorkConditionName = nodeWd.Attributes["SWorkConditionName"].Value.ToString(),
                        SWindSpeed = nodeWd.Attributes["SWindSpeed"].Value.ToString(),
                        STemperature = nodeWd.Attributes["STemperature"].Value.ToString(),
                        SIceThickness = nodeWd.Attributes["SIceThickness"].Value.ToString(),
                    };
                    listWd.Add(wd);
                }

                Weather weather = new Weather()
                {
                    Name = node.Attributes["SName"].Value.ToString(),
                    WorkConditions = listWd
                };
                list.Add(weather);
            }

            return list;
        }

        public override List<Weather> Read(string dir, string tableName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public override List<Weather> ReadLocal(string path)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            XmlNode rootNode = doc.FirstChild;
            if (rootNode == null)
                return new List<Weather>();

            List<Weather> list = new List<Weather>();

            foreach (XmlNode node in rootNode.ChildNodes)
            {
                List<WorkCondition> listWd = new List<WorkCondition>();
                foreach (XmlNode nodeWd in node.ChildNodes)
                {
                    WorkCondition wd = new WorkCondition()
                    {
                        SWorkConditionName = nodeWd.Attributes["SWorkConditionName"].Value.ToString(),
                        SWindSpeed = nodeWd.Attributes["SWindSpeed"].Value.ToString(),
                        STemperature = nodeWd.Attributes["STemperature"].Value.ToString(),
                        SIceThickness = nodeWd.Attributes["SIceThickness"].Value.ToString(),
                    };
                    listWd.Add(wd);
                }

                Weather weather = new Weather()
                {
                    Name = node.Attributes["SName"].Value.ToString(),
                    WorkConditions = listWd
                };
                list.Add(weather);
            }

            return list;
        }

        public override void Save(string path, List<Weather> weathers)
        {
            XmlDocument doc = new XmlDocument();

            XmlNode rootNode = doc.CreateElement("Root");
            doc.AppendChild(rootNode);

            foreach(var item in weathers)
            {
                XmlNode weatherNode = doc.CreateElement("KNode");
                rootNode.AppendChild(weatherNode);

                XmlAttribute weatherAttribute = doc.CreateAttribute("SName");
                weatherAttribute.Value = item.Name.ToString();
                weatherNode.Attributes.Append(weatherAttribute);

                foreach(var wdItem in item.WorkConditions)
                {
                    XmlNode wdNode = doc.CreateElement("KNode");
                    weatherNode.AppendChild(wdNode);

                    XmlAttribute iceThicknessAttribute = doc.CreateAttribute("SIceThickness");
                    iceThicknessAttribute.Value = wdItem.SIceThickness.ToString();
                    wdNode.Attributes.Append(iceThicknessAttribute);

                    XmlAttribute temperatureAttribute = doc.CreateAttribute("STemperature");
                    temperatureAttribute.Value = wdItem.STemperature.ToString();
                    wdNode.Attributes.Append(temperatureAttribute);

                    XmlAttribute windSpeedAttribute = doc.CreateAttribute("SWindSpeed");
                    windSpeedAttribute.Value = wdItem.SWindSpeed.ToString();
                    wdNode.Attributes.Append(windSpeedAttribute);

                    XmlAttribute workConditionNameAttribute = doc.CreateAttribute("SWorkConditionName");
                    workConditionNameAttribute.Value = wdItem.SWorkConditionName.ToString();
                    wdNode.Attributes.Append(workConditionNameAttribute);
                }
            }

            doc.Save(path);
        }
    }

    public class WeatherDbfReader : WeatherReader
    {
        public override List<Weather> Read(string path)
        {
            throw new NotImplementedException();
        }

        public override List<Weather> Read(string dir, string tableName)
        {
            DataTable DT = DbfUtils.ReadDbf(dir, tableName);

            List<WorkCondition> wcList = new List<WorkCondition>();
            foreach (DataRow row in DT.Rows)
            {
                wcList.Add(new WorkCondition
                {
                    SWorkConditionName = row["工况名称"].ToString(),
                    SWindSpeed = row["风速"].ToString(),
                    STemperature = row["气温"].ToString(),
                    SIceThickness = row["冰厚"].ToString(),
                });
            }

            Weather weather = new Weather
            {
                Name = "DBF数据",
                WorkConditions = wcList
            };

            List<Weather> weathers = new List<Weather>();
            weathers.Add(weather);

            return weathers;
        }

        public override List<Weather> ReadLocal(string path)
        {
            throw new NotImplementedException();
        }

        public override void Save(string path, List<Weather> weathers)
        {
            throw new NotImplementedException();
        }
    }

    public class WeatherReaderFactory
    {
        public static WeatherReader CreateReader(string type)
        {
            WeatherReader weatherReader = null;

            if (type.Equals("XML"))
            {
                weatherReader = new WeatherXmlReader();
            }
            else if (type.Equals("DBF"))
            {
                weatherReader = new WeatherDbfReader();
            }
            return weatherReader;
        }
    }
    
}