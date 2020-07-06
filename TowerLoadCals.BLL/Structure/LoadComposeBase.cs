using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Reflection;
using TowerLoadCals.Common;
using TowerLoadCals.DAL;
using TowerLoadCals.Mode;

namespace TowerLoadCals.BLL
{
    abstract public class LoadComposeBase
    {
        /// <summary>
        /// 从界面获取的公共参数
        /// </summary>
        protected StruCalseBaseParas Paras { get; set; }

        /// <summary>
        /// 从界面获取的线参数
        /// </summary>
        protected StruLineParas[] LineParasArr { get; set; }

        //这个参数用于线荷载分配，最好不要用于挂点分配
        protected StruLineParas LineParas { get; set; }

        /// <summary>
        /// 模板参数
        /// </summary>
        protected TowerTemplate Template { get; set; }

        protected HangingPointSettingParas HPSettingParas { get; set; }

        protected List<StruCalsDicGroup> DicGroup { get; set; }

        protected List<LoadDic> LoadDics { get; set; }

        protected float[,] XX { get; set; }
        protected float[,] YY { get; set; }
        protected float[,] ZZ { get; set; }

        protected int wireNum;
        protected int groudWireNum;
        protected int workConditionNum;

        protected List<string> ProcessString { get; set; }
        protected List<string> Process2String { get; set; }

        //下面三个数组保存的是从电气荷载文件读取的工况，
        //用于生成Load的输出文件
        protected float[] WorkConditionWind;
        protected float[] WorkConditionTemperate;
        protected float[] WorkConditionIceThickness;

        public LoadComposeBase(StruCalseBaseParas para, StruLineParas[] lineParas, HangingPointSettingParas hpParas, TowerTemplate template, string tablePath)
        {
            ProcessString = new List<string>();
            Process2String = new List<string>();

            Paras = para;
            LineParasArr = lineParas;
            HPSettingParas = hpParas;
            Template = template;

            DicGroup = StruLoadComposeDicReader.Read(GetDicPath());
            LoadDics = StruLoadComposeDicReader.DicRead(GetLoadDicPath());

            wireNum = Template.Wires.Count;
            groudWireNum = Template.Wires.Where(item => item.Contains("地")).Count();
            workConditionNum = Template.WorkConditongs.Count;

            DataSet ds = ExcelUtils.ReadExcel(tablePath);
            ReadWorkCondition(ds);
            ReadElectricLoad(ds);
        }

        //protected DataSet ReadExcel(string path)
        //{
        //    string strConn = "Provider=Microsoft.Ace.OLEDB.12.0;Data Source=" + path + ";Extended Properties=Excel 12.0";
        //    OleDbConnection conn = new OleDbConnection(strConn);

        //    conn.Open();

        //    DataTable dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
        //    string sheetName = dt.Rows[0]["TABLE_NAME"].ToString().Trim();//sheet默认排序
        //    string strExcel = string.Format("select * from [{0}]", sheetName);

        //    OleDbDataAdapter myCommand = new OleDbDataAdapter(strExcel, strConn);

        //    DataSet ds = new DataSet();
        //    myCommand.Fill(ds);

        //    return ds;
        //}

        abstract protected void ReadElectricLoad(DataSet ds);

        public List<StruCalsPointLoad> LoadCaculate(string path)
        {
            CalculateLoadDistribute(path + HPSettingParas.HangingPointSettingName + ".calc");

            var loadList = CalsPointsLoad(path + HPSettingParas.HangingPointSettingName + ".div");

            GenerateLoadFile(path + HPSettingParas.HangingPointSettingName + ".load", loadList);

            return loadList;
        }

        abstract public void CalculateLoadDistribute(string path);

