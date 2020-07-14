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
    /// 一般子串
    /// </summary>
    public class GeneralInsulatorViewModule : ViewModelBase
    {
        GeneralInsulatorService generalInsulatorService = new GeneralInsulatorService();//数据交互层

        /// <summary>
        /// 查询事件
        /// </summary>
        public DelegateCommand SearchCommand { get; private set; }

        /// <summary>
        /// 导出事件
        /// </summary>
        public DelegateCommand ExportCommand { get; private set; }

        GlobalInfo globalInfo;//获取文件保存地址
        public GeneralInsulatorViewModule()
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
                this.DataSource = new ObservableCollection<GeneralInsulator>(generalInsulatorService.GetList().Where(item => item.Name.Contains(searchInfo)).ToList());
            else
                this.DataSource = new ObservableCollection<GeneralInsulator>(generalInsulatorService.GetList());

        }

        public void doExportData()
        {
            try
            {
                //需要下载的数据
                IList<GeneralInsulator> list = DataSource.Where(item => item.IsSelected == true).ToList();

                //文件地址
                string path = globalInfo.ProjectPath + "\\BaseData\\StrData.xml";

                //加载xml文件
                XmlDocument doc = new XmlDocument();
                doc.Load(path);


                XmlNode rootNode = doc.GetElementsByTagName("StrDataCollection")[0];
                bool notExists = true;
                foreach (GeneralInsulator item in list)
                {
                    notExists = true;
                    XmlNodeList abc = rootNode.ChildNodes;
                    foreach (XmlNode xmlNode in abc)
                    {
                        if (xmlNode.Attributes.GetNamedItem("Name").InnerText == item.Name)
                        {
                            DialogResult dr = MessageBox.Show(string.Format("已经存在名称为【{0}】相同的信息，是否替换？", item.Name), "重复确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                            if (dr == DialogResult.OK)
                            {
                                notExists = false;
                                xmlNode.Attributes.GetNamedItem("Name").InnerText = item.Name;
                                xmlNode.Attributes.GetNamedItem("StrType").InnerText = item.StrType;
                                xmlNode.Attributes.GetNamedItem("Weight").InnerText = item.Weight.ToString();
                                xmlNode.Attributes.GetNamedItem("FitLength").InnerText = item.FitLength.ToString();
                                xmlNode.Attributes.GetNamedItem("PieceLength").InnerText = item.PieceLength.ToString();
                                xmlNode.Attributes.GetNamedItem("PieceNum").InnerText = item.PieceNum.ToString();
                                xmlNode.Attributes.GetNamedItem("GoldPieceNum").InnerText = item.GoldPieceNum.ToString();
                                xmlNode.Attributes.GetNamedItem("LNum").InnerText = item.LNum.ToString();
                                xmlNode.Attributes.GetNamedItem("DampLength").InnerText = item.DampLength.ToString();
                                break;
                            }

                        }
                    }
                    if (notExists)
                    {
                        XmlElement row = doc.CreateElement("StrData");
                        row.SetAttribute("Name", item.Name);//名称
                        row.SetAttribute("StrType", item.StrType);//串类型
                        row.SetAttribute("Weight", item.Weight.ToString());//重量
                        row.SetAttribute("FitLength", item.FitLength.ToString());//长度
                        row.SetAttribute("PieceLength", item.PieceLength.ToString());//单片绝缘子长度
                        row.SetAttribute("PieceNum", item.PieceNum.ToString());//片数
                        row.SetAttribute("GoldPieceNum", item.GoldPieceNum.ToString());//金具换算片数
                        row.SetAttribute("LNum", item.LNum.ToString());//联数
                        row.SetAttribute("DampLength", item.DampLength.ToString());//阻尼线长度
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
        private ObservableCollection<GeneralInsulator> dataSource;

        public ObservableCollection<GeneralInsulator> DataSource
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
            get { return isEnabledExport = File.Exists(globalInfo.ProjectPath + "\\BaseData\\StrData.xml"); }
            set { isEnabledExport = value; RaisePropertyChanged(() => IsEnabledExport); }
        }

        #endregion

    }
}
