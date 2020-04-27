using System;
using System.Collections.Generic;
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

        protected float[][] Wind { get; set; }

        protected float[][] GMax { get; set; }

        protected float[][] GMin { get; set; }

        protected float[][] Tension1 { get; set; }

        protected float[][] Tension2 { get; set; }

        protected float [][] XX { get; set; }
        protected float[][] YY { get; set; }
        protected float[][] ZZ { get; set; }

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
          
        protected void ConvertTable(float[][] table)
        {
            
        }

        public void CalculateLoadDistribute(out float[][] xx, out float[][] yy, out float[][] zz)
        {
            int calNums = Template.WorkConditionCombos.Where(item => item.IsCalculate).ToList().Count;

            XX = new float[calNums][];
            YY = new float[calNums][];
            ZZ = new float[calNums][];

            int i = 0, j = 0;
            

            foreach (var wd in Template.WorkConditionCombos)
            {
                if (!wd.IsCalculate)
                    continue;

                foreach(int wic in wd.WirdIndexCodes)
                {
                    int zhs = wic;

                    switch (wd.WorkConditionCode)
                    {
                        case "N1":
                        case "N2":
                        case "D1":
                        case "D2":
                            DistributeInWindAndLowTemperature(i,j, zhs, wd.TensionAngleCode, wd.WorkConditionCode);
                            break;
                    }
                    j++;
                }
                i++;
            }

            xx = XX;
            yy = YY;
            zz = ZZ;
        }

        /// <summary>
        /// 针对大风和低温的工况
        /// 工况代码D1,D2,N1,N2
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="zhs"></param>
        /// <param name="angle"></param>
        /// <param name="workConditionCode"></param>
        protected void DistributeInWindAndLowTemperature(int i, int j, int zhs, int angle, string workConditionCode)
        {
            float x1, x2, y1, y2, z1, z2;
            float rg, zg, Vcb;

            int mz1 = Template.WorkConditongs.Count;

            if (zhs <= mz1 && zhs > 0)
            {
                x1 = Wind[j][zhs];
                y1 = Tension1[j][zhs];
                y2 = Tension2[j][zhs];
                z1 = GMax[j][zhs];
                z2 = GMin[j][zhs];
            }
            else if (zhs < 0 && zhs >= -mz1)
            {
                x1 = Wind[j][Math.Abs(zhs)];
                y1 = Tension1[j][Math.Abs(zhs)];
                y2 = Tension2[j][Math.Abs(zhs)];
                z1 = GMax[j][Math.Abs(zhs)];
                z2 = GMin[j][Math.Abs(zhs)];
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
                throw new ArgumentException("第　" + i + "　工况，第 " + j + " 线条组合参数错误，只能为-" + -mz1 + "～" + mz1 + "之间 " + "0 + 16 " + "错误：1-207");
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

            XX[i][j] = formula.ZXNX(angle, x1, Vcb);
            YY[i][j] = formula.ZXNY(angle, x1, y1, y2, Vcb);
            ZZ[i][j] = formula.ZXNZ(zg, rg, Vcb);
        }

    }
}
