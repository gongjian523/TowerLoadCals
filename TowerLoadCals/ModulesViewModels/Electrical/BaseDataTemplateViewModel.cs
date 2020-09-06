using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using DevExpress.XtraRichEdit;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using TowerLoadCals.BLL;
using TowerLoadCals.Mode;
using TowerLoadCals.Mode.Electric;

namespace TowerLoadCals.ModulesViewModels.Electrical
{
    public class BaseDataTemplateViewModel : ViewModelBase
    {
        protected BaseDataTemplateViewModel()
        {
            var data = GlobalInfo.GetInstance().GetElecCalsSpecParas();


            //基本系数
            var basics = from v in data.Category
                         join w in data.WindCoef on v.Voltage equals w.Voltage
                         join j in data.JumpWireWind on v.Voltage equals j.Voltage
                         select new Basics_Output
                         {
                             Voltage = v.Voltage,
                             Category = v.Category,
                             WindCategory = w.Category,
                             Coef = j.Coef
                         };

            this.basicsSource = new ObservableCollection<Basics_Output>(basics.ToList());//基础系数


            this.loadSource = new ObservableCollection<ElecCalsVertLoadSpec>(data.VerticalLoad); //垂直荷载数据源

            var tension = data.BreakWireStress.GroupBy(k => new { k.IceThickness, k.Type, k.Terrain }).Select(g => new Tension_Output
            {
                IceThickness = g.Key.IceThickness,
                Type = g.Key.Type,
                Terrain = string.IsNullOrEmpty(g.Key.Terrain) ? "全部" : g.Key.Terrain,

                X_DWires = g.Where(item => item.TowerType == "悬垂塔" && item.WireType == "单导线").Count() > 0 ? g.Where(item => item.TowerType == "悬垂塔" && item.WireType == "单导线").First().Stress : 0,
                X_SWires = g.Where(item => item.TowerType == "悬垂塔" && item.WireType == "双分裂导线").Count() > 0 ? g.Where(item => item.TowerType == "悬垂塔" && item.WireType == "双分裂导线").First().Stress : 0,
                X_SFWires = g.Where(item => item.TowerType == "悬垂塔" && item.WireType == "双分裂以上导线").Count() > 0 ? g.Where(item => item.TowerType == "悬垂塔" && item.WireType == "双分裂以上导线").First().Stress : 0,
                X_GroundWires = g.Where(item => item.TowerType == "悬垂塔" && item.WireType == "地线").Count() > 0 ? g.Where(item => item.TowerType == "悬垂塔" && item.WireType == "地线").First().Stress : 0,

                N_DWires = g.Where(item => item.TowerType == "耐张塔" && item.WireType == "单导线").Count() > 0 ? g.Where(item => item.TowerType == "耐张塔" && item.WireType == "单导线").First().Stress : 0,
                N_SFWires = g.Where(item => item.TowerType == "耐张塔" && item.WireType == "双分裂以上导线").Count() > 0 ? g.Where(item => item.TowerType == "耐张塔" && item.WireType == "双分裂以上导线").First().Stress : 0,
                N_GroundWires = g.Where(item => item.TowerType == "耐张塔" && item.WireType == "地线").Count() > 0 ? g.Where(item => item.TowerType == "耐张塔" && item.WireType == "地线").First().Stress : 0
            }).ToList();

            var tension1 = data.BreakIceRate.GroupBy(k => new { k.IceThickness, k.Type }).Select(g => new Tension_Output
            {
                IceThickness = g.Key.IceThickness,
                Type = g.Key.Type,
                X_FCategory = g.Where(item => item.TowerType == "悬垂塔" && item.Category == "一类").Count() > 0 ? g.Where(item => item.TowerType == "悬垂塔" && item.Category == "一类").First().Percent : 0,
                X_SCategory = g.Where(item => item.TowerType == "悬垂塔" && item.Category == "二类").Count() > 0 ? g.Where(item => item.TowerType == "悬垂塔" && item.Category == "二类").First().Percent : 0,
                X_TCategory = g.Where(item => item.TowerType == "悬垂塔" && item.Category == "三类").Count() > 0 ? g.Where(item => item.TowerType == "悬垂塔" && item.Category == "三类").First().Percent : 0,

                N_FCategory = g.Where(item => item.TowerType == "耐张塔" && item.Category == "一类").Count() > 0 ? g.Where(item => item.TowerType == "耐张塔" && item.Category == "一类").First().Percent : 0,
                N_SCategory = g.Where(item => item.TowerType == "耐张塔" && item.Category == "二类").Count() > 0 ? g.Where(item => item.TowerType == "耐张塔" && item.Category == "二类").First().Percent : 0,
                N_TCategory = g.Where(item => item.TowerType == "耐张塔" && item.Category == "三类").Count() > 0 ? g.Where(item => item.TowerType == "耐张塔" && item.Category == "三类").First().Percent : 0

            }).ToList();

            List<Tension_Output> tensionlist = new List<Tension_Output>();
            foreach (Tension_Output item in tension)
            {
                item.Type = string.IsNullOrEmpty(item.Type) ? "0" : item.Type;
                Tension_Output w = tension1.Where(k => item.IceThickness == k.IceThickness && item.Type == k.Type).Count() > 0 ? tension1.Where(k => item.IceThickness == k.IceThickness && item.Type == k.Type).First() : null;
                if (w != null)
                {
                    item.X_FCategory = w.X_FCategory;
                    item.X_SCategory = w.X_FCategory;
                    item.X_TCategory = w.X_TCategory;

                    item.N_FCategory = w.N_FCategory;
                    item.N_SCategory = w.N_SCategory;
                    item.N_TCategory = w.N_TCategory;
                }
                tensionlist.Add(item);
            }
            this.tensionSource = new ObservableCollection<Tension_Output>(tensionlist.ToList()); //断线张力取值;


            //不均匀冰不平衡张力  
            //UnevenIceStress=不平衡张力
            var unevenIceStressList = data.UnevenIceStress.GroupBy(k => new { k.IceThickness, k.Type }).Select(g => new UnevenIceStress_Output
            {
                IceThickness = g.Key.IceThickness,
                Type = string.IsNullOrEmpty(g.Key.Type) ? "0" : g.Key.Type,
                X_Wires = g.Where(item => item.TowerType == "悬垂塔" && item.WireType == "导线").Count() > 0 ? g.Where(item => item.TowerType == "悬垂塔" && item.WireType == "导线").First().Stress : 0,
                X_GroundWires = g.Where(item => item.TowerType == "悬垂塔" && item.WireType == "地线").Count() > 0 ? g.Where(item => item.TowerType == "悬垂塔" && item.WireType == "地线").First().Stress : 0,
                N_Wires = g.Where(item => item.TowerType == "耐张塔" && item.WireType == "导线").Count() > 0 ? g.Where(item => item.TowerType == "耐张塔" && item.WireType == "导线").First().Stress : 0,
                N_GroundWires = g.Where(item => item.TowerType == "耐张塔" && item.WireType == "地线").Count() > 0 ? g.Where(item => item.TowerType == "耐张塔" && item.WireType == "地线").First().Stress : 0,
            }).ToList();

            this.iceUnbalanceSource = new ObservableCollection<UnevenIceStress_Output>(unevenIceStressList); //不均匀冰不平衡张力;



            //断线覆冰  
            //UnevenIceRate=不均匀冰覆冰
            var unevenIceRate = data.UnevenIceRate.GroupBy(k => new { k.Category }).Select(g => new UnevenIceRate_Output
            {
                Category = g.Key.Category,
                X_Side = g.Where(item => item.TowerType == "悬垂塔" && item.Side == "一侧").Count() > 0 ? g.Where(item => item.TowerType == "悬垂塔" && item.Side == "一侧").First().Percent : 0,
                X_OtherSide = g.Where(item => item.TowerType == "悬垂塔" && item.Side == "另一侧").Count() > 0 ? g.Where(item => item.TowerType == "悬垂塔" && item.Side == "另一侧").First().Percent : 0,
                N_Side = g.Where(item => item.TowerType == "悬垂塔" && item.Side == "一侧").Count() > 0 ? g.Where(item => item.TowerType == "耐张塔" && item.Side == "一侧").First().Percent : 0,
                N_OtherSide = g.Where(item => item.TowerType == "耐张塔" && item.Side == "另一侧").Count() > 0 ? g.Where(item => item.TowerType == "耐张塔" && item.Side == "另一侧").First().Percent : 0
            }).ToList();

            this.lineGradeSource = new ObservableCollection<UnevenIceRate_Output>(unevenIceRate); //不均匀冰不平衡张力;;
        }



