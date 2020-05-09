using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerLoadCals.Mode;
using TowerLoadCals.Mode.Structure;

namespace TowerLoadCals.BLL
{
    class FormulaLineCornerTower : FormulaTower
    {
        public FormulaLineCornerTower(FormulaParas paras, StruLineParas lineParas) : base(paras, lineParas)
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
        public float ZZNX(float a1 , float a2 , float a3, float a4, float a5, float a6)
        {
            return Paras.LoadRatio * Paras.RQ * a6 * (Paras.WindAdjustFactor * AngX(a1) * a2 + (a3 + a4) * (float)Math.Sin(a5 * PI / 360));
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
        public float ZZNY(float a1 , float a2 , float a3, float a4, float a5, float a6)
        {
            return Paras.LoadRatio * Paras.RQ * a6 * (Paras.WindAdjustFactor * AngY(a1) * a2 + (a3 - a4) * (float)Math.Cos(a5 * PI / 360));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <returns></returns>
        public float ZZNZ(float a1 , float a2 , float a3)
        {
            if (a1 >= 0)
                return Paras.LoadRatio * a2 * a1;
            else
                return Paras.LoadRatio * Paras.RQ * a1 * a3;
        
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
        public float ZZIYX(float a1 , float a2 , float a3, float a4, float a5, float a6, float a7)
        {
            return Paras.LoadRatio * a7 * a6 * (Paras.OtherWindAdjustFactor * AngX(a1) * a2 + (a3 + a4) * (float)Math.Sin(a5 * PI / 360));
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
        public float ZZIYY(float a1 , float a2 , float a3, float a4, float a5, float a6, float a7)
        {
            return Paras.LoadRatio * a7 * a6 * (Paras.OtherWindAdjustFactor * AngY(a1) * a2 + (a3 - a4) * (float)Math.Cos(a5 * PI / 360));
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
        public float ZZIYZ(float a1 , float a2 , float a3, float a6, float a7 )
        {
            if (a1 >= 0)
                return Paras.LoadRatio * a3 * a1 + Paras.LoadRatio * a7 * a6 * (a2 - a1);
            else
                return Paras.LoadRatio * a7 * a6 * a2;
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
        public float ZZTX(float a1 , float a2 , float a3, float a4, float a5)
        {
            return Paras.LoadRatio * Paras.RA * Paras.VcFCheck * (Paras.OtherWindAdjustFactor * AngX(a1) * a2 + (a3 + a4) * (float)Math.Sin(a5 * PI / 360));
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
        public float ZZTY(float a1 , float a2 , float a3, float a4, float a5)
        {
            return Paras.LoadRatio * Paras.RA * Paras.VcFCheck * (Paras.OtherWindAdjustFactor * AngY(a1) * a2 + (a3 - a4) * (float)Math.Cos(a5 * PI / 360));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <returns></returns>
        public float ZZTZ1(float a1 , float a2 )
        {
            return Paras.LoadRatio * Paras.RA * Paras.VcFCheck * a2;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <returns></returns>
        public float ZZTZ2(float a1 , float a2 )
        {
            if (a1 >= 0)
                return Paras.LoadRatio * Paras.RGCheck01 * a1 + Paras.LoadRatio * Paras.RA * Paras.VcFCheck * (a2 - a1);
            else
                return Paras.LoadRatio * Paras.RA * Paras.VcFCheck * a2;
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
        public float ZZLX(float a1 , float a2 , float a3, float a4, float a5)
        {
            return Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * (Paras.OtherWindAdjustFactor * AngX(a1) * a2 + (a3 + a4) * (float)Math.Sin(a5 * PI / 360));
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
        public float ZZLY(float a1 , float a2 , float a3, float a4, float a5)
        {
            return Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * (Paras.OtherWindAdjustFactor * AngY(a1) * a2 + (a3 - a4) * (float)Math.Cos(a5 * PI / 360));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a1"></param>
        /// <returns></returns>
        public float ZZLZ1(float a1 )
        {
            if (a1 >= 0)
                return Paras.LoadRatio * Paras.RGBad * a1;
            else
                return Paras.LoadRatio * Paras.VcFInstall * Paras.RQ * a1;
        }

        /// <summary>
        /// 正吊
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <returns></returns>
        public float ZZLZ2(float a1 , float a2 , float a3)
        {
            return Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * (a1 * Paras.DynamicCoef * a2 + a3);
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
        public float ZZGX(float a1 , float a2 , float a3, float a4, float a5, float a6)
        {
            return Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * (Paras.OtherWindAdjustFactor * AngX(a1) * a2 + (a3 + Paras.DynamicCoef * a6 + a4) * (float)Math.Sin(a5 * PI / 360));
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
        public float ZZGY(float a1 , float a2 , float a3, float a4, float a5, float a6)
        {
            return Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * (Paras.OtherWindAdjustFactor * AngY(a1) * a2 + (a3 + Paras.DynamicCoef * a6 - a4) * (float)Math.Cos(a5 * PI / 360));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <returns></returns>
        public float ZZGZ(float a1 , float a2 , float a3)
        {
            if (a1 >= 0)
                return Paras.LoadRatio * Paras.RGBad * a1 + Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * ((a2 - a1) + a3);
            else
                return Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * a2 + Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * a3;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <returns></returns>
        public float ZZCZ(float a1 , float a2 , float a3)
        {
            return Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * (Paras.DynamicCoef * a1 * a2 + a3);
        }
    }
}
