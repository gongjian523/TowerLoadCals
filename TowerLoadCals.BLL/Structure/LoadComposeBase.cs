using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Reflection;
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

        //挂点参数
        protected List<HangingPointParas> NormalXYPoints { get; set; }
        protected List<HangingPointParas> NormalZPoints { get; set; }
        protected List<HangingPointParas> InstallXYPoints { get; set; }
        protected List<HangingPointParas> InstallZPoints { get; set; }
        protected List<HangingPointParas> TurningPoints { get; set; }

        protected List<StruCalsDicGroup> DicGroup { get; set; }

        protected float[,] XX { get; set; }
        protected float[,] YY { get; set; }
        protected float[,] ZZ { get; set; }
 
        protected int wireNum;
        protected int groudWireNum;
        protected int workConditionNum;

        protected List<string> ProcessString { get; set; }
        protected List<string> Process2String { get; set; }

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

        public List<StruCalsPointLoad> CalsPointsLoad()
        {
            List<StruCalsPointLoad> pointsLoad = new List<StruCalsPointLoad>();

            int i = 0;
            foreach(var wireItem in Template.Wires)
            {
                int j=1
                foreach (var wdItem in Template.WorkConditionCombos)
                {
                    if(wireItem.Contains("地"))
                    {
                        HangingPointParas xyPara = NormalXYPoints.Where(item => item.WireType == wireItem).First();
                        int xyPNum = GetPointNum(xyPara);

                        StruCalsDicGroup xyGrp = DicGroup.Where(item => item.Group == "第一组" && item.Type == xyPara.StringType && item.Link == "[常规挂点XY向_dataTable]").First();
                        List<StruCalsDicComposeInfo> xyOption = xyGrp.Options.Where(item => item.Num == xyPNum).First().ComposrInfos;

                        HangingPointParas zPara = NormalZPoints.Where(item => item.WireType == wireItem).First();
                        int zPNum = GetPointNum(zPara);

                        StruCalsDicGroup zGrp = DicGroup.Where(item => item.Group == "第一组" && item.Type == zPara.StringType && item.Link == "[常规挂点Z向_dataTable]").First();
                        List<StruCalsDicComposeInfo> zOption = zGrp.Options.Where(item => item.Num == zPNum).First().ComposrInfos;

                        

                    }
                    else
                    {
                        
                    }
                    j++;
                }
                i++;
            }

            return pointsLoad;
        }


        protected int GetPointNum(HangingPointParas point)
        {
            int result = 0;
            //bool bOut = false ;
            Type pointType = point.GetType();

            for (int i = 0; i <= 8; i++)
            {
                //switch (i)
                //{
                //    case 1:
                //        if (point.Point1 != null && point.Point1 == "")
                //            result++;
                //        else
                //            bOut = true;
                //        break;
                //    case 2:
                //        if (point.Point2 != null && point.Point2 == "")
                //            result++;
                //        else
                //            bOut = true;
                //        break;
                //    case 3:
                //        if (point.Point3 != null && point.Point3 == "")
                //            result++;
                //        else
                //            bOut = true;
                //        break;
                //    case 4:
                //        if (point.Point4 != null && point.Point4 == "")
                //            result++;
                //        else
                //            bOut = true;
                //        break;
                //    case 5:
                //        if (point.Point5 != null && point.Point5 == "")
                //            result++;
                //        else
                //            bOut = true;
                //        break;
                //    case 6:
                //        if (point.Point6 != null && point.Point6 == "")
                //            result++;
                //        else
                //            bOut = true;
                //        break;
                //    case 7:
                //        if (point.Point7 != null && point.Point7 == "")
                //            result++;
                //        else
                //            bOut = true;
                //        break;
                //    case 8:
                //        if (point.Point8 != null && point.Point8 == "")
                //            result++;
                //        else
                //            bOut = true;
                //        break;
                //}

                //if (bOut)
                //    break;

                PropertyInfo piontPro = pointType.GetProperty("Point" + i.ToString(), BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public);
                if (piontPro != null)
                {
                    var pointName = piontPro.GetValue(point);

                    if (pointName != null && (string)pointName != "")
                        result++;
                    else
                        break;

                }
            }

            return result;
        }

    }
}
