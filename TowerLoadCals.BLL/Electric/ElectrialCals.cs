using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerLoadCals.Common;

namespace TowerLoadCals.BLL.Electric
{
    public class ElectrialCals
    {
        /// <summary>
        /// 气象条件
        /// </summary>
        protected WeatherUtils Weather;

        /// <summary>
        /// 导线
        /// </summary>
        protected WireUtils IndWire { get; set; }

        /// <summary>
        /// 地线
        /// </summary>
        protected WireUtils GrdWire { get; set; }

        /// <summary>
        /// OPGW
        /// </summary>
        protected WireUtils OPGWWire { get; set; }

        /// <summary>
        /// 跳线导线
        /// </summary>
        protected WireUtils JumWire { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected SideCalUtils SideParas { get; set; }
        

        protected ElectricalCommonUtils CommParas { get; set; }

        /// <summary>
        /// 配置计算数据,并刷新导线相关参数等
        /// </summary>
        /// <param name="WeathSor"></param>
        /// <param name="IndWireSor"></param>
        /// <param name="GrdWireSor"></param>
        /// <param name="OPGWWrieSor"></param>
        /// <param name="JumWireSor"></param>
        /// <param name="SideParaSor"></param>
        /// <param name="ComParaSor"></param>
        public void UpdataSor(WeatherUtils WeathSor, WireUtils IndWireSor, WireUtils GrdWireSor, WireUtils OPGWWrieSor, WireUtils JumWireSor, SideCalUtils SideParaSor, ElectricalCommonUtils ComParaSor)
        {
            Weather = XmlUtils.Clone(WeathSor);
            IndWire = XmlUtils.Clone(IndWireSor);
            GrdWire = XmlUtils.Clone(GrdWireSor);
            OPGWWire = XmlUtils.Clone(OPGWWrieSor);
            JumWire = XmlUtils.Clone(JumWireSor);
            SideParas = SideParaSor;
            CommParas = ComParaSor;
        }

        /// <summary>
        /// 刷新导地线计算
        /// </summary>
        /// <param name="spanVal"></param>
        public void FlashWireData(float spanVal)
        {
            IndWire.UpdataPara(Weather, CommParas, SideParas);
            IndWire.CalBZ();
            IndWire.SaveYLTabel(spanVal);
        }

        /// <summary>
        ///  刷新跳线计算
        /// </summary>
        /// <param name="spanVal"></param>
        public void FlashJumWireData(float spanVal)
        {
            JumWire.UpdataPara(Weather, CommParas, SideParas);
            JumWire.CalBZ();
            JumWire.SaveYLTabel(spanVal);
        }

    }
}
