using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.BLL.Electric
{
    public class ElectricalCalsToolBox3
    {
        //以下为求其它工况孤立档的线长参数函数
        public static float kzhi2(float elas, float mianji, float cosB, float span, float dxLoad, float jyzLength1,
            float jyzLength2, float jyzLoad1, float jyzLoad2, float yxDist1, float yxDist2, float yxLoad1, float yxLoad2)
        {
            //依次输入电线弹性模量、截面积、余弦值、档距、电线单位荷载、绝缘子串1长度、绝缘子串2长度、绝缘子串1单位荷载、
            //'绝缘子串2单位荷载、第一引下线后侧距离、第二引下线后侧距离、第一引下线单位荷载、第二引下线单位荷载
            int panduan;

            float[] b = new float[5];
            float[] dl = new float[5];
            float[] Q = new float[5];
            float[] Q1 = new float[5];
            float[] P = new float[5];
            float[] qq = new float[5];


            float L1 = span - jyzLength1 - jyzLength2;
            float sum = 0, sum1;
            float result;

            if (yxDist1 == 0) {
                yxLoad1 = 0;
            }
            else if (yxLoad1 == 0) {
                yxDist1 = 0;
            }

            if (yxDist2 == 0) {
                yxLoad2 = 0;
            }
            else if (yxLoad2 == 0) {
                yxDist2 = 0;
            }

            if( yxDist1 == 0 && yxDist2 == 0 ){
                panduan = 1;
            }
            else if( yxDist1 != 0 && yxDist2 == 0 ){
                panduan = 2;
            }
            else if( yxDist1 == 0 && yxDist2 != 0 ){
                panduan = 3;
            }
            else {
                panduan = 4;
            }

            switch (panduan)
            {
                case 1:
                    P[0] = jyzLoad1; P[1] = dxLoad; P[2] = jyzLoad2;
                    dl[0] = jyzLength1; dl[1] = L1; dl[2] = jyzLength2;
                    Q[0] = 1 / b[0] * (dl[0] * P[0] * (b[0] - dl[0] / 2) + dl[1] * P[1] * (b[1] - dl[1] / 2) + dl[2] * P[2] * (b[2] - dl[2] / 2));
                    Q1[0] = Q[0] - dl[0] * P[0];
                    Q[1] = Q1[0];
                    Q1[1] = Q[1] - dl[1] * P[1];
                    Q[2] = Q1[1];
                    Q1[2] = Q[2] - dl[2] * P[2];
                    for (int i = 0; i <= 2; i++)
                    {
                        sum1 = (float)((Math.Pow(Q[i], 3) - Math.Pow(Q1[i], 3)) / P[i]); ;
                        sum = sum + sum1;
                    }
                    result = (float)(mianji * elas * Math.Pow(cosB, 5) / 6 / L1 * sum);
                    break;
                case 2:
                    b[0] = span; b[1] = span - jyzLength1; b[2] = span - yxDist1; b[3] = jyzLength2;
                    P[0] = jyzLoad1; P[1] = dxLoad; P[2] = dxLoad; P[3] = jyzLoad2;
                    dl[0] = jyzLength1; dl[1] = yxDist1 - jyzLength1; dl[2] = span - yxDist1 - jyzLength2; dl[3] = jyzLength2;
                    qq[1] = 0; qq[2] = yxLoad1 * P[1] * cosB; qq[3] = 0;
                    Q[0] = 1 / b[0] * (dl[0] * P[0] * (b[0] - dl[0] / 2) + dl[1] * P[1] * (b[1] - dl[1] / 2) + dl[2] * P[2] * (b[2] - dl[2] / 2)
                        + dl[3] * P[3] * (b[3] - dl[3] / 2) + qq[1] * b[1] + qq[2] * b[2] + qq[3] * b[3]);
                    Q1[0] = Q[0] - dl[0] * P[0];
                    Q[1] = Q1[0] - qq[1];
                    Q1[1] = Q[1] - dl[1] * P[1];
                    Q[2] = Q1[1] - qq[2];
                    Q1[2] = Q[2] - dl[2] * P[2];
                    Q[3] = Q1[2] - qq[3];
                    Q1[3] = Q[3] - dl[3] * P[3];
                    for (int i = 0; i <= 3; i++)
                    {
                        sum1 = (float)((Math.Pow(Q[i], 3) - Math.Pow(Q1[i], 3)) / P[i]);
                        sum = sum + sum1;
                    }
                    result = (float)(mianji * elas * Math.Pow(cosB, 5) / 6 / L1 * sum);
                    break;
                case 3:
                    b[0] = span; b[1] = span - jyzLength1; b[2] = span - yxDist2; b[3] = jyzLength2;
                    P[0] = jyzLoad1; P[1] = dxLoad; P[2] = dxLoad; P[3] = jyzLoad2;
                    dl[0] = jyzLength1; dl[1] = yxDist2 - jyzLength1; dl[2] = span - yxDist2 - jyzLength2; dl[3] = jyzLength2;
                    qq[1] = 0; qq[2] = yxLoad2 * P[1] * cosB; qq[3] = 0;
                    Q[0] = 1 / b[0] * (dl[0] * P[0] * (b[0] - dl[0] / 2) + dl[1] * P[1] * (b[1] - dl[1] / 2) + dl[2] * P[2] * (b[2] - dl[2] / 2) + dl[3] * P[3] * (b[3] - dl[3] / 2) + qq[1] * b[1] + qq[2] * b[2] + qq[3] * b[3]);
                    Q1[0] = Q[0] - dl[0] * P[0];
                    Q[1] = Q1[0] - qq[1];
                    Q1[1] = Q[1] - dl[1] * P[1];
                    Q[2] = Q1[1] - qq[2];
                    Q1[2] = Q[2] - dl[2] * P[2];
                    Q[3] = Q1[2] - qq[3];
                    Q1[3] = Q[3] - dl[3] * P[3];
                    for (int i = 0; i <= 3; i++)
                    {
                        sum1 = (float)((Math.Pow(Q[i], 3) - Math.Pow(Q1[i], 3)) / P[i]);
                        sum = sum + sum1;
                    }
                    result = (float)(mianji * elas * Math.Pow(cosB, 5) / 6 / L1 * sum);
                    break;
                case 4:
                    b[0] = span; b[1] = span - jyzLength1; b[2] = span - yxDist1; b[3] = span - yxDist2; b[4] = jyzLength2;
                    P[0] = jyzLoad1; P[1] = dxLoad; P[2] = dxLoad; P[3] = dxLoad; P[4] = jyzLoad2;
                    dl[0] = jyzLength1; dl[1] = yxDist1 - jyzLength1; dl[2] = yxDist2 - yxDist1;
                    dl[3] = span - yxDist2 - jyzLength2; dl[4] = jyzLength2;
                    qq[1] = 0; qq[2] = yxLoad1 * P[1] * cosB; qq[3] = yxLoad2 * P[2] * cosB; qq[4] = 0;
                    Q[0] = 1 / b[0] * (dl[0] * P[0] * (b[0] - dl[0] / 2) + dl[1] * P[1] * (b[1] - dl[1] / 2) + dl[2] * P[2] * (b[2] - dl[2] / 2)
                        + dl[3] * P[3] * (b[3] - dl[3] / 2) + dl[4] * P[4] * (b[4] - dl[4] / 2) + qq[1] * b[1] + qq[2] * b[2] + qq[3] * b[3] + qq[4] * b[4]);
                    Q1[0] = Q[0] - dl[0] * P[0];
                    Q[1] = Q1[0] - qq[1];
                    Q1[1] = Q[1] - dl[1] * P[1];
                    Q[2] = Q1[1] - qq[2];
                    Q1[2] = Q[2] - dl[2] * P[2];
                    Q[3] = Q1[2] - qq[3];
                    Q1[3] = Q[3] - dl[3] * P[3];
                    Q[4] = Q1[3] - qq[4];
                    Q1[4] = Q[4] - dl[4] * P[4];
                    for (int i = 0; i <= 4; i++)
                    {
                        sum1 = (float)((Math.Pow(Q[i], 3) - Math.Pow(Q1[i], 3)) / P[i]);
                        sum = sum + sum1;
                    }

                    result = (float)(mianji * elas * Math.Pow(cosB, 5) / 6 / L1 * sum);
                    break;
                default:
                    result = 0;
                    break;
            }
            return result;
        }

        //以下为求各工况孤立档的张力函数
        public static float zhangli(float elas, float expa, float cosB, float mianji, float span, float dl, float tenYZ, float KzhiYZ, float tempYZ, float KzhiJG,
             float tempJG, string gongkuang, float jyzLength1, float jyzLength2)
        {
            //依次输入电线弹性模量、线膨胀系数、余弦值、截面积、档距、过牵引长度、控制工况张力、控制工况K值、控制工况温度、
            //待求工况K值、待求工况温度、控制工况、绝缘子串1长度、绝缘子串2长度
            float a, b, c, d, e;
            float zhangli;

            if (gongkuang != "过牵引") {
                a = KzhiYZ / (tenYZ * tenYZ) - tenYZ + expa * elas * mianji * cosB * (tempJG - tempYZ);
                b = KzhiJG;
            }
            else{
                a = KzhiYZ / (tenYZ * tenYZ) - tenYZ + expa * elas * mianji * cosB * (tempJG - tempYZ) + elas * mianji * dl * cosB / (span - jyzLength1 - jyzLength2);
                b = KzhiJG;
            }

            if (a > 0) {
                c = 1;
            }
            else if (a == 0){
                c = 0;
            }
            else{
                c = -1;
            }
            d = (float)(13.5 * b / Math.Pow(Math.Abs(a), 3) - c);

            //switch(d)
            //    case Is >= 1
            //        e = Log(d + Sqr(d* d - 1))
            //        zhangli = Abs(a) * (Exp(e / 3) + Exp(-e / 3) - c) / 3
            //    case Is <= 1
            //        e = Atn(-d / Sqr(1 - d* d)) + 2 * Atn[1]
            //        zhangli = Abs(a) * (2 * Cos(e / 3) - c) / 3
            //    case 0
            //        zhangli = b ^ (1 / 3)

            if(d >= 1){
                e = (float)Math.Log(d + Math.Sqrt(d * d - 1));
                zhangli = (float)(Math.Abs(a) * (Math.Exp(e / 3) + Math.Exp(-e / 3) - c) / 3);
            }
            else if(d <=1 ){
                e = (float)(Math.Atan(-d / Math.Sqrt(1 - d * d)) + 2 * Math.Atan(1));
                zhangli = (float)(Math.Abs(a) * (2 * Math.Cos(e / 3) - c) / 3);
            }
            else{
                zhangli = (float)Math.Pow(b, (1 / 3));
            }
            return zhangli;
        }
        
        public static float zhangli1(float a, float b)
        {
            float c, d, e;
            float zhangli1;

            if (a > 0){
                c = 1;
            }
            else if (a == 0){
                c = 0;
            }
            else{
                c = -1;
            }

            d = (float)(13.5 * b / Math.Pow(Math.Abs(a), 3) - c);

            if (d >= 1)
            {
                e = (float)Math.Log(d + Math.Sqrt(d * d - 1));
                zhangli1 = (float)(Math.Abs(a) * (Math.Exp(e / 3) + Math.Exp(-e / 3) - c) / 3);
            }
            else if (d <= 1)
            {
                e = (float)(Math.Atan(-d / Math.Sqrt(1 - d * d)) + 2 * Math.Atan(1));
                zhangli1 = (float)(Math.Abs(a) * (2 * Math.Cos(e / 3) - c) / 3);
            }
            else
            {
                zhangli1 = (float)Math.Pow(b, (1 / 3));
            }
            return zhangli1;

        }

    }

}
