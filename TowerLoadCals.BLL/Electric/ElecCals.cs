using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerLoadCals.Common;
using TowerLoadCals.Mode;

namespace TowerLoadCals.BLL.Electric
{
    public class ElecCals
    {
        protected string SequenceName;
        protected List<TowerSerial> TowerSerials = new List<TowerSerial>();
        protected List<ElecCalsParas> ParaList = new List<ElecCalsParas>();

        protected string ExcelPath;

        public ElecCals(string sequence, List<TowerSerial> towerSerials)
        {
            SequenceName = sequence;
            TowerSerials = towerSerials;
            UpdateParas();
        }

        protected void UpdateParas()
        {
            ParaList.Add(new ElecCalsParas(SequenceName, new TowerSerial(), TowerSerials[0]));

            for (int i = 1; i < TowerSerials.Count; i++)
            {
                ParaList.Add(new ElecCalsParas(SequenceName, TowerSerials[i-1], TowerSerials[i]));

                CopyTmeplateToDesDir(i);
            }
        }

        public void TowerSerialsCaculate()
        {
            for(int  i = 1; i < TowerSerials.Count-1; i++)
            {
                if (!TowerSerials[i].IsChecking)
                    continue;

                if(ParaList[i].TowerCals.TowerType == "悬垂塔")
                {
                    HuangTowerCaculate(i);
                }
                else
                {
                    StrainTowerCaculate(i);
                }

                ProvideMeterial(i);

                UpdateStruCalsParas(i);
            }
        }

        /// <summary>
        /// 将电气目录和结构目录中这个序列和每个塔的子目录建立好
        /// 将电气计算的Eexel模板和结构计算的模板复制到相应的目录下，便于后续计算
        /// </summary>
        /// <param name="seqence"></param>
        /// <param name="tower"></param>
        /// <returns></returns>
        protected void CopyTmeplateToDesDir(int index)
        {
            TowerSerial tower = TowerSerials[index];

            //建立电气计算下的目录
            string elecDirPath = ProjectUtils.GetInstance().ProjectPath + "\\" + ConstVar.ElecCalsStr + "\\" + SequenceName;
            if (!Directory.Exists(elecDirPath))
                Directory.CreateDirectory(elecDirPath);

            elecDirPath += "\\" + tower.TowerName;
            if (!Directory.Exists(elecDirPath))
                Directory.CreateDirectory(elecDirPath);

            //安装目录UserData里面复制电气模板
            string scrPath = Directory.GetCurrentDirectory() + "\\" + ConstVar.UserDataStr + "\\" + (tower.TowerType == 1 ? 
                ConstVar.ElecLoadLineTowerTemplateFileName : ConstVar.ElecLoadCornerTowerTemplateFileName);
            string destPath = elecDirPath + "\\" + ConstVar.ElecLoadTemplateFileName;

            File.Copy(scrPath, destPath);
        }

        protected void HuangTowerCaculate(int index)
        {
            ElecCalsParas paras = ParaList[index];

            ElecCalsRes SideCalRes = new ElecCalsRes();
            SideCalRes.SpanFit = paras.BackSpanFit;
            SideCalRes.IsBackSide = true; //只是为了计算45风
            SideCalRes.UpdataSor(paras.BackWeatherPara, paras.BackWirePara.Ind, paras.BackWirePara.Grd, paras.BackWirePara.OPGW, paras.JumpWirePara, paras.BackSidePara, paras.CommParas);

            TowerElecCals BackTower = ParaList[index - 1].TowerCals;
            BackTower.UpdataTowerTraHei();
            BackTower.UpdateAtitudeTower();

            TowerHangElecCals CalTower = (TowerHangElecCals)ParaList[index].TowerCals;
            CalTower.UpdataTowerTraHei();
            CalTower.UpdateAtitudeTower();

            TowerElecCals FrontTower = ParaList[index + 1].TowerCals;
            FrontTower.UpdataTowerTraHei();
            FrontTower.UpdateAtitudeTower();

            paras.CommParas.UpateIceCovrage(SideCalRes.Weather.WeathComm, SideCalRes.SideParas.IceArea);

            CalTower.GetAndUpdateSideRes(SideCalRes);

            CalTower.GetAndUpdateStrData(paras.StrDatas);

            for (int i = 0; i <= 9; i++)
            {
                CalTower.PhaseTraList[i].CalsStrLoad();
            }

            CalTower.FlashBackHeiSub(BackTower);
            CalTower.FlashFrontHeiSub(FrontTower);
            CalTower.UpdateDFCure();

            CalTower.Cals();

            CalTower.UpdateTensionDiff();
        }

