using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.Readers
{
    public class DbfReader
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
