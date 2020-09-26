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
using TowerLoadCals.Mode;
using TowerLoadCals.ModulesViewModels.TowerSequence;

namespace TowerLoadCals.Modules.TowerSequence
{
    /// <summary>
    /// WeatherConditionModule.xaml 的交互逻辑
    /// </summary>
    /// 

    public partial class TowerSequenceModule : UserControl
    {
        protected Worksheet worksheet;

        public TowerSequenceModule()
        {
            InitializeComponent();
            DataContext = new TowerSequenceViewModel(this);

            IWorkbook workbook = towerSerSC.Document;
            worksheet = workbook.Worksheets[0];
            //workbook.LoadDocument("D:\\00-项目\\Test1.xlsx", DocumentFormat.Xlsx);

            InitSpreadSheetTitle();
        }

        public void RefreshSpreadSheet(List<TowerSerial> dataSource)
        {
            RefreshValue(dataSource);
        }

        protected void InitSpreadSheetTitle()
        {
            worksheet.Cells[0, 0].Value = "是否验算";
            worksheet.Cells[0, 1].Value = "序号";
            worksheet.Cells[0, 2].Value = "塔位号";
            worksheet.Cells[0, 3].Value = "塔位点";
            worksheet.Cells[0, 4].Value = "塔型及呼高";
            worksheet.Cells[0, 5].Value = "塔位桩高程(m)";
            worksheet.Cells[0, 6].Value = "定位高差(m)";
            worksheet.Cells[0, 7].Value = "档距(m)";
            worksheet.Cells[0, 8].Value = "水平档距(m)";
            worksheet.Cells[0, 9].Value = "垂直档距(m)";
            worksheet.Cells[0, 10].Value = "耐张段长/代表档距";
            worksheet.Cells[0, 11].Value = "转角/中心桩位移(m)";
            worksheet.Cells[0, 12].Value = "公共参数";
            worksheet.Cells[0, 13].Value = "后侧档内参数";
            worksheet.Cells[0, 14].Value = "前侧档内参数";
            worksheet.Cells[0, 15].Value = "铁塔配置参数";
        }


