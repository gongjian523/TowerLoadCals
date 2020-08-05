using System.Data;
using System.Data.Odbc;

namespace TowerLoadCals.Common
{
    /// <summary>
    /// 读写dbf文件的接口
    /// </summary>
    public class DbfUtils
    {
        public static DataTable ReadDbf(string dir,string tableName)
        {
            OdbcConnection oConn = new System.Data.Odbc.OdbcConnection();
            oConn.ConnectionString = "Driver={Microsoft dBase Driver (*.dbf)};DefaultDir=" + dir;
            oConn.Open();
            OdbcDataAdapter odbcDataAdapt = new OdbcDataAdapter("select * from  " + tableName, oConn.ConnectionString);
            DataTable dtData = new DataTable();
            dtData.TableName = tableName;
            odbcDataAdapt.Fill(dtData);
            oConn.Close();
            return dtData;
        }
    }
}
