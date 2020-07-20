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

namespace TowerLoadCals.ModulesViewModels.Internet
{
    /// <summary>
    /// 导线
    /// </summary>
    public class StruCalsLibIceCoverViewModel : ViewModelBase
    {
        StruCalsLibIceCoverDataService struCalsLibIceCoverDataService = new StruCalsLibIceCoverDataService();//数据交互层

        /// <summary>
        /// 导出事件
        /// </summary>
        public DelegateCommand ExportCommand { get; private set; }

        GlobalInfo globalInfo;//获取文件保存地址
        public StruCalsLibIceCoverViewModel()
        {
            this.DataSource = new ObservableCollection<StruCalsLibIceCover>(struCalsLibIceCoverDataService.GetList());

            ExportCommand = new DelegateCommand(doExportData);
            globalInfo = GlobalInfo.GetInstance();

        }

        public void doExportData()
        {
            try
            {
                //需要下载的数据
                IList<StruCalsLibIceCover> list = DataSource.Where(item => item.IsSelected == true).ToList();

                //文件地址
                string path = Directory.GetCurrentDirectory() + "\\" + ConstVar.UserDataStr + "\\" + ConstVar.StruCalsLibFileName;

                //加载xml文件
                XmlDocument doc = new XmlDocument();
                doc.Load(path);

                //2：得到导线节点
                //3：判断是否存在相同型号 不重复直接新增
                //4：保存新文件

                XmlNode rootNode = doc.GetElementsByTagName("覆冰参数列表")[0];

                bool notExists = true;
                foreach (StruCalsLibIceCover item in list)
                {
                    notExists = true;
                    foreach (XmlNode xmlNode in rootNode.ChildNodes)
                    {
                        if (xmlNode.Attributes.GetNamedItem("序号").InnerText == item.Index.ToString())
                        {
                            DialogResult dr = MessageBox.Show(string.Format("已经存在序号为【{0}】相同的信息，是否替换？", item.Index), "重复确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                            if (dr == DialogResult.OK)
                            {
                                notExists = false;
                                xmlNode.Attributes.GetNamedItem("序号").InnerText = item.Index.ToString();
                                xmlNode.Attributes.GetNamedItem("覆冰厚度").InnerText = item.IceThickness.ToString();
                                xmlNode.Attributes.GetNamedItem("塔身风荷载增大系数").InnerText = item.TowerWindLoadAmplifyCoef.ToString();
                                xmlNode.Attributes.GetNamedItem("塔身垂荷增大系数").InnerText = item.TowerGravityLoadAmplifyCoef.ToString();
                                break;
                            }

                        }
                    }
                    if (notExists)
                    {
                        XmlElement row = doc.CreateElement("覆冰参数");
                        row.SetAttribute("序号", item.Index.ToString());
                        row.SetAttribute("覆冰厚度", item.IceThickness.ToString());
                        row.SetAttribute("塔身风荷载增大系数", item.TowerWindLoadAmplifyCoef.ToString());
                        row.SetAttribute("塔身垂荷增大系数", item.TowerGravityLoadAmplifyCoef.ToString());
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
        /// <summary>
        /// 数据源
        /// </summary>
        private ObservableCollection<StruCalsLibIceCover> dataSource;

        public ObservableCollection<StruCalsLibIceCover> DataSource
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
