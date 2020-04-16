using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerLoadCals.Mode;

namespace TowerLoadCals.BLL
{
    public class FormulaLineTower: FormulaTower
    {
        public FormulaLineTower(FormulaParas paras) :base(paras)
        {
        }

        /// <summary>
        /// 大风X
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <returns></returns>
        public float ZXNX(float a1, float a2, float a3)
        {
            return Paras.LoadRatio * Paras.RQ * a3 * Beta1(0) * AngX(a1) * a2;
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
        public float ZXNY(float a1, float a2, float a3, float a4, float a5)
        {
            return Paras.LoadRatio * Paras.RQ * a5 * (Beta1(0) * AngY(a1) * a2 + (a3 - a4));
        }

        /// <summary>
        /// 大风Z
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <returns></returns>
        public float ZXNZ(float a1, float a2, float a3)
        {
            if(a1 >= 0)
                //a2 即rg最大最小垂荷合并
                return Paras.LoadRatio * a2 * a1;
            else
                return Paras.LoadRatio * Paras.RQ * a1 * a3;
        }

        /// <summary>
        /// 覆冰X
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <param name="a4"></param>
        /// <returns></returns>
        public float ZXIYX(float a1, float a2, float a3, float a4)
        {
            //a4为分项系数
            return Paras.LoadRatio * a4 * a3 * Beta2(0) * AngX(a1) * a2; 
        }

        /// <summary>
        /// 覆冰Y
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <param name="a4"></param>
        /// <param name="a5"></param>
        /// <param name="a6"></param>
        /// <returns></returns>
        public float ZXIYY(float a1, float a2, float a3, float a4, float a5, float a6)
        {
            return Paras.LoadRatio * a6 * a5 * (Beta2(0) * AngY(a1) * a2 + (a3 - a4));
        }

        /// <summary>
        /// 覆冰Z
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <param name="a4"></param>
        /// <param name="a5"></param>
        /// <returns></returns>
        public float ZXIYZ(float a1, float a2, float a3, float a4, float a5)
        {
            if(a1 >= 0)
                return Paras.LoadRatio * (a3 * a1 + a4 * a5 * (a2 - a1));
            else
                //a3 即rg最大最小垂荷合并
                return Paras.LoadRatio * (a4 * a5 * a1 + a4 * a5 * (a2 - a1));
        }

        /// <summary>
        /// 已吊情况
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <returns></returns>
        public float ZXLX(float a1, float a2)
        {
            return Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * Beta2(0) * AngX(a1) * a2;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <param name="a4"></param>
        /// <returns></returns>
        public float ZXLY(float a1, float a2, float a3, float a4)
        {
            return Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * (Beta2(0) * AngY(a1) * a2 + (a3 - a4));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a1"></param>
        /// <returns></returns>
        public float ZXLZ1(float a1)
        {
            if(a1 >= 0)
                return Paras.LoadRatio * Paras.RGBad * a1;
            else
                return Paras.LoadRatio * Paras.RQ * a1 * Paras.VcFInstall;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <returns></returns>
        public float ZXLZ2(float a1, float a2, float a3)
        {
            //a2 a3 为吊装系数和附加荷载，导地线不同
            return Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * (a1 * DX(0) * a2 + a3);
        }

        /// <summary>
        /// 已锚
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <returns></returns>
        public float ZXMX2(float a1, float a2)
        {
            return Paras.LoadRatio * CofW(0) * Paras.RQ * Paras.VcFInstall * Beta2(0) * AngX(a1) * a2;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <param name="a4"></param>
        /// <returns></returns>
        public float ZXMY2(float a1, float a2, float a3, float a4)
        {
            return Paras.LoadRatio * CofW(0) * Paras.RQ * Paras.VcFInstall * Beta2(0) * AngY(a1) * a2 + Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * a3 * (1 - (float)Math.Cos(a4 * PI / 180));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <param name="a4"></param>
        /// <returns></returns>
        public float ZXMZ2(float a1, float a2, float a3, float a4)
        {
            return Paras.LoadRatio * Paras.RGBad * CofG(0) * a1 + Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * (a3 * (float)Math.Sin(a4 * PI /180) + a2);
        }

        /// <summary>
        /// 过滑车
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <returns></returns>
        public float GLX(float a1, float a2)
        {
            return Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * Beta2(0) * AngX(a1) * a2;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <returns></returns>
        public float GLY(float a1, float a2, float a3)
        {
            return Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * (Beta2(0) * AngY(a1) * a2 + DX(0) * a3);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <returns></returns>
        public float GLZ(float a1, float a2, float a3)
        {
            if (a1 >= 0)
                return Paras.LoadRatio * Paras.RGBad * a1 + Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * ((a2 - a1) + a3);
            else
                return Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * (a2 - 0) + Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * a3;
        }

        /// <summary>
        /// 施工提线
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <returns></returns>
        public float ZXCX(float a1, float a2)
        {

            return Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * Beta2(0) * AngX(a1) * a2;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <param name="a4"></param>
        /// <returns></returns>
        public float ZXCY(float a1, float a2, float a3, float a4)
        {
            return Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * Beta2(0) * AngY(a1) * a2 + Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * DX(0) * a3 * a4 * (float)Math.Sin(0);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <param name="a4"></param>
        /// <returns></returns>
        public float ZXCZ(float a2, float a3, float a4)
        {
            return Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * (DX(0) * a3 * a4 * (float)Math.Sin(90 * PI / 180) + a2);
        }

        /// <summary>
        /// 正锚
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <returns></returns>
        public float ZXMX1(float a1, float a2) {
            return  Paras.LoadRatio * CofW(0) * Paras.RQ * Paras.VcFInstall * Beta2(0) * AngX(a1) * a2;
        }

        /// <summary>
        /// 正锚
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <param name="a4"></param>
        /// <returns></returns>
        public float ZXMY1(float a1, float a2, float a3, float a4)
        {
            return Paras.LoadRatio * CofW(0) * Paras.RQ * Paras.VcFInstall * Beta2(0) * AngY(a1) * a2 + Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * DX(0) * a3 * (1 - (float)Math.Cos(a4 * PI / 180));
        }

        /// <summary>
        /// 正锚
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <param name="a4"></param>
        /// <returns></returns>
        public float ZXMZ1(float a1, float a2, float a3, float a4)
        {
            return Paras.LoadRatio * Paras.RGBad * CofG(0) * a1 + Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * (DX(0) * a3 * (float)Math.Sin(a4 * PI / 180) + a2);
        }

        /// <summary>
        /// OPGW开段计算 左地线开段
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <returns></returns>
        public float MO1X(float a1, float a2)
        {
            return Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * Beta2(0) * AngX(a1) * a2;
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
        public float MO1Y(float a1, float a2, float a3, float a4, float a5)
        {
            return Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * Beta2(0) * AngY(a1) * a2 + Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * (a3 - a4 * DX(0) * a5 * (1 - (float)Math.Cos(MaoJiao(0) * PI / 180)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a3"></param>
        /// <param name="a4"></param>
        /// <param name="a5"></param>
        /// <returns></returns>
        public float MO1Z(float a1, float a3, float a4, float a5)
        {
            if (a1 >= 0)
                return Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * (a3 * DX(0) * a4 * (float)Math.Sin(MaoJiao(0) * PI / 180) + a5) + Paras.LoadRatio * Paras.RGBad * a1;
            else
                return Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * (a3 * DX(0) * a4 * (float)Math.Sin(MaoJiao(0) * PI / 180) + a5) + Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * a1;
        }

        /// <summary>
        /// 右地线已架
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <returns></returns>
        public float MO2X(float a1, float a2)
        {
            return Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * Beta2(0) * AngX(a1) * a2;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="a3"></param>
        /// <param name="a4"></param>
        /// <returns></returns>
        public float MO2Y(float a1, float a2, float a3, float a4)
        {
            return Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * (Beta2(0) * AngY(a1) * a2 + (a3 - a4) * 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a1"></param>
        /// <returns></returns>
        public float MO2Z(float a1)
        {
            if (a1 >= 0)
                return Paras.LoadRatio * Paras.RGBad * a1;
            else
                return Paras.LoadRatio * Paras.RQ * Paras.VcFInstall * a1;
        }

        /// <summary>
        /// 脱冰跳跃相
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <returns></returns>
        public float ZXTX(float a1, float a2)
        {
            return Paras.LoadRatio * Paras.RA * Paras.VcFCheck * Beta2(0) * AngX(a1) * a2;
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
        public float ZXTY(float a1, float a2, float a3, float a4, float a5)
        {
            return Paras.LoadRatio * Paras.RA * Paras.VcFCheck * (Beta2(0) * AngY(a1) * a2 + (a3 - a4) * a5);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <returns></returns>
        public float ZXTZ1(float a1, float a2)
        {
            return Paras.LoadRatio * Paras.RA * Paras.VcFCheck * a2;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <returns></returns>
        public float ZXTZ2(float a1, float a2)
        {
            if (a1 >= 0)
                return Paras.LoadRatio * Paras.RGCheck01 * a1 + Paras.LoadRatio * Paras.RA * Paras.VcFCheck * (a2 - a1);
            else
                return Paras.LoadRatio * Paras.RA * Paras.VcFCheck * a2;
        }
    }
}
