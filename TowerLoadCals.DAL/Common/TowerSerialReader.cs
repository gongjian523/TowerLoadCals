using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TowerLoadCals.Mode;
using TowerLoadCals.Mode.Electric;

namespace TowerLoadCals.DAL.Common
{
    /// <summary>
    /// 杆塔序列 文件操作
    /// </summary>
    public static class TowerSerialReader
    {
        #region 读取原始TA文件
        /// <summary>
        /// 读取原始TA文件
        /// </summary>
        /// <param name="sourcePath">原始文件路径</param>
        /// <returns></returns>
        public static List<TowerSerial> ReadTa(string sourcePath)
        {
            List<string> lineList = new List<string>();
            List<TowerSerial> towerSeriList = new List<TowerSerial>();

            string line;

            // Read the file and display it line by line.  
            StreamReader file = new StreamReader(sourcePath);
            while ((line = file.ReadLine()) != null)
            {
                lineList.Add(line);
            }

            file.Close();
            string[] itemLine;
            TowerSerial tower;
            for (int i = 1; i < lineList.Count; i++)
            {
                itemLine = lineList[i].Split(',');
                tower = new TowerSerial();

                tower.ID = i;
                tower.TowerName = itemLine[0];
                tower.TowerType = int.Parse(itemLine[1]);//塔类型:直线塔1转角塔2门架3
                tower.TowerTypeName = itemLine[1].ToString() == "1" ? "直线塔" : itemLine[1].ToString() == "2" ? "耐张塔" : "终端塔";//塔类型:直线塔1转角塔2门架3
                tower.AccumulativeDistance = itemLine[2] == "" ? 0 : Convert.ToDouble(itemLine[2]);
                tower.PointAndGround_Distance = itemLine[3] == "" ? 0 : Convert.ToDouble(itemLine[3]);
                tower.KValueOfConductor = itemLine[4] == "" ? 0 : Convert.ToDouble(itemLine[4]);
                tower.DeviationDistance = itemLine[5] == "" ? 0 : Convert.ToDouble(itemLine[5]);
                tower.TowerFootElevation = itemLine[6] == "" ? 0 : Convert.ToDouble(itemLine[6]);
                tower.NameOfPositioningPile = itemLine[7];
                tower.TowerPattern = itemLine[8].Split('-')[0];
                tower.CallItHigh = itemLine[9] == "" ? 0 : Convert.ToDouble(itemLine[9]);
                tower.InsulationStringModel = itemLine[10];
                tower.InsulationStringNumber = 0;
                tower.InsulationStringLength = itemLine[11] == "" ? 0 : Convert.ToDouble(itemLine[11]);
                tower.BaseLevelDescent = itemLine[12] == "" ? 0 : Convert.ToDouble(itemLine[12]);
                tower.GroundWireKValue = itemLine[13] == "" ? 0 : Convert.ToDouble(itemLine[13]);
                tower.BackWireDistance = itemLine[14] == "" ? 0 : Convert.ToDouble(itemLine[14]);
                tower.FrontWireDistance = itemLine[15] == "" ? 0 : Convert.ToDouble(itemLine[15]);
                tower.ModelOfGroundWireFittings = itemLine[16];
                tower.QuantityOfGroundWireFittings = itemLine[17] == "" ? 0 : Convert.ToDouble(itemLine[17]);
                tower.BackGroundDistance = itemLine[18] == "" ? 0 : Convert.ToDouble(itemLine[18]);
                tower.FrontGroundDistance = itemLine[19] == "" ? 0 : Convert.ToDouble(itemLine[19]);
                tower.TurningAngle = itemLine[20] == "" ? 0 : Convert.ToDouble(itemLine[20]);//转角(度 分)
                tower.TurningAngleStr = itemLine[20] == "" || double.Parse(itemLine[20].ToString()) == 0 ? "" : (Convert.ToDouble(itemLine[20]) < 0 ? "右" : "左") + itemLine[20];//转角(度 分)
                tower.VerticalSpan = itemLine[21];
                tower.SafetyDistanceOfCutWire = itemLine[22] == "" ? 0 : Convert.ToDouble(itemLine[22]);
                tower.SafetyFactorKConductor = itemLine[23] == "" ? 0 : Convert.ToDouble(itemLine[23]);
                tower.SafetyFactorKValueOfGroundWire = itemLine[24] == "" ? 0 : Convert.ToDouble(itemLine[24]);
                tower.SingleORDoubleString = itemLine[25] == "" ? 0 : Convert.ToDouble(itemLine[25]);
                tower.TotalWeightOfHungWeightHammer = itemLine[26] == "" ? 0 : Convert.ToDouble(itemLine[26]);
                tower.WireStressInRearSight = itemLine[27] == "" ? 0 : Convert.ToDouble(itemLine[27]);
                tower.tensionInsulatorStringIsHangingUpsideDown = itemLine[28] == "1" ? true : false;
                tower.PlanDrawing = itemLine[29];
                tower.Display = itemLine[30];
                tower.NumberOfGroundWires = itemLine[31] == "" ? 0 : Convert.ToInt32(itemLine[31]);
                tower.ConductorModel = itemLine[32];
                tower.GroundWireModel = itemLine[33];
                tower.SteelWeight = itemLine[34];
                tower.GroundHorizontalDistance = itemLine[35] == "" ? 0 : Convert.ToDouble(itemLine[35]);
                tower.DistanceBetweenUpperAndLowerConductors = Convert.ToDouble(itemLine[36]);
                tower.MiddleAndLowerConductorDistance = itemLine[37] == "" ? 0 : Convert.ToDouble(itemLine[37]);
                tower.HeightOfGroundWireSupport = itemLine[38] == "" ? 0 : Convert.ToDouble(itemLine[38]);

                towerSeriList.Add(tower);
            }

            CalsParameters(towerSeriList);
            return towerSeriList;
        }


