using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.Common.Utils
{
    /// <summary>
    /// 自定义的计算类
    /// </summary>
    public class Calc
    {

        /// <summary>
        /// 反双曲正弦
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double Asinh(double x)
        {
            return Math.Log(x + Math.Sqrt(x * x + 1));
        }
    }
}
