using DevExpress.Mvvm;
using DevExpress.XtraRichEdit.Model.History;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TowerLoadCals.BLL;
using TowerLoadCals.BLL.Electric;
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
        /// 
        /// </summary>
        public DelegateCommand doCommSideParaSettingCommand { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public DelegateCommand doTowerParaSettingCommand { get; private set; }

        /// <summary>
        /// 刷新关联
        /// </summary>
        public DelegateCommand doRefreshCommand { get; private set; }

        /// <summary>
        /// 保存
        /// </summary>
        public DelegateCommand doSaveCommand { get; private set; }

        /// <summary>
        /// 计算
        /// </summary>
        public DelegateCommand doCaculateCommand { get; private set; }


        protected ProjectUtils projectUtils;

        protected TowerSequenceModule View;

        public TowerSequenceViewModel(TowerSequenceModule view )
        {
            View = view;
        }

        /// <summary>
        ///  获取数据源
        /// </summary>
        /// <param name="navg"></param>
        public void GetDataSource(string navg)
        {
            //string filePath = globalInfo.ProjectPath + "\\" + ConstVar.TowerSequenceStr + "\\" + navg + "\\TowerSequenceStr.xml";
            //if (File.Exists(filePath))
            //{
            //    //dataSource = new ObservableCollection<TowerSerial>(TowerSerialReader.ReadXml(globalInfo.ProjectPath, navg));
            //    towerList = TowerSerialReader.ReadXml(globalInfo.ProjectPath, navg);
            //}

            towerList = globalInfo.GetTowerSequenceBySequenceName(navg).Towers;
        }


        /// <summary>
        /// 旧界面使用Gird的数据源，现在使用Spreadsheet后已经废弃 
        /// </summary>
        [Obsolete]
        private ObservableCollection<TowerSerial> dataSource;
        [Obsolete]
        public ObservableCollection<TowerSerial> DataSource
        {
            get { return dataSource; }
            set { dataSource = value; RaisePropertyChanged(() => DataSource); }
        }


        private List<TowerSerial> towerList = new List<TowerSerial>();
        

        public override void UpDateView(string fileName, string para2 = "")
        {
            pageName = fileName;
            GetDataSource(fileName);
            View.RefreshSpreadSheet(towerList);
        }

        protected override void InitializeData()
        {
            projectUtils = ProjectUtils.GetInstance();

            doCommSideParaSettingCommand = new DelegateCommand(doCommSideParaSetting);
            doTowerParaSettingCommand = new DelegateCommand(doTowerParaSetting);
            doRefreshCommand = new DelegateCommand(doRefreshLink);
            doSaveCommand = new DelegateCommand(doSave);
            doCaculateCommand = new DelegateCommand(doCaculate);
        }

        #region 气象条件设置(弃用)
        WeatherConditionSettingWindow setting;
        /// <summary>
        /// 气象条件设置  气象区在公共参数中选择，在此界面中弃用
        /// </summary>
        [Obsolete]
        public void doWeatherConditionSetting()
        {
            //塔位号
            List<string> list = this.dataSource.Select(item => item.TowerName).ToList();

            setting = new WeatherConditionSettingWindow();

            ((WeatherConditionSettingWindowViewModel)(setting.DataContext)).CloseWindowEvent += WeatherConditionSettingWindowClosed;
            ((WeatherConditionSettingWindowViewModel)(setting.DataContext)).TowerNameList = list;
            setting.ShowDialog();
        }

        [Obsolete]
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

        #region 公共和档内参数设置
        CommSideParaSettingWindow commSideParaSettingWindow;
        /// <summary>
        /// 气象条件设置
        /// </summary>
        public void doCommSideParaSetting()
        {
            //塔位号
            List<string> list = towerList.Select(item => item.TowerName).ToList();

            commSideParaSettingWindow = new CommSideParaSettingWindow();
            ((CommSideParaSettingViewModel)commSideParaSettingWindow.DataContext).CloseWindowEvent += CommSideParaSettingWindowClosed;
            ((CommSideParaSettingViewModel)commSideParaSettingWindow.DataContext).TowerNameList = list;
            commSideParaSettingWindow.ShowDialog();
        }

        public void CommSideParaSettingWindowClosed(object sender, IList<CommSideParaSetting> list)
        {
            CommSideParaSettingViewModel model = (CommSideParaSettingViewModel)sender;

            model.CloseWindowEvent -= CommSideParaSettingWindowClosed;
            if (commSideParaSettingWindow != null) commSideParaSettingWindow.Close();
            commSideParaSettingWindow = null;

            //设置公共和档内参数
            if (list != null)
            {
                var sourcList = towerList;
                int startIndex = 0, endIndex = 0;
                foreach (CommSideParaSetting item in list)
                {
                    try
                    {
                        startIndex = towerList.Where(t => t.TowerName == item.StartTowerName).Single().ID;
                        endIndex = towerList.Where(t => t.TowerName == item.EndTowerName).Single().ID;
                        //筛选需要修改的序列信息
                        var sourceList = sourcList.Where(t => t.ID >= startIndex && t.ID <= endIndex).ToList();

                        foreach (TowerSerial serial in sourceList)
                        {
                            towerList.Where(k => k.ID == serial.ID).First().CommPar = item.CommPara;
                            if(startIndex != serial.ID)
                                towerList.Where(k => k.ID == serial.ID).First().BackSidePar = item.SidePara;
                            if (endIndex != serial.ID)
                                towerList.Where(k => k.ID == serial.ID).First().FrontSidePar = item.SidePara;
                        }
                    }
                    catch (Exception ex)
                    {
                        string msg = ex.Message;
                    }
                }

                View.RefreshCommSidePara(towerList.ToList());
            }
        }
        #endregion


        #region 铁塔配置参数设置
        TowerParaSettingWindow towerParaSettingWindow;
        /// <summary>
        /// 铁塔配置参数设置
        /// </summary>
        public void doTowerParaSetting()
        {
            //塔位号
            List<string> hangList = towerList.Where(item =>item.TowerType == 1).Select(item => item.TowerName).ToList();
            List<string> strainList = towerList.Where(item => item.TowerType != 1).Select(item => item.TowerName).ToList();

            towerParaSettingWindow = new TowerParaSettingWindow();
            ((TowerParaSettingViewModel)towerParaSettingWindow.DataContext).CloseWindowEvent += TowerParaSettingWindowClosed;
            ((TowerParaSettingViewModel)towerParaSettingWindow.DataContext).HangTowerNameList = hangList;
            ((TowerParaSettingViewModel)towerParaSettingWindow.DataContext).StrainTowerNameList = strainList;
            towerParaSettingWindow.ShowDialog();
        }

        public void TowerParaSettingWindowClosed(object sender, IList<TowrParaSetting> hangList, IList<TowrParaSetting> strainList)
        {
            TowerParaSettingViewModel model = (TowerParaSettingViewModel)sender;

            model.CloseWindowEvent -= TowerParaSettingWindowClosed;
            if (towerParaSettingWindow != null) towerParaSettingWindow.Close();
            towerParaSettingWindow = null;

            //设置铁塔配置参数
            if (hangList != null)
            {
                var sourcList = towerList;
                int startIndex = 0, endIndex = 0;
                foreach (TowrParaSetting item in hangList)
                {
                    try
                    {
                        startIndex = towerList.Where(t => t.TowerName == item.StartTowerName).Single().ID;
                        endIndex = towerList.Where(t => t.TowerName == item.EndTowerName).Single().ID;
                        //筛选需要修改的序列信息
                        var sourceList = sourcList.Where(t => t.ID >= startIndex && t.ID <= endIndex && t.TowerType == 1).ToList();

                        foreach (TowerSerial serial in sourceList)
                        {
                            towerList.Where(k => k.ID == serial.ID).First().TowerPar = item.TowerPara;
                        }
                    }
                    catch (Exception ex)
                    {
                        string msg = ex.Message;
                    }
                }
            }

            if (strainList != null)
            {
                var sourcList = towerList;
                int startIndex = 0, endIndex = 0;
                foreach (TowrParaSetting item in strainList)
                {
                    try
                    {
                        startIndex = towerList.Where(t => t.TowerName == item.StartTowerName).Single().ID;
                        endIndex = towerList.Where(t => t.TowerName == item.EndTowerName).Single().ID;
                        //筛选需要修改的序列信息
                        var sourceList = sourcList.Where(t => t.ID >= startIndex && t.ID <= endIndex && t.TowerType != 1).ToList();

                        foreach (TowerSerial serial in sourceList)
                        {
                            towerList.Where(k => k.ID == serial.ID).First().TowerPar = item.TowerPara;
                        }
                    }
                    catch (Exception ex)
                    {
                        string msg = ex.Message;
                    }
                }
            }

            if (hangList != null || strainList != null)
            {
                View.RefreshTowerPara(towerList.ToList());
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

            //string towerPath = globalInfo.ProjectPath + "\\" + ConstVar.DataBaseStr + "\\TowerStr.xml";
            //List<TowerStrData> towers = TowerStrDataReader.ReadLoadFile(towerPath).ToList();
            List<TowerStrData> towers = GlobalInfo.GetInstance().GetLocalTowerStrs();

            TowerStrData tower;//塔型实体
            List<string> StrHeight;//直线塔呼高
            double StrAllowHorSpan;// 直线塔档距序列字符串

            foreach (TowerSerial item in towerList)
            {
                //杆塔型号实体
                tower = towers.Where(t => t.Name == item.TowerPattern).FirstOrDefault();

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
                        if (item.VerticalSpan > tower.AllowedVerSpan && item.IsChecking == false)
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
                        if (item.VerticalSpan > tower.AllowedVerSpan)//垂直档距验算
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
                View.UpdateTowerPara(towerList);
                string savePath = projectUtils.ProjectPath + "\\" + ConstVar.TowerSequenceStr + "\\" + pageName;
                //保存计算后的杆塔序列文件
                TowerSerialReader.SaveDT(towerList, savePath);

                MessageBox.Show("保存成功!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存异常！异常信息:" + ex.Message);
            }
        }

        public override void Save()
        {

        }

        public override void DelSubItem(string itemName)
        {

        }

        public void doCaculate()
        {
            View.UpdateTowerPara(towerList);

            Task.Factory.StartNew(a =>
            {
                caculating(towerList);
            }, towerList);
        }

        protected void  caculating(List<TowerSerial> towers)
        {
            if (towers == null || towers.Count == 0)
                return;

            ElecCals elecCals = new ElecCals(pageName,towers);
            elecCals.TowerSerialsCaculate();
        }
    }
}
