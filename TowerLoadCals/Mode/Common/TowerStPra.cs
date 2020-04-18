using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.Mode
{
    /// <summary>
    /// 铁塔的标准设计条件
    /// </summary>
    public class TowerStPra
    {
        /// <summary>
        /// 电压等级
        /// </summary>
        public string VoltageLevel { set; get; }

        /// <summary>
        /// 杆塔型号
        /// </summary>
        public string TowerModel { set; get; }

        /// <summary>
        /// 图号
        /// </summary>
        public string PicNum { set; get; }

        /// <summary>
        /// 呼高
        /// </summary>
        public string Height { set; get; }

        /// <summary>
        /// 直线1耐张2
        /// </summary>
        public string Line1Tension2 { set; get; }

        /// <summary>
        /// 最大转角
        /// </summary>
        public string AngleMax { set; get; }

        /// <summary>
        /// 允许LH
        /// </summary>
        public string AllowedLH { set; get; }

        /// <summary>
        /// 允许LV
        /// </summary>
        public string AllowedLV { set; get; }

        /// <summary>
        /// 最大档距
        /// </summary>
        public string DistanceMax { set; get; }

        /// <summary>
        /// 允许摇摆角
        /// </summary>
        public string AllowSwingAngle { set; get; }

        /// <summary>
        /// 内过摇摆角
        /// </summary>
        public string InsideSwingAngle { set; get; }

        /// <summary>
        /// 外过摇摆角
        /// </summary>
        public string OutsideSwingAngle { set; get; }

        /// <summary>
        /// 水平线间距
        /// </summary>
        public string HorizontalWireDsitance { set; get; }

        /// <summary>
        /// 导地水平距
        /// </summary>
        public string EarthWireHorizontalDsitance { set; get; }

        /// <summary>
        /// 地线支架高
        /// </summary>
        public string EarthWireCarrierHeight { set; get; }

        /// <summary>
        /// 上下导线距
        /// </summary>
        public string UpDownWireDistance { set; get; }

        /// <summary>
        /// 中下导线距
        /// </summary>
        public string MiddleDownWireDistance { set; get; }

        /// <summary>
        /// 导平1垂2V3
        /// </summary>
        public string LineH1V2V3 { set; get; }

        /// <summary>
        /// 导平投影DP
        /// </summary>
        public string LineHProjectionDP { set; get; }

        /// <summary>
        /// 导垂投影DZ
        /// </summary>
        public string LineVProjectionDZ { set; get; }

        /// <summary>
        /// 正面根开米
        /// </summary>
        public string FrontDistanceM { set; get; }

        /// <summary>
        /// 侧面根开米
        /// </summary>
        public string SideDistanceM { set; get; }

        /// <summary>
        /// 钢重量KG
        /// </summary>
        public string SteelWeightKG { set; get; }

        /// <summary>
        /// A3F重量KG
        /// </summary>
        public string A3FWeightKG { set; get; }

        /// <summary>
        /// MN重量KG
        /// </summary>
        public string MNWeightKG { set; get; }

        /// <summary>
        /// 水泥重量KG
        /// </summary>
        public string CementWeightKG { set; get; }

        /// <summary>
        /// 本体造价
        /// </summary>
        public string BodyPrice { set; get; }

        /// <summary>
        /// 角影响LH
        /// </summary>
        public string AngelEffectLH { set; get; }

        /// <summary>
        /// 角影响档距
        /// </summary>
        public string AngelEffectDistance { set; get; }

        /// <summary>
        /// 单侧最大LV
        /// </summary>
        public string LVMax { set; get; }

        /// <summary>
        /// 最小LH
        /// </summary>
        public string LHMin { set; get; }

        /// <summary>
        /// 塔KV值
        /// </summary>
        public string TowerKV { set; get; }

        /// <summary>
        /// 截距
        /// </summary>
        public string Intercept { set; get; }

        /// <summary>
        /// 斜率
        /// </summary>
        public string Slope { set; get; }

        /// <summary>
        /// 截距增长值
        /// </summary>
        public string InterceptGrowthRate { set; get; }

        /// <summary>
        /// 斜率增长值
        /// </summary>
        public string SlopeGrowthRate { set; get; }

        /// <summary>
        /// 塔挂串个数
        /// </summary>
        public string TowerHangStringNum { set; get; }

        /// <summary>
        /// 串总个数
        /// </summary>
        public string TotalStringNum { set; get; }

        /// <summary>
        /// V串YN
        /// </summary>
        public string VStringYN { set; get; }

        /// <summary>
        /// 杆塔种类
        /// </summary>
        public string TowerType { set; get; }

        /// <summary>
        /// 基正面根开
        /// </summary>
        public string BaseFrontDistance { set; get; }

        /// <summary>
        /// 基侧面根开
        /// </summary>
        public string BaseSideDistance { set; get; }

        /// <summary>
        /// 基对角线长
        /// </summary>
        public string BaseDiagonal { set; get; }

        /// <summary>
        /// 预偏距离S1
        /// </summary>
        public string ExpectedDeviatingDistance { set; get; }

        /// <summary>
        /// 横担宽度
        /// </summary>
        public string SidelineWidth { set; get; }

        /// <summary>
        /// 是否紧凑塔
        /// </summary>
        public string IsTightTower { set; get; }

        /// <summary>
        /// V串夹角
        /// </summary>
        public string VStringAngle { set; get; }

        /// <summary>
        /// 埋深
        /// </summary>
        public string BuriedDeepth { set; get; }

        /// <summary>
        /// 裕度
        /// </summary>
        public string Margin { set; get; }

        /// <summary>
        /// 地栓型号
        /// </summary>
        public string GroundBoltModel { set; get; }

        /// <summary>
        /// 挂点ABC
        /// </summary>
        public string HungPointABC { set; get; }
    }
}
