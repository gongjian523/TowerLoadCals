using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// created by :glj
/// </summary>

namespace TowerLoadCals.Mode.Internet
{
    /// <summary>
    /// kb_linedixian    用户信息 
    /// </summary>
    [SugarTable("kb_admin")]
    public class UserInfo
    {
        public string UserName { get; set; }
        public string NickName { get; set; }
        public string Password { get; set; }

    }
}
