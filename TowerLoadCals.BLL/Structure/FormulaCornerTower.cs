using System;
using TowerLoadCals.Mode;

namespace TowerLoadCals.BLL
{
    public class FormulaCornerTower : FormulaTower
    {
        public FormulaCornerTower(FormulaParas paras) : base(paras)
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
        /// <returns></returns>
        public float ZNX(out string str, float a1, float a2, float a3, float a4, float a5, int BL3, int? nt = null, float? BLT = null)
        {
            float res = Paras.LoadRatio * Paras.RQ * a5 * (Paras.WindAdjustFactor * AngX(a1) * a3 + a4 * (float)Math.Sin(a2 * PI / 180)) * BL3;

            str = Paras.LoadRatio + " x " +  Paras.RQ + " x " + a5 + " x (" + Paras.WindAdjustFactor + " x " +  AngX(a1) + " x " + a3 + " + " +  a4 + " x sin(" + a2 + ") x " + BL3;

            if (nt.HasValue)
            {
                res = (float)(res * nt);
                str = str + " x " + nt;
            }

            if(BLT.HasValue)
            {
                res = (float)(res * BLT);
                str = str + " x " + BLT;
            }

            str = str + " = " + res.ToString("0.00");
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
        /// <returns></returns>
        public float ZNY(out string str, float a1, float a2, float a3, float a4, float a5, int? nt = null, float? BLT = null)
        {
            float res = Paras.LoadRatio * Paras.RQ * a5 * (Paras.WindAdjustFactor * AngY(a1) * a3 + a4 * (float)Math.Cos(a2 * PI / 180));
            str = Paras.LoadRatio + " x " + Paras.RQ + " x " + a5 + " x (" + Paras.WindAdjustFactor + " x " + AngY(a1) + " x " + a3 + " + " + a4 + " x cos(" + a2 + ")";

            if (nt.HasValue)
            {
                res = (float)(res * nt);
                str = str + " x " + nt;
            }

            if (BLT.HasValue)
            {
                res = (float)(res * BLT);
                str = str + " x " + BLT;
            }

            str = str + " = " + res.ToString("0.00");
            return res;

        }

        /// <summary>
        /// 大风Z
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a5"></param>
        /// <returns></returns>
        public float ZNZ(out string str, float a1, float a5, int? nt = null, float? BLT = null)
        {
            float res;

            if (a1 >= 0)
            {
                res = Paras.LoadRatio * Paras.RGBad * a1;
                str = Paras.LoadRatio + " x " + Paras.RGBad + " x " + a1;
            }
            else
            {
                res = Paras.LoadRatio * a5 * Paras.RQ * a1;
                str = Paras.LoadRatio + " x " + a5 + " x " + Paras.RQ + " x " + a1;
            }

            if (nt.HasValue)
            {
                res = (float)(res * nt);
                str = str + " x " + nt;
            }

            if (BLT.HasValue)
            {
                res = (float)(res * BLT);
                str = str + " x " + BLT;
            }

            str = str + " = " + res.ToString("0.00");
            return res;

        }

        /// <summary>
        /// 覆冰、验冰、断线、不均匀冰基本相同，合并
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <param name="a4"></param>
        /// <param name="a5"></param>
        /// <param name="a6"></param>
        /// <returns></returns>
        public float ZIBUYX(out string str, float a1, float a2, float a3, float a4, float a5, float a6,  int BL3, int? nt = null, float? BLT = null)
        {
            float res = Paras.LoadRatio * a6 * a5 * (Paras.OtherWindAdjustFactor * AngX(a1) * a3 + a4 * (float)Math.Sin(a2 * PI / 180)) * BL3;
            str = Paras.LoadRatio + " x " + a6 + " x " + a5 + " x (" + Paras.OtherWindAdjustFactor + " x " + AngX(a1) + " x " + a3 + " + " + " x sin(" + a2 + ")) x " + BL3;

            if (nt.HasValue)
            {
                res = (float)(res * nt);
                str = str + " x " + nt;
            }

            if (BLT.HasValue)
            {
                res = (float)(res * BLT);
                str = str + " x " + BLT;
            }

            str = str + " = " + res.ToString("0.00");
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
        public float ZIBUYY(out string str, float a1, float a2, float a3, float a4, float a5, float a6, int? nt = null, float? BLT = null)
        {
            float res =  Paras.LoadRatio * a6 * a5 * (Paras.OtherWindAdjustFactor * AngY(a1) * a3 + a4 * (float)Math.Cos(a2 * PI / 180));
            str = Paras.LoadRatio + " x " + a6 + " x " + a5 + " x (" + Paras.OtherWindAdjustFactor + " x " + AngY(a1) + " x " + a3 + " + " + a4 + " x cos(" + a2 + ")";

            if (nt.HasValue)
            {
                res = (float)(res * nt);
                str = str + " x " + nt;
            }

            if (BLT.HasValue)
            {
                res = (float)(res * BLT);
                str = str + " x " + BLT;
            }

            str = str + " = " + res.ToString("0.00");
            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a5"></param>
        /// <param name="a6"></param>
        /// <param name="a7"></param>
        /// <returns></returns>
        public float ZIBUYZ(out string str, float a1, float a2, float a5, float a6, float a7, int? nt = null, float? BLT = null)
        {
            float res;

            if (a1 >= 0)
            {
                res = Paras.LoadRatio * (a7 * a1 + a6 * a5 * (a2 - a1));
                str = Paras.LoadRatio + " x (" + a7 + " x " + a1 + " + " + a6 + " x " + a5 + " x (" + a2 + " - " + a1 + "))";
            }
            else
            {
                res = Paras.LoadRatio * a5 * a6 * a2;
                str = Paras.LoadRatio + " x " + a5 + " x " + a6 + " x " + a2;
            }

            if (nt.HasValue)
            {
                res = (float)(res * nt);
                str = str + " x " + nt;
            }

            if (BLT.HasValue)
            {
                res = (float)(res * BLT);
                str = str + " x " + BLT;
            }

            str = str + " = " + res.ToString("0.00");
            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <param name="a4"></param>
        /// <returns></returns>
        public float ZLX(out string str, float a1, float a2, float a3, float a4, int BL3, int? nt = null, float? BLT = null)
        {
            float res =  Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * (Paras.OtherWindAdjustFactor * AngX(a1) * a3 + a4 * (float)Math.Sin(a2 * PI / 180))* BL3;
            str = Paras.LoadRatio + " x " + Paras.RQ + " x " + Paras.VcFInstall + " x (" + Paras.OtherWindAdjustFactor + " x " + AngX(a1) + " x " + a3 + " + " + a4 + " x sin(" + a2 + ")) x " + BL3;

            if (nt.HasValue)
            {
                res = (float)(res * nt);
                str = str + " x " + nt;
            }

            if (BLT.HasValue)
            {
                res = (float)(res * BLT);
                str = str + " x " + BLT;
            }

            str = str + " = " + res.ToString("0.00");
            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <param name="a4"></param>
        /// <returns></returns>
        public float ZLY(out string str, float a1, float a2, float a3, float a4, int? nt = null, float? BLT = null)
        {
            float res = Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * (Paras.OtherWindAdjustFactor * AngY(a1) * a3 + a4 * (float)Math.Cos(a2 * PI / 180));
            str = Paras.LoadRatio + " x " + Paras.RQ + " x " + Paras.VcFInstall + " x (" + Paras.OtherWindAdjustFactor + " x " + AngY(a1) + " x " + a3 + " + " + a4 + " x cos(" + a2 + "))";

            if (nt.HasValue)
            {
                res = (float)(res * nt);
                str = str + " x " + nt;
            }

            if (BLT.HasValue)
            {
                res = (float)(res * BLT);
                str = str + " x " + BLT;
            }

            str = str + " = " + res.ToString("0.00");
            return res;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a1"></param>
        /// <returns></returns>
        public float ZLZ(out string str, float a1)
        {
            float res;

            if (a1 >= 0)
            {
                res = Paras.LoadRatio * Paras.RGBad * a1;
                str = Paras.LoadRatio + " x " + Paras.RGBad + " x " + a1;
            }
            else
            {
                res = Paras.LoadRatio* Paras.VcFInstall* Paras.RQ* a1;
                str = Paras.LoadRatio + " x " + Paras.VcFInstall + " x " + Paras.RQ + " x " + a1;
            }

            str = str + " = " + res.ToString("0.00");
            return res;
        }

        /// <summary>
        /// 正起吊
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <returns></returns>
        public float ZLT1Z(out string str, float a1, float a2, float a31, float a32, float? BLT = null, bool bBLTInside = false)
        {
            float res;

            if (bBLTInside)
            {
                res = Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * (a31 * a32 + Paras.LiftCoefJumper * a1 * a2 * (float)BLT * Paras.DynamicCoef);
                str = Paras.LoadRatio + " x " + Paras.RQ + " x " + Paras.VcFInstall + " x （" + a31 + " x " + a32 + " + " + Paras.LiftCoefJumper + " x " + a1 + " x " + a2 + " x " + BLT + " x " + Paras.DynamicCoef + ")";
            }
            else
            {
                res = Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * (a31 * a32 + Paras.LiftCoefJumper * a1 * a2 * Paras.DynamicCoef);
                str = Paras.LoadRatio + " x " + Paras.RQ + " x " + Paras.VcFInstall + " x （" + a31 + " x " + a32 + " + " + Paras.LiftCoefJumper + " x " + a1 + " x " + a2 + " x " + Paras.DynamicCoef + ")";

                if (BLT.HasValue)
                {
                    res = (float)(res * BLT);
                    str = str + " x " + BLT;
                }
            }

            str = str + " = " + res.ToString("0.00");
            return res;
        }

        /// <summary>
        /// 已起吊
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <returns></returns>
        public float ZLT2Z(out string str, float a1, float a2, float a3, float? BLT = null)
        {
            //a2.a3为安装附加荷载，已吊装时无，与直线塔统一
            //20160904 修改已吊表达式

            float res; 

            if (a1 >= 0)
            {
                res = Paras.LoadRatio * Paras.RGBad * a1 * a2;
                str = Paras.LoadRatio + " x " + Paras.RGBad + " x " + a1 + " x " + a2;
            }
                
            else
            {
                res = Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * a1 * a2;
                str = Paras.LoadRatio + " x " + Paras.RQ + " x " + Paras.VcFInstall + " x " + a1 + " x " + a2;
            }

            if (BLT.HasValue)
            {
                res = (float)(res * BLT);
                str = str + " x " + BLT;
            }

            str = str + " = " + res.ToString("0.00");
            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <param name="a4"></param>
        /// <returns></returns>
        public float ZM1X(out string str, float a1, float a2, float a3, float a4, float BL3) 
        {
            float res = Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * (Paras.OtherWindAdjustFactor * AngX(a1) * a3 + (a4 * Paras.DynamicCoef * LineParas.DrawingCoef - LineParas.TemporaryTension) * (float)Math.Sin(a2 * PI / 180)) * BL3;
            str = Paras.LoadRatio + " x " + Paras.RQ + " x " + Paras.VcFInstall + " x (" + Paras.OtherWindAdjustFactor + " x " + AngX(a1) + " x " + a3 + " + (" + a4 + " x " + Paras.DynamicCoef + " x " + LineParas.DrawingCoef + " - " + LineParas.TemporaryTension + ") x sin(" + a2 + ")) x " + BL3 + " = " + res.ToString("0.00");
            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <param name="a4"></param>
        /// <returns></returns>
        public float ZM1Y(out string str, float a1, float a2, float a3, float a4) 
        {
            float res = Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * (Paras.OtherWindAdjustFactor * AngY(a1) * a3 + (a4 * Paras.DynamicCoef * LineParas.DrawingCoef - LineParas.TemporaryTension) * (float)Math.Cos(a2 * PI / 180));
            str = Paras.LoadRatio + " x " + Paras.RQ + " x " + Paras.VcFInstall + " x (" + Paras.OtherWindAdjustFactor + " x " + AngY(a1) + " x " + a3 + " + (" + a4 * Paras.DynamicCoef + " x " + LineParas.DrawingCoef + " - " + LineParas.TemporaryTension + ") x sin(" + a2 + ") = " + res.ToString("0.00");
            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <returns></returns>
        public float ZM1Z(out string str, float a1, float a21, float a22)
        {
            if (a1 >= 0)
            {
                float res = Paras.LoadRatio * (Paras.RGBad * a1 + Paras.RQ * Paras.VcFInstall * (LineParas.TemporaryTension * (float)Math.Tan(Paras.TempStayWireAngle * PI / 180) + a21 * a22));
                str = Paras.LoadRatio + " x (" + Paras.RGBad + " x " + a1 + " + " + Paras.RQ * Paras.VcFInstall + " x (" + LineParas.TemporaryTension + " x tan(" + Paras.TempStayWireAngle + ") + " + a21 + " + " + a22 + ")) = " + res.ToString("0.00");
                return res;
            }
            else
            {
                float res =  Paras.LoadRatio * (Paras.RQ * Paras.VcFInstall * a1 + Paras.RQ * Paras.VcFInstall * (LineParas.TemporaryTension * (float)Math.Tan(Paras.TempStayWireAngle * PI / 180) + a21 * a22));
                str = Paras.LoadRatio + " x (" + Paras.RQ + " x " + Paras.VcFInstall + " x " + a1 + " + " + Paras.RQ + " x " + Paras.VcFInstall + " x (" + LineParas.TemporaryTension + " x tan(" + Paras.TempStayWireAngle + ") + " + a21 + " x " + a22 + ")) = " + res.ToString("0.00");
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
        /// <returns></returns>
        public float ZM2X(out string str, float a1, float a2, float a3, float a4, float BL3)  
        {
            float res = Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * (Paras.OtherWindAdjustFactor * AngX(a1) * a3 + (a4 - LineParas.TemporaryTension) * (float)Math.Sin(a2 * PI / 180)) * BL3;
            str = Paras.LoadRatio + " x " + Paras.RQ + " x " + Paras.VcFInstall + " x (" + Paras.OtherWindAdjustFactor + " x " + AngX(a1) + " x " + a3 + " + (" + a4 + " - " + LineParas.TemporaryTension + ") x sin(" + a2 + ")) x " + BL3 + "= " + res.ToString("0.00");
            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <param name="a4"></param>
        /// <returns></returns>
        public float ZM2Y(out string str, float a1, float a2, float a3, float a4) 
        {
            float res = Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * (Paras.OtherWindAdjustFactor * AngY(a1) * a3 + (a4 - LineParas.TemporaryTension) * (float)Math.Cos(a2 * PI / 180));
            str = Paras.LoadRatio + " x " + Paras.RQ + " x " + Paras.VcFInstall + " x " + Paras.OtherWindAdjustFactor + " x " + AngY(a1) + " x " + a3 + " x (" + a4 + " - " + LineParas.TemporaryTension + ") x cos(" + a2 + ")) = " + res.ToString("0.00");
            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a1"></param>
        /// <returns></returns>
        public float ZM2Z(out string str, float a1) 
        {
            if (a1 >= 0)
            {
                float res =  Paras.LoadRatio * (Paras.RGBad * a1 + Paras.RQ * Paras.VcFInstall * LineParas.TemporaryTension * (float)Math.Tan(Paras.TempStayWireAngle * PI / 180));
                str = Paras.LoadRatio + " x (" + Paras.RGBad + " x " + a1 + " + " + Paras.RQ + " x " + Paras.VcFInstall + " x " + LineParas.TemporaryTension + " x tan(" + Paras.TempStayWireAngle + ")) = " + res.ToString("0.00");
                return res;
            }
            else
            {
                float res = Paras.LoadRatio * (Paras.RQ * Paras.VcFInstall * a1 + Paras.RQ * Paras.VcFInstall * LineParas.TemporaryTension * (float)Math.Tan(Paras.TempStayWireAngle * PI / 180));
                str = Paras.LoadRatio + " x (" + Paras.RQ * Paras.VcFInstall + " x " + a1 + " + " + Paras.RQ * Paras.VcFInstall + " x " + LineParas.TemporaryTension + " x tan(" + Paras.TempStayWireAngle + ")) = " + res.ToString("0.00");
                return res;
            }
        }

        /// <summary>
        /// 正牵
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <param name="a4"></param>
        /// <returns></returns>
        public float MQ1X(out string str, float a1, float a2, float a3, float a4, float BL3)
        {
            float res = Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * (Paras.OtherWindAdjustFactor * AngX(a1) * a3 + a4 * Paras.DynamicCoef * LineParas.DrawingCoef * (1 - (float)Math.Cos(Paras.TractionAgnle * PI / 180)) * (float)Math.Sin(a2 * PI / 180)) * BL3;
            str = Paras.LoadRatio + " x " + Paras.RQ + " x " + Paras.VcFInstall + " x (" + Paras.OtherWindAdjustFactor + " x " + AngX(a1) + " x " + a3 + " + " + a4 + " x " + Paras.DynamicCoef + " x " + LineParas.DrawingCoef + " x (" + 1 + " - cos(" + Paras.TractionAgnle + ")) x sin(" + a2 + ")) x " + BL3 + " = " + res.ToString("0.00"); 
            return res;
        }

        /// <summary> + " x " + 
        /// 
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>        }

        /// <summary>
        /// 

        /// <param name="a4"></param>
        /// <returns></returns>
        public float MQ1Y(out string str, float a1, float a2, float a3, float a4)
        {
            float res = Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * (Paras.OtherWindAdjustFactor * AngY(a1) * a3 + a4 * Paras.DynamicCoef * LineParas.DrawingCoef * (1 - (float)Math.Cos(Paras.TractionAgnle * PI / 180)) * (float)Math.Cos(a2 * PI / 180));
            str = Paras.LoadRatio + " x " + Paras.RQ + " x " + Paras.VcFInstall + " x (" + Paras.OtherWindAdjustFactor + " x " + AngY(a1) + " x " + a3 + " + " + a4 + " x " + Paras.DynamicCoef + " x " + LineParas.DrawingCoef + " x (" + 1 + " - cos(" + Paras.TractionAgnle + ")) x cos(" + a2 + ")) = " + res.ToString("0.00");
            return res;
        }
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <returns></returns>
        public float MQ1Z(out string str, float a1, float a2, float a31, float a32)
        {
            if (a1 >= 0)
            {
                float res = Paras.LoadRatio * (Paras.RGBad * a1 + Paras.RQ * Paras.VcFInstall * (a2 * Paras.DynamicCoef * LineParas.DrawingCoef * (float)Math.Sin(Paras.TractionAgnle * PI / 180) + a31 * a32));
                str = Paras.LoadRatio + " x (" + Paras.RGBad + " x " + a1 + " + " + Paras.RQ + " x " + Paras.VcFInstall + " x (" + a2 * Paras.DynamicCoef + " x " + LineParas.DrawingCoef + " x sin(" + Paras.TractionAgnle * PI / 180 + ") x " + a31 + " x " + a32 + ")) = " + res.ToString("0.00");
                return res;
            }
            else
            {
                float res = Paras.LoadRatio * (Paras.RQ * Paras.VcFInstall * a1 + Paras.RQ * Paras.VcFInstall * (a2 * Paras.DynamicCoef * LineParas.DrawingCoef * (float)Math.Sin(Paras.TractionAgnle * PI / 180) + a31 * a32));
                str = Paras.LoadRatio + " x (" + Paras.RQ + " x " + Paras.VcFInstall + " x " + a1 + " + " + Paras.RQ * Paras.VcFInstall + " x (" + a2 * Paras.DynamicCoef + " x " + LineParas.DrawingCoef + " x sin(" + Paras.TractionAgnle + ") x " + a31 + " x " + a32 + ")) = " + res.ToString("0.00");
                return res;
            }
        }

        /// <summary>
        /// 已锚
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <param name="a4"></param>
        /// <returns></returns>
        public float MQ2X(out string str, float a1, float a2, float a3, float a4, float BL3) 
        {
            float res = Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * (Paras.OtherWindAdjustFactor * AngX(a1) * a3 + (a4 - LineParas.TemporaryTension) * (float)Math.Sin(a2 * PI / 180)) * BL3;
            str = Paras.LoadRatio + " x " + Paras.RQ + " x " + Paras.VcFInstall + " x (" + Paras.OtherWindAdjustFactor + " x " + AngX(a1) + " x " + a3 + " + (" + a4 + " - " + LineParas.TemporaryTension + ") x sin(" + a2 + ")) x " + BL3 + " = " + res.ToString("0.00");
            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <param name="a4"></param>
        /// <returns></returns>
        public float MQ2Y(out string str,  float a1, float a2, float a3, float a4) 
        {
            float res = Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * (Paras.OtherWindAdjustFactor * AngY(a1) * a3 - (a4 - LineParas.TemporaryTension) * (float)Math.Cos(a2 * PI / 180));
            str = Paras.LoadRatio + " x " + Paras.RQ + " x " + Paras.VcFInstall + " x (" + Paras.OtherWindAdjustFactor + " x " + AngY(a1) + " x " + a3 + " - (" + a4 + " - " + LineParas.TemporaryTension + ") x con(" + a2 + ")) = " + res.ToString("0.00");
            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a1"></param>
        /// <returns></returns>
        public float MQ2Z(out string str, float a1) 
        {
            if (a1 >= 0)
            {
                float res = Paras.LoadRatio * (Paras.RGBad * a1 + Paras.RQ * Paras.VcFInstall * LineParas.TemporaryTension * (float)Math.Tan(Paras.TempStayWireAngle * PI / 180));
                str =  Paras.LoadRatio + " x (" + Paras.RGBad + " x " + a1 + " + " + Paras.RQ + " x " + Paras.VcFInstall + " x " + LineParas.TemporaryTension + " x tan(" + Paras.TempStayWireAngle  +")) = " + res.ToString("0.00");
                return res;
            }
            else
            {
                float res = Paras.LoadRatio * (Paras.RQ * Paras.VcFInstall * a1 + Paras.RQ * Paras.VcFInstall * LineParas.TemporaryTension * (float)Math.Tan(Paras.TempStayWireAngle * PI / 180));
                str = Paras.LoadRatio + " x (" + Paras.RQ * Paras.VcFInstall + " x " + a1 + " + " + Paras.RQ * Paras.VcFInstall + " x " + LineParas.TemporaryTension + " x tan(" + Paras.TempStayWireAngle + ")) = " + res.ToString("0.00");
                return res;
            }
        }

        /// <summary>
        /// 已架
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <param name="a4"></param>
        /// <returns></returns>
        public float MQ3X(out string str, float a1, float a2, float a3, float a4, float BL3) 
        {
            float res =Paras.LoadRatio  * Paras.RQ * Paras.VcFInstall* (Paras.OtherWindAdjustFactor * AngX(a1) * a3 + a4* (float)Math.Sin(a2* PI / 180)) * BL3;
            str =  Paras.LoadRatio + " x " + Paras.RQ + " x " + Paras.VcFInstall + " x (" + Paras.OtherWindAdjustFactor + " x " + AngX(a1) + " x " + a3 + " + " + a4 + " x sin(" + a2 + ")) x " + BL3 + " = " + res.ToString("0.00");
            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <param name="a4"></param>
        /// <returns></returns>
        public float MQ3Y(out string str, float a1, float a2, float a3, float a4)
        {
            float res = Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * (Paras.OtherWindAdjustFactor * AngY(a1) * a3 + a4 * (float)Math.Cos(a2 * PI / 180));
            str = Paras.LoadRatio + " x " + Paras.RQ + " x " + Paras.VcFInstall + " x (" + Paras.OtherWindAdjustFactor + " x " + AngY(a1) + " x " + a3 + " + " + a4 + " x cos(" + a2 + ")) = " + res.ToString("0.00");
            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a1"></param>
        /// <returns></returns>
        public float MQ3Z(out string str, float a1)
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
                str =  Paras.LoadRatio + " x " + Paras.RQ + " x " + Paras.VcFInstall + " x " + a1+ " = " + res.ToString("0.00");
                return res;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a1"></param>
        /// <returns></returns>
        public float MQ5Y(out string str, float a1)
        {
            float res = Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * a1 * 0.5f;
            str = Paras.LoadRatio + " x " + Paras.RQ + " x " + Paras.VcFInstall + " x " + a1 + " x " + 0.5f + " = " + res.ToString("0.00");
            return res;
        }



        /// <summary>
        /// 前侧未挂
        /// </summary>
        /// <param name="a1"></param>
        /// <returns></returns>
        public float MQ4X(out string str, float a1, float BL3)
        {
            //临时拉线打到已锚侧，前侧不再有临时拉线作用
            str = "0.00";
            return 0.00f;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a1"></param>
        /// <returns></returns>
        public float MQ4Y(out string str, float a1)
        {
            str = "0.00";
            return 0.00f;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public float MQ4Z(out string str)
        {
            str = "0.00";
            return 0.00f;
        }

        /// <summary>
        /// 包括正紧和已紧，只是动力系数取值不同
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <param name="a4"></param>
        /// <param name="a5"></param>
        /// <param name="a6"></param>
        /// <returns></returns>
        public float JX(out string str, float a1, float a2, float a3, float a4, float a5, float a6, float BL3)
        {
            float res = Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * (Paras.OtherWindAdjustFactor * AngX(a1) * a2 + a3 * a4 * a5 * (1 - (float)Math.Cos(Paras.TractionAgnle * PI / 180)) * (float)Math.Sin(a6 * PI / 180)) * BL3;
            str = Paras.LoadRatio + " x " + Paras.RQ * Paras.VcFInstall + " x (" + Paras.OtherWindAdjustFactor + " x " + AngX(a1) + " x " + a2 + " + " + a3 + " x " + a4 + " x " + a5 + " x (" + 1 + " -  cos(" + Paras.TractionAgnle + ")) x sin(" + a6 + ")) x " + BL3 + " = " + res.ToString("0.00"); 
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
        public float JY(out string str, float a1, float a2, float a3, float a4, float a5, float a6)
        {
            float res=  Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * (Paras.OtherWindAdjustFactor * AngY(a1) * a2 + a3 * a4 * a5 * (1 - (float)Math.Cos(Paras.TractionAgnle * PI / 180)) * (float)Math.Cos(a6 * PI / 180));
            str = Paras.LoadRatio + " x " + Paras.RQ + " x " + Paras.VcFInstall + " x (" + Paras.OtherWindAdjustFactor + " x " + AngY(a1) + " x " + a2 + " + " + a3 + " x " + a4 + " x " + a5 + " x (" + 1 + " - cos(" + Paras.TractionAgnle + ")) x cos(" + a6 + ")) = " + res.ToString("0.00");
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
        public float JZ(out string str, float a1, float a2, float a3, float a4, float a51, float a52)
        {
            if (a1 >= 0)
            {
                float res = Paras.LoadRatio * (Paras.RGBad * a1 + Paras.RQ * Paras.VcFInstall * (a2 * a3 * a4 * (float)Math.Sin(Paras.TractionAgnle * PI / 180) + a51 * a52));
                str = Paras.LoadRatio + " x (" + Paras.RGBad + " x " + a1 + " + " + Paras.RQ + " x " + Paras.VcFInstall + " x (" + a2 + " x " + a3 + " x " + a4 + " x sin(" + Paras.TractionAgnle + ") + " + a51 + " x " + a52 + ")) = " + res.ToString("0.00");
                return res;
            }
            else
            {
                float res =  Paras.LoadRatio * (Paras.RQ * Paras.VcFInstall * a1 + Paras.RQ * Paras.VcFInstall * (a2 * a3 * a4 * (float)Math.Sin(Paras.TractionAgnle * PI / 180) + a51 * a52));
                str = Paras.LoadRatio + " x (" + Paras.RQ + " x " + Paras.VcFInstall + " x " + a1 + " + " + Paras.RQ + " x " + Paras.VcFInstall + " x (" + a2 + " x " + a3 + " x " + a4 + " x sin(" + Paras.TractionAgnle + ") + " + a51 + " x "+ a52 + ")) = " + res.ToString("0.00");
                return res;
            }
        }
    }
}
