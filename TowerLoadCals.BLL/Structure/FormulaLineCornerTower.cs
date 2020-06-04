using System;
using TowerLoadCals.Mode;

namespace TowerLoadCals.BLL
{
    public class FormulaLineCornerTower : FormulaTower
    {
        public FormulaLineCornerTower(FormulaParas paras) : base(paras)
        {
        }

        /// <summary>
        /// 大风X
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <param name="a4"></param>
        /// <param name="a5"></param>
        /// <param name="a6"></param>
        /// <returns></returns>
        public float ZZNX(float a1 , float a2 , float a3, float a4, float a5, float a6, out string str)
        {
            float res = Paras.LoadRatio * Paras.RQ * a6 * (Paras.WindAdjustFactor * AngX(a1) * a2 + (a3 + a4) * (float)Math.Sin(a5 * PI / 360));
            str = Paras.LoadRatio + " x " + Paras.RQ + " x " + a6 + " x (" + Paras.WindAdjustFactor + " x " + AngX(a1) + " x " + a2 + " + (" + a3 + " + " + a4 + ") x sin(" + a5 / 2 + ")) = " + res.ToString("0.00");
            return res;
        }

        /// <summary>
        /// 大风Y
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <param name="a4"></param>
        /// <param name="a5"></param>
        /// <param name="a6"></param>
        /// <returns></returns>
        public float ZZNY(float a1 , float a2 , float a3, float a4, float a5, float a6, out string str)
        {
            float res = Paras.LoadRatio * Paras.RQ * a6 * (Paras.WindAdjustFactor * AngY(a1) * a2 + (a3 - a4) * (float)Math.Cos(a5 * PI / 360));
            str = Paras.LoadRatio + " x " + Paras.RQ + " x " + a6 + " x (" + Paras.WindAdjustFactor + " x " + AngY(a1) + " x " + a2 + " + (" + a3 + " - " + a4 + ") x cos(" + a5 / 2 + ")) = " + res.ToString("0.00");
            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <returns></returns>
        public float ZZNZ(float a1 , float a2 , float a3, out string str)
        {
            if (a1 >= 0)
            {
                float res = Paras.LoadRatio * a2 * a1;
                str = Paras.LoadRatio + " x " + a2 + " x " + a1 + " = " + res.ToString("0.00");
                return res;
            }
            else
            {
                float res = Paras.LoadRatio * Paras.RQ * a1 * a3;
                str = Paras.LoadRatio + " x " + Paras.RQ + " x " + a1 + " x " + a3 + " = " + res.ToString("0.00"); ;
                return res;
            }
        }

        /// <summary>
        /// 不均匀冰
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <param name="a4"></param>
        /// <param name="a5"></param>
        /// <param name="a6"></param>
        /// <param name=""></param>
        /// <returns></returns>
        public float ZZIYX(float a1 , float a2 , float a3, float a4, float a5, float a6, float a7, out string str)
        {
            float res = Paras.LoadRatio * a7 * a6 * (Paras.OtherWindAdjustFactor * AngX(a1) * a2 + (a3 + a4) * (float)Math.Sin(a5 * PI / 360));
            str = Paras.LoadRatio + " x " + a7 + " x " + a6 + " x (" + Paras.OtherWindAdjustFactor + " x " + AngX(a1) + " x " +  a2 + " + (" + a3 + " + " + a4 + ") x sin(" + a5 / 2 + ")) = "  +res.ToString("0.00");
            return res;
        }

        /// <summary>
        /// a5为方向判断
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <param name="a4"></param>
        /// <param name="a5"></param>
        /// <param name="a6"></param>
        /// <param name=""></param>
        /// <returns></returns>
        public float ZZIYY(float a1 , float a2 , float a3, float a4, float a5, float a6, float a7, out string str)
        {
            float res = Paras.LoadRatio * a7 * a6 * (Paras.OtherWindAdjustFactor * AngY(a1) * a2 + (a3 - a4) * (float)Math.Cos(a5 * PI / 360));
            str = Paras.LoadRatio + " x " + a7 + " x " + a6 + " x (" + Paras.OtherWindAdjustFactor + " x " + AngY(a1) + " x " + a2 + " + (" + a3 + " - " + a4 + ") x cos(" + a5 / 2 + ")) = " + res.ToString("0.00");
            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <param name="a6"></param>
        /// <param name=""></param>
        /// <returns></returns>
        public float ZZIYZ(float a1 , float a2 , float a3, float a6, float a7, out string str)
        {
            if (a1 >= 0)
            {
                float res = Paras.LoadRatio * a3 * a1 + Paras.LoadRatio * a7 * a6 * (a2 - a1);
                str = Paras.LoadRatio + " x " + a3 + " x " + a1 + " + " + Paras.LoadRatio + " x " + a7 + " x " + a6 + " x (" + a2 + " - " + a1 + ") = " + res.ToString("0.00");
                return res;
            }
            else
            {
                float res = Paras.LoadRatio * a7 * a6 * a2;
                str = Paras.LoadRatio + " x " + a7 + " x " + a6 + " x " + a2 + " = " + res.ToString("0.00"); ;
                return res;
            }
        }

        /// <summary>
        /// 脱冰跳跃相
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <param name="a4"></param>
        /// <param name="a5"></param>
        /// <returns></returns>
        public float ZZTX(float a1 , float a2 , float a3, float a4, float a5, out string str)
        {
            float res = Paras.LoadRatio * Paras.RA * Paras.VcFCheck * (Paras.OtherWindAdjustFactor * AngX(a1) * a2 + (a3 + a4) * (float)Math.Sin(a5 * PI / 360));
            str = Paras.LoadRatio + " x " + Paras.RA + " x " + Paras.VcFCheck + " x (" + Paras.OtherWindAdjustFactor + " x " + AngX(a1) + " x " + a2 + " + (" + a3 + " + " + a4 + ") x sin(" + a5 / 2 + ")) = " + res.ToString("0.00");
            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <param name="a4"></param>
        /// <param name="a5"></param>
        /// <returns></returns>
        public float ZZTY(float a1 , float a2 , float a3, float a4, float a5, out string str)
        {
            float res = Paras.LoadRatio * Paras.RA * Paras.VcFCheck * (Paras.OtherWindAdjustFactor * AngY(a1) * a2 + (a3 - a4) * (float)Math.Cos(a5 * PI / 360));
            str = Paras.LoadRatio + " x " + Paras.RA + " x " + Paras.VcFCheck + " x (" + Paras.OtherWindAdjustFactor + " x " + AngY(a1) + " x " + a2 + " + (" + a3 + " = " + a4 + "） x cos(" + a5 / 2 + ")) = " + res.ToString("0.00");
            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <returns></returns>
        public float ZZTZ1(float a1 , float a2, out string str)
        {
            float res = Paras.LoadRatio * Paras.RA * Paras.VcFCheck * a2;
            str = Paras.LoadRatio + " x " + Paras.RA + " x " + Paras.VcFCheck + " x " + a2 + " = " + res.ToString("0.00"); 
            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <returns></returns>
        public float ZZTZ2(float a1 , float a2, out string str)
        {
            if (a1 >= 0)
            {
                float res = Paras.LoadRatio * Paras.RGCheck01 * a1 + Paras.LoadRatio * Paras.RA * Paras.VcFCheck * (a2 - a1);
                str = Paras.LoadRatio + " x " + Paras.RGCheck01 + " x " + a1 + " + " + Paras.LoadRatio + " x " + Paras.RA + " x " + Paras.VcFCheck + " x (" + a2 + " = " + a1 + ") = " + res.ToString("0.00");
                return res;
            }
            else
            {
                float res = Paras.LoadRatio * Paras.RA * Paras.VcFCheck * a2;
                str = Paras.LoadRatio + " x " + Paras.RA + " x " + Paras.VcFCheck + " x " + a2 + " = " + res.ToString("0.00");
                return res;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <param name="a4"></param>
        /// <param name="a5"></param>
        /// <returns></returns>
        public float ZZLX(float a1 , float a2 , float a3, float a4, float a5, out string str)
        {
            float res = Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * (Paras.OtherWindAdjustFactor * AngX(a1) * a2 + (a3 + a4) * (float)Math.Sin(a5 * PI / 360));
            str = Paras.LoadRatio + " x " + Paras.RQ + " x " + Paras.VcFInstall + " x (" + Paras.OtherWindAdjustFactor + " x " + AngX(a1) + " x " + a2 + " + (" + a3 + " + " + a4 + ") x sin(" + a5 / 2 + ")) = " + res.ToString("0.00");
            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <param name="a4"></param>
        /// <param name="a5"></param>
        /// <returns></returns>
        public float ZZLY(float a1 , float a2 , float a3, float a4, float a5, out string str)
        {
            float res = Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * (Paras.OtherWindAdjustFactor * AngY(a1) * a2 + (a3 - a4) * (float)Math.Cos(a5 * PI / 360));
            str = Paras.LoadRatio + " x " + Paras.RQ + " x " + Paras.VcFInstall + " x (" + Paras.OtherWindAdjustFactor + " x " + AngY(a1) + " x " + a2 + " + (" + a3 + " - " + a4 + ") x cos(" + a5 / 2 + ")) = " + res.ToString("0.00");
            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a1"></param>
        /// <returns></returns>
        public float ZZLZ1(float a1, out string str)
        {
            if (a1 >= 0)
            {
                float res = Paras.LoadRatio * Paras.RGBad * a1;
                str = Paras.LoadRatio + " x " + Paras.RGBad + " x " + a1 + " = " + res.ToString("0.00"); 
                return res;
            }
            else
            {
                float res = Paras.LoadRatio * Paras.VcFInstall * Paras.RQ * a1;
                str = Paras.LoadRatio + " x " + Paras.VcFInstall + " x " + Paras.RQ + " x " + a1 + " = " + res.ToString("0.00"); 
                return res;
            }
        }

        /// <summary>
        /// 正吊
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <returns></returns>
        public float ZZLZ2(float a1 , float a2 , float a31, float a32, out string str)
        {
            float res = Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * (a1 * Paras.DynamicCoef * a2 + a31*a32);
            str = Paras.LoadRatio + " x " + Paras.RQ + " x " + Paras.VcFInstall + " x (" + a1 + " x " + Paras.DynamicCoef + " x " + a2 + " + " + a31 + " x " + a32 + ") = " + res.ToString("0.00");
            return res;
        }

        /// <summary>
        /// 过滑车,正过滑车
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <param name="a4"></param>
        /// <param name="a5"></param>
        /// <param name="a6"></param>
        /// <returns></returns>
        public float ZZGX(float a1 , float a2 , float a3, float a4, float a5, float a6, out string str)
        {
            float res = Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * (Paras.OtherWindAdjustFactor * AngX(a1) * a2 + (a3 + Paras.DynamicCoef * a6 + a4) * (float)Math.Sin(a5 * PI / 360));
            str = Paras.LoadRatio + " x " + Paras.RQ + " x " + Paras.VcFInstall + " x (" + Paras.OtherWindAdjustFactor + " x " + AngX(a1) + " x " + a2 + " + (" + a3 + " + " + Paras.DynamicCoef + " x " + a6 + " + " + a4 + ") x sin(" + a5 / 2 + ")) = " + res.ToString("0.00");
            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <param name="a4"></param>
        /// <param name="a5"></param>
        /// <param name="a6"></param>
        /// <returns></returns>
        public float ZZGY(float a1 , float a2 , float a3, float a4, float a5, float a6, out string str)
        {
            float res = Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * (Paras.OtherWindAdjustFactor * AngY(a1) * a2 + (a3 + Paras.DynamicCoef * a6 - a4) * (float)Math.Cos(a5 * PI / 360));
            str = Paras.LoadRatio + " x " + Paras.RQ + " x " + Paras.VcFInstall + " x (" + Paras.OtherWindAdjustFactor + " x " + AngY(a1) + " x " + a2 + " + (" + a3 + " + " + Paras.DynamicCoef + " x " + a6 + " - " + a4 + ") x cos(" + a5 / 2 + ")) = " + res.ToString("0.00");
            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <returns></returns>
        public float ZZGZ(float a1 , float a2 , float a31, float a32, out string str)
        {
            if (a1 >= 0)
            {
                float res = Paras.LoadRatio * Paras.RGBad * a1 + Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * ((a2 - a1) + a31 * a32);
                str = Paras.LoadRatio + " x " + Paras.RGBad + " x " + a1 + " + " + Paras.LoadRatio + " x " + Paras.RQ + " x " + Paras.VcFInstall + " x ((" + a2 + " - " + a1 + ") + " + a31 + " x " + a32 + ") = " + res.ToString("0.00");
                return res;
            }
            else
            {
                float res = Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * a2 + Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * a31 * a32;
                str = Paras.LoadRatio + " x " + Paras.RQ + " x " + Paras.VcFInstall + " x " + a2 + " + " + Paras.LoadRatio + " x " + Paras.RQ + " x " + Paras.VcFInstall + " x " + a31 + " x " + a32 + " = " + res.ToString("0.00"); ;
                return res;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <returns></returns>
        public float ZZCZ(float a1 , float a2 , float a31, float a32,  out string str)
        {
            float res = Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * (Paras.DynamicCoef * a1 * a2 + a31 * a32);
            str = Paras.LoadRatio + " x " + Paras.RQ + " x " + Paras.VcFInstall + " x (" + Paras.DynamicCoef + " x " + a1 + " x " + a2 + " x " + a31 + " x " +  a32 + ") = " + res.ToString("0.00"); 
            return res;
        }
    }
}
