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
    public class WireViewModel: DaseDataBaseViewModel<Wire, List<WireType>>
    {
        public DelegateCommand CopyRowCommand { get; private set; }
        public DelegateCommand PasteRowCommand { get; private set; }

        protected WeatherXmlReader _weatherXmlReader = new WeatherXmlReader();

        protected string curType;


        protected override void InitializeItemsSource()
        {
            base.InitializeItemsSource();

            filePath = globalInfo.ProjectPath + "\\BaseData\\Wire.xml";

            CopyRowCommand = new DelegateCommand(CopyRow);
            PasteRowCommand = new DelegateCommand(PasteRow);
        }

        protected override void InitializeData()
        {
            BaseData = WireReader.Read(filePath);

            UpdateCurrentSelectedWire("导线");

        
        }

        protected void UpdateCurrentSelectedWire(string type)
        {
            curType = type;

            if (BaseData.Where(item => item.Type == curType).Count() == 0)
            {
                SelectedItems.Clear();
                SelectedItems.Add(new Wire { });
            }
            else
            {
                SelectedItems = new ObservableCollection<Wire>(BaseData.Where(item => item.Type == curType).First().Wires);
            }
        }

        protected void UpdateLastSelectedWire()
        {
            int index = BaseData.FindIndex(item => item.Type == curType);

            //这种情况只能是FitDataCollectionsz中没有保存相应的type的数据
            if (index == -1)
            {
                BaseData.Add(new WireType
                {
                    Type = curType,
                    Wires = SelectedItems.ToList()
                });
            }
            else
            {
                BaseData[index].Wires = SelectedItems.ToList();
            }
        }

        protected void CopyRow()
        {
            ;
        }

        protected void PasteRow()
        {
            ;
        }


        public override void Save()
        {
            UpdateLastSelectedWire();

            WireReader.Save(filePath, BaseData);
        }

        public override void UpDateView(string para1, string para2 = "")
        {
            UpdateCurrentSelectedWire(para1);
        }

        public override void DelSubItem(string itemName)
        {
            throw new NotImplementedException();
        }
    }
}
