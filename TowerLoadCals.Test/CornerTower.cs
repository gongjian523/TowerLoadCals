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
    public class CornerTower
    {
        [TestMethod]
        public void TestMethod6()
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

            Mode.TowerTemplate template = TemplateReader.Read(saveFileDialog.FileName);

            StruCalseBaseParas formulaParas = new StruCalseBaseParas();

            formulaParas.Type = TowerType.CornerTower;
            formulaParas.LoadRatio = 1;
            formulaParas.IsMethod1Selected = false;

            //结构重要性系数
            formulaParas.R1Install = 1.0f;
            formulaParas.R0Normal = 1.1f;

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
                    AnchorTension = 47.49f,
                    TemporaryTension = 10f,
                    AngleMin = 20f,
                    AngleMax = 40f,
                    isTurnRight = true,
                    DrawingCoef = 1.05f,
                    PulleyTensionDif = 0,
                },
                new StruLineParas
                {
                    TstringNum = 0,
                    WireExtraLoad = 4,
                    TwireExtraLoad = 0,
                    AnchorTension = 47.49f,
                    TemporaryTension = 10f,
                    AngleMin = 20f,
                    AngleMax = 40f,
                    isTurnRight = true,
                    DrawingCoef = 1.05f,
                    PulleyTensionDif = 0,
                },
                new StruLineParas
                {
                    TstringNum = 1,
                    WireExtraLoad = 12,
                    TwireExtraLoad = 6,
                    AnchorTension = 412.14f,
                    TemporaryTension = 40f,
                    AngleMin = 20f,
                    AngleMax = 40f,
                    isTurnRight = true,
                    DrawingCoef = 1.1f,
                    PulleyTensionDif = 50,
                },
                new StruLineParas
                {
                    TstringNum = 1,
                    WireExtraLoad = 12,
                    TwireExtraLoad = 6,
                    AnchorTension = 412.14f,
                    TemporaryTension = 40f,
                    AngleMin = 20f,
                    AngleMax = 40f,
                    isTurnRight = true,
                    DrawingCoef = 1.1f,
                    PulleyTensionDif = 50,
                },
                new StruLineParas
                {
                    TstringNum = 1,
                    WireExtraLoad = 12,
                    TwireExtraLoad = 6,
                    AnchorTension = 412.14f,
                    TemporaryTension = 40f,
                    AngleMin = 20f,
                    AngleMax = 40f,
                    isTurnRight = true,
                    DrawingCoef = 1.1f,
                    PulleyTensionDif = 50,
                },
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
                    Points =  new string[] {"40", "42" }
                },
                new HangingPointParas()
                {
                    Index = 3,
                    WireType = "左导",
                    StringType = "常规",
                    Points =  new string[] { "1511", "1513", "1601", "1603"}
                },
                new HangingPointParas()
                {
                    Index = 4,
                    WireType = "中导",
                    StringType = "常规",
                    Points =  new string[] {"1060", "1062"}
                },
                new HangingPointParas()
                {
                    Index = 5,
                    WireType = "右导",
                    StringType = "常规",
                    Points =  new string[] { "1600", "1602","1510", "1512"}
                },
            };

            List<HangingPointParas> normalTList = new List<HangingPointParas>()
            {
                new HangingPointParas()
                {
                    Index = 1,
                    WireType = "左地",
                    StringType = "无跳线"
                },
                new HangingPointParas()
                {
                    Index = 2,
                    WireType = "右地",
                    StringType = "无跳线",
                },
                new HangingPointParas()
                {
                    Index = 3,
                    WireType = "左导",
                    StringType = "I串",
                    Points =  new string[] { "1481", "1483"}
                },
                new HangingPointParas()
                {
                    Index = 4,
                    WireType = "中导",
                    StringType = "I串",
                    Points =  new string[] {"490", "492"}
                },
                new HangingPointParas()
                {
                    Index = 5,
                    WireType = "右导",
                    StringType = "I串",
                    Points =  new string[] { "1480", "1482"}
                },
            };

            List<HangingPointParas> intallTList = new List<HangingPointParas>()
            {
                new HangingPointParas()
                {
                    Index = 1,
                    WireType = "左导",
                    StringType = "I串",
                    Array = "第a组",
                    Points =  new string[] { "1481", "1483"}
                },
                new HangingPointParas()
                {
                    Index = 2,
                    WireType = "中导",
                    StringType = "I串",
                    Array = "第a组",
                    Points =  new string[] {"490", "492"}
                },
                new HangingPointParas()
                {
                    Index = 3,
                    WireType = "右导",
                    StringType = "I串",
                    Array = "第a组",
                    Points =  new string[] { "1480", "1482"}
                },
            };

            List<HangingPointParas> turingList = new List<HangingPointParas>()
            {
                new HangingPointParas()
                {
                    Index = 1,
                    WireType = "左导",
                    StringType = "I串",
                    Points =  new string[] { "1511", "1513", "1601", "1603" }
                },
                new HangingPointParas()
                {
                    Index = 2,
                    WireType = "中导",
                    StringType = "I串",
                    Points =  new string[] { "1040", "1042", "1050", "1052" }
                },
                new HangingPointParas()
                {
                    Index = 3,
                    WireType = "右导",
                    StringType = "I串",
                    Points =  new string[] { "1600", "1602", "1510", "1512" }
                },
            };

            HangingPointSettingParas ratioParas = new HangingPointSettingParas
            {
                BLTQ = 0.5f,
                BLTH = 0.5f,
                BLTZ = 0,
                BLDZTQ = 0.5f,
                BLDZTH = 0.5f,
                BLDZTZ = 0,

                IsTuringPointSeleced = true,

                NormalXYPoints = normalList,
                NormalZPoints = normalTList,
                InstallXYPoints = intallTList,
                InstallZPoints = intallTList,
                TurningPoints= turingList,
            };

            LoadComposeTensionTower loadCornerTower = new LoadComposeTensionTower(formulaParas, lineParas, ratioParas, template, openTemplateDialog.FileName);

            string filePath = saveFileDialog.FileName.Substring(0, saveFileDialog.FileName.Length - 3) + "calc";
            loadCornerTower.CalculateLoadDistribute(filePath);

            string filePath2 = saveFileDialog.FileName.Substring(0, saveFileDialog.FileName.Length - 3) + "div";
            List<StruCalsPointLoad> loadList = loadCornerTower.CalsPointsLoad(filePath2);

            string filePath3 = saveFileDialog.FileName.Substring(0, saveFileDialog.FileName.Length - 3) + "load";
            loadCornerTower.GenerateLoadFile(filePath3, loadList);
        }
    }
}
