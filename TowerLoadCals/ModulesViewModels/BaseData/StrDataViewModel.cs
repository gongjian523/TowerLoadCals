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
    public class StrDataViewModel: DaseDataBaseViewModel<StrData, List<StrDataCollection>>
    {
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
            BaseData = StrDataReader.Read(filePath);

            UpdateCurrentSelectedFitData("一般子串");                
        }

        protected void CopyRow()
        {
            ;
        }


        public override void Save()
        {
            UpdateLastSelectedFitData();

            StrDataReader.Save(filePath, BaseData);
        }

        protected void UpdateLastSelectedFitData()
        {
            int index = BaseData.FindIndex(item => item.Type == curType);

            //这种情况只能是FitDataCollectionsz中没有保存相应的type的数据
            if (index == -1)
            {
                BaseData.Add(new StrDataCollection
                {
                    Type = curType,
                    StrDatas = SelectedItems.ToList()
                }); 
            }
            else
            {
                BaseData[index].StrDatas = SelectedItems.ToList();
            }
        }

        protected void UpdateCurrentSelectedFitData(string type)
        {
            curType = type;

            if (BaseData.Where(item => item.Type == curType).Count() == 0)
            {
                SelectedItems.Clear();
                SelectedItems.Add(new StrData { });
            }
            else
            {
                SelectedItems = new ObservableCollection<StrData>(BaseData.Where(item => item.Type == curType).First().StrDatas);
            }
        }

        public override void UpDateView(string para1, string para2 = "")
        {
            UpdateCurrentSelectedFitData(para1);
        }

        public override void DelSubItem(string itemName)
        {
            throw new NotImplementedException();
        }
    }
}
