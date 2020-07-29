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
        //防震锤
        public SimpleClient<CounterWeight> CounterWeightDb => new SimpleClient<CounterWeight>(Db);
        //间隔棒
        public SimpleClient<Spacer> SpacerDb => new SimpleClient<Spacer>(Db);


        //绝缘子串(一般子串)
        public SimpleClient<GeneralInsulator> GeneralInsulatorDb => new SimpleClient<GeneralInsulator>(Db);

        //绝缘子串(硬跳线)
        public SimpleClient<RigidJumperInsulator> RigidJumperInsulatorDb => new SimpleClient<RigidJumperInsulator>(Db);

        //气象条件
        public SimpleClient<WorkConditionInternet> WorkConditionDb => new SimpleClient<WorkConditionInternet>(Db);

        //气象条件类别
        public SimpleClient<WorkConditionCategory> WorkConditionCategoryDb => new SimpleClient<WorkConditionCategory>(Db);

        //导线
        public SimpleClient<Wire> WireDb => new SimpleClient<Wire>(Db);
        //地线
        public SimpleClient<EarthWire> EarthWireDb => new SimpleClient<EarthWire>(Db);

        //覆冰参数库
        public SimpleClient<StruCalsLibIceCover> StruCalsLibIceCoverDb => new SimpleClient<StruCalsLibIceCover>(Db);

        //覆冰参数库
        public SimpleClient<StruCalsLibExtralLoad> StruCalsLibExtralLoadDb => new SimpleClient<StruCalsLibExtralLoad>(Db);

        //基本参数库
        public SimpleClient<StruCalsLibBaseData> StruCalsLibBaseDataDb => new SimpleClient<StruCalsLibBaseData>(Db);

        //基本参数库 明细
        public SimpleClient<StruCalsLibBaseData_Detail> StruCalsLibBaseData_DetailDb => new SimpleClient<StruCalsLibBaseData_Detail>(Db);

        //结构模板库 通用模板库
        public SimpleClient<StruTemplateLibGeneral> StruTemplateLibGeneralDb => new SimpleClient<StruTemplateLibGeneral>(Db);

    }
}