using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerLoadCals.Mode;

namespace TowerLoadCals.Readers
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
                    Index = i,
                    Num = aLines[0],
                    PosName = aLines[7].TrimStart(' '),
                    PosOffset = aLines[3].TrimStart(' '),
                    Pos = aLines[7] + "+" + aLines[3],
                    Type = Convert.ToInt16(aLines[1]),
                    Model = aLines[8],
                    Elevation = Convert.ToDouble(aLines[6]),
                    SubOfElv = Convert.ToDouble(aLines[12]),
                    TotalSpan = Convert.ToDouble(aLines[2]),
                    WireK = Convert.ToDouble(aLines[4]),
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
                    double x = tas[i].WireK * tas[i].FrontSpan * 0.001;
                    double y = ((-1) * tas[i].WireK * h * 0.001) / Math.Sinh(x);
                    tas[i].sec = Math.Log(y + Math.Sqrt(y * y + 1)) / (2 * tas[i].WireK * 0.001);
                    tas[i].BackVerticalSpan = Math.Ceiling(tas[i].FrontSpan / 2 + Math.Log(y + Math.Sqrt(y * y + 1)) / (2 * tas[i].WireK * 0.001));
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
    }
}
