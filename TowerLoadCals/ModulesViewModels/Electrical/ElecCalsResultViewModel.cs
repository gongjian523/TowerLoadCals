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
using TowerLoadCals.Modules.Electrical;

namespace TowerLoadCals.ModulesViewModels.Electrical
{
    /// <summary>
    /// 电气计算结果
    /// </summary>
    public class ElecCalsResultViewModel : ViewModelBase, IBaseViewModel, INotifyPropertyChanged
    {
        protected ElecCalsResultModule ViewMode;

        public ElecCalsResultViewModel(ElecCalsResultModule viewMode)
        {
            ViewMode = viewMode;
        }

        protected  void InitializeItemsSource()
        {
        }

        protected  void InitializeData()
        {

        }

        public void Save()
        {
        }

        protected void UpdateCurCommParas(string name)
        {

        }

        public  void UpDateView(string para1, string para2 = "")
        {
            UpdateCurCommParas(para1);
        }

        public  void DelSubItem(string itemName)
        {
            throw new NotImplementedException();
        }
    }


}