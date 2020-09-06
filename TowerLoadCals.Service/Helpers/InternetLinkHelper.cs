using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;



/// <summary>
/// created by : glj
/// </summary>


namespace TowerLoadCals.Service.Helpers
{
    public class InternetLinkHelper
    {
        /// <summary>
        /// 判断网络是否连接
        /// </summary>
        /// <returns></returns>
        public static bool GetInternetLink()
        {
            return false;
            string strBuff = "";
            Uri httpURL = new Uri("http://137.168.101.235:8885/index-checkifonline.html");

            HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create(httpURL);
            HttpWebResponse httpResp = (HttpWebResponse)httpReq.GetResponse();

            Stream respStream = httpResp.GetResponseStream();
            StreamReader respStreamReader = new StreamReader(respStream, Encoding.UTF8);
            strBuff = respStreamReader.ReadToEnd();

            //关闭文件流
            respStreamReader.Close();
            respStream.Close();

            BackResult result = JsonConvert.DeserializeObject<BackResult>(strBuff);
            if (result.status == "success")
            {
                return true;
            }
            return false;
        }
    }


    public class BackResult
    {
        public string status { get; set; }
        public string msg { get; set; }

    }

}
