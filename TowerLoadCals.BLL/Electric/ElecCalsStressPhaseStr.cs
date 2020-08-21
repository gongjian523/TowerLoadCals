using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TowerLoadCals.Common;
using TowerLoadCals.Common.Utils;
using TowerLoadCals.Mode;
using TowerLoadCals.Mode.Electric;

namespace TowerLoadCals.BLL.Electric
{
    public class ElecCalsStressPhaseStr
    {
        /// <summary>
        /// 相位置标记
        /// </summary>
        public int PhasePosType { get; set; }

        /// <summary>
        /// 前侧导线挂线串
        /// </summary>
        public ElecCalsStrData HangStr { get; set; }

        /// <summary>
        /// 跳线配置
        /// </summary>
        public ElecCalsStrData JumpStr { get; set; }

        /// <summary>
        /// 回路ID
        /// </summary>
        public int LoopID { get; set; }

        /// <summary>
        ///  相ID
        /// </summary>
        public int PhaseID { get; set; }

        /// <summary>
        /// 荷载列表，包括工况名称
        /// </summary>
        public List<string> LoadList { get; set; }

        /// <summary>
        /// 串长
        /// </summary>
        public double BaseStringEquLength { get; set; }


        /// <summary>
        /// 空间位置数据
        /// </summary>
        public PhaseSpaceStrUtils SpaceStr { get; set; } = new PhaseSpaceStrUtils();

        /// <summary>
        ///  线风压系数
        /// </summary>
        public double WireWindPara { get; set; }

        /// <summary>
        /// 绝缘子串风压系数
        /// </summary>
        public double StrWindPara { get; set; }

        /// <summary>
        ///  跳线风压系数
        /// </summary>
        public double JmWindPara { get; set; }

        /// <summary>
        /// 支撑管风压系数
        /// </summary>
        public double PropUpWindPara { get; set; }

        /// <summary>
        ///  线计算数据
        /// </summary>
        public ElecCalsWire WireData { get; set; }


        /// <summary>
        /// 跳线计算数据
        /// </summary>
        public ElecCalsWire JmWireData { get; set; }

        [XmlIgnore]
        public Dictionary<string, double> VerSpanDic { get; set; }

        [XmlIgnore]
        public Dictionary<string, double> HoriLoadDic { get; set; }

        /// <summary>
        /// 绝缘子串的荷载
        /// </summary>
        [XmlIgnore]
        public Dictionary<string, StrLoadResult> StrLoad { get; set; } = new Dictionary<string, StrLoadResult>();

        /// <summary>
        /// 受风面积
        /// </summary>
        public double WindArea { get; set; }

        /// <summary>
        /// 跳线绝缘子串的风荷载
        /// </summary>
        [XmlIgnore]
        public Dictionary<string, JumpStrLoadResult> JumpStrLoad { get; set; } = new Dictionary<string, JumpStrLoadResult>();


        public ElecCalsStressPhaseStr()
        {
        }


        /// <summary>
        /// 计算绝缘子串的风荷载和垂直荷载
        /// </summary>
        public void CalsStrLoad()
        {
            foreach (var weaItem in WireData.WeatherParas.WeathComm)
            {
                double wload = Math.Round(ElecCalsToolBox2.StringWind(HangStr.PieceNum, HangStr.LNum, HangStr.GoldPieceNum, weaItem.IceThickness, weaItem.WindSpeed, weaItem.BaseWindSpeed), 3);
                double vload = HangStr.Weight + WeightIceIn(weaItem.IceThickness) * (HangStr.PieceNum * HangStr.LNum + HangStr.GoldPieceNum);

                StrLoad.Add(weaItem.Name, new StrLoadResult
                {
                    WindLoad = wload,
                    VerLoad = vload,
                });
            }

            WindArea = 0.04 * (HangStr.PieceNum * HangStr.LNum + HangStr.GoldPieceNum);
        }

