using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TowerLoadCals.BLL.Electric;
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
            var pro = new ProjectInfo("新建工程", "施工图");
            pro.Volt = 500;
            pro.ACorDC = 0;

            WireUtils DxData = new WireUtils("JL/G1A-630/45", 1, 674, 33.8f, 2079, 63000, 20.9f, 150450, 0, 25,4);
            WireUtils GrdData = new WireUtils( "JLB20A-120", 2, 121.21f, 14.25f, 810, 147200, 13, 146180, 1, 10);
            WireUtils OPGWData = new WireUtils("OPGW-15-120-1", 3, 120, 15.2f, 832, 162000, 13, 147000, 2, 10);

            WeatherUtils OneWeath15 = new WeatherUtils("15mm27m/s", 1);
            WeatherUtils AnoWeath15 = new WeatherUtils("15mm27m/s", 2);

            List<WorkCondition> wkcList = new List<WorkCondition>()
            {
                new WorkCondition()
                {
                    SWorkConditionName = "最大风速",
                    Temperature = 10,
                    WindSpeed = 27,
                    IceThickness = 0,
                },
                new WorkCondition()
                {
                    SWorkConditionName = "最低气温",
                    Temperature = -20,
                    WindSpeed = 0,
                    IceThickness = 0,
                },
                new WorkCondition()
                {
                    SWorkConditionName = "最大覆冰",
                    Temperature = -5,
                    WindSpeed = 10,
                    IceThickness = 15,
                },
                new WorkCondition()
                {
                    SWorkConditionName = "平均气温",
                    Temperature = 15,
                    WindSpeed = 0,
                    IceThickness = 0,
                },
                new WorkCondition()
                {
                    SWorkConditionName = "最高气温",
                    Temperature = 50,
                    WindSpeed = 0,
                    IceThickness = 0,
                },
                new WorkCondition()
                {
                    SWorkConditionName = "安装情况",
                    Temperature = -10,
                    WindSpeed = 10,
                    IceThickness = 0,
                },
                new WorkCondition()
                {
                    SWorkConditionName = "不均匀风",
                    Temperature = 10,
                    WindSpeed = 27,
                    IceThickness = 0,
                },
                new WorkCondition()
                {
                    SWorkConditionName = "验算冰",
                    Temperature = -5,
                    WindSpeed = 10,
                    IceThickness = 15,
                },
                new WorkCondition()
                {
                    SWorkConditionName = "验算不均匀冰I",
                    Temperature = -5,
                    WindSpeed = 10,
                    IceThickness = 15,
                },
                new WorkCondition()
                {
                    SWorkConditionName = "验算不均匀冰II",
                    Temperature = -5,
                    WindSpeed = 10,
                    IceThickness = 15,
                },

            };

            OneWeath15.InsertGK(wkcList);
            AnoWeath15.InsertGK(wkcList);

            ElectricalCommonUtils commParas = new ElectricalCommonUtils();
            commParas.Volt = pro.Volt;

            SideCalUtils OneWrieSidePara = new SideCalUtils(0.95f, 2.85f, 0.25f, 1, 4.1f, 0.2f, 1, 4.1f, 0.2f);
            SideCalUtils AnoWrieSidePara = new SideCalUtils(0.95f, 2.8f, 0.25f, 1, 4.1f, 0.2f, 1, 4.1f, 0.2f);
            








        }
    }
}
