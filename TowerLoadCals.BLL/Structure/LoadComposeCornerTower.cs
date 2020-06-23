using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using TowerLoadCals.Mode;

namespace TowerLoadCals.BLL
{

    /// <summary>
    /// 耐脏塔的荷载分配
    /// </summary>
    public class LoadComposeCornerTower:LoadComposeBase
    {
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

        protected float[,] XLB { get; set; }
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

        public LoadComposeCornerTower(FormulaParas para, StruLineParas[] lineParas, HangingPointSettingParas ratioParas, TowerTemplate template, string tablePath) 
            : base(para, lineParas, ratioParas, template, tablePath)
        {
            formula = new FormulaCornerTower(para);
        }

        protected override void GetTable(DataSet ds)
        {
            //后续计算的序号从1开始
            WindDF = new float[Template.Wires.Count + 1, Template.WorkConditongs.Count + 1];
            WindDB = new float[Template.Wires.Count + 1, Template.WorkConditongs.Count + 1];
            WindXF = new float[Template.Wires.Count + 1, Template.WorkConditongs.Count + 1];
            WindXB = new float[Template.Wires.Count + 1, Template.WorkConditongs.Count + 1];
            GMaxF = new float[Template.Wires.Count + 1, Template.WorkConditongs.Count + 1];
            GMaxB = new float[Template.Wires.Count + 1, Template.WorkConditongs.Count + 1];
            GMinF = new float[Template.Wires.Count + 1, Template.WorkConditongs.Count + 1];
            GMinB = new float[Template.Wires.Count + 1, Template.WorkConditongs.Count + 1];
            TensionD = new float[Template.Wires.Count + 1, Template.WorkConditongs.Count + 1];
            TensionX = new float[Template.Wires.Count + 1, Template.WorkConditongs.Count + 1];

            WindTX = new float[Template.Wires.Count + 1, Template.WorkConditongs.Count + 1];
            GTX = new float[Template.Wires.Count + 1, Template.WorkConditongs.Count + 1];

            Tension45D = new float[Template.Wires.Count + 1, Template.WorkConditongs.Count + 1];
            Tension45X = new float[Template.Wires.Count + 1, Template.WorkConditongs.Count + 1];

            int dx = Template.Wires.Where(item => item.Contains("地")).Count();

            for (int j = 1; j <= Template.WorkConditongs.Count; j++)
            {
                for (int i = 1; i <= dx; i++)
                {
                    object obj = ds.Tables[0].Rows[4 + 11 * (i - 1)][j];
                    float.TryParse(obj.ToString(), out WindDF[i, j]);
                    object obj2 = ds.Tables[0].Rows[5 + 11 * (i - 1)][j];
                    float.TryParse(obj2.ToString(), out WindDB[i, j]);
                    object obj3 = ds.Tables[0].Rows[6 + 11 * (i - 1)][j];
                    float.TryParse(obj3.ToString(), out WindXF[i, j]);
                    object obj4 = ds.Tables[0].Rows[7 + 11 * (i - 1)][j];
                    float.TryParse(obj4.ToString(), out WindXB[i, j]);
                    object obj5 = ds.Tables[0].Rows[8 + 11 * (i - 1)][j];
                    float.TryParse(obj5.ToString(), out GMaxF[i, j]);
                    object obj6 = ds.Tables[0].Rows[9 + 11 * (i - 1)][j];
                    float.TryParse(obj6.ToString(), out GMaxB[i, j]);
                    object obj7 = ds.Tables[0].Rows[10 + 11 * (i - 1)][j];
                    float.TryParse(obj7.ToString(), out GMinF[i, j]);
                    object obj8 = ds.Tables[0].Rows[11 + 11 * (i - 1)][j];
                    float.TryParse(obj8.ToString(), out GMinB[i, j]);
                    object obj9 = ds.Tables[0].Rows[12 + 11 * (i - 1)][j];
                    float.TryParse(obj9.ToString(), out TensionD[i, j]);
                    object obj10 = ds.Tables[0].Rows[13 + 11 * (i - 1)][j];
                    float.TryParse(obj10.ToString(), out TensionX[i, j]);
                }
            }

            int offset = 11 * dx;

            for (int j = 1; j <= Template.WorkConditongs.Count; j++)
            {
                for (int i = 1; i <= Template.Wires.Count - dx; i++)
                {
                    object obj = ds.Tables[0].Rows[4  + offset + 13 * (i - 1)][j];
                    float.TryParse(obj.ToString(), out WindDF[dx + i, j]);
                    object obj2 = ds.Tables[0].Rows[5 + offset + 13 * (i - 1)][j];
                    float.TryParse(obj2.ToString(), out WindDB[dx + i, j]);
                    object obj3 = ds.Tables[0].Rows[6 + offset + 13 * (i - 1)][j];
                    float.TryParse(obj3.ToString(), out WindXF[dx + i, j]);
                    object obj4 = ds.Tables[0].Rows[7 + offset + 13 * (i - 1)][j];
                    float.TryParse(obj4.ToString(), out WindXB[dx + i, j]);
                    object obj5 = ds.Tables[0].Rows[8 + offset + 13 * (i - 1)][j];
                    float.TryParse(obj5.ToString(), out GMaxF[dx + i, j]);
                    object obj6 = ds.Tables[0].Rows[9 + offset + 13 * (i - 1)][j];
                    float.TryParse(obj6.ToString(), out GMaxB[dx + i, j]);
                    object obj7 = ds.Tables[0].Rows[10 + offset + 13 * (i - 1)][j];
                    float.TryParse(obj7.ToString(), out GMinF[dx + i, j]);
                    object obj8 = ds.Tables[0].Rows[11 + offset + 13 * (i - 1)][j];
                    float.TryParse(obj8.ToString(), out GMinB[dx + i, j]);
                    object obj9 = ds.Tables[0].Rows[12 + offset + 13 * (i - 1)][j];
                    float.TryParse(obj9.ToString(), out TensionD[dx + i, j]);
                    object obj10 = ds.Tables[0].Rows[13 + offset + 13 * (i - 1)][j];
                    float.TryParse(obj10.ToString(), out TensionX[dx + i, j]);
                    object obj11 = ds.Tables[0].Rows[14 + offset + 13 * (i - 1)][j];
                    float.TryParse(obj11.ToString(), out WindTX[dx + i, j]);
                    object obj12 = ds.Tables[0].Rows[15 + offset + 13 * (i - 1)][j];  
                    float.TryParse(obj12.ToString(), out GTX[dx + i, j]);
                }
            }
        }

        public override void CalculateLoadDistribute(string path)
        {
            int calNums = Template.WorkConditionCombos.Count;

            XLB = new float[calNums, Template.Wires.Count];
            YLB = new float[calNums, Template.Wires.Count];
            ZLB = new float[calNums, Template.Wires.Count];

            XLF = new float[calNums, Template.Wires.Count];
            YLF = new float[calNums, Template.Wires.Count];
            ZLF = new float[calNums, Template.Wires.Count];

            XTF = new float[calNums, Template.Wires.Count];
            YTF = new float[calNums, Template.Wires.Count];
            ZTF = new float[calNums, Template.Wires.Count];

            XTC = new float[calNums, Template.Wires.Count];
            YTC = new float[calNums, Template.Wires.Count];
            ZTC = new float[calNums, Template.Wires.Count];

            XTB = new float[calNums, Template.Wires.Count];
            YTB = new float[calNums, Template.Wires.Count];
            ZTB = new float[calNums, Template.Wires.Count];

            XX = new float[calNums, Template.Wires.Count];
            YY = new float[calNums, Template.Wires.Count];
            ZZ = new float[calNums, Template.Wires.Count];

            int i = -1, j = 1;
            int count = 0;

            ProcessString.Add(Template.Name + " " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

            foreach (var wd in Template.WorkConditionCombos)
            {
                i++;
                if (!wd.IsCalculate)
                    continue;

                count++;
                ProcessString.Add("");
                ProcessString.Add("No." + count.ToString() + " " + wd.WorkComment);

                j = 1;
                foreach (int wic in wd.WireIndexCodes)
                {
                    LineParas = LineParasArr[j - 1];
                    
                    LineParas.TemporaryTension = LineParas.TemporaryTension / Paras.LoadRatio;
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
                            //break;

                    }
                    j++;
                }
            }

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
            int tStringNum = LineParas.TstringNum;
            float AngleMax = LineParas.AngleMax / 2;
            float AngleMin = LineParas.AngleMin / 2;

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
                    e3 = AngleMax;
                    e4 = 2f;
                }
                else
                {
                    e1 = x12;
                    e2 = x22;
                    e3 = AngleMin;
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

                XLF[i, j - 1] = formula.ZNX(out string strXLF, angle, e3, c[1], c[2], Vcb, BL3);
                YLF[i, j - 1] = formula.ZNY(out string strYLF, angle, e3, c[1], c[2], Vcb);
                ZLF[i, j - 1] = formula.ZNZ(out string strZLF, c[3], Vcb);

                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fx= " + strXLF);
                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fy= " + strYLF);
                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fz= " + strZLF);

                if (Paras.IsBranchTower)
                {
                    //前侧转角  20161124分支塔添加
                    e3 = LineParas.AngleBack;
                }

                XLB[i, j - 1] = formula.ZNX(out string strXLB, angle, e3, c[4], c[5], Vcb, BL3);
                YLB[i, j - 1] = formula.ZNY(out string strYLB, angle, e3, c[4], -c[5], Vcb);
                ZLB[i, j - 1] = formula.ZNZ(out string strZLB, c[6], Vcb);

                ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fx= " + strXLB);
                ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fy= " + strYLB);
                ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fz= " + strZLB);
                ProcessString.Add(" ");

                if (tStringNum % 3 != 0 || tStringNum == 0)
                {
                    //0,1,2个跳线的时候 前后
                    //跳线串始终为一种计算方法
                    XTF[i, j - 1] = formula.ZNX(out string strXTF, angle, 0, c[7], 0, Vcb, BL3, tStringNum, HPSettingParas.BLTQ);
                    YTF[i, j - 1] = formula.ZNY(out string strYTF, angle, 0, c[7], 0, Vcb, tStringNum, HPSettingParas.BLTQ);
                    ZTF[i, j - 1] = formula.ZNZ(out string strZTF, c[8], Vcb, tStringNum, HPSettingParas.BLTQ);

                    if(tStringNum != 0)
                    {
                        ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fx= " + strXTF);
                        ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fy= " + strYTF);
                        ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fz= " + strZTF);
                    }

                    XTB[i, j - 1] = formula.ZNX(out string strXTB, angle, 0, c[7], 0, Vcb, BL3, tStringNum, HPSettingParas.BLTH);
                    YTB[i, j - 1] = formula.ZNY(out string strYTB, angle, 0, c[7], 0, Vcb, tStringNum, HPSettingParas.BLTH);
                    ZTB[i, j - 1] = formula.ZNZ(out string strZTB, c[8], Vcb, tStringNum, HPSettingParas.BLTH);

                    if (tStringNum != 0)
                    {
                        ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= " + strXTB);
                        ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= " + strYTB);
                        ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= " + strZTB);
                        ProcessString.Add(" ");
                    }
                }
                else if (tStringNum % 3 == 0 && tStringNum > 0)
                {
                    XTF[i, j - 1] = formula.ZNX(out string strXTF, angle, 0, c[7], 0, Vcb, BL3 , tStringNum,  HPSettingParas.BLTQ);
                    YTF[i, j - 1] = formula.ZNY(out string strYTF, angle, 0, c[7], 0, Vcb, tStringNum, HPSettingParas.BLTQ);
                    ZTF[i, j - 1] = formula.ZNZ(out string strZTF, c[8], Vcb, tStringNum, HPSettingParas.BLTQ);

                    ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fx= " + strXTF);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fy= " + strYTF);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fz= " + strZTF);

                    XTC[i, j - 1] = formula.ZNX(out string strXTC, angle, 0, c[7], 0, Vcb, BL3, tStringNum, HPSettingParas.BLTZ);
                    YTC[i, j - 1] = formula.ZNY(out string strYTC, angle, 0, c[7], 0, Vcb, tStringNum, HPSettingParas.BLTZ);
                    ZTC[i, j - 1] = formula.ZNZ(out string strZTC, c[8], Vcb, tStringNum, HPSettingParas.BLTZ);

                    ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fx= " + strXTC);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fy= " + strYTC);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fz= " + strZTC);

