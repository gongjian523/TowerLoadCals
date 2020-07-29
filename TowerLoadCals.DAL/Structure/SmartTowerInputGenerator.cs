using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TowerLoadCals.Common;

namespace TowerLoadCals.DAL.Structure
{
    public class SmartTowerInputGenerator
    {
        public static string InputGenerator(string loadPath, string templatePath)
        {
            if (!File.Exists(loadPath))
            {
                return "Load文件不存在！";
            }

            if (!File.Exists(templatePath))
            {
                return "结构计算模型不存在！";
            }

            StreamReader loadFile = new StreamReader(loadPath, Encoding.Default);
            List<string> loadStrs = new List<string>();
            string lineTmep = "";
            int lineNum = 0;
            int newPointsNum = 0, newWorkConditionsNum = 0;
            while ((lineTmep = loadFile.ReadLine()) != null)
            {
                lineNum++;
                string[] aWords = Regex.Split(lineTmep.Trim(), "\\s+");
                //工况行分割后一定大于4
                if(aWords.Count() > 4)
                {
                    newWorkConditionsNum++;
                }
                loadStrs.Add(lineTmep);
            }
            //挂点数通过文件内容的总行数和工况数计算
            newPointsNum = (lineNum - newWorkConditionsNum) / newWorkConditionsNum;
            loadFile.Close();

            StreamReader templateFile = new StreamReader(templatePath, Encoding.Default);
            List<string> templateStrs = new List<string>();

            int deleteStartIndex = 0;
            int oldPointsNum = 0, oldWorkConditionsNum = 0;
            lineNum = 0;

            while ((lineTmep = templateFile.ReadLine()) != null)
            {
                lineNum++;
                //前三行直接复制
                if (lineNum < 4)
                {
                    templateStrs.Add(lineTmep);
                }
                //第四行将新模型中的挂点数和工况数替换旧的
                else if (lineNum == 4)
                {
                    string[] aWords = Regex.Split(lineTmep.Trim(), "\\s+");
                    oldWorkConditionsNum = Convert.ToInt32(aWords[6]);
                    oldPointsNum = Convert.ToInt32(aWords[7]);

                    string temp = aWords[0].PadLeft(5) + aWords[1].PadLeft(5) + aWords[2].PadLeft(5) + aWords[3].PadLeft(5)
                        + aWords[4].PadLeft(5) + aWords[5].PadLeft(5) + newWorkConditionsNum.ToString().PadLeft(6) + newPointsNum.ToString().PadLeft(5)
                        + aWords[8].PadLeft(5) + aWords[9].PadLeft(6) + aWords[10].PadLeft(6);

                    templateStrs.Add(temp);

                }
                //还没有开始删除旧的工况信息和挂点信息
                else if(deleteStartIndex  == 0)
                {
                    templateStrs.Add(lineTmep);
                    string[] aWords = Regex.Split(lineTmep.Trim(), "\\s+");
                    if(aWords[3] == "A" || aWords[3] == "B" || aWords[3] == "C" || aWords[3] == "D")
                    {
                        deleteStartIndex = lineNum;
                    }
                }
                //插入新的工况信息和挂点信息
                else if(deleteStartIndex != 0 && lineNum == (deleteStartIndex + oldWorkConditionsNum + oldPointsNum * oldWorkConditionsNum + 1))
                {
                    templateStrs.AddRange(loadStrs);
                    templateStrs.Add(lineTmep);
                }
                //将后续信息一并复制
                else if(deleteStartIndex != 0 && lineNum > (deleteStartIndex + oldWorkConditionsNum + oldPointsNum * oldWorkConditionsNum + 1))
                {
                    templateStrs.Add(lineTmep);
                }
            }
            templateFile.Close();

            File.Delete(templatePath);
            FileUtils.TextSaveByLine(templatePath, templateStrs);
            return "";
        }
    }
}
