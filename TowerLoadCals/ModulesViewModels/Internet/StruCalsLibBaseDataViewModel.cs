using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using TowerLoadCals.BLL;
using TowerLoadCals.Mode;
using TowerLoadCals.Mode.Internet;
using TowerLoadCals.Service.Internet;


/// <summary>
/// created by :glj
/// </summary>

namespace TowerLoadCals.ModulesViewModels.Internet
{
    /// <summary>
    /// 导线
    /// </summary>
    public class StruCalsLibBaseDataViewModel : ViewModelBase
    {
        StruCalsLibBaseDataService baseService = new StruCalsLibBaseDataService();//数据交互层
        StruCalsLibBaseData_DetailService detailService = new StruCalsLibBaseData_DetailService();//数据交互层

        /// <summary>
        /// 导出事件
        /// </summary>
        public DelegateCommand ExportCommand { get; private set; }

        GlobalInfo globalInfo;//获取文件保存地址
        public StruCalsLibBaseDataViewModel()
        {
            OverhangingTower = new Tower();
            TensionTower = new Tower();

            ExportCommand = new DelegateCommand(doExportData);
            globalInfo = GlobalInfo.GetInstance();


            IList<StruCalsLibBaseData> baseList = baseService.GetList();//基本信息列表
            IList<StruCalsLibBaseData_Detail> detailList = detailService.GetList();//明细信息列表

            //页面返回结果
            StruCalsLibBaseData Overhangingbase = baseList.Where(item => item.BaseCategory == "悬垂塔基础参数").SingleOrDefault();
            StruCalsLibBaseData Tensionbase = baseList.Where(item => item.BaseCategory == "耐张塔基础参数").SingleOrDefault();

            //悬垂塔
            OverhangingTower.BaseData = Overhangingbase;
            OverhangingTower.GB50545Data = detailList.Where(item => item.ParentId == Overhangingbase.Id && item.Category == "GB50545-2010").SingleOrDefault();
            OverhangingTower.DLT5551Data = detailList.Where(item => item.ParentId == Overhangingbase.Id && item.Category == "DLT5551-2018").SingleOrDefault();
            //耐张塔
            TensionTower.BaseData = Tensionbase;
            TensionTower.GB50545Data = detailList.Where(item => item.ParentId == Tensionbase.Id && item.Category == "GB50545-2010").SingleOrDefault();
            TensionTower.DLT5551Data = detailList.Where(item => item.ParentId == Tensionbase.Id && item.Category == "DLT5551-2018").SingleOrDefault();
        }

