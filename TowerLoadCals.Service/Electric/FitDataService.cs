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
    /// 五金类
    /// </summary>
    public class FitDataService:DbContext
    {
        /// <summary>
        /// 查询所有信息
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IList<FitData> GetList(string filter)
        {
            return FitDataDb.GetList(item => item.Name.Contains(filter));
        }


    }
}
