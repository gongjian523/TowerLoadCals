using DevExpress.Mvvm;
using DevExpress.Mvvm.Native;
using DevExpress.Mvvm.POCO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using TowerLoadCals.BLL;
using TowerLoadCals.DAL;
using TowerLoadCals.Mode;
using TowerLoadCals.Mode.Internet;
using TowerLoadCals.Modules;
using TowerLoadCals.Service.Internet;

namespace TowerLoadCals.ModulesViewModels.Internet
{
    public class StruTemplateLibGeneralViewModel : ViewModelBase
    {
        StruTemplateLibGeneralService struTemplateLibGeneralService = new StruTemplateLibGeneralService();//数据交互层

        /// <summary>
        /// 查询事件
        /// </summary>
        public DelegateCommand SearchCommand { get; private set; }

        GlobalInfo globalInfo;//获取文件保存地址
        ProjectUtils proUtils;
        public StruTemplateLibGeneralViewModel()
        {
            doSearch();
            SearchCommand = new DelegateCommand(doSearch);
            globalInfo = GlobalInfo.GetInstance();
            proUtils = ProjectUtils.GetInstance();
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
                    IList<StruTemplateLibGeneral> list = struTemplateLibGeneralService.GetList();

                    this.DataSource = new ObservableCollection<StruTemplateLibGeneral>(from data in list
                                                                       from searchInfo in Str
                                                                       where
                                                                           data.FileName.Contains(searchInfo)
                                                                           || data.Category.Contains(searchInfo)
                                                                       select data);
                }
                else
                    this.DataSource = new ObservableCollection<StruTemplateLibGeneral>(struTemplateLibGeneralService.GetList());
            }
            else
                this.DataSource = new ObservableCollection<StruTemplateLibGeneral>(struTemplateLibGeneralService.GetList());
        }
        #region 文件查看

        protected StruTemplateEditWindow editWindow;

        /// <summary>
        /// 文件查看
        /// </summary>
        /// <param name="id"></param>
        public void CheckTemplate(int id)
        {
            StruTemplateLibGeneral item = this.DataSource.Where(data => data.Id == id).SingleOrDefault();

            TowerTemplate template = new NewTowerTemplateReader(TowerTypeStringConvert.TowerStringToType(item.Category)).ReadContentStream(item.Content, item.FileName);


            StruTemplateEditViewModel model = ViewModelSource.Create(() => new StruTemplateEditViewModel(template, true));
            model.CloseEditTemplateWindowEvent += CloseTemplateEditWindow;
            editWindow = new StruTemplateEditWindow();
            editWindow.DataContext = model;
            editWindow.ShowDialog();


        }

        protected void CloseTemplateEditWindow(object sender, bool isSave = false)
        {
            StruTemplateEditViewModel model = (StruTemplateEditViewModel)sender;
            model.CloseEditTemplateWindowEvent -= CloseTemplateEditWindow;
            if (editWindow != null) editWindow.Close();
            editWindow = null;
        }
        #endregion

        #region 文件下载

        /// <summary>
        /// 文件下载
        /// </summary>
        /// <param name="id"></param>
        public void ExportTemplate(int id)
        {
            try
            {
                StruTemplateLibGeneral item = DataSource.Where(data => data.Id == id).SingleOrDefault();

                //文件地址
                string path = globalInfo.ProjectPath + @"\" + globalInfo.ProjectName + @".lcp";

                if (path == @"\.lcp")
                {
                    MessageBox.Show("选择工程后才允许下载!");
                    return;
                }
                //加载xml文件
                XmlDocument doc = new XmlDocument();
                doc.Load(path);


                XmlNode rootNode = doc.GetElementsByTagName("GeneralStruTemplate")[0];
                if (rootNode == null)
                {
                    XmlNode rootBase = doc.GetElementsByTagName("BaseData")[0];
                    rootNode = doc.CreateElement("GeneralStruTemplate");
                    rootBase.AppendChild(rootNode);
                }
                bool notExists = true;//是否已经存在该模板
                foreach (XmlNode xmlNode in rootNode.ChildNodes)
                {
                    if (xmlNode.Attributes.GetNamedItem("Name").InnerText == item.FileName)
                    {
                        notExists = false;
                        DialogResult dr = MessageBox.Show(string.Format("已经存在模板名称【{0}】相同的信息，是否替换？", item.FileName), "重复确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (dr == DialogResult.OK)
                        {
                            xmlNode.Attributes.GetNamedItem("Name").InnerText = item.FileName;
                            xmlNode.Attributes.GetNamedItem("TowerType").InnerText = item.Category;
                        }
                    }
                    break;
                }
                if (notExists)
                {
                    XmlElement row = doc.CreateElement("TowerTemplate");
                    row.SetAttribute("Name", item.FileName);
                    row.SetAttribute("TowerType", item.Category);
                    rootNode.AppendChild(row);

                    //下载具体文件
                    string NewFilePath = proUtils.GetGeneralTowerTemplatePath(item.FileName, item.Category);
                    if (File.Exists(NewFilePath))
                        File.Delete(NewFilePath);
                    using (FileStream fsWrite = new FileStream(NewFilePath, FileMode.Create, FileAccess.Write))
                    {
                        byte[] buffer = Encoding.Default.GetBytes(item.Content);
                        //使用utf-8编码格式
                        fsWrite.Write(buffer, 0, buffer.Length);
                    };
                }
                doc.Save(path);


                MessageBox.Show("下载成功!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("下载失败，具体原因如下:{0}!", ex.Message));
            }
        }

        #endregion

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
        private ObservableCollection<StruTemplateLibGeneral> dataSource;

        public ObservableCollection<StruTemplateLibGeneral> DataSource
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
            get { return isEnabledExport = File.Exists(globalInfo.ProjectPath + @"\" + globalInfo.ProjectName + @".lcp"); }
            set { isEnabledExport = value; RaisePropertyChanged(() => IsEnabledExport); }
        }

        #endregion

        public void WirteFile()
        {
            string path = @"D:\杆塔项目\other\【0715】结构计算\Tower library";

            DirectoryInfo root = new DirectoryInfo(path);
            DirectoryInfo[] dics = root.GetDirectories();

            StruTemplateLibGeneral item;
            foreach (DirectoryInfo ParentInfo in dics)
            {
                foreach (FileInfo info in ParentInfo.GetFiles("*.dat"))
                {
                    using (FileStream fsRead = new FileStream(info.FullName, FileMode.Open, FileAccess.Read))
                    {
                        item = new StruTemplateLibGeneral();

                        byte[] buffer = new byte[fsRead.Length];//
                        int r = fsRead.Read(buffer, 0, buffer.Length);//返回本次实际读取到的有效字节数

                        item.FileName = Path.GetFileNameWithoutExtension(info.FullName);
                        item.Category = ParentInfo.Name;
                        item.FileExtension = info.Extension;
                        item.Content = System.Text.Encoding.Default.GetString(buffer);//使用utf-8编码格式
                        struTemplateLibGeneralService.Add(item);
                    }
                }
            }

        }
    }
}
