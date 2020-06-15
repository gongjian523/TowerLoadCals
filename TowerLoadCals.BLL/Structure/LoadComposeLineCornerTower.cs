using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using TowerLoadCals.Mode;

namespace TowerLoadCals.BLL
{

    /// <summary>
    /// 直线塔的荷载分配
    /// </summary>
    public class LoadComposeLineCornerTower:LoadComposeBase
    {
        protected float[,] Wind { get; set; }
        protected float[,] Windx { get; set; }

        protected float[,] GMax { get; set; }
        protected float[,] GMin { get; set; }

        protected float[,] TensionMax { get; set; }
        protected float[,] TensionMin { get; set; }

        protected FormulaLineCornerTower formula;

        public LoadComposeLineCornerTower(FormulaParas para, StruLineParas[] lineParas, StruRatioParas ratioParas, TowerTemplate template, string tablePath) 
            : base(para, lineParas, ratioParas, template, tablePath)
        {
            formula = new FormulaLineCornerTower(para);
        }

        protected override void GetTable(DataSet ds)
        {
            //后续计算的序号从1开始
            Wind = new float[Template.Wires.Count + 1, Template.WorkConditongs.Count + 1];
            Windx = new float[Template.Wires.Count + 1, Template.WorkConditongs.Count + 1];
            GMax = new float[Template.Wires.Count + 1, Template.WorkConditongs.Count + 1];
            GMin = new float[Template.Wires.Count + 1, Template.WorkConditongs.Count + 1];
            TensionMax = new float[Template.Wires.Count + 1, Template.WorkConditongs.Count + 1];
            TensionMin = new float[Template.Wires.Count + 1, Template.WorkConditongs.Count + 1];

            for (int j = 1; j <= Template.WorkConditongs.Count; j++)
            {
                for (int i = 1; i <= Template.Wires.Count; i++)
                {
                    object obj = ds.Tables[0].Rows[4 + 7 * (i - 1)][j];
                    float.TryParse(obj.ToString(), out Wind[i, j]);
                    object obj2 = ds.Tables[0].Rows[5 + 7 * (i - 1)][j];
                    float.TryParse(obj2.ToString(), out Windx[i, j]);
                    object obj3 = ds.Tables[0].Rows[6 + 7 * (i - 1)][j];
                    float.TryParse(obj3.ToString(), out GMax[i, j]);
                    object obj4 = ds.Tables[0].Rows[7 + 7 * (i - 1)][j];
                    float.TryParse(obj4.ToString(), out GMin[i, j]);
                    object obj5 = ds.Tables[0].Rows[8 + 7 * (i - 1)][j];
                    float.TryParse(obj5.ToString(), out TensionMax[i, j]);
                    object obj6 = ds.Tables[0].Rows[9 + 7 * (i - 1)][j];
                    float.TryParse(obj6.ToString(), out TensionMin[i, j]);
                }
            }
        }

