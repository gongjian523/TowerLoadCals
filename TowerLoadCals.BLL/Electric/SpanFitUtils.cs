namespace TowerLoadCals.BLL.Electric
{
    //档中金具结构
    public class SpanFitUtils
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
        /// 地线防振锤ID
        /// </summary>
        public int GrFZCID { get; set; }

        /// <summary>
        /// 地线防振锤数量
        /// </summary>
        public int NumGrFZC { get; set; }

        /// <summary>
        /// 间隔棒资源ID
        /// </summary>
        public int JGBID { get; set; }

        /// <summary>
        /// 间隔棒数量
        /// </summary>
        public int NumJGB { get; set; }


        public SpanFitUtils()
        {
            InFZCID = 0;
            NumInFZC = 0;
            GrFZCID = 0;
            NumGrFZC = 0;
            JGBID = 0;
            NumJGB = 0;    
        }


    }
}
