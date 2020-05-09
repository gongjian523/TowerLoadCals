using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerLoadCals.Mode;

namespace TowerLoadCals.BLL.Structure
{
    
    /// <summary>
    /// 直线塔的荷载分配
    /// </summary>
    public class LoadDistributeLineTower
    {
        /// <summary>
        /// 从界面获取的公共参数
        /// </summary>
        protected FormulaParas Paras { get; set; }

        /// <summary>
        /// 模板参数
        /// </summary>
        protected TowerTemplate Template { get; set; }

        protected float[,] Wind { get; set; }

        protected float[,] GMax { get; set; }

        protected float[,] GMin { get; set; }

        protected float[,] TensionMax { get; set; }

        protected float[,] TensionMin { get; set; }

        protected float [,] XX { get; set; }
        protected float[,] YY { get; set; }
        protected float[,] ZZ { get; set; }

        protected List<string>  ProcessString { get; set; }

        protected FormulaLineTower formula;


        public LoadDistributeLineTower(FormulaParas para, TowerTemplate template, float[][] table)
        {

            ProcessString = new List<string>();
            formula = new FormulaLineTower(para);

            Paras = para;
            Template = template;

            ConvertTable(table);
        }

        public LoadDistributeLineTower(FormulaParas para, TowerTemplate template)
        {

            ProcessString = new List<string>();
            formula = new FormulaLineTower(para);

            Paras = para;
            Template = template;

            GetTable();
        }


        protected void ConvertTable(float[][] table)
        {
            
        }

