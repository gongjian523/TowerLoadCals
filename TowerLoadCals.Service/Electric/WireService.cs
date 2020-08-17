using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using TowerLoadCals.Service.Helpers;

namespace TowerLoadCals.Service.Electric
{
    /// <summary>
    /// 导线 地线
    /// </summary>
    public class WireService : DbContext
    {
        /// <summary>
        /// 查询所有信息
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public  IList<Wire> GetList(string filter)
        {
             return   WireDb.GetList(item=>item.ModelSpecification.Contains(filter));
        }

    }
}
