﻿using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TowerLoadCals.BLL.Electric;
using TowerLoadCals.Common;
using TowerLoadCals.Mode;
using TowerLoadCals.Mode.Electric;

namespace TowerLoadCals.Test
{
    [TestClass]
    public class ElectrialCals
    {
        [TestMethod]
        public void TestMethod1()
        {
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog()
            {
                Filter = "Txt Files (*.txt)|*.txt"
            };

            if (saveFileDialog.ShowDialog() != true)
                return;

            var pro = new ProjectInfo("新建工程", "施工图");
            pro.Volt = 500;
            pro.VoltStr = "500kV";
            pro.ACorDC = 0;

            ElecCalsWire DxData = new ElecCalsWire("JL/G1A-630/45", 1, 674, 33.8f, 2079, 63000, 20.9, 150450, 0, 25,4);
            ElecCalsWire GrdData = new ElecCalsWire( "JLB20A-120", 2, 121.21, 14.25, 810, 147200, 13, 146180, 1, 10);
            ElecCalsWire OPGWData = new ElecCalsWire("OPGW-15-120-1", 3, 120, 15.2, 832, 162000, 13, 147000, 2, 10);

            ElecCalsWeaRes OneWeath15 = new ElecCalsWeaRes("15mm27m/s", 1);
            ElecCalsWeaRes AnoWeath15 = new ElecCalsWeaRes("15mm27m/s", 2);

            List<ElecCalsWorkCondition> wkcList = new List<ElecCalsWorkCondition>()
            {
                new ElecCalsWorkCondition()
                {
                    Name = "最大风速",
                    Temperature = 10,
                    WindSpeed = 27,
                    IceThickness = 0,
                },
                new ElecCalsWorkCondition()
                {
                    Name = "最低气温",
                    Temperature = -20,
                    WindSpeed = 0,
                    IceThickness = 0,
                },
                new ElecCalsWorkCondition()
                {
                    //即Excel中的"电线覆冰"
                    Name = "最大覆冰",
                    Temperature = -5,
                    WindSpeed = 10,
                    IceThickness = 15,
                },
                new ElecCalsWorkCondition()
                {
                    Name = "平均气温",
                    Temperature = 15,
                    WindSpeed = 0,
                    IceThickness = 0,
                },
                new ElecCalsWorkCondition()
                {
                    Name = "最高气温",
                    Temperature = 50,
                    WindSpeed = 0,
                    IceThickness = 0,
                },
                new ElecCalsWorkCondition()
                {
                    Name = "安装情况",
                    Temperature = -10,
                    WindSpeed = 10,
                    IceThickness = 0,
                },
                new ElecCalsWorkCondition()
                {
                    Name = "不均匀风",
                    Temperature = 10,
                    WindSpeed = 27,
                    IceThickness = 0,
                },
                new ElecCalsWorkCondition()
                {
                    Name = "验算冰",
                    Temperature = -5,
                    WindSpeed = 10,
                    IceThickness = 15,
                },
                new ElecCalsWorkCondition()
                {
                    Name = "验算不均匀冰I",
                    Temperature = -5,
                    WindSpeed = 10,
                    IceThickness = 15,
                },
                new ElecCalsWorkCondition()
                {
                    Name = "验算不均匀冰II",
                    Temperature = -5,
                    WindSpeed = 10,
                    IceThickness = 15,
                },
            };
            List<ElecCalsWorkCondition> wkcList2 = new List<ElecCalsWorkCondition>()
            {
                new ElecCalsWorkCondition()
                {
                    Name = "最大风速",
                    Temperature = 10,
                    WindSpeed = 27,
                    IceThickness = 0,
                },
                new ElecCalsWorkCondition()
                {
                    Name = "最低气温",
                    Temperature = -20,
                    WindSpeed = 0,
                    IceThickness = 0,
                },
                new ElecCalsWorkCondition()
                {
                    //即Excel中的"电线覆冰"
                    Name = "最大覆冰",
                    Temperature = -5,
                    WindSpeed = 10,
                    IceThickness = 25,
                },
                new ElecCalsWorkCondition()
                {
                    Name = "平均气温",
                    Temperature = 15,
                    WindSpeed = 0,
                    IceThickness = 0,
                },
                new ElecCalsWorkCondition()
                {
                    Name = "最高气温",
                    Temperature = 50,
                    WindSpeed = 0,
                    IceThickness = 0,
                },
                new ElecCalsWorkCondition()
                {
                    Name = "安装情况",
                    Temperature = -10,
                    WindSpeed = 10,
                    IceThickness = 0,
                },
                new ElecCalsWorkCondition()
                {
                    Name = "不均匀风",
                    Temperature = 10,
                    WindSpeed = 27,
                    IceThickness = 0,
                },
                new ElecCalsWorkCondition()
                {
                    Name = "验算冰",
                    Temperature = -5,
                    WindSpeed = 10,
                    IceThickness = 15,
                },
                new ElecCalsWorkCondition()
                {
                    Name = "验算不均匀冰I",
                    Temperature = -5,
                    WindSpeed = 10,
                    IceThickness = 15,
                },
                new ElecCalsWorkCondition()
                {
                    Name = "验算不均匀冰II",
                    Temperature = -5,
                    WindSpeed = 10,
                    IceThickness = 15,
                },

            };

            OneWeath15.InsertGK(wkcList);
            AnoWeath15.InsertGK(wkcList2);

            ElecCalsCommRes commParas = new ElecCalsCommRes();
            commParas.Volt = pro.Volt;
            commParas.VoltStr = pro.VoltStr;
            commParas.Terrain = "山地";
            commParas.TerrainPara = 0.16;
            commParas.IndAveHei = 20;
            commParas.GrdAveHei = 20;
            commParas.SetForIncrPara(1,1.05, 1,1.1,1.1, 1.1, 1.1, 1.1, 1.1);
            commParas.SetForMaxMinPara(1.025, 1.025, 1.122, 1.08, 0.95, 0.95, 1, 1);
            commParas.SetOverDrive(0,0);
            commParas.CalMethodPara(1,1,1,1,1,1,2,2,1,1);

            SideCalUtils OneWrieSidePara = new SideCalUtils(0.95, 2.85, 25, 1, 4.1, 20, 1, 4.1, 20);
            SideCalUtils AnoWrieSidePara = new SideCalUtils(0.9, 2.85f, 25, 1, 4.1, 20, 1, 4.1, 20);

            TowerStrainElecCals BackTower = new TowerStrainElecCals();
            TowerStrainElecCals CalTower = new TowerStrainElecCals();
            TowerHangElecCals FrontTower = new TowerHangElecCals();

            List<string> logList = new List<string>();

            BackTower.SetPosInf("NAB204", "SJC29152", 48, 1997.1, -4.1, 0, 0, 0);
            BackTower.SetAppreaPara(29.2f, 14.1f, 0, 37.2f, 29.2, 14.1, 0);
            BackTower.UpdataTowerTraHei();

            CalTower.SetPosInf("NAB205", "SJC29152", 48, 2277.1, -6.3, 0, 0, 45.98);
            CalTower.SetFrontBackPosInf(325, 430, 593, 593);
            CalTower.SetAppreaPara(29.2f, 14.1f, 0, 37.2f, 29.2, 14.1, 0);
            CalTower.UpdataTowerTraHei();

            FrontTower.SetPosInf("NAB206", "SZC27154A", 67, 2350.8, -5, 8, 0.5, 45.98);
            FrontTower.SetAppreaPara(30, 14.3f, 0, 34, 30, 14.3, 0);
            FrontTower.UpdataTowerTraHei();

            //打印高差
            logList.AddRange(BackTower.PrintHei("小号塔", false));
            logList.AddRange(FrontTower.PrintHei("大号塔", false));
            logList.AddRange(CalTower.PrintHei("计算塔", true));

            ElecCalsRes BackSideCalRes = new ElecCalsRes();
            ElecCalsRes FrontSideCalRes = new ElecCalsRes();

            BackSideCalRes.IceArea = "中冰区";
            FrontSideCalRes.IceArea = "中冰区";

            BackSideCalRes.UpdataSor(OneWeath15, DxData, GrdData, OPGWData, DxData, OneWrieSidePara, commParas);
            FrontSideCalRes.UpdataSor(AnoWeath15, DxData, GrdData, OPGWData, DxData, AnoWrieSidePara, commParas);

            //更新全局参数的断线覆冰率和不均冰覆冰率
            commParas.UpateIceCovrage(CalTower.TowerType, BackSideCalRes.Weather.WeathComm, BackSideCalRes.IceArea, FrontSideCalRes.Weather.WeathComm, FrontSideCalRes.IceArea);
            logList.Add(commParas.PrintIceCovrage());

            CalTower.GetAndUpdateSideRes(BackSideCalRes, FrontSideCalRes);

            logList.Add("\n小号塔张力：");
            logList.AddRange(CalTower.BackSideRes.PrintTension());

            logList.Add("\n大号塔张力：");
            logList.AddRange(CalTower.FrontSideRes.PrintTension());

            logList.Add("\n小号塔比载和应力：");
            logList.AddRange(CalTower.BackSideRes.PrintBzAndYL());

            logList.Add("\n大号塔比载和应力：");
            logList.AddRange(CalTower.FrontSideRes.PrintBzAndYL());

            FileUtils.TextSaveByLine(saveFileDialog.FileName, logList);

            return;
        }
    }
}
