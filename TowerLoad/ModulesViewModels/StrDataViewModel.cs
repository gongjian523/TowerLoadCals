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


namespace TowerLoadCals.Modules
{
    public class StrDataViewModel: DaseDataBaseViewModel<StrData>
    {
        public List<StrDataCollection> StrDataCollections { get; set; }

        public DelegateCommand CopyRowCommand { get; private set; }


        protected string curType;


        protected override void InitializeItemsSource()
        {
            base.InitializeItemsSource();

            filePath = globalInfo.ProjectPath + "\\BaseData\\StrData.xml";

            CopyRowCommand = new DelegateCommand(CopyRow);
        }

        protected override void InitializeData()
        {
            StrDataCollections = StrDataReader.Read(filePath);

            UpdateCurrentSelectedFitData("一般子串");                
        }

        protected override void  SelectedItemChanged(object para)
        {
            string sendItemName = ((TreeViewItem)para).Header.ToString();

            if (sendItemName == "绝缘子串")
                return;

            UpdateLastSelectedFitData();

            UpdateCurrentSelectedFitData(sendItemName);
        }

        protected void CopyRow()
        {
            ;
        }


        public override void Save()
        {
            UpdateLastSelectedFitData();

            StrDataReader.Save(filePath, StrDataCollections);
        }

        protected void UpdateLastSelectedFitData()
        {
            int index = StrDataCollections.FindIndex(item => item.Type == curType);

            //这种情况只能是FitDataCollectionsz中没有保存相应的type的数据
            if (index == -1)
            {
                StrDataCollections.Add(new StrDataCollection
                {
                    Type = curType,
                    StrDatas = SelectedItems.ToList()
                }); 
            }
            else
            {
                StrDataCollections[index].StrDatas = SelectedItems.ToList();
            }
        }

        protected void UpdateCurrentSelectedFitData(string type)
        {
            curType = type;

            if (StrDataCollections.Where(item => item.Type == curType).Count() == 0)
            {
                SelectedItems.Clear();
                SelectedItems.Add(new StrData { });
            }
            else
            {
                SelectedItems = new ObservableCollection<StrData>(StrDataCollections.Where(item => item.Type == curType).First().StrDatas);
            }
        }
    }
}
