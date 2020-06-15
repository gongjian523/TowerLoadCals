using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.Mode
{
    public class StruRatioParas
    {

        public StruRatioParas()
        {
            NormalXYPoints = new List<HangingPointParas>();
            NormalZPoints = new List<HangingPointParas>();
            InstallXYPoints = new List<HangingPointParas>();
            InstallZPoints = new List<HangingPointParas>();
            TurningPoints = new List<HangingPointParas>();

            VStrings = new List<VStringParas>();
        }

        /// <summary>
        /// 地线常规前侧
        /// </summary>
        public float GCQ { get; set; }

        /// <summary>
        /// 地线常规后侧
        /// </summary>
        public float GCH { get; set; }

        /// <summary>
        /// 地线悬臂内侧
        /// </summary>
        public float GXN { get; set; }

        /// <summary>
        /// 地线悬臂外侧
        /// </summary>
        public float GXW { get; set; }


        /// <summary>
        /// 导线前侧风荷其他
        /// </summary>
        public float DQWQ { get; set; }

        /// <summary>
        /// 导线后侧风荷其他
        /// </summary>
        public float DQWH { get; set; }

        /// <summary>
        /// 导线前侧垂荷其他
        /// </summary>
        public float DQCQ { get; set; }

        /// <summary>
        /// 导线后侧垂荷其他
        /// </summary>
        public float DQCH { get; set; }

        /// <summary>
        /// 导线前侧风荷吊装
        /// </summary>
        public float DDWQ { get; set; }

        /// <summary>
        /// 导线后侧风荷吊装
        /// </summary>
        public float DDWH { get; set; }

        /// <summary>
        /// 导线前侧垂荷吊装
        /// </summary>
        public float DDCQ { get; set; }

        /// <summary>
        /// 导线后侧垂荷吊装
        /// </summary>
        public float DDCH { get; set; }


        /// <summary>
        /// 导线前侧风荷锚线
        /// </summary>
        public float DMWQ { get; set; }

        /// <summary>
        /// 导线后侧风荷锚线
        /// </summary>
        public float DMWH { get; set; }

        /// <summary>
        /// 导线前侧垂荷锚线
        /// </summary>
        public float DMCQ { get; set; }

        /// <summary>
        /// 导线后侧垂荷锚线
        /// </summary>
        public float DMCH { get; set; }


        /// <summary>
        /// 常规情况跳线前比例
        /// </summary>
        public float BLTQ { get; set; }

        /// <summary>
        /// 常规情况跳线中比例
        /// </summary>
        public float BLTZ { get; set; }

        /// <summary>
        /// 常规情况跳线后比例
        /// </summary>
        public float BLTH { get; set; }

        /// <summary>
        /// 吊装情况跳线前比例
        /// </summary>
        public float BLDZTQ { get; set; }

        /// <summary>
        /// 吊装情况跳线中比例
        /// </summary>
        public float BLDZTZ { get; set; }

        /// <summary>
        /// 吊装情况跳线后比例
        /// </summary>
        public float BLDZTH { get; set; }


        public List<HangingPointParas> NormalXYPoints { get; set; }

        public List<HangingPointParas> NormalZPoints { get; set; }

        public List<HangingPointParas> InstallXYPoints { get; set; }

        public List<HangingPointParas> InstallZPoints { get; set; }

        public List<HangingPointParas> TurningPoints { get; set; }

        public List<VStringParas> VStrings { get; set; }

    }
}
