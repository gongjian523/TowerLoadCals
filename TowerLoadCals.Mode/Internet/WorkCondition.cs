using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.Mode.Internet
{
    /// <summary>
    /// 气象条件
    /// </summary>
    [SugarTable("workcondition")]
    public class WorkConditionInternet
    {

        /// <summary>
        /// IsChosen
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public bool IsChosen { get; set; }

        /// <summary>
        /// ID
        /// </summary>
        public string Id { get; set; }


        /// <summary>
        /// categoryid 类别
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string categoryid
        {
            get { return "200mm"; }
        }

        /// <summary>
        /// 工况名称
        /// </summary>
        public string SWorkConditionName { get; set; }

        /// <summary>
        /// 风速
        /// </summary>
        public double SWindSpeed { get; set; }

        /// <summary>
        /// 气温
        /// </summary>
        public double STemperature { get; set; }

        /// <summary>
        /// 冰厚
        /// </summary>
        public double SIceThickness { get; set; }
    }
}