        public virtual List<StruCalsPointLoad> CalsPointsLoad(string path)
        {
            List<StruCalsPointLoad> pointsLoad = new List<StruCalsPointLoad>();

            Process2String.Add("荷载分配明细表 生成时间: " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
            Process2String.Add(" ");

            int i = 0;
            foreach (var wireItem in Template.Wires)
            {
                int j = 0;
                foreach (var wdItem in Template.WorkConditionCombos)
                {
                    string groupStr, linkStrXY, linkStrZ;
                    List<HangingPointParas> pointsXY, pointsZ;

                    if (wireItem.Contains("地"))
                    {
                        groupStr = "第一组";
                        linkStrXY = "[常规挂点XY向_dataTable]";
                        linkStrZ = "[常规挂点Z向_dataTable]";
                        pointsXY = HPSettingParas.NormalXYPoints;
                        pointsZ = HPSettingParas.NormalZPoints;
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
                            pointsXY = HPSettingParas.NormalXYPoints;
                            pointsZ = HPSettingParas.NormalZPoints;
                        }
                        else if (wdItem.WorkConditionCode == "T")
                        {
                            if (Math.Abs(wdItem.WireIndexCodes[i]) < 10)
                                groupStr = "第二组";
                            else
                                groupStr = "第七组";
                            linkStrXY = "[常规挂点XY向_dataTable]";
                            linkStrZ = "[常规挂点Z向_dataTable]";
                            pointsXY = HPSettingParas.NormalXYPoints;
                            pointsZ = HPSettingParas.NormalZPoints;

                        }
                        //L;La;Lb;Lc;Ld;Le;Lf;Lg;Lh
                        else if (wdItem.WorkConditionCode.StartsWith("L"))
                        {
                            if (Math.Abs(wdItem.WireIndexCodes[i]) < 10)
                            {
                                groupStr = "第三组";
                                linkStrXY = "[常规挂点XY向_dataTable]";
                                linkStrZ = "[常规挂点Z向_dataTable]";
                                pointsXY = HPSettingParas.NormalXYPoints;
                                pointsZ = HPSettingParas.NormalZPoints;
                            }
                            else
                            {
                                groupStr = "第四组";
                                linkStrXY = "[吊装挂点XY向_dataTable]";
                                linkStrZ = "[吊装挂点Z向_dataTable]";
                                pointsXY = HPSettingParas.InstallXYPoints;
                                pointsZ = HPSettingParas.InstallZPoints;

                            }
                        }
                        //C;Ca;Cb;Cc;Cd;Ce;Cf;Cg;Ch;CL;CLa;CLb;CLc;CLd;CLe;CLf;CL;CLa;CLb;CLc;CLd;CLe;CLf;CLg;CLh
                        else if (wdItem.WorkConditionCode.StartsWith("C"))
                        {
                            if (Math.Abs(wdItem.WireIndexCodes[i]) < 10 || (wdItem.WorkConditionCode == "C" || wdItem.WorkConditionCode == "CL"))
                            {
                                groupStr = "第三组";
                                linkStrXY = "[常规挂点XY向_dataTable]";
                                linkStrZ = "[常规挂点Z向_dataTable]";
                                pointsXY = HPSettingParas.NormalXYPoints;
                                pointsZ = HPSettingParas.NormalZPoints;
                            }
                            else
                            {
                                groupStr = "第四组";
                                linkStrXY = "[吊装挂点XY向_dataTable]";
                                linkStrZ = "[吊装挂点Z向_dataTable]";
                                pointsXY = HPSettingParas.InstallXYPoints;
                                pointsZ = HPSettingParas.InstallZPoints;

                            }
                        }
                        //G;Ga;Gb;Gc;Gd;Ge;Gf;Gh;GL;GLa;GLb;GLc;GLd;GLe;GLf;GLg
                        else if (wdItem.WorkConditionCode.StartsWith("G"))
                        {
                            if (Math.Abs(wdItem.WireIndexCodes[i]) < 10)
                            {
                                if (wdItem.WorkConditionCode == "G" || wdItem.WorkConditionCode == "GL")
                                {
                                    groupStr = "第三组";
                                    linkStrXY = "[常规挂点XY向_dataTable]";
                                    linkStrZ = "[常规挂点Z向_dataTable]";
                                    pointsXY = HPSettingParas.NormalXYPoints;
                                    pointsZ = HPSettingParas.NormalZPoints;
                                }
                                else
                                {
                                    groupStr = "第六组";
                                    linkStrXY = "[吊装挂点XY向_dataTable]";
                                    linkStrZ = "[吊装挂点XY向_dataTable]";
                                    pointsXY = HPSettingParas.InstallXYPoints;
                                    pointsZ = HPSettingParas.InstallXYPoints;
                                }

                            }
                            else
                            {
                                //只有直线塔和直转塔会使用基类
                                if (Paras.Type == TowerType.LineTower)
                                {
                                    //直线塔
                                    if (Math.Abs(wdItem.WireIndexCodes[i]) < 1000)
                                        groupStr = "第五组";
                                    else
                                        groupStr = "第六组";

                                    if (wdItem.WorkConditionCode == "G" || wdItem.WorkConditionCode == "GL")
                                    {
                                        linkStrXY = "[常规挂点XY向_dataTable]";
                                        linkStrZ = "[常规挂点XY向_dataTable]";
                                        pointsXY = HPSettingParas.NormalXYPoints;
                                        pointsZ = HPSettingParas.NormalXYPoints;
                                    }
                                    else
                                    {
                                        linkStrXY = "[吊装挂点XY向_dataTable]";
                                        linkStrZ = "[吊装挂点XY向_dataTable]";
                                        pointsXY = HPSettingParas.InstallXYPoints;
                                        pointsZ = HPSettingParas.InstallXYPoints;
                                    }

                                }
                                else
                                {
                                    //直转塔
                                    groupStr = "第七组";
                                    linkStrXY = "[常规挂点XY向_dataTable]";
                                    linkStrZ = "[常规挂点Z向_dataTable]";
                                    pointsXY = HPSettingParas.NormalXYPoints;
                                    pointsZ = HPSettingParas.NormalZPoints;
                                }

                            }
                        }
                        //M;Ma;Mb;Mc;Md;Me;Mf;Mg;Mh
                        else if (wdItem.WorkConditionCode.StartsWith("M") && !wdItem.WorkConditionCode.StartsWith("MO"))
                        {
                            groupStr = "第五组";
                            if (wdItem.WorkConditionCode == "M")
                            {
                                linkStrXY = "[常规挂点XY向_dataTable]";
                                linkStrZ = "[常规挂点XY向_dataTable]";
                                pointsXY = HPSettingParas.NormalXYPoints;
                                pointsZ = HPSettingParas.NormalXYPoints;
                            }
                            else
                            {
                                linkStrXY = "[吊装挂点XY向_dataTable]";
                                linkStrZ = "[吊装挂点XY向_dataTable]";
                                pointsXY = HPSettingParas.InstallXYPoints;
                                pointsZ = HPSettingParas.InstallXYPoints;
                            }
                        }
                        // MO;MOa;MOb;MOc;MOd;MOe;MOf;MOh
                        else
                        {
                            groupStr = "第二组";
                            linkStrXY = "[常规挂点XY向_dataTable]";
                            linkStrZ = "[常规挂点Z向_dataTable]";
                            pointsXY = HPSettingParas.NormalXYPoints;
                            pointsZ = HPSettingParas.NormalZPoints;
                        }
                    }

                    //string wireType = wireItem.Contains("地") ? "地线": "导线";
                    //GetDicInfo(wireType, wdItem.WorkConditionCode, wdItem.WireIndexCodes[i], out string groupStr, out string linkStrXY, out string linkStrZ, out List<HangingPointParas> pointsXY, out List<HangingPointParas> pointsZ);

                    HangingPointLoadComposeBase hPLoadComposeX = new HangingPointLoadComposeBase(i, j, "X", XX, YY, ZZ, groupStr, linkStrXY, pointsXY, HPSettingParas, Template, DicGroup);
                    hPLoadComposeX.ComposeHangingPointsLoad(out string strX, out List<StruCalsPointLoad> pListX);

                    Process2String.Add(strX);
                    pointsLoad.AddRange(pListX);

                    HangingPointLoadComposeBase hPLoadComposeY = new HangingPointLoadComposeBase(i, j, "Y", XX, YY, ZZ, groupStr, linkStrXY, pointsXY, HPSettingParas, Template, DicGroup);
                    hPLoadComposeY.ComposeHangingPointsLoad(out string strY, out List<StruCalsPointLoad> pListY);

                    Process2String.Add(strY);
                    pointsLoad.AddRange(pListY);

                    HangingPointLoadComposeBase hPLoadComposeZ = new HangingPointLoadComposeBase(i, j, "Z", XX, YY, ZZ, groupStr, linkStrZ, pointsZ, HPSettingParas, Template, DicGroup);
                    hPLoadComposeZ.ComposeHangingPointsLoad(out string strZ, out List<StruCalsPointLoad> pListZ);

                    Process2String.Add(strZ);
                    pointsLoad.AddRange(pListZ);

                    j++;
                }
                i++;
            }

            TextUtils.TextSaveByLine(path, Process2String);

            return pointsLoad;
        }

