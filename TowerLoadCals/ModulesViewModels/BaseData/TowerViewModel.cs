using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using TowerLoadCals.DAL;
using TowerLoadCals.Mode;
using TowerLoadCals.ModulesViewModels;

namespace TowerLoadCals.Modules
{
    public class TowerViewModel: DaseDataBaseViewModel<TowerStrData, List<TowerStrCollection>>
    {
        public DelegateCommand CopyRowCommand { get; private set; }

        protected string curType;
        protected string curName;


        protected override void InitializeItemsSource()
        {
            base.InitializeItemsSource();

            filePath = globalInfo.ProjectPath + "\\BaseData\\TowerStr.xml";

            CopyRowCommand = new DelegateCommand(CopyRow);
        }

        protected override void InitializeData()
        {
            BaseData = TowerStrDataReader.Read(filePath);
        }

        protected void CopyRow()
        {
            ;
        }


        public override void Save()
        {
            List<WireType> wireType = new List<WireType>();
            WireReader.Save(filePath, wireType);
        }

        public override void UpDateView(string para1, string para2 = "")
        {
            throw new NotImplementedException();
        }

        public override void DelSubItem(string itemName)
        {
            throw new NotImplementedException();
        }
    }
}
