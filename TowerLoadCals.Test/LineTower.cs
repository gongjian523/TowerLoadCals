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

            Mode.TowerTemplate template = TemplateReader.Read(saveFileDialog.FileName);

            StruCalseBaseParas formulaParas = new StruCalseBaseParas();

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
            loadLineTower.GenerateLoadFile(filePath3, loadList);
        }

        /// <summary>
        /// 直线塔 V串 跳跃脱冰
        /// </summary>
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

            TowerTemplateReader TemplateReader = new TowerTemplateReader(TowerType.LineTower);

            Mode.TowerTemplate template = TemplateReader.Read(saveFileDialog.FileName);

            StruCalseBaseParas formulaParas = new StruCalseBaseParas();

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
                    AnchorTension = 27.64f,
                    PulleyTensionDif = 0,
                    DrawingCoef = 1.2f
                },
                new StruLineParas
                {
                    HoistingCoef = 2f,
                    WireExtraLoad = 4,
                    AnchorTension = 27.64f,
                    PulleyTensionDif = 0,
                    DrawingCoef = 1.2f
                },
                new StruLineParas
                {
                    HoistingCoef = 2f,
                    WireExtraLoad = 8,
                    AnchorTension = 133.37f,
                    PulleyTensionDif = 30,
                    DrawingCoef = 1.2f
                },
                 new StruLineParas
                {
                    HoistingCoef = 2f,
                    WireExtraLoad = 8,
                    AnchorTension = 133.37f,
                    PulleyTensionDif = 30,
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
                    Points =  new string[] { "111", "471"}
                },
                new HangingPointParas()
                {
                    Index = 4,
                    WireType = "右导",
                    StringType = "V1",
                    Points =  new string[] {"470", "110" }
                }
            };

            List<HangingPointParas> installList = new List<HangingPointParas>()
            {
                new HangingPointParas()
                {
                    Index = 1,
                    WireType = "左导",
                    Array = "第a组",
                    Points =  new string[] { "181", "183", "321", "323"}
                },
                new HangingPointParas()
                {
                    Index = 2,
                    WireType = "右导",
                    Array = "第a组",
                    Points =  new string[] { "180", "182", "320", "322"}
                },
                new HangingPointParas()
                {
                    Index = 3,
                    WireType = "左导",
                    Array = "第b组",
                    Points =  new string[] {"221", "223" }
                },
                new HangingPointParas()
                {
                    Index = 4,
                    WireType = "右导",
                    Array = "第b组",
                    Points =  new string[] { "220", "222" }
                },
            };

            List<VStringParas> vStringList = new List<VStringParas>()
            {
                new VStringParas()
                {
                    Index = "V1",
                    L1 = 10100,
                    H1 = 13403,
                    L2 = 10100,
                    H2 = 13403,
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
            loadLineTower.GenerateLoadFile(filePath3, loadList);
        }

        /// <summary>
        /// 直线塔 转向挂点
        /// </summary>
        [TestMethod]
        public void TestMethod7()
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
            formulaParas.WindAdjustFactor = 1f;
            formulaParas.OtherWindAdjustFactor = 1f;
            formulaParas.DynamicCoef = 1.1f;
            formulaParas.AnchorWindCoef = 0.7f;
            formulaParas.AnchorGravityCoef = 0.7f;
            formulaParas.AnchorAngle = 20f;

            formulaParas.IsMethod1Selected = true;

            StruLineParas[] lineParas = new StruLineParas[] {
                new StruLineParas
                {
                    HoistingCoef = 2f,
                    WireExtraLoad = 4,
                    AnchorTension = 43.32f,
                    PulleyTensionDif = 0,
                    DrawingCoef = 1.2f
                },
                new StruLineParas
                {
                    HoistingCoef = 2f,
                    WireExtraLoad = 4,
                    AnchorTension = 43.32f,
                    PulleyTensionDif = 0,
                    DrawingCoef = 1.2f
                },
                new StruLineParas
                {
                    HoistingCoef = 1.5f,
                    WireExtraLoad = 8,
                    AnchorTension = 829.39f,
                    PulleyTensionDif = 50,
                    DrawingCoef = 1.2f
                },
                 new StruLineParas
                {
                    HoistingCoef = 1.5f,
                    WireExtraLoad = 8,
                    AnchorTension = 829.39f,
                    PulleyTensionDif = 50,
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
                    Points =  new string[] { "421", "1241"}
                },
                new HangingPointParas()
                {
                    Index = 4,
                    WireType = "右导",
                    StringType = "V1",
                    Points =  new string[] {"1240", "420" }
                }
            };

            List<HangingPointParas> installList = new List<HangingPointParas>()
            {
                new HangingPointParas()
                {
                    Index = 1,
                    WireType = "左导",
                    Array = "第a组",
                    Points =  new string[] { "671", "673", "781", "783"}
                },
                new HangingPointParas()
                {
                    Index = 2,
                    WireType = "右导",
                    Array = "第a组",
                    Points =  new string[] { "670", "672", "780", "782"}
                },
                new HangingPointParas()
                {
                    Index = 3,
                    WireType = "左导",
                    Array = "第b组",
                    Points =  new string[] {"611", "613", "671", "673", "781", "783" , "791", "793"}
                },
                new HangingPointParas()
                {
                    Index = 4,
                    WireType = "右导",
                    Array = "第b组",
                    Points =  new string[] { "610", "612", "670", "672", "780", "782", "790", "792" }
                },
                new HangingPointParas()
                {
                    Index = 5,
                    WireType = "左导",
                    Array = "第c组",
                    Points =  new string[] { "671", "673", "781", "783"}
                },
                new HangingPointParas()
                {
                    Index = 6,
                    WireType = "右导",
                    Array = "第c组",
                    Points =  new string[] { "670", "672", "780", "782"}
                },
            };

            List<HangingPointParas> turningPoingts = new List<HangingPointParas>
            {
                new HangingPointParas
                {
                    Index = 1,
                    WireType = "左导",
                    Angle = 90,
                    Points =  new string[] { "1001", "1003"}
                },
                new HangingPointParas
                {
                    Index = 2,
                    WireType = "右导",
                    Angle = -90,
                    Points =  new string[] { "1000", "1002"}
                },
            };


            List<VStringParas> vStringList = new List<VStringParas>()
            {
                new VStringParas()
                {
                    Index = "V1",
                    L1 = 8350,
                    H1 = 9940,
                    L2 = 8350,
                    H2 = 9940,
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

                IsTuringPointSeleced = true,

                NormalXYPoints = normalList,
                NormalZPoints = normalList,
                InstallXYPoints = installList,
                InstallZPoints = installList,
                TurningPoints = turningPoingts,
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
    }
}
