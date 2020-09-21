using DevExpress.Spreadsheet;
using DevExpress.Xpf.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TowerLoadCals.BLL.Electric;
using TowerLoadCals.Mode;
using TowerLoadCals.ModulesViewModels.Electrical;
using TowerLoadCals.ModulesViewModels.TowerSequence;

namespace TowerLoadCals.Modules.Electrical
{
    /// <summary>
    /// ElecCalsResultModule.xaml 的交互逻辑
    /// </summary>
    public partial class ElecCalsResultModule : UserControl
    {
        protected Worksheet sheetPara;
        protected Worksheet sheetResult;
        protected Worksheet sheetProMater;
        protected Worksheet sheetElecLoad;

        protected TowerElecCals tower;

        public ElecCalsResultModule()
        {
            InitializeComponent();
            DataContext = new ElecCalsResultViewModel(this);

            IWorkbook workbook = elecCalsRstSC.Document;

            string path = "D:\\00-项目\\Test1.xlsx";
            workbook.LoadDocument(path, DocumentFormat.Xlsx);
            sheetPara = workbook.Worksheets[0];
            sheetResult = workbook.Worksheets[1];
            sheetProMater = workbook.Worksheets[2];
            sheetElecLoad = workbook.Worksheets[3];

            //InitSpreadSheetTitle();
        }

        public void LoadTowerResult(TowerElecCals calsTower)
        {
            tower = calsTower;
            if (calsTower.TowerType == "悬垂塔")
            {
                LoadHangTowerResult();
            }
            else
            {
                LoadStrainTowerResult();
            }
        }
        
