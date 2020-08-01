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

            //string fileName = @"D:\杆塔项目\other\【0722】读取文件 合并多文件\Z31.out";
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
                int itemNum = 0;//数组个数
                foreach (string item in striparr)
                {
                    tower = new TowerMember();
                    tower.Member = item.Substring(0, 10);//编号
                    tower.Section = item.Substring(12, 1) + item.Substring(11, 1) + item.Substring(13, 7);//截面
                    tower.Material = item.Substring(19, 1);//材质


                    List<string> itemSplit = item.Substring(20).Split(new[] { " " }, StringSplitOptions.None).Where(s => !string.IsNullOrEmpty(s)).ToList();
                    itemNum = itemSplit.Count;//数组个数

                    tower.LEN = itemSplit[0];//长度
                    tower.ULEN = itemSplit[1];//计算长度
                    tower.GR = itemSplit[2];//回转半径
                    tower.SR = itemSplit[3];//长细比
                    tower.ASR = itemSplit[4];//允许长细比
                    tower.GSFACR = itemSplit[5];//稳定系数
                    tower.Tens = itemSplit[6];//拉力
                    tower.TensCase = itemSplit[7].Trim() == "0" ? itemSplit[7] : itemSplit[7].Trim().Substring(itemSplit[7].Trim().Length-2).TrimStart('0');//受拉工况
                    tower.Comp = itemSplit[8];//压力
                    tower.CompCase = itemSplit[9].Trim() == "0" ? itemSplit[9] : itemSplit[9].Trim().Substring(itemSplit[9].Trim().Length - 2).TrimStart('0');//受压工况
                    tower.WFAC = itemSplit[10];//折减系数
                    tower.WSTR = itemSplit[11];//最大应力
                    tower.EFFIC = double.Parse(itemSplit[13]) * 100;//效率

                    if (itemNum == 18)
                    {
                        tower.Bolt = itemSplit[14].Substring(0, itemSplit[14].IndexOf('x'));//螺栓
                        tower.BoltNum = itemSplit[15].TrimEnd('^');//螺栓个数
                    }
                    else
                    {
                        tower.Bolt = itemSplit[14].Substring(0, itemSplit[14].IndexOf('x'));//螺栓
                        tower.BoltNum = itemSplit[15];//螺栓个数
                    }

                    tower.ReducingBoltNum = itemSplit[itemNum - 1];//减孔

                    resultList.Add(tower);
                }
            }
            return resultList;
        }
    }
}

