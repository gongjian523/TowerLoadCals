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
    /// 绝缘子串
    /// </summary>
    public class StrDataService : DbContext
    {

        /// <summary>
        /// 查询所有信息
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IList<StrData> GetList(string filter)
        {
            return StrDataDb.GetList(item => item.Name.Contains(filter));
        }

    }
}
