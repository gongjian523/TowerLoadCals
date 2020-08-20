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
            get { return "SmartTower";  }
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
    }
}
