using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerLoadCals.Mode;

namespace TowerLoadCals.BLL.Electric
{
    public class ElecCalsWeaRes
    {

        public int TotalConNum { get; set; }

        public string Name { get; set; }

        public List<ElecCalsWorkCondition> WeathComm { get; set; }

        public int ID { get; set; }

        public List<string> NameOfCtrWkCdt
        {
            get { return new List<string> { "最大风速", "最低气温", "最大覆冰", "平均气温" }; }
        }

        public List<string> NameOfWkCdt
        {
            get { return new List<string> { "最大风速","最低气温","最大覆冰","平均气温","最高气温","安装情况","不均匀风",
                "不均匀冰I","不均匀冰II", "验算冰","验算不均匀冰I","验算不均匀冰II","不均匀风",
                "顺线路外角侧45风","逆线路外角侧45风","顺线路内角侧45风","逆线路内角侧45风"
            }; }
        }

        /// <summary>
        /// 用于打印日志
        /// </summary>
        public List<string> NameOfWkCalsInd
        {
            get
            {
                return new List<string> { "最低气温","最大覆冰","平均气温","安装情况","最高气温", "验算冰",
                "换算最大风速","顺线路外角侧45风","逆线路外角侧45风","顺线路内角侧45风","逆线路内角侧45风",
                 "断线","不均匀冰I","不均匀冰II","验算不均匀冰I","验算不均匀冰II",
                 "换算不均匀风","覆冰无风","安装情况降温"};
            }
        }

        /// <summary>
        /// 用于打印日志
        /// </summary>
        public List<string> NameOfWkCalsGrd
        {
            get
            {
                return new List<string> { "最低气温","最大覆冰","平均气温","安装情况","最高气温", "验算冰",
                "断线","不均匀冰I","不均匀冰II","验算不均匀冰I","验算不均匀冰II",
                "换算最大风速","顺线路外角侧45风","逆线路外角侧45风","顺线路内角侧45风","逆线路内角侧45风",
                "断线(导线+5mm)","地线覆冰","不均匀冰I(导线+5mm)","不均匀冰II(导线+5mm)",
                "验算冰(导线+5mm)", "验算不均匀冰I(导线+5mm)","验算不均匀冰II(导线+5mm)",
                "换算不均匀风","覆冰无风","覆冰无风+5","安装情况降温",
                };
            }
        }

        public ElecCalsWeaRes()
        {

        }

        public ElecCalsWeaRes(string name, int id)
        {
            Name = name;
            ID = id;

            WeathComm = new List<ElecCalsWorkCondition>();
        }

        public void InsertGK(List<ElecCalsWorkCondition> iniWeather)
        {
            WeathComm.AddRange(iniWeather);
        }

        /// <summary>
        /// 换算最大风速值到平均高度
        /// </summary>
        public void ConverWind(double aveHei, double terrainPara)
        {
            if(WeathComm.Where(item => item.Name == "最大风速").Count() > 0)
            {
                var temp = WeathComm.Where(item => item.Name == "最大风速").First();
                WeathComm.Add(new ElecCalsWorkCondition
                {
                    //在计算中相当于Eecel"最大风速"
                    Name = "换算最大风速",
                    IceThickness = temp.IceThickness,
                    Temperature = temp.Temperature,
                    WindSpeed = ElecCalsToolBox.WindExChange(temp.WindSpeed, aveHei, terrainPara),
                    BaseWindSpeed = temp.WindSpeed,
                });
            }

            if (WeathComm.Where(item => item.Name == "不均匀风").Count() > 0)
            {
                var temp = WeathComm.Where(item => item.Name == "不均匀风").First();
                WeathComm.Add(new ElecCalsWorkCondition
                {
                    //在计算中相当于Eecel"不均匀风"
                    Name = "换算不均匀风",
                    IceThickness = temp.IceThickness,
                    Temperature = temp.Temperature,
                    WindSpeed = ElecCalsToolBox.WindExChange(temp.WindSpeed, aveHei, terrainPara),
                    BaseWindSpeed = temp.WindSpeed,
                });
            }
        }


