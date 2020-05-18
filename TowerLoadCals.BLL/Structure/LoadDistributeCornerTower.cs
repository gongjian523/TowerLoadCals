using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerLoadCals.Mode;
using TowerLoadCals.Mode.Structure;

namespace TowerLoadCals.BLL.Structure
{
    public class LoadDistributeCornerTower:LoadDistributeBase
    {
        /// <summary>
        /// 线条前侧大角度风荷
        /// </summary>
        protected float[,] WindDF { get; set; }
        /// <summary>
        /// 线条前侧小角度风荷
        /// </summary>
        protected float[,] WindXF { get; set; }
        /// <summary>
        /// 线条后侧大角度风荷
        /// </summary>
        protected float[,] WindDB { get; set; }
        /// <summary>
        /// 线条后侧小角度风荷
        /// </summary>
        protected float[,] WindXB { get; set; }

        /// <summary>
        /// 前侧最大垂荷
        /// </summary>
        protected float[,] GMaxF { get; set; }
        /// <summary>
        /// 前侧最小垂荷
        /// </summary>
        protected float[,] GMinF { get; set; }
        /// <summary>
        /// 后侧最大垂荷
        /// </summary>
        protected float[,] GMaxB { get; set; }
        /// <summary>
        /// 后侧最小垂荷
        /// </summary>
        protected float[,] GMinB { get; set; }

        /// <summary>
        /// 大张力
        /// </summary>
        protected float[,] TensionMax { get; set; }
        /// <summary>
        /// 小张力
        /// </summary>
        protected float[,] TensionMin { get; set; }

        /// <summary>
        /// 跳线风荷，不区分跳线跳串
        /// </summary>
        protected float[,] WindTX { get; set; }
        /// <summary>
        /// 跳线垂荷，不区分跳线跳串
        /// </summary>
        protected float[,] GTX { get; set; }

        /// <summary>
        /// 45度风大张力
        /// </summary>
        protected float[,] Tension45D { get; set; }
        /// <summary>
        /// 45度风小张力
        /// </summary>
        protected float[,] Tension45X { get; set; }

        protected float[,] XLF { get; set; }
        protected float[,] YLF { get; set; }
        protected float[,] ZLF { get; set; }

        protected float[,] XLB { get; set; }
        protected float[,] YLB { get; set; }
        protected float[,] ZLB { get; set; }

        protected float[,] XTF { get; set; }
        protected float[,] YTF { get; set; }
        protected float[,] ZTF { get; set; }

        protected float[,] XTC { get; set; }
        protected float[,] YTC { get; set; }
        protected float[,] ZTC { get; set; }

        protected float[,] XTB { get; set; }
        protected float[,] YTB { get; set; }
        protected float[,] ZTB { get; set; }



        public LoadDistributeCornerTower(FormulaParas para, StruLineParas[] lineParas, TowerTemplate template, float[][] table); base(para, lineParas, template, table)
        {

            //ProcessString = new List<string>();
            //formula = new FormulaLineTower(para);

            //Paras = para;
            //LineParasArr = lineParas;
            //Template = template;

            //ConvertTable(table);
        }