                    XTB[i, j - 1] = formula.ZNX(out string strXTB, angle, 0, c[7], 0, Vcb, BL3, tStringNum, HPSettingParas.BLTH);
                    YTB[i, j - 1] = formula.ZNY(out string strYTB, angle, 0, c[7], 0, Vcb, tStringNum, HPSettingParas.BLTH);
                    ZTB[i, j - 1] = formula.ZNZ(out string strZTB, c[8], Vcb, tStringNum, HPSettingParas.BLTH);

                    ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= " + strXTB);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= " + strYTB);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= " + strZTB);
                    ProcessString.Add(" ");
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

                XLF[i, j - 1] = formula.ZNX(out string strXLF, angle, e3, c[1], c[2], Vcb, BL3);
                YLF[i, j - 1] = formula.ZNY(out string strYLF, angle, e3, c[1], c[2], Vcb);
                ZLF[i, j - 1] = formula.ZNZ(out string strZLF, c[3], Vcb);

                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fx= " + XLF[i, j - 1].ToString("0.00"));
                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fy= " + YLF[i, j - 1].ToString("0.00"));
                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fz= " + ZLF[i, j - 1].ToString("0.00"));

                XLB[i, j - 1] = formula.ZNX(out string strXLB, angle, e3, c[4], c[5], Vcb, BL3);
                YLB[i, j - 1] = formula.ZNY(out string strYLB, angle, e3, c[4], -c[5], Vcb);
                ZLB[i, j - 1] = formula.ZNZ(out string strZLB, c[6], Vcb);

                ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fx= " + XLB[i, j - 1].ToString("0.00"));
                ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fy= " + YLB[i, j - 1].ToString("0.00"));
                ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fz= " + ZLB[i, j - 1].ToString("0.00"));
                ProcessString.Add(" ");

                if ( tStringNum % 3 != 0 || tStringNum == 0)
                {
                    //跳线串始终为一种计算方法
                    XTF[i, j - 1] = formula.ZNX(out string strXTF, angle, 0, c[7], 0, Vcb, BL3, tStringNum, HPSettingParas.BLTQ);
                    YTF[i, j - 1] = formula.ZNY(out string strYTF, angle, 0, c[7], 0, Vcb, tStringNum, HPSettingParas.BLTQ);
                    ZTF[i, j - 1] = formula.ZNZ(out string strZTF, c[8], Vcb, tStringNum, HPSettingParas.BLTQ);

                    if(tStringNum != 0)
                    {
                        ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fx= " + XTF[i, j - 1].ToString("0.00"));
                        ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fy= " + YTF[i, j - 1].ToString("0.00"));
                        ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fz= " + ZTF[i, j - 1].ToString("0.00"));
                    }

                    XTB[i, j - 1] = formula.ZNX(out string strXTB, angle, 0, c[7], 0, Vcb, BL3, tStringNum, HPSettingParas.BLTH);
                    YTB[i, j - 1] = formula.ZNY(out string strYTB, angle, 0, c[7], 0, Vcb, tStringNum, HPSettingParas.BLTH);
                    ZTB[i, j - 1] = formula.ZNZ(out string strZTB, c[8], Vcb, tStringNum, HPSettingParas.BLTH);

                    if (tStringNum != 0)
                    {
                        ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= " + XTB[i, j - 1].ToString("0.00"));
                        ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= " + YTB[i, j - 1].ToString("0.00"));
                        ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= " + ZTF[i, j - 1].ToString("0.00"));
                        ProcessString.Add(" ");
                    }
                }
                else if( tStringNum % 3 == 0 && tStringNum > 0 )
                {
                    XTF[i, j - 1] = formula.ZNX(out string strXTF, angle, 0, c[7], 0, Vcb, BL3, tStringNum, 0.33334f);
                    YTF[i, j - 1] = formula.ZNY(out string strYTF, angle, 0, c[7], 0, Vcb, tStringNum, 0.33334f);
                    ZTF[i, j - 1] = formula.ZNZ(out string strZTF, c[8], Vcb, tStringNum, 0.33334f);

                    ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fx= " + XTF[i, j - 1].ToString("0.00"));
                    ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fy= " + YTF[i, j - 1].ToString("0.00"));
                    ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fz= " + ZTF[i, j - 1].ToString("0.00"));

                    XTC[i, j - 1] = formula.ZNX(out string strXTC, angle, 0, c[7], 0, Vcb, BL3, tStringNum, 0.33334f);
                    YTC[i, j - 1] = formula.ZNY(out string strYTC, angle, 0, c[7], 0, Vcb, tStringNum, 0.33334f);
                    ZTC[i, j - 1] = formula.ZNZ(out string strZTC, c[8], Vcb, tStringNum, 0.33334f);

                    ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fx= " + XTC[i, j - 1].ToString("0.00"));
                    ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fy= " + YTC[i, j - 1].ToString("0.00"));
                    ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fz= " + ZTC[i, j - 1].ToString("0.00"));

                    XTB[i, j - 1] = formula.ZNX(out string strXTB, angle, 0, c[7], 0, Vcb, BL3, tStringNum, 0.33334f);
                    YTB[i, j - 1] = formula.ZNY(out string strYTB, angle, 0, c[7], 0, Vcb, tStringNum, 0.33334f);
                    ZTB[i, j - 1] = formula.ZNZ(out string strZTB, c[8], Vcb, tStringNum, 0.33334f);

                    ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= " + XTB[i, j - 1].ToString("0.00"));
                    ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= " + YTB[i, j - 1].ToString("0.00"));
                    ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= " + ZTB[i, j - 1].ToString("0.00"));
                    ProcessString.Add(" ");
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
            float[] c = new float[12];
            int BL2;
            float rg, Vloadx;

            WorkConditionCombo wd = Template.WorkConditionCombos[i];
            int angle = wd.WindDirectionCode;
            int zhs = wd.WireIndexCodes[j - 1];
            int BL3 = LineParas.isTurnRight ? 1 : -1;
            int tStringNum = LineParas.TstringNum;
            float AngleMax = LineParas.AngleMax / 2;
            float AngleMin = LineParas.AngleMin / 2;
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
                    e1 = x11; e2 = x21; e3 = AngleMax; e4 = 2; e5 = Paras.VcFNormal; Vloadx = Paras.RQ; rg = Paras.RGBad;
                }
                else if (wd.WorkConditionCode == "I2")
                {
                    e1 = x12; e2 = x22; e3 = AngleMin; e4 = 1; e5 = Paras.VcFNormal; Vloadx = Paras.RQ; rg = Paras.RGBad;
                }
                else if (wd.WorkConditionCode == "U1")
                {
                    e1 = x11; e2 = x21; e3 = AngleMax; e4 = 2; e5 = Paras.VcFUnevenIce; Vloadx = Paras.RQ; rg = Paras.RGBad;
                }
                else if (wd.WorkConditionCode == "U2")
                {
                    e1 = x12; e2 = x22; e3 = AngleMin; e4 = 1; e5 = Paras.VcFUnevenIce; Vloadx = Paras.RQ; rg = Paras.RGBad;
                }
                else if (wd.WorkConditionCode == "B1")
                {
                    e1 = x11; e2 = x21; e3 = AngleMax; e4 = 2; e5 = Paras.VcFBroken; Vloadx = Paras.RQ; rg = Paras.RGBad;
                }
                else if (wd.WorkConditionCode == "B2")
                {
                    e1 = x12; e2 = x22; e3 = AngleMin; e4 = 1; e5 = Paras.VcFBroken; Vloadx = Paras.RQ; rg = Paras.RGBad;
                }
                else if (wd.WorkConditionCode == "Y1")
                {
                    e1 = x11; e2 = x21; e3 = AngleMax; e4 = 2; e5 = Paras.VcFCheck; Vloadx = Paras.RA; rg = Paras.RGCheck01;
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
                            y3 = y2;
                            y4 = y1;
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

                XLF[i, j - 1] = formula.ZIBUYX(out string strXLF, angle, e3, c[1], c[2], e5, Vloadx, BL3);
                YLF[i, j - 1] = formula.ZIBUYY(out string strYLF, angle, e3, c[1], c[2], e5, Vloadx);
                ZLF[i, j - 1] = formula.ZIBUYZ(out string strZLF, c[9], c[3], e5, Vloadx, rg);

                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fx= " + strXLF);
                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fy= " + strYLF);
                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fz= " + strZLF);

                if (Paras.IsBranchTower)
                {
                    //前侧转角  20161124分支塔添加
                    e3 = LineParas.AngleBack;
                }

                XLB[i, j - 1] = formula.ZIBUYX(out string strXLB, angle, e3, c[4], c[5], e5, Vloadx, BL3);
                YLB[i, j - 1] = formula.ZIBUYY(out string strYLB, angle, e3, c[4], -c[5], e5, Vloadx);
                ZLB[i, j - 1] = formula.ZIBUYZ(out string strZLB, c[10], c[6], e5, Vloadx, rg);

                ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fx= " + strXLB);
                ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fy= " + strYLB);
                ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fz= " + strZLB);
                ProcessString.Add(" ");

                if (tStringNum % 3 != 0 || tStringNum == 0)
                {
                    //跳线串始终为一种计算方法
                    XTF[i, j - 1] = formula.ZIBUYX(out string strXTF, angle, 0, c[7], 0, e5, Vloadx, BL3, tStringNum, HPSettingParas.BLTQ);
                    YTF[i, j - 1] = formula.ZIBUYY(out string strYTF, angle, 0, c[7], 0, e5, Vloadx, tStringNum, HPSettingParas.BLTQ);
                    ZTF[i, j - 1] = formula.ZIBUYZ(out string strZTF, c[11], c[8], e5, Vloadx, rg, tStringNum, HPSettingParas.BLTQ);

                    if (tStringNum != 0)
                    {
                        ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fx= " + strXTF);
                        ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fy= " + strYTF);
                        ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fz= " + strZTF);
                    }

                    XTB[i, j - 1] = formula.ZIBUYX(out string strXTB, angle, 0, c[7], 0, e5, Vloadx, BL3, tStringNum, HPSettingParas.BLTH);
                    YTB[i, j - 1] = formula.ZIBUYY(out string strYTB, angle, 0, c[7], 0, e5, Vloadx, tStringNum, HPSettingParas.BLTH);
                    ZTB[i, j - 1] = formula.ZIBUYZ(out string strZTB, c[11], c[8], e5, Vloadx, rg, tStringNum, HPSettingParas.BLTH);

                    if (tStringNum != 0)
                    {
                        ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= " + strXTB);
                        ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= " + strYTB);
                        ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= " + strZTB);
                        ProcessString.Add(" ");
                    }
                }
                else
                {
                    XTF[i, j - 1] = formula.ZIBUYX(out string strXTF, angle, 0, c[7], 0, e5, Vloadx, BL3, tStringNum, HPSettingParas.BLTQ);
                    YTF[i, j - 1] = formula.ZIBUYY(out string strYTF, angle, 0, c[7], 0, e5, Vloadx, tStringNum, HPSettingParas.BLTQ);
                    ZTF[i, j - 1] = formula.ZIBUYZ(out string strZTF, c[11], c[8], e5, Vloadx, rg, tStringNum, HPSettingParas.BLTQ);
                    
                    ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fx= " + strXTF);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fy= " + strYTF);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fz= " + strZTF);

                    XTC[i, j - 1] = formula.ZIBUYX(out string strXTC, angle, 0, c[7], 0, e5, Vloadx, BL3, tStringNum, HPSettingParas.BLTZ);
                    YTC[i, j - 1] = formula.ZIBUYY(out string strYTC, angle, 0, c[7], 0, e5, Vloadx, tStringNum, HPSettingParas.BLTZ);
                    ZTC[i, j - 1] = formula.ZIBUYZ(out string strZTC, c[11], c[8], e5, Vloadx, rg, tStringNum, HPSettingParas.BLTZ);

                    ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fx= " + strXTC);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fy= " + strYTC);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fz= " + strZTC);

