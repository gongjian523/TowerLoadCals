using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerLoadCals.Mode;

namespace TowerLoadCals.DAL
{
    public static class StruCalsResultReader
    {
        public static List<StrCalsResult> Read(string path)
        {
            List<StrCalsResult> result = new List<StrCalsResult>();
            string sLine = "";

            StreamReader file = new StreamReader(path, Encoding.Default);
            while ((sLine = file.ReadLine()) != null)
            {

            }
            return result;
        }
    }
}
