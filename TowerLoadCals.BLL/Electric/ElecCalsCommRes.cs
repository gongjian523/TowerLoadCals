using System;
using System.Collections.Generic;
using System.Linq;
using TowerLoadCals.Mode;

namespace TowerLoadCals.BLL.Electric
{
    /// <summary>
    /// 电力计算的公共资源
    /// </summary>
    public class ElecCalsCommRes
    {
        /// <summary>
        /// 1,代表传统计算模式；2，采用示例铁塔计算模式
        /// 在程序中选择（结构计算中就是标准的选择）
        /// </summary>
        public int CalMode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Volt { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string VoltStr { get; set; }


        /// <summary>
        /// 新线系数，默认值为0.95
        /// </summary>
        public double NewPerPara { get; set; }

        /// <summary>
        /// 重力加速度常数
        /// </summary>
        public double GraAcc { get; set; }

        /// <summary>
        /// 地形类型
        /// </summary>
        public string Terrain { get; set; }

        /// <summary>
        /// 地形系数
        /// </summary>
        public double TerrainPara { get; set; }

        /// <summary>
        /// 地形类型
        /// </summary>
        public char TerType { get; set; }


        /// <summary>
        /// 导地线高空风压系数计算模式  
        /// 1.平均高度计算 2.按照挂点高减去2/3弧垂
        /// </summary>
        public int HeiDDType { get; set; }

        /// <summary>
        /// 跳串高空风压系数  
        /// 1.全部按照最高高度计算 2.  考虑支撑管高度
        /// </summary>
        public int HeiJmpType { get; set; }

        /// <summary>
        /// 导线截面增大系数
        /// </summary>
        public double SecIndInc { get; set; }

        /// <summary>
        /// 导线重量增大系数
        /// </summary>
        public double WeiIndInc { get; set; }

        /// <summary>
        /// 导线直径增大系数
        /// </summary>
        public double DiaIndInc { get; set; }

        /// <summary>
        /// 地线截面增大系数
        /// </summary>
        public double SecGrdInc { get; set; }

        /// <summary>
        /// 地线重量增大系数
        /// </summary>
        public double WeiGrdInc { get; set; }

        /// <summary>
        /// 地线直径增大系数
        /// </summary>
        public double DiaGrdInc { get; set; }

        /// <summary>
        /// OPGW截面增大系数
        /// </summary>
        public double SecOPGWInc { get; set; }

        /// <summary>
        /// OPGW重量增大系数
        /// </summary>
        public double WeiOPGWInc { get; set; }

        /// <summary>
        /// OPGW直径增大系数
        /// </summary>
        public double DiaOPGWInc { get; set; }


        /// <summary>
        ///大张力侧施工误差,
        /// </summary>
        public double BuildMaxPara { get; set; }

        /// <summary>
        /// 大张力侧安装误差
        /// </summary>
        public double InstMaxPara { get; set; }

        /// <summary>
        /// 大张力侧导线伸长系数
        /// </summary>
        public double IndExMaxPara { get; set; }

        /// <summary>
        ///大张力侧地线伸长系数
        /// </summary>
        public double GrdExMaxPara { get; set; }

        /// <summary>
        /// 小张力侧施工误差
        /// </summary>
        public double BuildMinPara { get; set; }

        /// <summary>
        /// 小张力侧安装误差
        /// </summary>
        public double InstMinPara { get; set; }

        /// <summary>
        /// 小张力侧导线伸长系数
        /// </summary>
        public double IndExMinPara { get; set; }

        /// <summary>
        /// 小张力侧地线伸长系数
        /// </summary>
        public double GrdExMinPara { get; set; }


        /// <summary>
        /// 导线过牵引系数
        /// </summary>
        public double IndODri { get; set; }

        /// <summary>
        /// 地线过牵引系数
        /// </summary>
        public double GrdODri { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public double IndBrePara { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double GrdBrePara { get; set; }

        /// <summary>
        /// 类别，有电压决定
        /// </summary>
        public string Catagory { get; set; }

        /// <summary>
        /// 不均匀冰覆冰率I
        /// </summary>
        public double UnbaIceCoverPerI { get; set; }

        /// <summary>
        /// 不均匀冰覆冰率II
        /// </summary>
        public double UnbaIceCoverPerII { get; set; }

        /// <summary>
        /// 断线覆冰率
        /// </summary>
        public double BreakIceCoverPer { get; set; }

        /// <summary>
        /// 断线覆冰参数 1：考虑断线覆冰率 2：不考虑断线覆冰率
        /// </summary>
        public double BreakIceCoverPara { get; set; }

        /// <summary>
        /// 导地线高空风压系数计算模式，1：线平均高 2:按照下相挂点高反算
        /// </summary>
        public int WireWindPara { get; set; }

        /// <summary>
        /// 跳线高空风压系数计算模式，1：挂线高，2：按照跳线中点高度，硬跳线按照实际高度
        /// </summary>
        public int JmpWindPara { get; set; }

        /// <summary>
        /// 地线覆冰张力计算模式，1：加5mm冰计算张力，2：不增加5mm冰计算
        /// </summary>
        public int GrdIceForcePara { get; set; }

        /// <summary>
        /// 地线不平衡张力取值，1:轻冰区不考虑增加5mm，2：重冰区增加5mm
        /// </summary>
        public int GrdIceUnbaPara { get; set; }

        /// <summary>
        /// 地线验算张力取值，1:不考虑增加5mm，2：考虑增加5mm
        /// </summary>
        public int GrdIceCheckPara { get; set; }

        /// <summary>
        /// 锚线张力取值方法，1:取两者最大值，2:系数法，3：降温法
        /// </summary>
        public int HandForcePara { get; set; }

        /// <summary>
        /// 断线张力填法：1：直线塔，0/张力差；2：耐张塔，max/(max-张力差)
        /// </summary>
        public int BreakInPara { get; set; }

        /// <summary>
        /// 不均匀冰最大张力取值：1：直线塔，0/张力差；2：耐张塔，max/(max-张力差)
        /// </summary>
        public int UnbaInPara { get; set; }

        /// <summary>
        /// 断线最大张力取值，1：最大允许张力，2：100%覆冰率断线情况
        /// </summary>
        public int BreakMaxPara { get; set; }

        /// <summary>
        /// 不均匀冰最大张力取值，1：最大允许张力，2：100%覆冰率断线情况
        /// </summary>
        public int UnbaMaxPara { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int LevelOfLine { get; set; }

        /// <summary>
        /// 导线计算平均高
        /// </summary>
        public double IndAveHei { get; set; }

        /// <summary>
        /// 地线计算平均高
        /// </summary>
        public double GrdAveHei { get; set; }

        /// <summary>
        /// 跳线绝缘子串长
        /// 暂时放在这里
        /// </summary>
        public double JumpStrLen { get; set; }

        /// <summary>
        /// 对地距离(m)
        /// </summary>
        public double GrdCl  {get; set;}

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
        /// 计算方法参数
        /// </summary>
        /// <param name="wireWindPara">导地线高空风压系数计算模式，1：线平均高 2:按照下相挂点高反算</param>
        /// <param name="jmpWindPara">跳线高空风压系数计算模式，1：挂线高，2：按照跳线中点高度，硬跳线按照实际高度</param>
        /// <param name="grdIceForcePara">地线覆冰张力计算模式，1：加5mm冰计算张力，2：不增加5mm冰计算</param>
        /// <param name="grdIceUnbaPara">地线不平衡张力取值，1:轻冰区不考虑增加5mm，2：重冰区增加5mm</param>
        /// <param name="grdIceCheckPara">地线验算张力取值，1:不考虑增加5mm，2：考虑增加5mm</param>
        /// <param name="handForcePara">锚线张力取值方法，1:取两者最大值，2:系数法，3：降温法</param>
        /// <param name="breakInPara">断线张力填法：1：直线塔，0/张力差；2：耐张塔，max/(max-张力差)</param>
        /// <param name="unbaMaxPara">不均匀冰张力取值，1：直线塔，0/张力差；2：耐张塔，max/(max-张力差)</param>
        /// <param name="breakMaxPara">断线最大张力取值，1：最大允许张力，2：100%覆冰率断线情况</param>
        /// <param name="unbaMaxPara">不均匀冰最大张力取值，1：最大允许张力，2：100%覆冰率断线情况</param>
        public void CalMethodPara(int wireWindPara= 1, int jmpWindPara= 1, int grdIceForcePara= 1, int grdIceUnbaPara= 1,
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


        public void  UpateIceCovrage(string towerType, List<ElecCalsWorkCondition> backWkCdts, string backIceArea, List<ElecCalsWorkCondition> frontWkCdts, string frontIceArea)
        {
            double backIceThick = 0, frontIceThick = 0;

            if(backWkCdts.Where(item => item.Name == "最大覆冰").Count() > 0) {
                backIceThick = backWkCdts.Where(item => item.Name == "最大覆冰").First().IceThickness;
        }
            if (frontWkCdts.Where(item => item.Name == "最大覆冰").Count() > 0)
        {
                frontIceThick = frontWkCdts.Where(item => item.Name == "最大覆冰").First().IceThickness;
        }

            Catagory = ElecCalsToolBox.GetCatogory(Volt.ToString());
            BreakIceCoverPer = ElecCalsToolBox.UBlanceR(towerType, backIceThick, frontIceThick, Catagory);
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

        public string PrintIceCovrage()
        {
            return "\n断线覆冰率: " + BreakIceCoverPer.ToString("0.00") + "  " + (BreakIceCoverPer == 1 ? "考虑断线覆冰率" : "不考虑断线覆冰率")
                + "    不均匀冰覆冰率: " + UnbaIceCoverPerI.ToString("0.00") + "  " + UnbaIceCoverPerII.ToString("0.00") + "    类别: " + Catagory;
    }

    }
}
