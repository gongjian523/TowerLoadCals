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

        protected WeatherXmlReader _weatherXmlReader = new WeatherXmlReader();

        protected string curType;

        protected ObservableCollection<Wire> wireDi = new ObservableCollection<Wire>();

        protected ObservableCollection<Wire> wireDao = new ObservableCollection<Wire>();

        protected override void InitializeItemsSource()
        {
            base.InitializeItemsSource();

            filePath = globalInfo.ProjectPath + "\\BaseData\\Wire.xml";

            CopyRowCommand = new DelegateCommand(CopyRow);
        }

        protected override void InitializeData()
        {
            BaseData = WireReader.Read(filePath);

            if (BaseData.Where(item => item.Type == "导线").Count() == 0)
            {
                wireDao.Add(new Wire { });
            }
            else
            {
                wireDao = new ObservableCollection<Wire>(BaseData.Where(item => item.Type == "导线").First().Wire);
            }

            if (BaseData.Where(item => item.Type == "地线").Count() == 0)
            {
                wireDi.Add(new Wire { });
            }
            else
            {
                wireDi = new ObservableCollection<Wire>(BaseData.Where(item => item.Type == "地线").First().Wire);
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
                wireDao = SelectedItems;
            }
            else
            {
                wireDi = SelectedItems;
            }

            if (((TreeViewItem)para).Header.ToString() == "导线")
            {
                curType = "导线";
                SelectedItems = wireDao;
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


        public override void Save()
        {
            List<WireType> wireType = new List<WireType>();

            wireType.Add(new WireType
            {
                Type = "导线",
                Wire = wireDao.ToList()
            }) ; 

            wireType.Add(new WireType
            {
                Type = "地线",
                Wire = wireDi.ToList()
            });

            WireReader.Save(filePath, wireType);
        }
    }
}
