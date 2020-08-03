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
        /// 重量
        /// </summary>
        public float Weight { get; set; }

        /// <summary>
        ///  长度
        /// </summary>
        public float FitLength { get; set; }

        /// <summary>
        ///  单片绝缘子长度
        /// </summary>
        public float PieceLength { get; set; }

        /// <summary>
        /// 片数
        /// </summary>
        public int PieceNum { get; set; }

        /// <summary>
        /// 金具换片数
        /// </summary>
        public int GoldPieceNum { get; set; }

        /// <summary>
        /// 联数
        /// </summary>
        public int LNum { get; set; }

        /// <summary>
        /// 阻尼线长度
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
        /// 
        /// </summary>
        public float Length { get; set; }

        public void UpdateLen()
        {
            Length = FitLength + PieceNum * PieceLength;
        }

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

    }
}
