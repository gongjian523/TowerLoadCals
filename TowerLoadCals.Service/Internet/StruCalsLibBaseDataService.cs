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
    /// #基本参数库
    /// </summary>
    public class StruCalsLibBaseDataService : DbContext
    {
        /// <summary>
        /// 查询所有信息
        /// </summary>
        /// <returns></returns>
        public IList<StruCalsLibBaseData> GetList()
        {
            return StruCalsLibBaseDataDb.GetList();
        }


    }
}
