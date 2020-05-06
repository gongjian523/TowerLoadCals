using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TowerLoadCals.Common;
using TowerLoadCals.Common.Utils;
using TowerLoadCals.DAL;
using TowerLoadCals.DAL.Electric;
using TowerLoadCals.Mode;
using TowerLoadCals.Mode.Electric;
using static TowerLoadCals.DAL.TowerTemplateReader;

namespace TowerLoadCals.Demo
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
            NodeXml root = XmlUtils.ReadXml(openFileDialog.FileName);//记录用户选择的文件路径

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
            
            DataTable DT = DbfUtils.ReadDbf(openFileDialog.FileName.Substring(0, openFileDialog.FileName.Length-1- openFileDialog.SafeFileName.Length), openFileDialog.SafeFileName);//记录用户选择的文件路径
            
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

                WeatherReader reader = WeatherReaderFactory.CreateReader("DBF");

                List<Weather> weatherList = reader.Read(openFileDialog.FileName.Substring(0, openFileDialog.FileName.Length - 1 - openFileDialog.SafeFileName.Length), openFileDialog.SafeFileName);

                weathers.Weathers.AddRange(weatherList);
                UpdateWeatherTreeView();
            }
            else
            {
                tabs.SelectedIndex = 1;

                List<TowerStPra> memberData = TowerStPraReader.Read(openFileDialog.FileName.Substring(0, openFileDialog.FileName.Length - 1 - openFileDialog.SafeFileName.Length), openFileDialog.SafeFileName);

                dataGridSLTa.DataContext = memberData;
                dataGridSLTa.Items.Refresh();
            }
            
        }

        private List<TowerSeri> memberData = new List<TowerSeri>();

        private void button_ReadTa_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "Excel Files (*.ta)|*.ta"
            };
            var result = openFileDialog.ShowDialog();
            if (result != true)
                return;

            List<TowerSeri> towerSeriList = TowerSeriReader.ReadTa(openFileDialog.FileName);//记录用户选择的文件路径

            memberData.Clear();
            memberData.AddRange(towerSeriList);


            tabs.SelectedIndex = 4;

            dataGridTa.DataContext = memberData;
            dataGridTa.Items.Refresh();
        }

        private List<Weather> listWeathers = new List<Weather>();

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

            WeatherReader weatherReader = WeatherReaderFactory.CreateReader("XML");
            listWeathers = weatherReader.Read(openFileDialog.FileName);

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

        private void button_SaveWeatherXML_Click(object sender, RoutedEventArgs e)
        {

            var saveFileDialog = new Microsoft.Win32.SaveFileDialog()
            {
                Filter = "Md Files (*.xml)|*.xml",
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                //WeatherReader weatherReader = WeatherReaderFactory.CreateReader("XML");
                //weatherReader.Save(saveFileDialog.FileName, listWeathers);

                XmlUtils.Save(saveFileDialog.FileName, listWeathers[0]);

                //XmlSerializer xs = new XmlSerializer(typeof(Weather));
                //StreamWriter sw = new StreamWriter(saveFileDialog.FileName);
                //xs.Serialize(sw, listWeathers[0]);
                //sw.Close();
            }

            return;
        }


        private void button_ReadLocalWeatherXML_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "Excel Files (*.xml)|*.xml"
            };
            var result = openFileDialog.ShowDialog();

            tabs.SelectedIndex = 0;

            if (result != true)
                return;

            listWeathers.Clear();
            //listWeathers = XmlUtils.Read<List<Weather>>(openFileDialog.FileName);

            WeatherReader weatherReader = WeatherReaderFactory.CreateReader("XML");
            listWeathers = weatherReader.ReadLocal(openFileDialog.FileName);

            if (listWeathers.Count > 0)
                weathers.Weathers.AddRange(listWeathers);

            UpdateWeatherTreeView();
        }


        List<TowerSeri> copy = new List<TowerSeri>(); 

        private void button_DG_Copy_Click(object sender, RoutedEventArgs e)
        {
            copy.Clear();
            foreach(TowerSeri item in dataGridTa.SelectedItems)
            {
                copy.Add(item);
            }
                
            return;
        }

        private void button_DG_Cut_Click(object sender, RoutedEventArgs e)
        {
            copy.Clear();
            TowerSeri item = (TowerSeri)dataGridTa.SelectedItem;
            int cnt = dataGridTa.SelectedItems.Count;
            memberData.RemoveRange(item.ID-1,cnt);

            for(int i = 0; i < memberData.Count; i++)
            {
                memberData[i].ID = i + 1;
            }

            dataGridTa.DataContext = memberData;
            dataGridTa.Items.Refresh();

            return;
        }

        private void button_DG_Paste_Click(object sender, RoutedEventArgs e)
        {
            TowerSeri item = (TowerSeri)dataGridTa.SelectedItem;
            memberData.InsertRange(item.ID - 1, copy);

            for (int i = 0; i < memberData.Count; i++)
            {
                memberData[i].ID = i + 1;
            }

            dataGridTa.DataContext = memberData;
            dataGridTa.Items.Refresh();

            return;
        }

        TowerStructure taStructureReader = new TowerStructure();

        private void button_ReadMD_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "Excel Files (*.xml)|*.xml"
            };

            if(openFileDialog.ShowDialog() == true)
            {
                taStructureReader = TowerStructureReader.Read(openFileDialog.FileName);
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
                TowerStructureReader.Save(saveFileDialog.FileName, taStructureReader);
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
            TowerTemplate template = new TowerTemplate();

            Button btn = (Button)sender;
            TowerTemplateReader taTemplateReader;
            if (btn.Name == "ReadLineTemplateBtn")
            {
                taTemplateReader = new TowerTemplateReader(TowerType.LineTower);
            }
            else if (btn.Name == "ReadCornerTemplateBtn")
            {
                taTemplateReader = new TowerTemplateReader(TowerType.CornerTower);
            }
            else if (btn.Name == "ReadLineCornerTemplateBtn")
            {
                taTemplateReader = new TowerTemplateReader(TowerType.LineCornerTower);
            }
            else if (btn.Name == "ReadTerminalTemplateBtn")
            {
                taTemplateReader = new TowerTemplateReader(TowerType.TerminalTower);
            }
            else 
            {
                taTemplateReader = new TowerTemplateReader(TowerType.BranchTower);
            }
            template = taTemplateReader.Read(openFileDialog.FileName);

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

        private void button_DesDecrypt_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "DLL Files (*.dat)|*.dat"
            };

            if (openFileDialog.ShowDialog() != true)
                return;

            // Read the file and display it line by line.  
            //System.IO.StreamReader file = new System.IO.StreamReader(openFileDialog.FileName, Encoding.Default);
            //string content = file.ReadToEnd();
            //file.Close();

            //string encryptString = DES.DesEncrypt(content, "12345678");

            var saveFileDialog = new Microsoft.Win32.SaveFileDialog()
            {
                Filter = "Dat Files (*.dll)|*.dll",
            };

            if (saveFileDialog.ShowDialog() != true)
                return;

            //System.IO.StreamWriter sfile = new System.IO.StreamWriter(saveFileDialog.FileName);
            //sfile.Flush();
            //sfile.Write(encryptString);
            //sfile.Close();

            DES.DesEncrypt(openFileDialog.FileName, saveFileDialog.FileName, "12345678");

        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_DesEncrypt_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "DLL Files (*.dll)|*.dll"
            };

            if (openFileDialog.ShowDialog() != true)
                return;

            // Read the file and display it line by line.  
            //System.IO.StreamReader file = new System.IO.StreamReader(openFileDialog.FileName, Encoding.Default);
            //string content = file.ReadToEnd();
            //file.Close();

            //string decryptString = DES.DesDecrypt(content, "12345678");

            var saveFileDialog = new Microsoft.Win32.SaveFileDialog()
            {
                Filter = "Dat Files (*.dat)|*.dat",
            };

            if (saveFileDialog.ShowDialog() != true)
                return;

            //System.IO.StreamWriter sfile = new System.IO.StreamWriter(saveFileDialog.FileName);
            //sfile.Flush();
            //sfile.Write(decryptString);
            //sfile.Close();

            DES.DesDecrypt(openFileDialog.FileName, saveFileDialog.FileName, "12345678");
        }


        public class student
        {
            public string studentId { get; set; }
        }

        public class Input
        {
            public string token { get; set; }
            public int x { get; set; }
            public int y { get; set; }
        }

        public class Output
        {
            public string status { get; set; }
            public string engine { get; set; }
            public int msg { get; set; }
        }

        private void button_PathonServer_Click(object sender, RoutedEventArgs e)
        {
            JsonSerializerSettings jsetting = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            //student st = new student
            //{
            //    studentId = "28845"
            //};
            //string JsonBody = JsonConvert.SerializeObject(st, Formatting.Indented, jsetting);
            //string str = HttpUtils.GetResponseString(HttpUtils.CreatePostHttpResponse("http://47.106.125.192:8082/umjapi/v2/count/listApp", JsonBody, null));
            //tbResult.Text = str;
            Button btn = (Button)sender;

            string strUrl = "http://137.168.101.235:9898/calOnline";
            Input input = new Input
            {
                token = "SANWEIYANFA@321$",
                x = Convert.ToInt16(tb1.Text.ToString()),
                y = Convert.ToInt16(tb2.Text.ToString())
            };

            tabs.SelectedIndex = 6;

            Output[] ops = HttpUtils.HttpPost<Output[], Input>(strUrl, input);
            tbResult.Text = ops[0].msg.ToString();
        }

        private void button_PathonLocal_Click(object sender, RoutedEventArgs e)
        {
            JsonSerializerSettings jsetting = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            string strUrl = "http://127.0.0.1:9898/calOnline";
            Input input = new Input
            {
                token = "SANWEIYANFA@321$",
                x = Convert.ToInt16(tb21.Text.ToString()),
                y = Convert.ToInt16(tb22.Text.ToString())
            };

            tabs.SelectedIndex = 7;

            Output[] ops = HttpUtils.HttpPost<Output[], Input>(strUrl, input);
            tbResult2.Text = ops[0].msg.ToString();
        }


        private void button_ReadProjectXML_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "Excel Files (*.xml)|*.xml"
            };
            var result = openFileDialog.ShowDialog();

            if (result != true)
                return;

            var list = ProInfoReader.Read(openFileDialog.FileName);
            
        }

        private void button_SaveProjectXML_Click(object sender, RoutedEventArgs e)
        {

            var saveFileDialog = new Microsoft.Win32.SaveFileDialog()
            {
                Filter = "XML Files (*.xml)|*.xml",
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                //WeatherReader weatherReader = WeatherReaderFactory.CreateReader("XML");
                //weatherReader.Save(saveFileDialog.FileName, listWeathers);

                List<ProInfo> list = new List<ProInfo>();
                list.Add(new ProInfo
                {
                    ID = 1,
                    Name = "Test",
                    FilesID = 2,
                    Stage = "Begine",
                    Volt = 220
                });

                ProInfoReader.Save(saveFileDialog.FileName, list);

                //XmlSerializer xs = new XmlSerializer(typeof(Weather));
                //StreamWriter sw = new StreamWriter(saveFileDialog.FileName);
                //xs.Serialize(sw, listWeathers[0]);
                //sw.Close();
            }

            return;
        }


        private void button_ReadWireXML_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "Xml Files (*.xml)|*.xml"
            };
            var result = openFileDialog.ShowDialog();

            if (result != true)
                return;

            var list = WireReader.Read(openFileDialog.FileName);

        }

        private void button_SaveWireXML_Click(object sender, RoutedEventArgs e)
        {

            var saveFileDialog = new Microsoft.Win32.SaveFileDialog()
            {
                Filter = "XML Files (*.xml)|*.xml",
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                //WeatherReader weatherReader = WeatherReaderFactory.CreateReader("XML");
                //weatherReader.Save(saveFileDialog.FileName, listWeathers);

                List<Wire1> list = new List<Wire1>();
                list.Add(new Wire1
                {
                    ID = 10,
                    Name = "Type1",
                    Sec = 100,
                    Dia = 200,
                    Wei = 300,
                    Elas = 2,
                    Coef = 3,
                    Fore = 4
                });

                WireReader.Save(saveFileDialog.FileName, list);

                //XmlSerializer xs = new XmlSerializer(typeof(Weather));
                //StreamWriter sw = new StreamWriter(saveFileDialog.FileName);
                //xs.Serialize(sw, listWeathers[0]);
                //sw.Close();
            }

            return;
        }


        private void button_ReadTaSeriXML_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "Xml Files (*.xml)|*.xml"
            };
            var result = openFileDialog.ShowDialog();

            if (result != true)
                return;

            var list = TowerSeriReader.Read(openFileDialog.FileName);

        }

        private void button_SaveTaSeriXML_Click(object sender, RoutedEventArgs e)
        {

            var saveFileDialog = new Microsoft.Win32.SaveFileDialog()
            {
                Filter = "XML Files (*.xml)|*.xml",
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                //WeatherReader weatherReader = WeatherReaderFactory.CreateReader("XML");
                //weatherReader.Save(saveFileDialog.FileName, listWeathers);

                List<TowerSeri> list = new List<TowerSeri>();
                list.Add(new TowerSeri
                {
                    ID = 0,
                    Num = "111",
                    PosName = "222",
                    PosOffset = "100",
                    Name = "Tower",  
                    Type = 1,   
                    Height = 0,  
                    ResID = "ReID",
                    Elevation = 0,  
                    SubOfElv = 0,
                    TotalSpan = 0,
                    FrontSpan = 0,
                    FrontHorizontalSpan = 0,
                    BackHorizontalSpan = 0,
                    HorizontalSpan = 0,
                    FrontVerticalSpan = 0,
                    BackVerticalSpan = 0,
                    VerticalSpan = "0",
                    AngelofApplication = 0,
                    FrontDRepresentSpan = 0,  
                    BackDRepresentSpan = 0,   
                    StringLength = 0,
                    FrontK = 0,
                    BackK = 0,
                    FrontWeatherID = "FrontWeatherID",
                    BackWeatherID = "BackWeatherID"
                });

                TowerSeriReader.Save(saveFileDialog.FileName, list);

                //XmlSerializer xs = new XmlSerializer(typeof(Weather));
                //StreamWriter sw = new StreamWriter(saveFileDialog.FileName);
                //xs.Serialize(sw, listWeathers[0]);
                //sw.Close();
            }

            return;
        }

        private void button_ReadStrDataXML_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "Xml Files (*.xml)|*.xml"
            };
            var result = openFileDialog.ShowDialog();

            if (result != true)
                return;

            var list = StrDataReader.Read(openFileDialog.FileName);

        }

        private void button_SaveStrDataXML_Click(object sender, RoutedEventArgs e)
        {

            var saveFileDialog = new Microsoft.Win32.SaveFileDialog()
            {
                Filter = "XML Files (*.xml)|*.xml",
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                List<StrData> list = new List<StrData>();
                list.Add(new StrData
                {
                    ID = 0,
                    Name = "string",
                    Type = "type",
                    Weight = 0,
                    FitLength = 0,
                    PieceLength = 0,
                    PieceNum = 0,
                    GoldPieceNum = 0,
                    LNum = 0,
                    DampLength = 0,
                    SuTubleLen = 0,
                    SoftLineLen = 0,
                });

                StrDataReader.Save(saveFileDialog.FileName, list);
            }

            return;
        }

        private void button_ReadFitDataXML_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "Xml Files (*.xml)|*.xml"
            };
            var result = openFileDialog.ShowDialog();

            if (result != true)
                return;

            var list = FitDataReader.Read(openFileDialog.FileName);

        }

        private void button_SaveFitDataXML_Click(object sender, RoutedEventArgs e)
        {

            var saveFileDialog = new Microsoft.Win32.SaveFileDialog()
            {
                Filter = "XML Files (*.xml)|*.xml",
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                List<FitData> list = new List<FitData>();
                list.Add(new FitData
                {
                    ID = 0,
                    Name = "string",
                    Type = "type",
                    Weight = 0,
                    SecWind = 0,
                });

                FitDataReader.Save(saveFileDialog.FileName, list);
            }

            return;
        }

        private void button_ReadTowerStrDataXML_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "Xml Files (*.xml)|*.xml"
            };
            var result = openFileDialog.ShowDialog();

            if (result != true)
                return;

            var list = TowerStrDataReader.Read(openFileDialog.FileName);
        }

        private void button_SaveTowerStrDataXML_Click(object sender, RoutedEventArgs e)
        {

            var saveFileDialog = new Microsoft.Win32.SaveFileDialog()
            {
                Filter = "XML Files (*.xml)|*.xml",
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                int[] arry1 = { 1, 2, 3, 4, 5 };
                int[] arry2 = { 11, 21, 31, 41, 51 };

                List<TowerStrData> list = new List<TowerStrData>();
                list.Add(new TowerStrData
                {
                    ID = 0,
                    VoltageLevel = 220,
                    Name = "string",
                    Type = 1,
                    CirNum = 1,
                    CurType = 2,
                    CalHeight = 3,
                    MinHeight = 220,
                    MaxHeight = 228,
                    AllowedHorSpan = 1,
                    OneSideMinHorSpan = 1,
                    OneSideMaxHorSpan = 0,
                    AllowedVerSpan = 0,
                    OneSideMinVerSpan = 220,
                    OneSideUpVerSpanMin = 1,
                    OneSideUpVerSpanMax = 1,
                    MinAngel = 0,
                    MaxAngel = 0,
                    DRepresentSpanMin = 220,
                    DRepresentSpanMax = 1,
                    StrHeightSer = String.Join(",", arry1.Select(p => p.ToString()).ToArray()),
                    StrAllowHorSpan = String.Join(",", arry2.Select(p => p.ToString()).ToArray()),
                    AngelToHorSpan = 0,
                    MaxAngHorSpan = 220,
                });

                TowerStrDataReader.Save(saveFileDialog.FileName, list);
            }

            return;
        }
    }

}
