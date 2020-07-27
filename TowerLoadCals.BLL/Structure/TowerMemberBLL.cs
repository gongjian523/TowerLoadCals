using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Forms;
using TowerLoadCals.Mode.Structure;

namespace TowerLoadCals.BLL.Structure
{
    public class TowerMemberBLL
    {
        public IList<TowerMember> TextFileReadAll()
        {
            IList<TowerMember> resultList = new List<TowerMember>();

            string fileName = @"D:\杆塔项目\other\【0722】\ZC3060B.out";
            string readEndStr = ""; //读取文本
            using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                StreamReader reader = new StreamReader(fs, Encoding.Default);
                readEndStr = reader.ReadToEnd();
            }
            if (!string.IsNullOrEmpty(readEndStr))
            {

                //截取数据文本断
                int startIndex = readEndStr.IndexOf("MEMBER REPORT");
                int lastIndex = readEndStr.IndexOf("TOWER       WEIGHT :  ( Kg )");
                string TowrStr = readEndStr.Substring(startIndex, lastIndex - startIndex);//数据字符串

                //去掉表结构，保留数据
                TowrStr = TowrStr.Replace("MEMBER REPORT", "").Replace("====================================================================================================================================", "").Replace(@"  MEMBER     SIZE      LEN.  U.LEN.   G.R.    S.R.  A.S.R. S.FAC.   TENS.  CASE    COMP.  CASE   W.FAC.   W.STR.  A.STR.     EFFIC     BOLT", "").Replace("------------------------------------------------------------------------------------------------------------------------------------", "");
                //去掉换行符，保留数据行
                List<string> striparr = TowrStr.Split(new string[] { "\r\n" }, StringSplitOptions.None).ToList();
                striparr = striparr.Where(s => !string.IsNullOrEmpty(s.TrimEnd())).ToList();

                TowerMember tower;
                int itemNum = 0;
                foreach (string item in striparr)
                {
                    tower = new TowerMember();
                    tower.Member = item.Substring(0, 10);//编号
                    tower.Section = item.Substring(12, 1) + item.Substring(11, 1) + item.Substring(13, 7);//截面
                    tower.Material = item.Substring(19, 1);//材质

                    List<string> itemSplit = item.Substring(20).Split(new[] { "  " }, StringSplitOptions.None).Where(s => !string.IsNullOrEmpty(s)).ToList();
                    itemNum = itemSplit.Count;
                    tower.WSTR = itemSplit[itemNum - 5];//最大应力
                    tower.EFFIC = double.Parse(itemSplit[itemNum - 3]) * 100;//效率
                    tower.Bolt = itemSplit[itemNum - 2].Substring(0, itemSplit[itemNum - 2].IndexOf('x'));//螺栓
                    tower.BoltNum = itemSplit[itemNum - 2].Substring(itemSplit[itemNum - 2].Length - 1);//螺栓个数

                    resultList.Add(tower);
                }
            }
            return resultList;
        }
        #region  合并文件
        public void UnionTextFile()
        {
            string inputFilePath_A = @"D:\杆塔项目\other\【0722】\古丽娟 周五 工作任务\古丽娟 周五 工作任务\示例数据\N28-Z31 一侧常规一侧悬臂 改塔库.load";
            string inputFilePath_B = @"D:\杆塔项目\other\【0722】\古丽娟 周五 工作任务\古丽娟 周五 工作任务\示例数据\N102 Z31 一侧常规一侧悬臂 改塔库.load";

            string fileName_A = "--【" + Path.GetFileNameWithoutExtension(inputFilePath_A) + "】";
            string fileName_B = "--【" + Path.GetFileNameWithoutExtension(inputFilePath_B) + "】";

            StringBuilder outFileStr = new StringBuilder();//合并后的头部输出信息

            int
                headRowCount_FileA = 0,
                headRowCount_FileB = 0;//文件前排信息行数

            Dictionary<int, List<string>> fileArray_A = new Dictionary<int, List<string>>();//文件一  数据集合
            Dictionary<int, List<string>> fileArray_B = new Dictionary<int, List<string>>();//文件二  数据集合

            ReadFileContent(inputFilePath_A, fileName_A, outFileStr, ref headRowCount_FileA, fileArray_A);

            ReadFileContent(inputFilePath_B, fileName_B, outFileStr, ref headRowCount_FileB, fileArray_B);


            //写入合并文件
            UnionFileContent(outFileStr, headRowCount_FileA, headRowCount_FileB, fileName_A, fileName_B, fileArray_A, fileArray_B);

            //输出文件流
            WriterFileContent(outFileStr);
        }


        #region 读取文件流 ReadFileContent

