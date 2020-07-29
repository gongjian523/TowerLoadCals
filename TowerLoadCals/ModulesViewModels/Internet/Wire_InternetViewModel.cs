using DevExpress.DataAccess.Native;
using DevExpress.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using TowerLoadCals.BLL;
using TowerLoadCals.Mode.Internet;
using TowerLoadCals.Service.Internet;

namespace TowerLoadCals.ModulesViewModels.Internet
{
    /// <summary>
    /// 导线
    /// </summary>
    public class Wire_InternetViewModel : ViewModelBase
    {
        WireService wireService = new WireService();//数据交互层

        /// <summary>
        /// 查询事件
        /// </summary>
        public DelegateCommand SearchCommand { get; private set; }

        /// <summary>
        /// 导出事件
        /// </summary>
        public DelegateCommand ExportCommand { get; private set; }

        GlobalInfo globalInfo;//获取文件保存地址
        public Wire_InternetViewModel()
        {
            doSearch();

            SearchCommand = new DelegateCommand(doSearch);
            ExportCommand = new DelegateCommand(doExportData);
            globalInfo = GlobalInfo.GetInstance();

        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="Para">更改数据源 导线和地线同用一个界面</param>

        public void doSearch()
        {
            if (!string.IsNullOrEmpty(SearchInfo))
                this.DataSource = new ObservableCollection<Wire>(wireService.GetList().Where(item => item.Name.Contains(SearchInfo) ||item.Category.Contains(SearchInfo)).ToList());
            else
                this.DataSource = new ObservableCollection<Wire>(wireService.GetList());
        }

        public void doExportData()
        {
            try
            {
                //需要下载的数据
                IList<Wire> list = DataSource.Where(item => item.IsSelected == true).ToList();

                //文件地址
                string path = globalInfo.ProjectPath + "\\BaseData\\Wire.xml";

                //加载xml文件
                XmlDocument doc = new XmlDocument();
                doc.Load(path);

                //2：得到导线节点
                //3：判断是否存在相同型号 不重复直接新增
                //4：保存新文件

                XmlNode rootNode = doc.GetElementsByTagName("WireType")[1];
                bool notExists = true;
                foreach (Wire item in list)
                {
                    notExists = true;
                    XmlNodeList abc = rootNode.ChildNodes;
                    foreach (XmlNode xmlNode in abc)
                    {
                        if (xmlNode.Attributes.GetNamedItem("ModelSpecification").InnerText == item.Name)
                        {
                            DialogResult dr = MessageBox.Show(string.Format("已经存在型号规格为【{0}】相同的信息，是否替换？", item.Name), "重复确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                            if (dr == DialogResult.OK)
                            {
                                notExists = false;
                                xmlNode.Attributes.GetNamedItem("ModelSpecification").InnerText = item.Name;
                                xmlNode.Attributes.GetNamedItem("WireType").InnerText = item.Category;
                                xmlNode.Attributes.GetNamedItem("SectionArea").InnerText = item.TotCroSection.ToString();
                                xmlNode.Attributes.GetNamedItem("ExternalDiameter").InnerText = item.TotDiaConductor.ToString();
                                xmlNode.Attributes.GetNamedItem("UnitLengthMass").InnerText = item.ConWeight.ToString();
                                xmlNode.Attributes.GetNamedItem("DCResistor").InnerText = item.DCRes.ToString();
                                xmlNode.Attributes.GetNamedItem("RatedBreakingForce").InnerText = item.UltTenStrength.ToString();
                                xmlNode.Attributes.GetNamedItem("ModulusElasticity").InnerText = item.ModElastioity.ToString();
                                xmlNode.Attributes.GetNamedItem("LineCoefficient").InnerText = item.CoeExpansion.ToString();
                                break;
                            }

                        }
                    }
                    if (notExists)
                    {
                        XmlElement row = doc.CreateElement("Wire");
                        row.SetAttribute("ModelSpecification", item.Name);
                        row.SetAttribute("WireType", item.Category);
                        row.SetAttribute("SectionArea", item.TotCroSection.ToString());
                        row.SetAttribute("ExternalDiameter", item.TotDiaConductor.ToString());
                        row.SetAttribute("UnitLengthMass", item.ConWeight.ToString());
                        row.SetAttribute("DCResistor", item.DCRes.ToString());
                        row.SetAttribute("RatedBreakingForce", item.UltTenStrength.ToString());
                        row.SetAttribute("ModulusElasticity", item.ModElastioity.ToString());
                        row.SetAttribute("LineCoefficient", item.CoeExpansion.ToString());
                        rootNode.AppendChild(row);
                    }
                }
                doc.Save(path);

                MessageBox.Show("下载成功!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("下载失败，具体原因如下:{0}!", ex.Message));
            }


        }

        private String searchInfo;
        /// <summary>
        /// 查询条件
        /// </summary>
        public String SearchInfo
        {
            get { return searchInfo; }
            set { searchInfo = value; RaisePropertyChanged(() => SearchInfo); }
        }

        /// <summary>
        /// 数据源
        /// </summary>
        private ObservableCollection<Wire> dataSource;

        public ObservableCollection<Wire> DataSource
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
            get { return isEnabledExport = File.Exists(globalInfo.ProjectPath + "\\BaseData\\Wire.xml"); }
            set { isEnabledExport = value; RaisePropertyChanged(() => IsEnabledExport); }
        }

    }
}
