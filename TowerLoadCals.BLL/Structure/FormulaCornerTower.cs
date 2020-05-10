using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerLoadCals.Mode;
using TowerLoadCals.Mode.Structure;

namespace TowerLoadCals.BLL
{
    class FormulaCornerTower : FormulaTower
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
        public float ZNX(float a1, float a2, float a3, float a4, float a5)
        {
            return Paras.LoadRatio * Paras.RQ * a5 * (Paras.WindAdjustFactor * AngX(a1) * a3 + a4 * (float)Math.Sin(a2 * PI / 180));
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
        public float ZNY(float a1, float a2, float a3, float a4, float a5)
        {
            return Paras.LoadRatio * Paras.RQ * a5 * (Paras.WindAdjustFactor * AngY(a1) * a3 + a4 * (float)Math.Cos(a2 * PI / 180));
        }

        /// <summary>
        /// 大风Z
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a5"></param>
        /// <returns></returns>
        public float ZNZ(float a1, float a5)
        {
            if (a1 >= 0)
                return Paras.LoadRatio * Paras.RGBad * a1;
            else
                return Paras.LoadRatio * a5 * Paras.RQ * a1;
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
        public float ZIBUYX(float a1, float a2, float a3, float a4, float a5, float a6)
        {
            return Paras.LoadRatio * a6 * a5 * (Paras.OtherWindAdjustFactor * AngX(a1) * a3 + a4 * (float)Math.Sin(a2 * PI / 180));
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
        public float ZIBUYY(float a1, float a2, float a3, float a4, float a5, float a6)
        {
            return Paras.LoadRatio * a6 * a5 * (Paras.OtherWindAdjustFactor * AngY(a1) * a3 + a4 * (float)Math.Cos(a2 * PI / 180));
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
        public float ZIBUYZ(float a1, float a2, float a5, float a6, float a7)
        {
            if (a1 >= 0)
                return Paras.LoadRatio * (a7 * a1 + a6 * a5 * (a2 - a1));
            else
                return Paras.LoadRatio * a5 * a6 * a2;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <param name="a4"></param>
        /// <returns></returns>
        public float ZLX(float a1, float a2, float a3, float a4)
        {
            return Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * (Paras.OtherWindAdjustFactor * AngX(a1) * a3 + a4 * (float)Math.Sin(a2 * PI / 180));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <param name="a4"></param>
        /// <returns></returns>
        public float ZLY(float a1, float a2, float a3, float a4)
        {
            return Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * (Paras.OtherWindAdjustFactor * AngY(a1) * a3 + a4 * (float)Math.Cos(a2 * PI / 180));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a1"></param>
        /// <returns></returns>
        public float ZLZ(float a1)
        {
            if (a1 >= 0)
                return Paras.LoadRatio * Paras.RGBad * a1;
            else
                return Paras.LoadRatio * Paras.VcFInstall * Paras.RQ * a1;
        }

        /// <summary>
        /// 正起吊
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <returns></returns>
        public float ZLT1Z(float a1, float a2, float a3)
        {
            return Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * (a3 + Paras.LiftCoefJumper * a1 * a2 * Paras.DynamicCoef);
        }

        /// <summary>
        /// 已起吊
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <returns></returns>
        public float ZLT2Z(float a1, float a2, float a3)
        {
            //a2.a3为安装附加荷载，已吊装时无，与直线塔统一
            //20160904 修改已吊表达式
            if (a1 >= 0)
                return Paras.LoadRatio * Paras.RGBad * a1 * a2;
            else
                return Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * a1 * a2;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <param name="a4"></param>
        /// <returns></returns>
        public float ZM1X(float a1, float a2, float a3, float a4) 
        {
            return Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * (Paras.OtherWindAdjustFactor * AngX(a1) * a3 + (a4 * Paras.DynamicCoef * LineParas.DrawingCoef - Paras.llz) * (float)Math.Sin(a2 * PI / 180));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <param name="a4"></param>
        /// <returns></returns>
        public float ZM1Y(float a1, float a2, float a3, float a4) 
        {
            return Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * (Paras.OtherWindAdjustFactor * AngY(a1) * a3 + (a4 * Paras.DynamicCoef * LineParas.DrawingCoef - Paras.llz) * (float)Math.Cos(a2 * PI / 180));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <returns></returns>
        public float ZM1Z(float a1, float a2)
        {
            if (a1 >= 0)
                return Paras.LoadRatio * (Paras.RGBad * a1 + Paras.RQ * Paras.VcFInstall * (Paras.llz * (float)Math.Tan(Paras.TempStayWireAngle * PI / 180) + a2));
            else
                return Paras.LoadRatio * (Paras.RQ * Paras.VcFInstall * a1 + Paras.RQ * Paras.VcFInstall * (Paras.llz * (float)Math.Tan(Paras.TempStayWireAngle * PI / 180) + a2));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <param name="a4"></param>
        /// <returns></returns>
        public float ZM2X(float a1, float a2, float a3, float a4)  
        {
            return Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * (Paras.OtherWindAdjustFactor * AngX(a1) * a3 + (a4 - Paras.llz) * (float)Math.Sin(a2 * PI / 180));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <param name="a4"></param>
        /// <returns></returns>
        public float ZM2Y(float a1, float a2, float a3, float a4) 
        {
            return Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * (Paras.OtherWindAdjustFactor * AngY(a1) * a3 + (a4 - Paras.llz) * (float)Math.Cos(a2 * PI / 180));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a1"></param>
        /// <returns></returns>
        public float ZM2Z(float a1) 
        {
            if (a1 >= 0)
                return Paras.LoadRatio * (Paras.RGBad * a1 + Paras.RQ * Paras.VcFInstall * Paras.llz * (float)Math.Tan(Paras.TempStayWireAngle * PI / 180));
            else
                return Paras.LoadRatio * (Paras.RQ * Paras.VcFInstall * a1 + Paras.RQ * Paras.VcFInstall * Paras.llz * (float)Math.Tan(Paras.TempStayWireAngle * PI / 180));
        }

        /// <summary>
        /// 正牵
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <param name="a4"></param>
        /// <returns></returns>
        public float MQ1X(float a1, float a2, float a3, float a4)
        {
            return Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * (Paras.OtherWindAdjustFactor * AngX(a1) * a3 + a4 * Paras.DynamicCoef * LineParas.DrawingCoef * (1 - (float)Math.Cos(Paras.TractionAgnle * PI / 180)) * (float)Math.Sin(a2 * PI / 180));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <param name="a4"></param>
        /// <returns></returns>
        public float MQ1Y(float a1, float a2, float a3, float a4)
        {
            return Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * (Paras.OtherWindAdjustFactor * AngY(a1) * a3 + a4 * Paras.DynamicCoef * LineParas.DrawingCoef * (1 - (float)Math.Cos(Paras.TractionAgnle * PI / 180)) * (float)Math.Cos(a2 * PI / 180));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <returns></returns>
        public float MQ1Z(float a1, float a2, float a3)
        {
            if (a1 >= 0)
                return Paras.LoadRatio * (Paras.RGBad * a1 + Paras.RQ * Paras.VcFInstall * (a2 * Paras.DynamicCoef * LineParas.DrawingCoef * (float)Math.Sin(Paras.TractionAgnle * PI / 180) + a3));
            else
                return Paras.LoadRatio * (Paras.RQ * Paras.VcFInstall * a1 + Paras.RQ * Paras.VcFInstall * (a2 * Paras.DynamicCoef * LineParas.DrawingCoef * (float)Math.Sin(Paras.TractionAgnle * PI / 180) + a3));
        }

        /// <summary>
        /// 已锚
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <param name="a4"></param>
        /// <returns></returns>
        public float MQ2X(float a1, float a2, float a3, float a4) 
        {
            return Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * (Paras.OtherWindAdjustFactor * AngX(a1) * a3 + (a4 - Paras.llz) * (float)Math.Sin(a2 * PI / 180));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <param name="a4"></param>
        /// <returns></returns>
        public float MQ2Y(float a1, float a2, float a3, float a4) 
        {
            return Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * (Paras.OtherWindAdjustFactor * AngY(a1) * a3 - (a4 - Paras.llz) * (float)Math.Cos(a2 * PI / 180));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a1"></param>
        /// <returns></returns>
        public float MQ2Z(float a1) 
        {
            if (a1 >= 0)
                return Paras.LoadRatio * (Paras.RGBad * a1 + Paras.RQ * Paras.VcFInstall * Paras.llz * (float)Math.Tan(Paras.TempStayWireAngle * PI / 180));
            else
                return Paras.LoadRatio * (Paras.RQ * Paras.VcFInstall * a1 + Paras.RQ * Paras.VcFInstall * Paras.llz * (float)Math.Tan(Paras.TempStayWireAngle * PI / 180));
        }

        /// <summary>
        /// 已架
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <param name="a4"></param>
        /// <returns></returns>
        public float MQ3X(float a1, float a2, float a3, float a4) 
        {
            return Paras.LoadRatio  * Paras.RQ * Paras.VcFInstall* (Paras.OtherWindAdjustFactor * AngX(a1) * a3 + a4* (float)Math.Sin(a2* PI / 180));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <param name="a4"></param>
        /// <returns></returns>
        public float MQ3Y(float a1, float a2, float a3, float a4)
        {
            return Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * (Paras.OtherWindAdjustFactor * AngY(a1) * a3 + a4 * (float)Math.Cos(a2 * PI / 180));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a1"></param>
        /// <returns></returns>
        public float MQ5Y(float a1)
        {
            return Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * a1 * 0.5f;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a1"></param>
        /// <returns></returns>
        public float MQ3Z(float a1)
        {
            if (a1 >= 0)
                return Paras.LoadRatio * Paras.RGBad * a1;
            else
                return Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * a1;
        }

        /// <summary>
        /// 前侧未挂
        /// </summary>
        /// <param name="a1"></param>
        /// <returns></returns>
        public float MQ4X(float a1)
        {
            //临时拉线打到已锚侧，前侧不再有临时拉线作用
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a1"></param>
        /// <returns></returns>
        public float MQ4Y(float a1)
        {
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public float MQ4Z()
        {
            return 0;
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
        public float JX(float a1, float a2, float a3, float a4, float a5, float a6)
        {
            return Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * (Paras.OtherWindAdjustFactor * AngX(a1) * a2 + a3 * a4 * a5 * (1 - (float)Math.Cos(Paras.TractionAgnle * PI / 180)) * (float)Math.Sin(a6 * PI / 180));
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
        public float JY(float a1, float a2, float a3, float a4, float a5, float a6)
        {
            return Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * (Paras.OtherWindAdjustFactor * AngY(a1) * a2 + a3 * a4 * a5 * (1 - (float)Math.Cos(Paras.TractionAgnle * PI / 180)) * (float)Math.Cos(a6 * PI / 180));
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
        public float JZ(float a1, float a2, float a3, float a4, float a5)
        {
            if (a1 >= 0)
                return Paras.LoadRatio * (Paras.RGBad * a1 + Paras.RQ * Paras.VcFInstall * (a2 * a3 * a4 * (float)Math.Sin(Paras.TractionAgnle * PI / 180) + a5));
            else
                return Paras.LoadRatio * (Paras.RQ * Paras.VcFInstall * a1 + Paras.RQ * Paras.VcFInstall * (a2 * a3 * a4 * (float)Math.Sin(Paras.TractionAgnle * PI / 180) + a5));
        }
    }
}
