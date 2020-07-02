using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TowerLoadCals.BLL;
using TowerLoadCals.Common;
using TowerLoadCals.DAL;
using TowerLoadCals.Mode;

namespace TowerLoadCals.Test
{
    [TestClass]
    public class LineTower
    {
        [TestMethod]
        public void TestMethod5()
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "DLL Files (*.dll)|*.dll"
            };

            if (openFileDialog.ShowDialog() != true)
                return;

            var openTemplateDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "Excel Files (*.xlsx)|*.xlsx"
            };

            if (openTemplateDialog.ShowDialog() != true)
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

            formulaParas.Type = TowerType.LineTower;
            formulaParas.LoadRatio = 1;

            //结构重要性系数
            formulaParas.R1Install = 1f;
            formulaParas.R0Normal = 1.1f;

            //荷载分项系数
            formulaParas.RGBad = 1.2f;
            formulaParas.RGGood = 1f;
            formulaParas.RQ = 1.4f;

            //可变荷载组合系数
            formulaParas.VcFNormal = 1f;
            formulaParas.VcFInstall = 0.9f;
            formulaParas.VcFBroken = 0.9f;
            formulaParas.VcFUnevenIce = 0.9f;
            formulaParas.VcFCheck = 0.75f;

            //其他参数
            formulaParas.WindAdjustFactor = 1.2f;
            formulaParas.OtherWindAdjustFactor = 1f;
            formulaParas.DynamicCoef = 1.1f;
            formulaParas.AnchorWindCoef = 0.7f;
            formulaParas.AnchorGravityCoef = 0.7f;
            formulaParas.AnchorAngle = 20f;

            StruLineParas[] lineParas = new StruLineParas[] {
                new StruLineParas
                {
                    HoistingCoef = 2f,
                    WireExtraLoad = 4,
                    AnchorTension = 55.60f,
                    PulleyTensionDif = 0,
                    DrawingCoef = 1.15f
                },
                new StruLineParas
                {
                    HoistingCoef = 2f,
                    WireExtraLoad = 4,
                    AnchorTension = 55.60f,
                    PulleyTensionDif = 0,
                    DrawingCoef = 1.15f
                },
                new StruLineParas
                {
                    HoistingCoef = 2f,
                    WireExtraLoad = 8,
                    AnchorTension = 428.53f,
                    PulleyTensionDif = 30,
                    DrawingCoef = 1.15f
                },
                 new StruLineParas
                {
                    HoistingCoef = 2f,
                    WireExtraLoad = 8,
                    AnchorTension = 428.53f,
                    PulleyTensionDif = 30,
                    DrawingCoef = 1.15f
                }
            };

            List<HangingPointParas> normalList = new List<HangingPointParas>()
            {
                new HangingPointParas()
                {
                    Index = 1,
                    WireType = "左地",
                    StringType = "常规",
                    Points =  new string[] {"11", "13" }
                },
                new HangingPointParas()
                {
                    Index = 2,
                    WireType = "右地",
                    StringType = "常规",
                    Points =  new string[] {"10", "12" }
                },
                new HangingPointParas()
                {
                    Index = 3,
                    WireType = "左导",
                    StringType = "V1",
                    Points =  new string[] { "101", "471"}
                },
                new HangingPointParas()
                {
                    Index = 4,
                    WireType = "右导",
                    StringType = "V1",
                    Points =  new string[] {"470", "100" }
                }
            };

            List<HangingPointParas> installList = new List<HangingPointParas>()
            {
                new HangingPointParas()
                {
                    Index = 1,
                    WireType = "左导",
                    Array = "第a组",
                    Points =  new string[] { "321", "323", "221", "223"}
                },
                new HangingPointParas()
                {
                    Index = 2,
                    WireType = "右导",
                    Array = "第a组",
                    Points =  new string[] { "320", "322", "220", "222"}
                },
                new HangingPointParas()
                {
                    Index = 3,
                    WireType = "左导",
                    Array = "第b组",
                    Points =  new string[] {"321", "323" }
                },
                new HangingPointParas()
                {
                    Index = 4,
                    WireType = "右导",
                    Array = "第b组",
                    Points =  new string[] { "320", "322" }
                },
            };

            List<VStringParas> vStringList = new List<VStringParas>()
            {
                new VStringParas()
                {
                    Index = "V1",
                    L1 = 9850,
                    H1 = 9026,
                    L2 = 9850,
                    H2 = 9026,
                    StressLimit = 0,
                    Angle = 0
                },
            };

            HangingPointSettingParas ratioParas = new HangingPointSettingParas()
            {
                GCQ = 0.5f,
                GCH = 0.5f,
                GXN = -1,
                GXW = 2,

                DQWQ = 0.7f,
                DQWH = 0.3f,
                DQCQ = 0.7f,
                DQCH = 0.3f,

                DDWQ = 0.7f,
                DDWH = 0.3f,
                DDCQ = 0.7f,
                DDCH = 0.3f,

                DMWQ = 0.7f,
                DMWH = 0.3f,
                DMCQ = 0.7f,
                DMCH = 0.3f,

                NormalXYPoints = normalList,
                NormalZPoints = normalList,
                InstallXYPoints = installList,
                InstallZPoints = installList,
                VStrings = vStringList,
            };

            LoadComposeLineTower loadLineTower = new LoadComposeLineTower(formulaParas, lineParas, ratioParas, template, openTemplateDialog.FileName);

            string filePath = saveFileDialog.FileName.Substring(0, saveFileDialog.FileName.Length - 3) + "cals";

            loadLineTower.CalculateLoadDistribute(filePath);

            string filePath2 = saveFileDialog.FileName.Substring(0, saveFileDialog.FileName.Length - 3) + "div";
            List<StruCalsPointLoad> loadList = loadLineTower.CalsPointsLoad(filePath2);

            string filePath3 = saveFileDialog.FileName.Substring(0, saveFileDialog.FileName.Length - 3) + "load";
            loadLineTower.SumPointsLoad(filePath3, loadList);
        }
    }
}
