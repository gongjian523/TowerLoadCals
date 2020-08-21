using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.BLL.Electric
{
    public class TowerElecCals
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
        public double Height { get; set; }

        /// <summary>
        ///  累距
        /// </summary>
        public double AccumDis { get; set; }

        /// <summary>
        ///  塔位高程
        /// </summary>
        public double Elevation { get; set; }

        /// <summary>
        /// 基面降低
        /// </summary>
        public double SubOfElv { get; set; }

        /// <summary>
        /// 导线等效串长
        /// </summary>
        public double RepStrIndLen { get; set; }

        /// <summary>
        /// 地线等效串长
        /// </summary>
        public double RepStrGrdLen { get; set; }

        /// <summary>
        /// 转角度数，小数，度
        /// </summary>
        public double AngelofApplication { get; set; }

        //考虑到多回路，不同分支的情况，将每根地线也作为一个回路中的一相
        //兼容传统模式，设计代表数据结构,均按照单侧实现
        /// <summary>
        ///  前侧位置结构
        /// </summary>
        public ElecCalsTowerPosStr FrontPosRes { get; set; }

        /// <summary>
        /// 后侧位置结构
        /// </summary>
        public ElecCalsTowerPosStr BackPosRes { get; set; }


        /// <summary>
        /// 后侧位置结构
        /// </summary>
        public ElecCalsTowerAppre FrontAppreRes { get; set; }

        /// <summary>
        /// 后侧位置结构
        /// </summary>
        public ElecCalsTowerAppre BackAppreRes { get; set; }


        /// <summary>
        /// 相列表，存储回路、相信息
        /// </summary>
        public List<ElecCalsStressPhaseStr> PhaseTraList { get; set; }

        /// <summary>
        /// 前侧的计算资源
        /// </summary>
        public ElecCalsRes BackSideRes { get; set; }

        /// <summary>
        /// 后侧的计算资源
        /// </summary>
        public ElecCalsRes FrontSideRes { get; set; }

        /// <summary>
        /// 铁塔外形数据
        /// </summary>
        public ElecCalsTowerAppre TowerAppre { get; set; }


        //为兼容传统负荷计算方法设计的结构，非传统方法应在类中，特征相绝对高度,初始值为0
        //默认情况下，两个地线的高度是一致的
        /// <summary>
        /// 塔中相挂点高（单回）/塔上相挂点高（双回）
        /// </summary>
        public double AbsUpSideHei { get; set; }

        /// <summary>
        /// 塔边相挂点高（单回）/塔中相挂点高（双回）
        /// </summary>
        public double AbsMidHei { get; set; }

        /// <summary>
        /// 塔边相挂点高（单回）/塔下相挂点高（双回）
        /// </summary>
        public double AbsDownSideHei { get; set; }

        /// <summary>
        /// 地线挂点高
        /// </summary>
        public double AbsGrdHei { get; set; }

        /// <summary>
        /// 塔中相跳线串挂点高（单回）/塔上相跳线串挂点高（双回）
        /// </summary>
        public double AbsUpJumHei { get; set; }

        /// <summary>
        /// 塔边相跳线串挂点高（单回）/塔中相跳线串挂点高（双回）
        /// </summary>
        public double AbsMidJumHei { get; set; }


        /// <summary>
        /// 塔边相跳线串挂点高（单回）/塔下相跳线串挂点高（双回）
        /// </summary>
        public double AbsDownJumHei { get; set; }

        //特征相地理高度
        /// <summary>
        /// 
        /// </summary>
        public double AtiUpSide { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double AtiMid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double AtiDownd { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double AtiGrd { get; set; }

        ///// <summary>
        ///// 上相（中相）与下横担高差
        ///// </summary>
        //public double UpSideInHei { get; set; }

        ///// <summary>
        ///// 中相（边相）与下横担高差（双回）
        ///// </summary>
        //public double MidInHei { get; set; }

        ///// <summary>
        ///// 下相（边相）与下横担高差（双回）
        ///// </summary>
        //public double DownSideHei { get; set; }

        ///// <summary>
        ///// 地线与下横担高差
        ///// </summary>
        //public double GrDHei { get; set; }


        ///// <summary>
        ///// 上相（中相）跳线挂点与下横担高差
        ///// </summary>
        //public double UpSideJuHei { get; set; }

        ///// <summary>
        ///// 中相（边相）跳线挂点与下横担高差
        ///// </summary>
        //public double DnSideJuHei { get; set; }

        ///// <summary>
        ///// 中相（边相）跳线挂点与下横担高差
        ///// </summary>
        //public double MidJuHei { get; set; }

        public virtual string TowerType { get; set; }

        public TowerElecCals()
        {
            FrontPosRes = new ElecCalsTowerPosStr();
            BackPosRes = new ElecCalsTowerPosStr();

            PhaseTraList = new List<ElecCalsStressPhaseStr>();
            for (int i = 0; i < 10; i++)
            {
                PhaseTraList.Add(new ElecCalsStressPhaseStr());
            }
        }

        /// <summary>
        /// 配置杆塔基本外形信息
        /// </summary>
        //public void SetAppreaPara(double upSideInHeiSor, double midInHeiSor, double downSideHeiSor, double grDHeiSor,
        //    double upSideJuHeiSor, double midJuHeiSor, double dnSideJuHeiSor)
        //{
        //    UpSideInHei = upSideInHeiSor;
        //    MidInHei = midInHeiSor;
        //    DownSideHei = downSideHeiSor;
        //    GrDHei = grDHeiSor;
        //    UpSideJuHei = upSideJuHeiSor;
        //    MidJuHei = midJuHeiSor;
        //    DnSideJuHei = dnSideJuHeiSor;
        //}

        /// <summary>
        /// 设置杆塔的基本信息,以前视方向为正方向
        /// </summary>
        /// <param name="num">塔号-编号</param>
        /// <param name="name">杆塔名称</param>
        /// <param name="hei">杆塔呼高</param>
        /// <param name="footHei">塔位高程</param>
        /// <param name="jm">基面降低</param>
        /// <param name="repStrIndLen">导线等效串长</param>
        /// <param name="repStrGrdLen">地线等效串长</param>
        /// <param name="angelofApplication"> ？？ 有待确认是否放在这里</param>
        public void SetPosInf(string num, string name, double hei,  double footHei, double jm,
               double repStrIndLen, double repStrGrdLen, double angelofApplication)
        {
            Num = num;
            Name = name;
            Height = hei;
            Elevation = footHei;
            SubOfElv = jm;
            RepStrIndLen = repStrIndLen;
            RepStrGrdLen = repStrGrdLen;
            AngelofApplication = angelofApplication;
        }

        /// <summary>
        /// 设置和前后侧塔的位置关系
        /// </summary>
        /// <param name="frontSpan">前侧档距</param>
        /// <param name="frontRes">前侧代表档距</param>
        /// <param name="backSpan">后侧档距</param>
        /// <param name="backRes">后侧代表档距</param>
        public void SetFrontBackPosInf(double frontSpan, double frontRes, double backSpan, double backRes)
        {
            FrontPosRes = new ElecCalsTowerPosStr(frontSpan, frontRes);
            BackPosRes = new ElecCalsTowerPosStr(backSpan, backRes);
        }


        /// <summary>
        /// 按照传统计算模式更新铁塔的本身各个挂点高度
        /// </summary>
        public virtual void UpdataTowerTraHei()
        {
            AbsUpSideHei = Height + TowerAppre.UpSideInHei;
            AbsMidHei = Height + TowerAppre.MidInHei;
            AbsDownSideHei = Height + TowerAppre.DnSideInHei;
            AbsGrdHei = Height + TowerAppre.GrDHei;
            AbsDownJumHei = Height + TowerAppre.DnSideJuHei;
            AbsMidJumHei = Height + TowerAppre.MidJuHei;
            AbsUpJumHei = Height + TowerAppre.UpSideJuHei;
        }

        /// <summary>
        /// 更新各相的绝对高度，考虑基面
        /// </summary>
        //public void UpdateAtitudeTower()
        //{
        //    double temAti = Elevation + SubOfElv;
        //    AtiUpSide = AbsUpSideHei + temAti;
        //    AtiMid = AbsMidHei + temAti;
        //    AtiDownd = AbsDownSideHei + temAti;
        //    AtiGrd = AbsGrdHei + temAti;
        //}

        public void UpdateAtitudeTower()
        {
            double temAti = Elevation + SubOfElv;
            AtiUpSide = AbsUpSideHei + temAti - RepStrIndLen;
            AtiMid = AbsMidHei + temAti - RepStrIndLen;
            AtiDownd = AbsDownSideHei + temAti - RepStrIndLen;
            AtiGrd = AbsGrdHei + temAti - RepStrGrdLen;
        }


        /// <summary>
        /// 采用后侧铁塔信息更新本塔数据
        /// </summary>
        /// <param name="BackTower"></param>
        public void UpdateBackInfo(TowerElecCals backTower)
        {
            BackPosRes.Span = backTower.FrontPosRes.Span;
            BackPosRes.DRepresentSpan = backTower.FrontPosRes.DRepresentSpan;
        }



        /// <summary>
        /// 更新铁塔前后水平档距和垂直档距，此处计算可能没用
        /// </summary>
        public void FlashHoriVetSpan()
        {
            BackPosRes.HorizontalSpan = Math.Ceiling(BackPosRes.Span / 2);
            FrontPosRes.HorizontalSpan = Math.Ceiling(FrontPosRes.Span / 2); 
        }


        public List<string> PrintHei(string prefix, bool bWithJump)
        {
            int spaceLength1 = 8, spaceLength2 = 8;

            List<string> rslt = new List<string>();

            string szTitle = prefix.PadRight(spaceLength1) + "塔上相挂点高  " + AbsUpSideHei.ToString("0.##").PadRight(spaceLength2)  
                + "塔中相挂点高 "+ AbsMidHei.ToString("0.##").PadRight(spaceLength2)
                + "塔下相挂点高 " + AbsDownSideHei.ToString("0.##").PadRight(spaceLength2)
                + "地线挂点高 " + AbsGrdHei.ToString("0.##").PadRight(spaceLength2);
            rslt.Add(szTitle);

            if(bWithJump)
            {
                string szTitleJump = prefix.PadRight(spaceLength1) + "塔上相跳线串挂点高 " + AbsUpJumHei.ToString("0.##").PadRight(spaceLength2)
                    + "塔中相跳线串挂点高 " + AbsMidJumHei.ToString("0.##").PadRight(spaceLength2)
                    + "塔下相跳线串挂点高 " + AbsDownJumHei.ToString("0.##").PadRight(spaceLength2);
                rslt.Add(szTitleJump);
            }

            return rslt; 

        }
    }
}
