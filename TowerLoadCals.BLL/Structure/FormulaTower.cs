using System;
using TowerLoadCals.Mode;

namespace TowerLoadCals.BLL
{
    public class FormulaTower
    {
        protected StruCalseBaseParas Paras { get; set; }

        protected StruLineParas LineParas { get; set; }

        protected float PI { get
            {
                return (float)Math.PI;
            }
        }

        public FormulaTower(StruCalseBaseParas paras)
        {
            Paras = paras;
        }

        //不能在构造函数中赋值，这个参数在不同线条上取值不一样
        public void SetStrLineParas(StruLineParas lineParas)
        {
            LineParas = lineParas;
        }
        
        #region 直线塔

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
                throw new Exception("风向错误");
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
                return 0.25f;
            else if (fenxiang == -90)
                return 0;
            else if (fenxiang == -60)
                return 0;
            else if (fenxiang == -45)
                return -0.15f;
            else
            {
                throw new Exception("风向错误");
                //return 1000;
            }
        }
        #endregion


        /// <summary>
        /// 转向挂点转向处X
        /// </summary>
        /// <param name="z"></param>
        /// <param name="angle"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public float TPTuringX(float z, float angle, out string str)
        {
            float res = (float)(-z * Math.Sin(angle * Math.PI / 180));
            str = "-" + z.ToString("0.00") + " x sin(" + angle + ") = " + res.ToString("0.00");
            return res;
        }

        /// <summary>
        /// 转向挂点转向处Y
        /// </summary>
        /// <param name="z"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public float TPTuringY(float y, out string str)
        {
            float res = y;
            str = res.ToString("0.00");
            return res;
        }

        /// <summary>
        /// 转向挂点转向处Z
        /// </summary>
        /// <param name="z"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public float TPTuringZ(float z, float angle, out string str)
        {
            float res = (float)(z + z * Math.Cos(angle * Math.PI / 180));
            str = z.ToString("0.00") + " + " + z.ToString("0.00") + " x cos(" + angle + ") = " + res.ToString("0.00");
            return res;
        }


        /// <summary>
        /// 转向挂点导线处X
        /// </summary>
        /// <param name="x"></param>
        /// <param name="z"></param>
        /// <param name="angle"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public float TPWireX(float x, float z, float angle, out string str)
        {
            float res = x + z * (float)Math.Sin(angle * Math.PI / 180);
            str = x.ToString("0.00") + " + " + z.ToString("0.00") + " x sin(" + angle + ") = " + res.ToString("0.00");
            return res;
        }

        /// <summary>
        /// 转向挂点导线处Y
        /// </summary>
        /// <param name="z"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public float TPWireY(float y, out string str)
        {
            float res = y;
            str = res.ToString("0.00");
            return res;
        }

        /// <summary>
        /// 转向挂点导线处Z
        /// </summary>
        /// <param name="z"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public float TPWireZ(float z, float angle, out string str)
        {
            float res = z - z * (float)Math.Cos(angle * Math.PI / 180);
            str = z.ToString("0.00") + " - " + z.ToString("0.00") + " x cos(" + angle + ") = " + res.ToString("0.00");
            return res;
        }

    }

}
