using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
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

            var calsParMenu = new SubMenuBase("", this, "计算参数", (e) => { });
            //calsParMenu.SetIcon("Menu_para.png");
            var struTemplateLibSubList = new List<SubMenuBase>() { };
            var test = new SubMenuBase("ElectricalCommonParModule", this, "    公共参数", (e) => { OnSelectedMenuChanged(e); });
            test.ParentNode = calsParMenu;
            struTemplateLibSubList.Add(test);
            var test1 = new SubMenuBase("ElectricalSideParModule", this, "    单侧参数", (e) => { OnSelectedMenuChanged(e); });
            test1.ParentNode = calsParMenu;
            struTemplateLibSubList.Add(test1);


            calsParMenu.ChildItems = struTemplateLibSubList;
            menuItem.Add(calsParMenu);
        }
        #endregion
    }
}
