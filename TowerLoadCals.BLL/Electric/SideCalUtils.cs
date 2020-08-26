using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.BLL.Electric
{
    public class SideCalUtils
    {
        /// <summary>
        /// 导线有效系数
        /// </summary>
        public double IndEffectPara { get; set; }

        /// <summary>
        /// 导线安全系数
        /// </summary>
        public double IndSafePara { get; set; }

        /// <summary>
        /// 导线年平均系数,按照百分比
        /// </summary>
        public double IndAnPara { get; set; }

        /// <summary>
        /// 地线有效系数
        /// </summary>
        public double GrdEffectPara { get; set; }

        /// <summary>
        /// 地线安全系数
        /// </summary>
        public double GrdSafePara { get; set; }

        /// <summary>
        /// 地线年均系数，按照百分比
        /// </summary>
        public double GrdAnPara { get; set; }

        /// <summary>
        ///  OPGW有效系数
        /// </summary>
        public double OPGWEffectPara { get; set; }

        /// <summary>
        ///  OPGW安全系数
        /// </summary>
        public double OPGWSafePara { get; set; }

        /// <summary>
        ///  #OPGW年均系数
        /// </summary>
        public double OPGWAnPara { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double IndMaxFor { get; set; }

        /// <summary>
        ///
        /// </summary>
        public double GrdMaxFor { get; set; }

        /// <summary>
        ///
        /// </summary>
        public double OPGWMaxFor { get; set; }

        public SideCalUtils()
        {

        }

        public SideCalUtils(double indEffectPara=2.5f, double indSafePara= 2.5f, double indAnPara= 0.25f, double grdEffectPara = 4, double grdSafePara= 4, double grdAnPara= 0.25f,
            double opgwEffectPara=4, double opgwSafePara = 4, double opgwAnPara= 0.2f)
        {
            IndEffectPara = indAnPara;
            IndSafePara = indSafePara; 
            IndAnPara = indAnPara;

            GrdEffectPara = GrdEffectPara;
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
