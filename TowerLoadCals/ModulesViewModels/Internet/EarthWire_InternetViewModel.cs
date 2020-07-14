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
using TowerLoadCals.Mode.Internet;
using TowerLoadCals.Service.Internet;

namespace TowerLoadCals.ModulesViewModels.Internet
{
    /// <summary>
    /// 地线
    /// </summary>
    public class EarthWire_InternetViewModel : ViewModelBase
    {
        EarthWireService earthWireService = new EarthWireService();//数据交互层

        /// <summary>
        /// 查询事件
        /// </summary>
        public DelegateCommand SearchCommand { get; private set; }

        /// <summary>
        /// 导出事件
        /// </summary>
        public DelegateCommand ExportCommand { get; private set; }

        GlobalInfo globalInfo;//获取文件保存地址
        public EarthWire_InternetViewModel()
        {
            doSearch();
            SearchCommand = new DelegateCommand(doSearch);
            ExportCommand = new DelegateCommand(doExportData);
            globalInfo = GlobalInfo.GetInstance();

        }
        /// <summary>
        /// 查询方法
        /// </summary>
        public void doSearch()
        {
            if (!string.IsNullOrEmpty(searchInfo))
                this.DataSource = new ObservableCollection<EarthWire>(earthWireService.GetList().Where(item => item.ModelSpecification.Contains(searchInfo)).ToList());
            else
                this.DataSource = new ObservableCollection<EarthWire>(earthWireService.GetList());
        }

        public void doExportData()
        {
            try
            {
                //需要下载的数据
                IList<EarthWire> list = DataSource.Where(item => item.IsSelected == true).ToList();

                //文件地址
                string path = globalInfo.ProjectPath + "\\BaseData\\Wire.xml";

                //加载xml文件
                XmlDocument doc = new XmlDocument();
                doc.Load(path);

                //2：得到导线节点
                //3：判断是否存在相同型号 不重复直接新增
                //4：保存新文件

                XmlNode rootNode = doc.GetElementsByTagName("WireType")[0];
                bool notExists = true;
                foreach (EarthWire item in list)
                {
                    notExists = true;
                    XmlNodeList abc = rootNode.ChildNodes;
                    foreach (XmlNode xmlNode in abc)
                    {
                        if (xmlNode.Attributes.GetNamedItem("ModelSpecification").InnerText == item.ModelSpecification)
                        {
                            DialogResult dr = MessageBox.Show(string.Format("已经存在型号规格为【{0}】相同的信息，是否替换？", item.ModelSpecification), "重复确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                            if (dr == DialogResult.OK)
                            {
                                notExists = false;
                                xmlNode.Attributes.GetNamedItem("ModelSpecification").InnerText = item.ModelSpecification;
                                xmlNode.Attributes.GetNamedItem("WireType").InnerText = item.WireType;
                                xmlNode.Attributes.GetNamedItem("SectionArea").InnerText = item.SectionArea.ToString();
                                xmlNode.Attributes.GetNamedItem("ExternalDiameter").InnerText = item.ExternalDiameter.ToString();
                                xmlNode.Attributes.GetNamedItem("UnitLengthMass").InnerText = item.UnitLengthMass.ToString();
                                xmlNode.Attributes.GetNamedItem("DCResistor").InnerText = item.DCResistor.ToString();
                                xmlNode.Attributes.GetNamedItem("RatedBreakingForce").InnerText = item.RatedBreakingForce.ToString();
                                xmlNode.Attributes.GetNamedItem("ModulusElasticity").InnerText = item.ModulusElasticity.ToString();
                                xmlNode.Attributes.GetNamedItem("LineCoefficient").InnerText = item.LineCoefficient.ToString();
                                break;
                            }

                        }
                    }
                    if (notExists)
                    {
                        XmlElement row = doc.CreateElement("Wire");
                        row.SetAttribute("ModelSpecification", item.ModelSpecification);
                        row.SetAttribute("WireType", item.WireType);
                        row.SetAttribute("SectionArea", item.SectionArea.ToString());
                        row.SetAttribute("ExternalDiameter", item.ExternalDiameter.ToString());
                        row.SetAttribute("UnitLengthMass", item.UnitLengthMass.ToString());
                        row.SetAttribute("DCResistor", item.DCResistor.ToString());
                        row.SetAttribute("RatedBreakingForce", item.RatedBreakingForce.ToString());
                        row.SetAttribute("ModulusElasticity", item.ModulusElasticity.ToString());
                        row.SetAttribute("LineCoefficient", item.LineCoefficient.ToString());
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
        #region 属性
        private String searchInfo;
        /// <summary>
        /// 发送消息
        /// </summary>
        public String SearchInfo
        {
            get { return searchInfo; }
            set { searchInfo = value; RaisePropertyChanged(() => SearchInfo); }
        }

        /// <summary>
        /// 数据源
        /// </summary>
        private ObservableCollection<EarthWire> dataSource;

        public ObservableCollection<EarthWire> DataSource
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

        #endregion

    }
}
