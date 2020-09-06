using DevExpress.Mvvm;
using DevExpress.XtraRichEdit.Model.History;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using TowerLoadCals.BLL;
using TowerLoadCals.DAL;
using TowerLoadCals.DAL.Common;
using TowerLoadCals.Mode;
using TowerLoadCals.Modules.TowerSequence;
using TowerLoadCals.ModulesViewModels;



/// <summary>
/// created by : glj
/// </summary>



namespace TowerLoadCals.ModulesViewModels.TowerSequence
{
    public class TowerSequenceViewModel : DaseDataBaseViewModel<TowerSerial, List<TowerSerial>>
    {
        string pageName = "";//当前序列名称
        /// <summary>
        /// 气象区设置
        /// </summary>
        public DelegateCommand doWeatherSettingCommand { get; private set; }

        /// <summary>
        /// 刷新关联
        /// </summary>
        public DelegateCommand doRefreshCommand { get; private set; }

        /// <summary>
        /// 保存
        /// </summary>
        public DelegateCommand doSaveCommand { get; private set; }


        protected ProjectUtils projectUtils;

        /// <summary>
        ///  获取数据源
        /// </summary>
        /// <param name="navg"></param>
        public void GetDataSource(string navg)
        {
            string filePath = globalInfo.ProjectPath + "\\" + ConstVar.TowerSequenceStr + "\\" + navg + "\\TowerSequenceStr.xml";
            if (File.Exists(filePath))
            {
                DataSource = new ObservableCollection<TowerSerial>(TowerSerialReader.ReadXml(globalInfo.ProjectPath, navg));
            }
        }
        /// <summary>
        /// 数据源
        /// </summary>
        private ObservableCollection<TowerSerial> dataSource;

        public ObservableCollection<TowerSerial> DataSource
        {
            get { return dataSource; }
            set { dataSource = value; RaisePropertyChanged(() => DataSource); }
        }

        public override void UpDateView(string fileName, string para2 = "")
        {
            pageName = fileName;
            GetDataSource(fileName);
        }

        protected override void InitializeData()
        {
            projectUtils = ProjectUtils.GetInstance();

            doWeatherSettingCommand = new DelegateCommand(doWeatherConditionSetting);
            doRefreshCommand = new DelegateCommand(doRefreshLink);
            doSaveCommand = new DelegateCommand(doSave);

        }

        #region 气象条件设置
        WeatherConditionSettingWindow setting;
        /// <summary>
        /// 气象条件设置
        /// </summary>
        public void doWeatherConditionSetting()
        {
            //塔位号
            List<string> list = this.dataSource.Select(item => item.TowerName).ToList();

            setting = new WeatherConditionSettingWindow();

            ((WeatherConditionSettingWindowViewModel)(setting.DataContext)).CloseWindowEvent += WeatherConditionSettingWindowClosed;
            ((WeatherConditionSettingWindowViewModel)(setting.DataContext)).TowerNameList = list;
            setting.ShowDialog();
        }

        public void WeatherConditionSettingWindowClosed(object sender, IList<WeatherConditionSetting> list)
        {
            WeatherConditionSettingWindowViewModel model = (WeatherConditionSettingWindowViewModel)sender;

            model.CloseWindowEvent -= WeatherConditionSettingWindowClosed;
            if (setting != null) setting.Close();
            setting = null;

            //设置了气象条件
            if (list != null)
            {
                var sourcList = this.dataSource;
                int startIndex = 0, endIndex = 0;
                foreach (WeatherConditionSetting item in list)
                {
                    try
                    {
                        startIndex = this.dataSource.Where(t => t.TowerName == item.StartTowerName).Single().ID;
                        endIndex = this.dataSource.Where(t => t.TowerName == item.EndTowerName).Single().ID;
                        //筛选需要修改的序列信息
                        var sourceList = sourcList.Where(t => t.ID >= startIndex && t.ID <= endIndex).ToList();

                        foreach (TowerSerial serial in sourceList)
                        {
                            this.dataSource.Where(k => k.ID == serial.ID).First().WeatherCondition = item.WeatherCondition;
                        }
                    }
                    catch (Exception ex)
                    {
                        string msg = ex.Message;
                    }
                }
            }
        }
        #endregion