        public void ConverWind(double aveHei, char terType)
        {
            if (WeathComm.Where(item => item.Name == "最大风速").Count() > 0)
            {
                var temp = WeathComm.Where(item => item.Name == "最大风速").First();
                WeathComm.Add(new ElecCalsWorkCondition
                {
                    //在计算中相当于Eecel"最大风速"
                    Name = "换算最大风速",
                    IceThickness = temp.IceThickness,
                    Temperature = temp.Temperature,
                    WindSpeed = ElecCalsToolBox.WindExChange(temp.WindSpeed, aveHei, terType),
                    BaseWindSpeed = temp.WindSpeed,
                });
            }

            if (WeathComm.Where(item => item.Name == "不均匀风").Count() > 0)
            {
                var temp = WeathComm.Where(item => item.Name == "不均匀风").First();
                WeathComm.Add(new ElecCalsWorkCondition
                {
                    //在计算中相当于Eecel"不均匀风"
                    Name = "换算不均匀风",
                    IceThickness = temp.IceThickness,
                    Temperature = temp.Temperature,
                    WindSpeed = ElecCalsToolBox.WindExChange(temp.WindSpeed, aveHei, terType),
                    BaseWindSpeed = temp.WindSpeed,
                });
            }
        }

        /// <summary>
        /// 换算45风的风速
        /// </summary>
        /// <param name="angle"></param>
        public void ConverWind45(bool isBackSide,double angle)
        {
            if (WeathComm.Where(item => item.Name == "换算最大风速").Count() > 0)
            {
                var temp = WeathComm.Where(item => item.Name == "换算最大风速").First();

                var wind1 = ElecCalsToolBox.Wind45ExChange1(temp.WindSpeed, angle);
                var wind2 = ElecCalsToolBox.Wind45ExChange2(temp.WindSpeed, angle);

                WeathComm.AddRange(new List<ElecCalsWorkCondition>()
                {
                    new ElecCalsWorkCondition()
                    {
                        Name = "顺线路外角侧45风",
                        IceThickness = temp.IceThickness,
                        Temperature = temp.Temperature,
                        WindSpeed = isBackSide ? wind1 : wind2,
                        BaseWindSpeed = temp.BaseWindSpeed,
                    },
                    new ElecCalsWorkCondition()
                    {
                        Name = "逆线路内角侧45风",
                        IceThickness = temp.IceThickness,
                        Temperature = temp.Temperature,
                        WindSpeed = isBackSide ? wind1 : wind2,
                        BaseWindSpeed = temp.BaseWindSpeed,
                    },
                });

                
                WeathComm.AddRange(new List<ElecCalsWorkCondition>()
                {
                    new ElecCalsWorkCondition()
                    {
                        Name = "逆线路外角侧45风",
                        IceThickness = temp.IceThickness,
                        Temperature = temp.Temperature,
                        WindSpeed = !isBackSide ? wind1 : wind2,
                        BaseWindSpeed = temp.BaseWindSpeed,
                    },
                    new ElecCalsWorkCondition()
                    {
                        Name = "顺线路内角侧45风",
                        IceThickness = temp.IceThickness,
                        Temperature = temp.Temperature,
                        WindSpeed = !isBackSide ? wind1 : wind2,
                        BaseWindSpeed = temp.BaseWindSpeed,
                    },
                });
            }
        }


        /// <summary>
        /// 增加地线覆冰计算，用于计算地线覆冰荷
        /// </summary>
        public void AddGrdWeath()
        {
            if (WeathComm.Where(item => item.Name == "最大覆冰").Count() > 0)
            {
                var temp = WeathComm.Where(item => item.Name == "最大覆冰").First();

                WeathComm.Add(new ElecCalsWorkCondition
                {
                    Name = "地线覆冰",
                    IceThickness = (temp.IceThickness > 0 ? temp.IceThickness + 5 : temp.IceThickness),
                    Temperature = temp.Temperature,
                    WindSpeed = temp.WindSpeed,
                });
            }
        }

