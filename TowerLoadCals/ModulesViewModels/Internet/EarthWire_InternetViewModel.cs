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
    public class EarthWire_InternetViewModel : ViewModelBase
    {
        EarthWireService  earthWireService = new EarthWireService();//数据交互层

        /// <summary>
        /// 查询事件
        /// </summary>
        public DelegateCommand SearchCommand { get; private set; }

        /// <summary>
        /// 导出事件
        /// </summary>
        public DelegateCommand ExportCommand { get; private set; }

        public EarthWire_InternetViewModel()
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
                this.DataSource = new ObservableCollection<EarthWire>(earthWireService.GetList().Where(item => item.ModelSpecification.Contains(searchInfo)).ToList());
            else
                this.DataSource = new ObservableCollection<EarthWire>(earthWireService.GetList());
        }

        public void doExportData()
        {

            int[] idStr = { 1, 2, 3 };

            IList<EarthWire> list = earthWireService.GetList().Where(item => idStr.Contains(item.Id)).ToList();//需要下载的数据

            //加载xml文件
            XmlDocument doc = new XmlDocument();
            doc.Load(@"C:\Users\Administrator\Desktop\杆塔负荷协同程序\BaseData\Wire.xml");

            XmlElement ele = doc.DocumentElement;
            //2：得到导线节点
            //3：判断是否存在相同型号 不重复直接新增
            //4：保存新文件

            XmlNode wireNode = doc.GetElementsByTagName("WireType")[0];

            foreach (EarthWire wire in list)
            {
                XmlElement row = doc.CreateElement("Wire");
                row.SetAttribute("ModelSpecification", wire.ModelSpecification);
                row.SetAttribute("WireType", wire.WireType);
                row.SetAttribute("SectionArea", wire.SectionArea.ToString());
                row.SetAttribute("ExternalDiameter", wire.ExternalDiameter.ToString());
                row.SetAttribute("UnitLengthMass", wire.UnitLengthMass);
                row.SetAttribute("DCResistor", wire.DCResistor);
                row.SetAttribute("RatedBreakingForce", wire.RatedBreakingForce);
                row.SetAttribute("ModulusElasticity", wire.ModulusElasticity.ToString());
                row.SetAttribute("LineCoefficient", wire.LineCoefficient);
                wireNode.AppendChild(row);
            }
            doc.Save(@"C:\Users\Administrator\Desktop\杆塔负荷协同程序\BaseData\Wire.xml");


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
        public ObservableCollection<EarthWire> DataSource { get; set; }

        #endregion

    }
}
