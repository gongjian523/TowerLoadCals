using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerLoadCals.Common;

namespace TowerLoadCals.ModulesViewModels
{
    abstract public class DaseDataBaseViewModel<T> : ViewModelBase, INotifyPropertyChanged where T:
    class{

        protected GlobalInfo globalInfo;
        protected string filePath;
        
        public DelegateCommand<object> SetSelectedItemCommand { get; private set; }

        private ObservableCollection<T> _selectedItems = new ObservableCollection<T>();
        public ObservableCollection<T> SelectedItems
        {
            get
            {
                return _selectedItems;
            }

            protected set
            {
                _selectedItems = value;
                RaisePropertyChanged("SelectedItems");
            }
        }

        public DaseDataBaseViewModel()
        {
            InitializeItemsSource();
            InitializeData();
        }

        protected virtual void InitializeItemsSource() {

            globalInfo = GlobalInfo.GetInstance();

            SetSelectedItemCommand = new DelegateCommand<object>(SelectedItemChanged);
        }

        abstract protected void InitializeData();

        abstract protected void SelectedItemChanged(object para);

        abstract public void Save();

    }
}
