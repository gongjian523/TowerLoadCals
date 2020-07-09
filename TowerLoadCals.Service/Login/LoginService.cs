using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TowerLoadCals.Mode.Login;
using TowerLoadCals.Service.Helpers;

namespace TowerLoadCals.Service.Login
{
    public class LoginService : DbContext
    {
        /// <summary>
        /// 服务器验证登录信息
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="Pwd">密码</param>
        /// <param name="key">通讯key</param>
        /// <returns></returns>
        public bool doLogin(UserInfo user)
        {
            //string Pwd = GenerateMD5(GenerateMD5(user.Password) + "A$%@#[]Mmm123098#@$");
            IList<UserInfo> list = UserInfoDb.AsQueryable().Where(item => item.UserName == "user" + user.UserName).ToList();

            //if(list!=null)
            //{

            //}

            return false;
        }
        /// <summary>
        /// MD5字符串加密
        /// </summary>
        /// <param name="txt"></param>
        /// <returns>加密后字符串</returns>
        public static string GenerateMD5(string pwd)
        {
            using (MD5 mi = MD5.Create())
            {
                byte[] buffer = Encoding.Default.GetBytes(pwd);
                //开始加密
                byte[] newBuffer = mi.ComputeHash(buffer);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < newBuffer.Length; i++)
                {
                    sb.Append(newBuffer[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }
    }
}