        /// <summary>
        /// 计算页面展示结果值 计算
        /// </summary>
        /// <param name="tower"></param>
        public static void CalsParameters(List<TowerSerial> list)
        {
            TowerSerial tower;
            TowerSerial beforeTower;

            double x = 0, y = 0;
            for (int i = 0; i < list.Count; i++)
            {
                tower = list[i];
                beforeTower = i == 0 ? new TowerSerial() : list[i - 1];

                tower.Span = tower.AccumulativeDistance - beforeTower.AccumulativeDistance;//档距=当前累距-前一项累距
                tower.HorizontalSpan = (beforeTower.Span + tower.Span) / 2;// 水平档距 = (前侧档距 + 后侧档距)/2
                tower.guadg = tower.TowerFootElevation + tower.CallItHigh + tower.BaseLevelDescent - tower.InsulationStringLength;//挂点高=塔角高程+呼陈高+基面下降-绝缘串长
                tower.h = tower.guadg - beforeTower.guadg;//挂线点高差=当前挂点高-前一项挂点高
                if (i != 0)
                {
                    x = tower.KValueOfConductor * tower.Span * 0.001;
                    y = -tower.KValueOfConductor * tower.h * 0.001 / Math.Sin(x);
                    tower.FrontVerticalSpan = (int)(tower.Span / 2 + Math.Asin(y) / (2 * tower.KValueOfConductor * 0.001) + 0.5);
                    tower.BackVerticalSpan = (int)(tower.Span - tower.FrontVerticalSpan + 0.5);
                }

                if (tower.TowerType == 1)
                {
                    tower.VerticalSpan = (tower.FrontVerticalSpan + beforeTower.BackVerticalSpan) + "";
                }
                else
                {
                    tower.VerticalSpan = beforeTower.BackVerticalSpan + "/" + tower.FrontVerticalSpan;
                }

                //tower.FrontSpan = (beforeTower.Span + tower.Span) / 2;// 前侧代表档距
                //tower.BackSpan = (beforeTower.Span + tower.Span) / 2;// 后侧代表档距
            }
            for (int i = 1; i < list.Count; i++)
            {
                list[i - 1].Span = list[i].Span;
                list[i - 1].HorizontalSpan = list[i].HorizontalSpan;
                if (i == list.Count - 1)
                {
                    list[i].Span = 0;
                    list[i].HorizontalSpan = 0;

                }
            }
        }
        #endregion

        #region 保存源文件

        /// <summary>
        /// 保存源文件
        /// </summary>
        /// <param name="fileName">杆塔序列名称</param>
        /// <param name="loadPath">源文件路径</param>
        /// <param name="savePath">保存文件路径</param>
        public static void CopySourceFile(string fileName, string loadPath, string savePath)
        {
            //文件地址
            Directory.CreateDirectory(savePath);//创建对应序列文件夹

            File.Copy(loadPath, (savePath + @"\source.TA"));//复制原文件，保存备份
        }
        #endregion


