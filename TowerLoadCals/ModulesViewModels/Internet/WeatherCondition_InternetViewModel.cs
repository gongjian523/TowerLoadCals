using DevExpress.Mvvm;
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

        public WeatherCondition_InternetViewModel()
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
                this.DataSource = new ObservableCollection<WorkConditionInternet>(weatherConditionService.GetList().Where(item => item.SWorkConditionName.Contains(searchInfo)).ToList());
            else 
                this.DataSource = new ObservableCollection<WorkConditionInternet>(weatherConditionService.GetList());
        }

        public void doExportData()
        {

            //int[] idStr = { 1, 2, 3 };

            //IList<WorkConditionInternet> list = weatherConditionService.GetList().Where(item => idStr.Contains(item.CategoryId)).ToList();//需要下载的数据

            ////加载xml文件
            //XmlDocument doc = new XmlDocument();
            //doc.Load(@"C:\Users\Administrator\Desktop\杆塔负荷协同程序\BaseData\WeatherCondition.xml");

            //XmlElement ele = doc.DocumentElement;
            ////2：得到导线节点
            ////3：判断是否存在相同型号 不重复直接新增
            ////4：保存新文件

            //for(int i=0;i<idStr.Length;i++)
            //{
            //    XmlElement categoryElement = doc.CreateElement("KNode");

            //    foreach (WorkConditionInternet wire in list)
            //    {
            //        XmlElement row = doc.CreateElement("KNode");
            //        row.SetAttribute("SIceThickness", wire.SIceThickness.ToString());
            //        row.SetAttribute("STemperature", wire.STemperature.ToString());
            //        row.SetAttribute("SWindSpeed", wire.SWindSpeed.ToString());
            //        row.SetAttribute("SWorkConditionName", wire.SWorkConditionName);
            //        wireNode.AppendChild(row);
            //    }



            //}
            //XmlNode wireNode = doc.GetElementsByTagName("WireType")[0];

            //XmlElement xmlElement = doc.CreateElement("KNode");

            //foreach (WorkConditionInternet wire in list)
            //{ 
            //    XmlElement row = doc.CreateElement("KNode");
            //    row.SetAttribute("SIceThickness", wire.SIceThickness.ToString());
            //    row.SetAttribute("STemperature", wire.STemperature.ToString());
            //    row.SetAttribute("SWindSpeed", wire.SWindSpeed.ToString());
            //    row.SetAttribute("SWorkConditionName", wire.SWorkConditionName);
            //    wireNode.AppendChild(row);
            //}
            //doc.Save(@"C:\Users\Administrator\Desktop\杆塔负荷协同程序\BaseData\WeatherCondition.xml");

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
        public ObservableCollection<WorkConditionInternet> DataSource { get; set; }

        #endregion

    }
}
