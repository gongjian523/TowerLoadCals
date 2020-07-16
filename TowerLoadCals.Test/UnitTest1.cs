using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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

            Mode.TowerTemplate template = TemplateReader.Read(saveFileDialog.FileName);

            StruCalseBaseParas formulaParas = new StruCalseBaseParas();

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
                    Array = "第a组",
                    Points =  new string[] {"3", "4"}
                },
                new HangingPointParas()
                {
                    Index = 3,
                    WireType = "左中导",
                    Array = "第a组",
                    Points =  new string[] {"11", "22" }
                },
                new HangingPointParas()
                {
                    Index = 4,
                    WireType = "右中导",
                    Array = "第a组",
                    Points =  new string[] { "33", "44" }
                },
                new HangingPointParas()
                {
                    Index = 5,
                    WireType = "左下导",
                    Array = "第a组",
                    Points =  new string[] {"111", "222" }
                },
                new HangingPointParas()
                {
                    Index = 6,
                    WireType = "右下导",
                    Array = "第a组",
                    Points =  new string[] {"333", "444" }
                },
            };

            HangingPointSettingParas ratioParas = new HangingPointSettingParas()
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

            loadLineTower.CalculateLoadDistribute(filePath);

            string filePath2 = saveFileDialog.FileName.Substring(0, saveFileDialog.FileName.Length - 3) + "div";
            List<StruCalsPointLoad>  loadList = loadLineTower.CalsPointsLoad(filePath2);

            string filePath3 = saveFileDialog.FileName.Substring(0, saveFileDialog.FileName.Length - 3) + "load";
            loadLineTower.GenerateLoadFile(filePath3, loadList);
        }

        [TestMethod]
        public void TestMethod1_LineTower_VString()
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

            Mode.TowerTemplate template = TemplateReader.Read(saveFileDialog.FileName);

            StruCalseBaseParas formulaParas = new StruCalseBaseParas();

            formulaParas.Type = TowerType.LineTower;
            formulaParas.LoadRatio = 1;

            //结构重要性系数
            formulaParas.R1Install = 1f;
            formulaParas.R0Normal = 1f;

            //荷载分项系数
            formulaParas.RGBad = 1.2f;
            formulaParas.RGGood = 1f;
            formulaParas.RQ = 1.4f;

            //可变荷载组合系数
            formulaParas.VcFNormal = 1f;
            formulaParas.VcFInstall = 0.90f;
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
                    WireExtraLoad = 2,
                    AnchorTension = 21.88f,
                    PulleyTensionDif = 0,
                    DrawingCoef = 1.2f
                },
                new StruLineParas
                {
                    HoistingCoef = 2f,
                    WireExtraLoad = 2,
                    AnchorTension = 21.88f,
                    PulleyTensionDif = 0,
                    DrawingCoef = 1.2f
                },
                new StruLineParas
                {
                    HoistingCoef = 2f,
                    WireExtraLoad = 4,
                    AnchorTension = 183.22f,
                    PulleyTensionDif = 25,
                    DrawingCoef = 1.2f
                },
                 new StruLineParas
                {
                    HoistingCoef = 2f,
                    WireExtraLoad = 4,
                    AnchorTension = 183.22f,
                    PulleyTensionDif = 25,
                    DrawingCoef = 1.2f
                },
                new StruLineParas
                {
                    HoistingCoef = 2f,
                    WireExtraLoad = 4,
                    AnchorTension = 183.22f,
                    PulleyTensionDif = 25,
                    DrawingCoef = 1.2f
                }
            };

            List<VStringParas> vStringList = new List<VStringParas>()
            {
                new VStringParas()
                {
                    Index = "V1",
                    L1 = 3350,
                    H1 = 3445,
                    L2 = 3350,
                    H2 = 3445,
                    StressLimit = 0,
                    Angle = 0
                },
                new VStringParas()
                {
                    Index = "V2",
                    L1 = 3350,
                    H1 = 3445,
                    L2 = 3350,
                    H2 = 3445,
                    StressLimit = 0,
                    Angle = 0
                },
                new VStringParas()
                {
                    Index = "V3",
                    L1 = 6576,
                    H1 = 2329,
                    L2 = 6576,
                    H2 = 2329,
                    StressLimit = 0,
                    Angle = 0
                },
            };

            List<HangingPointParas> normalList = new List<HangingPointParas>()
            {
                new HangingPointParas()
                {
                    Index = 1,
                    WireType = "左地",
                    StringType = "悬臂",
                    Points =  new string[] {"81", "71" }
                },
                new HangingPointParas()
                {
                    Index = 2,
                    WireType = "右地",
                    StringType = "悬臂",
                    Points =  new string[] {"80", "70" }
                },
                new HangingPointParas()
                {
                    Index = 3,
                    WireType = "左导",
                    StringType = "V1",
                    Points =  new string[] { "401", "10"}
                },
                new HangingPointParas()
                {
                    Index = 4,
                    WireType = "中导",
                    StringType = "V2",
                    Points =  new string[] {"701", "700" }
                },
                new HangingPointParas()
                {
                    Index = 5,
                    WireType = "右导",
                    StringType = "V3",
                    Points =  new string[] {"10", "400" }
                }
            };

            List<HangingPointParas> installXYList = new List<HangingPointParas>()
            {
                new HangingPointParas()
                {
                    Index = 1,
                    WireType = "左导",
                    Array = "第a组",
                    Points =  new string[] { "200", "202"}
                },
                new HangingPointParas()
                {
                    Index = 2,
                    WireType = "中导",
                    Array = "第a组",
                    Points =  new string[] {"100", "102"}
                },
                new HangingPointParas()
                {
                    Index = 3,
                    WireType = "右导",
                    Array = "第a组",
                    Points =  new string[] {"300", "302" }
                }
            };

            List<HangingPointParas> installZList = new List<HangingPointParas>()
            {
                new HangingPointParas()
                {
                    Index = 1,
                    WireType = "左导",
                    Array = "第a组",
                    Points =  new string[] { "251"}
                },
                new HangingPointParas()
                {
                    Index = 2,
                    WireType = "中导",
                    Array = "第a组",
                    Points =  new string[] {"10"}
                },
                new HangingPointParas()
                {
                    Index = 3,
                    WireType = "右导",
                    Array = "第a组",
                    Points =  new string[] {"250" }
                }
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

                DMWQ = 0.5f,
                DMWH = 0.5f,
                DMCQ = 0.5f,
                DMCH = 0.5f,

                NormalXYPoints = normalList,
                NormalZPoints = normalList,
                InstallXYPoints = installXYList,
                InstallZPoints = installZList,
                VStrings = vStringList,
            };

            LoadComposeLineTower loadLineTower = new LoadComposeLineTower(formulaParas, lineParas, ratioParas, template, openTemplateDialog.FileName);

            string filePath = saveFileDialog.FileName.Substring(0, saveFileDialog.FileName.Length - 3) + "cals";

            loadLineTower.CalculateLoadDistribute(filePath);

            string filePath2 = saveFileDialog.FileName.Substring(0, saveFileDialog.FileName.Length - 3) + "div";
            List<StruCalsPointLoad> loadList = loadLineTower.CalsPointsLoad(filePath2);

            string filePath3 = saveFileDialog.FileName.Substring(0, saveFileDialog.FileName.Length - 3) + "load";
            loadLineTower.GenerateLoadFile(filePath3, loadList);
        }

        [TestMethod]
        public void TestMethod2_LineCornerTower()
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

            Mode.TowerTemplate template = TemplateReader.Read(saveFileDialog.FileName);

            StruCalseBaseParas formulaParas = new StruCalseBaseParas();

            formulaParas.Type = TowerType.LineCornerTower;
            formulaParas.LoadRatio = 1;

            //结构重要性系数
            formulaParas.R1Install = 1.0f;
            formulaParas.R0Normal = 1.1f;

            //荷载分项系数
            formulaParas.RGBad = 1.2f;
            formulaParas.RGGood = 1.00f;
            formulaParas.RQ = 1.4f;

            //可变荷载组合系数
            formulaParas.VcFNormal = 1.0f;
            formulaParas.VcFInstall = 0.9f;
            formulaParas.VcFBroken = 0.9f;
            formulaParas.VcFUnevenIce = 0.9f;
            formulaParas.VcFCheck = 0.75f;

            //其他参数
            formulaParas.WindAdjustFactor = 1.2f;
            formulaParas.OtherWindAdjustFactor = 1.00f;
            formulaParas.DynamicCoef = 1.1f;

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
                    Points =  new string[] { "101", "103", "471", "471"}
                },
                new HangingPointParas()
                {
                    Index = 4,
                    WireType = "右导",
                    StringType = "V2",
                    Points =  new string[] {"470", "470", "100", "102" }
                }
            };

            List<HangingPointParas> installList = new List<HangingPointParas>()
            {
                new HangingPointParas()
                {
                    Index = 1,
                    WireType = "左导",
                    Array = "第a组",
                    Points =  new string[] { "321", "323"}
                },
                new HangingPointParas()
                {
                    Index = 2,
                    WireType = "右导",
                    Array = "第a组",
                    Points =  new string[] {"320", "322"}
                },
                new HangingPointParas()
                {
                    Index = 3,
                    WireType = "左导",
                    Array = "第b组",
                    Points =  new string[] {"181", "183" , "321", "323" }
                },
                new HangingPointParas()
                {
                    Index = 4,
                    WireType = "右导",
                    Array = "第b组",
                    Points =  new string[] { "350", "352", "320", "322" }
                },
                new HangingPointParas()
                {
                    Index = 5,
                    WireType = "左导",
                    Array = "第c组",
                    Points =  new string[] {"321", "323", "371", "373"}
                },
                new HangingPointParas()
                {
                    Index = 6,
                    WireType = "右导",
                    Array = "第c组",
                    Points =  new string[] {"320", "322", "220", "222"}
                },
            };

            List<VStringParas> vStringList = new List<VStringParas>()
            {
                new VStringParas()
                {
                    Index = "V1",
                    L1 = 12202,
                    H1 = 6217,
                    L2 = 9748,
                    H2 = 11617,
                    StressLimit = 0,
                    Angle = 103
                },
                new VStringParas()
                {
                    Index = "V2",
                    L1 = 12202,
                    H1 = 6217,
                    L2 = 9748,
                    H2 = 11617,
                    StressLimit = 0,
                    Angle = 103
                }
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

                NormalXYPoints = normalList,
                NormalZPoints = normalList,
                InstallXYPoints = installList,
                InstallZPoints = installList,
                VStrings = vStringList
            };

            LoadComposeLineCornerTower loadLineCornerTower = new LoadComposeLineCornerTower(formulaParas, lineParas, ratioParas, template, openTemplateDialog.FileName);

            string filePath = saveFileDialog.FileName.Substring(0, saveFileDialog.FileName.Length - 3) + "cals";

            loadLineCornerTower.CalculateLoadDistribute(filePath);

            string filePath2 = saveFileDialog.FileName.Substring(0, saveFileDialog.FileName.Length - 3) + "div";
            List<StruCalsPointLoad> loadList = loadLineCornerTower.CalsPointsLoad(filePath2);

            string filePath3 = saveFileDialog.FileName.Substring(0, saveFileDialog.FileName.Length - 3) + "load";
            loadLineCornerTower.GenerateLoadFile(filePath3, loadList);
        }

        [TestMethod]
        public void TestMethod3_CornerTower()
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
                    WireType = "左上导",
                    StringType = "常规",
                    Points =  new string[] { "321", "323"}
                },
                new HangingPointParas()
                {
                    Index = 4,
                    WireType = "右上导",
                    StringType = "常规",
                    Points =  new string[] {"320", "322"}
                },
                new HangingPointParas()
                {
                    Index = 5,
                    WireType = "左中导",
                    StringType = "常规",
                    Points =  new string[] { "601", "603"}
                },
                new HangingPointParas()
                {
                    Index = 6,
                    WireType = "右中导",
                    StringType = "常规",
                    Points =  new string[] {"600", "602"}
                },
                new HangingPointParas()
                {
                    Index = 7,
                    WireType = "左下导",
                    StringType = "常规",
                    Points =  new string[] { "921", "923"}
                },
                new HangingPointParas()
                {
                    Index = 8,
                    WireType = "右下导",
                    StringType = "常规",
                    Points =  new string[] {"920", "922"}
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
                    WireType = "左上导",
                    StringType = "I串",
                    Points =  new string[] { "301", "303"}
                },
                new HangingPointParas()
                {
                    Index = 4,
                    WireType = "右上导",
                    StringType = "I串",
                    Points =  new string[] {"300", "302"}
                },
                new HangingPointParas()
                {
                    Index = 5,
                    WireType = "左中导",
                    StringType = "I串",
                    Points =  new string[] { "601", "603"}
                },
                new HangingPointParas()
                {
                    Index = 6,
                    WireType = "右中导",
                    StringType = "I串",
                    Points =  new string[] {"600", "602"}
                },
                new HangingPointParas()
                {
                    Index = 7,
                    WireType = "左下导",
                    StringType = "I串",
                    Points =  new string[] { "901", "903"}
                },
                new HangingPointParas()
                {
                    Index = 8,
                    WireType = "右下导",
                    StringType = "I串",
                    Points =  new string[] {"900", "902"}
                },
            };

            List<HangingPointParas> intallTXYList = new List<HangingPointParas>()
            {
                new HangingPointParas()
                {
                    Index = 1,
                    WireType = "左上导",
                    StringType = "I串",
                    Array = "第a组",
                    Points =  new string[] { "301", "303"}
                },
                new HangingPointParas()
                {
                    Index = 2,
                    WireType = "右上导",
                    StringType = "I串",
                    Array = "第a组",
                    Points =  new string[] {"300", "302"}
                },
                new HangingPointParas()
                {
                    Index = 3,
                    WireType = "左中导",
                    StringType = "I串",
                    Array = "第a组",
                    Points =  new string[] { "601", "603"}
                },
                new HangingPointParas()
                {
                    Index = 4,
                    WireType = "右中导",
                    StringType = "I串",
                    Array = "第a组",
                    Points =  new string[] {"600", "602"}
                },
                new HangingPointParas()
                {
                    Index = 5,
                    WireType = "左下导",
                    StringType = "I串",
                    Array = "第a组",
                    Points =  new string[] { "901", "903"}
                },
                new HangingPointParas()
                {
                    Index = 6,
                    WireType = "右下导",
                    StringType = "I串",
                    Array = "第a组",
                    Points =  new string[] {"900", "902"}
                },
            };

            List<HangingPointParas> intallTZList = new List<HangingPointParas>()
            {
                new HangingPointParas()
                {
                    Index = 1,
                    WireType = "左上导",
                    StringType = "I串",
                    Array = "第a组",
                    Points =  new string[] { "311" }
                },
                new HangingPointParas()
                {
                    Index = 2,
                    WireType = "右上导",
                    StringType = "I串",
                    Array = "第a组",
                    Points =  new string[] { "310" }
                },
                new HangingPointParas()
                {
                    Index = 3,
                    WireType = "左中导",
                    StringType = "I串",
                    Array = "第a组",
                    Points =  new string[] { "611" }
                },
                new HangingPointParas()
                {
                    Index = 4,
                    WireType = "右中导",
                    StringType = "I串",
                    Array = "第a组",
                    Points =  new string[] { "610" }
                },
                new HangingPointParas()
                {
                    Index = 5,
                    WireType = "左下导",
                    StringType = "I串",
                    Array = "第a组",
                    Points =  new string[] { "911" }
                },
                new HangingPointParas()
                {
                    Index = 6,
                    WireType = "右下导",
                    StringType = "I串",
                    Array = "第a组",
                    Points =  new string[] { "910" }
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

                NormalXYPoints = normalList,
                NormalZPoints = normalTList,
                InstallXYPoints = intallTXYList,
                InstallZPoints = intallTZList,
            };

            LoadComposeTensionTower loadCornerTower = new LoadComposeTensionTower(formulaParas, lineParas, ratioParas, template, openTemplateDialog.FileName);

            string filePath = saveFileDialog.FileName.Substring(0, saveFileDialog.FileName.Length - 3) + "calc";
            loadCornerTower.CalculateLoadDistribute(filePath);

            string filePath2 = saveFileDialog.FileName.Substring(0, saveFileDialog.FileName.Length - 3) + "div";
            List<StruCalsPointLoad> loadList = loadCornerTower.CalsPointsLoad(filePath2);

            string filePath3 = saveFileDialog.FileName.Substring(0, saveFileDialog.FileName.Length - 3) + "load";
            loadCornerTower.GenerateLoadFile(filePath3, loadList);
        }

        [TestMethod]
        public void TestMethod3_CornerTower_VString()
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

            formulaParas.IsMethod1Selected = true;

            StruLineParas[] lineParas = new StruLineParas[] {
                new StruLineParas
                {
                    TstringNum = 0,
                    WireExtraLoad = 4,
                    TwireExtraLoad = 0,
                    AnchorTension = 78.47f,
                    TemporaryTension = 5f,
                    AngleMin = 0f,
                    AngleMax = 20f,
                    isTurnRight = true,
                    DrawingCoef = 1f
                },
                new StruLineParas
                {
                    TstringNum = 0,
                    WireExtraLoad = 4,
                    TwireExtraLoad = 0,
                    AnchorTension = 78.47f,
                    TemporaryTension = 5f,
                    AngleMin = 0f,
                    AngleMax = 20f,
                    isTurnRight = true,
                    DrawingCoef = 1f
                },
                new StruLineParas
                {
                    TstringNum = 2,
                    WireExtraLoad = 12,
                    TwireExtraLoad = 6,
                    AnchorTension = 292.18f,
                    TemporaryTension = 40f,
                    AngleMin = 0f,
                    AngleMax = 20f,
                    isTurnRight = true,
                    DrawingCoef = 1f
                },
                new StruLineParas
                {
                    TstringNum = 2,
                    WireExtraLoad = 12,
                    TwireExtraLoad = 6,
                    AnchorTension = 292.18f,
                    TemporaryTension = 40f,
                    AngleMin = 0f,
                    AngleMax = 20f,
                    isTurnRight = true,
                    DrawingCoef = 1f
                },
            };

            List<VStringParas> vStringList = new List<VStringParas>()
            {
                new VStringParas()
                {
                    Index = "V1",
                    L1 = 9000,
                    H1 = 8000,
                    L2 = 7000,
                    H2 = 8000,
                    StressLimit = 0,
                    Angle = 90
                },
            };

            List<HangingPointParas> normalList = new List<HangingPointParas>()
            {
                new HangingPointParas()
                {
                    Index = 1,
                    WireType = "左地",
                    StringType = "常规",
                    Points =  new string[] {"11", "13"}
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
                    StringType = "常规",
                    Points =  new string[] { "641", "643", "681", "683"}
                },
                new HangingPointParas()
                {
                    Index = 4,
                    WireType = "右导",
                    StringType = "常规",
                    Points =  new string[] {"640", "642", "680", "682" }
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
                    StringType = "V1",
                    Points =  new string[] { "1001", "1003"}
                },
                new HangingPointParas()
                {
                    Index = 4,
                    WireType = "右导",
                    StringType = "V1",
                    Points =  new string[] {"1000", "1002"}
                },
            };

            List<HangingPointParas> intallTList = new List<HangingPointParas>()
            {
                new HangingPointParas()
                {
                    Index = 1,
                    WireType = "左导",
                    StringType = "V1",
                    Array = "第a组",
                    Points =  new string[] { "1001", "1003"}
                },
                new HangingPointParas()
                {
                    Index = 2,
                    WireType = "右导",
                    StringType = "V1",
                    Array = "第a组",
                    Points =  new string[] {"1000", "1002"}
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

                NormalXYPoints = normalList,
                NormalZPoints = normalTList,
                InstallXYPoints = intallTList,
                InstallZPoints = intallTList,
                VStrings = vStringList,
            };

            LoadComposeTensionTower loadCornerTower = new LoadComposeTensionTower(formulaParas, lineParas, ratioParas, template, openTemplateDialog.FileName);

            string filePath = saveFileDialog.FileName.Substring(0, saveFileDialog.FileName.Length - 3) + "calc";
            loadCornerTower.CalculateLoadDistribute(filePath);

            string filePath2 = saveFileDialog.FileName.Substring(0, saveFileDialog.FileName.Length - 3) + "div";
            List<StruCalsPointLoad> loadList = loadCornerTower.CalsPointsLoad(filePath2);

            string filePath3 = saveFileDialog.FileName.Substring(0, saveFileDialog.FileName.Length - 3) + "load";
            loadCornerTower.GenerateLoadFile(filePath3, loadList);
        }

        [TestMethod]
        public void TestMethod3_CornerTower_1()
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
            formulaParas.R0Normal = 1.0f;

            //荷载分项系数
            formulaParas.RGBad = 1.2f;
            formulaParas.RGGood = 1.00f;
            formulaParas.RQ = 1.4f;

            //可变荷载组合系数
            formulaParas.VcFNormal = 1f;
            formulaParas.VcFInstall = 0.9f;
            formulaParas.VcFBroken = 0.9f;
            formulaParas.VcFUnevenIce = 0.9f;
            formulaParas.VcFCheck = 0.75f;

            //其他参数
            formulaParas.WindAdjustFactor = 1.0f;
            formulaParas.OtherWindAdjustFactor = 1.0f;
            formulaParas.DynamicCoef = 1.1f;
            formulaParas.LiftCoefJumper = 2f;
            formulaParas.TempStayWireAngle = 45f;
            formulaParas.TractionAgnle = 20f;

            formulaParas.IsMethod1Selected = false;

            StruLineParas[] lineParas = new StruLineParas[] {
                new StruLineParas
                {
                    TstringNum = 0,
                    WireExtraLoad = 2,
                    TwireExtraLoad = 0,
                    AnchorTension = 18.61f,
                    TemporaryTension = 5f,
                    AngleMin = 0f,
                    AngleMax = 20f,
                    isTurnRight = true,
                    DrawingCoef = 1.2f
                },
                new StruLineParas
                {
                    TstringNum = 0,
                    WireExtraLoad = 2,
                    TwireExtraLoad = 0,
                    AnchorTension = 18.61f,
                    TemporaryTension = 5f,
                    AngleMin = 0f,
                    AngleMax = 20f,
                    isTurnRight = true,
                    DrawingCoef = 1.2f
                },
                new StruLineParas
                {
                    TstringNum = 1,
                    WireExtraLoad = 6,
                    TwireExtraLoad = 2,
                    AnchorTension = 76.50f,
                    TemporaryTension = 30f,
                    AngleMin = 0f,
                    AngleMax = 20f,
                    isTurnRight = true,
                    DrawingCoef = 1.2f
                },
                new StruLineParas
                {
                    TstringNum = 2,
                    WireExtraLoad = 6,
                    TwireExtraLoad = 2,
                    AnchorTension = 76.50f,
                    TemporaryTension = 30f,
                    AngleMin = 0f,
                    AngleMax = 20f,
                    isTurnRight = true,
                    DrawingCoef = 1.2f
                },
                new StruLineParas
                {
                    TstringNum = 1,
                    WireExtraLoad = 6,
                    TwireExtraLoad = 2,
                    AnchorTension = 76.50f,
                    TemporaryTension = 30f,
                    AngleMin = 0f,
                    AngleMax = 20f,
                    isTurnRight = true,
                    DrawingCoef = 1.2f
                },
            };

            List<VStringParas> vStringList = new List<VStringParas>();


            List<HangingPointParas> normalList = new List<HangingPointParas>()
            {
                new HangingPointParas()
                {
                    Index = 1,
                    WireType = "左地",
                    StringType = "常规",
                    Points =  new string[] {"71", "73"}
                },
                new HangingPointParas()
                {
                    Index = 2,
                    WireType = "右地",
                    StringType = "常规",
                    Points =  new string[] {"70", "72" }
                },
                new HangingPointParas()
                {
                    Index = 3,
                    WireType = "左导",
                    StringType = "常规",
                    Points =  new string[] { "481", "483", "491", "493"}
                },
                new HangingPointParas()
                {
                    Index = 4,
                    WireType = "中导",
                    StringType = "常规",
                    Points =  new string[] {"301", "303", "331", "333" }
                },
                new HangingPointParas()
                {
                    Index = 5,
                    WireType = "右导",
                    StringType = "常规",
                    Points =  new string[] {"480", "482", "490", "492" }
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
                    Points =  new string[] { "471"}
                },
                new HangingPointParas()
                {
                    Index = 4,
                    WireType = "中导",
                    StringType = "I串",
                    Points =  new string[] {"171", "173"}
                },
                new HangingPointParas()
                {
                    Index = 5,
                    WireType = "右导",
                    StringType = "I串",
                    Points =  new string[] {"600"}
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
                    Points =  new string[] { "471"}
                },
                new HangingPointParas()
                {
                    Index = 2,
                    WireType = "中导",
                    StringType = "I串",
                    Array = "第a组",
                    Points =  new string[] {"171", "173"}
                },
                new HangingPointParas()
                {
                    Index = 3,
                    WireType = "右导",
                    StringType = "I串",
                    Array = "第a组",
                    Points =  new string[] {"600"}
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

                NormalXYPoints = normalList,
                NormalZPoints = normalTList,
                InstallXYPoints = intallTList,
                InstallZPoints = intallTList,
                VStrings = vStringList,
            };

            LoadComposeTensionTower loadCornerTower = new LoadComposeTensionTower(formulaParas, lineParas, ratioParas, template, openTemplateDialog.FileName);

            string filePath = saveFileDialog.FileName.Substring(0, saveFileDialog.FileName.Length - 3) + "calc";
            loadCornerTower.CalculateLoadDistribute(filePath);

            string filePath2 = saveFileDialog.FileName.Substring(0, saveFileDialog.FileName.Length - 3) + "div";
            List<StruCalsPointLoad> loadList = loadCornerTower.CalsPointsLoad(filePath2);

            string filePath3 = saveFileDialog.FileName.Substring(0, saveFileDialog.FileName.Length - 3) + "load";
            loadCornerTower.GenerateLoadFile(filePath3, loadList);
        }

        [TestMethod]
        public void TestMethod4_TerminalTower()
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

            Mode.TowerTemplate template = TemplateReader.Read(saveFileDialog.FileName);

            StruCalseBaseParas formulaParas = new StruCalseBaseParas();

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
                    StringType = "常规",
                    Points =  new string[] { "640", "642", "680", "682"}
                },
                new HangingPointParas()
                {
                    Index = 4,
                    WireType = "右导",
                    StringType = "常规",
                    Points =  new string[] {"641", "643", "681", "683"}
                }
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
                    Points =  new string[] { "1001", "1003"}
                },
                new HangingPointParas()
                {
                    Index = 4,
                    WireType = "右导",
                    StringType = "I串",
                    Points =  new string[] {"1000", "1002"}
                }
            };

            List<HangingPointParas> intallTList = new List<HangingPointParas>()
            {
                new HangingPointParas()
                {
                    Index = 1,
                    WireType = "左导",
                    StringType = "I串",
                    Array = "第a组",
                    Points =  new string[] { "1001", "1003"}
                },
                new HangingPointParas()
                {
                    Index = 2,
                    WireType = "右导",
                    StringType = "I串",
                    Array = "第a组",
                    Points =  new string[] {"1000", "1002"}
                }
            };


            HangingPointSettingParas ratioParas = new HangingPointSettingParas
            {
                BLTQ = 0.5f,
                BLTH = 0.5f,
                BLTZ = 0,
                BLDZTQ = 0.5f,
                BLDZTH = 0.5f,
                BLDZTZ = 0,

                NormalXYPoints = normalList,
                NormalZPoints = normalTList,
                InstallXYPoints = intallTList,
                InstallZPoints = intallTList,
            };

            LoadComposeTensionTower loadCornerTower = new LoadComposeTensionTower(formulaParas, lineParas, ratioParas, template, openTemplateDialog.FileName);

            string filePath = saveFileDialog.FileName.Substring(0, saveFileDialog.FileName.Length - 3) + "calc";
            loadCornerTower.CalculateLoadDistribute(filePath);

            string filePath2 = saveFileDialog.FileName.Substring(0, saveFileDialog.FileName.Length - 3) + "div";
            List<StruCalsPointLoad> loadList = loadCornerTower.CalsPointsLoad(filePath2);

            string filePath3 = saveFileDialog.FileName.Substring(0, saveFileDialog.FileName.Length - 3) + "load";
            loadCornerTower.GenerateLoadFile(filePath3, loadList);
        }

        [TestMethod]
        public void TestMethod5_BranchTower()
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

            Mode.TowerTemplate template = TemplateReader.Read(saveFileDialog.FileName);

            StruCalseBaseParas formulaParas = new StruCalseBaseParas();

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
                    WireType = "左上导",
                    StringType = "常规",
                    Points =  new string[] { "361", "363", "431", "433"}
                },
                new HangingPointParas()
                {
                    Index = 4,
                    WireType = "右上导",
                    StringType = "常规",
                    Points =  new string[] {"360", "362", "430", "432"}
                },
                new HangingPointParas()
                {
                    Index = 5,
                    WireType = "左中导",
                    StringType = "常规",
                    Points =  new string[] { "761", "763", "831", "833"}
                },
                new HangingPointParas()
                {
                    Index = 6,
                    WireType = "右中导",
                    StringType = "常规",
                    Points =  new string[] {"760", "762", "830", "832"}
                },
                new HangingPointParas()
                {
                    Index = 7,
                    WireType = "左下导",
                    StringType = "常规",
                    Points =  new string[] { "1261", "1263", "1331", "1333"}
                },
                new HangingPointParas()
                {
                    Index = 8,
                    WireType = "右下导",
                    StringType = "常规",
                    Points =  new string[] {"1260", "1262", "1330", "1332"}
                }
            };

            List<HangingPointParas> normalTList = new List<HangingPointParas>()
            {
                new HangingPointParas()
                {
                    Index = 1,
                    WireType = "左地",
                    StringType = "无跳线",
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
                    WireType = "左上导",
                    StringType = "I串",
                    Points =  new string[] { "211", "213" }
                },
                new HangingPointParas()
                {
                    Index = 4,
                    WireType = "右上导",
                    StringType = "I串",
                    Points =  new string[] {"210", "212" }
                },
                new HangingPointParas()
                {
                    Index = 5,
                    WireType = "左中导",
                    StringType = "I串",
                    Points =  new string[] { "811", "813" }
                },
                new HangingPointParas()
                {
                    Index = 6,
                    WireType = "右中导",
                    StringType = "I串",
                    Points =  new string[] {"810", "812" }
                },
                new HangingPointParas()
                {
                    Index = 7,
                    WireType = "左下导",
                    StringType = "I串",
                    Points =  new string[] { "1311", "1313"}
                },
                new HangingPointParas()
                {
                    Index = 8,
                    WireType = "右下导",
                    StringType = "I串",
                    Points =  new string[] {"1310", "1312"}
                }
            };

            List<HangingPointParas> installTList = new List<HangingPointParas>()
            {
                new HangingPointParas()
                {
                    Index = 1,
                    WireType = "左上导",
                    Array = "第a组",
                    StringType = "I串",
                    Points =  new string[] { "211", "213" }
                },
                new HangingPointParas()
                {
                    Index = 2,
                    WireType = "右上导",
                    Array = "第a组",
                    StringType = "I串",
                    Points =  new string[] {"210", "212" }
                },
                new HangingPointParas()
                {
                    Index = 3,
                    WireType = "左中导",
                    Array = "第a组",
                    StringType = "I串",
                    Points =  new string[] { "811", "813" }
                },
                new HangingPointParas()
                {
                    Index = 4,
                    WireType = "右中导",
                    Array = "第a组",
                    StringType = "I串",
                    Points =  new string[] {"810", "812" }
                },
                new HangingPointParas()
                {
                    Index = 5,
                    WireType = "左下导",
                    Array = "第a组",
                    StringType = "I串",
                    Points =  new string[] { "1311", "1312"}
                },
                new HangingPointParas()
                {
                    Index = 6,
                    WireType = "右下导",
                    Array = "第a组",
                    StringType = "I串",
                    Points =  new string[] {"1310", "1312"}
                }
            };


            HangingPointSettingParas ratioParas = new HangingPointSettingParas
            {
                BLTQ = 0.5f,
                BLTH = 0.5f,
                BLTZ = 0,
                BLDZTQ = 0.5f,
                BLDZTH = 0.5f,
                BLDZTZ = 0,

                NormalXYPoints = normalList,
                NormalZPoints = normalTList,
                InstallXYPoints = installTList,
                InstallZPoints = installTList,
            };

            LoadComposeTensionTower loadCornerTower = new LoadComposeTensionTower(formulaParas, lineParas, ratioParas, template, openTemplateDialog.FileName);

            string filePath = saveFileDialog.FileName.Substring(0, saveFileDialog.FileName.Length - 3) + "calc";
            loadCornerTower.CalculateLoadDistribute(filePath);

            string filePath2 = saveFileDialog.FileName.Substring(0, saveFileDialog.FileName.Length - 3) + "div";
            List<StruCalsPointLoad> loadList = loadCornerTower.CalsPointsLoad(filePath2);

            string filePath3 = saveFileDialog.FileName.Substring(0, saveFileDialog.FileName.Length - 3) + "load";
            loadCornerTower.GenerateLoadFile(filePath3, loadList);
        }

        [TestMethod]
        public void TestMethod6_DicReader()
        {
            List<StruCalsDicGroup> groups = StruLoadComposeDicReader.Read("D:\\01-代码\\TowerLoadCals\\TowerLoadCals\\UserData\\HPCompose-LineTower.xml");
        }

        [TestMethod]
        public void TestMethod6_TensionTowerDicReader()
        {
            List<StruCalsDicGroup> groups = StruLoadComposeDicReader.Read("D:\\01-代码\\TowerLoadCals\\TowerLoadCals\\UserData\\HPCompose-TensionTower.xml");
        }

        [TestMethod]
        public void TestMethod7_VString()
        {
            VStringCompose vStringCompose = new VStringCompose(12202, 9748, 6271, 11617, 0, 341.24f, 0.00f, 252.48f);

            Console.WriteLine("左侧 " + vStringCompose.VCX1.ToString("0.00").PadLeft(10) + vStringCompose.VCY1.ToString("0.00").PadLeft(10) + vStringCompose.VCZ1.ToString("0.00").PadLeft(10));
            Console.WriteLine("右侧 " + vStringCompose.VCX2.ToString("0.00").PadLeft(10) + vStringCompose.VCY2.ToString("0.00").PadLeft(10) + vStringCompose.VCZ2.ToString("0.00").PadLeft(10));
        }

        [TestMethod]
        public void TestMethod8_DicReader()
        {
            List<LoadDic> groups = StruLoadComposeDicReader.DicRead("D:\\01-代码\\TowerLoad\\TowerLoadCals\\UserData\\HPDic-LineTower.xml");
        }

        [TestMethod]
        public void TestMethod8_TensionTowerDicReader()
        {
            List<LoadDic> groups = StruLoadComposeDicReader.DicRead("D:\\01-代码\\TowerLoad\\TowerLoadCals\\UserData\\HPDic-TensionTower.xml");
        }


        [TestMethod]
        public void TestMethod10_CreatProjectConfigFile()
        {
            ProjectUtils pro = ProjectUtils.GetInstance();

            pro.CreateProject();

            pro.InsertStrucTowerNames(new List<string>(){ "tower1", "tower2", "tower3", "tower5"});
            pro.InsertStrucTowerName("tower4");

            pro.DeleteStrucTowerName("tower5");
            pro.DeleteStrucTowerNames(new List<string>() { "tower1", "tower3" });

        }

        [TestMethod]
        public void TestMethod11_ArrayInit()
        {
            float[,] arr = new float[5, 3];

        }


    }

}
