using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TowerLoadCals.Common.Utils;
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
        public StrDataUtils HangStr { get; set; }

        /// <summary>
        /// 跳线配置
        /// </summary>
        public StrDataUtils JumpStr { get; set; }

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
        public float BaseStringEquLength { get; set; }


        /// <summary>
        /// 空间位置数据
        /// </summary>
        public PhaseSpaceStrUtils SpaceStr { get; set; }

        /// <summary>
        ///  线风压系数
        /// </summary>
        public float WireWindPara { get; set; }

        /// <summary>
        /// 绝缘子串风压系数
        /// </summary>
        public float StrWindPara { get; set; }

        /// <summary>
        ///  跳线风压系数
        /// </summary>
        public float JmWindPara { get; set; }

        /// <summary>
        /// 支撑管风压系数
        /// </summary>
        public float PropUpWindPara { get; set; }

        /// <summary>
        ///  线计算数据
        /// </summary>
        public ElecCalsWire WrieData { get; set; }

        /// <summary>
        /// 跳线计算数据
        /// </summary>
        public ElecCalsWire JmWrieData { get; set; }

        [XmlIgnore]
        public Dictionary<string, double> VerSpanDic { get; set; }

        [XmlIgnore]
        public Dictionary<string, double> HoriLoadDic { get; set; }

        public ElecCalsStressPhaseStr()
        {

        }

        /// <summary>
        /// 计算垂直档距
        /// </summary>
        /// 
        public void UpdateVertialSpan(double span, double upSideInHei)
        {
            foreach (var nameWd in WrieData.WeatherParas.NameOfWkCdt)
            {
                BZResult bz = WrieData.BzDic[nameWd];

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
            foreach (var nameWd in WrieData.WeatherParas.NameOfWkCdt)
            {
                BZResult bz = WrieData.BzDic[nameWd];

                //这儿比载excel中用的是孤立档应力，用的是普通档应力
                //upSideInHei  这儿全部用的是上相导线高差， 有可能应该用的是自己的项的高差
                double rslt = HoriLoad(diaInc, WrieData.DevideNum, verSpan,  dampLength,  windLoad, strWindLaod);

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
