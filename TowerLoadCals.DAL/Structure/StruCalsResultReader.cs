﻿using System.Collections.Generic;
using System.IO;
using System.Text;
using TowerLoadCals.Mode;

namespace TowerLoadCals.DAL
{
    public static class StruCalsResultReader
    {
        public static List<StruCalsResult> Read(string path)
        {
            List<StruCalsResult> result = new List<StruCalsResult>();
            string sLine = "";

            StreamReader file = new StreamReader(path, Encoding.Default);
            while ((sLine = file.ReadLine()) != null)
            {

            }
            return result;
        }
    }
}