        #region 保存单个杆塔序列XML文件

        /// <summary>
        /// 保存单个杆塔序列XML文件
        /// </summary>
        /// <param name="list">读取文件列表</param>
        /// <param name="fileName">杆塔序列名称</param>
        /// <param name="loadPath">源文件路径</param>
        /// <param name="savePath">保存文件路径</param>
        public static void SaveDT(List<TowerSerial> list, string savePath)
        {
            savePath = savePath + @"\TowerSequenceStr.xml";
            if (File.Exists(savePath))
            {
                File.Delete(savePath);
            }
            //保存计算后的xml
            XmlDocument doc = new XmlDocument();
            XmlDeclaration declaration = doc.CreateXmlDeclaration("1.0", "UTF-8", "");//xml文档的声明部分
            doc.AppendChild(declaration);

            XmlElement root = doc.CreateElement("Root");
            doc.AppendChild(root);


            foreach (TowerSerial item in list)
            {
                XmlElement row = doc.CreateElement("Sequence");
                row.SetAttribute("ID", item.ID.ToString());//序号
                row.SetAttribute("IsChecking", item.IsChecking.ToString());//是否需要验算
                row.SetAttribute("WeatherCondition", item.WeatherCondition == null ? "" : item.WeatherCondition.ToString());//气象条件
                row.SetAttribute("TowerName", item.TowerName.ToString());//塔位号
                row.SetAttribute("NameOfPositioningPile", item.NameOfPositioningPile.ToString());//塔位点
                row.SetAttribute("TowerPattern", item.TowerPattern.ToString());//塔型
                row.SetAttribute("CallItHigh", item.CallItHigh.ToString());//呼高
                row.SetAttribute("TowerFootElevation", item.TowerFootElevation.ToString());//塔位桩高程
                row.SetAttribute("BaseLevelDescent", item.BaseLevelDescent.ToString());//定位高差
                row.SetAttribute("Span", item.Span.ToString());//档距
                row.SetAttribute("HorizontalSpan", item.HorizontalSpan.ToString());//水平档距
                row.SetAttribute("guadg", item.guadg.ToString());//挂点高
                row.SetAttribute("h", item.h.ToString());//挂线点高差
                row.SetAttribute("FrontVerticalSpan", item.FrontVerticalSpan.ToString());//前侧垂距
                row.SetAttribute("BackVerticalSpan", item.BackVerticalSpan.ToString());//后侧垂距
                row.SetAttribute("VerticalSpan", item.VerticalSpan.ToString());//垂直档距
                row.SetAttribute("FrontSpan", item.VerticalSpan.ToString());//前侧代表档距
                row.SetAttribute("BackSpan", item.VerticalSpan.ToString());//后侧代表档距
                row.SetAttribute("TurningAngle", item.TurningAngle.ToString());//转角中心桩位移(m)
                row.SetAttribute("TurningAngleStr", item.TurningAngleStr);//转角中心桩位移(m)
                row.SetAttribute("ConductorModel", item.ConductorModel.ToString());//导线型号
                row.SetAttribute("InsulationStringModel", item.InsulationStringModel.ToString());//导线串型号
                row.SetAttribute("InsulationStringSlices", item.InsulationStringSlices == null ? "" : item.InsulationStringSlices.ToString());//导线串片数
                row.SetAttribute("InsulationStringNumber", item.InsulationStringNumber.ToString());//导线串数量
                row.SetAttribute("InsulationStringLength", item.InsulationStringLength.ToString());//导线串长度
                row.SetAttribute("GroundWireModel", item.GroundWireModel.ToString());//地线型号
                row.SetAttribute("GroundWireInsulationStringModel", item.GroundWireInsulationStringModel == null ? "" : item.GroundWireInsulationStringModel.ToString());//地线串型号
                row.SetAttribute("GroundWireInsulationStringNumber", "0");//地线串数量

                root.AppendChild(row);
            }
            //最后将整个xml文件保存在D盘             
            doc.Save(savePath);

        }
        #endregion


        #region 直接读取本地文件xml信息

