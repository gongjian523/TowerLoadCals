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

                foreach (TowerStrData item in editData)
                {
                    XmlElement row = doc.CreateElement("Tower");
                    row.SetAttribute("ID", item.ID.ToString());
                    row.SetAttribute("Name", item.Name == null ? "" : item.Name);
                    row.SetAttribute("Type", item.Type.ToString());
                    row.SetAttribute("TypeName", item.TypeName.ToString());
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
                    row.SetAttribute("StrHeightSer", item.StrHeightSer == null ? "" : item.StrHeightSer.ToString());
                    row.SetAttribute("StrAllowHorSpan", item.StrAllowHorSpan == null ? "" : item.StrAllowHorSpan.ToString());
                    row.SetAttribute("AngelToHorSpan", item.AngelToHorSpan.ToString());
                    row.SetAttribute("MaxAngHorSpan", item.MaxAngHorSpan.ToString());
                    rootNode.AppendChild(row);

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
                string sourcePath = "";
                OpenFileDialog openFileDialog = new OpenFileDialog();

                openFileDialog.Filter = "Text Documents (*.dbf)|*.dbf|All Files|*.*";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    sourcePath = openFileDialog.FileName;

                    List<TowerStrData> list = TowerStPraReader.ReadImportFile(sourcePath);

                    if (list != null && list.Count > 0)
                    {
                        if (this.SelectedItems.Count > 0)
                        {
                            int index = SelectedItems.Count + 1;
                            TowerStrData tower;
                            foreach (TowerStrData item in list)
                            {
                                item.ID = index;
                                tower = this.SelectedItems.Where(k => k.Name == item.Name).First();
                                if (tower != null)
                                {
                                    DialogResult dr = MessageBox.Show(string.Format("已经存在名称为【{0}】相同的杆塔型号信息，是否替换？", item.Name), "重复确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                                    if (dr == DialogResult.OK)
                                    {
                                        this.SelectedItems.Where(k => k.Name == item.Name).First().VoltageLevel = 1000;
                                        //this.SelectedItems.Remove(this.SelectedItems.Where(k => k.Name == item.Name).First());
                                        //item.ID = tower.ID;
                                        //this.SelectedItems.Add(item);
                                    }
                                }
                                else
                                    this.SelectedItems.Add(item);
                                index++;
                            }
                        }
                        else {
                            this.SelectedItems = new ObservableCollection<TowerStrData>(list);
                        }
                    }
                    else
                    {
                        MessageBox.Show("无可导入信息");
                    }
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("导入过程出错，错误信息如下:" + ex.Message);
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
