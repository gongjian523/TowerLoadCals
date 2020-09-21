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

        //ture：按钮"繁"在应用中，显示完整的电气荷载
        //false：按钮"简"在应用中，显示简化后的电气荷载
        protected bool IsCompleteLoadViewUsed = true;

        //保存简化后的电气荷载
        protected List<WireElecLoadLine> SimpleLineElecLoads = new List<WireElecLoadLine>();
        protected List<WireElecLoadLineCorner> SimpleLineCornerElecLoads = new List<WireElecLoadLineCorner>();
        protected List<WireElecLoadCorner> SimpleCornerElecLoads = new List<WireElecLoadCorner>();

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

            UpdateElecLoadWorkSheetFromMem();
        }

        public void UpdateElecLoadWorkSheetFromMem()
        {
            if(IsCompleteLoadViewUsed)
            {
                UpdateCompleteElecLoadWorkSheet();
            }
            else
            {
                UpdateSimpleElecLoadWorkSheet();

            }
        }

        //worksheet显示完整的电气荷载
        protected void UpdateCompleteElecLoadWorkSheet()
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
                    elecLoadWorksheet.Cells[count++, 0].Value = ConstVar.StruCalsWindStr; //Wind;
                    elecLoadWorksheet.Cells[count++, 0].Value = ConstVar.StruCalsGmaxStr; //GMax;
                    elecLoadWorksheet.Cells[count++, 0].Value = ConstVar.StruCalsGminStr; //GMin;
                    elecLoadWorksheet.Cells[count++, 0].Value = ConstVar.StruCalsTmaxStr; //TensionMax;
                    elecLoadWorksheet.Cells[count++, 0].Value = ConstVar.StruCalsTminStr; //TensionMin;
                }
            }
            else if (towerType == TowerTypeEnum.LineCornerTower)
            {
                for (int i = 0; i < elecLoad.LineCornerElecLoads.Count; i++)
                {
                    var load = elecLoad.LineCornerElecLoads[i];
                    elecLoadWorksheet.Cells[count++, 0].Value = load.WireType;
                    elecLoadWorksheet.Cells[count++, 0].Value = ConstVar.StruCalsWindDStr; //Wind
                    elecLoadWorksheet.Cells[count++, 0].Value = ConstVar.StruCalsWindXStr; //Windx
                    elecLoadWorksheet.Cells[count++, 0].Value = ConstVar.StruCalsGmaxStr; //GMax;
                    elecLoadWorksheet.Cells[count++, 0].Value = ConstVar.StruCalsGminStr; //GMin;
                    elecLoadWorksheet.Cells[count++, 0].Value = ConstVar.StruCalsTmaxStr; //TensionMax;
                    elecLoadWorksheet.Cells[count++, 0].Value = ConstVar.StruCalsTminStr; //TensionMin;
                }
            }
            else
            {
                for (int i = 0; i < elecLoad.CornerElecLoads.Count; i++)
                {
                    var load = elecLoad.CornerElecLoads[i];
                    elecLoadWorksheet.Cells[count++, 0].Value = load.WireType;
                    elecLoadWorksheet.Cells[count++, 0].Value = ConstVar.StruCalsWindDFStr;  //WindDF;
                    elecLoadWorksheet.Cells[count++, 0].Value = ConstVar.StruCalsWindDBStr;  //WindDB;
                    elecLoadWorksheet.Cells[count++, 0].Value = ConstVar.StruCalsWindXFStr;  //WindXF;
                    elecLoadWorksheet.Cells[count++, 0].Value = ConstVar.StruCalsWindXBStr;  //WindXB;
                    elecLoadWorksheet.Cells[count++, 0].Value = ConstVar.StruCalsGMaxFStr;  //GMaxF;
                    elecLoadWorksheet.Cells[count++, 0].Value = ConstVar.StruCalsGMaxBStr;  //GMaxB;
                    elecLoadWorksheet.Cells[count++, 0].Value = ConstVar.StruCalsGMinFStr;  //GMinF;
                    elecLoadWorksheet.Cells[count++, 0].Value = ConstVar.StruCalsGMinBStr;  //GMinB;
                    elecLoadWorksheet.Cells[count++, 0].Value = ConstVar.StruCalsTmaxStr;  //TensionMax;
                    elecLoadWorksheet.Cells[count++, 0].Value = ConstVar.StruCalsTminStr;  //TensionMin;
                    if(load.WireType.Contains("导"))
                    {
                        elecLoadWorksheet.Cells[count++, 0].Value = ConstVar.StruCalsWindTXStr;  //WindTX;
                        elecLoadWorksheet.Cells[count++, 0].Value = ConstVar.StruCalsGTXStr;  //GTX;
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

        //worksheet显示简化的电气荷载
        protected void UpdateSimpleElecLoadWorkSheet()
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
                    if (load.WireType.Contains("导"))
                    {
                        elecLoadWorksheet.Cells[count++, 0].Value = "跳线风荷"; //WindTX;
                        elecLoadWorksheet.Cells[count++, 0].Value = "跳线垂荷"; //GTX;
                    }
                }
            }

            for (int i = 1; i <= elecLoad.WorkCondition.Count(); i++)
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
                        load = load == null ? new ElecLoadLine() : load;

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

        public void CollectElecLoadFromWorkSheet()
        {
            if (IsCompleteLoadViewUsed)
            {
                CollectCompleteElecLoadFromWorkSheet();
            }
            else
            {
                CollectSimplElecLoadFromWorkSheet();
            }
        }

        //从worksheet中收集完整的电气荷载，保存在全局结构体中
        protected void CollectCompleteElecLoadFromWorkSheet()
        {
            for (int i = 1; i <= elecLoad.WorkCondition.Count(); i++)
            {
                int count = 4;

                var wkCdt = elecLoad.WorkCondition.Where(item => item.Id == i).FirstOrDefault();
                if (wkCdt == null)
                    return;

                if (towerType == TowerTypeEnum.LineTower)
                {
                    for (int j = 0; j < elecLoad.LineElecLoads.Count; j++)
                    {
                        string wireType = elecLoadWorksheet.Cells[count++, 0].Value.ToString();
                        //查找导地线的荷载
                        int wireLoadId = elecLoad.LineElecLoads.FindIndex(it => it.WireType == wireType);
                        if (wireLoadId < 0)
                            continue;

                        //查找导地线所属工况的荷载 工况的id就是i
                        int loadId = elecLoad.LineElecLoads[wireLoadId].ElecLoad.FindIndex(it => it.WorkConditionId == i);
                        if (loadId < 0)
                            continue;

                        var load = elecLoad.LineElecLoads[wireLoadId].ElecLoad[loadId];
                        load.Wind = (float) elecLoadWorksheet.Cells[count++, i].Value.ToObject();
                        load.GMax = (float) elecLoadWorksheet.Cells[count++, i].Value.ToObject();
                        load.GMin = (float) elecLoadWorksheet.Cells[count++, i].Value.ToObject() ;
                        load.TensionMax = (float) elecLoadWorksheet.Cells[count++, i].Value.ToObject();
                        load.TensionMin = (float) elecLoadWorksheet.Cells[count++, i].Value.ToObject();
                    }
                }
                else if (towerType == TowerTypeEnum.LineCornerTower)
                {
                    for (int j = 0; j < elecLoad.LineCornerElecLoads.Count; i++)
                    {
                        string wireType = elecLoadWorksheet.Cells[count++, 0].Value.ToString();
                        //查找导地线的荷载
                        int wireLoadId = elecLoad.LineCornerElecLoads.FindIndex(it => it.WireType == wireType);
                        if (wireLoadId < 0)
                            continue;

                        //查找导地线所属工况的荷载 工况的id就是i
                        int loadId = elecLoad.LineCornerElecLoads[wireLoadId].ElecLoad.FindIndex(it => it.WorkConditionId == i);
                        if (loadId < 0)
                            continue;

                        var load = elecLoad.LineCornerElecLoads[wireLoadId].ElecLoad[loadId];
                        load.Wind = (float)elecLoadWorksheet.Cells[count++, i].Value.ToObject();
                        load.Windx = (float)elecLoadWorksheet.Cells[count++, i].Value.ToObject();
                        load.GMax = (float)elecLoadWorksheet.Cells[count++, i].Value.ToObject();
                        load.GMin = (float)elecLoadWorksheet.Cells[count++, i].Value.ToObject();
                        load.TensionMax = (float)elecLoadWorksheet.Cells[count++, i].Value.ToObject();
                        load.TensionMin = (float)elecLoadWorksheet.Cells[count++, i].Value.ToObject();
                    }
                }
                else
                {
                    for (int j = 0; j < elecLoad.CornerElecLoads.Count; i++)
                    {
                        string wireType = elecLoadWorksheet.Cells[count++, 0].Value.ToString();
                        //查找导地线的荷载
                        int wireLoadId = elecLoad.CornerElecLoads.FindIndex(it => it.WireType == wireType);
                        if (wireLoadId < 0)
                            continue;

                        //查找导地线所属工况的荷载 工况的id就是i
                        int loadId = elecLoad.CornerElecLoads[wireLoadId].ElecLoad.FindIndex(it => it.WorkConditionId == i);
                        if (loadId < 0)
                            continue;

                        var load = elecLoad.CornerElecLoads[wireLoadId].ElecLoad[loadId];
                        load.WindDF = (float)elecLoadWorksheet.Cells[count++, i].Value.ToObject();
                        load.WindDB = (float)elecLoadWorksheet.Cells[count++, i].Value.ToObject();
                        load.WindXF = (float)elecLoadWorksheet.Cells[count++, i].Value.ToObject();
                        load.WindXF = (float)elecLoadWorksheet.Cells[count++, i].Value.ToObject();
                        load.GMaxF = (float)elecLoadWorksheet.Cells[count++, i].Value.ToObject();
                        load.GMaxB = (float)elecLoadWorksheet.Cells[count++, i].Value.ToObject();
                        load.GMinF = (float)elecLoadWorksheet.Cells[count++, i].Value.ToObject();
                        load.GMinB = (float)elecLoadWorksheet.Cells[count++, i].Value.ToObject();
                        load.TensionD = (float)elecLoadWorksheet.Cells[count++, i].Value.ToObject();
                        load.TensionX = (float)elecLoadWorksheet.Cells[count++, i].Value.ToObject();

                        if (elecLoad.CornerElecLoads[wireLoadId].WireType.Contains("导"))
                        {
                            load.WindTX = (float)elecLoadWorksheet.Cells[count++, i].Value.ToObject();
                            load.GTX = (float)elecLoadWorksheet.Cells[count++, i].Value.ToObject();
                        }
                    }
                }
            }
        }

        //从worksheet中收集完整的电气荷载，保存在全局结构体中
        protected void CollectSimplElecLoadFromWorkSheet()
        {
        }

        protected void ConvertElecLoadComplteToSimple()
        {
            
        }
        protected void ConvertElecLoadSimpleToComplte()
        {
        }

        private void onSimpleBtnClick(object sender, RoutedEventArgs e)
        {
            IsCompleteLoadViewUsed = false;
            ConvertElecLoadComplteToSimple();
            UpdateElecLoadWorkSheetFromMem();
        }

        private void onCompleteBtnClick(object sender, RoutedEventArgs e)
        {
            IsCompleteLoadViewUsed = true;
            ConvertElecLoadSimpleToComplte();
            UpdateElecLoadWorkSheetFromMem();
        }
    }
}
