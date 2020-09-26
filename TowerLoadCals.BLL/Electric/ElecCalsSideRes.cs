using System.Xml.Serialization;

namespace TowerLoadCals.BLL.Electric
{
    public class ElecCalsSideRes
    {
        /// <summary>
        /// 
        /// </summary>
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public string WeatherListName { get; set; }

        /// <summary>
        /// 导线有效系数
        /// </summary>
        [XmlAttribute]
        public double IndEffectPara { get; set; }

        /// <summary>
        /// 导线安全系数
        /// </summary>
        [XmlAttribute]
        public double IndSafePara { get; set; }

        /// <summary>
        /// 导线年平均系数,按照百分比
        /// </summary>
        [XmlAttribute]
        public double IndAnPara { get; set; }

        /// <summary>
        /// 地线有效系数
        /// </summary>
        [XmlAttribute]
        public double GrdEffectPara { get; set; }

        /// <summary>
        /// 地线安全系数
        /// </summary>
        [XmlAttribute]
        public double GrdSafePara { get; set; }

        /// <summary>
        /// 地线年均系数，按照百分比
        /// </summary>
        [XmlAttribute]
        public double GrdAnPara { get; set; }

        /// <summary>
        ///  OPGW有效系数
        /// </summary>
        [XmlAttribute]
        public double OPGWEffectPara { get; set; }

        /// <summary>
        ///  OPGW安全系数
        /// </summary>
        [XmlAttribute]
        public double OPGWSafePara { get; set; }

        /// <summary>
        ///  #OPGW年均系数
        /// </summary>
        [XmlAttribute]
        public double OPGWAnPara { get; set; }


        /// <summary>
        /// 冰区类型：轻冰区，中冰区，重冰区
        /// </summary>
        [XmlAttribute]
        public string IceArea { get; set; }

        /// <summary>
        ///
        /// </summary>
        [XmlAttribute]
        public string IndName { get; set; }

        /// <summary>
        ///
        /// </summary>
        [XmlAttribute]
        public string GrdName { get; set; }

        /// <summary>
        ///
        /// </summary>
        [XmlAttribute]
        public string OPGWName { get; set; }

        /// <summary>
        ///
        /// </summary>
        [XmlAttribute]
        public int IndDevideNum { get; set; }

        /// <summary>
        ///
        /// </summary>
        [XmlAttribute]
        public double IndDecrTem { get; set; }

        /// <summary>
        ///
        /// </summary>
        [XmlAttribute]
        public double GrdDecrTem { get; set; }

        /// <summary>
        ///
        /// </summary>
        [XmlAttribute]
        public double OPGWDecrTem { get; set; }

        /// <summary>
        ///
        /// </summary>
        [XmlAttribute]
        public string IndJGBName { get; set; }

        /// <summary>
        ///
        /// </summary>
        [XmlAttribute]
        public string IndFZName { get; set; }

        /// <summary>
        ///
        /// </summary>
        [XmlAttribute]
        public string GrdFZName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlAttribute]
        public double IndMaxFor { get; set; }

        /// <summary>
        ///
        /// </summary>
        [XmlAttribute]
        public double GrdMaxFor { get; set; }

        /// <summary>
        ///
        /// </summary>
        [XmlAttribute]
        public double OPGWMaxFor { get; set; }

        /// <summary>
        ///
        /// </summary>
        [XmlAttribute]
        public double IndDrawingLen { get; set; }

        /// <summary>
        ///
        /// </summary>
        [XmlAttribute]
        public double GrdDrawingLen { get; set; }

        /// <summary>
        ///
        /// </summary>
        [XmlAttribute]
        public double FirDnLeadLen { get; set; }

        /// <summary>
        ///
        /// </summary>
        [XmlAttribute]
        public double FirDnLeadBackDist { get; set; }

        /// <summary>
        ///
        /// </summary>
        [XmlAttribute]
        public double SecDnLeadLen { get; set; }

        /// <summary>
        ///
        /// </summary>
        [XmlAttribute]
        public double SecDnLeadBackDist { get; set; }

        /// <summary>
        ///
        /// </summary>
        [XmlAttribute]
        public string FitDataCalsPara { get; set; }

        /// <summary>
        ///
        /// </summary>
        [XmlAttribute]
        public int IndJGBNum { get; set; }

        /// <summary>
        ///
        /// </summary>
        [XmlAttribute]
        public int IndFZNum { get; set; }

        /// <summary>
        ///
        /// </summary>
        [XmlAttribute]
        public int GrdFZNum { get; set; }


        public ElecCalsSideRes()
        {

        }

        public ElecCalsSideRes(double indEffectPara=2.5, double indSafePara= 2.5, double indAnPara= 0.25, double grdEffectPara = 4, double grdSafePara= 4, double grdAnPara= 0.25,
            double opgwEffectPara=4, double opgwSafePara = 4, double opgwAnPara= 0.2)
        {
            IndEffectPara = indEffectPara;
            IndSafePara = indSafePara; 
            IndAnPara = indAnPara;

            GrdEffectPara = grdEffectPara;
            GrdSafePara = grdSafePara;  
            GrdAnPara = grdAnPara;

            OPGWEffectPara = opgwEffectPara;
            OPGWSafePara = opgwSafePara; 
            OPGWAnPara = opgwAnPara;  
        }

        /// <summary>
        /// 设置孤立档计算参数
        /// </summary>
        /// <param name="IndMaxForSor"></param>
        /// <param name="GrdMaxForSor"></param>
        /// <param name="OPGWMaxForSor"></param>
        public void  SetAlonePara(double indMaxForSor, double grdMaxForSor, double oPGWMaxForSor)
        {
            IndMaxFor = indMaxForSor;
            GrdMaxFor = grdMaxForSor;
            OPGWMaxFor = oPGWMaxForSor;
        }

    }
}
