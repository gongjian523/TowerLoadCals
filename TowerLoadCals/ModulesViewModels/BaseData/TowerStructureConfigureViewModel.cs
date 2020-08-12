using DevExpress.Mvvm;
using DevExpress.Mvvm.Native;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using System.Xml;
using TowerLoadCals.BLL;
using TowerLoadCals.DAL;
using TowerLoadCals.Mode;
using TowerLoadCals.ModulesViewModels;

namespace TowerLoadCals.Modules
{
    public class TowerStructureConfigureViewModel : DaseDataBaseViewModel<TowerStrData, List<TowerStrCollection>>
    {
        public DelegateCommand CopyRowCommand { get; private set; }

        protected string curType;
        protected string curName;

        private List<string> templetDataSource;
        /// <summary>
        /// 发送消息
        /// </summary>
        public List<string> TempletDataSource
        {
            get { return templetDataSource; }
            set { templetDataSource = value; RaisePropertyChanged(() => TempletDataSource); }
        }


        protected override void InitializeItemsSource()
        {
            base.InitializeItemsSource();

            filePath = globalInfo.ProjectPath + "\\BaseData\\TowerStr.xml";

            CopyRowCommand = new DelegateCommand(CopyRow);




        }

        protected override void InitializeData()
        {
            //BaseData = TowerStrDataReader.Read(filePath);


            //工程文件和通用文件模板
            IList<string> projectList = ProjectUtils.GetInstance().GetProjectTowerTemplate().Select(item => "[工程模板]" + item.Name).ToList();
            IList<string> templateList = ProjectUtils.GetInstance().GetGeneralTowerTemplate().Select(item => "[通用模板]" + item.Name).ToList();

            this.templetDataSource = projectList.Concat(templateList).ToList();


            IList<TowerStrData> list = new List<TowerStrData>();
            TowerStrData tower = new TowerStrData();
            tower.ID = 1;
            tower.Name = "直转塔杆塔型号";
            tower.Type = 1;
            tower.TempletName = "[通用模板]单回交流中冰区-低温";
            tower.ModelName = "";
            tower.HangPointName = "";
            list.Add(tower);
            TowerStrData tower1 = new TowerStrData();
            tower1.ID = 2;
            tower1.Name = "终端塔杆塔型号";
            tower1.Type = 1;
            tower1.TempletName = "[通用模板]单回直流轻中冰区";
            tower1.ModelName = "";
            tower1.HangPointName = "";
            list.Add(tower1);

            this.dataSource = new ObservableCollection<TowerStrData>(list);

        }

        public void CheckModelTemplet(int id)
        {
            TowerStrData tower = this.DataSource.Where(item => item.ID == id).SingleOrDefault();

            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.DefaultExt = ".dat"; // Required file extension 
            fileDialog.Filter = "Text documents (.dat)|*.dat"; // Optional file extensions

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                tower.ModelName = Path.GetFileName(fileDialog.FileName);
                tower.ModelFileExtension = Path.GetFileNameWithoutExtension(fileDialog.FileName) + "-" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(fileDialog.FileName);

                UploadFile(tower, 1, fileDialog.FileName);//添加节点及负责相关文件
            }

        }

