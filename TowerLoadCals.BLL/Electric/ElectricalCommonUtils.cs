using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerLoadCals.Mode;

namespace TowerLoadCals.BLL.Electric
{
    public class ElectricalCommonUtils
    {
        /// <summary>
        /// 1,代表传统计算模式；2，采用示例铁塔计算模式
        /// </summary>
        public int CalMode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Volt { get; set; }

        /// <summary>
        /// 新线系数，默认值为0.95
        /// </summary>
        public float NewPerPara { get; set; }

        /// <summary>
        /// 重力加速度常数
        /// </summary>
        public float GraAcc { get; set; }

        /// <summary>
        /// 地形类型
        /// </summary>
        public char TerType { get; set; }

        /// <summary>
        /// 导地线高空风压系数计算模式  
        /// 1.平均高度计算 2.按照挂点高减去2/3弧垂
        /// </summary>
        public int HeiDDType { get; set; }

        /// <summary>
        /// 跳串高空风压系数  
        /// 1.全部按照最高高度计算 2.  考虑支撑管高度
        /// </summary>
        public int HeiJmpType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public float SecIndInc { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public float WeiIndInc { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public float DiaIndInc { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public float SecGrdInc { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public float WeiGrdInc { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public float DiaGrdInc { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public float SecOPGWInc { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public float WeiOPGWInc { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public float DiaOPGWInc { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public float BuildMaxPara { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public float InstMaxPara { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public float IndExMaxPara { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public float GrdExMaxPara { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public float BuildMinPara { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public float InstMinPara { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public float IndExMinPara { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public float GrdExMinPara { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public float IndODri { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public float GrdODri { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public float IndBrePara { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public float GrdBrePara { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public float UnbalanceMaxPer { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public float UnbalanceMinPer { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public float BreakPer { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int WireWindPara { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int JmpWindPara { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int GrdIceForcePara { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int GrdIceUnbaPara { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int GrdIceCheckPara { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int HandForcePara { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int BreakInPara { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int BreakMaxPara { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int UnbaMaxPara { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int LevelOfLine { get; set; }

        public float IndAveHei { get; private set; }
        public float GrdAveHei { get; private set; }

        public ElectricalCommonUtils(float newPerPara= 0.95f, float graAcc = 9.80665f, char terType= 'B',
                int heiDDType= 1, int heiJmpType= 1, int volt= 0, float indAveHei= 0, float grdAveHei= 0)
        {
            SetComPara(newPerPara, graAcc, terType, heiDDType, heiJmpType, indAveHei, grdAveHei);
            CalMode = 1;
            Volt = volt;
        }


        void SetComPara(float newPerPara= 0.95f, float graAcc= 9.80665f, char terType= 'B', int heiDDType= 1, 
            int heiJmpType= 1, float indAveHei= 0, float grdAveHei = 0)
        {
            //新线系数，默认值为0.95
            NewPerPara = newPerPara;
            //重力加速度常数
            GraAcc = graAcc;
            //地形类型 
            TerType = terType;
            //导地线高空风压系数计算模式  1.平均高度计算 2.按照挂点高减去2/3弧垂
            HeiDDType = heiDDType;
            //跳串高空风压系数  1.全部按照最高高度计算 2.  考虑支撑管高度
            HeiJmpType = heiJmpType;
            SetAverage(indAveHei, grdAveHei);
        }


        /// <summary>
        /// #导\地\OPGW增大系数,Sec代表界面,Wei代表重量，Dia代表直径
        /// </summary>
        /// <param name="secIndInc"></param>
        /// <param name="weiIndInc"></param>
        /// <param name="diaIndInc"></param>
        /// <param name="secGrdInc"></param>
        /// <param name="weiGrdInc"></param>
        /// <param name="diaGrdInc"></param>
        /// <param name="secOPGWInc"></param>
        /// <param name="weiOPGWInc"></param>
        /// <param name="diaOPGWInc"></param>
        public void SetForIncrPara(float secIndInc = 1, float weiIndInc = 1, float diaIndInc = 1, float secGrdInc = 1,
            float weiGrdInc = 1, float diaGrdInc = 1, float secOPGWInc = 1, float weiOPGWInc = 1, float diaOPGWInc = 1)
        {
            SecIndInc = secIndInc;
            WeiIndInc = weiIndInc;
            DiaIndInc = diaIndInc;
            SecGrdInc = secGrdInc;
            WeiGrdInc = weiGrdInc;
            DiaGrdInc = diaGrdInc;
            SecOPGWInc = secOPGWInc;
            WeiOPGWInc = weiOPGWInc;
            DiaOPGWInc = diaOPGWInc;
        }


        /// <summary>
        /// 张力系数，Build:施工误差,Inst:安装误差，IndEx：导线伸长系数，GrdEx：地线伸长系数
        /// </summary>
        /// <param name="buildMaxPara"></param>
        /// <param name="instMaxPara"></param>
        /// <param name="indExMaxPara"></param>
        /// <param name="grdExMaxPara"></param>
        /// <param name="buildMinPara"></param>
        /// <param name="instMinPara"></param>
        /// <param name="indExMinPara"></param>
        /// <param name="grdExMinPara"></param>
        public void SetForMaxMinPara(float buildMaxPara, float instMaxPara, float indExMaxPara, float grdExMaxPara,
            float buildMinPara, float instMinPara, float indExMinPara, float grdExMinPara)
        {
            BuildMaxPara = buildMaxPara;
            InstMaxPara = instMaxPara;
            IndExMaxPara = indExMaxPara;
            GrdExMaxPara = grdExMaxPara;
            BuildMinPara = buildMinPara;
            InstMinPara = instMinPara;
            IndExMinPara = indExMinPara;
            GrdExMinPara = grdExMinPara;
        }

        /// <summary>
        /// 过牵引系数,导线默认为0.2，地线默认为0.1
        /// </summary>
        /// <param name="indODri"></param>
        /// <param name="grdODri"></param>
        public void SetOverDrive(float indODri= 0.2f, float grdODri= 0.1f)
        {
            IndODri = indODri;
            GrdODri = grdODri;
        }

        /// <summary>
        /// 断线张力系数
        /// </summary>
        /// <param name="indBrePara"></param>
        /// <param name="grdBrePara"></param>
        public void SetBreakWirePara(float indBrePara = 0.7f, float grdBrePara = 1)
        {
            IndBrePara = indBrePara;
            GrdBrePara = grdBrePara;
        }


        /// <summary>
        /// 不均匀冰覆冰率，断线覆冰率
        /// </summary>
        /// <param name="unbalanceMaxPer"></param>
        /// <param name="unbalanceMinPer"></param>
        /// <param name="breakPer"></param>
        public void SetIcePercent(float unbalanceMaxPer= 1, float unbalanceMinPer= 0, float breakPer= 1)
        {
            UnbalanceMaxPer = unbalanceMaxPer;
            UnbalanceMinPer = unbalanceMinPer;
            BreakPer = breakPer;
        }

        /// <summary>
        /// 计算方法参数
        /// </summary>
        /// <param name="wireWindPara">导地线高空风压系数计算模式，1：线平均高 2:按照下相挂点高反算</param>
        /// <param name="jmpWindPara">跳线高空风压系数计算模式，1：挂线高，2：按照跳线中点高度，硬跳线按照实际高度</param>
        /// <param name="grdIceForcePara">地线覆冰张力计算模式，1：加5mm冰计算张力，2：不增加5mm冰计算</param>
        /// <param name="grdIceUnbaPara">地线不平衡张力取值，1:轻冰区不考虑增加5mm，2：重冰区增加5mm</param>
        /// <param name="grdIceCheckPara">地线验算张力取值，1:不考虑增加5mm，2：考虑增加5mm</param>
        /// <param name="handForcePara">锚线张力取值方法，1:取两者最大值，2:系数法，3：降温法</param>
        /// <param name="breakInPara">断线张力填法：1：直线塔，0/张力差；2：耐张塔，max/(max-张力差)</param>
        /// <param name="breakMaxPara">断线最大张力取值，1：最大允许张力，2：100%覆冰率断线情况</param>
        /// <param name="unbaMaxPara">不均匀冰最大张力取值，1：最大允许张力，2：100%覆冰率断线情况</param>
        public void CalMethodPara(int wireWindPara= 1, int jmpWindPara= 1, int grdIceForcePara= 1, int grdIceUnbaPara= 1,
            int grdIceCheckPara= 1, int handForcePara= 1, int breakInPara= 1, int breakMaxPara= 1, int unbaMaxPara= 1)
        {
            WireWindPara = wireWindPara;
            JmpWindPara = jmpWindPara;
            GrdIceForcePara = grdIceForcePara;
            GrdIceUnbaPara = grdIceUnbaPara;
            GrdIceCheckPara = grdIceCheckPara;
            HandForcePara = handForcePara;
            BreakInPara = breakInPara;
            BreakMaxPara = breakMaxPara;
            UnbaMaxPara = unbaMaxPara;      
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ProjeIn"></param>
        public void UpdataLevelOfLine(ProInfo projeIn)
        {
            LevelOfLine = ElectricalCalsToolBox .GetLevelOfLine(projeIn.Volt, projeIn.ACorDC);
        }

        public void SetAverage(float indAveHei= 0, float grdAveHei= 0)
        {
            if (indAveHei <= 0)
            {
                //如果平均高度未设置，那么根据电压等级取值,此处平均高度是计算应力弧垂
                IndAveHei = ElectricalCalsToolBox.AveHeightDefault(Volt);
            }
            else
            {
                IndAveHei = indAveHei;
            }

            if(grdAveHei <= 0)
            {
                GrdAveHei = IndAveHei;
            }
            else
            {
                GrdAveHei = grdAveHei;
            }
        }


    }
}
