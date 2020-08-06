using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.BLL.Electric
{
    public class TowerUtils
    {
        /// <summary>
        /// 
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 塔号
        /// </summary>
        public string Num { get; set; }

        /// <summary>
        /// 塔位名称
        /// </summary>
        public string PosName { get; set; }

        /// <summary>
        /// 塔位偏移
        /// </summary>
        public int PosOffset { get; set; }

        /// <summary>
        /// 杆塔名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 杆塔呼高
        /// </summary>
        public float Height { get; set; }

        /// <summary>
        ///  累距
        /// </summary>
        public float AccumDis { get; set; }

        /// <summary>
        ///  塔位高程
        /// </summary>
        public float Elevation { get; set; }

        /// <summary>
        /// 基面降低
        /// </summary>
        public float SubOfElv { get; set; }

        /// <summary>
        /// 转角度数，小数，度
        /// </summary>
        public float AngelofApplication { get; set; }

        //考虑到多回路，不同分支的情况，将每根地线也作为一个回路中的一相
        //兼容传统模式，设计代表数据结构,均按照单侧实现
        /// <summary>
        ///  前侧位置结构
        /// </summary>
        public TowerPosStrUtils FrontPosRes { get; set; }

        /// <summary>
        /// 后侧位置结构
        /// </summary>
        public TowerPosStrUtils BackPosRes { get; set; }

        /// <summary>
        /// 导线等效串长
        /// </summary>
        public float RepStrIndLen { get; set; }

        /// <summary>
        /// 地线等效串长
        /// </summary>
        public float RepStrGrdLen { get; set; }

        /// <summary>
        /// 相列表，存储回路、相信息
        /// </summary>
        public List<StressPhaseStrUtils> PhaseTraList { get; set; }

        /// <summary>
        /// 导线等效串长
        /// </summary>
        public ElectrialCals BackSideRes { get; set; }

        /// <summary>
        /// 地线等效串长
        /// </summary>
        public ElectrialCals FrontSideRes { get; set; }


        //为兼容传统负荷计算方法设计的结构，非传统方法应在类中，特征相绝对高度,初始值为0
        //默认情况下，两个地线的高度是一致的
        /// <summary>
        /// 
        /// </summary>
        public float AbsUpsideHei { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public float AbsMidHei { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public float AbsDownSideHei { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public float AbsGrdHei { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public float AbsDownJumHei { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public float AbsMidJumHei { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public float AbsUpJumHei { get; set; }

        //特征相地理高度
        /// <summary>
        /// 
        /// </summary>
        public float AtiUpSide { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public float AtiMid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public float AtiDownd { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public float AtiGrd { get; set; }

        /// <summary>
        /// 上相（中相）与下横担高差
        /// </summary>
        public float UpSideInHei { get; set; }

        /// <summary>
        /// 中相（边相）与下横担高差
        /// </summary>
        public float MidInHei { get; set; }

        /// <summary>
        /// 下相（边相）与下横担高差
        /// </summary>
        public float DownSideHei { get; set; }

        /// <summary>
        /// 地线与下横担高差
        /// </summary>
        public float GrDHei { get; set; }


        /// <summary>
        /// 上相（中相）跳线挂点与下横担高差
        /// </summary>
        public float UpSideJuHei { get; set; }

        /// <summary>
        /// 中相（边相）跳线挂点与下横担高差
        /// </summary>
        public float DnSideJuHei { get; set; }

        /// <summary>
        /// 中相（边相）跳线挂点与下横担高差
        /// </summary>
        public float MidJuHei { get; set; }




        /// <summary>
        /// 配置杆塔基本外形信息
        /// </summary>
        public void SetAppreaPara(float upSideInHeiSor, float midInHeiSor, float downSideHeiSor, float grDHeiSor,
            float upSideJuHeiSor, float midJuHeiSor, float dnSideJuHeiSor)
        {
            UpSideInHei = upSideInHeiSor;
            MidInHei = midInHeiSor;
            DownSideHei = downSideHeiSor;
            GrDHei = grDHeiSor;
            DnSideJuHei = upSideJuHeiSor;
            MidJuHei = midJuHeiSor;
            UpSideJuHei = dnSideJuHeiSor;
        }

        /// <summary>
        /// 设置杆塔的基本信息,以前视方向为正方向
        /// </summary>
        public void SetPosInf(string num, string name, float frontSpan,  float frontRes, float footHei, float jm,
               float repStrIndLen, float repStrGrdLen, float angelofApplication)
        {
            Num = num;
            Name = name;
            FrontPosRes.Span = frontSpan;
            FrontPosRes.DRepresentSpan = frontRes;
            Elevation = footHei;
            SubOfElv = jm;
            RepStrIndLen = repStrIndLen;
            RepStrGrdLen = repStrGrdLen;
            AngelofApplication = angelofApplication;
        }


        /// <summary>
        /// 按照传统计算模式更新铁塔的本身各个挂点高度
        /// </summary>
        public void UpdataTowerTraHei()
        {
            AbsUpsideHei = Height + UpSideInHei;
            AbsMidHei = Height + MidInHei;
            AbsDownSideHei = Height + DownSideHei;
            AbsGrdHei = Height + GrDHei;
            AbsDownJumHei = Height + DnSideJuHei;
            AbsMidJumHei = Height + MidJuHei;
            AbsUpJumHei = Height + UpSideJuHei;
            //对于直线塔，各相减去串长
            //if(type == 1)
            {
                AbsUpsideHei = AbsUpsideHei - RepStrIndLen;
                AbsMidHei = AbsMidHei - RepStrIndLen;
                AbsDownSideHei = AbsDownSideHei - RepStrIndLen;
                AbsGrdHei = AbsGrdHei - RepStrGrdLen;
            }
        }

        /// <summary>
        /// 更新各相的绝对高度，考虑基面
        /// </summary>
        public void UpdateAtitudeTower()
        {
            float temAti = Elevation + SubOfElv;
            AtiUpSide = AbsUpsideHei + temAti;
            AtiMid = AbsMidHei + temAti;
            AtiDownd = AbsDownSideHei + temAti;
            AtiGrd = AbsGrdHei + temAti;
        }

        /// <summary>
        /// 采用后侧铁塔信息更新本塔数据
        /// </summary>
        /// <param name="BackTower"></param>
        public void UpdateBackInfo(TowerUtils backTower)
        {
            BackPosRes.Span = backTower.FrontPosRes.Span;
            BackPosRes.DRepresentSpan = backTower.FrontPosRes.DRepresentSpan;
        }



        /// <summary>
        /// 更新铁塔前后水平档距和垂直档距，此处计算可能没用
        /// </summary>
        public void FlashHoriVetSpan()
        {
            BackPosRes.HorizontalSpan = (float)Math.Ceiling((double)BackPosRes.Span / 2);
            FrontPosRes.HorizontalSpan = (float)Math.Ceiling((double)FrontPosRes.Span / 2); 
        }

    }
}