        protected void StrainTowerCaculate(int index)
        {
            ElecCalsParas paras = ParaList[index];

            ElecCalsRes BackSideCalRes = new ElecCalsRes();
            ElecCalsRes FrontSideCalRes = new ElecCalsRes();

            BackSideCalRes.IsBackSide = true;
            BackSideCalRes.SpanFit = paras.BackSpanFit;

            FrontSideCalRes.IsBackSide = false;
            FrontSideCalRes.SpanFit = paras.FrontSpanFit;

            BackSideCalRes.UpdataSor(paras.BackWeatherPara, paras.BackWirePara.Ind, paras.BackWirePara.Grd, paras.BackWirePara.OPGW, paras.JumpWirePara, paras.BackSidePara, paras.CommParas);
            FrontSideCalRes.UpdataSor(paras.FrontWeatherPara, paras.FrontWirePara.Ind, paras.FrontWirePara.Grd, paras.FrontWirePara.OPGW, paras.JumpWirePara, paras.FrontSidePara, paras.CommParas);

            paras.CommParas.UpateIceCovrage("耐张塔", BackSideCalRes.Weather.WeathComm, BackSideCalRes.SideParas.IceArea, FrontSideCalRes.Weather.WeathComm, FrontSideCalRes.SideParas.IceArea);

            TowerElecCals BackTower = ParaList[index - 1].TowerCals;
            BackTower.UpdataTowerTraHei();
            BackTower.UpdateAtitudeTower();

            TowerStrainElecCals CalTower = (TowerStrainElecCals)ParaList[index].TowerCals;
            CalTower.UpdataTowerTraHei();
            CalTower.UpdateAtitudeTower();

            TowerElecCals FrontTower = ParaList[index + 1].TowerCals; 
            FrontTower.UpdataTowerTraHei();
            FrontTower.UpdateAtitudeTower();

            CalTower.GetAndUpdateSideRes(BackSideCalRes, FrontSideCalRes);

            CalTower.GetAndUpdateStrData(paras.StrDatas);

            for (int i = 0; i <= 9; i++)
            {
                CalTower.PhaseTraList[i].CalsStrLoad();
            }

            for (int i = 0; i <= 2; i++)
            {
                CalTower.PhaseTraList[i].CalsWindLoad(paras.CommParas.VoltStr, CalTower.PhaseTraList[i + 5].WireData.WeatherParas.WeathComm);
                CalTower.PhaseTraList[i + 5].CalsWindLoad(paras.CommParas.VoltStr, CalTower.PhaseTraList[i].WireData.WeatherParas.WeathComm);
            }

            CalTower.FlashBackHeiSub(BackTower);
            CalTower.FlashFrontHeiSub(FrontTower);
            CalTower.UpdateWindPara();

            CalTower.Cals();

            CalTower.CalDFCure();
            //CalTower.CalDFCurePY();

            CalTower.UpdateTensionDiff();
            CalTower.UpateAnchor();
            CalTower.CalsTensionChcek();

        }

