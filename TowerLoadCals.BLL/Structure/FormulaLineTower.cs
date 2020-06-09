using System;
using TowerLoadCals.Mode;


namespace TowerLoadCals.BLL
{
    public class FormulaLineTower: FormulaTower
    {
        public FormulaLineTower(FormulaParas paras) : base(paras)
        {
        }

        /// <summary>
        /// 大风和低温X
        /// 工况代码："N1", "N2", "D1", "D2"
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <returns></returns>
        public float ZXNX(float a1, float a2, float a3, out string str)
        {
            float res = Paras.LoadRatio * Paras.RQ * a3 * Paras.WindAdjustFactor * AngX(a1) * a2;
            str = Paras.LoadRatio + " x " + Paras.RQ  + " x " + a3 + " x " + Paras.WindAdjustFactor + " x " + AngX(a1) + " x " + a2  + " = " + res.ToString("0.00");
            return res;
        }

        /// <summary>
        /// 大风和低温Y
        /// 工况代码："N1", "N2", "D1", "D2"       /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <param name="a4"></param>
        /// <param name="a5"></param>
        /// <returns></returns>
        public float ZXNY(float a1, float a2, float a3, float a4, float a5, out string str)
        {
            float res = Paras.LoadRatio * Paras.RQ * a5 * (Paras.WindAdjustFactor * AngY(a1) * a2 + (a3 - a4));
            str = Paras.LoadRatio + " x " + Paras.RQ + " x " + a5 + " x (" + Paras.WindAdjustFactor + " x " + AngY(a1) + " x " + a2 + "+ (" + a3 + " - " +  a4 + ")) = " + res.ToString("0.00");
            return res;
        }

        /// <summary>
        /// 大风和低温Z
        /// 工况代码："N1", "N2", "D1", "D2"
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <returns></returns>
        public float ZXNZ(float a1, float a2, float a3, out string str)
        {
            if(a1 >= 0)
            {
                //a2 即rg最大最小垂荷合并
                float res = Paras.LoadRatio * a2 * a1;
                str = Paras.LoadRatio + " x " + a2 + " x " + a1 + " = " + res.ToString("0.00");
                return res;
            }
            else
            {
                float res = Paras.LoadRatio * Paras.RQ * a1 * a3;
                str = Paras.LoadRatio + " x " + Paras.RQ + " x " + a1 + " x " + a3 + " = " + res.ToString("0.00");
                return res;
            }
        }

        /// <summary>
        /// 覆冰-最低温、不均匀冰、断线、验算冰X
        /// 工况代码："I1", "I2", "U1", "U2", "B1", "B2", "Y1", "Y2"
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <param name="a4"></param>
        /// <returns></returns>
        public float ZXIYX(float a1, float a2, float a3, float a4, out string str)
        {
            //a4为分项系数
            float res = Paras.LoadRatio * a4 * a3 * Paras.OtherWindAdjustFactor * AngX(a1) * a2;
            str = Paras.LoadRatio + " x " + a4 + " x " + a3 + " x " + Paras.OtherWindAdjustFactor + " x " + AngX(a1) + " x " + a2 + " = " + res.ToString("0.00");
            return res;
        }

        /// <summary>
        /// 覆冰-最低温、不均匀冰、断线、验算冰Y
        /// 工况代码："I1", "I2", "U1", "U2", "B1", "B2", "Y1", "Y2"
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <param name="a4"></param>
        /// <param name="a5"></param>
        /// <param name="a6"></param>
        /// <returns></returns>
        public float ZXIYY(float a1, float a2, float a3, float a4, float a5, float a6, out string str)
        {
            float res = Paras.LoadRatio * a6 * a5 * (Paras.OtherWindAdjustFactor * AngY(a1) * a2 + (a3 - a4));
            str = Paras.LoadRatio + " x " + a6 + " x " + a5 + " x (" + Paras.OtherWindAdjustFactor + " x " + AngY(a1) + " x " + a2  + " + (" + a3 + " - " + a4 + ")) = " + res.ToString("0.00");
            return res;
        }

