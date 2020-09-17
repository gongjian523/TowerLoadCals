using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using DevExpress.Office.History;
using DevExpress.Xpf.Core;
using DevExpress.XtraRichEdit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using TowerLoadCals.BLL;
using TowerLoadCals.BLL.Electric;
using TowerLoadCals.DAL;
using TowerLoadCals.DAL.Common;
using TowerLoadCals.Mode;
using TowerLoadCals.Mode.Electric;
using TowerLoadCals.Modules;

namespace TowerLoadCals.ModulesViewModels.Electrical
{
    /// <summary>
    /// 单侧参数
    /// </summary>
    public class ElectricalSideParViewModel : DaseDataBaseViewModel<ElecCalsSideRes, List<ElecCalsSideRes>>
    {

        public List<string> WeatherListOptions
        {
            get
            {
                return GlobalInfo.GetInstance().GetLocalWeatherNames();
            }
        }

        public List<string> IceAreaOptions
        {
            get
            {
                return new List<string> { "无冰区", "轻冰区", "中冰区", "重冰区", };
            }
        }

        public List<string> IndOptions
        {
            get
            {
                return GlobalInfo.GetInstance().GetLocalWireNames(true);
            }
        }

        public List<string> GrdOptions
        {
            get
            {
                return GlobalInfo.GetInstance().GetLocalWireNames(false);
            }
        }

        public List<string> JGBNameOptions
        {
            get
            {
                return GlobalInfo.GetInstance().GetLocalFitDataNames("间隔棒");
            }
        }

        public List<string> FZNameOptions
        {
            get
            {
                return GlobalInfo.GetInstance().GetLocalFitDataNames("防震锤");
            }
        }

        public List<string> FitDataCalsParaOptions
        {
            get
            {
                return new List<string> { "1 按规则", "2 手动配置", }; ;
            }
        }
        


        public ElectricalSideParViewModel()
        {
        }

        protected override void InitializeItemsSource()
        {
            base.InitializeItemsSource();
        }

        protected override void InitializeData()
        {
            BaseData = globalInfo.GetElecCalsSideParasList();
            UpdateCurSideParas(BaseData[0].Name);
        }

        public override void Save()
        {
        }

        protected void UpdateCurSideParas(string name)
        {
            SideParas = BaseData.Where(item => item.Name == name).FirstOrDefault();

            if (SideParas == null)
            {
                MessageBox.Show("无法找到此档内参数！");
                SideParas = new ElecCalsSideRes();
            }
        }

        public override void UpDateView(string para1, string para2 = "")
        {
            UpdateCurSideParas(para1);
        }

        public override void DelSubItem(string itemName)
        {
            throw new NotImplementedException();
        }


        protected ElecCalsSideRes _sideParas { get; set; }
        public ElecCalsSideRes SideParas
        {
            get
            {
                return _sideParas;
            }

            protected set
            {
                _sideParas = value;
                RaisePropertyChanged("SideParas");
            }
        }

        public void onSave()
        {
            GlobalInfo.GetInstance().SaveElecCalsSideParasList();
        }
    }

}
