using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.Mode
{
    public class FormulaParas
    {
        /// <summary>
        /// 荷载比例
        /// GB50545-2010 DL/T5551-2018
        /// 旧名字 BL1
        /// </summary>
        public float LoadRatio { get; set; }

        /// <summary>
        /// 有利永久荷载分项系数
        /// GB50545-2010 DL/T5551-2018
        /// 旧名字 Dload_g
        /// </summary>
        public float RGGood { get; set; }

        /// <summary>
        /// 不利永久荷载分项系数
        /// GB50545-2010 DL/T5551-2018
        /// 旧名字 Dload_b
        /// </summary>
        public float RGBad { get; set; }

        /// <summary>
        /// 倾覆—永久荷载分项系数
        /// 旧名字 Dload_qh
        /// DL/T5551-2018
        /// </summary>
        public float RGOverturn { get; set; }

        /// <summary>
        /// 可变荷载分项系数
        /// GB50545-2010 DL/T5551-2018
        /// 旧名字 Vload
        /// </summary>
        public float RQ { get; set; }

        /// <summary>
        /// 偶然荷载分项系数
        /// GB50545-2010 DL/T5551-2018
        /// 旧名字 Vload_A
        /// </summary>
        public float RA { get; set; }

        /// <summary>
        /// 可变荷载组合系数_运行 GB50545-2010
        /// 可变荷载组合系数_大风 DL/T5551-2018
        /// 旧名字 Vcb_N
        /// </summary>
        public float VcFNormal { get; set; }

        /// <summary>
        /// 可变荷载组合系数_覆冰
        /// DL/T5551-2018
        /// 旧名字 Vcb_I
        /// </summary>
        public float VcFIce { get; set; }

        /// <summary>
        /// 可变荷载组合系数_低温
        /// GB50545-2010 DL/T5551-2018
        /// 旧名字 Vcb_D
        /// </summary>
        public float VcFCold { get; set; }

        /// <summary>
        /// 可变荷载组合系数_不均匀冰
        /// GB50545-2010 DL/T5551-2018
        /// 旧名字 Vcb_U
        /// </summary>
        public float VcFUnevenIce { get; set; }

        /// <summary>
        /// 可变荷载组合系数_断线
        /// GB50545-2010 DL/T5551-2018
        /// 旧名字 Vcb_B
        /// </summary>
        public float VcFBroken { get; set; }

        /// <summary>
        /// 可变荷载组合系数_安装
        /// GB50545-2010 DL/T5551-2018
        /// 旧名字 Vcb_L
        /// </summary>
        public float VcFInstall { get; set; }

        /// <summary>
        /// 可变荷载组合系数_验算     
        /// GB50545-2010 
        /// 旧名字 Vcb_Y
        /// </summary>
        public float VcFCheck { get; set; }

        /// <summary>
        /// 有利永久荷载分项系数_验算     
        /// GB50545-2010 DL/T5551-2018 
        /// 旧名字 Dload_Y1
        /// </summary>
        public float RGCheck01 { get; set; }

        /// <summary>
        /// 有利永久荷载分项系数_验算     
        /// GB50545-2010 DL/T5551-2018 
        /// 旧名字 Dload_Y2
        /// </summary>
        public float RGCheck02 { get; set; }

        /// <summary>
        /// 结构重要性系数_其他     
        /// DL/T5551-2018 
        /// 旧名字 r0
        /// </summary>
        public float R0Normal { get; set; }

        /// <summary>
        /// 结构重要性系数_安装     
        /// DL/T5551-2018 
        /// 旧名字 r1
        /// </summary>
        public float R1Install { get; set; }


        public float tdx { get; set; }

        public float llz { get; set; }

        public float dangle { get; set; }

        public float qangle { get; set; }

    }
}
