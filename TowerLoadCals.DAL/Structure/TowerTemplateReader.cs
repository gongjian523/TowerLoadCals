using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using TowerLoadCals.Mode;

namespace TowerLoadCals.DAL
{
    public class TowerTemplateReader
    {
        public TowerTemplate template = new TowerTemplate();

        protected TowerType Type { get; set;}

        protected int WireNum { get; set; }

        protected int WorkConditionNum { get; set; }

        protected int WorkConditonComboNum { get; set; }

        protected virtual int InstructionLine {
            get { return 2; }}

        protected virtual int WireLine { 
            get { return 4; }
        }

        protected virtual int WorkConditongsLine { get; set; }

        protected virtual int WorkConditongsComboStartLine { get; set; }

        public TowerTemplateReader()
        {

        }

        public TowerTemplateReader(TowerType type)
        {
            if(type == TowerType.LineTower)
            {
                WorkConditongsLine = 15;
                WorkConditongsComboStartLine = 17;

                template.TowerType = "直线塔";
            }
            else if (type == TowerType.LineCornerTower)
            {
                WorkConditongsLine = 16;
                WorkConditongsComboStartLine = 18;

                template.TowerType = "直线转角塔";
            }
            else
            {
                WorkConditongsLine = 26;
                WorkConditongsComboStartLine = 29;

               if(type == TowerType.CornerTower)
                    template.TowerType = "转角塔";
               else if (type == TowerType.TerminalTower)
                    template.TowerType = "终端塔";
               else
                    template.TowerType = "分支塔";

            }

            Type = type;

            template.Wires = new List<string>();
            template.WorkConditongs = new Dictionary<int, string>();
            template.WorkConditionCombos = new List<WorkConditionCombo>();
        }

        public TowerTemplate Read(string path)
        {
            string sLine = "";
            int iLineNum = 0;

            int iComboNum = 0;

            template.Name = path.Substring(path.LastIndexOf('\\')+1);
            template.Name = template.Name.Substring(0,template.Name.Length - 4);

            StreamReader file = new StreamReader(path,Encoding.Default);
            while ((sLine = file.ReadLine()) != null)
            {
                iLineNum++;

                if(iLineNum == InstructionLine)
                {
                    string[] aWords = Regex.Split(sLine.Trim(), "\\s+");
                    //WireNum = Convert.ToInt32(aWords[0]);
                    WorkConditionNum = Convert.ToInt32(aWords[2]);
                    WorkConditonComboNum = Convert.ToInt32(aWords[3]);
                }
                else if (iLineNum == WireLine)
                {
                    string[] aWords = Regex.Split(sLine.Trim(), "\\s+");
                    foreach (string sWord in aWords)
                    {
                        template.Wires.Add(sWord);
                    }
                    WireNum = template.Wires.Count;
                }
                else if(iLineNum  == WorkConditongsLine)
                {
                    string[] aWords = Regex.Split(sLine.Trim(), "\\s+");

                    for (int i = 0; i < aWords.Count(); i++)
                    {
                        int iDigitNum = 0;
                        int iTotal = i+1;
                        string str = aWords[i];

                        while (iTotal > 0)
                        {
                            iTotal = iTotal / 10;
                            iDigitNum++;
                        }

                        template.WorkConditongs.Add(Convert.ToInt32(aWords[i].ToString().Substring(0, iDigitNum)), aWords[i].ToString().Substring(iDigitNum+1));
                    }
                }
                else if(iLineNum >= WorkConditongsComboStartLine)
                {
                    if(IsWorkConditionComboLine(iComboNum))
                    //if(iComboNum < WorkConditonComboNum)
                    {
                        DecodeWorkCondition(sLine, out WorkConditionCombo combo);
                        template.WorkConditionCombos.Add(combo);
                        iComboNum++;
                    }
                }
            }
            file.Close();

            return template;
        }

        public TowerTemplate ReadContentStream(string content,string fileName)
        {
            int iLineNum = 0;

            int iComboNum = 0;

            template.Name = fileName;
           List<string> contents = content.Split(new string[] { "\r\n" }, StringSplitOptions.None).Where(item=>!string.IsNullOrEmpty(item.Trim())).ToList();

            foreach( string sLine in contents)
            { 
                iLineNum++;

                if (iLineNum == InstructionLine)
                {
                    string[] aWords = Regex.Split(sLine.Trim(), "\\s+");
                    //WireNum = Convert.ToInt32(aWords[0]);
                    WorkConditionNum = Convert.ToInt32(aWords[2]);
                    WorkConditonComboNum = Convert.ToInt32(aWords[3]);
                }
                else if (iLineNum == WireLine)
                {
                    string[] aWords = Regex.Split(sLine.Trim(), "\\s+");
                    foreach (string sWord in aWords)
                    {
                        template.Wires.Add(sWord);
                    }
                    WireNum = template.Wires.Count;
                }
                else if (iLineNum == WorkConditongsLine)
                {
                    string[] aWords = Regex.Split(sLine.Trim(), "\\s+");

                    for (int i = 0; i < aWords.Count(); i++)
                    {
                        int iDigitNum = 0;
                        int iTotal = i + 1;
                        string str = aWords[i];

                        while (iTotal > 0)
                        {
                            iTotal = iTotal / 10;
                            iDigitNum++;
                        }

                        template.WorkConditongs.Add(Convert.ToInt32(aWords[i].ToString().Substring(0, iDigitNum)), aWords[i].ToString().Substring(iDigitNum + 1));
                    }
                }
                else if (iLineNum >= WorkConditongsComboStartLine)
                {
                    if (IsWorkConditionComboLine(iComboNum))
                    //if(iComboNum < WorkConditonComboNum)
                    {
                        DecodeWorkCondition(sLine, out WorkConditionCombo combo);
                        template.WorkConditionCombos.Add(combo);
                        iComboNum++;
                    }
                }
            }

            return template;
        }


