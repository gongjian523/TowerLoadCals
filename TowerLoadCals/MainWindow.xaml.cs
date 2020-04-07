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
        public MainWindow()
        {
            InitializeComponent();
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

                ObservableCollection<Weather> memberData = new ObservableCollection<Weather>();

                foreach (DataRow row in DT.Rows)
                {
                    memberData.Add(new Weather
                    {
                        SWorkConditionName = row["工况名称"].ToString(),
                        SWindSpeed = row["风速"].ToString(),
                        STemperature = row["气温"].ToString(),
                        SIceThickness = row["冰厚"].ToString(),
                    });
                }

                dataGridWeather.DataContext = memberData;
                dataGridWeather.Items.Refresh();
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

            ObservableCollection<Ta> memberData = new ObservableCollection<Ta>();
            int i = 0;

            foreach (string str  in strList)
            {
                if (!str.Contains(","))
                    continue;
                string[] sArray = str.Split(',');
                i++;
                memberData.Add(new Ta
                {
                    Index = Convert.ToString(i),
                    TowerNum = sArray[0]
                });
            }

            tabs.SelectedIndex = 4;

            dataGridTa.DataContext = memberData;
            dataGridTa.Items.Refresh();
        }
    }
}