        /// <summary>
        /// 覆冰-最低温、不均匀冰、断线、验算冰Z
        /// 工况代码："I1", "I2", "U1", "U2", "B1", "B2", "Y1", "Y2"
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <param name="a4"></param>
        /// <param name="a5"></param>
        /// <returns></returns>
        public float ZXIYZ(float a1, float a2, float a3, float a4, float a5, out string str)
        {
            if (a1 >= 0)
            {
                float res = Paras.LoadRatio * (a3 * a1 + a4 * a5 * (a2 - a1));
                str = Paras.LoadRatio + " x (" + a3 + " x " + a1 + " + " + a4 + " x " + a5 + " x (" + a2 + " - " + a1 + "))= " + res.ToString("0.00");
                return res;
            }
            else
            {
                //a3 即rg最大最小垂荷合并
                float res = Paras.LoadRatio * (a4 * a5 * a1 + a4 * a5 * (a2 - a1));
                str = Paras.LoadRatio + " x (" + a4 + " x " + a5 + " x " + a1 + " + " + a4 + " x " + a5 + " x (" + a2 + " - " + a1 + "))= " + res.ToString("0.00");
                return res;
            }
        }

        /// <summary>
        /// 吊装X
        /// 工况代码："L", "La", "Lb", "Lc", "Ld", "Le", "Lf", "Lg", "Lh"
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <returns></returns>
        public float ZXLX(float a1, float a2, out string str)
        {
            float res = Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * Paras.OtherWindAdjustFactor * AngX(a1) * a2;
            str = Paras.LoadRatio + " x " + Paras.RQ + " x " + Paras.VcFInstall + " x " + Paras.OtherWindAdjustFactor + " x " + AngX(a1) + " x " + a2 + " = " + res.ToString("0.00");
            return res;
        }

        /// <summary>
        /// 吊装Y
        /// 工况代码："L", "La", "Lb", "Lc", "Ld", "Le", "Lf", "Lg", "Lh"
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <param name="a4"></param>
        /// <returns></returns>
        public float ZXLY(float a1, float a2, float a3, float a4, out string str)
        {
            float res = Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * (Paras.OtherWindAdjustFactor * AngY(a1) * a2 + (a3 - a4));
            str = Paras.LoadRatio + " x " + Paras.RQ + " x " + Paras.VcFInstall + " x (" + Paras.OtherWindAdjustFactor + " x " + AngY(a1) + " x " + a2 + " + (" + a3 + " - " + a4 +"))= " + res.ToString("0.00"); 
            return res;
        }

        /// <summary>
        /// 吊装Z1
        /// 工况代码："L", "La", "Lb", "Lc", "Ld", "Le", "Lf", "Lg", "Lh"
        /// </summary>
        /// <param name="a1"></param>
        /// <returns></returns>
        public float ZXLZ1(float a1, out string str)
        {
            if(a1 >= 0)
            {
                float res = Paras.LoadRatio * Paras.RGBad * a1;
                str = Paras.LoadRatio + " x " + Paras.RGBad + " x " + a1 +" = " + res.ToString("0.00"); 
                return res;
            }
            else
            {
                float res = Paras.LoadRatio * Paras.RQ * a1 * Paras.VcFInstall;
                str = Paras.LoadRatio + " x " + Paras.RQ + " x " + a1 + " x " + Paras.VcFInstall + " = " + res.ToString("0.00");
                return res;
            }
        }

        /// <summary>
        /// 吊装Z2
        /// 工况代码："L", "La", "Lb", "Lc", "Ld", "Le", "Lf", "Lg", "Lh"
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <returns></returns>
        public float ZXLZ2(float a1, float a2, float a31, float a32, out string str) 
        {
            //a2 a3 为吊装系数和附加荷载，导地线不同
            float res = Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * (a1 * Paras.DynamicCoef * a2 + a31*a32);
            str = Paras.LoadRatio + " x " + Paras.RQ + " x " + Paras.VcFInstall + " x (" + a1 + " x " + Paras.DynamicCoef + " x " + a2 + " + " + a31 + " x " + a32  + ") = " + res.ToString("0.00");
            return res;
        }

