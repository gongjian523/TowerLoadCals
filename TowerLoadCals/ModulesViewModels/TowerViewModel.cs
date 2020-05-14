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


        protected override void  SelectedItemChanged(object para)
        {
            if (((TreeViewItem)para).Header.ToString() == "导地线")
                return;
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
    }
}
