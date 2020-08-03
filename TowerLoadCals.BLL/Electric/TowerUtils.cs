using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.BLL.Electric
{
    public class TowerUtils
    {
        /// <summary>
        /// 配置杆塔基本外形信息
        /// </summary>
        public void SetAppreaPara()
        {

        }

        /// <summary>
        /// 设置杆塔的基本信息,以前视方向为正方向
        /// </summary>
        public void SetPosInf()
        {

        }


        /// <summary>
        /// 按照传统计算模式更新铁塔的本身各个挂点高度
        /// </summary>
        public void UpdataTowerTraHei()
        {

        }


        /// <summary>
        /// 更新各相的绝对高度，考虑基面
        /// </summary>
        public void UpdateAtitudeTower()
        {

        }

        /// <summary>
        /// 采用后侧铁塔信息更新本塔数据
        /// </summary>
        /// <param name="BackTower"></param>
        public void UpdateBackInfo(string BackTower)
        {

        }

        /// <summary>
        /// 更新相后侧高差，档距等数据，传统模式  本塔-后侧铁塔
        /// 有5个相位
        /// </summary>
        /// <param name="PntTower"></param>
        public void FlashBackHeiSub(string PntTower)
        {

        }

        /// <summary>
        /// 更新前侧高差，传统模式  本塔-前侧铁塔
        /// 有5个相位
        /// </summary>
        /// <param name="PntTower"></param>
        public void flashFrontHeiSub(string PntTower)
        {

        }

        /// <summary>
        /// 更新铁塔前后水平档距和垂直档距，此处计算可能没用
        /// </summary>
        public void FlashHoriVetSpan()
        {

        }



    }
}