        public LoadDistributeCornerTower(FormulaParas para, StruLineParas[] lineParas, TowerTemplate template); base(para, lineParas, template)
        {

            //ProcessString = new List<string>();
            //formula = new FormulaLineTower(para);

            //Paras = para;
            //LineParasArr = lineParas;
            //Template = template;

            GetTable("D:\\00-项目\\P-200325-杆塔负荷程序\\21.xlsx");
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

        protected override void ConvertTable(float[][] table)
        {
            throw new NotImplementedException();
        }

        protected override void GetTable(string path)
        {
            throw new NotImplementedException();
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
        /// 大小转角的覆冰、不均匀冰、验算冰、断线工况计算
        /// 工况代码"I1", "I2", "U1", "U2", "B1", "B2", "Y1", "Y2"
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
            int zhs = wd.WirdIndexCodes[j - 1], zhsMod;

            zhsMod = Math.Abs(zhs) % 100;

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

                if(wd.WorkConditionCode == "I1")
                {
                    e1 = x11; e2 = x21; e3 = maxangle; e4 = 2; e5 = Vcb_N; Vloadx = Vload; rg = Dload_b;
                }
                else if (wd.WorkConditionCode == "I2")
                {
                    e1 = x12; e2 = x22; e3 = minangle; e4 = 1; e5 = Vcb_N; Vloadx = Vload; rg = Dload_b;
                }
                else if(wd.WorkConditionCode == "U1")
                {
                    e1 = x11; e2 = x21; e3 = maxangle; e4 = 2; e5 = Vcb_U; Vloadx = Vload; rg = Dload_b;
                }
                else if(wd.WorkConditionCode == "U2")
                {
                    e1 = x12; e2 = x22; e3 = minangle; e4 = 1; e5 = Vcb_U; Vloadx = Vload; rg = Dload_b;
                }
                else if(wd.WorkConditionCode == "B1")
                {
                    e1 = x11; e2 = x21; e3 = maxangle; e4 = 2; e5 = Vcb_B; Vloadx = Vload; rg = Dload_b;
                }
                else if(wd.WorkConditionCode == "B2")
                {
                    e1 = x12; e2 = x22; e3 = minangle; e4 = 1; e5 = Vcb_B; Vloadx = Vload; rg = Dload_b;
                }
                else if(wd.WorkConditionCode == "Y1")
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

            if(Paras.baseParaFormRadioButton1 && zhs < 0)
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
                        
                        if(Math.Abs(zhs) > 1000)
                        {
                            //垂荷列数互换法则补充
                            //'特意针对垂荷组合为12情况
                            //仅适用于不均匀冰和验算不均匀冰工况，其余工况不适用。在检查模式中判断报错

                            int zhs1 = Convert.ToInt16(Math.Abs(zhs).ToString().Substring(1, 2));
                            float zzz11 = gmaxF(j, zhs1) * BL2;
                            float zzz12 = gminF(j, zhs1) * BL2;
                            float zzz21 = gmaxB(j, zhs1) * BL2;
                            float zzz22 = gminB(j, zhs1) * BL2;

                            if(Math.Abs(zhs).ToString().Substring(0, 1) == "1")
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
                                    Case "YB"
                                        c(1) = e1 : c(2) = y3 : c(3) = z11 : c(4) = e2 : c(5) = y4 : c(6) = z22 : c(7) = x3 : c(8) = z3 : c(9) = z011 : c(10) = z022 : c(11) = z03
                                    Case "0B"
                                        c(1) = e1 : c(2) = y3 : c(3) = 0 : c(4) = e2 : c(5) = y4 : c(6) = z22 : c(7) = x3 : c(8) = z3 : c(9) = 0 : c(10) = z022 : c(11) = z03
                                    Case "B0"
                                        c(1) = e1 : c(2) = y3 : c(3) = z12 : c(4) = e2 : c(5) = y4 : c(6) = 0 : c(7) = x3 : c(8) = z3 : c(9) = z012 : c(10) = 0 : c(11) = z03
                                    Case "Y0"
                                        c(1) = e1 : c(2) = y3 : c(3) = z11 : c(4) = e2 : c(5) = y4 : c(6) = 0 : c(7) = x3 : c(8) = z3 : c(9) = z011 : c(10) = 0 : c(11) = z03

                                    Case "BB"
                                        c(1) = e1 : c(2) = y3 : c(3) = z12 : c(4) = e2 : c(5) = y4 : c(6) = z22 : c(7) = x3 : c(8) = z3 : c(9) = z012 : c(10) = z022 : c(11) = z03
                                    Case "BY"
                                        c(1) = e1 : c(2) = y3 : c(3) = z12 : c(4) = e2 : c(5) = y4 : c(6) = z21 : c(7) = x3 : c(8) = z3 : c(9) = z012 : c(10) = z021 : c(11) = z03
                                    Case "0Y"
                                        c(1) = e1 : c(2) = y3 : c(3) = 0 : c(4) = e2 : c(5) = y4 : c(6) = z21 : c(7) = x3 : c(8) = z3 : c(9) = 0 : c(10) = z021 : c(11) = z03
                                        '   Case "00"
                                        '     c(1) = e1 : c(2) = y3 : c(3) = 0 : c(4) = e2 : c(5) = y4 : c(6) = 0 : c(7) = x3 : c(8) = z3 : c(9) = 0 : c(10) = 0 : c(11) = z03

                                        '2016.9.24 针对前后侧同时取某一侧的拔力新增,T表示某侧取相反侧上拔力计算

                                    Case "YT"
                                        c(1) = e1 : c(2) = y3 : c(3) = z11 : c(4) = e2 : c(5) = y4 : c(6) = z12 : c(7) = x3 : c(8) = z3 : c(9) = z011 : c(10) = z012 : c(11) = z03
                                    Case "0T"
                                        c(1) = e1 : c(2) = y3 : c(3) = 0 : c(4) = e2 : c(5) = y4 : c(6) = z12 : c(7) = x3 : c(8) = z3 : c(9) = 0 : c(10) = z012 : c(11) = z03
                                    Case "T0"
                                        c(1) = e1 : c(2) = y3 : c(3) = z22 : c(4) = e2 : c(5) = y4 : c(6) = 0 : c(7) = x3 : c(8) = z3 : c(9) = z022 : c(10) = 0 : c(11) = z03
                                    Case "TY"
                                        c(1) = e1 : c(2) = y3 : c(3) = z22 : c(4) = e2 : c(5) = y4 : c(6) = z21 : c(7) = x3 : c(8) = z3 : c(9) = z022 : c(10) = z021 : c(11) = z03
                                        '20190113新版程序
                    defa;
                }

            }
        }
        else if(wd .TensionAngleCode == "DX")
        {
            if(zhs >= 0)
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
                    y4 = mgdz;
                }
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
                        c(1) = e1; c(2) = y3; c(3) = z21; c(4) = e2; c(5) = y4; c(6) = z12; c(7) = x3; c(8) = z3; c(9) = z021; c(10) = z012; c(11) = z03; //根据酒湖线修改
                        break;
                    case "BB":
                        c(1) = e1; c(2) = y3; c(3) = z22; c(4) = e2; c(5) = y4; c(6) = z12; c(7) = x3; c(8) = z3; c(9) = z022; c(10) = z012; c(11) = z03;
                        break;
                    case "BY":
                        c(1) = e1; c(2) = y3; c(3) = z22; c(4) = e2; c(5) = y4; c(6) = z12; c(7) = x3; c(8) = z3; c(9) = z022; c(10) = z012; c(11) = z03; //根据酒湖线修改
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
                    //case "00":
                        // c(1) = e1; c(2) = y3; c(3) = 0; c(4) = e2; c(5) = y4; c(6) = 0; c(7) = x3; c(8) = z3; c(9) = 0; c(10) = 0; c(11) = z03;
                        //break;
                    case "YT":
                        c(1) = e1; c(2) = y3; c(3) = z12; c(4) = e2; c(5) = y4; c(6) = z11; c(7) = x3; c(8) = z3; c(9) = z012; c(10) = z011; c(11) = z03;
                        break;
                    case "0T":
                        c(1) = e1; c(2) = y3; c(3) = z12; c(4) = e2; c(5) = y4; c(6) = 0; c(7) = x3; c(8) = z3; c(9) = z012; c(10) = 0; c(11) = z03;
                        break;
                    case "T0":
                        c(1) = e1; c(2) = y3; c(3) = 0; c(4) = e2; c(5) = y4; c(6) = z22; c(7) = x3; c(8) = z3; c(9) = 0; c(10) = z022; c(11) = z03;
                        break;
                    case "TY":
                        c(1) = e1; c(2) = y3; c(3) = z21; c(4) = e2; c(5) = y4; c(6) = z22; c(7) = x3; c(8) = z3; c(9) = z021; c(10) = z022; c(11) = z03;
                        break;
                //20190113新版程序
                default:
                    c(1) = e1; c(2) = y3; c(4) = e2; c(5) = y4; c(7) = x3; c(8) = z3; c(11) = z03;
                    if(Math.Abs(zhs) > 10000)
                    {
                        //垂荷列数互换法则补充
                        //特意针对垂荷组合为12情况
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

                    }


                    break;
            }
            }

        }
    }
}
