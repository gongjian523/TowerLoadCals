using System.Collections.Generic;
using System.Xml.Serialization;
using TowerLoadCals.Mode;

namespace TowerLoadCals.BLL.Electric
{
    public class ElecCalsParas
    {
        //public ProjectInfo  Project { get; set; }

        [XmlAttribute]
        public string SequenceName { get; set; }

        //两侧的导地线参数
        public ElecCalsWireData BackWirePara { get; set; } = new ElecCalsWireData();
        public ElecCalsWireData FrontWirePara { get; set; } = new ElecCalsWireData();

        //跳线参数 两侧的跳线参数相同
        public ElecCalsWire JumpWirePara { get; set; } = new ElecCalsWire();

        //工况参数
        public ElecCalsWeaRes BackWeatherPara { get; set; } = new ElecCalsWeaRes();
        public ElecCalsWeaRes FrontWeatherPara { get; set; } = new ElecCalsWeaRes();

        //公共参数
        public ElecCalsCommRes CommParas { get; set; } = new ElecCalsCommRes();
        //两侧档内参数
        public ElecCalsSideRes BackSidePara { get; set; } = new ElecCalsSideRes();
        public ElecCalsSideRes FrontSidePara { get; set; } = new ElecCalsSideRes();
        //塔库配置参数
        public ElecCalsTowerRes TowerParas { get; set; } = new ElecCalsTowerRes();

        //两侧其他金具参数
        public ElecCalsSpanFit BackSpanFit { get; set; } = new ElecCalsSpanFit();
        public ElecCalsSpanFit FrontSpanFit { get; set; } = new ElecCalsSpanFit();

        //两侧的绝缘子串参数相同
        public ElecCalsStrDatas StrDatas { get; set; } = new ElecCalsStrDatas();

        public ElecCalsTowerAppre Appre { get; set; } = new ElecCalsTowerAppre();
        public TowerElecCals TowerCals { get; set; } = new TowerElecCals();

        protected GlobalInfo globalInst;

        public ElecCalsParas()
        {

        }

        public ElecCalsParas(string sequenceName, TowerSerial backTower, TowerSerial calsTower)
        {
            SequenceName = sequenceName;

            globalInst = GlobalInfo.GetInstance();

            CommParas = globalInst.GetElecCalsCommParasByName(calsTower.CommPar);

            BackSidePara = globalInst.GetElecCalsSideParasByName(calsTower.BackSidePar);
            UpdateSidePara(true, BackSidePara, out string waring1);

            FrontSidePara = globalInst.GetElecCalsSideParasByName(calsTower.FrontSidePar);
            UpdateSidePara(false, FrontSidePara, out string waring2);

            TowerParas = globalInst.GetElecCalsTowerParasByName(calsTower.TowerPar);
            UpdateTowerPara(out string warning3);

            UpdateTowerSerial(backTower, calsTower, out string waring4);
        }

