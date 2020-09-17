using System;
using System.Collections.Generic;
using System.Data;
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
using DevExpress.Spreadsheet;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Grid;
using TowerLoadCals.Mode;
using TowerLoadCals.Mode.Structure;

namespace TowerLoadCals.Modules
{
    /// <summary>
    /// StruCalsElecLoadModule.xaml 的交互逻辑
    /// </summary>
    public partial class StruCalsElecLoadModule
    {
        protected Worksheet elecLoadWorksheet;

        protected TowerTypeEnum towerType;
        protected StruCalsElecLoad elecLoad;

        public StruCalsElecLoadModule()
        {
            InitializeComponent();
            DataContext = new StruCalsElecLoadViewModel(this);

            IWorkbook workbook = elecLoadSC.Document;
            elecLoadWorksheet = workbook.Worksheets[0];
        }

        public void InitElecLoadSheet(TowerTypeEnum type,  StruCalsElecLoad load)
        {
            towerType = type;
            elecLoad = load;

            UpdateElecLoadWorkSheet();
        }

        protected void UpdateElecLoadWorkSheet()
        {
            int count = 1;
            elecLoadWorksheet.Cells[count++, 0].Value = "气温";
            elecLoadWorksheet.Cells[count++, 0].Value = "风速";
            elecLoadWorksheet.Cells[count++, 0].Value = "覆冰";

            if (towerType == TowerTypeEnum.LineTower)
            {
                for (int i = 0; i < elecLoad.LineElecLoads.Count; i++)
                {
                    var load = elecLoad.LineElecLoads[i];
                    elecLoadWorksheet.Cells[count++, 0].Value = load.WireType;
                    elecLoadWorksheet.Cells[count++, 0].Value = "风荷";     //Wind;
                    elecLoadWorksheet.Cells[count++, 0].Value = "最大垂荷"; //GMax;
                    elecLoadWorksheet.Cells[count++, 0].Value = "最小垂荷"; //GMin;
                    elecLoadWorksheet.Cells[count++, 0].Value = "最大张力"; //TensionMax;
                    elecLoadWorksheet.Cells[count++, 0].Value = "最小张力"; //TensionMin;
                }
            }
            else if (towerType == TowerTypeEnum.LineCornerTower)
            {
                for (int i = 0; i < elecLoad.LineCornerElecLoads.Count; i++)
                {
                    var load = elecLoad.LineCornerElecLoads[i];
                    elecLoadWorksheet.Cells[count++, 0].Value = load.WireType;
                    elecLoadWorksheet.Cells[count++, 0].Value = "最大风荷";   //Wind; 
                    elecLoadWorksheet.Cells[count++, 0].Value = "最小风荷";   //Windx;
                    elecLoadWorksheet.Cells[count++, 0].Value = "最大垂荷"; //GMax;
                    elecLoadWorksheet.Cells[count++, 0].Value = "最小垂荷"; //GMin;
                    elecLoadWorksheet.Cells[count++, 0].Value = "最大张力"; //TensionMax;
                    elecLoadWorksheet.Cells[count++, 0].Value = "最小张力"; //TensionMin;
                }
            }
            else
            {
                for (int i = 0; i < elecLoad.CornerElecLoads.Count; i++)
                {
                    var load = elecLoad.CornerElecLoads[i];
                    elecLoadWorksheet.Cells[count++, 0].Value = load.WireType;
                    elecLoadWorksheet.Cells[count++, 0].Value = "前侧最大风荷";   //WindDF;
                    elecLoadWorksheet.Cells[count++, 0].Value = "后侧最大风荷";   //WindDB;
                    elecLoadWorksheet.Cells[count++, 0].Value = "前侧最小风荷";   //WindXF;
                    elecLoadWorksheet.Cells[count++, 0].Value = "后侧最小风荷";   //WindXF;
                    elecLoadWorksheet.Cells[count++, 0].Value = "前侧最大垂荷"; //GMaxF;
                    elecLoadWorksheet.Cells[count++, 0].Value = "后侧最大垂荷"; //GMaxB;
                    elecLoadWorksheet.Cells[count++, 0].Value = "前侧最小垂荷"; //GMinF;
                    elecLoadWorksheet.Cells[count++, 0].Value = "后侧最小垂荷"; //GMinB;
                    elecLoadWorksheet.Cells[count++, 0].Value = "最大张力"; //TensionMax;
                    elecLoadWorksheet.Cells[count++, 0].Value = "最小张力"; //TensionMin;
                    if(load.WireType.Contains("导"))
                    {
                        elecLoadWorksheet.Cells[count++, 0].Value = "跳线风荷"; //WindTX;
                        elecLoadWorksheet.Cells[count++, 0].Value = "跳线垂荷"; //GTX;
                    }
                }
            }

            for (int i = 1; i <= elecLoad.WorkCondition.Count();i++)
            {
                count = 0;

                var wkCdt = elecLoad.WorkCondition.Where(item => item.Id == i).FirstOrDefault();
                if (wkCdt == null)
                    return;

                elecLoadWorksheet.Cells[count++, i].Value = wkCdt.Id + "." + wkCdt.Name;
                elecLoadWorksheet.Cells[count++, i].Value = wkCdt.Temperature;
                elecLoadWorksheet.Cells[count++, i].Value = wkCdt.WindSpeed;
                elecLoadWorksheet.Cells[count++, i].Value = wkCdt.IceThickness;


                if (towerType == TowerTypeEnum.LineTower)
                {
                    for (int j = 0; j < elecLoad.LineElecLoads.Count; j++)
                    {
                        var load = elecLoad.LineElecLoads[j].ElecLoad.Where(item => item.WorkConditionId == i).FirstOrDefault();
                        load = load == null ? new ElecLoadLine():load;

                        count++;
                        elecLoadWorksheet.Cells[count++, i].Value = load.Wind;     
                        elecLoadWorksheet.Cells[count++, i].Value = load.GMax; 
                        elecLoadWorksheet.Cells[count++, i].Value = load.GMin; 
                        elecLoadWorksheet.Cells[count++, i].Value = load.TensionMax; 
                        elecLoadWorksheet.Cells[count++, i].Value = load.TensionMin; 
                    }
                }
                else if (towerType == TowerTypeEnum.LineCornerTower)
                {
                    for (int j = 0; j < elecLoad.LineCornerElecLoads.Count; i++)
                    {
                        var load = elecLoad.LineCornerElecLoads[j].ElecLoad.Where(item => item.WorkConditionId == i).FirstOrDefault();
                        load = load == null ? new ElecLoadLineCorner() : load;

                        count++;
                        elecLoadWorksheet.Cells[count++, i].Value = load.Wind;   //Wind; 
                        elecLoadWorksheet.Cells[count++, i].Value = load.Windx;   //Windx;
                        elecLoadWorksheet.Cells[count++, i].Value = load.GMax; //GMax;
                        elecLoadWorksheet.Cells[count++, i].Value = load.GMin; //GMin;
                        elecLoadWorksheet.Cells[count++, i].Value = load.TensionMax; //TensionMax;
                        elecLoadWorksheet.Cells[count++, i].Value = load.TensionMin; //TensionMin;
                    }
                }
                else
                {
                    for (int j = 0; j < elecLoad.CornerElecLoads.Count; i++)
                    {
                        var load = elecLoad.CornerElecLoads[j].ElecLoad.Where(item => item.WorkConditionId == i).FirstOrDefault();
                        load = load == null ? new ElecLoadCorner() : load;

                        count++;
                        elecLoadWorksheet.Cells[count++, i].Value = load.WindDF; ;   //WindDF;
                        elecLoadWorksheet.Cells[count++, i].Value = load.WindDB; ;   //WindDB;
                        elecLoadWorksheet.Cells[count++, i].Value = load.WindXF; ;   //WindXF;
                        elecLoadWorksheet.Cells[count++, i].Value = load.WindXF; ;   //WindXF;
                        elecLoadWorksheet.Cells[count++, i].Value = load.GMaxF; ; //GMaxF;
                        elecLoadWorksheet.Cells[count++, i].Value = load.GMaxB; ; //GMaxB;
                        elecLoadWorksheet.Cells[count++, i].Value = load.GMinF; ; //GMinF;
                        elecLoadWorksheet.Cells[count++, i].Value = load.GMinB; ; //GMinB;
                        elecLoadWorksheet.Cells[count++, i].Value = load.TensionD; ; //TensionMax;
                        elecLoadWorksheet.Cells[count++, i].Value = load.TensionX; ; //TensionMin;
                        if (elecLoad.CornerElecLoads[j].WireType.Contains("导"))
                        {
                            elecLoadWorksheet.Cells[count++, i].Value = load.WindTX; //WindTX;
                            elecLoadWorksheet.Cells[count++, i].Value = load.GTX; //GTX;
                        }
                    }
                }
            }
        }
    }
}