        /// <summary>
        /// 针对锚线工况X 已锚
        /// 工况代码"M", "Ma", "Mb", "Mc", "Md", "Me", "Mf", "Mg", "Mh"
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <returns></returns>
        public float ZXMX2(float a1, float a2, out string str)
        {
            float res = Paras.LoadRatio * Paras.RQ * Paras.AnchorWindCoef * Paras.VcFInstall * Paras.OtherWindAdjustFactor * AngX(a1) * a2;
            str = Paras.LoadRatio + " x " + Paras.RQ  + " x " + Paras.AnchorWindCoef + " x " + Paras.VcFInstall + " x " + Paras.OtherWindAdjustFactor + " x " + AngX(a1) + " x " + a2 + " = " + res.ToString("0.00");
            return res;
        }

        /// <summary>
        /// 针对锚线工况Y 已锚
        /// 工况代码"M", "Ma", "Mb", "Mc", "Md", "Me", "Mf", "Mg", "Mh" 
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <param name="a4"></param>
        /// <returns></returns>
        public float ZXMY2(float a1, float a2, float a3, float a4, out string str)
        {
            float res = Paras.LoadRatio * Paras.RQ * Paras.AnchorWindCoef  * Paras.VcFInstall * Paras.OtherWindAdjustFactor * AngY(a1) * a2 + Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * a3 * (1 - (float)Math.Cos(a4 * PI / 180));
            str = Paras.LoadRatio + " x " + Paras.RQ + " x " + Paras.AnchorWindCoef + " x " + Paras.VcFInstall + " x " + Paras.OtherWindAdjustFactor + " x " + AngY(a1) + " x " + a2 + " + " + Paras.LoadRatio + " x " + Paras.RQ + " x " + Paras.VcFInstall + " x " + a3 + " x (1 - cos(" + a4 + ")) = " + res.ToString("0.00");
            return res;
        }

        /// <summary>
        /// 针对锚线工况Z 已锚
        /// 工况代码"M", "Ma", "Mb", "Mc", "Md", "Me", "Mf", "Mg", "Mh"
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <param name="a4"></param>
        /// <returns></returns>
        public float ZXMZ2(float a1, float a21, float a22, float a3, float a4, out string str)
        {
            float res =  Paras.LoadRatio * Paras.RGBad * Paras.AnchorGravityCoef * a1 + Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * (a3 * (float)Math.Sin(a4 * PI /180) + a21 * a22);
            str = Paras.LoadRatio + " x " + Paras.RGBad + " x " + Paras.AnchorGravityCoef + " x " + a1 + " + " + Paras.LoadRatio + " x " + Paras.RQ + " x " +  Paras.VcFInstall + " x (" + a3 + " x sin(" + a4 + ") + " + a21 + " x " + a22  + ") = " + res.ToString("0.00");
            return res;
        }

        /// <summary>
        /// 过滑车X
        /// 工况代码"G", "Ga", "Gb", "Gc", "Gd", "Ge", "Gf", "Gg", "Gh", "GLa", "GLb", "GLc", "GLd", "GLe", "GLf", "GLg", "GLh"
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <returns></returns>
        public float GLX(float a1, float a2, out string str)
        {
            float res = Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * Paras.OtherWindAdjustFactor * AngX(a1) * a2;
            str = Paras.LoadRatio + " x " + Paras.RQ + " x " + Paras.VcFInstall + " x " + Paras.OtherWindAdjustFactor + " x " + AngX(a1) + " x " + a2 + " = " + res.ToString("0.00");
            return res;
        }

        /// <summary>
        /// 过滑车Y
        /// 工况代码"G", "Ga", "Gb", "Gc", "Gd", "Ge", "Gf", "Gg", "Gh", "GLa", "GLb", "GLc", "GLd", "GLe", "GLf", "GLg", "GLh"
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <returns></returns>
        public float GLY(float a1, float a2, float a3, out string str)
        {
            float res = Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * (Paras.OtherWindAdjustFactor * AngY(a1) * a2 + Paras.DynamicCoef * a3);
            str = Paras.LoadRatio + " x " + Paras.RQ + " x " + Paras.VcFInstall + " x (" + Paras.OtherWindAdjustFactor + " x " + AngY(a1) + " x " + a2 + " + " + Paras.DynamicCoef + " x " + a3 + ") = " + res.ToString("0.00");
            return res;
        }

