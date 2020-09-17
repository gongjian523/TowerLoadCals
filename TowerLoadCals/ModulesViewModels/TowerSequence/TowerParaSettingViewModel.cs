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
    /// 杆塔序列-铁塔配置参数设置
    /// </summary>
    public class TowerParaSettingViewModel : ViewModelBase
    {

        protected TowerParaSettingViewModel()
        {
            var globalInfo = GlobalInfo.GetInstance();

            TowerParaNameList = globalInfo.GetElecCalsTowerParaNames();

            hangDataSource = new ObservableCollection<TowrParaSetting>(new List<TowrParaSetting>()
            {
                new TowrParaSetting(){ Index = 1, StartTowerName = "", EndTowerName = "", TowerPara = "" }
            });

            strainDataSource = new ObservableCollection<TowrParaSetting>(new List<TowrParaSetting>()
            {
                new TowrParaSetting(){ Index = 1, StartTowerName = "", EndTowerName = "", TowerPara = "" }
            });
        }


        public delegate void CloseWindowHandler(object sender, List<TowrParaSetting> hangList, List<TowrParaSetting> strainList);
        public event CloseWindowHandler CloseWindowEvent;

        /// <summary>
        /// 新增行
        /// </summary>
        public void HangAddNew()
        {
            int index = hangDataSource.Count + 1;
            hangDataSource.Add(new TowrParaSetting() { Index = index });
        }

        /// <summary>
        /// 删除行
        /// </summary>
        /// <param name="index"></param>
        public void HangDelRow(int index)
        {
            hangDataSource.Remove(hangDataSource.Where(item=>item.Index==index).Single());
        }

        /// <summary>
        /// 新增行
        /// </summary>
        public void StrainAddNew()
        {
            int index = strainDataSource.Count + 1;
            strainDataSource.Add(new TowrParaSetting() { Index = index });
        }

        /// <summary>
        /// 删除行
        /// </summary>
        /// <param name="index"></param>
        public void StrainDelRow(int index)
        {
            strainDataSource.Remove(strainDataSource.Where(item => item.Index == index).Single());
        }

        public virtual void onConfirm()
        {
            try
            {
                List<TowrParaSetting> hlist = new List<TowrParaSetting>();
                List<TowrParaSetting> slist = new List<TowrParaSetting>();

                foreach (TowrParaSetting item in hangDataSource)
                {
                    if (!string.IsNullOrEmpty(item.StartTowerName) || !string.IsNullOrEmpty(item.EndTowerName) || !string.IsNullOrEmpty(item.TowerPara))
                    {
                        if (string.IsNullOrEmpty(item.StartTowerName) || string.IsNullOrEmpty(item.EndTowerName) || string.IsNullOrEmpty(item.TowerPara))
                        {
                            MessageBox.Show("悬垂塔第" + item.Index + "行值不能为空，请确认！");
                            return;
                        }

                        if (item.StartTowerName.Substring(0, 1) != item.EndTowerName.Substring(0, 1))
                        {
                            MessageBox.Show("悬垂塔第" + item.Index + "行的【起始塔位号】与【终止塔位号】类型不一致，请确认！");
                            return;
                        }
                        hlist.Add(item);
                    }
                }

                foreach (TowrParaSetting item in strainDataSource)
                {
                    if (!string.IsNullOrEmpty(item.StartTowerName) || !string.IsNullOrEmpty(item.EndTowerName) || !string.IsNullOrEmpty(item.TowerPara))
                    {
                        if (string.IsNullOrEmpty(item.StartTowerName) || string.IsNullOrEmpty(item.EndTowerName) || string.IsNullOrEmpty(item.TowerPara))
                        {
                            MessageBox.Show("耐张塔第" + item.Index + "行值不能为空，请确认！");
                            return;
                        }

                        if (item.StartTowerName.Substring(0, 1) != item.EndTowerName.Substring(0, 1))
                        {
                            MessageBox.Show("耐张塔第" + item.Index + "行的【起始塔位号】与【终止塔位号】类型不一致，请确认！");
                            return;
                        }
                        slist.Add(item);
                    }
                }

                close(hlist, slist);
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存过程中出错，错误信息如下：" + ex.Message);
            }
        }

        public virtual void onConcel()
        {
            close(null, null);
        }

        protected virtual void close(List<TowrParaSetting> hangList, List<TowrParaSetting> strainList)
        {
            if (CloseWindowEvent != null)
                CloseWindowEvent(this, hangList, strainList);
        }


        private List<string> towerParaNameList;
        public List<string> TowerParaNameList
        {
            get { return towerParaNameList; }
            set { towerParaNameList = value; RaisePropertyChanged(() => TowerParaNameList); }
        }

        /// <summary>
        /// 铁塔配置参数列表
        /// </summary>
        private List<string> hangTowerNameList;
        public List<string> HangTowerNameList
        {
            get { return hangTowerNameList; }
            set { hangTowerNameList = value; RaisePropertyChanged(() => HangTowerNameList); }
        }

        private List<string> strainTowerNameList;
        public List<string> StrainTowerNameList
        {
            get { return strainTowerNameList; }
            set { strainTowerNameList = value; RaisePropertyChanged(() => StrainTowerNameList); }
        }

        /// <summary>
        /// 悬垂塔数据源
        /// </summary>
        private ObservableCollection<TowrParaSetting> hangDataSource;
        public ObservableCollection<TowrParaSetting> HangDataSource
        {
            get { return hangDataSource; }
            set { hangDataSource = value; RaisePropertyChanged(() => HangDataSource); }
        }

        /// <summary>
        /// 耐张塔数据源
        /// </summary>
        private ObservableCollection<TowrParaSetting> strainDataSource;
        public ObservableCollection<TowrParaSetting> StrainDataSource
        {
            get { return strainDataSource; }
            set { strainDataSource = value; RaisePropertyChanged(() => StrainDataSource); }
        }
    }

    public class TowrParaSetting
    {
        public int Index { get; set; }
        public string TowerSequenceName { get; set; }
        public string StartTowerName { get; set; }
        public string EndTowerName { get; set; }
        public string TowerPara { get; set; }
    }
}
