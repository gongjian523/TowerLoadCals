using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
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

        public LoadDistributeBase(FormulaParas para, StruLineParas[] lineParas, TowerTemplate template, string tablePath)
        {
            ProcessString = new List<string>();

            Paras = para;
            LineParasArr = lineParas;
            Template = template;

            GetTable(ReadExcel(tablePath));
        }

        protected DataSet ReadExcel(string path)
        {
            string strConn = "Provider=Microsoft.Ace.OLEDB.12.0;Data Source=" + path + ";" + "Extended Properties=Excel 12.0";
            OleDbConnection conn = new OleDbConnection(strConn);
            conn.Open();
            string strExcel = "";
            OleDbDataAdapter myCommand = null;
            DataSet ds = null;
            strExcel = "select * from [sheet1$]";
            myCommand = new OleDbDataAdapter(strExcel, strConn);
            ds = new DataSet();
            myCommand.Fill(ds, "table1");
            return ds;
        }

        abstract protected void GetTable(DataSet ds);

        abstract public void CalculateLoadDistribute(out float[,] xx, out float[,] yy, out float[,] zz, string path);

    }
}
