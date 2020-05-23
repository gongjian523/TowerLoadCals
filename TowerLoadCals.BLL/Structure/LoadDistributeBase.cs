using System.Collections.Generic;
using TowerLoadCals.Mode;

namespace TowerLoadCals.BLL
{
    abstract public class LoadDistributeBase
    {
        /// <summary>
        /// 从界面获取的公共参数
        /// </summary>
        protected FormulaParas Paras { get; set; }

        /// <summary>
        /// 从界面获取的先参数
        /// </summary>
        protected StruLineParas[] LineParasArr { get; set; }
        protected StruLineParas LineParas { get; set; }

        /// <summary>
        /// 模板参数
        /// </summary>
        protected TowerTemplate Template { get; set; }

        //protected float[,] Wind { get; set; }

        //protected float[,] GMax { get; set; }

        //protected float[,] GMin { get; set; }

        //protected float[,] TensionMax { get; set; }

        //protected float[,] TensionMin { get; set; }

        protected float[,] XX { get; set; }
        protected float[,] YY { get; set; }
        protected float[,] ZZ { get; set; }

        protected List<string> ProcessString { get; set; }

        protected FormulaLineTower formula;


        public LoadDistributeBase(FormulaParas para, StruLineParas[] lineParas, TowerTemplate template, float[][] table)
        {

            ProcessString = new List<string>();
            formula = new FormulaLineTower(para);

            Paras = para;
            LineParasArr = lineParas;
            Template = template;

            ConvertTable(table);
        }

        public LoadDistributeBase(FormulaParas para, StruLineParas[] lineParas, TowerTemplate template)
        {

            ProcessString = new List<string>();
            formula = new FormulaLineTower(para);

            Paras = para;
            LineParasArr = lineParas;
            Template = template;

            //GetTable();
        }

        abstract protected void GetTable(string path);

        abstract protected void ConvertTable(float [][] table);

        abstract public void CalculateLoadDistribute(out float[,] xx, out float[,] yy, out float[,] zz, string path);

    }
}
