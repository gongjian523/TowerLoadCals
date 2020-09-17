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
    public class ElecCalsTowerParaNameEditViewModel : ViewModelBase
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

        public ElecCalsTowerParaNameEditViewModel()
        {
        }

        public delegate void ElecCalsTowerParaNameEditCloseHandler(object sender, string e);
        public event ElecCalsTowerParaNameEditCloseHandler ElecCalsTowerParaNameEditCloseEvent;

        public void onConfirm()
        {
            var list = GlobalInfo.GetInstance().GetElecCalsTowerParasList();

            if(list.Where(item=> item.Name == Name).Count() > 0)
            {
                MessageBox.Show("同名参数已经存在，请更换一个名字");
            }
            else
            {
                list.Add(new ElecCalsTowerRes
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
            if (ElecCalsTowerParaNameEditCloseEvent != null)
                ElecCalsTowerParaNameEditCloseEvent(this, e);
        }
    }
}
