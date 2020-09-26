using DevExpress.Spreadsheet;
using DevExpress.Xpf.Spreadsheet;
using System.Collections.Generic;
using System.Linq;
using TowerLoadCals.Mode;
using TowerLoadCals.Mode.Electric;
using TowerLoadCals.Mode.Structure;

namespace TowerLoadCals.BLL.Electric
{
    public class ElecLoadProcessExcel
    {
        protected string Path;

        protected IWorkbook workbook;

        protected Worksheet sheetPara;
        protected Worksheet sheetCalsPro;
        protected Worksheet sheetCalsRest;
        protected Worksheet sheetElecLoad;
        protected Worksheet sheetLoadConv;

        protected SpreadsheetControl SC;

        protected TowerElecCals TowerCals;

        public ElecLoadProcessExcel(string  path, SpreadsheetControl sc = null)
        {
            Path = path;
            SC = sc == null ?  new SpreadsheetControl() : sc;

            workbook = SC.Document;
            workbook.LoadDocument(path, DocumentFormat.Xlsx);

            sheetPara = workbook.Worksheets[ConstVar.ElecLoadParaSheet];
            sheetCalsPro = workbook.Worksheets[ConstVar.ElecLoadCalsProSheet];
            sheetCalsRest = workbook.Worksheets[ConstVar.ElecLoadCalsRstSheet];
            sheetLoadConv = workbook.Worksheets[ConstVar.ElecLoadConvStruSheet];
        }

        public void CalsElecLoad(TowerElecCals calsTower)
        {
            TowerCals = calsTower;
            if (calsTower.TowerType == "悬垂塔")
            {
                LoadHangTowerResult();

                if(((TowerHangElecCals)calsTower).SideRes.CommParas.CircuitType == 1)
                    sheetElecLoad = workbook.Worksheets[ConstVar.ElecLoadSingleCircuitOutputSheet];
                else
                    sheetElecLoad = workbook.Worksheets[ConstVar.ElecLoadSingleCircuitOutputSheet];
            }
            else
            {
                LoadStrainTowerResult();

                if (((TowerStrainElecCals)calsTower).BackSideRes.CommParas.CircuitType == 1)
                    sheetElecLoad = workbook.Worksheets[ConstVar.ElecLoadSingleCircuitOutputSheet];
                else
                    sheetElecLoad = workbook.Worksheets[ConstVar.ElecLoadSingleCircuitOutputSheet];
            }

            Caculate();
        }

