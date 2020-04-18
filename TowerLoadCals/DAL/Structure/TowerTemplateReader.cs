using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TowerLoadCals.Mode;

namespace TowerLoadCals.Readers
{
    public class TowerTemplateReader
    {
        public enum TowerType
        {
            LineTower,
            LineCornerTower,
            CornerTower,
            TerminalTower,
            BranchTower
        }

        public TowerTemplate template = new TowerTemplate();

        protected TowerType Type { get; set;}

        protected int WireNum { get; set; }

        protected int WorkConditionNum { get; set; }

        protected int WorkConditonComboNum { get; set; }

        protected int InstructionLine {
            get { return 2; }}

        protected int WireLine { 
            get { return 4; }
        }

        protected int WorkConditongsLine { get; set; }

        protected int WorkConditongsComboStartLine { get; set; }


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
                    WireNum = Convert.ToInt16(aWords[0]);
                    WorkConditionNum = Convert.ToInt16(aWords[2]);
                    WorkConditonComboNum = Convert.ToInt16(aWords[3]);
                }
                else if (iLineNum == WireLine)
                {
                    string[] aWords = Regex.Split(sLine.Trim(), "\\s+");
                    foreach (string sWord in aWords)
                    {
                        template.Wires.Add(sWord);
                    }
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

                        template.WorkConditongs.Add(Convert.ToInt16(aWords[i].ToString().Substring(0, iDigitNum)), aWords[i].ToString().Substring(iDigitNum+1));
                    }
                }
                else if(iLineNum >= WorkConditongsComboStartLine)
                {
                    if(iComboNum < WorkConditonComboNum)
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


        protected void DecodeWorkCondition(string line, out WorkConditionCombo combo)
        {
            combo = new WorkConditionCombo()
            {
                WirdIndexCodes = new List<int>()
            };

            string[] aWords = Regex.Split(line.Trim(), "\\s+");

            int iIndex = 3;
            combo.IsCalculate = Convert.ToBoolean(aWords[1].ToString());
            combo.WorkConditionCode = aWords[2];

            if (Type == TowerType.LineTower)
            {
                combo.WindDirectionCode = aWords[iIndex];
            }
            else if (Type == TowerType.LineCornerTower)
            {
                combo.TensionAngleCode = aWords[iIndex];
                iIndex++;
                combo.WindDirectionCode = aWords[iIndex];
            }
            else
            {
                combo.TensionAngleCode = aWords[iIndex];
                iIndex++;
                combo.VertialLoadCode = aWords[iIndex];
                iIndex++;
                combo.WindDirectionCode = aWords[iIndex];
            }

            for (int i = 1; i <= WireNum; i++)
            {
                combo.WirdIndexCodes.Add(Convert.ToInt16(aWords[iIndex + i]));
            }

            iIndex++;
            combo.WorkCode = aWords[iIndex + WireNum].ToString();
            iIndex++;
            combo.WorkComment = aWords[iIndex + WireNum].ToString();
        }
    }
}