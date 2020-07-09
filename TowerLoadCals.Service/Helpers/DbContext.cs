using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerLoadCals.Mode;
using TowerLoadCals.Mode.Login;

namespace TowerLoadCals.Service.Helpers
{
    public class DbContext
    {

        public SqlSugarClient Db;
        public DbContext()
        {
            Db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = dbConfig.ConnectionString,
                DbType = DbType.MySql,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute,
                AopEvents = new AopEvents()
                {
                    OnLogExecuting = (sql, p) =>
                    {
                        Console.WriteLine(sql);
                    }
                }
            });
        }
        /// <summary>
        /// 用户名
        /// </summary>
        public SimpleClient<UserInfo> UserInfoDb => new SimpleClient<UserInfo>(Db);


    
        public SimpleClient<FitData> FitDataDb => new SimpleClient<FitData>(Db);
        public SimpleClient<StrData> StrDataDb => new SimpleClient<StrData>(Db);
        public SimpleClient<WorkCondition> WorkConditionDb => new SimpleClient<WorkCondition>(Db);
        public SimpleClient<Wire> WireDb => new SimpleClient<Wire>(Db);


    }
}