        protected void LoadHangTowerResult()
        {
            //更新参数
            int paraCnt = 1;

            ElecCalsRes sideRes = ((TowerHangElecCals)TowerCals).SideRes;
            sheetCalsPro.Cells[paraCnt, 1].Value = sideRes.CommParas.VoltStr;  //电压等级
            sheetCalsPro.Cells[paraCnt, 3].Value = sideRes.CommParas.CircuitType; // 单双回路类型

            paraCnt++;
            sheetCalsPro.Cells[paraCnt, 1].Value = sideRes.CommParas.bGrd1Break;  //地线1是否开断
            sheetCalsPro.Cells[paraCnt, 3].Value = sideRes.CommParas.bGrd2Break;  //地线2是否开断
            sheetCalsPro.Cells[paraCnt, 5].Value = sideRes.CommParas.GrdIceForcePara == 1 ? "考虑+5mm" : "不考虑+5mm"; //地线覆冰张力计算模式

            paraCnt++;
            sheetCalsPro.Cells[paraCnt, 1].Value = sideRes.CommParas.BreakMaxPara; //断线max取值
            sheetCalsPro.Cells[paraCnt, 3].Value = sideRes.CommParas.UnbaMaxPara;  //不均匀冰max取值
            sheetCalsPro.Cells[paraCnt, 5].Value = sideRes.CommParas.InsVerWeiPara; //安装垂直荷载取值
            sheetCalsPro.Cells[paraCnt, 7].Value = sideRes.CommParas.UnbaIceVerWeiPara; //不均匀冰垂直荷载取值

            paraCnt++;
            sheetCalsPro.Cells[paraCnt, 1].Value = sideRes.CommParas.HandForcePara == 1 ? " 两者大值" : (sideRes.CommParas.HandForcePara == 2 ? "系数法" : "降温法"); //锚线张力取值
            sheetCalsPro.Cells[paraCnt, 5].Value = sideRes.CommParas.GrdIceCheckPara == 1 ? "不考虑+5mm" : "考虑+5mm"; //地线验算张力取值

            paraCnt++;
            sheetCalsPro.Cells[paraCnt, 1].Value = TowerCals.AngelofApplication;
            var wea = sideRes.Weather.WeathComm.Where(item => item.Name == "最大风速").FirstOrDefault();
            sheetCalsPro.Cells[paraCnt, 3].Value = wea.WindSpeed;
            sheetCalsPro.Cells[paraCnt, 5].Value = sideRes.CommParas.IndInstallTenDiff; //导线安装张力差

            paraCnt++;
            sheetCalsPro.Cells[paraCnt, 1].Value = sideRes.TowerParas.IndUpliftForce; //导线上拔力
            sheetCalsPro.Cells[paraCnt, 3].Value = sideRes.TowerParas.Grd1UpliftForce; //一根导线上拔力
            sheetCalsPro.Cells[paraCnt, 5].Value = sideRes.TowerParas.Grd2pliftForce; //另一根导线上拔力


            //更新垂直档距
            int count = 3;
            for (int i = 0; i < 5; i++)
            {
                var phase = TowerCals.PhaseTraList[i];
                var phaseAn = TowerCals.PhaseTraList[i + 5];
                List<string> wkList = phase.WireData.WorkCdtNamesHang;

                for (int j = 0; j < wkList.Count; j++)
                {
                    sheetCalsPro.Cells[count, 0].Value = wkList[j];
                    sheetCalsPro.Cells[count, 1].Value = phase.LoadList[j].VetiSpanStr;
                    sheetCalsPro.Cells[count, 2].Value = phase.LoadList[j].VetiSpan;
                    sheetCalsPro.Cells[count, 3].Value = wkList[j];
                    sheetCalsPro.Cells[count, 4].Value = phaseAn.LoadList[j].VetiSpanStr;
                    sheetCalsPro.Cells[count, 5].Value = phaseAn.LoadList[j].VetiSpan;
                    count++;
                }
            }

            //更新水平荷载
            count += 2;
            for (int i = 0; i < 5; i++)
            {
                var phase = TowerCals.PhaseTraList[i];
                List<string> wkList = phase.WireData.WorkCdtNamesHang;
                for (int j = 0; j < wkList.Count; j++)
                {
                    sheetCalsPro.Cells[count, 0].Value = wkList[j];
                    sheetCalsPro.Cells[count, 1].Value = phase.LoadList[j].HorForStr;
                    sheetCalsPro.Cells[count, 2].Value = phase.LoadList[j].HorFor;
                    sheetCalsPro.Cells[count, 3].Value = wkList[j];
                    sheetCalsPro.Cells[count, 4].Value = phase.StrLoad[wkList[j]].WindLoadStr;
                    sheetCalsPro.Cells[count, 5].Value = phase.StrLoad[wkList[j]].WindLoad;
                    count++;
                }
            }

            //更新垂直荷载
            count += 2;
            for (int i = 0; i < 5; i++)
            {
                var phase = TowerCals.PhaseTraList[i];
                List<string> wkList = phase.WireData.WorkCdtNamesHang;

                for (int j = 0; j < wkList.Count; j++)
                {
                    sheetCalsPro.Cells[count, 0].Value = wkList[j];
                    sheetCalsPro.Cells[count, 1].Value = phase.LoadList[j].VerWeiStr;
                    sheetCalsPro.Cells[count, 2].Value = phase.LoadList[j].VerWei;
                    sheetCalsPro.Cells[count, 3].Value = wkList[j];
                    sheetCalsPro.Cells[count, 4].Value = phase.StrLoad[wkList[j]].VerLoadStr;
                    sheetCalsPro.Cells[count, 5].Value = phase.StrLoad[wkList[j]].VerLoad;
                    count++;
                }
            }

            //更新纵向荷载
            count += 1;
            for (int i = 0; i < 5; i++)
            {
                var phase = TowerCals.PhaseTraList[i];
                var phaseAn = TowerCals.PhaseTraList[i + 5];
                List<string> wkList = phase.WireData.WorkCdtNamesHang;

                for (int j = 0; j < wkList.Count; j++)
                {
                    sheetCalsPro.Cells[count, 0].Value = wkList[j];
                    sheetCalsPro.Cells[count, 1].Value = phase.LoadList[j].LoStrStr;
                    sheetCalsPro.Cells[count, 2].Value = phase.LoadList[j].LoStr;
                    sheetCalsPro.Cells[count, 3].Value = wkList[j];
                    sheetCalsPro.Cells[count, 4].Value = phaseAn.LoadList[j].LoStrStr;
                    sheetCalsPro.Cells[count, 5].Value = phaseAn.LoadList[j].LoStr;
                    sheetCalsPro.Cells[count, 6].Value = phaseAn.LoadList[j].LoStrStr;
                    sheetCalsPro.Cells[count, 7].Value = phaseAn.LoadList[j].LoStr;
                    sheetCalsPro.Cells[count, 8].Value = phaseAn.LoadList[j].LoStrStr;
                    sheetCalsPro.Cells[count, 9].Value = phaseAn.LoadList[j].LoStr;
                    count++;
                }
            }

            //断线
            sheetCalsPro.Cells[count, 1].Value = TowerCals.PhaseTraList[0].BreakTenDiffStr;
            sheetCalsPro.Cells[count, 2].Value = TowerCals.PhaseTraList[0].BreakTenDiff;
            sheetCalsPro.Cells[count, 7].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[0]).TenBreakMax;
            sheetCalsPro.Cells[count, 8].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[0]).TenBreakIceCov100;
            sheetCalsPro.Cells[count, 9].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[5]).TenBreakIceCov100;
            count++;

            sheetCalsPro.Cells[count, 1].Value = TowerCals.PhaseTraList[3].BreakTenDiffStr;
            sheetCalsPro.Cells[count, 2].Value = TowerCals.PhaseTraList[3].BreakTenDiff;
            sheetCalsPro.Cells[count, 4].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[3]).BreakTenDiffGrdBreStr;
            sheetCalsPro.Cells[count, 5].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[3]).BreakTenDiffGrdBre;
            sheetCalsPro.Cells[count, 7].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[3]).TenBreakMax;
            sheetCalsPro.Cells[count, 8].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[3]).TenBreakIceCov100;
            sheetCalsPro.Cells[count, 9].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[8]).TenBreakIceCov100;
            count++;

            sheetCalsPro.Cells[count, 1].Value = TowerCals.PhaseTraList[4].BreakTenDiffStr;
            sheetCalsPro.Cells[count, 2].Value = TowerCals.PhaseTraList[4].BreakTenDiff;
            sheetCalsPro.Cells[count, 4].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[4]).BreakTenDiffGrdBreStr;
            sheetCalsPro.Cells[count, 5].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[4]).BreakTenDiffGrdBre;
            sheetCalsPro.Cells[count, 7].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[4]).TenBreakMax;
            sheetCalsPro.Cells[count, 8].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[4]).TenBreakIceCov100;
            sheetCalsPro.Cells[count, 9].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[9]).TenBreakIceCov100;
            count += 2;

            sheetCalsPro.Cells[count, 1].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[0]).TenBreakMaxTensionS;
            sheetCalsPro.Cells[count, 2].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[0]).TenBreakMaxTensionL;
            sheetCalsPro.Cells[count, 4].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[0]).TenBreakMaxIceCov100S;
            sheetCalsPro.Cells[count, 5].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[0]).TenBreakMaxIceCov100L;
            sheetCalsPro.Cells[count, 7].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[0]).BreakTenMaxS;
            sheetCalsPro.Cells[count, 8].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[0]).BreakTenMaxL;
            count++;

            sheetCalsPro.Cells[count, 1].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[5]).TenBreakMaxTensionS;
            sheetCalsPro.Cells[count, 2].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[5]).TenBreakMaxTensionL;
            sheetCalsPro.Cells[count, 4].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[5]).TenBreakMaxIceCov100S;
            sheetCalsPro.Cells[count, 5].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[5]).TenBreakMaxIceCov100L;
            sheetCalsPro.Cells[count, 7].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[5]).BreakTenMaxS;
            sheetCalsPro.Cells[count, 8].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[5]).BreakTenMaxL;
            count++;

            sheetCalsPro.Cells[count, 1].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[3]).TenBreakMaxTensionS;
            sheetCalsPro.Cells[count, 2].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[3]).TenBreakMaxTensionL;
            sheetCalsPro.Cells[count, 4].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[3]).TenBreakMaxIceCov100S;
            sheetCalsPro.Cells[count, 5].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[3]).TenBreakMaxIceCov100L;
            sheetCalsPro.Cells[count, 7].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[3]).BreakTenMaxS;
            sheetCalsPro.Cells[count, 8].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[3]).BreakTenMaxL;
            count++;

            sheetCalsPro.Cells[count, 1].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[8]).TenBreakMaxTensionS;
            sheetCalsPro.Cells[count, 2].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[8]).TenBreakMaxTensionL;
            sheetCalsPro.Cells[count, 4].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[8]).TenBreakMaxIceCov100S;
            sheetCalsPro.Cells[count, 5].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[8]).TenBreakMaxIceCov100L;
            sheetCalsPro.Cells[count, 7].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[8]).BreakTenMaxS;
            sheetCalsPro.Cells[count, 8].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[8]).BreakTenMaxL;
            count++;

            sheetCalsPro.Cells[count, 1].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[4]).TenBreakMaxTensionS;
            sheetCalsPro.Cells[count, 2].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[4]).TenBreakMaxTensionL;
            sheetCalsPro.Cells[count, 4].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[4]).TenBreakMaxIceCov100S;
            sheetCalsPro.Cells[count, 5].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[4]).TenBreakMaxIceCov100L;
            sheetCalsPro.Cells[count, 7].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[4]).BreakTenMaxS;
            sheetCalsPro.Cells[count, 8].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[4]).BreakTenMaxL;
            count++;

            sheetCalsPro.Cells[count, 1].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[9]).TenBreakMaxTensionS;
            sheetCalsPro.Cells[count, 2].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[9]).TenBreakMaxTensionL;
            sheetCalsPro.Cells[count, 4].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[9]).TenBreakMaxIceCov100S;
            sheetCalsPro.Cells[count, 5].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[9]).TenBreakMaxIceCov100L;
            sheetCalsPro.Cells[count, 7].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[9]).BreakTenMaxS;
            sheetCalsPro.Cells[count, 8].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[9]).BreakTenMaxL;
            count++;

            sheetCalsPro.Cells[count, 1].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[9]).TenBreakMaxTensionS;
            sheetCalsPro.Cells[count, 2].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[9]).TenBreakMaxTensionL;
            sheetCalsPro.Cells[count, 4].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[9]).TenBreakMaxIceCov100S;
            sheetCalsPro.Cells[count, 5].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[9]).TenBreakMaxIceCov100L;
            sheetCalsPro.Cells[count, 7].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[9]).BreakTenMaxS;
            sheetCalsPro.Cells[count, 8].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[9]).BreakTenMaxL;
            count++;

            sheetCalsPro.Cells[count, 7].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[0]).TenBreakS;
            sheetCalsPro.Cells[count, 8].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[0]).TenBreakL;
            count++;

            sheetCalsPro.Cells[count, 7].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[5]).TenBreakS;
            sheetCalsPro.Cells[count, 8].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[5]).TenBreakL;
            count++;

            sheetCalsPro.Cells[count, 7].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[3]).TenBreakS;
            sheetCalsPro.Cells[count, 8].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[3]).TenBreakL;
            count++;

            sheetCalsPro.Cells[count, 7].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[8]).TenBreakS;
            sheetCalsPro.Cells[count, 8].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[8]).TenBreakL;
            count++;

            sheetCalsPro.Cells[count, 7].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[4]).TenBreakS;
            sheetCalsPro.Cells[count, 8].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[4]).TenBreakL;
            count++;

            sheetCalsPro.Cells[count, 7].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[9]).TenBreakS;
            sheetCalsPro.Cells[count, 8].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[9]).TenBreakL;
            count += 2;

            sheetCalsPro.Cells[count, 1].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[3]).TenBreakMaxTensionBreakS;
            sheetCalsPro.Cells[count, 2].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[3]).TenBreakMaxTensionBreakL;
            sheetCalsPro.Cells[count, 3].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[3]).TenBreakMaxIceCov100BreakS;
            sheetCalsPro.Cells[count, 4].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[3]).TenBreakMaxIceCov100BreakL;
            sheetCalsPro.Cells[count, 5].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[3]).BreakTenMaxBreakS;
            sheetCalsPro.Cells[count, 6].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[3]).BreakTenMaxBreakL;
            count++;

            sheetCalsPro.Cells[count, 1].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[8]).TenBreakMaxTensionBreakS;
            sheetCalsPro.Cells[count, 2].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[8]).TenBreakMaxTensionBreakL;
            sheetCalsPro.Cells[count, 3].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[8]).TenBreakMaxIceCov100BreakS;
            sheetCalsPro.Cells[count, 4].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[8]).TenBreakMaxIceCov100BreakL;
            sheetCalsPro.Cells[count, 5].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[8]).BreakTenMaxBreakS;
            sheetCalsPro.Cells[count, 6].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[8]).BreakTenMaxBreakL;
            count++;

            sheetCalsPro.Cells[count, 1].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[4]).TenBreakMaxTensionBreakS;
            sheetCalsPro.Cells[count, 2].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[4]).TenBreakMaxTensionBreakL;
            sheetCalsPro.Cells[count, 3].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[4]).TenBreakMaxIceCov100BreakS;
            sheetCalsPro.Cells[count, 4].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[4]).TenBreakMaxIceCov100BreakL;
            sheetCalsPro.Cells[count, 5].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[4]).BreakTenMaxBreakS;
            sheetCalsPro.Cells[count, 6].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[4]).BreakTenMaxBreakL;
            count++;

            sheetCalsPro.Cells[count, 1].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[9]).TenBreakMaxTensionBreakS;
            sheetCalsPro.Cells[count, 2].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[9]).TenBreakMaxTensionBreakL;
            sheetCalsPro.Cells[count, 3].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[9]).TenBreakMaxIceCov100BreakS;
            sheetCalsPro.Cells[count, 4].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[9]).TenBreakMaxIceCov100BreakL;
            sheetCalsPro.Cells[count, 5].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[9]).BreakTenMaxBreakS;
            sheetCalsPro.Cells[count, 6].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[9]).BreakTenMaxBreakL;
            count++;

            sheetCalsPro.Cells[count, 1].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[3]).BreakTenAdd5mmBreakS;
            sheetCalsPro.Cells[count, 2].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[3]).BreakTenAdd5mmBreakL;
            sheetCalsPro.Cells[count, 3].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[3]).BreakTenBreakS;
            sheetCalsPro.Cells[count, 4].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[3]).BreakTenBreakL;
            count++;

            sheetCalsPro.Cells[count, 1].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[8]).BreakTenAdd5mmBreakS;
            sheetCalsPro.Cells[count, 2].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[8]).BreakTenAdd5mmBreakL;
            sheetCalsPro.Cells[count, 3].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[8]).BreakTenBreakS;
            sheetCalsPro.Cells[count, 4].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[8]).BreakTenBreakL;
            count++;

            sheetCalsPro.Cells[count, 1].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[4]).BreakTenAdd5mmBreakS;
            sheetCalsPro.Cells[count, 2].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[4]).BreakTenAdd5mmBreakL;
            sheetCalsPro.Cells[count, 3].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[4]).BreakTenBreakS;
            sheetCalsPro.Cells[count, 4].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[4]).BreakTenBreakL;
            count++;

            sheetCalsPro.Cells[count, 1].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[9]).BreakTenAdd5mmBreakS;
            sheetCalsPro.Cells[count, 2].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[9]).BreakTenAdd5mmBreakL;
            sheetCalsPro.Cells[count, 3].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[9]).BreakTenBreakS;
            sheetCalsPro.Cells[count, 4].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[9]).BreakTenBreakL;
            count++;

            //不均匀冰计算
            sheetCalsPro.Cells[count, 1].Value = TowerCals.PhaseTraList[0].UnbaIceTenDiffStr;
            sheetCalsPro.Cells[count, 2].Value = TowerCals.PhaseTraList[0].UnbaIceTenDiff;
            sheetCalsPro.Cells[count, 7].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[0]).TenUnbaIceMax;
            sheetCalsPro.Cells[count, 8].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[0]).TenUnbaIceIceCov100;
            sheetCalsPro.Cells[count, 9].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[5]).TenUnbaIceIceCov100;
            count++;

            sheetCalsPro.Cells[count, 1].Value = TowerCals.PhaseTraList[3].UnbaIceTenDiffStr;
            sheetCalsPro.Cells[count, 2].Value = TowerCals.PhaseTraList[3].UnbaIceTenDiff;
            sheetCalsPro.Cells[count, 4].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[3]).UnbaIceTenDiffGrdBreStr;
            sheetCalsPro.Cells[count, 5].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[3]).UnbaIceTenDiffGrdBre;
            sheetCalsPro.Cells[count, 7].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[3]).TenUnbaIceMax;
            sheetCalsPro.Cells[count, 8].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[3]).TenUnbaIceIceCov100;
            sheetCalsPro.Cells[count, 9].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[8]).TenUnbaIceIceCov100;
            count++;

            sheetCalsPro.Cells[count, 1].Value = TowerCals.PhaseTraList[4].UnbaIceTenDiffStr;
            sheetCalsPro.Cells[count, 2].Value = TowerCals.PhaseTraList[4].UnbaIceTenDiff;
            sheetCalsPro.Cells[count, 4].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[4]).UnbaIceTenDiffGrdBreStr;
            sheetCalsPro.Cells[count, 5].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[4]).UnbaIceTenDiffGrdBre;
            sheetCalsPro.Cells[count, 7].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[4]).TenUnbaIceMax;
            sheetCalsPro.Cells[count, 8].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[4]).TenUnbaIceIceCov100;
            sheetCalsPro.Cells[count, 9].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[9]).TenUnbaIceIceCov100;
            count += 2;

            sheetCalsPro.Cells[count, 1].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[0]).TenUnbaIceMaxTensionS;
            sheetCalsPro.Cells[count, 2].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[0]).TenUnbaIceMaxTensionL;
            sheetCalsPro.Cells[count, 4].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[0]).TenUnbaIceMaxIceCov100S;
            sheetCalsPro.Cells[count, 5].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[0]).TenUnbaIceMaxIceCov100L;
            sheetCalsPro.Cells[count, 7].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[0]).UnbaIceTenMaxS;
            sheetCalsPro.Cells[count, 8].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[0]).UnbaIceTenMaxL;
            count++;

            sheetCalsPro.Cells[count, 1].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[5]).TenUnbaIceMaxTensionS;
            sheetCalsPro.Cells[count, 2].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[5]).TenUnbaIceMaxTensionL;
            sheetCalsPro.Cells[count, 4].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[5]).TenUnbaIceMaxIceCov100S;
            sheetCalsPro.Cells[count, 5].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[5]).TenUnbaIceMaxIceCov100L;
            sheetCalsPro.Cells[count, 7].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[5]).UnbaIceTenMaxS;
            sheetCalsPro.Cells[count, 8].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[5]).UnbaIceTenMaxL;
            count++;

            sheetCalsPro.Cells[count, 1].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[3]).TenUnbaIceMaxTensionS;
            sheetCalsPro.Cells[count, 2].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[3]).TenUnbaIceMaxTensionL;
            sheetCalsPro.Cells[count, 4].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[3]).TenUnbaIceMaxIceCov100S;
            sheetCalsPro.Cells[count, 5].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[3]).TenUnbaIceMaxIceCov100L;
            sheetCalsPro.Cells[count, 7].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[3]).UnbaIceTenMaxS;
            sheetCalsPro.Cells[count, 8].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[3]).UnbaIceTenMaxL;
            count++;

            sheetCalsPro.Cells[count, 1].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[8]).TenUnbaIceMaxTensionS;
            sheetCalsPro.Cells[count, 2].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[8]).TenUnbaIceMaxTensionL;
            sheetCalsPro.Cells[count, 4].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[8]).TenUnbaIceMaxIceCov100S;
            sheetCalsPro.Cells[count, 5].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[8]).TenUnbaIceMaxIceCov100L;
            sheetCalsPro.Cells[count, 7].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[8]).UnbaIceTenMaxS;
            sheetCalsPro.Cells[count, 8].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[8]).UnbaIceTenMaxL;
            count++;

            sheetCalsPro.Cells[count, 1].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[4]).TenUnbaIceMaxTensionS;
            sheetCalsPro.Cells[count, 2].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[4]).TenUnbaIceMaxTensionL;
            sheetCalsPro.Cells[count, 4].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[4]).TenUnbaIceMaxIceCov100S;
            sheetCalsPro.Cells[count, 5].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[4]).TenUnbaIceMaxIceCov100L;
            sheetCalsPro.Cells[count, 7].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[4]).UnbaIceTenMaxS;
            sheetCalsPro.Cells[count, 8].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[4]).UnbaIceTenMaxL;
            count++;

            sheetCalsPro.Cells[count, 1].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[9]).TenUnbaIceMaxTensionS;
            sheetCalsPro.Cells[count, 2].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[9]).TenUnbaIceMaxTensionL;
            sheetCalsPro.Cells[count, 4].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[9]).TenUnbaIceMaxIceCov100S;
            sheetCalsPro.Cells[count, 5].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[9]).TenUnbaIceMaxIceCov100L;
            sheetCalsPro.Cells[count, 7].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[9]).UnbaIceTenMaxS;
            sheetCalsPro.Cells[count, 8].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[9]).UnbaIceTenMaxL;
            count++;

            sheetCalsPro.Cells[count, 7].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[0]).TenUnbaIceS;
            sheetCalsPro.Cells[count, 8].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[0]).TenUnbaIceL;
            count++;

            sheetCalsPro.Cells[count, 7].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[5]).TenUnbaIceS;
            sheetCalsPro.Cells[count, 8].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[5]).TenUnbaIceL;
            count++;

            sheetCalsPro.Cells[count, 7].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[3]).TenUnbaIceS;
            sheetCalsPro.Cells[count, 8].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[3]).TenUnbaIceL;
            count++;

            sheetCalsPro.Cells[count, 7].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[8]).TenUnbaIceS;
            sheetCalsPro.Cells[count, 8].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[8]).TenUnbaIceL;
            count++;

            sheetCalsPro.Cells[count, 7].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[4]).TenUnbaIceS;
            sheetCalsPro.Cells[count, 8].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[4]).TenUnbaIceL;
            count++;

            sheetCalsPro.Cells[count, 7].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[9]).TenUnbaIceS;
            sheetCalsPro.Cells[count, 8].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[9]).TenUnbaIceL;
            count += 2;

            sheetCalsPro.Cells[count, 1].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[3]).TenUnbaIceMaxTensionBreakS;
            sheetCalsPro.Cells[count, 2].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[3]).TenUnbaIceMaxTensionBreakL;
            sheetCalsPro.Cells[count, 3].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[3]).TenUnbaIceMaxIceCov100BreakS;
            sheetCalsPro.Cells[count, 4].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[3]).TenUnbaIceMaxIceCov100BreakL;
            sheetCalsPro.Cells[count, 5].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[3]).UnbaIceTenMaxBreakS;
            sheetCalsPro.Cells[count, 6].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[3]).UnbaIceTenMaxBreakL;
            count++;

            sheetCalsPro.Cells[count, 1].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[8]).TenUnbaIceMaxTensionBreakS;
            sheetCalsPro.Cells[count, 2].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[8]).TenUnbaIceMaxTensionBreakL;
            sheetCalsPro.Cells[count, 3].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[8]).TenUnbaIceMaxIceCov100BreakS;
            sheetCalsPro.Cells[count, 4].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[8]).TenUnbaIceMaxIceCov100BreakL;
            sheetCalsPro.Cells[count, 5].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[8]).UnbaIceTenMaxBreakS;
            sheetCalsPro.Cells[count, 6].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[8]).UnbaIceTenMaxBreakL;
            count++;

            sheetCalsPro.Cells[count, 1].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[4]).TenUnbaIceMaxTensionBreakS;
            sheetCalsPro.Cells[count, 2].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[4]).TenUnbaIceMaxTensionBreakL;
            sheetCalsPro.Cells[count, 3].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[4]).TenUnbaIceMaxIceCov100BreakS;
            sheetCalsPro.Cells[count, 4].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[4]).TenUnbaIceMaxIceCov100BreakL;
            sheetCalsPro.Cells[count, 5].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[4]).UnbaIceTenMaxBreakS;
            sheetCalsPro.Cells[count, 6].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[4]).UnbaIceTenMaxBreakL;
            count++;

            sheetCalsPro.Cells[count, 1].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[9]).TenUnbaIceMaxTensionBreakS;
            sheetCalsPro.Cells[count, 2].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[9]).TenUnbaIceMaxTensionBreakL;
            sheetCalsPro.Cells[count, 3].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[9]).TenUnbaIceMaxIceCov100BreakS;
            sheetCalsPro.Cells[count, 4].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[9]).TenUnbaIceMaxIceCov100BreakL;
            sheetCalsPro.Cells[count, 5].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[9]).UnbaIceTenMaxBreakS;
            sheetCalsPro.Cells[count, 6].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[9]).UnbaIceTenMaxBreakL;
            count++;

            sheetCalsPro.Cells[count, 1].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[3]).UnbaIceTenAdd5mmBreakS;
            sheetCalsPro.Cells[count, 2].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[3]).UnbaIceTenAdd5mmBreakL;
            sheetCalsPro.Cells[count, 3].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[3]).UnbaIceTenBreakS;
            sheetCalsPro.Cells[count, 4].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[3]).UnbaIceTenBreakL;
            count++;

            sheetCalsPro.Cells[count, 1].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[8]).UnbaIceTenAdd5mmBreakS;
            sheetCalsPro.Cells[count, 2].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[8]).UnbaIceTenAdd5mmBreakL;
            sheetCalsPro.Cells[count, 3].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[8]).UnbaIceTenBreakS;
            sheetCalsPro.Cells[count, 4].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[8]).UnbaIceTenBreakL;
            count++;

            sheetCalsPro.Cells[count, 1].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[4]).UnbaIceTenAdd5mmBreakS;
            sheetCalsPro.Cells[count, 2].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[4]).UnbaIceTenAdd5mmBreakL;
            sheetCalsPro.Cells[count, 3].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[4]).UnbaIceTenBreakS;
            sheetCalsPro.Cells[count, 4].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[4]).UnbaIceTenBreakL;
            count++;

            sheetCalsPro.Cells[count, 1].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[9]).UnbaIceTenAdd5mmBreakS;
            sheetCalsPro.Cells[count, 2].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[9]).UnbaIceTenAdd5mmBreakL;
            sheetCalsPro.Cells[count, 3].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[9]).UnbaIceTenBreakS;
            sheetCalsPro.Cells[count, 4].Value = ((ElecCalsPhaseStrHang)TowerCals.PhaseTraList[9]).UnbaIceTenBreakL;

        }

        protected void LoadStrainTowerResult()
        {
            //更新参数
            int paraCnt = 1;

            ElecCalsRes backSideRes = ((TowerStrainElecCals)TowerCals).BackSideRes;
            ElecCalsRes frontSideRes = ((TowerStrainElecCals)TowerCals).FrontSideRes;

            sheetCalsPro.Cells[paraCnt, 1].Value = backSideRes.CommParas.VoltStr;  //电压等级
            sheetCalsPro.Cells[paraCnt, 3].Value = backSideRes.CommParas.CircuitType; // 单双回路类型

            paraCnt++;
            sheetCalsPro.Cells[paraCnt, 1].Value = backSideRes.CommParas.GrdIceForcePara == 1 ? "考虑+5mm" : "不考虑+5mm"; //地线覆冰张力计算模式
            sheetCalsPro.Cells[paraCnt, 3].Value = backSideRes.CommParas.GrdIceUnbaPara == 1 ? "考虑+5mm" : "不考虑+5mm"; //地线覆冰张力计算模式
            sheetCalsPro.Cells[paraCnt, 5].Value = backSideRes.CommParas.InsVerWeiPara; //安装垂直荷载取值
            sheetCalsPro.Cells[paraCnt, 7].Value = backSideRes.CommParas.UnbaIceVerWeiPara; //不均匀冰垂直荷载取值

            paraCnt++;
            sheetCalsPro.Cells[paraCnt, 1].Value = backSideRes.CommParas.BreakMaxPara; //断线max取值
            sheetCalsPro.Cells[paraCnt, 3].Value = backSideRes.CommParas.UnbaMaxPara;  //不均匀冰max取值
            sheetCalsPro.Cells[paraCnt, 5].Value = backSideRes.CommParas.GrdIceCheckPara == 1 ? "不考虑+5mm" : "考虑+5mm"; //地线验算张力取值
            sheetCalsPro.Cells[paraCnt, 7].Value = backSideRes.CommParas.HandForcePara == 1 ? " 两者大值" : (backSideRes.CommParas.HandForcePara == 2 ? "系数法" : "降温法"); //锚线张力取值

            paraCnt++;
            sheetCalsPro.Cells[paraCnt, 1].Value = TowerCals.AngelofApplication;
            var wea = frontSideRes.Weather.WeathComm.Where(item => item.Name == "最大风速").FirstOrDefault();
            sheetCalsPro.Cells[paraCnt, 3].Value = wea.WindSpeed;

            paraCnt++;
            sheetCalsPro.Cells[paraCnt, 1].Value = frontSideRes.IndWire.DevideNum; //大号导线分裂数
            sheetCalsPro.Cells[paraCnt, 3].Value = backSideRes.IndWire.DevideNum; //小号导线分裂数


            //更新垂直档距
            int count = 3;
            for (int i = 0; i < 5; i++)
            {
                var phase = TowerCals.PhaseTraList[i];
                var phaseAn = TowerCals.PhaseTraList[i + 5];
                List<string> wkList = phase.WireData.WorkCdtNamesStrain;

                for (int j = 0; j < wkList.Count; j++)
                {
                    sheetCalsPro.Cells[count, 0].Value = wkList[j];
                    sheetCalsPro.Cells[count, 1].Value = phase.LoadList[j].VetiSpanStr;
                    sheetCalsPro.Cells[count, 2].Value = phase.LoadList[j].VetiSpan;
                    sheetCalsPro.Cells[count, 3].Value = wkList[j];
                    sheetCalsPro.Cells[count, 4].Value = phaseAn.LoadList[j].VetiSpanStr;
                    sheetCalsPro.Cells[count, 5].Value = phaseAn.LoadList[j].VetiSpan;
                    count++;
                }
            }

            //更新水平荷载
            count += 2;
            for (int i = 0; i < 5; i++)
            {
                var phase = TowerCals.PhaseTraList[i];
                var phaseAn = TowerCals.PhaseTraList[i + 5];
                List<string> wkList = phase.WireData.WorkCdtNamesStrain;
                for (int j = 0; j < wkList.Count; j++)
                {
                    sheetCalsPro.Cells[count, 0].Value = wkList[j];
                    sheetCalsPro.Cells[count, 1].Value = phase.LoadList[j].HorForStr;
                    sheetCalsPro.Cells[count, 2].Value = phase.LoadList[j].HorFor;
                    sheetCalsPro.Cells[count, 3].Value = wkList[j];
                    sheetCalsPro.Cells[count, 4].Value = phaseAn.LoadList[j].HorForStr;
                    sheetCalsPro.Cells[count, 5].Value = phaseAn.LoadList[j].HorFor;
                    count++;
                }
            }

            //更新垂直荷载
            count += 2;
            for (int i = 0; i < 5; i++)
            {
                var phase = TowerCals.PhaseTraList[i];
                var phaseAn = TowerCals.PhaseTraList[i + 5];
                List<string> wkList = phase.WireData.WorkCdtNamesStrain;

                for (int j = 0; j < wkList.Count; j++)
                {
                    sheetCalsPro.Cells[count, 0].Value = wkList[j];
                    sheetCalsPro.Cells[count, 1].Value = phase.LoadList[j].VerWeiStr;
                    sheetCalsPro.Cells[count, 2].Value = phase.LoadList[j].VerWei;
                    sheetCalsPro.Cells[count, 3].Value = wkList[j];
                    sheetCalsPro.Cells[count, 4].Value = phaseAn.LoadList[j].VerWeiStr;
                    sheetCalsPro.Cells[count, 5].Value = phaseAn.LoadList[j].VerWei;
                    count++;
                }
            }

            //更新纵向荷载
            count += 1;
            for (int i = 0; i < 5; i++)
            {
                var phase = TowerCals.PhaseTraList[i];
                var phaseAn = TowerCals.PhaseTraList[i + 5];
                List<string> wkList = phase.WireData.WorkCdtNamesHang;

                for (int j = 0; j < wkList.Count; j++)
                {
                    sheetCalsPro.Cells[count, 0].Value = wkList[j];
                    sheetCalsPro.Cells[count, 1].Value = phase.LoadList[j].LoStrStr;
                    sheetCalsPro.Cells[count, 2].Value = phase.LoadList[j].LoStr;
                    sheetCalsPro.Cells[count, 3].Value = wkList[j];
                    sheetCalsPro.Cells[count, 4].Value = phaseAn.LoadList[j].LoStrStr;
                    sheetCalsPro.Cells[count, 5].Value = phaseAn.LoadList[j].LoStr;
                    sheetCalsPro.Cells[count, 6].Value = phase.LoadList[j].LoStrCheckStr;
                    sheetCalsPro.Cells[count, 7].Value = phaseAn.LoadList[j].LoStrCheckStr;

                    if (wkList[j] == "断线" || wkList[j] == "不均匀冰I" || wkList[j] == "不均匀冰II" || wkList[j] == "断线(导线+5mm)" || wkList[j] == "不均匀冰I(导线+5mm)" || wkList[j] == "不均匀冰II(导线+5mm)")
                    {
                        sheetCalsPro.Cells[count, 8].Value = phase.LoadList[j].LoStrCheck2;
                        sheetCalsPro.Cells[count, 9].Value = phaseAn.LoadList[j].LoStrCheck2;
                    }
                    count++;
                }
            }

            //张力差验算
            count++;
            sheetCalsPro.Cells[count, 1].Value = TowerCals.PhaseTraList[0].BreakTenDiffStr;
            sheetCalsPro.Cells[count, 2].Value = TowerCals.PhaseTraList[0].BreakTenDiff;
            sheetCalsPro.Cells[count, 4].Value = TowerCals.PhaseTraList[5].BreakTenDiffStr;
            sheetCalsPro.Cells[count, 5].Value = TowerCals.PhaseTraList[5].BreakTenDiff;
            sheetCalsPro.Cells[count, 6].Value = TowerCals.PhaseTraList[0].BreakTenMaxTemp;
            sheetCalsPro.Cells[count, 7].Value = TowerCals.PhaseTraList[5].BreakTenMaxTemp;
            sheetCalsPro.Cells[count, 8].Value = TowerCals.PhaseTraList[0].BreakTenMax;
            sheetCalsPro.Cells[count, 9].Value = TowerCals.PhaseTraList[5].BreakTenMax;

            count++;
            sheetCalsPro.Cells[count, 1].Value = TowerCals.PhaseTraList[3].BreakTenDiffStr;
            sheetCalsPro.Cells[count, 2].Value = TowerCals.PhaseTraList[3].BreakTenDiff;
            sheetCalsPro.Cells[count, 4].Value = TowerCals.PhaseTraList[8].BreakTenDiffStr;
            sheetCalsPro.Cells[count, 5].Value = TowerCals.PhaseTraList[8].BreakTenDiff;
            sheetCalsPro.Cells[count, 6].Value = TowerCals.PhaseTraList[3].BreakTenMaxTemp;
            sheetCalsPro.Cells[count, 7].Value = TowerCals.PhaseTraList[8].BreakTenMaxTemp;
            sheetCalsPro.Cells[count, 8].Value = TowerCals.PhaseTraList[3].BreakTenMax;
            sheetCalsPro.Cells[count, 9].Value = TowerCals.PhaseTraList[8].BreakTenMax;

            count++;
            sheetCalsPro.Cells[count, 1].Value = TowerCals.PhaseTraList[4].BreakTenDiffStr;
            sheetCalsPro.Cells[count, 2].Value = TowerCals.PhaseTraList[4].BreakTenDiff;
            sheetCalsPro.Cells[count, 4].Value = TowerCals.PhaseTraList[9].BreakTenDiffStr;
            sheetCalsPro.Cells[count, 5].Value = TowerCals.PhaseTraList[9].BreakTenDiff;
            sheetCalsPro.Cells[count, 6].Value = TowerCals.PhaseTraList[4].BreakTenMaxTemp;
            sheetCalsPro.Cells[count, 7].Value = TowerCals.PhaseTraList[9].BreakTenMaxTemp;
            sheetCalsPro.Cells[count, 8].Value = TowerCals.PhaseTraList[4].BreakTenMax;
            sheetCalsPro.Cells[count, 9].Value = TowerCals.PhaseTraList[9].BreakTenMax;

            count++;
            sheetCalsPro.Cells[count, 1].Value = TowerCals.PhaseTraList[0].UnbaIceTenDiffStr;
            sheetCalsPro.Cells[count, 2].Value = TowerCals.PhaseTraList[0].UnbaIceTenDiff;
            sheetCalsPro.Cells[count, 4].Value = TowerCals.PhaseTraList[5].UnbaIceTenDiffStr;
            sheetCalsPro.Cells[count, 5].Value = TowerCals.PhaseTraList[5].UnbaIceTenDiff;
            sheetCalsPro.Cells[count, 6].Value = TowerCals.PhaseTraList[0].UnbaIceTenMaxTemp;
            sheetCalsPro.Cells[count, 7].Value = TowerCals.PhaseTraList[5].UnbaIceTenMaxTemp;
            sheetCalsPro.Cells[count, 8].Value = TowerCals.PhaseTraList[0].UnbaIceTenMax;
            sheetCalsPro.Cells[count, 9].Value = TowerCals.PhaseTraList[5].UnbaIceTenMax;

            count++;
            sheetCalsPro.Cells[count, 1].Value = TowerCals.PhaseTraList[3].UnbaIceTenDiffStr;
            sheetCalsPro.Cells[count, 2].Value = TowerCals.PhaseTraList[3].UnbaIceTenDiff;
            sheetCalsPro.Cells[count, 4].Value = TowerCals.PhaseTraList[8].UnbaIceTenDiffStr;
            sheetCalsPro.Cells[count, 5].Value = TowerCals.PhaseTraList[8].UnbaIceTenDiff;
            sheetCalsPro.Cells[count, 6].Value = TowerCals.PhaseTraList[3].UnbaIceTenMaxTemp;
            sheetCalsPro.Cells[count, 7].Value = TowerCals.PhaseTraList[8].UnbaIceTenMaxTemp;
            sheetCalsPro.Cells[count, 8].Value = TowerCals.PhaseTraList[3].UnbaIceTenMax;
            sheetCalsPro.Cells[count, 9].Value = TowerCals.PhaseTraList[8].UnbaIceTenMax;

            count++;
            sheetCalsPro.Cells[count, 1].Value = TowerCals.PhaseTraList[4].UnbaIceTenDiffStr;
            sheetCalsPro.Cells[count, 2].Value = TowerCals.PhaseTraList[4].UnbaIceTenDiff;
            sheetCalsPro.Cells[count, 4].Value = TowerCals.PhaseTraList[9].UnbaIceTenDiffStr;
            sheetCalsPro.Cells[count, 5].Value = TowerCals.PhaseTraList[9].UnbaIceTenDiff;
            sheetCalsPro.Cells[count, 6].Value = TowerCals.PhaseTraList[4].UnbaIceTenMaxTemp;
            sheetCalsPro.Cells[count, 7].Value = TowerCals.PhaseTraList[9].UnbaIceTenMaxTemp;
            sheetCalsPro.Cells[count, 8].Value = TowerCals.PhaseTraList[4].UnbaIceTenMax;
            sheetCalsPro.Cells[count, 9].Value = TowerCals.PhaseTraList[9].UnbaIceTenMax;
            count += 2;

            sheetCalsPro.Cells[count, 1].Value = TowerCals.PhaseTraList[0].AnSideBreakTenDiff;
            sheetCalsPro.Cells[count, 2].Value = TowerCals.PhaseTraList[5].AnSideBreakTenDiff;
            sheetCalsPro.Cells[count, 3].Value = TowerCals.PhaseTraList[0].BothSidesBreakTenDiff;
            sheetCalsPro.Cells[count, 4].Value = TowerCals.PhaseTraList[5].BothSidesBreakTenDiff;
            count++;

            sheetCalsPro.Cells[count, 1].Value = TowerCals.PhaseTraList[3].AnSideBreakTenDiff;
            sheetCalsPro.Cells[count, 2].Value = TowerCals.PhaseTraList[8].AnSideBreakTenDiff;
            sheetCalsPro.Cells[count, 3].Value = TowerCals.PhaseTraList[3].BothSidesBreakTenDiff;
            sheetCalsPro.Cells[count, 4].Value = TowerCals.PhaseTraList[8].BothSidesBreakTenDiff;
            count++;

            sheetCalsPro.Cells[count, 1].Value = TowerCals.PhaseTraList[4].AnSideBreakTenDiff;
            sheetCalsPro.Cells[count, 2].Value = TowerCals.PhaseTraList[9].AnSideBreakTenDiff;
            sheetCalsPro.Cells[count, 3].Value = TowerCals.PhaseTraList[4].BothSidesBreakTenDiff;
            sheetCalsPro.Cells[count, 4].Value = TowerCals.PhaseTraList[9].BothSidesBreakTenDiff;
            count += 2;

            sheetCalsPro.Cells[count, 1].Value = TowerCals.PhaseTraList[0].AnSideBreakTenDiffCheck;
            sheetCalsPro.Cells[count, 2].Value = TowerCals.PhaseTraList[5].AnSideBreakTenDiffCheck;
            sheetCalsPro.Cells[count, 3].Value = TowerCals.PhaseTraList[0].BothSidesBreakTenDiffCheckTemp;
            sheetCalsPro.Cells[count, 4].Value = TowerCals.PhaseTraList[5].BothSidesBreakTenDiffCheckTemp;
            sheetCalsPro.Cells[count, 5].Value = TowerCals.PhaseTraList[0].BothSidesBreakTenDiffCheck;
            sheetCalsPro.Cells[count, 6].Value = TowerCals.PhaseTraList[5].BothSidesBreakTenDiffCheck;
            count++;

            sheetCalsPro.Cells[count, 1].Value = TowerCals.PhaseTraList[3].AnSideBreakTenDiffCheck;
            sheetCalsPro.Cells[count, 2].Value = TowerCals.PhaseTraList[8].AnSideBreakTenDiffCheck;
            sheetCalsPro.Cells[count, 3].Value = TowerCals.PhaseTraList[3].BothSidesBreakTenDiffCheckTemp;
            sheetCalsPro.Cells[count, 4].Value = TowerCals.PhaseTraList[8].BothSidesBreakTenDiffCheckTemp;
            sheetCalsPro.Cells[count, 5].Value = TowerCals.PhaseTraList[3].BothSidesBreakTenDiffCheck;
            sheetCalsPro.Cells[count, 6].Value = TowerCals.PhaseTraList[8].BothSidesBreakTenDiffCheck;
            count++;

            sheetCalsPro.Cells[count, 1].Value = TowerCals.PhaseTraList[4].AnSideBreakTenDiffCheck;
            sheetCalsPro.Cells[count, 2].Value = TowerCals.PhaseTraList[9].AnSideBreakTenDiffCheck;
            sheetCalsPro.Cells[count, 3].Value = TowerCals.PhaseTraList[4].BothSidesBreakTenDiffCheckTemp;
            sheetCalsPro.Cells[count, 4].Value = TowerCals.PhaseTraList[9].BothSidesBreakTenDiffCheckTemp;
            sheetCalsPro.Cells[count, 5].Value = TowerCals.PhaseTraList[4].BothSidesBreakTenDiffCheck;
            sheetCalsPro.Cells[count, 6].Value = TowerCals.PhaseTraList[9].BothSidesBreakTenDiffCheck;
            count += 2;

            sheetCalsPro.Cells[count, 2].Value = TowerCals.PhaseTraList[0].UnbaIceTenIDiffBothSids;
            sheetCalsPro.Cells[count, 3].Value = TowerCals.PhaseTraList[0].UnbaIceTenIDiff;
            sheetCalsPro.Cells[count, 4].Value = TowerCals.PhaseTraList[5].UnbaIceTenIDiff;
            count++;

            sheetCalsPro.Cells[count, 2].Value = TowerCals.PhaseTraList[0].UnbaIceTenIIDiffBothSids;
            sheetCalsPro.Cells[count, 3].Value = TowerCals.PhaseTraList[0].UnbaIceTenIIDiff;
            sheetCalsPro.Cells[count, 4].Value = TowerCals.PhaseTraList[5].UnbaIceTenIIDiff;
            count++;

            sheetCalsPro.Cells[count, 2].Value = TowerCals.PhaseTraList[3].UnbaIceTenIDiffBothSids;
            sheetCalsPro.Cells[count, 3].Value = TowerCals.PhaseTraList[3].UnbaIceTenIDiff;
            sheetCalsPro.Cells[count, 4].Value = TowerCals.PhaseTraList[8].UnbaIceTenIDiff;
            count++;

            sheetCalsPro.Cells[count, 2].Value = TowerCals.PhaseTraList[3].UnbaIceTenIIDiffBothSids;
            sheetCalsPro.Cells[count, 3].Value = TowerCals.PhaseTraList[3].UnbaIceTenIIDiff;
            sheetCalsPro.Cells[count, 4].Value = TowerCals.PhaseTraList[8].UnbaIceTenIIDiff;
            count++;

            sheetCalsPro.Cells[count, 2].Value = TowerCals.PhaseTraList[4].UnbaIceTenIDiffBothSids;
            sheetCalsPro.Cells[count, 3].Value = TowerCals.PhaseTraList[4].UnbaIceTenIDiff;
            sheetCalsPro.Cells[count, 4].Value = TowerCals.PhaseTraList[9].UnbaIceTenIDiff;
            count++;

            sheetCalsPro.Cells[count, 2].Value = TowerCals.PhaseTraList[4].UnbaIceTenIIDiffBothSids;
            sheetCalsPro.Cells[count, 3].Value = TowerCals.PhaseTraList[4].UnbaIceTenIIDiff;
            sheetCalsPro.Cells[count, 4].Value = TowerCals.PhaseTraList[9].UnbaIceTenIIDiff;
            count += 2;

            sheetCalsPro.Cells[count, 2].Value = TowerCals.PhaseTraList[0].UnbaIceTenIDiffBothSidsCheck;
            sheetCalsPro.Cells[count, 3].Value = TowerCals.PhaseTraList[0].UnbaIceTenIDiffCheckTemp;
            sheetCalsPro.Cells[count, 4].Value = TowerCals.PhaseTraList[5].UnbaIceTenIDiffCheckTemp;
            sheetCalsPro.Cells[count, 5].Value = TowerCals.PhaseTraList[0].UnbaIceTenIDiffCheck;
            sheetCalsPro.Cells[count, 6].Value = TowerCals.PhaseTraList[5].UnbaIceTenIDiffCheck;
            count++;

            sheetCalsPro.Cells[count, 2].Value = TowerCals.PhaseTraList[0].UnbaIceTenIIDiffBothSidsCheck;
            sheetCalsPro.Cells[count, 3].Value = TowerCals.PhaseTraList[0].UnbaIceTenIIDiffCheckTemp;
            sheetCalsPro.Cells[count, 4].Value = TowerCals.PhaseTraList[5].UnbaIceTenIIDiffCheckTemp;
            sheetCalsPro.Cells[count, 5].Value = TowerCals.PhaseTraList[0].UnbaIceTenIIDiffCheck;
            sheetCalsPro.Cells[count, 6].Value = TowerCals.PhaseTraList[5].UnbaIceTenIIDiffCheck;
            count++;

            sheetCalsPro.Cells[count, 2].Value = TowerCals.PhaseTraList[3].UnbaIceTenIDiffBothSidsCheck;
            sheetCalsPro.Cells[count, 3].Value = TowerCals.PhaseTraList[3].UnbaIceTenIDiffCheckTemp;
            sheetCalsPro.Cells[count, 4].Value = TowerCals.PhaseTraList[8].UnbaIceTenIDiffCheckTemp;
            sheetCalsPro.Cells[count, 5].Value = TowerCals.PhaseTraList[3].UnbaIceTenIDiffCheck;
            sheetCalsPro.Cells[count, 6].Value = TowerCals.PhaseTraList[8].UnbaIceTenIDiffCheck;
            count++;

            sheetCalsPro.Cells[count, 2].Value = TowerCals.PhaseTraList[3].UnbaIceTenIIDiffBothSidsCheck;
            sheetCalsPro.Cells[count, 3].Value = TowerCals.PhaseTraList[3].UnbaIceTenIIDiffCheckTemp;
            sheetCalsPro.Cells[count, 4].Value = TowerCals.PhaseTraList[8].UnbaIceTenIIDiffCheckTemp;
            sheetCalsPro.Cells[count, 5].Value = TowerCals.PhaseTraList[3].UnbaIceTenIIDiffCheck;
            sheetCalsPro.Cells[count, 6].Value = TowerCals.PhaseTraList[8].UnbaIceTenIIDiffCheck;
            count++;

            sheetCalsPro.Cells[count, 2].Value = TowerCals.PhaseTraList[4].UnbaIceTenIDiffBothSidsCheck;
            sheetCalsPro.Cells[count, 3].Value = TowerCals.PhaseTraList[4].UnbaIceTenIDiffCheckTemp;
            sheetCalsPro.Cells[count, 4].Value = TowerCals.PhaseTraList[9].UnbaIceTenIDiffCheckTemp;
            sheetCalsPro.Cells[count, 5].Value = TowerCals.PhaseTraList[4].UnbaIceTenIDiffCheck;
            sheetCalsPro.Cells[count, 6].Value = TowerCals.PhaseTraList[9].UnbaIceTenIDiffCheck;
            count++;

            sheetCalsPro.Cells[count, 2].Value = TowerCals.PhaseTraList[4].UnbaIceTenIIDiffBothSidsCheck;
            sheetCalsPro.Cells[count, 3].Value = TowerCals.PhaseTraList[4].UnbaIceTenIIDiffCheckTemp;
            sheetCalsPro.Cells[count, 4].Value = TowerCals.PhaseTraList[9].UnbaIceTenIIDiffCheckTemp;
            sheetCalsPro.Cells[count, 5].Value = TowerCals.PhaseTraList[4].UnbaIceTenIIDiffCheck;
            sheetCalsPro.Cells[count, 6].Value = TowerCals.PhaseTraList[9].UnbaIceTenIIDiffCheck;
            count += 2;

            for (int i = 0; i < 5; i++)
            {
                var phase = TowerCals.PhaseTraList[i];
                var phaseAn = TowerCals.PhaseTraList[i + 5];
                List<string> wkList = phase.WireData.WorkCdtNamesHang;

                if (phase.WireData.bGrd == 0)
                {
                    for (int j = 0; j < wkList.Count; j++)
                    {
                        sheetCalsPro.Cells[count, 0].Value = wkList[j];
                        sheetCalsPro.Cells[count, 1].Value = phase.LoadList[j].JumpHorForStr;
                        sheetCalsPro.Cells[count, 2].Value = phase.LoadList[j].JumpHorFor;
                        sheetCalsPro.Cells[count, 4].Value = wkList[j];
                        sheetCalsPro.Cells[count, 5].Value = phase.LoadList[j].JumpVerWeiStr;
                        sheetCalsPro.Cells[count, 7].Value = phase.LoadList[j].JumpVerWei;
                        count++;
                    }
                }
            }
        }

        protected void Caculate()
        {
            sheetCalsPro.Calculate();
            sheetLoadConv.Calculate();
            sheetElecLoad.Calculate();
        }

        public StruCalsElecLoad ProvideMeterial(TowerTemplate template)
        {
            List<ElecCalsWorkConditionBase> wkCdtList = new List<ElecCalsWorkConditionBase>();
            List<WireElecLoadLine> lineLoads = new List<WireElecLoadLine>();
            List<WireElecLoadCorner> cornerLoads = new List<WireElecLoadCorner>();
            List<StruCalsTension> tensions = new List<StruCalsTension>();

            //电气荷载模板中工况的个数
            int elecExcelWkCdtNum = TowerCals.TowerType == "悬垂塔" ? 11 : 12;

            //结构模板中工况在电气模板中的位置
            Dictionary<int, int> posDic = new Dictionary<int, int>();

            //获取工况
            for (int i = 1; i <= template.WorkConditongs.Count; i++)
            {
                //通过
                string struWkCdtName = StruWorkConditionNameConvertElec(template.WorkConditongs[i]);

                //查找工况所在的列数
                int j;
                for(j = 1; j <= elecExcelWkCdtNum; j++)
                {
                    if (struWkCdtName == sheetElecLoad.Cells[0, j].Value.ToString())
                        break;
                }
                if (j > elecExcelWkCdtNum)
                {
                    //没有找到对应的工况
                    wkCdtList.Add(new ElecCalsWorkConditionBase()
                    {
                        Id = i,
                        Name = template.WorkConditongs[i],
                    });
                    posDic[i] = 0;
                }
                else
                {
                    wkCdtList.Add(new ElecCalsWorkConditionBase()
                    {
                        Id = i,
                        Name = template.WorkConditongs[i],
                        Temperature = (double)(sheetElecLoad.Cells[1, j].Value.ToObject()),
                        WindSpeed = (double)(sheetElecLoad.Cells[2, j].Value.ToObject()),
                        IceThickness = (double)(sheetElecLoad.Cells[3, j].Value.ToObject()),
                    });
                    posDic[i] = j;
                }
            }

            for(int i = 0; i < template.Wires.Count; i++)
            {
                int elecWireNum = (((TowerHangElecCals)TowerCals).SideRes.CommParas.CircuitType == 1) ? 5 : 8;

                if (TowerCals.TowerType == "悬垂塔")
                {
                    //查到在电气荷载Excel中导地线所在的行数
                    int j = 0;
                    for (j = 0; j < elecWireNum; j++)
                    {
                        if (sheetElecLoad.Cells[4 + j * 6, 0].Value.ToString() == template.Wires[i])
                            break;
                    }

                    int rowPos = j >= elecWireNum ? 0 : 4 + j * 6;
                    List<ElecLoadLine> elecLoad = new List<ElecLoadLine>();

                    for (int k = 1; k <= template.WorkConditongs.Count; k++)
                    {
                        int colPos = posDic[k];

                        ElecLoadLine load = new ElecLoadLine()
                        {
                            WorkConditionId = k,
                            WorkConditionName = template.WorkConditongs[k],
                        };


                        if (rowPos!=0 && colPos != 0)
                        {
                            load.Wind = (float)sheetElecLoad.Cells[rowPos + 1, colPos].Value.ToObject();
                            load.GMax = (float)sheetElecLoad.Cells[rowPos + 2, colPos].Value.ToObject();
                            load.GMin = (float)sheetElecLoad.Cells[rowPos + 3, colPos].Value.ToObject();
                            load.TensionMax = (float)sheetElecLoad.Cells[rowPos + 4, colPos].Value.ToObject();
                            load.TensionMin = (float)sheetElecLoad.Cells[rowPos + 5, colPos].Value.ToObject();
                        }

                        elecLoad.Add(load);
                    }

                    lineLoads.Add(new WireElecLoadLine()
                    {
                        WireType = template.Wires[i],
                        ElecLoad = elecLoad,
                    });


                    StruCalsTension tension = new StruCalsTension()
                    {
                        WireType = template.Wires[i]
                    };

                    if(rowPos != 0)
                    {
                        tension.AnchorTen = (float)sheetElecLoad.Cells[rowPos, 12].Value.ToObject();
                        tension.PulleyTen = (float)sheetElecLoad.Cells[rowPos, 13].Value.ToObject();
                    }
                    tensions.Add(tension);
                }
                else
                {
                    //查到在电气荷载Excel中导地线所在的行数
                    int j = 0, rowPos = 4;
                    for (j = 0; j < elecWireNum; j++)
                    {
                        if (j < 2)
                            rowPos = 4 + 11 * j;
                        else if(j >= 2)
                            rowPos = 4 + 2 * 11 + (j - 2) * 13;

                        if (sheetElecLoad.Cells[rowPos, 0].Value.ToString() == template.Wires[i])
                            break;
                    }

                    if (j >= elecWireNum)
                        rowPos = 0;
                    List<ElecLoadCorner> elecLoad = new List<ElecLoadCorner>();

                    for (int k = 1; k <= template.WorkConditongs.Count; k++)
                    {
                        int colPos = posDic[k];

                        ElecLoadCorner load = new ElecLoadCorner()
                        {
                            WorkConditionId = k,
                            WorkConditionName = template.WorkConditongs[k],
                        };

                        if(rowPos != 0 && colPos != 0)
                        {
                            load.WindDF= (float)sheetElecLoad.Cells[rowPos + 1, k].Value.ToObject();
                            load.WindDB = (float)sheetElecLoad.Cells[rowPos + 2, k].Value.ToObject();
                            load.WindXF = (float)sheetElecLoad.Cells[rowPos + 3, k].Value.ToObject();
                            load.WindXB = (float)sheetElecLoad.Cells[rowPos + 4, k].Value.ToObject();
                            load.GMaxF = (float)sheetElecLoad.Cells[rowPos + 5, k].Value.ToObject();
                            load.GMaxB = (float)sheetElecLoad.Cells[rowPos + 6, k].Value.ToObject();
                            load.GMinF = (float)sheetElecLoad.Cells[rowPos + 7, k].Value.ToObject();
                            load.GMinB = (float)sheetElecLoad.Cells[rowPos + 8, k].Value.ToObject();
                            load.TensionD = (float)sheetElecLoad.Cells[rowPos + 9, k].Value.ToObject();
                            load.TensionX = (float)sheetElecLoad.Cells[rowPos + 10, k].Value.ToObject();
                            if (template.Wires.Contains("导"))
                            {
                                load.WindTX = (float)sheetElecLoad.Cells[rowPos + 11, k].Value.ToObject();
                                load.GTX = (float)sheetElecLoad.Cells[rowPos + 12, k].Value.ToObject();
                            }
                        }
                        elecLoad.Add(load);

                        StruCalsTension tension = new StruCalsTension()
                        {
                            WireType = template.Wires[i]
                        };

                        if (rowPos != 0)
                        {
                            tension.AnchorTen = (float)sheetElecLoad.Cells[rowPos, 13].Value.ToObject();
                            tension.TemporaryTen = (float)sheetElecLoad.Cells[rowPos, 14].Value.ToObject();
                            tension.AngleWinTenDF = (float)sheetElecLoad.Cells[rowPos+1, 16].Value.ToObject();
                            tension.AngleWinTenDB = (float)sheetElecLoad.Cells[rowPos+2, 16].Value.ToObject();
                            tension.AngleWinTenXF = (float)sheetElecLoad.Cells[rowPos+3, 16].Value.ToObject();
                            tension.AngleWinTenXB = (float)sheetElecLoad.Cells[rowPos+4, 16].Value.ToObject();
                        }
                        tensions.Add(tension);

                    }

                    cornerLoads.Add(new WireElecLoadCorner()
                    {
                        WireType = template.Wires[i],
                        ElecLoad = elecLoad,
                    });
                }
            }

            return new StruCalsElecLoad() {
                WorkCondition = wkCdtList,
                LineElecLoads = lineLoads,
                LineCornerElecLoads = new List<WireElecLoadLineCorner>(),
                CornerElecLoads = cornerLoads,
                ExtraLoad = tensions,
            };
        }
        
        protected string StruWorkConditionNameConvertElec(string elecWkCdtName)
        {
            switch(elecWkCdtName)
            {
                case "大风":
                    return "最大风速";
                case "覆冰":
                    return "覆冰";
                case "最低气温":
                    return "最低气温";
                case "安装":
                    return "安装情况";
                case "未断线":
                    return "未断线";
                case "断线":
                    return "断线";
                case "不均匀冰I":
                    return "不均匀冰I";
                case "不均匀冰II":
                    return "不均匀冰II";
                case "验算冰":
                    return "验算冰";
                case "验算不均匀冰I":
                    return "验算不均匀冰I";
                case "验算不均匀冰II":
                    return "验算不均匀冰II";
                case "不均匀风":
                    return "不均匀风";

            }
            return "";
        }

    }
}