        /// <summary>
        ///基础系数数据源
        /// </summary>
        private ObservableCollection<Basics_Output> basicsSource;

        public ObservableCollection<Basics_Output> BasicsSource
        {
            get { return basicsSource; }
            set { basicsSource = value; }
        }

        /// <summary>
        /// 垂直荷载数据源
        /// </summary>
        private ObservableCollection<ElecCalsVertLoadSpec> loadSource;

        public ObservableCollection<ElecCalsVertLoadSpec> LoadSource
        {
            get { return loadSource; }
            set { loadSource = value; }
        }
        /// <summary>
        /// 断线张力取值
        /// </summary>
        private ObservableCollection<Tension_Output> tensionSource;

        public ObservableCollection<Tension_Output> TensionSource
        {
            get { return tensionSource; }
            set { tensionSource = value; }
        }
        /// <summary>
        /// 不均匀冰不平衡张力
        /// </summary>
        private ObservableCollection<UnevenIceStress_Output> iceUnbalanceSource;

        public ObservableCollection<UnevenIceStress_Output> IceUnbalanceSource
        {
            get { return iceUnbalanceSource; }
            set { iceUnbalanceSource = value; }
        }
        /// <summary>
        /// 数据源
        /// </summary>
        private ObservableCollection<UnevenIceRate_Output> lineGradeSource;

        public ObservableCollection<UnevenIceRate_Output> LineGradeSource
        {
            get { return lineGradeSource; }
            set { lineGradeSource = value; }
        }
    }



}
