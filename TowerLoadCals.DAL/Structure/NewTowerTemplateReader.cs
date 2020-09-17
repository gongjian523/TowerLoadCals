using System.Collections.Generic;
using System.IO;
using TowerLoadCals.Common;
using TowerLoadCals.Mode;

namespace TowerLoadCals.DAL
{
    public class NewTowerTemplateReader: TowerTemplateReader
    {

        protected override int InstructionLine {
            get { return 0; }
        }

        protected override int WireLine { 
            get { return 2; }
        }

        protected override int WorkConditongsLine {
            get { return 4; }
        }
    
        protected override int WorkConditongsComboStartLine {
            get { return 6; }
        }

        public NewTowerTemplateReader(TowerTypeEnum type):base()
        {
            Type = type;

            template.TowerType = TowerTypeStringConvert.TowerTypeToString(type);
            template.Wires = new List<string>();
            template.WorkConditongs = new Dictionary<int, string>();
            template.WorkConditionCombos = new List<WorkConditionCombo>();
        }

        protected override bool IsWorkConditionComboLine(int comboNum)
        {
            //新模板中以工况组合结尾，可以默认为是工况组合行，故一直返回true
            return true;
        }

        public void Save(string path, TowerTemplate template)
        {
            List<string> strs = new List<string>();

            strs.Add("线条：//“地”即为地线，“导”即为导线，跳线随导线，且“左”，“右”具有实际意义");

            string wireStr = "";
            foreach(var wire in template.Wires)
            {
                wireStr += wire + "  ";
            }
            strs.Add(wireStr);

            strs.Add("气象条件：//大风为第一要素工况，其余随意");
            string wordCdStr = "";
            for(int i = 1; i <= template.WorkConditongs.Count; i++)
            {
                wordCdStr += i.ToString()+"." + template.WorkConditongs[i]+"  ";
            }
            strs.Add(wordCdStr);

            strs.Add("计算工况");
            foreach (var combo in template.WorkConditionCombos)
            {
                EncodeWorkCondition(out string comboStr, combo);
                strs.Add(comboStr);
            }

            if (File.Exists(path))
                File.Delete(path);

            FileUtils.TextSaveByLine(path, strs);
        }
    }
}