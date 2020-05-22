using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.Mode
{
    public class StruLineParas
    {
        /// <summary>
        /// 序号
        /// </summary>
        public float Index { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public float WireType { get; set; }

        /// <summary>
        /// 附加荷载 旧名称 fh 直线塔
        /// 线条附加荷载 旧名称 fh_1 转角塔
        /// </summary>
        public float WireExtraLoad { get; set; }

        /// <summary>
        /// 锚线张力
        /// 旧名称 mz 直线塔
        /// 旧名称 mxz 转角塔  
        /// </summary>
        public float AnchorTension { get; set; }

        /// <summary>
        /// 过滑车张力差
        /// 旧名称 ghcz 直线塔 转角塔
        /// </summary>
        public float PulleyTensionDif { get; set; }

        /// <summary>
        /// 吊荷系数
        /// 旧名称 diaoxi 直线塔 
        /// </summary>
        public float HoistingCoef { get; set; }

        /// <summary>   
        /// 最小转角
        /// 旧名称 xiaojiao 直线塔
        /// 旧名称 minangle 转角塔
        /// </summary>
        public float AngleMin { get; set; }

        /// <summary>
        /// 最大转角
        /// 旧名称 dajiao 直线塔
        /// 旧名称 minangle 转角塔
        /// </summary>
        public float AngleMax { get; set; }

        /// <summary>
        /// 临时拉张
        /// 旧名称 llz 转角塔
        /// </summary>
        public float TemporaryTension { get; set; }
        

        /// <summary>
        /// 过牵引系数
        /// 旧名称 gqx  直角塔 转角塔
        /// </summary>
        public float DrawingCoef { get; set; }

        /// <summary>
        /// 跳串数目
        /// 旧名称 nt 转角塔
        /// </summary>
        public float TstringNum { get; set; }

        /// <summary>
        /// 跳线附加荷载   
        /// 旧名称 fh_2  转角塔
        /// </summary>
        public float TwireExtraLoad { get; set; }

        /// <summary>
        /// 前侧转角
        /// 旧名称 qzjiao 转角塔
        /// </summary>
        public float AngleFront { get; set; }


        /// <summary>
        /// 后侧转角
        /// 旧名称 hzjiao 转角塔
        /// </summary>
        public float AngleBack { get; set; }

        /// <summary>
        /// 门构小张
        /// 旧名称 mgxz 转角塔
        /// </summary>
        public float PortalTensionMin { get; set; }

        /// <summary>
        /// 门构大张
        /// 旧名称 mgdz 转角塔
        /// </summary>
        public float PortalTensionMax { get; set; }

        /// <summary>
        /// 右转
        /// 旧名称 BL3 转角塔
        /// </summary>
        public bool isTurnRight { get; set; }
    }
}