        public override void CalculateLoadDistribute(out float[,] xx, out float[,] yy, out float[,] zz, string path)
        {
            int calNums = Template.WorkConditionCombos.Count;

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
                        case "G":
                        case "Ga":
                        case "Gb":
                        case "Gc":
                        case "Gd":
                        case "Ge":
                        case "Gf":
                        case "Gg":
                        case "Gh":
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
                            throw new Exception("直线塔工况代号超出范围" + "0 + 16" + "错误：1-230");
                            //break;
                    }
                    j++;
                }
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
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        protected void DistributeInWindAndLowTemperature(int i, int j)
        {
            float x1, y1, y2, z1, z2;
            float rg, zg, Vcb;
            float zjiao; 

            WorkConditionCombo wd = Template.WorkConditionCombos[i];

            int zhs = wd.WireIndexCodes[j-1];
            int angle = wd.WindDirectionCode;
            string workConditionCode = wd.WorkConditionCode;

            if (zhs <= workConditionNum && zhs > 0)
            {
                if( wd.TensionAngleCode == "D" )
                {
                    x1 = Wind[j, zhs];
                    zjiao = LineParas.AngleMax;
                } 
                //else if( wd.TensionAngleCode == "X" )
                else
                {
                    x1 = Windx[j, zhs];
                    zjiao = LineParas.AngleMin;
                }

                y1 = TensionMax[j, zhs];
                y2 = TensionMin[j, zhs];
                z1 = GMax[j, zhs];
                z2 = GMin[j, zhs];
            }
            else if (zhs < 0 && zhs >= -workConditionNum)
            {
                if( wd.TensionAngleCode == "D" )
                {
                    x1 = Wind[j, zhs];
                    zjiao = LineParas.AngleMax;
                }
                //else if( wd.TensionAngleCode == "X" )
                else
                {
                    x1 = Windx[j, zhs];
                    zjiao = LineParas.AngleMin;
                }

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
                zjiao = 0;
            }
            else
            {
                x1 = 9999;
                y1 = 9999;
                y2 = 9999;
                z1 = 9999;
                z2 = 9999;
                throw new Exception("第　" + i + "　工况，第 " + j + " 线条组合参数错误，只能为-" + -workConditionNum + "～" + workConditionNum + "之间 " + "0 + 16 " + "错误：1-218");
            }

            if (workConditionCode == "N1" || workConditionCode == "D1")
            {
                rg = Paras.RGBad;
                zg = z1;
            }
            //else if (workConditionCode == "N2" || workConditionCode == "D2")
            else
            {
                rg = Paras.RGGood;
                zg = z2;
            }

            if (workConditionCode == "N1" || workConditionCode == "N1")
            {
                Vcb = Paras.VcFNormal;
            }
            //else if (workConditionCode == "D2" || workConditionCode == "D2")
            else
            {
                Vcb = Paras.VcFCold;
            }

            //与直线塔有所差别，需要考虑角度力的影响，张力按角度在X向有所分离，在Y向存在荷载，下同
            XX[i, j - 1] = formula.ZZNX(angle, x1, y1, y2, zjiao, Vcb, out string strX);
            YY[i, j - 1] = formula.ZZNY(angle, x1, y1, y2, zjiao, Vcb, out string strY);
            ZZ[i, j - 1] = formula.ZZNZ(zg, rg, Vcb, out string strZ);

            ProcessString.Add(Template.Wires[j-1] + " Fx= " + strX);
            ProcessString.Add(Template.Wires[j-1] + " Fy= " + strY);
            ProcessString.Add(Template.Wires[j-1] + " Fz= " + strZ);
        }

