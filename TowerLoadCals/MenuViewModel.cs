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
using TowerLoadCals.Common;
using TowerLoadCals.DAL;
using TowerLoadCals.Mode;
using static TowerLoadCals.DAL.TowerTemplateReader;
using TowerLoadCals.BLL;
using System.Linq;

namespace TowerLoadCals
{
    public class ModuleInfo
    {
        protected ISupportServices parent;
        protected MainWindowViewModel parentVm;

        public ModuleInfo(string _type, object parent, string _title)
        {
            Type = _type;
            this.parent = (ISupportServices)parent;
            parentVm = (MainWindowViewModel)parent;
            Title = _title;
        }
        public string Type { get; protected set; }
        public virtual bool IsSelected { get; set; }
        public string Title { get; protected set; }
        public ImageSource Icon { get; set; }

        public void SetIcon(string icon)
        {
            //一定要把image文件加入到工程中
            Icon = ImageSourceHelper.GetImageSource(AssemblyHelper.GetResourceUri(typeof(ModuleInfo).Assembly, string.Format("Images/{0}", icon)));
        }

        public virtual void Show(object parameter = null)
        {
            INavigationService navigationService = parent.ServiceContainer.GetRequiredService<INavigationService>();
            navigationService.Navigate(Type, parameter, parent);
        }

        public virtual IEnumerable<MenuItemVM> MenuItems { get; set; }

        public virtual MenuItemVM SelectedMenuItem { get; set; }

    }

    public class MenuItemVM : ModuleInfo
    {
        public ICommand Command { get; set; }

        public MenuItemVM ParentNode { get; set; }

        public IList<MenuItemVM> ChildItems { get; set; }

        public Visibility ContextVisible { get; set; }

        public Visibility NewBtnVisible { get; set; }

        public Visibility EditBtnVisible { get; set; }

        public Visibility DelBtnVisible { get; set; }

        public Visibility LoadBtnVisible { get; set; }

        public Visibility CalsBtnVisible { get; set; }

        public MenuItemVM(string _type,
                        object parent,
                        string _title,
                        Action<MenuItemVM> func,
                        Visibility contextVisible = Visibility.Collapsed,
                        Visibility bNewBtnVisible = Visibility.Collapsed,
                        Visibility bEditBtnVisible = Visibility.Collapsed,
                        Visibility bDelBtnVisible = Visibility.Collapsed,
                        IList<MenuItemVM> children = null)
                        : base(_type, parent, _title)
        {
            Type = _type;
            Title = _title;
            this.parent = (ISupportServices)parent;

            Command = new DelegateCommand<MenuItemVM>(func);

            ContextVisible = contextVisible;
            NewBtnVisible = bNewBtnVisible;
            EditBtnVisible = bEditBtnVisible;
            DelBtnVisible = bDelBtnVisible;

            LoadBtnVisible = Visibility.Collapsed;
            CalsBtnVisible = Visibility.Collapsed;

            ChildItems = children;

            DelItemCommand = new DelegateCommand<object>(DelMenuItem);
            LoadCommand = new DelegateCommand<object>(Load);
            CalsCommand = new DelegateCommand<object>(Cals);
        }

        public DelegateCommand<object> DelItemCommand { get; private set; }
        void DelMenuItem(object menu)
        {
            INavigationService navigationService = parent.ServiceContainer.GetRequiredService<INavigationService>();
            IBaseViewModel curViewMode = navigationService.Current as IBaseViewModel;
            if (curViewMode == null)
                return;

            ((MenuItemVM)menu).ParentNode.ChildItems.Remove((MenuItemVM)menu);
            parentVm.UpdateNavigationBar();

            curViewMode.DelSubItem(((MenuItemVM)menu).Title);
        }

        public DelegateCommand<object> LoadCommand { get; private set; }
        void Load(object menu)
        {
            var openTemplateDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "DLL Files (*.dll)|*.dll"
            };

            if (openTemplateDialog.ShowDialog() != true)
                return;

            var openTableDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "Excel Files (*.xlsx)|*.xlsx"
            };

            if (openTableDialog.ShowDialog() != true)
                return;

            string file = openTemplateDialog.FileName.Substring(openTemplateDialog.FileName.Length - 3) + "dat";

            DES.DesDecrypt(openTemplateDialog.FileName, file, "12345678");

            TowerType type = TowerTypeStringConvert.TowerStringToType(((MenuItemVM)menu).Title);

