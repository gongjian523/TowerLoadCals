using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using TowerLoadCals.BLL;
using TowerLoadCals.Mode;
using TowerLoadCals.Mode.Internet;
using TowerLoadCals.Service.Internet;


/// <summary>
/// created by :glj
/// </summary>

namespace TowerLoadCals.ModulesViewModels.Internet
{
    /// <summary>
    /// 导线
    /// </summary>
    public class StruCalsLibExtralLoadViewModel : ViewModelBase
    {
        StruCalsLibExtralLoadDataService struCalsLibExtralLoadDataService = new StruCalsLibExtralLoadDataService();//数据交互层

        /// <summary>
        /// 导出事件
        /// </summary>
        public DelegateCommand ExportCommand { get; private set; }

        GlobalInfo globalInfo;//获取文件保存地址
        public StruCalsLibExtralLoadViewModel()
        {
            this.DataSource = new ObservableCollection<StruCalsLibExtralLoad>(struCalsLibExtralLoadDataService.GetList());

            ExportCommand = new DelegateCommand(doExportData);
            globalInfo = GlobalInfo.GetInstance();

        }

        public void doExportData()
        {
            try
            {
                //需要下载的数据
                IList<StruCalsLibExtralLoad> list = DataSource.Where(item => item.IsSelected == true).ToList();

                //文件地址
                string path = Directory.GetCurrentDirectory() + "\\" + ConstVar.UserDataStr + "\\" + ConstVar.StruCalsLibFileName;

                //加载xml文件
                XmlDocument doc = new XmlDocument();
                doc.Load(path);

                //2：得到导线节点
                //3：判断是否存在相同型号 不重复直接新增
                //4：保存新文件

                XmlNode rootNode = doc.GetElementsByTagName("附加荷载参数列表")[0];

                bool notExists = true;
                foreach (StruCalsLibExtralLoad item in list)
                {
                    notExists = true;
                    foreach (XmlNode xmlNode in rootNode.ChildNodes)
                    {
                        if (xmlNode.Attributes.GetNamedItem("序号").InnerText == item.Index.ToString())
                        {
                            notExists = false;
                            DialogResult dr = MessageBox.Show(string.Format("已经存在电压等级(kv)为【{0}】相同的信息，是否替换？", item.Voltage), "重复确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                            if (dr == DialogResult.OK)
                            {
                                xmlNode.Attributes.GetNamedItem("序号").InnerText = item.Index.ToString();
                                xmlNode.Attributes.GetNamedItem("电压等级").InnerText = item.Voltage.ToString();
                                xmlNode.Attributes.GetNamedItem("铁塔安装重要性系数").InnerText = item.InstallImportanceCoef.ToString();
                                xmlNode.Attributes.GetNamedItem("铁塔其他重要性系数").InnerText = item.OtherImportanceCoef.ToString();
                                xmlNode.Attributes.GetNamedItem("悬垂塔地线附加荷载").InnerText = item.OverhangingTowerEarthWireExtraLoad.ToString();
                                xmlNode.Attributes.GetNamedItem("悬垂塔导线附加荷载").InnerText = item.OverhangingTowerWireExtraLoad.ToString();
                                xmlNode.Attributes.GetNamedItem("耐张塔地线附加荷载").InnerText = item.TensionTowerEarthWireExtraLoad.ToString();
                                xmlNode.Attributes.GetNamedItem("耐张塔导线附加荷载").InnerText = item.TensionTowerWireExtraLoad.ToString();
                                xmlNode.Attributes.GetNamedItem("耐张塔跳线附加荷载").InnerText = item.TensionTowerJumperWireExtraLoad.ToString();
                            }
                            break;
                        }
                    }
                    if (notExists)
                    {
                        XmlElement row = doc.CreateElement("附加荷载参数");
                        row.SetAttribute("序号", item.Index.ToString());
                        row.SetAttribute("电压等级", item.Voltage.ToString());
                        row.SetAttribute("铁塔安装重要性系数", item.InstallImportanceCoef.ToString());
                        row.SetAttribute("铁塔其他重要性系数", item.OtherImportanceCoef.ToString());
                        row.SetAttribute("悬垂塔地线附加荷载", item.OverhangingTowerEarthWireExtraLoad.ToString());
                        row.SetAttribute("悬垂塔导线附加荷载", item.OverhangingTowerWireExtraLoad.ToString());
                        row.SetAttribute("耐张塔地线附加荷载", item.TensionTowerEarthWireExtraLoad.ToString());
                        row.SetAttribute("耐张塔导线附加荷载", item.TensionTowerWireExtraLoad.ToString());
                        row.SetAttribute("耐张塔跳线附加荷载", item.TensionTowerJumperWireExtraLoad.ToString());
                        rootNode.AppendChild(row);
                    }
                }
                doc.Save(path);

                MessageBox.Show("批量下载成功!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("下载失败，具体原因如下:{0}!", ex.Message));
            }

        }
        /// <summary>
        /// 数据源
        /// </summary>
        private ObservableCollection<StruCalsLibExtralLoad> dataSource;

        public ObservableCollection<StruCalsLibExtralLoad> DataSource
        {
            get { return dataSource; }
            set { dataSource = value; RaisePropertyChanged(() => DataSource); }
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        private bool isEnabledExport;
        public bool IsEnabledExport
        {
            get { return isEnabledExport = File.Exists(Directory.GetCurrentDirectory() + "\\" + ConstVar.UserDataStr + "\\" + ConstVar.StruCalsLibFileName); }
            set { isEnabledExport = value; RaisePropertyChanged(() => IsEnabledExport); }
        }

    }
}
