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

        public WeatherUtils(List<WorkCondition> iniWeather)
        {
            
        }

        public List<WorkCondition> WorkConditions;

        /// <summary>
        /// 换算最大风速值到平均高度
        /// </summary>
        protected void ConverWind()
        {

        }

        /// <summary>
        /// 增加地线覆冰计算，用于计算地线覆冰荷
        /// </summary>
        protected void AddGrdWeath()
        {

        }

        /// <summary>
        /// 增加其他需要应用的工况
        /// </summary>
        protected void AddOtherGk()
        {

        }
    
    }
}
