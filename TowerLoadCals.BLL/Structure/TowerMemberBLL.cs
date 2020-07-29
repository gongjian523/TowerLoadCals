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
        public IList<TowerMember> TextFileReadAll(string fileName)
        {
            IList<TowerMember> resultList = new List<TowerMember>();

            //fileName = @"C:\Users\zhifei\Desktop\测试\ZC3060B.out";
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
    }
}

