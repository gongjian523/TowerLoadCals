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
    public class ElectricalTowerParViewModel : DaseDataBaseViewModel<ElecCalsTowerRes, List<ElecCalsTowerRes>>
    {



        public List<string> JumpOptions
        {
            get
            {
                return GlobalInfo.GetInstance().GetLocalWireNames(true);
            }
        }

        public List<string> StrDataNameOptions
        {
            get
            {
                return GlobalInfo.GetInstance().GetLocalStrDataNames("一般子串");
            }
        }

        public List<string> JumpStrDataNameOptions
        {
            get
            {
                return GlobalInfo.GetInstance().GetLocalStrDataNames("硬跳线");
            }
        }


        public ElectricalTowerParViewModel()
        {
        }

        protected override void InitializeItemsSource()
        {
            base.InitializeItemsSource();
        }

        protected override void InitializeData()
        {
            BaseData = globalInfo.GetElecCalsTowerParasList();
            UpdateCurTowerParas(BaseData[0].Name);
        }

        public override void Save()
        {
        }

        protected void UpdateCurTowerParas(string name)
        {
            TowerParas = BaseData.Where(item => item.Name == name).FirstOrDefault();

            if (TowerParas == null)
            {
                MessageBox.Show("无法找到此铁塔配置参数！");
                TowerParas = new ElecCalsTowerRes();
            }
        }

        public override void UpDateView(string para1, string para2 = "")
        {
            UpdateCurTowerParas(para1);
        }

        public override void DelSubItem(string itemName)
        {
            throw new NotImplementedException();
        }


        protected ElecCalsTowerRes _towerParas { get; set; }
        public ElecCalsTowerRes TowerParas
        {
            get
            {
                return _towerParas;
            }

            protected set
            {
                _towerParas = value;
                RaisePropertyChanged("TowerParas");
            }
        }

        public void onSave()
        {
            globalInfo.SaveElecCalsTowerParasList();
        }
    }

}
