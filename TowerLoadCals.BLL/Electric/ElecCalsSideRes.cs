﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.BLL.Electric
{
    public class ElecCalsSideRes
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

        /// <summary>
        /// 冰区类型：轻冰区，中冰区，重冰区
        /// </summary>
        public string IceArea { get; set; }

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