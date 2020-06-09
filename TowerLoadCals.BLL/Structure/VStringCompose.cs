using System;

namespace TowerLoadCals.BLL
{
    public class VStringCompose
    {
        protected float xxx1;
        public float VCX1
        {   get
            {
                return xxx1;
            }
        }

        protected float xxx2;
        public float VCX2
        {
            get
            {
                return xxx2;
            }
        }

        protected float yyy1;
        public float VCY1
        {
            get
            {
                return yyy1;
            }
        }

        protected float yyy2;
        public float VCY2
        {
            get
            {
                return yyy2;
            }
        }

        protected float zzz1;
        public float VCZ1
        {
            get
            {
                return zzz1;
            }
        }

        protected float zzz2;
        public float VCZ2
        {
            get
            {
                return zzz2;
            }
        }

        public VStringCompose(float L1, float L2, float H1, float H2, float vc6, float xk, float yk, float zk)
        {

            //线条风荷载X向分配
            //V串分析
            float L = (float)Math.Sqrt((H2 - H1) * (H2 - H1) + (L2 + L1) * (L2 + L1));
            float cos1 = (L1 + L2) / L;
            float sin1 = (H2 - H1) / L;
            float S1 = (float)Math.Sqrt(L1 * L1 + H1 * H1);
            float S2 = (float)Math.Sqrt(L2 * L2 + H2 * H2);
            float St = (L + S1 + S2) / 2;
            float Area = (float)Math.Sqrt(St * (St - L) * (St - S1) * (St - S2));
            float H = 2 * Area / L;
            float L12 = (float)Math.Sqrt(S1 * S1 - H * H);
            float L22 = (float)Math.Sqrt(S2 * S2 - H * H);
            float cos3 = L12 / S1;
            float sin3 = H / S1;
            float cos4 = L22 / S2;
            float sin4 = H / S2;

            xxx1 = xk * cos1 - zk * sin1;
            yyy1 = yk;
            zzz1 = xk * sin1 + zk * cos1;

            float cos2;
            float sin2;

            if ((float)Math.Sqrt(zzz1 * zzz1 + yyy1 * yyy1) > 0)
            {
                cos2 = zzz1 / (float)Math.Sqrt(zzz1 * zzz1 + yyy1 * yyy1);
                sin2 = yyy1 / (float)Math.Sqrt(zzz1 * zzz1 + yyy1 * yyy1);
            }
            else
            {
                cos2 = 1;
                sin2 = 0;
            }

            if (zzz1 >= 0)
                zzz1 = (float)Math.Sqrt(yyy1 * yyy1 + zzz1 * zzz1);
            else
                zzz1 = -(float)Math.Sqrt(yyy1 * yyy1 + zzz1 * zzz1);

            float T1 = (zzz1 * cos4 + xxx1 * sin4) / (cos3 * sin4 + cos4 * sin3);
            float T2 = (zzz1 * cos3 - xxx1 * sin3) / (cos3 * sin4 + cos4 * sin3);

            if(T1 >= vc6 && T2 >= vc6)
            {
                //左侧
                xxx1 = T1 * cos3 * cos1 + T1 * sin3 * sin1 * cos2;
                yyy1 = T1 * sin3 * sin2;
                zzz1 = T1 * sin3 * cos1 * cos2 - T1 * cos3 * sin1;

                //右侧
                xxx2 = -T2 * cos4 * cos1 + T2 * sin4 * sin1 * cos2;
                yyy2 = T2 * sin4 * sin2;
                zzz2 = T2 * sin4 * cos1 * cos2 + T2 * cos4 * sin1;
            }
            else if(T1 < vc6 && T2 >= vc6)
            {
                // 左侧
                xxx1 = 0;
                yyy1 = 0;
                zzz1 = 0;
                //右侧
                xxx2 = xk;
                yyy2 = yk;
                zzz2 = zk;
            }
            else if(T1 >= vc6 && T2 < vc6)
            {
                xxx2 = 0;
                yyy2 = 0;
                zzz2 = 0;

                xxx1 = xk;
                yyy1 = yk;
                zzz1 = zk;
            }
            //else if(T1 < vc6 && T2 < vc6)
            else
            {
                // 左侧
                xxx1 = 0;
                yyy1 = 0;
                zzz1 = 0;
                //右侧
                xxx2 = 0;
                yyy2 = 0;
                zzz2 = 0;
                throw new Exception("V串分配错误,存在V串两侧均受压失效的情况");
            }
        }
    }
}
