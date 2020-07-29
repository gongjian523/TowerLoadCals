using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.Mode.Internet
{
    /// <summary>
    /// #附加荷载库
    /// </summary>
    [SugarTable("kb_struiceparas")]
    public class StruCalsLibExtralLoad
    {

        /// <summary>
        /// 页面是否选中
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public bool IsSelected { get; set; }
        /// <summary>
        /// id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 序号
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 电压等级
        /// </summary>
        public int Voltage { get; set; }

        /// <summary>
        /// 铁塔安装重要性系数
        /// </summary>
        public double InstallImportanceCoef { get; set; }

        /// <summary>
        /// 铁塔其他重要性系数
        /// </summary>
        public double OtherImportanceCoef { get; set; }

        /// <summary>
        /// 悬垂塔地线附加荷载
        /// </summary>
        public double OverhangingTowerEarthWireExtraLoad { get; set; }

        /// <summary>
        /// 悬垂塔导线附加荷载
        /// </summary>
        public double OverhangingTowerWireExtraLoad { get; set; }

        /// <summary>
        /// 耐张塔地线附加荷载
        /// </summary>
        public double TensionTowerEarthWireExtraLoad { get; set; }

        /// <summary>
        /// 耐张塔导线附加荷载
        /// </summary>
        public double TensionTowerWireExtraLoad { get; set; }

        /// <summary>
        /// 耐张塔跳线附加荷载
        /// </summary>
        public double TensionTowerJumperWireExtraLoad { get; set; }
    }
}
