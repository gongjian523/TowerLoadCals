using DevExpress.Mvvm;
using DevExpress.Xpf.Grid;
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


/// <summary>
/// created by :glj
/// </summary>

namespace TowerLoadCals.ModulesViewModels.Internet
{
    public class WeatherCondition_InternetViewModel : ViewModelBase
    {
        WeatherConditionService weatherConditionService = new WeatherConditionService();//数据交互层

        /// <summary>
        /// 查询事件
        /// </summary>
        public DelegateCommand SearchCommand { get; private set; }

        /// <summary>
        /// 导出事件
        /// </summary>
        public DelegateCommand ExportCommand { get; private set; }

        /// <summary>
        /// 选中按钮事件
        /// </summary>
        public DelegateCommand CheckCommand { get; private set; }

        GlobalInfo globalInfo;//获取文件保存地址
        public WeatherCondition_InternetViewModel()
        {
            doSearch();
            SearchCommand = new DelegateCommand(doSearch);
            ExportCommand = new DelegateCommand(doExportData);
            //ExportCommand = new DelegateCommand(doCheckData);
            globalInfo = GlobalInfo.GetInstance();

        }
        /// <summary>
        /// 查询方法
        /// </summary>
        public void doSearch()
        {
            if (!string.IsNullOrEmpty(searchInfo))
            {
                List<string> Str = searchInfo.Trim().Split(new[] { " " }, StringSplitOptions.None).Where(s => !string.IsNullOrEmpty(s)).ToList();

                if (Str != null && Str.Count > 0)
                {
                    IList<WorkConditionCollections> list = weatherConditionService.GetList();

                    this.DataSource = new ObservableCollection<WorkConditionCollections>(from data in list
                                                                                         from searchInfo in Str
                                                                                         where
                                                                                             data.CategoryName.Contains(searchInfo)
                                                                                         select data);
                }
                else
                    this.DataSource = new ObservableCollection<WorkConditionCollections>(weatherConditionService.GetList());
            }
            else
                this.DataSource = new ObservableCollection<WorkConditionCollections>(weatherConditionService.GetList());
        }
        /// <summary>
        /// 选中按钮
        /// </summary>
        public void doCheckData(int id)
        {
            int CategoryId = DataSource.Where(item => item.Id == id).FirstOrDefault().CategoryId;

            foreach (var item in DataSource)
            {
                if (item.CategoryId == CategoryId)
                {
                    item.IsSelected = true;
                }
            }
            this.dataSource = DataSource;
        }

        public void doExportData()
        {
            try
            {
                //需要下载的数据
                IList<WorkConditionCollections> list = DataSource.Where(item => item.IsSelected == true).ToList();
                var groups = list.GroupBy(item => item.CategoryName);

                //文件地址
                string path = globalInfo.ProjectPath + "\\BaseData\\WeatherCondition.xml";

                //加载xml文件
                XmlDocument doc = new XmlDocument();
                doc.Load(path);

                XmlNode rootNode = doc.GetElementsByTagName("Root")[0];
                IList<WorkConditionCollections> groupList = null;

                bool toAddNew = false;
                foreach (var group in groups)
                {
                    toAddNew = true;
                    foreach (XmlNode xmlNode in rootNode.ChildNodes)//循环冰区大类，查找是否已经存在大类
                    {
                        if (xmlNode.Attributes.GetNamedItem("SName").InnerText == group.Key)
                        {
                            DialogResult dr = MessageBox.Show(string.Format("已经存在冰区为【{0}】相同的信息，是否替换？", group.Key), "重复确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                            if (dr == DialogResult.OK)
                            {
                                toAddNew = true;
                                rootNode.RemoveChild(xmlNode);
                            }
                            else
                                toAddNew = false;


                            break;
                        }
                    }

                    if (toAddNew)
                    {
                        groupList = weatherConditionService.GetList().Where(item => item.CategoryName == group.Key).ToList();
                        XmlElement parentElement = doc.CreateElement("KNode");
                        parentElement.SetAttribute("SName", group.Key);
                        foreach (var item in groupList)
                        {
                            XmlElement childElement = doc.CreateElement("KNode");
                            childElement.SetAttribute("SIceThickness", item.SIceThickness.ToString());
                            childElement.SetAttribute("STemperature", item.STemperature.ToString());
                            childElement.SetAttribute("SWindSpeed", item.SWindSpeed.ToString());
                            childElement.SetAttribute("SWorkConditionName", item.SWorkConditionName.ToString());
                            parentElement.AppendChild(childElement);
                        }
                        rootNode.AppendChild(parentElement);
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

        public void CheckBox(GridGroupValueData data)
        {
            var list = DataSource.Where(item => item.CategoryName == data.Text).ToList();
            list.ForEach(item => item.IsSelected = !item.IsSelected);
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
        private ObservableCollection<WorkConditionCollections> dataSource;

        public ObservableCollection<WorkConditionCollections> DataSource
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
            get { return isEnabledExport = File.Exists(globalInfo.ProjectPath + "\\BaseData\\WeatherCondition.xml"); }
            set { isEnabledExport = value; RaisePropertyChanged(() => IsEnabledExport); }
        }

        #endregion

    }
}
