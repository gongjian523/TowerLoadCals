﻿using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TowerLoadCals.BLL;
using TowerLoadCals.Common;
using TowerLoadCals.DAL;
using TowerLoadCals.Mode;

namespace TowerLoadCals.Test
{
    [TestClass]
    public class XmlSerializerAndDeserializer
    {
        [TestMethod]
        public void TestMethod01_StruCalsParasSerializer()
        {
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog()
            {
                Filter = "XML Files (*.xml)|*.xml",
            };

            if (saveFileDialog.ShowDialog() != true)
                return;

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


            XmlUtils.Serializer(saveFileDialog.FileName, new StruCalsParasCompose(formulaParas, new List<StruLineParas>(lineParas), new List<HangingPointSettingParas> {
                ratioParas,
            }));

            StruCalsParasCompose calsParas = XmlUtils.Deserializer<StruCalsParasCompose>(saveFileDialog.FileName);
        }


        [TestMethod]
        public void TestMethod02_StruCalsLibParasSerializer()
        {
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog()
            {
                Filter = "XML Files (*.xml)|*.xml",
            };

            if (saveFileDialog.ShowDialog() != true)
                return;

            StruCalsLibBaseParas baseParasOverhanging = new StruCalsLibBaseParas
            {
                RGBad = 1.2f,
                RGGood = 1f,
                RQ = 1.4f,
                VcFNormal = 1,
                VcFInstall = 0.9f,
                VcFBroken = 0.9f,
                VcFUnevenIce = 0.9f,
                VcFCheck = 0.75f,
                WindAdjustFactor = 1.2f,
                OtherWindAdjustFactor = 1,
                DynamicCoef = 1.1f,
                DrawingCoef = 1.2f,
                AnchorWindCoef = 0.8f,
                AnchorGravityCoef = 0.8f,
                AnchorAngle = 20,
            };

            StruCalsLibBaseParas baseParasTension = new StruCalsLibBaseParas
            {
                RGBad = 1.2f,
                RGGood = 1f,
                RQ = 1.4f,
                VcFNormal = 1,
                VcFInstall = 0.9f,
                VcFBroken = 0.9f,
                VcFUnevenIce = 0.9f,
                VcFCheck = 0.75f,
                WindAdjustFactor = 1.2f,
                OtherWindAdjustFactor = 1,
                DynamicCoef = 1.1f,
                DrawingCoef = 1.2f,
                LiftCoefJumper = 2f,
                TempStayWireAngle = 45,
                TractionAgnle = 20,
            };

            List<StruCalsLibWireExtraLoadParas> wireExtraLoadParas = new List<StruCalsLibWireExtraLoadParas>
            {
                new StruCalsLibWireExtraLoadParas
                {
                    Index = 1,
                    Voltage = 110,
                    InstallImportanceCoef = 1,
                    OtherImportanceCoef = 1,
                    OverhangingTowerEarthWireExtraLoad = 1,
                    OverhangingTowerWireExtraLoad = 1.5f,
                    TensionTowerEarthWireExtraLoad = 1.5f,
                    TensionTowerWireExtraLoad = 2,
                    TensionTowerJumperWireExtraLoad = 1
                },
                new StruCalsLibWireExtraLoadParas
                {
                    Index = 2,
                    Voltage = 220,
                    InstallImportanceCoef = 1,
                    OtherImportanceCoef = 1,
                    OverhangingTowerEarthWireExtraLoad = 2,
                    OverhangingTowerWireExtraLoad = 3.5f,
                    TensionTowerEarthWireExtraLoad = 2,
                    TensionTowerWireExtraLoad = 4.5f,
                    TensionTowerJumperWireExtraLoad = 2
                },
                new StruCalsLibWireExtraLoadParas
                {
                    Index = 3,
                    Voltage = 330,
                    InstallImportanceCoef = 1,
                    OtherImportanceCoef = 1,
                    OverhangingTowerEarthWireExtraLoad = 2,
                    OverhangingTowerWireExtraLoad = 3.5f,
                    TensionTowerEarthWireExtraLoad = 2,
                    TensionTowerWireExtraLoad = 4.5f,
                    TensionTowerJumperWireExtraLoad = 2
                },
                new StruCalsLibWireExtraLoadParas
                {
                    Index = 4,
                    Voltage = 500,
                    InstallImportanceCoef = 1,
                    OtherImportanceCoef = 1,
                    OverhangingTowerEarthWireExtraLoad = 2,
                    OverhangingTowerWireExtraLoad = 4,
                    TensionTowerEarthWireExtraLoad = 2,
                    TensionTowerWireExtraLoad = 6,
                    TensionTowerJumperWireExtraLoad = 3
                },
                new StruCalsLibWireExtraLoadParas
                {
                    Index = 5,
                    Voltage = 750,
                    InstallImportanceCoef = 1,
                    OtherImportanceCoef = 1,
                    OverhangingTowerEarthWireExtraLoad = 2,
                    OverhangingTowerWireExtraLoad = 4,
                    TensionTowerEarthWireExtraLoad = 2,
                    TensionTowerWireExtraLoad = 6,
                    TensionTowerJumperWireExtraLoad = 3
                },
                new StruCalsLibWireExtraLoadParas
                {
                    Index = 6,
                    Voltage = 800,
                    InstallImportanceCoef = 1,
                    OtherImportanceCoef = 1.1f,
                    OverhangingTowerEarthWireExtraLoad = 4,
                    OverhangingTowerWireExtraLoad = 8,
                    TensionTowerEarthWireExtraLoad = 4,
                    TensionTowerWireExtraLoad = 12,
                    TensionTowerJumperWireExtraLoad = 6
                },
                new StruCalsLibWireExtraLoadParas
                {
                    Index = 7,
                    Voltage = 1000,
                    InstallImportanceCoef = 1,
                    OtherImportanceCoef = 1.1f,
                    OverhangingTowerEarthWireExtraLoad = 4,
                    OverhangingTowerWireExtraLoad = 8,
                    TensionTowerEarthWireExtraLoad = 4,
                    TensionTowerWireExtraLoad = 12,
                    TensionTowerJumperWireExtraLoad = 6
                },
                new StruCalsLibWireExtraLoadParas
                {
                    Index = 8,
                    Voltage = 1100,
                    InstallImportanceCoef = 1,
                    OtherImportanceCoef = 1.1f,
                    OverhangingTowerEarthWireExtraLoad = 4,
                    OverhangingTowerWireExtraLoad = 8,
                    TensionTowerEarthWireExtraLoad = 4,
                    TensionTowerWireExtraLoad = 12,
                    TensionTowerJumperWireExtraLoad = 6
                },
            };

            List<StruCalsLibIceCoverParas> iceCoverParas = new List<StruCalsLibIceCoverParas>
            {
                new StruCalsLibIceCoverParas
                {
                    Index = 1,
                    IceThickness = 5,
                    TowerWindLoadAmplifyCoef = 1.1f,
                    TowerGravityLoadAmplifyCoef = 1,
                },
                new StruCalsLibIceCoverParas
                {
                    Index = 2,
                    IceThickness = 10,
                    TowerWindLoadAmplifyCoef = 1.2f,
                    TowerGravityLoadAmplifyCoef = 1,
                },
                new StruCalsLibIceCoverParas
                {
                    Index = 3,
                    IceThickness = 15,
                    TowerWindLoadAmplifyCoef = 1.6f,
                    TowerGravityLoadAmplifyCoef = 1.2f,
                },
                new StruCalsLibIceCoverParas
                {
                    Index = 4,
                    IceThickness = 20,
                    TowerWindLoadAmplifyCoef = 1.8f,
                    TowerGravityLoadAmplifyCoef = 1.5f,
                },
                new StruCalsLibIceCoverParas
                {
                    Index = 5,
                    IceThickness = 25,
                    TowerWindLoadAmplifyCoef = 2f,
                    TowerGravityLoadAmplifyCoef = 1.8f,
                },
                new StruCalsLibIceCoverParas
                {
                    Index = 6,
                    IceThickness = 30,
                    TowerWindLoadAmplifyCoef = 2.2f,
                    TowerGravityLoadAmplifyCoef = 2f,
                },
                new StruCalsLibIceCoverParas
                {
                    Index = 7,
                    IceThickness = 40,
                    TowerWindLoadAmplifyCoef = 2.3f,
                    TowerGravityLoadAmplifyCoef = 2.2f,
                },
                new StruCalsLibIceCoverParas
                {
                    Index = 8,
                    IceThickness = 45,
                    TowerWindLoadAmplifyCoef = 2.3f,
                    TowerGravityLoadAmplifyCoef = 2.2f,
                },
            };

            var paras = new StruCalsLib {
                OverhangingTowerBaseParas = baseParasOverhanging,
                TensionTowerBaseParas = baseParasTension,
                WireExtraLoadParas = wireExtraLoadParas,
                IceCoverParas = iceCoverParas,
            };

            XmlUtils.Serializer(saveFileDialog.FileName, paras);

            var paras2 = XmlUtils.Deserializer<StruCalsLib>(saveFileDialog.FileName);
        }
    }
}