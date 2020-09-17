using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using TowerLoadCals.BLL;
using TowerLoadCals.BLL.Electric;
using TowerLoadCals.Common.Modules;
using TowerLoadCals.Common.ViewModels;
using TowerLoadCals.Modules;
using TowerLoadCals.Modules.TowerSequence;
using TowerLoadCals.ModulesViewModels;
using TowerLoadCals.ModulesViewModels.TowerSequence;



/// <summary>
/// created by : glj
/// </summary>


namespace TowerLoadCals
{
    public partial class MainWindowViewModel
    {
        ModuleMenu ElectricalCalsMenu;
        /// <summary>
        /// 打开或者新建工程时，初始化的结构计算按钮
        /// 为了节省操作工程的时间，不初始下这个按钮的子菜单
        /// </summary>
        /// <returns></returns>
        private ModuleMenu IniElectricalCalsModule()
        {
            ElectricalCalsMenu = new ModuleMenu("ElecCalsModule", this, "电气计算", (e) => { OnSelectedModuleChanged(e); });
            ElectricalCalsMenu.SetIcon("FolderList_32x32.png");

            //左侧基础菜单列表
            var menuItem = new List<SubMenuBase>() { };
            GetElectricalCalsMenuList(menuItem);
            ElectricalCalsMenu.MenuItems = menuItem;

            return ElectricalCalsMenu;
        }
        #region 左侧菜单列表
        /// <summary>
        /// 菜单列表
        /// </summary>
        /// <param name="menuItem"></param>
        private void GetElectricalCalsMenuList(List<SubMenuBase> menuItem)
        {
            var checkingMenu = new SubMenuBase("", this, "验算条件", (e) => { });
            //根据杆塔序列来确定子菜单
            #region 根据杆塔序列来确定子菜单
            var menu2ChildList = new List<SubMenuBase>() { };
            List<string> baseData = projectUtils.GetAllTowerSequenceNames();
            foreach (var item in baseData)
            {
                var subMenu = new SubMenuBase("ElectricalCheckingParModule", this, "    " + item, (e) => { OnSelectedBaseDataSubModuleChanged(e); });
                subMenu.ParentNode = checkingMenu;
                var subList = new List<SubMenuBase>() { };

                var subMenu1 = new SubMenuBase("", this, "        悬垂塔", (e) => { OnSelectedSubModuleItemChanged(e); });
                subMenu1.ParentNode = subMenu;
                subList.Add(subMenu1);

                var subMenu2 = new SubMenuBase("", this, "        耐张塔", (e) => { OnSelectedSubModuleItemChanged(e); });
                subMenu2.ParentNode = subMenu;
                subList.Add(subMenu2);

                subMenu.ChildItems = subList;

                menu2ChildList.Add(subMenu);
            }
            checkingMenu.ChildItems = menu2ChildList;

            menuItem.Add(checkingMenu);
            #endregion

            #region 根据本地保存的电气参数来配置子菜单
            GlobalInfo globalInfo = GlobalInfo.GetInstance();

            var calsParMenu = new SubMenuBase("", this, "计算参数", (e) => { });
            //calsParMenu.SetIcon("Menu_para.png");
            var struTemplateLibSubList = new List<SubMenuBase>() { };
            var test = new ECComParaModuleSubMenu("ElectricalCommonParModule", this, "    公共参数", (e) => { OnClickECCommParaMenu(e); });
            test.ParentNode = calsParMenu;

            List<ElecCalsCommRes> commParas = globalInfo.GetElecCalsCommParasList();
            var commParaSubList = new List<SubMenuBase>() { };
            for (int i = 0; i < commParas.Count; i++)
            {
                var commParaSubMenu = new SubMenuBase("", this, "    " + commParas[i].Name, (e) => { OnSelectedECParaChanged(e); });
                commParaSubMenu.ParentNode = test;
                commParaSubList.Add(commParaSubMenu);
            }
            test.ChildItems = commParaSubList;
            struTemplateLibSubList.Add(test);


            var test1 = new ECSideParaModuleSubMenu("ElectricalSideParModule", this, "    档内参数", (e) => { OnClickECSideParaMenu(e); });
            test1.ParentNode = calsParMenu;

            List<ElecCalsSideRes> sideParas = globalInfo.GetElecCalsSideParasList();
            var sideParaSubList = new List<SubMenuBase>() { };
            for (int i = 0; i < sideParas.Count; i++)
            {
                var sideParaSubMenu = new SubMenuBase("", this, "    " + sideParas[i].Name, (e) => { OnSelectedECParaChanged(e); });
                sideParaSubMenu.ParentNode = test1;
                sideParaSubList.Add(sideParaSubMenu);
            }
            test1.ChildItems = sideParaSubList;
            struTemplateLibSubList.Add(test1);

            var test2 = new ECTowerParaModuleSubMenu("ElectricalTowerParModule", this, "    铁塔配置参数", (e) => { OnClickECTowerParaMenu(e); });
            test2.ParentNode = calsParMenu;

            List<ElecCalsTowerRes> towerParas = globalInfo.GetElecCalsTowerParasList();
            var towerParaSubList = new List<SubMenuBase>() { };
            for (int i = 0; i < towerParas.Count; i++)
            {
                var towerParaSubMenu = new SubMenuBase("", this, "    " + towerParas[i].Name, (e) => { OnSelectedECParaChanged(e); });
                towerParaSubMenu.ParentNode = test2;
                towerParaSubList.Add(towerParaSubMenu);
            }
            test2.ChildItems = towerParaSubList;
            struTemplateLibSubList.Add(test2);

            calsParMenu.ChildItems = struTemplateLibSubList;
            menuItem.Add(calsParMenu);
            #endregion
        }