        protected virtual bool IsWorkConditionComboLine(int comboNum)
        {
            //旧模板中不一定以工况组合结尾，故需要增加一个判断
            return comboNum < WorkConditonComboNum ? true : false;
        }

        protected void DecodeWorkCondition(string line, out WorkConditionCombo combo)
        {
            combo = new WorkConditionCombo()
            {
                WireIndexCodes = new List<int>()
            };

            string[] aWords = Regex.Split(line.Trim(), "\\s+");

            int iIndex = 3;
            combo.Index = Convert.ToInt32(aWords[0]);
            combo.IsCalculate = Convert.ToBoolean(aWords[1].ToString());
            combo.WorkConditionCode = aWords[2];

            if (Type == TowerType.LineTower)
            {
                
                combo.WindDirectionCode = Convert.ToInt32(aWords[iIndex]);

                //在直线塔中用不着以下两个字段，主要是为了在编译模板中，
                //塔型转换后保存时占位
                combo.TensionAngleCode = "None";
                combo.VertialLoadCode = "None";
            }
            else if (Type == TowerType.LineCornerTower)
            {
                combo.TensionAngleCode = aWords[iIndex];
                iIndex++;
                combo.WindDirectionCode = Convert.ToInt32(aWords[iIndex]);

                //赋值理由同上
                combo.VertialLoadCode = "None";
            }
            else
            {
                combo.TensionAngleCode = aWords[iIndex];
                iIndex++;
                combo.VertialLoadCode = aWords[iIndex];
                iIndex++;
                combo.WindDirectionCode = Convert.ToInt32(aWords[iIndex]);
            }

            for (int i = 1; i <= WireNum; i++)
            {
                combo.WireIndexCodes.Add(Convert.ToInt32(aWords[iIndex + i]));
            }

            iIndex++;
            combo.WorkCode = Convert.ToInt32(aWords[iIndex + WireNum]);
            iIndex++;
            combo.WorkComment = aWords[iIndex + WireNum].ToString();
        }

        public static void ConvertSpecToWorkCondition(TowerTemplate template, List<WorkConditionComboSpec> workConditionSpecs, bool isCalculation = false)
        {
            List<WorkConditionCombo> workConditions = new List<WorkConditionCombo>();

            WorkConditionComboSpec item;
            //foreach (var item in workConditionSpecs)
                for (int index=0; index < workConditionSpecs.Count;index++)
            {
                item = workConditionSpecs[index];
                WorkConditionCombo wcc= new WorkConditionCombo();

                wcc.Index = item.Index;
                wcc.IsCalculate = item.IsCalculate;
                wcc.WorkConditionCode = isCalculation? template.WorkConditionCombos[index].WorkConditionCode: item.WorkConditionCode;
                if (item.TensionAngleCode != null)
                    wcc.TensionAngleCode = isCalculation ? template.WorkConditionCombos[index].TensionAngleCode : item.TensionAngleCode;
                if (item.VertialLoadCode != null)
                    wcc.VertialLoadCode = isCalculation ? template.WorkConditionCombos[index].VertialLoadCode : item.VertialLoadCode;
                wcc.WindDirectionCode = item.WindDirectionCode;
                wcc.WorkCode = item.WorkCode;

                //int wireIndexCodesNum = template.WorkConditionCombos[0].WireIndexCodes.Count;
                int wireIndexCodesNum = template.Wires.Count;
                wcc.WireIndexCodes = new List<int>();

                for (int i = 1; i <= wireIndexCodesNum; i++)
                {
                    Type itemType = item.GetType();
                    PropertyInfo itemPro = itemType.GetProperty("Wire" + i.ToString(), BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public);
                    if (itemPro != null)
                        wcc.WireIndexCodes.Add(isCalculation ? template.WorkConditionCombos[index].WireIndexCodes[i-1]:Convert.ToInt32(itemPro.GetValue(item, null)));
                }

                wcc.WorkComment = item.WorkComment;

                workConditions.Add(wcc);
            }

            template.WorkConditionCombos = workConditions;
        }

        protected void EncodeWorkCondition(out string comboStr, WorkConditionCombo combo)
        {
            comboStr = "";

            //添加序号
            comboStr += combo.Index.ToString().PadRight(8);

            //添加是否计算
            comboStr += combo.IsCalculate.ToString().PadRight(10);

            //添加工况代号
            comboStr +=  combo.WorkConditionCode.PadRight(8);

            if (Type == TowerType.LineTower)
            {
                comboStr += combo.WindDirectionCode.ToString().PadRight(8);
            }
            else if (Type == TowerType.LineCornerTower)
            {
                comboStr += combo.TensionAngleCode.PadRight(8);
                comboStr += combo.WindDirectionCode.ToString().PadRight(8);
            }
            else
            {
                comboStr += combo.TensionAngleCode.PadRight(8);
                comboStr += combo.VertialLoadCode.PadRight(8);
                comboStr += combo.WindDirectionCode.ToString().PadRight(8);
            }

            foreach(var  wireCode in combo.WireIndexCodes)
            {
                comboStr += wireCode.ToString().PadLeft(8);
            }

            //添加工况码
            comboStr += combo.WorkCode.ToString().PadLeft(8);
            //添加注释
            comboStr += "   " + combo.WorkComment; 
        }
    }
}