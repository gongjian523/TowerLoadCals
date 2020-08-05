﻿using System;
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
        public float IndEffectPara { get; set; }

        /// <summary>
        /// 导线安全系数
        /// </summary>
        public float IndSafePara { get; set; }

        /// <summary>
        /// 导线年平均系数,按照百分比
        /// </summary>
        public float IndAnPara { get; set; }

        /// <summary>
        /// 地线有效系数
        /// </summary>
        public float GrdEffectPara { get; set; }

        /// <summary>
        /// 地线安全系数
        /// </summary>
        public float GrdSafePara { get; set; }

        /// <summary>
        /// 地线年均系数，按照百分比
        /// </summary>
        public float GrdAnPara { get; set; }

        /// <summary>
        ///  OPGW有效系数
        /// </summary>
        public float OPGWEffectPara { get; set; }

        /// <summary>
        ///  OPGW安全系数
        /// </summary>
        public float OPGWSafePara { get; set; }

        /// <summary>
        ///  #OPGW年均系数
        /// </summary>
        public float OPGWAnPara { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public float IndMaxFor { get; set; }

        /// <summary>
        ///
        /// </summary>
        public float GrdMaxFor { get; set; }

        /// <summary>
        ///
        /// </summary>
        public float OPGWMaxFor { get; set; }

        public SideCalUtils(float indEffectPara=2.5f, float indSafePara= 2.5f, float indAnPara= 0.25f, float grdEffectPara = 4, float grdSafePara= 4, float grdAnPara= 0.25f,
            float opgwEffectPara=4, float opgwSafePara = 4, float opgwAnPara= 0.2f)
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
        public void  SetAlonePara(float indMaxForSor, float grdMaxForSor, float oPGWMaxForSor)
        {
            IndMaxFor = indMaxForSor;
            GrdMaxFor = grdMaxForSor;
            OPGWMaxFor = oPGWMaxForSor;
        }

    }
}