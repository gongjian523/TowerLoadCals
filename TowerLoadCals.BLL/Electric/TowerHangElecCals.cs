﻿using TowerLoadCals.Common;

namespace TowerLoadCals.BLL.Electric
{
    public class TowerHangElecCals : TowerElecCals
    {
        public TowerHangElecCals()
        {
            
        }

        /// <summary>
        /// 按照传统计算模式更新铁塔的本身各个挂点高度
        /// </summary>
        public override void UpdataTowerTraHei()
        {
            AbsUpSideHei = Height + UpSideInHei - RepStrIndLen;
            AbsMidHei = Height + MidInHei - RepStrIndLen;
            AbsDownSideHei = Height + DownSideHei - RepStrIndLen;
            AbsGrdHei = Height + GrDHei - RepStrGrdLen;
            AbsDownJumHei = Height + DnSideJuHei;
            AbsMidJumHei = Height + MidJuHei;
            AbsUpJumHei = Height + UpSideJuHei;
        }
    }
}
