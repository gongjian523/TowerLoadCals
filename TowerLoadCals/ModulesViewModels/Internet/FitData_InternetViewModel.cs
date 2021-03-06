﻿using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TowerLoadCals.Mode.Internet;
using TowerLoadCals.Service.Internet;

namespace TowerLoadCals.ModulesViewModels.Internet
{
    public class FitData_InternetViewModel : ViewModelBase
    {
        FitDataService fitDataService = new FitDataService();//数据交互层

        /// <summary>
        /// 查询事件
        /// </summary>
        public DelegateCommand SearchCommand { get; private set; }

        /// <summary>
        /// 导出事件
        /// </summary>
        public DelegateCommand ExportCommand { get; private set; }

        public FitData_InternetViewModel()
        {
            doSearch();
            SearchCommand = new DelegateCommand(doSearch);
            ExportCommand = new DelegateCommand(doExportData);

        }
        /// <summary>
        /// 查询方法
        /// </summary>
        public void doSearch()
        {
            if (!string.IsNullOrEmpty(searchInfo))
                this.DataSource = new ObservableCollection<GeneralInsulator>(fitDataService.GetList().Where(item => item.Name.Contains(searchInfo)).ToList());
            else
                this.DataSource = new ObservableCollection<GeneralInsulator>(fitDataService.GetList());
        }


        public void doExportData()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(@"C:\Users\Administrator\Desktop\杆塔负荷协同程序\BaseData\Wire.xml");
            // 得到根节点bookstore

            XmlNode xn = doc.SelectSingleNode("Root");

            //得到根节点的所有子节点
            XmlNodeList xnl = xn.ChildNodes;

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
        /// 发送消息
        /// </summary>
        public ObservableCollection<GeneralInsulator> DataSource { get; set; }

        #endregion

    }
}
