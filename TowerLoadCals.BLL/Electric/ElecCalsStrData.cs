using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TowerLoadCals.BLL.Electric
{

    //RigidJumperInsulator 
    //GeneralInsulator
    /// <summary>
    /// fda
    /// </summary>
    public class ElecCalsStrData
    {
        /// <summary>
        /// 导线安全系数
        /// </summary>
        [XmlAttribute]
        public int ID { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [XmlAttribute]
        public string Name { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        [XmlAttribute]
        public int Type { get; set; }

        /// <summary>
        /// 重量(导地线串重)
        /// </summary>
        [XmlAttribute]
        public double Weight { get; set; }

        /// <summary>
        ///  长度(金具结构高度)
        /// </summary>
        [XmlAttribute]
        public double FitLength { get; set; }

        /// <summary>
        ///  单片绝缘子长度(绝缘子结构高度（mm）)
        /// </summary>
        [XmlAttribute]
        public double PieceLength { get; set; }

        /// <summary>
        /// 片数(绝缘子片数)
        /// </summary>
        [XmlAttribute]
        public int PieceNum { get; set; }

        /// <summary>
        /// 金具换片数(导地线串金具折片数)
        /// </summary>
        [XmlAttribute]
        public int GoldPieceNum { get; set; }

        /// <summary>
        /// 联数(绝缘子联数)
        /// </summary>
        [XmlAttribute]
        public int LNum { get; set; }

        /// <summary>
        /// 阻尼线长度(阻尼线长)
        /// </summary>
        [XmlAttribute]
        public double DampLength { get; set; }

        /// <summary>
        ///  硬跳线参数，支撑管长度 单位m
        /// </summary>
        [XmlAttribute]
        public double SuTubleLen { get; set; }

        /// <summary>
        ///  硬跳线参数，软跳线长度
        /// </summary>
        [XmlAttribute]
        public double SoftLineLen { get; set; }

        /// <summary>
        /// 硬跳线间隔棒数量
        /// </summary>
        [XmlAttribute]
        public int JGBNum { get; set; }

        /// <summary>
        /// 间隔棒资源ID
        /// </summary>
        [XmlAttribute]
        public int JGBPnt { get; set; }

        /// <summary>
        ///支撑管直径,单位mm,
        /// </summary>
        [XmlAttribute]
        public double SuTubleDia { get; set; }

        /// <summary>
        /// 单位长度重量,单位kg
        /// </summary>
        [XmlAttribute]
        public double SuTubleWei { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlIgnore]
        public double Length { get; set; }

        public void UpdateLen()
        {
            Length = FitLength + PieceNum * PieceLength;
        }

        /// <summary>
        /// 通常使用的工况：小号侧
        /// </summary>
        [XmlIgnore]
        public ElecCalsWeaRes WeaParas { get; set; }

        /// <summary>
        /// 大号侧
        /// </summary>
        [XmlIgnore]
        public ElecCalsWeaRes WeaAnSideParas { get; set; }

        /// <summary>
        /// 公共参数
        /// </summary>
        [XmlIgnore]
        public ElecCalsCommRes CommPara { get; set; }

        public ElecCalsStrData()
        {
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
        public void SetIGPara(double weightSor, int lNumSor, int  pieceNumSor, double pieceLengthSor, int goldPieceNumSor,
               double fitLengthSor, double dampLengthSor)
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
        public void SetJumPara(double weightSor, int lNumSor, int pieceNumSor, int goldPieceNumSor, double softLineLenSor= 0, 
            int jGBNumSor= 0, double suTubleLenSor= 0, double suTubleDiSor = 0, double suTubleWeiSor= 0)
        {
            Weight = weightSor;
            LNum = lNumSor;
            PieceNum = pieceNumSor;
            GoldPieceNum = goldPieceNumSor;

            SoftLineLen = softLineLenSor;
            JGBNum = jGBNumSor;
            SuTubleLen = suTubleLenSor; 
            SuTubleDia = suTubleDiSor / 1000; 
            SuTubleWei = suTubleWeiSor;  
        }

        /// <summary>
        /// 更新参数
        /// </summary>
        /// <param name="weaData">工况参数</param>
        public void UpdataPara(ElecCalsCommRes commData, ElecCalsWeaRes weaData, ElecCalsWeaRes anWeaData)
        {
            CommPara = commData;
            WeaParas = weaData;
            WeaAnSideParas = anWeaData;
        }
    }

}
