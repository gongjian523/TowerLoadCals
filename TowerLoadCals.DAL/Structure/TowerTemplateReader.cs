﻿using System;
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
                WireIndexCodes = new List<int>()
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
                combo.WireIndexCodes.Add(Convert.ToInt16(aWords[iIndex + i]));
            }

            iIndex++;
            combo.WorkCode = aWords[iIndex + WireNum].ToString();
            iIndex++;
            combo.WorkComment = aWords[iIndex + WireNum].ToString();
        }

        static public List<WorkConditionComboSpec> ConvertTemplateToSpec(TowerTemplate template)
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


                for (int i = 1; i <= item.WireIndexCodes.Count; i++)
                {
                    Type specType = spec.GetType();
                    PropertyInfo specPro = specType.GetProperty("Wire" + i.ToString(), BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public);
                    if (specPro != null)
                        specPro.SetValue(spec, item.WireIndexCodes[i - 1]);
                }

                spec.WorkComment = item.WorkComment;

                listSpec.Add(spec);
            }

            return listSpec;
        }


        static public void ConvertSpeToWorkCondition(TowerTemplate template, List<WorkConditionComboSpec> workConditionSpecs)
        {
            List<WorkConditionCombo> workConditions = new List<WorkConditionCombo>();

            foreach (var item in workConditionSpecs)
            {
                WorkConditionCombo wcc= new WorkConditionCombo();

                wcc.Index = item.Index;
                wcc.IsCalculate = item.IsCalculate;
                wcc.WorkConditionCode = item.WorkConditionCode;
                if (item.TensionAngleCode != null)
                    wcc.TensionAngleCode = item.TensionAngleCode;
                if (item.VertialLoadCode != null)
                    wcc.VertialLoadCode = item.VertialLoadCode;
                wcc.WindDirectionCode = item.WindDirectionCode;
                wcc.WorkCode = item.WorkCode;

                int wireIndexCodesNum = template.WorkConditionCombos[0].WireIndexCodes.Count;
                wcc.WireIndexCodes = new List<int>();

                //if (wireIndexCodesNum >= 1)
                //    wcc.WireIndexCodes.Add(item.Wire1);
                //if (wireIndexCodesNum >= 2)
                //    wcc.WireIndexCodes.Add(item.Wire2);
                //if (wireIndexCodesNum >= 3)
                //    wcc.WireIndexCodes.Add(item.Wire3);
                //if (wireIndexCodesNum >= 4)
                //    wcc.WireIndexCodes.Add(item.Wire4);
                //if (wireIndexCodesNum >= 5)
                //    wcc.WireIndexCodes.Add(item.Wire5);
                //if (wireIndexCodesNum >= 6)
                //    wcc.WireIndexCodes.Add(item.Wire6);
                //if (wireIndexCodesNum >= 7)
                //    wcc.WireIndexCodes.Add(item.Wire7);
                //if (wireIndexCodesNum >= 8)
                //    wcc.WireIndexCodes.Add(item.Wire8);
                //if (wireIndexCodesNum >= 9)
                //    wcc.WireIndexCodes.Add(item.Wire9);
                //if (wireIndexCodesNum >= 10)
                //    wcc.WireIndexCodes.Add(item.Wire10);
                //if (wireIndexCodesNum >= 11)
                //    wcc.WireIndexCodes.Add(item.Wire11);
                //if (wireIndexCodesNum >= 12)
                //    wcc.WireIndexCodes.Add(item.Wire12);
                //if (wireIndexCodesNum >= 13)
                //    wcc.WireIndexCodes.Add(item.Wire13);
                //if (wireIndexCodesNum >= 14)
                //    wcc.WireIndexCodes.Add(item.Wire14);
                //if (wireIndexCodesNum >= 15)
                //    wcc.WireIndexCodes.Add(item.Wire15);
                //if (wireIndexCodesNum >= 16)
                //    wcc.WireIndexCodes.Add(item.Wire16);

                for (int i = 1; i <= wireIndexCodesNum; i++)
                {
                    Type itemType = item.GetType();
                    PropertyInfo itemPro = itemType.GetProperty("Wire" + i.ToString(), BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public);
                    if (itemPro != null)
                        wcc.WireIndexCodes.Add(Convert.ToInt16(itemPro.GetValue(item, null)));
                }


                wcc.WorkComment = item.WorkComment;

                workConditions.Add(wcc);
            }

            template.WorkConditionCombos = workConditions;
        }
    }
}