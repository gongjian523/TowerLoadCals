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

        public List<WorkCondition> WeathColl { get; set; }

        public int ID { get; set; }

        public List<string> NameOfCtrWkCdt
        {
            get { return new List<string> { "最大风速", "最低气温", "最大覆冰", "平均气温" }; }
        }

        public WeatherUtils(string name, int id)
        {
            Name = name;
            ID = id;
        }

        public void InsertGK(List<WorkCondition> iniWeather)
        {
            WeathColl.AddRange(iniWeather);
        }

        /// <summary>
        /// 换算最大风速值到平均高度
        /// </summary>
        public void ConverWind(float aveHei, char terType)
        {
            if(WeathColl.Where(item => item.SWorkConditionName == "最大风速").Count() > 0)
            {
                var temp = WeathColl.Where(item => item.SWorkConditionName == "最大风速").First();
                WeathColl.Add(new WorkCondition
                {
                    SWorkConditionName = "换算最大风速",
                    IceThickness = temp.IceThickness,
                    Temperature = temp.Temperature,
                    WindSpeed = ElectricalCalsToolBox.WindExChange(temp.WindSpeed, aveHei, terType),
                });
            }

            if (WeathColl.Where(item => item.SWorkConditionName == "不均匀风").Count() > 0)
            {
                var temp = WeathColl.Where(item => item.SWorkConditionName == "不均匀风").First();
                WeathColl.Add(new WorkCondition
                {
                    SWorkConditionName = "换算不均匀风",
                    IceThickness = temp.IceThickness,
                    Temperature = temp.Temperature,
                    WindSpeed = ElectricalCalsToolBox.WindExChange(temp.WindSpeed, aveHei, terType),
                });
            }
        }

        /// <summary>
        /// 增加地线覆冰计算，用于计算地线覆冰荷
        /// </summary>
        public void AddGrdWeath()
        {
            if (WeathColl.Where(item => item.SWorkConditionName == "最大覆冰").Count() > 0)
            {
                var temp = WeathColl.Where(item => item.SWorkConditionName == "最大覆冰").First();

                WeathColl.Add(new WorkCondition
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
            if (WeathColl.Where(item => item.SWorkConditionName == "最大覆冰").Count() > 0)
            {
                var temp = WeathColl.Where(item => item.SWorkConditionName == "最大覆冰").First();

                WeathColl.Add(new WorkCondition
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