        public void CheckHangPointTemplate(int id)
        {
            TowerStrData tower = this.DataSource.Where(item => item.ID == id).SingleOrDefault();

            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.DefaultExt = ".xml"; // Required file extension 
            fileDialog.Filter = "Text documents (.xml)|*.xml"; // Optional file extensions

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                tower.HangPointName = Path.GetFileName(fileDialog.FileName);
                tower.HangPointFileExtension = Path.GetFileNameWithoutExtension(fileDialog.FileName) + "-" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(fileDialog.FileName);


                UploadFile(tower, 2, fileDialog.FileName);//添加节点及负责相关文件
            }

        }

        //保存上传文件
        /// <summary>
        /// 保存杆塔型号结构计算模型，挂点文件名称
        /// </summary>
        /// <param name="tower">杆塔型号</param>
        /// <param name="type">1：结构计算模型 2：挂点文件</param>
        public void UploadFile(TowerStrData tower, int type, string uploadFilePath)
        {
            string filePath = globalInfo.ProjectPath + "\\BaseData\\TowerStr.xml"; ;
            //加载xml文件
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);

            //获取到指定节点
            XmlNodeList xmlNodeList = doc.GetElementsByTagName("TowerCollection")[0].ChildNodes;

            foreach (XmlNode xmlNode in xmlNodeList)
            {
                if (xmlNode.Attributes.GetNamedItem("Name").InnerXml == tower.Name && xmlNode.Attributes.GetNamedItem("Type").InnerXml == tower.Type.ToString())
                {
                    if (type == 1)//结构计算模型
                    {

                        if (xmlNode.Attributes.GetNamedItem("ModelName")!=null&& xmlNode.Attributes.GetNamedItem("ModelName").InnerXml == tower.ModelName)//如果存在该节点，则修改
                        {
                            xmlNode.Attributes.GetNamedItem("ModelName").InnerText = tower.ModelName;
                            xmlNode.Attributes.GetNamedItem("ModelFileExtension").InnerText = tower.ModelFileExtension;
                        }
                        else//不存在则添加
                        {
                            XmlAttribute xmlAttribute = doc.CreateAttribute("ModelName");
                            xmlAttribute.InnerText = tower.ModelName;
                            xmlNode.Attributes.Append(xmlAttribute);

                            XmlAttribute xmlAttribute1 = doc.CreateAttribute("ModelFileExtension");
                            xmlAttribute1.InnerText = tower.ModelFileExtension;
                            xmlNode.Attributes.Append(xmlAttribute1);
                        }
                    }
                    else//挂点文件
                    {
                        if (xmlNode.Attributes.GetNamedItem("HangPointName") != null && xmlNode.Attributes.GetNamedItem("HangPointName").InnerXml == tower.HangPointName)//如果存在该节点，则修改
                        {
                            xmlNode.Attributes.GetNamedItem("HangPointName").InnerXml = tower.HangPointName;
                            xmlNode.Attributes.GetNamedItem("HangPointFileExtension").InnerText = tower.HangPointFileExtension;
                        }
                        else//不存在则添加
                        {
                            XmlAttribute xmlAttribute = doc.CreateAttribute("HangPointName");
                            xmlAttribute.InnerText = tower.HangPointName;
                            xmlNode.Attributes.Append(xmlAttribute);

                            XmlAttribute xmlAttribute1 = doc.CreateAttribute("HangPointFileExtension");
                            xmlAttribute1.InnerText = tower.HangPointFileExtension;
                            xmlNode.Attributes.Append(xmlAttribute1);
                        }
                    }
                    break;
                }
            }
            //判断上传文件夹是否包含该 杆塔型号 文件夹，如果包含，直接保存到对应文件夹中，如不包含，穿件文件夹，并保存该上传文件
            string uploadFolder = globalInfo.ProjectPath + @"\BaseData\TowerUploadFile\" + tower.Name + "[" + tower.Type + "]";
            string sourceName = type == 1?(uploadFolder + @"\" + tower.ModelFileExtension):(uploadFolder + @"\" + tower.HangPointFileExtension);

            if (File.Exists(uploadFolder))
            {
                File.Copy(uploadFilePath, sourceName);
            }
            else
            {

                System.IO.Directory.CreateDirectory(uploadFolder);
                File.Copy(uploadFilePath, sourceName);
            }
            doc.Save(filePath);
        }

        /// <summary>
        /// 数据源
        /// </summary>
        private ObservableCollection<TowerStrData> dataSource;

        public ObservableCollection<TowerStrData> DataSource
        {
            get { return dataSource; }
            set { dataSource = value; RaisePropertyChanged(() => DataSource); }
        }
        protected void CopyRow()
        {
            ;
        }


        public override void Save()
        {
            List<WireType> wireType = new List<WireType>();
            WireReader.Save(filePath, wireType);
        }


        public override void UpDateView(string para1, string para2 = "")
        {
            throw new NotImplementedException();
        }

        public override void DelSubItem(string itemName)
        {
            throw new NotImplementedException();
        }
    }
}
