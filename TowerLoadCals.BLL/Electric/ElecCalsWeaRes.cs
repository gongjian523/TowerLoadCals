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

        public List<WorkCondition> WeathComm { get; set; }

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


        public ElecCalsWeaRes(string name, int id)
        {
            Name = name;
            ID = id;

            WeathComm = new List<WorkCondition>();
        }

        public void InsertGK(List<WorkCondition> iniWeather)
        {
            WeathComm.AddRange(iniWeather);
        }

        /// <summary>
        /// 换算最大风速值到平均高度
        /// </summary>
        public void ConverWind(float aveHei, char terType)
        {
            if(WeathComm.Where(item => item.SWorkConditionName == "最大风速").Count() > 0)
            {
                var temp = WeathComm.Where(item => item.SWorkConditionName == "最大风速").First();
                WeathComm.Add(new WorkCondition
                {
                    //在计算中相当于Eecel"最大风速"
                    SWorkConditionName = "换算最大风速",
                    IceThickness = temp.IceThickness,
                    Temperature = temp.Temperature,
                    WindSpeed = ElecCalsToolBox.WindExChange(temp.WindSpeed, aveHei, terType),
                    BaseWindSpeed = temp.WindSpeed,
                });
            }

            if (WeathComm.Where(item => item.SWorkConditionName == "不均匀风").Count() > 0)
            {
                var temp = WeathComm.Where(item => item.SWorkConditionName == "不均匀风").First();
                WeathComm.Add(new WorkCondition
                {
                    //在计算中相当于Eecel"不均匀风"
                    SWorkConditionName = "换算不均匀风",
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
        /// <param name="WeathComm"></param>
        /// <param name="angle"></param>
        public void ConverWind45(float angle)
        {
            if (WeathComm.Where(item => item.SWorkConditionName == "换算最大风速").Count() > 0)
            {
                var temp = WeathComm.Where(item => item.SWorkConditionName == "换算最大风速").First();

                var wind1 = ElecCalsToolBox.Wind45ExChange1(temp.WindSpeed, angle);
                WeathComm.AddRange( new List<WorkCondition>()
                {   
                    new WorkCondition()
                    {
                        SWorkConditionName = "顺线路外角侧45风",
                        IceThickness = temp.IceThickness,
                        Temperature = temp.Temperature,
                        WindSpeed = wind1,
                        BaseWindSpeed = temp.BaseWindSpeed,
                    },
                    new WorkCondition()
                    {
                        SWorkConditionName = "逆线路内角侧45风",
                        IceThickness = temp.IceThickness,
                        Temperature = temp.Temperature,
                        WindSpeed = wind1,
                        BaseWindSpeed = temp.BaseWindSpeed,
                    },
                });

                var wind2 = ElecCalsToolBox.Wind45ExChange2(temp.WindSpeed, angle);
                WeathComm.AddRange(new List<WorkCondition>()
                {
                    new WorkCondition()
                    {
                        SWorkConditionName = "逆线路外角侧45风",
                        IceThickness = temp.IceThickness,
                        Temperature = temp.Temperature,
                        WindSpeed = wind2,
                        BaseWindSpeed = temp.BaseWindSpeed,
                    },
                    new WorkCondition()
                    {
                        SWorkConditionName = "顺线路内角侧45风",
                        IceThickness = temp.IceThickness,
                        Temperature = temp.Temperature,
                        WindSpeed = wind2,
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
            if (WeathComm.Where(item => item.SWorkConditionName == "最大覆冰").Count() > 0)
            {
                var temp = WeathComm.Where(item => item.SWorkConditionName == "最大覆冰").First();

                WeathComm.Add(new WorkCondition
                {
                    SWorkConditionName = "地线覆冰",
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
            if (WeathComm.Where(item => item.SWorkConditionName == "最大覆冰").Count() > 0)
            {
                var temp = WeathComm.Where(item => item.SWorkConditionName == "最大覆冰").First();

                WeathComm.Add(new WorkCondition
                {
                    SWorkConditionName = "覆冰无风",
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
        /// <param name="bAdd5mm">地线不平衡张力考虑+5mm： 1 增加； 0 不增加</param>
        /// <param name="dia">直径</param>
        /// <param name="breakIceCover">断线覆冰率1</param>
        /// <param name="breakIceCover">断线覆冰率2 ?? 这个值怎么区还有待确认</param>
        public void AddBreakGK(int bGrd,  int bAdd5mm, float dia, float breakIceCover1, float breakIceCover2)
        {
            float thickness;
            WorkCondition iceWkCdt;

            if (bGrd == 1 && bAdd5mm == 1)
            {
                //地线不平衡张力考虑+5m时，要找到地线覆冰工况
                if (WeathComm.Where(item => item.SWorkConditionName == "地线覆冰").Count() <= 0)
                    return;
                iceWkCdt = WeathComm.Where(item => item.SWorkConditionName == "地线覆冰").First();
                thickness = ElecCalsToolBox.IceThicknessExChange(dia, breakIceCover1, iceWkCdt.IceThickness);
            }
            else
            {
                //导线以及地线不平衡张力不考虑+5m 要找到最大覆冰工况
                if (WeathComm.Where(item => item.SWorkConditionName == "最大覆冰").Count() <= 0)
                    return;
                iceWkCdt = WeathComm.Where(item => item.SWorkConditionName == "最大覆冰").First();
                thickness = ElecCalsToolBox.IceThicknessExChange(dia, breakIceCover2, iceWkCdt.IceThickness);
            }

            WeathComm.Add(new WorkCondition()
            {
                SWorkConditionName = "断线",
                IceThickness = thickness,
                Temperature = iceWkCdt.Temperature,
                WindSpeed = iceWkCdt.WindSpeed,
            });
        }

        /// <summary>
        /// 增加不均匀冰I、II工况
        /// </summary>
        /// <param name="bGrd">是否是地线：1 地线； 0 导线</param>
        /// <param name="dia">直径</param>
        /// <param name="iceCover1">不均匀覆冰率I</param>
        /// <param name="iceCover2">不均匀覆冰率I</param>
        public void AddUnevenIceGK(int bGrd, float dia, float iceCover1, float iceCover2)
        {
            WorkCondition iceWkCdt;

            //温度和风速来做验算冰工况
            if (WeathComm.Where(item => item.SWorkConditionName == "验算冰").Count() <= 0)
                return;

            var checkWkCdt = WeathComm.Where(item => item.SWorkConditionName == "验算冰").First();
            float temp = checkWkCdt.Temperature;
            float wind = checkWkCdt.WindSpeed;


            if (bGrd == 1)
            {
                //地线要找到地线覆冰工况
                if (WeathComm.Where(item => item.SWorkConditionName == "地线覆冰").Count() <= 0)
                    return;
                iceWkCdt = WeathComm.Where(item => item.SWorkConditionName == "地线覆冰").First();
            }
            else
            {
                //导线要找到最大覆冰工况
                if (WeathComm.Where(item => item.SWorkConditionName == "最大覆冰").Count() <= 0)
                    return;
                iceWkCdt = WeathComm.Where(item => item.SWorkConditionName == "最大覆冰").First();
            }

            float thickness1 = ElecCalsToolBox.IceThicknessExChange(dia, iceCover1, iceWkCdt.IceThickness);
            float thickness2 = ElecCalsToolBox.IceThicknessExChange(dia, iceCover2, iceWkCdt.IceThickness);

            WeathComm.AddRange(new List<WorkCondition>()
            {
                new WorkCondition()
                {
                    SWorkConditionName = "不均匀冰I",
                    IceThickness = thickness1,
                    Temperature = temp,
                    WindSpeed = wind,
                },
                new WorkCondition()
                {
                    SWorkConditionName = "不均匀冰II",
                    IceThickness = thickness2,
                    Temperature = temp,
                    WindSpeed = wind,
                },
            });
        }


        /// <summary>
        /// 更新地线的验算工况的冰厚
        /// </summary>
        public void UpdateGrdCkeckGK()
        {
            int index1 = WeathComm.FindIndex(item => item.SWorkConditionName == "验算冰");
            if(index1 >= 0)
            {
                WeathComm[index1].IceThickness += 5;
            }

            int index2 = WeathComm.FindIndex(item => item.SWorkConditionName == "验算不均匀冰I");
            if (index2 >= 0)
            {
                WeathComm[index2].IceThickness += 5;
            }

            int index3 = WeathComm.FindIndex(item => item.SWorkConditionName == "验算不均匀冰I");
            if (index3 >= 0)
            {
                WeathComm[index3].IceThickness += 5;
            }
        }

        


    }
}