        protected void GetTable()
        {
            string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + "D:\\00-项目\\P-200325-杆塔负荷程序\\21.xlsx" + ";" + "Extended Properties=Excel 8.0;";
            OleDbConnection conn = new OleDbConnection(strConn);
            conn.Open();
            string strExcel = "";
            OleDbDataAdapter myCommand = null;
            DataSet ds = null;
            strExcel = "select * from [sheet1$]";
            myCommand = new OleDbDataAdapter(strExcel, strConn);
            ds = new DataSet();
            myCommand.Fill(ds, "table1");

            //Wind = new float[][] { new float[]{0,0,0,0,0,0,0,0 }, new float[] {0,0,0,0,0,0,0,0 }, new float[] { 0, 0, 0, 0, 0, 0, 0, 0 }, new float[] { 0, 0, 0, 0, 0, 0, 0, 0 },
            //new float[]{0,0,0,0,0,0,0,0 }, new float[] {0,0,0,0,0,0,0,0 }, new float[] { 0, 0, 0, 0, 0, 0, 0, 0 }, new float[] { 0, 0, 0, 0, 0, 0, 0, 0 },
            //new float[]{0,0,0,0,0,0,0,0 }, new float[] {0,0,0,0,0,0,0,0 }, new float[] { 0, 0, 0, 0, 0, 0, 0, 0 }, new float[] { 0, 0, 0, 0, 0, 0, 0, 0 }};
            //GMax = new float[][] { new float[]{0,0,0,0,0,0,0,0 }, new float[] {0,0,0,0,0,0,0,0 }, new float[] { 0, 0, 0, 0, 0, 0, 0, 0 }, new float[] { 0, 0, 0, 0, 0, 0, 0, 0 },
            //new float[]{0,0,0,0,0,0,0,0 }, new float[] {0,0,0,0,0,0,0,0 }, new float[] { 0, 0, 0, 0, 0, 0, 0, 0 }, new float[] { 0, 0, 0, 0, 0, 0, 0, 0 },
            //new float[]{0,0,0,0,0,0,0,0 }, new float[] {0,0,0,0,0,0,0,0 }, new float[] { 0, 0, 0, 0, 0, 0, 0, 0 }, new float[] { 0, 0, 0, 0, 0, 0, 0, 0 }};
            //GMin = new float[][] { new float[]{0,0,0,0,0,0,0,0 }, new float[] {0,0,0,0,0,0,0,0 }, new float[] { 0, 0, 0, 0, 0, 0, 0, 0 }, new float[] { 0, 0, 0, 0, 0, 0, 0, 0 },
            //new float[]{0,0,0,0,0,0,0,0 }, new float[] {0,0,0,0,0,0,0,0 }, new float[] { 0, 0, 0, 0, 0, 0, 0, 0 }, new float[] { 0, 0, 0, 0, 0, 0, 0, 0 },
            //new float[]{0,0,0,0,0,0,0,0 }, new float[] {0,0,0,0,0,0,0,0 }, new float[] { 0, 0, 0, 0, 0, 0, 0, 0 }, new float[] { 0, 0, 0, 0, 0, 0, 0, 0 }};
            //TensionMax = new float[][] { new float[]{0,0,0,0,0,0,0,0 }, new float[] {0,0,0,0,0,0,0,0 }, new float[] { 0, 0, 0, 0, 0, 0, 0, 0 }, new float[] { 0, 0, 0, 0, 0, 0, 0, 0 },
            //new float[]{0,0,0,0,0,0,0,0 }, new float[] {0,0,0,0,0,0,0,0 }, new float[] { 0, 0, 0, 0, 0, 0, 0, 0 }, new float[] { 0, 0, 0, 0, 0, 0, 0, 0 },
            //new float[]{0,0,0,0,0,0,0,0 }, new float[] {0,0,0,0,0,0,0,0 }, new float[] { 0, 0, 0, 0, 0, 0, 0, 0 }, new float[] { 0, 0, 0, 0, 0, 0, 0, 0 }};
            //TensionMin = new float[][] { new float[]{0,0,0,0,0,0,0,0 }, new float[] {0,0,0,0,0,0,0,0 }, new float[] { 0, 0, 0, 0, 0, 0, 0, 0 }, new float[] { 0, 0, 0, 0, 0, 0, 0, 0 },
            //new float[]{0,0,0,0,0,0,0,0 }, new float[] {0,0,0,0,0,0,0,0 }, new float[] { 0, 0, 0, 0, 0, 0, 0, 0 }, new float[] { 0, 0, 0, 0, 0, 0, 0, 0 },
            //new float[]{0,0,0,0,0,0,0,0 }, new float[] {0,0,0,0,0,0,0,0 }, new float[] { 0, 0, 0, 0, 0, 0, 0, 0 }, new float[] { 0, 0, 0, 0, 0, 0, 0, 0 }};


            Wind = new float[Template.WorkConditongs.Count, Template.Wires.Count];
            GMax = new float[Template.WorkConditongs.Count, Template.Wires.Count];
            GMin = new float[Template.WorkConditongs.Count, Template.Wires.Count];
            TensionMax = new float[Template.WorkConditongs.Count, Template.Wires.Count];
            TensionMin = new float[Template.WorkConditongs.Count, Template.Wires.Count];


            for (int j = 0; j < Template.WorkConditongs.Count; j++)
            {
                for(int i = 0; i < Template.Wires.Count;  i++ )
                {
                    object obj = ds.Tables[0].Rows[4 + 6 * i][j+1];
                    float.TryParse(obj.ToString(), out Wind[j,i]);
                    object obj2 = ds.Tables[0].Rows[5 + 6 * i][j + 1];
                    float.TryParse(obj2.ToString(), out GMax[j,i]);
                    object obj3 = ds.Tables[0].Rows[6 + 6 * i][j + 1];
                    float.TryParse(obj3.ToString(), out GMin[j,i]);
                    object obj4 = ds.Tables[0].Rows[7 + 6 * i][j + 1];
                    float.TryParse(obj4.ToString(), out TensionMax[j,i]);
                    object obj5 = ds.Tables[0].Rows[8 + 6 * i][j + 1];
                    float.TryParse(obj5.ToString(), out TensionMin[j,i]);
                }
            }
        }

