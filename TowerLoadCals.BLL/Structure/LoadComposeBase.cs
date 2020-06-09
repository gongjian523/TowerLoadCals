using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using TowerLoadCals.Mode;

namespace TowerLoadCals.BLL
{
    abstract public class LoadComposeBase
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

        protected float[,] XX { get; set; }
        protected float[,] YY { get; set; }
        protected float[,] ZZ { get; set; }
 
        protected int wireNum;
        protected int groudWireNum;
        protected int workConditionNum;

        protected List<string> ProcessString { get; set; }

        public LoadComposeBase(FormulaParas para, StruLineParas[] lineParas, TowerTemplate template, string tablePath)
        {
            ProcessString = new List<string>();

            Paras = para;
            LineParasArr = lineParas;
            Template = template;

            wireNum = Template.Wires.Count;
            groudWireNum = Template.Wires.Where(item => item.Contains("地")).Count();
            workConditionNum = Template.WorkConditongs.Count;

            GetTable(ReadExcel(tablePath));
        }

        protected DataSet ReadExcel(string path)
        {
            string strConn = "Provider=Microsoft.Ace.OLEDB.12.0;Data Source=" + path + ";Extended Properties=Excel 12.0";
            OleDbConnection conn = new OleDbConnection(strConn);
            //conn.Open();
            //string strExcel = "select * from [sheet1$]";
            //OleDbDataAdapter myCommand = new OleDbDataAdapter(strExcel, strConn);
            //DataSet ds = new DataSet();
            //myCommand.Fill(ds, "table1");

            conn.Open();

            DataTable dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            string sheetName = dt.Rows[0]["TABLE_NAME"].ToString().Trim();//sheet默认排序
            string strExcel = string.Format("select * from [{0}]", sheetName);

            OleDbDataAdapter myCommand = new OleDbDataAdapter(strExcel, strConn);

            DataSet ds = new DataSet();
            myCommand.Fill(ds);

            return ds;
        }

        abstract protected void GetTable(DataSet ds);

        abstract public void CalculateLoadDistribute(out float[,] xx, out float[,] yy, out float[,] zz, string path);

    }
}
