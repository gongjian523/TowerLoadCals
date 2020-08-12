using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.BLL.Electric
{
    public class StrDataUtils
    {
        /// <summary>
        /// 导线安全系数
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 重量(导地线串重)
        /// </summary>
        public float Weight { get; set; }

        /// <summary>
        ///  长度(金具结构高度)
        /// </summary>
        public float FitLength { get; set; }

        /// <summary>
        ///  单片绝缘子长度(绝缘子结构高度（mm）)
        /// </summary>
        public float PieceLength { get; set; }

        /// <summary>
        /// 片数(绝缘子片数)
        /// </summary>
        public int PieceNum { get; set; }

        /// <summary>
        /// 金具换片数(导地线串金具折片数)
        /// </summary>
        public int GoldPieceNum { get; set; }

        /// <summary>
        /// 联数(绝缘子联数)
        /// </summary>
        public int LNum { get; set; }

        /// <summary>
        /// 阻尼线长度(阻尼线长)
        /// </summary>
        public float DampLength { get; set; }

        /// <summary>
        ///  硬跳线参数，支撑管长度 单位m
        /// </summary>
        public float SuTubleLen { get; set; }

        /// <summary>
        ///  硬跳线参数，软跳线长度
        /// </summary>
        public float SoftLineLen { get; set; }

        /// <summary>
        /// 硬跳线间隔棒数量
        /// </summary>
        public int JGBNum { get; set; }

        /// <summary>
        /// 间隔棒资源ID
        /// </summary>
        public int JGBPnt { get; set; }

        /// <summary>
        ///支撑管直径,单位mm,
        /// </summary>
        public float SuTubleDi { get; set; }

        /// <summary>
        /// 单位长度重量,单位kg
        /// </summary>
        public float SuTubleWei { get; set; }

        /// <summary>
        /// 保存风荷载
        /// </summary>
        public Dictionary<string, float> WindLoad  {get; set;}

        /// <summary>
        /// 保存垂直荷载
        /// </summary>
        public Dictionary<string, float> VerLoad { get; set; }

        /// <summary>
        /// 受风面积
        /// </summary>
        public float WindArea { get; set; }


        /// <summary>
        /// 保存跳线绝缘子串的风荷载
        /// </summary>
        public Dictionary<string, float> JumpStrWindLoad { get; set; }

        /// <summary>
        /// 保存跳线的风荷载
        /// </summary>
        public Dictionary<string, float> JumpWindLoad { get; set; }

        /// <summary>
        /// 保存支撑管线的风荷载
        /// </summary>
        public Dictionary<string, float> SupportWindLoad { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public float Length { get; set; }

        public void UpdateLen()
        {
            Length = FitLength + PieceNum * PieceLength;
        }

        /// <summary>
        /// 通常使用的工况：小号侧
        /// </summary>
        public WeatherUtils WeaParas { get; set; }

        /// <summary>
        /// 大号侧
        /// </summary>
        public WeatherUtils WeaAnSideParas { get; set; }

        /// <summary>
        /// 公共参数
        /// </summary>
        public ElecCalsCommRes CommPara { get; set; }

        /// <summary>
        /// 设置导地线串参数
        /// </summary>
        /// <param name="weightSor"></param>
        /// <param name="lNumSor"></param>
        /// <param name="pieceNumSor"></param>
        /// <param name="pieceLengthSor"></param>
        /// <param name="goldPieceNumSor"></param>
        /// <param name="fitLengthSor"></param>
        /// <param name="dampLengthSor"></param>
        public void SetIGPara(float weightSor, int lNumSor, int  pieceNumSor, float pieceLengthSor, int goldPieceNumSor,
               float fitLengthSor, float dampLengthSor)
        {
            Weight = weightSor;
            LNum = lNumSor;
            PieceNum = pieceNumSor;
            PieceLength = pieceLengthSor;
            GoldPieceNum = goldPieceNumSor;
            FitLength = fitLengthSor;
            DampLength = dampLengthSor;
        }

        /// <summary>
        /// 设置跳线参数,包含导线部分参数
        /// </summary>
        /// <param name="weightSor"></param>
        /// <param name="lNumSor"></param>
        /// <param name="pieceNumSor"></param>
        /// <param name="goldPieceNumSor"></param>
        /// <param name="softLineLenSor"></param>
        /// <param name="jGBNumSor"></param>
        /// <param name="suTubleLenSor"></param>
        /// <param name="suTubleDiSor"></param>
        /// <param name="suTubleWeiSor"></param>
        public void SetJumPara(float weightSor, int lNumSor, int pieceNumSor, int goldPieceNumSor, float softLineLenSor= 0, 
            int jGBNumSor= 0, float suTubleLenSor= 0, float suTubleDiSor = 0, float suTubleWeiSor= 0)
        {
            Weight = weightSor;
            LNum = lNumSor;
            PieceNum = pieceNumSor;
            GoldPieceNum = goldPieceNumSor;

            SoftLineLen = softLineLenSor;
            JGBNum = jGBNumSor;
            SuTubleLen = suTubleLenSor; 
            SuTubleDi = suTubleDiSor / 1000; 
            SuTubleWei = suTubleWeiSor;  
        }

        /// <summary>
        /// 更新参数
        /// </summary>
        /// <param name="weaData">工况参数</param>
        public void UpdataPara(ElecCalsCommRes commData, WeatherUtils weaData, WeatherUtils anWeaData)
        {
            CommPara = commData;
            WeaParas = weaData;
            WeaAnSideParas = anWeaData;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="CalType"></param>
        public void CalHezai()
        {
            foreach (var weaItem in WeaParas.WeathComm)
            {
                float wload = (float)Math.Round(ElecCalsToolBox2.StringWind(PieceNum, LNum, GoldPieceNum, weaItem.IceThickness, weaItem.WindSpeed, weaItem.BaseWindSpeed), 3);
                float vload = Weight + WeightIceIn(weaItem.IceThickness) * (PieceNum * LNum + GoldPieceNum);
                WindLoad.Add(weaItem.SWorkConditionName, wload);
                VerLoad.Add(weaItem.SWorkConditionName, vload);
            }

            WindArea = (float)(0.04 * (PieceNum * LNum + GoldPieceNum));
        }


        public void CalsWindLoad(float jumpDia)
        {
            foreach (var weaItem in WeaParas.WeathComm)
            {
                var anWea = WeaAnSideParas.WeathComm.Where(wea => wea.SWorkConditionName == weaItem.SWorkConditionName).First();

                float temp = weaItem.Temperature;

                //冰厚，风速和基本风速需要比较大号侧和小号侧相应的工况，取其中的较大值
                float iceThick = (anWea != null && anWea.IceThickness > weaItem.IceThickness) ? anWea.IceThickness : weaItem.IceThickness;
                float windSpeed = (anWea != null && anWea.WindSpeed > weaItem.WindSpeed) ? anWea.WindSpeed : weaItem.WindSpeed;
                float baseWindSpeed = (anWea != null && anWea.BaseWindSpeed > weaItem.BaseWindSpeed) ? anWea.BaseWindSpeed : weaItem.BaseWindSpeed;

                float jmupStrLoad = (float)Math.Round(ElecCalsToolBox2.StringWind(PieceNum, LNum, GoldPieceNum, iceThick, windSpeed, baseWindSpeed), 3);
                JumpStrWindLoad.Add(weaItem.SWorkConditionName,jmupStrLoad);

                float jmupLoad = (float)(Math.Round(ElecCalsToolBox2.WindPaT(CommPara.VoltStr, jumpDia, iceThick, windSpeed, baseWindSpeed), 3) * GoldPieceNum);
                JumpWindLoad.Add(weaItem.SWorkConditionName, jmupLoad);

                float supportLoad = (float)(Math.Round(ElecCalsToolBox2.WindPaT(CommPara.VoltStr, SuTubleDi, iceThick, windSpeed, baseWindSpeed), 3) * SuTubleLen);
                SupportWindLoad.Add(weaItem.SWorkConditionName, supportLoad);

            }
        }

        protected float WeightIceIn(float iceThick)
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