        public void CalculateLoadDistribute(out float[,] xx, out float[,] yy, out float[,] zz, string path)
        {
            int calNums = Template.WorkConditionCombos.Where(item => item.IsCalculate).ToList().Count;

            XX = new float[calNums, Template.Wires.Count];
            YY = new float[calNums, Template.Wires.Count];
            ZZ = new float[calNums, Template.Wires.Count];

            int i = 0, j = 0;

            ProcessString.Add(Template.Name + " " + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"));

            foreach (var wd in Template.WorkConditionCombos)
            {
                ProcessString.Add("");
                ProcessString.Add(wd.WorkComment);

                if (!wd.IsCalculate)
                    continue;

                j = 0;
                foreach(int wic in wd.WirdIndexCodes)
                {
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
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        protected void DistributeInWindAndLowTemperature(int i, int j)
        {
            float x1, y1, y2, z1, z2;
            float rg, zg, Vcb;

            WorkConditionCombo wd = Template.WorkConditionCombos[i];

            int zhs = wd.WirdIndexCodes[j];
            int angle = wd.WindDirectionCode;
            string workConditionCode = wd.WorkConditionCode;

            int mz1 = Template.WorkConditongs.Count;

            if (zhs <= mz1 && zhs > 0)
            {
                x1 = Wind[j,zhs];
                y1 = TensionMax[j, zhs];
                y2 = TensionMin[j, zhs];
                z1 = GMax[j, zhs];
                z2 = GMin[j, zhs];
            }
            else if (zhs < 0 && zhs >= -mz1)
            {
                x1 = Wind[j, Math.Abs(zhs)];
                y1 = TensionMax[j, Math.Abs(zhs)];
                y2 = TensionMin[j, Math.Abs(zhs)];
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
                throw new Exception("第　" + i + "　工况，第 " + j + " 线条组合参数错误，只能为-" + -mz1 + "～" + mz1 + "之间 " + "0 + 16 " + "错误：1-207");
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

            XX[i,j] = formula.ZXNX(angle, x1, Vcb, out string strX);
            YY[i,j] = formula.ZXNY(angle, x1, y1, y2, Vcb, out string strY);
            ZZ[i,j] = formula.ZXNZ(zg, rg, Vcb, out string strZ);

            ProcessString.Add(Template.Wires[j] + " Fx= " + strX);
            ProcessString.Add(Template.Wires[j] + " Fy= " + strY);
            ProcessString.Add(Template.Wires[j] + " Fz= " + strZ);
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

            int zhs = wd.WirdIndexCodes[j];
            int angle = wd.WindDirectionCode;
            string workConditionCode = wd.WorkConditionCode;
            int mz1 = Template.WorkConditongs.Count;

            int zhsM = (Math.Abs(zhs)) % 1000;

            if (zhs != 0)
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

            if(zhsM > mz1)
            {
                x1 = 9999;
                y1 = 9999;
                y2 = 9999;
                z1 = 9999;
                z2 = 9999;
                z3 = 9999;
                z4 = 9999;
                throw new Exception("第　" + i + "　工况，第 " + j + " 线条组合参数错误，只能为-" + -mz1 + "～" + mz1 + "之间 " + "0 + 16 " + "错误：1-208");
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
            else if (workConditionCode == "U1")
            {
                rg = Paras.RGBad;
                zg = z1;
                zg1 = z2;
                fhn = Paras.VcFUnevenIce;
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
        
            XX[i,j] = formula.ZXIYX(angle, x1, fhn, Vloadx,out string strX);
            YY[i,j] = formula.ZXIYY(angle, x1, y1, y2, fhn, Vloadx, out string strY);
            ZZ[i,j] = formula.ZXIYZ(zg1, zg, rg, fhn, Vloadx, out string strZ);

            ProcessString.Add(Template.Wires[j] + " Fx= " + strX);
            ProcessString.Add(Template.Wires[j] + " Fy= " + strY);
            ProcessString.Add(Template.Wires[j] + " Fz= " + strZ);
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

            int zhs = wd.WirdIndexCodes[j], zhsAM;
            int angle = wd.WindDirectionCode;
            string workConditionCode = wd.WorkConditionCode;
            int mz1 = Template.WorkConditongs.Count;

            if(Math.Abs(zhs) <= mz1 && Math.Abs(zhs) > 0)
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

                XX[i,j] = formula.ZXTX(angle, x1, out string strX);
                YY[i,j] = formula.ZXTY(angle, x1, y1, y2, fuhao, out string strY);
                ZZ[i,j] = formula.ZXTZ2(z2, z1, out string strZ);

                ProcessString.Add(Template.Wires[j] + " Fx= " + strX);
                ProcessString.Add(Template.Wires[j] + " Fy= " + strY);
                ProcessString.Add(Template.Wires[j] + " Fz= " + strZ);

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

                XX[i,j] = formula.ZXTX(angle, x1, out string strX);
                YY[i,j] = formula.ZXTY(angle, x1, y1, y2, fuhao, out string strY);
                ZZ[i,j] = formula.ZXTZ1(z2, z1, out string strZ);

                ProcessString.Add(Template.Wires[j] + " Fx= " + strX);
                ProcessString.Add(Template.Wires[j] + " Fy= " + strY);
                ProcessString.Add(Template.Wires[j] + " Fz= " + strZ);
            }
            else if(zhs == 0)
            {
                XX[i,j] = 0;
                YY[i,j] = 0;
                ZZ[i,j] = 0;

                ProcessString.Add(Template.Wires[j] + " Fx= " + 0);
                ProcessString.Add(Template.Wires[j] + " Fy= " + 0);
                ProcessString.Add(Template.Wires[j] + " Fz= " + 0);
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

            int zhs = wd.WirdIndexCodes[j], zhsAM;
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

                XX[i,j] = formula.ZXLX(angle, x1, out string strX);
                YY[i,j] = formula.ZXLY(angle, x1, y1, y2, out string strY);
                ZZ[i,j] = formula.ZXLZ1(z1, out string strZ);

                ProcessString.Add(Template.Wires[j] + " Fx= " + strX);
                ProcessString.Add(Template.Wires[j] + " Fy= " + strY);
                ProcessString.Add(Template.Wires[j] + " Fz= " + strZ);

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
                    zg = Paras.diaoxi;
                    fhn = Math.Abs(zhs) / 1000;
                    zhsAM = Math.Abs(zhs) % 1000;
                }
                else
                {
                    fuhao = 1;
                    //此处为吊装荷载系数
                    zg = Paras.diaoxi;
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

                XX[i,j] = formula.ZXLX(angle, x1, out string strX);
                YY[i,j] = formula.ZXLY(angle, x1, y1, y2, out string strY);
                ZZ[i,j] = formula.ZXLZ2(z1, zg, Paras.fh * fhn, out string strZ);

                ProcessString.Add(Template.Wires[j] + " Fx= " + strX);
                ProcessString.Add(Template.Wires[j] + " Fy= " + strY);
                ProcessString.Add(Template.Wires[j] + " Fz= " + strZ);
            }
            else if (zhs == 0)
            {
                XX[i,j] = 0;
                YY[i,j] = 0;
                ZZ[i,j] = 0;

                ProcessString.Add(Template.Wires[j] + " Fx= " + 0);
                ProcessString.Add(Template.Wires[j] + " Fy= " + 0);
                ProcessString.Add(Template.Wires[j] + " Fz= " + 0);
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

            int zhs = wd.WirdIndexCodes[j], zhsAM;
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

                if (j <= Paras.dxl && y1 >= Paras.mz)
                {
                    //地线有开段时
                    deta1 = y1;
                    deta2 = 90;
                    deta3 = 0;
                }
                else
                {
                    //地线不开段和导线
                    deta1 = Paras.mz;
                    deta2 = Paras.AnchorAngle;
                    deta3 = Paras.AnchorAngle;
                }

                x1 = Wind[j,zhsAM];
                z1 = GMax[j,zhsAM];
                z2 = GMin[j,zhsAM];

                XX[i,j] = formula.ZXMX2(angle, x1, out string strX);
                YY[i,j] = formula.ZXMY2(angle, x1, deta1 * fuhao, deta2, out string strY);
                ZZ[i,j] = formula.ZXMZ2(z1, Paras.fh * fhn, deta1, deta3, out string strZ);

                ProcessString.Add(Template.Wires[j] + " Fx= " + strX);
                ProcessString.Add(Template.Wires[j] + " Fy= " + strY);
                ProcessString.Add(Template.Wires[j] + " Fz= " + strZ);

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

                    if (j <= Paras.dxl && y1 >= Paras.mz)
                    {
                        //地线有开段时
                        if (y1 > 0)
                        {
                            deta1 = y1 * Paras.gqx;
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
                        deta1 = Paras.mz;
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

                    if (j <= Paras.dxl && Math.Abs(y1) >= Math.Abs(Paras.mz))
                    {
                        //地线有开段时
                        if (y1 < 0)
                        {
                            deta1 = y1 * Paras.gqx;
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
                        deta1 = -Paras.mz;
                        deta2 = Paras.AnchorAngle;
                        deta3 = Paras.AnchorAngle;
                    }
                }
                XX[i,j] = formula.ZXMX1(angle, x1, out string strX);
                YY[i,j] = formula.ZXMY1(angle, x1, deta1, deta2, out string strY);
                ZZ[i,j] = formula.ZXMZ1(z1, Paras.fh * fhn, deta1, deta3, out string strZ);

                ProcessString.Add(Template.Wires[j] + " Fx= " + strX);
                ProcessString.Add(Template.Wires[j] + " Fy= " + strY);
                ProcessString.Add(Template.Wires[j] + " Fz= " + strZ);
            }
            else if (zhs == 0)
            {
                XX[i,j] = 0;
                YY[i,j] = 0;
                ZZ[i,j] = 0;

                ProcessString.Add(Template.Wires[j] + " Fx= " + 0);
                ProcessString.Add(Template.Wires[j] + " Fy= " + 0);
                ProcessString.Add(Template.Wires[j] + " Fz= " + 0);
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

            int zhs = wd.WirdIndexCodes[j], zhsAM;
            int angle = wd.WindDirectionCode;
            string workConditionCode = wd.WorkConditionCode;
            int mz1 = Template.WorkConditongs.Count;

            //OPGW锚线特殊工况，其表达式与常规锚线有所不同，需区分
            if (zhs < 1000 && zhs > 0)
            {
                x1 = Wind[j,zhs];
                y1 = TensionMax[j,zhs];
                y2 = TensionMin[j,zhs];
                z1 = GMax[j,zhs];
                z2 = GMin[j,zhs];

                //刚老师意见，无论开断与否，OPGW已架相均应该有荷载，与常规不符
                XX[i,j] = formula.MO2X(angle, x1, out string strX);
                YY[i,j] = formula.MO2Y(angle, x1, y1, y2, out string strY);
                ZZ[i,j] = formula.MO2Z(z1, out string strZ);

                ProcessString.Add(Template.Wires[j] + " Fx= " + strX);
                ProcessString.Add(Template.Wires[j] + " Fy= " + strY);
                ProcessString.Add(Template.Wires[j] + " Fz= " + strZ);

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
                    deta1 = Paras.gqx;
                    XX[i,j] = formula.MO1X(angle, x1, out string strX);
                    XX[i,j] = formula.MO1Y(angle, x1, y1, deta1, y2, out string strY);
                    XX[i,j] = formula.MO1Z(z1, y1, deta1, Paras.fh * fhn, out string strZ);

                    ProcessString.Add(Template.Wires[j] + " Fx= " + strX);
                    ProcessString.Add(Template.Wires[j] + " Fy= " + strY);
                    ProcessString.Add(Template.Wires[j] + " Fz= " + strZ);
                }
                else
                {
                    deta1 = 1;
                    XX[i,j] = 0;
                    YY[i,j] = 0;
                    ZZ[i,j] = 0;

                    ProcessString.Add(Template.Wires[j] + " Fx= " + 0);
                    ProcessString.Add(Template.Wires[j] + " Fy= " + 0);
                    ProcessString.Add(Template.Wires[j] + " Fz= " + 0);
                }
            }
            else if (zhs == 0)
            {
                XX[i,j] = 0;
                YY[i,j] = 0;
                ZZ[i,j] = 0;

                ProcessString.Add(Template.Wires[j] + " Fx= " + 0);
                ProcessString.Add(Template.Wires[j] + " Fy= " + 0);
                ProcessString.Add(Template.Wires[j] + " Fz= " + 0);
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

            int zhs = wd.WirdIndexCodes[j], zhsAM;
            int angle = wd.WindDirectionCode;
            string workConditionCode = wd.WorkConditionCode;
            int mz1 = Template.WorkConditongs.Count;

            
            if (Math.Abs(zhs) < mz1 && Math.Abs(zhs) > 0)
            {
                //已安装
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

                XX[i,j] = formula.ZXLX(angle, x1, out string strX);
                YY[i,j] = formula.ZXLY(angle, x1, y1, y2, out string strY);
                ZZ[i,j] = formula.ZXLZ1(z1, out string strZ);

                ProcessString.Add(Template.Wires[j] + " Fx= " + strX);
                ProcessString.Add(Template.Wires[j] + " Fy= " + strY);
                ProcessString.Add(Template.Wires[j] + " Fz= " + strZ);

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

                if(zhsAM > mz1)
                {
                    throw new Exception("第　" + i + "　工况，第 " + j + " 线条组合参数错误" + "0 + 16 " + "错误：1-213");
                }

                y1 = TensionMax[j,zhsAM];
                y2 = TensionMin[j,zhsAM];

                if (j <= Paras.dxl && y1 >= Paras.mz)
                {
                    //地线有开段时
                    deta1 = y1;
                    deta2 = 90;
                    deta3 = 0;
                }
                else
                {
                    //地线不开段和导线
                    deta1 = Paras.mz;
                    deta2 = Paras.AnchorAngle;
                    deta3 = Paras.AnchorAngle;
                }

                x1 = Wind[j,zhsAM];
                z1 = GMax[j,zhsAM];
                z2 = GMin[j,zhsAM];

                XX[i,j] = formula.ZXMX2(angle, x1, out string strX);
                YY[i,j] = formula.ZXMY2(angle, x1, deta1 * fuhao, deta2, out string strY);
                ZZ[i,j] = formula.ZXMZ2(z1, Paras.fh * fhn, deta1, deta3, out string strZ);

                ProcessString.Add(Template.Wires[j] + " Fx= " + strX);
                ProcessString.Add(Template.Wires[j] + " Fy= " + strY);
               ProcessString.Add(Template.Wires[j] + " Fz= " + strZ);
                
            }
            else if ( Math.Abs(zhs) > 1000)
            {
                //过滑车 >10000 表示转向过滑车
                if (zhs > 0)
                {
                    y1 = Paras.ghcz;
                }
                else
                {
                    y1 = -Paras.ghcz;
                }

                if(Math.Abs(zhs) < 1000)
                {
                    fhn = Math.Abs(zhs) / 1000;
                }
                else
                {
                    fhn = (Math.Abs(zhs) - 10000) / 1000;
                }

                zhsAM = Math.Abs(zhs) % 1000;

                if(zhsAM > mz1)
                {
                    throw new Exception("第　" + i + "　工况，第 " + j + " 线条组合参数错误" + "0 + 16 " + "错误：1-214");
                }

                x1 = Wind[j,zhsAM];
                z1 = GMax[j,1];
                z2 = GMax[j,zhsAM];

                XX[i,j] = formula.GLX(angle, x1, out string strX);
                YY[i,j] = formula.GLY(angle, x1, y1, out string strY);
                ZZ[i,j] = formula.GLZ(z1, z2, Paras.fh* fhn, out string strZ);

                ProcessString.Add(Template.Wires[j] + " Fx= " + strX);
                ProcessString.Add(Template.Wires[j] + " Fy= " + strY);
                ProcessString.Add(Template.Wires[j] + " Fz= " + strZ);

                //TODO
                //If Abs(Val(Me.DataGridView7.Rows(i -1).Cells(j + 3).Value)) > 10000 And Me.truningPoint_CkBox.Checked = True Then
                //angf = Trim(Me.DataGridView3.Rows(j - 1 - dxl).Cells(2).Value)
                // XX(i, j + lz - dxl) = -ZZ(i, j) * Sin(angf * PI / 180)
                //YY(i, j + lz - dxl) = YY(i, j)
                //ZZ(i, j + lz - dxl) = ZZ(i, j) + ZZ(i, j) * Cos(angf * PI / 180)

                //PrintLine(1, Form5.DataGridView13.Rows(j - 1).Cells(1).Value & "转向处 Fx= -" & Format(ZZ(i, j), "0.00") & " x " & "sin(" & angf & ")= " & Format(XX(i, j + lz - dxl), "0.00"))
                //PrintLine(1, Form5.DataGridView13.Rows(j - 1).Cells(1).Value & "转向处 Fy= " & Format(YY(i, j), "0.00"))
                //PrintLine(1, Form5.DataGridView13.Rows(j - 1).Cells(1).Value & "转向处 Fz= " & Format(ZZ(i, j), "0.00") & " + " & Format(ZZ(i, j), "0.00") & " x " & "cos(" & angf & ")= " & Format(ZZ(i, j + lz - dxl), "0.00"))

                //Dim fzz!, fzz1!
                //fzz = XX(i, j)
                //fzz1 = ZZ(i, j)
                //XX(i, j) = XX(i, j) + ZZ(i, j) * Sin(angf * PI / 180)
                //YY(i, j) = YY(i, j)
                //ZZ(i, j) = ZZ(i, j) - ZZ(i, j) * Cos(angf * PI / 180)

                //PrintLine(1, Form5.DataGridView13.Rows(j - 1).Cells(1).Value & "导线处 Fx= " & Format(fzz, "0.00") & " + " & Format(fzz1, "0.00") & " x " & "sin(" & angf & ")= " & Format(XX(i, j), "0.00"))
                //PrintLine(1, Form5.DataGridView13.Rows(j - 1).Cells(1).Value & "导线处 Fy= " & Format(YY(i, j), "0.00"))
                //PrintLine(1, Form5.DataGridView13.Rows(j - 1).Cells(1).Value & "导线处 Fz= " & Format(fzz1, "0.00") & " - " & Format(fzz1, "0.00") & " x " & "cos(" & angf & ")= " & Format(ZZ(i, j), "0.00"))

                //End If

            }
            else if (zhs == 0)
            {
                XX[i,j] = 0;
                YY[i,j] = 0;
                ZZ[i,j] = 0;

                ProcessString.Add(Template.Wires[j] + " Fx= " + 0);
                ProcessString.Add(Template.Wires[j] + " Fy= " + 0);
                ProcessString.Add(Template.Wires[j] + " Fz= " + 0);
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

            int zhs = wd.WirdIndexCodes[j], zhsAM;
            int angle = wd.WindDirectionCode;
            string workConditionCode = wd.WorkConditionCode;
            int mz1 = Template.WorkConditongs.Count;


            if (Math.Abs(zhs) < mz1 && Math.Abs(zhs) > 0)
            {
                //已安装
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

                XX[i,j] = formula.ZXLX(angle, x1, out string strX);
                YY[i,j] = formula.ZXLY(angle, x1, y1, y2, out string strY);
                ZZ[i,j] = formula.ZXLZ1(z1, out string strZ);

                ProcessString.Add(Template.Wires[j] + " Fx= " + strX);
                ProcessString.Add(Template.Wires[j] + " Fy= " + strY);
                ProcessString.Add(Template.Wires[j] + " Fz= " + strZ);

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

                if (zhsAM > mz1)
                {
                    throw new Exception("第　" + i + "　工况，第 " + j + " 线条组合参数错误" + "0 + 16 " + "错误：1-215");
                }

                y1 = TensionMax[j,zhsAM];
                y2 = TensionMin[j,zhsAM];

                if (j <= Paras.dxl && y1 >= Paras.mz)
                {
                    //地线有开段时
                    deta1 = y1;
                    deta2 = 90;
                    deta3 = 0;
                }
                else
                {
                    //地线不开段和导线
                    deta1 = Paras.mz;
                    deta2 = Paras.AnchorAngle;
                    deta3 = Paras.AnchorAngle;
                }

                x1 = Wind[j,zhsAM];
                z1 = GMax[j,zhsAM];
                z2 = GMin[j,zhsAM];

                XX[i,j] = formula.ZXMX2(angle, x1, out string strX);
                YY[i,j] = formula.ZXMY2(angle, x1, deta1 * fuhao, deta2, out string strY);
                ZZ[i,j] = formula.ZXMZ2(z1, Paras.fh * fhn, deta1, deta3, out string strZ);

                ProcessString.Add(Template.Wires[j] + " Fx= " + strX);
                ProcessString.Add(Template.Wires[j] + " Fy= " + strY);
                ProcessString.Add(Template.Wires[j] + " Fz= " + strZ);

            }
            else if (Math.Abs(zhs) > 1000)
            {
                //施工提线相
                if (Math.Abs(zhs) < 1000)
                {
                    fhn = Math.Abs(zhs) / 1000;
                }
                else
                {
                    fhn = (Math.Abs(zhs) - 10000) / 1000;
                }

                zhsAM = Math.Abs(zhs) % 1000;

                if (zhsAM > mz1)
                {
                    throw new Exception("第　" + i + "　工况，第 " + j + " 线条组合参数错误" + "0 + 16 " + "错误：1-216");
                }


                x1 = Wind[j,zhsAM];
                z1 = GMax[j,zhsAM];
                z2 = GMin[j,zhsAM];

                XX[i,j] = formula.ZXCX(angle, x1, out string strX);
                YY[i,j] = formula.ZXCY(angle, x1, Paras.diaoxi, z1, out string strY);
                ZZ[i,j] = formula.ZXCZ(Paras.fh * fhn, Paras.diaoxi, z1, out string strZ);

                ProcessString.Add(Template.Wires[j] + " Fx= " + strX);
                ProcessString.Add(Template.Wires[j] + " Fy= " + strY);
                ProcessString.Add(Template.Wires[j] + " Fz= " + strZ);

                //TODO
                //Dim angf%
                //If Abs(Val(Me.DataGridView7.Rows(i - 1).Cells(j + 3).Value)) > 10000 And Me.truningPoint_CkBox.Checked = True Then
                //    angf = Trim(Me.DataGridView3.Rows(j - 1 - dxl).Cells(2).Value)
                //    XX(i, j + lz - dxl) = -ZZ(i, j) * Sin(angf * PI / 180)
                //    YY(i, j + lz - dxl) = YY(i, j)
                //    ZZ(i, j + lz - dxl) = ZZ(i, j) + ZZ(i, j) * Cos(angf * PI / 180)

                //    PrintLine(1, Form5.DataGridView13.Rows(j - 1).Cells(1).Value & "转向处 Fx= -" & Format(ZZ(i, j), "0.00") & " x " & "sin(" & angf & ")= " & Format(XX(i, j + lz - dxl), "0.00"))
                //    PrintLine(1, Form5.DataGridView13.Rows(j - 1).Cells(1).Value & "转向处 Fy= " & Format(YY(i, j), "0.00"))
                //    PrintLine(1, Form5.DataGridView13.Rows(j - 1).Cells(1).Value & "转向处 Fz= " & Format(ZZ(i, j), "0.00") & " + " & Format(ZZ(i, j), "0.00") & " x " & "cos(" & angf & ")= " & Format(ZZ(i, j + lz - dxl), "0.00"))

                //    Dim fzz!, fzz1!
                //    fzz = XX(i, j)
                //    fzz1 = ZZ(i, j)
                //    XX(i, j) = XX(i, j) + ZZ(i, j) * Sin(angf * PI / 180)
                //    YY(i, j) = YY(i, j)
                //    ZZ(i, j) = ZZ(i, j) - ZZ(i, j) * Cos(angf * PI / 180)

                //    PrintLine(1, Form5.DataGridView13.Rows(j - 1).Cells(1).Value & "导线处 Fx= " & Format(fzz, "0.00") & " + " & Format(fzz1, "0.00") & " x " & "sin(" & angf & ")= " & Format(XX(i, j), "0.00"))
                //    PrintLine(1, Form5.DataGridView13.Rows(j - 1).Cells(1).Value & "导线处 Fy= " & Format(YY(i, j), "0.00"))
                //    PrintLine(1, Form5.DataGridView13.Rows(j - 1).Cells(1).Value & "导线处 Fz= " & Format(fzz1, "0.00") & " - " & Format(fzz1, "0.00") & " x " & "cos(" & angf & ")= " & Format(ZZ(i, j), "0.00"))

                //End If

            }
            else if (zhs == 0)
            {
                XX[i,j] = 0;
                YY[i,j] = 0;
                ZZ[i,j] = 0;

                ProcessString.Add(Template.Wires[j] + " Fx= " + 0);
                ProcessString.Add(Template.Wires[j] + " Fy= " + 0);
                ProcessString.Add(Template.Wires[j] + " Fz= " + 0);
            }
        }
    }
}
