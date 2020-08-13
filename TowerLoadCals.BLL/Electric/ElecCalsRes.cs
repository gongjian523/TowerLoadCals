using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerLoadCals.Common;

namespace TowerLoadCals.BLL.Electric
{
    /// <summary>
    /// 电力计算资资源
    /// </summary>
    public class ElecCalsRes
    {
        /// <summary>
        /// 气象条件
        /// </summary>
        public ElecCalsWeaRes Weather;

        /// <summary>
        /// 导线
        /// </summary>
        public WireUtils IndWire { get; set; }

        /// <summary>
        /// 地线
        /// </summary>
        public WireUtils GrdWire { get; set; }

        /// <summary>
        /// OPGW
        /// </summary>
        public WireUtils OPGWWire { get; set; }

        /// <summary>
        /// 跳线导线
        /// </summary>
        public WireUtils JumWire { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public SideCalUtils SideParas { get; set; }


        public ElecCalsCommRes CommParas { get; set; }

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
        public void UpdataSor(ElecCalsWeaRes WeathSor, WireUtils IndWireSor, WireUtils GrdWireSor, WireUtils OPGWWrieSor, WireUtils JumWireSor, SideCalUtils SideParaSor, ElecCalsCommRes ComParaSor)
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
