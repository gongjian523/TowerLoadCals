using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using DevExpress.Xpf.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerLoadCals.Mode;
using static TowerLoadCals.DAL.TowerTemplateReader;

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
            for (num = 1; num < 2; num++)
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

    public class HangingPoint:ViewModelBase,INotifyPropertyChanged
    {


        public static HangingPoint Create(string title)
        {
            return ViewModelSource.Create(() => new HangingPoint(title));
        }
        protected HangingPoint(string title)
        {
            Title = title;
            Type = TowerType.LineTower;

            VStings = new ObservableCollection<VStingParas>();
        }

        public string Title { get; private set; }

        public TowerType Type { get; set; }

        public bool IsLineTower
        {
            get
            {
                return Type == TowerType.LineTower;
            }
        }

        public bool IsTensionTower
        {
            get
            {
                return(Type == TowerType.CornerTower || Type ==  TowerType.BranchTower || Type == TowerType.TerminalTower);
            }
        }

        public string HPColumnXYName
        {
            get
            {
                return IsTensionTower ? "跳线吊装挂点 XY向" : "吊装挂点 XY向";
            }
        }

        public string HPColumnZName
        {
            get
            {
                return IsTensionTower ? "跳线吊装挂点 Z向" : "吊装挂点 Z向";
            }
        }

        public string HPCheckBoxName
        {
            get
            {
                return IsTensionTower ? "过滑车挂架挂点" : "转向挂点";
            }
        }


        private bool _isTensionHPCheck = false;
        public bool IsTurnHPChecked
        {
            get
            {
                return _isTensionHPCheck;
            }
            set
            {
                _isTensionHPCheck = value;
                RaisePropertyChanged("IsTurnHPChecked");
            }
        }

        private bool _isNormalHangingPointVisible = true;
        public bool IsNormalHangingPointVisible
        {
            get
            {
                return _isNormalHangingPointVisible;
            }
            set
            {
                _isNormalHangingPointVisible = value;
                RaisePropertyChanged("IsNormalHangingPointVisible");
            }
        }

        public void NormalHangingPointVisible(bool isNormalVisible)
        {
            IsNormalHangingPointVisible = isNormalVisible;
        }

        public ObservableCollection<VStingParas> VStings { get;  set; }
    }


}
