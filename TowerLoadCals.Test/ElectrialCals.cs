using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TowerLoadCals.BLL.Electric;
using TowerLoadCals.Common;
using TowerLoadCals.Mode.Electric;

namespace TowerLoadCals.Test
{
    [TestClass]
    public class ElectrialCals
    {
        [TestMethod]
        public void TestMethod1_TowerStrain()
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

            ElecCalsWire DxData = new ElecCalsWire("JL/G1A-630/45", 1, 674, 33.8, 2079, 63000,20.9, 150450, 0, 25,4);
            ElecCalsWire GrdData = new ElecCalsWire( "JLB20A-120", 2, 121.21, 14.25, 810, 147200, 13, 146180, 1, 10);
            ElecCalsWire OPGWData = new ElecCalsWire("OPGW-15-120-1", 3, 120, 15.2, 832, 162000,13, 147000, 2, 10);

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

            ElecCalsCommRes CommParas = new ElecCalsCommRes();
            CommParas.Volt = pro.Volt;
            CommParas.VoltStr = pro.VoltStr;
            CommParas.Terrain = "山地";
            CommParas.TerrainPara = 0.16;
            CommParas.IndAveHei = 20;
            CommParas.GrdAveHei = 20;
            CommParas.GrdCl = 12;
            CommParas.SetForIncrPara(1,1.05, 1,1.1,1.1, 1.1, 1.1, 1.1, 1.1);
            CommParas.SetForMaxMinPara(1.025, 1.025, 1.122, 1.08, 0.95, 0.95, 1, 1);
            CommParas.SetOverDrive(0,0);
            CommParas.CalMethodParaStrain(1,1,1,1,1,1,2,2,1,1);

            ElecCalsSideRes OneWrieSidePara = new ElecCalsSideRes(0.95, 2.85, 25, 1, 4.1, 20, 1, 4.1, 20);
            OneWrieSidePara.IceArea = "中冰区";
            ElecCalsSideRes AnoWrieSidePara = new ElecCalsSideRes(0.95, 2.8, 25, 1, 4.1, 20, 1, 4.1, 20);
            AnoWrieSidePara.IceArea = "中冰区";

            ElecCalsTowerAppre BackAppre = new ElecCalsTowerAppre();
            BackAppre.SetTraPara(29.2, 14.1, 0, 37.2, 29.2, 14.1, 0);
            ElecCalsTowerAppre CalsAppre = new ElecCalsTowerAppre();
            CalsAppre.SetTraPara(29.2, 14.1, 0, 37.2, 29.2, 14.1, 0);
            ElecCalsTowerAppre FrontAppre = new ElecCalsTowerAppre();
            FrontAppre.SetTraPara(30, 14.3, 0, 34, 30, 14.3, 0);

            ElecCalsSpanFit BackSpanFit = new ElecCalsSpanFit(0, 4, 10, 0, 2, 7, 0, 12, 15);
            ElecCalsSpanFit FrontSpanFit = new ElecCalsSpanFit(0, 0, 10, 0, 2, 7, 0, 7, 15);

            TowerStrainElecCals BackTower = new TowerStrainElecCals();
            TowerStrainElecCals CalTower = new TowerStrainElecCals();
            TowerHangElecCals FrontTower = new TowerHangElecCals();

            List<string> LogList = new List<string>();

            BackTower.SetPosInf("NAB204", "SJC29152", 48, 1997.1, -4.1, 0, 0, 0);
            BackTower.TowerAppre = BackAppre;
            BackTower.UpdataTowerTraHei();
            BackTower.UpdateAtitudeTower();

            CalTower.SetPosInf("NAB205", "SJC29152", 48, 2277.1, -6.3, 0, 0, 45.98);
            CalTower.SetFrontBackPosInf(325, 430, 593, 593);
            CalTower.TowerAppre = CalsAppre;
            CalTower.UpdataTowerTraHei();
            CalTower.UpdateAtitudeTower();

            FrontTower.SetPosInf("NAB206", "SZC27154A", 67, 2350.8, -5, 8, 0.5, 45.98);
            FrontTower.TowerAppre = FrontAppre;
            FrontTower.UpdataTowerTraHei();
            FrontTower.UpdateAtitudeTower();

