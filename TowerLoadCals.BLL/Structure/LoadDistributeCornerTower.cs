using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using TowerLoadCals.Mode;

namespace TowerLoadCals.BLL
{

    /// <summary>
    /// 耐脏塔的荷载分配
    /// </summary>
    public class LoadDistributeCornerTower:LoadDistributeBase
    {
        ///// <summary>
        ///// 从界面获取的公共参数
        ///// </summary>
        //protected FormulaParas Paras { get; set; }

        ///// <summary>
        ///// 从界面获取的先参数
        ///// </summary>
        //protected StruLineParas[] LineParasArr { get; set; }
        //protected StruLineParas LineParas { get; set; }

        ///// <summary>
        ///// 模板参数
        ///// </summary>
        //protected TowerTemplate Template { get; set; }

        protected float[,] Wind { get; set; }

        protected float[,] GMax { get; set; }

        protected float[,] GMin { get; set; }

        protected float[,] TensionMax { get; set; }

        protected float[,] TensionMin { get; set; }

        //protected float [,] XX { get; set; }
        //protected float[,] YY { get; set; }
        //protected float[,] ZZ { get; set; }

        //protected List<string>  ProcessString { get; set; }

        protected FormulaCornerTower formula;


        public LoadDistributeCornerTower(FormulaParas para, StruLineParas[] lineParas, TowerTemplate template, float[][] table) : base(para, lineParas, template, table)
        {

            //ProcessString = new List<string>();
            //formula = new FormulaLineTower(para);

            //Paras = para;
            //LineParasArr = lineParas;
            //Template = template;

            //ConvertTable(table);
            formula = new FormulaCornerTower(para);
        }

        public LoadDistributeCornerTower(FormulaParas para, StruLineParas[] lineParas, TowerTemplate template) : base(para, lineParas, template)
        {

            //ProcessString = new List<string>();
            //formula = new FormulaLineTower(para);

            //Paras = para;
            //LineParasArr = lineParas;
            //Template = template;
            formula = new FormulaCornerTower(para);
            GetTable("D:\\00-项目\\P-200325-杆塔负荷程序\\21.xlsx");
        }


        protected override void ConvertTable(float[][] table)
        {
            
        }

        protected override void GetTable(string path)
        {
            string strConn = "Provider=Microsoft.Ace.OLEDB.12.0;" + path + ";" + "Extended Properties=Excel 12.0;";
            OleDbConnection conn = new OleDbConnection(strConn);
            conn.Open();
            string strExcel = "";
            OleDbDataAdapter myCommand = null;
            DataSet ds = null;
            strExcel = "select * from [sheet1$]";
            myCommand = new OleDbDataAdapter(strExcel, strConn);
            ds = new DataSet();
            myCommand.Fill(ds, "table1");

            //后续计算的序号从1开始
            Wind = new float[Template.Wires.Count + 1, Template.WorkConditongs.Count + 1];
            GMax = new float[Template.Wires.Count + 1, Template.WorkConditongs.Count + 1];
            GMin = new float[Template.Wires.Count + 1, Template.WorkConditongs.Count + 1];
            TensionMax = new float[Template.Wires.Count + 1, Template.WorkConditongs.Count + 1];
            TensionMin = new float[Template.Wires.Count + 1, Template.WorkConditongs.Count + 1];


            for (int j = 1; j <= Template.WorkConditongs.Count; j++)
            {
                for (int i = 1; i <= Template.Wires.Count; i++)
                {
                    object obj = ds.Tables[0].Rows[4 + 6 * (i - 1)][j];
                    float.TryParse(obj.ToString(), out Wind[i, j]);
                    object obj2 = ds.Tables[0].Rows[5 + 6 * (i - 1)][j];
                    float.TryParse(obj2.ToString(), out GMax[i, j]);
                    object obj3 = ds.Tables[0].Rows[6 + 6 * (i - 1)][j];
                    float.TryParse(obj3.ToString(), out GMin[i, j]);
                    object obj4 = ds.Tables[0].Rows[7 + 6 * (i - 1)][j];
                    float.TryParse(obj4.ToString(), out TensionMax[i, j]);
                    object obj5 = ds.Tables[0].Rows[8 + 6 * (i - 1)][j];
                    float.TryParse(obj5.ToString(), out TensionMin[i, j]);
                }
            }
        }