        protected void UpdateSidePara(bool isBackSide, ElecCalsSideRes sidePara, out string waring)
        {
            waring = "";

            //组装气候工况
            if (sidePara.WeatherListName != null && sidePara.WeatherListName != "")
            {
                Weather waeBack = globalInst.GetLocalWeatherByName(sidePara.WeatherListName);
                List<ElecCalsWorkCondition> ListWkCdt = new List<ElecCalsWorkCondition>();

                if (waeBack.WorkConditions != null && waeBack.WorkConditions.Count > 0)
                {
                    foreach (var item in waeBack.WorkConditions)
                    {
                        ListWkCdt.Add(new ElecCalsWorkCondition(item));
                    }
                }

                if (isBackSide)
                {
                    BackWeatherPara = new ElecCalsWeaRes()
                    {
                        Name = sidePara.WeatherListName, 
                        ID = 1,
                        WeathComm = ListWkCdt,
                    };
                }
                else
                {
                    FrontWeatherPara = new ElecCalsWeaRes()
                    {
                        Name = sidePara.WeatherListName,
                        ID = 2,
                        WeathComm = ListWkCdt,
                    };
                }
            }
            else
            {
                waring += "气候工况 ";
            }

            //组装导地线
            Wire Ind, Grd, OPGW;
            if (sidePara.IndName != null && sidePara.IndName != "")
            {
                Ind = globalInst.GetLocalWireByName(true, sidePara.IndName);
            }
            else
            {
                Ind = new Wire();
                waring += "导线 ";
            }

            if (sidePara.GrdName != null && sidePara.GrdName != "")
            {
                Grd = globalInst.GetLocalWireByName(false, sidePara.GrdName);
            }
            else
            {
                Grd = new Wire();
                waring += "地线 ";
            }

            if (sidePara.OPGWName != null && sidePara.OPGWName != "")
            {
                OPGW = globalInst.GetLocalWireByName(false, sidePara.OPGWName);
            }
            else
            {
                OPGW = new Wire();
                waring += "OPGW ";
            }

            if (isBackSide)
            {
                BackWirePara = new ElecCalsWireData()
                {
                    Ind = new ElecCalsWire(Ind.ModelSpecification, 1, Ind.SectionArea, Ind.ExternalDiameter, Ind.UnitLengthMass, Ind.ModulusElasticity,
                        Ind.LineCoefficient, Ind.RatedBreakingForce, 0, sidePara.IndDecrTem, sidePara.IndDevideNum),
                    Grd = new ElecCalsWire(Grd.ModelSpecification, 2, Grd.SectionArea, Grd.ExternalDiameter, Grd.UnitLengthMass, Grd.ModulusElasticity,
                        Grd.LineCoefficient, Grd.RatedBreakingForce, 1, sidePara.GrdDecrTem),
                    OPGW = new ElecCalsWire(OPGW.ModelSpecification, 1, OPGW.SectionArea, OPGW.ExternalDiameter, OPGW.UnitLengthMass, OPGW.ModulusElasticity,
                        OPGW.LineCoefficient, OPGW.RatedBreakingForce, 2, sidePara.OPGWDecrTem),
                };
            }
            else
            {
                FrontWirePara = new ElecCalsWireData()
                {
                    Ind = new ElecCalsWire(Ind.ModelSpecification, 1, Ind.SectionArea, Ind.ExternalDiameter, Ind.UnitLengthMass, Ind.ModulusElasticity,
                        Ind.LineCoefficient, Ind.RatedBreakingForce, 0, sidePara.IndDecrTem, sidePara.IndDevideNum),
                    Grd = new ElecCalsWire(Grd.ModelSpecification, 2, Grd.SectionArea, Grd.ExternalDiameter, Grd.UnitLengthMass, Grd.ModulusElasticity,
                        Grd.LineCoefficient, Grd.RatedBreakingForce, 1, sidePara.GrdDecrTem),
                    OPGW = new ElecCalsWire(OPGW.ModelSpecification, 1, OPGW.SectionArea, OPGW.ExternalDiameter, OPGW.UnitLengthMass, OPGW.ModulusElasticity,
                        OPGW.LineCoefficient, OPGW.RatedBreakingForce, 2, sidePara.OPGWDecrTem),
                };
            }

            //更新间隔棒和防震锤
            FitData IndFZ, IndJGB, GrdFZ;
            if (sidePara.IndFZName != null && sidePara.IndFZName != "")
            {
                IndFZ = globalInst.GetLocalFitDataByName("防震锤", sidePara.IndFZName);
            }
            else
            {
                IndFZ = new FitData();
                waring += "导线防震锤 ";
            }

            if (sidePara.IndJGBName != null && sidePara.IndJGBName != "")
            {
                IndJGB = globalInst.GetLocalFitDataByName("间隔棒", sidePara.IndJGBName);
            }
            else
            {
                IndJGB = new FitData();
                waring += "导线间隔棒 ";
            }

            if (sidePara.GrdFZName != null && sidePara.GrdFZName != "")
            {
                GrdFZ = globalInst.GetLocalFitDataByName("防震锤", sidePara.GrdFZName);
            }
            else
            {
                GrdFZ = new FitData();
                waring += "地线防震锤 ";
            }
            int IndFZNum = 0, IndJGBNum = 0, GrdFZNum = 0;

            //按照规则赋值的方法还不清楚
            if (sidePara.FitDataCalsPara == "1 按规则")
            {

            }
            else
            {
                IndFZNum = sidePara.IndFZNum;
                IndJGBNum = sidePara.IndJGBNum;
                GrdFZNum = sidePara.GrdFZNum;
            }

            if (isBackSide)
            {
                BackSpanFit = new ElecCalsSpanFit(0, IndFZNum, IndFZ.Weight, 0, GrdFZNum, GrdFZ.Weight, 0, IndJGBNum, IndJGB.Weight);
            }
            else
            {
                FrontSpanFit = new ElecCalsSpanFit(0, IndFZNum, IndFZ.Weight, 0, GrdFZNum, GrdFZ.Weight, 0, IndJGBNum, IndJGB.Weight);
            }

        }

