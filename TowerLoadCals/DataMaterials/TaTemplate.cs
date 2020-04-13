using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TowerLoadCals.DataMaterials
{
    public class WorkConditionCombo
    {
        /// <summary>
        /// 
        /// </summary>
        public bool para1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string para2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string para3 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string para4 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string para5 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<int> Indexs {get; set;}

        /// <summary>
        /// 注释
        /// </summary>
        public string Comment { get; set; }
    }


    public class TaTemplate
    {
        /// <summary>
        /// 塔模板名字
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 塔型：直线塔、直线转角塔、转角塔、终端塔、分支塔
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 线名
        /// </summary>
        public List<string>  Wires { get; set; }

        /// <summary>
        /// 工况名列表
        /// </summary>
        public Dictionary<int, string>  WorkConditongs { get; set; }

        /// <summary>
        /// 工况组合列表
        /// </summary>
        public List<WorkConditionCombo>  WorkConditionCombos { get; set; }
    }






}