using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.BLL.Electric
{
    public class TowerStrDataUtis
    {
        /// <summary>
        ///
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 铁塔外形参数对象
        /// </summary>
        public int AppreID { get; set; }

        /// <summary>
        /// 电压等级
        /// </summary>
        public int VoltageLevel { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 类型，1.直线塔；2.转角塔;3.OPGW开断塔
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        ///杆塔回路数
        /// </summary>
        public int CirNum { get; set; }

        /// <summary>
        ///交直流杆塔,交流0，直流1
        /// </summary>
        public int CurType { get; set; }

        /// <summary>
        ///地线数量
        /// </summary>
        public int GroundNum { get; set; }

        /// <summary>
        /// 计算高度
        /// </summary>
        public float CalHeight { get; set; }

        /// <summary>
        /// 最小呼高
        /// </summary>
        public float MinHeight { get; set; }

        /// <summary>
        /// 最大呼高
        /// </summary>
        public float MaxHeight { get; set; }

        /// <summary>
        /// 设计水平档距
        /// </summary>
        public float AllowedHorSpan { get; set; }

        /// <summary>
        /// 单侧最小水平档距
        /// </summary>
        public float OneSideMinHorSpan { get; set; }

        /// <summary>
        /// 单侧最大水平档距
        /// </summary>
        public float OneSideMaxHorSpan { get; set; }

        /// <summary>
        /// 最大垂直档距
        /// </summary>
        public float AllowedVerSpan { get; set; }


        /// <summary>
        /// 单侧最小垂直档距
        /// </summary>
        public float OneSideMinVerSpan { get; set; }

        /// <summary>
        /// 单侧最大垂直档距
        /// </summary>
        public float OneSideMaxVerSpan { get; set; }

        /// <summary>
        /// 单侧上拔最小档距
        /// </summary>
        public float OneSideUpVerSpanMin { get; set; }

        /// <summary>
        /// 单侧上拔最大档距
        /// </summary>
        public float OneSideUpVerSpanMax { get; set; }

        /// <summary>
        /// 最小转角
        /// </summary>
        public float MinAngel { get; set; }

        /// <summary>
        /// 最大转角
        /// </summary>
        public float MaxAngel { get; set; }

        /// <summary>
        /// 最小代表档距
        /// </summary>
        public float DRepresentSpanMin { get; set; }

        /// <summary>
        /// 最大代表档距
        /// </summary>
        public float DRepresentSpanMax { get; set; }

        /// <summary>
        /// 直线塔呼高序列
        /// </summary>
        public List<float> HeightSer { get; set; }

        /// <summary>
        /// 直线塔档距序列
        /// </summary>
        public List<float> AllowHorSpan { get; set; }

        /// <summary>
        /// 耐张塔角度折档距
        /// </summary>
        public float AngelToHorSpan { get; set; }

        /// <summary>
        /// 耐张塔最大应用水平档距
        /// </summary>
        public float MaxAngHorSpan { get; set; }
    }
}
