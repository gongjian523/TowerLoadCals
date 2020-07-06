using System.Data;
using System.Data.OleDb;

namespace TowerLoadCals.Common
{
    public class ExcelUtils
    {
        public static  DataSet ReadExcel(string path)
        {
            string strConn = "Provider=Microsoft.Ace.OLEDB.12.0;Data Source=" + path + ";Extended Properties=Excel 12.0";
            OleDbConnection conn = new OleDbConnection(strConn);

            conn.Open();

            DataTable dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            string sheetName = dt.Rows[0]["TABLE_NAME"].ToString().Trim();//sheet默认排序
            string strExcel = string.Format("select * from [{0}]", sheetName);

            OleDbDataAdapter myCommand = new OleDbDataAdapter(strExcel, strConn);

            DataSet ds = new DataSet();
            myCommand.Fill(ds);

            conn.Close();

            return ds;
        }
    }
}
