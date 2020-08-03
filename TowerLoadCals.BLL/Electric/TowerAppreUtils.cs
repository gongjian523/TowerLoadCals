using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.BLL.Electric
{
    public class TowerAppreUtils
    {
        /// <summary>
        /// 
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int CirNum { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int CurType { get; set; }

        /// <summary>
        /// 标记基本参数是否设置完毕
        /// </summary>
        public bool SetBasePara { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int GroundNum { get; set; }


        /// <summary>
        /// 上相导线高差
        /// </summary>
        public float UpSideInHei { get; set; }

        /// <summary>
        /// 中相导线高差
        /// </summary>
        public float MidInHei { get; set; }

        /// <summary>
        /// 地线高差
        /// </summary>
        public float GrDHei { get; set; }

        /// <summary>
        /// 上相跳线高差
        /// </summary>
        public float UpSideJuHei { get; set; }

        /// <summary>
        ///  中相跳线高差
        /// </summary>
        public float MidJuHei { get; set; }

        /// <summary>
        /// 下相跳线高差
        /// </summary>
        public float DnSideJuHei { get; set; }

        /// <summary>
        /// 支持传统参数设置
        /// </summary>
        /// <param name="type"></param>
        /// <param name="cirNum"></param>
        /// <param name="groundNum"></param>
        /// <param name="curType"></param>
        public void GetParaFromTower(int type= 1, int cirNum= 1, int groundNum= 2, int curType= 0)
        {
            Type = type;
            CirNum = cirNum;
            CurType = curType;
            SetBasePara = true;
            GroundNum = groundNum;
        }

        /// <summary>
        /// 设置传统参数
        /// </summary>
        /// <param name="upSideInHei"></param>
        /// <param name="midInHei"></param>
        /// <param name="grDHei"></param>
        /// <param name="upSideJuHei"></param>
        /// <param name="midJuHei"></param>
        /// <param name="dnSideJuHei"></param>
        public void SetTraPara(float upSideInHei, float midInHei, float grDHei, float upSideJuHei, float midJuHei, float dnSideJuHei)
        {
            UpSideInHei = upSideInHei;
            MidInHei = midInHei;
            GrDHei = grDHei;
            UpSideJuHei = upSideJuHei;
            MidJuHei = midJuHei;
            DnSideJuHei = dnSideJuHei;
        }
    }
}
