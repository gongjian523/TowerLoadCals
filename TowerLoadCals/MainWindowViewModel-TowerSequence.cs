using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using TowerLoadCals.Modules;
using TowerLoadCals.Modules.TowerSequence;
using TowerLoadCals.ModulesViewModels;
using TowerLoadCals.ModulesViewModels.TowerSequence;

namespace TowerLoadCals
{
    public partial class MainWindowViewModel
    {
        /// <summary>
        /// 打开或者新建工程时，初始化的结构计算按钮
        /// 为了节省操作工程的时间，不初始下这个按钮的子菜单
        /// </summary>
        /// <returns></returns>
        private ModuleMenu IniTowerSequenceModule()
        {
            ModuleMenu towerSequenceMudule = new ModuleMenu("TowerSequenceModule", this, "杆塔序列", (e) => { OnTowerSequenceModuleSeleced(e); });
            towerSequenceMudule.SetIcon("FolderList_32x32.png");

            return towerSequenceMudule;
        }

        /// <summary>
        /// 点击结构计算按钮时的操作
        /// </summary>
        /// <param name="mv"></param>
        private void OnTowerSequenceModuleSeleced(ModuleMenu mv)
        {
            NewTowerSequenceTowerBtnVisibity = Visibility.Visible;
            NewStruCalsTowerBtnVisibity = Visibility.Collapsed;

            //以前没有加载过子菜单，从配置文件中读出所有塔位名称
            if (mv.MenuItems == null || mv.MenuItems.Count() == 0)
            {
                var menuItems = new List<SubMenuBase>() { };

                var menuList = projectUtils.GetAllTowerSequenceNames();

                foreach (var name in menuList)
                {
                    SubMenuBase menu = new SubMenuBase("TowerSequenceModule", this, name, (e) => { OnSelectedTowerSequenceChanged(e); });
                    menu.SetIcon("Menu_weather.png");
                    menuItems.Add(menu);

                }

                mv.MenuItems = menuItems;
            }

            MenuItems = new ObservableCollection<SubMenuBase>(SelectedModuleInfo.MenuItems);
        }
        /// <summary>
        /// 切换界面信息
        /// </summary>
        /// <param name="vm"></param>
        private void OnSelectedTowerSequenceChanged(SubMenuBase vm)
        {
            UpdateSubModule(vm); 
            
            var subVm = GetCurSubModuleVM();

            if (subVm == null)
                return;

            subVm.UpDateView(vm.Title.Trim());
        }

        public void NewTowerSequenceSubMenuItem(SubMenuBase menuVm)
        {
            MenuItems = new ObservableCollection<SubMenuBase>(SelectedModuleInfo.MenuItems);
            menuVm.ChildItems[0].Show(menuVm.Title);
        }

        AddTowerSequenceWindow addTowerSequenceWindow;
        #region 新增杆塔序列
        public void AddTowerSequence()
        {
            addTowerSequenceWindow = new AddTowerSequenceWindow();
            ((AddTowerSequenceViewModel)(addTowerSequenceWindow.DataContext)).CloseWindowEvent += AddTowerSequenceWindowClosed;
            addTowerSequenceWindow.ShowDialog();
        }
        public void AddTowerSequenceWindowClosed(object sender, string newSequenceName)
        {
            AddTowerSequenceViewModel model = (AddTowerSequenceViewModel)sender;
            model.CloseWindowEvent -= AddTowerSequenceWindowClosed;
            if (addTowerSequenceWindow != null) addTowerSequenceWindow.Close();
            addTowerSequenceWindow = null;

            if (newSequenceName == null || newSequenceName == "")
            {
                return;
            }

            SubMenuBase newSequenceMenu = new SubMenuBase("TowerSequenceModule", this, newSequenceName, (e) => { OnSelectedTowerSequenceChanged(e); });
            newSequenceMenu.SetIcon("Menu_weather.png");

            SelectedModuleInfo.MenuItems.Add(newSequenceMenu);

            MenuItems = new ObservableCollection<SubMenuBase>(SelectedModuleInfo.MenuItems);
        }
        #endregion

        //新增杆塔序列按钮是否可见
        protected Visibility _newTowerSequenceTowerBtnVisibity = Visibility.Collapsed;
        public Visibility NewTowerSequenceTowerBtnVisibity
        {
            set
            {
                _newTowerSequenceTowerBtnVisibity = value;
                RaisePropertyChanged("NewTowerSequenceTowerBtnVisibity");
            }
            get
            {
                return _newTowerSequenceTowerBtnVisibity;
            }
        }

    }
}
