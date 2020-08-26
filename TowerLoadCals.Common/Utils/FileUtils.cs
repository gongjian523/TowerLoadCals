using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TowerLoadCals.Common
{
    //保存文本文档
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


        public static void TextSaveByLineInOneFile(string path, List<string> lineStr)
        {
            using (StreamWriter writer = File.AppendText(path))
            {
                foreach (string s in lineStr)
                {
                    writer.WriteLine(s);
                }
                writer.Flush();
                writer.Close();
            }

        }


        public static string PadRightEx(string str, int totalByteCount)
        {
            if (str == null)
                str = "XXX";

            Encoding coding = Encoding.GetEncoding("gb2312");
            int dcount = 0;
            foreach (char ch in str.ToCharArray())
            {
                if (coding.GetByteCount(ch.ToString()) == 2)
                    dcount++;
            }
            string w = str.PadRight(totalByteCount - dcount);
            return w;
        }

        public static string PadLeft(string str, int totalByteCount)
        {
            Encoding coding = Encoding.GetEncoding("gb2312");
            int dcount = 0;
            foreach (char ch in str.ToCharArray())
            {
                if (coding.GetByteCount(ch.ToString()) == 2)
                    dcount++;
            }
            string w = str.PadLeft(totalByteCount - dcount);
            return w;
        }
    }
}
