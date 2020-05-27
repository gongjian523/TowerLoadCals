using DevExpress.Mvvm.POCO;
using DevExpress.Xpf.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TowerLoadCals.Modules
{
    public class HangingPointViewModel
    {
        protected int num = 0;

        public static HangingPointViewModel Create()
        {
            return ViewModelSource.Create(() => new HangingPointViewModel());
        }
        protected HangingPointViewModel()
        {
            HangingPoints = new ObservableCollection<HangingPoint>();
            for (num = 1; num < 6; num++)
            {
                HangingPoints.Add(HangingPoint.Create("挂点方案" + num.ToString()));
            }
        }

        public virtual ObservableCollection<HangingPoint> HangingPoints { get; protected set; }
        public void AddNewTab(TabControlTabAddingEventArgs e)
        {
            e.Item = HangingPoint.Create("挂点方案" + num.ToString());
            num++;
        }
    }

    public class HangingPoint
    {
        public static HangingPoint Create(string title)
        {
            return ViewModelSource.Create(() => new HangingPoint(title));
        }
        protected HangingPoint(string title)
        {
            Title = title;
        }

        public string Title { get; private set; }
    }


}