        //更新塔库配置参数
        protected void UpdateTowerPara(out string waring)
        {
            waring = "";

            StrData upIndStr, midIndStr, dnIndStr, grdStr, upJumpStr, midJumpStr, dnJumpStr;
            if (TowerParas.UpIndStrDataName != null && TowerParas.UpIndStrDataName != "")
            {
                upIndStr = globalInst.GetLocalStrDataByName("一般子串", TowerParas.UpIndStrDataName);
            }
            else
            {
                upIndStr = new StrData();
                waring += "上相导线串 ";
            }

            if (TowerParas.MidIndStrDataName != null && TowerParas.MidIndStrDataName != "")
            {
                midIndStr = globalInst.GetLocalStrDataByName("一般子串", TowerParas.MidIndStrDataName);
            }
            else
            {
                midIndStr = new StrData();
                waring += "中相导线串 ";
            }

            if (TowerParas.DnIndStrDataName != null && TowerParas.DnIndStrDataName != "")
            {
                dnIndStr = globalInst.GetLocalStrDataByName("一般子串", TowerParas.DnIndStrDataName);
            }
            else
            {
                dnIndStr = new StrData();
                waring += "下相导线串 ";
            }

            if (TowerParas.GrdStrDataName != null && TowerParas.GrdStrDataName != "")
            {
                grdStr = globalInst.GetLocalStrDataByName("一般子串", TowerParas.GrdStrDataName);
            }
            else
            {
                grdStr = new StrData();
                waring += "地线线串 ";
            }

            if (TowerParas.UpJumpStrDataName != null && TowerParas.UpJumpStrDataName != "")
            {
                upJumpStr = globalInst.GetLocalStrDataByName("硬跳线", TowerParas.UpJumpStrDataName);
            }
            else
            {
                upJumpStr = new StrData();
                waring += "上相跳线串 ";
            }

            if (TowerParas.MidJumpStrDataName != null && TowerParas.MidJumpStrDataName != "")
            {
                midJumpStr = globalInst.GetLocalStrDataByName("硬跳线", TowerParas.MidJumpStrDataName);
            }
            else
            {
                midJumpStr = new StrData();
                waring += "中相跳线串 ";
            }

            if (TowerParas.DnJumpStrDataName != null && TowerParas.DnJumpStrDataName != "")
            {
                dnJumpStr = globalInst.GetLocalStrDataByName("硬跳线", TowerParas.DnJumpStrDataName);
            }
            else
            {
                dnJumpStr = new StrData();
                waring += "下相跳线串 ";
            }

            StrDatas = new ElecCalsStrDatas();
            StrDatas.UpIndStr.SetIGPara(upIndStr.Weight, upIndStr.LNum, upIndStr.PieceNum, upIndStr.PieceLength, upIndStr.GoldPieceNum,
                upIndStr.FitLength, upIndStr.DampLength);
            StrDatas.MidIndStr.SetIGPara(midIndStr.Weight, midIndStr.LNum, midIndStr.PieceNum, midIndStr.PieceLength, midIndStr.GoldPieceNum,
                midIndStr.FitLength, midIndStr.DampLength);
            StrDatas.DnIndStr.SetIGPara(dnIndStr.Weight, dnIndStr.LNum, dnIndStr.PieceNum, dnIndStr.PieceLength, dnIndStr.GoldPieceNum,
                dnIndStr.FitLength, dnIndStr.DampLength);
            StrDatas.GrdStr.SetIGPara(grdStr.Weight, grdStr.LNum, grdStr.PieceNum, grdStr.PieceLength, grdStr.GoldPieceNum,
                grdStr.FitLength, grdStr.DampLength);

            StrDatas.UpJumpStr.SetJumPara(upJumpStr.Weight, upJumpStr.LNum, upJumpStr.PieceNum, upJumpStr.GoldPieceNum, upJumpStr.SoftLineLen,
                upJumpStr.JGBNum, upJumpStr.SuTubleLen, TowerParas.SuTubleDia, TowerParas.SuTubleWei);
            StrDatas.MidJumpStr.SetJumPara(midJumpStr.Weight, midJumpStr.LNum, midJumpStr.PieceNum, midJumpStr.GoldPieceNum, midJumpStr.SoftLineLen,
                midJumpStr.JGBNum, midJumpStr.SuTubleLen, TowerParas.SuTubleDia, TowerParas.SuTubleWei);
            StrDatas.DnJumpStr.SetJumPara(dnJumpStr.Weight, dnJumpStr.LNum, dnJumpStr.PieceNum, dnJumpStr.GoldPieceNum, dnJumpStr.SoftLineLen,
                dnJumpStr.JGBNum, dnJumpStr.SuTubleLen, TowerParas.SuTubleDia, TowerParas.SuTubleWei);

            Wire jump;
            if (TowerParas.JumpName != null && TowerParas.JumpName != "")
            {
                jump = globalInst.GetLocalWireByName(true, TowerParas.JumpName);
            }
            else
            {
                jump = new Wire();
                waring += "跳线 ";
            }

            JumpWirePara = new ElecCalsWire(jump.ModelSpecification, 1, jump.SectionArea, jump.ExternalDiameter, jump.UnitLengthMass, jump.ModulusElasticity,
                jump.LineCoefficient, jump.RatedBreakingForce, 0, 0, TowerParas.JumpDevideNum);
        }

