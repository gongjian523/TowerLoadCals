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
        /// 导线等效串长
        /// </summary>
        public float RepStrIndLen { get; set; }

        /// <summary>
        /// 地线等效串长
        /// </summary>
        public float RepStrGrdLen { get; set; }

        /// <summary>
        /// 转角度数，小数，度
        /// </summary>
        public float AngelofApplication { get; set; }

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
        public List<StressPhaseStrUtils> PhaseTraList { get; set; }

        /// <summary>
        /// 前侧的计算资源
        /// </summary>
        public ElecCalsRes BackSideRes { get; set; }

        /// <summary>
        /// 后侧的计算资源
        /// </summary>
        public ElecCalsRes FrontSideRes { get; set; }


        //为兼容传统负荷计算方法设计的结构，非传统方法应在类中，特征相绝对高度,初始值为0
        //默认情况下，两个地线的高度是一致的
        /// <summary>
        /// 塔中相挂点高（单回）/塔上相挂点高（双回）
        /// </summary>
        public float AbsUpSideHei { get; set; }

        /// <summary>
        /// 塔边相挂点高（单回）/塔中相挂点高（双回）
        /// </summary>
        public float AbsMidHei { get; set; }

        /// <summary>
        /// 塔边相挂点高（单回）/塔下相挂点高（双回）
        /// </summary>
        public float AbsDownSideHei { get; set; }

        /// <summary>
        /// 地线挂点高
        /// </summary>
        public float AbsGrdHei { get; set; }

        /// <summary>
        /// 塔中相跳线串挂点高（单回）/塔上相跳线串挂点高（双回）
        /// </summary>
        public float AbsUpJumHei { get; set; }

        /// <summary>
        /// 塔边相跳线串挂点高（单回）/塔中相跳线串挂点高（双回）
        /// </summary>
        public float AbsMidJumHei { get; set; }


        /// <summary>
        /// 塔边相跳线串挂点高（单回）/塔下相跳线串挂点高（双回）
        /// </summary>
        public float AbsDownJumHei { get; set; }

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
        /// 中相（边相）与下横担高差（双回）
        /// </summary>
        public float MidInHei { get; set; }

        /// <summary>
        /// 下相（边相）与下横担高差（双回）
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
        /// 中相串μz（单回）/上相串μz（双回）
        /// </summary>
        public float UpStrMuz { get; set; }

        /// <summary>
        /// 边相串μz（单回）/中相串μz（双回）
        /// </summary>
        public float MidStrMuz { get; set; }

        /// <summary>
        /// 边相串μz（单回）/下相串μz（双回）
        /// </summary>
        public float DnStrMuz { get; set; }

        /// <summary>
        /// 地线串μz
        /// </summary>
        public float GrdStrMuz { get; set; }

        /// <summary>
        /// 中相跳线绝缘子μz（单回）/上相跳线绝缘子μz（双回）
        /// </summary>
        public float UpJumpStrMuz { get; set; }

        /// <summary>
        /// 边相跳线绝缘子μz（单回）/中相跳线绝缘子μz（双回）
        /// </summary>
        public float MidJumpStrMuz { get; set; }

        /// <summary>
        /// 边相跳线绝缘子μz（单回）/下相跳线绝缘子μz（双回）
        /// </summary>
        public float DnJumpStrMuz { get; set; }


        /// <summary>
        /// 中相跳线支撑管μz（单回）/上相跳线支撑管μz（双回）
        /// </summary>
        public float UpJumpSupMuz { get; set; }

        /// <summary>
        /// 边相跳线支撑管μz（单回）/中相跳线支撑管μz（双回）
        /// </summary>
        public float MidJumpSupMuz { get; set; }

        /// <summary>
        /// 边相跳线支撑管μz（单回）/下相跳线支撑管μz（双回）
        /// </summary>
        public float DnJumpSupMuz { get; set; }

        public TowerElecCals()
        {
            FrontPosRes = new ElecCalsTowerPosStr();
            BackPosRes = new ElecCalsTowerPosStr();
        }

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
            UpSideJuHei = upSideJuHeiSor;
            MidJuHei = midJuHeiSor;
            DnSideJuHei = dnSideJuHeiSor;
        }

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
        public void SetPosInf(string num, string name, float hei,  float footHei, float jm,
               float repStrIndLen, float repStrGrdLen, float angelofApplication)
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
        public void SetFrontBackPosInf(float frontSpan, float frontRes, float backSpan, float backRes)
        {
            FrontPosRes = new ElecCalsTowerPosStr(frontSpan, frontRes);
            BackPosRes = new ElecCalsTowerPosStr(backSpan, backRes);
        }


        /// <summary>
        /// 按照传统计算模式更新铁塔的本身各个挂点高度
        /// </summary>
        public virtual void UpdataTowerTraHei()
        {
            AbsUpSideHei = Height + UpSideInHei;
            AbsMidHei = Height + MidInHei;
            AbsDownSideHei = Height + DownSideHei;
            AbsGrdHei = Height + GrDHei;
            AbsDownJumHei = Height + DnSideJuHei;
            AbsMidJumHei = Height + MidJuHei;
            AbsUpJumHei = Height + UpSideJuHei;
        }

        /// <summary>
        /// 更新各相的绝对高度，考虑基面
        /// </summary>
        public void UpdateAtitudeTower()
        {
            float temAti = Elevation + SubOfElv;
            AtiUpSide = AbsUpSideHei + temAti;
            AtiMid = AbsMidHei + temAti;
            AtiDownd = AbsDownSideHei + temAti;
            AtiGrd = AbsGrdHei + temAti;
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

        public void UpdateFrontAppre(TowerElecCals frontTower)
        {
            UpdateAppre(FrontAppreRes, frontTower, FrontSideRes);
        }

        public void UpdateBackAppre(TowerElecCals backTower)
        {
            UpdateAppre(BackAppreRes, backTower, BackSideRes);
        }

        protected void UpdateAppre(ElecCalsTowerAppre towerAppre, TowerElecCals tower, ElecCalsRes calRes)
        {
            towerAppre.UpSideInHei = (float)Math.Round(CalsHei(AbsUpSideHei, Elevation ,SubOfElv, RepStrIndLen) - CalsHei(tower.AbsUpSideHei, tower.Elevation, tower.SubOfElv, tower.RepStrIndLen), 3);
            towerAppre.MidInHei = (float)Math.Round(CalsHei(AbsMidHei, Elevation, SubOfElv, RepStrIndLen) - CalsHei(tower.AbsMidHei, tower.Elevation, tower.SubOfElv, tower.RepStrIndLen), 3);
            towerAppre.DnSideInHei = (float)Math.Round(CalsHei(AbsDownSideHei, Elevation, SubOfElv, RepStrIndLen) - CalsHei(tower.AbsDownSideHei, tower.Elevation, tower.SubOfElv, tower.RepStrIndLen), 3);
            towerAppre.GrDHei  = (float)Math.Round(CalsHei(AbsGrdHei, Elevation, SubOfElv, RepStrGrdLen) - CalsHei(tower.AbsGrdHei, tower.Elevation, tower.SubOfElv, tower.RepStrGrdLen), 3);

            towerAppre.UpInMuz = CalsWireMuz(calRes.CommParas.WireWindPara, AbsUpSideHei, calRes.IndWire.WindVerSag, calRes.CommParas.IndAveHei);
            towerAppre.MidInMuz = CalsWireMuz(calRes.CommParas.WireWindPara, AbsMidHei, calRes.IndWire.WindVerSag, calRes.CommParas.IndAveHei);
            towerAppre.DnInMuz = CalsWireMuz(calRes.CommParas.WireWindPara, AbsDownSideHei, calRes.IndWire.WindVerSag, calRes.CommParas.IndAveHei);
            towerAppre.GrdMuz = CalsWireMuz(calRes.CommParas.WireWindPara, AbsUpSideHei,  Math.Min(calRes.GrdWire.WindVerSag, calRes.OPGWWire.WindVerSag), calRes.CommParas.IndAveHei);
        }

        protected float CalsHei(float wireHei, float elevation, float subOfElv, float repWireLen)
        {
            return (wireHei + elevation + subOfElv - repWireLen);
        }

        /// <summary>
        /// 计算线高空风压系数
        /// </summary>
        /// <param name="wireWindPara">计算方式 1：线平均高 2:按照下相挂点高反算 </param>
        /// <param name="wireHei">导线挂点高</param>
        /// <param name="wireWindVerSag">大风垂直方向弧垂</param>
        /// <param name="avaHei">导线计算平均高</param>
        /// <returns></returns>
        protected float CalsWireMuz(int wireWindPara, float wireHei, float wireWindVerSag , float avaHei)
        {
            //1：线平均高 2:按照下相挂点高反算
            if (wireWindPara == 1)
            {
                //按线平均高
                return (int)(Math.Pow(((wireHei - 2 / 3 * wireWindVerSag) / avaHei), 0.32) * 1000 + 0.5) / 1000;
            }
            else
            {
                //按挂点高
                //这儿wireHei在Excel中指定是平均上/中/下/地线高，在 计算方式为按照挂点高时，直接等于塔上/中/下相挂点高
                return (int)(Math.Pow((wireHei / avaHei), 0.32) * 1000 + 0.5) / 1000;
            }
        }

        /// <summary>
        /// 更新绝缘子串
        /// </summary>
        public void UpdateAppreStr()
        {
            UpStrMuz = CalsStrMuz(AbsUpSideHei, FrontSideRes.CommParas.IndAveHei);
            MidStrMuz = CalsStrMuz(AbsMidHei, FrontSideRes.CommParas.IndAveHei);
            DnStrMuz = CalsStrMuz(AbsDownSideHei, FrontSideRes.CommParas.IndAveHei);
            GrdStrMuz = CalsStrMuz(AbsGrdHei, FrontSideRes.CommParas.IndAveHei);

            UpJumpStrMuz = CalsJumpStrMuz(FrontSideRes.CommParas.JmpWindPara, AbsUpJumHei, FrontSideRes.CommParas.IndAveHei, FrontSideRes.CommParas.JumpStrLen);
            MidJumpStrMuz = CalsJumpStrMuz(FrontSideRes.CommParas.JmpWindPara, AbsMidJumHei, FrontSideRes.CommParas.IndAveHei, FrontSideRes.CommParas.JumpStrLen);
            DnJumpStrMuz = CalsJumpStrMuz(FrontSideRes.CommParas.JmpWindPara, AbsDownJumHei, FrontSideRes.CommParas.IndAveHei, FrontSideRes.CommParas.JumpStrLen);

            UpJumpSupMuz = CalsJumpSupMuz(FrontSideRes.CommParas.JmpWindPara, AbsUpJumHei, FrontSideRes.CommParas.IndAveHei, FrontSideRes.CommParas.JumpStrLen);
            MidJumpSupMuz = CalsJumpSupMuz(FrontSideRes.CommParas.JmpWindPara, AbsMidJumHei, FrontSideRes.CommParas.IndAveHei, FrontSideRes.CommParas.JumpStrLen);
            DnJumpSupMuz = CalsJumpSupMuz(FrontSideRes.CommParas.JmpWindPara, AbsDownJumHei, FrontSideRes.CommParas.IndAveHei, FrontSideRes.CommParas.JumpStrLen);


        }

        /// <summary>
        /// 计算绝缘子串高空风压系数
        /// </summary>
        /// <param name="wireHei">挂点高</param>
        /// <param name="avaHei">导线计算平均高</param>
        /// <returns></returns>
        protected float CalsStrMuz(float wireHei, float avaHei)
        {
            return (float) Math.Round( Math.Pow((wireHei / avaHei), 0.32),  3);
        }


        /// <summary>
        /// 计算跳串高空风压系数
        /// </summary>
        /// <param name="wireWindPara">跳串高空风压系数</param>
        /// <param name="strHei"></param>
        /// <param name="avaHei">导线计算平均高</param>
        /// <param name="jumpStrLen">跳线绝缘子串长</param>
        /// <returns></returns>
        protected float CalsJumpStrMuz(int jumpWindPara, float strHei, float avaHei, float jumpStrLen)
        {
            //1：挂线高，2：按照跳线中点高度，硬跳线按照实际高度
            if (jumpWindPara == 1)
            {
                //按挂点高
                return (float)Math.Round(Math.Pow(strHei/avaHei, 0.32),3);
            }
            else
            {
                //按平均高
                return (float)Math.Round(Math.Pow((strHei - jumpStrLen/2)/avaHei,0.32),3);
            }
        }

        /// <summary>
        /// 计算支撑管高空风压系数
        /// </summary>
        /// <param name="wireWindPara">跳串高空风压系数</param>
        /// <param name="strHei"></param>
        /// <param name="avaHei">导线计算平均高</param>
        /// <param name="jumpStrLen">跳线绝缘子串长</param>
        /// <returns></returns>
        protected float CalsJumpSupMuz(int jumpWindPara, float strHei, float avaHei, float jumpStrLen)
        {
            //1：挂线高，2：按照跳线中点高度，硬跳线按照实际高度
            if (jumpWindPara == 1)
            {
                //按挂点高
                return (float)Math.Round(Math.Pow(strHei / avaHei, 0.32), 3);
            }
            else
            {
                //按平均高
                return (float)Math.Round(Math.Pow((strHei - jumpStrLen) / avaHei, 0.32), 3);
            }
        }

        /// <summary>
        /// 更新铁塔前后水平档距和垂直档距，此处计算可能没用
        /// </summary>
        public void FlashHoriVetSpan()
        {
            BackPosRes.HorizontalSpan = (float)Math.Ceiling((double)BackPosRes.Span / 2);
            FrontPosRes.HorizontalSpan = (float)Math.Ceiling((double)FrontPosRes.Span / 2); 
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
