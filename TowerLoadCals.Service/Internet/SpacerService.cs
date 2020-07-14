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
    /// 五金类
    /// </summary>
    public class SpacerService : DbContext
    {
        /// <summary>
        /// 查询所有信息
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IList<Spacer> GetList()
        {
            return SpacerDb.GetList();
        }


    }
}
