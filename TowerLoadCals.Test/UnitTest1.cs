﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using TowerLoadCals.BLL;
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
        public void TestMethod1_LineTower()
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
            formulaParas.RGBad = 1.3f;
            formulaParas.RGGood = 1.05f;
            formulaParas.RQ = 1.5f;
            //formulaParas.RGCheck01 = 1.3f;

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

            //formulaParas.RA = 1.5f;

            StruLineParas[] lineParas = new StruLineParas[] {
                new StruLineParas
                {
                    HoistingCoef = 2.5f,
                    WireExtraLoad = 4,
                    AnchorTension = 61.20f,
                    PulleyTensionDif = 0,
                    DrawingCoef = 1.3f
                },
                new StruLineParas
                {
                    HoistingCoef = 2.5f,
                    WireExtraLoad = 4,
                    AnchorTension = 61.20f,
                    PulleyTensionDif = 0,
                    DrawingCoef = 1.3f
                },
                new StruLineParas
                {
                    HoistingCoef = 2.5f,
                    WireExtraLoad = 8,
                    AnchorTension = 375.23f,
                    PulleyTensionDif = 50,
                    DrawingCoef = 1.3f
                },
                 new StruLineParas
                {
                    HoistingCoef = 2.5f,
                    WireExtraLoad = 8,
                    AnchorTension = 375.23f,
                    PulleyTensionDif = 50,
                    DrawingCoef = 1.3f
                },
                new StruLineParas
                {
                    HoistingCoef = 2.5f,
                    WireExtraLoad = 8,
                    AnchorTension = 375.23f,
                    PulleyTensionDif = 50,
                    DrawingCoef = 1.3f
                },
                new StruLineParas
                {
                    HoistingCoef = 2.5f,
                    WireExtraLoad = 8,
                    AnchorTension = 375.23f,
                    PulleyTensionDif = 50,
                    DrawingCoef = 1.3f
                },
                 new StruLineParas
                {
                    HoistingCoef = 2.5f,
                    WireExtraLoad = 8,
                    AnchorTension = 375.23f,
                    PulleyTensionDif = 50,
                    DrawingCoef = 1.3f
                },
                new StruLineParas
                {
                    HoistingCoef = 2.5f,
                    WireExtraLoad = 8,
                    AnchorTension = 375.23f,
                    PulleyTensionDif = 50,
                    DrawingCoef = 1.3f
                }
            };

            List<HangingPointParas> normalList = new List<HangingPointParas>()
            {
                new HangingPointParas()
                {
                    Index = 1,
                    WireType = "左地",
                    StringType = "常规",
                    Points =  new string[] {"91", "93" }
                },
                new HangingPointParas()
                {
                    Index = 2,
                    WireType = "右地",
                    StringType = "常规",
                    Points =  new string[] {"90", "92" }
                },
                new HangingPointParas()
                {
                    Index = 3,
                    WireType = "左上导",
                    StringType = "I串",
                    Points =  new string[] { "291"}
                },
                new HangingPointParas()
                {
                    Index = 4,
                    WireType = "右上导",
                    StringType = "I串",
                    Points =  new string[] {"290" }
                },
                new HangingPointParas()
                {
                    Index = 5,
                    WireType = "左中导",
                    StringType = "I串",
                    Points =  new string[] {"671", "673" }
                },
                new HangingPointParas()
                {
                    Index = 6,
                    WireType = "右中导",
                    StringType = "I串",
                    Points =  new string[] { "670", "672" }
                },
                new HangingPointParas()
                {
                    Index = 7,
                    WireType = "左下导",
                    StringType = "I串",
                    Points =  new string[] {"1111", "1113" }
                },
                new HangingPointParas()
                {
                    Index = 8,
                    WireType = "右下导",
                    StringType = "I串",
                    Points =  new string[] {"1110", "1112" }
                },
            };

            List<HangingPointParas> installList = new List<HangingPointParas>()
            {
                new HangingPointParas()
                {
                    Index = 1,
                    WireType = "左上导",
                    Array = "第a组",
                    Points =  new string[] { "1", "2"}
                },
                new HangingPointParas()
                {
                    Index = 2,
                    WireType = "右上导",
                    StringType = "第a组",
                    Points =  new string[] {"3", "4"}
                },
                new HangingPointParas()
                {
                    Index = 3,
                    WireType = "左中导",
                    StringType = "第a组",
                    Points =  new string[] {"11", "22" }
                },
                new HangingPointParas()
                {
                    Index = 4,
                    WireType = "右中导",
                    StringType = "第a组",
                    Points =  new string[] { "33", "44" }
                },
                new HangingPointParas()
                {
                    Index = 5,
                    WireType = "左下导",
                    StringType = "第a组",
                    Points =  new string[] {"111", "222" }
                },
                new HangingPointParas()
                {
                    Index = 6,
                    WireType = "右下导",
                    StringType = "第a组",
                    Points =  new string[] {"333", "444" }
                },
            };

            StruRatioParas ratioParas = new StruRatioParas()
            {
                GCQ = 0.5f,
                GCH = 0.5f,
                GXN = -1,
                GXW = 2,

                DQWQ = 0.5f,
                DQWH = 0.5f,
                DQCQ = 0.6f,
                DQCH = 0.4f,

                DDWQ = 0.5f,
                DDWH = 0.5f,
                DDCQ = 0.6f,
                DDCH = 0.4f,

                DMWQ = 0.5f,
                DMWH = 0.5f,
                DMCQ = 0.6f,
                DMCH = 0.4f,

                NormalXYPoints = normalList,
                NormalZPoints = normalList,
                InstallXYPoints = installList,
                InstallZPoints = installList,
            };

            LoadComposeLineTower loadLineTower = new LoadComposeLineTower(formulaParas, lineParas, ratioParas,  template, openTemplateDialog.FileName);

            string filePath = saveFileDialog.FileName.Substring(0, saveFileDialog.FileName.Length - 3) + "cals";

            loadLineTower.CalculateLoadDistribute(out float[,] xx, out float[,] yy, out float[,] zz, filePath);

            string filePath2 = saveFileDialog.FileName.Substring(0, saveFileDialog.FileName.Length - 3) + "div";
            loadLineTower.CalsPointsLoad(filePath2);
        }

        [TestMethod]
        public void TestMethod2()
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

            TowerTemplateReader TemplateReader = new TowerTemplateReader(TowerType.LineCornerTower);

            TowerTemplate template = TemplateReader.Read(saveFileDialog.FileName);

            FormulaParas formulaParas = new FormulaParas();

            formulaParas.Type = TowerType.LineCornerTower;
            formulaParas.LoadRatio = 1;

            //结构重要性系数
            formulaParas.R1Install = 1.0f;
            formulaParas.R0Normal = 1.1f;

            //荷载分项系数
            formulaParas.RGBad = 1.2f;
            formulaParas.RGGood = 1.00f;
            formulaParas.RQ = 1.4f;
            //formulaParas.RGCheck01 = 1.3f;

            //可变荷载组合系数
            formulaParas.VcFNormal = 1.1f;
            formulaParas.VcFInstall = 0.9f;
            formulaParas.VcFBroken = 0.9f;
            formulaParas.VcFUnevenIce = 0.9f;
            formulaParas.VcFCheck = 0.75f;

            //其他参数
            formulaParas.WindAdjustFactor = 1.2f;
            formulaParas.OtherWindAdjustFactor = 1.00f;
            formulaParas.DynamicCoef = 1.1f;

            //formulaParas.RA = 1.5f;

            StruLineParas[] lineParas = new StruLineParas[] {
                new StruLineParas
                {
                    HoistingCoef = 2f,
                    WireExtraLoad = 4f,
                    PulleyTensionDif = 0,
                    AngleMin = 3f,
                    AngleMax = 12f,
                    DrawingCoef = 1.2f
                },
                new StruLineParas
                {
                    HoistingCoef = 2f,
                    WireExtraLoad = 4f,
                    PulleyTensionDif = 0,
                    AngleMin = 3f,
                    AngleMax = 12f,
                    DrawingCoef = 1.2f
                },
                new StruLineParas
                {
                    HoistingCoef = 2f,
                    WireExtraLoad = 8f,
                    PulleyTensionDif = 30,
                    AngleMin = 3f,
                    AngleMax = 12f,
                    DrawingCoef = 1.2f
                },
                 new StruLineParas
                {
                    HoistingCoef = 2f,
                    WireExtraLoad = 8f,
                    PulleyTensionDif = 30,
                    AngleMin = 3f,
                    AngleMax = 12f,
                    DrawingCoef = 1.2f
                }
            };

            StruRatioParas ratioParas = new StruRatioParas();

            LoadComposeLineCornerTower loadLineCornerTower = new LoadComposeLineCornerTower(formulaParas, lineParas, ratioParas, template, openTemplateDialog.FileName);

            string filePath = saveFileDialog.FileName.Substring(0, saveFileDialog.FileName.Length - 3) + "txt";

            loadLineCornerTower.CalculateLoadDistribute(out float[,] xx, out float[,] yy, out float[,] zz, filePath);
        }

        [TestMethod]
        public void TestMethod3()
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

            TowerTemplateReader TemplateReader = new TowerTemplateReader(TowerType.CornerTower);

            TowerTemplate template = TemplateReader.Read(saveFileDialog.FileName);

            FormulaParas formulaParas = new FormulaParas();

            formulaParas.Type = TowerType.CornerTower;
            formulaParas.LoadRatio = 1;
            formulaParas.IsMethod1Selected = false;

            //结构重要性系数
            formulaParas.R1Install = 1.0f;
            formulaParas.R0Normal = 1.0f;

            //荷载分项系数
            formulaParas.RGBad = 1.2f;
            formulaParas.RGGood = 1.00f;
            formulaParas.RQ = 1.4f;
            //formulaParas.RGCheck01 = 1.3f;

            //可变荷载组合系数
            formulaParas.VcFNormal = 1f;
            formulaParas.VcFInstall = 0.9f;
            formulaParas.VcFBroken = 0.9f;
            formulaParas.VcFUnevenIce = 0.9f;
            formulaParas.VcFCheck = 0.75f;

            //其他参数
            formulaParas.WindAdjustFactor = 1.2f;
            formulaParas.OtherWindAdjustFactor = 1.00f;
            formulaParas.DynamicCoef = 1.1f;
            formulaParas.LiftCoefJumper = 2f;
            formulaParas.TempStayWireAngle = 45f;
            formulaParas.TractionAgnle = 20f;

            formulaParas.IsMethod1Selected = false;

            //formulaParas.RA = 1.5f;

            StruLineParas[] lineParas = new StruLineParas[] {
                new StruLineParas
                {
                    TstringNum = 0,
                    WireExtraLoad = 2,
                    TwireExtraLoad = 0,
                    AnchorTension = 37.15f,
                    TemporaryTension = 5f,
                    AngleMin = 0f,
                    AngleMax = 30f,
                    isTurnRight = true,
                    DrawingCoef = 1.2f
                },
                new StruLineParas
                {
                    TstringNum = 0,
                    WireExtraLoad = 2,
                    TwireExtraLoad = 0,
                    AnchorTension = 37.15f,
                    TemporaryTension = 5f,
                    AngleMin = 0f,
                    AngleMax = 30f,
                    isTurnRight = true,
                    DrawingCoef = 1.2f
                },
                new StruLineParas
                {
                    TstringNum = 2,
                    WireExtraLoad = 6,
                    TwireExtraLoad = 2,
                    AnchorTension = 90.41f,
                    TemporaryTension = 20f,
                    AngleMin = 0f,
                    AngleMax = 30f,
                    isTurnRight = true,
                    DrawingCoef = 1.2f
                },
                new StruLineParas
                {
                    TstringNum = 2,
                    WireExtraLoad = 6,
                    TwireExtraLoad = 2,
                    AnchorTension = 90.41f,
                    TemporaryTension = 20f,
                    AngleMin = 0f,
                    AngleMax = 30f,
                    isTurnRight = true,
                    DrawingCoef = 1.2f
                },
                new StruLineParas
                {
                    TstringNum = 2,
                    WireExtraLoad = 6,
                    TwireExtraLoad = 2,
                    AnchorTension = 90.41f,
                    TemporaryTension = 20f,
                    AngleMin = 0f,
                    AngleMax = 30f,
                    isTurnRight = true,
                    DrawingCoef = 1.2f
                },
                new StruLineParas
                {
                    TstringNum = 2,
                    WireExtraLoad = 6,
                    TwireExtraLoad = 2,
                    AnchorTension = 90.41f,
                    TemporaryTension = 20f,
                    AngleMin = 0f,
                    AngleMax = 30f,
                    isTurnRight = true,
                    DrawingCoef = 1.2f
                },
                new StruLineParas
                {
                    TstringNum = 2,
                    WireExtraLoad = 6,
                    TwireExtraLoad = 2,
                    AnchorTension = 90.41f,
                    TemporaryTension = 20f,
                    AngleMin = 0f,
                    AngleMax = 30f,
                    isTurnRight = true,
                    DrawingCoef = 1.2f
                },
                 new StruLineParas
                {
                    TstringNum = 2,
                    WireExtraLoad = 6,
                    TwireExtraLoad = 2,
                    AnchorTension = 90.41f,
                    TemporaryTension = 20f,
                    AngleMin = 0f,
                    AngleMax = 30f,
                    isTurnRight = true,
                    DrawingCoef = 1.2f
                }
            };

            StruRatioParas ratioParas = new StruRatioParas
            {
                BLTQ = 0.5f,
                BLTH = 0.5f,
                BLTZ = 0,
                BLDZTQ = 0.5f,
                BLDZTH = 0.5f,
                BLDZTZ = 0
            };

            LoadComposeCornerTower loadCornerTower = new LoadComposeCornerTower(formulaParas, lineParas, ratioParas, template, openTemplateDialog.FileName);

            string filePath = saveFileDialog.FileName.Substring(0, saveFileDialog.FileName.Length - 3) + "txt";

            loadCornerTower.CalculateLoadDistribute(out float[,] xx, out float[,] yy, out float[,] zz, filePath);
        }

        [TestMethod]
        public void TestMethod4()
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

            TowerTemplateReader TemplateReader = new TowerTemplateReader(TowerType.TerminalTower);

            TowerTemplate template = TemplateReader.Read(saveFileDialog.FileName);

            FormulaParas formulaParas = new FormulaParas();

            formulaParas.Type = TowerType.TerminalTower;
            formulaParas.LoadRatio = 1;
            formulaParas.IsMethod1Selected = false;

            //结构重要性系数
            formulaParas.R1Install = 1.0f;
            formulaParas.R0Normal = 1.0f;

            //荷载分项系数
            formulaParas.RGBad = 1.2f;
            formulaParas.RGGood = 1.00f;
            formulaParas.RQ = 1.4f;
            //formulaParas.RGCheck01 = 1.3f;

            //可变荷载组合系数
            formulaParas.VcFNormal = 1f;
            formulaParas.VcFInstall = 0.9f;
            formulaParas.VcFBroken = 0.9f;
            formulaParas.VcFUnevenIce = 0.9f;
            formulaParas.VcFCheck = 0.75f;

            //其他参数
            formulaParas.WindAdjustFactor = 1.2f;
            formulaParas.OtherWindAdjustFactor = 1.00f;
            formulaParas.DynamicCoef = 1.1f;
            formulaParas.LiftCoefJumper = 2f;
            formulaParas.TempStayWireAngle = 45f;
            formulaParas.TractionAgnle = 20f;

            formulaParas.IsMethod1Selected = false;

            //formulaParas.RA = 1.5f;

            StruLineParas[] lineParas = new StruLineParas[] {
                new StruLineParas
                {
                    TstringNum = 0,
                    WireExtraLoad = 4,
                    TwireExtraLoad = 0,
                    AnchorTension = 156.94f,
                    TemporaryTension = 10,
                    AngleMin = 0f,
                    AngleMax = 30f,
                    isTurnRight = true,
                    PortalTensionMin = 0,
                    PortalTensionMax = 20,
                    DrawingCoef = 1
                },
                new StruLineParas
                {
                    TstringNum = 0,
                    WireExtraLoad = 4,
                    TwireExtraLoad = 0,
                    AnchorTension = 156.94f,
                    TemporaryTension = 10,
                    AngleMin = 0f,
                    AngleMax = 30f,
                    isTurnRight = true,
                    PortalTensionMin = 0,
                    PortalTensionMax = 20,
                    DrawingCoef = 1
                },
                new StruLineParas
                {
                    TstringNum = 2,
                    WireExtraLoad = 12,
                    TwireExtraLoad = 6,
                    AnchorTension = 292.18f,
                    TemporaryTension = 40,
                    AngleMin = 0f,
                    AngleMax = 30f,
                    isTurnRight = true,
                    PortalTensionMin = 0,
                    PortalTensionMax = 60,
                    DrawingCoef = 1
                },
                new StruLineParas
                {
                    TstringNum = 2,
                    WireExtraLoad = 12,
                    TwireExtraLoad = 6,
                    AnchorTension = 292.18f,
                    TemporaryTension = 40,
                    AngleMin = 0f,
                    AngleMax = 30f,
                    isTurnRight = true,
                    PortalTensionMin = 0,
                    PortalTensionMax = 60,
                    DrawingCoef = 1
                }
            };

            StruRatioParas ratioParas = new StruRatioParas
            {
                BLTQ = 0.5f,
                BLTH = 0.5f,
                BLTZ = 0,
                BLDZTQ = 0.5f,
                BLDZTH = 0.5f,
                BLDZTZ = 0
            };

            LoadComposeCornerTower loadCornerTower = new LoadComposeCornerTower(formulaParas, lineParas, ratioParas, template, openTemplateDialog.FileName);

            string filePath = saveFileDialog.FileName.Substring(0, saveFileDialog.FileName.Length - 3) + "txt";

            loadCornerTower.CalculateLoadDistribute(out float[,] xx, out float[,] yy, out float[,] zz, filePath);
        }

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

            TowerTemplateReader TemplateReader = new TowerTemplateReader(TowerType.BranchTower);

            TowerTemplate template = TemplateReader.Read(saveFileDialog.FileName);

            FormulaParas formulaParas = new FormulaParas();

            formulaParas.Type = TowerType.BranchTower;
            formulaParas.LoadRatio = 1;
            formulaParas.IsMethod1Selected = false;

            //结构重要性系数
            formulaParas.R1Install = 1.0f;
            formulaParas.R0Normal = 1.0f;

            //荷载分项系数
            formulaParas.RGBad = 1.2f;
            formulaParas.RGGood = 1.00f;
            formulaParas.RQ = 1.4f;
            //formulaParas.RGCheck01 = 1.3f;

            //可变荷载组合系数
            formulaParas.VcFNormal = 1f;
            formulaParas.VcFInstall = 0.9f;
            formulaParas.VcFBroken = 0.9f;
            formulaParas.VcFUnevenIce = 0.9f;
            formulaParas.VcFCheck = 0.75f;

            //其他参数
            formulaParas.WindAdjustFactor = 1f;
            formulaParas.OtherWindAdjustFactor = 1.00f;
            formulaParas.DynamicCoef = 1.1f;
            formulaParas.LiftCoefJumper = 2f;
            formulaParas.TempStayWireAngle = 45f;
            formulaParas.TractionAgnle = 20f;

            formulaParas.IsMethod1Selected = false;

            //formulaParas.RA = 1.5f;

            StruLineParas[] lineParas = new StruLineParas[] {
                new StruLineParas
                {
                    TstringNum = 0,
                    WireExtraLoad = 4,
                    TwireExtraLoad = 0,
                    AnchorTension = 46.36f,
                    TemporaryTension = 10,
                    AngleFront = 3.92f,
                    AngleBack = -1.82f,
                    isTurnRight = true,
                    DrawingCoef = 1.05f
                },
                new StruLineParas
                {
                    TstringNum = 0,
                    WireExtraLoad = 4,
                    TwireExtraLoad = 0,
                    AnchorTension = 51.81f,
                    TemporaryTension = 10,
                    AngleFront = 3.92f,
                    AngleBack = 9.67f,
                    isTurnRight = true,
                    DrawingCoef = 1.05f
                },
                new StruLineParas
                {
                    TstringNum = 1,
                    WireExtraLoad = 12,
                    TwireExtraLoad = 6,
                    AnchorTension = 328.07f,
                    TemporaryTension = 40,
                    AngleFront = 3.92f,
                    AngleBack = -1.82f,
                    isTurnRight = true,
                    DrawingCoef = 1.1f
                },
                new StruLineParas
                {
                    TstringNum = 1,
                    WireExtraLoad = 12,
                    TwireExtraLoad = 6,
                    AnchorTension = 339.20f,
                    TemporaryTension = 40,
                    AngleFront = 3.92f,
                    AngleBack = 9.67f,
                    isTurnRight = true,
                    DrawingCoef = 1.1f
                },
                new StruLineParas
                {
                    TstringNum = 1,
                    WireExtraLoad = 12,
                    TwireExtraLoad = 6,
                    AnchorTension = 328.07f,
                    TemporaryTension = 40,
                    AngleFront = 3.92f,
                    AngleBack = -1.82f,
                    isTurnRight = true,
                    DrawingCoef = 1.1f
                },
                new StruLineParas
                {
                    TstringNum = 1,
                    WireExtraLoad = 12,
                    TwireExtraLoad = 6,
                    AnchorTension = 339.20f,
                    TemporaryTension = 40,
                    AngleFront = 3.92f,
                    AngleBack = 9.67f,
                    isTurnRight = true,
                    DrawingCoef = 1.1f
                },
                new StruLineParas
                {
                    TstringNum = 1,
                    WireExtraLoad = 12,
                    TwireExtraLoad = 6,
                    AnchorTension = 328.07f,
                    TemporaryTension = 40,
                    AngleFront = 3.92f,
                    AngleBack = -1.82f,
                    isTurnRight = true,
                    DrawingCoef = 1.1f
                },
                new StruLineParas
                {
                    TstringNum = 1,
                    WireExtraLoad = 12,
                    TwireExtraLoad = 6,
                    AnchorTension = 339.20f,
                    TemporaryTension = 40,
                    AngleFront = 3.92f,
                    AngleBack = 9.67f,
                    isTurnRight = true,
                    DrawingCoef = 1.1f
                }
            };

            StruRatioParas ratioParas = new StruRatioParas
            {
                BLTQ = 0.5f,
                BLTH = 0.5f,
                BLTZ = 0,
                BLDZTQ = 0.5f,
                BLDZTH = 0.5f,
                BLDZTZ = 0
            };

            LoadComposeCornerTower loadCornerTower = new LoadComposeCornerTower(formulaParas, lineParas, ratioParas, template, openTemplateDialog.FileName);

            string filePath = saveFileDialog.FileName.Substring(0, saveFileDialog.FileName.Length - 3) + "txt";

            loadCornerTower.CalculateLoadDistribute(out float[,] xx, out float[,] yy, out float[,] zz, filePath);
        }

        [TestMethod]
        public void TestMethod6_DicReader()
        {
            List<StruCalsDicGroup> groups = StruLoadComposeDicReader.Read("D:\\01-代码\\TowerLoadCals\\TowerLoadCals\\UserData\\HPCompose-LineTower.xml");
        }
            
    }



}