            //打印高差
            LogList.AddRange(BackTower.PrintHei("小号塔", false));
            LogList.AddRange(FrontTower.PrintHei("大号塔", false));
            LogList.AddRange(CalTower.PrintHei("计算塔", true));

            ElecCalsRes BackSideCalRes = new ElecCalsRes();
            ElecCalsRes FrontSideCalRes = new ElecCalsRes();

            BackSideCalRes.IsBackSide = true;
            BackSideCalRes.SpanFit = BackSpanFit;

            FrontSideCalRes.IsBackSide = false;
            FrontSideCalRes.SpanFit = FrontSpanFit;

            BackSideCalRes.UpdataSor(OneWeath15, DxData, GrdData, OPGWData, DxData, OneWrieSidePara, CommParas);
            FrontSideCalRes.UpdataSor(AnoWeath15, DxData, GrdData, OPGWData, DxData, AnoWrieSidePara, CommParas);

            //更新全局参数的断线覆冰率和不均冰覆冰率
            CommParas.UpateIceCovrage(CalTower.TowerType, BackSideCalRes.Weather.WeathComm, BackSideCalRes.SideParas.IceArea, FrontSideCalRes.Weather.WeathComm, FrontSideCalRes.SideParas.IceArea);
            LogList.Add(CommParas.PrintIceCovrageStrain());

            CalTower.GetAndUpdateSideRes(BackSideCalRes, FrontSideCalRes);

            LogList.Add("\n小号塔张力：");
            LogList.AddRange(CalTower.BackSideRes.PrintTension(CalTower.TowerType));

            LogList.Add("\n大号塔张力：");
            LogList.AddRange(CalTower.FrontSideRes.PrintTension(CalTower.TowerType));

            LogList.Add("\n小号塔比载和应力：");
            LogList.AddRange(CalTower.BackSideRes.PrintBzAndYL(CalTower.TowerType));

            LogList.Add("\n大号塔比载和应力：");
            LogList.AddRange(CalTower.FrontSideRes.PrintBzAndYL(CalTower.TowerType));

            //绝缘子串参数
            ElecCalsStrData IndStr = new ElecCalsStrData();
            IndStr.SetIGPara(1500, 2, 29, 205, 15, 2.5, 0);
            ElecCalsStrData GrdStr = new ElecCalsStrData();
            GrdStr.SetIGPara(40, 2, 2, 146, 4, 2.5, 0);
            ElecCalsStrData JumpStr = new ElecCalsStrData();
            //JumpStr.SetJumPara(400, 1, 38, 8, 30, 4, 0, 20, 10);
            JumpStr.SetJumPara(400, 1, 38, 8, 30, 4, 0);

            CalTower.GetAndUpdateStrData(IndStr, GrdStr, JumpStr);

            for(int i = 0; i <= 9; i++)
            {
                CalTower.PhaseTraList[i].CalsStrLoad();
            }

            //导线有2侧，每侧有3相，因为三相的数据一样，故每侧打印一相
            LogList.Add("\n小号侧绝缘子串风荷载和垂直荷载：");
            LogList.Add("导线");
            LogList.AddRange(CalTower.PhaseTraList[0].PrintStrLoad(CalTower.TowerType));
            LogList.Add("地线");
            LogList.AddRange(CalTower.PhaseTraList[3].PrintStrLoad(CalTower.TowerType));

            LogList.Add("\n大号侧绝缘子串风荷载和垂直荷载：");
            LogList.Add("导线");
            LogList.AddRange(CalTower.PhaseTraList[5].PrintStrLoad(CalTower.TowerType));
            LogList.Add("地线");
            LogList.AddRange(CalTower.PhaseTraList[8].PrintStrLoad(CalTower.TowerType));

            for (int i = 0; i <= 2; i++)
            {
                CalTower.PhaseTraList[i].CalsWindLoad(CommParas.VoltStr, CalTower.PhaseTraList[i+5].WireData.WeatherParas.WeathComm);
                CalTower.PhaseTraList[i+5].CalsWindLoad(CommParas.VoltStr, CalTower.PhaseTraList[i].WireData.WeatherParas.WeathComm);
            }

