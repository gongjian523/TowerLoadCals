using System.Data;
using System.Data.Odbc;
using System.IO;

namespace TowerLoadCals.Common
{
    /// <summary>
    /// 读写dbf文件的接口
    /// </summary>
    public class DbfUtils
    {
        public static DataTable ReadDbf(string filePath, string tableName)
        {
            string fileName = Path.GetFileName(filePath);
            filePath = Path.GetDirectoryName(filePath);

            OdbcConnection conn = null;
            string connectStr = "Driver={Microsoft dBASE Driver (*.dbf)}; Dbq=" + filePath;
            conn = new System.Data.Odbc.OdbcConnection(connectStr);
            string sql = "select * from " + fileName;
            System.Data.Odbc.OdbcDataAdapter da = new System.Data.Odbc.OdbcDataAdapter(sql, conn);
            conn.Open();
            DataTable dt = new DataTable();
            da.Fill(dt);
            conn.Close();
            return dt;
        }
    }
}