        /// <summary>
        /// 直接读取本地文件xml信息 同时需要判断  塔型、导线型号、导线串型、地线型号 是否满足在本项目中存在
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public static List<TowerSerial> ReadXml(string projectDir, string navg)
        {
            #region 获取本项目 塔型、导线型号、导线串型、地线型号 信息

            string filePath = "";
            string basePath = projectDir + "\\" + ConstVar.DataBaseStr + "\\";
            //判断塔型
            filePath = basePath + "TowerStr.xml";
            List<TowerStrData> towerList = TowerStrDataReader.ReadLoadFile(filePath).ToList();

            List<String> towerNameList = towerList.Select(t => t.Name).ToList();

            //导地线
            filePath = basePath + "Wire.xml";
            List<WireType> allList = WireReader.Read(filePath);
            //判断导线
            List<string> wireList = allList.Where(w => w.Type == "导线").First().Wires.Select(w => w.ModelSpecification).ToList();
            //判断地线
            List<string> groundList = allList.Where(g => g.Type == "地线").First().Wires.Select(g => g.ModelSpecification).ToList();
            //判断绝缘串
            filePath = basePath + @"\StrData.xml";
            List<string> strList = StrDataReader.Read(filePath).Where(s => s.Type == "一般子串").First().StrDatas.Select(s => s.Name).ToList();
            #endregion


            //读取序列信息，并进行逻辑判断
            string loadPath = projectDir + "\\" + ConstVar.TowerSequenceStr + "\\" + navg + "\\TowerSequenceStr.xml";

            List<TowerSerial> towerSeriList = new List<TowerSerial>();

            XmlDocument doc = new XmlDocument();
            doc.Load(loadPath);

            XmlNode rootNode = doc.GetElementsByTagName("Root")[0];

            foreach (XmlNode xmlNode in rootNode.ChildNodes)
            {
                TowerSerial item = new TowerSerial();
                item.ID = int.Parse(xmlNode.Attributes.GetNamedItem("ID").InnerText);//序号
                item.WeatherCondition = xmlNode.Attributes.GetNamedItem("WeatherCondition").InnerText;//气象条件
                item.TowerName = xmlNode.Attributes.GetNamedItem("TowerName").InnerText;//塔位号
                item.NameOfPositioningPile = xmlNode.Attributes.GetNamedItem("NameOfPositioningPile").InnerText;//塔位点
                item.TowerPattern = xmlNode.Attributes.GetNamedItem("TowerPattern").InnerText;//塔型
                item.CallItHigh = double.Parse(xmlNode.Attributes.GetNamedItem("CallItHigh").InnerText);//呼高
                item.TowerFootElevation = double.Parse(xmlNode.Attributes.GetNamedItem("TowerFootElevation").InnerText);//塔位桩高程
                item.BaseLevelDescent = double.Parse(xmlNode.Attributes.GetNamedItem("BaseLevelDescent").InnerText);//定位高差
                item.Span = double.Parse(xmlNode.Attributes.GetNamedItem("Span").InnerText);//档距
                item.HorizontalSpan = double.Parse(xmlNode.Attributes.GetNamedItem("HorizontalSpan").InnerText);//水平档距
                item.VerticalSpan = xmlNode.Attributes.GetNamedItem("VerticalSpan").InnerText;//垂直档距
                //item.TensionSpan = double.Parse(xmlNode.Attributes.GetNamedItem("TensionSpan").InnerText);//耐张段长/代表档距
                item.TurningAngle = double.Parse(xmlNode.Attributes.GetNamedItem("TurningAngle").InnerText);//转角中心桩位移(m)
                item.TurningAngleStr = xmlNode.Attributes.GetNamedItem("TurningAngleStr").InnerText;//转角中心桩位移(m)
                item.ConductorModel = xmlNode.Attributes.GetNamedItem("ConductorModel").InnerText;//导线型号
                item.InsulationStringModel = xmlNode.Attributes.GetNamedItem("InsulationStringModel").InnerText;//导线串型号
                item.InsulationStringSlices = xmlNode.Attributes.GetNamedItem("InsulationStringSlices").InnerText;//导线串片数
                item.InsulationStringNumber = double.Parse(xmlNode.Attributes.GetNamedItem("InsulationStringNumber").InnerText);//导线串数量
                item.InsulationStringLength = double.Parse(xmlNode.Attributes.GetNamedItem("InsulationStringLength").InnerText);//导线串长度
                item.GroundWireModel = xmlNode.Attributes.GetNamedItem("GroundWireModel").InnerText;//地线型号
                item.GroundWireInsulationStringModel = xmlNode.Attributes.GetNamedItem("GroundWireInsulationStringModel").InnerText;//地线串型号
                item.GroundWireInsulationStringNumber = xmlNode.Attributes.GetNamedItem("GroundWireInsulationStringNumber").InnerText;//地线串数量


                if (towerNameList.Contains(item.TowerPattern))//塔型判断
                {
                    item.ExistsTowerPattern = true;
                }
                if (wireList.Contains(item.ConductorModel))//导线型号判断
                {
                    item.ExistsConductorModel = true;
                }
                if (groundList.Contains(item.GroundWireModel))//地线型号判断
                {
                    item.ExistsGroundWireModel = true;
                }
                if (strList.Contains(item.InsulationStringModel))//导线串型号判断
                {
                    item.ExistsWireInsulationString = true;
                }

                JudgeCheck(towerList, item);//判断是否需要验算

                towerSeriList.Add(item);
            }
            return towerSeriList;
        }

