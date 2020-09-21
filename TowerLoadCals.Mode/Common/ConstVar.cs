using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.Mode
{
    /// <summary>
    /// 此类用于工程文件中，各种常规字符串的定义
    /// </summary>
    public static class ConstVar
    {
        #region 电气计算 
        //重力加速度常量
        public static double GraAcc
        {
            get { return 9.80665; }
        }

        //电气计算Excel模板中参数Sheet的名称
        public static string ElecLoadParaSheet
        {
            get { return "基础数据"; }
        }

        public static string ElecLoadCalsProSheet
        {
            get { return "计算过程"; }
        }

        public static string ElecLoadCalsRstSheet
        {
            get { return "提资（计算结果）"; }
        }

        public static string ElecLoadSingleCircuitOutputSheet
        {
            get { return "单回路结构输出"; }
        }

        public static string ElecLoadDoubleCircuitOutputSheet
        {
            get { return "双回路结构输出"; }
        }

        public static string ElecLoadConvStruSheet
        {
            get { return "电气--->结构"; }
        }
        #endregion

        public static string WeatherConditioFileName
        {
            get { return "WeatherCondition.xml"; }
        }

        public static string WireFileName
        {
            get { return "Wire.xml"; }
        }

        public static string FitDataFileName
        {
            get { return "FitData.xml"; }
        }

        public static string StrDataFileName
        {
            get { return "StrData.xml"; }
        }

        public static string StruCalsParasFileName
        {
            get { return "计算参数.xml"; }
        }

        public static string StruCalsElecLoadFileName
        {
            get { return "电气负荷.xlsx"; }
        }

        public static string StruCalsLibFileName
        {
            get { return "StruCalsParasLib.xml"; }
        }

        public static string ConfigSettingsFileName
        {
            get { return "ConfigSettings.xml"; }
        }
        public static string TemplateCorrespondTableFileName
        {
            get { return "TemplateCorrespondTable.xml"; }
        }

        public static string ElecCalsSpecFileName
        {
            get { return "ElecCalsSpec.xml"; }
        }

        public static string ElecCalsCommParasFileName
        {
            get { return "ElecCalsCommParas.xml"; }
        }

        public static string ElecCalsSideParasFileName
        {
            get { return "ElecCalsSideParas.xml"; }
        }

        public static string ElecCalsTowerParasFileName
        {
            get { return "ElecCalsTowerParas.xml"; }
        }

        public static string SmartTowerQTName
        {
            get { return "SmartTower_QT.exe"; }
        }

        public static string SmartTowerConsoleName
        {
            get { return "SmartTower_Console.exe"; }
        }

        public static string SmartTowerIntFileName
        {
            get { return "data.ini"; }
        }

        public static string SmartTowerIntCHFileName
        {
            get { return "DataCh.ini"; }
        }

        public static string DataBaseStr
        {
            get { return "BaseData"; }
        }

        public static string StruCalsStr
        {
            get { return "StruCals"; }
        }

        public static string ElecCalsStr
        {
            get { return "ElecCals"; }
        }

        /// <summary>
        /// 杆塔序列
        /// </summary>
        public static string TowerSequenceStr
        {
            get { return "TowerSequence"; }
        }

        public static string UserDataStr
        {
            get { return "UserData"; }
        }

        public static string FullStressStr
        {
            get { return "FullStress"; }
        }

        public static string GeneralStruTemplateStr
        {
            get { return "GeneralStruTemplate"; }
        }

        public static string ProjectStruTemplateStr
        {
            get { return "ProjectStruTemplate"; }
        }

        public static string TowerTypeStr
        {
            get { return "TowerType"; }
        }

        public static string TowerTemplateStr
        {
            get { return "TowerTemplate"; }
        }

        public static string LineCornerTowerStr
        {
            get { return "LineCornerTower"; }
        }

        public static string LineTowerStr
        {
            get { return "LineTower"; }
        }

        public static string CornerTowerStr
        {
            get { return "CornerTower"; }
        }

        public static string BranchTowerStr
        {
            get { return "BranchTower"; }
        }

        public static string TerminalTowerStr
        {
            get { return "TerminalTower"; }
        }

        public static string SmartTowerStr
        {
            get { return "SmartTower"; }
        }

        public static string SmartTowerMode1Str
        {
            get { return "正常计算"; }
        }

        public static string SmartTowerMode2Str
        {
            get { return "基础作用力BetaZ=1"; }
        }

        public static string SmartTowerMode3Str
        {
            get { return "基础作用力betaZ=-1/2+1"; }
        }

        #region 结构计算中各种力的中文名称
        //Wind;
        public static string StruCalsWindStr
        {
            get { return "风荷wind"; } 
        }

        //GMax;
        public static string StruCalsGmaxStr
        {
            get { return "垂荷Gmax"; }
        }

        //GMin;
        public static string StruCalsGminStr
        {
            get { return "垂荷Gmin"; }
        }

        //TensionMax;
        public static string StruCalsTmaxStr
        {
            get { return "张力Tmax"; }
        }

        //TensionMin;
        public static string StruCalsTminStr
        {
            get { return "张力Tmin"; }
        }

        //Wind;
        public static string StruCalsWindDStr
        {
            get { return "风荷wind大转角"; }
        }

        //Windx;
        public static string StruCalsWindXStr
        {
            get { return "风荷wind小转角"; }
        }

        //WindDF;
        public static string StruCalsWindDFStr
        {
            get { return "前侧风荷wind大转角"; }
        }

        //WindXF;
        public static string StruCalsWindXFStr
        {
            get { return "前侧风荷wind小转角"; }
        }

        //WindDB;
        public static string StruCalsWindDBStr
        {
            get { return "后侧风荷wind大转角"; }
        }

        //WindXB;
        public static string StruCalsWindXBStr
        {
            get { return "后侧风荷wind大转角"; }
        }

        //GMaxF
        public static string StruCalsGMaxFStr
        {
            get { return "前侧垂荷Gmax"; }
        }

        //GMaxB
        public static string StruCalsGMaxBStr
        {
            get { return "后侧垂荷Gmax"; }
        }

        //GMinF
        public static string StruCalsGMinFStr
        {
            get { return "前侧垂荷Gmin"; }
        }

        //GMinB
        public static string StruCalsGMinBStr
        {
            get { return "后侧垂荷Gmin"; }
        }


        //WindTX
        public static string StruCalsWindTXStr
        {
            get { return "跳线风荷wind"; }
        }

        //GTX
        public static string StruCalsGTXStr
        {
            get { return "跳线垂荷G"; }
        }


        #endregion
    }
}
