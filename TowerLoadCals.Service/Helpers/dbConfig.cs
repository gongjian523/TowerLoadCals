using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/// <summary>
/// created by : glj
/// </summary>


namespace TowerLoadCals.Service.Helpers
{
    public class dbConfig
    {
        /// <summary>
        /// Account have permission to create database
        /// 用有建库权限的数据库账号
        /// </summary>
        /// 
        public static string ConnectionString = "server= 137.168.101.235;port=3306 ;uid=modledbadmin;pwd=Modledbadmin@321;database=modlesdatabase";
    }
}
