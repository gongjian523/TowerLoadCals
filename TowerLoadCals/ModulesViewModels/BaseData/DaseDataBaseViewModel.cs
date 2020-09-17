using DevExpress.Mvvm;
using System.Collections.ObjectModel;
using System.ComponentModel;
using TowerLoadCals.BLL;

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

        }

        abstract protected void InitializeData();

        //abstract protected void SelectedItemChanged(object para);

        abstract public void Save();
        abstract public void UpDateView(string para1, string para2 = "");
        abstract public void DelSubItem(string itemName);
    }
}