                    XTB[i, j - 1] = formula.ZIBUYX(out string strXTB, angle, 0, c[7], 0, e5, Vloadx, BL3, tStringNum, HPSettingParas.BLTH);
                    YTB[i, j - 1] = formula.ZIBUYY(out string strYTB, angle, 0, c[7], 0, e5, Vloadx, tStringNum, HPSettingParas.BLTH);
                    ZTB[i, j - 1] = formula.ZIBUYZ(out string strZTB, c[11], c[8], e5, Vloadx, rg, tStringNum, HPSettingParas.BLTH);

                    ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= " + strXTB);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= " + strYTB);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= " + strZTB);
                    ProcessString.Add(" ");
                }
            }
            else if(zhs == 0)
            {
                //该向无荷载
                BL2 = 0;
                XLF[i, j - 1] = 0.00f;
                YLF[i, j - 1] = 0.00f;
                ZLF[i, j - 1] = 0.00f;

                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fx= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fy= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fz= 0.00");

                XLB[i, j - 1] = 0.00f;
                YLB[i, j - 1] = 0.00f;
                ZLB[i, j - 1] = 0.00f;

                ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fx= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fy= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fz= 0.00");
                ProcessString.Add(" ");

                //跳线串始终为一种计算方法
                if (tStringNum % 3 != 0 || tStringNum == 0)
                {
                    XTF[i, j - 1] = 0.00f;
                    YTF[i, j - 1] = 0.00f;
                    ZTF[i, j - 1] = 0.00f;

                    if (tStringNum != 0)
                    {
                        ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fx= 0.00");
                        ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fy= 0.00");
                        ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fz= 0.00");
                    }

                    XTB[i, j - 1] = 0.00f;
                    YTB[i, j - 1] = 0.00f;
                    ZTB[i, j - 1] = 0.00f;

                    if (tStringNum != 0)
                    {
                        ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= 0.00");
                        ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= 0.00");
                        ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= 0.00");
                        ProcessString.Add(" ");
                    }
                }
                else
                {
                    XTF[i, j - 1] = 0.00f;
                    YTF[i, j - 1] = 0.00f;
                    ZTF[i, j - 1] = 0.00f;

                    ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fx= 0.00");
                    ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fy= 0.00");
                    ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fz= 0.00");

                    XTC[i, j - 1] = 0.00f;
                    YTC[i, j - 1] = 0.00f;
                    ZTC[i, j - 1] = 0.00f;

                    ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fx= 0.00");
                    ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fy= 0.00");
                    ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fz= 0.00");

                    XTB[i, j - 1] = 0.00f;
                    YTB[i, j - 1] = 0.00f;
                    ZTB[i, j - 1] = 0.00f;

                    ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= 0.00");
                    ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= 0.00");
                    ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= 0.00");
                    ProcessString.Add(" ");
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
            int BL3 = LineParas.isTurnRight ? 1 : -1;
            int tStringNum = LineParas.TstringNum;
            float AngleMax = LineParas.AngleMax / 2;
            float AngleMin = LineParas.AngleMin / 2;
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
                e1 = x11 ; e2 = x21 ; e3 = AngleMax;
            }
            else
            {
                e1 = x12 ; e2 = x22 ; e3 = AngleMin;
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
                XLF[i, j - 1] = 0.00f;
                YLF[i, j - 1] = 0.00f;
                ZLF[i, j - 1] = 0.00f;

                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fx= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fy= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fz= 0.00");

                XLB[i, j - 1] = 0.00f;
                YLB[i, j - 1] = 0.00f;
                ZLB[i, j - 1] = 0.00f;

                ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fx= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fy= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fz= 0.00");
                ProcessString.Add(" ");
            }
            else
            {
                if(Paras.IsBranchTower)
                {
                    //前后转角  20161124分支塔添加
                    e3 = LineParas.AngleFront;
                }

                XLF[i, j - 1] = formula.ZLX(out string strXLF, angle, e3, c[1], c[2], BL3);
                YLF[i, j - 1] = formula.ZLY(out string strYLF, angle, e3, c[1], c[2]);
                ZLF[i, j - 1] = formula.ZLZ(out string strZLF, c[3]);

                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fx= " + strXLF);
                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fy= " + strYLF);
                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fz= " + strZLF);

                if (Paras.IsBranchTower)
                {
                    //前后转角  20161124分支塔添加
                    e3 = LineParas.AngleBack;
                }

                XLB[i, j - 1] = formula.ZLX(out string strXLB, angle, e3, c[4], c[5], BL3);
                YLB[i, j - 1] = formula.ZLY(out string strYLB, angle, e3, c[4], -c[5]);
                ZLB[i, j - 1] = formula.ZLZ(out string strZLB, c[6]);

                ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fx= " + strXLB);
                ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fy= " + strYLB);
                ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fz= " + strZLB);
                ProcessString.Add(" ");
            }

            if(zhs < 100 && zhs > 0)
            {
                //表该相跳线为不吊装，跳线荷载为0  i
                //中间吊装未吊
                if(tStringNum % 3 != 0 || tStringNum == 0)
                {
                    XTF[i, j - 1] = 0.00f;
                    YTF[i, j - 1] = 0.00f;
                    ZTF[i, j - 1] = 0.00f;

                    XTB[i, j - 1] = 0.00f;
                    YTB[i, j - 1] = 0.00f;
                    ZTB[i, j - 1] = 0.00f;

                    if (tStringNum > 0)
                    {
                        ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fx= 0.00");
                        ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fy= 0.00");
                        ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fz= 0.00");

                        ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= 0.00");
                        ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= 0.00");
                        ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= 0.00");
                        ProcessString.Add(" ");
                    }
                }
                else if(tStringNum % 3 == 0 || tStringNum > 0)
                {
                    XTF[i, j - 1] = 0.00f;
                    YTF[i, j - 1] = 0.00f;
                    ZTF[i, j - 1] = 0.00f;

                    XTC[i, j - 1] = 0.00f;
                    YTC[i, j - 1] = 0.00f;
                    ZTC[i, j - 1] = 0.00f;

                    XTB[i, j - 1] = 0.00f;
                    YTB[i, j - 1] = 0.00f;
                    ZTB[i, j - 1] = 0.00f;

                    ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fx= 0.00");
                    ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fy= 0.00");
                    ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fz= 0.00");

                    ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fx= 0.00");
                    ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fy= 0.00");
                    ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fz= 0.00");

                    ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= 0.00");
                    ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= 0.00");
                    ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= 0.00");
                    ProcessString.Add(" ");
                }

            }
            else if(zhs.ToString().Length == 5 && (zhs.ToString().Substring(0,3) == "200" || zhs.ToString().Substring(0, 3) == "100"))
            {
                //单侧吊装未吊  双I 双V                             
                if (tStringNum % 3 != 0 || tStringNum == 0)
                {
                    XTF[i, j - 1] = 0.00f;
                    YTF[i, j - 1] = 0.00f;
                    ZTF[i, j - 1] = 0.00f;

                    XTB[i, j - 1] = 0.00f;
                    YTB[i, j - 1] = 0.00f;
                    ZTB[i, j - 1] = 0.00f;

                    if (tStringNum > 0)
                    {
                        ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fx= 0.00");
                        ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fy= 0.00");
                        ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fz= 0.00");

                        ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= 0.00");
                        ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= 0.00");
                        ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= 0.00");
                        ProcessString.Add(" ");
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
                if (tStringNum % 3 != 0 || tStringNum == 0)
                {
                    XTF[i, j - 1] = 0.00f;
                    YTF[i, j - 1] = 0.00f;
                    ZTF[i, j - 1] = 0.00f;

                    XTC[i, j - 1] = 0.00f;
                    YTC[i, j - 1] = 0.00f;
                    ZTC[i, j - 1] = 0.00f;

                    XTB[i, j - 1] = 0.00f;
                    YTB[i, j - 1] = 0.00f;
                    ZTB[i, j - 1] = 0.00f;

                    if (tStringNum % 3 == 0 && tStringNum > 0)
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
                        ProcessString.Add(" ");
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
                if (tStringNum % 3 != 0 || tStringNum == 0)
                {
                    //已吊，中间吊装
                    //20160903 单侧吊装按单个串荷考虑  中间吊装考虑前后侧比例  只针对跳线串数不为3的情况

                    XTF[i, j - 1] = formula.ZLX(out string strXTF, angle, 0, c[7], 0, BL3, tStringNum, HPSettingParas.BLTQ);
                    YTF[i, j - 1] = formula.ZLY(out string strYTF, angle, 0, c[7], 0, tStringNum, HPSettingParas.BLTQ);
                    ZTF[i, j - 1] = formula.ZLT2Z(out string strZTF, c[8], tStringNum, 0, HPSettingParas.BLTQ);

                    if (tStringNum > 0)
                    {
                        ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fx= " + strXTF);
                        ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fy= " + strYTF);
                        ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fz= " + strZTF);
                    }

                    XTB[i, j - 1] = formula.ZLX(out string strXTB, angle, 0, c[7], 0, BL3, tStringNum, HPSettingParas.BLTH);
                    YTB[i, j - 1] = formula.ZLY(out string strYTB, angle, 0, c[7], 0, tStringNum, HPSettingParas.BLTH);
                    ZTB[i, j - 1] = formula.ZLT2Z(out string strZTB, c[8], tStringNum, 0, HPSettingParas.BLTH);

                    if (tStringNum > 0)
                    {
                        ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= " + strXTB);
                        ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= " + strYTB);
                        ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= " + strZTB);
                        ProcessString.Add(" ");
                    }
                }
                else if (tStringNum % 3 == 0 && tStringNum > 0)
                {
                    XTF[i, j - 1] = formula.ZLX(out string strXTF, angle, 0, c[7], 0, BL3, tStringNum, HPSettingParas.BLTQ);
                    YTF[i, j - 1] = formula.ZLY(out string strYTF, angle, 0, c[7], 0, tStringNum, HPSettingParas.BLTQ);
                    ZTF[i, j - 1] = formula.ZLT2Z(out string strZTF, c[8], tStringNum, 0, HPSettingParas.BLTQ);

                    ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fx= " + strXTF);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fy= " + strYTF);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fz= " + strZTF);

                    XTC[i, j - 1] = formula.ZLX(out string strXTC, angle, 0, c[7], 0, BL3, tStringNum, HPSettingParas.BLTZ);
                    YTC[i, j - 1] = formula.ZLY(out string strYTC, angle, 0, c[7], 0, tStringNum, HPSettingParas.BLTZ);
                    ZTC[i, j - 1] = formula.ZLT2Z(out string strZTC, c[8], tStringNum, 0, HPSettingParas.BLTZ);
                        
                    ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fx= " + strXTC);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fy= " + strYTC);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fz= " + strZTC);

                    XTB[i, j - 1] = formula.ZLX(out string strXTB, angle, 0, c[7], 0, BL3, tStringNum, HPSettingParas.BLTH);
                    YTB[i, j - 1] = formula.ZLY(out string strYTB, angle, 0, c[7], 0, tStringNum, HPSettingParas.BLTH);
                    ZTB[i, j - 1] = formula.ZLT2Z(out string strZTB, c[8], tStringNum, 0, HPSettingParas.BLTH); 

                    ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= " + strXTB);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= " + strYTB);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= " + strZTB);
                    ProcessString.Add(" ");
                }
            }
            else if (zhs > 2000 && zhs.ToString().Length == 4)
            {
                //单侧吊装未吊  双I 双V                            
                if (tStringNum % 3 == 0 && tStringNum > 0)
                {
                    //'中间吊装正吊
                    fhn = 1;
                    XTF[i, j - 1] = formula.ZLX(out string strXTF, angle, 0, c[7], 0, BL3, tStringNum, HPSettingParas.BLTQ);
                    YTF[i, j - 1] = formula.ZLY(out string strYTF, angle, 0, c[7], 0, tStringNum, HPSettingParas.BLTQ);
                    ZTF[i, j - 1] = formula.ZLT1Z(out string strZTF, c[8], tStringNum, fh_2, fhn, HPSettingParas.BLTQ);
                    
                    ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fx= " + strXTF);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fy= " + strYTF);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fz= " + strZTF);

                    XTC[i, j - 1] = formula.ZLX(out string strXTC, angle, 0, c[7], 0, BL3, tStringNum, HPSettingParas.BLTZ);
                    YTC[i, j - 1] = formula.ZLY(out string strYTC, angle, 0, c[7], 0, tStringNum, HPSettingParas.BLTZ);
                    ZTC[i, j - 1] = formula.ZLT1Z(out string strZTC, c[8], tStringNum, fh_2, fhn, HPSettingParas.BLTZ);

                    ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fx= " + strXTC);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fy= " + strYTC);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fz= " + strZTC);

                    XTB[i, j - 1] = formula.ZLX(out string strXTB, angle, 0, c[7], 0, BL3, tStringNum, HPSettingParas.BLTH);
                    YTB[i, j - 1] = formula.ZLY(out string strYTB, angle, 0, c[7], 0, tStringNum, HPSettingParas.BLTH);
                    ZTB[i, j - 1] = formula.ZLT1Z(out string strZTB, c[8], tStringNum, fh_2, fhn, HPSettingParas.BLTH);

                    ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= " + strXTB);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= " + strYTB);
                    ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= " + strZTB);
                    ProcessString.Add(" ");

                }
                else if (tStringNum % 3 != 0 || tStringNum == 0)
                {
                    //20160903 单侧吊装按单个串荷考虑  中间吊装考虑前后侧比例  只针对跳线串数不为3的情况
                    fhn = 1;
                    XTF[i, j - 1] = formula.ZLX(out string strXTF, angle, 0, c[7], 0, BL3, tStringNum, HPSettingParas.BLTQ);
                    YTF[i, j - 1] = formula.ZLY(out string strYTF, angle, 0, c[7], 0, tStringNum, HPSettingParas.BLTQ);
                    ZTF[i, j - 1] = formula.ZLT1Z(out string strZTF, c[8], tStringNum, fh_2, fhn, HPSettingParas.BLTQ);

                    if(tStringNum > 0)
                    {
                        ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fx= " + strXTF);
                        ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fy= " + strYTF);
                        ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fz= " + strZTF);
                    }

                    XTB[i, j - 1] = formula.ZLX(out string strXTB, angle, 0, c[7], 0, BL3, tStringNum, HPSettingParas.BLTH);
                    YTB[i, j - 1] = formula.ZLY(out string strYTB, angle, 0, c[7], 0, tStringNum, HPSettingParas.BLTH);
                    ZTB[i, j - 1] = formula.ZLT1Z(out string strZTB, c[8], tStringNum, fh_2, fhn, HPSettingParas.BLTH);

                    if (tStringNum > 0)
                    {
                        ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= " + strXTB);
                        ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= " + strYTB);
                        ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= " + strZTB);
                        ProcessString.Add(" ");
                    }
                }
            }
            else if (zhs.ToString().Length == 5 && zhs.ToString().Substring(0, 1) == "1" && zhs.ToString().Substring(0, 3) != "100")
            {
                //单侧集中吊装   1AB0i系列    注意AB不能同时取1或2  排他
                fhn = 1;
                if(tStringNum == 0)
                {
                    XTF[i, j - 1] = 0.00f;
                    YTF[i, j - 1] = 0.00f;
                    ZTF[i, j - 1] = 0.00f;

                    XTB[i, j - 1] = 0.00f;
                    YTB[i, j - 1] = 0.00f;
                    ZTB[i, j - 1] = 0.00f;
                }
                else
                {
                    switch (zhs.ToString().Substring(1, 1))
                    {
                        case "0":
                            //未吊
                            XTF[i, j - 1] = 0.00f;
                            YTF[i, j - 1] = 0.00f;
                            ZTF[i, j - 1] = 0.00f;

                            if (j > groudWireNum)
                            {
                                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fx= 0.00");
                                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fy= 0.00");
                                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fz= 0.00");
                            }
                            break;
                        case "1":
                            //已吊
                            XTF[i, j - 1] = formula.ZLX(out string strXTF, angle, 0, c[7], 0, BL3, tStringNum);
                            YTF[i, j - 1] = formula.ZLY(out string strYTF, angle, 0, c[7], 0, tStringNum);
                            ZTF[i, j - 1] = formula.ZLT2Z(out string strZTF, c[8], tStringNum, 0);
                            if (j > groudWireNum)
                            {
                                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fx= " + strXTF);
                                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fy= " + strYTF);
                                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fz= " + strYTF);
                            }
                            break;
                        case "2":
                            //正吊
                            XTF[i, j - 1] = formula.ZLX(out string strXTF2, angle, 0, c[7], 0, BL3, tStringNum);
                            YTF[i, j - 1] = formula.ZLY(out string strYTF2, angle, 0, c[7], 0, tStringNum);
                            ZTF[i, j - 1] = formula.ZLT1Z(out string strZTF2, c[8], tStringNum, fh_2, fhn);
                            if (j > groudWireNum)
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
                            XTB[i, j - 1] = 0.00f;
                            YTB[i, j - 1] = 0.00f;
                            ZTB[i, j - 1] = 0.00f;

                            if (j > groudWireNum)
                            {
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= 0.00");
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= 0.00");
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= 0.00");
                                ProcessString.Add(" ");
                            }
                            break;
                        case "1":
                            //已吊
                            XTB[i, j - 1] = formula.ZLX(out string strXTB, angle, 0, c[7], 0, BL3, tStringNum);
                            YTB[i, j - 1] = formula.ZLY(out string strYTB, angle, 0, c[7], 0, tStringNum);
                            ZTB[i, j - 1] = formula.ZLT2Z(out string strZTB, c[8], tStringNum, 0);
                            if (j > groudWireNum)
                            {
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= " + strXTB);
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= " + strYTB);
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= " + strZTB);
                                ProcessString.Add(" ");
                            }
                            break;
                        case "2":
                            //正吊
                            XTB[i, j - 1] = formula.ZLX(out string strXTB1, angle, 0, c[7], 0, BL3, tStringNum);
                            YTB[i, j - 1] = formula.ZLY(out string strYTB1, angle, 0, c[7], 0, tStringNum);
                            ZTB[i, j - 1] = formula.ZLT1Z(out string strZTB1, c[8], tStringNum, fh_2, fhn);
                            if (j > groudWireNum)
                            {
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= " + strXTB1);
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= " + strYTB1);
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= " + strZTB1);
                                ProcessString.Add(" ");
                            }
                            break;
                    }
                }
            }
            else if (zhs.ToString().Length == 5 && zhs.ToString().Substring(0, 1) == "2" && zhs.ToString().Substring(0, 3) != "200")
            {
                //吊装荷载前后分开吊装  不区分吊串数  荷载直接按比例前后分配
                fhn = 1;
                if (tStringNum == 0)
                {
                    XTF[i, j - 1] = 0.00f;
                    YTF[i, j - 1] = 0.00f;
                    ZTF[i, j - 1] = 0.00f;

                    XTB[i, j - 1] = 0.00f;
                    YTB[i, j - 1] = 0.00f;
                    ZTB[i, j - 1] = 0.00f;
                }
                else
                {
                    switch (zhs.ToString().Substring(1, 1))
                    {
                        case "0":
                            //未吊
                            XTF[i, j - 1] = 0.00f;
                            YTF[i, j - 1] = 0.00f;
                            ZTF[i, j - 1] = 0.00f;

                            if (j > groudWireNum)
                            {
                                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fx= 0.00");
                                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fy= 0.00");
                                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fz= 0.00");
                            }
                            break;
                        case "1":
                            //已吊
                            XTF[i, j - 1] = formula.ZLX(out string strXTF, angle, 0, c[7], 0, BL3, tStringNum, HPSettingParas.BLDZTQ);
                            YTF[i, j - 1] = formula.ZLY(out string strYTF, angle, 0, c[7], 0, tStringNum, HPSettingParas.BLDZTQ);
                            ZTF[i, j - 1] = formula.ZLT2Z(out string strZTF, c[8], tStringNum, 0, HPSettingParas.BLDZTQ);
                            if (j > groudWireNum)
                            {
                                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fx= " + strXTF);
                                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fy= " + strYTF);
                                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fz= " + strZTF);
                            }
                            break;
                        case "2":
                            //正吊
                            XTF[i, j - 1] = formula.ZLX(out strXTF, angle, 0, c[7], 0, BL3, tStringNum, HPSettingParas.BLDZTQ);
                            YTF[i, j - 1] = formula.ZLY(out strYTF, angle, 0, c[7], 0, tStringNum, HPSettingParas.BLDZTQ);
                            ZTF[i, j - 1] = formula.ZLT1Z(out strZTF, c[8], tStringNum, fh_2, fhn, HPSettingParas.BLDZTQ, true);
                            if (j > groudWireNum)
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
                            XTB[i, j - 1] = 0.00f;
                            YTB[i, j - 1] = 0.00f;
                            ZTB[i, j - 1] = 0.00f;

                            if (j > groudWireNum)
                            {
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= 0.00");
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= 0.00");
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= 0.00");
                                ProcessString.Add(" ");
                            }
                            break;
                        case "1":
                            //已吊
                            XTB[i, j - 1] = formula.ZLX(out string strXTB, angle, 0, c[7], 0, BL3, tStringNum, HPSettingParas.BLDZTH);
                            YTB[i, j - 1] = formula.ZLY(out string strYTB, angle, 0, c[7], 0, tStringNum, HPSettingParas.BLDZTH);
                            ZTB[i, j - 1] = formula.ZLT2Z(out string strZTB, c[8], tStringNum * HPSettingParas.BLDZTH, 0);
                            if (j > groudWireNum)
                            {
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= " + strXTB);
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= " + strYTB);
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= " + strZTB);
                                ProcessString.Add(" ");
                            }
                            break;
                        case "2":
                            //正吊
                            XTB[i, j - 1] = formula.ZLX(out strXTB, angle, 0, c[7], 0, BL3, tStringNum, HPSettingParas.BLDZTH);
                            YTB[i, j - 1] = formula.ZLY(out strYTB, angle, 0, c[7], 0, tStringNum, HPSettingParas.BLDZTH);
                            ZTB[i, j - 1] = formula.ZLT1Z(out strZTB, c[8], tStringNum, fh_2, fhn, HPSettingParas.BLDZTH, true);
                            if (j > groudWireNum)
                            {
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= " + strXTB);
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= " + strYTB);
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= " + strZTB);
                                ProcessString.Add(" ");
                            }
                            break;
                    }
                }
            }
            else if (zhs.ToString().Length == 6 && zhs.ToString().Substring(0, 1) == "1" && zhs.ToString().Substring(0, 4) != "1000")
            {
                //3I或3V  单侧集中吊装
                fhn = 1;
                if (tStringNum == 0)
                {
                    XTF[i, j - 1] = 0.00f;
                    YTF[i, j - 1] = 0.00f;
                    ZTF[i, j - 1] = 0.00f;

                    XTC[i, j - 1] = 0.00f;
                    YTC[i, j - 1] = 0.00f;
                    ZTC[i, j - 1] = 0.00f;

                    XTB[i, j - 1] = 0.00f;
                    YTB[i, j - 1] = 0.00f;
                    ZTB[i, j - 1] = 0.00f;

                    //没跳线，不输出
                }
                else
                {
                    switch (zhs.ToString().Substring(1, 1))
                    {
                        case "0":
                            //未吊
                            XTF[i, j - 1] = 0.00f;
                            YTF[i, j - 1] = 0.00f;
                            ZTF[i, j - 1] = 0.00f;

                            if (j > groudWireNum)
                            {
                                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fx= 0.00");
                                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fy= 0.00");
                                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fz= 0.00");
                            }
                            break;
                        case "1":
                            //已吊
                            XTF[i, j - 1] = formula.ZLX(out string strXTF, angle, 0, c[7], 0, BL3, tStringNum);
                            YTF[i, j - 1] = formula.ZLY(out string strYTF, angle, 0, c[7], 0, tStringNum);
                            ZTF[i, j - 1] = formula.ZLT2Z(out string strZTF, c[8], tStringNum, 0);
                            if (j > groudWireNum)
                            {
                                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fx= " + strXTF);
                                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fy= " + strYTF);
                                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fz= " + strZTF);
                            }
                            break;
                        case "2":
                            //正吊
                            //  nt = 1   '此处错误
                            XTF[i, j - 1] = formula.ZLX(out strXTF, angle, 0, c[7], 0, BL3, tStringNum);
                            YTF[i, j - 1] = formula.ZLY(out strYTF, angle, 0, c[7], 0, tStringNum);
                            ZTF[i, j - 1] = formula.ZLT1Z(out strZTF, c[8], tStringNum, fh_2, fhn);
                            if (j > groudWireNum)
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
                            XTB[i, j - 1] = 0.00f;
                            YTB[i, j - 1] = 0.00f;
                            ZTB[i, j - 1] = 0.00f;
                            tStringNum = 1;

                            if (j > groudWireNum)
                            {
                                ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fx= 0.00");
                                ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fy= 0.00");
                                ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fz= 0.00");
                            }
                            break;
                        case "1":
                            //已吊
                            //   nt = 1    此处错误
                            XTB[i, j - 1] = formula.ZLX(out string strXTB, angle, 0, c[7], 0, BL3, tStringNum);
                            YTB[i, j - 1] = formula.ZLY(out string strYTB, angle, 0, c[7], 0, tStringNum);
                            ZTB[i, j - 1] = formula.ZLT2Z(out string strZTB, c[8], tStringNum, 0);
                            if (j > groudWireNum)
                            {
                                ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fx= " + strXTB);
                                ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fy= " + strYTB);
                                ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fz= " + strZTB);
                            }
                            break;
                        case "2":
                            //正吊         
                            //  nt = 1    '此处错误
                            XTB[i, j - 1] = formula.ZLX(out strXTB, angle, 0, c[7], 0, BL3, tStringNum);
                            YTB[i, j - 1] = formula.ZLY(out strYTB, angle, 0, c[7], 0, tStringNum);
                            ZTB[i, j - 1] = formula.ZLT1Z(out strZTB, c[8], tStringNum, fh_2, fhn);
                            if (j > groudWireNum)
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
                            XTB[i, j - 1] = 0.00f;
                            YTB[i, j - 1] = 0.00f;
                            ZTB[i, j - 1] = 0.00f;
                            tStringNum = 1;

                            if (j > groudWireNum)
                            {
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= 0.00");
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= 0.00");
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= 0.00");
                                ProcessString.Add(" ");
                            }
                            break;
                        case "1":
                            //已吊
                            //   nt = 1    此处错误
                            XTB[i, j - 1] = formula.ZLX(out string strXTB, angle, 0, c[7], 0, BL3, tStringNum);
                            YTB[i, j - 1] = formula.ZLY(out string strYTB, angle, 0, c[7], 0, tStringNum);
                            ZTB[i, j - 1] = formula.ZLT2Z(out string strZTB, c[8], tStringNum, 0);
                            if (j > groudWireNum)
                            {
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= " + strXTB);
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= " + strYTB);
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= " + strZTB);
                                ProcessString.Add(" ");
                            }
                            break;
                        case "2":
                            //正吊         
                            //  nt = 1    '此处错误
                            XTB[i, j - 1] = formula.ZLX(out strXTB, angle, 0, c[7], 0, BL3, tStringNum);
                            YTB[i, j - 1] = formula.ZLY(out strYTB, angle, 0, c[7], 0, tStringNum);
                            ZTB[i, j - 1] = formula.ZLT1Z(out strZTB, c[8], tStringNum, fh_2, fhn);
                            if (j > groudWireNum)
                            {
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= " + strXTB);
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= " + strYTB);
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= " + strZTB);
                                ProcessString.Add(" ");
                            }
                            break;
                    }
                }
            }
            else if (zhs.ToString().Length == 6 && zhs.ToString().Substring(0, 1) == "2" && zhs.ToString().Substring(0, 4) != "2000")
            {
                //3I或3V   分开吊装
                fhn = 1;
                if (tStringNum == 0)
                {
                    XTF[i, j - 1] = 0.00f;
                    YTF[i, j - 1] = 0.00f;
                    ZTF[i, j - 1] = 0.00f;

                    XTC[i, j - 1] = 0.00f;
                    YTC[i, j - 1] = 0.00f;
                    ZTC[i, j - 1] = 0.00f;

                    XTB[i, j - 1] = 0.00f;
                    YTB[i, j - 1] = 0.00f;
                    ZTB[i, j - 1] = 0.00f;

                    //没跳线，不输出
                }
                else
                {
                    switch (zhs.ToString().Substring(1, 1))
                    {
                        case "0":
                            //未吊
                            XTF[i, j - 1] = 0.00f;
                            YTF[i, j - 1] = 0.00f;
                            ZTF[i, j - 1] = 0.00f;

                            if (j > groudWireNum)
                            {
                                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fx= 0.00");
                                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fy= 0.00");
                                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fz= 0.00");
                            }
                            break;
                        case "1":
                            //已吊
                            XTF[i, j - 1] = formula.ZLX(out string strXTF, angle, 0, c[7], 0, BL3, tStringNum, HPSettingParas.BLDZTQ);
                            YTF[i, j - 1] = formula.ZLY(out string strYTF, angle, 0, c[7], 0, tStringNum, HPSettingParas.BLDZTQ);
                            ZTF[i, j - 1] = formula.ZLT2Z(out string strZTF, c[8], tStringNum, 0, HPSettingParas.BLDZTQ);
                            if (j > groudWireNum)
                            {
                                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fx= " + strXTF);
                                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fy= " + strYTF);
                                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fz= " + strZTF);
                            }
                            break;
                        case "2":
                            //正吊
                            //  nt = 1   
                            XTF[i, j - 1] = formula.ZLX(out strXTF, angle, 0, c[7], 0, BL3, tStringNum, HPSettingParas.BLDZTQ);
                            YTF[i, j - 1] = formula.ZLY(out strYTF, angle, 0, c[7], 0, tStringNum, HPSettingParas.BLDZTQ);
                            ZTF[i, j - 1] = formula.ZLT1Z(out strZTF, c[8], tStringNum, fh_2, fhn, HPSettingParas.BLDZTQ, true);
                            if (j > groudWireNum)
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
                            XTC[i, j - 1] = 0.00f;
                            YTC[i, j - 1] = 0.00f;
                            ZTC[i, j - 1] = 0.00f;
                            //nt = 1

                            if (j > groudWireNum)
                            {
                                ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fx= 0.00");
                                ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fy= 0.00");
                                ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fz= 0.00");
                            }
                            break;
                        case "1":
                            //已吊
                            //   nt = 1 
                            XTC[i, j - 1] = formula.ZLX(out string strXTC, angle, 0, c[7], 0, BL3, tStringNum, HPSettingParas.BLDZTZ);
                            YTC[i, j - 1] = formula.ZLY(out string strYTC, angle, 0, c[7], 0, tStringNum, HPSettingParas.BLDZTZ);
                            ZTC[i, j - 1] = formula.ZLT2Z(out string strZTC, c[8], tStringNum, 0, HPSettingParas.BLDZTZ);
                            if (j > groudWireNum)
                            {
                                ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fx= " + strXTC);
                                ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fy= " + strYTC);
                                ProcessString.Add(Template.Wires[j - 1] + "跳线中部 Fz= " + strZTC);
                            }
                            break;
                        case "2":
                            //正吊         
                            //  nt = 1    
                            XTC[i, j - 1] = formula.ZLX(out strXTC, angle, 0, c[7], 0, BL3, tStringNum, HPSettingParas.BLDZTZ);
                            YTC[i, j - 1] = formula.ZLY(out strYTC, angle, 0, c[7], 0, tStringNum, HPSettingParas.BLDZTZ);
                            ZTC[i, j - 1] = formula.ZLT1Z(out strZTC, c[8], tStringNum, fh_2, fhn, HPSettingParas.BLDZTZ, true);
                            if (j > groudWireNum)
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
                            XTB[i, j - 1] = 0.00f;
                            YTB[i, j - 1] = 0.00f;
                            ZTB[i, j - 1] = 0.00f;
                            //nt = 1

                            if (j > groudWireNum)
                            {
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= 0.00");
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= 0.00");
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= 0.00");
                                ProcessString.Add(" ");
                            }
                            break;
                        case "1":
                            //已吊
                            //   nt = 1    此处错误
                            XTB[i, j - 1] = formula.ZLX(out string strXTB, angle, 0, c[7], 0, BL3, tStringNum, HPSettingParas.BLDZTH);
                            YTB[i, j - 1] = formula.ZLY(out string strYTB, angle, 0, c[7], 0, tStringNum, HPSettingParas.BLDZTH);
                            ZTB[i, j - 1] = formula.ZLT2Z(out string strZTB, c[8], tStringNum, 0, HPSettingParas.BLDZTH);
                            if (j > groudWireNum)
                            {
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= " + strXTB);
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= " + strYTB);
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= " + strZTB);
                                ProcessString.Add(" ");
                            }
                            break;
                        case "2":
                            //正吊         
                            //  nt = 1    '此处错误
                            XTB[i, j - 1] = formula.ZLX(out strXTB, angle, 0, c[7], 0, BL3, tStringNum, HPSettingParas.BLDZTH);
                            YTB[i, j - 1] = formula.ZLY(out strYTB, angle, 0, c[7], 0, tStringNum, HPSettingParas.BLDZTH);
                            ZTB[i, j - 1] = formula.ZLT1Z(out strZTB, c[8], tStringNum, fh_2, fhn, HPSettingParas.BLDZTH, true);
                            if (j > groudWireNum)
                            {
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= " + strXTB);
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= " + strYTB);
                                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= " + strZTB);
                                ProcessString.Add(" ");
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

            WorkConditionCombo wd = Template.WorkConditionCombos[i];

            int zhs = wd.WireIndexCodes[j-1], zhsx;
            int angle = wd.WindDirectionCode;
            string workConditionCode = wd.WorkConditionCode;
            int BL3 = LineParas.isTurnRight ? 1 : -1;
            int tStringNum = LineParas.TstringNum;
            float AngleMax = LineParas.AngleMax / 2;
            float AngleMin = LineParas.AngleMin / 2;
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
                e1 = x11; e2 = x21; e3 = AngleMax;
            }
            //else if (wd.WorkConditionCode == "M2")
            else
            {
                e1 = x12; e2 = x22; e3 = AngleMin;
            }

            if (wd.TensionAngleCode == "D0")
            {
                switch (wd.VertialLoadCode)
                {
                    case "Y0":
                        if(Paras.IsCornerTower || Paras.IsBranchTower)
                        {
                            c[1] = e1 ; c[2] = Math.Max(y1, LineParas.AnchorTension) ; c[3] = z11;
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
                XLF[i, j - 1] = formula.ZM1X(out string strXLF, angle, e3, c[1], c[2], BL3);
                YLF[i, j - 1] = formula.ZM1Y(out string strYLF, angle, e3, c[1], c[2]);
                ZLF[i, j - 1] = formula.ZM1Z(out string strZLF, c[3], fhn, fh_1);
                
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

                XLF[i, j - 1] = formula.ZM2X(out string strXLF, angle, e3, c[1], c[2], BL3);
                YLF[i, j - 1] = formula.ZM2Y(out string strYLF, angle, e3, c[1], c[2]);
                ZLF[i, j - 1] = formula.ZM2Z(out string strZLF, c[3]);

                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fx= " + strXLF);
                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fy= " + strYLF);
                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fz= " + strZLF);

            }
            else if(zhs == 0)
            {
                XLF[i, j - 1] = 0.00f;
                YLF[i, j - 1] = 0.00f;
                ZLF[i, j - 1] = 0.00f;

                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fx= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fy= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fz= 0.00");
            }

            XLB[i, j - 1] = 0.00f;
            YLB[i, j - 1] = 0.00f;
            ZLB[i, j - 1] = 0.00f;

            ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fx= 0.00");
            ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fy= 0.00");
            ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fz= 0.00");
            ProcessString.Add(" ");

            XTF[i, j - 1] = 0.00f;
            YTF[i, j - 1] = 0.00f;
            ZTF[i, j - 1] = 0.00f;

            XTC[i, j - 1] = 0.00f;
            YTC[i, j - 1] = 0.00f;
            ZTC[i, j - 1] = 0.00f;

            XTB[i, j - 1] = 0.00f;
            YTB[i, j - 1] = 0.00f;
            ZTB[i, j - 1] = 0.00f;

            if (tStringNum % 3 != 0)
            {
                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fx= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fy= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fz= 0.00");

                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= 0.00");
                ProcessString.Add(" ");
            }
            else if(tStringNum % 3 == 0 && tStringNum > 0)
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
                ProcessString.Add(" ");
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

            WorkConditionCombo wd = Template.WorkConditionCombos[i];

            int zhs = wd.WireIndexCodes[j - 1];
            int angle = wd.WindDirectionCode;
            string workConditionCode = wd.WorkConditionCode;
            int BL3 = LineParas.isTurnRight ? 1 : -1;
            int tStringNum = LineParas.TstringNum;
            float AngleMax = LineParas.AngleMax / 2;
            float AngleMin = LineParas.AngleMin / 2;
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
                e1 = x11 ; e2 = x21 ; e3 = AngleMax;
            }
            //else if (wd.WorkConditionCode == "J2")
            else
            {
                e1 = x12 ; e2 = x22 ; e3 = AngleMin;
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
                XLF[i, j - 1] = formula.JX(out string strXLF, angle, c[1], Paras.DynamicCoef, LineParas.DrawingCoef, c[2], e3, BL3);
                YLF[i, j - 1] = formula.JY(out string strYLF, angle, c[1], Paras.DynamicCoef, LineParas.DrawingCoef, c[2], e3);
                ZLF[i, j - 1] = formula.JZ(out string strZLF, c[3], Paras.DynamicCoef, LineParas.DrawingCoef, c[2], fh_1, fhn);


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

                XLF[i, j - 1] = formula.JX(out string strXLF, angle, c[1], 1.0f, LineParas.DrawingCoef, c[2], e3, BL3);
                YLF[i, j - 1] = formula.JY(out string strYLF, angle, c[1], 1.0f, LineParas.DrawingCoef, c[2], e3);
                ZLF[i, j - 1] = formula.JZ(out string strZLF, c[3], 1.0f, LineParas.DrawingCoef, c[2], fh_1, fhn);

                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fx= " + strXLF);
                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fy= " + strYLF);
                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fz= " + strZLF);

            }
            else if (zhs == 0)
            {
                XLF[i, j - 1] = 0.00f;
                YLF[i, j - 1] = 0.00f;
                ZLF[i, j - 1] = 0.00f;

                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fx= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fy= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fz= 0.00");
            }

            XLB[i, j - 1] = 0.00f;
            YLB[i, j - 1] = 0.00f;
            ZLB[i, j - 1] = 0.00f;

            ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fx= 0.00");
            ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fy= 0.00");
            ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fz= 0.00");

            XTF[i, j - 1] = 0.00f;
            YTF[i, j - 1] = 0.00f;
            ZTF[i, j - 1] = 0.00f;

            XTC[i, j - 1] = 0.00f;
            YTC[i, j - 1] = 0.00f;
            ZTC[i, j - 1] = 0.00f;

            XTB[i, j - 1] = 0.00f;
            YTB[i, j - 1] = 0.00f;
            ZTB[i, j - 1] = 0.00f;

            if (tStringNum % 3 != 0)
            {
                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fx= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fy= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fz= 0.00");

                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= 0.00");
            }
            else if (tStringNum % 3 == 0 && tStringNum > 0)
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

            WorkConditionCombo wd = Template.WorkConditionCombos[i];

            int zhs = wd.WireIndexCodes[j - 1];
            int angle = wd.WindDirectionCode;
            string workConditionCode = wd.WorkConditionCode;
            int BL3 = LineParas.isTurnRight ? 1 : -1;
            int tStringNum = LineParas.TstringNum;
            float AngleMax = LineParas.AngleMax / 2;
            float AngleMin = LineParas.AngleMin / 2;
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
                e1 = x11 ; e2 = x21 ; e3 = AngleMax;
            }
            //else if (wd.WorkConditionCode == "MQ2")
            else
            {
                e1 = x12 ; e2 = x22 ; e3 = AngleMin;
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

                XLF[i, j - 1] = formula.MQ1X(out string strXLF, angle, e3, c[1], c[2], BL3);
                YLF[i, j - 1] = formula.MQ1Y(out string strYLF, angle, e3, c[1], c[2]);
                ZLF[i, j - 1] = formula.MQ1Z(out string strZLF, c[3], c[2], fhn, fh_1);

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

                XLB[i, j - 1] = formula.MQ2X(out string strXLB, angle, e3, c[1], c[2], BL3);
                YLB[i, j - 1] = formula.MQ2Y(out string strYLB, angle, e3, c[1], c[2]);
                ZLB[i, j - 1] = formula.MQ2Z(out string strZLB, c[3]);

                ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fx= " + strXLB);
                ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fy= " + strYLB);
                ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fz= " + strZLB);
                ProcessString.Add(" ");
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

                XLF[i, j - 1] = formula.MQ3X(out string strXLF, angle, e3, c[1], c[2], BL3);
                YLF[i, j - 1] = formula.MQ3Y(out string strYLF, angle, e3, c[1], c[2]);
                ZLF[i, j - 1] = formula.MQ3Z(out string strZLF, c[3]);

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

                XLB[i, j - 1] = formula.MQ3X(out string strXLB, angle, e3, c[1], c[2], BL3);
                YLB[i, j - 1] = formula.MQ3Y(out string strYLB, angle, e3, c[1], -c[2]);
                ZLB[i, j - 1] = formula.MQ3Z(out string strZLB, c[3]);

                ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fx= " + strXLB);
                ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fy= " + strYLB);
                ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fz= " + strZLB);
                ProcessString.Add(" ");
            }
            else if (zhs < 100 && zhs > 0)
            {
                //其余未挂
                if (Paras.IsBranchTower)
                {
                    //前侧转角  20161124分支塔添加
                    e3 = LineParas.AngleFront;
                }
                XLF[i, j - 1] = formula.MQ4X(out string strXLF, e3, BL3);
                YLF[i, j - 1] = formula.MQ4Y(out string strYLF, e3);
                ZLF[i, j - 1] = formula.MQ4Z(out string strZLF);

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

                XLB[i, j - 1] = formula.MQ2X(out string strXLB, angle, e3, c[1], c[2], BL3);
                YLB[i, j - 1] = formula.MQ2Y(out string strYLB, angle, e3, c[1], c[2]);
                ZLB[i, j - 1] = formula.MQ2Z(out string strZLB, c[3]);

                ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fx= " + strXLB);
                ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fy= " + strYLB);
                ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fz= " + strZLB);
                ProcessString.Add(" ");

            }
            else if (zhs == 0)
            {
                XLF[i, j - 1] = 0.00f;
                YLF[i, j - 1] = 0.00f;
                ZLF[i, j - 1] = 0.00f;

                XLB[i, j - 1] = 0.00f;
                YLB[i, j - 1] = 0.00f;
                ZLB[i, j - 1] = 0.00f;

                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fx= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fy= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "线条前侧 Fz= 0.00");

                ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fx= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fy= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fz= 0.00");
            }

            XTF[i, j - 1] = 0.00f;
            YTF[i, j - 1] = 0.00f;
            ZTF[i, j - 1] = 0.00f;

            XTC[i, j - 1] = 0.00f;
            YTC[i, j - 1] = 0.00f;
            ZTC[i, j - 1] = 0.00f;

            XTB[i, j - 1] = 0.00f;
            YTB[i, j - 1] = 0.00f;
            ZTB[i, j - 1] = 0.00f;

            if (tStringNum % 3 != 0)
            {
                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fx= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fy= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fz= 0.00");

                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= 0.00");
            }
            else if (tStringNum % 3 == 0 && tStringNum > 0)
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

            WorkConditionCombo wd = Template.WorkConditionCombos[i];

            int zhs = wd.WireIndexCodes[j - 1];
            int angle = wd.WindDirectionCode;
            string workConditionCode = wd.WorkConditionCode;
            int mz1 = Template.WorkConditongs.Count;
            int BL3 = LineParas.isTurnRight ? 1 : -1;
            int tStringNum = LineParas.TstringNum;
            float AngleMax = LineParas.AngleMax / 2;
            float AngleMin = LineParas.AngleMin / 2;

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
                e1 = x11 ; e2 = x21 ; e3 = AngleMax;
            }
            //else if (wd.WorkConditionCode == "G2")
            else
            {
                e1 = x12 ; e2 = x22 ; e3 = AngleMin;
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

                    XLF[i, j - 1] = formula.MQ3X(out string strXLF, angle, e3, c[1], c[2], BL3);
                    YLF[i, j - 1] = formula.MQ3Y(out string strYLF, angle, e3, c[1], c[2]);
                    ZLF[i, j - 1] = formula.MQ3Z(out string strZLF, c[3]);

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

                    XLB[i, j - 1] = formula.MQ3X(out string strXLB, angle, e3, c[1], c[2], BL3);
                    YLB[i, j - 1] = formula.MQ3Y(out string strYLB, angle, e3, c[1], -c[2]);
                    ZLB[i, j - 1] = formula.MQ3Z(out string strZLB, c[3]);

                    ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fx= " + strXLB);
                    ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fy= " + strYLB);
                    ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fz= " + strZLB);
                    ProcessString.Add(" ");
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

                    XLF[i, j - 1] = formula.MQ3X(out string strXLF, angle, e3, c[1], c[2], BL3);
                    YLF[i, j - 1] = formula.MQ5Y(out string strYLF, LineParas.PulleyTensionDif);
                    ZLF[i, j - 1] = formula.MQ3Z(out string strZLF, c[3]);
    
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

                    XLB[i, j - 1] = formula.MQ3X(out string strXLB, angle, e3, c[1], c[2], BL3);
                    YLB[i, j - 1] = formula.MQ5Y(out string strYLB, LineParas.PulleyTensionDif);
                    ZLB[i, j - 1] = formula.MQ3Z(out string strZLB, c[3]);
    
                    ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fx= " + strXLB);
                    ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fy= " + strYLB);
                    ProcessString.Add(Template.Wires[j - 1] + "线条后侧 Fz= " + strZLB);
                    ProcessString.Add(" ");
                }
                else if (zhs == 0)
                {
                    XLF[i, j - 1] = 0.00f;
                    YLF[i, j - 1] = 0.00f;
                    ZLF[i, j - 1] = 0.00f;

                    XLB[i, j - 1] = 0.00f;
                    YLB[i, j - 1] = 0.00f;
                    ZLB[i, j - 1] = 0.00f;

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

            XTF[i, j - 1] = 0.00f;
            YTF[i, j - 1] = 0.00f;
            ZTF[i, j - 1] = 0.00f;

            XTC[i, j - 1] = 0.00f;
            YTC[i, j - 1] = 0.00f;
            ZTC[i, j - 1] = 0.00f;

            XTB[i, j - 1] = 0.00f;
            YTB[i, j - 1] = 0.00f;
            ZTB[i, j - 1] = 0.00f;

            if (tStringNum % 3 != 0)
            {
                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fx= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fy= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "跳线前侧 Fz= 0.00");

                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fx= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fy= 0.00");
                ProcessString.Add(Template.Wires[j - 1] + "跳线后侧 Fz= 0.00");
            }
            else if (tStringNum % 3 == 0 && tStringNum > 0)
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


        public override List<StruCalsPointLoad> CalsPointsLoad(string path)
        {
            List<StruCalsPointLoad> pointsLoad = new List<StruCalsPointLoad>();

            for(int wireIndex = 0; wireIndex < HPSettingParas.InstallXYPoints.Count; wireIndex++)
            {
                var stringType = HPSettingParas.NormalZPoints.Where(item => item.WireType == HPSettingParas.InstallXYPoints[wireIndex].WireType).First().StringType;
                HPSettingParas.InstallXYPoints[wireIndex].StringType = stringType;
                HPSettingParas.InstallZPoints[wireIndex].StringType = stringType;
            } 

            Process2String.Add("荷载分配明细表 生成时间: " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
            Process2String.Add(" ");

            int i = 0;
            foreach (var wireItem in Template.Wires)
            {
                pointsLoad.AddRange(CalsPointsLoadInWire(i));
                i++;
            }

            i = 0;
            foreach (var wireItem in Template.Wires)
            {
                pointsLoad.AddRange(CalsPointsLoadInTWire(i));
                i++;
            }

            using (FileStream fileStream = File.OpenWrite(path))
            {
                using (StreamWriter writer = new StreamWriter(fileStream))
                {
                    foreach (string s in Process2String)
                    {
                        writer.WriteLine(s);
                    }
                    writer.Flush();
                    writer.Close();
                }
            }

            return pointsLoad;
        }


        protected List<StruCalsPointLoad> CalsPointsLoadInWire(int i)
        {
            List<StruCalsPointLoad> pointsLoad = new List<StruCalsPointLoad>();

            string wireItem = Template.Wires[i];

            List<string> positions = new List<string> { "前侧", "后侧" };

            foreach (var posItem in positions)
            {
                int j = 0;
                foreach (var wdItem in Template.WorkConditionCombos)
                {
                    string groupStr, linkStr;
                    List<HangingPointParas> points;

                    if (wireItem.Contains("地"))
                    {
                        groupStr = "第一组";
                        linkStr = "[常规导线挂点_dataTable]";
                        points = HPSettingParas.NormalXYPoints;
                    }
                    else
                    {
                        if ((wdItem.WorkConditionCode == "G1" || wdItem.WorkConditionCode == "G2") && Math.Abs(wdItem.WireIndexCodes[i]) > 100)
                        {
                            groupStr = "第五组";
                            linkStr = "[过滑车挂点_dataTable]";
                            points = HPSettingParas.TurningPoints;
                        }
                        else
                        {
                            groupStr = "第二组";
                            linkStr = "[常规导线挂点_dataTable]";
                            points = HPSettingParas.NormalXYPoints;
                        }
                    }

                    float[,] xx, yy, zz;

                    if(posItem == "前侧")
                    {
                        xx = XLF;
                        yy = YLF;
                        zz = ZLF;
                    }
                    else
                    {
                        xx = XLB;
                        yy = YLB;
                        zz = ZLB;
                    }

                    HangingPointLoadComposeCornerTower hPLoadComposeX = new HangingPointLoadComposeCornerTower(i, j, posItem, "X", xx, yy, zz, groupStr, linkStr, points, HPSettingParas, Template, DicGroup);
                    hPLoadComposeX.ComposeHangingPointsLoad(out string strX, out List<StruCalsPointLoad> pListX);

                    Process2String.Add(strX);
                    pointsLoad.AddRange(pListX);

                    HangingPointLoadComposeCornerTower hPLoadComposeY = new HangingPointLoadComposeCornerTower(i, j, posItem, "Y", xx, yy, zz, groupStr, linkStr, points, HPSettingParas, Template, DicGroup);
                    hPLoadComposeY.ComposeHangingPointsLoad(out string strY, out List<StruCalsPointLoad> pListY);

                    Process2String.Add(strY);
                    pointsLoad.AddRange(pListY);

                    HangingPointLoadComposeCornerTower hPLoadComposeZ = new HangingPointLoadComposeCornerTower(i, j, posItem, "Z", xx, yy, zz, groupStr, linkStr, points, HPSettingParas, Template, DicGroup);
                    hPLoadComposeZ.ComposeHangingPointsLoad(out string strZ, out List<StruCalsPointLoad> pListZ);

                    Process2String.Add(strZ);
                    pointsLoad.AddRange(pListZ);

                    j++;
                }
            }



            return pointsLoad;
        }

        protected List<StruCalsPointLoad> CalsPointsLoadInTWire(int i)
        {
            List<StruCalsPointLoad> pointsLoad = new List<StruCalsPointLoad>();

            string wireItem = Template.Wires[i];

            StruLineParas linePara = LineParasArr[i]; 

            List<string> positions = new List<string> { "前侧", "后侧" };

            if(linePara.TstringNum == 0)
            {
                return pointsLoad;
            }
            else if(linePara.TstringNum % 3 == 0)
            {
                positions = new List<string> { "前侧", "中部", "后侧" };
            }
            else
            {
                positions = new List<string> { "前侧", "后侧" };
            }

            foreach (var posItem in positions)
            {
                int j = 0;
                foreach (var wdItem in Template.WorkConditionCombos)
                {
                    string groupStr, linkStrXY, linkStrZ;
                    List<HangingPointParas> pointsXY, pointsZ;


                    if ((wdItem.WorkConditionCode == "L1" || wdItem.WorkConditionCode == "L2" || wdItem.WorkConditionCode == "L1a" || wdItem.WorkConditionCode == "L2a") 
                        && Math.Abs(wdItem.WireIndexCodes[i]) > 1000)
                    {
                        groupStr = "第四组";
                        linkStrXY = "[吊装跳线挂点XY向_dataTable]";
                        pointsXY = HPSettingParas.InstallXYPoints;
                        linkStrZ = "[吊装跳线挂点Z向_dataTable]";
                        pointsZ = HPSettingParas.InstallZPoints;
                    }
                    else
                    {
                        groupStr = "第三组";
                        linkStrXY = "[常规跳线挂点_dataTable]";
                        pointsXY = HPSettingParas.NormalZPoints;
                        linkStrZ = "[常规跳线挂点_dataTable]";
                        pointsZ = HPSettingParas.NormalZPoints;
                    }

                    float[,] xx, yy, zz;

                    if (posItem == "前侧")
                    {
                        xx = XTF;
                        yy = YTF;
                        zz = ZTF;
                    }
                    else if(posItem == "中部")
                    {
                        xx = XTC;
                        yy = YTC;
                        zz = ZTC;
                    }
                    else
                    {
                        xx = XTB;
                        yy = YTB;
                        zz = ZTB;
                    }


                    HangingPointLoadComposeCornerTower hPLoadComposeX = new HangingPointLoadComposeCornerTower(i, j, posItem, "X", xx, yy, zz, groupStr, linkStrXY, pointsXY, HPSettingParas, Template, DicGroup);
                    hPLoadComposeX.ComposeHangingPointsLoad(out string strX, out List<StruCalsPointLoad> pListX);

                    Process2String.Add(strX);
                    pointsLoad.AddRange(pListX);

                    HangingPointLoadComposeCornerTower hPLoadComposeY = new HangingPointLoadComposeCornerTower(i, j, posItem, "Y", xx, yy, zz, groupStr, linkStrXY, pointsXY, HPSettingParas, Template, DicGroup);
                    hPLoadComposeY.ComposeHangingPointsLoad(out string strY, out List<StruCalsPointLoad> pListY);

                    Process2String.Add(strY);
                    pointsLoad.AddRange(pListY);

                    HangingPointLoadComposeCornerTower hPLoadComposeZ = new HangingPointLoadComposeCornerTower(i, j, posItem, "Z", xx, yy, zz, groupStr, linkStrZ, pointsZ, HPSettingParas, Template, DicGroup);
                    hPLoadComposeZ.ComposeHangingPointsLoad(out string strZ, out List<StruCalsPointLoad> pListZ);

                    Process2String.Add(strZ);
                    pointsLoad.AddRange(pListZ);

                    j++;
                }
            }

            return pointsLoad;
        }

        protected override string GetDicPath()
        {
            return Directory.GetCurrentDirectory() + "\\UserData\\HPCompose-TensionTower.xml";
            //return "D:\\01-代码\\TowerLoadCals\\TowerLoadCals\\UserData\\HPCompose-TensionTower.xml";
        }
    }
}
