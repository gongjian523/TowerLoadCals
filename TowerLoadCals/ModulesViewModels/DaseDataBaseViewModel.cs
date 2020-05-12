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
    abstract public class DaseDataBaseViewModel<T,K> : ViewModelBase, IBaseViewModel, INotifyPropertyChanged where T :
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

        private K _baseData { get; set; }
        public K  BaseData
        {
            get
            {
                return _baseData;
            }

            protected set
            {
                _baseData = value;
                RaisePropertyChanged("BaseData");
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