        protected void RefreshValue(List<TowerSerial> dataSource)
        {
            int preSt = 0, preEnd = 0;

            for (int i = 0; i < dataSource.Count(); i++)
            {
                worksheet.Cells[1 + 2 * i, 0].Value = dataSource[i].IsChecking;//是否验算
                worksheet.Cells[1 + 2 * i, 1].Value = dataSource[i].ID;//序号
                worksheet.Cells[1 + 2 * i, 2].Value = dataSource[i].TowerName;//塔位号
                worksheet.Cells[1 + 2 * i, 3].Value = dataSource[i].NameOfPositioningPile;//塔位点
                worksheet.Cells[1 + 2 * i, 4].Value = dataSource[i].TowerPattern + "-" + dataSource[i].CallItHigh;//塔型及呼高
                worksheet.Cells[1 + 2 * i, 5].Value = dataSource[i].TowerFootElevation;//塔位桩高程
                worksheet.Cells[1 + 2 * i, 6].Value = dataSource[i].BaseLevelDescent;//定位高差

                if(i  ==  0)
                    worksheet.Cells[1 + 2 * i, 7].Value = dataSource[i].Span;//档距
                else
                    worksheet.Cells[1 + 2 * i - 1, 7].Value = dataSource[i].Span;//档距

                worksheet.Cells[1 + 2 * i, 8].Value = dataSource[i].HorizontalSpan;//水平档距
                worksheet.Cells[1 + 2 * i, 9].Value = dataSource[i].TowerType == 1 ? dataSource[i].VerticalSpan.ToString() : 
                    dataSource[i].BackVerticalSpan + "/" + dataSource[i].FrontVerticalSpan;//垂直档距
                
                worksheet.Cells[1 + 2 * i, 11].Value = dataSource[i].TurningAngleStr; //转角/中心桩位移
                worksheet.Cells[1 + 2 * i, 12].Value = dataSource[i].CommPar; //公共参数
                worksheet.Cells[1 + 2 * i, 13].Value = dataSource[i].BackSidePar; //后侧档内参数
                worksheet.Cells[1 + 2 * i, 14].Value = dataSource[i].FrontSidePar; //前侧档内参数
                worksheet.Cells[1 + 2 * i, 15].Value = dataSource[i].TowerPar; //铁塔配置参数

                string strCoA = "A" + (2 + 2 * i).ToString() + ":A" + (2 + 2 * i + 1).ToString();
                worksheet.MergeCells(worksheet.Range[strCoA]);//是否验算
                string strCoB = "B" + (2 + 2 * i).ToString() + ":B" + (2 + 2 * i + 1).ToString();
                worksheet.MergeCells(worksheet.Range[strCoB]);//序号
                string strCoC = "C" + (2 + 2 * i).ToString() + ":C" + (2 + 2 * i + 1).ToString();
                worksheet.MergeCells(worksheet.Range[strCoC]);//塔位号
                string strCoD = "D" + (2 + 2 * i).ToString() + ":D" + (2 + 2 * i + 1).ToString();
                worksheet.MergeCells(worksheet.Range[strCoD]);//塔位点
                string strCoE = "E" + (2 + 2 * i).ToString() + ":E" + (2 + 2 * i + 1).ToString();
                worksheet.MergeCells(worksheet.Range[strCoE]);//塔型及呼高
                string strCoF = "F" + (2 + 2 * i).ToString() + ":F" + (2 + 2 * i + 1).ToString();
                worksheet.MergeCells(worksheet.Range[strCoF]);//塔位桩高程
                string strCoG = "G" + (2 + 2 * i).ToString() + ":G" + (2 + 2 * i + 1).ToString();
                worksheet.MergeCells(worksheet.Range[strCoG]);//定位高差

                if (i != dataSource.Count() - 1)
                {
                    string strCoH = "H" + (2 + 2 * i + 1).ToString() + ":H" + (2 + 2 * i + 2).ToString();
                    worksheet.MergeCells(worksheet.Range[strCoH]);//档距
                }

                string strCoI = "I" + (2 + 2 * i).ToString() + ":I" + (2 + 2 * i + 1).ToString();
                worksheet.MergeCells(worksheet.Range[strCoI]);//水平档距
                string strCoJ = "J" + (2 + 2 * i).ToString() + ":J" + (2 + 2 * i + 1).ToString();
                worksheet.MergeCells(worksheet.Range[strCoJ]);//垂直档距
                //string strCoL = "L" + (2 + 2 * i).ToString() + ":L" + (2 + 2 * i + 1).ToString();
                //worksheet.MergeCells(worksheet.Range[strCoL]);//转角/中心桩位移
                string strCoM = "M" + (2 + 2 * i).ToString() + ":M" + (2 + 2 * i + 1).ToString();
                worksheet.MergeCells(worksheet.Range[strCoM]);//公共参数
                string strCoN = "N" + (2 + 2 * i).ToString() + ":N" + (2 + 2 * i + 1).ToString();
                worksheet.MergeCells(worksheet.Range[strCoN]);//后侧档内参数
                string strCoO = "O" + (2 + 2 * i).ToString() + ":O" + (2 + 2 * i + 1).ToString();
                worksheet.MergeCells(worksheet.Range[strCoO]);//前侧档内参数
                string strCoP = "P" + (2 + 2 * i).ToString() + ":P" + (2 + 2 * i + 1).ToString();
                worksheet.MergeCells(worksheet.Range[strCoP]);//铁塔配置参数


                preEnd = i;
                if (dataSource[i].TowerType != 1 && i != 0)
                {
                    worksheet.Cells[2 + 2 * preSt + 1, 10].Value = dataSource[i].BackAccPreSpan + "/" + dataSource[i].BackPreSpan;//耐张段长/代表档距

                    string strCoK = "K" + (2 + 2 * preSt + 1).ToString() + ":K" + (2 + 2 * preEnd).ToString();
                    worksheet.MergeCells(worksheet.Range[strCoK]);//耐张段长/代表档距

                    preSt = preEnd;
                }
            }
        }

        public void RefreshCommSidePara(List<TowerSerial> dataSource)
        {
           for(int i = 0; i < dataSource.Count; i++ )
           {
                worksheet.Cells[1 + 2 * (dataSource[i].ID-1), 12].Value = dataSource[i].CommPar;//公共参数 
                worksheet.Cells[1 + 2 * (dataSource[i].ID - 1), 13].Value = dataSource[i].BackSidePar;//公共参数 
                worksheet.Cells[1 + 2 * (dataSource[i].ID - 1), 14].Value = dataSource[i].FrontSidePar;//公共参数 
           }
        }


        public void RefreshTowerPara(List<TowerSerial> dataSource)
        {
            for (int i = 0; i < dataSource.Count; i++)
            {
                worksheet.Cells[1 + 2 * (dataSource[i].ID - 1), 15].Value = dataSource[i].TowerPar;//公共参数 
            }
        }
        
        //只保存是否验算
        public void UpdateTowerPara(List<TowerSerial> dataSource)
        {
            for (int i = 0; i < dataSource.Count(); i++)
            {
                dataSource[i].IsChecking = worksheet.Cells[1 + 2 * i, 0].Value.BooleanValue;
                dataSource[i].CommPar = worksheet.Cells[1 + 2 * i, 12].Value.ToString(); //公共参数
                dataSource[i].BackSidePar = worksheet.Cells[1 + 2 * i, 13].Value.ToString(); //后侧档内参数
                dataSource[i].FrontSidePar = worksheet.Cells[1 + 2 * i, 14].Value.ToString(); //前侧档内参数
                dataSource[i].TowerPar = worksheet.Cells[1 + 2 * i, 15].Value.ToString() ; //铁塔配置参数
            }
        }
    }
}
