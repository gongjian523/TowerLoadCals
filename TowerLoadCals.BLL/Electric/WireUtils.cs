using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerLoadCals.Mode.Electric;

namespace TowerLoadCals.BLL.Electric
{
    public class WireUtils
    {

        public int ID {get; set;}

        /// <summary>
        /// 导线型号
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 截面积 mm2
        /// </summary>
        public float Sec { get; set; }

        /// <summary>
        /// 直径 mm
        /// </summary>
        public float Dia { get; set; }

        /// <summary>
        /// 单位质量  Kg/Km,默认换算为kg/m
        /// </summary>
        public float Wei { get; set; }

        /// <summary>
        /// 综合弹性系数 Gpa 10^6 N/m2
        /// </summary>
        public float Elas { get; set; }

        /// <summary>
        /// 线性膨胀系数 10^-6/℃
        /// </summary>
        public float Coef { get; set; }

        /// <summary>
        /// 计算拉断力 N
        /// </summary>
        public float Fore { get; set; }

        /// <summary>
        /// #0：导线；1：普通地线；2：OPGW
        /// </summary>
        public int bGrd { get; set; }

        /// <summary>
        /// #降温值
        /// </summary>
        public float DecrTem { get; set; }

        /// <summary>
        /// 导线分裂数
        /// </summary>
        public float DevideNum { get; set; }

        /// <summary>
        /// 控制应力
        /// </summary>
        public float CtrlYL { get; set; }

        /// <summary>
        /// 平均应力
        /// </summary>
        public float AvaYL { get; set; }

        /// <summary>
        /// 比载字典
        /// </summary>
        public Dictionary<string, BZResult> BzDic { get; set; }

