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
        /// 从界面获取的线参数
        /// </summary>
        protected StruLineParas[] LineParasArr { get; set; }
        protected StruLineParas LineParas { get; set; }

        /// <summary>
        /// 模板参数
        /// </summary>
        protected TowerTemplate Template { get; set; }

        protected StruRatioParas RatioParas { get; set; }

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

        public LoadComposeBase(FormulaParas para, StruLineParas[] lineParas, StruRatioParas ratioParas, TowerTemplate template, string tablePath)
        {
            ProcessString = new List<string>();
            Process2String = new List<string>();

            Paras = para;
            LineParasArr = lineParas;
            RatioParas = ratioParas;
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

            Process2String.Add("荷载分配明细表 生成时间: " + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"));

            int j = 1;
            foreach(var wdItem in Template.WorkConditionCombos)
            {
                int i = 0;
                foreach (var wireItem in Template.Wires)
                {
                    string groupStr, linkStrXY, linkStrZ;
                    List<HangingPointParas> pointsXY, pointsZ;


                    if (wireItem.Contains("地"))
                    {
                        groupStr = "第一组";
                        linkStrXY = "[常规挂点XY向_dataTable]";
                        linkStrZ = "[常规挂点Z向_dataTable]";
                        pointsXY = NormalXYPoints;
                        pointsZ = NormalZPoints;
                    }
                    else
                    {
                        //N1;N2;D1;D2;I1;I2;U1;U2;B1;B2;Y1;Y2;
                        if (wdItem.WorkConditionCode.StartsWith("N") || wdItem.WorkConditionCode.StartsWith("D") || wdItem.WorkConditionCode.StartsWith("I")
                            || wdItem.WorkConditionCode.StartsWith("U") || wdItem.WorkConditionCode.StartsWith("B") || wdItem.WorkConditionCode.StartsWith("Y"))
                        {
                            groupStr = "第二组";
                            linkStrXY = "[常规挂点XY向_dataTable]";
                            linkStrZ = "[常规挂点Z向_dataTable]";
                            pointsXY = NormalXYPoints;
                            pointsZ = NormalZPoints;
                        }
                        else if (wdItem.WorkConditionCode == "T")
                        {
                            if (Math.Abs(wdItem.WireIndexCodes[j]) < 10)
                                groupStr = "第二组";
                            else
                                groupStr = "第七组";
                            linkStrXY = "[常规挂点XY向_dataTable]";
                            linkStrZ = "[常规挂点Z向_dataTable]";
                            pointsXY = NormalXYPoints;
                            pointsZ = NormalZPoints;

                        }
                        //L;La;Lb;Lc;Ld;Le;Lf;Lg;Lh
                        else if (wdItem.WorkConditionCode.StartsWith("L"))
                        {
                            if (Math.Abs(wdItem.WireIndexCodes[j]) < 10)
                            {
                                groupStr = "第三组";
                                linkStrXY = "[常规挂点XY向_dataTable]";
                                linkStrZ = "[常规挂点Z向_dataTable]";
                                pointsXY = NormalXYPoints;
                                pointsZ = NormalZPoints;

                            }
                            else
                            {
                                groupStr = "第四组";
                                linkStrXY = "[吊装挂点XY向_dataTable]";
                                linkStrZ = "[吊装挂点Z向_dataTable]";
                                pointsXY = InstallXYPoints;
                                pointsZ = InstallZPoints;

                            }
                        }
                        //C;Ca;Cb;Cc;Cd;Ce;Cf;Cg;Ch;CL;CLa;CLb;CLc;CLd;CLe;CLf;CL;CLa;CLb;CLc;CLd;CLe;CLf;CLg;CLh
                        else if (wdItem.WorkConditionCode.StartsWith("C"))
                        {
                            if (Math.Abs(wdItem.WireIndexCodes[j]) < 10)
                            {
                                groupStr = "第三组";
                                linkStrXY = "[常规挂点XY向_dataTable]";
                                linkStrZ = "[常规挂点Z向_dataTable]";
                                pointsXY = NormalXYPoints;
                                pointsZ = NormalZPoints;

                            }
                            else
                            {
                                groupStr = "第四组";
                                linkStrXY = "[吊装挂点XY向_dataTable]";
                                linkStrZ = "[吊装挂点Z向_dataTable]";
                                pointsXY = InstallXYPoints;
                                pointsZ = InstallZPoints;

                            }
                        }
                        //G;Ga;Gb;Gc;Gd;Ge;Gf;Gh;GL;GLa;GLb;GLc;GLd;GLe;GLf;GLg
                        else if (wdItem.WorkConditionCode.StartsWith("G"))
                        {
                            if (Math.Abs(wdItem.WireIndexCodes[j]) < 10)
                            {
                                groupStr = "第三组";
                                linkStrXY = "[常规挂点XY向_dataTable]";
                                linkStrZ = "[常规挂点Z向_dataTable]";
                                pointsXY = NormalXYPoints;
                                pointsZ = NormalZPoints;

                            }
                            else
                            {
                                if (Math.Abs(wdItem.WireIndexCodes[j]) < 1000)
                                    groupStr = "第五组";
                                else
                                    groupStr = "第六组";

                                if (wdItem.WorkConditionCode == "G" || wdItem.WorkConditionCode == "GL")
                                {
                                    linkStrXY = "[常规挂点XY向_dataTable]";
                                    linkStrZ = "[常规挂点Z向_dataTable]";
                                    pointsXY = NormalXYPoints;
                                    pointsZ = NormalZPoints;
                                }
                                else
                                {
                                    linkStrXY = "[吊装挂点XY向_dataTable]";
                                    linkStrZ = "[吊装挂点Z向_dataTable]";
                                    pointsXY = InstallXYPoints;
                                    pointsZ = InstallZPoints;
                                }

                            }
                        }
                        //M;Ma;Mb;Mc;Md;Me;Mf;Mg;Mh
                        if(wdItem.WorkConditionCode.StartsWith("M") && !wdItem.WorkConditionCode.StartsWith("MO"))
                        {
                            groupStr = "第五组";
                            if (wdItem.WorkConditionCode == "M")
                            {
                                linkStrXY = "[常规挂点XY向_dataTable]";
                                linkStrZ = "[常规挂点Z向_dataTable]";
                                pointsXY = NormalXYPoints;
                                pointsZ = NormalZPoints;
                            }
                            else
                            {
                                linkStrXY = "[吊装挂点XY向_dataTable]";
                                linkStrZ = "[吊装挂点Z向_dataTable]";
                                pointsXY = InstallXYPoints;
                                pointsZ = InstallZPoints;
                            }
                        }
                        // MO;MOa;MOb;MOc;MOd;MOe;MOf;MOh
                        else
                        {
                            groupStr = "第二组";
                            linkStrXY = "[常规挂点XY向_dataTable]";
                            linkStrZ = "[常规挂点Z向_dataTable]";
                            pointsXY = NormalXYPoints;
                            pointsZ = NormalZPoints;
                        }
                    }

                    HangingPointLoadComposeBase hPLoadComposeX = new HangingPointLoadComposeBase(i, j, "X", XX, groupStr, linkStrXY, pointsXY, RatioParas, DicGroup);
                    hPLoadComposeX.ComposeHangingPointsLoad(out string strX, out List<StruCalsPointLoad> pListX);

                    Process2String.Add(strX);
                    pointsLoad.AddRange(pListX);

                    HangingPointLoadComposeBase hPLoadComposeY = new HangingPointLoadComposeBase(i, j, "Y", YY, groupStr, linkStrXY, pointsXY, RatioParas, DicGroup);
                    hPLoadComposeY.ComposeHangingPointsLoad(out string strY, out List<StruCalsPointLoad> pListY);

                    Process2String.Add(strY);
                    pointsLoad.AddRange(pListY);
                    
                    HangingPointLoadComposeBase hPLoadComposeZ = new HangingPointLoadComposeBase(i, j, "Z", ZZ, groupStr, linkStrZ, pointsZ, RatioParas, DicGroup);
                    hPLoadComposeZ.ComposeHangingPointsLoad(out string strZ, out List<StruCalsPointLoad> pListZ);

                    Process2String.Add(strZ);
                    pointsLoad.AddRange(pListZ);

                    i++;
                }
                j++;
            }

            return pointsLoad;
        }

    }
}
