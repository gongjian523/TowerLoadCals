using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using DevExpress.Office.History;
using DevExpress.XtraRichEdit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using TowerLoadCals.BLL;
using TowerLoadCals.DAL.Common;
using TowerLoadCals.Mode;
using TowerLoadCals.Mode.Electric;

namespace TowerLoadCals.ModulesViewModels.Electrical
{
    public class ElectricalCheckingParViewModel : DaseDataBaseViewModel<TowerSerial, List<TowerSerial>>
    {
        public string SequenceName { get; set; }//序列名称
        public string TowerType { get; set; }//塔类型

        /// <summary>
        /// 保存
        /// </summary>
        public DelegateCommand doSaveCommand { get; private set; }

        protected ElectricalCheckingParViewModel()
        {
            this.engineerParDataSource = null;
            this.frontParDataSource = null;
            this.backParDataSource = null;
            doSaveCommand = new DelegateCommand(doSave);

            bindDataSource(true);
        }

        /// <summary>
        /// 保存方法
        /// </summary>
        public void doSave()
        {
            try
            {
                //保存计算后的杆塔序列文件
                TowerSerialReader.SaveXmlBySequenceNameAndTowerType(GlobalInfo.GetInstance().ProjectPath, SequenceName, this.dataSource.ToList());

                MessageBox.Show("保存成功!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存异常！异常信息:" + ex.Message);
            }
        }

        protected override void InitializeData()
        {
        }

        public override void Save()
        {
            throw new NotImplementedException();
        }

        public override void UpDateView(string para1, string para2 = "")
        {
            this.SequenceName = para1;
            this.TowerType = para2;

            bindDataSource(false);
        }

        protected void bindDataSource(bool Initialize)
        {
            if (Initialize)
            {
                SelectedItems=new ObservableCollection<TowerSerial>((new List<TowerSerial>()  ));
            }
            else
            {
                SelectedItems.Clear();
                SelectedItems = new ObservableCollection<TowerSerial>(TowerSerialReader.ReadXmlBySequenceNameAndTowerType(GlobalInfo.GetInstance().ProjectPath, SequenceName, TowerType));
            }
        }


        public override void DelSubItem(string itemName)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        ///基础系数数据源
        /// </summary>
        private ObservableCollection<TowerSerial> dataSource;

        public ObservableCollection<TowerSerial> DataSource
        {
            get { return dataSource; }
            set { dataSource = value; }
        }

        private List<string> engineerParDataSource;
        /// <summary>
        /// 工程参数
        /// </summary>
        public List<string> EngineerParDataSource
        {
            get { return engineerParDataSource; }
            set { engineerParDataSource = value; }
        }

        private List<string> frontParDataSource;
        /// <summary>
        /// 前侧相参数
        /// </summary>
        public List<string> FrontParDataSource
        {
            get { return frontParDataSource; }
            set { frontParDataSource = value; }
        }

        private List<string> backParDataSource;
        /// <summary>
        /// 后侧相参数
        /// </summary>
        public List<string> BackParDataSource
        {
            get { return backParDataSource; }
            set { backParDataSource = value; }
        }

    }

}