        public void GenerateLoadFile(string path, List<StruCalsPointLoad> pointLoads)
            {
            List<string> processString = new List<string>();

            StruCalsLib libParas = GlobalInfo.GetInstance().GetStruCalsLibParas();
            if (libParas == null)
                return;

            if (libParas.IceCoverParas == null || libParas.IceCoverParas.Count == 0)
                return;

            #region 保存工况
            for(int i = 0; i < Template.WorkConditionCombos.Count; i++)
                {
                WorkConditionCombo wcCombo = Template.WorkConditionCombos[i];

                float wcWind = WorkConditionWind[wcCombo.WorkCode-1];

                float wdIceThickness = WorkConditionIceThickness[wcCombo.WorkCode - 1];

                float windLoad = 1, gravityLoad = 1;
                
                if(libParas.IceCoverParas.Where(item => item.IceThickness == wdIceThickness).Count() > 0)
                    {
                    windLoad = libParas.IceCoverParas.Where(item => item.IceThickness == wdIceThickness).First().TowerWindLoadAmplifyCoef;
                    gravityLoad = libParas.IceCoverParas.Where(item => item.IceThickness == wdIceThickness).First().TowerGravityLoadAmplifyCoef;
                    }

                //Todo
                float comboCoef = Paras.VcFNormal * windLoad;

                float windAngle = wcCombo.WindDirectionCode;

                //Todo
                float importanceCoef = Paras.R1Install;

                float temperature = WorkConditionTemperate[wcCombo.WorkCode - 1];

                string str = wcWind.ToString("0.000").PadLeft(8) + comboCoef.ToString("0.000").PadLeft(8) + windAngle.ToString("0.000").PadLeft(8)
                    + importanceCoef.ToString("0.000").PadLeft(8) + gravityLoad.ToString("0.000").PadLeft(8) + temperature.ToString("0.000").PadLeft(8)
                    + "   " + (i + 1).ToString() + "-" + wcCombo.WorkComment;

                processString.Add(str);

        }

            #endregion

            #region 保存挂点荷载
            List<int> points = pointLoads.Select(p => p.Name).Distinct().ToList();

            points.Sort();

            foreach (var point in points)
            {
                for (int j = 0; j < Template.WorkConditionCombos.Count; j++)
                {
                    string str = (j == 0) ? point.ToString().PadLeft(9) : (" ").PadLeft(9);
                    float xLoad = pointLoads.Where(p => p.Name == point && p.WorkConditionId == j && p.Orientation == "X").Sum(p => p.Load);
                    float yLoad = pointLoads.Where(p => p.Name == point && p.WorkConditionId == j && p.Orientation == "Y").Sum(p => p.Load);
                    float zLoad = pointLoads.Where(p => p.Name == point && p.WorkConditionId == j && p.Orientation == "Z").Sum(p => p.Load);

                    str += xLoad.ToString("0.00").PadLeft(14) + yLoad.ToString("0.00").PadLeft(9) + zLoad.ToString("0.00").PadLeft(9);
                    processString.Add(str);
                }
            }
            #endregion

            TextUtils.TextSaveByLine(path, processString);
                    }

