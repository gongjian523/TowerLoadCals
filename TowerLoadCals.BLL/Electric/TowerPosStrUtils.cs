namespace TowerLoadCals.BLL.Electric
{
    //铁塔空间位置结构
    public class TowerPosStrUtils
    {
        /// <summary>
        /// 档距
        /// </summary>
        public float Span { get; set; }

        /// <summary>
        /// HorizontalSpan
        /// </summary>
        public float HorizontalSpan { get; set; }

        /// <summary>
        /// 垂直档距
        /// </summary>
        public float VerticalSpan { get; set; }

        /// <summary>
        /// 代表档距
        /// </summary>
        public float DRepresentSpan { get; set; }

        public TowerPosStrUtils()
        {
            Span = 0;
            HorizontalSpan = 0;
            VerticalSpan = 0;
            DRepresentSpan = 0;
        }
    }
}
