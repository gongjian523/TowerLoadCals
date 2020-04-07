using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.Readers
{
    public class TaReader
    {
        public static List<string> ReadTa(string path)
        {
            List<string> list = new List<string>();

            string line;

            // Read the file and display it line by line.  
            System.IO.StreamReader file = new System.IO.StreamReader(path);
            while ((line = file.ReadLine()) != null)
            {
                list.Add(line);
            }

            file.Close();
            return list;
        }
    }
}
