using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.BLL.Electric
{
    public class ElectricalCalsToolBox
    {

        /// <summary>
        /// 换算高度
        /// 注意：风荷载调整系数再核实
        /// </summary>
        /// <param name="voltage"></param>
        /// <param name="ACorDC"></param>
        /// <returns></returns>
        public static int GetLevelOfLine(int voltage, int ACorDC)
        {
            //默认都为3类线路
            int LeValue = 3;   
            if(ACorDC == 1)
            {
                //直流线路都认为是1类线路
                LeValue = 1;
            } 
            else if(voltage >= 330)
            {
                //交流330kV以上认为是1类线路
                LeValue = 1;
            }
            else if(voltage >= 220)
            {
                //交流220kV以上认为是2类线路
                LeValue = 2;
            }

            return LeValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="voltage"></param>
        /// <returns></returns>
        public static float AveHeightDefault(int voltage)
        {
            float height = 0;

            if (voltage <= 220)
            {
                height = 15;
            }
            else if( voltage > 220 && voltage <= 500)
            {
                height = 20;
            }
            else if( voltage > 500 && voltage <= 1000)
            {
                height = 30;
            }
            else if (voltage >= 1100)
            {
                height = 40;
            }
            return height;
        }

        /// <summary>
        /// 电线体形系数μsc
        /// </summary>
        /// <param name="ice">工况覆冰厚度</param>
        /// <param name="d">导线直径</param>
        /// <returns></returns>
        public static float WindEpson(float ice, float d)
        {
            float Epson;

            //风载体形系数
            if (d >= 17 && ice <= 0)
            {
                Epson = 1.1f;
            }
            else
            {
                Epson = 1.1f;
            }
            return Epson;
        }


        /// 风压不均匀系数Alpha
        /// V为工况的风速,CalType=1为应力计算，其他为间隙校核
        /// </summary>
        public static float WindAlpha(float wind, float ice = 0, int CalType= 1)
        {
            //风速不均匀系数
            float Alpha = 0;

            if(CalType == 1)
            {
                if (wind < 20)
                {
                    Alpha = 1;
                }
                else if (wind >= 20 && wind < 27)
                {
                    Alpha = 0.85f;
                }
                else if (wind >= 27 && wind < 31.5)
                {
                    Alpha = 0.75f;
                }
                else
                {
                    Alpha = 0.7f;
                }
            }
            else
            {
                if (wind < 20)
                {
                    Alpha = 1;
                }
                else if( wind >= 20 && wind< 27)
                {
                    Alpha = 0.75f;
                }
                else if(wind >= 27 && wind< 31.5)
                {
                    Alpha = 0.61f;
                }
                else
                {
                    Alpha = 0.61f;
                }
            }

            //#覆冰情况下，直接取1
            if (ice > 0)
            {
                Alpha = 1;
            }
            return Alpha;
        }

        /// <summary>
        ///根据地形类别换算风速
        ///地形类别，B类系数1.6，其他类待定
        /// </summary>
        /// <param name="wind"></param>
        /// <param name="height"></param>
        /// <param name="TerType"></param>
        /// <returns></returns>
        public static float WindExChange(float wind, float height, char TerType)
        {
            //换算后风速
            if (TerType == 'A')
            {
                wind = (float)(wind * Math.Sqrt(1.379) * (Math.Pow((height / 10),0.12)));
            }
            else if (TerType == 'B')
            {
                wind = (float)(wind * Math.Sqrt(1) * (Math.Pow((height / 10), 0.16)));
            }
            else if (TerType == 'C')
            {
                wind = (float)(wind * Math.Sqrt(0.616) * (Math.Pow((height / 10), 0.22)));
            }
            else if (TerType== 'D')
            {
                wind = (float)(wind * Math.Sqrt(0.318) * (Math.Pow((height / 10),0.30)));
            }

            return (float)Math.Round(wind, 2);
        }

        /// <summary>
        /// 风荷载增大系数B
        /// </summary>
        /// <param name="ice"> ice覆冰厚度，不支持覆冰为</param>
        /// <returns></returns>
        public static float WindLoadEnlargeCoe(float ice)
        {
            //风荷载增大系数B
            float enlargeCoe;
            if (ice <= 5)
            {
                enlargeCoe = 1;
            }
            else if( ice >= 5 && ice<10)
            {
                enlargeCoe = 1.1f;
            }
            else if( ice >= 10 && ice<15)
            {
                enlargeCoe = 1.2f;
            }
            //覆冰增大系数的取值与传统计算不符，注意后期调整
            else if( ice >= 15 && ice<20)
            {
                enlargeCoe = 1.3f;
            }
            else if( ice >= 20 && ice<30)
            {
                enlargeCoe = 1.5f;
            }
            else if( ice >= 30 && ice<= 50)
            {
                enlargeCoe = 1.8f;
            }
            else
            {
                //大于20mm取中间值，规范要求为2.0-2.5
                enlargeCoe = 2;  
            }
            return enlargeCoe;
        }

        /// <summary>
        /// 求解方程，形式为x**2（x+a）=b,采用邵天晓方法计算
        /// </summary>
        /// <param name="Tj_A"></param>
        /// <param name="Tj_B"></param>
        /// <returns></returns>
        public static float caculateCurDelta(float Tj_A, float Tj_B)
        {
            var CalA = Math.Pow(Tj_B, 2) / 4 - Tj_B * Math.Pow(Tj_A, 3) / 27;
            var CalB = Tj_B / 2 - Math.Pow(Tj_A, 3) / 27;

            double Delta;

            if (CalA >= 0)
            {
                Delta = Math.Pow((CalB + Math.Sqrt(CalA)), (1 / 3)) + Math.Pow((CalB - Math.Sqrt(CalA)), (1 / 3)) - Tj_A / 3;
            }
            else
            {
                double Tho;
                if( CalB > 0)
                {
                    Tho = Math.Atan(Math.Sqrt(-CalA) / CalB);
                }
                else if( CalB== 0)
                {
                    Tho = Math.PI / 2;
                }
                else
                {
                    Tho = Math.PI + Math.Atan(Math.Sqrt(-CalA) / CalB);
                }
                Delta = Tj_A / 3 * (2 * Math.Cos(Tho / 3) - 1);
            }

            return (float)Delta;
        }

    }
}
