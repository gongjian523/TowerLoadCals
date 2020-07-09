using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TowerLoadCals.DAL;
using TowerLoadCals.Mode;
using TowerLoadCals.ModulesViewModels;


namespace TowerLoadCals.Modules
{
    public class FitDataViewModel: DaseDataBaseViewModel<FitData, List<FitDataCollection> >
    {
        public DelegateCommand CopyRowCommand { get; private set; }

        protected string curType;

        protected override void InitializeItemsSource()
        {
            base.InitializeItemsSource();

            filePath = globalInfo.ProjectPath + "\\BaseData\\FitData.xml";

            CopyRowCommand = new DelegateCommand(CopyRow);
        }

        protected override void InitializeData()
        {
            BaseData = FitDataReader.Read(filePath);
            UpdateCurrentSelectedFitData("防震锤");                
        }

        protected void CopyRow()
        {
            ;
        }


        public override void Save()
        {
            UpdateLastSelectedFitData();

            FitDataReader.Save(filePath, BaseData);
        }

        protected void UpdateLastSelectedFitData()
        {
            int index = BaseData.FindIndex(item => item.Type == curType);

            //这种情况只能是FitDataCollectionsz中没有保存相应的type的数据
            if (index == -1)
            {
                BaseData.Add(new FitDataCollection
                {
                    Type = curType,
                    FitDatas = SelectedItems.ToList()
                }); 
            }
            else
            {
                BaseData[index].FitDatas = SelectedItems.ToList();
            }
        }

        protected void UpdateCurrentSelectedFitData(string type)
        {
            curType = type;

            if (BaseData.Where(item => item.Type == curType).Count() == 0)
            {
                SelectedItems.Clear();
                SelectedItems.Add(new FitData { });
            }
            else
            {
                SelectedItems = new ObservableCollection<FitData>(BaseData.Where(item => item.Type == curType).First().FitDatas);
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