        /// <summary>
        /// 过滑车Z
        /// 工况代码"G", "Ga", "Gb", "Gc", "Gd", "Ge", "Gf", "Gg", "Gh", "GLa", "GLb", "GLc", "GLd", "GLe", "GLf", "GLg", "GLh"
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <returns></returns>
        public float GLZ(float a1, float a2, float a31,float a32,  out string str)
        {
            if (a1 >= 0)
            {
                float res = Paras.LoadRatio * Paras.RGBad * a1 + Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * ((a2 - a1) + a31 * a32);
                str = Paras.LoadRatio + " x " + Paras.RGBad + " x " + a1 + " + " + Paras.LoadRatio + " x " + Paras.RQ + " x " + Paras.VcFInstall + " x ((" + a2 + " - " + a1 + " ) +  " + a31 + " x " + a32 + ") = " + res.ToString("0.00");
                return res;
            }
            else
            {
                float res = Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * (a2 - 0) + Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * a31 * a32;
                str = Paras.LoadRatio + " x " + Paras.RQ + " x " + Paras.VcFInstall + " x (" + a2 + " - 0) + " + Paras.LoadRatio + " x " + Paras.RQ + " x " + Paras.VcFInstall + " x " + a31 + " x " + a32 + " = " + res.ToString("0.00");
                return res;
            }
                
        }

        /// <summary>
        /// 针对施工提线工况 X
        /// 工况代码"C", "Ca", "Cb", "Cc", "Cd", "Ce", "Cf", "Cg", "Ch", "CLa", "CLb", "CLc", "CLd", "CLe", "CLf", "CLg", "CLh"
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <returns></returns>
        public float ZXCX(float a1, float a2, out string str)
        {
            float res = Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * Paras.OtherWindAdjustFactor * AngX(a1) * a2;
            str = Paras.LoadRatio + " x " + Paras.RQ + " x " + Paras.VcFInstall + " x " + Paras.OtherWindAdjustFactor + " x " + AngX(a1) + " x " + a2 + " = " + res.ToString("0.00");
            return res;
        }

        /// <summary>
        /// 针对施工提线工况 Y
        /// 工况代码"C", "Ca", "Cb", "Cc", "Cd", "Ce", "Cf", "Cg", "Ch", "CLa", "CLb", "CLc", "CLd", "CLe", "CLf", "CLg", "CLh"
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <param name="a4"></param>
        /// <returns></returns>
        public float ZXCY(float a1, float a2, float a3, float a4, out string str)
        {
            float res =  Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * Paras.OtherWindAdjustFactor * AngY(a1) * a2 + Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * Paras.DynamicCoef * a3 * a4 * (float)Math.Sin(0);
            str = Paras.LoadRatio + " x " + Paras.RQ + " x " + Paras.VcFInstall + " x " + Paras.OtherWindAdjustFactor + " x " + AngY(a1) + " x " + a2 + " + " + Paras.LoadRatio + " x " + Paras.RQ + " x " + Paras.VcFInstall + " x " + Paras.DynamicCoef + " x " + a3 + " x " + a4 + " x sin(0) =" + res.ToString("0.00"); ;
            return res;
        }


        /// <summary>
        /// 针对施工提线工况 Z
        /// 工况代码"C", "Ca", "Cb", "Cc", "Cd", "Ce", "Cf", "Cg", "Ch", "CLa", "CLb", "CLc", "CLd", "CLe", "CLf", "CLg", "CLh"
        /// </summary>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <param name="a4"></param>
        /// <returns></returns>
        public float ZXCZ(float a2, float a3, float a4, out string str)
        {
            float res = Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * (Paras.DynamicCoef * a3 * a4 * (float)Math.Sin(90 * PI / 180) + a2);
            str = Paras.LoadRatio + " x " + Paras.RQ + " x " + Paras.VcFInstall + " x (" + Paras.DynamicCoef + " x " + a3 + " x " + a4 + " x  sin(" + 90 + ") + " + a2 + ") = " + res.ToString("0.00");
            return res;
        }