        /// <summary>
        /// 针对覆冰-最低温、不均匀冰、断线、验算冰工况
        /// 工况代码"I1", "I2", "U1", "U2", "B1", "B2", "Y1", "Y2"
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        protected void DistributeInIce(int i, int j)
        {
            float x1, y1, y2, z1, z2, z3, z4;
            float rg, zg, zg1, fhn, Vloadx;
            float zjiao;

            WorkConditionCombo wd = Template.WorkConditionCombos[i];

            int zhs = wd.WireIndexCodes[j-1];
            int angle = wd.WindDirectionCode;
            string workConditionCode = wd.WorkConditionCode;

            int zhsM = (Math.Abs(zhs)) % 1000;

            if (zhs > 0)
            {
                if( wd.TensionAngleCode == "D" )
                {
                    x1 = Wind[j, zhsM];
                    zjiao = LineParas.AngleMax;
                } 
                //else if( wd.TensionAngleCode == "X" )
                else
                {
                    x1 = Windx[j, zhsM];
                    zjiao = LineParas.AngleMin;
                }

                y1 = TensionMax[j, zhsM];
                y2 = TensionMin[j, zhsM];
                z1 = GMax[j, zhsM];
                z2 = GMax[j, 1];
                z3 = GMin[j, zhsM];
                z4 = GMin[j, 1];
            }
            else if(zhs < 0)
            {
                if( wd.TensionAngleCode == "D" )
                {
                    x1 = Wind[j, zhsM];
                    zjiao = LineParas.AngleMax;
                }
                //else if( wd.TensionAngleCode == "X" )
                else
                {
                    x1 = Windx[j, zhsM];
                    zjiao = LineParas.AngleMin;
                }
                y1 = TensionMin[j, zhsM];
                y2 = TensionMax[j, zhsM];
                z1 = GMax[j, zhsM];
                z2 = GMax[j, 1];
                z3 = GMin[j, zhsM];
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
                zjiao = 0;
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
                throw new Exception("第　" + i + "　工况，第 " + j + " 线条组合参数错误，只能为-" + -workConditionNum + "～" + workConditionNum + "之间 " + "0 + 16 " + "错误：1-219");
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

            XX[i, j - 1] = formula.ZZIYX(angle, x1, y1, y2, zjiao, fhn, Vloadx, out string strX);
            YY[i, j - 1] = formula.ZZIYY(angle, x1, y1, y2, zjiao, fhn, Vloadx, out string strY);
            ZZ[i, j - 1] = formula.ZZIYZ(zg1, zg, rg, fhn, Vloadx, out string strZ);

            ProcessString.Add(Template.Wires[j-1] + " Fx= " + strX);
            ProcessString.Add(Template.Wires[j-1] + " Fy= " + strY);
            ProcessString.Add(Template.Wires[j-1] + " Fz= " + strZ);
        }

        /// <summary>
        /// 针对脱冰跳跃工况
        /// 工况代码"T"
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        protected void DistributeInJump(int i, int j)
        {
            float x1, y1, y2, z1, z2, z3, z4;
            float zjiao;

            WorkConditionCombo wd = Template.WorkConditionCombos[i];

            int zhs = wd.WireIndexCodes[j-1], zhsAM;
            int angle = wd.WindDirectionCode;
            string workConditionCode = wd.WorkConditionCode;

            if(Math.Abs(zhs) <= workConditionNum && Math.Abs(zhs) > 0)
            {
                //正常覆冰相
                if (zhs > 0)
                {
                    if( wd.TensionAngleCode == "D" )
                    {
                        x1 = Wind[j, zhs];
                        zjiao = LineParas.AngleMax;
                    } 
                    //else if( wd.TensionAngleCode == "X" )
                    else
                    {
                        x1 = Windx[j, zhs];
                        zjiao = LineParas.AngleMin;
                    }
                    y1 = TensionMax[j, zhs];
                    y2 = TensionMin[j, zhs];
                    z1 = GMax[j, zhs];
                    z2 = GMax[j, 1];
                    z3 = GMin[j, zhs];
                    z4 = GMin[j, 1];
                }
                else
                {
                    if( wd.TensionAngleCode == "D" )
                    {
                        x1 = Wind[j, Math.Abs(zhs)];
                        zjiao = LineParas.AngleMax;
                    }
                    //else if( wd.TensionAngleCode == "X" )
                    else
                    {
                        x1 = Windx[j, Math.Abs(zhs)];
                        zjiao = LineParas.AngleMin;
                    }
                    y1 = TensionMin[j, Math.Abs(zhs)];
                    y2 = TensionMax[j, Math.Abs(zhs)];
                    z1 = GMax[j, Math.Abs(zhs)];
                    z2 = GMax[j, 1];
                    z3 = GMin[j, Math.Abs(zhs)];
                    z4 = GMin[j, 1];
                }


                //j从1开始计数，但是XX YY ZZ 从0开始
                XX[i, j - 1] = formula.ZZTX(angle, x1, y1, y2, zjiao, out string strX);
                YY[i, j - 1] = formula.ZZTY(angle, x1, y1, y2, zjiao, out string strY);
                ZZ[i, j - 1] = formula.ZZTZ2(z2, z1, out string strZ);

                ProcessString.Add(Template.Wires[j-1] + " Fx= " + strX);
                ProcessString.Add(Template.Wires[j-1] + " Fy= " + strY);
                ProcessString.Add(Template.Wires[j-1] + " Fz= " + strZ);

            }
            else if(Math.Abs(zhs) > 1000)
            {
                zhsAM = Math.Abs(zhs) % 1000;

                //脱冰跳跃相
                
                if (zhs > 1000)
                {

                    if (zhsAM > workConditionNum)
                    {
                        throw new Exception("第　" + i + "　工况，第 " + j + " 线条组合参数错误" + "0 + 16 " + "错误：1-220");
                    }

                    if( wd.TensionAngleCode == "D" )
                    {
                        x1 = Wind[j, zhsAM];
                        zjiao = LineParas.AngleMax;
                    } 
                    //else if( wd.TensionAngleCode == "X" )
                    else
                    {
                        x1 = Windx[j, zhsAM];
                        zjiao = LineParas.AngleMin;
                    }
                    y1 = TensionMax[j, zhsAM];
                    y2 = TensionMin[j, zhsAM];
                    z1 = GMax[j, zhsAM];
                    z2 = GMax[j, 1];
                    z3 = GMin[j, zhsAM];
                    z4 = GMin[j, 1];
                }
                else
                {
                    if (zhsAM > workConditionNum)
                    {
                        throw new Exception("第　" + i + "　工况，第 " + j + " 线条组合参数错误" + "0 + 16 " + "错误：1-221");
                    }

                    if( wd.TensionAngleCode == "D" )
                    {
                        x1 = Wind[j, zhsAM];
                        zjiao = LineParas.AngleMax;
                    } 
                    //else if( wd.TensionAngleCode == "X" )
                    else
                    {
                        x1 = Windx[j, zhsAM];
                        zjiao = LineParas.AngleMin;
                    }

                    y1 = TensionMin[j, zhsAM];
                    y2 = TensionMax[j, zhsAM];
                    z1 = GMax[j, zhsAM];
                    z2 = GMax[j, 1];
                    z3 = GMin[j, zhsAM];
                    z4 = GMin[j, 1];
                }

                XX[i, j - 1] = formula.ZZTX(angle, x1, y1, y2, zjiao, out string strX);
                YY[i, j - 1] = formula.ZZTY(angle, x1, y1, y2, zjiao, out string strY);
                ZZ[i, j - 1] = formula.ZZTZ1(z4, z3, out string strZ);

                ProcessString.Add(Template.Wires[j-1] + " Fx= " + strX);
                ProcessString.Add(Template.Wires[j-1] + " Fy= " + strY);
                ProcessString.Add(Template.Wires[j-1] + " Fz= " + strZ);
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
            float x1, y1, y2, z1, z2, z3, z4;
            float zg, fhn;
            float zjiao;

            WorkConditionCombo wd = Template.WorkConditionCombos[i];

            int zhs = wd.WireIndexCodes[j-1], zhsAM;
            int angle = wd.WindDirectionCode;
            string workConditionCode = wd.WorkConditionCode;
            float fh = LineParas.WireExtraLoad / Paras.LoadRatio;

            if (Math.Abs(zhs) <= workConditionNum && Math.Abs(zhs) > 0)
            {
                //已安装，无附加荷载
                if (zhs <= workConditionNum && zhs > 0)
                {
                    if( wd.TensionAngleCode == "D" )
                    {
                        x1 = Wind[j, zhs];
                        zjiao = LineParas.AngleMax;
                    }
                    //else if( wd.TensionAngleCode == "X" )
                    else
                    {
                        x1 = Windx[j, zhs];
                        zjiao = LineParas.AngleMin;
                    }
                    y1 = TensionMax[j, zhs];
                    y2 = TensionMin[j, zhs];
                    z1 = GMax[j, zhs];
                    z2 = GMax[j, 1];
                }
                else
                {
                    if( wd.TensionAngleCode == "D" )
                    {
                        x1 = Wind[j, Math.Abs(zhs)];
                        zjiao = LineParas.AngleMax;
                    } 
                    //else if( wd.TensionAngleCode == "X" )
                    else
                    {
                        x1 = Windx[j, Math.Abs(zhs)];
                        zjiao = LineParas.AngleMin;
                    }
                    y1 = TensionMin[j, Math.Abs(zhs)];
                    y2 = TensionMax[j, Math.Abs(zhs)];
                    z1 = GMax[j, Math.Abs(zhs)];
                    z2 = GMax[j, 1];
                }

                //j从1开始计数，但是XX YY ZZ 从0开始
                XX[i, j - 1] = formula.ZZLX(angle, x1, y1, y2, zjiao, out string strX);
                YY[i, j - 1] = formula.ZZLY(angle, x1, y1, y2, zjiao, out string strY);
                ZZ[i, j - 1] = formula.ZZLZ1(z1, out string strZ);

                ProcessString.Add(Template.Wires[j-1] + " Fx= " + strX);
                ProcessString.Add(Template.Wires[j-1] + " Fy= " + strY);
                ProcessString.Add(Template.Wires[j-1] + " Fz= " + strZ);

            }
            else if (Math.Abs(zhs) > 100)
            {
                // 包含检修提线和吊装
                if (zhs > 1000)
                {
                    fhn = Math.Abs(zhs) / 1000;
                    zhsAM = Math.Abs(zhs) % 1000;

                    if (zhsAM > workConditionNum)
                    {
                        throw new Exception("第　" + i + "　工况，第 " + j + " 线条组合参数错误" + "0 + 16 " + "错误：1-222");
                    }

                    if( wd.TensionAngleCode == "D" )
                    {
                        x1 = Wind[j, zhsAM];
                        zjiao = LineParas.AngleMax;
                    } 
                    //else if( wd.TensionAngleCode == "X" )
                    else
                    {
                        x1 = Windx[j, zhsAM];
                        zjiao = LineParas.AngleMin;
                    }
                    y1 = TensionMax[j, zhsAM];
                    y2 = TensionMin[j, zhsAM];
                    z1 = GMax[j, zhsAM];
                    z2 = GMax[j, 1];
                    z3 = GMin[j, zhsAM];
                    z4 = GMin[j, 1];
                    zg = LineParas.HoistingCoef;
                }
                else if(zhs < -1000)
                {

                    fhn = Math.Abs(zhs) / 1000;
                    zhsAM = Math.Abs(zhs) % 1000;

                    if (zhsAM > workConditionNum)
                    {
                        throw new Exception("第　" + i + "　工况，第 " + j + " 线条组合参数错误" + "0 + 16 " + "错误：1-223");
                    }

                    if( wd.TensionAngleCode == "D" )
                    {
                        x1 = Wind[j, zhsAM];
                        zjiao = LineParas.AngleMax;
                    } 
                    //else if( wd.TensionAngleCode == "X" )
                    else
                    {
                        x1 = Windx[j, zhsAM];
                        zjiao = LineParas.AngleMin;
                    }

                    y1 = TensionMin[j, zhsAM];
                    y2 = TensionMax[j, zhsAM];
                    z1 = GMax[j, zhsAM];
                    z2 = GMax[j, 1];
                    z3 = GMin[j, zhsAM];
                    z4 = GMin[j, 1];
                    zg = LineParas.HoistingCoef;
                }
                else if(zhs > 100 && zhs < 1000)
                {
                    fhn = Math.Abs(zhs) / 100;
                    zhsAM = Math.Abs(zhs) % 100;

                    if (zhsAM > workConditionNum)
                    {
                        throw new Exception("第　" + i + "　工况，第 " + j + " 线条组合参数错误" + "0 + 16 " + "错误：1-224");
                    }

                    if( wd.TensionAngleCode == "D" )
                    {
                        x1 = Wind[j, zhsAM];
                        zjiao = LineParas.AngleMax;
                    } 
                    //else if( wd.TensionAngleCode == "X" )
                    else
                    {
                        x1 = Windx[j, zhsAM];
                        zjiao = LineParas.AngleMin;
                    }
                    y1 = TensionMax[j, zhsAM];
                    y2 = TensionMin[j, zhsAM];
                    z1 = GMax[j, zhsAM];
                    z2 = GMax[j, 1];
                    z3 = GMin[j, zhsAM];
                    z4 = GMin[j, 1];
                    zg = 1.0f;
                }
                else
                {
                    fhn = Math.Abs(zhs) / 100;
                    zhsAM = Math.Abs(zhs) % 100;

                    if (zhsAM > workConditionNum)
                    {
                        throw new Exception("第　" + i + "　工况，第 " + j + " 线条组合参数错误" + "0 + 16 " + "错误：1-225");
                    }

                    if( wd.TensionAngleCode == "D" )
                    {
                        x1 = Wind[j, zhs];
                        zjiao = LineParas.AngleMax;
                    } 
                    //else if( wd.TensionAngleCode == "X" )
                    else
                    {
                        x1 = Windx[j, zhs];
                        zjiao = LineParas.AngleMin;
                    }
                    y1 = TensionMin[j, zhs];
                    y2 = TensionMax[j, zhs];
                    z1 = GMax[j, zhs];
                    z2 = GMax[j, 1];
                    z3 = GMin[j, zhs];
                    z4 = GMin[j, 1];
                    zg = 1.0f;
                }

                XX[i, j - 1] = formula.ZZLX(angle, x1, y1, y2, zjiao, out string strX);
                YY[i, j - 1] = formula.ZZLY(angle, x1, y1, y2, zjiao, out string strY);
                ZZ[i, j - 1] = formula.ZZLZ2(z1, zg, fh, fhn, out string strZ);

                ProcessString.Add(Template.Wires[j-1] + " Fx= " + strX);
                ProcessString.Add(Template.Wires[j-1] + " Fy= " + strY);
                ProcessString.Add(Template.Wires[j-1] + " Fz= " + strZ);
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
        /// 针对过滑车工况,只考虑最大垂荷
        /// 工况代码"G", "Ga", "Gb", "Gc", "Gd", "Ge", "Gf", "Gg", "Gh", "GLa", "GLb", "GLc", "GLd", "GLe", "GLf", "GLg", "GLh"
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        protected void DistributeInHuache(int i, int j)
        {
            float x1, y1, y2, z1, z2, z3, z4;
            int fuhao;
            float fhn = 0.0f,zjiao;

            WorkConditionCombo wd = Template.WorkConditionCombos[i];

            int zhs = wd.WireIndexCodes[j-1], zhsAM;
            int angle = wd.WindDirectionCode;
            string workConditionCode = wd.WorkConditionCode;
            float fh = LineParas.WireExtraLoad / Paras.LoadRatio;

            if (Math.Abs(zhs) < workConditionNum && Math.Abs(zhs) > 0)
            {
                //已安装，无附加荷载
                if (zhs <= workConditionNum && zhs > 0)
                {
                    if( wd.TensionAngleCode == "D" )
                    {
                        x1 = Wind[j, zhs];
                        zjiao = LineParas.AngleMax;
                    } 
                    //else if( wd.TensionAngleCode == "X" )
                    else
                    {
                        x1 = Windx[j, zhs];
                        zjiao = LineParas.AngleMin;
                    }
                    y1 = TensionMax[j, zhs];
                    y2 = TensionMin[j, zhs];
                    z1 = GMax[j, zhs];
                    z2 = GMax[j, 1];
                    fuhao = 1;
                }
                else
                {
                    if( wd.TensionAngleCode == "D" )
                    {
                        x1 = Wind[j, Math.Abs(zhs)];
                        zjiao = LineParas.AngleMax;
                    } 
                    //else if( wd.TensionAngleCode == "X" )
                    else
                    {
                        x1 = Windx[j, Math.Abs(zhs)];
                        zjiao = LineParas.AngleMin;
                    }
                    y1 = TensionMin[j, Math.Abs(zhs)];
                    y2 = TensionMax[j, Math.Abs(zhs)];
                    z1 = GMax[j, Math.Abs(zhs)];
                    z2 = GMax[j, 1];
                    fuhao = -1;
                }

                XX[i, j - 1] = formula.ZZLX(angle, x1, y1, y2, zjiao, out string strX);
                YY[i, j - 1] = formula.ZZLY(angle, x1, y1, y2, zjiao, out string strY);
                ZZ[i, j - 1] = formula.ZZLZ1(z1, out string strZ);

                ProcessString.Add(Template.Wires[j-1] + " Fx= " + strX);
                ProcessString.Add(Template.Wires[j-1] + " Fy= " + strY);
                ProcessString.Add(Template.Wires[j-1] + " Fz= " + strZ);

            }
            else if ( Math.Abs(zhs) > 1000)
            {
                if(zhs > 1000)
                {
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
                        throw new Exception("第　" + i + "　工况，第 " + j + " 线条组合参数错误" + "0 + 16 " + "错误：1-226");
                    }

                    if( wd.TensionAngleCode == "D" )
                    {
                        x1 = Wind[j, zhs];
                        zjiao = LineParas.AngleMax;
                    }
                    //else if( wd.TensionAngleCode == "X" )
                    else
                    {
                        x1 = Windx[j, zhs];
                        zjiao = LineParas.AngleMin;
                    }
                    y1 = TensionMax[j, zhs];
                    y2 = TensionMin[j, zhs];
                    z1 = GMax[j, zhs];
                    z2 = GMax[j, 1];
                    z3 = GMin[j, zhs];
                    z4 = GMin[j, 1];
                    fuhao = 1;
                }
                else if(zhs < -1000)
                {
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
                        throw new Exception("第　" + i + "　工况，第 " + j + " 线条组合参数错误" + "0 + 16 " + "错误：1-227");
                    }
                }

                if( wd.TensionAngleCode == "D" )
                {
                    x1 = Wind[j, zhs];
                    zjiao = LineParas.AngleMax;
                }
                //else if( wd.TensionAngleCode == "X" )
                else
                {
                    x1 = Windx[j, zhs];
                    zjiao = LineParas.AngleMin;
                }
                y1 = TensionMin[j, zhs];
                y2 = TensionMax[j, zhs];
                z1 = GMax[j, zhs];
                z2 = GMax[j, 1];
                z3 = GMin[j, zhs];
                z4 = GMin[j, 1];
                fuhao = -1;

                XX[i, j - 1] = formula.ZZGX(angle, x1, y1, y2, zjiao, LineParas.PulleyTensionDif,  out string strX);
                YY[i, j - 1] = formula.ZZGY(angle, x1, y1, y2, zjiao, LineParas.PulleyTensionDif * fuhao, out string strY);
                ZZ[i, j - 1] = formula.ZZGZ(z2, z1, fh, fhn, out string strZ);

                ProcessString.Add(Template.Wires[j-1] + " Fx= " + strX);
                ProcessString.Add(Template.Wires[j-1] + " Fy= " + strY);
                ProcessString.Add(Template.Wires[j-1] + " Fz= " + strZ);

                //If Abs(Val(Me.DataGridView7.Rows(i -1).Cells(j + 3).Value)) > 10000 And Me.truningPoint_CkBox.Checked = True Then
                {
                    float angf = 0;
                    //float angf = Trim(Me.DataGridView3.Rows(j - 1 - dxl).Cells(2).Value);
                    XX[i, j - 1 + wireNum - groudWireNum] = (float)(-ZZ[i, j - 1] * Math.Sin(angf * Math.PI / 180));
                    YY[i, j - 1 + wireNum - groudWireNum] = YY[i, j - 1];
                    ZZ[i, j - 1 + wireNum - groudWireNum] = (float)(ZZ[i, j - 1] + ZZ[i, j - 1] * Math.Cos(angf * Math.PI / 180));

                    ProcessString.Add(Template.Wires[j - 1] + "转向处 Fx= -" + ZZ[i, j - 1].ToString("0.00") + " * sin(" + angf + ") = " + XX[i, j + wireNum - groudWireNum].ToString("0.00"));
                    ProcessString.Add(Template.Wires[j - 1] + "转向处 Fy= " + YY[i, j - 1].ToString("0.00"));
                    ProcessString.Add(Template.Wires[j - 1] + "转向处 Fz= " + ZZ[i, j - 1].ToString("0.00") + " + " + ZZ[i, j - 1].ToString("0.00") + " * cos(" + angf + ") = " + ZZ[i, j - 1 + wireNum - groudWireNum].ToString("0.00"));

                    float fzz = XX[i, j - 1];
                    float fzz1 = ZZ[i, j - 1];
                    XX[i, j - 1] = XX[i, j - 1] + ZZ[i, j - 1] * (float)Math.Sin(angf * Math.PI / 180);
                    YY[i, j - 1] = YY[i, j - 1];
                    ZZ[i, j - 1] = ZZ[i, j - 1] - ZZ[i, j - 1] * (float)Math.Cos(angf * Math.PI / 180);

                    ProcessString.Add(Template.Wires[j - 1] + "导线处 Fx= " + fzz.ToString("0.00") + " + " + fzz1.ToString("0.00") + " * sin(" + angf + ") = " + XX[i, j - 1].ToString("0.00"));
                    ProcessString.Add(Template.Wires[j - 1] + "导线处 Fy= " + YY[i, j - 1].ToString("0.00"));
                    ProcessString.Add(Template.Wires[j - 1] + "导线处 Fz= " + fzz1.ToString("0.00") + " - " + fzz1.ToString("0.00") + " * cos(" + angf + ") = " + ZZ[i, j - 1].ToString("0.00"));
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

        /// <summary>
        /// 针对施工提线工况，只考虑最大垂荷，多用于直流输电塔
        /// 工况代码"C", "Ca", "Cb", "Cc", "Cd", "Ce", "Cf", "Cg", "Ch", "CLa", "CLb", "CLc", "CLd", "CLe", "CLf", "CLg", "CLh"
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        protected void DistributeInTixian(int i, int j)
        {
            float x1, y1, y2, z1;
            float fhn, zjiao;

            WorkConditionCombo wd = Template.WorkConditionCombos[i];

            int zhs = wd.WireIndexCodes[j-1], zhsAM;
            int angle = wd.WindDirectionCode;
            float fh = LineParas.WireExtraLoad / Paras.LoadRatio;

            if (Math.Abs(zhs) <= workConditionNum && Math.Abs(zhs) > 0)
            {
                //已安装
                if (zhs <= workConditionNum && zhs > 0)
                {
                    if( wd.TensionAngleCode == "D" )
                    {
                        x1 = Wind[j, zhs];
                        zjiao = LineParas.AngleMax;
                    }
                    //else if( wd.TensionAngleCode == "X" )
                    else
                    {
                        x1 = Windx[j, zhs];
                        zjiao = LineParas.AngleMin;
                    }
                    y1 = TensionMax[j, zhs];
                    y2 = TensionMin[j, zhs];
                    z1 = GMax[j, zhs];
                    //z2 = GMax[j, 1];
                }
                else
                {
                    if( wd.TensionAngleCode == "D" )
                    {
                        x1 = Wind[j, Math.Abs(zhs)];
                        zjiao = LineParas.AngleMax;
                    } 
                    //else if( wd.TensionAngleCode == "X" )
                    else
                    {
                        x1 = Windx[j, Math.Abs(zhs)];
                        zjiao = LineParas.AngleMin;
                    }
                    y1 = TensionMin[j, Math.Abs(zhs)];
                    y2 = TensionMax[j, Math.Abs(zhs)];
                    z1 = GMax[j, Math.Abs(zhs)];
                    //z2 = GMax[j, 1];
                }

                //已安装，无附加荷载
                XX[i, j - 1] = formula.ZZLX(angle, x1, y1, y2, zjiao, out string strX);
                YY[i, j - 1] = formula.ZZLY(angle, x1, y1, y2, zjiao, out string strY);
                ZZ[i, j - 1] = formula.ZZLZ1(z1, out string strZ);

                ProcessString.Add(Template.Wires[j-1] + " Fx= " + strX);
                ProcessString.Add(Template.Wires[j-1] + " Fy= " + strY);
                ProcessString.Add(Template.Wires[j-1] + " Fz= " + strZ);

            }
            else if (Math.Abs(zhs) > 1000)
            {
                //过滑车相
                if(zhs > 1000)
                {
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
                        throw new Exception("第　" + i + "　工况，第 " + j + " 线条组合参数错误" + "0 + 16 " + "错误：1-228");
                    }

                    if( wd.TensionAngleCode == "D" )
                    {
                        x1 = Wind[j, zhsAM];
                        zjiao = LineParas.AngleMax;
                    } 
                    //else if( wd.TensionAngleCode == "X" )
                    else
                    {
                        x1 = Windx[j, zhsAM];
                        zjiao = LineParas.AngleMin;
                    }
                    y1 = TensionMax[j, zhsAM];
                    y2 = TensionMin[j, zhsAM];
                    z1 = GMax[j, zhsAM];
                    //z2 = GMax[j, 1];
                    //z3 = GMin[j, zhsAM];
                    //z4 = GMin[j, 1];
                }
                else if(zhs < -1000)
                {
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
                        throw new Exception("第　" + i + "　工况，第 " + j + " 线条组合参数错误" + "0 + 16 " + "错误：1-229");
                    }
                    if( wd.TensionAngleCode == "D" )
                    {
                        x1 = Wind[j, zhsAM];
                        zjiao = LineParas.AngleMax;
                    } 
                    //else if( wd.TensionAngleCode == "X" )
                    else
                    {
                        x1 = Windx[j, zhsAM];
                        zjiao = LineParas.AngleMin;
                    }
                    y1 = TensionMin[j, zhsAM];
                    y2 = TensionMax[j, zhsAM];
                    z1 = GMax[j, zhsAM];
                    //z2 = GMax[j, 1];
                    //z3 = GMin[j, zhsAM];
                    //z4 = GMin[j, 1];
                }
                else
                {
                    zjiao = 0;
                    x1 = 0;
                    y1 = 0;
                    y2 = 0;
                    z1 = 0;
                    fhn = 0;
                }

                XX[i, j - 1] = formula.ZZLX(angle, x1, y1, y2, zjiao, out string strX);
                YY[i, j - 1] = formula.ZZLY(angle, x1, y1, y2, zjiao, out string strY);
                ZZ[i, j - 1] = formula.ZZCZ(z1, LineParas.HoistingCoef, fhn, fh, out string strZ);

                ProcessString.Add(Template.Wires[j-1] + " Fx= " + strX);
                ProcessString.Add(Template.Wires[j-1] + " Fy= " + strY);
                ProcessString.Add(Template.Wires[j-1] + " Fz= " + strZ);

                //If Abs(Val(Me.DataGridView7.Rows(i -1).Cells(j + 3).Value)) > 10000 And Me.truningPoint_CkBox.Checked = True Then
                {
                    float angf = 0;
                    //float angf = Trim(Me.DataGridView3.Rows(j - 1 - dxl).Cells(2).Value);
                    XX[i, j - 1 + wireNum - groudWireNum] = (float)(-ZZ[i, j - 1] * Math.Sin(angf * Math.PI / 180));
                    YY[i, j - 1 + wireNum - groudWireNum] = YY[i, j - 1];
                    ZZ[i, j - 1 + wireNum - groudWireNum] = (float)(ZZ[i, j - 1] + ZZ[i, j - 1] * Math.Cos(angf * Math.PI / 180));

                    ProcessString.Add(Template.Wires[j - 1] + "转向处 Fx= -" + ZZ[i, j - 1].ToString("0.00") + " * sin(" + angf + ") = " + XX[i, j - 1 + wireNum - groudWireNum].ToString("0.00"));
                    ProcessString.Add(Template.Wires[j - 1] + "转向处 Fy= " + YY[i, j - 1].ToString("0.00"));
                    ProcessString.Add(Template.Wires[j - 1] + "转向处 Fz= " + ZZ[i, j - 1].ToString("0.00") + " + " + ZZ[i, j - 1].ToString("0.00") + " * cos(" + angf + ") = " + ZZ[i, j - 1 + wireNum - groudWireNum].ToString("0.00"));

                    float fzz = XX[i, j - 1];
                    float fzz1 = ZZ[i, j - 1];
                    XX[i, j - 1] = XX[i, j - 1] + ZZ[i, j - 1] * (float)Math.Sin(angf * Math.PI / 180);
                    YY[i, j - 1] = YY[i, j - 1];
                    ZZ[i, j - 1] = ZZ[i, j - 1] - ZZ[i, j - 1] * (float)Math.Cos(angf * Math.PI / 180);

                    ProcessString.Add(Template.Wires[j - 1] + "导线处 Fx= " + fzz.ToString("0.00") + " + " + fzz1.ToString("0.00") + " * sin(" + angf + ") = " + XX[i, j - 1].ToString("0.00"));
                    ProcessString.Add(Template.Wires[j - 1] + "导线处 Fy= " + YY[i, j - 1].ToString("0.00"));
                    ProcessString.Add(Template.Wires[j - 1] + "导线处 Fz= " + fzz1.ToString("0.00") + " - " + fzz1.ToString("0.00") + " * cos(" + angf + ") = " + ZZ[i, j - 1].ToString("0.00"));
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
