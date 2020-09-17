using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using TowerLoadCals.Mode;

namespace TowerLoadCals.BLL.Electric
{
    /// <summary>
    /// 电力计算的公共资源
    /// </summary>
    public class ElecCalsCommRes
    {

        /// <summary>
        /// 
        /// </summary>
        [XmlAttribute]
        public string Name { get; set; }

        /// <summary>
        /// 1,代表传统计算模式；2，采用示例铁塔计算模式
        /// 在程序中选择（结构计算中就是标准的选择）
        /// </summary>
        [XmlAttribute]
        public int CalMode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlAttribute]
        public int Volt { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlAttribute]
        public string VoltStr { get; set; }


        /// <summary>
        /// 新线系数，默认值为0.95
        /// </summary>
        [XmlAttribute]
        public double NewPerPara { get; set; }

        /// <summary>
        /// 重力加速度常数
        /// </summary>
        [XmlAttribute]
        public double GraAcc { get; set; }

        /// <summary>
        /// 地形类型
        /// </summary>
        [XmlAttribute]
        public string Terrain { get; set; }

        /// <summary>
        /// 地形系数
        /// </summary>
        [XmlAttribute]
        public double TerrainPara { get; set; }

        /// <summary>
        /// 地形类型
        /// </summary>
        [XmlAttribute]
        public char TerType { get; set; }

        /// <summary>
        /// 导地线高空风压系数计算模式  
        /// 1.平均高度计算 2.按照挂点高减去2/3弧垂
        /// </summary>
        [XmlAttribute]
        public int HeiDDType { get; set; }

        /// <summary>
        /// 跳串高空风压系数  
        /// 1.全部按照最高高度计算 2.  考虑支撑管高度
        /// </summary>
        [XmlAttribute]
        public int HeiJmpType { get; set; }

        /// <summary>
        /// 导线截面增大系数
        /// </summary>
        [XmlAttribute]
        public double SecIndInc { get; set; }

        /// <summary>
        /// 导线重量增大系数
        /// </summary>
        [XmlAttribute]
        public double WeiIndInc { get; set; }

        /// <summary>
        /// 导线直径增大系数
        /// </summary>
        [XmlAttribute]
        public double DiaIndInc { get; set; }

        /// <summary>
        /// 地线截面增大系数
        /// </summary>
        [XmlAttribute]
        public double SecGrdInc { get; set; }

        /// <summary>
        /// 地线重量增大系数
        /// </summary>
        [XmlAttribute]
        public double WeiGrdInc { get; set; }

        /// <summary>
        /// 地线直径增大系数
        /// </summary>
        [XmlAttribute]
        public double DiaGrdInc { get; set; }

        /// <summary>
        /// OPGW截面增大系数
        /// </summary>
        [XmlAttribute]
        public double SecOPGWInc { get; set; }

        /// <summary>
        /// OPGW重量增大系数
        /// </summary>
        [XmlAttribute]
        public double WeiOPGWInc { get; set; }

        /// <summary>
        /// OPGW直径增大系数
        /// </summary>
        [XmlAttribute]
        public double DiaOPGWInc { get; set; }


        /// <summary>
        ///大张力侧施工误差,
        /// </summary>
        [XmlAttribute]
        public double BuildMaxPara { get; set; }

        /// <summary>
        /// 大张力侧安装误差
        /// </summary>
        [XmlAttribute]
        public double InstMaxPara { get; set; }

        /// <summary>
        /// 大张力侧导线伸长系数
        /// </summary>
        [XmlAttribute]
        public double IndExMaxPara { get; set; }

        /// <summary>
        ///大张力侧地线伸长系数
        /// </summary>
        [XmlAttribute]
        public double GrdExMaxPara { get; set; }

        /// <summary>
        /// 小张力侧施工误差
        /// </summary>
        [XmlAttribute]
        public double BuildMinPara { get; set; }

        /// <summary>
        /// 小张力侧安装误差
        /// </summary>
        [XmlAttribute]
        public double InstMinPara { get; set; }

