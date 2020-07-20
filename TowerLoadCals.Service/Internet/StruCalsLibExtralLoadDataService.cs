using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerLoadCals.Mode.Internet;
using TowerLoadCals.Service.Helpers;

namespace TowerLoadCals.Service.Internet
{
    /// <summary>
    /// #附加荷载库
    /// </summary>
    public class StruCalsLibExtralLoadDataService : DbContext
    {

        /// <summary>
        /// 查询所有信息
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IList<StruCalsLibExtralLoad> GetList()
        {
            return StruCalsLibExtralLoadDb.GetList();
        }

    }
}