        public void doExportData()
        {
            try
            {

                //文件地址
                string path = Directory.GetCurrentDirectory() + "\\" + ConstVar.UserDataStr + "\\" + ConstVar.StruCalsLibFileName;
                if (File.Exists(path))
                {
                    DialogResult dr = MessageBox.Show(string.Format("请确认是否替换现有基本参数裤数据？"), "替换确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (dr == DialogResult.OK)
                    {//加载xml文件
                        XmlDocument doc = new XmlDocument();
                        doc.Load(path);

                        //2：得到导线节点
                        //3：判断是否存在相同型号 不重复直接新增
                        //4：保存新文件

                        XmlNode overhanging_Node = doc.GetElementsByTagName("悬垂塔基础参数")[0];
                        ModifyRootNode(overhanging_Node, "overhanging");
                        XmlNode tension_Node = doc.GetElementsByTagName("耐张塔基础参数")[0];
                        ModifyRootNode(tension_Node, "tension");
                        doc.Save(path);

                        MessageBox.Show("下载成功!");
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("下载失败，具体原因如下:{0}!", ex.Message));
            }

        }

        #region 下载修改节点数据

        /// <summary>
        /// 下载修改节点数据
        /// </summary>
        /// <param name="rootNode"></param>
        /// <param name="baseCategory"></param>
        private void ModifyRootNode(XmlNode rootNode, string baseCategory)
        {
            StruCalsLibBaseData root = new StruCalsLibBaseData();
            if (baseCategory == "overhanging")
            {
                root = OverhangingTower.BaseData;
            }
            else if (baseCategory == "tension")
            {

                root = TensionTower.BaseData;
            }
            rootNode.Attributes.GetNamedItem("大风线条风压调整系数").InnerText = root.WindAdjustFactor.ToString();
            rootNode.Attributes.GetNamedItem("其他情况线条风压调整系数").InnerText = root.OtherWindAdjustFactor.ToString();
            rootNode.Attributes.GetNamedItem("安装动力系数").InnerText = root.DynamicCoef.ToString();
            rootNode.Attributes.GetNamedItem("过牵引系数").InnerText = root.DrawingCoef.ToString();
            rootNode.Attributes.GetNamedItem("锚线风荷系数").InnerText = root.AnchorWindCoef.ToString();
            rootNode.Attributes.GetNamedItem("锚线垂荷系数").InnerText = root.AnchorGravityCoef.ToString();
            rootNode.Attributes.GetNamedItem("锚角").InnerText = root.AnchorAngle.ToString();
            rootNode.Attributes.GetNamedItem("跳线吊装系数").InnerText = root.LiftCoefJumper.ToString();
            rootNode.Attributes.GetNamedItem("临时拉线对地夹角").InnerText = root.TempStayWireAngle.ToString();
            rootNode.Attributes.GetNamedItem("牵引角度").InnerText = root.TractionAgnle.ToString();

            ModifyRowNode(rootNode.ChildNodes[0], baseCategory, "GB50545-2010");
            ModifyRowNode(rootNode.ChildNodes[1], baseCategory, "DLT5551-2018");
        }


        private void ModifyRowNode(XmlNode row, string baseCategory, string category)
        {
            StruCalsLibBaseData_Detail detail = new StruCalsLibBaseData_Detail();
            if (baseCategory == "overhanging")
            {
                if (category == "GB50545-2010")
                    detail = OverhangingTower.GB50545Data;
                else if (category == "DLT5551-2018")
                    detail = OverhangingTower.DLT5551Data;
            }
            else if (baseCategory == "tension")
            {
                if (category == "GB50545-2010")
                    detail = TensionTower.GB50545Data;
                else if (category == "DLT5551-2018")
                    detail = TensionTower.DLT5551Data;
            }

            row.Attributes.GetNamedItem("恒荷载分项系数-不利").InnerText = detail.RGBad.ToString();
            row.Attributes.GetNamedItem("恒荷载分项系数-有利").InnerText = detail.RGGood.ToString();
            row.Attributes.GetNamedItem("活荷载分项系数").InnerText = detail.RQ.ToString();
            row.Attributes.GetNamedItem("可变荷载组合系数-安装").InnerText = detail.VcFInstall.ToString();
            row.Attributes.GetNamedItem("可变荷载组合系数-断线").InnerText = detail.VcFBroken.ToString();
            row.Attributes.GetNamedItem("可变荷载组合系数-不均匀冰").InnerText = detail.VcFUnevenIce.ToString();

            if (category == "GB50545-2010")
            {

                row.Attributes.GetNamedItem("可变荷载组合系数-运行").InnerText = detail.VcFNormal.ToString();
                row.Attributes.GetNamedItem("可变荷载组合系数-验算").InnerText = detail.VcFCheck.ToString();
            }
            else if (category == "DLT5551-2018")
            {

                row.Attributes.GetNamedItem("恒荷载分项系数-抗倾覆").InnerText = detail.RGOverturn.ToString();
                row.Attributes.GetNamedItem("可变荷载组合系数-大风").InnerText = detail.VcGNormal.ToString();
                row.Attributes.GetNamedItem("可变荷载组合系数-覆冰").InnerText = detail.VcFIce.ToString();
                row.Attributes.GetNamedItem("可变荷载组合系数-低温").InnerText = detail.VcFCold.ToString();
            }

        }
        #endregion


        /// <summary>
        /// 数据源
        /// </summary>
        public Tower OverhangingTower { get; set; }

        public Tower TensionTower { get; set; }

        /// <summary>
        /// 发送消息
        /// </summary>
        private bool isEnabledExport;
        public bool IsEnabledExport
        {
            get { return isEnabledExport = File.Exists(Directory.GetCurrentDirectory() + "\\" + ConstVar.UserDataStr + "\\" + ConstVar.StruCalsLibFileName); }
            set { isEnabledExport = value; RaisePropertyChanged(() => IsEnabledExport); }
        }

    }

    /// <summary>
    /// 塔
    /// </summary>
    public class Tower
    {
        public StruCalsLibBaseData BaseData { get; set; }
        public StruCalsLibBaseData_Detail GB50545Data { get; set; }
        public StruCalsLibBaseData_Detail DLT5551Data { get; set; }

    }
}