        /// <summary>
        /// 小张力侧导线伸长系数
        /// </summary>
        [XmlAttribute]
        public double IndExMinPara { get; set; }

        /// <summary>
        /// 小张力侧地线伸长系数
        /// </summary>
        [XmlAttribute]
        public double GrdExMinPara { get; set; }


        /// <summary>
        /// 导线过牵引系数
        /// </summary>
        [XmlAttribute]
        public double IndODri { get; set; }

        /// <summary>
        /// 地线过牵引系数
        /// </summary>
        [XmlAttribute]
        public double GrdODri { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [XmlIgnore]
        public double IndBrePara { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlIgnore]
        public double GrdBrePara { get; set; }

        /// <summary>
        /// 类别，有电压决定
        /// </summary>
        [XmlIgnore]
        public string Catagory { get; set; }

        /// <summary>
        /// 不均匀冰覆冰率I
        /// </summary>
        [XmlIgnore]
        public double UnbaIceCoverPerI { get; set; }

        /// <summary>
        /// 不均匀冰覆冰率II
        /// </summary>
        [XmlIgnore]
        public double UnbaIceCoverPerII { get; set; }

        /// <summary>
        /// 断线覆冰率
        /// </summary>
        [XmlIgnore]
        public double BreakIceCoverPer { get; set; }
        [XmlIgnore]
        public double BreakIceCoverPerII { get; set; }

        /// <summary>
        /// 断线覆冰参数 1：考虑断线覆冰率 2：不考虑断线覆冰率
        /// </summary>
        [XmlAttribute]
        public double BreakIceCoverPara { get; set; }

        /// <summary>
        /// 导地线高空风压系数计算模式，1：线平均高(Ecex中的挂点高) 2:按照下相挂点高反算（线平均高）
        /// </summary>
        [XmlAttribute]
        public int WireWindPara { get; set; }

        /// <summary>
        /// 跳线高空风压系数计算模式，1：挂线高，2：按照跳线中点高度，硬跳线按照实际高度
        /// </summary>
        [XmlAttribute]
        public int JmpWindPara { get; set; }

        /// <summary>
        /// 地线覆冰张力计算模式，1：加5mm冰计算张力，2：不增加5mm冰计算
        /// </summary>
        [XmlAttribute]
        public int GrdIceForcePara { get; set; }

        /// <summary>
        /// 地线不平衡张力取值，1:轻冰区不考虑增加5mm，2：重冰区增加5mm
        /// </summary>
        [XmlAttribute]
        public int GrdIceUnbaPara { get; set; }

        /// <summary>
        /// 地线验算张力取值，1:不考虑增加5mm，2：考虑增加5mm
        /// </summary>
        [XmlAttribute]
        public int GrdIceCheckPara { get; set; }

        /// <summary>
        /// 锚线张力取值方法，1:取两者最大值，2:系数法，3：降温法
        /// </summary>
        [XmlAttribute]
        public int HandForcePara { get; set; }

        /// <summary>
        /// 断线张力填法：1：直线塔，0/张力差；2：耐张塔，max/(max-张力差)
        /// </summary>
        [XmlAttribute]
        public int BreakInPara { get; set; }

        /// <summary>
        /// 不均匀冰最大张力取值：1：直线塔，0/张力差；2：耐张塔，max/(max-张力差)
        /// </summary>
        [XmlAttribute]
        public int UnbaInPara { get; set; }

        /// <summary>
        /// 断线最大张力取值，1：最大允许张力，2：100%覆冰率断线情况
        /// </summary>
        [XmlAttribute]
        public int BreakMaxPara { get; set; }

        /// <summary>
        /// 不均匀冰最大张力取值，1：最大允许张力，2：100%覆冰率断线情况
        /// </summary>
        [XmlAttribute]
        public int UnbaMaxPara { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlIgnore]
        public int LevelOfLine { get; set; }

        /// <summary>
        /// 导线计算平均高
        /// </summary>
        [XmlAttribute]
        public double IndAveHei { get; set; }

        /// <summary>
        /// 地线计算平均高
        /// </summary>
        [XmlAttribute]
        public double GrdAveHei { get; set; }

        /// <summary>
        /// 跳线绝缘子串长
        /// 暂时放在这里
        /// </summary>
        [XmlIgnore]
        public double JumpStrLen { get; set; }

        /// <summary>
        /// 对地距离(m)
        /// </summary>
        [XmlAttribute]
        public double GrdCl  {get; set;}

        /// <summary>
        /// 安装垂直荷载取值
        /// </summary>
        [XmlAttribute]
        public string InsVerWeiPara { get; set; }

        /// <summary>
        /// 不均匀冰垂直荷载取值
        /// </summary>
        [XmlAttribute]
        public string UnbaIceVerWeiPara { get; set; }

        /// <summary>
        /// 手动确定高度系数
        /// </summary>
        [XmlAttribute]
        public string AutoHeiPara { get; set; } = "否";


        /// <summary>
        ///上(边)相导线μz
        /// </summary>
        [XmlAttribute]
        public double UpIndWindPara { get; set; }

        /// <summary>
        ///上(边)相串μz
        /// </summary>
        [XmlAttribute]
        public double UpStrWindPara { get; set; }

        /// <summary>
        ///上(边)相跳线绝缘子μz
        /// </summary>
        [XmlAttribute]
        public double UpJumpWindPara { get; set; }

        /// <summary>
        ///中相导线μz
        /// </summary>
        [XmlAttribute]
        public double MidIndWindPara { get; set; }

        /// <summary>
        ///中相串μz
        /// </summary>
        [XmlAttribute]
        public double MidStrWindPara { get; set; }

        /// <summary>
        ///中相跳线绝缘子μz
        /// </summary>
        [XmlAttribute]
        public double MidJumpWindPara { get; set; }


        /// <summary>
        ///下(边)相导线μz
        /// </summary>
        [XmlAttribute]
        public double DnIndWindPara { get; set; }

        /// <summary>
        ///下(边)相串μz
        /// </summary>
        [XmlAttribute]
        public double DnStrWindPara { get; set; }

        /// <summary>
        ///下(边)相跳线绝缘子μz
        /// </summary>
        [XmlAttribute]
        public double DnJumpWindPara { get; set; }

        /// <summary>
        ///地线μz
        /// </summary>
        [XmlAttribute]
        public double GrdWindPara { get; set; }

        /// <summary>
        ///地线串μz
        /// </summary>
        [XmlAttribute]
        public double GrdStrWindPara { get; set; }


        public ElecCalsCommRes()
        {
            SetComPara();
            CalMode = 1;
            Volt = 0;
        }


        void SetComPara(double newPerPara= 0.95, double graAcc= 9.80665, char terType= 'B', int heiDDType= 1, 
            int heiJmpType= 1, double indAveHei= 0, double grdAveHei = 0)
        {
            //新线系数，默认值为0.95
            NewPerPara = newPerPara;
            //重力加速度常数
            GraAcc = graAcc;
            //地形类型 
            TerType = terType;
            //导地线高空风压系数计算模式  1.平均高度计算 2.按照挂点高减去2/3弧垂
            HeiDDType = heiDDType;
            //跳串高空风压系数  1.全部按照最高高度计算 2.  考虑支撑管高度
            HeiJmpType = heiJmpType;
            SetAverage(indAveHei, grdAveHei);
        }


        /// <summary>
        /// #导\地\OPGW增大系数,Sec代表界面,Wei代表重量，Dia代表直径
        /// </summary>
        /// <param name="secIndInc"> </param>
        /// <param name="weiIndInc"></param>
        /// <param name="diaIndInc"></param>
        /// <param name="secGrdInc"></param>
        /// <param name="weiGrdInc"></param>
        /// <param name="diaGrdInc"></param>
        /// <param name="secOPGWInc"></param>
        /// <param name="weiOPGWInc"></param>
        /// <param name="diaOPGWInc"></param>
        public void SetForIncrPara(double secIndInc = 1, double weiIndInc = 1, double diaIndInc = 1, double secGrdInc = 1,
            double weiGrdInc = 1, double diaGrdInc = 1, double secOPGWInc = 1, double weiOPGWInc = 1, double diaOPGWInc = 1)
        {
            SecIndInc = secIndInc;
            WeiIndInc = weiIndInc;
            DiaIndInc = diaIndInc;
            SecGrdInc = secGrdInc;
            WeiGrdInc = weiGrdInc;
            DiaGrdInc = diaGrdInc;
            SecOPGWInc = secOPGWInc;
            WeiOPGWInc = weiOPGWInc;
            DiaOPGWInc = diaOPGWInc;
        }


        /// <summary>
        /// 张力系数，Build:施工误差,Inst:安装误差，IndEx：导线伸长系数，GrdEx：地线伸长系数
        /// </summary>
        /// <param name="buildMaxPara"></param>
        /// <param name="instMaxPara"></param>
        /// <param name="indExMaxPara"></param>
        /// <param name="grdExMaxPara"></param>
        /// <param name="buildMinPara"></param>
        /// <param name="instMinPara"></param>
        /// <param name="indExMinPara"></param>
        /// <param name="grdExMinPara"></param>
        public void SetForMaxMinPara(double buildMaxPara, double instMaxPara, double indExMaxPara, double grdExMaxPara,
            double buildMinPara, double instMinPara, double indExMinPara, double grdExMinPara)
        {
            BuildMaxPara = buildMaxPara;
            InstMaxPara = instMaxPara;
            IndExMaxPara = indExMaxPara;
            GrdExMaxPara = grdExMaxPara;

            BuildMinPara = buildMinPara;
            InstMinPara = instMinPara;
            IndExMinPara = indExMinPara;
            GrdExMinPara = grdExMinPara;
        }

        /// <summary>
        /// 过牵引系数,导线默认为0.2，地线默认为0.1
        /// </summary>
        /// <param name="indODri"></param>
        /// <param name="grdODri"></param>
        public void SetOverDrive(double indODri= 0.2f, double grdODri= 0.1f)
        {
            IndODri = indODri;
            GrdODri = grdODri;
        }

        /// <summary>
        /// 断线张力系数
        /// </summary>
        /// <param name="indBrePara"></param>
        /// <param name="grdBrePara"></param>
        public void SetBreakWirePara(double indBrePara = 0.7f, double grdBrePara = 1)
        {
            IndBrePara = indBrePara;
            GrdBrePara = grdBrePara;
        }

        /// <summary>
        /// 不均匀冰覆冰率，断线覆冰率
        /// </summary>
        /// <param name="unbalanceMaxPer"></param>
        /// <param name="unbalanceMinPer"></param>
        /// <param name="breakPer"></param>
        public void SetIcePercent(double unbalanceMaxPer= 1, double unbalanceMinPer= 0, double breakPer= 1)
        {
            UnbaIceCoverPerI = unbalanceMaxPer;
            UnbaIceCoverPerI = unbalanceMinPer;
            BreakIceCoverPer = breakPer;
        }

        /// <summary>
        /// 计算方法参数 耐张塔
        /// </summary>
        /// <param name="wireWindPara">导地线高空风压系数计算模式，1：线平均高 2:按照下相挂点高反算</param>
        /// <param name="jmpWindPara">跳线高空风压系数计算模式，1：挂线高，2：按照跳线中点高度，硬跳线按照实际高度</param>
        /// <param name="grdIceForcePara">地线覆冰张力计算模式，1：加5mm冰计算张力，2：不增加5mm冰计算</param>
        /// <param name="grdIceUnbaPara">地线不平衡张力取值，1:轻冰区不考虑增加5mm，2：重冰区增加5mm</param>
        /// <param name="grdIceCheckPara">地线验算张力取值，1:不考虑增加5mm，2：考虑增加5mm</param>
        /// <param name="handForcePara">锚线张力取值方法，1:取两者最大值，2:系数法，3：降温法</param>
        /// <param name="breakInPara">断线张力填法：1：直线塔，0/张力差；2：耐张塔，max/(max-张力差)</param>
        /// <param name="unbaInPara">不均匀冰张力取值，1：直线塔，0/张力差；2：耐张塔，max/(max-张力差)</param>
        /// <param name="breakMaxPara">断线最大张力取值，1：最大允许张力，2：100%覆冰率断线情况</param>
        /// <param name="unbaMaxPara">不均匀冰最大张力取值，1：最大允许张力，2：100%覆冰率断线情况</param>
        public void CalMethodParaStrain(int wireWindPara= 1, int jmpWindPara= 1, int grdIceForcePara= 1, int grdIceUnbaPara= 1,
            int grdIceCheckPara= 1, int handForcePara= 1, int breakInPara= 1, int unbaInPara = 1, int breakMaxPara= 1, int unbaMaxPara= 1)
        {
            WireWindPara = wireWindPara;
            JmpWindPara = jmpWindPara;
            GrdIceForcePara = grdIceForcePara;
            GrdIceUnbaPara = grdIceUnbaPara;
            GrdIceCheckPara = grdIceCheckPara;
            HandForcePara = handForcePara;
            BreakInPara = breakInPara;
            UnbaInPara = unbaInPara;
            BreakMaxPara = breakMaxPara;
            UnbaMaxPara = unbaMaxPara;      
        }


        /// <summary>
        /// 计算方法参数 悬垂塔
        /// </summary>
        /// <param name="grdIceForcePara">地线覆冰张力计算模式，1：加5mm冰计算张力，2：不增加5mm冰计算</param>
        /// <param name="grdIceUnbaPara">地线不平衡张力取值，1:轻冰区不考虑增加5mm，2：重冰区增加5mm</param>
        /// <param name="grdIceCheckPara">地线验算张力取值，1:不考虑增加5mm，2：考虑增加5mm</param>
        /// <param name="iceThichkness">最大覆冰（电线覆冰）的冰厚</param>
        /// <param name="breakMaxPara">断线最大张力取值，1：最大允许张力，2：100%覆冰率断线情况</param>
        /// <param name="unbaInPara">不均匀冰最大张力取值，1：最大允许张力，2：100%覆冰率断线情况</param>
        public void CalMethodParaHang(int grdIceForcePara = 1, int grdIceUnbaPara = 1, int grdIceCheckPara = 1, double iceThichkness = 0, 
             int breakMaxPara = 1, int unbaInPara = 1)
        {
            GrdIceForcePara = grdIceForcePara;
            GrdIceUnbaPara = grdIceUnbaPara;
            GrdIceCheckPara = grdIceCheckPara;

            BreakMaxPara = breakMaxPara;
            UnbaMaxPara = iceThichkness <= 15 ? 1 : 2; 
            BreakInPara = iceThichkness <= 15 ? 2 : 1;
            UnbaInPara = unbaInPara;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ProjeIn"></param>
        public void UpdataLevelOfLine(ProInfo projeIn)
        {
            LevelOfLine = ElecCalsToolBox .GetLevelOfLine(projeIn.Volt, projeIn.ACorDC);
        }

        public void SetAverage(double indAveHei= 0, double grdAveHei= 0)
        {
            if (indAveHei <= 0)
            {
                //如果平均高度未设置，那么根据电压等级取值,此处平均高度是计算应力弧垂
                IndAveHei = ElecCalsToolBox.AveHeightDefault(Volt);
            }
            else
            {
                IndAveHei = indAveHei;
            }

            if(grdAveHei <= 0)
            {
                GrdAveHei = IndAveHei;
            }
            else
            {
                GrdAveHei = grdAveHei;
            }
        }

        /// <summary>
        /// 更新耐张塔的覆冰率
        /// </summary>
        /// <param name="towerType"></param>
        /// <param name="backWkCdts"></param>
        /// <param name="backIceArea"></param>
        /// <param name="frontWkCdts"></param>
        /// <param name="frontIceArea"></param>
        public void  UpateIceCovrage(string towerType, List<ElecCalsWorkCondition> backWkCdts, string backIceArea, List<ElecCalsWorkCondition> frontWkCdts, string frontIceArea)
        {
            var backIceWkCdt = backWkCdts.Where(item => item.Name == "最大覆冰").FirstOrDefault();
            double backIceThick = backIceWkCdt == null ? 0 : backIceWkCdt.IceThickness;

            var frontIceWkCdt = frontWkCdts.Where(item => item.Name == "最大覆冰").FirstOrDefault();
            double frontIceThick = frontIceWkCdt == null ? 0 : frontIceWkCdt.IceThickness;

            Catagory = ElecCalsToolBox.GetCatogory(Volt.ToString());
            //BreakIceCoverPer = ElecCalsToolBox.UBlanceR(towerType, backIceThick, frontIceThick, Catagory);
            BreakIceCoverPer = ElecCalsToolBox.UBlanceR(towerType, Math.Max(backIceThick, frontIceThick), Catagory);
            //1：考虑断线覆冰率 2：不考虑断线覆冰率
            BreakIceCoverPara = (Math.Max(backIceThick, frontIceThick) >= 20) ? 1 : 2;

            if (backIceThick != frontIceThick)
            {
                UnbaIceCoverPerI = 1;
                UnbaIceCoverPerII = 0;
            }
            else
            {
                if (backIceArea == "重冰区" || frontIceArea == "重冰区" || backIceArea == "中冰区" || frontIceArea == "中冰区")
                {
                    UnbaIceCoverPerI = ElecCalsToolBox.IBlanceR1(towerType, Catagory);
                    UnbaIceCoverPerII = ElecCalsToolBox.IBlanceR2(towerType, Catagory);
                }
                else
                {
                    UnbaIceCoverPerI = 1;
                    UnbaIceCoverPerII = 1;
                }
            }
        }

        /// <summary>
        /// 更新悬垂塔的的覆冰率
        /// </summary>
        /// <param name="wkDdts"></param>
        /// <param name="iceArea"></param>
        public void UpateIceCovrage(List<ElecCalsWorkCondition> wkDdts, string iceArea)
        {
            //在悬垂塔的BreakIceCoverPerII，UnbaIceCoverPerII指的应该是地线开断的情况
            var backIceWkCdt = wkDdts.Where(item => item.Name == "最大覆冰").FirstOrDefault();
            double iceThick = backIceWkCdt == null ? 0 : backIceWkCdt.IceThickness;

            Catagory = ElecCalsToolBox.GetCatogory(Volt.ToString());
            BreakIceCoverPer = ElecCalsToolBox.UBlanceR("悬垂塔", iceThick, Catagory);
            BreakIceCoverPerII = ElecCalsToolBox.UBlanceR("耐张塔", iceThick, Catagory);

            //1：考虑断线覆冰率 2：不考虑断线覆冰率
            BreakIceCoverPara = iceThick >= 20  ? 1 : 2;

            if (iceArea == "轻冰区")
            {
                UnbaIceCoverPerI = 1;
                UnbaIceCoverPerII = 1;
            }
            else
            {
                UnbaIceCoverPerI = ElecCalsToolBox.IBlanceR2("悬垂塔", Catagory);
                UnbaIceCoverPerII = ElecCalsToolBox.IBlanceR2("耐张塔", Catagory);
            }
        }

        public string PrintIceCovrageStrain()
        {
            return "\n断线覆冰率: " + BreakIceCoverPer.ToString("0.00") + "  " + (BreakIceCoverPara == 1 ? "考虑断线覆冰率" : "不考虑断线覆冰率")
                + "    不均匀冰覆冰率: " + UnbaIceCoverPerI.ToString("0.00") + "  " + UnbaIceCoverPerII.ToString("0.00") + "    类别: " + Catagory;
        }

        public string PrintIceCovrageHang()
        {
            return "\n断线覆冰率: " + BreakIceCoverPer.ToString("0.00") + "  "  + BreakIceCoverPerII.ToString("0.00") + "  " + (BreakIceCoverPara == 1 ? "考虑断线覆冰率" : "不考虑断线覆冰率")
                + "    不均匀冰覆冰率: " + UnbaIceCoverPerI.ToString("0.00") + "  " + UnbaIceCoverPerII.ToString("0.00") + "    类别: " + Catagory;
        }

    }
}
