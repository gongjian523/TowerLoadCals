﻿using System;
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
using System.Collections.ObjectModel;
using System.IO;
using System.Diagnostics;
using TowerLoadCals.DAL.Structure;
using TowerLoadCals.BLL.Structure;

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
        public virtual List<SubMenuBase> InternetMenuItems { get; set; }

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

    /// <summary>
    /// 结构计算模块的按钮
    /// </summary>
    public class StrCalsModuleSubMenu : SubMenuBase
    {
        public string Sequence { get; set; }

        public StrCalsModuleSubMenu(string _type,
                        object parent,
                        string _title,
                        string _seqence,
                        Action<StrCalsModuleSubMenu> func,
                        IList<SubMenuBase> children = null)
                        : base(_type, parent, _title, null)
            {
            Type = _type;
            Title = _title;
            Sequence = _seqence;
            this.parent = (ISupportServices)parent;

            CommandClick = new DelegateCommand<StrCalsModuleSubMenu>(func);

            SetIcon("Menu_tower.png");

            ContextVisible = Visibility.Visible;

            //Command1Name = "设置";
            //Command1BtnVisible = Visibility.Visible;

            Command2Name = "结构计算";
            Command2BtnVisible = Visibility.Visible;

            Command3Name = "满应力分析";
            Command3BtnVisible = Visibility.Visible;

            //Command4Name = "删除";
            //Command4BtnVisible = Visibility.Visible;

            ChildItems = children;
        }

        public override void Command1(SubMenuBase menu)
        {
            parentVm.ShowEidtStruCalsTowerWindow(menu.Title.Trim());
        }


        public override void Command2(SubMenuBase menu)
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
            if (globalInfo.StruCalsParas.Where(item => item.TowerName == ((SubMenuBase)menu).Title && item.SequenceName == ((StrCalsModuleSubMenu)menu).Sequence).Count() <= 0)
            {
                if (((StrCalsModuleSubMenu)menu).Sequence == "")
                {
                    ProjectUtils.GetInstance().ReadStruCalsTowerParas(((SubMenuBase)menu).Title);
                }
                else
                {
                    ProjectUtils.GetInstance().ReadStruCalsTowerParas(((SubMenuBase)menu).Title, ((StrCalsModuleSubMenu)menu).Sequence);
                }
                    
            }

            StruCalsParasCompose paras = globalInfo.StruCalsParas.Where(para => para.TowerName == ((SubMenuBase)menu).Title && para.SequenceName == ((StrCalsModuleSubMenu)menu).Sequence).FirstOrDefault();
            if (paras == null)
                return;

            ConvertSpecToWorkCondition(paras.Template, paras.WorkConditions,true);
            string path = saveFileDialog.FileName.Substring(0, saveFileDialog.FileName.Length - 5);

            for (int i = 0; i < paras.HPSettingsParas.Count(); i++)
            {
                LoadComposeBase loadCompose;

                if (paras.BaseParas.Type == TowerTypeEnum.LineTower)
                {
                    loadCompose = new LoadComposeLineTower(paras.BaseParas, paras.LineParas.ToArray(), paras.HPSettingsParas[i], paras.Template, paras.ElectricalLoadFilePath);
                }
                else if (paras.BaseParas.Type == TowerTypeEnum.LineCornerTower)
                {
                    loadCompose = new LoadComposeLineCornerTower(paras.BaseParas, paras.LineParas.ToArray(), paras.HPSettingsParas[i], paras.Template, paras.ElectricalLoadFilePath);
                }
                //剩下的都属于耐张塔
                else
                {
                    loadCompose = new LoadComposeTensionTower(paras.BaseParas, paras.LineParas.ToArray(), paras.HPSettingsParas[i], paras.Template, paras.ElectricalLoadFilePath);
                }

                paras.ResultPointLoad.AddRange(loadCompose.LoadCaculate(path));
            }
        }

        public override void Command3(SubMenuBase menu)
        {
            var loadFileDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "Load Files (*.load)|*.load",
            };

            if (loadFileDialog.ShowDialog() != true)
                return;

            string stQtPath = GlobalInfo.GetInstance().GetSmartTowerPath();
            if (stQtPath == null || stQtPath == "")
            {
                MessageBox.Show(" 请设置SmartTower程序的路径！");
                return;
            }

            if (!File.Exists(stQtPath))
            {
                MessageBox.Show("无法找到SmartTower，请设置它的路径！");
                return;
            }

            string stConsolePath = stQtPath.Substring(0, stQtPath.LastIndexOf("\\")) + "\\" + ConstVar.SmartTowerConsoleName;
            if (!File.Exists(stConsolePath))
            {
                return;
            }

            string mode = GlobalInfo.GetInstance().GetSmartTowerMode().ToString();

            GlobalInfo globalInfo = GlobalInfo.GetInstance();

            if (globalInfo.StruCalsParas.Where(item => item.TowerName == ((SubMenuBase)menu).Title && item.SequenceName == ((StrCalsModuleSubMenu)menu).Sequence).Count() <= 0)
            {
                if (((StrCalsModuleSubMenu)menu).Sequence == null)
                    ProjectUtils.GetInstance().ReadStruCalsTowerParas(((SubMenuBase)menu).Title);
                else
                    ProjectUtils.GetInstance().ReadStruCalsTowerParas(((SubMenuBase)menu).Title, ((StrCalsModuleSubMenu)menu).Sequence);
            }

            StruCalsParasCompose paras = globalInfo.StruCalsParas.Where(para => para.TowerName == ((SubMenuBase)menu).Title && para.SequenceName == ((StrCalsModuleSubMenu)menu).Sequence).FirstOrDefault();
            if (paras == null)
                return;

            string path = paras.FullStressTemplatePaths[0];
            if (!File.Exists(path))
                return;

            SmartTowerInputGenerator.InputGenerator(loadFileDialog.FileName, path);

            string stParas = path + " " + mode;
            //string stParas1 = "C:\\Users\\zhifei\\Desktop\\测试\\StruCals\\直线塔7\\满应力分析\\Z31.dat 0";      //0: 正常计算 1:基础作用力BetaZ=1 2：基础作用力betaZ=-1/2+1 不容许有空格  
            ProcessStartInfo startInfo = new ProcessStartInfo(stConsolePath, stParas);
            //startInfo.UseShellExecute = false;
            //startInfo.CreateNoWindow = true;
            Process process = new Process();
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
            //}

            TowerMemberBLL memberBLL = new TowerMemberBLL();
            string outPath = path.Substring(0, path.LastIndexOf(".")) + ".out";
            paras.ResultFullStess = memberBLL.TextFileReadAll(outPath).ToList();

        }

        /// <summary>
        /// 删除命令
        /// </summary>
        /// <param name="menu"></param>
        public override void Command4(SubMenuBase menu)
        {
            var  struCalsList =  GlobalInfo.GetInstance().StruCalsParas;

            if (struCalsList == null)
                return;

            int index = struCalsList.FindIndex(item => item.TowerName == menu.Title.Trim());
            if (index == -1)
                return;

            struCalsList.RemoveAt(index);

            if (parentVm == null)
                return;

            parentVm.SelectedModuleInfo.MenuItems.Add(menu);

            parentVm.MenuItems = new ObservableCollection<SubMenuBase>(parentVm.SelectedModuleInfo.MenuItems);
        }
    }



    /// <summary>
    /// 结构计算模块的按钮
    /// </summary>
    public class ECComParaModuleSubMenu : SubMenuBase
    {

        public ECComParaModuleSubMenu(string _type,
                        object parent,
                        string _title,
                        Action<ECComParaModuleSubMenu> func,
                        IList<SubMenuBase> children = null)
                        : base(_type, parent, _title, null)
        {
            Type = _type;
            Title = _title;
            this.parent = (ISupportServices)parent;

            CommandClick = new DelegateCommand<ECComParaModuleSubMenu>(func);

            ContextVisible = Visibility.Visible;

            Command1Name = "新增";
            Command1BtnVisible = Visibility.Visible;

            ChildItems = children;
        }

        public override void Command1(SubMenuBase menu)
        {
            parentVm.ShowEidtECCommParaNameWindow();
        }
    }


    public class ECSideParaModuleSubMenu : SubMenuBase
    {
        public ECSideParaModuleSubMenu(string _type,
                        object parent,
                        string _title,
                        Action<ECSideParaModuleSubMenu> func,
                        IList<SubMenuBase> children = null)
                        : base(_type, parent, _title, null)
        {
            Type = _type;
            Title = _title;
            this.parent = (ISupportServices)parent;

            CommandClick = new DelegateCommand<ECSideParaModuleSubMenu>(func);

            ContextVisible = Visibility.Visible;

            Command1Name = "新增";
            Command1BtnVisible = Visibility.Visible;

            ChildItems = children;
        }

        public override void Command1(SubMenuBase menu)
        {
            parentVm.ShowEidtECSideParaNameWindow();
        }
    }

    public class ECTowerParaModuleSubMenu : SubMenuBase
    {
        public ECTowerParaModuleSubMenu(string _type,
                        object parent,
                        string _title,
                        Action<ECTowerParaModuleSubMenu> func,
                        IList<SubMenuBase> children = null)
                        : base(_type, parent, _title, null)
        {
            Type = _type;
            Title = _title;
            this.parent = (ISupportServices)parent;

            CommandClick = new DelegateCommand<ECTowerParaModuleSubMenu>(func);

            ContextVisible = Visibility.Visible;

            Command1Name = "新增";
            Command1BtnVisible = Visibility.Visible;

            ChildItems = children;
        }

        public override void Command1(SubMenuBase menu)
        {
            parentVm.ShowEidtECTowerParaNameWindow();
        }
    }


    //电气计算模块的按钮 暂空
    public class ElecCalsModuleSubMenu
    {

    }


}
