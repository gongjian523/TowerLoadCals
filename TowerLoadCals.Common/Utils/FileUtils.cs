using System.Collections.Generic;
using System.IO;

namespace TowerLoadCals.Common
{
    public class FileUtils
    {
        public static void TextSaveByLine(string path, List<string>lineStr)
        {
            using (FileStream fileStream = File.OpenWrite(path))
            {
                using (StreamWriter writer = new StreamWriter(fileStream))
                {
                    foreach (string s in lineStr)
                    {
                        writer.WriteLine(s);
                    }
                    writer.Flush();
                    writer.Close();
                }
            }
        }
        }
}
