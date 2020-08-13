using DevExpress.Map.Kml.Model;
using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using TowerLoadCals.DAL;
using TowerLoadCals.Mode;
using TowerLoadCals.ModulesViewModels;

namespace TowerLoadCals.Modules
{
    public class TowerViewModel : DaseDataBaseViewModel<TowerStrData, List<TowerStrCollection>>
    {
        public DelegateCommand CopyRowCommand { get; private set; }

        /// <summary>
        /// 导入dbf
        /// </summary>
        public DelegateCommand DBFImportCommand { get; private set; }
        /// <summary>
        /// 保存xml
        /// </summary>
        public DelegateCommand SaveXmlCommand { get; private set; }

        protected string curType;
        protected string curName;


        protected override void InitializeItemsSource()
        {
            base.InitializeItemsSource();
            filePath = globalInfo.ProjectPath + "\\BaseData\\TowerStr.xml";


            CopyRowCommand = new DelegateCommand(CopyRow);
        }

        protected override void InitializeData()
        {

            SaveXmlCommand = new DelegateCommand(SaveXml);
            DBFImportCommand = new DelegateCommand(DBFImport);

            //BaseData = TowerStrDataReader.Read(@"D:\杆塔项目\other\【0810】\铁塔库.xml");

            this.SelectedItems = new ObservableCollection<TowerStrData>(TowerStrDataReader.ReadLoadFile(filePath));//获取本地已保存信息

        }

        #region 保存xml文件信息

