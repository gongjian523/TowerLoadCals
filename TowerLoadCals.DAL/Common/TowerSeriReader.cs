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
    public class TowerSeriReader
    {
        public static List<TowerSeri> ReadTa(string path)
        {
            List<string> lineList = new List<string>();
            List<TowerSeri> towerSeriList = new List<TowerSeri>();

            string line;

            // Read the file and display it line by line.  
            System.IO.StreamReader file = new System.IO.StreamReader(path);
            while ((line = file.ReadLine()) != null)
            {
                lineList.Add(line);
            }

            file.Close();

            int i = 0;

            foreach (string str in lineList)
            {
                if (!str.Contains(","))
                    continue;
                string[] aLines= str.Split(',');
                i++;
                towerSeriList.Add(new TowerSeri
                {
                    ID = i,
                    Num = aLines[0],
                    PosName = aLines[7].TrimStart(' '),
                    PosOffset = aLines[3].TrimStart(' '),
                    Pos = aLines[7] + "+" + aLines[3],
                    Type = Convert.ToInt32(aLines[1]),
                    Model = aLines[8],
                    Elevation = Convert.ToDouble(aLines[6]),
                    SubOfElv = Convert.ToDouble(aLines[12]),
                    TotalSpan = Convert.ToDouble(aLines[2]),
                    BackK = Convert.ToDouble(aLines[4]),
                    Height = Convert.ToDouble(aLines[9]),
                    StringLength = Convert.ToDouble(aLines[11]),
                    AngelofApplication = Convert.ToDouble(aLines[20])
                });
            }

            CalsParameters(towerSeriList);

            return towerSeriList;
        }


        private static void CalsParameters(List<TowerSeri> tas)
        {
            for (int i = 0; i < tas.Count; i++)
            {
                //获取塔名和呼高
                tas[i].Name = tas[i].Model.Split('-')[0];

                //前侧档距 = 前一个塔的累距-自己的累距
                if (i == tas.Count - 1)
                    tas[i].FrontSpan = 0;
                else
                    tas[i].FrontSpan = Math.Ceiling(tas[i + 1].TotalSpan - tas[i].TotalSpan);

                //后侧档距 = 自己的累距-前一个塔的累距
                //水平档距 = (前侧档距 + 后侧档距) / 2
                double backSpan = 0;
                if (i != 0)
                    backSpan = tas[i].TotalSpan - tas[i - 1].TotalSpan;
                tas[i].HorizontalSpan = Math.Ceiling((tas[i].FrontSpan + backSpan) / 2);

                if (tas[i].Type == 2)
                    tas[i].StringLength = 0;

                tas[i].guadg = tas[i].Elevation + tas[i].Height + tas[i].SubOfElv - tas[i].StringLength;

                if (i != 0)
                {
                    double h = tas[i].guadg - tas[i - 1].guadg;
                    double x = tas[i].BackK * tas[i].FrontSpan * 0.001;
                    double y = ((-1) * tas[i].BackK * h * 0.001) / Math.Sinh(x);
                    tas[i].sec = Math.Log(y + Math.Sqrt(y * y + 1)) / (2 * tas[i].BackK * 0.001);
                    tas[i].BackVerticalSpan = Math.Ceiling(tas[i].FrontSpan / 2 + Math.Log(y + Math.Sqrt(y * y + 1)) / (2 * tas[i].BackK * 0.001));
                    tas[i].FrontVerticalSpan = Math.Ceiling(tas[i].FrontSpan - tas[i].BackVerticalSpan);

                    if (tas[i].Type == 1)
                    {
                        double vs = tas[i].BackVerticalSpan + tas[i - 1].FrontVerticalSpan;
                        tas[i].VerticalSpan = vs.ToString();
                    }
                    else
                    {
                        tas[i].VerticalSpan = tas[i - 1].FrontVerticalSpan.ToString() + "/" + tas[i].BackVerticalSpan.ToString();
                    }
                }
                else
                {
                    tas[i].VerticalSpan = "0";
                }
            }
        }


        public static List<TowerSeri> Read(string path)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            XmlNode rootNode = doc.GetElementsByTagName("Root")[0];
            if (rootNode == null)
                return new List<TowerSeri>();

            List<TowerSeri> list = new List<TowerSeri>();

            foreach (XmlNode node in rootNode.ChildNodes)
            {
                TowerSeri seri = new TowerSeri()
                {
                    ID = Convert.ToInt32(node.Attributes["ID"].Value.ToString()),
                    Num = node.Attributes["Num"].Value.ToString(),
                    PosName = node.Attributes["PosName"].Value.ToString(),
                    PosOffset = node.Attributes["PosOffset"].Value.ToString(),
                    Name = node.Attributes["Name"].Value.ToString(),
                    Type = Convert.ToInt32(node.Attributes["Type"].Value.ToString()),
                    Height = Convert.ToInt32(node.Attributes["Height"].Value.ToString()),
                    ResID = node.Attributes["ResID"].Value.ToString(),
                    Elevation = Convert.ToInt32(node.Attributes["Elevation"].Value.ToString()),
                    TotalSpan = Convert.ToInt32(node.Attributes["TotalSpan"].Value.ToString()),
                    FrontSpan = Convert.ToInt32(node.Attributes["FrontSpan"].Value.ToString()),
                    FrontHorizontalSpan = Convert.ToInt32(node.Attributes["FrontHorizontalSpan"].Value.ToString()),
                    BackHorizontalSpan = Convert.ToInt32(node.Attributes["BackHorizontalSpan"].Value.ToString()),
                    HorizontalSpan = Convert.ToInt32(node.Attributes["HorizontalSpan"].Value.ToString()),
                    FrontVerticalSpan = Convert.ToInt32(node.Attributes["FrontVerticalSpan"].Value.ToString()),
                    BackVerticalSpan = Convert.ToInt32(node.Attributes["BackVerticalSpan"].Value.ToString()),
                    VerticalSpan = node.Attributes["VerticalSpan"].Value.ToString(),
                    FrontDRepresentSpan = Convert.ToInt32(node.Attributes["FrontDRepresentSpan"].Value.ToString()),
                    BackDRepresentSpan = Convert.ToInt32(node.Attributes["BackDRepresentSpan"].Value.ToString()),
                    StringLength = Convert.ToInt32(node.Attributes["StringLength"].Value.ToString()),
                    FrontWeatherID = node.Attributes["FrontWeatherID"].Value.ToString(),
                    BackWeatherID = node.Attributes["BackWeatherID"].Value.ToString(),
                };
                list.Add(seri);
            }

            return list;
        }

        public static void Save(string path, List<TowerSeri> infos)
        {
            XmlUtils.Save(path, infos);
        }
    }
}
