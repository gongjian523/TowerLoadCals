namespace TowerLoadCals.BLL.Electric
{
    //相空间位置结构，用于存储相空间信息，暂时存储档距和高差
    public class PhaseSpaceStrUtils
    {
        /// <summary>
        /// 档距
        /// </summary>
        public double Span { get; set; }

        /// <summary>
        /// 高差数据
        /// </summary>
        public double SubHei { get; set; }

        /// <summary>
        /// 线挂点高度
        /// </summary>
        public double GDHei { get; set; }

        /// <summary>
        /// 线平均高度
        /// </summary>
        public double WireHight { get; set; }

        /// <summary>
        /// 串平均高度
        /// </summary>
        public double StrHeight { get; set; }

        /// <summary>
        /// 跳线平均高度
        /// </summary>
        public double JmHeight { get; set; }

        /// <summary>
        /// 支撑管高度
        /// </summary>
        public double SupHeight { get; set; }

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
