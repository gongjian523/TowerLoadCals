using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerLoadCals.Mode;

namespace TowerLoadCals.BLL
{
    public class FormulaTower
    {
        protected FormulaParas Paras { get; set; }

        protected float PI { get
            {
                return 3.14f;
            }
        }

        public FormulaTower(FormulaParas paras)
        {
            Paras = paras;
        }

        #region 直线塔
        protected float Beta1(float para)
        {
            return 0;
        }

        protected float Beta2(float para)
        {
            return 0;
        }


        protected float DX(float para)
        {
            return 0;
        }

        protected float CofW(float para)
        {
            return 0;
        }

        protected float CofG(float para)
        {
            return 0;
        }

        protected float MaoJiao(float para)
        {
            return 0;
        }

        /// <summary>
        /// 线条风荷载X向分配
        /// </summary>
        /// <param name="fenxiang"></param>
        /// <param name="Single"></param>
        /// <returns></returns>
        protected float AngX(float fenxiang)
        {
            if (fenxiang == 90)
                return 1;
            else if (fenxiang == 60)
                return 0.75f;
            else if (fenxiang == 45)
                return 0.5f;
            else if (fenxiang == 0)
                return 0;
            else if (fenxiang == -90)
                return -1;
            else if (fenxiang == -60)
                return -0.75f;
            else if (fenxiang == -45)
                return -0.5f;
            else
            {
                throw new ArgumentException("风向错误");
                //return 1000;
            }
        }

        /// <summary>
        /// 线条风荷载Y向分配
        /// </summary>
        /// <param name="fenxiang"></param>
        /// <param name="Single"></param>
        /// <returns></returns>
        protected float AngY(float fenxiang)
        {
            if (fenxiang == 90)
                return 0;
            else if (fenxiang == 60)
                return 0;
            else if (fenxiang == 45)
                return 0.15f;
            else if (fenxiang == 0)
                return 0;
            else if (fenxiang == -90)
                return 0;
            else if (fenxiang == -60)
                return -0;
            else if (fenxiang == -45)
                return -0.15f;
            else
            {
                throw new ArgumentException("风向错误");
                //return 1000;
            }
        }
        #endregion

        protected float gqx (float para)
        {
            return 0;
        }

    }

}