        //更新塔序列参数
        protected void UpdateTowerSerial(TowerSerial backTowerSerial, TowerSerial calsTowerSerial, out string waring)
        {
            TowerStrData towerStr = globalInst.GetLocalTowerStrByName(calsTowerSerial.TowerPattern);
            Appre = new ElecCalsTowerAppre()
            {
                UpSideInHei = towerStr.UpSideInHei,
                MidInHei = towerStr.MidInHei,
                DnSideInHei = towerStr.DnSideInHei,
                GrDHei = towerStr.GrDHei,
                UpSideJuHei = towerStr.UpSideJuHei,
                MidJuHei = towerStr.MidJuHei,
                DnSideJuHei = towerStr.DnSideJuHei,
            };

            TowerElecCals tower = new TowerElecCals();
            tower.SetPosInf(calsTowerSerial.TowerName, calsTowerSerial.TowerPattern, calsTowerSerial.CallItHigh, calsTowerSerial.TowerFootElevation, calsTowerSerial.BaseLevelDescent, TowerParas.RepStrIndLen,
                TowerParas.RepStrGrdLen, calsTowerSerial.TurningAngle);
            tower.SetFrontBackPosInf(calsTowerSerial.Span, calsTowerSerial.FrontPreSpan, backTowerSerial.Span, backTowerSerial.FrontPreSpan);

            waring = "";
        }
    }


    public class ElecCalsWireData
    {
        public ElecCalsWire Ind { get; set; } = new ElecCalsWire();
        public ElecCalsWire Grd { get; set; } = new ElecCalsWire();
        public ElecCalsWire OPGW { get; set; } = new ElecCalsWire();
    }

    public class ElecCalsWeatherData
    {
        public string Name { get; set; }
        public List<ElecCalsWorkCondition> WeatherConditions { get; set; } = new List<ElecCalsWorkCondition>();
    }


    public class ElecCalsStrDatas
    {
        public ElecCalsStrData UpIndStr { get; set; } = new ElecCalsStrData();
        public ElecCalsStrData MidIndStr { get; set; } = new ElecCalsStrData();
        public ElecCalsStrData DnIndStr { get; set; } = new ElecCalsStrData();

        public ElecCalsStrData UpJumpStr { get; set; } = new ElecCalsStrData();
        public ElecCalsStrData MidJumpStr { get; set; } = new ElecCalsStrData();
        public ElecCalsStrData DnJumpStr { get; set; } = new ElecCalsStrData();

        public ElecCalsStrData GrdStr { get; set; } = new ElecCalsStrData();

    }


}
