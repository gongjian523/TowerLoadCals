using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TowerLoadCals.Common.Utils;
using TowerLoadCals.Mode;
using TowerLoadCals.Mode.Electric;

/// <summary>
/// created by :glj
/// </summary>

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
                tower.TurningAngle = itemLine[21] == "" ? 0 : Convert.ToDouble(itemLine[21]);//转角(度 分)
                tower.TurningAngleStr = itemLine[21] == "" || double.Parse(itemLine[21].ToString()) == 0 ? "" : (Convert.ToDouble(itemLine[21]) < 0 ? "右:" : "左:") + itemLine[21].ToString().Trim(' ').TrimStart('-');//转角(度 分)
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
        /// 计算页面展示结果值 计算 后期调整
        /// </summary>
        /// <param name="tower"></param>
        public static void CalsParameters(List<TowerSerial> list)
        {
            TowerSerial tower;
            TowerSerial backTower;
            TowerSerial frontTower;

            double x = 0, y = 0;
            double accPreSpan = 0 , accPreSpan1 = 0 , accPreSpan2 = 0, accPreSpan3 = 0, accPreSpanRst = 0;
            int preSt = 0, preEnd = 0; 

            for (int i = 0; i < list.Count; i++)
            {
                tower = list[i];
                backTower = i == 0 ? list[0] : list[i - 1];

                tower.Span = tower.AccumulativeDistance - backTower.AccumulativeDistance;//档距=当前累距-前一项累距
            }


            for (int i = 0; i < list.Count; i++)
            {
                tower = list[i];
                backTower = i == 0 ? new TowerSerial() : list[i - 1];
                frontTower = i == list.Count - 1 ? new TowerSerial() : list[i + 1];

                //tower.Span = tower.AccumulativeDistance - beforeTower.AccumulativeDistance;//档距=当前累距-前一项累距
                tower.HorizontalSpan = (frontTower.Span + tower.Span) / 2;// 水平档距 = (前侧档距 + 后侧档距)/2
                tower.guadg = tower.TowerType == 1 ? tower.TowerFootElevation + tower.CallItHigh + tower.BaseLevelDescent - tower.InsulationStringLength
                    : tower.TowerFootElevation + tower.CallItHigh + tower.BaseLevelDescent;//挂点高=塔角高程+呼陈高+基面下降-绝缘串长
                tower.h = tower.guadg - backTower.guadg;//挂线点高差=当前挂点高-前一项挂点高

                double subHb;

                if (i != 0)
                {
                    x = tower.KValueOfConductor * tower.Span * 0.001;
                    y = tower.KValueOfConductor * tower.h * 0.001 / Math.Sinh(x);
                    tower.BackVerticalSpan = Math.Floor(tower.Span / 2 + Calc.Asinh(y) / (2 * tower.KValueOfConductor * 0.001) + 0.5);
                    backTower.FrontVerticalSpan = Math.Floor(tower.Span - tower.BackVerticalSpan + 0.5);

                    subHb = tower.Span / Math.Sqrt(Math.Pow(tower.h, 2) + Math.Pow(tower.Span, 2));
                    accPreSpan = accPreSpan + tower.Span / subHb;
                    accPreSpan1 = accPreSpan1 + tower.Span;
                    accPreSpan2 = accPreSpan2 + tower.Span / Math.Pow(subHb, 2);
                    accPreSpan3 = accPreSpan3 + Math.Pow(tower.Span, 3) * subHb;
                }

                preEnd = i;
                if(tower.TowerType != 1 && i != 0)
                {
                    accPreSpanRst = Math.Floor(accPreSpan2 / accPreSpan * Math.Sqrt(accPreSpan3 / accPreSpan) + 0.5);
                    list[preSt].FrontPreSpan = accPreSpanRst;
                    list[preSt].FrontAccPreSpan = accPreSpan1;

                    for(int j = preSt + 1; j < preEnd; j++)
                    {
                        list[j].FrontPreSpan = list[j].BackPreSpan = accPreSpanRst;
                        list[j].FrontAccPreSpan = list[j].BackAccPreSpan = accPreSpan1;
                    }

                    list[preEnd].BackPreSpan = accPreSpanRst;
                    list[preEnd].BackAccPreSpan = accPreSpan1;

                    preSt = preEnd;

                    accPreSpan = accPreSpan1 = accPreSpan2 = accPreSpan3 = 0;
                }
            }

            for (int i = 0; i < list.Count; i++)
            {
                list[i].VerticalSpan = list[i].FrontVerticalSpan + list[i].BackVerticalSpan ;
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
                row.SetAttribute("TowerType", item.TowerType.ToString());//原始塔型类型
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
                row.SetAttribute("FrontPreSpan", item.FrontPreSpan.ToString());//前侧代表档距
                row.SetAttribute("FrontAccPreSpan", item.FrontAccPreSpan.ToString());//前侧代表档距
                row.SetAttribute("BackPreSpan", item.BackPreSpan.ToString());//后侧代表档距
                row.SetAttribute("BackAccPreSpan", item.BackAccPreSpan.ToString());//后侧耐张段长度
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


                //验算相关参数
                row.SetAttribute("WireCounterWeightNum", "0");//导线防震锤数量
                row.SetAttribute("GroundCounterWeightNum", "0");//地线防震锤数量
                row.SetAttribute("WireSpacerNum", "0");//导线间隔棒数量

                row.SetAttribute("CommPar", item.CommPar);
                row.SetAttribute("FrontSidePar", item.FrontSidePar);
                row.SetAttribute("BackSidePar", item.BackSidePar);
                row.SetAttribute("TowerPar", item.TowerPar);


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
                item.IsChecking =bool.Parse(xmlNode.Attributes.GetNamedItem("IsChecking").InnerText);//气象条件
                item.WeatherCondition = xmlNode.Attributes.GetNamedItem("WeatherCondition").InnerText;//气象条件
                item.TowerName = xmlNode.Attributes.GetNamedItem("TowerName").InnerText;//塔位号
                item.TowerType = int.Parse(xmlNode.Attributes.GetNamedItem("TowerType").InnerText); //塔型
                item.NameOfPositioningPile = xmlNode.Attributes.GetNamedItem("NameOfPositioningPile").InnerText;//塔位点
                item.TowerPattern = xmlNode.Attributes.GetNamedItem("TowerPattern").InnerText;//塔型
                item.CallItHigh = double.Parse(xmlNode.Attributes.GetNamedItem("CallItHigh").InnerText);//呼高
                item.TowerFootElevation = double.Parse(xmlNode.Attributes.GetNamedItem("TowerFootElevation").InnerText);//塔位桩高程
                item.BaseLevelDescent = double.Parse(xmlNode.Attributes.GetNamedItem("BaseLevelDescent").InnerText);//定位高差
                item.Span = double.Parse(xmlNode.Attributes.GetNamedItem("Span").InnerText);//档距
                item.HorizontalSpan = double.Parse(xmlNode.Attributes.GetNamedItem("HorizontalSpan").InnerText);//水平档距
                item.VerticalSpan = double.Parse(xmlNode.Attributes.GetNamedItem("VerticalSpan").InnerText);//垂直档距
                item.FrontVerticalSpan = double.Parse(xmlNode.Attributes.GetNamedItem("FrontVerticalSpan").InnerText);//前侧垂直档距
                item.BackVerticalSpan = double.Parse(xmlNode.Attributes.GetNamedItem("BackVerticalSpan").InnerText);//后侧垂直档距
                item.FrontPreSpan = double.Parse(xmlNode.Attributes.GetNamedItem("FrontPreSpan").InnerText);//前侧代表档距
                item.FrontAccPreSpan = double.Parse(xmlNode.Attributes.GetNamedItem("FrontAccPreSpan").InnerText);//前侧耐张段长度
                item.BackPreSpan = double.Parse(xmlNode.Attributes.GetNamedItem("BackPreSpan").InnerText);//后侧代表档距
                item.BackAccPreSpan = double.Parse(xmlNode.Attributes.GetNamedItem("BackAccPreSpan").InnerText);//后侧耐张段长度
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

                item.CommPar = xmlNode.Attributes.GetNamedItem("CommPar").InnerText;
                item.FrontSidePar = xmlNode.Attributes.GetNamedItem("FrontSidePar").InnerText;
                item.BackSidePar = xmlNode.Attributes.GetNamedItem("BackSidePar").InnerText;
                item.TowerPar = xmlNode.Attributes.GetNamedItem("TowerPar").InnerText;

                towerSeriList.Add(item);
            }
            return towerSeriList;
        }
        #endregion


        #region 验算计算 根据所选杆塔序列读取不同模式下的相关信息
        /// <summary>
        /// 验算计算 根据所选杆塔序列读取不同模式下的相关信息
        /// </summary>
        /// <param name="projectDir">路径</param>
        /// <param name="navg">杆塔序列名称</param>
        /// <param name="Type">悬垂塔  耐张塔</param>
        /// <returns></returns>
        public static List<TowerSerial> ReadXmlBySequenceNameAndTowerType(string projectDir, string sequenceName, string Type)
        {
            //判断塔型
            string basePath = projectDir + "\\" + ConstVar.DataBaseStr + "\\TowerStr.xml";
            List<TowerStrData> towerList = TowerStrDataReader.ReadLoadFile(basePath).ToList();

            string towerTypeStr = Type == "悬垂塔" ? "悬垂塔,直线塔,直线转角塔" : "耐张塔,转角塔,终端塔";

            //读取序列信息，并进行逻辑判断
            string loadPath = projectDir + "\\" + ConstVar.TowerSequenceStr + "\\" + sequenceName + "\\TowerSequenceStr.xml";

            List<TowerSerial> towerSeriList = new List<TowerSerial>();

            XmlDocument doc = new XmlDocument();
            doc.Load(loadPath);

            XmlNode rootNode = doc.GetElementsByTagName("Root")[0];
            int index = 1;
            string towerType = "";
            foreach (XmlNode xmlNode in rootNode.ChildNodes)
            {
                try
                {
                    towerType = towerList.Where(item => item.Name == xmlNode.Attributes.GetNamedItem("TowerPattern").InnerText).First().TypeName;//塔型类型
                }
                catch (Exception)
                {
                    towerType = "";
                }
                if (towerType!=""&&towerTypeStr.Contains(towerType) && bool.Parse(xmlNode.Attributes.GetNamedItem("IsChecking").InnerText))//判断塔型及是否勾选验算
                {
                    TowerSerial item = new TowerSerial();
                    item.ID = index;//序号
                    item.TowerName = xmlNode.Attributes.GetNamedItem("TowerName").InnerText;//塔位号
                    item.NameOfPositioningPile = xmlNode.Attributes.GetNamedItem("NameOfPositioningPile").InnerText;//塔位点
                    item.TowerPattern = xmlNode.Attributes.GetNamedItem("TowerPattern").InnerText;//塔型
                    item.WireCounterWeightNum = int.Parse(xmlNode.Attributes.GetNamedItem("WireCounterWeightNum").InnerText);//导线防震锤数量
                    item.GroundCounterWeightNum = int.Parse(xmlNode.Attributes.GetNamedItem("GroundCounterWeightNum").InnerText);//地线防震锤数量
                    item.WireSpacerNum = int.Parse(xmlNode.Attributes.GetNamedItem("WireSpacerNum").InnerText);//导线间隔棒数量
                    item.EngineerPar = int.Parse(xmlNode.Attributes.GetNamedItem("EngineerPar").InnerText);//工程参数
                    item.FrontSidePar = xmlNode.Attributes.GetNamedItem("FrontSidePar").InnerText;//前侧参数
                    item.BackSidePar = xmlNode.Attributes.GetNamedItem("BackPar").InnerText;//后侧参数
                    item.CommPar = xmlNode.Attributes.GetNamedItem("CommPar").InnerText;//公共参数
                    item.TowerPar = xmlNode.Attributes.GetNamedItem("TowerPar").InnerText;//后侧相参数

                    towerSeriList.Add(item);

                    index++;
                }
            }
            return towerSeriList;
        }
        #endregion


        #region 保存 验算计算根据所选杆塔序列读取不同模式下的相关信息
        /// <summary>
        ///保存  验算计算 根据所选杆塔序列读取不同模式下的相关信息
        /// </summary>
        /// <param name="projectDir">路径</param>
        /// <param name="navg">杆塔序列名称</param>
        /// <returns></returns>
        public static void SaveXmlBySequenceNameAndTowerType(string projectDir, string sequenceName, List<TowerSerial> list)
        {
            //读取序列信息，并进行逻辑判断
            string path = projectDir + "\\" + ConstVar.TowerSequenceStr + "\\" + sequenceName + "\\TowerSequenceStr.xml";

            //加载xml文件
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            XmlNode rootNode = doc.GetElementsByTagName("Root")[0];
            foreach (TowerSerial item in list)
            {
                foreach (XmlNode xmlNode in rootNode.ChildNodes)
                {

                    if (xmlNode.Attributes.GetNamedItem("TowerName").InnerText == item.TowerName)
                    {
                        xmlNode.Attributes.GetNamedItem("WireCounterWeightNum").InnerText = item.WireCounterWeightNum.ToString();//导线防震锤数量
                        xmlNode.Attributes.GetNamedItem("GroundCounterWeightNum").InnerText = item.GroundCounterWeightNum.ToString();//地线防震锤数量
                        xmlNode.Attributes.GetNamedItem("WireSpacerNum").InnerText = item.WireSpacerNum.ToString();//导线间隔棒数量
                        xmlNode.Attributes.GetNamedItem("EngineerPar").InnerText = item.EngineerPar.ToString();//工程参数
                        xmlNode.Attributes.GetNamedItem("FrontSidePar").InnerText = item.FrontSidePar.ToString();//前侧档内参数
                        xmlNode.Attributes.GetNamedItem("BackSidePar").InnerText = item.BackSidePar.ToString();//后侧档内参数
                        xmlNode.Attributes.GetNamedItem("CommPar").InnerText = item.CommPar.ToString();//公共参数
                        xmlNode.Attributes.GetNamedItem("TowerPar").InnerText = item.TowerPar.ToString();//铁塔配置参数
                        break;
                    }
                }
            }
            doc.Save(path);
        }
        #endregion
    }
}