            LogList.Add("\n跳线绝缘子串风荷载：");
            LogList.AddRange(CalTower.PrintJumpStrLoad());

            CalTower.FlashBackHeiSub(BackTower);
            CalTower.FlashFrontHeiSub(FrontTower);
            CalTower.UpdateWindPara();
            LogList.Add("\n高差和风压系数：");
            LogList.AddRange(CalTower.PrintWindPara());

            CalTower.Cals();
            LogList.AddRange(CalTower.PrintCalsReslt());

            CalTower.UpdateTensionDiff();
            CalTower.UpateAnchor();
            CalTower.CalsTensionChcek();
            LogList.AddRange(CalTower.PrintTensionDiff());
            LogList.AddRange(CalTower.PrintBreakUnabCheck());
            LogList.AddRange(CalTower.PrintAnchor());

            CalTower.CalDFCure();
            LogList.AddRange(CalTower.PrintDFCure());

            CalTower.CalDFCurePY();
            LogList.AddRange(CalTower.PrintDFCurePY());

            FileUtils.TextSaveByLine(saveFileDialog.FileName, LogList);
            return;
        }


        [TestMethod]
        public void TestMethod2_TowerHang()
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

            ElecCalsWire DxData = new ElecCalsWire("JL/G2A-720/50-45/7", 1, 775.41, 36.23, 2397.7, 63000, 20.9, 170600, 0, 25, 4);
            ElecCalsWire GrdData = new ElecCalsWire("JLB20A-150", 2, 148.07, 15.75, 989.4, 147200, 13, 178570, 1, 10);
            ElecCalsWire OPGWData = new ElecCalsWire("JLB20A-150", 3, 148.07, 15.75, 989.4, 147200, 13, 178570, 2, 10);

            ElecCalsWeaRes Weath15 = new ElecCalsWeaRes("15mm27m/s", 1);
            List<ElecCalsWorkCondition> wkcList = new List<ElecCalsWorkCondition>()
            {
                new ElecCalsWorkCondition()
                {
                    Name = "最大风速",
                    Temperature = 0,
                    WindSpeed = 31,
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
                    IceThickness = 20,
                },
                new ElecCalsWorkCondition()
                {
                    Name = "平均气温",
                    Temperature = 10,
                    WindSpeed = 0,
                    IceThickness = 0,
                },
                new ElecCalsWorkCondition()
                {
                    Name = "最高气温",
                    Temperature = 40,
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
                    Temperature = 0,
                    WindSpeed = 31,
                    IceThickness = 0,
                },
                new ElecCalsWorkCondition()
                {
                    Name = "验算冰",
                    Temperature = -5,
                    WindSpeed = 10,
                    IceThickness = 10,
                },
                new ElecCalsWorkCondition()
                {
                    Name = "验算不均匀冰I",
                    Temperature = -5,
                    WindSpeed = 10,
                    IceThickness = 10,
                },
                new ElecCalsWorkCondition()
                {
                    Name = "验算不均匀冰II",
                    Temperature = -5,
                    WindSpeed = 10,
                    IceThickness = 10,
                },
            };

            Weath15.InsertGK(wkcList);

            ElecCalsCommRes CommParas = new ElecCalsCommRes();
            CommParas.Volt = pro.Volt;
            CommParas.VoltStr = pro.VoltStr;
            CommParas.Terrain = "山地";
            CommParas.TerType = 'B';
            CommParas.TerrainPara = 0.16;
            CommParas.IndAveHei = 20;
            CommParas.GrdAveHei = 20;
            CommParas.GrdCl = 12;
            CommParas.SetForIncrPara();
            CommParas.SetForMaxMinPara(1.025, 1.025, 1.15/1.025, 1.1/1.025, 0.95, 0.95, 1, 1);

            var iceThick = wkcList.Find(item => item.Name == "最大覆冰").IceThickness;
            CommParas.CalMethodParaHang(1, 1, 1, iceThick, 1, 2);

            ElecCalsSideRes OneWrieSidePara = new ElecCalsSideRes(0.95, 2.5, 25, 1, 4, 25, 1, 4, 25);
            OneWrieSidePara.IceArea = "重冰区";

