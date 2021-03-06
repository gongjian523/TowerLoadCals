﻿using System;
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
    /// 五金类
    /// </summary>
    public class CounterWeightService:DbContext
    {
        /// <summary>
        /// 查询所有信息
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IList<CounterWeight> GetList()
        {
            return CounterWeightDb.GetList();
        }


    }
}
