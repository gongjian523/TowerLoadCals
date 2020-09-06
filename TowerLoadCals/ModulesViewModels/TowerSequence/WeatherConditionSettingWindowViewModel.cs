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
    /// 杆塔序列-气象条件设置
    /// </summary>
    public class WeatherConditionSettingWindowViewModel : ViewModelBase
    {
        protected ProjectUtils projectUtils;
        protected GlobalInfo globalInfo;
        protected WeatherXmlReader _weatherXmlReader = new WeatherXmlReader();

        protected WeatherConditionSettingWindowViewModel()
        {
            projectUtils = ProjectUtils.GetInstance();
            globalInfo = GlobalInfo.GetInstance();

            //气象条件
            string filePath = globalInfo.ProjectPath + "\\BaseData\\WeatherCondition.xml";
            this.WeatherConditionList = _weatherXmlReader.ReadLocal(filePath).Select(item => item.Name).ToList();

            List<WeatherConditionSetting> list = new List<WeatherConditionSetting>();
            list.Add(new WeatherConditionSetting() { Index = 1, StartTowerName = "", EndTowerName = "", WeatherCondition = "" });

            this.dataSource = new ObservableCollection<WeatherConditionSetting>(list);
        }



        public delegate void CloseWindowHandler(object sender, List<WeatherConditionSetting> resultList);
        public event CloseWindowHandler CloseWindowEvent;

        /// <summary>
        /// 新增行
        /// </summary>
        public void AddNew()
        {
            int index = this.dataSource.Count + 1;
            this.dataSource.Add(new WeatherConditionSetting() { Index = index });
        }

        /// <summary>
        /// 删除行
        /// </summary>
        /// <param name="index"></param>
        public void DelRow(int index)
        {
            this.dataSource.Remove(dataSource.Where(item=>item.Index==index).Single());
        }

        public virtual void onConfirm()
        {
            try
            {
                List<WeatherConditionSetting> list = new List<WeatherConditionSetting>();

                foreach (WeatherConditionSetting item in this.dataSource)
                {
                    if (!string.IsNullOrEmpty(item.StartTowerName) || !string.IsNullOrEmpty(item.EndTowerName) || !string.IsNullOrEmpty(item.WeatherCondition))
                    {
                        if (string.IsNullOrEmpty(item.StartTowerName) || string.IsNullOrEmpty(item.EndTowerName) || string.IsNullOrEmpty(item.WeatherCondition))
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

        protected virtual void close(List<WeatherConditionSetting> resultList)
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


        private List<string> weatherConditionList;
        /// <summary>
        /// 气象条件
        /// </summary>
        public List<string> WeatherConditionList
        {
            get { return weatherConditionList; }
            set { weatherConditionList = value; RaisePropertyChanged(() => WeatherConditionList); }
        }

        /// <summary>
        /// 数据源
        /// </summary>
        private ObservableCollection<WeatherConditionSetting> dataSource;

        public ObservableCollection<WeatherConditionSetting> DataSource
        {
            get { return dataSource; }
            set { dataSource = value; RaisePropertyChanged(() => DataSource); }
        }
    }

    public class WeatherConditionSetting
    {
        public int Index { get; set; }
        public string TowerSequenceName { get; set; }
        public string StartTowerName { get; set; }
        public string EndTowerName { get; set; }
        public string WeatherCondition { get; set; }
    }
}
