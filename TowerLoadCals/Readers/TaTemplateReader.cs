using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TowerLoadCals.DataMaterials;

namespace TowerLoadCals.Readers
{
    public class TaTemplateReader
    {
        public TaTemplate template = new TaTemplate();

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

        protected String[] seperate = new string[]
        {
            " ",
            "  ",
            "   ",
            "    ",
            "     ",
            "      ",
            "       ",
            "        "
        };

        public TaTemplateReader(string type, int workConditongsLine, int workConditongsComboStartLine)
        {
            WorkConditongsLine = workConditongsLine;
            WorkConditongsComboStartLine = workConditongsComboStartLine;

            template.Type = type;
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
                    string[] sArray = line.Split(seperate, StringSplitOptions.RemoveEmptyEntries);
                    WireNum = Convert.ToInt16(sArray[0]);
                    WorkConditionNum = Convert.ToInt16(sArray[2]);
                    WorkConditonComboNum = Convert.ToInt16(sArray[3]);
                }
                else if (lineNum == WireLine)
                {
                    string[] sArray = line.Split(seperate, StringSplitOptions.RemoveEmptyEntries); ;
                    foreach(string str in sArray)
                    {
                        template.Wires.Add(str);
                    }
                }
                else if(lineNum  == WorkConditongsLine)
                {
                    string[] sArray = line.Split(seperate, StringSplitOptions.RemoveEmptyEntries); 

                    for(int i = 0; i < sArray.Count(); i++)
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


        virtual protected void DecodeWorkCondition(string line, out WorkConditionCombo combo)
        {
            combo = new WorkConditionCombo()
            {
                Indexs = new List<int>()
            };
        }

    }

    /// <summary>
    /// 直线塔模板解析
    /// </summary>
    public class LineTowerTemplateReader : TaTemplateReader
    {
        public LineTowerTemplateReader() : base("直线塔", 15, 17)
        {
            
        }

        override protected void  DecodeWorkCondition(string line, out WorkConditionCombo combo)
        {
            combo = new WorkConditionCombo()
            {
                Indexs = new List<int>()
            };

            string[] sArray = line.Split(seperate, StringSplitOptions.RemoveEmptyEntries); ;

            combo.para1 = Convert.ToBoolean(sArray[1].ToString());
            combo.para2 = sArray[2];
            combo.para3 = sArray[3];

            for (int i = 1; i <= WireNum; i++)
            {
                combo.Indexs.Add(Convert.ToInt16(sArray[3 + i]));
            }

            combo.Comment = sArray[3 + WireNum + 1 + 1].ToString();
        }
    }


    /// <summary>
    /// 直线转角塔模板解析
    /// </summary>
    public class LineCornerTowerTemplateReader : TaTemplateReader
    {
        public LineCornerTowerTemplateReader() : base("直线转角塔", 16, 18)
        {

        }

        override protected void DecodeWorkCondition(string line, out WorkConditionCombo combo)
        {
            combo = new WorkConditionCombo()
            {
                Indexs = new List<int>()
            };

            string[] sArray = line.Split(seperate, StringSplitOptions.RemoveEmptyEntries); ;

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
    }

    /// <summary>
    /// 转角塔模板解析
    /// </summary>
    public class CornerTowerTemplateReader : TaTemplateReader
    {
        public CornerTowerTemplateReader() : base("转角塔", 26, 29)
        {

        }

        override protected void DecodeWorkCondition(string line, out WorkConditionCombo combo)
        {
            combo = new WorkConditionCombo()
            {
                Indexs = new List<int>()
            };

            string[] sArray = line.Split(seperate, StringSplitOptions.RemoveEmptyEntries); ;

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

    /// <summary>
    /// 终端塔模板解析
    /// </summary>
    public class TerminalTowerTemplateReader : TaTemplateReader
    {
        public TerminalTowerTemplateReader() : base("终端塔", 26, 29)
        {

        }

        override protected void DecodeWorkCondition(string line, out WorkConditionCombo combo)
        {
            combo = new WorkConditionCombo()
            {
                Indexs = new List<int>()
            };

            string[] sArray = line.Split(seperate, StringSplitOptions.RemoveEmptyEntries); ;

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

    /// <summary>
    /// 分支塔模板解析
    /// </summary>
    public class BranchTowerTemplateReader : TaTemplateReader
    {
        public BranchTowerTemplateReader() : base("分支塔", 26, 29)
        {

        }

        override protected void DecodeWorkCondition(string line, out WorkConditionCombo combo)
        {
            combo = new WorkConditionCombo()
            {
                Indexs = new List<int>()
            };

            string[] sArray = line.Split(seperate, StringSplitOptions.RemoveEmptyEntries); ;

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