        /// <summary>
        /// 使用excel模板进行提资
        /// </summary>
        /// <param name="index"></param>
        protected void ProvideMeterial(int index)
        {
            ExcelPath = ProjectUtils.GetInstance().ProjectPath + "\\" + ConstVar.ElecCalsStr + "\\" + SequenceName  + "\\" + TowerSerials[index].TowerName + "\\" + ConstVar.ElecLoadTemplateFileName;

            ElecLoadProcessExcel precessExcel = new ElecLoadProcessExcel(ExcelPath);
            precessExcel.CalsElecLoad(ParaList[index].TowerCals);
        }

        protected void UpdateStruCalsParas(int index)
        {
            TowerSerial tower = TowerSerials[index];

            string strucDirPath = ProjectUtils.GetInstance().ProjectPath + "\\" + ConstVar.StruCalsStr + "\\" + SequenceName;
            if (!Directory.Exists(strucDirPath))
                Directory.CreateDirectory(strucDirPath);

            strucDirPath += "\\" + tower.TowerName;
            if (!Directory.Exists(strucDirPath))
                Directory.CreateDirectory(strucDirPath);

            TowerStrData towerStr = GlobalInfo.GetInstance().GetLocalTowerStrByName(tower.TowerName);

            //复制结构计算的模板，结构计算的模板要从通用模板库里面复制
            string struTempPathSce = ProjectUtils.GetInstance().ProjectPath + "\\" + ConstVar.DataBaseStr + "\\" + ConstVar.GeneralStruTemplateStr + "\\" + TowerTypeToString(tower.TowerType) + "\\" + towerStr.TempletName.Remove(0,6) + ".dat";
            string struTempPathDes = ProjectUtils.GetInstance().ProjectPath + "\\" + ConstVar.StruCalsStr + "\\" + SequenceName + "\\" + towerStr.Name + "\\" + ConstVar.StruCalsStr + "\\" + towerStr.TempletName.Remove(0, 6) + ".dat";
            File.Copy(struTempPathSce, struTempPathDes);

            //复制应力计算模板文件，需要从TowerUploadFile下相应的塔文件里复制，一共需要复制三个文件：模板文件、data.ini、DataCh.ini
            string stressTempDirSce = ProjectUtils.GetInstance().ProjectPath + "\\" + ConstVar.DataBaseStr + "\\" + ConstVar.TowerUploadFileStr + "\\" + towerStr.Name + "[" + tower.TowerType.ToString() + "]\\";
            string stressTempDirDes = ProjectUtils.GetInstance().ProjectPath + "\\" + ConstVar.StruCalsStr + "\\" + SequenceName + "\\" + towerStr.Name + "\\" + ConstVar.FullStressStr + "\\";
            File.Copy(stressTempDirSce + towerStr.ModelFileExtension, stressTempDirDes + towerStr.ModelName);
            File.Copy(stressTempDirSce + ConstVar.SmartTowerIntFileName, stressTempDirDes + ConstVar.SmartTowerIntFileName);
            File.Copy(stressTempDirSce + ConstVar.SmartTowerIntCHFileName, stressTempDirDes + ConstVar.SmartTowerIntCHFileName);

            List<string> stressTempPathList = new List<string> { towerStr.ModelName, ConstVar.SmartTowerIntFileName, ConstVar.SmartTowerIntCHFileName };

            //解码挂点参数
            string hpSettingPath = stressTempDirSce + towerStr.HangPointFileExtension;
            List<HangingPointSettingParas> HPSettingsParas = XmlUtils.Deserializer<List<HangingPointSettingParas>>(hpSettingPath);

            ProjectUtils.GetInstance().NewStruCalsTower(towerStr.Name, SequenceName, ExcelPath,  TowerTypeToString(tower.TowerType), ParaList[index].CommParas.Volt, struTempPathDes, stressTempPathList, HPSettingsParas);
        }


        protected string TowerTypeToString (int towerType)
        {
            string towerStr = "";
            switch (towerType)
            {
                case 1:
                    towerStr = ConstVar.LineTowerStr;
                    break;
                case 2:
                    towerStr = ConstVar.CornerTowerStr;
                    break;
            }

            return towerStr;
        }

    }

}