        /// <summary>
        /// 保存xml文件信息
        /// </summary>
        public void SaveXml()
        {
            try
            {
                var editData = this.SelectedItems.ToList();

                XmlDocument doc = new XmlDocument();
                doc.Load(filePath);

                XmlNode rootNode = doc.GetElementsByTagName("Root")[0];
                rootNode.RemoveAll();//移除所有节点，全部新增

                bool isExists = false;
                foreach (TowerStrData item in editData)
                {
                    //isExists = false;
                    //foreach (XmlNode xmlNode in rootNode.ChildNodes)
                    //{
                    //    if (xmlNode.Attributes.GetNamedItem("Name").InnerText == item.Name)
                    //    {
                    //        isExists = true;
                    //        xmlNode.Attributes.GetNamedItem("Name").InnerText = item.Name;
                    //        xmlNode.Attributes.GetNamedItem("Type").InnerText = item.Type.ToString();
                    //        xmlNode.Attributes.GetNamedItem("VoltageLevel").InnerText = item.VoltageLevel.ToString();
                    //        xmlNode.Attributes.GetNamedItem("CirNum").InnerText = item.CirNum.ToString();
                    //        xmlNode.Attributes.GetNamedItem("CurType").InnerText = item.CurType.ToString();
                    //        xmlNode.Attributes.GetNamedItem("MinAngel").InnerText = item.MinAngel.ToString();
                    //        xmlNode.Attributes.GetNamedItem("MaxAngel").InnerText = item.MaxAngel.ToString();
                    //        xmlNode.Attributes.GetNamedItem("CalHeight").InnerText = item.CalHeight.ToString();
                    //        xmlNode.Attributes.GetNamedItem("MinHeight").InnerText = item.MinHeight.ToString();
                    //        xmlNode.Attributes.GetNamedItem("MaxHeight").InnerText = item.MaxHeight.ToString();
                    //        xmlNode.Attributes.GetNamedItem("AllowedHorSpan").InnerText = item.AllowedHorSpan.ToString();
                    //        xmlNode.Attributes.GetNamedItem("OneSideMinHorSpan").InnerText = item.OneSideMinHorSpan.ToString();
                    //        xmlNode.Attributes.GetNamedItem("OneSideMaxHorSpan").InnerText = item.OneSideMaxHorSpan.ToString();
                    //        xmlNode.Attributes.GetNamedItem("AllowedVerSpan").InnerText = item.AllowedVerSpan.ToString();
                    //        xmlNode.Attributes.GetNamedItem("OneSideMinVerSpan").InnerText = item.OneSideMinVerSpan.ToString();
                    //        xmlNode.Attributes.GetNamedItem("OneSideMaxVerSpan").InnerText = item.OneSideMaxVerSpan.ToString();
                    //        xmlNode.Attributes.GetNamedItem("OneSideUpVerSpanMin").InnerText = item.OneSideUpVerSpanMin.ToString();
                    //        xmlNode.Attributes.GetNamedItem("OneSideUpVerSpanMax").InnerText = item.OneSideUpVerSpanMax.ToString();
                    //        xmlNode.Attributes.GetNamedItem("DRepresentSpanMin").InnerText = item.DRepresentSpanMin.ToString();
                    //        xmlNode.Attributes.GetNamedItem("DRepresentSpanMax").InnerText = item.DRepresentSpanMax.ToString();
                    //        xmlNode.Attributes.GetNamedItem("StrHeightSer").InnerText = item.StrHeightSer.ToString();
                    //        xmlNode.Attributes.GetNamedItem("StrAllowHorSpan").InnerText = item.StrAllowHorSpan.ToString();
                    //        xmlNode.Attributes.GetNamedItem("AngelToHorSpan").InnerText = item.AngelToHorSpan.ToString();
                    //        xmlNode.Attributes.GetNamedItem("MaxAngHorSpan").InnerText = item.MaxAngHorSpan.ToString();
                    //        break;
                    //    }
                    //}

                    //if (!isExists)
                    //{
                        XmlElement row = doc.CreateElement("KNode");
                        row.SetAttribute("Name", item.Name);
                        row.SetAttribute("Type", item.Type.ToString());
                        row.SetAttribute("VoltageLevel", item.VoltageLevel.ToString());
                        row.SetAttribute("CirNum", item.CirNum.ToString());
                        row.SetAttribute("CurType", item.CurType.ToString());
                        row.SetAttribute("MinAngel", item.MinAngel.ToString());
                        row.SetAttribute("MaxAngel", item.MaxAngel.ToString());
                        row.SetAttribute("CalHeight", item.CalHeight.ToString());
                        row.SetAttribute("MinHeight", item.MinHeight.ToString());
                        row.SetAttribute("MaxHeight", item.MaxHeight.ToString());
                        row.SetAttribute("AllowedHorSpan", item.AllowedHorSpan.ToString());
                        row.SetAttribute("OneSideMinHorSpan", item.OneSideMinHorSpan.ToString());
                        row.SetAttribute("OneSideMaxHorSpan", item.OneSideMaxHorSpan.ToString());
                        row.SetAttribute("AllowedVerSpan", item.AllowedVerSpan.ToString());
                        row.SetAttribute("OneSideMinVerSpan", item.OneSideMinVerSpan.ToString());
                        row.SetAttribute("OneSideMaxVerSpan", item.OneSideMaxVerSpan.ToString());
                        row.SetAttribute("OneSideUpVerSpanMin", item.OneSideUpVerSpanMin.ToString());
                        row.SetAttribute("OneSideUpVerSpanMax", item.OneSideUpVerSpanMax.ToString());
                        row.SetAttribute("DRepresentSpanMin", item.DRepresentSpanMin.ToString());
                        row.SetAttribute("DRepresentSpanMax", item.DRepresentSpanMax.ToString());
                        row.SetAttribute("StrHeightSer", item.StrHeightSer.ToString());
                        row.SetAttribute("StrAllowHorSpan",item.StrAllowHorSpan!=null? item.StrAllowHorSpan.ToString():"");
                        row.SetAttribute("AngelToHorSpan", item.AngelToHorSpan.ToString());
                        row.SetAttribute("MaxAngHorSpan", item.MaxAngHorSpan.ToString());
                        rootNode.AppendChild(row);
                    //}
                }
                doc.Save(filePath);
                MessageBox.Show("保存信息成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存信息失败，请确认后重试！错误信息为：" + ex.Message);
            }
        }
        #endregion

        #region 导入dbf文件
        /// <summary>
        /// 导入dbf文件
        /// </summary>
        public void DBFImport()
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();

                openFileDialog.Filter = "Text Documents (*.dbf)|*.dbf|All Files|*.*";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog.FileName;
                }
                List<TowerStrData> list = TowerStPraReader.Read1(filePath);

                if (list != null && list.Count > 0)
                {
                    foreach (TowerStrData item in list)
                    {
                        this.SelectedItems.Add(item);
                    }
                }
                else
                    MessageBox.Show("无可导入信息");
            }
            catch (Exception ex)
            {
                MessageBox.Show("导入过程出错，错误信息如下:"+ex.Message);
            }
        }
        #endregion


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