        /// <summary>
        /// 增加其他需要应用的工况
        /// </summary>
        public void AddOtherGk()
        {
            if (WeathComm.Where(item => item.Name == "最大覆冰").Count() > 0)
            {
                var temp = WeathComm.Where(item => item.Name == "最大覆冰").First();

                WeathComm.Add(new ElecCalsWorkCondition
                {
                    Name = "覆冰无风",
                    IceThickness = temp.IceThickness,
                    WindSpeed = 0,
                    Temperature = temp.Temperature,
                });
            }
        }

        /// <summary>
        /// 增加断线工况
        /// </summary>
        /// <param name="bGrd">是否是地线：1 地线； 0 导线</param>
        /// <param name="grdIceUnbaPara">地线不平衡张力取值，1:轻冰区不考虑增加5mm，2：重冰区增加5mm</param>
        /// <param name="dia">直径</param>
        /// <param name="breakIceCover">断线覆冰率</param>
        public void AddBreakGK(string name, int bGrd,  int grdIceUnbaPara, double dia, double breakIceCover)
        {
            double thickness;
            ElecCalsWorkCondition iceWkCdt;

            var elecCalsSpec = GlobalInfo.GetInstance().GetElecCalsSpecParas();
            if (elecCalsSpec == null)
                return ;

            if (bGrd == 1 && grdIceUnbaPara == 2)
            {
                //地线不平衡张力考虑+5m时，要找到地线覆冰工况
                if (WeathComm.Where(item => item.Name == "地线覆冰").Count() <= 0)
                    return;
                iceWkCdt = WeathComm.Where(item => item.Name == "地线覆冰").First();
                thickness = ElecCalsToolBox.IceThicknessExChange(dia, breakIceCover, iceWkCdt.IceThickness);
            }
            else
            {
                //导线以及地线不平衡张力不考虑+5m 要找到最大覆冰工况
                if (WeathComm.Where(item => item.Name == "最大覆冰").Count() <= 0)
                    return;
                iceWkCdt = WeathComm.Where(item => item.Name == "最大覆冰").First();
                thickness = ElecCalsToolBox.IceThicknessExChange(dia, breakIceCover, iceWkCdt.IceThickness);
            }

            WeathComm.Add(new ElecCalsWorkCondition()
            {
                Name = name,
                IceThickness = thickness,
                Temperature = elecCalsSpec.BreakTemp,
                WindSpeed = elecCalsSpec.BreakWind,
            });
        }

