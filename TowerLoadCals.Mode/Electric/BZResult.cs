namespace TowerLoadCals.Mode.Electric
{
    public class BZResult
    {
        public double g1 { get; set; }

        public double g2 { get; set; }

        public double g3 { get; set; }

        public double g4 { get; set; }

        public double g5 { get; set; }

        public double g6 { get; set; }

        public double g7 { get; set; }

        public double gi { get; set; }

        /// <summary>
        /// 比载
        /// </summary>
        public double BiZai { get; set;}

        /// <summary>
        /// 垂直比载
        /// </summary>
        public double VerBizai { get; set; }

        /// <summary>
        /// 横向比载
        /// </summary>
        public double HorBizai { get; set; }

        /// <summary>
        /// 应力
        /// </summary>
        public double Stress { get; set; }

        /// <summary>
        /// 垂直荷载
        /// </summary>
        public double VerHezai { get; set; }

        /// <summary>
        /// 风荷载
        /// </summary>
        public double WindHezai { get; set; }
    }
}
