using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TowerLoadCals.Mode;

namespace TowerLoadCals.DAL
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
            combo.Index = Convert.ToInt16(aWords[0]);
            combo.IsCalculate = Convert.ToBoolean(aWords[1].ToString());
            combo.WorkConditionCode = aWords[2];

            if (Type == TowerType.LineTower)
            {
                combo.WindDirectionCode = Convert.ToInt16(aWords[iIndex]);
            }
            else if (Type == TowerType.LineCornerTower)
            {
                combo.TensionAngleCode = aWords[iIndex];
                iIndex++;
                combo.WindDirectionCode = Convert.ToInt16(aWords[iIndex]);
            }
            else
            {
                combo.TensionAngleCode = aWords[iIndex];
                iIndex++;
                combo.VertialLoadCode = aWords[iIndex];
                iIndex++;
                combo.WindDirectionCode = Convert.ToInt16(aWords[iIndex]);
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

        public List<WorkConditionComboSpec> ConvertTemplateToSpec(TowerTemplate template)
        {
            List<WorkConditionComboSpec> listSpec = new List<WorkConditionComboSpec>();

            if (template == null)
                return listSpec;

            foreach(var item in template.WorkConditionCombos)
            {
                WorkConditionComboSpec spec = new WorkConditionComboSpec();

                spec.Index = item.Index;
                spec.IsCalculate = item.IsCalculate;
                spec.WorkConditionCode = item.WorkConditionCode;
                if(item.TensionAngleCode != null)
                    spec.TensionAngleCode = item.TensionAngleCode;
                if (item.VertialLoadCode != null)
                    spec.VertialLoadCode = item.VertialLoadCode;
                spec.WindDirectionCode = item.WindDirectionCode;
                spec.WorkCode = item.WorkCode;

                if (item.WirdIndexCodes.Count >= 1)
                    spec.Wire1 = item.WirdIndexCodes[0];
                if (item.WirdIndexCodes.Count >= 2)
                    spec.Wire2 = item.WirdIndexCodes[1];
                if (item.WirdIndexCodes.Count >= 3)
                    spec.Wire3 = item.WirdIndexCodes[2];
                if (item.WirdIndexCodes.Count >= 4)
                    spec.Wire4 = item.WirdIndexCodes[3];
                if (item.WirdIndexCodes.Count >= 5)
                    spec.Wire5 = item.WirdIndexCodes[4];
                if (item.WirdIndexCodes.Count >= 6)
                    spec.Wire6 = item.WirdIndexCodes[5];
                if (item.WirdIndexCodes.Count >= 7)
                    spec.Wire7 = item.WirdIndexCodes[6];
                if (item.WirdIndexCodes.Count >= 8)
                    spec.Wire8 = item.WirdIndexCodes[7];
                if (item.WirdIndexCodes.Count >= 9)
                    spec.Wire9 = item.WirdIndexCodes[8];
                if (item.WirdIndexCodes.Count >= 10)
                    spec.Wire10 = item.WirdIndexCodes[9];
                if (item.WirdIndexCodes.Count >= 11)
                    spec.Wire11 = item.WirdIndexCodes[10];
                if (item.WirdIndexCodes.Count >= 12)
                    spec.Wire12 = item.WirdIndexCodes[11];
                if (item.WirdIndexCodes.Count >= 13)
                    spec.Wire13 = item.WirdIndexCodes[12];
                if (item.WirdIndexCodes.Count >= 14)
                    spec.Wire14 = item.WirdIndexCodes[13];
                if (item.WirdIndexCodes.Count >= 15)
                    spec.Wire15 = item.WirdIndexCodes[14];
                if (item.WirdIndexCodes.Count >= 16)
                    spec.Wire16 = item.WirdIndexCodes[15];

                spec.WorkComment = item.WorkComment;

                listSpec.Add(spec);
            }

            return listSpec;
        }
    }
}