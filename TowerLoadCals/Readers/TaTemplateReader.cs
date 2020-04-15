using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TowerLoadCals.DataMaterials;

namespace TowerLoadCals.Readers
{
    public class TaTemplateReader
    {

        public enum TowerType
        {
            LineTower,
            LineCornerTower,
            CornerTower,
            TerminalTower,
            BranchTower
        }

        public TaTemplate template = new TaTemplate();

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


        public TaTemplateReader(TowerType type)
        {
            if(type == TowerType.LineTower)
            {
                WorkConditongsLine = 15;
                WorkConditongsComboStartLine = 17;

                template.Type = "直线塔";
            }
            else if (type == TowerType.LineCornerTower)
            {
                WorkConditongsLine = 16;
                WorkConditongsComboStartLine = 18;

                template.Type = "直线转角塔";
            }
            else
            {
                WorkConditongsLine = 26;
                WorkConditongsComboStartLine = 29;

               if(type == TowerType.CornerTower)
                    template.Type = "转角塔";
               else if (type == TowerType.TerminalTower)
                    template.Type = "终端塔";
               else
                    template.Type = "分支塔";

            }

            Type = type;

            template.Wires = new List<string>();
            template.WorkConditongs = new Dictionary<int, string>();
            template.WorkConditionCombos = new List<WorkConditionCombo>();
        }

        public TaTemplate Read(string path)
        {
            string line = "";
            int lineNum = 0;

            int comboNum = 0;

            template.Name = path.Substring(path.LastIndexOf('\\')+1);
            template.Name = template.Name.Substring(0,template.Name.Length - 4);

            StreamReader file = new StreamReader(path,Encoding.Default);
            while ((line = file.ReadLine()) != null)
            {
                lineNum++;

                if(lineNum == InstructionLine)
                {
                    string[] sArray = Regex.Split(line.Trim(), "\\s+");
                    WireNum = Convert.ToInt16(sArray[0]);
                    WorkConditionNum = Convert.ToInt16(sArray[2]);
                    WorkConditonComboNum = Convert.ToInt16(sArray[3]);
                }
                else if (lineNum == WireLine)
                {
                    string[] sArray = Regex.Split(line.Trim(), "\\s+");
                    foreach (string str in sArray)
                    {
                        template.Wires.Add(str);
                    }
                }
                else if(lineNum  == WorkConditongsLine)
                {
                    string[] sArray = Regex.Split(line.Trim(), "\\s+");

                    for (int i = 0; i < sArray.Count(); i++)
                    {
                        int digitNum = 0;
                        int total = i+1;
                        string str = sArray[i];

                        while (total > 0)
                        {
                            total = total / 10;
                            digitNum++;
                        }

                        template.WorkConditongs.Add(Convert.ToInt16(sArray[i].ToString().Substring(0, digitNum)), sArray[i].ToString().Substring(digitNum+1));
                    }
                }
                else if(lineNum >= WorkConditongsComboStartLine)
                {
                    if(comboNum < WorkConditonComboNum)
                    {
                        DecodeWorkCondition(line, out WorkConditionCombo combo);
                        template.WorkConditionCombos.Add(combo);
                        comboNum++;
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
                Indexs = new List<int>()
            };

            string[] sArray = Regex.Split(line.Trim(), "\\s+");

            if(Type == TowerType.LineTower)
            {
                combo.para1 = Convert.ToBoolean(sArray[1].ToString());
                combo.para2 = sArray[2];
                combo.para3 = sArray[3];

                for (int i = 1; i <= WireNum; i++)
                {
                    combo.Indexs.Add(Convert.ToInt16(sArray[3 + i]));
                }

                combo.Comment = sArray[3 + WireNum + 1 + 1].ToString();
            }
            else if(Type == TowerType.LineCornerTower)
            {
                combo.para1 = Convert.ToBoolean(sArray[1].ToString());
                combo.para2 = sArray[2];
                combo.para3 = sArray[3];
                combo.para4 = sArray[4];

                for (int i = 1; i <= WireNum; i++)
                {
                    combo.Indexs.Add(Convert.ToInt16(sArray[4 + i]));
                }

                combo.Comment = sArray[4 + WireNum + 1 + 1].ToString();
            }
            else
            {
                combo.para1 = Convert.ToBoolean(sArray[1].ToString());
                combo.para2 = sArray[2];
                combo.para3 = sArray[3];
                combo.para4 = sArray[4];
                combo.para5 = sArray[5];

                for (int i = 1; i <= WireNum; i++)
                {
                    combo.Indexs.Add(Convert.ToInt16(sArray[5 + i]));
                }

                combo.Comment = sArray[5 + WireNum + 1 + 1].ToString();
            }
        }
    }
}