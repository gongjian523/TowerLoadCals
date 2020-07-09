using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerLoadCals.Mode;
using TowerLoadCals.Service.Helpers;

namespace TowerLoadCals.Service.Electric
{
    /// <summary>
    /// 气象条件
    /// </summary>
    public class WeatherConditionService : DbContext
    {
        
        /// <summary>
      /// 查询所有信息
      /// </summary>
      /// <param name="filter"></param>
      /// <returns></returns>
        public IList<WorkCondition> GetList(string filter)
        {
            return WorkConditionDb.GetList(item => item.SWorkConditionName.Contains(filter));
        }
    }
}
