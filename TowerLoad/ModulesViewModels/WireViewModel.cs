using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Grid;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using TowerLoadCals.Common;
using TowerLoadCals.DAL;
using TowerLoadCals.Mode;
using TowerLoadCals.ModulesViewModels;
using TextBox = System.Windows.Controls.TextBox;

namespace TowerLoadCals.Modules
{
    public class WireViewModel: DaseDataBaseViewModel<WireCh>
    {
        public List<WireType> WireTypes { get; set; }

        private ObservableCollection<WireCh> _SelectedWire = new ObservableCollection<WireCh>();
        public ObservableCollection<WireCh> SelectedWire
        {
            get
            {
                return _SelectedWire;
            }

            private set
            {
                _SelectedWire = value;
                RaisePropertyChanged("SelectedWire");
            }
        }

        public DelegateCommand CopyRowCommand { get; private set; }


        protected WeatherXmlReader _weatherXmlReader = new WeatherXmlReader();

        protected string curType;

        protected ObservableCollection<WireCh> wireDi = new ObservableCollection<WireCh>();

        protected ObservableCollection<WireCh> wireDao = new ObservableCollection<WireCh>();


        protected override void InitializeItemsSource()
        {
            base.InitializeItemsSource();

            filePath = globalInfo.ProjectPath + "\\BaseData\\Wire.xml";

            CopyRowCommand = new DelegateCommand(CopyRow);
        }

        protected override void InitializeData()
        {
            WireTypes = WireReader.Read(filePath);

            if (WireTypes.Where(item => item.Type == "导线").Count() == 0)
            {
                wireDi.Add(new WireCh { });
            }
            else
            {
                var list = WireTypes.Where(item => item.Type == "导线").First().Wire;
                WireEntityConvert(wireDao, list, false);
            }

            if (WireTypes.Where(item => item.Type == "地线").Count() == 0)
            {
                wireDi.Add(new WireCh { });
            }
            else
            {
                var list = WireTypes.Where(item => item.Type == "地线").First().Wire;
                WireEntityConvert(wireDi, list, false);
            }

            curType = "导线";
            SelectedItems = wireDao;
            
        }


        protected override void  SelectedItemChanged(object para)
        {
            if (((TreeViewItem)para).Header.ToString() == "导地线")
                return;

            if(curType == "导线")
            {
                wireDao = SelectedWire;
            }
            else
            {
                wireDi = SelectedWire;
            }

            if (((TreeViewItem)para).Header.ToString() == "导线")
            {
                curType = "导线";
                SelectedWire = wireDao;
                return;
            }
            else
            {
                curType = "地线";
                SelectedItems = wireDi;
                return;
            }
        }

        protected void CopyRow()
        {
            ;
        }

        protected void WireEntityConvert(ObservableCollection<WireCh> entityCh, List<Wire> entity, bool isChtoEn = true)
        {
            if(isChtoEn)
            {
                entity.Clear();
                foreach (WireCh chItem in entityCh)
                {
                    entity.Add(new Wire
                    {
                        ModelSpecification = chItem.型号规格,
                        WireType = chItem.线类型,
                        SectionArea = chItem.截面积,
                        ExternalDiameter = chItem.外径,
                        UnitLengthMass = chItem.单位长度质量,
                        DCResistor = chItem.直流电阻,
                        RatedBreakingForce = chItem.额定拉断力,
                        ModulusElasticity = chItem.弹性模量,
                        LineCoefficient = chItem.线性膨胀系数,
                    });
                }
            }
            else
            {
                entityCh.Clear();
                foreach (Wire ehItem in entity)
                {
                    entityCh.Add(new WireCh
                    {
                        型号规格 = ehItem.ModelSpecification,
                        线类型 = ehItem.WireType,
                        截面积 = ehItem.SectionArea,
                        外径 = ehItem.ExternalDiameter,
                        单位长度质量 = ehItem.UnitLengthMass,
                        直流电阻 = ehItem.DCResistor,
                        额定拉断力 = ehItem.RatedBreakingForce,
                        弹性模量 = ehItem.ModulusElasticity,
                        线性膨胀系数 = ehItem.LineCoefficient,
                    });
                }
            }
        }

        public override void Save()
        {
            List<WireType> wireType = new List<WireType>();

            List<Wire> entityDao = new List<Wire>();
            WireEntityConvert(wireDao, entityDao);
            wireType.Add(new WireType
            {
                Type = "导线",
                Wire = entityDao
            });

            List<Wire> entityDi = new List<Wire>();
            WireEntityConvert(wireDi, entityDi);
            wireType.Add(new WireType
            {
                Type = "地线",
                Wire = entityDi
            });

            WireReader.Save(filePath, wireType);
        }
    }
}
