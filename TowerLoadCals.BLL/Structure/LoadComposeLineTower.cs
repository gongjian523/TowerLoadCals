using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using TowerLoadCals.Common;
using TowerLoadCals.Mode;

namespace TowerLoadCals.BLL
{

    /// <summary>
    /// 直线塔的荷载分配
    /// </summary>
    public class LoadComposeLineTower:LoadComposeBase
    {
        protected float[,] Wind { get; set; }

        protected float[,] GMax { get; set; }
        protected float[,] GMin { get; set; }

        protected float[,] TensionMax { get; set; }
        protected float[,] TensionMin { get; set; }

        //protected FormulaLineTower ((FormulaLineTower)formula);

        public LoadComposeLineTower(StruCalseBaseParas para, StruLineParas[] lineParas, HangingPointSettingParas ratioParas, TowerTemplate template, string tablePath) 
            : base(para, lineParas, ratioParas, template, tablePath)
        {
            formula = new FormulaLineTower(para);
        }

        protected override void ReadElectricLoad(DataSet ds)
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

        public override void CalculateLoadDistribute(string path)
        {
            int calNums = Template.WorkConditionCombos.Count;

            XX = new float[calNums, Template.Wires.Count];
            YY = new float[calNums, Template.Wires.Count];
            ZZ = new float[calNums, Template.Wires.Count];

            XXT = new float[calNums, Template.Wires.Count];
            YYT = new float[calNums, Template.Wires.Count];
            ZZT = new float[calNums, Template.Wires.Count];

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
                    formula.SetStrLineParas(LineParas);

                    int zhs = wic;

                    switch (wd.WorkConditionCode)
                    {
                        case "N1":
                        case "N2":
                        case "D1":
                        case "D2":
                            DistributeInWindAndLowTemperature(i,j);
                            break;
                        case "I1":
                        case "I2":
                        case "U1":
                        case "U2":
                        case "B1":
                        case "B2":
                        case "Y1":
                        case "Y2":
                            DistributeInIce(i, j);
                            break;
                        case "T":
                            DistributeInJump(i, j);
                            break;
                        case "L":
                        case "La":
                        case "Lb":
                        case "Lc":
                        case "Ld":
                        case "Le":
                        case "Lf":
                        case "Lg":
                        case "Lh":
                            DistributeInLift(i, j);
                            break;
                        case "M":
                        case "Ma":
                        case "Mb":
                        case "Mc":
                        case "Md":
                        case "Me":
                        case "Mf":
                        case "Mg":
                        case "Mh":
                            DistributeInAnchor(i, j);
                            break;
                        case "MO":
                        case "MOa":
                        case "MOb":
                        case "MOc":
                        case "MOd":
                        case "MOe":
                        case "MOf":
                        case "MOg":
                        case "MOh":
                            DistributeInOPGWAnchor(i, j);
                            break;
                        case "G":
                        case "Ga":
                        case "Gb":
                        case "Gc":
                        case "Gd":
                        case "Ge":
                        case "Gf":
                        case "Gg":
                        case "Gh":
                        case "GL":
                        case "GLa":
                        case "GLb":
                        case "GLc":
                        case "GLd":
                        case "GLe":
                        case "GLf":
                        case "GLg":
                        case "GLh":
                            DistributeInHuache(i, j);
                            break;
                        case "C":
                        case "Ca":
                        case "Cb":
                        case "Cc":
                        case "Cd":
                        case "Ce":
                        case "Cf":
                        case "Cg":
                        case "Ch":
                        case "CL":
                        case "CLa":
                        case "CLb":
                        case "CLc":
                        case "CLd":
                        case "CLe":
                        case "CLf":
                        case "CLg":
                        case "CLh":
                            DistributeInTixian(i, j);
                            break;
                        default:
                            throw new Exception("直线塔工况代号超出范围" + "0 + 16" + "错误：1-217");
                            //break;
                    }
                    j++;
                }
            }

