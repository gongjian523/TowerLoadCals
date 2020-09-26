using System.Xml.Serialization;

namespace TowerLoadCals.BLL.Electric
{
    //档中金具结构
    //CounterWeight 
    //Spacer 
    public class ElecCalsSpanFit
    {
        /// <summary>
        /// 导线防振锤资源ID
        /// </summary>
        [XmlAttribute]
        public int InFZCID { get; set; }

        /// <summary>
        /// 导线防振锤数量
        /// </summary>
        [XmlAttribute]
        public int NumInFZC { get; set; }

        /// <summary>
        /// 导线防振锤重量
        /// </summary>
        [XmlAttribute]
        public double WeiInFZC { get; set; }

        /// <summary>
        /// 地线防振锤ID
        /// </summary>
        [XmlAttribute]
        public int GrFZCID { get; set; }

        /// <summary>
        /// 地线防振锤数量
        /// </summary>
        [XmlAttribute]
        public int NumGrFZC { get; set; }

        /// <summary>
        /// 地线防振锤重量
        /// </summary>
        [XmlAttribute]
        public double WeiGrFZC { get; set; }

        /// <summary>
        /// 间隔棒资源ID
        /// </summary>
        [XmlAttribute]
        public int JGBID { get; set; }

        /// <summary>
        /// 间隔棒数量
        /// </summary>
        [XmlAttribute]
        public int NumJGB { get; set; }

        /// <summary>
        /// 间隔棒重量
        /// </summary>
        [XmlAttribute]
        public double WeiJGB { get; set; }


        public ElecCalsSpanFit()
        {
        }

        public ElecCalsSpanFit(int inFZCID = 0, int numInFZC = 0, double weiInFZC = 0, int grFZCID = 0, int numGrFZC = 0, double weiGrFZC = 0, int jGBID = 0, int numJGB = 0, double weiJGB = 0)
        {
            InFZCID = inFZCID;
            NumInFZC = numInFZC;
            GrFZCID = grFZCID;
            NumGrFZC = numGrFZC;
            JGBID = jGBID;
            NumJGB = numJGB;

            WeiInFZC = weiInFZC;
            WeiGrFZC = weiGrFZC;
            WeiJGB = weiJGB;
        }

    }
}