        /// <summary>
        /// 针对锚线工况X 正锚
        /// 工况代码"M", "Ma", "Mb", "Mc", "Md", "Me", "Mf", "Mg", "Mh"
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <returns></returns>
        public float ZXMX1(float a1, float a2, out string str)
        {
            float res = Paras.LoadRatio * Paras.RQ * Paras.AnchorWindCoef * Paras.VcFInstall * Paras.OtherWindAdjustFactor * AngX(a1) * a2;
            str = Paras.LoadRatio + " x " + Paras.RQ + " x " + Paras.AnchorWindCoef + " x " + Paras.VcFInstall + " x " + Paras.OtherWindAdjustFactor + " x " + AngX(a1) + " x " + a2 + " = " + res.ToString("0.00"); 
            return res;
        }

        /// <summary>
        /// 针对锚线工况Y 正锚
        /// 工况代码"M", "Ma", "Mb", "Mc", "Md", "Me", "Mf", "Mg", "Mh"
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <param name="a4"></param>
        /// <returns></returns>
        public float ZXMY1(float a1, float a2, float a3, float a4, out string str)
        {
            float res = Paras.LoadRatio * Paras.RQ * Paras.AnchorWindCoef *  Paras.VcFInstall * Paras.OtherWindAdjustFactor * AngY(a1) * a2 + Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * Paras.DynamicCoef * a3 * (1 - (float)Math.Cos(a4 * PI / 180));
            str = Paras.LoadRatio + " x " + Paras.RQ   + " x " + Paras.AnchorWindCoef + " x " + Paras.VcFInstall + " x " + Paras.OtherWindAdjustFactor + " x " + AngY(a1) + " x " + a2 + " + " + Paras.LoadRatio + " x " + Paras.RQ + " x " + Paras.VcFInstall + " x " + Paras.DynamicCoef + " x " + a3 + " x (1 - cos(" + a4 + ")) = " + res.ToString("0.00");
            return res;
        }

        /// <summary>
        /// 针对锚线工况Z 正锚
        /// 工况代码"M", "Ma", "Mb", "Mc", "Md", "Me", "Mf", "Mg", "Mh"
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <param name="a4"></param>
        /// <returns></returns>
        public float ZXMZ1(float a1, float a21, float a22, float a3, float a4, out string str)
        {
            float res = Paras.LoadRatio * Paras.RGBad * Paras.AnchorGravityCoef * a1 + Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * (Paras.DynamicCoef * a3 * (float)Math.Sin(a4 * PI / 180) + a21 * a22);
            str = Paras.LoadRatio + " x " + Paras.RGBad + " x " + Paras.AnchorGravityCoef + " x " + a1 + " + " + Paras.LoadRatio + " x " + Paras.RQ + " x " + Paras.VcFInstall + " x (" + Paras.DynamicCoef + " x " + a3 + " x sin(" + a4  + ") + "+ a21 + " x " + a22 + ") = " + res.ToString("0.00");
            return res;
        }

        /// <summary>
        /// 针对OPGW锚线工况X
        /// 工况代码"MO", "MOa", "MOb", "MOc", "MOd", "MOe", "MOf", "MOg", "MOh"
        /// OPGW开段计算 左地线开段
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <returns></returns>
        public float MO1X(float a1, float a2, out string str)
        {
            float res = Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * Paras.OtherWindAdjustFactor * AngX(a1) * a2;
            str = Paras.LoadRatio + " x " + Paras.RQ + " x " + Paras.VcFInstall + " x " + Paras.OtherWindAdjustFactor + " x " + AngX(a1) + " x " + a2 + " = " + res.ToString("0.00"); ;
            return res;
        }

