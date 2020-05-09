using System;
using System.Data;
using System.Data.OleDb;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TowerLoadCals.BLL;
using TowerLoadCals.BLL.Structure;
using TowerLoadCals.Common;
using TowerLoadCals.DAL;
using TowerLoadCals.Mode;
using static TowerLoadCals.DAL.TowerTemplateReader;

namespace TowerLoadCals.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "DLL Files (*.dll)|*.dll"
            };

            if (openFileDialog.ShowDialog() != true)
                return;

            var saveFileDialog = new Microsoft.Win32.SaveFileDialog()
            {
                Filter = "Dat Files (*.dat)|*.dat",
            };

            if (saveFileDialog.ShowDialog() != true)
                return;

            DES.DesDecrypt(openFileDialog.FileName, saveFileDialog.FileName, "12345678");

            TowerTemplateReader TemplateReader = new TowerTemplateReader(TowerType.LineTower);

            TowerTemplate template = TemplateReader.Read(saveFileDialog.FileName);

            FormulaParas formulaParas = new FormulaParas();


            //结构重要性系数
            formulaParas.R1Install = 1f;
            formulaParas.R0Normal = 1.1f;

            //荷载分项系数
            formulaParas.RGBad = 1.3f;
            formulaParas.RGGood = 1.05f;
            formulaParas.RQ = 1.5f;

            //可变荷载组合系数
            formulaParas.VcFNormal = 1.1f;
            formulaParas.VcFInstall = 0.95f;
            formulaParas.VcFBroken = 0.95f;
            formulaParas.VcFUnevenIce = 0.95f;
            formulaParas.VcFCheck = 0.8f;

            //其他参数
            formulaParas.WindAdjustFactor = 1.3f;
            formulaParas.OtherWindAdjustFactor = 0.95f;
            formulaParas.DynamicCoef = 1.2f;
            formulaParas.AnchorWindCoef = 0.8f;
            formulaParas.AnchorGravityCoef = 0.8f;
            formulaParas.AnchorAngle = 25f;


            LoadDistributeLineTower loadLineTower = new LoadDistributeLineTower(formulaParas,template);

            string filePath = saveFileDialog.FileName.Substring(0, saveFileDialog.FileName.Length - 3) + "txt";

            loadLineTower.CalculateLoadDistribute(out float[,] xx, out float[,] yy, out float[,] zz, filePath);
        }
    }



}