            ElecCalsTowerAppre BackAppre = new ElecCalsTowerAppre();
            BackAppre.SetTraPara(27.5, 13.3, 0, 35.3);
            ElecCalsTowerAppre CalsAppre = new ElecCalsTowerAppre();
            CalsAppre.SetTraPara(26.1, 12.4, 0, 28.8);
            ElecCalsTowerAppre FrontAppre = new ElecCalsTowerAppre();
            FrontAppre.SetTraPara(27.5, 13.3, 0, 35.3);

            ElecCalsSpanFit SpanFit = new ElecCalsSpanFit(0, 5, 10, 0, 4, 5, 0, 18, 10);

            TowerHangElecCals BackTower = new TowerHangElecCals();
            TowerHangElecCals CalTower = new TowerHangElecCals();
            TowerHangElecCals FrontTower = new TowerHangElecCals();

            BackTower.SetPosInf("N6098", "SJ252D", 29, 1254.09997558594, -3, 0, 0, 0);
            BackTower.TowerAppre = BackAppre;
            BackTower.UpdataTowerTraHei();
            BackTower.UpdateAtitudeTower();

            CalTower.SetPosInf("N6099", "SZ253", 48, 1273, -4.7, 6, 0.3, 0);
            CalTower.SetFrontBackPosInf(374, 428, 466, 428);
            CalTower.TowerAppre = CalsAppre;
            CalTower.UpdataTowerTraHei();
            CalTower.UpdateAtitudeTower();

            FrontTower.SetPosInf("N6100", "SJ252D", 33, 1309.69995117187, -5, 0, 0, 0);
            FrontTower.TowerAppre = FrontAppre;
            FrontTower.UpdataTowerTraHei();
            FrontTower.UpdateAtitudeTower();

            List<string> LogList = new List<string>();

            LogList.AddRange(BackTower.PrintHei("小号塔", false));
            LogList.AddRange(FrontTower.PrintHei("大号塔", false));
            LogList.AddRange(CalTower.PrintHei("计算塔", false));

            ElecCalsRes SideCalRes = new ElecCalsRes();
            SideCalRes.SpanFit = SpanFit;
            SideCalRes.IsBackSide = true; //只是为了计算45风
            SideCalRes.UpdataSor(Weath15, DxData, GrdData, OPGWData, DxData, OneWrieSidePara, CommParas);

            CommParas.UpateIceCovrage(SideCalRes.Weather.WeathComm, SideCalRes.SideParas.IceArea);
            LogList.Add(CommParas.PrintIceCovrageHang());

            CalTower.GetAndUpdateSideRes(SideCalRes);

            LogList.Add("\n张力：");
            LogList.AddRange(CalTower.SideRes.PrintTension(CalTower.TowerType));

            LogList.Add("\n比载和应力：");
            LogList.AddRange(CalTower.SideRes.PrintBzAndYL(CalTower.TowerType));

            //绝缘子串参数
            ElecCalsStrData IndStr = new ElecCalsStrData();
            IndStr.SetIGPara(701.58, 1, 28, 0, 8, 0, 0);
            ElecCalsStrData GrdStr = new ElecCalsStrData();
            GrdStr.SetIGPara(29.08, 2, 1, 0, 4, 0, 0);
    
            CalTower.GetAndUpdateStrData(IndStr, GrdStr);

            for (int i = 0; i <= 9; i++)
            {
                CalTower.PhaseTraList[i].CalsStrLoad();
            }

            //导线有2侧，每侧有3相，因为三相的数据一样，故每侧打印一相
            LogList.Add("\n侧绝缘子串风荷载和垂直荷载：");
            LogList.Add("导线");
            LogList.AddRange(CalTower.PhaseTraList[0].PrintStrLoad(CalTower.TowerType));
            LogList.Add("地线");
            LogList.AddRange(CalTower.PhaseTraList[3].PrintStrLoad(CalTower.TowerType));

            CalTower.FlashBackHeiSub(BackTower);
            CalTower.FlashFrontHeiSub(FrontTower);
            CalTower.UpdateDFCure();
            LogList.AddRange(CalTower.PrintDFCure());

            CalTower.Cals();
            LogList.AddRange(CalTower.PrintCalsReslt());

            FileUtils.TextSaveByLine(saveFileDialog.FileName, LogList);
        }
    }
}
