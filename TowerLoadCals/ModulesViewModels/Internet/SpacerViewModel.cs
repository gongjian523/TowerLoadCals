﻿using DevExpress.Mvvm;
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
    /// <summary>
    /// 间隔棒
    /// </summary>
    public class SpacerViewModel : ViewModelBase
    {
        SpacerService spacerService = new SpacerService();//数据交互层

        /// <summary>
        /// 查询事件
        /// </summary>
        public DelegateCommand SearchCommand { get; private set; }

        /// <summary>
        /// 导出事件
        /// </summary>
        public DelegateCommand ExportCommand { get; private set; }

        GlobalInfo globalInfo;//获取文件保存地址
        public SpacerViewModel()
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
            {
                List<string> Str = searchInfo.Trim().Split(new[] { " " }, StringSplitOptions.None).Where(s => !string.IsNullOrEmpty(s)).ToList();

                if (Str != null && Str.Count > 0)
                {
                    IList<Spacer> list = spacerService.GetList();

                    this.DataSource = new ObservableCollection<Spacer>(from data in list
                                                                                     from searchInfo in Str
                                                                                     where
                                                                                         data.Name.Contains(searchInfo)
                                                                                         || data.CategorySub.Contains(searchInfo)
                                                                                     select data);
                }
                else
                    this.DataSource = new ObservableCollection<Spacer>(spacerService.GetList());
            }
            else
                this.DataSource = new ObservableCollection<Spacer>(spacerService.GetList());
        }


        public void doExportData()
        {
            try
            {
                //需要下载的数据
                IList<Spacer> list = DataSource.Where(item => item.IsSelected == true).ToList();

                //文件地址
                string path = globalInfo.ProjectPath + "\\BaseData\\FitData.xml";

                //加载xml文件
                XmlDocument doc = new XmlDocument();
                doc.Load(path);


                XmlNode rootNode = doc.GetElementsByTagName("FitDataCollection")[1];
                bool notExists = true;
                foreach (Spacer item in list)
                {
                    notExists = true;
                    XmlNodeList abc = rootNode.ChildNodes;
                    foreach (XmlNode xmlNode in abc)
                    {
                        if (xmlNode.Attributes.GetNamedItem("Name").InnerText == item.Name)
                        {
                            notExists = false;
                            DialogResult dr = MessageBox.Show(string.Format("已经存在名称为【{0}】相同的信息，是否替换？", item.Name), "重复确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                            if (dr == DialogResult.OK)
                            {
                                xmlNode.Attributes.GetNamedItem("Name").InnerText = item.Name;
                                xmlNode.Attributes.GetNamedItem("Model").InnerText = item.CategorySub;
                                xmlNode.Attributes.GetNamedItem("Weight").InnerText = item.Weight.ToString();
                                xmlNode.Attributes.GetNamedItem("Voltage").InnerText = item.Voltage.ToString();
                                xmlNode.Attributes.GetNamedItem("SecWind").InnerText = item.SecWind.ToString();
                            }
                            break;
                        }
                    }
                    if (notExists)
                    {
                        XmlElement row = doc.CreateElement("FitData");
                        row.SetAttribute("Name", item.Name);//名称
                        row.SetAttribute("Model", item.CategorySub);//型号
                        row.SetAttribute("Weight", item.Weight.ToString());//重量
                        row.SetAttribute("Voltage", item.Voltage.ToString());//电压等级
                        row.SetAttribute("SecWind", item.SecWind.ToString());//受风面积
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
        private ObservableCollection<Spacer> dataSource;

        public ObservableCollection<Spacer> DataSource
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
            get { return isEnabledExport = File.Exists(globalInfo.ProjectPath + "\\BaseData\\FitData.xml"); }
            set { isEnabledExport = value; RaisePropertyChanged(() => IsEnabledExport); }
        }

        #endregion

    }
}