        protected void LoadHangTowerResult()
        {
            //更新参数
            int paraCnt = 1;

            ElecCalsRes sideRes = ((TowerHangElecCals)tower).SideRes;
            sheetResult.Cells[paraCnt, 1].Value = sideRes.CommParas.VoltStr;  //电压等级
            sheetResult.Cells[paraCnt, 3].Value = sideRes.CommParas.CircuitType; // 单双回路类型

            paraCnt++;
            sheetResult.Cells[paraCnt, 1].Value = sideRes.CommParas.bGrd1Break;  //地线1是否开断
            sheetResult.Cells[paraCnt, 3].Value = sideRes.CommParas.bGrd2Break;  //地线2是否开断
            sheetResult.Cells[paraCnt, 5].Value = sideRes.CommParas.GrdIceForcePara == 1 ? "考虑+5mm" : "不考虑+5mm"; //地线覆冰张力计算模式

            paraCnt++;
            sheetResult.Cells[paraCnt, 1].Value = sideRes.CommParas.BreakMaxPara ; //断线max取值
            sheetResult.Cells[paraCnt, 3].Value = sideRes.CommParas.UnbaMaxPara;  //不均匀冰max取值
            sheetResult.Cells[paraCnt, 5].Value = sideRes.CommParas.InsVerWeiPara; //安装垂直荷载取值
            sheetResult.Cells[paraCnt, 7].Value = sideRes.CommParas.UnbaIceVerWeiPara; //不均匀冰垂直荷载取值

            paraCnt++;
            sheetResult.Cells[paraCnt, 1].Value = sideRes.CommParas.HandForcePara == 1 ? " 两者大值" : (sideRes.CommParas.HandForcePara == 2 ?  "系数法" : "降温法"); //锚线张力取值
            sheetResult.Cells[paraCnt, 5].Value = sideRes.CommParas.GrdIceCheckPara == 1 ?"不考虑+5mm" : "考虑+5mm"; //地线验算张力取值

            paraCnt++;
            sheetResult.Cells[paraCnt, 1].Value = tower.AngelofApplication;
            var wea = sideRes.Weather.WeathComm.Where(item => item.Name == "最大风速").FirstOrDefault();
            sheetResult.Cells[paraCnt, 3].Value = wea.WindSpeed;
            sheetResult.Cells[paraCnt, 5].Value = sideRes.CommParas.IndInstallTenDiff; //导线安装张力差

            paraCnt++;
            sheetResult.Cells[paraCnt, 1].Value = sideRes.TowerParas.IndUpliftForce; //导线上拔力
            sheetResult.Cells[paraCnt, 3].Value = sideRes.TowerParas.Grd1UpliftForce; //一根导线上拔力
            sheetResult.Cells[paraCnt, 5].Value = sideRes.TowerParas.Grd2pliftForce; //另一根导线上拔力
            

            //更新垂直档距
            int count = 3;
            for(int i = 0; i < 5; i++)
            {
                var phase = tower.PhaseTraList[i];
                var phaseAn = tower.PhaseTraList[i+5];
                List<string> wkList = phase.WireData.WorkCdtNamesHang;

                for(int j = 0; j < wkList.Count; j++)
                {
                    sheetResult.Cells[count, 0].Value = wkList[j];
                    sheetResult.Cells[count, 1].Value = phase.LoadList[j].VetiSpanStr;
                    sheetResult.Cells[count, 2].Value = phase.LoadList[j].VetiSpan;
                    sheetResult.Cells[count, 3].Value = wkList[j];
                    sheetResult.Cells[count, 4].Value = phaseAn.LoadList[j].VetiSpanStr;
                    sheetResult.Cells[count, 5].Value = phaseAn.LoadList[j].VetiSpan;
                    count++;
                }
            }

            //更新水平荷载
            count += 2;
            for (int i = 0; i < 5; i++)
            {
                var phase = tower.PhaseTraList[i];
                List<string> wkList = phase.WireData.WorkCdtNamesHang;
                for (int j = 0; j < wkList.Count; j++)
                {
                    sheetResult.Cells[count, 0].Value = wkList[j];
                    sheetResult.Cells[count, 1].Value = phase.LoadList[j].HorForStr;
                    sheetResult.Cells[count, 2].Value = phase.LoadList[j].HorFor;
                    sheetResult.Cells[count, 3].Value = wkList[j];
                    sheetResult.Cells[count, 4].Value = phase.StrLoad[wkList[j]].WindLoadStr;
                    sheetResult.Cells[count, 5].Value = phase.StrLoad[wkList[j]].WindLoad;
                    count++;
                }
            }

            //更新垂直荷载
            count += 2;
            for (int i = 0; i < 5; i++)
            {
                var phase = tower.PhaseTraList[i];
                List<string> wkList = phase.WireData.WorkCdtNamesHang;

                for (int j = 0; j < wkList.Count; j++)
                {
                    sheetResult.Cells[count, 0].Value = wkList[j];
                    sheetResult.Cells[count, 1].Value = phase.LoadList[j].VerWeiStr;
                    sheetResult.Cells[count, 2].Value = phase.LoadList[j].VerWei;
                    sheetResult.Cells[count, 3].Value = wkList[j];
                    sheetResult.Cells[count, 4].Value = phase.StrLoad[wkList[j]].VerLoadStr;
                    sheetResult.Cells[count, 5].Value = phase.StrLoad[wkList[j]].VerLoad;
                    count++;
                }
            }

            //更新纵向荷载
            count += 1;
            for (int i = 0; i < 5; i++)
            {
                var phase = tower.PhaseTraList[i];
                var phaseAn = tower.PhaseTraList[i + 5];
                List<string> wkList = phase.WireData.WorkCdtNamesHang;

                for (int j = 0; j < wkList.Count; j++)
                {
                    sheetResult.Cells[count, 0].Value = wkList[j];
                    sheetResult.Cells[count, 1].Value = phase.LoadList[j].LoStrStr;
                    sheetResult.Cells[count, 2].Value = phase.LoadList[j].LoStr;
                    sheetResult.Cells[count, 3].Value = wkList[j];
                    sheetResult.Cells[count, 4].Value = phaseAn.LoadList[j].LoStrStr;
                    sheetResult.Cells[count, 5].Value = phaseAn.LoadList[j].LoStr;
                    sheetResult.Cells[count, 6].Value = phaseAn.LoadList[j].LoStrStr;
                    sheetResult.Cells[count, 7].Value = phaseAn.LoadList[j].LoStr;
                    sheetResult.Cells[count, 8].Value = phaseAn.LoadList[j].LoStrStr;
                    sheetResult.Cells[count, 9].Value = phaseAn.LoadList[j].LoStr;
                    count++;
                }
            }

            //断线
            sheetResult.Cells[count, 1].Value = tower.PhaseTraList[0].BreakTenDiffStr;
            sheetResult.Cells[count, 2].Value = tower.PhaseTraList[0].BreakTenDiff;
            sheetResult.Cells[count, 7].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[0]).TenBreakMax;
            sheetResult.Cells[count, 8].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[0]).TenBreakIceCov100;
            sheetResult.Cells[count, 9].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[5]).TenBreakIceCov100;
            count++;

            sheetResult.Cells[count, 1].Value = tower.PhaseTraList[3].BreakTenDiffStr;
            sheetResult.Cells[count, 2].Value = tower.PhaseTraList[3].BreakTenDiff;
            sheetResult.Cells[count, 4].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[3]).BreakTenDiffGrdBreStr;
            sheetResult.Cells[count, 5].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[3]).BreakTenDiffGrdBre;
            sheetResult.Cells[count, 7].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[3]).TenBreakMax;
            sheetResult.Cells[count, 8].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[3]).TenBreakIceCov100;
            sheetResult.Cells[count, 9].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[8]).TenBreakIceCov100;
            count++;

            sheetResult.Cells[count, 1].Value = tower.PhaseTraList[4].BreakTenDiffStr;
            sheetResult.Cells[count, 2].Value = tower.PhaseTraList[4].BreakTenDiff;
            sheetResult.Cells[count, 4].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[4]).BreakTenDiffGrdBreStr;
            sheetResult.Cells[count, 5].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[4]).BreakTenDiffGrdBre;
            sheetResult.Cells[count, 7].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[4]).TenBreakMax;
            sheetResult.Cells[count, 8].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[4]).TenBreakIceCov100;
            sheetResult.Cells[count, 9].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[9]).TenBreakIceCov100;
            count += 2;

            sheetResult.Cells[count, 1].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[0]).TenBreakMaxTensionS;
            sheetResult.Cells[count, 2].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[0]).TenBreakMaxTensionL;
            sheetResult.Cells[count, 4].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[0]).TenBreakMaxIceCov100S;
            sheetResult.Cells[count, 5].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[0]).TenBreakMaxIceCov100L;
            sheetResult.Cells[count, 7].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[0]).BreakTenMaxS;
            sheetResult.Cells[count, 8].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[0]).BreakTenMaxL;
            count++;

            sheetResult.Cells[count, 1].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[5]).TenBreakMaxTensionS;
            sheetResult.Cells[count, 2].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[5]).TenBreakMaxTensionL;
            sheetResult.Cells[count, 4].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[5]).TenBreakMaxIceCov100S;
            sheetResult.Cells[count, 5].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[5]).TenBreakMaxIceCov100L;
            sheetResult.Cells[count, 7].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[5]).BreakTenMaxS;
            sheetResult.Cells[count, 8].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[5]).BreakTenMaxL;
            count++;

            sheetResult.Cells[count, 1].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[3]).TenBreakMaxTensionS;
            sheetResult.Cells[count, 2].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[3]).TenBreakMaxTensionL;
            sheetResult.Cells[count, 4].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[3]).TenBreakMaxIceCov100S;
            sheetResult.Cells[count, 5].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[3]).TenBreakMaxIceCov100L;
            sheetResult.Cells[count, 7].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[3]).BreakTenMaxS;
            sheetResult.Cells[count, 8].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[3]).BreakTenMaxL;
            count++;

            sheetResult.Cells[count, 1].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[8]).TenBreakMaxTensionS;
            sheetResult.Cells[count, 2].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[8]).TenBreakMaxTensionL;
            sheetResult.Cells[count, 4].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[8]).TenBreakMaxIceCov100S;
            sheetResult.Cells[count, 5].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[8]).TenBreakMaxIceCov100L;
            sheetResult.Cells[count, 7].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[8]).BreakTenMaxS;
            sheetResult.Cells[count, 8].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[8]).BreakTenMaxL;
            count++;

            sheetResult.Cells[count, 1].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[4]).TenBreakMaxTensionS;
            sheetResult.Cells[count, 2].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[4]).TenBreakMaxTensionL;
            sheetResult.Cells[count, 4].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[4]).TenBreakMaxIceCov100S;
            sheetResult.Cells[count, 5].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[4]).TenBreakMaxIceCov100L;
            sheetResult.Cells[count, 7].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[4]).BreakTenMaxS;
            sheetResult.Cells[count, 8].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[4]).BreakTenMaxL;
            count++;

            sheetResult.Cells[count, 1].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[9]).TenBreakMaxTensionS;
            sheetResult.Cells[count, 2].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[9]).TenBreakMaxTensionL;
            sheetResult.Cells[count, 4].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[9]).TenBreakMaxIceCov100S;
            sheetResult.Cells[count, 5].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[9]).TenBreakMaxIceCov100L;
            sheetResult.Cells[count, 7].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[9]).BreakTenMaxS;
            sheetResult.Cells[count, 8].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[9]).BreakTenMaxL;
            count++;

            sheetResult.Cells[count, 1].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[9]).TenBreakMaxTensionS;
            sheetResult.Cells[count, 2].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[9]).TenBreakMaxTensionL;
            sheetResult.Cells[count, 4].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[9]).TenBreakMaxIceCov100S;
            sheetResult.Cells[count, 5].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[9]).TenBreakMaxIceCov100L;
            sheetResult.Cells[count, 7].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[9]).BreakTenMaxS;
            sheetResult.Cells[count, 8].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[9]).BreakTenMaxL;
            count++;

            sheetResult.Cells[count, 7].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[0]).TenBreakS;
            sheetResult.Cells[count, 8].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[0]).TenBreakL;
            count++;

            sheetResult.Cells[count, 7].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[5]).TenBreakS;
            sheetResult.Cells[count, 8].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[5]).TenBreakL;
            count++;

            sheetResult.Cells[count, 7].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[3]).TenBreakS;
            sheetResult.Cells[count, 8].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[3]).TenBreakL;
            count++;

            sheetResult.Cells[count, 7].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[8]).TenBreakS;
            sheetResult.Cells[count, 8].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[8]).TenBreakL;
            count++;

            sheetResult.Cells[count, 7].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[4]).TenBreakS;
            sheetResult.Cells[count, 8].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[4]).TenBreakL;
            count++;

            sheetResult.Cells[count, 7].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[9]).TenBreakS;
            sheetResult.Cells[count, 8].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[9]).TenBreakL;
            count+=2;

            sheetResult.Cells[count, 1].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[3]).TenBreakMaxTensionBreakS;
            sheetResult.Cells[count, 2].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[3]).TenBreakMaxTensionBreakL;
            sheetResult.Cells[count, 3].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[3]).TenBreakMaxIceCov100BreakS;
            sheetResult.Cells[count, 4].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[3]).TenBreakMaxIceCov100BreakL;
            sheetResult.Cells[count, 5].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[3]).BreakTenMaxBreakS;
            sheetResult.Cells[count, 6].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[3]).BreakTenMaxBreakL;
            count++;

            sheetResult.Cells[count, 1].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[8]).TenBreakMaxTensionBreakS;
            sheetResult.Cells[count, 2].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[8]).TenBreakMaxTensionBreakL;
            sheetResult.Cells[count, 3].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[8]).TenBreakMaxIceCov100BreakS;
            sheetResult.Cells[count, 4].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[8]).TenBreakMaxIceCov100BreakL;
            sheetResult.Cells[count, 5].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[8]).BreakTenMaxBreakS;
            sheetResult.Cells[count, 6].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[8]).BreakTenMaxBreakL;
            count++;

            sheetResult.Cells[count, 1].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[4]).TenBreakMaxTensionBreakS;
            sheetResult.Cells[count, 2].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[4]).TenBreakMaxTensionBreakL;
            sheetResult.Cells[count, 3].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[4]).TenBreakMaxIceCov100BreakS;
            sheetResult.Cells[count, 4].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[4]).TenBreakMaxIceCov100BreakL;
            sheetResult.Cells[count, 5].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[4]).BreakTenMaxBreakS;
            sheetResult.Cells[count, 6].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[4]).BreakTenMaxBreakL;
            count++;

            sheetResult.Cells[count, 1].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[9]).TenBreakMaxTensionBreakS;
            sheetResult.Cells[count, 2].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[9]).TenBreakMaxTensionBreakL;
            sheetResult.Cells[count, 3].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[9]).TenBreakMaxIceCov100BreakS;
            sheetResult.Cells[count, 4].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[9]).TenBreakMaxIceCov100BreakL;
            sheetResult.Cells[count, 5].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[9]).BreakTenMaxBreakS;
            sheetResult.Cells[count, 6].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[9]).BreakTenMaxBreakL;
            count++;

            sheetResult.Cells[count, 1].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[3]).BreakTenAdd5mmBreakS;
            sheetResult.Cells[count, 2].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[3]).BreakTenAdd5mmBreakL;
            sheetResult.Cells[count, 3].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[3]).BreakTenBreakS;
            sheetResult.Cells[count, 4].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[3]).BreakTenBreakL;
            count++;

            sheetResult.Cells[count, 1].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[8]).BreakTenAdd5mmBreakS;
            sheetResult.Cells[count, 2].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[8]).BreakTenAdd5mmBreakL;
            sheetResult.Cells[count, 3].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[8]).BreakTenBreakS;
            sheetResult.Cells[count, 4].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[8]).BreakTenBreakL;
            count++;

            sheetResult.Cells[count, 1].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[4]).BreakTenAdd5mmBreakS;
            sheetResult.Cells[count, 2].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[4]).BreakTenAdd5mmBreakL;
            sheetResult.Cells[count, 3].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[4]).BreakTenBreakS;
            sheetResult.Cells[count, 4].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[4]).BreakTenBreakL;
            count++;

            sheetResult.Cells[count, 1].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[9]).BreakTenAdd5mmBreakS;
            sheetResult.Cells[count, 2].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[9]).BreakTenAdd5mmBreakL;
            sheetResult.Cells[count, 3].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[9]).BreakTenBreakS;
            sheetResult.Cells[count, 4].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[9]).BreakTenBreakL;
            count++;

            //不均匀冰计算
            sheetResult.Cells[count, 1].Value = tower.PhaseTraList[0].UnbaIceTenDiffStr;
            sheetResult.Cells[count, 2].Value = tower.PhaseTraList[0].UnbaIceTenDiff;
            sheetResult.Cells[count, 7].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[0]).TenUnbaIceMax;
            sheetResult.Cells[count, 8].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[0]).TenUnbaIceIceCov100;
            sheetResult.Cells[count, 9].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[5]).TenUnbaIceIceCov100;
            count++;

            sheetResult.Cells[count, 1].Value = tower.PhaseTraList[3].UnbaIceTenDiffStr;
            sheetResult.Cells[count, 2].Value = tower.PhaseTraList[3].UnbaIceTenDiff;
            sheetResult.Cells[count, 4].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[3]).UnbaIceTenDiffGrdBreStr;
            sheetResult.Cells[count, 5].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[3]).UnbaIceTenDiffGrdBre;
            sheetResult.Cells[count, 7].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[3]).TenUnbaIceMax;
            sheetResult.Cells[count, 8].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[3]).TenUnbaIceIceCov100;
            sheetResult.Cells[count, 9].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[8]).TenUnbaIceIceCov100;
            count++;

            sheetResult.Cells[count, 1].Value = tower.PhaseTraList[4].UnbaIceTenDiffStr;
            sheetResult.Cells[count, 2].Value = tower.PhaseTraList[4].UnbaIceTenDiff;
            sheetResult.Cells[count, 4].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[4]).UnbaIceTenDiffGrdBreStr;
            sheetResult.Cells[count, 5].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[4]).UnbaIceTenDiffGrdBre;
            sheetResult.Cells[count, 7].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[4]).TenUnbaIceMax;
            sheetResult.Cells[count, 8].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[4]).TenUnbaIceIceCov100;
            sheetResult.Cells[count, 9].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[9]).TenUnbaIceIceCov100;
            count+=2;

            sheetResult.Cells[count, 1].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[0]).TenUnbaIceMaxTensionS;
            sheetResult.Cells[count, 2].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[0]).TenUnbaIceMaxTensionL;
            sheetResult.Cells[count, 4].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[0]).TenUnbaIceMaxIceCov100S;
            sheetResult.Cells[count, 5].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[0]).TenUnbaIceMaxIceCov100L;
            sheetResult.Cells[count, 7].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[0]).UnbaIceTenMaxS;
            sheetResult.Cells[count, 8].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[0]).UnbaIceTenMaxL;
            count++;

            sheetResult.Cells[count, 1].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[5]).TenUnbaIceMaxTensionS;
            sheetResult.Cells[count, 2].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[5]).TenUnbaIceMaxTensionL;
            sheetResult.Cells[count, 4].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[5]).TenUnbaIceMaxIceCov100S;
            sheetResult.Cells[count, 5].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[5]).TenUnbaIceMaxIceCov100L;
            sheetResult.Cells[count, 7].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[5]).UnbaIceTenMaxS;
            sheetResult.Cells[count, 8].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[5]).UnbaIceTenMaxL;
            count++;

            sheetResult.Cells[count, 1].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[3]).TenUnbaIceMaxTensionS;
            sheetResult.Cells[count, 2].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[3]).TenUnbaIceMaxTensionL;
            sheetResult.Cells[count, 4].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[3]).TenUnbaIceMaxIceCov100S;
            sheetResult.Cells[count, 5].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[3]).TenUnbaIceMaxIceCov100L;
            sheetResult.Cells[count, 7].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[3]).UnbaIceTenMaxS;
            sheetResult.Cells[count, 8].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[3]).UnbaIceTenMaxL;
            count++;

            sheetResult.Cells[count, 1].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[8]).TenUnbaIceMaxTensionS;
            sheetResult.Cells[count, 2].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[8]).TenUnbaIceMaxTensionL;
            sheetResult.Cells[count, 4].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[8]).TenUnbaIceMaxIceCov100S;
            sheetResult.Cells[count, 5].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[8]).TenUnbaIceMaxIceCov100L;
            sheetResult.Cells[count, 7].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[8]).UnbaIceTenMaxS;
            sheetResult.Cells[count, 8].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[8]).UnbaIceTenMaxL;
            count++;

            sheetResult.Cells[count, 1].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[4]).TenUnbaIceMaxTensionS;
            sheetResult.Cells[count, 2].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[4]).TenUnbaIceMaxTensionL;
            sheetResult.Cells[count, 4].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[4]).TenUnbaIceMaxIceCov100S;
            sheetResult.Cells[count, 5].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[4]).TenUnbaIceMaxIceCov100L;
            sheetResult.Cells[count, 7].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[4]).UnbaIceTenMaxS;
            sheetResult.Cells[count, 8].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[4]).UnbaIceTenMaxL;
            count++;

            sheetResult.Cells[count, 1].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[9]).TenUnbaIceMaxTensionS;
            sheetResult.Cells[count, 2].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[9]).TenUnbaIceMaxTensionL;
            sheetResult.Cells[count, 4].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[9]).TenUnbaIceMaxIceCov100S;
            sheetResult.Cells[count, 5].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[9]).TenUnbaIceMaxIceCov100L;
            sheetResult.Cells[count, 7].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[9]).UnbaIceTenMaxS;
            sheetResult.Cells[count, 8].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[9]).UnbaIceTenMaxL;
            count++;

            sheetResult.Cells[count, 7].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[0]).TenUnbaIceS;
            sheetResult.Cells[count, 8].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[0]).TenUnbaIceL;
            count++;

            sheetResult.Cells[count, 7].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[5]).TenUnbaIceS;
            sheetResult.Cells[count, 8].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[5]).TenUnbaIceL;
            count++;

            sheetResult.Cells[count, 7].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[3]).TenUnbaIceS;
            sheetResult.Cells[count, 8].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[3]).TenUnbaIceL;
            count++;

            sheetResult.Cells[count, 7].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[8]).TenUnbaIceS;
            sheetResult.Cells[count, 8].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[8]).TenUnbaIceL;
            count++;

            sheetResult.Cells[count, 7].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[4]).TenUnbaIceS;
            sheetResult.Cells[count, 8].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[4]).TenUnbaIceL;
            count++;

            sheetResult.Cells[count, 7].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[9]).TenUnbaIceS;
            sheetResult.Cells[count, 8].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[9]).TenUnbaIceL;
            count += 2;

            sheetResult.Cells[count, 1].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[3]).TenUnbaIceMaxTensionBreakS;
            sheetResult.Cells[count, 2].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[3]).TenUnbaIceMaxTensionBreakL;
            sheetResult.Cells[count, 3].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[3]).TenUnbaIceMaxIceCov100BreakS;
            sheetResult.Cells[count, 4].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[3]).TenUnbaIceMaxIceCov100BreakL;
            sheetResult.Cells[count, 5].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[3]).UnbaIceTenMaxBreakS;
            sheetResult.Cells[count, 6].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[3]).UnbaIceTenMaxBreakL;
            count++;

            sheetResult.Cells[count, 1].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[8]).TenUnbaIceMaxTensionBreakS;
            sheetResult.Cells[count, 2].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[8]).TenUnbaIceMaxTensionBreakL;
            sheetResult.Cells[count, 3].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[8]).TenUnbaIceMaxIceCov100BreakS;
            sheetResult.Cells[count, 4].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[8]).TenUnbaIceMaxIceCov100BreakL;
            sheetResult.Cells[count, 5].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[8]).UnbaIceTenMaxBreakS;
            sheetResult.Cells[count, 6].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[8]).UnbaIceTenMaxBreakL;
            count++;

            sheetResult.Cells[count, 1].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[4]).TenUnbaIceMaxTensionBreakS;
            sheetResult.Cells[count, 2].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[4]).TenUnbaIceMaxTensionBreakL;
            sheetResult.Cells[count, 3].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[4]).TenUnbaIceMaxIceCov100BreakS;
            sheetResult.Cells[count, 4].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[4]).TenUnbaIceMaxIceCov100BreakL;
            sheetResult.Cells[count, 5].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[4]).UnbaIceTenMaxBreakS;
            sheetResult.Cells[count, 6].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[4]).UnbaIceTenMaxBreakL;
            count++;

            sheetResult.Cells[count, 1].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[9]).TenUnbaIceMaxTensionBreakS;
            sheetResult.Cells[count, 2].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[9]).TenUnbaIceMaxTensionBreakL;
            sheetResult.Cells[count, 3].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[9]).TenUnbaIceMaxIceCov100BreakS;
            sheetResult.Cells[count, 4].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[9]).TenUnbaIceMaxIceCov100BreakL;
            sheetResult.Cells[count, 5].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[9]).UnbaIceTenMaxBreakS;
            sheetResult.Cells[count, 6].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[9]).UnbaIceTenMaxBreakL;
            count++;

            sheetResult.Cells[count, 1].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[3]).UnbaIceTenAdd5mmBreakS;
            sheetResult.Cells[count, 2].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[3]).UnbaIceTenAdd5mmBreakL;
            sheetResult.Cells[count, 3].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[3]).UnbaIceTenBreakS;
            sheetResult.Cells[count, 4].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[3]).UnbaIceTenBreakL;
            count++;

            sheetResult.Cells[count, 1].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[8]).UnbaIceTenAdd5mmBreakS;
            sheetResult.Cells[count, 2].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[8]).UnbaIceTenAdd5mmBreakL;
            sheetResult.Cells[count, 3].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[8]).UnbaIceTenBreakS;
            sheetResult.Cells[count, 4].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[8]).UnbaIceTenBreakL;
            count++;

            sheetResult.Cells[count, 1].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[4]).UnbaIceTenAdd5mmBreakS;
            sheetResult.Cells[count, 2].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[4]).UnbaIceTenAdd5mmBreakL;
            sheetResult.Cells[count, 3].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[4]).UnbaIceTenBreakS;
            sheetResult.Cells[count, 4].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[4]).UnbaIceTenBreakL;
            count++;

            sheetResult.Cells[count, 1].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[9]).UnbaIceTenAdd5mmBreakS;
            sheetResult.Cells[count, 2].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[9]).UnbaIceTenAdd5mmBreakL;
            sheetResult.Cells[count, 3].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[9]).UnbaIceTenBreakS;
            sheetResult.Cells[count, 4].Value = ((ElecCalsPhaseStrHang)tower.PhaseTraList[9]).UnbaIceTenBreakL;

        }

        protected void LoadStrainTowerResult()
        {
            //更新参数
            int paraCnt = 1;

            ElecCalsRes backSideRes = ((TowerStrainElecCals)tower).BackSideRes;
            ElecCalsRes frontSideRes = ((TowerStrainElecCals)tower).FrontSideRes;

            sheetResult.Cells[paraCnt, 1].Value = backSideRes.CommParas.VoltStr;  //电压等级
            sheetResult.Cells[paraCnt, 3].Value = backSideRes.CommParas.CircuitType; // 单双回路类型

            paraCnt++;
            sheetResult.Cells[paraCnt, 1].Value = backSideRes.CommParas.GrdIceForcePara == 1 ? "考虑+5mm" : "不考虑+5mm"; //地线覆冰张力计算模式
            sheetResult.Cells[paraCnt, 3].Value = backSideRes.CommParas.GrdIceUnbaPara == 1 ? "考虑+5mm" : "不考虑+5mm"; //地线覆冰张力计算模式
            sheetResult.Cells[paraCnt, 5].Value = backSideRes.CommParas.InsVerWeiPara; //安装垂直荷载取值
            sheetResult.Cells[paraCnt, 7].Value = backSideRes.CommParas.UnbaIceVerWeiPara; //不均匀冰垂直荷载取值

            paraCnt++;
            sheetResult.Cells[paraCnt, 1].Value = backSideRes.CommParas.BreakMaxPara; //断线max取值
            sheetResult.Cells[paraCnt, 3].Value = backSideRes.CommParas.UnbaMaxPara;  //不均匀冰max取值
            sheetResult.Cells[paraCnt, 5].Value = backSideRes.CommParas.GrdIceCheckPara == 1 ? "不考虑+5mm" : "考虑+5mm"; //地线验算张力取值
            sheetResult.Cells[paraCnt, 7].Value = backSideRes.CommParas.HandForcePara == 1 ? " 两者大值" : (backSideRes.CommParas.HandForcePara == 2 ? "系数法" : "降温法"); //锚线张力取值

            paraCnt++;
            sheetResult.Cells[paraCnt, 1].Value = tower.AngelofApplication;
            var wea = frontSideRes.Weather.WeathComm.Where(item => item.Name == "最大风速").FirstOrDefault();
            sheetResult.Cells[paraCnt, 3].Value = wea.WindSpeed;

            paraCnt++;
            sheetResult.Cells[paraCnt, 1].Value = frontSideRes.IndWire.DevideNum; //大号导线分裂数
            sheetResult.Cells[paraCnt, 3].Value = backSideRes.IndWire.DevideNum; //小号导线分裂数


            //更新垂直档距
            int count = 3;
            for (int i = 0; i < 5; i++)
            {
                var phase = tower.PhaseTraList[i];
                var phaseAn = tower.PhaseTraList[i + 5];
                List<string> wkList = phase.WireData.WorkCdtNamesStrain;

                for (int j = 0; j < wkList.Count; j++)
                {
                    sheetResult.Cells[count, 0].Value = wkList[j];
                    sheetResult.Cells[count, 1].Value = phase.LoadList[j].VetiSpanStr;
                    sheetResult.Cells[count, 2].Value = phase.LoadList[j].VetiSpan;
                    sheetResult.Cells[count, 3].Value = wkList[j];
                    sheetResult.Cells[count, 4].Value = phaseAn.LoadList[j].VetiSpanStr;
                    sheetResult.Cells[count, 5].Value = phaseAn.LoadList[j].VetiSpan;
                    count++;
                }
            }

            //更新水平荷载
            count += 2;
            for (int i = 0; i < 5; i++)
            {
                var phase = tower.PhaseTraList[i];
                var phaseAn = tower.PhaseTraList[i + 5];
                List<string> wkList = phase.WireData.WorkCdtNamesStrain;
                for (int j = 0; j < wkList.Count; j++)
                {
                    sheetResult.Cells[count, 0].Value = wkList[j];
                    sheetResult.Cells[count, 1].Value = phase.LoadList[j].HorForStr;
                    sheetResult.Cells[count, 2].Value = phase.LoadList[j].HorFor;
                    sheetResult.Cells[count, 3].Value = wkList[j];
                    sheetResult.Cells[count, 4].Value = phaseAn.LoadList[j].HorForStr; 
                    sheetResult.Cells[count, 5].Value = phaseAn.LoadList[j].HorFor;
                    count++;
                }
            }

            //更新垂直荷载
            count += 2;
            for (int i = 0; i < 5; i++)
            {
                var phase = tower.PhaseTraList[i];
                var phaseAn = tower.PhaseTraList[i + 5];
                List<string> wkList = phase.WireData.WorkCdtNamesStrain;

                for (int j = 0; j < wkList.Count; j++)
                {
                    sheetResult.Cells[count, 0].Value = wkList[j];
                    sheetResult.Cells[count, 1].Value = phase.LoadList[j].VerWeiStr;
                    sheetResult.Cells[count, 2].Value = phase.LoadList[j].VerWei;
                    sheetResult.Cells[count, 3].Value = wkList[j];
                    sheetResult.Cells[count, 4].Value = phaseAn.LoadList[j].VerWeiStr;
                    sheetResult.Cells[count, 5].Value = phaseAn.LoadList[j].VerWei;
                    count++;
                }
            }

            //更新纵向荷载
            count += 1;
            for (int i = 0; i < 5; i++)
            {
                var phase = tower.PhaseTraList[i];
                var phaseAn = tower.PhaseTraList[i + 5];
                List<string> wkList = phase.WireData.WorkCdtNamesHang;

                for (int j = 0; j < wkList.Count; j++)
                {
                    sheetResult.Cells[count, 0].Value = wkList[j];
                    sheetResult.Cells[count, 1].Value = phase.LoadList[j].LoStrStr;
                    sheetResult.Cells[count, 2].Value = phase.LoadList[j].LoStr;
                    sheetResult.Cells[count, 3].Value = wkList[j];
                    sheetResult.Cells[count, 4].Value = phaseAn.LoadList[j].LoStrStr;
                    sheetResult.Cells[count, 5].Value = phaseAn.LoadList[j].LoStr;
                    sheetResult.Cells[count, 6].Value = phase.LoadList[j].LoStrCheckStr;
                    sheetResult.Cells[count, 7].Value = phaseAn.LoadList[j].LoStrCheckStr;


                    if (wkList[j] == "断线" || wkList[j] == "不均匀冰I" || wkList[j] == "不均匀冰II" || wkList[j] == "断线(导线+5mm)" || wkList[j] == "不均匀冰I(导线+5mm)" || wkList[j] == "不均匀冰II(导线+5mm)")
                    {
                        sheetResult.Cells[count, 8].Value = phase.LoadList[j].LoStrCheck2;
                        sheetResult.Cells[count, 9].Value = phaseAn.LoadList[j].LoStrCheck2;
                    }
                    count++;
                }
            }

            //张力差验算
            count++;
            sheetResult.Cells[count, 1].Value = tower.PhaseTraList[0].BreakTenDiffStr;
            sheetResult.Cells[count, 2].Value = tower.PhaseTraList[0].BreakTenDiff;
            sheetResult.Cells[count, 4].Value = tower.PhaseTraList[5].BreakTenDiffStr;
            sheetResult.Cells[count, 5].Value = tower.PhaseTraList[5].BreakTenDiff;
            sheetResult.Cells[count, 6].Value = tower.PhaseTraList[0].BreakTenMaxTemp;
            sheetResult.Cells[count, 7].Value = tower.PhaseTraList[5].BreakTenMaxTemp;
            sheetResult.Cells[count, 8].Value = tower.PhaseTraList[0].BreakTenMax;
            sheetResult.Cells[count, 9].Value = tower.PhaseTraList[5].BreakTenMax;

            count++;
            sheetResult.Cells[count, 1].Value = tower.PhaseTraList[3].BreakTenDiffStr;
            sheetResult.Cells[count, 2].Value = tower.PhaseTraList[3].BreakTenDiff;
            sheetResult.Cells[count, 4].Value = tower.PhaseTraList[8].BreakTenDiffStr;
            sheetResult.Cells[count, 5].Value = tower.PhaseTraList[8].BreakTenDiff;
            sheetResult.Cells[count, 6].Value = tower.PhaseTraList[3].BreakTenMaxTemp;
            sheetResult.Cells[count, 7].Value = tower.PhaseTraList[8].BreakTenMaxTemp;
            sheetResult.Cells[count, 8].Value = tower.PhaseTraList[3].BreakTenMax;
            sheetResult.Cells[count, 9].Value = tower.PhaseTraList[8].BreakTenMax;

            count++;
            sheetResult.Cells[count, 1].Value = tower.PhaseTraList[4].BreakTenDiffStr;
            sheetResult.Cells[count, 2].Value = tower.PhaseTraList[4].BreakTenDiff;
            sheetResult.Cells[count, 4].Value = tower.PhaseTraList[9].BreakTenDiffStr;
            sheetResult.Cells[count, 5].Value = tower.PhaseTraList[9].BreakTenDiff;
            sheetResult.Cells[count, 6].Value = tower.PhaseTraList[4].BreakTenMaxTemp;
            sheetResult.Cells[count, 7].Value = tower.PhaseTraList[9].BreakTenMaxTemp;
            sheetResult.Cells[count, 8].Value = tower.PhaseTraList[4].BreakTenMax;
            sheetResult.Cells[count, 9].Value = tower.PhaseTraList[9].BreakTenMax;

            count++;
            sheetResult.Cells[count, 1].Value = tower.PhaseTraList[0].UnbaIceTenDiffStr;
            sheetResult.Cells[count, 2].Value = tower.PhaseTraList[0].UnbaIceTenDiff;
            sheetResult.Cells[count, 4].Value = tower.PhaseTraList[5].UnbaIceTenDiffStr;
            sheetResult.Cells[count, 5].Value = tower.PhaseTraList[5].UnbaIceTenDiff;
            sheetResult.Cells[count, 6].Value = tower.PhaseTraList[0].UnbaIceTenMaxTemp;
            sheetResult.Cells[count, 7].Value = tower.PhaseTraList[5].UnbaIceTenMaxTemp;
            sheetResult.Cells[count, 8].Value = tower.PhaseTraList[0].UnbaIceTenMax;
            sheetResult.Cells[count, 9].Value = tower.PhaseTraList[5].UnbaIceTenMax;

            count++;
            sheetResult.Cells[count, 1].Value = tower.PhaseTraList[3].UnbaIceTenDiffStr;
            sheetResult.Cells[count, 2].Value = tower.PhaseTraList[3].UnbaIceTenDiff;
            sheetResult.Cells[count, 4].Value = tower.PhaseTraList[8].UnbaIceTenDiffStr;
            sheetResult.Cells[count, 5].Value = tower.PhaseTraList[8].UnbaIceTenDiff;
            sheetResult.Cells[count, 6].Value = tower.PhaseTraList[3].UnbaIceTenMaxTemp;
            sheetResult.Cells[count, 7].Value = tower.PhaseTraList[8].UnbaIceTenMaxTemp;
            sheetResult.Cells[count, 8].Value = tower.PhaseTraList[3].UnbaIceTenMax;
            sheetResult.Cells[count, 9].Value = tower.PhaseTraList[8].UnbaIceTenMax;

            count++;
            sheetResult.Cells[count, 1].Value = tower.PhaseTraList[4].UnbaIceTenDiffStr;
            sheetResult.Cells[count, 2].Value = tower.PhaseTraList[4].UnbaIceTenDiff;
            sheetResult.Cells[count, 4].Value = tower.PhaseTraList[9].UnbaIceTenDiffStr;
            sheetResult.Cells[count, 5].Value = tower.PhaseTraList[9].UnbaIceTenDiff;
            sheetResult.Cells[count, 6].Value = tower.PhaseTraList[4].UnbaIceTenMaxTemp;
            sheetResult.Cells[count, 7].Value = tower.PhaseTraList[9].UnbaIceTenMaxTemp;
            sheetResult.Cells[count, 8].Value = tower.PhaseTraList[4].UnbaIceTenMax;
            sheetResult.Cells[count, 9].Value = tower.PhaseTraList[9].UnbaIceTenMax;
            count +=2;

            sheetResult.Cells[count, 1].Value = tower.PhaseTraList[0].AnSideBreakTenDiff;
            sheetResult.Cells[count, 2].Value = tower.PhaseTraList[5].AnSideBreakTenDiff;
            sheetResult.Cells[count, 3].Value = tower.PhaseTraList[0].BothSidesBreakTenDiff;
            sheetResult.Cells[count, 4].Value = tower.PhaseTraList[5].BothSidesBreakTenDiff;
            count++;

            sheetResult.Cells[count, 1].Value = tower.PhaseTraList[3].AnSideBreakTenDiff;
            sheetResult.Cells[count, 2].Value = tower.PhaseTraList[8].AnSideBreakTenDiff;
            sheetResult.Cells[count, 3].Value = tower.PhaseTraList[3].BothSidesBreakTenDiff;
            sheetResult.Cells[count, 4].Value = tower.PhaseTraList[8].BothSidesBreakTenDiff;
            count++;

            sheetResult.Cells[count, 1].Value = tower.PhaseTraList[4].AnSideBreakTenDiff;
            sheetResult.Cells[count, 2].Value = tower.PhaseTraList[9].AnSideBreakTenDiff;
            sheetResult.Cells[count, 3].Value = tower.PhaseTraList[4].BothSidesBreakTenDiff;
            sheetResult.Cells[count, 4].Value = tower.PhaseTraList[9].BothSidesBreakTenDiff;
            count += 2;

            sheetResult.Cells[count, 1].Value = tower.PhaseTraList[0].AnSideBreakTenDiffCheck;
            sheetResult.Cells[count, 2].Value = tower.PhaseTraList[5].AnSideBreakTenDiffCheck;
            sheetResult.Cells[count, 3].Value = tower.PhaseTraList[0].BothSidesBreakTenDiffCheckTemp;
            sheetResult.Cells[count, 4].Value = tower.PhaseTraList[5].BothSidesBreakTenDiffCheckTemp;
            sheetResult.Cells[count, 5].Value = tower.PhaseTraList[0].BothSidesBreakTenDiffCheck;
            sheetResult.Cells[count, 6].Value = tower.PhaseTraList[5].BothSidesBreakTenDiffCheck;
            count++;

            sheetResult.Cells[count, 1].Value = tower.PhaseTraList[3].AnSideBreakTenDiffCheck;
            sheetResult.Cells[count, 2].Value = tower.PhaseTraList[8].AnSideBreakTenDiffCheck;
            sheetResult.Cells[count, 3].Value = tower.PhaseTraList[3].BothSidesBreakTenDiffCheckTemp;
            sheetResult.Cells[count, 4].Value = tower.PhaseTraList[8].BothSidesBreakTenDiffCheckTemp;
            sheetResult.Cells[count, 5].Value = tower.PhaseTraList[3].BothSidesBreakTenDiffCheck;
            sheetResult.Cells[count, 6].Value = tower.PhaseTraList[8].BothSidesBreakTenDiffCheck;
            count++;

            sheetResult.Cells[count, 1].Value = tower.PhaseTraList[4].AnSideBreakTenDiffCheck;
            sheetResult.Cells[count, 2].Value = tower.PhaseTraList[9].AnSideBreakTenDiffCheck;
            sheetResult.Cells[count, 3].Value = tower.PhaseTraList[4].BothSidesBreakTenDiffCheckTemp;
            sheetResult.Cells[count, 4].Value = tower.PhaseTraList[9].BothSidesBreakTenDiffCheckTemp;
            sheetResult.Cells[count, 5].Value = tower.PhaseTraList[4].BothSidesBreakTenDiffCheck;
            sheetResult.Cells[count, 6].Value = tower.PhaseTraList[9].BothSidesBreakTenDiffCheck;
            count += 2;

            sheetResult.Cells[count, 2].Value = tower.PhaseTraList[0].UnbaIceTenIDiffBothSids;
            sheetResult.Cells[count, 3].Value = tower.PhaseTraList[0].UnbaIceTenIDiff;
            sheetResult.Cells[count, 4].Value = tower.PhaseTraList[5].UnbaIceTenIDiff;
            count++;

            sheetResult.Cells[count, 2].Value = tower.PhaseTraList[0].UnbaIceTenIIDiffBothSids;
            sheetResult.Cells[count, 3].Value = tower.PhaseTraList[0].UnbaIceTenIIDiff;
            sheetResult.Cells[count, 4].Value = tower.PhaseTraList[5].UnbaIceTenIIDiff;
            count++;

            sheetResult.Cells[count, 2].Value = tower.PhaseTraList[3].UnbaIceTenIDiffBothSids;
            sheetResult.Cells[count, 3].Value = tower.PhaseTraList[3].UnbaIceTenIDiff;
            sheetResult.Cells[count, 4].Value = tower.PhaseTraList[8].UnbaIceTenIDiff;
            count++;

            sheetResult.Cells[count, 2].Value = tower.PhaseTraList[3].UnbaIceTenIIDiffBothSids;
            sheetResult.Cells[count, 3].Value = tower.PhaseTraList[3].UnbaIceTenIIDiff;
            sheetResult.Cells[count, 4].Value = tower.PhaseTraList[8].UnbaIceTenIIDiff;
            count++;

            sheetResult.Cells[count, 2].Value = tower.PhaseTraList[4].UnbaIceTenIDiffBothSids;
            sheetResult.Cells[count, 3].Value = tower.PhaseTraList[4].UnbaIceTenIDiff;
            sheetResult.Cells[count, 4].Value = tower.PhaseTraList[9].UnbaIceTenIDiff;
            count++;

            sheetResult.Cells[count, 2].Value = tower.PhaseTraList[4].UnbaIceTenIIDiffBothSids;
            sheetResult.Cells[count, 3].Value = tower.PhaseTraList[4].UnbaIceTenIIDiff;
            sheetResult.Cells[count, 4].Value = tower.PhaseTraList[9].UnbaIceTenIIDiff;
            count+=2;

            sheetResult.Cells[count, 2].Value = tower.PhaseTraList[0].UnbaIceTenIDiffBothSidsCheck;
            sheetResult.Cells[count, 3].Value = tower.PhaseTraList[0].UnbaIceTenIDiffCheckTemp;
            sheetResult.Cells[count, 4].Value = tower.PhaseTraList[5].UnbaIceTenIDiffCheckTemp;
            sheetResult.Cells[count, 5].Value = tower.PhaseTraList[0].UnbaIceTenIDiffCheck;
            sheetResult.Cells[count, 6].Value = tower.PhaseTraList[5].UnbaIceTenIDiffCheck;
            count++;

            sheetResult.Cells[count, 2].Value = tower.PhaseTraList[0].UnbaIceTenIIDiffBothSidsCheck;
            sheetResult.Cells[count, 3].Value = tower.PhaseTraList[0].UnbaIceTenIIDiffCheckTemp;
            sheetResult.Cells[count, 4].Value = tower.PhaseTraList[5].UnbaIceTenIIDiffCheckTemp;
            sheetResult.Cells[count, 5].Value = tower.PhaseTraList[0].UnbaIceTenIIDiffCheck;
            sheetResult.Cells[count, 6].Value = tower.PhaseTraList[5].UnbaIceTenIIDiffCheck;
            count++;

            sheetResult.Cells[count, 2].Value = tower.PhaseTraList[3].UnbaIceTenIDiffBothSidsCheck;
            sheetResult.Cells[count, 3].Value = tower.PhaseTraList[3].UnbaIceTenIDiffCheckTemp;
            sheetResult.Cells[count, 4].Value = tower.PhaseTraList[8].UnbaIceTenIDiffCheckTemp;
            sheetResult.Cells[count, 5].Value = tower.PhaseTraList[3].UnbaIceTenIDiffCheck;
            sheetResult.Cells[count, 6].Value = tower.PhaseTraList[8].UnbaIceTenIDiffCheck;
            count++;

            sheetResult.Cells[count, 2].Value = tower.PhaseTraList[3].UnbaIceTenIIDiffBothSidsCheck;
            sheetResult.Cells[count, 3].Value = tower.PhaseTraList[3].UnbaIceTenIIDiffCheckTemp;
            sheetResult.Cells[count, 4].Value = tower.PhaseTraList[8].UnbaIceTenIIDiffCheckTemp;
            sheetResult.Cells[count, 5].Value = tower.PhaseTraList[3].UnbaIceTenIIDiffCheck;
            sheetResult.Cells[count, 6].Value = tower.PhaseTraList[8].UnbaIceTenIIDiffCheck;
            count++;

            sheetResult.Cells[count, 2].Value = tower.PhaseTraList[4].UnbaIceTenIDiffBothSidsCheck;
            sheetResult.Cells[count, 3].Value = tower.PhaseTraList[4].UnbaIceTenIDiffCheckTemp;
            sheetResult.Cells[count, 4].Value = tower.PhaseTraList[9].UnbaIceTenIDiffCheckTemp;
            sheetResult.Cells[count, 5].Value = tower.PhaseTraList[4].UnbaIceTenIDiffCheck;
            sheetResult.Cells[count, 6].Value = tower.PhaseTraList[9].UnbaIceTenIDiffCheck;
            count++;

            sheetResult.Cells[count, 2].Value = tower.PhaseTraList[4].UnbaIceTenIIDiffBothSidsCheck;
            sheetResult.Cells[count, 3].Value = tower.PhaseTraList[4].UnbaIceTenIIDiffCheckTemp;
            sheetResult.Cells[count, 4].Value = tower.PhaseTraList[9].UnbaIceTenIIDiffCheckTemp;
            sheetResult.Cells[count, 5].Value = tower.PhaseTraList[4].UnbaIceTenIIDiffCheck;
            sheetResult.Cells[count, 6].Value = tower.PhaseTraList[9].UnbaIceTenIIDiffCheck;
            count += 2;

            for (int i = 0; i < 5; i++)
            {
                var phase = tower.PhaseTraList[i];
                var phaseAn = tower.PhaseTraList[i + 5];
                List<string> wkList = phase.WireData.WorkCdtNamesHang;

                if(phase.WireData.bGrd == 0)
                {
                    for (int j = 0; j < wkList.Count; j++)
                    {
                        sheetResult.Cells[count, 0].Value = wkList[j];
                        sheetResult.Cells[count, 1].Value = phase.LoadList[j].JumpHorForStr;
                        sheetResult.Cells[count, 2].Value = phase.LoadList[j].JumpHorFor;
                        sheetResult.Cells[count, 4].Value = wkList[j];
                        sheetResult.Cells[count, 5].Value = phase.LoadList[j].JumpVerWeiStr;
                        sheetResult.Cells[count, 7].Value = phase.LoadList[j].JumpVerWei;
                        count++;
                    }
                }
            }
        }


        


    }
}
