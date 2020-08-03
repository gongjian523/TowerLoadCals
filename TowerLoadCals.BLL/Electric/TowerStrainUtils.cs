using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.BLL.Electric
{
    public class TowerStrainUtils:TowerUtils
    {
        public TowerStrainUtils()
        {

        }

        /// <summary>
        /// 拷贝前后计算资源进入计算模型，并更新导线、地线数据，前期是铁塔本体数据已更新
        /// </summary>
        /// <param name="BackSideResSor"></param>
        /// <param name="FrontSideResSor"></param>
        void GetAndUpdateSideRes(string BackSideResSor, string FrontSideResSor)
        {

        }

        /// <summary>
        /// 计算各个工况的垂直档距，耐张塔分为前后侧计算
        /// </summary>
        void CalHoriVetValue()
        {

        }

        /// <summary>
        /// 计算大风工况下垂直方向的应用弧垂
        /// </summary>
        void CalDFCure()
        {

        }

    }
}