        private void OnClickECCommParaMenu(SubMenuBase vm)
        {
            if (GlobalInfo.GetInstance().GetElecCalsCommParasList().Count == 0)
            {
                System.Windows.MessageBox.Show("本地没有保存任何公共参数！");
                return;
            }
            OnSelectedMenuChanged(vm);
        }

        private void OnClickECSideParaMenu(SubMenuBase vm)
        {
            if (GlobalInfo.GetInstance().GetElecCalsSideParasList().Count == 0)
            {
                System.Windows.MessageBox.Show("本地没有保存任何档内参数！");
                return;
            }
            OnSelectedMenuChanged(vm);
        }

        private void OnClickECTowerParaMenu(SubMenuBase vm)
        {
            if (GlobalInfo.GetInstance().GetElecCalsTowerParasList().Count == 0)
            {
                System.Windows.MessageBox.Show("本地没有保存任何铁塔配置参数！");
                return;
            }
            OnSelectedMenuChanged(vm);
        }


        private void OnSelectedECParaChanged(SubMenuBase vm)
        {
            //先判断是否需要加载新子模块
            UpdateSubModule(vm.ParentNode);

            var subVm = GetCurSubModuleVM();

            if (subVm == null)
                return;

            subVm.UpDateView(vm.Title.Trim());
        }

        #endregion

        #region 电气计算公共参数名字的编辑

        ElecCalsCommParaNameEditWindow elecCalsCommParaNameEditWindow;

        public void ShowEidtECCommParaNameWindow()
        {
            elecCalsCommParaNameEditWindow = new ElecCalsCommParaNameEditWindow();
            ((ElecCalsCommParaNameEditViewModel)(elecCalsCommParaNameEditWindow.DataContext)).ElecCalsCommParaNameEditCloseEvent += EidtECCommParaNameWindowClosed;
            elecCalsCommParaNameEditWindow.ShowDialog();
        }

        public void EidtECCommParaNameWindowClosed(object sender, string newParaName)
        {
            ElecCalsCommParaNameEditViewModel model = (ElecCalsCommParaNameEditViewModel)sender;
            model.ElecCalsCommParaNameEditCloseEvent -= EidtECCommParaNameWindowClosed;
            if (elecCalsCommParaNameEditWindow != null) elecCalsCommParaNameEditWindow.Close();
            elecCalsCommParaNameEditWindow = null;

            if (newParaName == null || newParaName == "")
            {
                return;
            }

            var calsMenu = SelectedModuleInfo.MenuItems.Where(item => item.Title.Trim() == "计算参数").FirstOrDefault();
            if (calsMenu == null)
                return;

            var comPaMenu = calsMenu.ChildItems.Where(item => item.Title.Trim() == "公共参数").FirstOrDefault();
            if (comPaMenu == null)
                return;
            SubMenuBase commParaSubMenu = new SubMenuBase("", this, "    " + newParaName, (e) => { OnSelectedECParaChanged(e); });
            commParaSubMenu.ParentNode = comPaMenu;
            comPaMenu.ChildItems.Add(commParaSubMenu);

            OnSelectedECParaChanged(commParaSubMenu);
            MenuItems = new ObservableCollection<SubMenuBase>(SelectedModuleInfo.MenuItems);
        }
        #endregion