        /// <summary>
        /// 计算跳线绝缘子串的风荷载
        /// </summary>
        /// <param name="volt">电压</param>
        /// <param name="anSideWkCdtList">另一侧的工况</param>
        public void CalsWindLoad(string volt, List<ElecCalsWorkCondition>  anSideWkCdtList)
        {
            foreach (var weaItem in WireData.WeatherParas.WeathComm)
            {
                var anWea = anSideWkCdtList.Where(wea => wea.Name == weaItem.Name).FirstOrDefault();

                JumpStrLoadResult rslt = new JumpStrLoadResult();

                rslt.Temperature = weaItem.Temperature;

                //冰厚，风速和基本风速需要比较大号侧和小号侧相应的工况，取其中的较大值
                rslt.IceThickness = (anWea != null && anWea.IceThickness > weaItem.IceThickness) ? anWea.IceThickness : weaItem.IceThickness;
                rslt.WindSpeed = (anWea != null && anWea.WindSpeed > weaItem.WindSpeed) ? anWea.WindSpeed : weaItem.WindSpeed;
                rslt.BaseWindSpeed = (anWea != null && anWea.BaseWindSpeed > weaItem.BaseWindSpeed) ? anWea.BaseWindSpeed : weaItem.BaseWindSpeed;

                rslt.JumpStrWindLoad = Math.Round(ElecCalsToolBox2.StringWind(JumpStr.PieceNum, JumpStr.LNum, JumpStr.GoldPieceNum, rslt.IceThickness, rslt.WindSpeed, rslt.BaseWindSpeed), 3);
                rslt.JumpWindLoad = Math.Round(ElecCalsToolBox2.WindPaT(volt, JmWireData.Dia, rslt.IceThickness, rslt.WindSpeed, rslt.BaseWindSpeed)/1000, 3) * JumpStr.SoftLineLen;
                rslt.SuTubleWindLoad = Math.Round(ElecCalsToolBox2.WindPaT(volt, JumpStr.SuTubleDi, rslt.IceThickness, rslt.WindSpeed, rslt.BaseWindSpeed)/1000, 3) * JumpStr.SuTubleLen;

                JumpStrLoad.Add(weaItem.Name, rslt);
            }
        }

        /// <summary>
        /// 计算垂直档距
        /// </summary>
        /// 
        public void UpdateVertialSpan(double span, double upSideInHei)
        {
            foreach (var nameWd in WireData.WeatherParas.NameOfWkCdt)
            {
                BZResult bz = WireData.BzDic[nameWd];

                //这儿比载excel中用的是孤立档应力，用的是普通档应力
                //upSideInHei  这儿全部用的是上相导线高差， 有可能应该用的是自己的项的高差
                double rslt = VerticalSpan(span, upSideInHei, bz.Stress, bz.BiZai);

                VerSpanDic.Add(nameWd, rslt);
            }
        }

        /// <summary>
        /// 垂直档距
        /// </summary>
        /// <param name="span">档距</param>
        /// <param name="upSideInHei">上相导线高差</param>
        /// <param name="stress">孤立档应力</param>
        /// <param name="specLoad">比载</param>
        /// <returns></returns>
        protected double VerticalSpan(double span, double upSideInHei, double stress, double specLoad)
        {
            return Math.Round(span / 2 + stress / specLoad * Calc.Asinh(specLoad * upSideInHei / (2 * stress * Math.Sinh(specLoad * span / 2 / stress))), 2);
        }


        public void UpdateHeriLoad(double diaInc, double verSpan, double dampLength, double windLoad, double strWindLaod)
        {
            foreach (var nameWd in WireData.WeatherParas.NameOfWkCdt)
            {
                BZResult bz = WireData.BzDic[nameWd];

                //这儿比载excel中用的是孤立档应力，用的是普通档应力
                //upSideInHei  这儿全部用的是上相导线高差， 有可能应该用的是自己的项的高差
                double rslt = HoriLoad(diaInc, WireData.DevideNum, verSpan,  dampLength,  windLoad, strWindLaod);

                HoriLoadDic.Add(nameWd, rslt);
            }
        }


        /// <summary>
        /// 水平荷载
        /// </summary>
        /// <param name="diaInc">直径增大系数</param>
        /// <param name="devideNum"> 分裂系数</param>
        /// <param name="verSpan">水平档距</param>
        /// <param name="dampLength">阻尼线长</param>
        /// <param name="windLoad">风荷载, 来源用wire,Bizai中的风荷载</param>
        /// <param name="strWindLaod">绝缘子串的风荷载</param>
        /// <returns></returns>
        protected double HoriLoad(double diaInc, double devideNum, double verSpan, double dampLength, double windLoad, double strWindLaod)
        {
            return Math.Round((diaInc * WireWindPara * devideNum * (verSpan / 2 + dampLength) * windLoad + StrWindPara * strWindLaod), 2);
        }


        /// <summary>
        /// 垂直荷载
        /// </summary>
        /// <param name="weightInc">重量增大 来自公共参数</param>
        /// <param name="devideNum">分裂系数</param>
        /// <param name="verLoad">垂直荷载 来源用wire,Bizai中的垂直荷载</param>
        /// <param name="计算过程C4">垂直档距，就是前面算的</param>
        /// <param name="strVerLoad">绝缘子串的垂直荷载</param>
        /// <param name="intervalTubeNum">每相导线间隔棒数</param>
        /// <param name="spaceBatonWei">导线间隔棒重</param>
        /// <param name="antiVibraHamWei">导线防振锤重</param>
        /// <param name="iceThick">覆冰厚度 来源用wire.wea</param>
        /// <param name="antiViraNum">每相导线防震锤</param>
        /// <param name="dampLength">阻尼线长</param>
        /// <returns></returns>
        protected double VerLoad(double weightInc, double devideNum, double verLoad, double 计算过程C4, double strVerLoad, int spacerBatonNum
            , double spaceBatonWei, double antiVibraHamWei, double iceThick, double antiViraNum, double dampLength)
        {
            return weightInc * devideNum * verLoad * 计算过程C4 * +strVerLoad + spacerBatonNum * (spaceBatonWei + iceThick / 5) 
                + antiViraNum *(antiVibraHamWei + iceThick /5) + verLoad * dampLength;
        }


