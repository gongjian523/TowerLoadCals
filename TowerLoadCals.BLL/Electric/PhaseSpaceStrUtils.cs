namespace TowerLoadCals.BLL.Electric
{
    //相空间位置结构，用于存储相空间信息，暂时存储档距和高差
    public class PhaseSpaceStrUtils
    {
        /// <summary>
        /// 档距
        /// </summary>
        public float Span { get; set; }

        /// <summary>
        /// 高差数据
        /// </summary>
        public float SubHei { get; set; }

        /// <summary>
        /// 线挂点高度
        /// </summary>
        public float GDHei { get; set; }

        /// <summary>
        /// 线平均高度
        /// </summary>
        public float WireHight { get; set; }

        /// <summary>
        /// 串平均高度
        /// </summary>
        public float StrHeight { get; set; }

        /// <summary>
        /// 跳线平均高度
        /// </summary>
        public float JmHeight { get; set; }

        /// <summary>
        /// 支撑管高度
        /// </summary>
        public float SupHeight { get; set; }

        public PhaseSpaceStrUtils()
        {
            Span = 0;
            SubHei = 0;
            GDHei = 0;
            WireHight = 0;
            StrHeight = 0;
            JmHeight = 0;
            SupHeight = 0;
        }
    }
}
