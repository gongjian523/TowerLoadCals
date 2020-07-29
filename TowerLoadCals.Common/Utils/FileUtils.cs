using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TowerLoadCals.Common
{
    public class FileUtils
    {
        public static void TextSaveByLine(string path, List<string>lineStr)
        {
            using (FileStream fileStream = File.OpenWrite(path))
            {
                using (StreamWriter writer = new StreamWriter(fileStream, Encoding.Default))
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