        /// <summary>
        /// 读取文件流
        /// </summary>
        /// <param name="inputFilePath">读取文件地址</param>
        /// <param name="fileName">文件名</param>
        /// <param name="outFileheardStr">后期需要写入的文件流</param>
        /// <param name="headRowCount_File">文件头文件行</param>
        /// <param name="fileArray">当前文件数据集合</param>
        private static void ReadFileContent(string inputFilePath, string fileName, StringBuilder outFileheardStr, ref int headRowCount_File, Dictionary<int, List<string>> fileArray)
        {
            int key = 0;
            string ReadLineStr = "";
            List<string> values = null, stripArr = null;

            //读取第一个文本信息
            using (FileStream fs = new FileStream(inputFilePath, FileMode.Open))
            {
                StreamReader reader = new StreamReader(fs, Encoding.Default);

                while ((ReadLineStr = reader.ReadLine()) != null)
                {
                    ReadLineStr = ReadLineStr.Trim();
                    stripArr = ReadLineStr.Split(new[] { "  " }, StringSplitOptions.None).Where(s => !string.IsNullOrEmpty(s)).ToList();
                    if (stripArr.Count > 4)
                    {
                        outFileheardStr.AppendLine(string.Format("{0}    {1}", ReadLineStr, headRowCount_File == 0 ? fileName : ""));
                        headRowCount_File++;
                    }
                    if (stripArr.Count == 4)
                    {
                        if (values != null)
                            fileArray.Add(key, values);

                        key = int.Parse(stripArr[0]);
                        values = new List<string>();
                        values.Add(stripArr[1] + "    " + stripArr[2] + "    " + stripArr[3] + "     " + fileName);

                    }
                    if (stripArr.Count == 3)
                    {
                        values.Add(stripArr[0] + "    " + stripArr[1] + "    " + stripArr[2]);
                    }

                }
                if (values != null)
                {
                    fileArray.Add(key, values);
                    values = null;
                }
                reader.Close();//关闭读流
            }
        }
        #endregion

        #region 合并文件数据
        /// <summary>
        ///合并文件数据
        /// </summary>
        /// <param name="outFileheardStr">合并后的头部输出信息</param>
        /// <param name="headRowCount_FileA">文件A数据行</param>
        /// <param name="headRowCount_FileB">文件B数据行</param>
        /// <param name="fileName_A">A文件名</param>
        /// <param name="fileName_B">B文件名</param>
        /// <param name="fileArray_A">文件A数据集合</param>
        /// <param name="fileArray_B">文件B数据集合</param>

        private static void UnionFileContent(StringBuilder outFileheardStr, int headRowCount_FileA, int headRowCount_FileB, String fileName_A, String fileName_B, Dictionary<int, List<string>> fileArray_A, Dictionary<int, List<string>> fileArray_B)
        {
            IEnumerable<int> keys = fileArray_A.Keys.Union<int>(fileArray_B.Keys).OrderBy(k => k);//合并key值

            int valuePairIndex = 0;//判断是否增加key值
            foreach (int item in keys)
            {
                if (fileArray_A.ContainsKey(item) && fileArray_B.ContainsKey(item))//两个文件都存在该key值
                {
                    valuePairIndex = 0;//判断是否增加key值
                    foreach (string valuePair in fileArray_A.Where(q => q.Key == item).FirstOrDefault().Value)
                    {
                        if (valuePairIndex == 0)//如果为第一行数据则增加key值
                        {
                            outFileheardStr.AppendLine(string.Format("{0}          {1}", item, valuePair));
                            valuePairIndex++;
                        }
                        else
                            outFileheardStr.AppendLine(string.Format("{0}          {1}", new string(' ', item.ToString().Length), valuePair));
                    }

                    foreach (string valuePair in fileArray_B.Where(q => q.Key == item).FirstOrDefault().Value)
                    {
                        outFileheardStr.AppendLine(string.Format("{0}          {1}", new string(' ', item.ToString().Length), valuePair));

                    }

                }
                //A文件存在key，B文件不存在key
                if (fileArray_A.ContainsKey(item) && !fileArray_B.ContainsKey(item))
                {
                    valuePairIndex = 0;//判断是否增加key值
                    foreach (string valuePair in fileArray_A.Where(q => q.Key == item).FirstOrDefault().Value)
                    {
                        if (valuePairIndex == 0)//如果为第一行数据则增加key值
                        {
                            outFileheardStr.AppendLine(string.Format("{0}          {1}", item, valuePair));
                            valuePairIndex++;
                        }
                        else
                            outFileheardStr.AppendLine(string.Format("{0}          {1}", new string(' ', item.ToString().Length), valuePair));
                    }

                    for (int i = 0; i < headRowCount_FileB; i++)
                    {
                        outFileheardStr.AppendLine(string.Format("{0}          0.00    0.00    0.00     {1}", new string(' ', item.ToString().Length), i == 0 ? fileName_B : ""));
                    }

                }
                //A文件不存在Key，B文件存在Key
                if (!fileArray_A.ContainsKey(item) && fileArray_B.ContainsKey(item))
                {
                    for (int i = 0; i < headRowCount_FileA; i++)
                    {
                        outFileheardStr.AppendLine(string.Format("{0}          0.00    0.00    0.00     {1}", i == 0 ? item.ToString() : "", i == 0 ? fileName_A : ""));
                    }

                    foreach (string valuePair in fileArray_B.Where(q => q.Key == item).FirstOrDefault().Value)
                    {
                        outFileheardStr.AppendLine(string.Format("{0}          {1}", new string(' ', item.ToString().Length), valuePair));
                    }

                }
            }

        }
        #endregion

        #region 输出信息流

        /// <summary>
        ///输出信息流 
        /// </summary>
        /// <param name="outFileStr">合并后的输出信息</param>
        private static void WriterFileContent(StringBuilder outFileStr)
        {

            //写入合并文件
            string outFilePath = @"D:\杆塔项目\other\【0722】\古丽娟 周五 工作任务\古丽娟 周五 工作任务\示例数据\合并1.load";

            using (FileStream fs = new FileStream(outFilePath, FileMode.Create))
            {
                StreamWriter writer = new StreamWriter(fs, Encoding.Default);

                writer.Write(outFileStr.ToString());

                writer.Close();//关闭写流
            }
        }
        #endregion

        #endregion 
    }
}

