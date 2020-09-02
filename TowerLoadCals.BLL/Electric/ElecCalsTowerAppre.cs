using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.BLL.Electric
{
    public class ElecCalsTowerAppre
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
        /// 上相导线高差(上相（中相）与下横担高差)
        /// </summary>
        public double UpSideInHei { get; set; }

        /// <summary>
        /// 中相导线高差(中相（边相）与下横担高差)
        /// </summary>
        public double MidInHei { get; set; }

        /// <summary>
        /// 下相导线高差(下相（边相）与下横担高差)
        /// </summary>
        public double DnSideInHei { get; set; }

        /// <summary>
        /// 地线高差(地线与下横担高差)
        /// </summary>
        public double GrDHei { get; set; }

        /// <summary>
        /// 上相跳线高差(上相（中相）跳线挂点与下横担高差)
        /// </summary>
        public double UpSideJuHei { get; set; }

        /// <summary>
        ///  中相跳线高差(中相（边相）跳线挂点与下横担高差)
        /// </summary>
        public double MidJuHei { get; set; }

        /// <summary>
        /// 下相跳线高差(下相（边相）跳线挂点与下横担高差)
        /// </summary>
        public double DnSideJuHei { get; set; }

        public ElecCalsTowerAppre()
        {
        }

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
        /// 设置传统参数 耐张塔
        /// </summary>
        /// <param name="upSideInHei"></param>
        /// <param name="midInHei"></param>
        /// <param name="grDHei"></param>
        /// <param name="upSideJuHei"></param>
        /// <param name="midJuHei"></param>
        /// <param name="dnSideJuHei"></param>
        public void SetTraPara(double upSideInHei, double midInHei,  double dnSideInHei, double grDHei, double upSideJuHei, double midJuHei, double dnSideJuHei)
        {
            UpSideInHei = upSideInHei;
            MidInHei = midInHei;
            DnSideInHei = dnSideInHei;
            GrDHei = grDHei;
            UpSideJuHei = upSideJuHei;
            MidJuHei = midJuHei;
            DnSideJuHei = dnSideJuHei;
        }


        /// <summary>
        /// 设置传统参数 悬垂塔
        /// </summary>
        /// <param name="upSideInHei"></param>
        /// <param name="midInHei"></param>
        /// <param name="dnSideInHei"></param>
        /// <param name="grDHei"></param>
        public void SetTraPara(double upSideInHei, double midInHei, double dnSideInHei, double grDHei)
        {
            UpSideInHei = upSideInHei;
            MidInHei = midInHei;
            DnSideInHei = dnSideInHei;
            GrDHei = grDHei;
        }
    }
}