            TowerTemplateReader templateReader = new TowerTemplateReader(type);
            TowerTemplate template = templateReader.Read(file);

            var globalInfo = GlobalInfo.GetInstance();
            int index = globalInfo.StruCalsParas.FindIndex(para => para.TowerName == ((MenuItemVM)menu).Title);

            StruCalsParas paras = new StruCalsParas(((MenuItemVM)menu).Title, openTableDialog.FileName, template);

            if (index <  0)
            {
                globalInfo.StruCalsParas.Add(paras);
            }
            else
            {
                globalInfo.StruCalsParas[index] = paras;
            }

            if (((MenuItemVM)menu).MenuItems == null)
            {
                ((MenuItemVM)menu).MenuItems = new List<MenuItemVM>();
            }

            if (((MenuItemVM)menu).MenuItems.Count() == 0)
            {
                parentVm.NewTowerSubMenuItem(((MenuItemVM)menu));
            }

        }

        public DelegateCommand<object> CalsCommand { get; private set; }
        void Cals(object menu)
        {
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog()
            {
                Filter = "Result Files (*.calc)|*.calc",
            };

            if (saveFileDialog.ShowDialog() != true)
                return;

            GlobalInfo globalInfo = GlobalInfo.GetInstance();

            StruCalsParas paras = globalInfo.StruCalsParas.Where(para => para.TowerName == ((MenuItemVM)menu).Title).First();
            if (paras == null)
                return;

            ConvertSpeToWorkCondition(paras.Template, paras.WorkConditions);
            string path = saveFileDialog.FileName.Substring(0, saveFileDialog.FileName.Length - 5);


            for (int i = 0; i < paras.HPSettingsParas.Count(); i++)
            {
                LoadComposeBase loadCompose;

                if (((MenuItemVM)menu).Title == "直线塔")
                {
                    loadCompose = new LoadComposeLineTower(paras.BaseParas, paras.LineParas.ToArray(), paras.HPSettingsParas[i], paras.Template, paras.TablePath);
                }
                else if (((MenuItemVM)menu).Title == "直转塔")
                {
                    loadCompose = new LoadComposeLineCornerTower(paras.BaseParas, paras.LineParas.ToArray(), paras.HPSettingsParas[i], paras.Template, paras.TablePath);
                }
                //else if (((MenuItemVM)menu).Title == "转角塔")
                else
                {
                    loadCompose = new LoadComposeCornerTower(paras.BaseParas, paras.LineParas.ToArray(), paras.HPSettingsParas[i], paras.Template, paras.TablePath);
                }

                paras.ResultPointLoad.AddRange(loadCompose.LoadCaculate(path));
            }

            //if (((MenuItemVM)menu).Title == "直线塔")
            //{
            //    ConvertSpeToWorkCondition(paras.Template, paras.WorkConditions);
            //    LoadComposeLineTower loadLineTower = new LoadComposeLineTower(paras.BaseParas, paras.LineParas.ToArray(), paras.HPSettingsParas, paras.Template, paras.TablePath);
            //    loadLineTower.CalculateLoadDistribute(out float[,] xx, out float[,] yy, out float[,] zz, saveFileDialog.FileName);
            //}
            //else if (((MenuItemVM)menu).Title == "直转塔")
            //{
            //    ConvertSpeToWorkCondition(paras.Template, paras.WorkConditions);
            //    LoadComposeLineCornerTower loadLineCornerTower = new LoadComposeLineCornerTower(paras.BaseParas, paras.LineParas.ToArray(), paras.HPSettingsParas, paras.Template, paras.TablePath);
            //    loadLineCornerTower.CalculateLoadDistribute(out float[,] xx, out float[,] yy, out float[,] zz, saveFileDialog.FileName);
            //}
            ////else if (((MenuItemVM)menu).Title == "转角塔")
            //else
            //{
            //    ConvertSpeToWorkCondition(paras.Template, paras.WorkConditions);
            //    LoadComposeCornerTower loadCornerTower = new LoadComposeCornerTower(paras.BaseParas, paras.LineParas.ToArray(), paras.HPSettingsParas, paras.Template, paras.TablePath);
            //    loadCornerTower.CalculateLoadDistribute(out float[,] xx, out float[,] yy, out float[,] zz, saveFileDialog.FileName);
            //}



            
        }


        public void NewMenuItem(MenuItemVM menu)
        {

            ;

        }

    }
}