            FileUtils.TextSaveByLine(path, ProcessString);
        }

        /// <summary>
        /// 针对大风和低温的工况
        /// 工况代码"N1", "N2", "D1", "D2"
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        protected void DistributeInWindAndLowTemperature(int i, int j)
        {
            float x1, y1, y2, z1, z2;
            float rg, zg, Vcb;

            WorkConditionCombo wd = Template.WorkConditionCombos[i];

            int zhs = wd.WireIndexCodes[j-1];
            int angle = wd.WindDirectionCode;
            string workConditionCode = wd.WorkConditionCode;

            if (zhs <= workConditionNum && zhs > 0)
            {
                x1 = Wind[j,zhs];
                y1 = TensionMax[j, zhs];
                y2 = TensionMin[j, zhs];
                z1 = GMax[j, zhs];
                z2 = GMin[j, zhs];
            }
            else if (zhs < 0 && zhs >= -workConditionNum)
            {
                x1 = Wind[j, Math.Abs(zhs)];
                y1 = TensionMin[j, Math.Abs(zhs)];
                y2 = TensionMax[j, Math.Abs(zhs)];
                z1 = GMax[j,Math.Abs(zhs)];
                z2 = GMin[j,Math.Abs(zhs)];
            }
            else if (zhs == 0)
            {
                x1 = 0;
                y1 = 0;
                y2 = 0;
                z1 = 0;
                z2 = 0;
            }
            else
            {
                x1 = 9999;
                y1 = 9999;
                y2 = 9999;
                z1 = 9999;
                z2 = 9999;
                throw new Exception("第　" + i + "　工况，第 " + j + " 线条组合参数错误，只能为-" + -workConditionNum + "～" + workConditionNum + "之间 " + "0 + 16 " + "错误：1-207");
            }

            if (workConditionCode == "N1")
            {
                rg = Paras.RGBad;
                zg = z1;
                Vcb = Paras.VcFNormal;
            }
            else if (workConditionCode == "N2")
            {
                rg = Paras.RGGood;
                zg = z2;
                Vcb = Paras.VcFNormal;
            }
            else if (workConditionCode == "D1")
            {
                rg = Paras.RGBad;
                zg = z1;
                Vcb = Paras.VcFCold;
            }
            else
            {
                rg = Paras.RGGood;
                zg = z2;
                Vcb = Paras.VcFCold;
            }

            XX[i,j-1] = ((FormulaLineTower)formula).ZXNX(angle, x1, Vcb, out string strX);
            YY[i,j-1] = ((FormulaLineTower)formula).ZXNY(angle, x1, y1, y2, Vcb, out string strY);
            ZZ[i,j-1] = ((FormulaLineTower)formula).ZXNZ(zg, rg, Vcb, out string strZ);

            GenerateLineLoadString(j, strX, strY, strZ);
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
            float x1, y1, y2, z1, z2, z3, z4;
            float rg, zg, zg1, fhn, Vloadx;

            WorkConditionCombo wd = Template.WorkConditionCombos[i];

            int zhs = wd.WireIndexCodes[j-1];
            int angle = wd.WindDirectionCode;
            string workConditionCode = wd.WorkConditionCode;

            int zhsM = (Math.Abs(zhs)) % 1000;

            if (zhs > 0)
            {
                x1 = Wind[j, zhsM];
                y1 = TensionMax[j, zhsM];
                y2 = TensionMin[j, zhsM];
                z1 = GMax[j, zhsM];
                //第一列必须为正常运行大风工况
                z2 = GMax[j,1];
                z3 = GMin[j, zhsM];
                //第一列必须为正常运行大风工况
                z4 = GMin[j,1];
            }
            else if(zhs < 0)
            {
                x1 = Wind[j, zhsM];
                y1 = TensionMin[j, zhsM];
                y2 = TensionMax[j, zhsM];
                z1 = GMax[j, zhsM];
                //第一列必须为正常运行大风工况
                z2 = GMax[j, 1];
                z3 = GMin[j, zhsM];
                //第一列必须为正常运行大风工况
                z4 = GMin[j, 1];
            }
            else 
            {
                x1 = 0;
                y1 = 0;
                y2 = 0;
                z1 = 0;
                z2 = 0;
                z3 = 0;
                z4 = 0;
            }

            if(zhsM > workConditionNum)
            {
                x1 = 9999;
                y1 = 9999;
                y2 = 9999;
                z1 = 9999;
                z2 = 9999;
                z3 = 9999;
                z4 = 9999;
                throw new Exception("第　" + i + "　工况，第 " + j + " 线条组合参数错误，只能为-" + -workConditionNum + "～" + workConditionNum + "之间 " + "0 + 16 " + "错误：1-208");
            }

            if (workConditionCode == "I1")
            {
                rg = Paras.RGBad;
                zg = z1;
                zg1 = z2;
                fhn = Paras.VcFNormal;
                Vloadx = Paras.RQ;
            }
            else if (workConditionCode == "I2")
            {
                rg = Paras.RGGood;
                zg = z3;
                zg1 = z4;
                fhn = Paras.VcFNormal;
                Vloadx = Paras.RQ;
            }
            else if (workConditionCode == "U1")
            {
                rg = Paras.RGBad;
                zg = z1;
                zg1 = z2;
                fhn = Paras.VcFUnevenIce;
                Vloadx = Paras.RQ;
            }
            else if (workConditionCode == "U2")
            {
                rg = Paras.RGGood;
                zg = z3;
                zg1 = z4;
                fhn = Paras.VcFUnevenIce;
                Vloadx = Paras.RQ;
            }
            else if (workConditionCode == "B1")
            {
                rg = Paras.RGBad;
                zg = z1;
                zg1 = z2;
                fhn = Paras.VcFBroken;
                Vloadx = Paras.RQ;
            }
            else if (workConditionCode == "B2")
            {
                rg = Paras.RGGood;
                zg = z3;
                zg1 = z4;
                fhn = Paras.VcFBroken;
                Vloadx = Paras.RQ;
            }
            else if (workConditionCode == "Y1")
            {
                rg = Paras.RGCheck01;
                zg = z1;
                zg1 = z2;
                fhn = Paras.VcFCheck;
                Vloadx = Paras.RA;
            }
            else
            {
                rg = Paras.RGCheck02;
                zg = z3;
                zg1 = z4;
                fhn = Paras.VcFCheck;
                Vloadx = Paras.RA;
            }
        
            XX[i, j - 1] = ((FormulaLineTower)formula).ZXIYX(angle, x1, fhn, Vloadx,out string strX);
            YY[i, j - 1] = ((FormulaLineTower)formula).ZXIYY(angle, x1, y1, y2, fhn, Vloadx, out string strY);
            ZZ[i, j - 1] = ((FormulaLineTower)formula).ZXIYZ(zg1, zg, rg, fhn, Vloadx, out string strZ);

            GenerateLineLoadString(j, strX, strY, strZ);
        }

        /// <summary>
        /// 针对脱冰跳跃工况
        /// 工况代码"T"
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        protected void DistributeInJump(int i, int j)
        {
            float x1, y1, y2, z1, z2;
            int fuhao;

            WorkConditionCombo wd = Template.WorkConditionCombos[i];

            int zhs = wd.WireIndexCodes[j-1], zhsAM;
            int angle = wd.WindDirectionCode;
            string workConditionCode = wd.WorkConditionCode;

            if(Math.Abs(zhs) <= workConditionNum && Math.Abs(zhs) > 0)
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
                XX[i,j-1] = ((FormulaLineTower)formula).ZXTX(angle, x1, out string strX);
                YY[i,j-1] = ((FormulaLineTower)formula).ZXTY(angle, x1, y1, y2, fuhao, out string strY);
                ZZ[i,j-1] = ((FormulaLineTower)formula).ZXTZ2(z2, z1, out string strZ);

                GenerateLineLoadString(j, strX, strY, strZ);
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

                if (zhsAM > workConditionNum || fuhao > 1)
                {
                    throw new Exception("第　" + i + "　工况，第 " + j + " 线条组合参数错误，只能为-" + -workConditionNum + "～" + workConditionNum + "之间 " + "0 + 16 " + "错误：1-209");
                }

                x1 = Wind[j,zhsAM];
                z1 = GMin[j,zhsAM];
                z2 = GMin[j,1];

                //j从1开始计数，但是XX YY ZZ 从0开始
                XX[i,j-1] = ((FormulaLineTower)formula).ZXTX(angle, x1, out string strX);
                YY[i,j-1] = ((FormulaLineTower)formula).ZXTY(angle, x1, y1, y2, fuhao, out string strY);
                ZZ[i,j-1] = ((FormulaLineTower)formula).ZXTZ1(z2, z1, out string strZ);

                GenerateLineLoadString(j, strX, strY, strZ);
            }
            else if(zhs == 0)
            {
                //j从1开始计数，但是XX YY ZZ 从0开始
                XX[i,j-1] = 0;
                YY[i,j-1] = 0;
                ZZ[i,j-1] = 0;

                GenerateLineLoadString(j, "0.00", "0.00", "0.00");
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

            int zhs = wd.WireIndexCodes[j-1], zhsAM;
            int angle = wd.WindDirectionCode;
            string workConditionCode = wd.WorkConditionCode;
            float fh = LineParas.WireExtraLoad / Paras.LoadRatio;

            if (Math.Abs(zhs) <= workConditionNum && Math.Abs(zhs) > 0)
            {
                //吊装工况统一按最大垂荷考虑，不计算最小垂荷
                if (zhs <= workConditionNum && zhs > 0)
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
                XX[i,j-1] = ((FormulaLineTower)formula).ZXLX(angle, x1, out string strX);
                YY[i,j-1] = ((FormulaLineTower)formula).ZXLY(angle, x1, y1, y2, out string strY);
                ZZ[i,j-1] = ((FormulaLineTower)formula).ZXLZ1(z1, out string strZ);

                GenerateLineLoadString(j, strX, strY, strZ);
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

                if (zhsAM > workConditionNum)
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
                XX[i,j-1] = ((FormulaLineTower)formula).ZXLX(angle, x1, out string strX);
                YY[i,j-1] = ((FormulaLineTower)formula).ZXLY(angle, x1, y1, y2, out string strY);
                ZZ[i,j-1] = ((FormulaLineTower)formula).ZXLZ2(z1, zg, fh, fhn, out string strZ);

                GenerateLineLoadString(j, strX, strY, strZ);
            }
            else if (zhs == 0)
            {
                //j从1开始计数，但是XX YY ZZ 从0开始
                XX[i,j-1] = 0;
                YY[i,j-1] = 0;
                ZZ[i,j-1] = 0;

                GenerateLineLoadString(j, "0.00", "0.00", "0.00");
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

            int zhs = wd.WireIndexCodes[j-1], zhsAM;
            int angle = wd.WindDirectionCode;
            string workConditionCode = wd.WorkConditionCode;
            float fh = LineParas.WireExtraLoad / Paras.LoadRatio;

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

                if(zhsAM > workConditionNum)
                {
                    throw new Exception("第　" + i + "　工况，第 " + j + " 线条组合参数错误" + "0 + 16 " + "错误：1-211");
                }

                y1 = TensionMax[j,zhsAM];
                y2 = TensionMin[j,zhsAM];

                if (j <= earthWireNum && y1 >= LineParas.AnchorTension)
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
                XX[i,j-1] = ((FormulaLineTower)formula).ZXMX2(angle, x1, out string strX);
                YY[i,j-1] = ((FormulaLineTower)formula).ZXMY2(angle, x1, deta1 * fuhao, deta2, out string strY);
                ZZ[i,j-1] = ((FormulaLineTower)formula).ZXMZ2(z1, fh, fhn, deta1, deta3, out string strZ);

                GenerateLineLoadString(j, strX, strY, strZ);
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

                if (zhsAM > workConditionNum )
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

                    if (j <= earthWireNum && y1 >= LineParas.AnchorTension)
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

                    if (j <= earthWireNum && Math.Abs(y1) >= Math.Abs(LineParas.AnchorTension))
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
                XX[i,j-1] = ((FormulaLineTower)formula).ZXMX1(angle, x1, out string strX);
                YY[i,j-1] = ((FormulaLineTower)formula).ZXMY1(angle, x1, deta1, deta2, out string strY);
                ZZ[i,j-1] = ((FormulaLineTower)formula).ZXMZ1(z1, fh , fhn, deta1, deta3, out string strZ);

                GenerateLineLoadString(j, strX, strY, strZ);
            }
            else if (zhs == 0)
            {
                //j从1开始计数，但是XX YY ZZ 从0开始
                XX[i,j-1] = 0;
                YY[i,j-1] = 0;
                ZZ[i,j-1] = 0;

                GenerateLineLoadString(j, "0.00", "0.00", "0.00");
            }
        }

        /// <summary>
        /// 针对OPGW锚线工况
        /// 工况代码"MO", "MOa", "MOb", "MOc", "MOd", "MOe", "MOf", "MOg", "MOh"
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        protected void DistributeInOPGWAnchor(int i, int j)
        {
            float x1, y1, y2, z1, z2;
            float fhn;
            float deta1;

            WorkConditionCombo wd = Template.WorkConditionCombos[i];

            int zhs = wd.WireIndexCodes[j-1], zhsAM;
            int angle = wd.WindDirectionCode;
            string workConditionCode = wd.WorkConditionCode;
            float fh = LineParas.WireExtraLoad / Paras.LoadRatio;

            //OPGW锚线特殊工况，其表达式与常规锚线有所不同，需区分
            if (zhs < 1000 && zhs > 0)
            {
                x1 = Wind[j,zhs];
                y1 = TensionMax[j,zhs];
                y2 = TensionMin[j,zhs];
                z1 = GMax[j,zhs];
                z2 = GMin[j,zhs];

                //j从1开始计数，但是XX YY ZZ 从0开始
                //刚老师意见，无论开断与否，OPGW已架相均应该有荷载，与常规不符
                XX[i,j-1] = ((FormulaLineTower)formula).MO2X(angle, x1, out string strX);
                YY[i,j-1] = ((FormulaLineTower)formula).MO2Y(angle, x1, y1, y2, out string strY);
                ZZ[i,j-1] = ((FormulaLineTower)formula).MO2Z(z1, out string strZ);

                GenerateLineLoadString(j, strX, strY, strZ);
            }
            else if ( zhs > 1000)
            {
                fhn = zhs / 1000;
                zhsAM = zhs % 1000;

                x1 = Wind[j,zhsAM];
                y1 = TensionMax[j,zhsAM];
                y2 = TensionMin[j,zhsAM];
                z1 = GMax[j,zhsAM];
                z2 = GMin[j,zhsAM];
                                    
                //左地线
                if( y1 > 0)
                {
                    //j从1开始计数，但是XX YY ZZ 从0开始
                    deta1 = LineParas.DrawingCoef;
                    XX[i,j-1] = ((FormulaLineTower)formula).MO1X(angle, x1, out string strX);
                    XX[i,j-1] = ((FormulaLineTower)formula).MO1Y(angle, x1, y1, deta1, y2, out string strY);
                    XX[i,j-1] = ((FormulaLineTower)formula).MO1Z(z1, y1, deta1, fh , fhn, out string strZ);

                    GenerateLineLoadString(j, strX, strY, strZ);
                }
                else
                {
                    deta1 = 1;

                    //j从1开始计数，但是XX YY ZZ 从0开始
                    XX[i,j-1] = 0;
                    YY[i,j-1] = 0;
                    ZZ[i,j-1] = 0;

                    GenerateLineLoadString(j, "0.00", "0.00", "0.00");
                }
            }
            else if (zhs == 0)
            {
                //j从1开始计数，但是XX YY ZZ 从0开始
                XX[i,j-1] = 0;
                YY[i,j-1] = 0;
                ZZ[i,j-1] = 0;

                GenerateLineLoadString(j, "0.00", "0.00", "0.00");
            }
        }

        /// <summary>
        /// 针对过滑车工况,只考虑最大垂荷
        /// 工况代码"G", "Ga", "Gb", "Gc", "Gd", "Ge", "Gf", "Gg", "Gh", "GLa", "GLb", "GLc", "GLd", "GLe", "GLf", "GLg", "GLh"
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        protected void DistributeInHuache(int i, int j)
        {
            float x1, y1, y2, z1, z2;
            int fuhao;
            float fhn;
            float deta1, deta2, deta3;

            WorkConditionCombo wd = Template.WorkConditionCombos[i];

            int zhs = wd.WireIndexCodes[j-1], zhsAM;
            int angle = wd.WindDirectionCode;
            string workConditionCode = wd.WorkConditionCode;
            float fh = LineParas.WireExtraLoad / Paras.LoadRatio;

            if (Math.Abs(zhs) < workConditionNum && Math.Abs(zhs) > 0)
            {
                //已安装
                if (zhs <= workConditionNum && zhs > 0)
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
                XX[i,j-1] = ((FormulaLineTower)formula).ZXLX(angle, x1, out string strX);
                YY[i,j-1] = ((FormulaLineTower)formula).ZXLY(angle, x1, y1, y2, out string strY);
                ZZ[i,j-1] = ((FormulaLineTower)formula).ZXLZ1(z1, out string strZ);

                GenerateLineLoadString(j, strX, strY, strZ);
            }
            else if ( Math.Abs(zhs) > 100 && Math.Abs(zhs) < 1000)
            {
                //已锚相
                if( zhs > 0)
                {
                    fuhao = 1;
                }
                else
                {
                    fuhao = -1;
                }

                fhn = Math.Abs(zhs) / 100;
                zhsAM = Math.Abs(zhs) % 100;

                if(zhsAM > workConditionNum)
                {
                    throw new Exception("第　" + i + "　工况，第 " + j + " 线条组合参数错误" + "0 + 16 " + "错误：1-213");
                }

                y1 = TensionMax[j,zhsAM];
                y2 = TensionMin[j,zhsAM];

                if (j <= earthWireNum && y1 >= LineParas.AnchorTension)
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
                XX[i,j-1] = ((FormulaLineTower)formula).ZXMX2(angle, x1, out string strX);
                YY[i,j-1] = ((FormulaLineTower)formula).ZXMY2(angle, x1, deta1 * fuhao, deta2, out string strY);
                ZZ[i,j-1] = ((FormulaLineTower)formula).ZXMZ2(z1, fh , fhn, deta1, deta3, out string strZ);

                GenerateLineLoadString(j, strX, strY, strZ);
            }
            else if ( Math.Abs(zhs) > 1000)
            {
                //过滑车 >10000 表示转向过滑车
                if (zhs > 0)
                {
                    y1 = LineParas.PulleyTensionDif;
                }
                else
                {
                    y1 = -LineParas.PulleyTensionDif;
                }

                if(Math.Abs(zhs) < 10000)
                {
                    fhn = Math.Abs(zhs) / 1000;
                }
                else
                {
                    fhn = (Math.Abs(zhs) - 10000) / 1000;
                }

                zhsAM = Math.Abs(zhs) % 1000;

                if(zhsAM > workConditionNum)
                {
                    throw new Exception("第　" + i + "　工况，第 " + j + " 线条组合参数错误" + "0 + 16 " + "错误：1-214");
                }

                x1 = Wind[j,zhsAM];
                z1 = GMax[j,1];
                z2 = GMax[j,zhsAM];

                //j从1开始计数，但是XX YY ZZ 从0开始
                XX[i,j-1] = ((FormulaLineTower)formula).GLX(angle, x1, out string strX);
                YY[i,j-1] = ((FormulaLineTower)formula).GLY(angle, x1, y1, out string strY);
                ZZ[i,j-1] = ((FormulaLineTower)formula).GLZ(z1, z2, fh, fhn, out string strZ);

                GenerateLineLoadString(j, strX, strY, strZ);

                //工况代号大于10000，并且选择了转向挂点
                if (Math.Abs(zhs) > 10000 && HPSettingParas.IsTuringPointSeleced == true)
                {
                    CalsTurningPointsLoad(i, j);
                }
                
            }
            else if (zhs == 0)
            {
                //j从1开始计数，但是XX YY ZZ 从0开始
                XX[i,j-1] = 0;
                YY[i,j-1] = 0;
                ZZ[i,j-1] = 0;

                GenerateLineLoadString(j, "0.00", "0.00", "0.00");
            }
        }

        /// <summary>
        /// 针对施工提线工况，只考虑最大垂荷，多用于直流输电塔
        /// 工况代码"C", "Ca", "Cb", "Cc", "Cd", "Ce", "Cf", "Cg", "Ch", "CLa", "CLb", "CLc", "CLd", "CLe", "CLf", "CLg", "CLh"
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        protected void DistributeInTixian(int i, int j)
        {
            float x1, y1, y2, z1, z2;
            int fuhao;
            float fhn;
            float deta1, deta2, deta3;

            WorkConditionCombo wd = Template.WorkConditionCombos[i];

            int zhs = wd.WireIndexCodes[j-1], zhsAM;
            int angle = wd.WindDirectionCode;
            string workConditionCode = wd.WorkConditionCode;
            float fh = LineParas.WireExtraLoad / Paras.LoadRatio;

            if (Math.Abs(zhs) < workConditionNum && Math.Abs(zhs) > 0)
            {
                //已安装
                if (zhs <= workConditionNum && zhs > 0)
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
                XX[i,j-1] = ((FormulaLineTower)formula).ZXLX(angle, x1, out string strX);
                YY[i,j-1] = ((FormulaLineTower)formula).ZXLY(angle, x1, y1, y2, out string strY);
                ZZ[i,j-1] = ((FormulaLineTower)formula).ZXLZ1(z1, out string strZ);

                GenerateLineLoadString(j, strX, strY, strZ);
            }
            else if (Math.Abs(zhs) > 100 && Math.Abs(zhs) < 1000)
            {
                //已锚相
                if (zhs > 0)
                {
                    fuhao = 1;
                }
                else
                {
                    fuhao = -1;
                }

                fhn = Math.Abs(zhs) / 100;
                zhsAM = Math.Abs(zhs) % 100;

                if (zhsAM > workConditionNum)
                {
                    throw new Exception("第　" + i + "　工况，第 " + j + " 线条组合参数错误" + "0 + 16 " + "错误：1-215");
                }

                y1 = TensionMax[j,zhsAM];
                y2 = TensionMin[j,zhsAM];

                if (j <= earthWireNum && y1 >= LineParas.AnchorTension)
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
                XX[i,j-1] = ((FormulaLineTower)formula).ZXMX2(angle, x1, out string strX);
                YY[i,j-1] = ((FormulaLineTower)formula).ZXMY2(angle, x1, deta1 * fuhao, deta2, out string strY);
                ZZ[i,j-1] = ((FormulaLineTower)formula).ZXMZ2(z1, fh , fhn, deta1, deta3, out string strZ);

                GenerateLineLoadString(j, strX, strY, strZ);
            }
            else if (Math.Abs(zhs) > 1000)
            {
                //施工提线相
                if (Math.Abs(zhs) < 10000)
                {
                    fhn = Math.Abs(zhs) / 1000;
                }
                else
                {
                    fhn = (Math.Abs(zhs) - 10000) / 1000;
                }

                zhsAM = Math.Abs(zhs) % 1000;

                if (zhsAM > workConditionNum)
                {
                    throw new Exception("第　" + i + "　工况，第 " + j + " 线条组合参数错误" + "0 + 16 " + "错误：1-216");
                }

                x1 = Wind[j,zhsAM];
                z1 = GMax[j,zhsAM];
                z2 = GMin[j,zhsAM];

                //j从1开始计数，但是XX YY ZZ 从0开始
                XX[i,j-1] = ((FormulaLineTower)formula).ZXCX(angle, x1, out string strX);
                YY[i,j-1] = ((FormulaLineTower)formula).ZXCY(angle, x1, LineParas.HoistingCoef, z1, out string strY);
                ZZ[i,j-1] = ((FormulaLineTower)formula).ZXCZ(fh * fhn, LineParas.HoistingCoef, z1, out string strZ);

                GenerateLineLoadString(j, strX, strY, strZ);
                
                //工况代号大于10000，并且选择了转向挂点
                if (Math.Abs(zhs) > 10000 && HPSettingParas.IsTuringPointSeleced == true)
                {
                    CalsTurningPointsLoad(i, j);
                }

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
