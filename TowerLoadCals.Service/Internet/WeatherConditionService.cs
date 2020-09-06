using SqlSugar;
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
    /// 气象条件
    /// </summary>
    public class WeatherConditionService : DbContext
    {

        /// <summary>
        /// 查询所有信息
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IList<WorkConditionCollections> GetList()
        {
            return Db.Queryable<WorkConditionInternet, WorkConditionCategory>((work, category) => new object[] {
            JoinType.Left,
            work.CategoryId==category.Id
        }).Select<WorkConditionCollections>().ToList();
        }
    }
}
