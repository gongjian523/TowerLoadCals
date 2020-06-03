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

        protected float[,] Wind { get; set; }

        protected float[,] GMax { get; set; }

        protected float[,] GMin { get; set; }

        protected float[,] TensionMax { get; set; }

        protected float[,] TensionMin { get; set; }

        protected float[,] WindDF { get; set; }
        protected float[,] WindXF { get; set; }
        protected float[,] WindDB { get; set; }
        protected float[,] WindXB { get; set; }

        protected float[,] WindTX { get; set; }

        protected float[,] GMaxF { get; set; }
        protected float[,] GMinF { get; set; }
        protected float[,] GMaxB { get; set; }
        protected float[,] GMinB { get; set; }
        protected float[,] GTX { get; set; }

        protected float[,] TensionD { get; set; }
        protected float[,] TensionX { get; set; }
        protected float[,] Tension45D { get; set; }
        protected float[,] Tension45X { get; set; }

        protected float [,] XLB { get; set; }
        protected float[,] YLB { get; set; }
        protected float[,] ZLB { get; set; }

        protected float[,] XLF { get; set; }
        protected float[,] YLF { get; set; }
        protected float[,] ZLF { get; set; }

        protected float[,] XTF { get; set; }
        protected float[,] YTF { get; set; }
        protected float[,] ZTF { get; set; }

        protected float[,] XTC { get; set; }
        protected float[,] YTC { get; set; }
        protected float[,] ZTC { get; set; }

        protected float[,] XTB { get; set; }
        protected float[,] YTB { get; set; }
        protected float[,] ZTB { get; set; }

        protected FormulaCornerTower formula;

        protected RatioParas Ratio { get; set; }

        public LoadDistributeCornerTower(FormulaParas para, StruLineParas[] lineParas, RatioParas ratioParas, TowerTemplate template, string tablePath) : base(para, lineParas, template, tablePath)
        {
            formula = new FormulaCornerTower(para);
            Ratio = ratioParas;
        }

        protected override void GetTable(DataSet ds)
        {
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
                foreach (int wic in wd.WireIndexCodes)
                {
                    LineParas = LineParasArr[j - 1];
                    LineParas.TemporaryTension = LineParas.TemporaryTension / Paras.LoadRatio;
                    LineParas.AngleMax = LineParas.AngleMax / 2;
                    LineParas.AngleMin = LineParas.AngleMin / 2;
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
                            DistributeInJ(i, j);
                            break;
                        case "MQ1":
                        case "MQ2":
                            DistributeInMQ(i, j);
                            break;
                        case "G1":
                        case "G2":
                            DistributeInG(i, j);
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
            float Vcb;

            WorkConditionCombo wd = Template.WorkConditionCombos[i];
            int angle = wd.WindDirectionCode;
            int zhs = wd.WireIndexCodes[j - 1];
            int BL3 = LineParas.isTurnRight ? 1 : -1;
            //跳线数 = 导线数
            int nt = Template.Wires.Where(item => item.Contains("导")).Count();

            if (Math.Abs(zhs) > 0)
            {
                x11 = WindDF[j, Math.Abs(zhs)];
                x12 = WindXF[j, Math.Abs(zhs)];
                x21 = WindDB[j, Math.Abs(zhs)];
                x22 = WindXB[j, Math.Abs(zhs)];
                x3 = WindTX[j, Math.Abs(zhs)];
                z11 = GMaxF[j, Math.Abs(zhs)];
                z12 = GMinF[j, Math.Abs(zhs)];
                z21 = GMaxB[j, Math.Abs(zhs)];
                z22 = GMinB[j, Math.Abs(zhs)];
                z3 = GTX[j, Math.Abs(zhs)];

                if (wd.WorkConditionCode == "N1" || wd.WorkConditionCode == "D1")
                {
                    e1 = x11;
                    e2 = x21;
                    e3 = LineParas.AngleMax;
                    e4 = 2f;
                }
                else
                {
                    e1 = x12;
                    e2 = x22;
                    e3 = LineParas.AngleMin;
                    e4 = 1f;
                }

                if (wd.WorkConditionCode == "N1" || wd.WorkConditionCode == "D1")
                {
                    Vcb = Paras.VcFNormal;
                }
                else
                {
                    Vcb = Paras.VcFCold;
                }


                if (angle == 45)
                {
                    y1 = Tension45D[j, (int)e4];
                    y2 = Tension45X[j, (int)e4];
                }
                else
                {
                    y1 = TensionD[j, Math.Abs(zhs)];
                    y2 = TensionX[j, Math.Abs(zhs)];
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
                        y4 = LineParas.PortalTensionMax;
                    }

                    switch (wd.VertialLoadCode)
                    {
                        case "YY":
                            c[1] = e1;
                            c[2] = y3;
                            c[3] = z11;
                            c[4] = e2;
                            c[5] = y4;
                            c[6] = z21;
                            c[7] = x3;
                            c[8] = z3;
                            break;

                        case "YB":
                            c[1] = e1;
                            c[2] = y3;
                            c[3] = z11;
                            c[4] = e2;
                            c[5] = y4;
                            c[6] = z22;
                            c[7] = x3;
                            c[8] = z3;
                            break;

                        case "0B":
                            c[1] = e1;
                            c[2] = y3;
                            c[3] = 0;
                            c[4] = e2;
                            c[5] = y4;
                            c[6] = z22;
                            c[7] = x3;
                            c[8] = z3;
                            break;

                        case "B0":
                            c[1] = e1;
                            c[2] = y3;
                            c[3] = z12;
                            c[4] = e2;
                            c[5] = y4;
                            c[6] = 0;
                            c[7] = x3;
                            c[8] = z3;
                            break;

                        case "Y0":
                            c[1] = e1;
                            c[2] = y3;
                            c[3] = z11;
                            c[4] = e2;
                            c[5] = y4;
                            c[6] = 0;
                            c[7] = x3;
                            c[8] = z3;
                            break;

                        case "BB":
                            c[1] = e1;
                            c[2] = y3;
                            c[3] = z12;
                            c[4] = e2;
                            c[5] = y4;
                            c[6] = z22;
                            c[7] = x3;
                            c[8] = z3;
                            break;

                        case "BY":
                            c[1] = e1;
                            c[2] = y3;
                            c[3] = z12;
                            c[4] = e2;
                            c[5] = y4;
                            c[6] = z21;
                            c[7] = x3;
                            c[8] = z3;
                            break;

                        case "0Y":
                            c[1] = e1;
                            c[2] = y3;
                            c[3] = 0;
                            c[4] = e2;
                            c[5] = y4;
                            c[6] = z21;
                            c[7] = x3;
                            c[8] = z3;
                            break;
                        //case "00":
                        //    c[1] = e1;
                        //    c[2] = y3;
                        //    c[3] = 0;
                        //    c[4] = e2;
                        //    c[5] = y4;
                        //    c[6] = 0;
                        //    c[7] = x3;
                        //    c[8] = z3;
                        //    break;
                        //2016.9.24 针对前后侧同时取某一侧的拔力新增,T表示某侧取相反侧上拔力计算

                        case "YT":
                            c[1] = e1;
                            c[2] = y3;
                            c[3] = z11;
                            c[4] = e2;
                            c[5] = y4;
                            c[6] = z12;
                            c[7] = x3;
                            c[8] = z3;
                            break;

                        case "0T":
                            c[1] = e1;
                            c[2] = y3;
                            c[3] = 0;
                            c[4] = e2;
                            c[5] = y4;
                            c[6] = z12;
                            c[7] = x3;
                            c[8] = z3;
                            break;

                        case "T0":
                            c[1] = e1;
                            c[2] = y3;
                            c[3] = z22;
                            c[4] = e2;
                            c[5] = y4;
                            c[6] = 0;
                            c[7] = x3;
                            c[8] = z3;
                            break;

                        case "TY":
                            c[1] = e1;
                            c[2] = y3;
                            c[3] = z22;
                            c[4] = e2;
                            c[5] = y4;
                            c[6] = z21;
                            c[7] = x3;
                            c[8] = z3;
                            break;

                        //20190113新版程序
                        default:
                            c[1] = e1;
                            c[2] = y3;
                            c[4] = e2;
                            c[5] = y4;
                            c[7] = x3;
                            c[8] = z3;
                            if (wd.VertialLoadCode.Substring(0, 1) == "0")
                                c[3] = 0;
                            else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                c[3] = z11;
                            else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                c[3] = z12;
                            else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                c[3] = z21;
                            else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                c[3] = z22;
                            else
                                throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-1");

                            if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                c[6] = 0;
                            else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                c[6] = z11;
                            else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                c[6] = z12;
                            else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                c[6] = z21;
                            else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                c[6] = z22;
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
                            y4 = LineParas.PortalTensionMin;
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
                            y4 = LineParas.PortalTensionMin;
                        }
                    }

                    switch (wd.VertialLoadCode)
                    {
                        case "YY":
                            c[1] = e1;
                            c[2] = y3;
                            c[3] = z11;
                            c[4] = e2;
                            c[5] = y4;
                            c[6] = z21;
                            c[7] = x3;
                            c[8] = z3;
                            break;
                        case "YB":
                            c[1] = e1;
                            c[2] = y3;
                            c[3] = z11;
                            c[4] = e2;
                            c[5] = y4;
                            c[6] = z22;
                            c[7] = x3;
                            c[8] = z3;
                            break;
                        case "0B":
                            c[1] = e1;
                            c[2] = y3;
                            c[3] = 0;
                            c[4] = e2;
                            c[5] = y4;
                            c[6] = z22;
                            c[7] = x3;
                            c[8] = z3;
                            break;
                        case "B0":
                            c[1] = e1;
                            c[2] = y3;
                            c[3] = z12;
                            c[4] = e2;
                            c[5] = y4;
                            c[6] = 0;
                            c[7] = x3;
                            c[8] = z3;
                            break;
                        case "Y0":
                            c[1] = e1;
                            c[2] = y3;
                            c[3] = z11;
                            c[4] = e2;
                            c[5] = y4;
                            c[6] = 0;
                            c[7] = x3;
                            c[8] = z3;
                            break;
                        case "BB":
                            c[1] = e1;
                            c[2] = y3;
                            c[3] = z12;
                            c[4] = e2;
                            c[5] = y4;
                            c[6] = z22;
                            c[7] = x3;
                            c[8] = z3;
                            break;
                        case "BY":
                            c[1] = e1;
                            c[2] = y3;
                            c[3] = z12;
                            c[4] = e2;
                            c[5] = y4;
                            c[6] = z21;
                            c[7] = x3;
                            c[8] = z3;
                            break;
                        case "0Y":
                            c[1] = e1;
                            c[2] = y3;
                            c[3] = 0;
                            c[4] = e2;
                            c[5] = y4;
                            c[6] = z21;
                            c[7] = x3;
                            c[8] = z3;
                            break;

                        //case "00":                            
                        //    c[1] = e1;
                        //    c[2] = y3;
                        //    c[3] = 0;
                        //    c[4] = e2;
                        //    c[5] = y4;
                        //    c[6] = 0;
                        //    c[7] = x3;
                        //    c[8] = z3;
                        //    break;
                        //2016.9.24 针对前后侧同时取某一侧的拔力新增,T表示某侧取相反侧上拔力计算
                        case "YT":
                            c[1] = e1;
                            c[2] = y3;
                            c[3] = z11;
                            c[4] = e2;
                            c[5] = y4;
                            c[6] = z12;
                            c[7] = x3;
                            c[8] = z3;
                            break;
                        case "TY":
                            c[1] = e1;
                            c[2] = y3;
                            c[3] = z22;
                            c[4] = e2;
                            c[5] = y4;
                            c[6] = z21;
                            c[7] = x3;
                            c[8] = z3;
                            break;
                        case "0T":
                            c[1] = e1;
                            c[2] = y3;
                            c[3] = 0;
                            c[4] = e2;
                            c[5] = y4;
                            c[6] = z12;
                            c[7] = x3;
                            c[8] = z3;
                            break;
                        case "T0":
                            c[1] = e1;
                            c[2] = y3;
                            c[3] = z22;
                            c[4] = e2;
                            c[5] = y4;
                            c[6] = 0;
                            c[7] = x3;
                            c[8] = z3;
                            break;
                        //20190113新版程序
                        default:
                            c[1] = e1;
                            c[2] = y3;
                            c[4] = e2;
                            c[5] = y4;
                            c[7] = x3;
                            c[8] = z3;
                            if (wd.VertialLoadCode.Substring(0, 1) == "0")
                                c[3] = 0;
                            else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                c[3] = z11;
                            else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                c[3] = z12;
                            else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                c[3] = z21;
                            else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                c[3] = z22;
                            else
                                throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-3");

                            if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                c[6] = 0;
                            else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                c[6] = z11;
                            else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                c[6] = z12;
                            else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                c[6] = z21;
                            else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                c[6] = z22;
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
                            y4 = LineParas.PortalTensionMin;
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
                            y4 = LineParas.PortalTensionMin;
                        }
                    }

                    switch (wd.VertialLoadCode)
                    {
                        case "YY":
                            c[1] = e1;
                            c[2] = y3;
                            c[3] = z11;
                            c[4] = e2;
                            c[5] = y4;
                            c[6] = z21;
                            c[7] = x3;
                            c[8] = z3;
                            break;
                        case "YB":
                            c[1] = e1;
                            c[2] = y3;
                            c[3] = z11;
                            c[4] = e2;
                            c[5] = y4;
                            c[6] = z22;
                            c[7] = x3;
                            c[8] = z3;
                            break;
                        case "0B":
                            c[1] = e1;
                            c[2] = y3;
                            c[3] = 0;
                            c[4] = e2;
                            c[5] = y4;
                            c[6] = z22;
                            c[7] = x3;
                            c[8] = z3;
                            break;
                        case "B0":
                            c[1] = e1;
                            c[2] = y3;
                            c[3] = z12;
                            c[4] = e2;
                            c[5] = y4;
                            c[6] = 0;
                            c[7] = x3;
                            c[8] = z3;
                            break;
                        case "Y0":
                            c[1] = e1;
                            c[2] = y3;
                            c[3] = z11;
                            c[4] = e2;
                            c[5] = y4;
                            c[6] = 0;
                            c[7] = x3;
                            c[8] = z3;
                            break;
                        case "BB":
                            c[1] = e1;
                            c[2] = y3;
                            c[3] = z12;
                            c[4] = e2;
                            c[5] = y4;
                            c[6] = z22;
                            c[7] = x3;
                            c[8] = z3;
                            break;
                        case "BY":
                            c[1] = e1;
                            c[2] = y3;
                            c[3] = z12;
                            c[4] = e2;
                            c[5] = y4;
                            c[6] = z21;
                            c[7] = x3;
                            c[8] = z3;
                            break;
                        case "0Y":
                            c[1] = e1;
                            c[2] = y3;
                            c[3] = 0;
                            c[4] = e2;
                            c[5] = y4;
                            c[6] = z21;
                            c[7] = x3;
                            c[8] = z3;
                            break;

                        //case "00":
                        //    c[1] = e1;
                        //    c[2] = y3;
                        //    c[3] = 0;
                        //    c[4] = e2;
                        //    c[5] = y4;
                        //    c[6] = 0;
                        //    c[7] = x3;
                        //    c[8] = z3;
                        //    break;
                        //2016.9.24 针对前后侧同时取某一侧的拔力新增,T表示某侧取相反侧上拔力计算
                        case "YT":
                            c[1] = e1;
                            c[2] = y3;
                            c[3] = z11;
                            c[4] = e2;
                            c[5] = y4;
                            c[6] = z12;
                            c[7] = x3;
                            c[8] = z3;
                            break;
                        case "TY":
                            c[1] = e1;
                            c[2] = y3;
                            c[3] = z22;
                            c[4] = e2;
                            c[5] = y4;
                            c[6] = z21;
                            c[7] = x3;
                            c[8] = z3;
                            break;
                        case "0T":
                            c[1] = e1;
                            c[2] = y3;
                            c[3] = 0;
                            c[4] = e2;
                            c[5] = y4;
                            c[6] = z12;
                            c[7] = x3;
                            c[8] = z3;
                            break;
                        case "T0":
                            c[1] = e1;
                            c[2] = y3;
                            c[3] = z22;
                            c[4] = e2;
                            c[5] = y4;
                            c[6] = 0;
                            c[7] = x3;
                            c[8] = z3;
                            break;
                        //20190113新版程序
                        default:
                            c[1] = e1;
                            c[2] = y3;
                            c[4] = e2;
                            c[5] = y4;
                            c[7] = x3;
                            c[8] = z3;
                            if (wd.VertialLoadCode.Substring(0, 1) == "0")
                                c[3] = 0;
                            else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                c[3] = z11;
                            else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                c[3] = z12;
                            else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                c[3] = z21;
                            else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                c[3] = z22;
                            else
                                throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-5");

                            if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                c[6] = 0;
                            else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                c[6] = z11;
                            else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                c[6] = z12;
                            else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                c[6] = z21;
                            else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                c[6] = z22;
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
                        y4 = LineParas.PortalTensionMin;
                    }


                    switch (wd.VertialLoadCode)
                    {
                        case "YY":
                            c[1] = e1;
                            c[2] = y3;
                            c[3] = z11;
                            c[4] = e2;
                            c[5] = y4;
                            c[6] = z21;
                            c[7] = x3;
                            c[8] = z3;
                            break;
                        case "YB":
                            c[1] = e1;
                            c[2] = y3;
                            c[3] = z11;
                            c[4] = e2;
                            c[5] = y4;
                            c[6] = z22;
                            c[7] = x3;
                            c[8] = z3;
                            break;
                        case "0B":
                            c[1] = e1;
                            c[2] = y3;
                            c[3] = 0;
                            c[4] = e2;
                            c[5] = y4;
                            c[6] = z22;
                            c[7] = x3;
                            c[8] = z3;
                            break;
                        case "B0":
                            c[1] = e1;
                            c[2] = y3;
                            c[3] = z12;
                            c[4] = e2;
                            c[5] = y4;
                            c[6] = 0;
                            c[7] = x3;
                            c[8] = z3;
                            break;
                        case "Y0":
                            c[1] = e1;
                            c[2] = y3;
                            c[3] = z11;
                            c[4] = e2;
                            c[5] = y4;
                            c[6] = 0;
                            c[7] = x3;
                            c[8] = z3;
                            break;
                        case "BB":
                            c[1] = e1;
                            c[2] = y3;
                            c[3] = z12;
                            c[4] = e2;
                            c[5] = y4;
                            c[6] = z22;
                            c[7] = x3;
                            c[8] = z3;
                            break;
                        case "BY":
                            c[1] = e1;
                            c[2] = y3;
                            c[3] = z12;
                            c[4] = e2;
                            c[5] = y4;
                            c[6] = z21;
                            c[7] = x3;
                            c[8] = z3;
                            break;
                        case "0Y":
                            c[1] = e1;
                            c[2] = y3;
                            c[3] = 0;
                            c[4] = e2;
                            c[5] = y4;
                            c[6] = z21;
                            c[7] = x3;
                            c[8] = z3;
                            break;

                        //case "00":
                        //    c[1] = e1;
                        //    c[2] = y3;
                        //    c[3] = 0;
                        //    c[4] = e2;
                        //    c[5] = y4;
                        //    c[6] = 0;
                        //    c[7] = x3;
                        //    c[8] = z3;
                        //    break;
                        //2016.9.24 针对前后侧同时取某一侧的拔力新增,T表示某侧取相反侧上拔力计算
                        case "YT":
                            c[1] = e1;
                            c[2] = y3;
                            c[3] = z11;
                            c[4] = e2;
                            c[5] = y4;
                            c[6] = z12;
                            c[7] = x3;
                            c[8] = z3;
                            break;

                        case "0T":
                            c[1] = e1;
                            c[2] = y3;
                            c[3] = 0;
                            c[4] = e2;
                            c[5] = y4;
                            c[6] = z12;
                            c[7] = x3;
                            c[8] = z3;
                            break;
                        case "T0":
                            c[1] = e1;
                            c[2] = y3;
                            c[3] = z22;
                            c[4] = e2;
                            c[5] = y4;
                            c[6] = 0;
                            c[7] = x3;
                            c[8] = z3;
                            break;
                        case "TY":
                            c[1] = e1;
                            c[2] = y3;
                            c[3] = z22;
                            c[4] = e2;
                            c[5] = y4;
                            c[6] = z21;
                            c[7] = x3;
                            c[8] = z3;
                            break;
                        //20190113新版程序
                        default:
                            c[1] = e1;
                            c[2] = y3;
                            c[4] = e2;
                            c[5] = y4;
                            c[7] = x3;
                            c[8] = z3;
                            if (wd.VertialLoadCode.Substring(0, 1) == "0")
                                c[3] = 0;
                            else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                c[3] = z11;
                            else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                c[3] = z12;
                            else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                c[3] = z21;
                            else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                c[3] = z22;
                            else
                                throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-7");

                            if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                c[6] = 0;
                            else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                c[6] = z11;
                            else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                c[6] = z12;
                            else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                c[6] = z21;
                            else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                c[6] = z22;
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
                            c[1] = e1;
                            c[2] = y3;
                            c[3] = z12;
                            c[4] = 0;
                            c[5] = y4;
                            c[6] = 0;
                            c[7] = 0;
                            c[8] = 0;
                            break;
                        case "Y0":
                            c[1] = e1;
                            c[2] = y3;
                            c[3] = z11;
                            c[4] = 0;
                            c[5] = y4;
                            c[6] = 0;
                            c[7] = 0;
                            c[8] = 0;
                            break;
                        case "T0":
                            c[1] = e1;
                            c[2] = y3;
                            c[3] = z22;
                            c[4] = 0;
                            c[5] = y4;
                            c[6] = 0;
                            c[7] = 0;
                            c[8] = 0;
                            break;
                        //20190113新版程序
                        default:
                            c[1] = e1;
                            c[2] = y3;
                            c[4] = 0;
                            c[5] = 0;
                            c[7] = 0;
                            c[8] = 0;
                            if (wd.VertialLoadCode.Substring(0, 1) == "0")
                                c[3] = 0;
                            else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                c[3] = z11;
                            else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                c[3] = z12;
                            else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                c[3] = z21;
                            else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                c[3] = z22;
                            else
                                throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-9");

                            if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                c[6] = 0;
                            else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                c[6] = z11;
                            else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                c[6] = z12;
                            else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                c[6] = z21;
                            else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                c[6] = z22;
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
                            c[1] = e1;
                            c[2] = y3;
                            c[3] = z12;
                            c[4] = 0;
                            c[5] = y4;
                            c[6] = 0;
                            c[7] = 0;
                            c[8] = 0;
                            break;
                        case "Y0":
                            c[1] = e1;
                            c[2] = y3;
                            c[3] = z11;
                            c[4] = 0;
                            c[5] = y4;
                            c[6] = 0;
                            c[7] = 0;
                            c[8] = 0;
                            break;
                        case "T0":
                            c[1] = e1;
                            c[2] = y3;
                            c[3] = z22;
                            c[4] = 0;
                            c[5] = y4;
                            c[6] = 0;
                            c[7] = 0;
                            c[8] = 0;
                            break;
                        //20190113新版程序
                        default:
                            c[1] = e1;
                            c[2] = y3;
                            c[4] = 0;
                            c[5] = 0;
                            c[7] = 0;
                            c[8] = 0;
                            if (wd.VertialLoadCode.Substring(0, 1) == "0")
                                c[3] = 0;
                            else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                c[3] = z11;
                            else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                c[3] = z12;
                            else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                c[3] = z21;
                            else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                c[3] = z22;
                            else
                                throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-11");

                            if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                c[6] = 0;
                            else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                c[6] = z11;
                            else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                c[6] = z12;
                            else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                c[6] = z21;
                            else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                c[6] = z22;
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
                        y4 = LineParas.PortalTensionMax;
                    }

                    switch (wd.VertialLoadCode)
                    {
                        case "0B":
                            c[1] = 0;
                            c[2] = y3;
                            c[3] = 0;
                            c[4] = e2;
                            c[5] = y4;
                            c[6] = z22;
                            c[7] = 0;
                            c[8] = 0;
                            break;
                        case "0Y":
                            c[1] = 0;
                            c[2] = y3;
                            c[3] = 0;
                            c[4] = e2;
                            c[5] = y4;
                            c[6] = z21;
                            c[7] = 0;
                            c[8] = 0;
                            break;
                        case "0T":
                            c[1] = 0;
                            c[2] = y3;
                            c[3] = 0;
                            c[4] = e2;
                            c[5] = y4;
                            c[6] = z12;
                            c[7] = 0;
                            c[8] = 0;
                            break;
                        //20190113新版程序
                        default:
                            c[1] = 0;
                            c[2] = 0;
                            c[4] = e2;
                            c[5] = y4;
                            c[7] = 0;
                            c[8] = 0;
                            if (wd.VertialLoadCode.Substring(0, 1) == "0")
                                c[3] = 0;
                            else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                c[3] = z11;
                            else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                c[3] = z12;
                            else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                c[3] = z21;
                            else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                c[3] = z22;
                            else
                                throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-13");

                            if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                c[6] = 0;
                            else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                c[6] = z11;
                            else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                c[6] = z12;
                            else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                c[6] = z21;
                            else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                c[6] = z22;
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
                        y4 = LineParas.PortalTensionMin;
                    }

                    switch (wd.VertialLoadCode)
                    {
                        case "0B":
                            c[1] = 0;
                            c[2] = y3;
                            c[3] = 0;
                            c[4] = e2;
                            c[5] = y4;
                            c[6] = z22;
                            c[7] = 0;
                            c[8] = 0;
                            break;
                        case "0Y":
                            c[1] = 0;
                            c[2] = y3;
                            c[3] = 0;
                            c[4] = e2;
                            c[5] = y4;
                            c[6] = z21;
                            c[7] = 0;
                            c[8] = 0;
                            break;
                        case "0T":
                            c[1] = 0;
                            c[2] = y3;
                            c[3] = 0;
                            c[4] = e2;
                            c[5] = y4;
                            c[6] = z12;
                            c[7] = 0;
                            c[8] = 0;
                            break;
                        //20190113新版程序
                        default:
                            c[1] = 0;
                            c[2] = 0;
                            c[4] = e2;
                            c[5] = y4;
                            c[7] = 0;
                            c[8] = 0;
                            if (wd.VertialLoadCode.Substring(0, 1) == "0")
                                c[3] = 0;
                            else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                c[3] = z11;
                            else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                c[3] = z12;
                            else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                c[3] = z21;
                            else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                c[3] = z22;
                            else
                                throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-11");

                            if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                c[6] = 0;
                            else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                c[6] = z11;
                            else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                c[6] = z12;
                            else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                c[6] = z21;
                            else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                c[6] = z22;
                            else
                                throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-12");
                            break;
                    }

                }

                if (Paras.IsBranchTower)
                {
                    //前侧转角  20161124分支塔添加
                    e3 = LineParas.AngleFront;
                }

                XLF[i, j] = formula.ZNX(out string strXLF, angle, e3, c[1], c[2], Vcb, BL3);
                YLF[i, j] = formula.ZNY(out string strYLF, angle, e3, c[1], c[2], Vcb);
                ZLF[i, j] = formula.ZNZ(out string strZLF, c[3], Vcb);

                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fx= " + strXLF);
                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fy= " + strYLF);
                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fz= " + strZLF);

                if (Paras.IsBranchTower)
                {
                    //前侧转角  20161124分支塔添加
                    e3 = LineParas.AngleBack;
                }

                XLB[i, j] = formula.ZNX(out string strXLB, angle, e3, c[4], c[5], Vcb, BL3);
                YLB[i, j] = formula.ZNY(out string strYLB, angle, e3, c[4], -c[5], Vcb);
                ZLB[i, j] = formula.ZNZ(out string strZLB, c[6], Vcb);

                ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fx= " + strXLB);
                ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fy= " + strYLB);
                ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fz= " + strZLB);

                if (nt % 3 != 0 || nt == 0)
                {
                    //0,1,2个跳线的时候 前后
                    //跳线串始终为一种计算方法
                    XTF[i, j] = formula.ZNX(out string strXTF, angle, 0, c[7], 0, Vcb, BL3, nt, Ratio.BLTQ);
                    YTF[i, j] = formula.ZNY(out string strYTF, angle, 0, c[7], 0, Vcb, nt, Ratio.BLTQ);
                    ZTF[i, j] = formula.ZNZ(out string strZTF, c[8], Vcb, nt, Ratio.BLTQ);

                    ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fx= " + strXTF);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fy= " + strYTF);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fz= " + strZTF);

                    XTB[i, j] = formula.ZNX(out string strXTB, angle, 0, c[7], 0, Vcb, BL3, nt, Ratio.BLTH);
                    YTB[i, j] = formula.ZNY(out string strYTB, angle, 0, c[7], 0, Vcb, nt, Ratio.BLTH);
                    ZTB[i, j] = formula.ZNZ(out string strZTB, c[8], Vcb, nt, Ratio.BLTH);

                    ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= " + strXTB);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= " + strYTB);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= " + strZTB);
                }
                else if (nt % 3 == 0 && nt > 0)
                {
                    XTF[i, j] = formula.ZNX(out string strXTF, angle, 0, c[7], 0, Vcb, BL3 , nt,  Ratio.BLTQ);
                    YTF[i, j] = formula.ZNY(out string strYTF, angle, 0, c[7], 0, Vcb, nt, Ratio.BLTQ);
                    ZTF[i, j] = formula.ZNZ(out string strZTF, c[8], Vcb, nt, Ratio.BLTQ);

                    ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fx= " + strXTF);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fy= " + strYTF);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fz= " + strZTF);

                    XTC[i, j] = formula.ZNX(out string strXTC, angle, 0, c[7], 0, Vcb, BL3, nt, Ratio.BLTZ);
                    YTC[i, j] = formula.ZNY(out string strYTC, angle, 0, c[7], 0, Vcb, nt, Ratio.BLTZ);
                    ZTC[i, j] = formula.ZNZ(out string strZTC, c[8], Vcb, nt, Ratio.BLTZ);

                    ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fx= " + strXTC);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fy= " + strYTC);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fz= " + strZTC);

                    XTB[i, j] = formula.ZNX(out string strXTB, angle, 0, c[7], 0, Vcb, BL3, nt, Ratio.BLTH);
                    YTB[i, j] = formula.ZNY(out string strYTB, angle, 0, c[7], 0, Vcb, nt, Ratio.BLTH);
                    ZTB[i, j] = formula.ZNZ(out string strZTB, c[8], Vcb, nt, Ratio.BLTH);

                    ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= " + strXTB);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= " + strYTB);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= " + strZTB);
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
                c[1] = e1;
                c[2] = y1;
                c[3] = z11;
                c[4] = e2;
                c[5] = y2;
                c[6] = z21;
                c[7] = x3;
                c[8] = z3;
                e3 = 0; //自行赋值，源代码没有
                Vcb = 0; //自行赋值，源代码没有

                XLF[i, j] = formula.ZNX(out string strXLF, angle, e3, c[1], c[2], Vcb, BL3);
                YLF[i, j] = formula.ZNY(out string strYLF, angle, e3, c[1], c[2], Vcb);
                ZLF[i, j] = formula.ZNZ(out string strZLF, c[3], Vcb);

                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fx= " + strXLF);
                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fy= " + strYLF);
                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fz= " + strZLF);

                XLB[i, j] = formula.ZNX(out string strXLB, angle, e3, c[4], c[5], Vcb, BL3);
                YLB[i, j] = formula.ZNY(out string strYLB, angle, e3, c[4], -c[5], Vcb);
                ZLB[i, j] = formula.ZNZ(out string strZLB, c[6], Vcb);

                ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fx= " + strXLB);
                ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fy= " + strYLB);
                ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fz= " + strZLB);

                if( nt % 3 != 0 | nt == 0)
                {
                    //跳线串始终为一种计算方法
                    XTF[i, j] = formula.ZNX(out string strXTF, angle, 0, c[7], 0, Vcb, BL3, nt, Ratio.BLTQ);
                    YTF[i, j] = formula.ZNY(out string strYTF, angle, 0, c[7], 0, Vcb, nt, Ratio.BLTQ);
                    ZTF[i, j] = formula.ZNZ(out string strZTF, c[8], Vcb, nt, Ratio.BLTQ);

                    ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fx= " + strXTF);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fy= " + strYTF);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fz= " + strZTF);

                    XTB[i, j] = formula.ZNX(out string strXTB, angle, 0, c[7], 0, Vcb, BL3, nt, Ratio.BLTH);
                    YTB[i, j] = formula.ZNY(out string strYTB, angle, 0, c[7], 0, Vcb, nt, Ratio.BLTH);
                    ZTB[i, j] = formula.ZNZ(out string strZTB, c[8], Vcb, nt, Ratio.BLTH);
                                            
                    ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= " + strXTB);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= " + strYTB);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= " + strZTB);
                }
                else if( nt % 3 == 0 && nt > 0 )
                {
                    XTF[i, j] = formula.ZNX(out string strXTF, angle, 0, c[7], 0, Vcb, BL3, nt, 0.33334f);
                    YTF[i, j] = formula.ZNY(out string strYTF, angle, 0, c[7], 0, Vcb, nt, 0.33334f);
                    ZTF[i, j] = formula.ZNZ(out string strZTF, c[8], Vcb, nt, 0.33334f);

                    ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fx= " + strYTF);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fy= " + strYLF);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fz= " + strZTF);

                    XTC[i, j] = formula.ZNX(out string strXTC, angle, 0, c[7], 0, Vcb, BL3, nt, 0.33334f);
                    YTC[i, j] = formula.ZNY(out string strYTC, angle, 0, c[7], 0, Vcb, nt, 0.33334f);
                    ZTC[i, j] = formula.ZNZ(out string strZTC, c[8], Vcb, nt, 0.33334f);

                    ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fx= " + strXTC);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fy= " + strYTC);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fz= " + strZTC);

                    XTB[i, j] = formula.ZNX(out string strXTB, angle, 0, c[7], 0, Vcb, BL3, nt, 0.33334f);
                    YTB[i, j] = formula.ZNY(out string strYTB, angle, 0, c[7], 0, Vcb, nt, 0.33334f);
                    ZTB[i, j] = formula.ZNZ(out string strZTB, c[8], Vcb, nt, 0.33334f);

                    ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= " + strXTB);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= " + strYTB);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= " + strZTB);
                }
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
            int BL2;
            float rg, Vloadx;

            WorkConditionCombo wd = Template.WorkConditionCombos[i];
            int angle = wd.WindDirectionCode;
            int zhs = wd.WireIndexCodes[j - 1];
            int BL3 = LineParas.isTurnRight ? 1 : -1;
            //跳线数 = 导线数
            int nt = Template.Wires.Where(item => item.Contains("导")).Count();
            int zhsx = Math.Abs(zhs) % 100;

            if (Math.Abs(zhs) > 0)
            {
                BL2 = 1;
                y1 = TensionD[j, zhsx] * BL2;
                y2 = TensionX[j, zhsx] * BL2;
                x11 = WindDF[j, zhsx] * BL2;
                x12 = WindXF[j, zhsx] * BL2;
                x21 = WindDB[j, zhsx] * BL2;
                x22 = WindXB[j, zhsx] * BL2;
                x3 = WindTX[j, zhsx] * BL2;
                z11 = GMaxF[j, zhsx] * BL2;
                z12 = GMinF[j, zhsx] * BL2;
                z21 = GMaxB[j, zhsx] * BL2;
                z22 = GMinB[j, zhsx] * BL2;
                z3 = GTX[j, zhsx] * BL2;
                z011 = GMaxF[j, 1] * BL2;
                z012 = GMinF[j, 1] * BL2;
                z021 = GMaxB[j, 1] * BL2;
                z022 = GMinB[j, 1] * BL2;
                z03 = GTX[j, 1] * BL2;

                if (wd.WorkConditionCode == "I1")
                {
                    e1 = x11; e2 = x21; e3 = LineParas.AngleMax; e4 = 2; e5 = Paras.VcFNormal; Vloadx = Paras.RQ; rg = Paras.RGBad;
                }
                else if (wd.WorkConditionCode == "I2")
                {
                    e1 = x12; e2 = x22; e3 = LineParas.AngleMin; e4 = 1; e5 = Paras.VcFNormal; Vloadx = Paras.RQ; rg = Paras.RGBad;
                }
                else if (wd.WorkConditionCode == "U1")
                {
                    e1 = x11; e2 = x21; e3 = LineParas.AngleMax; e4 = 2; e5 = Paras.VcFUnevenIce; Vloadx = Paras.RQ; rg = Paras.RGBad;
                }
                else if (wd.WorkConditionCode == "U2")
                {
                    e1 = x12; e2 = x22; e3 = LineParas.AngleMin; e4 = 1; e5 = Paras.VcFUnevenIce; Vloadx = Paras.RQ; rg = Paras.RGBad;
                }
                else if (wd.WorkConditionCode == "B1")
                {
                    e1 = x11; e2 = x21; e3 = LineParas.AngleMax; e4 = 2; e5 = Paras.VcFBroken; Vloadx = Paras.RQ; rg = Paras.RGBad;
                }
                else if (wd.WorkConditionCode == "B2")
                {
                    e1 = x12; e2 = x22; e3 = LineParas.AngleMin; e4 = 1; e5 = Paras.VcFBroken; Vloadx = Paras.RQ; rg = Paras.RGBad;
                }
                else if (wd.WorkConditionCode == "Y1")
                {
                    e1 = x11; e2 = x21; e3 = LineParas.AngleMax; e4 = 2; e5 = Paras.VcFCheck; Vloadx = Paras.RA; rg = Paras.RGCheck01;
                }
                else
                {
                    e1 = x12; e2 = x22; e3 = LineParas.AngleMin; e4 = 1; e5 = Paras.VcFCheck; Vloadx = Paras.RA; rg = Paras.RGCheck01;
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
                        y4 = LineParas.PortalTensionMax;
                    }

                    if (Paras.IsMethod1Selected && zhs < 0)
                    {
                        //按最严重情况考虑扭转 20170620新增  扭转相垂荷按最严重情况考虑
                        switch (wd.VertialLoadCode)
                        {
                            case "YY":
                                c[1] = e1; c[2] = y3; c[3] = z21; c[4] = e2; c[5] = y4; c[6] = z11; c[7] = x3; c[8] = z3; c[9] = z021; c[10] = z011; c[11] = z03;
                                break;
                            case "YB":
                                c[1] = e1; c[2] = y3; c[3] = z21; c[4] = e2; c[5] = y4; c[6] = z12; c[7] = x3; c[8] = z3; c[9] = z021; c[10] = z012; c[11] = z03;//根据酒湖线修改
                                break;
                            case "BB":
                                c[1] = e1; c[2] = y3; c[3] = z22; c[4] = e2; c[5] = y4; c[6] = z12; c[7] = x3; c[8] = z3; c[9] = z022; c[10] = z012; c[11] = z03;
                                break;
                            case "BY":
                                c[1] = e1; c[2] = y3; c[3] = z22; c[4] = e2; c[5] = y4; c[6] = z12; c[7] = x3; c[8] = z3; c[9] = z022; c[10] = z012; c[11] = z03;//根据酒湖线修改
                                break;
                            //2017.11.1 根据乌东德补充，添加B0等后续组合情况
                            case "0B":
                                c[1] = e1; c[2] = y3; c[3] = z22; c[4] = e2; c[5] = y4; c[6] = 0; c[7] = x3; c[8] = z3; c[9] = z022; c[10] = 0; c[11] = z03;
                                break;
                            case "B0":
                                c[1] = e1; c[2] = y3; c[3] = 0; c[4] = e2; c[5] = y4; c[6] = z12; c[7] = x3; c[8] = z3; c[9] = 0; c[10] = z012; c[11] = z03;
                                break;
                            case "Y0":
                                c[1] = e1; c[2] = y3; c[3] = 0; c[4] = e2; c[5] = y4; c[6] = z11; c[7] = x3; c[8] = z3; c[9] = 0; c[10] = z011; c[11] = z03;
                                break;
                            case "0Y":
                                c[1] = e1; c[2] = y3; c[3] = z21; c[4] = e2; c[5] = y4; c[6] = 0; c[7] = x3; c[8] = z3; c[9] = z021; c[10] = 0; c[11] = z03;
                                break;
                            //case "00";
                            //c[1] = e1; c[2] = y3; c[3] = 0; c[4] = e2; c[5] = y4; c[6] = 0; c[7] = x3; c[8] = z3; c[9] = 0; c[10] = 0; c[11] = z03;
                            //break;
                            case "YT":
                                c[1] = e1; c[2] = y3; c[3] = z12; c[4] = e2; c[5] = y4; c[6] = z11; c[7] = x3; c[8] = z3; c[9] = z012; c[10] = z011; c[11] = z03;
                                break;
                            case "0T":
                                c[1] = e1; c[2] = y3; c[3] = z12; c[4] = e2; c[5] = y4; c[6] = 0; c[7] = x3; c[8] = z3; c[9] = z012; c[10] = 0; c[11] = z03;
                                break;
                            case "T0":
                                c[1] = e1; c[2] = y3; c[3] = 0; c[4] = e2; c[5] = y4; c[6] = z22; c[7] = x3; c[8] = z3; c[9] = 0; c[10] = z022; c[11] = z03;
                                break;
                            case "TY":
                                c[1] = e1; c[2] = y3; c[3] = z21; c[4] = e2; c[5] = y4; c[6] = z22; c[7] = x3; c[8] = z3; c[9] = z021; c[10] = z022; c[11] = z03;
                                break;
                            //20190113新版程序
                            default:
                                c[1] = e1; c[2] = y3; c[4] = e2; c[5] = y4; c[7] = x3; c[8] = z3; c[11] = z03;
                                if (Math.Abs(zhs) > 10000)
                                {
                                    //垂荷列数互换法则补充
                                    //'特意针对垂荷组合为12情况
                                    //仅适用于不均匀冰和验算不均匀冰工况，其余工况不适用。在检查模式中判断报错

                                    int zhs1 = Convert.ToInt16(Math.Abs(zhs).ToString().Substring(1, 2));
                                    float zzz11 = GMaxF[j, zhs1] * BL2;
                                    float zzz12 = GMinF[j, zhs1] * BL2;
                                    float zzz21 = GMaxB[j, zhs1] * BL2;
                                    float zzz22 = GMinB[j, zhs1] * BL2;

                                    if (Math.Abs(zhs).ToString().Substring(0, 1) == "1")
                                    {
                                        //前侧垂荷需要替换 万位数为1
                                        if (wd.VertialLoadCode.Substring(0, 1) == "0")
                                        {
                                            c[3] = 0; c[9] = 0;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                        {
                                            c[3] = zzz11; c[9] = z011;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                        {
                                            c[3] = zzz12; c[9] = z012;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                        {
                                            c[3] = zzz21; c[9] = z021;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                        {
                                            c[3] = zzz22; c[9] = z022;
                                        }
                                        else
                                        {
                                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                        }

                                        if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                        {
                                            c[6] = 0; c[10] = 0;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                        {
                                            c[6] = z11; c[10] = z011;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                        {
                                            c[6] = z12; c[10] = z012;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                        {
                                            c[6] = z21; c[10] = z021;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                        {
                                            c[6] = z22; c[10] = z022;
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
                                            c[3] = 0; c[9] = 0;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                        {
                                            c[3] = z11; c[9] = z011;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                        {
                                            c[3] = z12; c[9] = z012;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                        {
                                            c[3] = z21; c[9] = z021;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                        {
                                            c[3] = z22; c[9] = z022;
                                        }
                                        else
                                        {
                                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                        }

                                        if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                        {
                                            c[6] = 0; c[10] = 0;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                        {
                                            c[6] = zzz11; c[10] = z011;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                        {
                                            c[6] = zzz12; c[10] = z012;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                        {
                                            c[6] = zzz21; c[10] = z021;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                        {
                                            c[6] = zzz22; c[10] = z022;
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
                                        c[3] = 0; c[9] = 0;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                    {
                                        c[3] = z11; c[9] = z011;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                    {
                                        c[3] = z12; c[9] = z012;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                    {
                                        c[3] = z21; c[9] = z021;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                    {
                                        c[3] = z22; c[9] = z022;
                                    }
                                    else
                                    {
                                        throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                    }

                                    if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                    {
                                        c[6] = 0; c[10] = 0;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                    {
                                        c[6] = z11; c[10] = z011;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                    {
                                        c[6] = z12; c[10] = z012;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                    {
                                        c[6] = z21; c[10] = z021;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                    {
                                        c[6] = z22; c[10] = z022;
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
                                c[1] = e1 ; c[2] = y3 ; c[3] = z11 ; c[4] = e2 ; c[5] = y4 ; c[6] = z21 ; c[7] = x3 ; c[8] = z3 ; c[9] = z011 ; c[10] = z021 ; c[11] = z03;
                                break;
                            case "YB":
                                c[1] = e1 ; c[2] = y3 ; c[3] = z11 ; c[4] = e2 ; c[5] = y4 ; c[6] = z22 ; c[7] = x3 ; c[8] = z3 ; c[9] = z011 ; c[10] = z022 ; c[11] = z03;
                                break;
                            case "0B":
                                c[1] = e1 ; c[2] = y3 ; c[3] = 0 ; c[4] = e2 ; c[5] = y4 ; c[6] = z22 ; c[7] = x3 ; c[8] = z3 ; c[9] = 0 ; c[10] = z022 ; c[11] = z03;
                                break;
                            case "B0":
                                c[1] = e1 ; c[2] = y3 ; c[3] = z12 ; c[4] = e2 ; c[5] = y4 ; c[6] = 0 ; c[7] = x3 ; c[8] = z3 ; c[9] = z012 ; c[10] = 0 ; c[11] = z03;
                                break;
                            case "Y0":
                                c[1] = e1 ; c[2] = y3 ; c[3] = z11 ; c[4] = e2 ; c[5] = y4 ; c[6] = 0 ; c[7] = x3 ; c[8] = z3 ; c[9] = z011 ; c[10] = 0 ; c[11] = z03;
                                break;
                            case "BB":
                                c[1] = e1 ; c[2] = y3 ; c[3] = z12 ; c[4] = e2 ; c[5] = y4 ; c[6] = z22 ; c[7] = x3 ; c[8] = z3 ; c[9] = z012 ; c[10] = z022 ; c[11] = z03;
                                break;
                            case "BY":
                                c[1] = e1 ; c[2] = y3 ; c[3] = z12 ; c[4] = e2 ; c[5] = y4 ; c[6] = z21 ; c[7] = x3 ; c[8] = z3 ; c[9] = z012 ; c[10] = z021 ; c[11] = z03;
                                break;
                            case "0Y":
                                c[1] = e1 ; c[2] = y3 ; c[3] = 0 ; c[4] = e2 ; c[5] = y4 ; c[6] = z21 ; c[7] = x3 ; c[8] = z3 ; c[9] = 0 ; c[10] = z021 ; c[11] = z03;
                                break;
                            //case "00":
                            //c[1] = e1 ; c[2] = y3 ; c[3] = 0 ; c[4] = e2 ; c[5] = y4 ; c[6] = 0 ; c[7] = x3 ; c[8] = z3 ; c[9] = 0 ; c[10] = 0 ; c[11] = z03;
                                //break;
                            //2016.9.24 针对前后侧同时取某一侧的拔力新增,T表示某侧取相反侧上拔力计算
                            case "YT":
                                c[1] = e1 ; c[2] = y3 ; c[3] = z11 ; c[4] = e2 ; c[5] = y4 ; c[6] = z12 ; c[7] = x3 ; c[8] = z3 ; c[9] = z011 ; c[10] = z012 ; c[11] = z03;
                                break;
                            case "0T":
                                c[1] = e1 ; c[2] = y3 ; c[3] = 0 ; c[4] = e2 ; c[5] = y4 ; c[6] = z12 ; c[7] = x3 ; c[8] = z3 ; c[9] = 0 ; c[10] = z012 ; c[11] = z03;
                                break;
                            case "T0":
                                c[1] = e1 ; c[2] = y3 ; c[3] = z22 ; c[4] = e2 ; c[5] = y4 ; c[6] = 0 ; c[7] = x3 ; c[8] = z3 ; c[9] = z022 ; c[10] = 0 ; c[11] = z03;
                                break;
                            case "TY":
                                c[1] = e1 ; c[2] = y3 ; c[3] = z22 ; c[4] = e2 ; c[5] = y4 ; c[6] = z21 ; c[7] = x3 ; c[8] = z3 ; c[9] = z022 ; c[10] = z021 ; c[11] = z03;
                                break;
                            //20190113新版程序
                            default:
                                c[1] = e1 ; c[2] = y3 ; c[4] = e2 ; c[5] = y4 ; c[7] = x3 ; c[8] = z3 ; c[11] = z03;
                                if (Math.Abs(zhs) > 10000)
                                {
                                    //垂荷列数互换法则补充
                                    //'特意针对垂荷组合为12情况
                                    //仅适用于不均匀冰和验算不均匀冰工况，其余工况不适用。在检查模式中判断报错

                                    int zhs1 = Convert.ToInt16(Math.Abs(zhs).ToString().Substring(1, 2));
                                    float zzz11 = GMaxF[j, zhs1] * BL2;
                                    float zzz12 = GMinF[j, zhs1] * BL2;
                                    float zzz21 = GMaxB[j, zhs1] * BL2;
                                    float zzz22 = GMinB[j, zhs1] * BL2;

                                    if (Math.Abs(zhs).ToString().Substring(0, 1) == "1")
                                    {
                                        //前侧垂荷需要替换 万位数为1
                                        if (wd.VertialLoadCode.Substring(0, 1) == "0")
                                        {
                                            c[3] = 0; c[9] = 0;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                        {
                                            c[3] = zzz11; c[9] = z011;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                        {
                                            c[3] = zzz12; c[9] = z012;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                        {
                                            c[3] = zzz21; c[9] = z021;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                        {
                                            c[3] = zzz22; c[9] = z022;
                                        }
                                        else
                                        {
                                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                        }

                                        if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                        {
                                            c[6] = 0; c[10] = 0;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                        {
                                            c[6] = z11; c[10] = z011;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                        {
                                            c[6] = z12; c[10] = z012;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                        {
                                            c[6] = z21; c[10] = z021;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                        {
                                            c[6] = z22; c[10] = z022;
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
                                            c[3] = 0; c[9] = 0;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                        {
                                            c[3] = z11; c[9] = z011;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                        {
                                            c[3] = z12; c[9] = z012;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                        {
                                            c[3] = z21; c[9] = z021;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                        {
                                            c[3] = z22; c[9] = z022;
                                        }
                                        else
                                        {
                                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                        }

                                        if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                        {
                                            c[6] = 0; c[10] = 0;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                        {
                                            c[6] = zzz11; c[10] = z011;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                        {
                                            c[6] = zzz12; c[10] = z012;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                        {
                                            c[6] = zzz21; c[10] = z021;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                        {
                                            c[6] = zzz22; c[10] = z022;
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
                                        c[3] = 0 ; c[9] = 0;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                    {
                                        c[3] = z11 ; c[9] = z011;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                    {
                                        c[3] = z12 ; c[9] = z012;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                    {
                                        c[3] = z21 ; c[9] = z021;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                    {
                                        c[3] = z22 ; c[9] = z022;
                                    }
                                    else
                                    {
                                        throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                    }

                                    if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                    {
                                        c[6] = 0 ; c[10] = 0;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                    {
                                        c[6] = z11; c[10] = z011;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                    {
                                        c[6] = z12; c[10] = z012;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                    {
                                        c[6] = z21; c[10] = z021;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                    {
                                        c[6] = z22; c[10] = z022;
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
                            y3 = y1;
                            y4 = y2;
                        }
                        else
                        {
                            y3 = y1;
                            y4 = LineParas.PortalTensionMin;
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
                            y4 = LineParas.PortalTensionMax;
                        }
                    }

                    if (Paras.IsMethod1Selected && zhs < 0)
                    {
                        //按最严重情况考虑扭转 20170620新增  扭转相垂荷按最严重情况考虑
                        switch (wd.VertialLoadCode)
                        {
                            case "YY":
                                c[1] = e1 ; c[2] = y3 ; c[3] = z21 ; c[4] = e2 ; c[5] = y4 ; c[6] = z11 ; c[7] = x3 ; c[8] = z3 ; c[9] = z021 ; c[10] = z011 ; c[11] = z03;
                                break;
                            case "YB":
                                c[1] = e1 ; c[2] = y3 ; c[3] = z21 ; c[4] = e2 ; c[5] = y4 ; c[6] = z12 ; c[7] = x3 ; c[8] = z3 ; c[9] = z021 ; c[10] = z012 ; c[11] = z03; //根据酒湖线修改
                                break;
                            case "BB":
                                c[1] = e1 ; c[2] = y3 ; c[3] = z22 ; c[4] = e2 ; c[5] = y4 ; c[6] = z12 ; c[7] = x3 ; c[8] = z3 ; c[9] = z022 ; c[10] = z012 ; c[11] = z03;
                                break;
                            case "BY":
                                c[1] = e1 ; c[2] = y3 ; c[3] = z22 ; c[4] = e2 ; c[5] = y4 ; c[6] = z12 ; c[7] = x3 ; c[8] = z3 ; c[9] = z022 ; c[10] = z012 ; c[11] = z03;  //根据酒湖线修改
                                break;
                            //2017.11.1 根据乌东德补充，添加B0等后续组合情况
                            case "0B":
                                c[1] = e1 ; c[2] = y3 ; c[3] = z22 ; c[4] = e2 ; c[5] = y4 ; c[6] = 0 ; c[7] = x3 ; c[8] = z3 ; c[9] = z022 ; c[10] = 0 ; c[11] = z03;
                                break;
                            case "B0":
                                c[1] = e1 ; c[2] = y3 ; c[3] = 0 ; c[4] = e2 ; c[5] = y4 ; c[6] = z12 ; c[7] = x3 ; c[8] = z3 ; c[9] = 0 ; c[10] = z012 ; c[11] = z03;
                                break;
                            case "Y0":
                                c[1] = e1 ; c[2] = y3 ; c[3] = 0 ; c[4] = e2 ; c[5] = y4 ; c[6] = z11 ; c[7] = x3 ; c[8] = z3 ; c[9] = 0 ; c[10] = z011 ; c[11] = z03;
                                break;
                            case "0Y":
                                c[1] = e1 ; c[2] = y3 ; c[3] = z21 ; c[4] = e2 ; c[5] = y4 ; c[6] = 0 ; c[7] = x3 ; c[8] = z3 ; c[9] = z021 ; c[10] = 0 ; c[11] = z03;
                                break;
                            //case "00":
                            //c[1] = e1 ; c[2] = y3 ; c[3] = 0 ; c[4] = e2 ; c[5] = y4 ; c[6] = 0 ; c[7] = x3 ; c[8] = z3 ; c[9] = 0 ; c[10] = 0 ; c[11] = z03;
                            //break;
                            case "YT":
                                c[1] = e1 ; c[2] = y3 ; c[3] = z12 ; c[4] = e2 ; c[5] = y4 ; c[6] = z11 ; c[7] = x3 ; c[8] = z3 ; c[9] = z012 ; c[10] = z011 ; c[11] = z03;
                                break;
                            case "0T":
                                c[1] = e1 ; c[2] = y3 ; c[3] = z12 ; c[4] = e2 ; c[5] = y4 ; c[6] = 0 ; c[7] = x3 ; c[8] = z3 ; c[9] = z012 ; c[10] = 0 ; c[11] = z03;
                                break;
                            case "T0":
                                c[1] = e1 ; c[2] = y3 ; c[3] = 0 ; c[4] = e2 ; c[5] = y4 ; c[6] = z22 ; c[7] = x3 ; c[8] = z3 ; c[9] = 0 ; c[10] = z022 ; c[11] = z03;
                                break;
                            case "TY":
                                c[1] = e1 ; c[2] = y3 ; c[3] = z21 ; c[4] = e2 ; c[5] = y4 ; c[6] = z22 ; c[7] = x3 ; c[8] = z3 ; c[9] = z021 ; c[10] = z022 ; c[11] = z03;
                                break;
                            //20190113新版程序
                            default:
                                c[1] = e1 ; c[2] = y3 ; c[4] = e2 ; c[5] = y4 ; c[7] = x3 ; c[8] = z3 ; c[11] = z03;
                                if (Math.Abs(zhs) > 10000)
                                {
                                    //垂荷列数互换法则补充
                                    //'特意针对垂荷组合为12情况
                                    //仅适用于不均匀冰和验算不均匀冰工况，其余工况不适用。在检查模式中判断报错

                                    int zhs1 = Convert.ToInt16(Math.Abs(zhs).ToString().Substring(1, 2));
                                    float zzz11 = GMaxF[j, zhs1] * BL2;
                                    float zzz12 = GMinF[j, zhs1] * BL2;
                                    float zzz21 = GMaxB[j, zhs1] * BL2;
                                    float zzz22 = GMinB[j, zhs1] * BL2;

                                    if (Math.Abs(zhs).ToString().Substring(0, 1) == "1")
                                    {
                                        //前侧垂荷需要替换 万位数为1
                                        if (wd.VertialLoadCode.Substring(0, 1) == "0")
                                        {
                                            c[3] = 0; c[9] = 0;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                        {
                                            c[3] = zzz11; c[9] = z011;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                        {
                                            c[3] = zzz12; c[9] = z012;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                        {
                                            c[3] = zzz21; c[9] = z021;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                        {
                                            c[3] = zzz22; c[9] = z022;
                                        }
                                        else
                                        {
                                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                        }

                                        if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                        {
                                            c[6] = 0; c[10] = 0;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                        {
                                            c[6] = z11; c[10] = z011;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                        {
                                            c[6] = z12; c[10] = z012;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                        {
                                            c[6] = z21; c[10] = z021;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                        {
                                            c[6] = z22; c[10] = z022;
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
                                            c[3] = 0; c[9] = 0;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                        {
                                            c[3] = z11; c[9] = z011;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                        {
                                            c[3] = z12; c[9] = z012;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                        {
                                            c[3] = z21; c[9] = z021;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                        {
                                            c[3] = z22; c[9] = z022;
                                        }
                                        else
                                        {
                                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                        }

                                        if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                        {
                                            c[6] = 0; c[10] = 0;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                        {
                                            c[6] = zzz11; c[10] = z011;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                        {
                                            c[6] = zzz12; c[10] = z012;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                        {
                                            c[6] = zzz21; c[10] = z021;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                        {
                                            c[6] = zzz22; c[10] = z022;
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
                                        c[3] = 0; c[9] = 0;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                    {
                                        c[3] = z11; c[9] = z011;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                    {
                                        c[3] = z12; c[9] = z012;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                    {
                                        c[3] = z21; c[9] = z021;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                    {
                                        c[3] = z22; c[9] = z022;
                                    }
                                    else
                                    {
                                        throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                    }

                                    if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                    {
                                        c[6] = 0; c[10] = 0;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                    {
                                        c[6] = z11; c[10] = z011;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                    {
                                        c[6] = z12; c[10] = z012;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                    {
                                        c[6] = z21; c[10] = z021;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                    {
                                        c[6] = z22; c[10] = z022;
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
                                c[1] = e1 ; c[2] = y3 ; c[3] = z11 ; c[4] = e2 ; c[5] = y4 ; c[6] = z21 ; c[7] = x3 ; c[8] = z3 ; c[9] = z011 ; c[10] = z021 ; c[11] = z03;
                                break;
                            case "YB":
                                c[1] = e1 ; c[2] = y3 ; c[3] = z11 ; c[4] = e2 ; c[5] = y4 ; c[6] = z22 ; c[7] = x3 ; c[8] = z3 ; c[9] = z011 ; c[10] = z022 ; c[11] = z03;
                                break;
                            case "0B":
                                c[1] = e1 ; c[2] = y3 ; c[3] = 0 ; c[4] = e2 ; c[5] = y4 ; c[6] = z22 ; c[7] = x3 ; c[8] = z3 ; c[9] = 0 ; c[10] = z022 ; c[11] = z03;
                                break;  
                            case "B0":
                                c[1] = e1 ; c[2] = y3 ; c[3] = z12 ; c[4] = e2 ; c[5] = y4 ; c[6] = 0 ; c[7] = x3 ; c[8] = z3 ; c[9] = z012 ; c[10] = 0 ; c[11] = z03;
                                break;
                            case "Y0":
                                c[1] = e1 ; c[2] = y3 ; c[3] = z11 ; c[4] = e2 ; c[5] = y4 ; c[6] = 0 ; c[7] = x3 ; c[8] = z3 ; c[9] = z011 ; c[10] = 0 ; c[11] = z03;
                                break;
                            case "BB":
                                c[1] = e1 ; c[2] = y3 ; c[3] = z12 ; c[4] = e2 ; c[5] = y4 ; c[6] = z22 ; c[7] = x3 ; c[8] = z3 ; c[9] = z012 ; c[10] = z022 ; c[11] = z03;
                                break;
                            case "BY":
                                c[1] = e1 ; c[2] = y3 ; c[3] = z12 ; c[4] = e2 ; c[5] = y4 ; c[6] = z21 ; c[7] = x3 ; c[8] = z3 ; c[9] = z012 ; c[10] = z021 ; c[11] = z03;
                                break;
                            case "0Y":
                                c[1] = e1 ; c[2] = y3 ; c[3] = 0 ; c[4] = e2 ; c[5] = y4 ; c[6] = z21 ; c[7] = x3 ; c[8] = z3 ; c[9] = 0 ; c[10] = z021 ; c[11] = z03;
                                break;
                            //case "00":
                                //c[1] = e1 ; c[2] = y3 ; c[3] = 0 ; c[4] = e2 ; c[5] = y4 ; c[6] = 0 ; c[7] = x3 ; c[8] = z3 ; c[9] = 0 ; c[10] = 0 ; c[11] = z03;
                                //break;
                            //2016.9.24 针对前后侧同时取某一侧的拔力新增,T表示某侧取相反侧上拔力计算
                            case "YT":
                                c[1] = e1 ; c[2] = y3 ; c[3] = z11 ; c[4] = e2 ; c[5] = y4 ; c[6] = z12 ; c[7] = x3 ; c[8] = z3 ; c[9] = z011 ; c[10] = z012 ; c[11] = z03;
                                break;
                            case "0T":
                                c[1] = e1 ; c[2] = y3 ; c[3] = 0 ; c[4] = e2 ; c[5] = y4 ; c[6] = z12 ; c[7] = x3 ; c[8] = z3 ; c[9] = 0 ; c[10] = z012 ; c[11] = z03;
                                break;
                            case "T0":
                                c[1] = e1 ; c[2] = y3 ; c[3] = z22 ; c[4] = e2 ; c[5] = y4 ; c[6] = 0 ; c[7] = x3 ; c[8] = z3 ; c[9] = z022 ; c[10] = 0 ; c[11] = z03;
                                break;
                            case "TY":
                                c[1] = e1 ; c[2] = y3 ; c[3] = z22 ; c[4] = e2 ; c[5] = y4 ; c[6] = z21 ; c[7] = x3 ; c[8] = z3 ; c[9] = z022 ; c[10] = z021 ; c[11] = z03;
                                break;
                            //20190113新版程序
                            default:
                                c[1] = e1 ; c[2] = y3 ; c[4] = e2 ; c[5] = y4 ; c[7] = x3 ; c[8] = z3 ; c[11] = z03;
                                if (Math.Abs(zhs) > 10000)
                                {
                                    //垂荷列数互换法则补充
                                    //'特意针对垂荷组合为12情况
                                    //仅适用于不均匀冰和验算不均匀冰工况，其余工况不适用。在检查模式中判断报错

                                    int zhs1 = Convert.ToInt16(Math.Abs(zhs).ToString().Substring(1, 2));
                                    float zzz11 = GMaxF[j, zhs1] * BL2;
                                    float zzz12 = GMinF[j, zhs1] * BL2;
                                    float zzz21 = GMaxB[j, zhs1] * BL2;
                                    float zzz22 = GMinB[j, zhs1] * BL2;

                                    if (Math.Abs(zhs).ToString().Substring(0, 1) == "1")
                                    {
                                        //前侧垂荷需要替换 万位数为1
                                        if (wd.VertialLoadCode.Substring(0, 1) == "0")
                                        {
                                            c[3] = 0; c[9] = 0;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                        {
                                            c[3] = zzz11; c[9] = z011;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                        {
                                            c[3] = zzz12; c[9] = z012;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                        {
                                            c[3] = zzz21; c[9] = z021;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                        {
                                            c[3] = zzz22; c[9] = z022;
                                        }
                                        else
                                        {
                                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                        }

                                        if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                        {
                                            c[6] = 0; c[10] = 0;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                        {
                                            c[6] = z11; c[10] = z011;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                        {
                                            c[6] = z12; c[10] = z012;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                        {
                                            c[6] = z21; c[10] = z021;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                        {
                                            c[6] = z22; c[10] = z022;
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
                                            c[3] = 0; c[9] = 0;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                        {
                                            c[3] = z11; c[9] = z011;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                        {
                                            c[3] = z12; c[9] = z012;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                        {
                                            c[3] = z21; c[9] = z021;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                        {
                                            c[3] = z22; c[9] = z022;
                                        }
                                        else
                                        {
                                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                        }

                                        if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                        {
                                            c[6] = 0; c[10] = 0;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                        {
                                            c[6] = zzz11; c[10] = z011;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                        {
                                            c[6] = zzz12; c[10] = z012;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                        {
                                            c[6] = zzz21; c[10] = z021;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                        {
                                            c[6] = zzz22; c[10] = z022;
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
                                        c[3] = 0 ; c[9] = 0;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                    {
                                        c[3] = z11 ; c[9] = z011;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                    {
                                        c[3] = z12 ; c[9] = z012;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                    {
                                        c[3] = z21 ; c[9] = z021;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                    {
                                        c[3] = z22 ; c[9] = z022;
                                    }
                                    else
                                    {
                                        throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                    }

                                    if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                    {
                                        c[6] = 0 ; c[10] = 0;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                    {
                                        c[6] = z11; c[10] = z011;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                    {
                                        c[6] = z12; c[10] = z012;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                    {
                                        c[6] = z21; c[10] = z021;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                    {
                                        c[6] = z22; c[10] = z022;
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
                            y3 = y1;
                            y4 = y2;
                        }
                        else
                        {
                            y3 = y2;
                            y4 = LineParas.PortalTensionMax;
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
                            y4 = LineParas.PortalTensionMin;
                        }
                    }

                    if (Paras.IsMethod1Selected && zhs < 0)
                    {
                        //按最严重情况考虑扭转 20170620新增  扭转相垂荷按最严重情况考虑
                        switch (wd.VertialLoadCode)
                        {
                            case "YY":
                                c[1] = e1 ; c[2] = y3 ; c[3] = z21 ; c[4] = e2 ; c[5] = y4 ; c[6] = z11 ; c[7] = x3 ; c[8] = z3 ; c[9] = z021 ; c[10] = z011 ; c[11] = z03;
                                break;
                            case "YB":
                                c[1] = e1 ; c[2] = y3 ; c[3] = z21 ; c[4] = e2 ; c[5] = y4 ; c[6] = z12 ; c[7] = x3 ; c[8] = z3 ; c[9] = z021 ; c[10] = z012 ; c[11] = z03; //根据酒湖线修改
                                break;
                            case "BB":
                                c[1] = e1 ; c[2] = y3 ; c[3] = z22 ; c[4] = e2 ; c[5] = y4 ; c[6] = z12 ; c[7] = x3 ; c[8] = z3 ; c[9] = z022 ; c[10] = z012 ; c[11] = z03;
                                break;
                            case "BY":
                                c[1] = e1 ; c[2] = y3 ; c[3] = z22 ; c[4] = e2 ; c[5] = y4 ; c[6] = z12 ; c[7] = x3 ; c[8] = z3 ; c[9] = z022 ; c[10] = z012 ; c[11] = z03;  //根据酒湖线修改
                                break;
                            //2017.11.1 根据乌东德补充，添加B0等后续组合情况
                            case "0B":
                                c[1] = e1 ; c[2] = y3 ; c[3] = z22 ; c[4] = e2 ; c[5] = y4 ; c[6] = 0 ; c[7] = x3 ; c[8] = z3 ; c[9] = z022 ; c[10] = 0 ; c[11] = z03;
                                break;
                            case "B0":
                                c[1] = e1 ; c[2] = y3 ; c[3] = 0 ; c[4] = e2 ; c[5] = y4 ; c[6] = z12 ; c[7] = x3 ; c[8] = z3 ; c[9] = 0 ; c[10] = z012 ; c[11] = z03;
                                break;
                            case "Y0":
                                c[1] = e1 ; c[2] = y3 ; c[3] = 0 ; c[4] = e2 ; c[5] = y4 ; c[6] = z11 ; c[7] = x3 ; c[8] = z3 ; c[9] = 0 ; c[10] = z011 ; c[11] = z03;
                                break;
                            case "0Y":
                                c[1] = e1 ; c[2] = y3 ; c[3] = z21 ; c[4] = e2 ; c[5] = y4 ; c[6] = 0 ; c[7] = x3 ; c[8] = z3 ; c[9] = z021 ; c[10] = 0 ; c[11] = z03;
                                break;
                            //case "00":
                                //c[1] = e1 ; c[2] = y3 ; c[3] = 0 ; c[4] = e2 ; c[5] = y4 ; c[6] = 0 ; c[7] = x3 ; c[8] = z3 ; c[9] = 0 ; c[10] = 0 ; c[11] = z03;
                                //break;
                            case "YT":
                                c[1] = e1 ; c[2] = y3 ; c[3] = z12 ; c[4] = e2 ; c[5] = y4 ; c[6] = z11 ; c[7] = x3 ; c[8] = z3 ; c[9] = z012 ; c[10] = z011 ; c[11] = z03;
                                break;
                            case "0T":
                                c[1] = e1 ; c[2] = y3 ; c[3] = z12 ; c[4] = e2 ; c[5] = y4 ; c[6] = 0 ; c[7] = x3 ; c[8] = z3 ; c[9] = z012 ; c[10] = 0 ; c[11] = z03;
                                break;
                            case "T0":
                                c[1] = e1 ; c[2] = y3 ; c[3] = 0 ; c[4] = e2 ; c[5] = y4 ; c[6] = z22 ; c[7] = x3 ; c[8] = z3 ; c[9] = 0 ; c[10] = z022 ; c[11] = z03;
                                break;
                            case "TY":
                                c[1] = e1 ; c[2] = y3 ; c[3] = z21 ; c[4] = e2 ; c[5] = y4 ; c[6] = z22 ; c[7] = x3 ; c[8] = z3 ; c[9] = z021 ; c[10] = z022 ; c[11] = z03;
                                break;
                            //20190113新版程序
                            default:
                                c[1] = e1 ; c[2] = y3 ; c[4] = e2 ; c[5] = y4 ; c[7] = x3 ; c[8] = z3 ; c[11] = z03;
                                if (Math.Abs(zhs) > 10000)
                                {
                                    //垂荷列数互换法则补充
                                    //'特意针对垂荷组合为12情况
                                    //仅适用于不均匀冰和验算不均匀冰工况，其余工况不适用。在检查模式中判断报错

                                    int zhs1 = Convert.ToInt16(Math.Abs(zhs).ToString().Substring(1, 2));
                                    float zzz11 = GMaxF[j, zhs1] * BL2;
                                    float zzz12 = GMinF[j, zhs1] * BL2;
                                    float zzz21 = GMaxB[j, zhs1] * BL2;
                                    float zzz22 = GMinB[j, zhs1] * BL2;

                                    if (Math.Abs(zhs).ToString().Substring(0, 1) == "1")
                                    {
                                        //前侧垂荷需要替换 万位数为1
                                        if (wd.VertialLoadCode.Substring(0, 1) == "0")
                                        {
                                            c[3] = 0; c[9] = 0;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                        {
                                            c[3] = zzz11; c[9] = z011;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                        {
                                            c[3] = zzz12; c[9] = z012;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                        {
                                            c[3] = zzz21; c[9] = z021;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                        {
                                            c[3] = zzz22; c[9] = z022;
                                        }
                                        else
                                        {
                                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                        }

                                        if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                        {
                                            c[6] = 0; c[10] = 0;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                        {
                                            c[6] = z11; c[10] = z011;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                        {
                                            c[6] = z12; c[10] = z012;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                        {
                                            c[6] = z21; c[10] = z021;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                        {
                                            c[6] = z22; c[10] = z022;
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
                                            c[3] = 0; c[9] = 0;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                        {
                                            c[3] = z11; c[9] = z011;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                        {
                                            c[3] = z12; c[9] = z012;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                        {
                                            c[3] = z21; c[9] = z021;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                        {
                                            c[3] = z22; c[9] = z022;
                                        }
                                        else
                                        {
                                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                        }

                                        if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                        {
                                            c[6] = 0; c[10] = 0;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                        {
                                            c[6] = zzz11; c[10] = z011;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                        {
                                            c[6] = zzz12; c[10] = z012;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                        {
                                            c[6] = zzz21; c[10] = z021;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                        {
                                            c[6] = zzz22; c[10] = z022;
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
                                        c[3] = 0; c[9] = 0;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                    {
                                        c[3] = z11; c[9] = z011;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                    {
                                        c[3] = z12; c[9] = z012;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                    {
                                        c[3] = z21; c[9] = z021;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                    {
                                        c[3] = z22; c[9] = z022;
                                    }
                                    else
                                    {
                                        throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                    }

                                    if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                    {
                                        c[6] = 0; c[10] = 0;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                    {
                                        c[6] = z11; c[10] = z011;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                    {
                                        c[6] = z12; c[10] = z012;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                    {
                                        c[6] = z21; c[10] = z021;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                    {
                                        c[6] = z22; c[10] = z022;
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
                                c[1] = e1 ; c[2] = y3 ; c[3] = z11 ; c[4] = e2 ; c[5] = y4 ; c[6] = z21 ; c[7] = x3 ; c[8] = z3 ; c[9] = z011 ; c[10] = z021 ; c[11] = z03;
                                break;
                            case "YB":
                                c[1] = e1 ; c[2] = y3 ; c[3] = z11 ; c[4] = e2 ; c[5] = y4 ; c[6] = z22 ; c[7] = x3 ; c[8] = z3 ; c[9] = z011 ; c[10] = z022 ; c[11] = z03;
                                break;
                            case "0B":
                                c[1] = e1 ; c[2] = y3 ; c[3] = 0 ; c[4] = e2 ; c[5] = y4 ; c[6] = z22 ; c[7] = x3 ; c[8] = z3 ; c[9] = 0 ; c[10] = z022 ; c[11] = z03;
                                break;
                            case "B0":
                                c[1] = e1 ; c[2] = y3 ; c[3] = z12 ; c[4] = e2 ; c[5] = y4 ; c[6] = 0 ; c[7] = x3 ; c[8] = z3 ; c[9] = z012 ; c[10] = 0 ; c[11] = z03;
                                break;
                            case "Y0":
                                c[1] = e1 ; c[2] = y3 ; c[3] = z11 ; c[4] = e2 ; c[5] = y4 ; c[6] = 0 ; c[7] = x3 ; c[8] = z3 ; c[9] = z011 ; c[10] = 0 ; c[11] = z03;
                                break;
                            case "BB":
                                c[1] = e1 ; c[2] = y3 ; c[3] = z12 ; c[4] = e2 ; c[5] = y4 ; c[6] = z22 ; c[7] = x3 ; c[8] = z3 ; c[9] = z012 ; c[10] = z022 ; c[11] = z03;
                                break;
                            case "BY":
                                c[1] = e1 ; c[2] = y3 ; c[3] = z12 ; c[4] = e2 ; c[5] = y4 ; c[6] = z21 ; c[7] = x3 ; c[8] = z3 ; c[9] = z012 ; c[10] = z021 ; c[11] = z03;
                                break;
                            case "0Y":
                                c[1] = e1 ; c[2] = y3 ; c[3] = 0 ; c[4] = e2 ; c[5] = y4 ; c[6] = z21 ; c[7] = x3 ; c[8] = z3 ; c[9] = 0 ; c[10] = z021 ; c[11] = z03;
                                break;
                            //case "00":
                            //c[1] = e1 ; c[2] = y3 ; c[3] = 0 ; c[4] = e2 ; c[5] = y4 ; c[6] = 0 ; c[7] = x3 ; c[8] = z3 ; c[9] = 0 ; c[10] = 0 ; c[11] = z03;
                            //break;
                            //2016.9.24 针对前后侧同时取某一侧的拔力新增,T表示某侧取相反侧上拔力计算
                            case "YT":
                                c[1] = e1 ; c[2] = y3 ; c[3] = z11 ; c[4] = e2 ; c[5] = y4 ; c[6] = z12 ; c[7] = x3 ; c[8] = z3 ; c[9] = z011 ; c[10] = z012 ; c[11] = z03;
                                break;
                            case "0T":
                                c[1] = e1 ; c[2] = y3 ; c[3] = 0 ; c[4] = e2 ; c[5] = y4 ; c[6] = z12 ; c[7] = x3 ; c[8] = z3 ; c[9] = 0 ; c[10] = z012 ; c[11] = z03;
                                break;
                            case "T0":
                                c[1] = e1 ; c[2] = y3 ; c[3] = z22 ; c[4] = e2 ; c[5] = y4 ; c[6] = 0 ; c[7] = x3 ; c[8] = z3 ; c[9] = z022 ; c[10] = 0 ; c[11] = z03;
                                break;
                            case "TY":
                                c[1] = e1 ; c[2] = y3 ; c[3] = z22 ; c[4] = e2 ; c[5] = y4 ; c[6] = z21 ; c[7] = x3 ; c[8] = z3 ; c[9] = z022 ; c[10] = z021 ; c[11] = z03;
                                break;
                            //20190113新版程序
                            default:
                                c[1] = e1 ; c[2] = y3 ; c[4] = e2 ; c[5] = y4 ; c[7] = x3 ; c[8] = z3 ; c[11] = z03;
                                if (Math.Abs(zhs) > 10000)
                                {
                                    //垂荷列数互换法则补充
                                    //'特意针对垂荷组合为12情况
                                    //仅适用于不均匀冰和验算不均匀冰工况，其余工况不适用。在检查模式中判断报错

                                    int zhs1 = Convert.ToInt16(Math.Abs(zhs).ToString().Substring(1, 2));
                                    float zzz11 = GMaxF[j, zhs1] * BL2;
                                    float zzz12 = GMinF[j, zhs1] * BL2;
                                    float zzz21 = GMaxB[j, zhs1] * BL2;
                                    float zzz22 = GMinB[j, zhs1] * BL2;

                                    if (Math.Abs(zhs).ToString().Substring(0, 1) == "1")
                                    {
                                        //前侧垂荷需要替换 万位数为1
                                        if (wd.VertialLoadCode.Substring(0, 1) == "0")
                                        {
                                            c[3] = 0; c[9] = 0;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                        {
                                            c[3] = zzz11; c[9] = z011;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                        {
                                            c[3] = zzz12; c[9] = z012;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                        {
                                            c[3] = zzz21; c[9] = z021;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                        {
                                            c[3] = zzz22; c[9] = z022;
                                        }
                                        else
                                        {
                                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                        }

                                        if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                        {
                                            c[6] = 0; c[10] = 0;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                        {
                                            c[6] = z11; c[10] = z011;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                        {
                                            c[6] = z12; c[10] = z012;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                        {
                                            c[6] = z21; c[10] = z021;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                        {
                                            c[6] = z22; c[10] = z022;
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
                                            c[3] = 0; c[9] = 0;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                        {
                                            c[3] = z11; c[9] = z011;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                        {
                                            c[3] = z12; c[9] = z012;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                        {
                                            c[3] = z21; c[9] = z021;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                        {
                                            c[3] = z22; c[9] = z022;
                                        }
                                        else
                                        {
                                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                        }

                                        if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                        {
                                            c[6] = 0; c[10] = 0;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                        {
                                            c[6] = zzz11; c[10] = z011;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                        {
                                            c[6] = zzz12; c[10] = z012;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                        {
                                            c[6] = zzz21; c[10] = z021;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                        {
                                            c[6] = zzz22; c[10] = z022;
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
                                        c[3] = 0 ; c[9] = 0;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                    {
                                        c[3] = z11 ; c[9] = z011;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                    {
                                        c[3] = z12 ; c[9] = z012;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                    {
                                        c[3] = z21 ; c[9] = z021;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                    {
                                        c[3] = z22 ; c[9] = z022;
                                    }
                                    else
                                    {
                                        throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                    }

                                    if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                    {
                                        c[6] = 0 ; c[10] = 0;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                    {
                                        c[6] = z11; c[10] = z011;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                    {
                                        c[6] = z12; c[10] = z012;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                    {
                                        c[6] = z21; c[10] = z021;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                    {
                                        c[6] = z22; c[10] = z022;
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
                        y4 = LineParas.PortalTensionMin;
                    }

                    if (Paras.IsMethod1Selected && zhs < 0)
                    {
                        //按最严重情况考虑扭转 20170620新增  扭转相垂荷按最严重情况考虑
                        switch (wd.VertialLoadCode)
                        {
                            case "YY":
                                c[1] = e1 ; c[2] = y3 ; c[3] = z21 ; c[4] = e2 ; c[5] = y4 ; c[6] = z11 ; c[7] = x3 ; c[8] = z3 ; c[9] = z021 ; c[10] = z011 ; c[11] = z03;
                                break;
                            case "YB":
                                c[1] = e1 ; c[2] = y3 ; c[3] = z21 ; c[4] = e2 ; c[5] = y4 ; c[6] = z12 ; c[7] = x3 ; c[8] = z3 ; c[9] = z021 ; c[10] = z012 ; c[11] = z03; //根据酒湖线修改
                                break;
                            case "BB":
                                c[1] = e1 ; c[2] = y3 ; c[3] = z22 ; c[4] = e2 ; c[5] = y4 ; c[6] = z12 ; c[7] = x3 ; c[8] = z3 ; c[9] = z022 ; c[10] = z012 ; c[11] = z03;
                                break;
                            case "BY":
                                c[1] = e1 ; c[2] = y3 ; c[3] = z22 ; c[4] = e2 ; c[5] = y4 ; c[6] = z12 ; c[7] = x3 ; c[8] = z3 ; c[9] = z022 ; c[10] = z012 ; c[11] = z03;   //根据酒湖线修改
                                break;
                            //2017.11.1 根据乌东德补充，添加B0等后续组合情况
                            case "0B":
                                c[1] = e1 ; c[2] = y3 ; c[3] = z22 ; c[4] = e2 ; c[5] = y4 ; c[6] = 0 ; c[7] = x3 ; c[8] = z3 ; c[9] = z022 ; c[10] = 0 ; c[11] = z03;
                                break;
                            case "B0":
                                c[1] = e1 ; c[2] = y3 ; c[3] = 0 ; c[4] = e2 ; c[5] = y4 ; c[6] = z12 ; c[7] = x3 ; c[8] = z3 ; c[9] = 0 ; c[10] = z012 ; c[11] = z03;
                                break;
                            case "Y0":
                                c[1] = e1 ; c[2] = y3 ; c[3] = 0 ; c[4] = e2 ; c[5] = y4 ; c[6] = z11 ; c[7] = x3 ; c[8] = z3 ; c[9] = 0 ; c[10] = z011 ; c[11] = z03;
                                break;
                            case "0Y":
                                c[1] = e1 ; c[2] = y3 ; c[3] = z21 ; c[4] = e2 ; c[5] = y4 ; c[6] = 0 ; c[7] = x3 ; c[8] = z3 ; c[9] = z021 ; c[10] = 0 ; c[11] = z03;
                                break;
                            //case "00":
                            //c[1] = e1 ; c[2] = y3 ; c[3] = 0 ; c[4] = e2 ; c[5] = y4 ; c[6] = 0 ; c[7] = x3 ; c[8] = z3 ; c[9] = 0 ; c[10] = 0 ; c[11] = z03;
                            //    break;
                            case "YT":
                                c[1] = e1 ; c[2] = y3 ; c[3] = z12 ; c[4] = e2 ; c[5] = y4 ; c[6] = z11 ; c[7] = x3 ; c[8] = z3 ; c[9] = z012 ; c[10] = z011 ; c[11] = z03;
                                break;
                            case "0T":
                                c[1] = e1 ; c[2] = y3 ; c[3] = z12 ; c[4] = e2 ; c[5] = y4 ; c[6] = 0 ; c[7] = x3 ; c[8] = z3 ; c[9] = z012 ; c[10] = 0 ; c[11] = z03;
                                break;
                            case "T0":
                                c[1] = e1 ; c[2] = y3 ; c[3] = 0 ; c[4] = e2 ; c[5] = y4 ; c[6] = z22 ; c[7] = x3 ; c[8] = z3 ; c[9] = 0 ; c[10] = z022 ; c[11] = z03;
                                break;
                            case "TY":
                                c[1] = e1 ; c[2] = y3 ; c[3] = z21 ; c[4] = e2 ; c[5] = y4 ; c[6] = z22 ; c[7] = x3 ; c[8] = z3 ; c[9] = z021 ; c[10] = z022 ; c[11] = z03;
                                break;
                            //20190113新版程序
                            default:
                                c[1] = e1 ; c[2] = y3 ; c[4] = e2 ; c[5] = y4 ; c[7] = x3 ; c[8] = z3 ; c[11] = z03;
                                if (Math.Abs(zhs) > 10000)
                                {
                                    //垂荷列数互换法则补充
                                    //'特意针对垂荷组合为12情况
                                    //仅适用于不均匀冰和验算不均匀冰工况，其余工况不适用。在检查模式中判断报错

                                    int zhs1 = Convert.ToInt16(Math.Abs(zhs).ToString().Substring(1, 2));
                                    float zzz11 = GMaxF[j, zhs1] * BL2;
                                    float zzz12 = GMinF[j, zhs1] * BL2;
                                    float zzz21 = GMaxB[j, zhs1] * BL2;
                                    float zzz22 = GMinB[j, zhs1] * BL2;

                                    if (Math.Abs(zhs).ToString().Substring(0, 1) == "1")
                                    {
                                        //前侧垂荷需要替换 万位数为1
                                        if (wd.VertialLoadCode.Substring(0, 1) == "0")
                                        {
                                            c[3] = 0; c[9] = 0;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                        {
                                            c[3] = zzz11; c[9] = z011;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                        {
                                            c[3] = zzz12; c[9] = z012;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                        {
                                            c[3] = zzz21; c[9] = z021;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                        {
                                            c[3] = zzz22; c[9] = z022;
                                        }
                                        else
                                        {
                                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                        }

                                        if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                        {
                                            c[6] = 0; c[10] = 0;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                        {
                                            c[6] = z11; c[10] = z011;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                        {
                                            c[6] = z12; c[10] = z012;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                        {
                                            c[6] = z21; c[10] = z021;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                        {
                                            c[6] = z22; c[10] = z022;
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
                                            c[3] = 0; c[9] = 0;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                        {
                                            c[3] = z11; c[9] = z011;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                        {
                                            c[3] = z12; c[9] = z012;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                        {
                                            c[3] = z21; c[9] = z021;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                        {
                                            c[3] = z22; c[9] = z022;
                                        }
                                        else
                                        {
                                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                        }

                                        if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                        {
                                            c[6] = 0; c[10] = 0;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                        {
                                            c[6] = zzz11; c[10] = z011;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                        {
                                            c[6] = zzz12; c[10] = z012;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                        {
                                            c[6] = zzz21; c[10] = z021;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                        {
                                            c[6] = zzz22; c[10] = z022;
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
                                        c[3] = 0; c[9] = 0;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                    {
                                        c[3] = z11; c[9] = z011;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                    {
                                        c[3] = z12; c[9] = z012;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                    {
                                        c[3] = z21; c[9] = z021;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                    {
                                        c[3] = z22; c[9] = z022;
                                    }
                                    else
                                    {
                                        throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                    }

                                    if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                    {
                                        c[6] = 0; c[10] = 0;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                    {
                                        c[6] = z11; c[10] = z011;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                    {
                                        c[6] = z12; c[10] = z012;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                    {
                                        c[6] = z21; c[10] = z021;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                    {
                                        c[6] = z22; c[10] = z022;
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
                                c[1] = e1 ; c[2] = y3 ; c[3] = z11 ; c[4] = e2 ; c[5] = y4 ; c[6] = z21 ; c[7] = x3 ; c[8] = z3 ; c[9] = z011 ; c[10] = z021 ; c[11] = z03;
                                break;
                            case "YB":
                                c[1] = e1 ; c[2] = y3 ; c[3] = z11 ; c[4] = e2 ; c[5] = y4 ; c[6] = z22 ; c[7] = x3 ; c[8] = z3 ; c[9] = z011 ; c[10] = z022 ; c[11] = z03;
                                break;
                            case "0B":
                                c[1] = e1 ; c[2] = y3 ; c[3] = 0 ; c[4] = e2 ; c[5] = y4 ; c[6] = z22 ; c[7] = x3 ; c[8] = z3 ; c[9] = 0 ; c[10] = z022 ; c[11] = z03;
                                break;
                            case "B0":
                                c[1] = e1 ; c[2] = y3 ; c[3] = z12 ; c[4] = e2 ; c[5] = y4 ; c[6] = 0 ; c[7] = x3 ; c[8] = z3 ; c[9] = z012 ; c[10] = 0 ; c[11] = z03;
                                break;
                            case "Y0":
                                c[1] = e1 ; c[2] = y3 ; c[3] = z11 ; c[4] = e2 ; c[5] = y4 ; c[6] = 0 ; c[7] = x3 ; c[8] = z3 ; c[9] = z011 ; c[10] = 0 ; c[11] = z03;
                                break;
                            case "BB":
                                c[1] = e1 ; c[2] = y3 ; c[3] = z12 ; c[4] = e2 ; c[5] = y4 ; c[6] = z22 ; c[7] = x3 ; c[8] = z3 ; c[9] = z012 ; c[10] = z022 ; c[11] = z03;
                                break;
                            case "BY":
                                c[1] = e1 ; c[2] = y3 ; c[3] = z12 ; c[4] = e2 ; c[5] = y4 ; c[6] = z21 ; c[7] = x3 ; c[8] = z3 ; c[9] = z012 ; c[10] = z021 ; c[11] = z03;
                                break;
                            case "0Y":
                                c[1] = e1 ; c[2] = y3 ; c[3] = 0 ; c[4] = e2 ; c[5] = y4 ; c[6] = z21 ; c[7] = x3 ; c[8] = z3 ; c[9] = 0 ; c[10] = z021 ; c[11] = z03;
                                break;
                            //case "00":
                            //c[1] = e1 ; c[2] = y3 ; c[3] = 0 ; c[4] = e2 ; c[5] = y4 ; c[6] = 0 ; c[7] = x3 ; c[8] = z3 ; c[9] = 0 ; c[10] = 0 ; c[11] = z03;
                            //break;
                            //2016.9.24 针对前后侧同时取某一侧的拔力新增,T表示某侧取相反侧上拔力计算
                            case "YT":
                                c[1] = e1 ; c[2] = y3 ; c[3] = z11 ; c[4] = e2 ; c[5] = y4 ; c[6] = z12 ; c[7] = x3 ; c[8] = z3 ; c[9] = z011 ; c[10] = z012 ; c[11] = z03;
                                break;
                            case "0T":
                                c[1] = e1 ; c[2] = y3 ; c[3] = 0 ; c[4] = e2 ; c[5] = y4 ; c[6] = z12 ; c[7] = x3 ; c[8] = z3 ; c[9] = 0 ; c[10] = z012 ; c[11] = z03;
                                break;
                            case "T0":
                                c[1] = e1 ; c[2] = y3 ; c[3] = z22 ; c[4] = e2 ; c[5] = y4 ; c[6] = 0 ; c[7] = x3 ; c[8] = z3 ; c[9] = z022 ; c[10] = 0 ; c[11] = z03;
                                break;
                            case "TY":
                                c[1] = e1 ; c[2] = y3 ; c[3] = z22 ; c[4] = e2 ; c[5] = y4 ; c[6] = z21 ; c[7] = x3 ; c[8] = z3 ; c[9] = z022 ; c[10] = z021 ; c[11] = z03;
                                break;
                            //20190113新版程序
                            default:
                                c[1] = e1 ; c[2] = y3 ; c[4] = e2 ; c[5] = y4 ; c[7] = x3 ; c[8] = z3 ; c[11] = z03;
                                if (Math.Abs(zhs) > 10000)
                                {
                                    //垂荷列数互换法则补充
                                    //'特意针对垂荷组合为12情况
                                    //仅适用于不均匀冰和验算不均匀冰工况，其余工况不适用。在检查模式中判断报错

                                    int zhs1 = Convert.ToInt16(Math.Abs(zhs).ToString().Substring(1, 2));
                                    float zzz11 = GMaxF[j, zhs1] * BL2;
                                    float zzz12 = GMinF[j, zhs1] * BL2;
                                    float zzz21 = GMaxB[j, zhs1] * BL2;
                                    float zzz22 = GMinB[j, zhs1] * BL2;

                                    if (Math.Abs(zhs).ToString().Substring(0, 1) == "1")
                                    {
                                        //前侧垂荷需要替换 万位数为1
                                        if (wd.VertialLoadCode.Substring(0, 1) == "0")
                                        {
                                            c[3] = 0; c[9] = 0;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                        {
                                            c[3] = zzz11; c[9] = z011;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                        {
                                            c[3] = zzz12; c[9] = z012;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                        {
                                            c[3] = zzz21; c[9] = z021;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                        {
                                            c[3] = zzz22; c[9] = z022;
                                        }
                                        else
                                        {
                                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                        }

                                        if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                        {
                                            c[6] = 0; c[10] = 0;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                        {
                                            c[6] = z11; c[10] = z011;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                        {
                                            c[6] = z12; c[10] = z012;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                        {
                                            c[6] = z21; c[10] = z021;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                        {
                                            c[6] = z22; c[10] = z022;
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
                                            c[3] = 0; c[9] = 0;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                        {
                                            c[3] = z11; c[9] = z011;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                        {
                                            c[3] = z12; c[9] = z012;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                        {
                                            c[3] = z21; c[9] = z021;
                                        }
                                        else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                        {
                                            c[3] = z22; c[9] = z022;
                                        }
                                        else
                                        {
                                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                        }

                                        if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                        {
                                            c[6] = 0; c[10] = 0;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                        {
                                            c[6] = zzz11; c[10] = z011;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                        {
                                            c[6] = zzz12; c[10] = z012;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                        {
                                            c[6] = zzz21; c[10] = z021;
                                        }
                                        else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                        {
                                            c[6] = zzz22; c[10] = z022;
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
                                        c[3] = 0; c[9] = 0;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                    {
                                        c[3] = z11 ; c[9] = z011;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                    {
                                        c[3] = z12 ; c[9] = z012;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                    {
                                        c[3] = z21 ; c[9] = z021;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                    {
                                        c[3] = z22 ; c[9] = z022;
                                    }
                                    else
                                    {
                                        throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                    }

                                    if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                    {
                                        c[6] = 0 ; c[10] = 0;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                    {
                                        c[6] = z11; c[10] = z011;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                    {
                                        c[6] = z12; c[10] = z012;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                    {
                                        c[6] = z21; c[10] = z021;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                    {
                                        c[6] = z22; c[10] = z022;
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
                        //case "00":
                        //    c[1] = e1 ; c[2] = y3 ; c[3] = 0 ; c[4] = 0 ; c[5] = 0 ; c[6] = 0 ; c[7] = 0 ; c[8] = 0 ; c[9] = 0 ; c[10] = 0 ; c[11] = 0;
                        //break;
                        case "B0":
                            c[1] = e1 ; c[2] = y3 ; c[3] = z12 ; c[4] = 0 ; c[5] = y4 ; c[6] = 0 ; c[7] = 0 ; c[8] = 0 ; c[9] = z012 ; c[10] = 0 ; c[11] = 0;
                            break;
                        case "Y0":
                            c[1] = e1 ; c[2] = y3 ; c[3] = z11 ; c[4] = 0 ; c[5] = y4 ; c[6] = 0 ; c[7] = 0 ; c[8] = 0 ; c[9] = z011 ; c[10] = 0 ; c[11] = 0;
                            break;
                        //2016.9.24 针对前后侧同时取某一侧的拔力新增,T表示某侧取相反侧上拔力计算
                        case "T0":
                            c[1] = e1 ; c[2] = y3 ; c[3] = z22 ; c[4] = 0 ; c[5] = y4 ; c[6] = 0 ; c[7] = 0 ; c[8] = 0 ; c[9] = z022 ; c[10] = 0 ; c[11] = 0;
                            break;
                        //20190113新版程序
                        default:
                            c[1] = e1 ; c[2] = y3 ; c[4] = 0 ; c[5] = 0 ; c[7] = 0 ; c[8] = 0 ; c[11] = 0;
                            if (Math.Abs(zhs) > 10000)
                            {
                                //垂荷列数互换法则补充
                                //'特意针对垂荷组合为12情况
                                //仅适用于不均匀冰和验算不均匀冰工况，其余工况不适用。在检查模式中判断报错

                                int zhs1 = Convert.ToInt16(Math.Abs(zhs).ToString().Substring(1, 2));
                                float zzz11 = GMaxF[j, zhs1] * BL2;
                                float zzz12 = GMinF[j, zhs1] * BL2;
                                float zzz21 = GMaxB[j, zhs1] * BL2;
                                float zzz22 = GMinB[j, zhs1] * BL2;

                                if (Math.Abs(zhs).ToString().Substring(0, 1) == "1")
                                {
                                    //前侧垂荷需要替换 万位数为1
                                    if (wd.VertialLoadCode.Substring(0, 1) == "0")
                                    {
                                        c[3] = 0; c[9] = 0;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                    {
                                        c[3] = zzz11; c[9] = z011;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                    {
                                        c[3] = zzz12; c[9] = z012;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                    {
                                        c[3] = zzz21; c[9] = z021;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                    {
                                        c[3] = zzz22; c[9] = z022;
                                    }
                                    else
                                    {
                                        throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                    }

                                    if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                    {
                                        c[6] = 0; c[10] = 0;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                    {
                                        c[6] = z11; c[10] = z011;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                    {
                                        c[6] = z12; c[10] = z012;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                    {
                                        c[6] = z21; c[10] = z021;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                    {
                                        c[6] = z22; c[10] = z022;
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
                                        c[3] = 0; c[9] = 0;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                    {
                                        c[3] = z11; c[9] = z011;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                    {
                                        c[3] = z12; c[9] = z012;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                    {
                                        c[3] = z21; c[9] = z021;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                    {
                                        c[3] = z22; c[9] = z022;
                                    }
                                    else
                                    {
                                        throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                    }

                                    if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                    {
                                        c[6] = 0; c[10] = 0;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                    {
                                        c[6] = zzz11; c[10] = z011;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                    {
                                        c[6] = zzz12; c[10] = z012;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                    {
                                        c[6] = zzz21; c[10] = z021;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                    {
                                        c[6] = zzz22; c[10] = z022;
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
                                    c[3] = 0; c[9] = 0;
                                }
                                else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                {
                                    c[3] = z11; c[9] = z011;
                                }
                                else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                {
                                    c[3] = z12; c[9] = z012;
                                }
                                else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                {
                                    c[3] = z21; c[9] = z021;
                                }
                                else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                {
                                    c[3] = z22; c[9] = z022;
                                }
                                else
                                {
                                    throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                }

                                if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                {
                                    c[6] = 0; c[10] = 0;
                                }
                                else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                {
                                    c[6] = z11; c[10] = z011;
                                }
                                else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                {
                                    c[6] = z12; c[10] = z012;
                                }
                                else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                {
                                    c[6] = z21; c[10] = z021;
                                }
                                else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                {
                                    c[6] = z22; c[10] = z022;
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
                        //case "00":
                        //     c[1] = e1 ; c[2] = y3 ; c[3] = 0 ; c[4] = 0 ; c[5] = 0 ; c[6] = 0 ; c[7] = 0 ; c[8] = 0 ; c[9] = 0 ; c[10] = 0 ; c[11] = 0;
                        //break;
                        case "B0":
                            c[1] = e1 ; c[2] = y3 ; c[3] = z12 ; c[4] = 0 ; c[5] = y4 ; c[6] = 0 ; c[7] = 0 ; c[8] = 0 ; c[9] = z012 ; c[10] = 0 ; c[11] = 0;
                            break;
                        case "Y0":
                            c[1] = e1 ; c[2] = y3 ; c[3] = z11 ; c[4] = 0 ; c[5] = y4 ; c[6] = 0 ; c[7] = 0 ; c[8] = 0 ; c[9] = z011 ; c[10] = 0 ; c[11] = 0;
                            break;
                        //2016.9.24 针对前后侧同时取某一侧的拔力新增,T表示某侧取相反侧上拔力计算
                        case "T0":
                            c[1] = e1 ; c[2] = y3 ; c[3] = z22 ; c[4] = 0 ; c[5] = y4 ; c[6] = 0 ; c[7] = 0 ; c[8] = 0 ; c[9] = z022 ; c[10] = 0 ; c[11] = 0;
                            break;
                        //20190113新版程序
                        default:
                            c[1] = e1 ; c[2] = y3 ; c[4] = 0 ; c[5] = 0 ; c[7] = 0 ; c[8] = 0 ; c[11] = 0;
                            if (Math.Abs(zhs) > 10000)
                            {
                                //垂荷列数互换法则补充
                                //'特意针对垂荷组合为12情况
                                //仅适用于不均匀冰和验算不均匀冰工况，其余工况不适用。在检查模式中判断报错

                                int zhs1 = Convert.ToInt16(Math.Abs(zhs).ToString().Substring(1, 2));
                                float zzz11 = GMaxF[j, zhs1] * BL2;
                                float zzz12 = GMinF[j, zhs1] * BL2;
                                float zzz21 = GMaxB[j, zhs1] * BL2;
                                float zzz22 = GMinB[j, zhs1] * BL2;

                                if (Math.Abs(zhs).ToString().Substring(0, 1) == "1")
                                {
                                    //前侧垂荷需要替换 万位数为1
                                    if (wd.VertialLoadCode.Substring(0, 1) == "0")
                                    {
                                        c[3] = 0; c[9] = 0;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                    {
                                        c[3] = zzz11; c[9] = z011;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                    {
                                        c[3] = zzz12; c[9] = z012;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                    {
                                        c[3] = zzz21; c[9] = z021;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                    {
                                        c[3] = zzz22; c[9] = z022;
                                    }
                                    else
                                    {
                                        throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                    }

                                    if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                    {
                                        c[6] = 0; c[10] = 0;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                    {
                                        c[6] = z11; c[10] = z011;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                    {
                                        c[6] = z12; c[10] = z012;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                    {
                                        c[6] = z21; c[10] = z021;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                    {
                                        c[6] = z22; c[10] = z022;
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
                                        c[3] = 0; c[9] = 0;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                    {
                                        c[3] = z11; c[9] = z011;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                    {
                                        c[3] = z12; c[9] = z012;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                    {
                                        c[3] = z21; c[9] = z021;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                    {
                                        c[3] = z22; c[9] = z022;
                                    }
                                    else
                                    {
                                        throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                    }

                                    if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                    {
                                        c[6] = 0; c[10] = 0;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                    {
                                        c[6] = zzz11; c[10] = z011;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                    {
                                        c[6] = zzz12; c[10] = z012;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                    {
                                        c[6] = zzz21; c[10] = z021;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                    {
                                        c[6] = zzz22; c[10] = z022;
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
                                    c[3] = 0; c[9] = 0;
                                }
                                else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                {
                                    c[3] = z11; c[9] = z011;
                                }
                                else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                {
                                    c[3] = z12; c[9] = z012;
                                }
                                else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                {
                                    c[3] = z21; c[9] = z021;
                                }
                                else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                {
                                    c[3] = z22; c[9] = z022;
                                }
                                else
                                {
                                    throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                }

                                if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                {
                                    c[6] = 0; c[10] = 0;
                                }
                                else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                {
                                    c[6] = z11; c[10] = z011;
                                }
                                else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                {
                                    c[6] = z12; c[10] = z012;
                                }
                                else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                {
                                    c[6] = z21; c[10] = z021;
                                }
                                else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                {
                                    c[6] = z22; c[10] = z022;
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
                        y4 = LineParas.PortalTensionMax;
                    }

                    //按最严重情况考虑扭转 20170620新增  扭转相垂荷按最严重情况考虑
                    switch (wd.VertialLoadCode)
                    {
                        //case "00":
                        //c[1] = 0 ; c[2] = 0 ; c[3] = 0 ; c[4] = 0 ; c[5] = e2 ; c[6] = y4 ; c[7] = 0 ; c[8] = 0 ; c[9] = 0 ; c[10] = 0 ; c[11] = 0;
                        //break;
                        case "0B":
                            c[1] = 0 ; c[2] = 0 ; c[3] = 0 ; c[4] = e2 ; c[5] = y4 ; c[6] = z22 ; c[7] = 0 ; c[8] = 0 ; c[9] = 0 ; c[10] = z022 ; c[11] = 0;
                            break;
                        case "0Y":
                            c[1] = 0 ; c[2] = 0 ; c[3] = 0 ; c[4] = e2 ; c[5] = y4 ; c[6] = z21 ; c[7] = 0 ; c[8] = 0 ; c[9] = 0 ; c[10] = z012 ; c[11] = 0;
                            break;
                        //2016.9.24 针对前后侧同时取某一侧的拔力新增,T表示某侧取相反侧上拔力计算
                        case "0T":
                            c[1] = 0 ; c[2] = 0 ; c[3] = 0 ; c[4] = e2 ; c[5] = y4 ; c[6] = z12 ; c[7] = 0 ; c[8] = 0 ; c[9] = 0 ; c[10] = z012 ; c[11] = 0;
                            break;
                        //20190113新版程序
                        default:
                            c[1] = 0 ; c[2] = 0 ; c[4] = e2 ; c[5] = y4 ; c[7] = 0 ; c[8] = 0 ; c[11] = 0;
                            if (Math.Abs(zhs) > 10000)
                            {
                                //垂荷列数互换法则补充
                                //'特意针对垂荷组合为12情况
                                //仅适用于不均匀冰和验算不均匀冰工况，其余工况不适用。在检查模式中判断报错

                                int zhs1 = Convert.ToInt16(Math.Abs(zhs).ToString().Substring(1, 2));
                                float zzz11 = GMaxF[j, zhs1] * BL2;
                                float zzz12 = GMinF[j, zhs1] * BL2;
                                float zzz21 = GMaxB[j, zhs1] * BL2;
                                float zzz22 = GMinB[j, zhs1] * BL2;

                                if (Math.Abs(zhs).ToString().Substring(0, 1) == "1")
                                {
                                    //前侧垂荷需要替换 万位数为1
                                    if (wd.VertialLoadCode.Substring(0, 1) == "0")
                                    {
                                        c[3] = 0; c[9] = 0;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                    {
                                        c[3] = zzz11; c[9] = z011;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                    {
                                        c[3] = zzz12; c[9] = z012;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                    {
                                        c[3] = zzz21; c[9] = z021;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                    {
                                        c[3] = zzz22; c[9] = z022;
                                    }
                                    else
                                    {
                                        throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                    }

                                    if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                    {
                                        c[6] = 0; c[10] = 0;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                    {
                                        c[6] = z11; c[10] = z011;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                    {
                                        c[6] = z12; c[10] = z012;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                    {
                                        c[6] = z21; c[10] = z021;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                    {
                                        c[6] = z22; c[10] = z022;
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
                                        c[3] = 0; c[9] = 0;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                    {
                                        c[3] = z11; c[9] = z011;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                    {
                                        c[3] = z12; c[9] = z012;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                    {
                                        c[3] = z21; c[9] = z021;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                    {
                                        c[3] = z22; c[9] = z022;
                                    }
                                    else
                                    {
                                        throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                    }

                                    if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                    {
                                        c[6] = 0; c[10] = 0;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                    {
                                        c[6] = zzz11; c[10] = z011;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                    {
                                        c[6] = zzz12; c[10] = z012;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                    {
                                        c[6] = zzz21; c[10] = z021;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                    {
                                        c[6] = zzz22; c[10] = z022;
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
                                    c[3] = 0; c[9] = 0;
                                }
                                else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                {
                                    c[3] = z11; c[9] = z011;
                                }
                                else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                {
                                    c[3] = z12; c[9] = z012;
                                }
                                else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                {
                                    c[3] = z21; c[9] = z021;
                                }
                                else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                {
                                    c[3] = z22; c[9] = z022;
                                }
                                else
                                {
                                    throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                }

                                if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                {
                                    c[6] = 0; c[10] = 0;
                                }
                                else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                {
                                    c[6] = z11; c[10] = z011;
                                }
                                else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                {
                                    c[6] = z12; c[10] = z012;
                                }
                                else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                {
                                    c[6] = z21; c[10] = z021;
                                }
                                else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                {
                                    c[6] = z22; c[10] = z022;
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
                        y4 = LineParas.PortalTensionMin;
                    }

                    //按最严重情况考虑扭转 20170620新增  扭转相垂荷按最严重情况考虑
                    switch (wd.VertialLoadCode)
                    {
                        //case "00":
                        //c[1] = 0 ; c[2] = 0 ; c[3] = 0 ; c[4] = e2 ; c[5] = y4 ; c[6] = 0 ; c[7] = 0 ; c[8] = 0 ; c[9] = 0 ; c[10] = 0 ; c[11] = 0;
                        //break;
                        case "0B":
                            c[1] = 0 ; c[2] = 0 ; c[3] = 0 ; c[4] = e2 ; c[5] = y4 ; c[6] = z22 ; c[7] = 0 ; c[8] = 0 ; c[9] = 0 ; c[10] = z022 ; c[11] = 0;
                            break;
                        case "0Y":
                            c[1] = 0 ; c[2] = 0 ; c[3] = 0 ; c[4] = e2 ; c[5] = y4 ; c[6] = z21 ; c[7] = 0 ; c[8] = 0 ; c[9] = 0 ; c[10] = z012 ; c[11] = 0;
                            break;
                        //2016.9.24 针对前后侧同时取某一侧的拔力新增,T表示某侧取相反侧上拔力计算
                        case "0T":
                            c[1] = 0 ; c[2] = 0 ; c[3] = 0 ; c[4] = e2 ; c[5] = y4 ; c[6] = z12 ; c[7] = 0 ; c[8] = 0 ; c[9] = 0 ; c[10] = z012 ; c[11] = 0;
                            break;
                        //20190113新版程序
                        default:
                            c[1] = 0 ; c[2] = 0 ; c[4] = e2 ; c[5] = y4 ; c[7] = 0 ; c[8] = 0 ; c[11] = 0;
                            if (Math.Abs(zhs) > 10000)
                            {
                                //垂荷列数互换法则补充
                                //'特意针对垂荷组合为12情况
                                //仅适用于不均匀冰和验算不均匀冰工况，其余工况不适用。在检查模式中判断报错

                                int zhs1 = Convert.ToInt16(Math.Abs(zhs).ToString().Substring(1, 2));
                                float zzz11 = GMaxF[j, zhs1] * BL2;
                                float zzz12 = GMinF[j, zhs1] * BL2;
                                float zzz21 = GMaxB[j, zhs1] * BL2;
                                float zzz22 = GMinB[j, zhs1] * BL2;

                                if (Math.Abs(zhs).ToString().Substring(0, 1) == "1")
                                {
                                    //前侧垂荷需要替换 万位数为1
                                    if (wd.VertialLoadCode.Substring(0, 1) == "0")
                                    {
                                        c[3] = 0; c[9] = 0;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                    {
                                        c[3] = zzz11; c[9] = z011;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                    {
                                        c[3] = zzz12; c[9] = z012;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                    {
                                        c[3] = zzz21; c[9] = z021;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                    {
                                        c[3] = zzz22; c[9] = z022;
                                    }
                                    else
                                    {
                                        throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                    }

                                    if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                    {
                                        c[6] = 0; c[10] = 0;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                    {
                                        c[6] = z11; c[10] = z011;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                    {
                                        c[6] = z12; c[10] = z012;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                    {
                                        c[6] = z21; c[10] = z021;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                    {
                                        c[6] = z22; c[10] = z022;
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
                                        c[3] = 0; c[9] = 0;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                    {
                                        c[3] = z11; c[9] = z011;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                    {
                                        c[3] = z12; c[9] = z012;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                    {
                                        c[3] = z21; c[9] = z021;
                                    }
                                    else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                    {
                                        c[3] = z22; c[9] = z022;
                                    }
                                    else
                                    {
                                        throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                    }

                                    if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                    {
                                        c[6] = 0; c[10] = 0;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                    {
                                        c[6] = zzz11; c[10] = z011;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                    {
                                        c[6] = zzz12; c[10] = z012;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                    {
                                        c[6] = zzz21; c[10] = z021;
                                    }
                                    else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                    {
                                        c[6] = zzz22; c[10] = z022;
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
                                    c[3] = 0; c[9] = 0;
                                }
                                else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                                {
                                    c[3] = z11; c[9] = z011;
                                }
                                else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                                {
                                    c[3] = z12; c[9] = z012;
                                }
                                else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                                {
                                    c[3] = z21; c[9] = z021;
                                }
                                else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                                {
                                    c[3] = z22; c[9] = z022;
                                }
                                else
                                {
                                    throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-15");
                                }

                                if (wd.VertialLoadCode.Substring(1, 1) == "0")
                                {
                                    c[6] = 0; c[10] = 0;
                                }
                                else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                                {
                                    c[6] = z11; c[10] = z011;
                                }
                                else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                                {
                                    c[6] = z12; c[10] = z012;
                                }
                                else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                                {
                                    c[6] = z21; c[10] = z021;
                                }
                                else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                                {
                                    c[6] = z22; c[10] = z022;
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
                    e3 = LineParas.AngleFront;
                }

                XLF[i, j] = formula.ZIBUYX(out string strXLF, angle, e3, c[1], c[2], e5, Vloadx, BL3);
                YLF[i, j] = formula.ZIBUYY(out string strYLF, angle, e3, c[1], c[2], e5, Vloadx);
                ZLF[i, j] = formula.ZIBUYZ(out string strZLF, c[9], c[3], e5, Vloadx, rg);

                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fx= " + strXLF);
                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fy= " + strYLF);
                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fz= " + strZLF);

                if (Paras.IsBranchTower)
                {
                    //前侧转角  20161124分支塔添加
                    e3 = LineParas.AngleBack;
                }

                XLB[i, j] = formula.ZIBUYX(out string strXLB, angle, e3, c[4], c[5], e5, Vloadx, BL3);
                YLB[i, j] = formula.ZIBUYY(out string strYLB, angle, e3, c[4], -c[5], e5, Vloadx);
                ZLB[i, j] = formula.ZIBUYZ(out string strZLB, c[10], c[6], e5, Vloadx, rg);

                ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fx= " + strXLB);
                ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fy= " + strYLB);
                ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fz= " + strZLB);

                if (nt % 3 != 0 || nt == 0)
                {
                    //跳线串始终为一种计算方法
                    XTF[i, j] = formula.ZIBUYX(out string strXTF, angle, 0, c[7], 0, e5, Vloadx, BL3, nt, Ratio.BLTQ);
                    YTF[i, j] = formula.ZIBUYY(out string strYTF, angle, 0, c[7], 0, e5, Vloadx, nt, Ratio.BLTQ);
                    ZTF[i, j] = formula.ZIBUYZ(out string strZTF, c[11], c[8], e5, Vloadx, rg, nt, Ratio.BLTQ);

                    ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fx= " + strXTF);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fy= " + strYTF);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fz= " + strZTF);

                    XTB[i, j] = formula.ZIBUYX(out string strXTB, angle, 0, c[7], 0, e5, Vloadx, BL3, nt, Ratio.BLTH);
                    YTB[i, j] = formula.ZIBUYY(out string strYTB, angle, 0, c[7], 0, e5, Vloadx, nt, Ratio.BLTH);
                    ZTB[i, j] = formula.ZIBUYZ(out string strZTB, c[11], c[8], e5, Vloadx, rg, nt, Ratio.BLTH);

                    ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= " + strXTB);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= " + strYTB);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= " + strZTB);
                }
                else
                {
                    XTF[i, j] = formula.ZIBUYX(out string strXTF, angle, 0, c[7], 0, e5, Vloadx, BL3, nt, Ratio.BLTQ);
                    YTF[i, j] = formula.ZIBUYY(out string strYTF, angle, 0, c[7], 0, e5, Vloadx, nt, Ratio.BLTQ);
                    ZTF[i, j] = formula.ZIBUYZ(out string strZTF, c[11], c[8], e5, Vloadx, rg, nt, Ratio.BLTQ);
                    
                    ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fx= " + strXTF);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fy= " + strYTF);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fz= " + strZTF);

                    XTC[i, j] = formula.ZIBUYX(out string strXTC, angle, 0, c[7], 0, e5, Vloadx, BL3, nt, Ratio.BLTZ);
                    YTC[i, j] = formula.ZIBUYY(out string strYTC, angle, 0, c[7], 0, e5, Vloadx, nt, Ratio.BLTZ);
                    ZTC[i, j] = formula.ZIBUYZ(out string strZTC, c[11], c[8], e5, Vloadx, rg, nt, Ratio.BLTZ);

                    ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fx= " + strXTC);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fy= " + strYTC);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fz= " + strZTC);

                    XTB[i, j] = formula.ZIBUYX(out string strXTB, angle, 0, c[7], 0, e5, Vloadx, BL3, nt, Ratio.BLTH);
                    YTB[i, j] = formula.ZIBUYY(out string strYTB, angle, 0, c[7], 0, e5, Vloadx, nt, Ratio.BLTH);
                    ZTB[i, j] = formula.ZIBUYZ(out string strZTB, c[11], c[8], e5, Vloadx, rg, nt, Ratio.BLTH);

                    ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= " + strXTB);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= " + strYTB);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= " + strZTB);
                }
            }
            else if(zhs == 0)
            {
                //该向无荷载
                BL2 = 0;
                XLF[i, j] = 0.00f;
                YLF[i, j] = 0.00f;
                ZLF[i, j] = 0.00f;

                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fx= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fy= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fz= 0.00");

                XLB[i, j] = 0.00f;
                YLB[i, j] = 0.00f;
                ZLB[i, j] = 0.00f;

                ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fx= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fy= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fz= 0.00");

                //跳线串始终为一种计算方法
                if (nt % 3 != 0 || nt == 0)
                {
                    XTF[i, j] = 0.00f;
                    YTF[i, j] = 0.00f;
                    ZTF[i, j] = 0.00f;

                    ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fx= 0.00");
                    ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fy= 0.00");
                    ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fz= 0.00");

                    XTB[i, j] = 0.00f;
                    YTB[i, j] = 0.00f;
                    ZTB[i, j] = 0.00f;

                    ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= 0.00");
                    ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= 0.00");
                    ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= 0.00");
                }
                else
                {
                    XTF[i, j] = 0.00f;
                    YTF[i, j] = 0.00f;
                    ZTF[i, j] = 0.00f;

                    ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fx= 0.00");
                    ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fy= 0.00");
                    ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fz= 0.00");

                    XTC[i, j] = 0.00f;
                    YTC[i, j] = 0.00f;
                    ZTC[i, j] = 0.00f;

                    ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fx= 0.00");
                    ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fy= 0.00");
                    ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fz= 0.00");

                    XTB[i, j] = 0.00f;
                    YTB[i, j] = 0.00f;
                    ZTB[i, j] = 0.00f;

                    ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= 0.00");
                    ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= 0.00");
                    ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= 0.00");
                }
            }
        }

        /// <summary>
        /// 针对脱冰跳跃工况
        /// 工况代码"L1","L2","L1a","L2a","L1b","L2b",
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        protected void DistributeInLift(int i, int j)
        {
            float x11, x12, x21, x22, x3;
            float y1, y2, y3, y4;
            float z11, z12, z21, z22, z3;
            float e1, e2, e3;
            float[] c = new float[11];
            int BL2;
            int fhn;

            WorkConditionCombo wd = Template.WorkConditionCombos[i];

            int zhs = wd.WireIndexCodes[j-1], zhsx = zhs % 100;
            int angle = wd.WindDirectionCode;
            string workConditionCode = wd.WorkConditionCode;
            int mz1 = Template.WorkConditongs.Count;
            int BL3 = LineParas.isTurnRight ? 1 : -1;
            //跳线数 = 导线数
            int nt = Template.Wires.Where(item => item.Contains("导")).Count();
            int dxl = Template.Wires.Where(item => item.Contains("地")).Count();
            float fh_2 = LineParas.TwireExtraLoad / Paras.LoadRatio;

            BL2 = 1;
            y1 = TensionD[j, zhsx] * BL2;
            y2 = TensionX[j, zhsx] * BL2;
            x11 = WindDF[j, zhsx] * BL2;
            x12 = WindXF[j, zhsx] * BL2;
            x21 = WindDB[j, zhsx] * BL2;
            x22 = WindXB[j, zhsx] * BL2;
            x3 = WindTX[j, zhsx] * BL2;
            z11 = GMaxF[j, zhsx] * BL2;
            z12 = GMinF[j, zhsx] * BL2;
            z21 = GMaxB[j, zhsx] * BL2;
            z22 = GMinB[j, zhsx] * BL2;
            z3 = GTX[j, zhsx] * BL2;

            if (wd.WorkConditionCode == "L1" || wd.WorkConditionCode == "L1a" || wd.WorkConditionCode == "L1b")
            {
                e1 = x11 ; e2 = x21 ; e3 = LineParas.AngleMax;
            }
            else
            {
                e1 = x12 ; e2 = x22 ; e3 = LineParas.AngleMin;
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
                    y4 = LineParas.PortalTensionMax;
                }

                //按最严重情况考虑扭转 20170620新增  扭转相垂荷按最严重情况考虑
                switch (wd.VertialLoadCode)
                {
                    case "YY":
                        c[1] = e1 ; c[2] = y3 ; c[3] = z11 ; c[4] = e2 ; c[5] = y4 ; c[6] = z21 ; c[7] = x3 ; c[8] = z3;
                        break;
                    case "YB":
                        c[1] = e1 ; c[2] = y3 ; c[3] = z11 ; c[4] = e2 ; c[5] = y4 ; c[6] = z22 ; c[7] = x3 ; c[8] = z3;
                        break;
                    case "0B":
                        c[1] = e1 ; c[2] = y3 ; c[3] = 0 ; c[4] = e2 ; c[5] = y4 ; c[6] = z22 ; c[7] = x3 ; c[8] = z3;
                        break;
                    case "B0": 
                        c[1] = e1 ; c[2] = y3 ; c[3] = z12 ; c[4] = e2 ; c[5] = y4 ; c[6] = 0 ; c[7] = x3 ; c[8] = z3;
                        break;
                    case "Y0":
                        c[1] = e1 ; c[2] = y3 ; c[3] = z11 ; c[4] = e2 ; c[5] = y4 ; c[6] = 0 ; c[7] = x3 ; c[8] = z3;
                        break;
                    case "BB":
                        c[1] = e1 ; c[2] = y3 ; c[3] = z12 ; c[4] = e2 ; c[5] = y4 ; c[6] = z22 ; c[7] = x3 ; c[8] = z3;
                        break;
                    case "BY":
                        c[1] = e1 ; c[2] = y3 ; c[3] = z12 ; c[4] = e2 ; c[5] = y4 ; c[6] = z21 ; c[7] = x3 ; c[8] = z3;
                        break;
                    case "0Y":
                        c[1] = e1 ; c[2] = y3 ; c[3] = 0 ; c[4] = e2 ; c[5] = y4 ; c[6] = z21 ; c[7] = x3 ; c[8] = z3;
                        break;
                    //case "00":
                    // c[1] = e1 ; c[2] = y3 ; c[3] = 0 ; c[4] = e2 ; c[5] = y4 ; c[6] = 0 ; c[7] = x3 ; c[8] = z3;
                    // break;
                    //2016.9.24 针对前后侧同时取某一侧的拔力新增,T表示某侧取相反侧上拔力计算
                    case "YT":
                        c[1] = e1 ; c[2] = y3 ; c[3] = z11 ; c[4] = e2 ; c[5] = y4 ; c[6] = z12 ; c[7] = x3 ; c[8] = z3;
                        break;
                    case "0T":
                        c[1] = e1 ; c[2] = y3 ; c[3] = 0 ; c[4] = e2 ; c[5] = y4 ; c[6] = z12 ; c[7] = x3 ; c[8] = z3;
                        break;
                    case "T0":
                        c[1] = e1 ; c[2] = y3 ; c[3] = z22 ; c[4] = e2 ; c[5] = y4 ; c[6] = 0 ; c[7] = x3 ; c[8] = z3;
                        break;
                    case "TY":
                        c[1] = e1 ; c[2] = y3 ; c[3] = z22 ; c[4] = e2 ; c[5] = y4 ; c[6] = z21 ; c[7] = x3 ; c[8] = z3;
                        break;
                    //20190113新版程序
                    default:
                        c[1] = e1 ; c[2] = y3 ; c[4] = e2 ; c[5] = y4 ; c[7] = x3 ; c[8] = z3;
                        if (wd.VertialLoadCode.Substring(0, 1) == "0")
                        {
                            c[3] = 0;
                        }
                        else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                        {
                            c[3] = z11;
                        }
                        else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                        {
                            c[3] = z12;
                        }
                        else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                        {
                            c[3] = z21;
                        }
                        else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                        {
                            c[3] = z22;
                        }
                        else
                        {
                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-25");
                        }

                        if (wd.VertialLoadCode.Substring(1, 1) == "0")
                        {
                            c[6] = 0;
                        }
                        else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                        {
                            c[6] = z11;
                        }
                        else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                        {
                            c[6] = z12;
                        }
                        else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                        {
                            c[6] = z21;
                        }
                        else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                        {
                            c[6] = z22;
                        }
                        else
                        {
                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-16");
                        }

                        break;
                }
            }
            else if (wd.TensionAngleCode == "DX")
            {
                if (Paras.IsCornerTower || Paras.IsBranchTower)
                {
                    y3 = y1;
                    y4 = y2;
                }
                else
                {
                    y3 = y1;
                    y4 = LineParas.PortalTensionMin;
                }

                //按最严重情况考虑扭转 20170620新增  扭转相垂荷按最严重情况考虑
                switch (wd.VertialLoadCode)
                {
                    case "YY":
                        c[1] = e1 ; c[2] = y3 ; c[3] = z11 ; c[4] = e2 ; c[5] = y4 ; c[6] = z21 ; c[7] = x3 ; c[8] = z3;
                        break;
                    case "YB":
                        c[1] = e1 ; c[2] = y3 ; c[3] = z11 ; c[4] = e2 ; c[5] = y4 ; c[6] = z22 ; c[7] = x3 ; c[8] = z3;
                        break;
                    case "0B":
                        c[1] = e1 ; c[2] = y3 ; c[3] = 0 ; c[4] = e2 ; c[5] = y4 ; c[6] = z22 ; c[7] = x3 ; c[8] = z3;
                        break;
                    case "B0":
                        c[1] = e1 ; c[2] = y3 ; c[3] = z12 ; c[4] = e2 ; c[5] = y4 ; c[6] = 0 ; c[7] = x3 ; c[8] = z3;
                        break;
                    case "Y0":
                        c[1] = e1 ; c[2] = y3 ; c[3] = z11 ; c[4] = e2 ; c[5] = y4 ; c[6] = 0 ; c[7] = x3 ; c[8] = z3;
                        break;
                    case "BB":
                        c[1] = e1 ; c[2] = y3 ; c[3] = z12 ; c[4] = e2 ; c[5] = y4 ; c[6] = z22 ; c[7] = x3 ; c[8] = z3;
                        break;
                    case "BY":
                        c[1] = e1 ; c[2] = y3 ; c[3] = z12 ; c[4] = e2 ; c[5] = y4 ; c[6] = z21 ; c[7] = x3 ; c[8] = z3;
                        break;
                    case "0Y":
                        c[1] = e1 ; c[2] = y3 ; c[3] = 0 ; c[4] = e2 ; c[5] = y4 ; c[6] = z21 ; c[7] = x3 ; c[8] = z3;
                        break;
                    case "00":
                        c[1] = e1 ; c[2] = y3 ; c[3] = 0 ; c[4] = e2 ; c[5] = y4 ; c[6] = 0 ; c[7] = x3 ; c[8] = z3;
                        break;
                    //2016.9.24 针对前后侧同时取某一侧的拔力新增,T表示某侧取相反侧上拔力计算
                    case "YT":
                        c[1] = e1 ; c[2] = y3 ; c[3] = z11 ; c[4] = e2 ; c[5] = y4 ; c[6] = z12 ; c[7] = x3 ; c[8] = z3;
                        break;
                    case "0T":
                        c[1] = e1 ; c[2] = y3 ; c[3] = 0 ; c[4] = e2 ; c[5] = y4 ; c[6] = z12 ; c[7] = x3 ; c[8] = z3;
                        break;
                    case "T0":
                        c[1] = e1 ; c[2] = y3 ; c[3] = z22 ; c[4] = e2 ; c[5] = y4 ; c[6] = 0 ; c[7] = x3 ; c[8] = z3;
                        break;
                    case "TY":
                        c[1] = e1 ; c[2] = y3 ; c[3] = z22 ; c[4] = e2 ; c[5] = y4 ; c[6] = z21 ; c[7] = x3 ; c[8] = z3;
                        break;
                    //20190113新版程序
                    default:
                        c[1] = e1 ; c[2] = y3 ; c[4] = e2 ; c[5] = y4 ; c[7] = x3 ; c[8] = z3;
                        if (wd.VertialLoadCode.Substring(0, 1) == "0")
                        {
                            c[3] = 0;
                        }
                        else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                        {
                            c[3] = z11;
                        }
                        else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                        {
                            c[3] = z12;
                        }
                        else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                        {
                            c[3] = z21;
                        }
                        else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                        {
                            c[3] = z22;
                        }
                        else
                        {
                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-25");
                        }

                        if (wd.VertialLoadCode.Substring(1, 1) == "0")
                        {
                            c[6] = 0;
                        }
                        else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                        {
                            c[6] = z11;
                        }
                        else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                        {
                            c[6] = z12;
                        }
                        else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                        {
                            c[6] = z21;
                        }
                        else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                        {
                            c[6] = z22;
                        }
                        else
                        {
                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-16");
                        }

                        break;
                }
            }
            else if (wd.TensionAngleCode == "XD")
            {
                if (Paras.IsCornerTower || Paras.IsBranchTower)
                {
                    y3 = y2;
                    y4 = y1;
                }
                else
                {
                    y3 = y2;
                    y4 = LineParas.PortalTensionMax;
                }

                //按最严重情况考虑扭转 20170620新增  扭转相垂荷按最严重情况考虑
                switch (wd.VertialLoadCode)
                {
                    case "YY":
                        c[1] = e1 ; c[2] = y3 ; c[3] = z11 ; c[4] = e2 ; c[5] = y4 ; c[6] = z21 ; c[7] = x3 ; c[8] = z3;
                        break;
                    case "YB":
                        c[1] = e1 ; c[2] = y3 ; c[3] = z11 ; c[4] = e2 ; c[5] = y4 ; c[6] = z22 ; c[7] = x3 ; c[8] = z3;
                        break;
                    case "0B":
                        c[1] = e1 ; c[2] = y3 ; c[3] = 0 ; c[4] = e2 ; c[5] = y4 ; c[6] = z22 ; c[7] = x3 ; c[8] = z3;
                        break;
                    case "B0":
                        c[1] = e1 ; c[2] = y3 ; c[3] = z12 ; c[4] = e2 ; c[5] = y4 ; c[6] = 0 ; c[7] = x3 ; c[8] = z3;
                        break;
                    case "Y0":
                        c[1] = e1 ; c[2] = y3 ; c[3] = z11 ; c[4] = e2 ; c[5] = y4 ; c[6] = 0 ; c[7] = x3 ; c[8] = z3;
                        break;
                    case "BB":
                        c[1] = e1 ; c[2] = y3 ; c[3] = z12 ; c[4] = e2 ; c[5] = y4 ; c[6] = z22 ; c[7] = x3 ; c[8] = z3;
                        break;
                    case "BY":
                        c[1] = e1 ; c[2] = y3 ; c[3] = z12 ; c[4] = e2 ; c[5] = y4 ; c[6] = z21 ; c[7] = x3 ; c[8] = z3;
                        break;
                    case "0Y":
                        c[1] = e1 ; c[2] = y3 ; c[3] = 0 ; c[4] = e2 ; c[5] = y4 ; c[6] = z21 ; c[7] = x3 ; c[8] = z3;
                        break;
                    case "00":
                        c[1] = e1 ; c[2] = y3 ; c[3] = 0 ; c[4] = e2 ; c[5] = y4 ; c[6] = 0 ; c[7] = x3 ; c[8] = z3;
                        break;
                    //2016.9.24 针对前后侧同时取某一侧的拔力新增,T表示某侧取相反侧上拔力计算
                    case "YT":
                        c[1] = e1 ; c[2] = y3 ; c[3] = z11 ; c[4] = e2 ; c[5] = y4 ; c[6] = z12 ; c[7] = x3 ; c[8] = z3;
                        break;
                    case "0T":
                        c[1] = e1 ; c[2] = y3 ; c[3] = 0 ; c[4] = e2 ; c[5] = y4 ; c[6] = z12 ; c[7] = x3 ; c[8] = z3;
                        break;
                    case "T0":
                        c[1] = e1 ; c[2] = y3 ; c[3] = z22 ; c[4] = e2 ; c[5] = y4 ; c[6] = 0 ; c[7] = x3 ; c[8] = z3;
                        break;
                    case "TY":
                        c[1] = e1 ; c[2] = y3 ; c[3] = z22 ; c[4] = e2 ; c[5] = y4 ; c[6] = z21 ; c[7] = x3 ; c[8] = z3;
                        break;
                    //20190113新版程序
                    default:
                        c[1] = e1 ; c[2] = y3 ; c[4] = e2 ; c[5] = y4 ; c[7] = x3 ; c[8] = z3;
                        if (wd.VertialLoadCode.Substring(0, 1) == "0")
                        {
                            c[3] = 0;
                        }
                        else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                        {
                            c[3] = z11;
                        }
                        else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                        {
                            c[3] = z12;
                        }
                        else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                        {
                            c[3] = z21;
                        }
                        else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                        {
                            c[3] = z22;
                        }
                        else
                        {
                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-25");
                        }

                        if (wd.VertialLoadCode.Substring(1, 1) == "0")
                        {
                            c[6] = 0;
                        }
                        else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                        {
                            c[6] = z11;
                        }
                        else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                        {
                            c[6] = z12;
                        }
                        else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                        {
                            c[6] = z21;
                        }
                        else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                        {
                            c[6] = z22;
                        }
                        else
                        {
                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-16");
                        }

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
                    y4 = LineParas.PortalTensionMin;
                }

                //按最严重情况考虑扭转 20170620新增  扭转相垂荷按最严重情况考虑
                switch (wd.VertialLoadCode)
                {
                    case "YY":
                        c[1] = e1 ; c[2] = y3 ; c[3] = z11 ; c[4] = e2 ; c[5] = y4 ; c[6] = z21 ; c[7] = x3 ; c[8] = z3;
                        break;
                    case "YB":
                        c[1] = e1 ; c[2] = y3 ; c[3] = z11 ; c[4] = e2 ; c[5] = y4 ; c[6] = z22 ; c[7] = x3 ; c[8] = z3;
                        break;
                    case "0B":
                        c[1] = e1 ; c[2] = y3 ; c[3] = 0 ; c[4] = e2 ; c[5] = y4 ; c[6] = z22 ; c[7] = x3 ; c[8] = z3;
                        break;
                    case "B0":
                        c[1] = e1 ; c[2] = y3 ; c[3] = z12 ; c[4] = e2 ; c[5] = y4 ; c[6] = 0 ; c[7] = x3 ; c[8] = z3;
                        break;
                    case "Y0":
                        c[1] = e1 ; c[2] = y3 ; c[3] = z11 ; c[4] = e2 ; c[5] = y4 ; c[6] = 0 ; c[7] = x3 ; c[8] = z3;
                        break;
                    case "BB":
                        c[1] = e1 ; c[2] = y3 ; c[3] = z12 ; c[4] = e2 ; c[5] = y4 ; c[6] = z22 ; c[7] = x3 ; c[8] = z3;
                        break;
                    case "BY":
                        c[1] = e1 ; c[2] = y3 ; c[3] = z12 ; c[4] = e2 ; c[5] = y4 ; c[6] = z21 ; c[7] = x3 ; c[8] = z3;
                        break;
                    case "0Y":
                        c[1] = e1 ; c[2] = y3 ; c[3] = 0 ; c[4] = e2 ; c[5] = y4 ; c[6] = z21 ; c[7] = x3 ; c[8] = z3;
                        break;
                    case "00":
                        c[1] = e1 ; c[2] = y3 ; c[3] = 0 ; c[4] = e2 ; c[5] = y4 ; c[6] = 0 ; c[7] = x3 ; c[8] = z3;
                        break;
                    //2016.9.24 针对前后侧同时取某一侧的拔力新增,T表示某侧取相反侧上拔力计算
                    case "YT":
                        c[1] = e1 ; c[2] = y3 ; c[3] = z11 ; c[4] = e2 ; c[5] = y4 ; c[6] = z12 ; c[7] = x3 ; c[8] = z3;
                        break;
                    case "0T":
                        c[1] = e1 ; c[2] = y3 ; c[3] = 0 ; c[4] = e2 ; c[5] = y4 ; c[6] = z12 ; c[7] = x3 ; c[8] = z3;
                        break;
                    case "T0":
                        c[1] = e1 ; c[2] = y3 ; c[3] = z22 ; c[4] = e2 ; c[5] = y4 ; c[6] = 0 ; c[7] = x3 ; c[8] = z3;
                        break;
                    case "TY":
                        c[1] = e1 ; c[2] = y3 ; c[3] = z22 ; c[4] = e2 ; c[5] = y4 ; c[6] = z21 ; c[7] = x3 ; c[8] = z3;
                        break;
                    //20190113新版程序
                    default:
                        c[1] = e1 ; c[2] = y3 ; c[4] = e2 ; c[5] = y4 ; c[7] = x3 ; c[8] = z3;
                        if (wd.VertialLoadCode.Substring(0, 1) == "0")
                        {
                            c[3] = 0;
                        }
                        else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                        {
                            c[3] = z11;
                        }
                        else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                        {
                            c[3] = z12;
                        }
                        else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                        {
                            c[3] = z21;
                        }
                        else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                        {
                            c[3] = z22;
                        }
                        else
                        {
                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-25");
                        }

                        if (wd.VertialLoadCode.Substring(1, 1) == "0")
                        {
                            c[6] = 0;
                        }
                        else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                        {
                            c[6] = z11;
                        }
                        else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                        {
                            c[6] = z12;
                        }
                        else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                        {
                            c[6] = z21;
                        }
                        else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                        {
                            c[6] = z22;
                        }
                        else
                        {
                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-16");
                        }

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
                        c[1] = e1 ; c[2] = y3 ; c[3] = z12 ; c[4] = 0 ; c[5] = y4 ; c[6] = 0 ; c[7] = 0 ; c[8] = 0;
                        break;
                    case "Y0":
                        c[1] = e1 ; c[2] = y3 ; c[3] = z11 ; c[4] = 0 ; c[5] = y4 ; c[6] = 0 ; c[7] = 0 ; c[8] = 0;
                        break;
                    //case "00":
                    //c[1] = 0 ; c[2] = 0 ; c[3] = 0 ; c[4] = 0 ; c[5] = y4 ; c[6] = 0 ; c[7] = 0 ; c[8] = 0;
                    //break;
                    //2016.9.24 针对前后侧同时取某一侧的拔力新增,T表示某侧取相反侧上拔力计算
                    case "T0":
                        c[1] = e1 ; c[2] = y3 ; c[3] = z22 ; c[4] = 0 ; c[5] = y4 ; c[6] = 0 ; c[7] = 0 ; c[8] = 0;
                        break;
                    //20190113新版程序
                    default:
                        c[1] = e1 ; c[2] = y3 ; c[4] = 0 ; c[5] = y4 ; c[7] = 0 ; c[8] = 0;
                        if (wd.VertialLoadCode.Substring(0, 1) == "0")
                        {
                            c[3] = 0;
                        }
                        else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                        {
                            c[3] = z11;
                        }
                        else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                        {
                            c[3] = z12;
                        }
                        else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                        {
                            c[3] = z21;
                        }
                        else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                        {
                            c[3] = z22;
                        }
                        else
                        {
                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-25");
                        }

                        if (wd.VertialLoadCode.Substring(1, 1) == "0")
                        {
                            c[6] = 0;
                        }
                        else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                        {
                            c[6] = z11;
                        }
                        else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                        {
                            c[6] = z12;
                        }
                        else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                        {
                            c[6] = z21;
                        }
                        else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                        {
                            c[6] = z22;
                        }
                        else
                        {
                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-16");
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

                switch (wd.VertialLoadCode)
                {
                    case "B0":
                        c[1] = e1 ; c[2] = y3 ; c[3] = z12 ; c[4] = 0 ; c[5] = y4 ; c[6] = 0 ; c[7] = 0 ; c[8] = 0;
                        break;
                    case "Y0":
                        c[1] = e1 ; c[2] = y3 ; c[3] = z11 ; c[4] = 0 ; c[5] = y4 ; c[6] = 0 ; c[7] = 0 ; c[8] = 0;
                        break;
                    //case "00":
                    //c[1] = 0 ; c[2] = 0 ; c[3] = 0 ; c[4] = 0 ; c[5] = 0 ; c[6] = 0 ; c[7] = 0 ; c[8] = 0;
                    //break;
                    //2016.9.24 针对前后侧同时取某一侧的拔力新增,T表示某侧取相反侧上拔力计算
                    case "T0":
                        c[1] = e1 ; c[2] = y3 ; c[3] = z22 ; c[4] = 0 ; c[5] = y4 ; c[6] = 0 ; c[7] = 0 ; c[8] = 0;
                        break;
                    //20190113新版程序
                    default:
                        c[1] = e1 ; c[2] = y3 ; c[4] = 0 ; c[5] = y4 ; c[7] = 0 ; c[8] = 0;
                        if (wd.VertialLoadCode.Substring(0, 1) == "0")
                        {
                            c[3] = 0;
                        }
                        else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                        {
                            c[3] = z11;
                        }
                        else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                        {
                            c[3] = z12;
                        }
                        else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                        {
                            c[3] = z21;
                        }
                        else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                        {
                            c[3] = z22;
                        }
                        else
                        {
                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-25");
                        }

                        if (wd.VertialLoadCode.Substring(1, 1) == "0")
                        {
                            c[6] = 0;
                        }
                        else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                        {
                            c[6] = z11;
                        }
                        else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                        {
                            c[6] = z12;
                        }
                        else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                        {
                            c[6] = z21;
                        }
                        else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                        {
                            c[6] = z22;
                        }
                        else
                        {
                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-16");
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
                    y4 = LineParas.PortalTensionMax;
                }

                switch (wd.VertialLoadCode)
                {
                    case "0B":
                        c[1] = 0 ; c[2] = y3 ; c[3] = 0 ; c[4] = e2 ; c[5] = y4 ; c[6] = z22 ; c[7] = 0 ; c[8] = 0;
                        break;
                    case "0Y":
                        c[1] = 0 ; c[2] = y3 ; c[3] = 0 ; c[4] = e2 ; c[5] = y4 ; c[6] = z21 ; c[7] = 0 ; c[8] = 0;
                        break;
                    //case "00":
                    //c[1] = 0 ; c[2] = 0 ; c[3] = 0 ; c[4] = 0 ; c[5] = 0 ; c[6] = 0 ; c[7] = 0 ; c[8] = 0;
                    //break;
                    //2016.9.24 针对前后侧同时取某一侧的拔力新增,T表示某侧取相反侧上拔力计算
                    case "0T":
                        c[1] = 0 ; c[2] = y3 ; c[3] = 0 ; c[4] = e2 ; c[5] = y4 ; c[6] = z12 ; c[7] = 0 ; c[8] = 0;
                        break;
                    default:
                        c[1] = 0 ; c[2] = y3 ; c[4] = e2 ; c[5] = y4 ; c[7] = 0 ; c[8] = 0;
                        if (wd.VertialLoadCode.Substring(0, 1) == "0")
                        {
                            c[3] = 0;
                        }
                        else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                        {
                            c[3] = z11;
                        }
                        else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                        {
                            c[3] = z12;
                        }
                        else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                        {
                            c[3] = z21;
                        }
                        else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                        {
                            c[3] = z22;
                        }
                        else
                        {
                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-25");
                        }

                        if (wd.VertialLoadCode.Substring(1, 1) == "0")
                        {
                            c[6] = 0;
                        }
                        else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                        {
                            c[6] = z11;
                        }
                        else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                        {
                            c[6] = z12;
                        }
                        else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                        {
                            c[6] = z21;
                        }
                        else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                        {
                            c[6] = z22;
                        }
                        else
                        {
                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-26");
                        }

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
                    y4 = LineParas.PortalTensionMin;
                }

                switch (wd.VertialLoadCode)
                {
                    case "0B":
                        c[1] = 0 ; c[2] = y3 ; c[3] = 0 ; c[4] = e2 ; c[5] = y4 ; c[6] = z22 ; c[7] = 0 ; c[8] = 0;
                        break;
                    case "0Y":
                        c[1] = 0 ; c[2] = y3 ; c[3] = 0 ; c[4] = e2 ; c[5] = y4 ; c[6] = z21 ; c[7] = 0 ; c[8] = 0;
                        break;
                    //case "00":
                    // c[1] = 0 ; c[2] = 0 ; c[3] = 0 ; c[4] = 0 ; c[5] = 0 ; c[6] = 0 ; c[7] = 0 ; c[8] = 0;
                    //break;
                    //2016.9.24 针对前后侧同时取某一侧的拔力新增,T表示某侧取相反侧上拔力计算
                    case "0T":
                        c[1] = 0 ; c[2] = y3 ; c[3] = 0 ; c[4] = e2 ; c[5] = y4 ; c[6] = z12 ; c[7] = 0 ; c[8] = 0;
                        break;
                    default:
                        c[1] = 0 ; c[2] = y3 ; c[4] = e2 ; c[5] = y4 ; c[7] = 0 ; c[8] = 0;

                        if (wd.VertialLoadCode.Substring(0, 1) == "0")
                        {
                            c[3] = 0;
                        }
                        else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                        {
                            c[3] = z11;
                        }
                        else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                        {
                            c[3] = z12;
                        }
                        else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                        {
                            c[3] = z21;
                        }
                        else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                        {
                            c[3] = z22;
                        }
                        else
                        {
                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-25");
                        }

                        if (wd.VertialLoadCode.Substring(1, 1) == "0")
                        {
                            c[6] = 0;
                        }
                        else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                        {
                            c[6] = z11;
                        }
                        else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                        {
                            c[6] = z12;
                        }
                        else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                        {
                            c[6] = z21;
                        }
                        else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                        {
                            c[6] = z22;
                        }
                        else
                        {
                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-26");
                        }

                        break;
                }
            }

            if(zhs == 0)
            {
                XLF[i, j] = 0.00f;
                YLF[i, j] = 0.00f;
                ZLF[i, j] = 0.00f;

                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fx= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fy= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fz= 0.00");

                XLB[i, j] = 0.00f;
                YLB[i, j] = 0.00f;
                ZLB[i, j] = 0.00f;

                ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fx= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fy= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fz= 0.00");
            }
            else
            {
                if(Paras.IsBranchTower)
                {
                    //前后转角  20161124分支塔添加
                    e3 = LineParas.AngleFront;
                }

                XLF[i, j] = formula.ZLX(out string strXLF, angle, e3, c[1], c[2], BL3);
                YLF[i, j] = formula.ZLY(out string strYLF, angle, e3, c[1], c[2]);
                ZLF[i, j] = formula.ZLZ(out string strZLF, c[3]);

                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fx= " + strXLF);
                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fy= " + strYLF);
                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fz= " + strZLF);

                if (Paras.IsBranchTower)
                {
                    //前后转角  20161124分支塔添加
                    e3 = LineParas.AngleBack;
                }

                XLB[i, j] = formula.ZLX(out string strXLB, angle, e3, c[4], c[5], BL3);
                YLB[i, j] = formula.ZLY(out string strYLB, angle, e3, c[4], -c[5]);
                ZLB[i, j] = formula.ZLZ(out string strZLB, c[6]);

                ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fx= " + strXLB);
                ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fy= " + strYLB);
                ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fz= " + strZLB);
            }

            if(zhs < 100 && zhs > 0)
            {
                //表该相跳线为不吊装，跳线荷载为0  i
                //中间吊装未吊
                if(nt % 3 != 0 || nt == 0)
                {
                    XTF[i, j] = 0.00f;
                    YTF[i, j] = 0.00f;
                    ZTF[i, j] = 0.00f;

                    XTB[i, j] = 0.00f;
                    YTB[i, j] = 0.00f;
                    ZTB[i, j] = 0.00f;

                    if (nt > 0)
                    {
                        ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fx= 0.00");
                        ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fy= 0.00");
                        ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fz= 0.00");

                        ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= 0.00");
                        ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= 0.00");
                        ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= 0.00");
                    }
                }
                else if(nt % 3 == 0 || nt > 0)
                {
                    XTF[i, j] = 0.00f;
                    YTF[i, j] = 0.00f;
                    ZTF[i, j] = 0.00f;

                    XTC[i, j] = 0.00f;
                    YTC[i, j] = 0.00f;
                    ZTC[i, j] = 0.00f;

                    XTB[i, j] = 0.00f;
                    YTB[i, j] = 0.00f;
                    ZTB[i, j] = 0.00f;

                    ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fx= 0.00");
                    ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fy= 0.00");
                    ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fz= 0.00");

                    ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fx= 0.00");
                    ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fy= 0.00");
                    ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fz= 0.00");

                    ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= 0.00");
                    ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= 0.00");
                    ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= 0.00");
                }

            }
            else if(zhs.ToString().Length == 5 && (zhs.ToString().Substring(0,3) == "200" || zhs.ToString().Substring(0, 3) == "100"))
            {
                //单侧吊装未吊  双I 双V                             
                if (nt % 3 != 0 || nt == 0)
                {
                    XTF[i, j] = 0.00f;
                    YTF[i, j] = 0.00f;
                    ZTF[i, j] = 0.00f;

                    XTB[i, j] = 0.00f;
                    YTB[i, j] = 0.00f;
                    ZTB[i, j] = 0.00f;

                    if (nt > 0)
                    {
                        ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fx= 0.00");
                        ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fy= 0.00");
                        ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fz= 0.00");

                        ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= 0.00");
                        ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= 0.00");
                        ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= 0.00");
                    }
                }
                else
                {
                    throw new Exception("线条" + j + "跳串数目与跳线吊装工况组合参数矛盾，请核实工况" + i + "0 + 16 " + "错误：1-231");
                }
            }
            else if (zhs.ToString().Length == 6 && (zhs.ToString().Substring(0, 4) == "3000" || zhs.ToString().Substring(0, 4) == "1000"))
            {
                //单侧吊装未吊  双I 双V                            
                if (nt % 3 != 0 || nt == 0)
                {
                    XTF[i, j] = 0.00f;
                    YTF[i, j] = 0.00f;
                    ZTF[i, j] = 0.00f;

                    XTC[i, j] = 0.00f;
                    YTC[i, j] = 0.00f;
                    ZTC[i, j] = 0.00f;

                    XTB[i, j] = 0.00f;
                    YTB[i, j] = 0.00f;
                    ZTB[i, j] = 0.00f;

                    if (nt % 3 == 0 && nt > 0)
                    {
                        ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fx= 0.00");
                        ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fy= 0.00");
                        ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fz= 0.00");

                        ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fx= 0.00");
                        ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fy= 0.00");
                        ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fz= 0.00");

                        ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= 0.00");
                        ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= 0.00");
                        ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= 0.00");
                    }
                }
                else
                {
                    throw new Exception("线条" + j + "跳串数目与跳线吊装工况组合参数矛盾，请核实工况" + i + "0 + 16 " + "错误：1-232");
                }
            }
            else if (zhs > 1000 && zhs < 2000 && zhs.ToString().Substring(0, 1) == "1")
            {
                fhn = 1;

                //单侧吊装未吊  双I 双V                            
                if (nt % 3 != 0 || nt == 0)
                {
                    //已吊，中间吊装
                    //20160903 单侧吊装按单个串荷考虑  中间吊装考虑前后侧比例  只针对跳线串数不为3的情况

                    XTF[i, j] = formula.ZLX(out string strXTF, angle, 0, c[7], 0, BL3, nt, Ratio.BLTQ);
                    YTF[i, j] = formula.ZLY(out string strYTF, angle, 0, c[7], 0, nt, Ratio.BLTQ);
                    ZTF[i, j] = formula.ZLT2Z(out string strZTF, c[8], nt, 0, Ratio.BLTQ);

                    if (nt > 0)
                    {
                        ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fx= " + strXTF);
                        ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fy= " + strYTF);
                        ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fz= " + strZTF);
                    }

                    XTB[i, j] = formula.ZLX(out string strXTB, angle, 0, c[7], 0, BL3, nt, Ratio.BLTH);
                    YTB[i, j] = formula.ZLY(out string strYTB, angle, 0, c[7], 0, nt, Ratio.BLTH);
                    ZTB[i, j] = formula.ZLT2Z(out string strZTB, c[8], nt, 0, Ratio.BLTH);

                    if (nt > 0)
                    {
                        ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= " + strXTB);
                        ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= " + strYTB);
                        ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= " + strZTB);
                    }
                }
                else if (nt % 3 == 0 && nt > 0)
                {
                    XTF[i, j] = formula.ZLX(out string strXTF, angle, 0, c[7], 0, BL3, nt, Ratio.BLTQ);
                    YTF[i, j] = formula.ZLY(out string strYTF, angle, 0, c[7], 0, nt, Ratio.BLTQ);
                    ZTF[i, j] = formula.ZLT2Z(out string strZTF, c[8], nt, 0, Ratio.BLTQ);

                    ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fx= " + strXTF);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fy= " + strYTF);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fz= " + strZTF);

                    XTC[i, j] = formula.ZLX(out string strXTC, angle, 0, c[7], 0, BL3, nt, Ratio.BLTZ);
                    YTC[i, j] = formula.ZLY(out string strYTC, angle, 0, c[7], 0, nt, Ratio.BLTZ);
                    ZTC[i, j] = formula.ZLT2Z(out string strZTC, c[8], nt, 0, Ratio.BLTZ);
                        
                    ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fx= " + strXTC);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fy= " + strYTC);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fz= " + strZTC);

                    XTB[i, j] = formula.ZLX(out string strXTB, angle, 0, c[7], 0, BL3, nt, Ratio.BLTH);
                    YTB[i, j] = formula.ZLY(out string strYTB, angle, 0, c[7], 0, nt, Ratio.BLTH);
                    ZTB[i, j] = formula.ZLT2Z(out string strZTB, c[8], nt, 0, Ratio.BLTH); 

                    ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= " + strXTB);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= " + strYTB);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= " + strZTB);
                }
            }
            else if (zhs > 2000 && zhs.ToString().Length == 4)
            {
                //单侧吊装未吊  双I 双V                            
                if (nt % 3 == 0 && nt > 0)
                {
                    //'中间吊装正吊
                    fhn = 1;
                    XTF[i, j] = formula.ZLX(out string strXTF, angle, 0, c[7], 0, BL3, nt, Ratio.BLTQ);
                    YTF[i, j] = formula.ZLY(out string strYTF, angle, 0, c[7], 0, nt, Ratio.BLTQ);
                    ZTF[i, j] = formula.ZLT1Z(out string strZTF, c[8], nt, fh_2, fhn, Ratio.BLTQ);
                    
                    ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fx= " + strXTF);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fy= " + strYTF);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fz= " + strZTF);

                    XTC[i, j] = formula.ZLX(out string strXTC, angle, 0, c[7], 0, BL3, nt, Ratio.BLTZ);
                    YTC[i, j] = formula.ZLY(out string strYTC, angle, 0, c[7], 0, nt, Ratio.BLTZ);
                    ZTC[i, j] = formula.ZLT1Z(out string strZTC, c[8], nt, fh_2, fhn, Ratio.BLTZ);

                    ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fx= " + strXTC);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fy= " + strYTC);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fz= " + strZTC);

                    XTB[i, j] = formula.ZLX(out string strXTB, angle, 0, c[7], 0, BL3, nt, Ratio.BLTH);
                    YTB[i, j] = formula.ZLY(out string strYTB, angle, 0, c[7], 0, nt, Ratio.BLTH);
                    ZTB[i, j] = formula.ZLT1Z(out string strZTB, c[8], nt, fh_2, fhn, Ratio.BLTH);

                    ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= " + strXTB);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= " + strYTB);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= " + strZTB);
                    
                }
                else if (nt % 3 != 0 || nt == 0)
                {
                    //20160903 单侧吊装按单个串荷考虑  中间吊装考虑前后侧比例  只针对跳线串数不为3的情况
                    fhn = 1;
                    XTF[i, j] = formula.ZLX(out string strXTF, angle, 0, c[7], 0, BL3, nt, Ratio.BLTQ);
                    YTF[i, j] = formula.ZLY(out string strYTF, angle, 0, c[7], 0, nt, Ratio.BLTQ);
                    ZTF[i, j] = formula.ZLT1Z(out string strZTF, c[8], nt, fh_2, fhn, Ratio.BLTQ);

                    if(nt > 0)
                    {
                        ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fx= " + strXTF);
                        ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fy= " + strYTF);
                        ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fz= " + strZTF);
                    }

                    XTB[i, j] = formula.ZLX(out string strXTB, angle, 0, c[7], 0, BL3, nt, Ratio.BLTH);
                    YTB[i, j] = formula.ZLY(out string strYTB, angle, 0, c[7], 0, nt, Ratio.BLTH);
                    ZTB[i, j] = formula.ZLT1Z(out string strZTB, c[8], nt, fh_2, fhn, Ratio.BLTH);

                    if (nt > 0)
                    {
                        ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= " + strXTB);
                        ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= " + strYTB);
                        ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= " + strZTB);
                    }
                }
            }
            else if (zhs.ToString().Length == 5 && zhs.ToString().Substring(0, 1) == "1" && zhs.ToString().Substring(0, 3) != "100")
            {
                //单侧集中吊装   1AB0i系列    注意AB不能同时取1或2  排他
                fhn = 1;
                if(nt == 0)
                {
                    XTF[i, j] = 0.00f;
                    YTF[i, j] = 0.00f;
                    ZTF[i, j] = 0.00f;

                    XTB[i, j] = 0.00f;
                    YTB[i, j] = 0.00f;
                    ZTB[i, j] = 0.00f;
                }
                else
                {
                    switch (zhs.ToString().Substring(1, 1))
                    {
                        case "0":
                            //未吊
                            XTF[i, j] = 0.00f;
                            YTF[i, j] = 0.00f;
                            ZTF[i, j] = 0.00f;

                            if (j > dxl)
                            {
                                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fx= 0.00");
                                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fy= 0.00");
                                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fz= 0.00");
                            }
                            break;
                        case "1":
                            //已吊
                            XTF[i, j] = formula.ZLX(out string strXTF, angle, 0, c[7], 0, BL3, nt);
                            YTF[i, j] = formula.ZLY(out string strYTF, angle, 0, c[7], 0, nt);
                            ZTF[i, j] = formula.ZLT2Z(out string strZTF, c[8], nt, 0);
                            if (j > dxl)
                            {
                                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fx= " + strXTF);
                                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fy= " + strYTF);
                                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fz= " + strYTF);
                            }
                            break;
                        case "2":
                            //正吊
                            XTF[i, j] = formula.ZLX(out string strXTF2, angle, 0, c[7], 0, BL3, nt);
                            YTF[i, j] = formula.ZLY(out string strYTF2, angle, 0, c[7], 0, nt);
                            ZTF[i, j] = formula.ZLT1Z(out string strZTF2, c[8], nt, fh_2, fhn);
                            if (j > dxl)
                            {
                                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fx= " + strXTF2);
                                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fy= " + strYTF2);
                                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fz= " + strZTF2);
                            }
                            break;
                    }
                    switch (zhs.ToString().Substring(2, 1))
                    {
                        case "0":
                            //未吊
                            XTB[i, j] = 0.00f;
                            YTB[i, j] = 0.00f;
                            ZTB[i, j] = 0.00f;

                            if (j > dxl)
                            {
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= 0.00");
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= 0.00");
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= 0.00");
                            }
                            break;
                        case "1":
                            //已吊
                            XTB[i, j] = formula.ZLX(out string strXTB, angle, 0, c[7], 0, BL3, nt);
                            YTB[i, j] = formula.ZLY(out string strYTB, angle, 0, c[7], 0, nt);
                            ZTB[i, j] = formula.ZLT2Z(out string strZTB, c[8], nt, 0);
                            if (j > dxl)
                            {
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= " + strXTB);
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= " + strYTB);
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= " + strZTB);
                            }
                            break;
                        case "2":
                            //正吊
                            XTB[i, j] = formula.ZLX(out string strXTB1, angle, 0, c[7], 0, BL3, nt);
                            YTB[i, j] = formula.ZLY(out string strYTB1, angle, 0, c[7], 0, nt);
                            ZTB[i, j] = formula.ZLT1Z(out string strZTB1, c[8], nt, fh_2, fhn);
                            if (j > dxl)
                            {
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= " + strXTB1);
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= " + strYTB1);
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= " + strZTB1);
                            }
                            break;
                    }
                }
            }
            else if (zhs.ToString().Length == 5 && zhs.ToString().Substring(0, 1) == "2" && zhs.ToString().Substring(0, 3) != "200")
            {
                //吊装荷载前后分开吊装  不区分吊串数  荷载直接按比例前后分配
                fhn = 1;
                if (nt == 0)
                {
                    XTF[i, j] = 0.00f;
                    YTF[i, j] = 0.00f;
                    ZTF[i, j] = 0.00f;

                    XTB[i, j] = 0.00f;
                    YTB[i, j] = 0.00f;
                    ZTB[i, j] = 0.00f;
                }
                else
                {
                    switch (zhs.ToString().Substring(1, 1))
                    {
                        case "0":
                            //未吊
                            XTF[i, j] = 0.00f;
                            YTF[i, j] = 0.00f;
                            ZTF[i, j] = 0.00f;

                            if (j > dxl)
                            {
                                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fx= 0.00");
                                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fy= 0.00");
                                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fz= 0.00");
                            }
                            break;
                        case "1":
                            //已吊
                            XTF[i, j] = formula.ZLX(out string strXTF, angle, 0, c[7], 0, BL3, nt, Ratio.BLDZTQ);
                            YTF[i, j] = formula.ZLY(out string strYTF, angle, 0, c[7], 0, nt, Ratio.BLDZTQ);
                            ZTF[i, j] = formula.ZLT2Z(out string strZTF, c[8], nt, 0, Ratio.BLDZTQ);
                            if (j > dxl)
                            {
                                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fx= " + strXTF);
                                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fy= " + strYTF);
                                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fz= " + strZTF);
                            }
                            break;
                        case "2":
                            //正吊
                            XTF[i, j] = formula.ZLX(out strXTF, angle, 0, c[7], 0, BL3, nt, Ratio.BLDZTQ);
                            YTF[i, j] = formula.ZLY(out strYTF, angle, 0, c[7], 0, nt, Ratio.BLDZTQ);
                            ZTF[i, j] = formula.ZLT1Z(out strZTF, c[8], nt, fh_2, fhn, Ratio.BLDZTQ, true);
                            if (j > dxl)
                            {
                                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fx= " + strXTF);
                                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fy= " + strYTF);
                                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fz= " + strZTF);
                            }
                            break;
                    }
                    switch (zhs.ToString().Substring(2, 1))
                    {
                        case "0":
                            //未吊
                            XTB[i, j] = 0.00f;
                            YTB[i, j] = 0.00f;
                            ZTB[i, j] = 0.00f;

                            if (j > dxl)
                            {
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= 0.00");
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= 0.00");
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= 0.00");
                            }
                            break;
                        case "1":
                            //已吊
                            XTB[i, j] = formula.ZLX(out string strXTB, angle, 0, c[7], 0, BL3, nt, Ratio.BLDZTH);
                            YTB[i, j] = formula.ZLY(out string strYTB, angle, 0, c[7], 0, nt, Ratio.BLDZTH);
                            ZTB[i, j] = formula.ZLT2Z(out string strZTB, c[8], nt * Ratio.BLDZTH, 0);
                            if (j > dxl)
                            {
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= " + strXTB);
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= " + strYTB);
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= " + strZTB);
                            }
                            break;
                        case "2":
                            //正吊
                            XTB[i, j] = formula.ZLX(out strXTB, angle, 0, c[7], 0, BL3, nt, Ratio.BLDZTH);
                            YTB[i, j] = formula.ZLY(out strYTB, angle, 0, c[7], 0, nt, Ratio.BLDZTH);
                            ZTB[i, j] = formula.ZLT1Z(out strZTB, c[8], nt, fh_2, fhn, Ratio.BLDZTH, true);
                            if (j > dxl)
                            {
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= " + strXTB);
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= " + strYTB);
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= " + strZTB);
                            }
                            break;
                    }
                }
            }
            else if (zhs.ToString().Length == 6 && zhs.ToString().Substring(0, 1) == "1" && zhs.ToString().Substring(0, 4) != "1000")
            {
                //3I或3V  单侧集中吊装
                fhn = 1;
                if (nt == 0)
                {
                    XTF[i, j] = 0.00f;
                    YTF[i, j] = 0.00f;
                    ZTF[i, j] = 0.00f;

                    XTC[i, j] = 0.00f;
                    YTC[i, j] = 0.00f;
                    ZTC[i, j] = 0.00f;

                    XTB[i, j] = 0.00f;
                    YTB[i, j] = 0.00f;
                    ZTB[i, j] = 0.00f;

                    //没跳线，不输出
                }
                else
                {
                    switch (zhs.ToString().Substring(1, 1))
                    {
                        case "0":
                            //未吊
                            XTF[i, j] = 0.00f;
                            YTF[i, j] = 0.00f;
                            ZTF[i, j] = 0.00f;

                            if (j > dxl)
                            {
                                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fx= 0.00");
                                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fy= 0.00");
                                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fz= 0.00");
                            }
                            break;
                        case "1":
                            //已吊
                            XTF[i, j] = formula.ZLX(out string strXTF, angle, 0, c[7], 0, BL3, nt);
                            YTF[i, j] = formula.ZLY(out string strYTF, angle, 0, c[7], 0, nt);
                            ZTF[i, j] = formula.ZLT2Z(out string strZTF, c[8], nt, 0);
                            if (j > dxl)
                            {
                                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fx= " + strXTF);
                                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fy= " + strYTF);
                                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fz= " + strZTF);
                            }
                            break;
                        case "2":
                            //正吊
                            //  nt = 1   '此处错误
                            XTF[i, j] = formula.ZLX(out strXTF, angle, 0, c[7], 0, BL3, nt);
                            YTF[i, j] = formula.ZLY(out strYTF, angle, 0, c[7], 0, nt);
                            ZTF[i, j] = formula.ZLT1Z(out strZTF, c[8], nt, fh_2, fhn);
                            if (j > dxl)
                            {
                                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fx= " + strXTF);
                                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fy= " + strYTF);
                                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fz= " + strZTF);
                            }
                            break;
                    }
                    switch (zhs.ToString().Substring(2, 1))
                    {
                        case "0":
                            //未吊
                            XTB[i, j] = 0.00f;
                            YTB[i, j] = 0.00f;
                            ZTB[i, j] = 0.00f;
                            nt = 1;

                            if (j > dxl)
                            {
                                ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fx= 0.00");
                                ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fy= 0.00");
                                ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fz= 0.00");
                            }
                            break;
                        case "1":
                            //已吊
                            //   nt = 1    此处错误
                            XTB[i, j] = formula.ZLX(out string strXTB, angle, 0, c[7], 0, BL3, nt);
                            YTB[i, j] = formula.ZLY(out string strYTB, angle, 0, c[7], 0, nt);
                            ZTB[i, j] = formula.ZLT2Z(out string strZTB, c[8], nt, 0);
                            if (j > dxl)
                            {
                                ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fx= " + strXTB);
                                ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fy= " + strYTB);
                                ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fz= " + strZTB);
                            }
                            break;
                        case "2":
                            //正吊         
                            //  nt = 1    '此处错误
                            XTB[i, j] = formula.ZLX(out strXTB, angle, 0, c[7], 0, BL3, nt);
                            YTB[i, j] = formula.ZLY(out strYTB, angle, 0, c[7], 0, nt);
                            ZTB[i, j] = formula.ZLT1Z(out strZTB, c[8], nt, fh_2, fhn);
                            if (j > dxl)
                            {
                                ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fx= " + strXTB);
                                ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fy= " + strYTB);
                                ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fz= " + strZTB);
                            }
                            break;
                    }
                    switch (zhs.ToString().Substring(3, 1))
                    {
                        case "0":
                            //未吊
                            XTB[i, j] = 0.00f;
                            YTB[i, j] = 0.00f;
                            ZTB[i, j] = 0.00f;
                            nt = 1;

                            if (j > dxl)
                            {
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= 0.00");
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= 0.00");
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= 0.00");
                            }
                            break;
                        case "1":
                            //已吊
                            //   nt = 1    此处错误
                            XTB[i, j] = formula.ZLX(out string strXTB, angle, 0, c[7], 0, BL3, nt);
                            YTB[i, j] = formula.ZLY(out string strYTB, angle, 0, c[7], 0, nt);
                            ZTB[i, j] = formula.ZLT2Z(out string strZTB, c[8], nt, 0);
                            if (j > dxl)
                            {
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= " + strXTB);
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= " + strYTB);
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= " + strZTB);
                            }
                            break;
                        case "2":
                            //正吊         
                            //  nt = 1    '此处错误
                            XTB[i, j] = formula.ZLX(out strXTB, angle, 0, c[7], 0, BL3, nt);
                            YTB[i, j] = formula.ZLY(out strYTB, angle, 0, c[7], 0, nt);
                            ZTB[i, j] = formula.ZLT1Z(out strZTB, c[8], nt, fh_2, fhn);
                            if (j > dxl)
                            {
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= " + strXTB);
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= " + strYTB);
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= " + strZTB);
                            }
                            break;
                    }
                }
            }
            else if (zhs.ToString().Length == 6 && zhs.ToString().Substring(0, 1) == "2" && zhs.ToString().Substring(0, 4) != "2000")
            {
                //3I或3V   分开吊装
                fhn = 1;
                if (nt == 0)
                {
                    XTF[i, j] = 0.00f;
                    YTF[i, j] = 0.00f;
                    ZTF[i, j] = 0.00f;

                    XTC[i, j] = 0.00f;
                    YTC[i, j] = 0.00f;
                    ZTC[i, j] = 0.00f;

                    XTB[i, j] = 0.00f;
                    YTB[i, j] = 0.00f;
                    ZTB[i, j] = 0.00f;

                    //没跳线，不输出
                }
                else
                {
                    switch (zhs.ToString().Substring(1, 1))
                    {
                        case "0":
                            //未吊
                            XTF[i, j] = 0.00f;
                            YTF[i, j] = 0.00f;
                            ZTF[i, j] = 0.00f;

                            if (j > dxl)
                            {
                                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fx= 0.00");
                                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fy= 0.00");
                                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fz= 0.00");
                            }
                            break;
                        case "1":
                            //已吊
                            XTF[i, j] = formula.ZLX(out string strXTF, angle, 0, c[7], 0, BL3, nt, Ratio.BLDZTQ);
                            YTF[i, j] = formula.ZLY(out string strYTF, angle, 0, c[7], 0, nt, Ratio.BLDZTQ);
                            ZTF[i, j] = formula.ZLT2Z(out string strZTF, c[8], nt, 0, Ratio.BLDZTQ);
                            if (j > dxl)
                            {
                                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fx= " + strXTF);
                                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fy= " + strYTF);
                                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fz= " + strZTF);
                            }
                            break;
                        case "2":
                            //正吊
                            //  nt = 1   
                            XTF[i, j] = formula.ZLX(out strXTF, angle, 0, c[7], 0, BL3, nt, Ratio.BLDZTQ);
                            YTF[i, j] = formula.ZLY(out strYTF, angle, 0, c[7], 0, nt, Ratio.BLDZTQ);
                            ZTF[i, j] = formula.ZLT1Z(out strZTF, c[8], nt, fh_2, fhn, Ratio.BLDZTQ, true);
                            if (j > dxl)
                            {
                                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fx= " + strXTF);
                                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fy= " + strYTF);
                                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fz= " + strZTF);
                            }
                            break;
                    }
                    switch (zhs.ToString().Substring(2, 1))
                    {
                        case "0":
                            //未吊
                            XTC[i, j] = 0.00f;
                            YTC[i, j] = 0.00f;
                            ZTC[i, j] = 0.00f;
                            //nt = 1

                            if (j > dxl)
                            {
                                ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fx= 0.00");
                                ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fy= 0.00");
                                ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fz= 0.00");
                            }
                            break;
                        case "1":
                            //已吊
                            //   nt = 1 
                            XTC[i, j] = formula.ZLX(out string strXTC, angle, 0, c[7], 0, BL3, nt, Ratio.BLDZTZ);
                            YTC[i, j] = formula.ZLY(out string strYTC, angle, 0, c[7], 0, nt, Ratio.BLDZTZ);
                            ZTC[i, j] = formula.ZLT2Z(out string strZTC, c[8], nt, 0, Ratio.BLDZTZ);
                            if (j > dxl)
                            {
                                ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fx= " + strXTC);
                                ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fy= " + strYTC);
                                ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fz= " + strZTC);
                            }
                            break;
                        case "2":
                            //正吊         
                            //  nt = 1    
                            XTC[i, j] = formula.ZLX(out strXTC, angle, 0, c[7], 0, BL3, nt, Ratio.BLDZTZ);
                            YTC[i, j] = formula.ZLY(out strYTC, angle, 0, c[7], 0, nt, Ratio.BLDZTZ);
                            ZTC[i, j] = formula.ZLT1Z(out strZTC, c[8], nt, fh_2, fhn, Ratio.BLDZTZ, true);
                            if (j > dxl)
                            {
                                ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fx= " + strXTC);
                                ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fy= " + strYTC);
                                ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fz= " + strZTC);
                            }
                            break;
                    }
                    switch (zhs.ToString().Substring(3, 1))
                    {
                        case "0":
                            //未吊
                            XTB[i, j] = 0.00f;
                            YTB[i, j] = 0.00f;
                            ZTB[i, j] = 0.00f;
                            //nt = 1

                            if (j > dxl)
                            {
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= 0.00");
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= 0.00");
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= 0.00");
                            }
                            break;
                        case "1":
                            //已吊
                            //   nt = 1    此处错误
                            XTB[i, j] = formula.ZLX(out string strXTB, angle, 0, c[7], 0, BL3, nt, Ratio.BLDZTH);
                            YTB[i, j] = formula.ZLY(out string strYTB, angle, 0, c[7], 0, nt, Ratio.BLDZTH);
                            ZTB[i, j] = formula.ZLT2Z(out string strZTB, c[8], nt, 0, Ratio.BLDZTH);
                            if (j > dxl)
                            {
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= " + strXTB);
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= " + strYTB);
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= " + strZTB);
                            }
                            break;
                        case "2":
                            //正吊         
                            //  nt = 1    '此处错误
                            XTB[i, j] = formula.ZLX(out strXTB, angle, 0, c[7], 0, BL3, nt, Ratio.BLDZTH);
                            YTB[i, j] = formula.ZLY(out strYTB, angle, 0, c[7], 0, nt, Ratio.BLDZTH);
                            ZTB[i, j] = formula.ZLT1Z(out strZTB, c[8], nt, fh_2, fhn, Ratio.BLDZTH, true);
                            if (j > dxl)
                            {
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= " + strXTB);
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= " + strYTB);
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= " + strZTB);
                            }
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 针对锚线工况
        /// 工况代码"M1", "M2",
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        protected void DistributeInAnchor(int i, int j)
        {
            float x11, x12, x21, x22, x3;
            float y1, y2;
            float z11, z12, z21, z22, z3;
            float e1, e2, e3;
            float[] c = new float[12];
            int fhn = 0;
            //int BL2;

            WorkConditionCombo wd = Template.WorkConditionCombos[i];

            int zhs = wd.WireIndexCodes[j-1], zhsx;
            int angle = wd.WindDirectionCode;
            string workConditionCode = wd.WorkConditionCode;
            int mz1 = Template.WorkConditongs.Count;
            int BL3 = LineParas.isTurnRight ? 1 : -1;
            //跳线数 = 导线数
            int nt = Template.Wires.Where(item => item.Contains("导")).Count();
            float fh_1 = LineParas.WireExtraLoad / Paras.LoadRatio;

            //锚线工况，大小转角情况
            if (zhs == 0)
            {
                //BL2 = 0;
                //未锚
                zhsx = 0;
                y1 = 0;
                y2 = 0;
                x11 = 0;
                x12 = 0;
                x21 = 0;
                x22 = 0;
                x3 = 0;
                z11 = 0;
                z12 = 0;
                z21 = 0;
                z22 = 0;
                z3 = 0;
            }
            else if(zhs < 1000 && zhs > 0)
            {
                //BL2 = 1;
                //已锚
                zhsx = zhs;
                y1 = TensionD[j, zhsx];
                y2 = TensionX[j, zhsx];
                x11 = WindDF[j, zhsx];
                x12 = WindXF[j, zhsx];
                x21 = WindDB[j, zhsx];
                x22 = WindXB[j, zhsx];
                x3 = WindTX[j, zhsx];
                z11 = GMaxF[j, zhsx];
                z12 = GMinF[j, zhsx];
                z21 = GMaxB[j, zhsx];
                z22 = GMinB[j, zhsx];
                z3 = GTX[j, zhsx];
            }
            else  if(zhs > 1000)
            {
                //BL2 = 1;
                //正锚
                fhn = zhs / 1000;
                zhsx = zhs % 1000;
                y1 = TensionD[j, zhsx];
                y2 = TensionX[j, zhsx];
                x11 = WindDF[j, zhsx];
                x12 = WindXF[j, zhsx];
                x21 = WindDB[j, zhsx];
                x22 = WindXB[j, zhsx];
                x3 = WindTX[j, zhsx];
                z11 = GMaxF[j, zhsx];
                z12 = GMinF[j, zhsx];
                z21 = GMaxB[j, zhsx];
                z22 = GMinB[j, zhsx];
                z3 = GTX[j, zhsx]; 
            }
            else
            {
                //BL2 = 0;
                //未锚
                zhsx = 0;
                y1 = 0;
                y2 = 0;
                x11 = 0;
                x12 = 0;
                x21 = 0;
                x22 = 0;
                x3 = 0;
                z11 = 0;
                z12 = 0;
                z21 = 0;
                z22 = 0;
                z3 = 0; 
            }

            if (wd.WorkConditionCode == "M1")
            {
                e1 = x11; e2 = x21; e3 = LineParas.AngleMax;
            }
            //else if (wd.WorkConditionCode == "M2")
            else
            {
                e1 = x12; e2 = x22; e3 = LineParas.AngleMin;
            }

            if (wd.TensionAngleCode == "D0")
            {
                switch (wd.VertialLoadCode)
                {
                    case "Y0":
                        if(Paras.IsCornerTower || Paras.IsBranchTower)
                        {
                            c[1] = e1 ; c[2] = Math.Max(y1, LineParas.AnchorTension) ; c[3] = z12;
                        }
                        else
                        {
                            c[1] = e1 ; c[2] = Math.Max(y1, LineParas.AnchorTension) ; c[3] = z11;
                        }
                        break;
                    case "B0":
                        if (Paras.IsCornerTower || Paras.IsBranchTower)
                        {
                            c[1] = e1 ; c[2] = Math.Max(y1, LineParas.AnchorTension) ; c[3] = z12;
                        }
                        else
                        {
                            c[1] = e1 ; c[2] = Math.Max(y1, LineParas.AnchorTension) ; c[3] = z12;
                        }
                        break;
                    //2016.9.24 针对前后侧同时取某一侧的拔力新增,T表示某侧取相反侧上拔力计算
                    case "T0":
                        if (Paras.IsCornerTower || Paras.IsBranchTower)
                        {
                            c[1] = e1 ; c[2] = Math.Max(y1, LineParas.AnchorTension) ; c[3] = z22;
                        }
                        else
                        {
                            c[1] = e1 ; c[2] = Math.Max(y1, LineParas.AnchorTension) ; c[3] = z22;
                        }
                        break;
                    default:
                        c[1] = e1 ; c[2] = Math.Max(y1, LineParas.AnchorTension);
                        if (wd.VertialLoadCode.Substring(0, 1) == "0")
                        {
                            c[3] = 0;
                        }
                        else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                        {
                            c[3] = z11;
                        }
                        else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                        {
                            c[3] = z12;
                        }
                        else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                        {
                            c[3] = z21;
                        }
                        else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                        {
                            c[3] = z22;
                        }
                        else
                        {
                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-25");
                        }

                        if (wd.VertialLoadCode.Substring(1, 1) == "0")
                        {
                            c[6] = 0;
                        }
                        else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                        {
                            c[6] = z11;
                        }
                        else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                        {
                            c[6] = z12;
                        }
                        else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                        {
                            c[6] = z21;
                        }
                        else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                        {
                            c[6] = z22;
                        }
                        else
                        {
                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-16");
                        }

                        break;
                }
            }

            if(zhs > 1000)
            {
                if(Paras.IsBranchTower)
                {
                    //前侧转角  20161124分支塔添加
                    e3 = LineParas.AngleFront;
                }
                XLF[i, j] = formula.ZM1X(out string strXLF, angle, e3, c[1], c[2], BL3);
                YLF[i, j] = formula.ZM1Y(out string strYLF, angle, e3, c[1], c[2]);
                ZLF[i, j] = formula.ZM1Z(out string strZLF, c[3], fhn, fh_1);
                
                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fx= " + strXLF);
                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fy= " + strYLF);
                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fz= " + strZLF);
            }
            else if(zhs < 1000 && zhs >0)
            {
                if (Paras.IsBranchTower)
                {
                    //前侧转角  20161124分支塔添加
                    e3 = LineParas.AngleFront;
                }

                XLF[i, j] = formula.ZM2X(out string strXLF, angle, e3, c[1], c[2], BL3);
                YLF[i, j] = formula.ZM2Y(out string strYLF, angle, e3, c[1], c[2]);
                ZLF[i, j] = formula.ZM2Z(out string strZLF, c[3]);

                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fx= " + strXLF);
                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fy= " + strYLF);
                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fz= " + strZLF);

            }
            else if(zhs == 0)
            {
                XLF[i, j] = 0.00f;
                YLF[i, j] = 0.00f;
                ZLF[i, j] = 0.00f;

                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fx= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fy= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fz= 0.00");
            }

            XLB[i, j] = 0.00f;
            YLB[i, j] = 0.00f;
            ZLB[i, j] = 0.00f;

            ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fx= 0.00");
            ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fy= 0.00");
            ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fz= 0.00");

            XTF[i, j] = 0.00f;
            YTF[i, j] = 0.00f;
            ZTF[i, j] = 0.00f;

            XTC[i, j] = 0.00f;
            YTC[i, j] = 0.00f;
            ZTC[i, j] = 0.00f;

            XTB[i, j] = 0.00f;
            YTB[i, j] = 0.00f;
            ZTB[i, j] = 0.00f;

            if (nt % 3 != 0)
            {
                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fx= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fy= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fz= 0.00");

                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= 0.00");
            }
            else if(nt % 3 == 0 && nt > 0)
            {
                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fx= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fy= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fz= 0.00");

                ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fx= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fy= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fz= 0.00");

                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= 0.00");
            }
        }
        
        /// <summary>
        /// 工况代码"J1", "J2",
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        protected void DistributeInJ(int i, int j)
        {
            float x11, x12, x21, x22, x3;
            float y1, y2;
            float z11, z12, z21, z22, z3;
            float e1, e2, e3;
            float[] c = new float[12];
            int fhn = 0, zhsx;
            //int BL2;

            WorkConditionCombo wd = Template.WorkConditionCombos[i];

            int zhs = wd.WireIndexCodes[j - 1];
            int angle = wd.WindDirectionCode;
            string workConditionCode = wd.WorkConditionCode;
            int mz1 = Template.WorkConditongs.Count;
            int BL3 = LineParas.isTurnRight ? 1 : -1;
            //跳线数 = 导线数
            int nt = Template.Wires.Where(item => item.Contains("导")).Count();
            float fh_1 = LineParas.WireExtraLoad / Paras.LoadRatio;

            if (zhs == 0)
            {
                //BL2 = 0;
                //未紧
                zhsx = 0;
                y1 = 0;
                y2 = 0;
                x11 = 0;
                x12 = 0;
                x21 = 0;
                x22 = 0;
                x3 = 0;
                z11 = 0;
                z12 = 0;
                z21 = 0;
                z22 = 0;
                z3 = 0;
            }
            else if (zhs < 1000 && zhs > 0)
            {
                //BL2 = 1;
                //已紧
                zhsx = zhs % 100;
                fhn = zhs / 100;
                if (fhn < 1)
                    fhn = 1;
                y1 = TensionD[j, zhsx];
                y2 = TensionX[j, zhsx];
                x11 = WindDF[j, zhsx];
                x12 = WindXF[j, zhsx];
                x21 = WindDB[j, zhsx];
                x22 = WindXB[j, zhsx];
                x3 = WindTX[j, zhsx];
                z11 = GMaxF[j, zhsx];
                z12 = GMinF[j, zhsx];
                z21 = GMaxB[j, zhsx];
                z22 = GMinB[j, zhsx];
                z3 = GTX[j, zhsx];
            }
            else if (zhs > 1000)
            {
                //BL2 = 1;
                //正紧
                zhsx = zhs % 1000;
                fhn = zhs / 1000;
                y1 = TensionD[j, zhsx];
                y2 = TensionX[j, zhsx];
                x11 = WindDF[j, zhsx];
                x12 = WindXF[j, zhsx];
                x21 = WindDB[j, zhsx];
                x22 = WindXB[j, zhsx];
                x3 = WindTX[j, zhsx];
                z11 = GMaxF[j, zhsx];
                z12 = GMinF[j, zhsx];
                z21 = GMaxB[j, zhsx];
                z22 = GMinB[j, zhsx];
                z3 = GTX[j, zhsx];
            }
            else
            {
                //BL2 = 0;
                zhsx = 0;
                y1 = 0;
                y2 = 0;
                x11 = 0;
                x12 = 0;
                x21 = 0;
                x22 = 0;
                x3 = 0;
                z11 = 0;
                z12 = 0;
                z21 = 0;
                z22 = 0;
                z3 = 0;
            }

            if (wd.WorkConditionCode == "J1")
            {
                e1 = x11 ; e2 = x21 ; e3 = LineParas.AngleMax;
            }
            //else if (wd.WorkConditionCode == "J2")
            else
            {
                e1 = x12 ; e2 = x22 ; e3 = LineParas.AngleMin;
            }

            if (wd.TensionAngleCode == "D0")
            {
                switch (wd.VertialLoadCode)
                {
                    case "Y0":
                        if (Paras.IsCornerTower || Paras.IsBranchTower)
                        {
                            c[1] = e1 ; c[2] = Math.Max(y1, LineParas.AnchorTension) ; c[3] = z12;
                        }
                        else
                        {
                            c[1] = e1 ; c[2] = Math.Max(y1, LineParas.AnchorTension) ; c[3] = z11;
                        }
                        break;
                    case "B0":
                        if (Paras.IsCornerTower || Paras.IsBranchTower)
                        {
                            c[1] = e1 ; c[2] = Math.Max(y1, LineParas.AnchorTension) ; c[3] = z12;
                        }
                        else
                        {
                            c[1] = e1 ; c[2] = Math.Max(y1, LineParas.AnchorTension) ; c[3] = z12;
                        }
                        break;
                    //2016.9.24 针对前后侧同时取某一侧的拔力新增,T表示某侧取相反侧上拔力计算
                    case "T0":
                        if (Paras.IsCornerTower || Paras.IsBranchTower)
                        {
                            c[1] = e1 ; c[2] = Math.Max(y1, LineParas.AnchorTension) ; c[3] = z22;
                        }
                        else
                        {
                            c[1] = e1 ; c[2] = Math.Max(y1, LineParas.AnchorTension) ; c[3] = z22;
                        }
                        break;
                    default:
                        c[1] = e1 ; c[2] = Math.Max(y1, LineParas.AnchorTension);
                        if (wd.VertialLoadCode.Substring(0, 1) == "0")
                        {
                            c[3] = 0;
                        }
                        else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                        {
                            c[3] = z11;
                        }
                        else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                        {
                            c[3] = z12;
                        }
                        else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                        {
                            c[3] = z21;
                        }
                        else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                        {
                            c[3] = z22;
                        }
                        else
                        {
                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-25");
                        }

                        if (wd.VertialLoadCode.Substring(1, 1) == "0")
                        {
                            c[6] = 0;
                        }
                        else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                        {
                            c[6] = z11;
                        }
                        else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                        {
                            c[6] = z12;
                        }
                        else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                        {
                            c[6] = z21;
                        }
                        else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                        {
                            c[6] = z22;
                        }
                        else
                        {
                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-16");
                        }

                        break;
                }
            }

            if (zhs > 1000)
            {
                if (Paras.IsBranchTower)
                {
                    //前侧转角  20161124分支塔添加
                    e3 = LineParas.AngleFront;
                }
                XLF[i, j] = formula.JX(out string strXLF, angle, c[1], Paras.DynamicCoef, LineParas.DrawingCoef, c[2], e3, BL3);
                YLF[i, j] = formula.JY(out string strYLF, angle, c[1], Paras.DynamicCoef, LineParas.DrawingCoef, c[2], e3);
                ZLF[i, j] = formula.JZ(out string strZLF, c[3], Paras.DynamicCoef, LineParas.DrawingCoef, c[2], fh_1, fhn);


                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fx= " + strXLF);
                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fy= " + strYLF);
                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fz= " + strZLF);
            }
            else if (zhs < 1000 && zhs > 0)
            {
                if (Paras.IsBranchTower)
                {
                    //前侧转角  20161124分支塔添加
                    e3 = LineParas.AngleFront;
                }

                XLF[i, j] = formula.JX(out string strXLF, angle, c[1], 1.0f, LineParas.DrawingCoef, c[2], e3, BL3);
                YLF[i, j] = formula.JY(out string strYLF, angle, c[1], 1.0f, LineParas.DrawingCoef, c[2], e3);
                ZLF[i, j] = formula.JZ(out string strZLF, c[3], 1.0f, LineParas.DrawingCoef, c[2], fh_1, fhn);

                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fx= " + strXLF);
                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fy= " + strYLF);
                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fz= " + strZLF);

            }
            else if (zhs == 0)
            {
                XLF[i, j] = 0.00f;
                YLF[i, j] = 0.00f;
                ZLF[i, j] = 0.00f;

                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fx= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fy= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fz= 0.00");
            }

            XLB[i, j] = 0.00f;
            YLB[i, j] = 0.00f;
            ZLB[i, j] = 0.00f;

            ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fx= 0.00");
            ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fy= 0.00");
            ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fz= 0.00");

            XTF[i, j] = 0.00f;
            YTF[i, j] = 0.00f;
            ZTF[i, j] = 0.00f;

            XTC[i, j] = 0.00f;
            YTC[i, j] = 0.00f;
            ZTC[i, j] = 0.00f;

            XTB[i, j] = 0.00f;
            YTB[i, j] = 0.00f;
            ZTB[i, j] = 0.00f;

            if (nt % 3 != 0)
            {
                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fx= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fy= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fz= 0.00");

                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= 0.00");
            }
            else if (nt % 3 == 0 && nt > 0)
            {
                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fx= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fy= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fz= 0.00");

                ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fx= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fy= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fz= 0.00");

                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= 0.00");
            }
        }

        /// <summary>
        /// 锚兼牵工况
        /// 工况代码"MQ1", "MQ2",
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        protected void DistributeInMQ(int i, int j)
        {
            float x11, x12, x21, x22, x3;
            float y1, y2;
            float z11, z12, z21, z22, z3;
            float e1, e2, e3;
            float t1 = 0, t2 = 0;
            float[] c = new float[12];
            int fhn = 0, zhsx;
            //int BL2;

            WorkConditionCombo wd = Template.WorkConditionCombos[i];

            int zhs = wd.WireIndexCodes[j - 1];
            int angle = wd.WindDirectionCode;
            string workConditionCode = wd.WorkConditionCode;
            int mz1 = Template.WorkConditongs.Count;
            int BL3 = LineParas.isTurnRight ? 1 : -1;
            //跳线数 = 导线数
            int nt = Template.Wires.Where(item => item.Contains("导")).Count();
            float fh_1 = LineParas.WireExtraLoad / Paras.LoadRatio;

            if ( zhs< 100 && zhs > 0 )
            {
                //后侧已锚，前侧未挂
                //BL2 = 1;
                zhsx = zhs;
            }
            else if(zhs > 100 && zhs < 1000)
            {
                //已架
                //BL2 = 1;
                zhsx = zhs % 100;
                fhn = zhs / 100;
            }
            else if(zhs > 1000)
            {
                //后侧已锚，前侧正牵
                //BL2 = 1;
                fhn = zhs / 1000;
                zhsx = zhs % 1000;
            }
            else
            {
                //BL2 = 1;
                zhsx = zhs;
                fhn = zhs / 1000;
            }

            y1 = TensionD[j, zhsx];
            y2 = TensionX[j, zhsx];
            x11 = WindDF[j, zhsx];
            x12 = WindXF[j, zhsx];
            x21 = WindDB[j, zhsx];
            x22 = WindXB[j, zhsx];
            x3 = WindTX[j, zhsx];
            z11 = GMaxF[j, zhsx];
            z12 = GMinF[j, zhsx];
            z21 = GMaxB[j, zhsx];
            z22 = GMinB[j, zhsx];
            z3 = GTX[j, zhsx];

            if (wd.WorkConditionCode == "MQ1")
            {
                e1 = x11 ; e2 = x21 ; e3 = LineParas.AngleMax;
            }
            //else if (wd.WorkConditionCode == "MQ2")
            else
            {
                e1 = x12 ; e2 = x22 ; e3 = LineParas.AngleMin;
            }

            if (wd.TensionAngleCode == "DX")
            {
                switch (wd.VertialLoadCode)
                {
                    case "YY":
                        t1 = z11;
                        t2 = z21;
                        break;
                    case "YB":
                        t1 = z11;
                        t2 = z22;
                        break;
                    case "BY":
                        t1 = z12;
                        t2 = z21;
                        break;
                    case "BB":
                        t1 = z12;
                        t2 = z22;
                        break;
                    //2016.9.24 针对前后侧同时取某一侧的拔力新增,T表示某侧取相反侧上拔力计算
                    case "YT":
                        t1 = z11;
                        t2 = z12;
                        break;
                    case "TY":
                        t1 = z22;
                        t2 = z21;
                        break;
                    default:
                        if (wd.VertialLoadCode.Substring(0, 1) == "0")
                        {
                            t1 = 0;
                        }
                        else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                        {
                            t1 = z11;
                        }
                        else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                        {
                            t1 = z12;
                        }
                        else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                        {
                            t1 = z21;
                        }
                        else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                        {
                            t1 = z22;
                        }
                        else
                        {
                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-25");
                        }

                        if (wd.VertialLoadCode.Substring(1, 1) == "0")
                        {
                            t2 = 0;
                        }
                        else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                        {
                            t2 = z11;
                        }
                        else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                        {
                            t2 = z12;
                        }
                        else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                        {
                            t2 = z21;
                        }
                        else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                        {
                            t2 = z22;
                        }
                        else
                        {
                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-16");
                        }

                        break;
                }
            }

            if (zhs > 1000)
            {
                //前侧正牵
                if(Paras.IsCornerTower || Paras.IsBranchTower)
                {
                    c[1] = e1 ; c[2] = Math.Max(y1, LineParas.AnchorTension) ; c[3] = t1;
                }
                else if(Paras.IsTerminalTower)
                {
                    c[1] = e1 ; c[2] = Math.Max(y1, LineParas.AnchorTension) ; c[3] = t1;
                }

                if (Paras.IsBranchTower)
                {
                    //前侧转角  20161124分支塔添加
                    e3 = LineParas.AngleFront;
                }

                XLF[i, j] = formula.MQ1X(out string strXLF, angle, e3, c[1], c[2], BL3);
                YLF[i, j] = formula.MQ1Y(out string strYLF, angle, e3, c[1], c[2]);
                ZLF[i, j] = formula.MQ1Z(out string strZLF, c[3], c[2], fhn, fh_1);

                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fx= " + strXLF);
                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fy= " + strYLF);
                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fz= " + strZLF);


                //后侧已锚
                if (Paras.IsCornerTower || Paras.IsBranchTower)
                {
                    c[1] = e2 ; c[2] = Math.Max(y2, LineParas.AnchorTension) ; c[3] = t2;
                }
                else if (Paras.IsTerminalTower)
                {
                    c[1] = e2 ; c[2] = Math.Max(LineParas.PortalTensionMin, LineParas.AnchorTension);  c[3] = t2;
                }

                if (Paras.IsBranchTower)
                {
                    //前侧转角  20161124分支塔添加
                    e3 = LineParas.AngleBack;
                }

                XLB[i, j] = formula.MQ2X(out string strXLB, angle, e3, c[1], c[2], BL3);
                YLB[i, j] = formula.MQ2Y(out string strYLB, angle, e3, c[1], c[2]);
                ZLB[i, j] = formula.MQ2Z(out string strZLB, c[3]);

                ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fx= " + strXLB);
                ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fy= " + strYLB);
                ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fz= " + strZLB);
            }
            else if (zhs > 100 && zhs < 1000)
            {
                //前后侧已架
                if (Paras.IsCornerTower || Paras.IsBranchTower)
                {
                    c[1] = e1; c[2] = y1; c[3] = t1;
                }
                else if (Paras.IsTerminalTower)
                {
                    c[1] = e1; c[2] = y1; c[3] = t1;
                }

                if (Paras.IsBranchTower)
                {
                    //前侧转角  20161124分支塔添加
                    e3 = LineParas.AngleFront;
                }

                XLF[i, j] = formula.MQ3X(out string strXLF, angle, e3, c[1], c[2], BL3);
                YLF[i, j] = formula.MQ3Y(out string strYLF, angle, e3, c[1], c[2]);
                ZLF[i, j] = formula.MQ3Z(out string strZLF, c[3]);

                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fx= " + strXLF);
                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fy= " + strYLF);
                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fz= " + strZLF);


                //后侧已锚
                if (Paras.IsCornerTower || Paras.IsBranchTower)
                {
                    c[1] = e2 ; c[2] = y2 ; c[3] = t2;
                }
                else if (Paras.IsTerminalTower)
                {
                    c[1] = e2 ; c[2] = LineParas.PortalTensionMin ; c[3] = t2;
                }

                if (Paras.IsBranchTower)
                {
                    //前侧转角  20161124分支塔添加
                    e3 = LineParas.AngleBack;
                }

                XLB[i, j] = formula.MQ3X(out string strXLB, angle, e3, c[1], c[2], BL3);
                YLB[i, j] = formula.MQ3Y(out string strYLB, angle, e3, c[1], -c[2]);
                ZLB[i, j] = formula.MQ3Z(out string strZLB, c[3]);

                ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fx= " + strXLB);
                ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fy= " + strYLB);
                ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fz= " + strZLB);
            }
            else if (zhs < 100 && zhs > 0)
            {
                //其余未挂
                if (Paras.IsBranchTower)
                {
                    //前侧转角  20161124分支塔添加
                    e3 = LineParas.AngleFront;
                }
                XLF[i, j] = formula.MQ4X(out string strXLF, e3, BL3);
                YLF[i, j] = formula.MQ4Y(out string strYLF, e3);
                ZLF[i, j] = formula.MQ4Z(out string strZLF);

                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fx= " + strXLF);
                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fy= " + strYLF);
                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fz= " + strZLF);


                //后侧正锚
                if (Paras.IsCornerTower || Paras.IsBranchTower)
                {
                    c[1] = e2 ; c[2] = Math.Max(y2, LineParas.AnchorTension) ; c[3] = t2;
                }
                else if (Paras.IsTerminalTower)
                {
                    c[1] = e2 ; c[2] = Math.Max(LineParas.PortalTensionMin, LineParas.AnchorTension) ; c[3] = t2;
                }

                if (Paras.IsBranchTower)
                {
                    //前侧转角  20161124分支塔添加
                    e3 = LineParas.AngleBack;
                }

                XLB[i, j] = formula.MQ2X(out string strXLB, angle, e3, c[1], c[2], BL3);
                YLB[i, j] = formula.MQ2Y(out string strYLB, angle, e3, c[1], c[2]);
                ZLB[i, j] = formula.MQ2Z(out string strZLB, c[3]);

                ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fx= " + strXLB);
                ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fy= " + strYLB);
                ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fz= " + strZLB);

            }
            else if (zhs == 0)
            {
                XLF[i, j] = 0.00f;
                YLF[i, j] = 0.00f;
                ZLF[i, j] = 0.00f;

                XLB[i, j] = 0.00f;
                YLB[i, j] = 0.00f;
                ZLB[i, j] = 0.00f;

                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fx= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fy= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fz= 0.00");

                ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fx= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fy= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fz= 0.00");
            }

            XTF[i, j] = 0.00f;
            YTF[i, j] = 0.00f;
            ZTF[i, j] = 0.00f;

            XTC[i, j] = 0.00f;
            YTC[i, j] = 0.00f;
            ZTC[i, j] = 0.00f;

            XTB[i, j] = 0.00f;
            YTB[i, j] = 0.00f;
            ZTB[i, j] = 0.00f;

            if (nt % 3 != 0)
            {
                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fx= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fy= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fz= 0.00");

                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= 0.00");
            }
            else if (nt % 3 == 0 && nt > 0)
            {
                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fx= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fy= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fz= 0.00");

                ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fx= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fy= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fz= 0.00");

                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= 0.00");
            }
        }

        /// <summary>
        /// 过滑车工况
        /// 工况代码"G1", "G2",
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        protected void DistributeInG(int i, int j)
        {
            float x11, x12, x21, x22, x3;
            float y1, y2;
            float z11, z12, z21, z22, z3;
            float e1, e2, e3;
            float t1, t2;
            float[] c = new float[12];
            int fhn, zhsx;
            //int BL2;

            WorkConditionCombo wd = Template.WorkConditionCombos[i];

            int zhs = wd.WireIndexCodes[j - 1];
            int angle = wd.WindDirectionCode;
            string workConditionCode = wd.WorkConditionCode;
            int mz1 = Template.WorkConditongs.Count;
            int BL3 = LineParas.isTurnRight ? 1 : -1;
            //跳线数 = 导线数
            int nt = Template.Wires.Where(item => item.Contains("导")).Count();

            if (zhs < 100 && zhs > 0)
            {
                //已过滑车
                //BL2 = 1;
                zhsx = zhs;
            }
            else if (zhs > 1000)
            {
                //正过滑车
                //BL2 = 1;
                fhn = zhs / 1000;
                zhsx = zhs % 1000;
            }
            else
            {
                //BL2 = 1;
                zhsx = zhs;
            }

            y1 = TensionD[j, zhsx];
            y2 = TensionX[j, zhsx];
            x11 = WindDF[j, zhsx];
            x12 = WindXF[j, zhsx];
            x21 = WindDB[j, zhsx];
            x22 = WindXB[j, zhsx];
            x3 = WindTX[j, zhsx];
            z11 = GMaxF[j, zhsx];
            z12 = GMinF[j, zhsx]; 
            z21 = GMaxB[j, zhsx]; 
            z22 = GMinB[j, zhsx];
            z3 = GTX[j, zhsx];

            if (wd.WorkConditionCode == "G1")
            {
                e1 = x11 ; e2 = x21 ; e3 = LineParas.AngleMax;
            }
            //else if (wd.WorkConditionCode == "G2")
            else
            {
                e1 = x12 ; e2 = x22 ; e3 = LineParas.AngleMin;
            }

            if (wd.TensionAngleCode == "DX" || wd.TensionAngleCode == "DD")
            {
                switch (wd.VertialLoadCode)
                {
                    case "YY":
                        t1 = z11;
                        t2 = z21;
                        break;
                    case "YB":
                        t1 = z11;
                        t2 = z22;
                        break;
                    case "BY":
                        t1 = z12;
                        t2 = z21;
                        break;
                    case "BB":
                        t1 = z12;
                        t2 = z22;
                        break;
                    //2016.9.24 针对前后侧同时取某一侧的拔力新增,T表示某侧取相反侧上拔力计算
                    case "YT":
                        t1 = z11;
                        t2 = z12;
                        break;
                    case "TY":
                        t1 = z22;
                        t2 = z21;
                        break;
                    default:
                        if (wd.VertialLoadCode.Substring(0, 1) == "0")
                        {
                            t1 = 0;
                        }
                        else if (wd.VertialLoadCode.Substring(0, 1) == "1")
                        {
                            t1 = z11;
                        }
                        else if (wd.VertialLoadCode.Substring(0, 1) == "2")
                        {
                            t1 = z12;
                        }
                        else if (wd.VertialLoadCode.Substring(0, 1) == "3")
                        {
                            t1 = z21;
                        }
                        else if (wd.VertialLoadCode.Substring(0, 1) == "4")
                        {
                            t1 = z22;
                        }
                        else
                        {
                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-25");
                        }

                        if (wd.VertialLoadCode.Substring(1, 1) == "0")
                        {
                            t2 = 0;
                        }
                        else if (wd.VertialLoadCode.Substring(1, 1) == "1")
                        {
                            t2 = z11;
                        }
                        else if (wd.VertialLoadCode.Substring(1, 1) == "2")
                        {
                            t2 = z12;
                        }
                        else if (wd.VertialLoadCode.Substring(1, 1) == "3")
                        {
                            t2 = z21;
                        }
                        else if (wd.VertialLoadCode.Substring(1, 1) == "4")
                        {
                            t2 = z22;
                        }
                        else
                        {
                            throw new Exception("工况" + i + "垂荷组合参数有误，请核实!" + "0 + 32 " + "错误：20190113-16");
                        }

                        break;
                }

                if (zhs < 100 && zhs > 0)
                {
                    //已过滑车
                    if (Paras.IsCornerTower || Paras.IsBranchTower)
                    {
                        c[1] = e1 ; c[2] = y1 ; c[3] = t1;
                    }
                    else if (Paras.IsTerminalTower)
                    {
                        c[1] = e1 ; c[2] = y1 ; c[3] = t1;
                    }

                    if (Paras.IsBranchTower)
                    {
                        //前侧转角  20161124分支塔添加
                        e3 = LineParas.AngleFront;
                    }

                    XLF[i, j] = formula.MQ3X(out string strXLF, angle, e3, c[1], c[2], BL3);
                    YLF[i, j] = formula.MQ3Y(out string strYLF, angle, e3, c[1], c[2]);
                    ZLF[i, j] = formula.MQ3Z(out string strZLF, c[3]);

                    ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fx= " + strXLF);
                    ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fy= " + strYLF);
                    ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fz= " + strZLF);

                    if (wd.TensionAngleCode == "DX")
                    {
                        //后侧已锚
                        if (Paras.IsCornerTower || Paras.IsBranchTower)
                        {
                            c[1] = e2 ; c[2] = y2 ; c[3] = t2;
                        }
                        else if (Paras.IsTerminalTower)
                        {
                            c[1] = e2 ; c[2] = LineParas.PortalTensionMin ; c[3] = t2;
                        }

                        if (Paras.IsBranchTower)
                        {
                            //前侧转角  20161124分支塔添加
                            e3 = LineParas.AngleBack;
                        }
                    }
                    else if (wd.TensionAngleCode == "DX")
                    {
                        //后侧已锚
                        if (Paras.IsCornerTower || Paras.IsBranchTower)
                        {
                            c[1] = e2 ; c[2] = y1 ; c[3] = t2;
                        }
                        else if (Paras.IsTerminalTower)
                        {
                            c[1] = e2 ; c[2] = y1 ; c[3] = t2;
                        }

                        if (Paras.IsBranchTower)
                        {
                            //前侧转角  20161124分支塔添加
                            e3 = LineParas.AngleBack;
                        }
                    }

                    XLB[i, j] = formula.MQ3X(out string strXLB, angle, e3, c[1], c[2], BL3);
                    YLB[i, j] = formula.MQ3Y(out string strYLB, angle, e3, c[1], -c[2]);
                    ZLB[i, j] = formula.MQ3Z(out string strZLB, c[3]);

                    ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fx= " + strXLB);
                    ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fy= " + strYLB);
                    ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fz= " + strZLB);
                }
                else if (zhs > 1000)
                {
                    //正过滑车，无张力差
                    if (Paras.IsCornerTower || Paras.IsBranchTower)
                    {
                        c[1] = e1 ; c[2] = Math.Max(y1, y2) ; c[3] = t1;
                    }
                    else if (Paras.IsTerminalTower)
                    {
                        c[1] = e1 ; c[2] = Math.Max(y1, y2) ; c[3] = t1;
                    }

                    if (Paras.IsBranchTower)
                    {
                        //前侧转角  20161124分支塔添加
                        e3 = LineParas.AngleFront;
                    }

                    XLF[i, j] = formula.MQ3X(out string strXLF, angle, e3, c[1], c[2], BL3);
                    YLF[i, j] = formula.MQ5Y(out string strYLF, LineParas.PulleyTensionDif);
                    ZLF[i, j] = formula.MQ3Z(out string strZLF, c[3]);
    
                    ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fx= " + strXLF);
                    ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fy= " + strYLF);
                    ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fz= " + strZLF);

                    if (Paras.IsCornerTower || Paras.IsBranchTower)
                    {
                        c[1] = e2 ; c[2] = y1 ; c[3] = t2;
                    }
                    else if (Paras.IsTerminalTower)
                    {
                        c[1] = e2 ; c[2] = LineParas.PortalTensionMax ; c[3] = t2;
                    }

                    if (Paras.IsBranchTower)
                    {
                        //前侧转角  20161124分支塔添加
                        e3 = LineParas.AngleBack;
                    }

                    XLB[i, j] = formula.MQ3X(out string strXLB, angle, e3, c[1], c[2], BL3);
                    YLB[i, j] = formula.MQ5Y(out string strYLB, LineParas.PulleyTensionDif);
                    ZLB[i, j] = formula.MQ3Z(out string strZLB, c[3]);
    
                    ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fx= " + strXLB);
                    ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fy= " + strYLB);
                    ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fz= " + strZLB);
                }
                else if (zhs == 0)
                {
                    XLF[i, j] = 0.00f;
                    YLF[i, j] = 0.00f;
                    ZLF[i, j] = 0.00f;

                    XLB[i, j] = 0.00f;
                    YLB[i, j] = 0.00f;
                    ZLB[i, j] = 0.00f;

                    ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fx= 0.00");
                    ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fy= 0.00");
                    ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fz= 0.00");

                    ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fx= 0.00");
                    ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fy= 0.00");
                    ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fz= 0.00");
                }
            }
            else
            {
                throw new Exception("工况" + i + ".线条" + j +  "前后张力信息有误");
            }

            XTF[i, j] = 0.00f;
            YTF[i, j] = 0.00f;
            ZTF[i, j] = 0.00f;

            XTC[i, j] = 0.00f;
            YTC[i, j] = 0.00f;
            ZTC[i, j] = 0.00f;

            XTB[i, j] = 0.00f;
            YTB[i, j] = 0.00f;
            ZTB[i, j] = 0.00f;

            if (nt % 3 != 0)
            {
                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fx= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fy= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fz= 0.00");

                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= 0.00");
            }
            else if (nt % 3 == 0 && nt > 0)
            {
                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fx= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fy= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fz= 0.00");

                ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fx= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fy= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fz= 0.00");

                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= 0.00");
            }
        }


    }
}
