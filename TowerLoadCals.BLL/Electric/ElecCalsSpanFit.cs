namespace TowerLoadCals.BLL.Electric
{
    //档中金具结构
    public class ElecCalsSpanFit
    {
        /// <summary>
        /// 导线防振锤资源ID
        /// </summary>
        public int InFZCID { get; set; }

        /// <summary>
        /// 导线防振锤数量
        /// </summary>
        public int NumInFZC { get; set; }

        /// <summary>
        /// 导线防振锤重量
        /// </summary>
        public int WeiInFZC { get; set; }

        /// <summary>
        /// 地线防振锤ID
        /// </summary>
        public int GrFZCID { get; set; }

        /// <summary>
        /// 地线防振锤数量
        /// </summary>
        public int NumGrFZC { get; set; }

        /// <summary>
        /// 地线防振锤重量
        /// </summary>
        public int WeiGrFZC { get; set; }

        /// <summary>
        /// 间隔棒资源ID
        /// </summary>
        public int JGBID { get; set; }

        /// <summary>
        /// 间隔棒数量
        /// </summary>
        public int NumJGB { get; set; }

        /// <summary>
        /// 间隔棒重量
        /// </summary>
        public int WeiJGB { get; set; }


        public ElecCalsSpanFit()
        {
        }

        public ElecCalsSpanFit(int inFZCID = 0, int numInFZC = 0, int weiInFZC = 0, int grFZCID = 0, int numGrFZC = 0, int weiGrFZC = 0, int jGBID = 0, int numJGB = 0, int weiJGB = 0)
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
