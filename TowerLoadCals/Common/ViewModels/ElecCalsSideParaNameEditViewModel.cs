using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TowerLoadCals.BLL;
using TowerLoadCals.BLL.Electric;
using TowerLoadCals.Converter;
using TowerLoadCals.Mode;

namespace TowerLoadCals.Common.ViewModels
{
    public class ElecCalsSideParaNameEditViewModel : ViewModelBase
    {
        protected string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                RaisePropertyChanged("Name");
                RaisePropertyChanged("ConfirmCanExecute");
            }
        }

        public bool ConfirmCanExecute
        {
            get
            {
                return (Name != null && Name !="");
            }
        }

        public ElecCalsSideParaNameEditViewModel()
        {
        }

        public delegate void ElecCalsSideParaNameEditCloseHandler(object sender, string e);
        public event ElecCalsSideParaNameEditCloseHandler ElecCalsSideParaNameEditCloseEvent;

        public void onConfirm()
        {
            var list = GlobalInfo.GetInstance().GetElecCalsSideParasList();

            if(list.Where(item=> item.Name == Name).Count() > 0)
            {
                MessageBox.Show("同名参数已经存在，请更换一个名字");
            }
            else
            {
                list.Add(new ElecCalsSideRes
                {
                    Name = Name,
                });
                close(Name);
            }
        }

        public void onConcel()
        {
            close("");
        }

        protected void close(string e)
        {
            if (ElecCalsSideParaNameEditCloseEvent != null)
                ElecCalsSideParaNameEditCloseEvent(this, e);
        }
    }
}