        /// <summary>
        /// 应力字典
        /// </summary>
        public Dictionary<string, float> ForSpanDic { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public WeatherUtils  WeatherParas { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ElectricalCommonUtils CommonParas { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public SideCalUtils SideParas { get; set; }

        public Dictionary<string, float>  YLTable { get; set; }


        public string CtrNaSave { get; set; }

        public float Comg1 { get; set; }

        public float EffectPara { get; set; }
        public float SafePara { get; private set; }
        public float AvePara { get; private set; }
        public float OPGWEffectPara { get; private set; }
        public float OPGWSafePara { get; private set; }
        public float OPGWAnPara { get; private set; }

        public WireUtils(string name = "", int id = 0, float sec = 0, float dia = 0, float wei = 0, float elas = 0, 
            float coef = 0, float fore = 0, int grd = 0, float decrTem = 0, int devide = 0)
        {
            ID = id;
            Name = name;
            Sec = sec;
            Dia = dia;
            Wei = wei / 1000;
            Elas = elas;
            Coef = (float)(coef * 1e-6);
            Fore = fore;
            bGrd = grd;
            DecrTem = decrTem;
            DevideNum = devide;
            BzDic = new Dictionary<string, BZResult>();
            ForSpanDic = new Dictionary<string, float>();
        }

        /// <summary>
        /// 更新参数
        /// </summary>
        /// <param name="weaData"></param>
        /// <param name="coePara">通用系数</param>
        /// <param name="wirePara">导线参数</param>
        public void UpdataPara(WeatherUtils weaData, ElectricalCommonUtils coePara, SideCalUtils sideParas)
        {
            WeatherParas = weaData;
            CommonParas = coePara;
            SideParas = sideParas;
        }

        /// <summary>
        /// 计算指定档距下应力比载并存入对象中,一般采用这个接口计算
        /// </summary>
        /// <param name="span"></param>
        public void SaveYLTabel(float span)
        {
            YLTable = CalFTable(span);
        }

        /// <summary>
        /// #计算导线比载,默认采用老规范计算，计算参数取1，新规范取2
        /// </summary>
        /// <param name="CalType"></param>
        public void CalBZ(int CalType = 1)
        {
            // 导线平均高度
            //todo 
            //aveHei = CommonParas.AveHei
            var aveHei = CommonParas.IndAveHei;
            var terType = CommonParas.TerType;

            WeatherParas.ConverWind(aveHei, terType);

            // 如果是地线计算，增加地线覆冰工况
            if (bGrd > 0)
                WeatherParas.AddGrdWeath();

            //增加荷载计算中需要的工况：
            WeatherParas.AddOtherGk();

            //计算比载
            var graAcc = CommonParas.GraAcc;

            //换算最大风速值
            var maxWindCon = WeatherParas.WeathComm.Where(item => item.SWorkConditionName == "换算最大风速").First().WindSpeed;          
            var maxWindSt = WeatherParas.WeathComm.Where(item => item.SWorkConditionName == "最大风速").First().WindSpeed;

            float maxUpbaWindSt = 0, maxUpbaWindCon = 0;

            //如果存在不均匀风工况
            if (WeatherParas.WeathComm.Where(item => item.SWorkConditionName == "不均匀风").Count()> 0)
            {
                maxUpbaWindSt = WeatherParas.WeathComm.Where(item => item.SWorkConditionName == "不均匀风").First().WindSpeed;
                maxUpbaWindCon = WeatherParas.WeathComm.Where(item => item.SWorkConditionName == "换算不均匀风").First().WindSpeed;
            }

            //g1为统一值
            Comg1 = Wei * graAcc / Sec;

            foreach(var weaItem in WeatherParas.WeathComm)
            {
                if(CalType == 1)
                {
                    BZResult bz = new BZResult();

                    //温度
                    var temVal = weaItem.Temperature;
                    float winVal;

                    // 大风工况采用换算后的数值计算
                    if (weaItem.SWorkConditionName == "最大风速")
                    {
                        winVal = maxWindCon;
                    }
                    else if (weaItem.SWorkConditionName == "不均匀风")
                    {
                        winVal = maxUpbaWindCon;
                    }
                    else
                    {
                        winVal = weaItem.WindSpeed;
                    }

                    float iceVal = weaItem.IceThickness;

                    float alpha = ElectricalCalsToolBox.WindAlpha(winVal, iceVal, 1);
                    bz.g1 = Comg1;

                    float g2 = (float)(graAcc * 0.9 * Math.PI * iceVal * (iceVal + Dia) * 1e-3 / Sec);
                    bz.g2 = g2;

                    float g3 = Comg1 + g2;
                    bz.g3 = g3;

                    // Usc系数不区分覆冰与不覆冰
                    float usc = ElectricalCalsToolBox.WindEpson(iceVal, Dia);
                    //横向比载
                    float g4 = (float)(0.625 * Math.Pow(winVal, 2) * Dia * alpha * usc * 1e-3 / Sec);
                    bz.g4 = g4;

                    //在覆冰计算中，计入覆冰增大系数
                    float bex = ElectricalCalsToolBox.WindLoadEnlargeCoe(iceVal);
                    float g5 = (float)(0.625 * Math.Pow(winVal, 2) * (Dia + 2 * iceVal) * alpha * usc * bex * 1e-3 / Sec);
                    bz.g5 = g5;

                    float g6 = (float)Math.Sqrt(Math.Pow(Comg1,2) + Math.Pow(g4, 2));
                    bz.g6 = g6;

                    float g7 = (float)Math.Sqrt(Math.Pow(g3, 2) + Math.Pow(g5, 2));
                    bz.g7 = g7;

                    BzDic.Add(weaItem.SWorkConditionName, bz);
                }
            }
        }


        /// <summary>
        /// 计算给定档距下各个工况的应力
        /// </summary>
        /// <param name="span"></param>
        /// <returns></returns>
        protected Dictionary<string, float> CalFTable(float span)
        {
            if (bGrd == 0)
            {
                EffectPara = SideParas.IndEffectPara;
                SafePara = SideParas.IndSafePara;
                AvePara =SideParas.IndAnPara;
            }
            else if(bGrd == 1)
            {
                EffectPara = SideParas.GrdEffectPara;
                SafePara = SideParas.GrdSafePara;
                AvePara = SideParas.GrdAnPara;
            }

            else
            {
                OPGWEffectPara = SideParas.OPGWEffectPara;
                //OPGW安全系数
                OPGWSafePara = SideParas.OPGWSafePara;
                //OPGW年均系数 
                OPGWAnPara = SideParas.OPGWAnPara;     
            }

            //最大允许应力   
            var maxPerFor = Fore * CommonParas.NewPerPara / SafePara / Sec;
            //年均应力
            var avePerFor = Fore * CommonParas.NewPerPara * AvePara / Sec;

            // 需要确认Fore 是不是计算破断力N  这两个参数和上面两个参数是不是一回事
            CtrlYL = (float)Math.Round(Fore / Sec / 9.80665 * EffectPara / SafePara, 3);
            AvaYL = (float)Math.Round(Fore / Sec / 9.80665 * EffectPara * AvePara / 100, 3);

            //控制工况计算

            //存储中间计算结果
            Dictionary<string, float> YLCalDic = new Dictionary<string, float>();
            Dictionary<string, float> ForCalDic = new Dictionary<string, float>();

            foreach (var nameWd in WeatherParas.NameOfCtrWkCdt)
            {
                float calFor = nameWd != "平均气温" ? maxPerFor : avePerFor;

                //均采用综合比载计算
                float calBz = BzDic[nameWd].g6;
                float temVal = (float)Convert.ToDecimal(WeatherParas.WeathComm.Where(item => item.SWorkConditionName == nameWd).First().STemperature);
                float fm = (float)((Elas * Math.Pow(calBz, 2) * Math.Pow(span, 2) / 24 / Math.Pow(calFor, 2)) - (calFor + Coef * Elas * temVal));
                YLCalDic.Add(nameWd, fm);
                //存储每个工况初始控制应力取值
                ForCalDic.Add(nameWd, calFor);
            }

            //确定控制工况
            //存储各个工况应力,采用字典模式
            Dictionary<string, float> ForDic = new Dictionary<string, float>();

            //CtrName sorted(YLCalDic.items(), key = lambda kv: kv[1], reverse = True)[0]
            string CtrName = "";
            float CtrFm = 0;

            ForDic.Add(CtrName, CtrFm);

            //将控制工况存储下来
            CtrNaSave = CtrName;

            foreach(var wd in WeatherParas.WeathComm)
            {
                if (wd.SWorkConditionName == CtrName)
                    continue;

                float calBz = BzDic[wd.SWorkConditionName].g6;
                float temVal = WeatherParas.WeathComm.Where(item => item.SWorkConditionName == wd.SWorkConditionName).First().Temperature;
                float temA = CtrFm + Coef * Elas * temVal;
                float temB = (float)(Elas * Math.Pow(calBz, 2) * Math.Pow(span, 2)) / 24;
                float calFor = ElectricalCalsToolBox.caculateCurDelta(temA, temB);
                ForDic.Add(wd.SWorkConditionName, calFor);
            }

            return ForDic;
        }

    }
}