        #region  电气计算档内参数名字的编辑

        ElecCalsSideParaNameEditWindow elecCalsSideParaNameEditWindow;

        public void ShowEidtECSideParaNameWindow()
        {
            elecCalsSideParaNameEditWindow = new ElecCalsSideParaNameEditWindow();
            ((ElecCalsSideParaNameEditViewModel)(elecCalsSideParaNameEditWindow.DataContext)).ElecCalsSideParaNameEditCloseEvent += EidtECSideParaNameWindowClosed;
            elecCalsSideParaNameEditWindow.ShowDialog();
        }

        public void EidtECSideParaNameWindowClosed(object sender, string newParaName)
        {
            ElecCalsSideParaNameEditViewModel model = (ElecCalsSideParaNameEditViewModel)sender;
            model.ElecCalsSideParaNameEditCloseEvent -= EidtECSideParaNameWindowClosed;
            if (elecCalsSideParaNameEditWindow != null) elecCalsSideParaNameEditWindow.Close();
            elecCalsSideParaNameEditWindow = null;

            if (newParaName == null || newParaName == "")
            {
                return;
            }

            var calsMenu = SelectedModuleInfo.MenuItems.Where(item => item.Title.Trim() == "计算参数").FirstOrDefault();
            if (calsMenu == null)
                return;

            var sidePaMenu = calsMenu.ChildItems.Where(item => item.Title.Trim() == "档内参数").FirstOrDefault();
            if (sidePaMenu == null)
                return;
            SubMenuBase sideParaSubMenu = new SubMenuBase("", this, "    " + newParaName, (e) => { OnSelectedECParaChanged(e); });
            sideParaSubMenu.ParentNode = sidePaMenu;
            sidePaMenu.ChildItems.Add(sideParaSubMenu);

            OnSelectedECParaChanged(sideParaSubMenu);
            MenuItems = new ObservableCollection<SubMenuBase>(SelectedModuleInfo.MenuItems);
        }
        #endregion

        #region 电气计算铁塔配置参数名字的编辑

        ElecCalsTowerParaNameEditWindow elecCalsTowerParaNameEditWindow;

        public void ShowEidtECTowerParaNameWindow()
        {
            elecCalsTowerParaNameEditWindow = new ElecCalsTowerParaNameEditWindow();
            ((ElecCalsTowerParaNameEditViewModel)(elecCalsTowerParaNameEditWindow.DataContext)).ElecCalsTowerParaNameEditCloseEvent += EditECTowerParaNameWindowClosed;
            elecCalsTowerParaNameEditWindow.ShowDialog();
        }

        public void EditECTowerParaNameWindowClosed(object sender, string newParaName)
        {
            ElecCalsTowerParaNameEditViewModel model = (ElecCalsTowerParaNameEditViewModel)sender;
            model.ElecCalsTowerParaNameEditCloseEvent -= EditECTowerParaNameWindowClosed;
            if (elecCalsTowerParaNameEditWindow != null) elecCalsTowerParaNameEditWindow.Close();
            elecCalsTowerParaNameEditWindow = null;

            if (newParaName == null || newParaName == "")
            {
                return;
            }

            var calsMenu = SelectedModuleInfo.MenuItems.Where(item => item.Title.Trim() == "计算参数").FirstOrDefault();
            if (calsMenu == null)
                return;

            var towerPaMenu = calsMenu.ChildItems.Where(item => item.Title.Trim() == "铁塔配置参数").FirstOrDefault();
            if (towerPaMenu == null)
                return;
            SubMenuBase towerParaSubMenu = new SubMenuBase("", this, "    " + newParaName, (e) => { OnSelectedECParaChanged(e); });
            towerParaSubMenu.ParentNode = towerPaMenu;
            towerPaMenu.ChildItems.Add(towerParaSubMenu);

            OnSelectedECParaChanged(towerParaSubMenu);
            MenuItems = new ObservableCollection<SubMenuBase>(SelectedModuleInfo.MenuItems);
        }
        #endregion
    }
}
