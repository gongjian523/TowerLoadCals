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
using System.Web;
using System.Web.Script.Serialization;
using TowerLoadCals.Mode.Internet;
using TowerLoadCals.Service.Helpers;
using TowerLoadCals.Service.Internet;



/// <summary>
/// created by : glj
/// </summary>


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
            string SecretKey = "A$%@#[]Mmm123098#@$";
            string key = MD5Help.GenerateMD5(user.Password);
            string Pwd = MD5Help.GenerateMD5(key + SecretKey);

            //SecretKey = A$%@#[]Mmm123098#@$
            //MD5(password) = db2009160ef4754806e921cf843a1b93
            //MD5(MD5(password) + key) = e0d76b1869c74f4eea54abcb8475da63
            //YOUR KEY = aa{ "status":"error","msg":"\u6388\u6743\u5931\u8d25\uff0c\u8d26\u53f7\u6216\u5bc6\u7801\u9519\u8bef!"}

            UserInfoService userInfoDal = new UserInfoService();
            IList<UserInfo> list = userInfoDal.GetList().Where(item => item.UserName == "user" + user.UserName).ToList();

            if (list != null && list.Count != 0)
            {
                string url = string.Format("http://137.168.101.235:8885/user-externallogin-{0}-{1}.html", user.UserName, Pwd);//接口账号密码

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);//创建request
                request.Method = "GET";//提交数据方式
                request.ContentType = "application/json";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();//发送目标请求
                string jsonString;//json字符串

                using (Stream stream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(stream, Encoding.UTF8);

                    jsonString = HttpUtility.UrlDecode(reader.ReadToEnd());//如果有编码问题就用这个方法
                    //jsonString = reader.ReadToEnd();//得到json字符串
                }

                if (jsonString.Contains("success"))
                {
                    user.NickName = list[0].NickName;



                }
                return jsonString.Contains("success") ? true : false;
            }
            return false;

        }

    }
}