        /// <summary>
        /// 针对OPGW锚线工况Y
        /// 工况代码"MO", "MOa", "MOb", "MOc", "MOd", "MOe", "MOf", "MOg", "MOh"
        /// OPGW开段计算 左地线开段
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <param name="a4"></param>
        /// <param name="a5"></param>
        /// <returns></returns>
        public float MO1Y(float a1, float a2, float a3, float a4, float a5, out string str)
        {
            float res = Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * Paras.OtherWindAdjustFactor * AngY(a1) * a2 + Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * (a3 - a4 * Paras.DynamicCoef * a5 * (1 - (float)Math.Cos(Paras.AnchorAngle * PI / 180)));
            str = Paras.LoadRatio + " x " + Paras.RQ + " x " + Paras.VcFInstall + " x " + Paras.OtherWindAdjustFactor + " x " + AngY(a1) + " x " + a2 + " + " + Paras.LoadRatio + " x " + Paras.RQ + " x " + Paras.VcFInstall + " x (" + a3 + " - " + a4 + " x " + Paras.DynamicCoef + " x " + a5 + " x (" + 1 + " - cos( " + Paras.AnchorAngle  + "))) = " + res.ToString("0.00");
            return res;
        }

        /// <summary>
        /// 针对OPGW锚线工况Z
        /// 工况代码"MO", "MOa", "MOb", "MOc", "MOd", "MOe", "MOf", "MOg", "MOh"
        /// OPGW开段计算 左地线开段
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a3"></param>
        /// <param name="a4"></param>
        /// <param name="a5"></param>
        /// <returns></returns>
        public float MO1Z(float a1, float a3, float a4, float a51, float a52, out string str)
        {
            if (a1 >= 0)
            {
                float res = Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * (a3 * Paras.DynamicCoef * a4 * (float)Math.Sin(Paras.AnchorAngle * PI / 180) + a51 * a52) + Paras.LoadRatio * Paras.RGBad * a1;
                str = Paras.LoadRatio + " x " + Paras.RQ + " x " + Paras.VcFInstall + " x (" + a3 + " x " + Paras.DynamicCoef + " x " + a4 + " x sin(" + Paras.AnchorAngle  +") + "  + a51 + " x " + a52  + ") + " +  Paras.LoadRatio + " x " + Paras.RGBad + " x " + a1 + " = " + res.ToString("0.00");
                return res;
            }
            else
            {
                float res = Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * (a3 * Paras.DynamicCoef * a4 * (float)Math.Sin(Paras.AnchorAngle * PI / 180) + a51 * a52) + Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * a1;
                str = Paras.LoadRatio + " x " + Paras.RQ + " x " + Paras.VcFInstall + " x (" + a3 + " x " + Paras.DynamicCoef + " x " + a4 + " x sin(" + Paras.AnchorAngle  + ") + " + a51 + " x " + a52 + ") + "  + Paras.LoadRatio + " x " + Paras.RQ + " x " + Paras.VcFInstall + " x " + a1 + " = " + res.ToString("0.00");
                return res;
            }
        }

        /// <summary>
        /// 针对OPGW锚线工况X
        /// 工况代码"MO", "MOa", "MOb", "MOc", "MOd", "MOe", "MOf", "MOg", "MOh"
        /// 右地线已架
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <returns></returns>
        public float MO2X(float a1, float a2, out string str)
        {
            float res = Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * Paras.OtherWindAdjustFactor * AngX(a1) * a2;
            str = Paras.LoadRatio + " x " + Paras.RQ + " x " + Paras.VcFInstall + " x " + Paras.OtherWindAdjustFactor + " x " + AngX(a1) + " x " + a2 + " = " + res.ToString("0.00");
            return res;
        }

        /// <summary>
        /// 针对OPGW锚线工况X
        /// 工况代码"MO", "MOa", "MOb", "MOc", "MOd", "MOe", "MOf", "MOg", "MOh"
        /// 右地线已架
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <param name="a4"></param>
        /// <returns></returns>
        public float MO2Y(float a1, float a2, float a3, float a4, out string str)
        {
            float res = Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * (Paras.OtherWindAdjustFactor * AngY(a1) * a2 + (a3 - a4) * 0);
            str = Paras.LoadRatio + " x " + Paras.RQ + " x " + Paras.VcFInstall + " x (" + Paras.OtherWindAdjustFactor + " x " + AngY(a1) + " x " + a2 + " + (" + a3 + " - " + a4 + ") x 0) = " + res.ToString("0.00");
            return res;
        }

