using System;
using System.Collections.Generic;
using System.Windows;
using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using DevExpress.Utils;
using TowerLoadCals.ModulesViewModels;
using System.Windows.Media;
using DevExpress.Xpf.Core.Native;
using System.Windows.Input;
using TowerLoadCals.DAL;
using TowerLoadCals.Mode;
using static TowerLoadCals.DAL.TowerTemplateReader;
using TowerLoadCals.BLL;
using System.Linq;

namespace TowerLoadCals
{
    public class ModuleMenu
    {
        protected ISupportServices parent;
        protected MainWindowViewModel parentVm;

        public ICommand Command { get; set; }

        public ModuleMenu(string _type, object parent, string _title, Action<ModuleMenu> func = null)
        {
            Type = _type;
            this.parent = (ISupportServices)parent;
            parentVm = (MainWindowViewModel)parent;
            Title = _title;

            if(func != null)
                Command = new DelegateCommand<ModuleMenu>(func);
        }
        public string Type { get; protected set; }
        public virtual bool IsSelected { get; set; }
        public string Title { get; protected set; }
        public ImageSource Icon { get; set; }

        public void SetIcon(string icon)
        {
            //一定要把image文件加入到工程中
            Icon = ImageSourceHelper.GetImageSource(AssemblyHelper.GetResourceUri(typeof(ModuleMenu).Assembly, string.Format("Images/{0}", icon)));
        }

        public virtual void Show(object parameter = null)
        {
            INavigationService navigationService = parent.ServiceContainer.GetRequiredService<INavigationService>();
            navigationService.Navigate(Type, parameter, parent);
        }

        public virtual List<SubMenuBase> MenuItems { get; set; }

        public virtual SubMenuBase SelectedMenuItem { get; set; }

    }

    public class SubMenuBase : ModuleMenu
    {
        public ICommand CommandClick { get; set; }

        public SubMenuBase ParentNode { get; set; }

        public IList<SubMenuBase> ChildItems { get; set; }

        public virtual Visibility ContextVisible { get; set; }

        public virtual string Command1Name { get; set; }
        public virtual Visibility Command1BtnVisible { get; set; }

        public virtual string Command2Name { get; set; }
        public virtual Visibility Command2BtnVisible { get; set; }

        public virtual string Command3Name { get; set; }
        public virtual Visibility Command3BtnVisible { get; set; }

        public virtual string Command4Name { get; set; }
        public virtual Visibility Command4BtnVisible { get; set; }


        public SubMenuBase(string _type,
                        object parent,
                        string _title,
                        Action<SubMenuBase> func,
                        IList<SubMenuBase> children = null)
                        : base(_type, parent, _title)
        {
            Type = _type;
            Title = _title;
            this.parent = (ISupportServices)parent;

            if(func != null)
                CommandClick = new DelegateCommand<SubMenuBase>(func);

            ContextVisible = Visibility.Collapsed;

            Command1BtnVisible = Visibility.Collapsed;
            Command2BtnVisible = Visibility.Collapsed;
            Command3BtnVisible = Visibility.Collapsed;
            Command4BtnVisible = Visibility.Collapsed;

            ChildItems = children;
        }

        public virtual void Command1(SubMenuBase menu)
        {
            ;
        }

        public virtual void Command2(SubMenuBase menu)
        {
            ;
        }

        public virtual void Command3(SubMenuBase menu)
        {
            ;
        }

        public virtual void Command4(SubMenuBase menu)
        {
            ;
        }
    }

    public class StrCalsModuleSubMenu : SubMenuBase
    {

        public StrCalsModuleSubMenu(string _type,
                        object parent,
                        string _title,
                        Action<StrCalsModuleSubMenu> func,
                        IList<SubMenuBase> children = null)
                        : base(_type, parent, _title, null)
        {
            Type = _type;
            Title = _title;
            this.parent = (ISupportServices)parent;

            CommandClick = new DelegateCommand<StrCalsModuleSubMenu>(func);

            SetIcon("Menu_tower.png");

            ContextVisible = Visibility.Visible;

            Command1Name = "计算";
            Command1BtnVisible = Visibility.Visible;

            Command2Name = "重新加载";
            Command2BtnVisible = Visibility.Visible;

            Command3Name = "删除";
            Command3BtnVisible = Visibility.Visible;

            ChildItems = children;
        }


        public override void Command1(SubMenuBase menu)
        {
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog()
            {
                Filter = "Result Files (*.calc)|*.calc",
            };

            if (saveFileDialog.ShowDialog() != true)
                return;

            GlobalInfo globalInfo = GlobalInfo.GetInstance();

            //StruCalsParas中塔位数据，是在点击这个塔位的页面后才加载的GlobalInfo中，
            //下面代码针对的是，没有打开这个塔位的页面而直接进行计算的情况
            if (globalInfo.StruCalsParas.Where(item => item.TowerName == ((SubMenuBase)menu).Title).Count() <= 0)
            {
                ProjectUtils.GetInstance().ReadStruCalsTowerParas(((SubMenuBase)menu).Title);
            }

            StruCalsParas paras = globalInfo.StruCalsParas.Where(para => para.TowerName == ((SubMenuBase)menu).Title).First();
            if (paras == null)
                return;

            ConvertSpeToWorkCondition(paras.Template, paras.WorkConditions);
            string path = saveFileDialog.FileName.Substring(0, saveFileDialog.FileName.Length - 5);

            for (int i = 0; i < paras.HPSettingsParas.Count(); i++)
            {
                LoadComposeBase loadCompose;

                if (paras.BaseParas.Type == TowerType.LineTower)
                {
                    loadCompose = new LoadComposeLineTower(paras.BaseParas, paras.LineParas.ToArray(), paras.HPSettingsParas[i], paras.Template, paras.ElectricalLoadFilePath);
                }
                else if (paras.BaseParas.Type == TowerType.LineCornerTower)
                {
                    loadCompose = new LoadComposeLineCornerTower(paras.BaseParas, paras.LineParas.ToArray(), paras.HPSettingsParas[i], paras.Template, paras.ElectricalLoadFilePath);
                }
                //剩下的都属于耐张塔
                else
                {
                    loadCompose = new LoadComposeCornerTower(paras.BaseParas, paras.LineParas.ToArray(), paras.HPSettingsParas[i], paras.Template, paras.ElectricalLoadFilePath);
                }

                paras.ResultPointLoad.AddRange(loadCompose.LoadCaculate(path));
            }
        }

        public override void Command2(SubMenuBase menu)
        {
            ;
        }

        public override void Command3(SubMenuBase menu)
        {
            INavigationService navigationService = parent.ServiceContainer.GetRequiredService<INavigationService>();
            IBaseViewModel curViewMode = navigationService.Current as IBaseViewModel;
            if (curViewMode == null)
                return;

            menu.ParentNode.ChildItems.Remove(menu);
            parentVm.UpdateNavigationBar();

            curViewMode.DelSubItem(menu.Title);
        }
    }



}