        /// <summary>
        /// 超前判断 对当前列表数据进行条件判断 
        /// 垂直档距字段根据塔型可能会包含/,后期需要进行读取逻辑判断 
        /// 修
        /// </summary>
        public void doRefreshLink()
        {
            //DialogResult dr = MessageBox.Show("页面信息可能已发生改变，请确认是否保存当前页面数据？", "保存确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            //if (dr == DialogResult.OK)
            //{
            //    doSave();//保存方法xml
            //}
            //GetDataSource(pageName);

            string towerPath = globalInfo.ProjectPath + "\\" + ConstVar.DataBaseStr + "\\TowerStr.xml";

            List<TowerStrData> towerList = TowerStrDataReader.ReadLoadFile(towerPath).ToList();

            TowerStrData tower;//塔型实体
            List<string> StrHeight;//直线塔呼高
            double StrAllowHorSpan;// 直线塔档距序列字符串

            foreach (TowerSerial item in this.dataSource)
            {
                //杆塔型号实体
                tower = towerList.Where(t => t.Name == item.TowerPattern).FirstOrDefault();

                if (tower != null)//塔型判断
                {
                    if (tower.TypeName == "直线塔" || tower.TypeName == "直线转角塔" || tower.TypeName == "终端塔" || tower.TypeName == "分支塔")
                    {
                        //                      水平档距的验算
                        //实际使用直线塔水平档距大于铁塔使用条件中水平档距的
                        StrHeight = tower.StrHeightSer.Split(',').ToList();
                        int index = StrHeight.IndexOf(item.CallItHigh.ToString());
                        StrAllowHorSpan = double.Parse(tower.StrAllowHorSpan.Split(',').ToList()[index]);

                        if (item.HorizontalSpan > StrAllowHorSpan && item.IsChecking == false)//如果水平档距大于了设定呼高设置最大值
                        {
                            item.IsChecking = true;
                        }
                        //                     垂直档距验算 
                        //实际使用直线塔垂直档距大于铁塔使用条件中垂直档距的
                        if (double.Parse(item.VerticalSpan) > tower.AllowedVerSpan && item.IsChecking == false)
                        {
                            item.IsChecking = true;
                        }
                        if (tower.TypeName == "直线塔")//直线塔判断
                        {
                            //                    带角度直线塔验算
                            //实际使用时直线塔带角度的，全部进行验算
                            if (item.TurningAngle != 0 && item.IsChecking == false)
                            {
                                item.IsChecking = true;
                            }
                        }
                        else if (tower.TypeName == "直线转角塔" || tower.TypeName == "终端塔" || tower.TypeName == "分支塔")//直线转角塔判断
                        {
                            //                        带角度直线塔验算
                            //转角度数超过塔库中使用条件最大转角的，就验算。
                            if (item.TurningAngle > tower.MaxAngel && item.IsChecking == false)
                            {
                                item.IsChecking = true;
                            }
                        }
                    }
                    else //耐张塔
                    {
                        if (item.HorizontalSpan > tower.AllowedHorSpan && item.IsChecking == false)//设计水平档距
                        {
                            item.IsChecking = true;
                        }
                        if (item.HorizontalSpan > tower.MaxAngHorSpan && item.IsChecking == false)//最大水平档距
                        {
                            item.IsChecking = true;
                        }
                        if (item.HorizontalSpan > (tower.AllowedHorSpan + (tower.MaxAngel - item.TurningAngle) * tower.AngelToHorSpan) && item.IsChecking == false)//水平档距
                        {
                            item.IsChecking = true;
                        }
                        if (Math.Abs(item.VerticalSpan.Split('/').Sum(k => double.Parse(k))) > tower.AllowedVerSpan)//垂直档距验算
                        {
                            item.IsChecking = true;
                        }
                    }
                }

            }
        }


        /// <summary>
        /// 保存方法
        /// </summary>
        public void doSave()
        {
            try
            {
                string savePath = projectUtils.ProjectPath + "\\" + ConstVar.TowerSequenceStr + "\\" + pageName;
                //保存计算后的杆塔序列文件
                TowerSerialReader.SaveDT(this.DataSource.ToList(), savePath);

                MessageBox.Show("保存成功!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存异常！异常信息:" + ex.Message);
            }
        }

        public override void Save()
        {
            throw new NotImplementedException();
        }

        public override void DelSubItem(string itemName)
        {
            //DialogResult  result;
            //result = MessageBox.Show("确定要删除该序列码？", "删除确认", MessageBoxButtons.YesNoCancel);
            //if (result == DialogResult.Yes)
            //{
            //    if (Directory.Exists(savePath))
            //    {
            //        Directory.Delete(savePath, true);
            //    }

            //}
        }
    }
}
