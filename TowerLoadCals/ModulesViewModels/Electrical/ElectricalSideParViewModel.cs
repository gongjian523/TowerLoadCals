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
    public class ElectricalSideParViewModel : ElecCalsSideResViewModel
    {

        protected int num = 0;

        public static ElectricalSideParViewModel Create()
        {
            return ViewModelSource.Create(() => new ElectricalSideParViewModel());
        }
        protected ElectricalSideParViewModel()
        {
        }

        protected override void OnParameterChanged(object parameter)
        {
            InitializeData((string)parameter);
        }

        protected override void InitializeData(string towerName)
        {
            base.InitializeData(towerName);

            ElectricalSidePars = new ObservableCollection<ElectricalSidePar>();
            for (num = 1; num < 2; num++)
            {
                ElectricalSidePars.Add(ElectricalSidePar.Create("单侧参数" + num.ToString(), elecCalsSideRes));
            }
        }

        public virtual ObservableCollection<ElectricalSidePar> ElectricalSidePars { get; protected set; }
        public void AddNewTab(TabControlTabAddingEventArgs e)
        {

            e.Item = ElectricalSidePar.Create("单侧参数" + num.ToString(), elecCalsSideRes);
            num++;
        }

    }

    public class ElectricalSidePar : ViewModelBase, INotifyPropertyChanged
    {
        public static ElectricalSidePar Create(string title, ElecCalsSideRes calsParas)
        {
            return ViewModelSource.Create(() => new ElectricalSidePar(title, calsParas));
        }
        protected ElectricalSidePar(string title, ElecCalsSideRes calsParas)
        {
            Title = title;



        }
        public string Title { get; private set; }
    }
}
