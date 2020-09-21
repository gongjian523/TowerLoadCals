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

namespace TowerLoadCals.ModulesViewModels.Electrical
{
    /// <summary>
    /// 公共参数
    /// </summary>
    public class ElectricalCommonParViewModel : DaseDataBaseViewModel<ElecCalsCommRes, List<ElecCalsCommRes>>
    {
        public List<String> VoltOptions
        {
            get
            {
                return new List<string>() { "110kV", "220kV", "330kV", "500kV", "700kV", "1000kV", "±500", "±800", "±1100kV" }; 
            }
        }

        public List<String> CalModeOptions
        {
            get
            {
                return new List<string>() { "1 GB50545-2010", "2 新规则", };
            }
        }


        public List<String> CircuitTypeOptions
        {
            get
            {
                return new List<string>() { "1 单回路", "2 双回路", };
            }
        }

        public List<String> TerrainOptions
        {
            get
            {
                return new List<string> { "山地", "平丘" };
            }
        }

        public List<char> TerTypeOptions
        {
            get
            {
                return new List<char> { 'A', 'B', 'C', 'D', };
            }
        }

        public List<string> BoolOptions
        {
            get
            {
                return new List<string> { "是", "否", };
            }
        }

        public List<string> VerWeiOptions
        {
            get
            {
                return new List<string> { "冰荷载百分比", "计算值", };
            }
        }

        public List<string> WireWindParaOptions
        {
            get
            {
                return new List<string> { "按挂点高", "按线平均高", };
            }
        }

        public List<string> JumpWindParaOptions
        {
            get
            {
                return new List<string> { "按挂点高", "按平均高", };
            }
        }

        //
        public List<string> GrdIceForceParaOptions
        {
            get
            {
                return new List<string> { "+5mm冰张力", "正常冰张力", };
            }
        }

        public List<string> GrdIceUnbaParaOptions
        {
            get
            {
                return new List<string> { "不考虑+5mm", "考虑+5mm", };
            }
        }

        public List<string> HandForceParaOptions
        {
            get
            {
                return new List<string> { "两者大值", "系数法", "降温法", };
            }
        }

        public List<string> InParaOptions
        {
            get
            {
                return new List<string> { "0/张力差", "max/(max-张力差)" };
            }
        }

        public List<string> MaxParaOptions
        {
            get
            {
                return new List<string> { "最大允许张力", "100%覆冰率断线工况", };
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        public DelegateCommand doSaveCommand { get; private set; }
        protected int num = 0;

        private ElecCalsCommRes _comParas { get; set; }
        public ElecCalsCommRes ComParas
        {
            get
            {
                return _comParas;
            }

            protected set
            {
                _comParas = value;
                RaisePropertyChanged("ComParas");
            }
        }

        public ElectricalCommonParViewModel()
        {
        }

        protected override void InitializeItemsSource()
        {
            base.InitializeItemsSource();
        }

        protected override void InitializeData()
        {
            BaseData = globalInfo.GetElecCalsCommParasList();
            UpdateCurCommParas(BaseData[0].Name);
        }

        public override void Save()
        {
        }

        protected void UpdateCurCommParas(string name)
        {
            ComParas = BaseData.Where(item => item.Name == name).FirstOrDefault();

            if(ComParas == null)
            {
                MessageBox.Show("无法找到此公共参数！");
                ComParas = new ElecCalsCommRes();
            }
        }

        public override void UpDateView(string para1, string para2 = "")
        {
            UpdateCurCommParas(para1);
        }

        public override void DelSubItem(string itemName)
        {
            throw new NotImplementedException();
        }
    }


}