        /// <summary>
        /// 增加不均匀冰I、II工况
        /// </summary>
        /// <param name="bAdd5mm">是否增加5mm/param>
        /// <param name="dia">直径</param>
        /// <param name="iceCover1">不均匀覆冰率I</param>
        /// <param name="iceCover2">不均匀覆冰率I</param>
        public void AddUnevenIceGK(bool bAdd5mm, double dia, bool isBackSide, double iceCover1, double iceCover2)
        {
            ElecCalsWorkCondition iceWkCdt;

            var elecCalsSpec = GlobalInfo.GetInstance().GetElecCalsSpecParas();
            if (elecCalsSpec == null)
                return;

            if (bAdd5mm)
            {
                //增加5mm要地线覆冰工况
                if (WeathComm.Where(item => item.Name == "地线覆冰").Count() <= 0)
                    return;
                iceWkCdt = WeathComm.Where(item => item.Name == "地线覆冰").First();
            }
            else
            {
                //不增加5mm要最大覆冰工况
                if (WeathComm.Where(item => item.Name == "最大覆冰").Count() <= 0)
                    return;
                iceWkCdt = WeathComm.Where(item => item.Name == "最大覆冰").First();
            }

            double thickness1 = ElecCalsToolBox.IceThicknessExChange(dia, iceCover1, iceWkCdt.IceThickness);
            double thickness2 = ElecCalsToolBox.IceThicknessExChange(dia, iceCover2, iceWkCdt.IceThickness);

            WeathComm.AddRange(new List<ElecCalsWorkCondition>()
            {
                new ElecCalsWorkCondition()
                {
                    Name = bAdd5mm ? "不均匀冰I(导线+5mm)" : "不均匀冰I",
                    IceThickness = isBackSide ? thickness1 : thickness2,
                    Temperature = elecCalsSpec.UnevenIceTemp,
                    WindSpeed = elecCalsSpec.UnevenIceWind,
                },
                new ElecCalsWorkCondition()
                {
                    Name = bAdd5mm ? "不均匀冰II(导线+5mm)" : "不均匀冰II",
                    IceThickness = !isBackSide ? thickness1 : thickness2,
                    Temperature = elecCalsSpec.UnevenIceTemp,
                    WindSpeed = elecCalsSpec.UnevenIceWind,
                },
            });
        }


        /// <summary>
        /// 更新地线的验算工况的冰厚
        /// </summary>
        public void AddCkeckGKIcr5mm()
        {
            if(WeathComm.Where(item => item.Name == "验算冰").Count() > 0)
            {
                var chekWkCdt = WeathComm.Where(item => item.Name == "验算冰").First();
                WeathComm.Add(new ElecCalsWorkCondition
                {
                    Name = "验算冰(导线+5mm)",
                    IceThickness = chekWkCdt.IceThickness + 5,
                    Temperature = chekWkCdt.Temperature,
                    WindSpeed = chekWkCdt.WindSpeed,
                });
            }

            if (WeathComm.Where(item => item.Name == "验算不均匀冰I").Count() > 0)
            {
                var chekIWkCdt = WeathComm.Where(item => item.Name == "验算不均匀冰I").First();
                WeathComm.Add(new ElecCalsWorkCondition
                {
                    Name = "验算不均匀冰I(导线+5mm)",
                    IceThickness = chekIWkCdt.IceThickness + 5,
                    Temperature = chekIWkCdt.Temperature,
                    WindSpeed = chekIWkCdt.WindSpeed,
                });
            }

            if (WeathComm.Where(item => item.Name == "验算不均匀冰II").Count() > 0)
            {
                var chekIIWkCdt = WeathComm.Where(item => item.Name == "验算不均匀冰II").First();
                WeathComm.Add(new ElecCalsWorkCondition
                {
                    Name = "验算不均匀冰II(导线+5mm)",
                    IceThickness = chekIIWkCdt.IceThickness + 5,
                    Temperature = chekIIWkCdt.Temperature,
                    WindSpeed = chekIIWkCdt.WindSpeed,
                });
            }

            if (WeathComm.Where(item => item.Name == "地线覆冰").Count() > 0)
            {
                var iceWkCdt = WeathComm.Where(item => item.Name == "地线覆冰").First();
                WeathComm.Add(new ElecCalsWorkCondition
                {
                    Name = "覆冰无风+5",
                    IceThickness = iceWkCdt.IceThickness,
                    Temperature = iceWkCdt.Temperature,
                    WindSpeed = 0,
                });
        }
        }

        public void AddInstallColdGk(double decrTem)
        {
            var wkCdt = WeathComm.Where(item => item.Name == "安装情况").First();
            if (wkCdt == null)
                return;
            WeathComm.Add(new ElecCalsWorkCondition
            {
                Name = "安装情况降温",
                IceThickness = wkCdt.IceThickness,
                Temperature = wkCdt.Temperature - decrTem,
                WindSpeed = wkCdt.WindSpeed,
            });
        }
        


    }
}