        /// <summary>
        /// 针对OPGW锚线工况X
        /// 工况代码"MO", "MOa", "MOb", "MOc", "MOd", "MOe", "MOf", "MOg", "MOh"
        /// 右地线已架
        /// </summary>
        /// <param name="a1"></param>
        /// <returns></returns>
        public float MO2Z(float a1, out string str)
        {
            if (a1 >= 0)
            {
                float res = Paras.LoadRatio * Paras.RGBad * a1;
                str = Paras.LoadRatio + " x " + Paras.RGBad + " x " + a1 + " = " + res.ToString("0.00");
                return res;
            }
            else
            {
                float res = Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * a1;
                str = Paras.LoadRatio + " x " + Paras.RQ + " x " + Paras.VcFInstall + " x " + a1 + " = " + res.ToString("0.00");
                return res;
            }
                
        }

        /// <summary>
        /// 脱冰跳跃相X
        /// 工况代码："T"
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <returns></returns>
        public float ZXTX(float a1, float a2, out string str)
        {
            float res = Paras.LoadRatio * Paras.RA * Paras.VcFCheck * Paras.OtherWindAdjustFactor * AngX(a1) * a2;
            str = Paras.LoadRatio + " x " + Paras.RA + " x " + Paras.VcFCheck + " x " + Paras.OtherWindAdjustFactor + " x " + AngX(a1) + " x " + a2 + " = " + res.ToString("0.00");
            return res;
        }

        /// <summary>
        /// 脱冰跳跃相Y
        /// 工况代码："T"
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <param name="a4"></param>
        /// <param name="a5"></param>
        /// <returns></returns>
        public float ZXTY(float a1, float a2, float a3, float a4, float a5, out string str)
        {
            float res =  Paras.LoadRatio * Paras.RA * Paras.VcFCheck * (Paras.OtherWindAdjustFactor * AngY(a1) * a2 + (a3 - a4) * a5);
            str = Paras.LoadRatio + " x " + Paras.RA + " x " + Paras.VcFCheck + " x (" + Paras.OtherWindAdjustFactor + " x " + AngY(a1) + " x " + a2 + " + (" + a3 + " - " + a4 + ") x " + a5 + ") = " + res.ToString("0.00");
            return res;
        }

        /// <summary>
        /// 脱冰跳跃相Z1
        /// 工况代码："T"
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <returns></returns>
        public float ZXTZ1(float a1, float a2, out string str)
        {
            float res = Paras.LoadRatio * Paras.RA * Paras.VcFCheck * a2;
            str = Paras.LoadRatio + " x " + Paras.RA + " x " + Paras.VcFCheck + " x " + a2 + " = " + res.ToString("0.00"); 
            return res;
        }

        /// <summary>
        /// 脱冰跳跃相Z2
        /// 工况代码："T"
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <returns></returns>
        public float ZXTZ2(float a1, float a2, out string str)
        {
            if (a1 >= 0)
            {
                float res = Paras.LoadRatio * Paras.RGCheck01 * a1 + Paras.LoadRatio * Paras.RA * Paras.VcFCheck * (a2 - a1);
                str = Paras.LoadRatio + " x " + Paras.RGCheck01 + " x " + a1 + " + " + Paras.LoadRatio + " x " + Paras.RA + " x " + Paras.VcFCheck + " x (" + a2 + " - " + a1 + ") = " + res.ToString("0.00");
                return res;
            }
            else
            {
                float res = Paras.LoadRatio * Paras.RA * Paras.VcFCheck * a2;
                str = Paras.LoadRatio + " x " + Paras.RA + " x " + Paras.VcFCheck + " x " + a2 + " = " + res.ToString("0.00"); ;
                return res;
            }               
        }
    }
}
