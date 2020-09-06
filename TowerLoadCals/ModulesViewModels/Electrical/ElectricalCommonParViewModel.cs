using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using DevExpress.Office.History;
using DevExpress.Xpf.Core;
using DevExpress.XtraRichEdit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using TowerLoadCals.BLL;
using TowerLoadCals.BLL.Electric;
using TowerLoadCals.DAL;
using TowerLoadCals.DAL.Common;
using TowerLoadCals.Mode;
using TowerLoadCals.Mode.Electric;

namespace TowerLoadCals.ModulesViewModels.Electrical
{
    /// <summary>
    /// 公共参数
    /// </summary>
    public class ElectricalCommonParViewModel : ElecCalsCommResViewModel
    {
        /// <summary>
        /// 保存
        /// </summary>
        public DelegateCommand doSaveCommand { get; private set; }
        protected int num = 0;

        public static ElectricalCommonParViewModel Create()
        {
            return ViewModelSource.Create(() => new ElectricalCommonParViewModel());
        }
        protected ElectricalCommonParViewModel()
        {
        }
        //protected ElectricalCommonParViewModel()
        //{
        //    //doSaveCommand = new DelegateCommand(doSave);
        //    ////导/地 线
        //    //filePath = globalInfo.ProjectPath + "\\BaseData\\Wire.xml";
        //    //List<WireType> wireList = WireReader.Read(filePath);
        //    //var Wires = wireList.Where(item => item.Type == "导线").First().Wires;
        //    //var ground = wireList.Where(item => item.Type == "地线").First().Wires;


        //    ////串类型
        //    //filePath = globalInfo.ProjectPath + "\\BaseData\\StrData.xml";
        //    //List<StrDataCollection> strList=  StrDataReader.Read(filePath);
        //    //var StrData = strList.Where(item => item.Type == "一般子串").First().StrDatas;
        //    //var StrData1 = strList.Where(item => item.Type == "硬跳线").First().StrDatas;

        //    //var StrAllData = StrData.Concat(StrData1).ToList();//所有串类型
        //}

        /// <summary>
        /// 保存方法
        /// </summary>
        public void doSave()
            {
                try
                {

                    MessageBox.Show("保存成功!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("保存异常！异常信息:" + ex.Message);
                }
            }

        protected override void OnParameterChanged(object parameter)
        {
            InitializeData((string)parameter);
        }

        protected override void InitializeData(string towerName)
        {
            base.InitializeData(towerName);

            ElectricalCommonPars = new ObservableCollection<ElectricalCommonPar>();
            for (num = 1; num < 2; num++)
            {
                ElectricalCommonPars.Add(ElectricalCommonPar.Create("公共参数" + num.ToString(), elecCalsCommRes));
            }

        }

        public virtual ObservableCollection<ElectricalCommonPar> ElectricalCommonPars { get; protected set; }
        public void AddNewTab(TabControlTabAddingEventArgs e)
        {
            e.Item = ElectricalCommonPar.Create("公共参数" + num.ToString(), elecCalsCommRes);
            num++;
        }

    }

    public class ElectricalCommonPar : ViewModelBase, INotifyPropertyChanged
    {
        public static ElectricalCommonPar Create(string title, ElecCalsCommRes calsParas)
        {
            return ViewModelSource.Create(() => new ElectricalCommonPar(title, calsParas));
        }
        protected ElectricalCommonPar(string title, ElecCalsCommRes calsParas)
        {
            Title = title;
        }
        public string Title { get; private set; }
    }
}