        public override void CalculateLoadDistribute(out float[,] xx, out float[,] yy, out float[,] zz, string path)
        {
            int calNums = Template.WorkConditionCombos.Where(item => item.IsCalculate).ToList().Count;

            XX = new float[calNums, Template.Wires.Count];
            YY = new float[calNums, Template.Wires.Count];
            ZZ = new float[calNums, Template.Wires.Count];

            int i = 0, j = 1;

            ProcessString.Add(Template.Name + " " + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"));

            foreach (var wd in Template.WorkConditionCombos)
            {
                ProcessString.Add("");
                ProcessString.Add(wd.WorkComment);

                if (!wd.IsCalculate)
                    continue;

                j = 1;
                foreach (int wic in wd.WirdIndexCodes)
                {
                    LineParas = LineParasArr[j - 1];
                    formula.SetStrLineParas(LineParas);

                    int zhs = wic;

                    switch (wd.WorkConditionCode)
                    {
                        case "N1":
                        case "N2":
                        case "D1":
                        case "D2":
                            DistributeInWindAndLowTemperature(i, j);
                            break;
                        case "I1":
                        case "I2":
                        case "U1":
                        case "U2":
                        case "Y1":
                        case "Y2":
                        case "B1":
                        case "B2":
                            DistributeInIce(i, j);
                            break;
                        case "L1":
                        case "L2":
                        case "L1a":
                        case "L2a":
                        case "L1b":
                        case "L2b":
                            DistributeInLift(i, j);
                            break;
                        case "M1":
                        case "M2":
                            DistributeInAnchor(i, j);
                            break;
                        case "J1":
                        case "J2":
                            //DistributeInOPGWAnchor(i, j);
                            break;
                        case "MQ1":
                        case "MQ2":
                            //DistributeInHuache(i, j);
                            break;
                        case "G1":
                        case "G2":
                            //DistributeInTixian(i, j);
                            break;
                        default:
                            throw new Exception("直线塔工况代号超出范围" + "0 + 16" + "错误：1-217");
                            break;

                    }
                    j++;
                }
                i++;
            }

            xx = XX;
            yy = YY;
            zz = ZZ;

            using (FileStream fileStream = File.OpenWrite(path))
            {
                using (StreamWriter writer = new StreamWriter(fileStream))
                {
                    foreach (string s in ProcessString)
                    {
                        writer.WriteLine(s);
                    }
                    writer.Flush();
                    writer.Close();
                }
            }
        }

        /// <summary>
        /// 针对大风和低温的工况
        /// 工况代码"N1", "N2", "D1", "D2"
        /// '正常运行大风工况，1、2分表表示大小转角
        /// '根据大小转角情况，张力情况，垂荷情况，选用不同的数据进行计算
        /// '前后张力DD、DX、XD、XX、D0、0D等
        /// '前后垂荷 YY、YB、BB、BY、YT、TY等等
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        protected void DistributeInWindAndLowTemperature(int i, int j)
        {
            float x11, x12, x21, x22, x3;
            float z11, z12, z21, z22, z3;
            float e1, e2, e3, e4;
            float y1, y2, y3, y4;
            float[] c = new float[9];
            float mgxz = 0;

            WorkConditionCombo wd = Template.WorkConditionCombos[i];

            int zhs = wd.WirdIndexCodes[j - 1];

            if (Math.Abs(zhs) > 0)
            {
                x11 = WindDF(j, Abs(zhs));
                x12 = windXF(j, Abs(zhs));
                x21 = windDB(j, Abs(zhs));
                x22 = windXB(j, Abs(zhs));
                x3 = windTX(j, Abs(zhs));
                z11 = gmaxF(j, Abs(zhs));
                z12 = gminF(j, Abs(zhs));
                z21 = gmaxB(j, Abs(zhs));
                z22 = gminB(j, Abs(zhs));
                z3 = gTX(j, Abs(zhs));

                if (wd.WorkConditionCode == "N1" || wd.WorkConditionCode == "D1")
                {
                    e1 = x11;
                    e2 = x21;
                    e3 = maxangle;
                    e4 = 2f;
                }
                else
                {
                    e1 = x12;
                    e2 = x22;
                    e3 = minangle;
                    e4 = 1f;
                }

                if (wd.WorkConditionCode == "N1" || wd.WorkConditionCode == "D1")
                {
                    Vcb = Vcb_N;
                }
                else
                {
                    Vcb = Vcb_D;
                }


                if (angle == 45)
                {
                    y1 = Tension45D(j, e4);
                    y2 = Tension45X(j, e4);
                }
                else
                {
                    y1 = tensionD(j, Abs(zhs));
                    y2 = tensionX(j, Abs(zhs));
                }

                if (wd.TensionAngleCode == "DD")
                {
                    if (Paras.IsCornerTower || Paras.IsBranchTower)
                    {
                        y3 = y1;
                        y4 = y1;
                    }
                    else
                    {
                        y3 = y1;
                        y4 = mgdz;
                    }

                    switch (wd.VertialLoadCode)
                    {
                        case "YY":
                            c(1) = e1;
                            c(2) = y3;
                            c(3) = z11;
                            c(4) = e2;
                            c(5) = y4;
                            c(6) = z21;
                            c(7) = x3;
                            c(8) = z3;
                            break;

                        case "YB":
                            c(1) = e1;
                            c(2) = y3;
                            c(3) = z11;
                            c(4) = e2;
                            c(5) = y4;
                            c(6) = z22;
                            c(7) = x3;
                            c(8) = z3;
                            break;

                        case "0B":
                            c(1) = e1;
                            c(2) = y3;
                            c(3) = 0;
                            c(4) = e2;
                            c(5) = y4;
                            c(6) = z22;
                            c(7) = x3;
                            c(8) = z3;
                            break;

                        case "B0":
                            c(1) = e1;
                            c(2) = y3;
                            c(3) = z12;
                            c(4) = e2;
                            c(5) = y4;
                            c(6) = 0;
                            c(7) = x3;
                            c(8) = z3;
                            break;

                        case "Y0":
                            c(1) = e1;
                            c(2) = y3;
                            c(3) = z11;
                            c(4) = e2;
                            c(5) = y4;
                            c(6) = 0;
                            c(7) = x3;
                            c(8) = z3;
                            break;

                        case "BB":
                            c(1) = e1;
                            c(2) = y3;
                            c(3) = z12;
                            c(4) = e2;
                            c(5) = y4;
                            c(6) = z22;
                            c(7) = x3;
                            c(8) = z3;
                            break;

                        case "BY":
                            c(1) = e1;
                            c(2) = y3;
                            c(3) = z12;
                            c(4) = e2;
                            c(5) = y4;
                            c(6) = z21;
                            c(7) = x3;
                            c(8) = z3;
                            break;

                        case "0Y":
                            c(1) = e1;
                            c(2) = y3;
                            c(3) = 0;
                            c(4) = e2;
                            c(5) = y4;
                            c(6) = z21;
                            c(7) = x3;
                            c(8) = z3;
                            break;
                        //case "00":
                        //    c(1) = e1;
                        //    c(2) = y3;
                        //    c(3) = 0;
                        //    c(4) = e2;
                        //    c(5) = y4;
                        //    c(6) = 0;
                        //    c(7) = x3;
                        //    c(8) = z3;
                        //    break;
                        //2016.9.24 针对前后侧同时取某一侧的拔力新增,T表示某侧取相反侧上拔力计算

                        case "YT":
                            c(1) = e1;
                            c(2) = y3;
                            c(3) = z11;
                            c(4) = e2;
                            c(5) = y4;
                            c(6) = z12;
                            c(7) = x3;
                            c(8) = z3;
                            break;

                        case "0T":
                            c(1) = e1;
                            c(2) = y3;
                            c(3) = 0;
                            c(4) = e2;
                            c(5) = y4;
                            c(6) = z12;
                            c(7) = x3;
                            c(8) = z3;
                            break;

                        case "T0":
                            c(1) = e1;
                            c(2) = y3;
                            c(3) = z22;
                            c(4) = e2;
                            c(5) = y4;
                            c(6) = 0;
                            c(7) = x3;
                            c(8) = z3;
                            break;

                        case "TY":
                            c(1) = e1;
                            c(2) = y3;
                            c(3) = z22;
                            c(4) = e2;
                            c(5) = y4;
                            c(6) = z21;
                            c(7) = x3;
                            c(8) = z3;
                            break;

                        //20190113新版程序
                        default:
                            c(1) = e1;
                            c(2) = y3;
                            c(4) = e2;
                            c(5) = y4;
                            c(7) = x3;
                            c(8) = z3;
                            if (wd.VertialLoadCode.Substring(0, 1) == "0")
                                c(3) = 0;
                            else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                c(3) = z11;
                            else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                c(3) = z12;
                            else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                c(3) = z21;
                            else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                c(3) = z22;
                            else
                                throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-1");

                            if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                c(6) = 0;
                            else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                c(6) = z11;
                            else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                c(6) = z12;
                            else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                c(6) = z21;
                            else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                c(6) = z22;
                            else
                                throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-1");
                            break;
                    }


                }
                else if (wd.TensionAngleCode == "DX")
                {
                    if (zhs > 0)
                    {
                        if (Paras.IsCornerTower || Paras.IsBranchTower)
                        {
                            y3 = y1;
                            y4 = y2;
                        }
                        else
                        {
                            y3 = y1;
                            y4 = mgxz;
                        }
                    }
                    else
                    {
                        if (Paras.IsCornerTower || Paras.IsBranchTower)
                        {
                            y3 = y2;
                            y4 = y1;
                        }
                        else
                        {
                            y3 = y2;
                            y4 = mgxz;
                        }
                    }

                    switch (wd.VertialLoadCode)
                    {
                        case "YY":
                            c(1) = e1;
                            c(2) = y3;
                            c(3) = z11;
                            c(4) = e2;
                            c(5) = y4;
                            c(6) = z21;
                            c(7) = x3;
                            c(8) = z3;
                            break;
                        case "YB":
                            c(1) = e1;
                            c(2) = y3;
                            c(3) = z11;
                            c(4) = e2;
                            c(5) = y4;
                            c(6) = z22;
                            c(7) = x3;
                            c(8) = z3;
                            break;
                        case "0B":
                            c(1) = e1;
                            c(2) = y3;
                            c(3) = 0;
                            c(4) = e2;
                            c(5) = y4;
                            c(6) = z22;
                            c(7) = x3;
                            c(8) = z3;
                            break;
                        case "B0":
                            c(1) = e1;
                            c(2) = y3;
                            c(3) = z12;
                            c(4) = e2;
                            c(5) = y4;
                            c(6) = 0;
                            c(7) = x3;
                            c(8) = z3;
                            break;
                        case "Y0":
                            c(1) = e1;
                            c(2) = y3;
                            c(3) = z11;
                            c(4) = e2;
                            c(5) = y4;
                            c(6) = 0;
                            c(7) = x3;
                            c(8) = z3;
                            break;
                        case "BB":
                            c(1) = e1;
                            c(2) = y3;
                            c(3) = z12;
                            c(4) = e2;
                            c(5) = y4;
                            c(6) = z22;
                            c(7) = x3;
                            c(8) = z3;
                            break;
                        case "BY":
                            c(1) = e1;
                            c(2) = y3;
                            c(3) = z12;
                            c(4) = e2;
                            c(5) = y4;
                            c(6) = z21;
                            c(7) = x3;
                            c(8) = z3;
                            break;
                        case "0Y":
                            c(1) = e1;
                            c(2) = y3;
                            c(3) = 0;
                            c(4) = e2;
                            c(5) = y4;
                            c(6) = z21;
                            c(7) = x3;
                            c(8) = z3;
                            break;

                        //case "00":                            
                        //    c(1) = e1;
                        //    c(2) = y3;
                        //    c(3) = 0;
                        //    c(4) = e2;
                        //    c(5) = y4;
                        //    c(6) = 0;
                        //    c(7) = x3;
                        //    c(8) = z3;
                        //    break;
                        //2016.9.24 针对前后侧同时取某一侧的拔力新增,T表示某侧取相反侧上拔力计算
                        case "YT":
                            c(1) = e1;
                            c(2) = y3;
                            c(3) = z11;
                            c(4) = e2;
                            c(5) = y4;
                            c(6) = z12;
                            c(7) = x3;
                            c(8) = z3;
                            break;
                        case "TY":
                            c(1) = e1;
                            c(2) = y3;
                            c(3) = z22;
                            c(4) = e2;
                            c(5) = y4;
                            c(6) = z21;
                            c(7) = x3;
                            c(8) = z3;
                            break;
                        case "0T":
                            c(1) = e1;
                            c(2) = y3;
                            c(3) = 0;
                            c(4) = e2;
                            c(5) = y4;
                            c(6) = z12;
                            c(7) = x3;
                            c(8) = z3;
                            break;
                        case "T0":
                            c(1) = e1;
                            c(2) = y3;
                            c(3) = z22;
                            c(4) = e2;
                            c(5) = y4;
                            c(6) = 0;
                            c(7) = x3;
                            c(8) = z3;
                            break;
                        //20190113新版程序
                        default:
                            c(1) = e1;
                            c(2) = y3;
                            c(4) = e2;
                            c(5) = y4;
                            c(7) = x3;
                            c(8) = z3;
                            if (wd.VertialLoadCode.Substring(0, 1) == "0")
                                c(3) = 0;
                            else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                c(3) = z11;
                            else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                c(3) = z12;
                            else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                c(3) = z21;
                            else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                c(3) = z22;
                            else
                                throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-3");

                            if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                c(6) = 0;
                            else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                c(6) = z11;
                            else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                c(6) = z12;
                            else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                c(6) = z21;
                            else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                c(6) = z22;
                            else
                                throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-4");
                            break;
                    }

                }
                else if (wd.TensionAngleCode == "XD")
                {
                    if (zhs > 0)
                    {
                        if (Paras.IsCornerTower || Paras.IsBranchTower)
                        {
                            y3 = y2;
                            y4 = y1;
                        }
                        else
                        {
                            y3 = y2;
                            y4 = mgxz;
                        }
                    }
                    else
                    {
                        if (Paras.IsCornerTower || Paras.IsBranchTower)
                        {
                            y3 = y1;
                            y4 = y2;
                        }
                        else
                        {
                            y3 = y1;
                            y4 = mgxz;
                        }
                    }

                    switch (wd.VertialLoadCode)
                    {
                        case "YY":
                            c(1) = e1;
                            c(2) = y3;
                            c(3) = z11;
                            c(4) = e2;
                            c(5) = y4;
                            c(6) = z21;
                            c(7) = x3;
                            c(8) = z3;
                            break;
                        case "YB":
                            c(1) = e1;
                            c(2) = y3;
                            c(3) = z11;
                            c(4) = e2;
                            c(5) = y4;
                            c(6) = z22;
                            c(7) = x3;
                            c(8) = z3;
                            break;
                        case "0B":
                            c(1) = e1;
                            c(2) = y3;
                            c(3) = 0;
                            c(4) = e2;
                            c(5) = y4;
                            c(6) = z22;
                            c(7) = x3;
                            c(8) = z3;
                            break;
                        case "B0":
                            c(1) = e1;
                            c(2) = y3;
                            c(3) = z12;
                            c(4) = e2;
                            c(5) = y4;
                            c(6) = 0;
                            c(7) = x3;
                            c(8) = z3;
                            break;
                        case "Y0":
                            c(1) = e1;
                            c(2) = y3;
                            c(3) = z11;
                            c(4) = e2;
                            c(5) = y4;
                            c(6) = 0;
                            c(7) = x3;
                            c(8) = z3;
                            break;
                        case "BB":
                            c(1) = e1;
                            c(2) = y3;
                            c(3) = z12;
                            c(4) = e2;
                            c(5) = y4;
                            c(6) = z22;
                            c(7) = x3;
                            c(8) = z3;
                            break;
                        case "BY":
                            c(1) = e1;
                            c(2) = y3;
                            c(3) = z12;
                            c(4) = e2;
                            c(5) = y4;
                            c(6) = z21;
                            c(7) = x3;
                            c(8) = z3;
                            break;
                        case "0Y":
                            c(1) = e1;
                            c(2) = y3;
                            c(3) = 0;
                            c(4) = e2;
                            c(5) = y4;
                            c(6) = z21;
                            c(7) = x3;
                            c(8) = z3;
                            break;

                        //case "00":
                        //    c(1) = e1;
                        //    c(2) = y3;
                        //    c(3) = 0;
                        //    c(4) = e2;
                        //    c(5) = y4;
                        //    c(6) = 0;
                        //    c(7) = x3;
                        //    c(8) = z3;
                        //    break;
                        //2016.9.24 针对前后侧同时取某一侧的拔力新增,T表示某侧取相反侧上拔力计算
                        case "YT":
                            c(1) = e1;
                            c(2) = y3;
                            c(3) = z11;
                            c(4) = e2;
                            c(5) = y4;
                            c(6) = z12;
                            c(7) = x3;
                            c(8) = z3;
                            break;
                        case "TY":
                            c(1) = e1;
                            c(2) = y3;
                            c(3) = z22;
                            c(4) = e2;
                            c(5) = y4;
                            c(6) = z21;
                            c(7) = x3;
                            c(8) = z3;
                            break;
                        case "0T":
                            c(1) = e1;
                            c(2) = y3;
                            c(3) = 0;
                            c(4) = e2;
                            c(5) = y4;
                            c(6) = z12;
                            c(7) = x3;
                            c(8) = z3;
                            break;
                        case "T0":
                            c(1) = e1;
                            c(2) = y3;
                            c(3) = z22;
                            c(4) = e2;
                            c(5) = y4;
                            c(6) = 0;
                            c(7) = x3;
                            c(8) = z3;
                            break;
                        //20190113新版程序
                        default:
                            c(1) = e1;
                            c(2) = y3;
                            c(4) = e2;
                            c(5) = y4;
                            c(7) = x3;
                            c(8) = z3;
                            if (wd.VertialLoadCode.Substring(0, 1) == "0")
                                c(3) = 0;
                            else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                c(3) = z11;
                            else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                c(3) = z12;
                            else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                c(3) = z21;
                            else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                c(3) = z22;
                            else
                                throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-5");

                            if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                c(6) = 0;
                            else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                c(6) = z11;
                            else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                c(6) = z12;
                            else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                c(6) = z21;
                            else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                c(6) = z22;
                            else
                                throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-6");
                            break;
                    }

                }
                else if (wd.TensionAngleCode == "XX")
                {
                    if (Paras.IsCornerTower || Paras.IsBranchTower)
                    {
                        y3 = y2;
                        y4 = y2;
                    }
                    else
                    {
                        y3 = y2;
                        y4 = mgxz;
                    }


                    switch (wd.VertialLoadCode)
                    {
                        case "YY":
                            c(1) = e1;
                            c(2) = y3;
                            c(3) = z11;
                            c(4) = e2;
                            c(5) = y4;
                            c(6) = z21;
                            c(7) = x3;
                            c(8) = z3;
                            break;
                        case "YB":
                            c(1) = e1;
                            c(2) = y3;
                            c(3) = z11;
                            c(4) = e2;
                            c(5) = y4;
                            c(6) = z22;
                            c(7) = x3;
                            c(8) = z3;
                            break;
                        case "0B":
                            c(1) = e1;
                            c(2) = y3;
                            c(3) = 0;
                            c(4) = e2;
                            c(5) = y4;
                            c(6) = z22;
                            c(7) = x3;
                            c(8) = z3;
                            break;
                        case "B0":
                            c(1) = e1;
                            c(2) = y3;
                            c(3) = z12;
                            c(4) = e2;
                            c(5) = y4;
                            c(6) = 0;
                            c(7) = x3;
                            c(8) = z3;
                            break;
                        case "Y0":
                            c(1) = e1;
                            c(2) = y3;
                            c(3) = z11;
                            c(4) = e2;
                            c(5) = y4;
                            c(6) = 0;
                            c(7) = x3;
                            c(8) = z3;
                            break;
                        case "BB":
                            c(1) = e1;
                            c(2) = y3;
                            c(3) = z12;
                            c(4) = e2;
                            c(5) = y4;
                            c(6) = z22;
                            c(7) = x3;
                            c(8) = z3;
                            break;
                        case "BY":
                            c(1) = e1;
                            c(2) = y3;
                            c(3) = z12;
                            c(4) = e2;
                            c(5) = y4;
                            c(6) = z21;
                            c(7) = x3;
                            c(8) = z3;
                            break;
                        case "0Y":
                            c(1) = e1;
                            c(2) = y3;
                            c(3) = 0;
                            c(4) = e2;
                            c(5) = y4;
                            c(6) = z21;
                            c(7) = x3;
                            c(8) = z3;
                            break;

                        //case "00":
                        //    c(1) = e1;
                        //    c(2) = y3;
                        //    c(3) = 0;
                        //    c(4) = e2;
                        //    c(5) = y4;
                        //    c(6) = 0;
                        //    c(7) = x3;
                        //    c(8) = z3;
                        //    break;
                        //2016.9.24 针对前后侧同时取某一侧的拔力新增,T表示某侧取相反侧上拔力计算
                        case "YT":
                            c(1) = e1;
                            c(2) = y3;
                            c(3) = z11;
                            c(4) = e2;
                            c(5) = y4;
                            c(6) = z12;
                            c(7) = x3;
                            c(8) = z3;
                            break;

                        case "0T":
                            c(1) = e1;
                            c(2) = y3;
                            c(3) = 0;
                            c(4) = e2;
                            c(5) = y4;
                            c(6) = z12;
                            c(7) = x3;
                            c(8) = z3;
                            break;
                        case "T0":
                            c(1) = e1;
                            c(2) = y3;
                            c(3) = z22;
                            c(4) = e2;
                            c(5) = y4;
                            c(6) = 0;
                            c(7) = x3;
                            c(8) = z3;
                            break;
                        case "TY":
                            c(1) = e1;
                            c(2) = y3;
                            c(3) = z22;
                            c(4) = e2;
                            c(5) = y4;
                            c(6) = z21;
                            c(7) = x3;
                            c(8) = z3;
                            break;
                        //20190113新版程序
                        default:
                            c(1) = e1;
                            c(2) = y3;
                            c(4) = e2;
                            c(5) = y4;
                            c(7) = x3;
                            c(8) = z3;
                            if (wd.VertialLoadCode.Substring(0, 1) == "0")
                                c(3) = 0;
                            else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                c(3) = z11;
                            else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                c(3) = z12;
                            else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                c(3) = z21;
                            else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                c(3) = z22;
                            else
                                throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-7");

                            if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                c(6) = 0;
                            else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                c(6) = z11;
                            else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                c(6) = z12;
                            else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                c(6) = z21;
                            else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                c(6) = z22;
                            else
                                throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-8");
                            break;
                    }

                }
                else if (wd.TensionAngleCode == "D0")
                {
                    if (Paras.IsCornerTower || Paras.IsBranchTower)
                    {
                        y3 = y1;
                        y4 = 0;
                    }
                    else
                    {
                        y3 = y1;
                        y4 = 0;
                    }


                    switch (wd.VertialLoadCode)
                    {
                        case "B0":
                            c(1) = e1;
                            c(2) = y3;
                            c(3) = z12;
                            c(4) = 0;
                            c(5) = y4;
                            c(6) = 0;
                            c(7) = 0;
                            c(8) = 0;
                            break;
                        case "Y0":
                            c(1) = e1;
                            c(2) = y3;
                            c(3) = z11;
                            c(4) = 0;
                            c(5) = y4;
                            c(6) = 0;
                            c(7) = 0;
                            c(8) = 0;
                            break;
                        case "T0":
                            c(1) = e1;
                            c(2) = y3;
                            c(3) = z22;
                            c(4) = 0;
                            c(5) = y4;
                            c(6) = 0;
                            c(7) = 0;
                            c(8) = 0;
                            break;
                        //20190113新版程序
                        default:
                            c(1) = e1;
                            c(2) = y3;
                            c(4) = 0;
                            c(5) = 0;
                            c(7) = 0;
                            c(8) = 0;
                            if (wd.VertialLoadCode.Substring(0, 1) == "0")
                                c(3) = 0;
                            else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                c(3) = z11;
                            else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                c(3) = z12;
                            else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                c(3) = z21;
                            else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                c(3) = z22;
                            else
                                throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-9");

                            if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                c(6) = 0;
                            else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                c(6) = z11;
                            else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                c(6) = z12;
                            else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                c(6) = z21;
                            else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                c(6) = z22;
                            else
                                throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-10");
                            break;
                    }

                }
                else if (wd.TensionAngleCode == "X0")
                {
                    if (Paras.IsCornerTower || Paras.IsBranchTower)
                    {
                        y3 = y2;
                        y4 = 0;
                    }
                    else
                    {
                        y3 = y2;
                        y4 = 0;
                    }

                    switch (wd.VertialLoadCode)
                    {
                        case "B0":
                            c(1) = e1;
                            c(2) = y3;
                            c(3) = z12;
                            c(4) = 0;
                            c(5) = y4;
                            c(6) = 0;
                            c(7) = 0;
                            c(8) = 0;
                            break;
                        case "Y0":
                            c(1) = e1;
                            c(2) = y3;
                            c(3) = z11;
                            c(4) = 0;
                            c(5) = y4;
                            c(6) = 0;
                            c(7) = 0;
                            c(8) = 0;
                            break;
                        case "T0":
                            c(1) = e1;
                            c(2) = y3;
                            c(3) = z22;
                            c(4) = 0;
                            c(5) = y4;
                            c(6) = 0;
                            c(7) = 0;
                            c(8) = 0;
                            break;
                        //20190113新版程序
                        default:
                            c(1) = e1;
                            c(2) = y3;
                            c(4) = 0;
                            c(5) = 0;
                            c(7) = 0;
                            c(8) = 0;
                            if (wd.VertialLoadCode.Substring(0, 1) == "0")
                                c(3) = 0;
                            else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                c(3) = z11;
                            else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                c(3) = z12;
                            else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                c(3) = z21;
                            else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                c(3) = z22;
                            else
                                throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-11");

                            if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                c(6) = 0;
                            else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                c(6) = z11;
                            else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                c(6) = z12;
                            else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                c(6) = z21;
                            else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                c(6) = z22;
                            else
                                throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-12");
                            break;
                    }

                }
                else if (wd.TensionAngleCode == "0D")
                {
                    if (Paras.IsCornerTower || Paras.IsBranchTower)
                    {
                        y3 = 0;
                        y4 = y1;
                    }
                    else
                    {
                        y3 = 0;
                        y4 = mgdz;
                    }

                    switch (wd.VertialLoadCode)
                    {
                        case "0B":
                            c(1) = 0;
                            c(2) = y3;
                            c(3) = 0;
                            c(4) = e2;
                            c(5) = y4;
                            c(6) = z22;
                            c(7) = 0;
                            c(8) = 0;
                            break;
                        case "0Y":
                            c(1) = 0;
                            c(2) = y3;
                            c(3) = 0;
                            c(4) = e2;
                            c(5) = y4;
                            c(6) = z21;
                            c(7) = 0;
                            c(8) = 0;
                            break;
                        case "0T":
                            c(1) = 0;
                            c(2) = y3;
                            c(3) = 0;
                            c(4) = e2;
                            c(5) = y4;
                            c(6) = z12;
                            c(7) = 0;
                            c(8) = 0;
                            break;
                        //20190113新版程序
                        default:
                            c(1) = 0;
                            c(2) = 0;
                            c(4) = e2;
                            c(5) = y4;
                            c(7) = 0;
                            c(8) = 0;
                            if (wd.VertialLoadCode.Substring(0, 1) == "0")
                                c(3) = 0;
                            else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                c(3) = z11;
                            else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                c(3) = z12;
                            else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                c(3) = z21;
                            else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                c(3) = z22;
                            else
                                throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-13");

                            if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                c(6) = 0;
                            else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                c(6) = z11;
                            else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                c(6) = z12;
                            else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                c(6) = z21;
                            else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                c(6) = z22;
                            else
                                throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-14");
                            break;
                    }

                }
                else if (wd.TensionAngleCode == "0X")
                {
                    if (Paras.IsCornerTower || Paras.IsBranchTower)
                    {
                        y3 = 0;
                        y4 = y2;
                    }
                    else
                    {
                        y3 = 0;
                        y4 = mgxz;
                    }

                    switch (wd.VertialLoadCode)
                    {
                        case "0B":
                            c(1) = 0;
                            c(2) = y3;
                            c(3) = 0;
                            c(4) = e2;
                            c(5) = y4;
                            c(6) = z22;
                            c(7) = 0;
                            c(8) = 0;
                            break;
                        case "0Y":
                            c(1) = 0;
                            c(2) = y3;
                            c(3) = 0;
                            c(4) = e2;
                            c(5) = y4;
                            c(6) = z21;
                            c(7) = 0;
                            c(8) = 0;
                            break;
                        case "0T":
                            c(1) = 0;
                            c(2) = y3;
                            c(3) = 0;
                            c(4) = e2;
                            c(5) = y4;
                            c(6) = z12;
                            c(7) = 0;
                            c(8) = 0;
                            break;
                        //20190113新版程序
                        default:
                            c(1) = 0;
                            c(2) = 0;
                            c(4) = e2;
                            c(5) = y4;
                            c(7) = 0;
                            c(8) = 0;
                            if (wd.VertialLoadCode.Substring(0, 1) == "0")
                                c(3) = 0;
                            else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                c(3) = z11;
                            else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                c(3) = z12;
                            else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                c(3) = z21;
                            else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                c(3) = z22;
                            else
                                throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-11");

                            if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                c(6) = 0;
                            else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                c(6) = z11;
                            else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                c(6) = z12;
                            else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                c(6) = z21;
                            else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                c(6) = z22;
                            else
                                throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-12");
                            break;
                    }

                }

                if (Paras.IsBranchTower)
                {
                    //前侧转角  20161124分支塔添加
                    e3 = qzjiao;
                }

                XLF(i, j) = formula.ZNX(angle, e3, c(1), c(2), Vcb, out string strXLF) * BL3;
                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fx= " + BL3 + " x " + strXLF);
                YLF(i, j) = formula.ZNY(angle, e3, c(1), c(2), Vcb, out string strYLF);
                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fy= " + BL1 + " x " + strYLF);
                ZLF(i, j) = formula.ZNZ(c(3), Vcb, out string strZLF);
                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fz= " + BL1 + " x " + strZLF);

                if (Paras.IsBranchTower)
                {
                    //前侧转角  20161124分支塔添加
                    e3 = qzjiao;
                }

                XLB(i, j) = formula.ZNX(angle, e3, c(4), c(5), Vcb, out string strXLB) * BL3;
                ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fx= " + BL3 + " x " + strXLF);
                YLB(i, j) = formula.ZNY(angle, e3, c(4), -c(5), Vcb, out string strYLB);
                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fy= " + BL1 + " x " + strYLB);
                ZLB(i, j) = formula.ZNZ(c(6), Vcb, out string strZLB);
                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fz= " + BL1 + " x " + strZLB);

                if (nt % 3 != 0 || nt == 0)
                {
                    '0,1,2个跳线的时候 前后
                                        '跳线串始终为一种计算方法
                                        XTF(i, j) = ZNX(angle, 0, c(7), 0, Vcb) * BL3 * nt * BLTQ;
                    YTF(i, j) = ZNY(angle, 0, c(7), 0, Vcb) * nt * BLTQ;
                    ZTF(i, j) = ZNZ(c(8), Vcb) * nt * BLTQ;

                    XTB(i, j) = ZNX(angle, 0, c(7), 0, Vcb) * BL3 * nt * BLTH;
                    YTB(i, j) = ZNY(angle, 0, c(7), 0, Vcb) * nt * BLTH;
                    ZTB(i, j) = ZNZ(c(8), Vcb) * nt * BLTH;
                }
                else if (nt % 3 == 0 && nt > 0)
                {
                    XTF(i, j) = ZNX(angle, 0, c(7), 0, Vcb) * BL3 * nt * BLTQ;
                    YTF(i, j) = ZNY(angle, 0, c(7), 0, Vcb) * nt * BLTQ;
                    ZTF(i, j) = ZNZ(c(8), Vcb) * nt * BLTQ;

                    XTC(i, j) = ZNX(angle, 0, c(7), 0, Vcb) * BL3 * nt * BLTZ;
                    YTC(i, j) = ZNY(angle, 0, c(7), 0, Vcb) * nt * BLTZ;
                    ZTC(i, j) = ZNZ(c(8), Vcb) * nt * BLTZ;

                    XTB(i, j) = ZNX(angle, 0, c(7), 0, Vcb) * BL3 * nt * BLTH;
                    YTB(i, j) = ZNY(angle, 0, c(7), 0, Vcb) * nt * BLTH;
                    ZTB(i, j) = ZNZ(c(8), Vcb) * nt * BLTH;
                }
            }
            else
            {
                y1 = 0;
                y2 = 0;
                e1 = 0;
                x12 = 0;
                e2 = 0;
                x22 = 0;
                x3 = 0;
                z11 = 0;
                z12 = 0;
                z21 = 0;
                z22 = 0;
                z3 = 0;
                c(1) = e1;
                c(2) = y1;
                c(3) = z11;
                c(4) = e2;
                c(5) = y2;
                c(6) = z21;
                c(7) = x3;
                c(8) = z3;

                XLF(i, j) = ZNX(angle, e3, c(1), c(2), Vcb) * BL3;
                YLF(i, j) = ZNY(angle, e3, c(1), c(2), Vcb);
                ZLF(i, j) = ZNZ(c(3), Vcb);

                XLB(i, j) = ZNX(angle, e3, c(4), c(5), Vcb) * BL3;
                YLB(i, j) = ZNY(angle, e3, c(4), -c(5), Vcb);
                ZLB(i, j) = ZNZ(c(6), Vcb);
            }


        }

        /// <summary>
        /// 针对覆冰-最低温、不均匀冰、断线、验算冰工况
        /// 工况代码"I1", "I2", "U1", "U2", "B1", "B2", "Y1", "Y2"
        /// 1为最大垂荷，2为最小垂荷
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        protected void DistributeInIce(int i, int j)
        {
            float x11, x12, x21, x22, x3;
            float z11, z12, z21, z22, z3;
            float z011, z012, z021, z022, z03;
            float e1, e2, e3, e4, e5;
            float y1, y2, y3, y4;
            float[] c = new float[11];
            float mgxz = 0;
            int BL2;

            WorkConditionCombo wd = Template.WorkConditionCombos[i];
            int zhs = wd.WirdIndexCodes[j - 1], zhsAM;

            zhsAM = Math.Abs(zhs) % 100;

            if (Math.Abs(zhs) > 0)
            {
                BL2 = 1;
                y1 = tensionD(j, zhsx) * BL2;
                y2 = tensionX(j, zhsx) * BL2;
                x11 = windDF(j, zhsx) * BL2;
                x12 = windXF(j, zhsx) * BL2;
                x21 = windDB(j, zhsx) * BL2;
                x22 = windXB(j, zhsx) * BL2;
                x3 = windTX(j, zhsx) * BL2;
                z11 = gmaxF(j, zhsx) * BL2;
                z12 = gminF(j, zhsx) * BL2;
                z21 = gmaxB(j, zhsx) * BL2;
                z22 = gminB(j, zhsx) * BL2;
                z3 = gTX(j, zhsx) * BL2;
                z011 = gmaxF(j, 1) * BL2;
                z012 = gminF(j, 1) * BL2;
                z021 = gmaxB(j, 1) * BL2;
                z022 = gminB(j, 1) * BL2;
                z03 = gTX(j, 1) * BL2;

                if (wd.WorkConditionCode == "I1")
                {
                    e1 = x11; e2 = x21; e3 = maxangle; e4 = 2; e5 = Vcb_N; Vloadx = Vload; rg = Dload_b;
                }
                else if (wd.WorkConditionCode == "I2")
                {
                    e1 = x12; e2 = x22; e3 = minangle; e4 = 1; e5 = Vcb_N; Vloadx = Vload; rg = Dload_b;
                }
                else if (wd.WorkConditionCode == "U1")
                {
                    e1 = x11; e2 = x21; e3 = maxangle; e4 = 2; e5 = Vcb_U; Vloadx = Vload; rg = Dload_b;
                }
                else if (wd.WorkConditionCode == "U2")
                {
                    e1 = x12; e2 = x22; e3 = minangle; e4 = 1; e5 = Vcb_U; Vloadx = Vload; rg = Dload_b;
                }
                else if (wd.WorkConditionCode == "B1")
                {
                    e1 = x11; e2 = x21; e3 = maxangle; e4 = 2; e5 = Vcb_B; Vloadx = Vload; rg = Dload_b;
                }
                else if (wd.WorkConditionCode == "B2")
                {
                    e1 = x12; e2 = x22; e3 = minangle; e4 = 1; e5 = Vcb_B; Vloadx = Vload; rg = Dload_b;
                }
                else if (wd.WorkConditionCode == "Y1")
                {
                    e1 = x11; e2 = x21; e3 = maxangle; e4 = 2; e5 = Vcb_Y; Vloadx = Vload_A; rg = Dload_Y1;
                }
                else
                {
                    e1 = x12; e2 = x22; e3 = minangle; e4 = 1; e5 = Vcb_Y; Vloadx = Vload_A; rg = Dload_Y1;
                }

                if (wd.TensionAngleCode == "DD")
                {
                    if (Paras.IsCornerTower || Paras.IsBranchTower)
                    {
                        y3 = y1;
                        y4 = y1;
                    }
                    else
                    {
                        y3 = y1;
                        y4 = mgdz;
                    }

                    if (Paras.baseParaFormRadioButton1 && zhs < 0)
                    {
                        //按最严重情况考虑扭转 20170620新增  扭转相垂荷按最严重情况考虑
                        switch (wd.VertialLoadCode)
                        {
                            case "YY":
                                c(1) = e1; c(2) = y3; c(3) = z21; c(4) = e2; c(5) = y4; c(6) = z11; c(7) = x3; c(8) = z3; c(9) = z021; c(10) = z011; c(11) = z03;
                                break;
                            case "YB":
                                c(1) = e1; c(2) = y3; c(3) = z21; c(4) = e2; c(5) = y4; c(6) = z12; c(7) = x3; c(8) = z3; c(9) = z021; c(10) = z012; c(11) = z03;//根据酒湖线修改
                                break;
                            case "BB":
                                c(1) = e1; c(2) = y3; c(3) = z22; c(4) = e2; c(5) = y4; c(6) = z12; c(7) = x3; c(8) = z3; c(9) = z022; c(10) = z012; c(11) = z03;
                                break;
                            case "BY":
                                c(1) = e1; c(2) = y3; c(3) = z22; c(4) = e2; c(5) = y4; c(6) = z12; c(7) = x3; c(8) = z3; c(9) = z022; c(10) = z012; c(11) = z03;//根据酒湖线修改
                                break;
                            //2017.11.1 根据乌东德补充，添加B0等后续组合情况
                            case "0B":
                                c(1) = e1; c(2) = y3; c(3) = z22; c(4) = e2; c(5) = y4; c(6) = 0; c(7) = x3; c(8) = z3; c(9) = z022; c(10) = 0; c(11) = z03;
                                break;
                            case "B0":
                                c(1) = e1; c(2) = y3; c(3) = 0; c(4) = e2; c(5) = y4; c(6) = z12; c(7) = x3; c(8) = z3; c(9) = 0; c(10) = z012; c(11) = z03;
                                break;
                            case "Y0":
                                c(1) = e1; c(2) = y3; c(3) = 0; c(4) = e2; c(5) = y4; c(6) = z11; c(7) = x3; c(8) = z3; c(9) = 0; c(10) = z011; c(11) = z03;
                                break;
                            case "0Y":
                                c(1) = e1; c(2) = y3; c(3) = z21; c(4) = e2; c(5) = y4; c(6) = 0; c(7) = x3; c(8) = z3; c(9) = z021; c(10) = 0; c(11) = z03;
                                break;
                            //case "00";
                            //c(1) = e1; c(2) = y3; c(3) = 0; c(4) = e2; c(5) = y4; c(6) = 0; c(7) = x3; c(8) = z3; c(9) = 0; c(10) = 0; c(11) = z03;
                            //break;
                            case "YT":
                                c(1) = e1; c(2) = y3; c(3) = z12; c(4) = e2; c(5) = y4; c(6) = z11; c(7) = x3; c(8) = z3; c(9) = z012; c(10) = z011; c(11) = z03;
                                break;
                            case "0T":
                                c(1) = e1; c(2) = y3; c(3) = z12; c(4) = e2; c(5) = y4; c(6) = 0; c(7) = x3; c(8) = z3; c(9) = z012; c(10) = 0; c(11) = z03;
                                break;
                            case "T0":
                                c(1) =: e1; c(2) = y3; c(3) = 0; c(4) = e2; c(5) = y4; c(6) = z22; c(7) = x3; c(8) = z3; c(9) = 0; c(10) = z022; c(11) = z03;
                                break;
                            case "TY":
                                c(1) = e1; c(2) = y3; c(3) = z21; c(4) = e2; c(5) = y4; c(6) = z22; c(7) = x3; c(8) = z3; c(9) = z021; c(10) = z022; c(11) = z03;
                                break;
                            //20190113新版程序
                            default:
                                c(1) = e1; c(2) = y3; c(4) = e2; c(5) = y4; c(7) = x3; c(8) = z3; c(11) = z03;
                                if (Math.Abs(zhs) > 10000)
                                {
                                    //垂荷列数互换法则补充
                                    //'特意针对垂荷组合为12情况
                                    //仅适用于不均匀冰和验算不均匀冰工况，其余工况不适用。在检查模式中判断报错

                                    int zhs1 = Convert.ToInt16(Math.Abs(zhs).ToString().Substring(1, 2));
                                    float zzz11 = gmaxF(j, zhs1) * BL2;
                                    float zzz12 = gminF(j, zhs1) * BL2;
                                    float zzz21 = gmaxB(j, zhs1) * BL2;
                                    float zzz22 = gminB(j, zhs1) * BL2;

                                    if (Math.Abs(zhs).ToString().Substring(0, 1) == "1")
                                    {
                                        //前侧垂荷需要替换 万位数为1
                                        if (wd.VertialLoadCode.Substring(0, 1) == "0")
                                        {
                                            c(3) = 0; c(9) = 0;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                        {
                                            c(3) = zzz11; c(9) = z011;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                        {
                                            c(3) = zzz12; c(9) = z012;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                        {
                                            c(3) = zzz21; c(9) = z021;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                        {
                                            c(3) = zzz22; c(9) = z022;
                                        }
                                        else
                                        {
                                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                        }

                                        if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                        {
                                            c(6) = 0; c(10) = 0;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                        {
                                            c(6) = z11; c(10) = z011;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                        {
                                            c(6) = z12; c(10) = z012;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                        {
                                            c(6) = z21; c(10) = z021;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                        {
                                            c(6) = z22; c(10) = z022;
                                        }
                                        else
                                        {
                                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-16");
                                        }

                                    }
                                    else if (Math.Abs(zhs).ToString().Substring(0, 1) == "2")
                                    {
                                        //后侧垂荷需要替换  万位数为2
                                        if (wd.VertialLoadCode.Substring(0, 1) == "0")
                                        {
                                            c(3) = 0; c(9) = 0;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                        {
                                            c(3) = z11; c(9) = z011;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                        {
                                            c(3) = z12; c(9) = z012;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                        {
                                            c(3) = z21; c(9) = z021;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                        {
                                            c(3) = z22; c(9) = z022;
                                        }
                                        else
                                        {
                                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                        }

                                        if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                        {
                                            c(6) = 0; c(10) = 0;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                        {
                                            c(6) = zzz11; c(10) = z011;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                        {
                                            c(6) = zzz12; c(10) = z012;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                        {
                                            c(6) = zzz21; c(10) = z021;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                        {
                                            c(6) = zzz22; c(10) = z022;
                                        }
                                        else
                                        {
                                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-16");
                                        }

                                    }
                                    else
                                    {
                                        throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190115-01");
                                    }

                                }
                                else
                                {
                                    //前侧垂荷需要替换 万位数为1
                                    if (wd.VertialLoadCode.Substring(0, 1) == "0")
                                    {
                                        c(3) = 0; c(9) = 0;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                    {
                                        c(3) = z11; c(9) = z011;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                    {
                                        c(3) = z12; c(9) = z012;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                    {
                                        c(3) = z21; c(9) = z021;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                    {
                                        c(3) = z22; c(9) = z022;
                                    }
                                    else
                                    {
                                        throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                    }

                                    if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                    {
                                        c(6) = 0; c(10) = 0;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                    {
                                        c(6) = z11; c(10) = z011;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                    {
                                        c(6) = z12; c(10) = z012;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                    {
                                        c(6) = z21; c(10) = z021;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                    {
                                        c(6) = z22; c(10) = z022;
                                    }
                                    else
                                    {
                                        throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-16");
                                    }
                                }
                                break;
                        }
                    }
                    else
                    {
                        switch (wd.VertialLoadCode)
                        {
                            case "YY":
                                c(1) = e1 : c(2) = y3 : c(3) = z11 : c(4) = e2 : c(5) = y4 : c(6) = z21 : c(7) = x3 : c(8) = z3 : c(9) = z011 : c(10) = z021 : c(11) = z03
                            case "YB"v
                                c(1) = e1 : c(2) = y3 : c(3) = z11 : c(4) = e2 : c(5) = y4 : c(6) = z22 : c(7) = x3 : c(8) = z3 : c(9) = z011 : c(10) = z022 : c(11) = z03
                            case "0B":
                                c(1) = e1 : c(2) = y3 : c(3) = 0 : c(4) = e2 : c(5) = y4 : c(6) = z22 : c(7) = x3 : c(8) = z3 : c(9) = 0 : c(10) = z022 : c(11) = z03
                            case "B0":
                                c(1) = e1 : c(2) = y3 : c(3) = z12 : c(4) = e2 : c(5) = y4 : c(6) = 0 : c(7) = x3 : c(8) = z3 : c(9) = z012 : c(10) = 0 : c(11) = z03
                            case "Y0:
                                c(1) = e1 : c(2) = y3 : c(3) = z11 : c(4) = e2 : c(5) = y4 : c(6) = 0 : c(7) = x3 : c(8) = z3 : c(9) = z011 : c(10) = 0 : c(11) = z03

                            case "BB":
                                c(1) = e1 : c(2) = y3 : c(3) = z12 : c(4) = e2 : c(5) = y4 : c(6) = z22 : c(7) = x3 : c(8) = z3 : c(9) = z012 : c(10) = z022 : c(11) = z03
                            case "BY":
                                c(1) = e1 : c(2) = y3 : c(3) = z12 : c(4) = e2 : c(5) = y4 : c(6) = z21 : c(7) = x3 : c(8) = z3 : c(9) = z012 : c(10) = z021 : c(11) = z03
                            case "0Y":
                                c(1) = e1 : c(2) = y3 : c(3) = 0 : c(4) = e2 : c(5) = y4 : c(6) = z21 : c(7) = x3 : c(8) = z3 : c(9) = 0 : c(10) = z021 : c(11) = z03
                            //case "00":
                            //c(1) = e1 : c(2) = y3 : c(3) = 0 : c(4) = e2 : c(5) = y4 : c(6) = 0 : c(7) = x3 : c(8) = z3 : c(9) = 0 : c(10) = 0 : c(11) = z03
                            //2016.9.24 针对前后侧同时取某一侧的拔力新增,T表示某侧取相反侧上拔力计算
                            case "YT":
                                c(1) = e1 : c(2) = y3 : c(3) = z11 : c(4) = e2 : c(5) = y4 : c(6) = z12 : c(7) = x3 : c(8) = z3 : c(9) = z011 : c(10) = z012 : c(11) = z03
                            case "0T":
                                c(1) = e1 : c(2) = y3 : c(3) = 0 : c(4) = e2 : c(5) = y4 : c(6) = z12 : c(7) = x3 : c(8) = z3 : c(9) = 0 : c(10) = z012 : c(11) = z03
                            case "T0":
                                c(1) = e1 : c(2) = y3 : c(3) = z22 : c(4) = e2 : c(5) = y4 : c(6) = 0 : c(7) = x3 : c(8) = z3 : c(9) = z022 : c(10) = 0 : c(11) = z03
                            case "TY":
                                c(1) = e1 : c(2) = y3 : c(3) = z22 : c(4) = e2 : c(5) = y4 : c(6) = z21 : c(7) = x3 : c(8) = z3 : c(9) = z022 : c(10) = z021 : c(11) = z03
                            //20190113新版程序
                            default;
                                c(1) = e1 : c(2) = y3 : c(4) = e2 : c(5) = y4 : c(7) = x3 : c(8) = z3 : c(11) = z03;
                                if (Math.Abs(zhs) > 10000)
                                {
                                    //垂荷列数互换法则补充
                                    //'特意针对垂荷组合为12情况
                                    //仅适用于不均匀冰和验算不均匀冰工况，其余工况不适用。在检查模式中判断报错

                                    int zhs1 = Convert.ToInt16(Math.Abs(zhs).ToString().Substring(1, 2));
                                    float zzz11 = gmaxF(j, zhs1) * BL2;
                                    float zzz12 = gminF(j, zhs1) * BL2;
                                    float zzz21 = gmaxB(j, zhs1) * BL2;
                                    float zzz22 = gminB(j, zhs1) * BL2;

                                    if (Math.Abs(zhs).ToString().Substring(0, 1) == "1")
                                    {
                                        //前侧垂荷需要替换 万位数为1
                                        if (wd.VertialLoadCode.Substring(0, 1) == "0")
                                        {
                                            c(3) = 0; c(9) = 0;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                        {
                                            c(3) = zzz11; c(9) = z011;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                        {
                                            c(3) = zzz12; c(9) = z012;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                        {
                                            c(3) = zzz21; c(9) = z021;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                        {
                                            c(3) = zzz22; c(9) = z022;
                                        }
                                        else
                                        {
                                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                        }

                                        if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                        {
                                            c(6) = 0; c(10) = 0;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                        {
                                            c(6) = z11; c(10) = z011;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                        {
                                            c(6) = z12; c(10) = z012;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                        {
                                            c(6) = z21; c(10) = z021;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                        {
                                            c(6) = z22; c(10) = z022;
                                        }
                                        else
                                        {
                                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-16");
                                        }
                                    }
                                    else if (Math.Abs(zhs).ToString().Substring(0, 1) == "2")
                                    {
                                        //后侧垂荷需要替换  万位数为2
                                        if (wd.VertialLoadCode.Substring(0, 1) == "0")
                                        {
                                            c(3) = 0; c(9) = 0;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                        {
                                            c(3) = z11; c(9) = z011;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                        {
                                            c(3) = z12; c(9) = z012;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                        {
                                            c(3) = z21; c(9) = z021;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                        {
                                            c(3) = z22; c(9) = z022;
                                        }
                                        else
                                        {
                                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                        }

                                        if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                        {
                                            c(6) = 0; c(10) = 0;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                        {
                                            c(6) = zzz11; c(10) = z011;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                        {
                                            c(6) = zzz12; c(10) = z012;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                        {
                                            c(6) = zzz21; c(10) = z021;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                        {
                                            c(6) = zzz22; c(10) = z022;
                                        }
                                        else
                                        {
                                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-16");
                                        }
                                    }
                                    else
                                    {
                                        throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190115-01");
                                    }

                                }
                                else
                                {
                                    if (wd.VertialLoadCode.Substring(0, 1) == "0")
                                    {
                                        c(3) = 0 : c(9) = 0
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                    {
                                        c(3) = z11 : c(9) = z011
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                    {
                                        c(3) = z12 : c(9) = z012
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                    {
                                        c(3) = z21 : c(9) = z021
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                    {
                                        c(3) = z22 : c(9) = z022
                                    }
                                    else
                                    {
                                        throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                    }

                                    if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                    {
                                        c(6) = 0 : c(10) = 0
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                    {
                                        c(6) = z11; c(10) = z011;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                    {
                                        c(6) = z12; c(10) = z012;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                    {
                                        c(6) = z21; c(10) = z021;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                    {
                                        c(6) = z22; c(10) = z022;
                                    }
                                    else
                                    {
                                        throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-16");
                                    }
                                }
                                break;
                        }
                    }
                }
                else if (wd.TensionAngleCode == "DX")
                {
                    if (zhs > 0)
                    {
                        if (Paras.IsCornerTower || Paras.IsBranchTower)
                        {
                            y3 = y1
                            y4 = y2
                        }
                        else
                        {
                            y3 = y1
                            y4 = mgxz
                        }
                    }
                    else
                    {
                        if (Paras.IsCornerTower || Paras.IsBranchTower)
                        {
                            y3 = y1
                            y4 = y2
                        }
                        else
                        {
                            y3 = y2
                            y4 = mgdz
                        }
                    }

                    if (Paras.baseParaFormRadioButton1 && zhs < 0)
                    {
                        //按最严重情况考虑扭转 20170620新增  扭转相垂荷按最严重情况考虑
                        switch (wd.VertialLoadCode)
                        {
                            case "YY":
                                c(1) = e1 : c(2) = y3 : c(3) = z21 : c(4) = e2 : c(5) = y4 : c(6) = z11 : c(7) = x3 : c(8) = z3 : c(9) = z021 : c(10) = z011 : c(11) = z03
                                break;
                            case "YB":
                                c(1) = e1 : c(2) = y3 : c(3) = z21 : c(4) = e2 : c(5) = y4 : c(6) = z12 : c(7) = x3 : c(8) = z3 : c(9) = z021 : c(10) = z012 : c(11) = z03 //根据酒湖线修改
                                break;
                            case "BB":
                                c(1) = e1 : c(2) = y3 : c(3) = z22 : c(4) = e2 : c(5) = y4 : c(6) = z12 : c(7) = x3 : c(8) = z3 : c(9) = z022 : c(10) = z012 : c(11) = z03
                                break;
                            case "BY":
                                c(1) = e1 : c(2) = y3 : c(3) = z22 : c(4) = e2 : c(5) = y4 : c(6) = z12 : c(7) = x3 : c(8) = z3 : c(9) = z022 : c(10) = z012 : c(11) = z03   //根据酒湖线修改
                                break;
                            //2017.11.1 根据乌东德补充，添加B0等后续组合情况
                            case "0B":
                                c(1) = e1 : c(2) = y3 : c(3) = z22 : c(4) = e2 : c(5) = y4 : c(6) = 0 : c(7) = x3 : c(8) = z3 : c(9) = z022 : c(10) = 0 : c(11) = z03
                                break;
                            case "B0":
                                c(1) = e1 : c(2) = y3 : c(3) = 0 : c(4) = e2 : c(5) = y4 : c(6) = z12 : c(7) = x3 : c(8) = z3 : c(9) = 0 : c(10) = z012 : c(11) = z03
                                break;
                            case "Y0":
                                c(1) = e1 : c(2) = y3 : c(3) = 0 : c(4) = e2 : c(5) = y4 : c(6) = z11 : c(7) = x3 : c(8) = z3 : c(9) = 0 : c(10) = z011 : c(11) = z03
                                break;
                            case "0Y":
                                c(1) = e1 : c(2) = y3 : c(3) = z21 : c(4) = e2 : c(5) = y4 : c(6) = 0 : c(7) = x3 : c(8) = z3 : c(9) = z021 : c(10) = 0 : c(11) = z03
                                break;
                            //case "00";
                            //c(1) = e1 : c(2) = y3 : c(3) = 0 : c(4) = e2 : c(5) = y4 : c(6) = 0 : c(7) = x3 : c(8) = z3 : c(9) = 0 : c(10) = 0 : c(11) = z03
                            //break;
                            case "YT":
                                c(1) = e1 : c(2) = y3 : c(3) = z12 : c(4) = e2 : c(5) = y4 : c(6) = z11 : c(7) = x3 : c(8) = z3 : c(9) = z012 : c(10) = z011 : c(11) = z03
                                break;
                            case "0T":
                                c(1) = e1 : c(2) = y3 : c(3) = z12 : c(4) = e2 : c(5) = y4 : c(6) = 0 : c(7) = x3 : c(8) = z3 : c(9) = z012 : c(10) = 0 : c(11) = z03
                                break;
                            case "T0":
                                c(1) = e1 : c(2) = y3 : c(3) = 0 : c(4) = e2 : c(5) = y4 : c(6) = z22 : c(7) = x3 : c(8) = z3 : c(9) = 0 : c(10) = z022 : c(11) = z03
                                break;
                            case "TY":
                                c(1) = e1 : c(2) = y3 : c(3) = z21 : c(4) = e2 : c(5) = y4 : c(6) = z22 : c(7) = x3 : c(8) = z3 : c(9) = z021 : c(10) = z022 : c(11) = z03
                                break;
                            //20190113新版程序
                            default:
                                c(1) = e1 : c(2) = y3 : c(4) = e2 : c(5) = y4 : c(7) = x3 : c(8) = z3 : c(11) = z03
                                if (Math.Abs(zhs) > 10000)
                                {
                                    //垂荷列数互换法则补充
                                    //'特意针对垂荷组合为12情况
                                    //仅适用于不均匀冰和验算不均匀冰工况，其余工况不适用。在检查模式中判断报错

                                    int zhs1 = Convert.ToInt16(Math.Abs(zhs).ToString().Substring(1, 2));
                                    float zzz11 = gmaxF(j, zhs1) * BL2;
                                    float zzz12 = gminF(j, zhs1) * BL2;
                                    float zzz21 = gmaxB(j, zhs1) * BL2;
                                    float zzz22 = gminB(j, zhs1) * BL2;

                                    if (Math.Abs(zhs).ToString().Substring(0, 1) == "1")
                                    {
                                        //前侧垂荷需要替换 万位数为1
                                        if (wd.VertialLoadCode.Substring(0, 1) == "0")
                                        {
                                            c(3) = 0; c(9) = 0;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                        {
                                            c(3) = zzz11; c(9) = z011;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                        {
                                            c(3) = zzz12; c(9) = z012;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                        {
                                            c(3) = zzz21; c(9) = z021;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                        {
                                            c(3) = zzz22; c(9) = z022;
                                        }
                                        else
                                        {
                                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                        }

                                        if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                        {
                                            c(6) = 0; c(10) = 0;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                        {
                                            c(6) = z11; c(10) = z011;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                        {
                                            c(6) = z12; c(10) = z012;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                        {
                                            c(6) = z21; c(10) = z021;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                        {
                                            c(6) = z22; c(10) = z022;
                                        }
                                        else
                                        {
                                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-16");
                                        }

                                    }
                                    else if (Math.Abs(zhs).ToString().Substring(0, 1) == "2")
                                    {
                                        //后侧垂荷需要替换  万位数为2
                                        if (wd.VertialLoadCode.Substring(0, 1) == "0")
                                        {
                                            c(3) = 0; c(9) = 0;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                        {
                                            c(3) = z11; c(9) = z011;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                        {
                                            c(3) = z12; c(9) = z012;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                        {
                                            c(3) = z21; c(9) = z021;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                        {
                                            c(3) = z22; c(9) = z022;
                                        }
                                        else
                                        {
                                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                        }

                                        if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                        {
                                            c(6) = 0; c(10) = 0;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                        {
                                            c(6) = zzz11; c(10) = z011;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                        {
                                            c(6) = zzz12; c(10) = z012;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                        {
                                            c(6) = zzz21; c(10) = z021;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                        {
                                            c(6) = zzz22; c(10) = z022;
                                        }
                                        else
                                        {
                                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-16");
                                        }

                                    }
                                    else
                                    {
                                        throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190115-01");
                                    }

                                }
                                else
                                {
                                    if (wd.VertialLoadCode.Substring(0, 1) == "0")
                                    {
                                        c(3) = 0; c(9) = 0;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                    {
                                        c(3) = z11; c(9) = z011;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                    {
                                        c(3) = z12; c(9) = z012;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                    {
                                        c(3) = z21; c(9) = z021;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                    {
                                        c(3) = z22; c(9) = z022;
                                    }
                                    else
                                    {
                                        throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                    }

                                    if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                    {
                                        c(6) = 0; c(10) = 0;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                    {
                                        c(6) = z11; c(10) = z011;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                    {
                                        c(6) = z12; c(10) = z012;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                    {
                                        c(6) = z21; c(10) = z021;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                    {
                                        c(6) = z22; c(10) = z022;
                                    }
                                    else
                                    {
                                        throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-16");
                                    }
                                }
                                break;
                        }
                    }
                    else
                    {
                        switch (wd.VertialLoadCode)
                        {
                            case "YY":
                                c(1) = e1 : c(2) = y3 : c(3) = z11 : c(4) = e2 : c(5) = y4 : c(6) = z21 : c(7) = x3 : c(8) = z3 : c(9) = z011 : c(10) = z021 : c(11) = z03
                            case "YB"v
                                c(1) = e1 : c(2) = y3 : c(3) = z11 : c(4) = e2 : c(5) = y4 : c(6) = z22 : c(7) = x3 : c(8) = z3 : c(9) = z011 : c(10) = z022 : c(11) = z03
                            case "0B":
                                c(1) = e1 : c(2) = y3 : c(3) = 0 : c(4) = e2 : c(5) = y4 : c(6) = z22 : c(7) = x3 : c(8) = z3 : c(9) = 0 : c(10) = z022 : c(11) = z03
                            case "B0":
                                c(1) = e1 : c(2) = y3 : c(3) = z12 : c(4) = e2 : c(5) = y4 : c(6) = 0 : c(7) = x3 : c(8) = z3 : c(9) = z012 : c(10) = 0 : c(11) = z03
                            case "Y0:
                                c(1) = e1 : c(2) = y3 : c(3) = z11 : c(4) = e2 : c(5) = y4 : c(6) = 0 : c(7) = x3 : c(8) = z3 : c(9) = z011 : c(10) = 0 : c(11) = z03

                            case "BB":
                                c(1) = e1 : c(2) = y3 : c(3) = z12 : c(4) = e2 : c(5) = y4 : c(6) = z22 : c(7) = x3 : c(8) = z3 : c(9) = z012 : c(10) = z022 : c(11) = z03
                            case "BY":
                                c(1) = e1 : c(2) = y3 : c(3) = z12 : c(4) = e2 : c(5) = y4 : c(6) = z21 : c(7) = x3 : c(8) = z3 : c(9) = z012 : c(10) = z021 : c(11) = z03
                            case "0Y":
                                c(1) = e1 : c(2) = y3 : c(3) = 0 : c(4) = e2 : c(5) = y4 : c(6) = z21 : c(7) = x3 : c(8) = z3 : c(9) = 0 : c(10) = z021 : c(11) = z03
                            //case "00":
                            //c(1) = e1 : c(2) = y3 : c(3) = 0 : c(4) = e2 : c(5) = y4 : c(6) = 0 : c(7) = x3 : c(8) = z3 : c(9) = 0 : c(10) = 0 : c(11) = z03
                            //2016.9.24 针对前后侧同时取某一侧的拔力新增,T表示某侧取相反侧上拔力计算
                            case "YT":
                                c(1) = e1 : c(2) = y3 : c(3) = z11 : c(4) = e2 : c(5) = y4 : c(6) = z12 : c(7) = x3 : c(8) = z3 : c(9) = z011 : c(10) = z012 : c(11) = z03
                            case "0T":
                                c(1) = e1 : c(2) = y3 : c(3) = 0 : c(4) = e2 : c(5) = y4 : c(6) = z12 : c(7) = x3 : c(8) = z3 : c(9) = 0 : c(10) = z012 : c(11) = z03
                            case "T0":
                                c(1) = e1 : c(2) = y3 : c(3) = z22 : c(4) = e2 : c(5) = y4 : c(6) = 0 : c(7) = x3 : c(8) = z3 : c(9) = z022 : c(10) = 0 : c(11) = z03
                            case "TY":
                                c(1) = e1 : c(2) = y3 : c(3) = z22 : c(4) = e2 : c(5) = y4 : c(6) = z21 : c(7) = x3 : c(8) = z3 : c(9) = z022 : c(10) = z021 : c(11) = z03
                            //20190113新版程序
                            default;
                                c(1) = e1 : c(2) = y3 : c(4) = e2 : c(5) = y4 : c(7) = x3 : c(8) = z3 : c(11) = z03;
                                if (Math.Abs(zhs) > 10000)
                                {
                                    //垂荷列数互换法则补充
                                    //'特意针对垂荷组合为12情况
                                    //仅适用于不均匀冰和验算不均匀冰工况，其余工况不适用。在检查模式中判断报错

                                    int zhs1 = Convert.ToInt16(Math.Abs(zhs).ToString().Substring(1, 2));
                                    float zzz11 = gmaxF(j, zhs1) * BL2;
                                    float zzz12 = gminF(j, zhs1) * BL2;
                                    float zzz21 = gmaxB(j, zhs1) * BL2;
                                    float zzz22 = gminB(j, zhs1) * BL2;

                                    if (Math.Abs(zhs).ToString().Substring(0, 1) == "1")
                                    {
                                        //前侧垂荷需要替换 万位数为1
                                        if (wd.VertialLoadCode.Substring(0, 1) == "0")
                                        {
                                            c(3) = 0; c(9) = 0;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                        {
                                            c(3) = zzz11; c(9) = z011;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                        {
                                            c(3) = zzz12; c(9) = z012;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                        {
                                            c(3) = zzz21; c(9) = z021;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                        {
                                            c(3) = zzz22; c(9) = z022;
                                        }
                                        else
                                        {
                                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                        }

                                        if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                        {
                                            c(6) = 0; c(10) = 0;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                        {
                                            c(6) = z11; c(10) = z011;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                        {
                                            c(6) = z12; c(10) = z012;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                        {
                                            c(6) = z21; c(10) = z021;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                        {
                                            c(6) = z22; c(10) = z022;
                                        }
                                        else
                                        {
                                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-16");
                                        }
                                    }
                                    else if (Math.Abs(zhs).ToString().Substring(0, 1) == "2")
                                    {
                                        //后侧垂荷需要替换  万位数为2
                                        if (wd.VertialLoadCode.Substring(0, 1) == "0")
                                        {
                                            c(3) = 0; c(9) = 0;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                        {
                                            c(3) = z11; c(9) = z011;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                        {
                                            c(3) = z12; c(9) = z012;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                        {
                                            c(3) = z21; c(9) = z021;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                        {
                                            c(3) = z22; c(9) = z022;
                                        }
                                        else
                                        {
                                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                        }

                                        if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                        {
                                            c(6) = 0; c(10) = 0;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                        {
                                            c(6) = zzz11; c(10) = z011;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                        {
                                            c(6) = zzz12; c(10) = z012;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                        {
                                            c(6) = zzz21; c(10) = z021;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                        {
                                            c(6) = zzz22; c(10) = z022;
                                        }
                                        else
                                        {
                                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-16");
                                        }
                                    }
                                    else
                                    {
                                        throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190115-01");
                                    }

                                }
                                else
                                {
                                    if (wd.VertialLoadCode.Substring(0, 1) == "0")
                                    {
                                        c(3) = 0 : c(9) = 0
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                    {
                                        c(3) = z11 : c(9) = z011
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                    {
                                        c(3) = z12 : c(9) = z012
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                    {
                                        c(3) = z21 : c(9) = z021
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                    {
                                        c(3) = z22 : c(9) = z022
                                    }
                                    else
                                    {
                                        throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                    }

                                    if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                    {
                                        c(6) = 0 : c(10) = 0
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                    {
                                        c(6) = z11; c(10) = z011;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                    {
                                        c(6) = z12; c(10) = z012;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                    {
                                        c(6) = z21; c(10) = z021;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                    {
                                        c(6) = z22; c(10) = z022;
                                    }
                                    else
                                    {
                                        throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-16");
                                    }
                                }
                                break;
                        }
                    }
                }
                else if (wd.TensionAngleCode == "XD")
                {
                    if (zhs > 0)
                    {
                        if (Paras.IsCornerTower || Paras.IsBranchTower)
                        {
                            y3 = y1
                            y4 = y2
                        }
                        else
                        {
                            y3 = y2;
                            y4 = mgdz;
                        }
                    }
                    else
                    {
                        if (Paras.IsCornerTower || Paras.IsBranchTower)
                        {
                            y3 = y1;
                            y4 = y2;
                        }
                        else
                        {
                            y3 = y2;
                            y4 = mgxz;
                        }
                    }

                    if (Paras.baseParaFormRadioButton1 && zhs < 0)
                    {
                        //按最严重情况考虑扭转 20170620新增  扭转相垂荷按最严重情况考虑
                        switch (wd.VertialLoadCode)
                        {
                            case "YY":
                                c(1) = e1 : c(2) = y3 : c(3) = z21 : c(4) = e2 : c(5) = y4 : c(6) = z11 : c(7) = x3 : c(8) = z3 : c(9) = z021 : c(10) = z011 : c(11) = z03
                                break;
                            case "YB":
                                c(1) = e1 : c(2) = y3 : c(3) = z21 : c(4) = e2 : c(5) = y4 : c(6) = z12 : c(7) = x3 : c(8) = z3 : c(9) = z021 : c(10) = z012 : c(11) = z03 //根据酒湖线修改
                                break;
                            case "BB":
                                c(1) = e1 : c(2) = y3 : c(3) = z22 : c(4) = e2 : c(5) = y4 : c(6) = z12 : c(7) = x3 : c(8) = z3 : c(9) = z022 : c(10) = z012 : c(11) = z03
                                break;
                            case "BY":
                                c(1) = e1 : c(2) = y3 : c(3) = z22 : c(4) = e2 : c(5) = y4 : c(6) = z12 : c(7) = x3 : c(8) = z3 : c(9) = z022 : c(10) = z012 : c(11) = z03   //根据酒湖线修改
                                break;
                            //2017.11.1 根据乌东德补充，添加B0等后续组合情况
                            case "0B"
                                c(1) = e1 : c(2) = y3 : c(3) = z22 : c(4) = e2 : c(5) = y4 : c(6) = 0 : c(7) = x3 : c(8) = z3 : c(9) = z022 : c(10) = 0 : c(11) = z03
                            case "B0"
                                c(1) = e1 : c(2) = y3 : c(3) = 0 : c(4) = e2 : c(5) = y4 : c(6) = z12 : c(7) = x3 : c(8) = z3 : c(9) = 0 : c(10) = z012 : c(11) = z03
                            case "Y0"
                                c(1) = e1 : c(2) = y3 : c(3) = 0 : c(4) = e2 : c(5) = y4 : c(6) = z11 : c(7) = x3 : c(8) = z3 : c(9) = 0 : c(10) = z011 : c(11) = z03
                            case "0Y"
                                c(1) = e1 : c(2) = y3 : c(3) = z21 : c(4) = e2 : c(5) = y4 : c(6) = 0 : c(7) = x3 : c(8) = z3 : c(9) = z021 : c(10) = 0 : c(11) = z03
                            //case "00"
                            //c(1) = e1 : c(2) = y3 : c(3) = 0 : c(4) = e2 : c(5) = y4 : c(6) = 0 : c(7) = x3 : c(8) = z3 : c(9) = 0 : c(10) = 0 : c(11) = z03
                            case "YT"
                                c(1) = e1 : c(2) = y3 : c(3) = z12 : c(4) = e2 : c(5) = y4 : c(6) = z11 : c(7) = x3 : c(8) = z3 : c(9) = z012 : c(10) = z011 : c(11) = z03
                            case "0T"
                                c(1) = e1 : c(2) = y3 : c(3) = z12 : c(4) = e2 : c(5) = y4 : c(6) = 0 : c(7) = x3 : c(8) = z3 : c(9) = z012 : c(10) = 0 : c(11) = z03
                            case "T0"
                                c(1) = e1 : c(2) = y3 : c(3) = 0 : c(4) = e2 : c(5) = y4 : c(6) = z22 : c(7) = x3 : c(8) = z3 : c(9) = 0 : c(10) = z022 : c(11) = z03
                            case "TY"
                                c(1) = e1 : c(2) = y3 : c(3) = z21 : c(4) = e2 : c(5) = y4 : c(6) = z22 : c(7) = x3 : c(8) = z3 : c(9) = z021 : c(10) = z022 : c(11) = z03
                            //20190113新版程序
                            default:
                                c(1) = e1 : c(2) = y3 : c(4) = e2 : c(5) = y4 : c(7) = x3 : c(8) = z3 : c(11) = z03
                                if (Math.Abs(zhs) > 10000)
                                {
                                    //垂荷列数互换法则补充
                                    //'特意针对垂荷组合为12情况
                                    //仅适用于不均匀冰和验算不均匀冰工况，其余工况不适用。在检查模式中判断报错

                                    int zhs1 = Convert.ToInt16(Math.Abs(zhs).ToString().Substring(1, 2));
                                    float zzz11 = gmaxF(j, zhs1) * BL2;
                                    float zzz12 = gminF(j, zhs1) * BL2;
                                    float zzz21 = gmaxB(j, zhs1) * BL2;
                                    float zzz22 = gminB(j, zhs1) * BL2;

                                    if (Math.Abs(zhs).ToString().Substring(0, 1) == "1")
                                    {
                                        //前侧垂荷需要替换 万位数为1
                                        if (wd.VertialLoadCode.Substring(0, 1) == "0")
                                        {
                                            c(3) = 0; c(9) = 0;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                        {
                                            c(3) = zzz11; c(9) = z011;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                        {
                                            c(3) = zzz12; c(9) = z012;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                        {
                                            c(3) = zzz21; c(9) = z021;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                        {
                                            c(3) = zzz22; c(9) = z022;
                                        }
                                        else
                                        {
                                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                        }

                                        if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                        {
                                            c(6) = 0; c(10) = 0;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                        {
                                            c(6) = z11; c(10) = z011;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                        {
                                            c(6) = z12; c(10) = z012;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                        {
                                            c(6) = z21; c(10) = z021;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                        {
                                            c(6) = z22; c(10) = z022;
                                        }
                                        else
                                        {
                                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-16");
                                        }

                                    }
                                    else if (Math.Abs(zhs).ToString().Substring(0, 1) == "2")
                                    {
                                        //后侧垂荷需要替换  万位数为2
                                        if (wd.VertialLoadCode.Substring(0, 1) == "0")
                                        {
                                            c(3) = 0; c(9) = 0;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                        {
                                            c(3) = z11; c(9) = z011;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                        {
                                            c(3) = z12; c(9) = z012;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                        {
                                            c(3) = z21; c(9) = z021;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                        {
                                            c(3) = z22; c(9) = z022;
                                        }
                                        else
                                        {
                                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                        }

                                        if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                        {
                                            c(6) = 0; c(10) = 0;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                        {
                                            c(6) = zzz11; c(10) = z011;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                        {
                                            c(6) = zzz12; c(10) = z012;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                        {
                                            c(6) = zzz21; c(10) = z021;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                        {
                                            c(6) = zzz22; c(10) = z022;
                                        }
                                        else
                                        {
                                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-16");
                                        }

                                    }
                                    else
                                    {
                                        throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190115-01");
                                    }

                                }
                                else
                                {
                                    if (wd.VertialLoadCode.Substring(0, 1) == "0")
                                    {
                                        c(3) = 0; c(9) = 0;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                    {
                                        c(3) = z11; c(9) = z011;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                    {
                                        c(3) = z12; c(9) = z012;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                    {
                                        c(3) = z21; c(9) = z021;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                    {
                                        c(3) = z22; c(9) = z022;
                                    }
                                    else
                                    {
                                        throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                    }

                                    if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                    {
                                        c(6) = 0; c(10) = 0;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                    {
                                        c(6) = z11; c(10) = z011;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                    {
                                        c(6) = z12; c(10) = z012;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                    {
                                        c(6) = z21; c(10) = z021;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                    {
                                        c(6) = z22; c(10) = z022;
                                    }
                                    else
                                    {
                                        throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-16");
                                    }
                                }
                                break;
                        }
                    }
                    else
                    {
                        switch (wd.VertialLoadCode)
                        {
                            case "YY"
                                c(1) = e1 : c(2) = y3 : c(3) = z11 : c(4) = e2 : c(5) = y4 : c(6) = z21 : c(7) = x3 : c(8) = z3 : c(9) = z011 : c(10) = z021 : c(11) = z03
                            case "YB"
                                c(1) = e1 : c(2) = y3 : c(3) = z11 : c(4) = e2 : c(5) = y4 : c(6) = z22 : c(7) = x3 : c(8) = z3 : c(9) = z011 : c(10) = z022 : c(11) = z03
                            case "0B"
                                c(1) = e1 : c(2) = y3 : c(3) = 0 : c(4) = e2 : c(5) = y4 : c(6) = z22 : c(7) = x3 : c(8) = z3 : c(9) = 0 : c(10) = z022 : c(11) = z03
                            case "B0"
                                c(1) = e1 : c(2) = y3 : c(3) = z12 : c(4) = e2 : c(5) = y4 : c(6) = 0 : c(7) = x3 : c(8) = z3 : c(9) = z012 : c(10) = 0 : c(11) = z03
                            case "Y0"
                                c(1) = e1 : c(2) = y3 : c(3) = z11 : c(4) = e2 : c(5) = y4 : c(6) = 0 : c(7) = x3 : c(8) = z3 : c(9) = z011 : c(10) = 0 : c(11) = z03

                            case "BB"
                                c(1) = e1 : c(2) = y3 : c(3) = z12 : c(4) = e2 : c(5) = y4 : c(6) = z22 : c(7) = x3 : c(8) = z3 : c(9) = z012 : c(10) = z022 : c(11) = z03
                            case "BY"
                                c(1) = e1 : c(2) = y3 : c(3) = z12 : c(4) = e2 : c(5) = y4 : c(6) = z21 : c(7) = x3 : c(8) = z3 : c(9) = z012 : c(10) = z021 : c(11) = z03
                            case "0Y"
                                c(1) = e1 : c(2) = y3 : c(3) = 0 : c(4) = e2 : c(5) = y4 : c(6) = z21 : c(7) = x3 : c(8) = z3 : c(9) = 0 : c(10) = z021 : c(11) = z03
                            //case "00"
                            //c(1) = e1 : c(2) = y3 : c(3) = 0 : c(4) = e2 : c(5) = y4 : c(6) = 0 : c(7) = x3 : c(8) = z3 : c(9) = 0 : c(10) = 0 : c(11) = z03
                            //2016.9.24 针对前后侧同时取某一侧的拔力新增,T表示某侧取相反侧上拔力计算
                            case "YT"
                                c(1) = e1 : c(2) = y3 : c(3) = z11 : c(4) = e2 : c(5) = y4 : c(6) = z12 : c(7) = x3 : c(8) = z3 : c(9) = z011 : c(10) = z012 : c(11) = z03
                            case "0T"
                                c(1) = e1 : c(2) = y3 : c(3) = 0 : c(4) = e2 : c(5) = y4 : c(6) = z12 : c(7) = x3 : c(8) = z3 : c(9) = 0 : c(10) = z012 : c(11) = z03
                            case "T0"
                                c(1) = e1 : c(2) = y3 : c(3) = z22 : c(4) = e2 : c(5) = y4 : c(6) = 0 : c(7) = x3 : c(8) = z3 : c(9) = z022 : c(10) = 0 : c(11) = z03
                            case "TY"
                                c(1) = e1 : c(2) = y3 : c(3) = z22 : c(4) = e2 : c(5) = y4 : c(6) = z21 : c(7) = x3 : c(8) = z3 : c(9) = z022 : c(10) = z021 : c(11) = z03
                            //20190113新版程序
                            default;
                                c(1) = e1 : c(2) = y3 : c(4) = e2 : c(5) = y4 : c(7) = x3 : c(8) = z3 : c(11) = z03
                                if (Math.Abs(zhs) > 10000)
                                {
                                    //垂荷列数互换法则补充
                                    //'特意针对垂荷组合为12情况
                                    //仅适用于不均匀冰和验算不均匀冰工况，其余工况不适用。在检查模式中判断报错

                                    int zhs1 = Convert.ToInt16(Math.Abs(zhs).ToString().Substring(1, 2));
                                    float zzz11 = gmaxF(j, zhs1) * BL2;
                                    float zzz12 = gminF(j, zhs1) * BL2;
                                    float zzz21 = gmaxB(j, zhs1) * BL2;
                                    float zzz22 = gminB(j, zhs1) * BL2;

                                    if (Math.Abs(zhs).ToString().Substring(0, 1) == "1")
                                    {
                                        //前侧垂荷需要替换 万位数为1
                                        if (wd.VertialLoadCode.Substring(0, 1) == "0")
                                        {
                                            c(3) = 0; c(9) = 0;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                        {
                                            c(3) = zzz11; c(9) = z011;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                        {
                                            c(3) = zzz12; c(9) = z012;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                        {
                                            c(3) = zzz21; c(9) = z021;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                        {
                                            c(3) = zzz22; c(9) = z022;
                                        }
                                        else
                                        {
                                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                        }

                                        if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                        {
                                            c(6) = 0; c(10) = 0;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                        {
                                            c(6) = z11; c(10) = z011;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                        {
                                            c(6) = z12; c(10) = z012;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                        {
                                            c(6) = z21; c(10) = z021;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                        {
                                            c(6) = z22; c(10) = z022;
                                        }
                                        else
                                        {
                                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-16");
                                        }
                                    }
                                    else if (Math.Abs(zhs).ToString().Substring(0, 1) == "2")
                                    {
                                        //后侧垂荷需要替换  万位数为2
                                        if (wd.VertialLoadCode.Substring(0, 1) == "0")
                                        {
                                            c(3) = 0; c(9) = 0;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                        {
                                            c(3) = z11; c(9) = z011;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                        {
                                            c(3) = z12; c(9) = z012;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                        {
                                            c(3) = z21; c(9) = z021;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                        {
                                            c(3) = z22; c(9) = z022;
                                        }
                                        else
                                        {
                                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                        }

                                        if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                        {
                                            c(6) = 0; c(10) = 0;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                        {
                                            c(6) = zzz11; c(10) = z011;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                        {
                                            c(6) = zzz12; c(10) = z012;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                        {
                                            c(6) = zzz21; c(10) = z021;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                        {
                                            c(6) = zzz22; c(10) = z022;
                                        }
                                        else
                                        {
                                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-16");
                                        }
                                    }
                                    else
                                    {
                                        throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190115-01");
                                    }

                                }
                                else
                                {
                                    if (wd.VertialLoadCode.Substring(0, 1) == "0")
                                    {
                                        c(3) = 0 : c(9) = 0
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                    {
                                        c(3) = z11 : c(9) = z011
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                    {
                                        c(3) = z12 : c(9) = z012
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                    {
                                        c(3) = z21 : c(9) = z021
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                    {
                                        c(3) = z22 : c(9) = z022
                                    }
                                    else
                                    {
                                        throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                    }

                                    if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                    {
                                        c(6) = 0 : c(10) = 0
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                    {
                                        c(6) = z11; c(10) = z011;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                    {
                                        c(6) = z12; c(10) = z012;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                    {
                                        c(6) = z21; c(10) = z021;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                    {
                                        c(6) = z22; c(10) = z022;
                                    }
                                    else
                                    {
                                        throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-16");
                                    }
                                }
                                break;
                        }
                    }
                }
                else if (wd.TensionAngleCode == "XX")
                {
                    if (Paras.IsCornerTower || Paras.IsBranchTower)
                    {
                        y3 = y2;
                        y4 = y2;
                    }
                    else
                    {
                        y3 = y2;
                        y4 = mgxz;
                    }

                    if (Paras.baseParaFormRadioButton1 && zhs < 0)
                    {
                        //按最严重情况考虑扭转 20170620新增  扭转相垂荷按最严重情况考虑
                        switch (wd.VertialLoadCode)
                        {
                            case "YY"
                                c(1) = e1 : c(2) = y3 : c(3) = z21 : c(4) = e2 : c(5) = y4 : c(6) = z11 : c(7) = x3 : c(8) = z3 : c(9) = z021 : c(10) = z011 : c(11) = z03
                            case "YB"
                                c(1) = e1 : c(2) = y3 : c(3) = z21 : c(4) = e2 : c(5) = y4 : c(6) = z12 : c(7) = x3 : c(8) = z3 : c(9) = z021 : c(10) = z012 : c(11) = z03 '根据酒湖线修改
                            case "BB"
                                c(1) = e1 : c(2) = y3 : c(3) = z22 : c(4) = e2 : c(5) = y4 : c(6) = z12 : c(7) = x3 : c(8) = z3 : c(9) = z022 : c(10) = z012 : c(11) = z03
                            case "BY"
                                c(1) = e1 : c(2) = y3 : c(3) = z22 : c(4) = e2 : c(5) = y4 : c(6) = z12 : c(7) = x3 : c(8) = z3 : c(9) = z022 : c(10) = z012 : c(11) = z03   '根据酒湖线修改

                            //2017.11.1 根据乌东德补充，添加B0等后续组合情况
                            case "0B"
                                c(1) = e1 : c(2) = y3 : c(3) = z22 : c(4) = e2 : c(5) = y4 : c(6) = 0 : c(7) = x3 : c(8) = z3 : c(9) = z022 : c(10) = 0 : c(11) = z03
                            case "B0"
                                c(1) = e1 : c(2) = y3 : c(3) = 0 : c(4) = e2 : c(5) = y4 : c(6) = z12 : c(7) = x3 : c(8) = z3 : c(9) = 0 : c(10) = z012 : c(11) = z03
                            case "Y0"
                                c(1) = e1 : c(2) = y3 : c(3) = 0 : c(4) = e2 : c(5) = y4 : c(6) = z11 : c(7) = x3 : c(8) = z3 : c(9) = 0 : c(10) = z011 : c(11) = z03
                            case "0Y"
                                c(1) = e1 : c(2) = y3 : c(3) = z21 : c(4) = e2 : c(5) = y4 : c(6) = 0 : c(7) = x3 : c(8) = z3 : c(9) = z021 : c(10) = 0 : c(11) = z03
                            //case "00"
                            //c(1) = e1 : c(2) = y3 : c(3) = 0 : c(4) = e2 : c(5) = y4 : c(6) = 0 : c(7) = x3 : c(8) = z3 : c(9) = 0 : c(10) = 0 : c(11) = z03
                            case "YT"
                                c(1) = e1 : c(2) = y3 : c(3) = z12 : c(4) = e2 : c(5) = y4 : c(6) = z11 : c(7) = x3 : c(8) = z3 : c(9) = z012 : c(10) = z011 : c(11) = z03
                            case "0T"
                                c(1) = e1 : c(2) = y3 : c(3) = z12 : c(4) = e2 : c(5) = y4 : c(6) = 0 : c(7) = x3 : c(8) = z3 : c(9) = z012 : c(10) = 0 : c(11) = z03
                            case "T0"
                                c(1) = e1 : c(2) = y3 : c(3) = 0 : c(4) = e2 : c(5) = y4 : c(6) = z22 : c(7) = x3 : c(8) = z3 : c(9) = 0 : c(10) = z022 : c(11) = z03
                            case "TY"
                                c(1) = e1 : c(2) = y3 : c(3) = z21 : c(4) = e2 : c(5) = y4 : c(6) = z22 : c(7) = x3 : c(8) = z3 : c(9) = z021 : c(10) = z022 : c(11) = z03
                            //20190113新版程序
                            default:
                                c(1) = e1 : c(2) = y3 : c(4) = e2 : c(5) = y4 : c(7) = x3 : c(8) = z3 : c(11) = z03
                                if (Math.Abs(zhs) > 10000)
                                {
                                    //垂荷列数互换法则补充
                                    //'特意针对垂荷组合为12情况
                                    //仅适用于不均匀冰和验算不均匀冰工况，其余工况不适用。在检查模式中判断报错

                                    int zhs1 = Convert.ToInt16(Math.Abs(zhs).ToString().Substring(1, 2));
                                    float zzz11 = gmaxF(j, zhs1) * BL2;
                                    float zzz12 = gminF(j, zhs1) * BL2;
                                    float zzz21 = gmaxB(j, zhs1) * BL2;
                                    float zzz22 = gminB(j, zhs1) * BL2;

                                    if (Math.Abs(zhs).ToString().Substring(0, 1) == "1")
                                    {
                                        //前侧垂荷需要替换 万位数为1
                                        if (wd.VertialLoadCode.Substring(0, 1) == "0")
                                        {
                                            c(3) = 0; c(9) = 0;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                        {
                                            c(3) = zzz11; c(9) = z011;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                        {
                                            c(3) = zzz12; c(9) = z012;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                        {
                                            c(3) = zzz21; c(9) = z021;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                        {
                                            c(3) = zzz22; c(9) = z022;
                                        }
                                        else
                                        {
                                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                        }

                                        if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                        {
                                            c(6) = 0; c(10) = 0;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                        {
                                            c(6) = z11; c(10) = z011;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                        {
                                            c(6) = z12; c(10) = z012;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                        {
                                            c(6) = z21; c(10) = z021;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                        {
                                            c(6) = z22; c(10) = z022;
                                        }
                                        else
                                        {
                                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-16");
                                        }

                                    }
                                    else if (Math.Abs(zhs).ToString().Substring(0, 1) == "2")
                                    {
                                        //后侧垂荷需要替换  万位数为2
                                        if (wd.VertialLoadCode.Substring(0, 1) == "0")
                                        {
                                            c(3) = 0; c(9) = 0;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                        {
                                            c(3) = z11; c(9) = z011;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                        {
                                            c(3) = z12; c(9) = z012;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                        {
                                            c(3) = z21; c(9) = z021;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                        {
                                            c(3) = z22; c(9) = z022;
                                        }
                                        else
                                        {
                                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                        }

                                        if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                        {
                                            c(6) = 0; c(10) = 0;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                        {
                                            c(6) = zzz11; c(10) = z011;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                        {
                                            c(6) = zzz12; c(10) = z012;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                        {
                                            c(6) = zzz21; c(10) = z021;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                        {
                                            c(6) = zzz22; c(10) = z022;
                                        }
                                        else
                                        {
                                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-16");
                                        }

                                    }
                                    else
                                    {
                                        throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190115-01");
                                    }

                                }
                                else
                                {
                                    if (wd.VertialLoadCode.Substring(0, 1) == "0")
                                    {
                                        c(3) = 0; c(9) = 0;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                    {
                                        c(3) = z11; c(9) = z011;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                    {
                                        c(3) = z12; c(9) = z012;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                    {
                                        c(3) = z21; c(9) = z021;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                    {
                                        c(3) = z22; c(9) = z022;
                                    }
                                    else
                                    {
                                        throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                    }

                                    if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                    {
                                        c(6) = 0; c(10) = 0;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                    {
                                        c(6) = z11; c(10) = z011;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                    {
                                        c(6) = z12; c(10) = z012;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                    {
                                        c(6) = z21; c(10) = z021;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                    {
                                        c(6) = z22; c(10) = z022;
                                    }
                                    else
                                    {
                                        throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-16");
                                    }
                                }
                                break;
                        }
                    }
                    else
                    {
                        switch (wd.VertialLoadCode)
                        {
                            case "YY"
                                c(1) = e1 : c(2) = y3 : c(3) = z11 : c(4) = e2 : c(5) = y4 : c(6) = z21 : c(7) = x3 : c(8) = z3 : c(9) = z011 : c(10) = z021 : c(11) = z03
                            case "YB"
                                c(1) = e1 : c(2) = y3 : c(3) = z11 : c(4) = e2 : c(5) = y4 : c(6) = z22 : c(7) = x3 : c(8) = z3 : c(9) = z011 : c(10) = z022 : c(11) = z03
                            case "0B"
                                c(1) = e1 : c(2) = y3 : c(3) = 0 : c(4) = e2 : c(5) = y4 : c(6) = z22 : c(7) = x3 : c(8) = z3 : c(9) = 0 : c(10) = z022 : c(11) = z03
                            case "B0"
                                c(1) = e1 : c(2) = y3 : c(3) = z12 : c(4) = e2 : c(5) = y4 : c(6) = 0 : c(7) = x3 : c(8) = z3 : c(9) = z012 : c(10) = 0 : c(11) = z03
                            case "Y0"
                                c(1) = e1 : c(2) = y3 : c(3) = z11 : c(4) = e2 : c(5) = y4 : c(6) = 0 : c(7) = x3 : c(8) = z3 : c(9) = z011 : c(10) = 0 : c(11) = z03

                            case "BB"
                                c(1) = e1 : c(2) = y3 : c(3) = z12 : c(4) = e2 : c(5) = y4 : c(6) = z22 : c(7) = x3 : c(8) = z3 : c(9) = z012 : c(10) = z022 : c(11) = z03
                            case "BY"
                                c(1) = e1 : c(2) = y3 : c(3) = z12 : c(4) = e2 : c(5) = y4 : c(6) = z21 : c(7) = x3 : c(8) = z3 : c(9) = z012 : c(10) = z021 : c(11) = z03
                            case "0Y"
                                c(1) = e1 : c(2) = y3 : c(3) = 0 : c(4) = e2 : c(5) = y4 : c(6) = z21 : c(7) = x3 : c(8) = z3 : c(9) = 0 : c(10) = z021 : c(11) = z03
                            //case "00"
                            //c(1) = e1 : c(2) = y3 : c(3) = 0 : c(4) = e2 : c(5) = y4 : c(6) = 0 : c(7) = x3 : c(8) = z3 : c(9) = 0 : c(10) = 0 : c(11) = z03
                            //2016.9.24 针对前后侧同时取某一侧的拔力新增,T表示某侧取相反侧上拔力计算
                            case "YT"
                                c(1) = e1 : c(2) = y3 : c(3) = z11 : c(4) = e2 : c(5) = y4 : c(6) = z12 : c(7) = x3 : c(8) = z3 : c(9) = z011 : c(10) = z012 : c(11) = z03
                            case "0T"
                                c(1) = e1 : c(2) = y3 : c(3) = 0 : c(4) = e2 : c(5) = y4 : c(6) = z12 : c(7) = x3 : c(8) = z3 : c(9) = 0 : c(10) = z012 : c(11) = z03
                            case "T0"
                                c(1) = e1 : c(2) = y3 : c(3) = z22 : c(4) = e2 : c(5) = y4 : c(6) = 0 : c(7) = x3 : c(8) = z3 : c(9) = z022 : c(10) = 0 : c(11) = z03
                            case "TY"
                                c(1) = e1 : c(2) = y3 : c(3) = z22 : c(4) = e2 : c(5) = y4 : c(6) = z21 : c(7) = x3 : c(8) = z3 : c(9) = z022 : c(10) = z021 : c(11) = z03
                            //20190113新版程序
                            default;
                                c(1) = e1 : c(2) = y3 : c(4) = e2 : c(5) = y4 : c(7) = x3 : c(8) = z3 : c(11) = z03
                                if (Math.Abs(zhs) > 10000)
                                {
                                    //垂荷列数互换法则补充
                                    //'特意针对垂荷组合为12情况
                                    //仅适用于不均匀冰和验算不均匀冰工况，其余工况不适用。在检查模式中判断报错

                                    int zhs1 = Convert.ToInt16(Math.Abs(zhs).ToString().Substring(1, 2));
                                    float zzz11 = gmaxF(j, zhs1) * BL2;
                                    float zzz12 = gminF(j, zhs1) * BL2;
                                    float zzz21 = gmaxB(j, zhs1) * BL2;
                                    float zzz22 = gminB(j, zhs1) * BL2;

                                    if (Math.Abs(zhs).ToString().Substring(0, 1) == "1")
                                    {
                                        //前侧垂荷需要替换 万位数为1
                                        if (wd.VertialLoadCode.Substring(0, 1) == "0")
                                        {
                                            c(3) = 0; c(9) = 0;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                        {
                                            c(3) = zzz11; c(9) = z011;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                        {
                                            c(3) = zzz12; c(9) = z012;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                        {
                                            c(3) = zzz21; c(9) = z021;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                        {
                                            c(3) = zzz22; c(9) = z022;
                                        }
                                        else
                                        {
                                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                        }

                                        if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                        {
                                            c(6) = 0; c(10) = 0;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                        {
                                            c(6) = z11; c(10) = z011;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                        {
                                            c(6) = z12; c(10) = z012;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                        {
                                            c(6) = z21; c(10) = z021;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                        {
                                            c(6) = z22; c(10) = z022;
                                        }
                                        else
                                        {
                                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-16");
                                        }
                                    }
                                    else if (Math.Abs(zhs).ToString().Substring(0, 1) == "2")
                                    {
                                        //后侧垂荷需要替换  万位数为2
                                        if (wd.VertialLoadCode.Substring(0, 1) == "0")
                                        {
                                            c(3) = 0; c(9) = 0;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                        {
                                            c(3) = z11; c(9) = z011;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                        {
                                            c(3) = z12; c(9) = z012;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                        {
                                            c(3) = z21; c(9) = z021;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                        {
                                            c(3) = z22; c(9) = z022;
                                        }
                                        else
                                        {
                                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                        }

                                        if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                        {
                                            c(6) = 0; c(10) = 0;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                        {
                                            c(6) = zzz11; c(10) = z011;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                        {
                                            c(6) = zzz12; c(10) = z012;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                        {
                                            c(6) = zzz21; c(10) = z021;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                        {
                                            c(6) = zzz22; c(10) = z022;
                                        }
                                        else
                                        {
                                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-16");
                                        }
                                    }
                                    else
                                    {
                                        throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190115-01");
                                    }

                                }
                                else
                                {
                                    if (wd.VertialLoadCode.Substring(0, 1) == "0")
                                    {
                                        c(3) = 0; c(9) = 0;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                    {
                                        c(3) = z11 : c(9) = z011
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                    {
                                        c(3) = z12 : c(9) = z012
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                    {
                                        c(3) = z21 : c(9) = z021
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                    {
                                        c(3) = z22 : c(9) = z022
                                    }
                                    else
                                    {
                                        throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                    }

                                    if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                    {
                                        c(6) = 0 : c(10) = 0
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                    {
                                        c(6) = z11; c(10) = z011;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                    {
                                        c(6) = z12; c(10) = z012;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                    {
                                        c(6) = z21; c(10) = z021;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                    {
                                        c(6) = z22; c(10) = z022;
                                    }
                                    else
                                    {
                                        throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-16");
                                    }
                                }
                                break;
                        }
                    }
                }
                else if (wd.TensionAngleCode == "D0")
                {
                    if (Paras.IsCornerTower || Paras.IsBranchTower)
                    {
                        y3 = y1;
                        y4 = 0;
                    }
                    else
                    {
                        y3 = y1;
                        y4 = 0;
                    }

                    //按最严重情况考虑扭转 20170620新增  扭转相垂荷按最严重情况考虑
                    switch (wd.VertialLoadCode)
                    {
                        //case "00"
                        //    c(1) = e1 : c(2) = y3 : c(3) = 0 : c(4) = 0 : c(5) = 0 : c(6) = 0 : c(7) = 0 : c(8) = 0 : c(9) = 0 : c(10) = 0 : c(11) = 0
                        case "B0"
                            c(1) = e1 : c(2) = y3 : c(3) = z12 : c(4) = 0 : c(5) = y4 : c(6) = 0 : c(7) = 0 : c(8) = 0 : c(9) = z012 : c(10) = 0 : c(11) = 0
                        case "Y0"
                            c(1) = e1 : c(2) = y3 : c(3) = z11 : c(4) = 0 : c(5) = y4 : c(6) = 0 : c(7) = 0 : c(8) = 0 : c(9) = z011 : c(10) = 0 : c(11) = 0
                        //2016.9.24 针对前后侧同时取某一侧的拔力新增,T表示某侧取相反侧上拔力计算
                        case "T0"
                            c(1) = e1 : c(2) = y3 : c(3) = z22 : c(4) = 0 : c(5) = y4 : c(6) = 0 : c(7) = 0 : c(8) = 0 : c(9) = z022 : c(10) = 0 : c(11) = 0
                        //20190113新版程序
                        default:
                            c(1) = e1 : c(2) = y3 : c(4) = 0 : c(5) = 0 : c(7) = 0 : c(8) = 0 : c(11) = 0
                            if (Math.Abs(zhs) > 10000)
                            {
                                //垂荷列数互换法则补充
                                //'特意针对垂荷组合为12情况
                                //仅适用于不均匀冰和验算不均匀冰工况，其余工况不适用。在检查模式中判断报错

                                int zhs1 = Convert.ToInt16(Math.Abs(zhs).ToString().Substring(1, 2));
                                float zzz11 = gmaxF(j, zhs1) * BL2;
                                float zzz12 = gminF(j, zhs1) * BL2;
                                float zzz21 = gmaxB(j, zhs1) * BL2;
                                float zzz22 = gminB(j, zhs1) * BL2;

                                if (Math.Abs(zhs).ToString().Substring(0, 1) == "1")
                                {
                                    //前侧垂荷需要替换 万位数为1
                                    if (wd.VertialLoadCode.Substring(0, 1) == "0")
                                    {
                                        c(3) = 0; c(9) = 0;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                    {
                                        c(3) = zzz11; c(9) = z011;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                    {
                                        c(3) = zzz12; c(9) = z012;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                    {
                                        c(3) = zzz21; c(9) = z021;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                    {
                                        c(3) = zzz22; c(9) = z022;
                                    }
                                    else
                                    {
                                        throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                    }

                                    if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                    {
                                        c(6) = 0; c(10) = 0;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                    {
                                        c(6) = z11; c(10) = z011;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                    {
                                        c(6) = z12; c(10) = z012;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                    {
                                        c(6) = z21; c(10) = z021;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                    {
                                        c(6) = z22; c(10) = z022;
                                    }
                                    else
                                    {
                                        throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-16");
                                    }

                                }
                                else if (Math.Abs(zhs).ToString().Substring(0, 1) == "2")
                                {
                                    //后侧垂荷需要替换  万位数为2
                                    if (wd.VertialLoadCode.Substring(0, 1) == "0")
                                    {
                                        c(3) = 0; c(9) = 0;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                    {
                                        c(3) = z11; c(9) = z011;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                    {
                                        c(3) = z12; c(9) = z012;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                    {
                                        c(3) = z21; c(9) = z021;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                    {
                                        c(3) = z22; c(9) = z022;
                                    }
                                    else
                                    {
                                        throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                    }

                                    if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                    {
                                        c(6) = 0; c(10) = 0;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                    {
                                        c(6) = zzz11; c(10) = z011;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                    {
                                        c(6) = zzz12; c(10) = z012;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                    {
                                        c(6) = zzz21; c(10) = z021;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                    {
                                        c(6) = zzz22; c(10) = z022;
                                    }
                                    else
                                    {
                                        throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-16");
                                    }

                                }
                                else
                                {
                                    throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190115-01");
                                }

                            }
                            else
                            {
                                if (wd.VertialLoadCode.Substring(0, 1) == "0")
                                {
                                    c(3) = 0; c(9) = 0;
                                }
                                else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                {
                                    c(3) = z11; c(9) = z011;
                                }
                                else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                {
                                    c(3) = z12; c(9) = z012;
                                }
                                else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                {
                                    c(3) = z21; c(9) = z021;
                                }
                                else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                {
                                    c(3) = z22; c(9) = z022;
                                }
                                else
                                {
                                    throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                }

                                if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                {
                                    c(6) = 0; c(10) = 0;
                                }
                                else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                {
                                    c(6) = z11; c(10) = z011;
                                }
                                else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                {
                                    c(6) = z12; c(10) = z012;
                                }
                                else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                {
                                    c(6) = z21; c(10) = z021;
                                }
                                else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                {
                                    c(6) = z22; c(10) = z022;
                                }
                                else
                                {
                                    throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-16");
                                }
                            }
                            break;
                    }

                }
                else if (wd.TensionAngleCode == "X0")
                {
                    if (Paras.IsCornerTower || Paras.IsBranchTower)
                    {
                        y3 = y2;
                        y4 = 0;
                    }
                    else
                    {
                        y3 = y2;
                        y4 = 0;
                    }

                    //按最严重情况考虑扭转 20170620新增  扭转相垂荷按最严重情况考虑
                    switch (wd.VertialLoadCode)
                    {
                        //case "00"
                        //     c(1) = e1 : c(2) = y3 : c(3) = 0 : c(4) = 0 : c(5) = 0 : c(6) = 0 : c(7) = 0 : c(8) = 0 : c(9) = 0 : c(10) = 0 : c(11) = 0
                        case "B0"
                            c(1) = e1 : c(2) = y3 : c(3) = z12 : c(4) = 0 : c(5) = y4 : c(6) = 0 : c(7) = 0 : c(8) = 0 : c(9) = z012 : c(10) = 0 : c(11) = 0
                        case "Y0"
                            c(1) = e1 : c(2) = y3 : c(3) = z11 : c(4) = 0 : c(5) = y4 : c(6) = 0 : c(7) = 0 : c(8) = 0 : c(9) = z011 : c(10) = 0 : c(11) = 0
                            //2016.9.24 针对前后侧同时取某一侧的拔力新增,T表示某侧取相反侧上拔力计算
                        case "T0"
                            c(1) = e1 : c(2) = y3 : c(3) = z22 : c(4) = 0 : c(5) = y4 : c(6) = 0 : c(7) = 0 : c(8) = 0 : c(9) = z022 : c(10) = 0 : c(11) = 0
                        //20190113新版程序
                        default:
                            c(1) = e1 : c(2) = y3 : c(4) = 0 : c(5) = 0 : c(7) = 0 : c(8) = 0 : c(11) = 0
                            if (Math.Abs(zhs) > 10000)
                            {
                                //垂荷列数互换法则补充
                                //'特意针对垂荷组合为12情况
                                //仅适用于不均匀冰和验算不均匀冰工况，其余工况不适用。在检查模式中判断报错

                                int zhs1 = Convert.ToInt16(Math.Abs(zhs).ToString().Substring(1, 2));
                                float zzz11 = gmaxF(j, zhs1) * BL2;
                                float zzz12 = gminF(j, zhs1) * BL2;
                                float zzz21 = gmaxB(j, zhs1) * BL2;
                                float zzz22 = gminB(j, zhs1) * BL2;

                                if (Math.Abs(zhs).ToString().Substring(0, 1) == "1")
                                {
                                    //前侧垂荷需要替换 万位数为1
                                    if (wd.VertialLoadCode.Substring(0, 1) == "0")
                                    {
                                        c(3) = 0; c(9) = 0;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                    {
                                        c(3) = zzz11; c(9) = z011;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                    {
                                        c(3) = zzz12; c(9) = z012;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                    {
                                        c(3) = zzz21; c(9) = z021;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                    {
                                        c(3) = zzz22; c(9) = z022;
                                    }
                                    else
                                    {
                                        throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                    }

                                    if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                    {
                                        c(6) = 0; c(10) = 0;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                    {
                                        c(6) = z11; c(10) = z011;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                    {
                                        c(6) = z12; c(10) = z012;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                    {
                                        c(6) = z21; c(10) = z021;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                    {
                                        c(6) = z22; c(10) = z022;
                                    }
                                    else
                                    {
                                        throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-16");
                                    }

                                }
                                else if (Math.Abs(zhs).ToString().Substring(0, 1) == "2")
                                {
                                    //后侧垂荷需要替换  万位数为2
                                    if (wd.VertialLoadCode.Substring(0, 1) == "0")
                                    {
                                        c(3) = 0; c(9) = 0;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                    {
                                        c(3) = z11; c(9) = z011;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                    {
                                        c(3) = z12; c(9) = z012;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                    {
                                        c(3) = z21; c(9) = z021;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                    {
                                        c(3) = z22; c(9) = z022;
                                    }
                                    else
                                    {
                                        throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                    }

                                    if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                    {
                                        c(6) = 0; c(10) = 0;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                    {
                                        c(6) = zzz11; c(10) = z011;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                    {
                                        c(6) = zzz12; c(10) = z012;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                    {
                                        c(6) = zzz21; c(10) = z021;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                    {
                                        c(6) = zzz22; c(10) = z022;
                                    }
                                    else
                                    {
                                        throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-16");
                                    }

                                }
                                else
                                {
                                    throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190115-01");
                                }

                            }
                            else
                            {
                                if (wd.VertialLoadCode.Substring(0, 1) == "0")
                                {
                                    c(3) = 0; c(9) = 0;
                                }
                                else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                {
                                    c(3) = z11; c(9) = z011;
                                }
                                else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                {
                                    c(3) = z12; c(9) = z012;
                                }
                                else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                {
                                    c(3) = z21; c(9) = z021;
                                }
                                else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                {
                                    c(3) = z22; c(9) = z022;
                                }
                                else
                                {
                                    throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                }

                                if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                {
                                    c(6) = 0; c(10) = 0;
                                }
                                else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                {
                                    c(6) = z11; c(10) = z011;
                                }
                                else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                {
                                    c(6) = z12; c(10) = z012;
                                }
                                else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                {
                                    c(6) = z21; c(10) = z021;
                                }
                                else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                {
                                    c(6) = z22; c(10) = z022;
                                }
                                else
                                {
                                    throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-16");
                                }
                            }
                            break;
                    }

                }
                else if (wd.TensionAngleCode == "0D")
                {
                    if (Paras.IsCornerTower || Paras.IsBranchTower)
                    {
                        y3 = 0;
                        y4 = y1;
                    }
                    else
                    {
                        y3 = 0;
                        y4 = mgdz;
                    }

                    //按最严重情况考虑扭转 20170620新增  扭转相垂荷按最严重情况考虑
                    switch (wd.VertialLoadCode)
                    {
                        //case "00"
                        //c(1) = 0 : c(2) = 0 : c(3) = 0 : c(4) = 0 : c(5) = e2 : c(6) = y4 : c(7) = 0 : c(8) = 0 : c(9) = 0 : c(10) = 0 : c(11) = 0
                        case "0B"
                            c(1) = 0 : c(2) = 0 : c(3) = 0 : c(4) = e2 : c(5) = y4 : c(6) = z22 : c(7) = 0 : c(8) = 0 : c(9) = 0 : c(10) = z022 : c(11) = 0
                        case "0Y"
                            c(1) = 0 : c(2) = 0 : c(3) = 0 : c(4) = e2 : c(5) = y4 : c(6) = z21 : c(7) = 0 : c(8) = 0 : c(9) = 0 : c(10) = z012 : c(11) = 0
                            //2016.9.24 针对前后侧同时取某一侧的拔力新增,T表示某侧取相反侧上拔力计算
                        case "0T"
                            c(1) = 0 : c(2) = 0 : c(3) = 0 : c(4) = e2 : c(5) = y4 : c(6) = z12 : c(7) = 0 : c(8) = 0 : c(9) = 0 : c(10) = z012 : c(11) = 0
                        //20190113新版程序
                        default:
                            c(1) = 0 : c(2) = 0 : c(4) = e2 : c(5) = y4 : c(7) = 0 : c(8) = 0 : c(11) = 0
                            if (Math.Abs(zhs) > 10000)
                            {
                                //垂荷列数互换法则补充
                                //'特意针对垂荷组合为12情况
                                //仅适用于不均匀冰和验算不均匀冰工况，其余工况不适用。在检查模式中判断报错

                                int zhs1 = Convert.ToInt16(Math.Abs(zhs).ToString().Substring(1, 2));
                                float zzz11 = gmaxF(j, zhs1) * BL2;
                                float zzz12 = gminF(j, zhs1) * BL2;
                                float zzz21 = gmaxB(j, zhs1) * BL2;
                                float zzz22 = gminB(j, zhs1) * BL2;

                                if (Math.Abs(zhs).ToString().Substring(0, 1) == "1")
                                {
                                    //前侧垂荷需要替换 万位数为1
                                    if (wd.VertialLoadCode.Substring(0, 1) == "0")
                                    {
                                        c(3) = 0; c(9) = 0;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                    {
                                        c(3) = zzz11; c(9) = z011;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                    {
                                        c(3) = zzz12; c(9) = z012;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                    {
                                        c(3) = zzz21; c(9) = z021;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                    {
                                        c(3) = zzz22; c(9) = z022;
                                    }
                                    else
                                    {
                                        throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                    }

                                    if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                    {
                                        c(6) = 0; c(10) = 0;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                    {
                                        c(6) = z11; c(10) = z011;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                    {
                                        c(6) = z12; c(10) = z012;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                    {
                                        c(6) = z21; c(10) = z021;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                    {
                                        c(6) = z22; c(10) = z022;
                                    }
                                    else
                                    {
                                        throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-16");
                                    }

                                }
                                else if (Math.Abs(zhs).ToString().Substring(0, 1) == "2")
                                {
                                    //后侧垂荷需要替换  万位数为2
                                    if (wd.VertialLoadCode.Substring(0, 1) == "0")
                                    {
                                        c(3) = 0; c(9) = 0;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                    {
                                        c(3) = z11; c(9) = z011;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                    {
                                        c(3) = z12; c(9) = z012;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                    {
                                        c(3) = z21; c(9) = z021;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                    {
                                        c(3) = z22; c(9) = z022;
                                    }
                                    else
                                    {
                                        throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                    }

                                    if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                    {
                                        c(6) = 0; c(10) = 0;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                    {
                                        c(6) = zzz11; c(10) = z011;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                    {
                                        c(6) = zzz12; c(10) = z012;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                    {
                                        c(6) = zzz21; c(10) = z021;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                    {
                                        c(6) = zzz22; c(10) = z022;
                                    }
                                    else
                                    {
                                        throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-16");
                                    }

                                }
                                else
                                {
                                    throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190115-01");
                                }

                            }
                            else
                            {
                                if (wd.VertialLoadCode.Substring(0, 1) == "0")
                                {
                                    c(3) = 0; c(9) = 0;
                                }
                                else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                {
                                    c(3) = z11; c(9) = z011;
                                }
                                else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                {
                                    c(3) = z12; c(9) = z012;
                                }
                                else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                {
                                    c(3) = z21; c(9) = z021;
                                }
                                else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                {
                                    c(3) = z22; c(9) = z022;
                                }
                                else
                                {
                                    throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                }

                                if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                {
                                    c(6) = 0; c(10) = 0;
                                }
                                else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                {
                                    c(6) = z11; c(10) = z011;
                                }
                                else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                {
                                    c(6) = z12; c(10) = z012;
                                }
                                else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                {
                                    c(6) = z21; c(10) = z021;
                                }
                                else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                {
                                    c(6) = z22; c(10) = z022;
                                }
                                else
                                {
                                    throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-16");
                                }
                            }
                            break;
                    }
                }
                //else if (wd.TensionAngleCode == "0X")
                else
                {
                    if (Paras.IsCornerTower || Paras.IsBranchTower)
                    {
                        y3 = 0;
                        y4 = y1;
                    }
                    else
                    {
                        y3 = 0;
                        y4 = mgxz;
                    }

                    //按最严重情况考虑扭转 20170620新增  扭转相垂荷按最严重情况考虑
                    switch (wd.VertialLoadCode)
                    {
                        //case "00"
                        //c(1) = 0 : c(2) = 0 : c(3) = 0 : c(4) = e2 : c(5) = y4 : c(6) = 0 : c(7) = 0 : c(8) = 0 : c(9) = 0 : c(10) = 0 : c(11) = 0
                        case "0B"
                            c(1) = 0 : c(2) = 0 : c(3) = 0 : c(4) = e2 : c(5) = y4 : c(6) = z22 : c(7) = 0 : c(8) = 0 : c(9) = 0 : c(10) = z022 : c(11) = 0
                        case "0Y"
                            c(1) = 0 : c(2) = 0 : c(3) = 0 : c(4) = e2 : c(5) = y4 : c(6) = z21 : c(7) = 0 : c(8) = 0 : c(9) = 0 : c(10) = z012 : c(11) = 0
                        //2016.9.24 针对前后侧同时取某一侧的拔力新增,T表示某侧取相反侧上拔力计算
                        case "0T"
                            c(1) = 0 : c(2) = 0 : c(3) = 0 : c(4) = e2 : c(5) = y4 : c(6) = z12 : c(7) = 0 : c(8) = 0 : c(9) = 0 : c(10) = z012 : c(11) = 0
                        //20190113新版程序
                        default:
                            c(1) = 0 : c(2) = 0 : c(4) = e2 : c(5) = y4 : c(7) = 0 : c(8) = 0 : c(11) = 0
                            if (Math.Abs(zhs) > 10000)
                            {
                                //垂荷列数互换法则补充
                                //'特意针对垂荷组合为12情况
                                //仅适用于不均匀冰和验算不均匀冰工况，其余工况不适用。在检查模式中判断报错

                                int zhs1 = Convert.ToInt16(Math.Abs(zhs).ToString().Substring(1, 2));
                                float zzz11 = gmaxF(j, zhs1) * BL2;
                                float zzz12 = gminF(j, zhs1) * BL2;
                                float zzz21 = gmaxB(j, zhs1) * BL2;
                                float zzz22 = gminB(j, zhs1) * BL2;

                                if (Math.Abs(zhs).ToString().Substring(0, 1) == "1")
                                {
                                    //前侧垂荷需要替换 万位数为1
                                    if (wd.VertialLoadCode.Substring(0, 1) == "0")
                                    {
                                        c(3) = 0; c(9) = 0;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                    {
                                        c(3) = zzz11; c(9) = z011;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                    {
                                        c(3) = zzz12; c(9) = z012;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                    {
                                        c(3) = zzz21; c(9) = z021;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                    {
                                        c(3) = zzz22; c(9) = z022;
                                    }
                                    else
                                    {
                                        throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                    }

                                    if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                    {
                                        c(6) = 0; c(10) = 0;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                    {
                                        c(6) = z11; c(10) = z011;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                    {
                                        c(6) = z12; c(10) = z012;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                    {
                                        c(6) = z21; c(10) = z021;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                    {
                                        c(6) = z22; c(10) = z022;
                                    }
                                    else
                                    {
                                        throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-16");
                                    }

                                }
                                else if (Math.Abs(zhs).ToString().Substring(0, 1) == "2")
                                {
                                    //后侧垂荷需要替换  万位数为2
                                    if (wd.VertialLoadCode.Substring(0, 1) == "0")
                                    {
                                        c(3) = 0; c(9) = 0;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                    {
                                        c(3) = z11; c(9) = z011;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                    {
                                        c(3) = z12; c(9) = z012;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                    {
                                        c(3) = z21; c(9) = z021;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                    {
                                        c(3) = z22; c(9) = z022;
                                    }
                                    else
                                    {
                                        throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                    }

                                    if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                    {
                                        c(6) = 0; c(10) = 0;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                    {
                                        c(6) = zzz11; c(10) = z011;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                    {
                                        c(6) = zzz12; c(10) = z012;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                    {
                                        c(6) = zzz21; c(10) = z021;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                    {
                                        c(6) = zzz22; c(10) = z022;
                                    }
                                    else
                                    {
                                        throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-16");
                                    }

                                }
                                else
                                {
                                    throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190115-01");
                                }

                            }
                            else
                            {
                                if (wd.VertialLoadCode.Substring(0, 1) == "0")
                                {
                                    c(3) = 0; c(9) = 0;
                                }
                                else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                {
                                    c(3) = z11; c(9) = z011;
                                }
                                else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                {
                                    c(3) = z12; c(9) = z012;
                                }
                                else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                {
                                    c(3) = z21; c(9) = z021;
                                }
                                else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                {
                                    c(3) = z22; c(9) = z022;
                                }
                                else
                                {
                                    throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                }

                                if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                {
                                    c(6) = 0; c(10) = 0;
                                }
                                else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                {
                                    c(6) = z11; c(10) = z011;
                                }
                                else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                {
                                    c(6) = z12; c(10) = z012;
                                }
                                else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                {
                                    c(6) = z21; c(10) = z021;
                                }
                                else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                {
                                    c(6) = z22; c(10) = z022;
                                }
                                else
                                {
                                    throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-16");
                                }
                            }
                            break;
                    }
                }

                if (Paras.IsBranchTower)
                {
                    //前侧转角  20161124分支塔添加
                    e3 = hzjiao;
                }

                XLF(i, j) = ZIBUYX(angle, e3, c(1), c(2), e5, Vloadx) * BL3;
                YLF(i, j) = ZIBUYY(angle, e3, c(1), c(2), e5, Vloadx);
                ZLF(i, j) = ZIBUYZ(c(9), c(3), e5, Vloadx, rg);

                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fx= " + strX);
                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fy= " + strY);
                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fz= " + strZ);

                if (Paras.IsBranchTower)
                {
                    //前侧转角  20161124分支塔添加
                    e3 = hzjiao;
                }

                XLB(i, j) = ZIBUYX(angle, e3, c(4), c(5), e5, Vloadx) * BL3
                YLB(i, j) = ZIBUYY(angle, e3, c(4), -c(5), e5, Vloadx)
                ZLB(i, j) = ZIBUYZ(c(10), c(6), e5, Vloadx, rg)

                ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fx= " + strX);
                ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fy= " + strY);
                ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fz= " + strZ);

                if (nt % 3 != 0 || nt == 0)
                {
                    //跳线串始终为一种计算方法
                    XTF(i, j) = ZIBUYX(angle, 0, c(7), 0, e5, Vloadx) * BL3 * nt * BLTQ
                    YTF(i, j) = ZIBUYY(angle, 0, c(7), 0, e5, Vloadx) * nt * BLTQ
                    ZTF(i, j) = ZIBUYZ(c(11), c(8), e5, Vloadx, rg) * nt * BLTQ

                    ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fx= " + strX);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fy= " + strY);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fz= " + strZ);

                    XTB(i, j) = ZIBUYX(angle, 0, c(7), 0, e5, Vloadx) * BL3 * nt * BLTH
                    YTB(i, j) = ZIBUYY(angle, 0, c(7), 0, e5, Vloadx) * nt * BLTH
                    ZTB(i, j) = ZIBUYZ(c(11), c(8), e5, Vloadx, rg) * nt * BLTH

                    ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= " + strX);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= " + strY);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= " + strZ);
                }
                else
                {
                    XTF(i, j) = ZIBUYX(angle, 0, c(7), 0, e5, Vloadx) * BL3 * nt * BLTQ
                    YTF(i, j) = ZIBUYY(angle, 0, c(7), 0, e5, Vloadx) * nt * BLTQ
                    ZTF(i, j) = ZIBUYZ(c(11), c(8), e5, Vloadx, rg) * nt * BLTQ
                    
                    ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fx= " + strX);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fy= " + strY);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fz= " + strZ);

                    XTC(i, j) = ZIBUYX(angle, 0, c(7), 0, e5, Vloadx) * BL3 * nt * BLTZ
                    YTC(i, j) = ZIBUYY(angle, 0, c(7), 0, e5, Vloadx) * nt * BLTZ
                    ZTC(i, j) = ZIBUYZ(c(11), c(8), e5, Vloadx, rg) * nt * BLTZ

                    ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fx= " + strX);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fy= " + strY);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fz= " + strZ);

                    XTB(i, j) = ZIBUYX(angle, 0, c(7), 0, e5, Vloadx) * BL3 * nt * BLTH
                    YTB(i, j) = ZIBUYY(angle, 0, c(7), 0, e5, Vloadx) * nt * BLTH
                    ZTB(i, j) = ZIBUYZ(c(11), c(8), e5, Vloadx, rg) * nt * BLTH

                    ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= " + strX);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= " + strY);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= " + strZ);
                }
            }
            else if(zhs == 0)
            {
                //该向无荷载
                BL2 = 0;
                XLF(i, j) = 0;
                YLF(i, j) = 0;
                ZLF(i, j) = 0;

                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fx= " + strX);
                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fy= " + strY);
                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fz= " + strZ);

                XLB(i, j) = 0;
                YLB(i, j) = 0;
                ZLB(i, j) = 0;

                ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fx= " + strX);
                ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fy= " + strY);
                ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fz= " + strZ);

                //跳线串始终为一种计算方法
                if (nt % 3 != 0 || nt == 0)
                {
                    XTF(i, j) = 0;
                    YTF(i, j) = 0;
                    ZTF(i, j) = 0;

                    ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fx= " + strX);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fy= " + strY);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fz= " + strZ);

                    XTB(i, j) = 0;
                    YTB(i, j) = 0;
                    ZTB(i, j) = 0;

                    ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= " + strX);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= " + strY);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= " + strZ);
                }
                else
                {
                    XTF(i, j) = 0;
                    YTF(i, j) = 0;
                    ZTF(i, j) = 0;

                    ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fx= " + strX);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fy= " + strY);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fz= " + strZ);

                    XTC(i, j) = 0;
                    YTC(i, j) = 0;
                    ZTC(i, j) = 0;

                    ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fx= " + strX);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fy= " + strY);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fz= " + strZ);

                    XTB(i, j) = 0;
                    YTB(i, j) = 0;
                    ZTB(i, j) = 0;

                    ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= " + strX);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= " + strY);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= " + strZ);
                }
            }
        }

        /// <summary>
        /// 针对脱冰跳跃工况
        /// 工况代码"L1","L2","L1a","L2a","L1b","L2b",
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        protected void DistributeInJump(int i, int j)
        {
            float x1, y1, y2, z1, z2;
            int fuhao;

            WorkConditionCombo wd = Template.WorkConditionCombos[i];

            int zhs = wd.WirdIndexCodes[j-1], zhsx = zhs % 100;
            int angle = wd.WindDirectionCode;
            string workConditionCode = wd.WorkConditionCode;
            int mz1 = Template.WorkConditongs.Count;

            BL2 = 1;
            y1 = tensionD(j, zhsx) * BL2;
            y2 = tensionX(j, zhsx) * BL2;
            x11 = windDF(j, zhsx) * BL2;
            x12 = windXF(j, zhsx) * BL2;
            x21 = windDB(j, zhsx) * BL2;
            x22 = windXB(j, zhsx) * BL2;
            x3 = windTX(j, zhsx) * BL2;
            z11 = gmaxF(j, zhsx) * BL2;
            z12 = gminF(j, zhsx) * BL2;
            z21 = gmaxB(j, zhsx) * BL2;
            z22 = gminB(j, zhsx) * BL2;
            z3 = gTX(j, zhsx) * BL2;

            if (wd.WorkConditionCode == "L1" || wd.WorkConditionCode == "L1a" || wd.WorkConditionCode == "L1b")
            {
                e1 = x11 : e2 = x21 : e3 = maxangle;
            }
            else
            {
                e1 = x12 : e2 = x22 : e3 = minangle;
            }


            if (Math.Abs(zhs) <= mz1 && Math.Abs(zhs) > 0)
            {
                //正常覆冰相
                if (zhs > 0)
                {
                    fuhao = 1;
                    y1 = TensionMax[j,Math.Abs(zhs)];
                    y2 = TensionMin[j,Math.Abs(zhs)];
                }
                else
                {
                    fuhao = -1;
                    y1 = TensionMin[j,Math.Abs(zhs)];
                    y2 = TensionMax[j,Math.Abs(zhs)];
                }

                x1 = Wind[j,Math.Abs(zhs)];
                z1 = GMax[j,Math.Abs(zhs)];
                z2 = GMax[j,1];

                //j从1开始计数，但是XX YY ZZ 从0开始
                //XX[i,j-1] = formula.ZXTX(angle, x1, out string strX);
                //YY[i,j-1] = formula.ZXTY(angle, x1, y1, y2, fuhao, out string strY);
                //ZZ[i,j-1] = formula.ZXTZ2(z2, z1, out string strZ);

                //ProcessString.Add(Template.Wires[j-1] + " Fx= " + strX);
                //ProcessString.Add(Template.Wires[j-1] + " Fy= " + strY);
                //ProcessString.Add(Template.Wires[j-1] + " Fz= " + strZ);

            }
            else if(Math.Abs(zhs) > 1000)
            {
                zhsAM = Math.Abs(zhs) % 1000;

                //脱冰跳跃相

                if (zhs > 0)
                {
                    fuhao = 1;
                    y1 = TensionMax[j,zhsAM];
                    y2 = TensionMin[j,zhsAM];
                }
                else
                {
                    fuhao = -1;
                    y1 = TensionMin[j,zhsAM];
                    y2 = TensionMax[j,zhsAM];
                }

                if (zhsAM > mz1 || fuhao > 1)
                {
                    throw new Exception("第　" + i + "　工况，第 " + j + " 线条组合参数错误，只能为-" + -mz1 + "～" + mz1 + "之间 " + "0 + 16 " + "错误：1-209");
                }

                x1 = Wind[j,zhsAM];
                z1 = GMin[j,zhsAM];
                z2 = GMin[j,1];

                //j从1开始计数，但是XX YY ZZ 从0开始
                //XX[i,j-1] = formula.ZXTX(angle, x1, out string strX);
                //YY[i,j-1] = formula.ZXTY(angle, x1, y1, y2, fuhao, out string strY);
                //ZZ[i,j-1] = formula.ZXTZ1(z2, z1, out string strZ);

                //ProcessString.Add(Template.Wires[j-1] + " Fx= " + strX);
                //ProcessString.Add(Template.Wires[j-1] + " Fy= " + strY);
                //ProcessString.Add(Template.Wires[j-1] + " Fz= " + strZ);
            }
            else if(zhs == 0)
            {
                //j从1开始计数，但是XX YY ZZ 从0开始
                XX[i,j-1] = 0;
                YY[i,j-1] = 0;
                ZZ[i,j-1] = 0;


                ProcessString.Add(Template.Wires[j-1] + " Fx= " + "0.00");
                ProcessString.Add(Template.Wires[j-1] + " Fy= " + "0.00");
                ProcessString.Add(Template.Wires[j-1] + " Fz= " + "0.00");
            }
        }

        /// <summary>
        /// 针对吊装工况
        /// 工况代码"L", "La", "Lb", "Lc", "Ld", "Le", "Lf", "Lg", "Lh"
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        protected void DistributeInLift(int i, int j)
        {
            float x1, y1, y2, z1, z2;
            int fuhao;
            float zg, fhn;

            WorkConditionCombo wd = Template.WorkConditionCombos[i];

            int zhs = wd.WirdIndexCodes[j-1], zhsAM;
            int angle = wd.WindDirectionCode;
            string workConditionCode = wd.WorkConditionCode;
            int mz1 = Template.WorkConditongs.Count;

            if (Math.Abs(zhs) <= mz1 && Math.Abs(zhs) > 0)
            {
                //吊装工况统一按最大垂荷考虑，不计算最小垂荷
                if (zhs <= mz1 && zhs > 0)
                {
                    y1 = TensionMax[j,Math.Abs(zhs)];
                    y2 = TensionMin[j,Math.Abs(zhs)];
                }
                else
                {
                    y1 = TensionMin[j,Math.Abs(zhs)];
                    y2 = TensionMax[j,Math.Abs(zhs)];
                }

                x1 = Wind[j,Math.Abs(zhs)];
                z1 = GMax[j,Math.Abs(zhs)];
                z2 = GMin[j,Math.Abs(zhs)];

                //j从1开始计数，但是XX YY ZZ 从0开始
                //XX[i,j-1] = formula.ZXLX(angle, x1, out string strX);
                //YY[i,j-1] = formula.ZXLY(angle, x1, y1, y2, out string strY);
                //ZZ[i,j-1] = formula.ZXLZ1(z1, out string strZ);

                //ProcessString.Add(Template.Wires[j-1] + " Fx= " + strX);
                //ProcessString.Add(Template.Wires[j-1] + " Fy= " + strY);
                //ProcessString.Add(Template.Wires[j-1] + " Fz= " + strZ);

            }
            else if (zhs > 100 || zhs < -100)
            {
                // 包含检修提线和吊装
                if (zhs > 100 && zhs < 1000)
                {
                    fuhao = 1;
                    //此处为吊装荷载系数，检修提线时取1.0
                    zg = 1;
                    fhn = Math.Abs(zhs) / 100;
                    zhsAM = Math.Abs(zhs) % 100;
                }
                else if(zhs > -1000 && zhs < -100)
                {
                    fuhao = -1;
                    //此处为吊装荷载系数，检修提线时取1.0
                    zg = 1;
                    fhn = Math.Abs(zhs) / 100;
                    zhsAM = Math.Abs(zhs) % 100;
                }
                else if(zhs < -1000)
                {
                    fuhao = -1;
                    //此处为吊装荷载系数
                    zg = LineParas.HoistingCoef;
                    fhn = Math.Abs(zhs) / 1000;
                    zhsAM = Math.Abs(zhs) % 1000;
                }
                else
                {
                    fuhao = 1;
                    //此处为吊装荷载系数
                    zg = LineParas.HoistingCoef;
                    fhn = Math.Abs(zhs) / 1000;
                    zhsAM = Math.Abs(zhs) % 1000;
                }

                if (zhsAM > mz1)
                {
                    throw new Exception("第　" + i + "　工况，第 " + j + " 线条组合参数错误" + "0 + 16 " + "错误：1-210");
                }

                x1 = Wind[j,zhsAM];
                z1 = GMax[j,zhsAM];
                z2 = GMin[j,zhsAM];

                if(fuhao == 1)
                {
                    y1 = TensionMax[j,zhsAM];
                    y2 = TensionMin[j,zhsAM];
                }
                else
                {
                    y1 = TensionMin[j,zhsAM];
                    y2 = TensionMax[j,zhsAM];
                }

                //j从1开始计数，但是XX YY ZZ 从0开始
                //XX[i,j-1] = formula.ZXLX(angle, x1, out string strX);
                //YY[i,j-1] = formula.ZXLY(angle, x1, y1, y2, out string strY);
                //ZZ[i,j-1] = formula.ZXLZ2(z1, zg, LineParas.WireExtraLoad, fhn, out string strZ);

                //ProcessString.Add(Template.Wires[j-1] + " Fx= " + strX);
                //ProcessString.Add(Template.Wires[j-1] + " Fy= " + strY);
                //ProcessString.Add(Template.Wires[j-1] + " Fz= " + strZ);
            }
            else if (zhs == 0)
            {
                //j从1开始计数，但是XX YY ZZ 从0开始
                XX[i,j-1] = 0;
                YY[i,j-1] = 0;
                ZZ[i,j-1] = 0;

                ProcessString.Add(Template.Wires[j-1] + " Fx= " + "0.00");
                ProcessString.Add(Template.Wires[j-1] + " Fy= " + "0.00");
                ProcessString.Add(Template.Wires[j-1] + " Fz= " + "0.00");
            }
        }

        /// <summary>
        /// 针对锚线工况
        /// 工况代码"M", "Ma", "Mb", "Mc", "Md", "Me", "Mf", "Mg", "Mh"
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        protected void DistributeInAnchor(int i, int j)
        {
            float x1, y1, y2, z1, z2;
            int fuhao;
            float fhn;
            float deta1, deta2, deta3;

            WorkConditionCombo wd = Template.WorkConditionCombos[i];

            int zhs = wd.WirdIndexCodes[j-1], zhsAM;
            int angle = wd.WindDirectionCode;
            string workConditionCode = wd.WorkConditionCode;
            int mz1 = Template.WorkConditongs.Count;

            //锚线工况，统一按最大垂荷考虑
            if (Math.Abs(zhs) < 1000 && Math.Abs(zhs) > 0)
            {
                //已锚相
                if( zhs > 0)
                {
                    fuhao = 1;
                }
                else
                {
                    //不应出现
                    fuhao = -1;
                }

                fhn = Math.Abs(zhs) / 100;
                if (fhn < 1)
                    fhn = 1;

                zhsAM = Math.Abs(zhs) % 100;

                if(zhsAM > mz1)
                {
                    throw new Exception("第　" + i + "　工况，第 " + j + " 线条组合参数错误" + "0 + 16 " + "错误：1-211");
                }

                y1 = TensionMax[j,zhsAM];
                y2 = TensionMin[j,zhsAM];

                if (j <= Paras.dxl && y1 >= LineParas.AnchorTension)
                {
                    //地线有开段时
                    deta1 = y1;
                    deta2 = 90;
                    deta3 = 0;
                }
                else
                {
                    //地线不开段和导线
                    deta1 = LineParas.AnchorTension;
                    deta2 = Paras.AnchorAngle;
                    deta3 = Paras.AnchorAngle;
                }

                x1 = Wind[j,zhsAM];
                z1 = GMax[j,zhsAM];
                z2 = GMin[j,zhsAM];

                //j从1开始计数，但是XX YY ZZ 从0开始
                //XX[i,j-1] = formula.ZXMX2(angle, x1, out string strX);
                //YY[i,j-1] = formula.ZXMY2(angle, x1, deta1 * fuhao, deta2, out string strY);
                //ZZ[i,j-1] = formula.ZXMZ2(z1, LineParas.WireExtraLoad, fhn, deta1, deta3, out string strZ);

                //ProcessString.Add(Template.Wires[j-1] + " Fx= " + strX);
                //ProcessString.Add(Template.Wires[j-1] + " Fy= " + strY);
                //ProcessString.Add(Template.Wires[j-1] + " Fz= " + strZ);

            }
            else if (Math.Abs(zhs) > 1000)
            {
                //正锚
                
                if(zhs > 0)
                {
                    fuhao = 1;
                }
                else
                {
                    fuhao = -1;
                }

                fhn = Math.Abs(zhs) / 1000;
                zhsAM = Math.Abs(zhs) % 1000;

                if (zhsAM > mz1 )
                {
                    throw new Exception("第　" + i + "　工况，第 " + j + " 线条组合参数错误" +  "0 + 16+" + "错误：1-212");
                }

                x1 = Wind[j,zhsAM];
                z1 = GMax[j,zhsAM];
                z2 = GMin[j,zhsAM];

                //'正锚相
                if (fuhao > 0)
                {
                    y1 = TensionMax[j,zhsAM];
                    y2 = TensionMin[j,zhsAM];

                    if (j <= Paras.dxl && y1 >= LineParas.AnchorTension)
                    {
                        //地线有开段时
                        if (y1 > 0)
                        {
                            deta1 = y1 * LineParas.DrawingCoef;
                        }
                        else
                        {
                            deta1 = y1;
                        }
                        deta2 = 90;
                        deta3 = 0;
                    }
                    else
                    {
                        //地线不开段和导线
                        deta1 = LineParas.AnchorTension;
                        deta2 = Paras.AnchorAngle;
                        deta3 = Paras.AnchorAngle;
                    }
                }
                else
                {
                    //此种情况不应出现
                    //反向张力情况，锚线张力反向
                    y1 = -TensionMax[j,zhsAM];
                    y2 = -TensionMin[j,zhsAM];

                    if (j <= Paras.dxl && Math.Abs(y1) >= Math.Abs(LineParas.AnchorTension))
                    {
                        //地线有开段时
                        if (y1 < 0)
                        {
                            deta1 = y1 * LineParas.DrawingCoef;
                        }
                        else
                        {
                            deta1 = y1;
                        }

                        deta2 = 90;
                        deta3 = 0;
                    }
                    else
                    {
                        //地线不开段和导线
                        deta1 = -LineParas.AnchorTension;
                        deta2 = Paras.AnchorAngle;
                        deta3 = Paras.AnchorAngle;
                    }
                }

                //j从1开始计数，但是XX YY ZZ 从0开始
                //XX[i,j-1] = formula.ZXMX1(angle, x1, out string strX);
                //YY[i,j-1] = formula.ZXMY1(angle, x1, deta1, deta2, out string strY);
                //ZZ[i,j-1] = formula.ZXMZ1(z1, LineParas.WireExtraLoad , fhn, deta1, deta3, out string strZ);

                //ProcessString.Add(Template.Wires[j-1] + " Fx= " + strX);
                //ProcessString.Add(Template.Wires[j-1] + " Fy= " + strY);
                //ProcessString.Add(Template.Wires[j-1] + " Fz= " + strZ);
            }
            else if (zhs == 0)
            {
                //j从1开始计数，但是XX YY ZZ 从0开始
                XX[i,j-1] = 0;
                YY[i,j-1] = 0;
                ZZ[i,j-1] = 0;

                ProcessString.Add(Template.Wires[j-1] + " Fx= " + "0.00");
                ProcessString.Add(Template.Wires[j-1] + " Fy= " + "0.00");
                ProcessString.Add(Template.Wires[j-1] + " Fz= " + "0.00");
            }
        }






    }
}
