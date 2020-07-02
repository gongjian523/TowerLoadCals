using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.Mode
{
    /// <summary>
    /// 此类用于工程文件中，各种常规字符串的定义
    /// </summary>
    public static class ConstVar
    {
        public static string StruCalsParasFileName
        {
            get { return "计算参数.xml"; }
        }

        public static string StruCalsElecLoadFileName
        {
            get { return "电气负荷.xlsx"; }
        }

        public static string StruCalsLibFileName
        {
            get { return "StruCalsParasLib.xml"; }
        }

        public static string DataBaseStr
        {
            get { return "BaseData"; }
        }

        public static string StruCalsStr
        {
            get { return "StruCals"; }
        }

        public static string UserDataStr
        {
            get { return "UserData"; }
        }
    }
}
