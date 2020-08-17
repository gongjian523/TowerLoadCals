namespace TowerLoadCals.BLL.Electric
{
    //铁塔空间位置结构
    public class ElecCalsTowerPosStr
    {
        /// <summary>
        /// 档距
        /// </summary>
        public double Span { get; set; }

        /// <summary>
        /// HorizontalSpan
        /// </summary>
        public double HorizontalSpan { get; set; }

        /// <summary>
        /// 垂直档距
        /// </summary>
        public double VerticalSpan { get; set; }

        /// <summary>
        /// 代表档距
        /// </summary>
        public double DRepresentSpan { get; set; }

        public ElecCalsTowerPosStr()
        {
        }

        public ElecCalsTowerPosStr(double span = 0, double repSpan = 0, double horiSpan = 0, double verSpan = 0)
        {
            Span = span;
            DRepresentSpan = repSpan;

            HorizontalSpan = horiSpan;
            VerticalSpan = verSpan;
        }
    }
}
