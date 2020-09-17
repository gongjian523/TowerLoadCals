using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TowerLoadCals.Common.Utils;
using System.Windows.Input;
using TowerLoadCals.BLL;
using TowerLoadCals.DAL.Common;
using TowerLoadCals.Mode;
using System.IO;
using Microsoft.Win32;
using TowerLoadCals.DAL;
using System.Collections.ObjectModel;
using DevExpress.DataProcessing.InMemoryDataProcessor;

/// <summary>
/// created by : glj
/// </summary>

namespace TowerLoadCals.ModulesViewModels.TowerSequence
{
    /// <summary>
    /// 杆塔序列-公共和档内参数设置
    /// </summary>
    public class CommSideParaSettingViewModel : ViewModelBase
    {
        protected CommSideParaSettingViewModel()
        {
            var globalInfo = GlobalInfo.GetInstance();

            CommParaNameList = globalInfo.GetElecCalsCommParaNames();
            SideParaNameList = globalInfo.GetElecCalsSideParaNames();

            dataSource = new ObservableCollection<CommSideParaSetting>(new List<CommSideParaSetting> {
                new CommSideParaSetting() { Index = 1, StartTowerName = "", EndTowerName = "", CommPara = "",SidePara = "" }
            });
        }

        public delegate void CloseWindowHandler(object sender, List<CommSideParaSetting> resultList);
        public event CloseWindowHandler CloseWindowEvent;

        /// <summary>
        /// 新增行
        /// </summary>
        public void AddNew()
        {
            int index = dataSource.Count + 1;
            dataSource.Add(new CommSideParaSetting() { Index = index });
        }

        /// <summary>
        /// 删除行
        /// </summary>
        /// <param name="index"></param>
        public void DelRow(int index)
        {
            dataSource.Remove(dataSource.Where(item=>item.Index==index).Single());
        }

        public virtual void onConfirm()
        {
            try
            {
                List<CommSideParaSetting> list = new List<CommSideParaSetting>();

                foreach (CommSideParaSetting item in this.dataSource)
                {
                    if (!string.IsNullOrEmpty(item.StartTowerName) || !string.IsNullOrEmpty(item.EndTowerName) || !string.IsNullOrEmpty(item.CommPara) || !string.IsNullOrEmpty(item.SidePara))
                    {
                        if (string.IsNullOrEmpty(item.StartTowerName) || string.IsNullOrEmpty(item.EndTowerName) || string.IsNullOrEmpty(item.CommPara) || string.IsNullOrEmpty(item.SidePara))
                        {
                            MessageBox.Show("第" + item.Index + "行值不能为空，请确认！");
                            return;
                        }

                        if (item.StartTowerName.Substring(0, 1) != item.EndTowerName.Substring(0, 1))
                        {
                            MessageBox.Show("第" + item.Index + "行的【起始塔位号】与【终止塔位号】类型不一致，请确认！");
                            return;
                        }
                        list.Add(item);
                    }
                }

                close(list);
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存过程中出错，错误信息如下：" + ex.Message);
            }
        }

        public virtual void onConcel()
        {
            close(null );
        }

        protected virtual void close(List<CommSideParaSetting> resultList)
        {
            if (CloseWindowEvent != null)
                CloseWindowEvent(this, resultList);
        }

        private List<string> towerNameList;
        /// <summary>
        /// 塔位号列表
        /// </summary>
        public List<string> TowerNameList
        {
            get { return towerNameList; }
            set { towerNameList = value; RaisePropertyChanged(() => TowerNameList); }
        }

        /// <summary>
        /// 公共参数名字列表
        /// </summary>
        private List<string> _commParaNameList;
        public List<string> CommParaNameList
        {
            get { return _commParaNameList; }
            set { _commParaNameList = value; RaisePropertyChanged(() => CommParaNameList); }
        }

        /// <summary>
        /// 档内参数名字列表
        /// </summary>
        private List<string> _sideParaNameList;
        public List<string> SideParaNameList
        {
            get { return _sideParaNameList; }
            set { _sideParaNameList = value; RaisePropertyChanged(() => SideParaNameList); }
        }

        /// <summary>
        /// 数据源
        /// </summary>
        private ObservableCollection<CommSideParaSetting> dataSource;
        public ObservableCollection<CommSideParaSetting> DataSource
        {
            get { return dataSource; }
            set { dataSource = value; RaisePropertyChanged(() => DataSource); }
        }
    }

    public class CommSideParaSetting
    {
        public int Index { get; set; }
        public string TowerSequenceName { get; set; }
        public string StartTowerName { get; set; }
        public string EndTowerName { get; set; }
        public string CommPara { get; set; }
        public string SidePara { get; set; }
    }
}
