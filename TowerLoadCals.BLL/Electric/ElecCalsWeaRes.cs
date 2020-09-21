using System.Collections.Generic;
using System.Linq;

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

        /// <summary>
        /// 
        /// </summary>
        public List<string> NameOfWkCalsIndStrain
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
        /// 
        /// </summary>
        public List<string> NameOfWkCalsGrdStrain
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

        public List<string> NameOfWkStainForStru
        {
            get
            {
                return new List<string> { "最大风速","最大覆冰","最低气温","安装情况",
                "未断线","断线","不均匀冰I","验算冰","不均匀冰II","验算不均匀冰I","验算不均匀冰II","不均匀风","顺线路外角侧45风","逆线路外角侧45风","顺线路内角侧45风","逆线路内角侧45风",
                };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<string> NameOfWkCalsIndHang
        {
            get
            {
                return new List<string> { "最低气温","最大覆冰","平均气温","安装情况","最高气温", "验算冰",
                "换算最大风速","顺线路外角侧45风","逆线路外角侧45风","顺线路内角侧45风","逆线路内角侧45风",
                 "断线",  "不均匀冰满冰","不均匀冰20%","断线开断","不均匀冰开断",
                 "换算不均匀风","断线满冰","安装情况降温"};
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<string> NameOfWkCalsGrdHang
        {
            get
            {
                return new List<string> { "最低气温","最大覆冰","平均气温","安装情况","最高气温", "验算冰",
                "断线","不均匀冰满冰","不均匀冰20%","断线开断","不均匀冰开断",
                "换算最大风速","顺线路外角侧45风","逆线路外角侧45风","顺线路内角侧45风","逆线路内角侧45风",
                "断线(导线+5mm)","地线覆冰","不均匀冰满冰(导线+5mm)","不均匀冰20%(导线+5mm)",
                "验算冰(导线+5mm)", "断线开断(导线+5mm)","不均匀冰开断(导线+5mm)",
                "换算不均匀风","断线满冰","断线满冰(导线+5mm)","安装情况降温",
                };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<string> NameOfWkHangForStru
        {
            get
            {
                return new List<string> { "最大风速","最大覆冰","最低气温","安装情况",
                "未断线","断线","不均匀冰I","验算冰"
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
            var temp1 = WeathComm.Where(item => item.Name == "最大风速").FirstOrDefault();

            if (temp1 == null)
                temp1 = new ElecCalsWorkCondition(); 
            WeathComm.Add(new ElecCalsWorkCondition
            {
                //在计算中相当于Eecel"最大风速"
                Name = "换算最大风速",
                IceThickness = temp1.IceThickness,
                Temperature = temp1.Temperature,
                WindSpeed = ElecCalsToolBox.WindExChange(temp1.WindSpeed, aveHei, terrainPara),
                BaseWindSpeed = temp1.WindSpeed,
            });
                
            var temp = WeathComm.Where(item => item.Name == "不均匀风").FirstOrDefault();
            if (temp == null)
                temp = new ElecCalsWorkCondition();
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


        public void ConverWind(double aveHei, char terType)
        {
            var temp1 = WeathComm.Where(item => item.Name == "最大风速").FirstOrDefault();
            if (temp1 == null)
                temp1 = new ElecCalsWorkCondition();
            WeathComm.Add(new ElecCalsWorkCondition
            {
                //在计算中相当于Eecel"最大风速"
                Name = "换算最大风速",
                IceThickness = temp1.IceThickness,
                Temperature = temp1.Temperature,
                WindSpeed = ElecCalsToolBox.WindExChange(temp1.WindSpeed, aveHei, terType),
                BaseWindSpeed = temp1.WindSpeed,
            });


            var temp = WeathComm.Where(item => item.Name == "不均匀风").FirstOrDefault();
            if (temp == null)
                temp = new ElecCalsWorkCondition();
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

        /// <summary>
        /// 换算45风的风速
        /// </summary>
        /// <param name="angle"></param>
        public void ConverWind45(bool isBackSide,double angle)
        {
            var temp = WeathComm.Where(item => item.Name == "换算最大风速").FirstOrDefault();

            var wind1 = ElecCalsToolBox.Wind45ExChange1(temp == null ? 0 : temp.WindSpeed, angle);
            var wind2 = ElecCalsToolBox.Wind45ExChange2(temp == null ? 0 : temp.WindSpeed, angle);

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


        /// <summary>
        /// 增加地线覆冰计算，用于计算地线覆冰荷
        /// </summary>
        public void AddGrdWeath()
        {
            var temp = WeathComm.Where(item => item.Name == "最大覆冰").FirstOrDefault();

            if (temp == null)
                temp = new ElecCalsWorkCondition();

            WeathComm.Add(new ElecCalsWorkCondition
            {
                Name = "地线覆冰",
                IceThickness = (temp.IceThickness > 0 ? temp.IceThickness + 5 : temp.IceThickness),
                Temperature = temp.Temperature,
                WindSpeed = temp.WindSpeed,
            });
        }

        /// <summary>
        /// 增加其他需要应用的工况
        /// </summary>
        public void AddOtherGk()
        {
            var temp = WeathComm.Where(item => item.Name == "最大覆冰").FirstOrDefault();

            WeathComm.Add(new ElecCalsWorkCondition
            {
                Name = "覆冰无风",
                IceThickness = temp == null ? 0 : temp.IceThickness,
                WindSpeed = 0,
                Temperature = temp == null ? 0 : temp.Temperature,
            });
        }

        /// <summary>
        /// 增加断线工况-耐张塔
        /// </summary>
        /// <param name="bGrd">是否是地线：1 地线； 0 导线</param>
        /// <param name="grdIceUnbaPara">地线不平衡张力取值，1:轻冰区不考虑增加5mm，2：重冰区增加5mm</param>
        /// <param name="dia">直径</param>
        /// <param name="breakIceCover">断线覆冰率</param>
        public void AddBreakGKStrain(string name, int bGrd,  int grdIceUnbaPara, double dia, double breakIceCover)
        {
            double thickness;
            ElecCalsWorkCondition iceWkCdt;

            var elecCalsSpec = GlobalInfo.GetInstance().GetElecCalsSpecParas();
            if (elecCalsSpec == null)
                return ;

            if (bGrd == 1 && grdIceUnbaPara == 2)
            {
                //地线不平衡张力考虑+5m时，要找到地线覆冰工况
                iceWkCdt = WeathComm.Where(item => item.Name == "地线覆冰").FirstOrDefault();
                if (iceWkCdt == null)
                    return;
                thickness = ElecCalsToolBox.IceThicknessExChange(dia, breakIceCover, iceWkCdt.IceThickness);
            }
            else
            {
                //导线以及地线不平衡张力不考虑+5m 要找到最大覆冰工况
                iceWkCdt = WeathComm.Where(item => item.Name == "最大覆冰").FirstOrDefault();
                if (iceWkCdt == null)
                    return;
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
        /// 增加断线工况-悬垂塔
        /// </summary>
        /// <param name="bGrd">是否是地线：1 地线； 0 导线</param>
        /// <param name="grdIceUnbaPara">地线不平衡张力取值，1:轻冰区不考虑增加5mm，2：重冰区增加5mm</param>
        /// <param name="dia">直径</param>
        /// <param name="breakIceCover">断线覆冰率</param>
        public void AddBreakGKHang(bool bAdd5mm, double dia, double breakIceCover1, double breakIceCover2)
        {
            var elecCalsSpec = GlobalInfo.GetInstance().GetElecCalsSpecParas();
            if (elecCalsSpec == null)
                return;

            ElecCalsWorkCondition iceWkCdt;

            if (bAdd5mm)
            {
                iceWkCdt = WeathComm.Where(item => item.Name == "地线覆冰").FirstOrDefault();
            }
            else
            {
                iceWkCdt = WeathComm.Where(item => item.Name == "最大覆冰").FirstOrDefault();
            }
            double iceThickness = iceWkCdt == null ? 0 : iceWkCdt.IceThickness;

            double thickness1 = ElecCalsToolBox.IceThicknessExChange(dia, breakIceCover1, iceThickness);
            double thickness2 = ElecCalsToolBox.IceThicknessExChange(dia, breakIceCover2, iceThickness);

            WeathComm.Add(new ElecCalsWorkCondition()
            {
                Name = bAdd5mm ? "断线(导线+5mm)" : "断线",
                IceThickness = thickness1,
                Temperature = elecCalsSpec.BreakTemp,
                WindSpeed = elecCalsSpec.BreakWind,
            });

            WeathComm.Add(new ElecCalsWorkCondition()
            {
                Name = bAdd5mm ? "断线开断(导线+5mm)" : "断线开断",
                IceThickness = thickness2,
                Temperature = elecCalsSpec.BreakTemp,
                WindSpeed = elecCalsSpec.BreakWind,
            });

            WeathComm.Add(new ElecCalsWorkCondition()
            {
                Name = bAdd5mm ? "断线满冰(导线+5mm)" : "断线满冰",
                IceThickness = iceThickness,
                Temperature = iceWkCdt == null ? 0 : iceWkCdt.Temperature,
                WindSpeed = 0,
            });
        }

        /// <summary>
        /// 增加不均匀冰I、II工况
        /// </summary>
        /// <param name="bAdd5mm">是否增加5mm/param>
        /// <param name="dia">直径</param>
        /// <param name="iceCover1">不均匀覆冰率I</param>
        /// <param name="iceCover2">不均匀覆冰率I</param>
        public void AddUnevenIceGKStrain(bool bAdd5mm, double dia, bool isBackSide, double iceCover1, double iceCover2)
        {
            ElecCalsWorkCondition iceWkCdt;

            var elecCalsSpec = GlobalInfo.GetInstance().GetElecCalsSpecParas();
            if (elecCalsSpec == null)
                return;

            if (bAdd5mm)
            {
                //增加5mm要地线覆冰工况
                iceWkCdt = WeathComm.Where(item => item.Name == "地线覆冰").FirstOrDefault();
            }
            else
            {
                //不增加5mm要最大覆冰工况
                iceWkCdt = WeathComm.Where(item => item.Name == "最大覆冰").FirstOrDefault();
            }

            double thickness1 = ElecCalsToolBox.IceThicknessExChange(dia, iceCover1, iceWkCdt == null ? 0 : iceWkCdt.IceThickness);
            double thickness2 = ElecCalsToolBox.IceThicknessExChange(dia, iceCover2, iceWkCdt == null ? 0 : iceWkCdt.IceThickness);

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


        public void AddUnevenIceGKHang(bool bAdd5mm, double dia, double iceCover1, double iceCover2)
        {
            ElecCalsWorkCondition iceWkCdt;

            var elecCalsSpec = GlobalInfo.GetInstance().GetElecCalsSpecParas();
            if (elecCalsSpec == null)
                return;

            if (bAdd5mm)
            {
                //增加5mm要地线覆冰工况
                iceWkCdt = WeathComm.Where(item => item.Name == "地线覆冰").FirstOrDefault();
            }
            else
            {
                //不增加5mm要最大覆冰工况
                iceWkCdt = WeathComm.Where(item => item.Name == "最大覆冰").FirstOrDefault();
            }


            double thickness = iceWkCdt == null ? 0 : iceWkCdt.IceThickness;
            double thickness1 = ElecCalsToolBox.IceThicknessExChange(dia, iceCover1, thickness);
            double thickness2 = ElecCalsToolBox.IceThicknessExChange(dia, iceCover2, thickness);

            WeathComm.AddRange(new List<ElecCalsWorkCondition>()
            {
                new ElecCalsWorkCondition()
                {
                    Name = bAdd5mm ? "不均匀冰满冰(导线+5mm)" : "不均匀冰满冰",
                    IceThickness = thickness,
                    Temperature = elecCalsSpec.UnevenIceTemp,
                    WindSpeed = elecCalsSpec.UnevenIceWind,
                },
                new ElecCalsWorkCondition()
                {
                    Name = bAdd5mm ? "不均匀冰20%(导线+5mm)" : "不均匀冰20%",
                    IceThickness = thickness1,
                    Temperature = elecCalsSpec.UnevenIceTemp,
                    WindSpeed = elecCalsSpec.UnevenIceWind,
                },
                new ElecCalsWorkCondition()
                {
                    Name = bAdd5mm ? "不均匀冰开断(导线+5mm)" : "不均匀冰开断",
                    IceThickness = thickness2,
                    Temperature = elecCalsSpec.UnevenIceTemp,
                    WindSpeed = elecCalsSpec.UnevenIceWind,
                },
            });
        }



        /// <summary>
        /// 更新地线的验算工况的冰厚
        /// </summary>
        public void AddCkeckGKIcr5mm(string towerType)
        {
            var chekWkCdt = WeathComm.Where(item => item.Name == "验算冰").FirstOrDefault();
            if (chekWkCdt == null)
                chekWkCdt = new ElecCalsWorkCondition();
            WeathComm.Add(new ElecCalsWorkCondition
            {
                Name = "验算冰(导线+5mm)",
                IceThickness = chekWkCdt.IceThickness + 5,
                Temperature = chekWkCdt.Temperature,
                WindSpeed = chekWkCdt.WindSpeed,
            });

            var iceWkCdt = WeathComm.Where(item => item.Name == "地线覆冰").FirstOrDefault();
            if (iceWkCdt == null)
                iceWkCdt = new ElecCalsWorkCondition();
            WeathComm.Add(new ElecCalsWorkCondition
            {
                Name = "覆冰无风+5",
                IceThickness = iceWkCdt.IceThickness,
                Temperature = iceWkCdt.Temperature,
                WindSpeed = 0,
            });

            if (towerType == "悬垂塔")
                return;

            var chekIWkCdt = WeathComm.Where(item => item.Name == "验算不均匀冰I").FirstOrDefault();
            if (chekIWkCdt == null)
                chekIWkCdt = new ElecCalsWorkCondition();
            WeathComm.Add(new ElecCalsWorkCondition
            {
                Name = "验算不均匀冰I(导线+5mm)",
                IceThickness = chekIWkCdt.IceThickness + 5,
                Temperature = chekIWkCdt.Temperature,
                WindSpeed = chekIWkCdt.WindSpeed,
            });

            var chekIIWkCdt = WeathComm.Where(item => item.Name == "验算不均匀冰II").FirstOrDefault();
            if (chekIIWkCdt == null)
                chekIIWkCdt = new ElecCalsWorkCondition();
            WeathComm.Add(new ElecCalsWorkCondition
            {
                Name = "验算不均匀冰II(导线+5mm)",
                IceThickness = chekIIWkCdt.IceThickness + 5,
                Temperature = chekIIWkCdt.Temperature,
                WindSpeed = chekIIWkCdt.WindSpeed,
            });
        }

        public void AddInstallColdGk(double decrTem)
        {
            var wkCdt = WeathComm.Where(item => item.Name == "安装情况").FirstOrDefault();
            if (wkCdt == null)
                wkCdt = new ElecCalsWorkCondition();
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
