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
    /// 绝缘子串
    /// </summary>
    public class StrDataService : DbContext
    {

        /// <summary>
        /// 查询所有信息
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IList<Insulator> GetList()
        {
            return StrDataDb.GetList();
        }

    }
}
