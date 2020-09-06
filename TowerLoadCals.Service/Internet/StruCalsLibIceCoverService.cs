using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerLoadCals.Mode.Internet;
using TowerLoadCals.Service.Helpers;



/// <summary>
/// created by : glj
/// </summary>


namespace TowerLoadCals.Service.Internet
{
    /// <summary>
    /// 覆冰参数库
    /// </summary>
    public class StruCalsLibIceCoverDataService : DbContext
    {
        /// <summary>
        /// 查询所有信息
        /// </summary>
        /// <returns></returns>
        public IList<StruCalsLibIceCover> GetList()
        {
            return StruCalsLibIceCoverDb.GetList();
        }


    }
}