        /// <summary>
        /// 考虑后期可能直接进行修改时进行判断，单独提取方法
        /// </summary>
        /// <param name="towerList">杆塔列表</param>
        /// <param name="item">序列塔位号实体</param>
        public static void JudgeCheck(List<TowerStrData> towerList, TowerSerial item)
        {
            TowerStrData tower;//塔型实体
            List<string> StrHeight;//直线塔呼高
            double StrAllowHorSpan;// 直线塔档距序列字符串

            //杆塔型号实体
            tower = towerList.Where(t => t.Name == item.TowerPattern).FirstOrDefault();

            if (tower != null)//塔型判断
            {
                if (tower.TypeName == "直线塔" || tower.TypeName == "直线转角塔" || tower.TypeName == "终端塔" || tower.TypeName == "分支塔")
                {
                    //                      水平档距的验算
                    //实际使用直线塔水平档距大于铁塔使用条件中水平档距的
                    StrHeight = tower.StrHeightSer.Split(',').ToList();
                    int index = StrHeight.IndexOf(item.CallItHigh.ToString());
                    StrAllowHorSpan = double.Parse(tower.StrAllowHorSpan.Split(',').ToList()[index]);

                    if (item.HorizontalSpan > StrAllowHorSpan && item.IsChecking == false)//如果水平档距大于了设定呼高设置最大值
                    {
                        item.IsChecking = true;
                    }
                    //                     垂直档距验算
                    //实际使用直线塔垂直档距大于铁塔使用条件中垂直档距的
                    if (double.Parse(item.VerticalSpan) > tower.AllowedVerSpan && item.IsChecking == false)
                    {
                        item.IsChecking = true;
                    }
                    if (tower.TypeName == "直线塔")//直线塔判断
                    {
                        //                    带角度直线塔验算
                        //实际使用时直线塔带角度的，全部进行验算
                        if (item.TurningAngle != 0 && item.IsChecking == false)
                        {
                            item.IsChecking = true;
                        }
                    }
                    else if (tower.TypeName == "直线转角塔" || tower.TypeName == "终端塔" || tower.TypeName == "分支塔")//直线转角塔判断
                    {
                        //                        带角度直线塔验算
                        //转角度数超过塔库中使用条件最大转角的，就验算。
                        if (item.TurningAngle > tower.MaxAngel && item.IsChecking == false)
                        {
                            item.IsChecking = true;
                        }
                    }
                }
                else //耐张塔
                {
                    if (item.HorizontalSpan > tower.AllowedHorSpan && item.IsChecking == false)//水平档距
                    {
                        item.IsChecking = true;
                    }
                    if (item.HorizontalSpan > tower.MaxAngHorSpan && item.IsChecking == false)//水平档距
                    {
                        item.IsChecking = true;
                    }
                    if (item.HorizontalSpan > (tower.AllowedHorSpan + (tower.MaxAngel - item.TurningAngle) * tower.AngelToHorSpan) && item.IsChecking == false)//水平档距
                    {
                        item.IsChecking = true;
                    }
                    if (Math.Abs(item.VerticalSpan.Split('/').Sum(k=>double.Parse(k))) > tower.AllowedVerSpan)//垂直档距验算
                    {
                        item.IsChecking = true;
                    }
                }

            }
        }
        #endregion

    }
}
