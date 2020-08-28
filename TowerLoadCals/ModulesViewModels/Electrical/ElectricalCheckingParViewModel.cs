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
    public class ElectricalCheckingParViewModel : ViewModelBase
    {
        public string SequenceName { get; set; }//序列名称
        public string TowerType { get; set; }//塔类型

        /// <summary>
        /// 保存
        /// </summary>
        public DelegateCommand doSaveCommand { get; private set; }

        protected ElectricalCheckingParViewModel()
        {
            this.SequenceName = "序列一";
            this.TowerType = "悬垂塔";
            doSaveCommand = new DelegateCommand(doSave);
            this.dataSource = new ObservableCollection<TowerSerial>(TowerSerialReader.ReadXmlBySequenceNameAndTowerType(GlobalInfo.GetInstance().ProjectPath, SequenceName, TowerType));
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


        /// <summary>
        ///基础系数数据源
        /// </summary>
        private ObservableCollection<TowerSerial> dataSource;

        public ObservableCollection<TowerSerial> DataSource
        {
            get { return dataSource; }
            set { dataSource = value; }
        }

    }

}
