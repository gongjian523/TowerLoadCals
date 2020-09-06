using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerLoadCals.BLL.Structure;
using TowerLoadCals.Mode.Structure;
using TowerLoadCals.Modules;

namespace TowerLoadCals.ModulesViewModels.StruCals
{
    public class TowerMemberViewModel : StruCalsBaseViewModel
    {
        
        public DelegateCommand SearchAllCommand { get; private set; }
        public DelegateCommand SearchErrorCommand { get; private set; }

        

        public TowerMemberViewModel()
        {
            //memberBLL.UnionTextFile();
            //doSearchAll();//初始数据加载所有信息
            SearchAllCommand = new DelegateCommand(doSearchAll);
            SearchErrorCommand = new DelegateCommand(doSearchError);
        }


        protected override void OnParameterChanged(object parameter)
        {
            InitializeData((string)parameter);
        }

        protected override void InitializeData(string towerName)
        {
            base.InitializeData(towerName);

            if(struCalsParas.ResultFullStess == null)
            {
                DataSource = new ObservableCollection<TowerMember>();
            }
            else
            {
                DataSource = new ObservableCollection<TowerMember>(struCalsParas.ResultFullStess);
            }

        }

        /// <summary>
        /// 全部显示
        /// </summary>
        public void doSearchAll()
        {
            //memberBLL.TextFileReadAll())
            DataSource = new ObservableCollection<TowerMember>(struCalsParas.ResultFullStess);

        }
        
        /// <summary>
        /// 问题显示 
        /// 修
        /// </summary>
        public void doSearchError()
        {
            DataSource = new ObservableCollection<TowerMember>(struCalsParas.ResultFullStess.Where(item => item.EFFIC > 100));
        }

        /// <summary>
        /// 数据源
        /// </summary>
        private ObservableCollection<TowerMember> dataSource;

        public ObservableCollection<TowerMember> DataSource
        {
            get { return dataSource; }
            set { dataSource = value; RaisePropertyChanged(() => DataSource); }
        }

    }
}
