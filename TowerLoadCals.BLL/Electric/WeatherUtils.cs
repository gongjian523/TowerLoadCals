using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerLoadCals.Mode;

namespace TowerLoadCals.BLL.Electric
{
    public class WeatherUtils
    {

        public int TotalConNum { get; set; }

        public string Name { get; set; }

        /// <summary>
        ///输入气象区
        /// </summary>
        public List<WorkCondition> WeathComm { get; set; }

        /// <summary>
        /// 导线气象区
        /// </summary>
        public List<WorkCondition> WeathInd { get; set; }

        /// <summary>
        /// 地线气象区
        /// </summary>
        public List<WorkCondition> WeathGrd { get; set; }


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


        public WeatherUtils(string name, int id)
        {
            Name = name;
            ID = id;

            WeathComm = new List<WorkCondition>();
            WeathInd = new List<WorkCondition>();
            WeathGrd = new List<WorkCondition>();
        }

        public void InsertGK(List<WorkCondition> iniWeather)
        {
            WeathComm.AddRange(iniWeather);
        }

        /// <summary>
        /// 换算最大风速值到平均高度
        /// </summary>
        public void ConverWind(List<WorkCondition>  wkdList, float aveHei, char terType)
        {
            if(WeathComm.Where(item => item.SWorkConditionName == "最大风速").Count() > 0)
            {
                var temp = WeathComm.Where(item => item.SWorkConditionName == "最大风速").First();
                wkdList.Add(new WorkCondition
                {
                    SWorkConditionName = "换算最大风速",
                    IceThickness = temp.IceThickness,
                    Temperature = temp.Temperature,
                    WindSpeed = ElectricalCalsToolBox.WindExChange(temp.WindSpeed, aveHei, terType),
                });
            }

            if (WeathComm.Where(item => item.SWorkConditionName == "不均匀风").Count() > 0)
            {
                var temp = WeathComm.Where(item => item.SWorkConditionName == "不均匀风").First();
                wkdList.Add(new WorkCondition
                {
                    SWorkConditionName = "换算不均匀风",
                    IceThickness = temp.IceThickness,
                    Temperature = temp.Temperature,
                    WindSpeed = ElectricalCalsToolBox.WindExChange(temp.WindSpeed, aveHei, terType),
                });
            }
        }

        /// <summary>
        /// 换算45风的风速
        /// </summary>
        /// <param name="wkdList"></param>
        /// <param name="angle"></param>
        public void ConverWind45(List<WorkCondition> wkdList, float angle)
        {
            if (wkdList.Where(item => item.SWorkConditionName == "换算最大风速").Count() > 0)
            {
                var temp = wkdList.Where(item => item.SWorkConditionName == "换算最大风速").First();

                var wind1 = ElectricalCalsToolBox.Wind45ExChange1(temp.WindSpeed, angle);
                wkdList.AddRange( new List<WorkCondition>()
                {   
                    new WorkCondition()
                    {
                        SWorkConditionName = "顺线路外角侧45风",
                        IceThickness = temp.IceThickness,
                        Temperature = temp.Temperature,
                        WindSpeed = wind1,
                    },
                    new WorkCondition()
                    {
                        SWorkConditionName = "逆线路内角侧45风",
                        IceThickness = temp.IceThickness,
                        Temperature = temp.Temperature,
                        WindSpeed = wind1,
                    },
                });

                var wind2 = ElectricalCalsToolBox.Wind45ExChange2(temp.WindSpeed, angle);
                wkdList.AddRange(new List<WorkCondition>()
                {
                    new WorkCondition()
                    {
                        SWorkConditionName = "逆线路外角侧45风",
                        IceThickness = temp.IceThickness,
                        Temperature = temp.Temperature,
                        WindSpeed = wind2,
                    },
                    new WorkCondition()
                    {
                        SWorkConditionName = "顺线路内角侧45风",
                        IceThickness = temp.IceThickness,
                        Temperature = temp.Temperature,
                        WindSpeed = wind2,
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
    
    }
}
