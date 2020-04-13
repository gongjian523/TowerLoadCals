using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using TowerLoadCals.DataMaterials;
using TowerLoadCals.Readers;

namespace TowerLoadCals
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private WeatherCollection weathers = new WeatherCollection();
        int weatherDbfNum = 0;

        public MainWindow()
        {
            InitializeComponent();
            weathers.Weathers = new List<Weather>();
        }

        private void button_ReadXML_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "Excel Files (*.xml)|*.xml"
            };
            var result = openFileDialog.ShowDialog();
            
            tabs.SelectedIndex = 0;

            if (result != true)
                return;
            NodeXml root = XmlReader.ReadXml(openFileDialog.FileName);//记录用户选择的文件路径

            xmlTreeView.Items.Clear();

            TreeViewItem subItem = new TreeViewItem();
            FormTreeView(root, subItem);
            xmlTreeView.Items.Add(subItem);

        }

        private void FormTreeView(NodeXml node, TreeViewItem item)
        {
            string str = "";

            foreach (KeyValuePair<string, string> kv in node.atts)
            {
                str += kv.Key + "=" + kv.Value + " ";
            }

            item.Header = str;

            foreach (NodeXml subNode in node.subNodes)
            {
                TreeViewItem subItem = new TreeViewItem();
                FormTreeView(subNode, subItem);
                item.Items.Add(subItem);
            }
        }


        private void button_ReadDBF_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "Excel Files (*.DBF)|*.DBF"
            };
            var result = openFileDialog.ShowDialog();
            if (result != true)
                return;
            
            DataTable DT = DbfReader.ReadDbf(openFileDialog.FileName.Substring(0, openFileDialog.FileName.Length-1- openFileDialog.SafeFileName.Length), openFileDialog.SafeFileName);//记录用户选择的文件路径
            

            Button btn = (Button)sender;
            if(btn.Name == "ReadXiandbfBtn")
            {
                tabs.SelectedIndex = 2;

                ObservableCollection<SLXian> memberData = new ObservableCollection<SLXian>();

                foreach(DataRow row in DT.Rows)
                {
                    memberData.Add(new SLXian {
                        WireType = row["电线型号"].ToString(),
                        SectionMM2 = row["截面MM2"].ToString(),
                        OuterDiameterMM = row["外径MM"].ToString(),
                        WeightKG = row["重量KG每KM"].ToString(),
                        ElasticityCoefficientKG = row["弹性系数KG"].ToString(),
                        ElasticityCoefficientN = row["弹性系数N"].ToString(),
                        LinearExpansionCoefficient = row["线膨胀系数"].ToString(),
                        BreakingStressKGMM = row["断应力KGMM"].ToString(),
                        BreakingForceN = row["断拉力N"].ToString(),
                        Wire1EarthWire2 = row["导线1地线2"].ToString(),
                        NominalCrossSection = row["标称截面"].ToString()
                    });
                }

                dataGridXian.DataContext = memberData;
                dataGridXian.Items.Refresh();
            }
            else if (btn.Name == "ReadWeatherdbfBtn")
            {
                tabs.SelectedIndex = 3;

                weatherDbfNum++;

                List<WorkCondition> wcList = new List<WorkCondition>();
                foreach (DataRow row in DT.Rows)
                {
                    wcList.Add(new WorkCondition
                    {
                        SWorkConditionName = row["工况名称"].ToString(),
                        SWindSpeed = row["风速"].ToString(),
                        STemperature = row["气温"].ToString(),
                        SIceThickness = row["冰厚"].ToString(),
                    });
                }

                Weather weather = new Weather
                {
                    Name = "DBF数据"+ weatherDbfNum.ToString(),
                    WorkConditions = wcList
                };
                weathers.Weathers.Add(weather);
                UpdateWeatherTreeView();
            }
            else
            {
                tabs.SelectedIndex = 1;

                ObservableCollection<SLTa> memberData = new ObservableCollection<SLTa>();

                foreach (DataRow row in DT.Rows)
                {
                    memberData.Add(new SLTa
                    {
                        VoltageLevel = row["电压等级"].ToString(),
                        TowerModel = row["杆塔型号"].ToString(),
                        PicNum = row["图号"].ToString(),
                        Height = row["呼高"].ToString(),
                        Line1Tension2 = row["直线1耐张2"].ToString(),
                        AngleMax = row["最大转角"].ToString(),
                        AllowedLH = row["允许LH"].ToString(),
                        AllowedLV = row["允许LV"].ToString(),
                        DistanceMax = row["最大档距"].ToString(),
                        AllowSwingAngle = row["允许摇摆角"].ToString(),
                        InsideSwingAngle = row["内过摇摆角"].ToString(),
                        OutsideSwingAngle = row["外过摇摆角"].ToString(),
                        HorizontalWireDsitance = row["水平线间距"].ToString(),
                        EarthWireHorizontalDsitance = row["导地水平距"].ToString(),
                        EarthWireCarrierHeight = row["地线支架高"].ToString(),
                        UpDownWireDistance = row["上下导线距"].ToString(),
                        MiddleDownWireDistance = row["中下导线距"].ToString(),
                        LineH1V2V3 = row["导平1垂2V3"].ToString(),
                        LineHProjectionDP = row["导平投影DP"].ToString(),
                        LineVProjectionDZ = row["导垂投影DZ"].ToString(),
                        FrontDistanceM = row["正面根开米"].ToString(),
                        SideDistanceM = row["侧面根开米"].ToString(),
                        SteelWeightKG = row["钢重量KG"].ToString(),
                        A3FWeightKG = row["A3F重量KG"].ToString(),
                        MNWeightKG = row["MN重量KG"].ToString(),
                        CementWeightKG = row["水泥重量KG"].ToString(),
                        BodyPrice = row["本体造价"].ToString(),
                        AngelEffectLH = row["角影响LH"].ToString(),
                        AngelEffectDistance = row["角影响档距"].ToString(),
                        LVMax = row["单侧最大LV"].ToString(),
                        LHMin = row["最小LH"].ToString(),
                        TowerKV = row["塔KV值"].ToString(),
                        Intercept = row["截距"].ToString(),
                        Slope = row["斜率"].ToString(),
                        InterceptGrowthRate = row["截距增长值"].ToString(),
                        SlopeGrowthRate = row["斜率增长值"].ToString(),
                        TowerHangStringNum = row["塔挂串个数"].ToString(),
                        TotalStringNum = row["串总个数"].ToString(),
                        VStringYN = row["V串YN"].ToString(),
                        TowerType = row["杆塔种类"].ToString(),
                        BaseFrontDistance = row["基正面根开"].ToString(),
                        BaseSideDistance = row["基侧面根开"].ToString(),
                        BaseDiagonal = row["基对角线长"].ToString(),
                        ExpectedDeviatingDistance = row["预偏距离S1"].ToString(),
                        SidelineWidth = row["横担宽度"].ToString(),
                        IsTightTower = row["是否紧凑塔"].ToString(),
                        VStringAngle = row["V串夹角"].ToString(),
                        BuriedDeepth = row["埋深"].ToString(),
                        Margin = row["裕度"].ToString(),
                        GroundBoltModel = row["地栓型号"].ToString(),
                        HungPointABC = row["挂点ABC"].ToString()
                    });
                }

                dataGridSLTa.DataContext = memberData;
                dataGridSLTa.Items.Refresh();
            }
            
        }

        private List<Ta> memberData = new List<Ta>();

        private void button_ReadTa_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "Excel Files (*.ta)|*.ta"
            };
            var result = openFileDialog.ShowDialog();
            if (result != true)
                return;

            List<string> strList = TaReader.ReadTa(openFileDialog.FileName);//记录用户选择的文件路径

            //List<Ta> memberData = new List<Ta>();
            memberData.Clear();
            int i = 0;

            foreach (string str  in strList)
            {
                if (!str.Contains(","))
                    continue;
                string[] sArray = str.Split(',');
                i++;
                memberData.Add(new Ta
                {
                    Index = i,
                    Num = sArray[0],
                    PosName = sArray[7].TrimStart(' '),
                    PosOffset = sArray[3].TrimStart(' '),
                    Pos = sArray[7] + "+" + sArray[3],
                    Type = Convert.ToInt16(sArray[1]),
                    Model = sArray[8],
                    Elevation = Convert.ToDouble(sArray[6]),
                    SubOfElv = Convert.ToDouble(sArray[12]),
                    TotalSpan = Convert.ToDouble(sArray[2]),
                    WireK = Convert.ToDouble(sArray[4]),
                    Height = Convert.ToDouble(sArray[9]),
                    StringLength = Convert.ToDouble(sArray[11]),
                    AngelofApplication = Convert.ToDouble(sArray[20])
                });
            }

            CalsParameters(memberData);

            tabs.SelectedIndex = 4;

            dataGridTa.DataContext = memberData;
            dataGridTa.Items.Refresh();
        }

        private void CalsParameters(List<Ta> tas)
        {
            for(int i = 0; i< tas.Count; i++)
            {
                //获取塔名和呼高
                string[] arr = tas[i].Model.Split('-');
                tas[i].Name = arr[0];

                //前侧档距 = 前一个塔的累距-自己的累距
                if (i == tas.Count - 1)
                    tas[i].FrontSpan = 0;
                else
                    tas[i].FrontSpan = Math.Ceiling(tas[i + 1].TotalSpan - tas[i].TotalSpan);

                //后侧档距 = 自己的累距-前一个塔的累距
                //水平档距 = (前侧档距 + 后侧档距) / 2
                double backSpan = 0;
                if (i != 0)
                    backSpan = tas[i].TotalSpan - tas[i - 1].TotalSpan;
                tas[i].HorizontalSpan = Math.Ceiling( (tas[i].FrontSpan + backSpan) / 2);

                if (tas[i].Type == 2)
                    tas[i].StringLength = 0;

                tas[i].guadg = tas[i].Elevation + tas[i].Height + tas[i].SubOfElv - tas[i].StringLength;

                if (i != 0)
                {
                    double h = tas[i].guadg - tas[i-1].guadg;
                    double x = tas[i].WireK * tas[i].FrontSpan * 0.001;
                    double y = ((-1) * tas[i].WireK * h * 0.001) / Math.Sinh(x);
                    tas[i].sec = Math.Log(y + Math.Sqrt(y * y + 1)) / (2 * tas[i].WireK * 0.001);
                    tas[i].BackVerticalSpan = Math.Ceiling(tas[i].FrontSpan / 2 + Math.Log(y + Math.Sqrt(y * y + 1)) / (2 * tas[i].WireK * 0.001));
                    tas[i].FrontVerticalSpan = Math.Ceiling(tas[i].FrontSpan - tas[i].BackVerticalSpan);

                    if (tas[i].Type == 1)
                    {
                        double vs = tas[i].BackVerticalSpan + tas[i - 1].FrontVerticalSpan;
                        tas[i].VerticalSpan = vs.ToString();
                    }
                    else
                    {
                        tas[i].VerticalSpan = tas[i - 1].FrontVerticalSpan.ToString() + "/" + tas[i].BackVerticalSpan.ToString();
                    }
                }
                else
                {
                    tas[i].VerticalSpan = "0";
                }
            }
        }

        private void button_ReadWeatherXML_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "Excel Files (*.xml)|*.xml"
            };
            var result = openFileDialog.ShowDialog();

            tabs.SelectedIndex = 0;

            if (result != true)
                return;
            //NodeXml root = XmlReader.ReadXml(openFileDialog.FileName);//记录用户选择的文件路径

            List<Weather> listWeathers = XmlReader.ReadWeather(openFileDialog.FileName);

            if(listWeathers.Count > 0)
                weathers.Weathers.AddRange(listWeathers);

            UpdateWeatherTreeView();
        }

        private void UpdateWeatherTreeView()
        {
            tabs.SelectedIndex = 3;

            weatherTreeView.Items.Clear();

            foreach(var item  in weathers.Weathers)
            {
                TreeViewItem subItem = new TreeViewItem();
                subItem.Header = "气象区名称 = " + item.Name;
                
                foreach(var wdItem in item.WorkConditions)
                {
                    TreeViewItem wdSubItem = new TreeViewItem();
                    wdSubItem.Header = "SWorkConditionName=" + wdItem.SWorkConditionName + " SWindSpeed=" + wdItem.SWindSpeed
                        + " STemperature=" + wdItem.STemperature + " SIceThickness=" + wdItem.SIceThickness;
                    subItem.Items.Add(wdSubItem);
                }
                
                weatherTreeView.Items.Add(subItem);
            }
        }

        List<Ta> copy = new List<Ta>(); 

        private void button_DG_Copy_Click(object sender, RoutedEventArgs e)
        {
            copy.Clear();
            foreach(Ta item in dataGridTa.SelectedItems)
            {
                copy.Add(item);
            }
                
            return;
        }

        private void button_DG_Cut_Click(object sender, RoutedEventArgs e)
        {
            copy.Clear();
            Ta item = (Ta)dataGridTa.SelectedItem;
            int cnt = dataGridTa.SelectedItems.Count;
            memberData.RemoveRange(item.Index-1,cnt);

            for(int i = 0; i < memberData.Count; i++)
            {
                memberData[i].Index = i + 1;
            }

            dataGridTa.DataContext = memberData;
            dataGridTa.Items.Refresh();

            return;
        }

        private void button_DG_Paste_Click(object sender, RoutedEventArgs e)
        {
            Ta item = (Ta)dataGridTa.SelectedItem;
            memberData.InsertRange(item.Index - 1, copy);

            for (int i = 0; i < memberData.Count; i++)
            {
                memberData[i].Index = i + 1;
            }

            dataGridTa.DataContext = memberData;
            dataGridTa.Items.Refresh();

            return;
        }

        TaStructure taStructureReader = new TaStructure();

        private void button_ReadMD_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "Excel Files (*.xml)|*.xml"
            };

            if(openFileDialog.ShowDialog() == true)
            {
                taStructureReader = TaStructureReader.Read(openFileDialog.FileName);
            }

            return;
        }

        private void button_SaveMD_Click(object sender, RoutedEventArgs e)
        {

            var saveFileDialog = new Microsoft.Win32.SaveFileDialog()
            {
                Filter = "Md Files (*.xml)|*.xml",
                InitialDirectory = @"D:\"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                TaStructureReader.Save(saveFileDialog.FileName, taStructureReader);
            }

            return;
        }


        private void button_ReadTaTemplate_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "Excel Files (*.dat)|*.dat"
            };
            var result = openFileDialog.ShowDialog();

            if (result != true)
                return;
            TaTemplate template = new TaTemplate();

            Button btn = (Button)sender;
            if (btn.Name == "ReadLineTemplateBtn")
            {
                LineTowerTemplateReader taTemplateReader = new LineTowerTemplateReader();
                template = taTemplateReader.Read(openFileDialog.FileName);

            }
            else if (btn.Name == "ReadCornerTemplateBtn")
            {
                CornerTowerTemplateReader taTemplateReader = new CornerTowerTemplateReader();
                template = taTemplateReader.Read(openFileDialog.FileName);
            }
            else if (btn.Name == "ReadLineCornerTemplateBtn")
            {
                LineCornerTowerTemplateReader taTemplateReader = new LineCornerTowerTemplateReader();
                template = taTemplateReader.Read(openFileDialog.FileName);
            }
            else if (btn.Name == "ReadTerminalTemplateBtn")
            {
                TerminalTowerTemplateReader taTemplateReader = new TerminalTowerTemplateReader();
                template = taTemplateReader.Read(openFileDialog.FileName);
            }
            else 
            {
                BranchTowerTemplateReader taTemplateReader = new BranchTowerTemplateReader();
                template = taTemplateReader.Read(openFileDialog.FileName);
            }

            tabs.SelectedIndex = 5;

            nameLabel.Content = template.Name;

            string wire = "";
            foreach (var item in template.Wires)
            {
                if (wire == "")
                {
                    wire = item;
                }
                else
                {
                    wire = wire + " " + item;
                }
            }
            wireLabel.Content = wire;

            string wd = "";
            foreach (var item in template.WorkConditongs)
            {
                if (wd == "")
                {
                    wd = item.Key.ToString() + "." + item.Value;
                }
                else
                {
                    wd = wd + " " + item.Key.ToString() + "." + item.Value;
                }
            }
            wdLabel.Content = wd;



        }
    }

}
