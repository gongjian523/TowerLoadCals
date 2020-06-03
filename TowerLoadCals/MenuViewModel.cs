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

            TowerType type;

            if (((MenuItemVM)menu).Title == "直线塔")
                type = TowerType.LineTower;
            else if (((MenuItemVM)menu).Title == "直转塔")
                type = TowerType.LineCornerTower;
            else if (((MenuItemVM)menu).Title == "转角塔")
                type = TowerType.CornerTower;
            else if (((MenuItemVM)menu).Title == "分支塔")
                type = TowerType.BranchTower;
            else
                type = TowerType.TerminalTower;

            TowerTemplateReader templateReader = new TowerTemplateReader(type);
            TowerTemplate template = templateReader.Read(file);
            List<WorkConditionComboSpec> workConditions = ConvertTemplateToSpec(template);

            var globalInfo = GlobalInfo.GetInstance();
            int index = globalInfo.StruCalsParas.FindIndex(para => para.TowerName == ((MenuItemVM)menu).Title);

            List<StruLineParas> lineParas = new List<StruLineParas>();

            for (int i = 0; i < template.Wires.Count; i++)
            {
                lineParas.Add(new StruLineParas { Index = i + 1, WireType = template.Wires[i] });
            }

            if (index <  0)
            {
                globalInfo.StruCalsParas.Add(new StruCalsParas
                {
                    TowerName = ((MenuItemVM)menu).Title,
                    TablePath = openTableDialog.FileName,
                    Template = template,
                    WorkConditions = workConditions,
                    BaseParas = new FormulaParas() { SelectedStandard = "GB50545-2010" , Type = type, IsMethod1Selected = true},
                    LineParas = lineParas
                });
            }
            else
            {
                globalInfo.StruCalsParas[index].TablePath = openTableDialog.FileName;
                globalInfo.StruCalsParas[index].Template = template;
                globalInfo.StruCalsParas[index].WorkConditions = workConditions;
                globalInfo.StruCalsParas[index].BaseParas = new FormulaParas() { SelectedStandard = "GB50545-2010", Type = type, IsMethod1Selected = true };
                globalInfo.StruCalsParas[index].LineParas = lineParas;
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
                Filter = "Result Files (*.txt)|*.txt",
            };

            if (saveFileDialog.ShowDialog() != true)
                return;

            GlobalInfo globalInfo = GlobalInfo.GetInstance();

            StruCalsParas paras = globalInfo.StruCalsParas.Where(para => para.TowerName == ((MenuItemVM)menu).Title).First();

            if (paras == null)
                return;

            TowerType type;

            if (((MenuItemVM)menu).Title == "直线塔")
            {
                ConvertSpeToWorkCondition(paras.Template, paras.WorkConditions);
                LoadDistributeLineTower loadLineTower = new LoadDistributeLineTower(paras.BaseParas, paras.LineParas.ToArray(), paras.Template, paras.TablePath);
                loadLineTower.CalculateLoadDistribute(out float[,] xx, out float[,] yy, out float[,] zz, saveFileDialog.FileName);
            }
            else if (((MenuItemVM)menu).Title == "直转塔")
            {
                type = TowerType.LineCornerTower;
            }
            else if (((MenuItemVM)menu).Title == "转角塔")
            {
                type = TowerType.CornerTower;
            }
            else if (((MenuItemVM)menu).Title == "分支塔")
            {
                type = TowerType.BranchTower;
            }
            else
            {
                type = TowerType.TerminalTower;

            }

        }


        public void NewMenuItem(MenuItemVM menu)
        {

            ;

        }

    }
}
