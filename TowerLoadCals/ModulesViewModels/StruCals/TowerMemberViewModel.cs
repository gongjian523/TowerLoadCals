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
    public class TowerMemberViewModel : ViewModelBase
    {
        TowerMemberBLL memberBLL = new TowerMemberBLL();
        public DelegateCommand SearchAllCommand { get; private set; }
        public DelegateCommand SearchErrorCommand { get; private set; }
        public TowerMemberViewModel()
        {
            memberBLL.UnionTextFile();
            doSearchAll();//初始数据加载所有信息
            SearchAllCommand = new DelegateCommand(doSearchAll);
            SearchErrorCommand = new DelegateCommand(doSearchError);
        }

        /// <summary>
        /// 查询方法
        /// </summary>
        public void doSearchAll()
        {
            this.DataSource = new ObservableCollection<TowerMember>(memberBLL.TextFileReadAll());

        }
        /// <summary>
        /// 查询方法
        /// </summary>
        public void doSearchError()
        {
            this.DataSource = new ObservableCollection<TowerMember>(memberBLL.TextFileReadAll().Where(item => item.EFFIC > 100));
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
