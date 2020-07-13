using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerLoadCals.Mode.Internet;

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
        public SimpleClient<GeneralInsulator> FitDataDb => new SimpleClient<GeneralInsulator>(Db);


        //绝缘子串(一般子串)
        public SimpleClient<GeneralInsulator> GeneralInsulatorDb => new SimpleClient<GeneralInsulator>(Db);

        //绝缘子串(硬跳线)
        public SimpleClient<RigidJumperInsulator> RigidJumperInsulatorDb => new SimpleClient<RigidJumperInsulator>(Db);
        public SimpleClient<WorkConditionInternet> WorkConditionDb => new SimpleClient<WorkConditionInternet>(Db);
        public SimpleClient<Wire> WireDb => new SimpleClient<Wire>(Db);
        public SimpleClient<EarthWire> EarthWireDb => new SimpleClient<EarthWire>(Db);


    }
}