        protected virtual string GetDicPath()
        {
            return Directory.GetCurrentDirectory() + "\\UserData\\HPCompose-LineTower.xml";
        }

        protected virtual string GetLoadDicPath()
        {
            return Directory.GetCurrentDirectory() + "\\UserData\\HPDic-LineTower.xml";
        }


        protected virtual void GetDicInfo(string wire, string WorkConditionCode, int WireIndexCodes, out string groupStr, out string linkStrXY, out string linkStrZ, out List<HangingPointParas> pointsXY, out List<HangingPointParas> pointsZ)
        {
            LoadDic dic;

            var CandiateList1 = LoadDics.Where(item => item.Wire == wire).ToList();


            //if(CandiateList1.Count == 1)
            {
                groupStr = CandiateList1[0].Group;
                linkStrXY = CandiateList1[0].LinkXY;
                linkStrZ = CandiateList1[0].LinkZ;
                pointsXY = HPString2List(CandiateList1[0].PointXY);
                pointsZ = HPString2List(CandiateList1[0].PointZ);
            }
        }

        protected List<HangingPointParas> HPString2List(string str)
        {
            return new List<HangingPointParas>();
        }

        /// <summary>
        /// 从电气荷载文件中读取工况参数
        /// </summary>
        /// <param name="ds"></param>
        protected void ReadWorkCondition(DataSet ds)
        {
            WorkConditionTemperate = new float[Template.WorkConditongs.Count];
            WorkConditionWind = new float[Template.WorkConditongs.Count];
            WorkConditionIceThickness = new float[Template.WorkConditongs.Count];

            for (int i = 0; i < Template.WorkConditongs.Count; i++)
            {
                object obj = ds.Tables[0].Rows[0][i+1];
                float.TryParse(obj.ToString(), out WorkConditionTemperate[i]);

                object obj2 = ds.Tables[0].Rows[1][i+1];
                float.TryParse(obj.ToString(), out WorkConditionWind[i]);

                object obj3 = ds.Tables[0].Rows[1][i+1];
                float.TryParse(obj.ToString(), out WorkConditionIceThickness[i]);
    }
        }
    }
}
 
