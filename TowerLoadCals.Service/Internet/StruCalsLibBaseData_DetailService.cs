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
    /// #基本参数库明细
    /// </summary>
    public class StruCalsLibBaseData_DetailService : DbContext
    {
        /// <summary>
        /// 查询所有信息
        /// </summary>
        /// <returns></returns>
        public IList<StruCalsLibBaseData_Detail> GetList()
        {
            return StruCalsLibBaseData_DetailDb.GetList();
        }


    }
}