        /// <summary>
        /// 跳线串水平荷载
        /// </summary>
        /// <param name="jumpStrWindLoad">跳线绝缘子串风荷载</param>
        /// <param name="jumpWinLoad">跳线绝缘子串风荷载</param>
        /// <param name="jumpDeviedeNum">跳线分裂系数</param>
        /// <param name="PropUpWindLoad">支撑管风荷载</param>
        /// <returns></returns>
        protected double JumpHoriLoad(double jumpStrWindLoad,  double jumpWinLoad, double PropUpWindLoad, int jumpDeviedeNum)
        {
            return jumpStrWindLoad * JmWindPara + jumpWinLoad * JmWindPara * jumpDeviedeNum + PropUpWindLoad * PropUpWindPara;
        }

        /// <summary>
        /// 跳线串垂直荷载
        /// </summary>
        /// <param name="jumpStrWei">跳线串重 B44 </param>
        /// <param name="pieceNum">绝缘子片数 D44 </param>
        /// <param name="lNum">联数 C44 </param>
        /// <param name="goldPieceNum">导地线金具折片数 E44 </param>
        /// <param name="suTubleLen">支撑管长度 F44 </param>
        /// <param name="spaceBatonNum">间隔棒数 H44</param>
        /// <param name="softJumpLen">单相单根软跳线长 G44</param>
        /// <param name="spaceBatonWei">导线间隔棒重 A48  </param>
        /// <param name="jumpDivideNum">跳线分裂数 E48 </param>
        /// <param name="suTubleWei">支撑管单重 H48</param>
        /// <param name="suTubleDia">支撑管直径 G48</param>
        /// <param name="unitWei">跳线的单位重量 F139 </param>
        /// <param name="dia">跳线的直径 C139</param>
        /// <param name="iceThick">此工况对应的冰厚 D141 </param>
        /// <returns></returns>
        public double JumpVerLoad(double jumpStrWei, double pieceNum, double lNum, double goldPieceNum,  double suTubleLen, double softJumpLen, double spaceBatonNum,  double iceThick, 
            double unitWei, double dia, double spaceBatonWei, double jumpDivideNum, double suTubleWei, double suTubleDia)
        {
            return jumpStrWei + (pieceNum * lNum + goldPieceNum) * WeightIceIn(iceThick) + (unitWei + 2.8274334 * iceThick * (dia + iceThick) / 1000) * softJumpLen * jumpDivideNum 
                + (suTubleWei + 2.82743334 * iceThick * (suTubleDia + iceThick) / 1000) * suTubleLen + spaceBatonNum * (spaceBatonWei + iceThick / 5);
        }


        public List<string> PrintStrLoad()
        {
            List<string> rslt = new List<string>();

            string str = FileUtils.PadRightEx("受风面积:" + WindArea.ToString("0.##"), 16);
            rslt.Add(str);

            string strTitle = FileUtils.PadRightEx("气象条件", 26) + FileUtils.PadRightEx("温度：", 8) + FileUtils.PadRightEx("风速：", 8) + FileUtils.PadRightEx("覆冰：", 8)
                + FileUtils.PadRightEx("基本风速：", 12) + FileUtils.PadRightEx("风荷载：", 12) + FileUtils.PadRightEx("垂直荷载：", 12);
            rslt.Add(strTitle);

            foreach (var name in WireData.WorkCdtNames)
            {
                var wea = WireData.WeatherParas.WeathComm.Where(item => item.Name == name).FirstOrDefault();

                if (wea == null)
                    continue;

                string strValue = FileUtils.PadRightEx(name, 26) + FileUtils.PadRightEx(wea.Temperature.ToString(), 8) + FileUtils.PadRightEx(wea.WindSpeed.ToString(), 8) + FileUtils.PadRightEx(wea.IceThickness.ToString(), 8)
                    + FileUtils.PadRightEx(wea.BaseWindSpeed.ToString(), 12) + FileUtils.PadRightEx(StrLoad[name].WindLoad.ToString("0.###"), 12) + FileUtils.PadRightEx(StrLoad[name].VerLoad.ToString("0.###"), 12);
                rslt.Add(strValue);
            }

            return rslt;
        }



        protected double WeightIceIn(double iceThick)
        {
            //Dim exZai As Boolean
            //Dim IceBank As Double
            //exZai = False
            //For i = 0 To 6
            //    IceBank = AAIce.Cells(1, 1 + i).Value
            //    If Abs(IceBank -Ice) < 0.001 Then
            //       WeightIceIn = AAIce.Cells(2, 1 + i).Value
            //        exZai = True
            //    End If
            //Next i

            return (iceThick / 5);
        }





    }
}
