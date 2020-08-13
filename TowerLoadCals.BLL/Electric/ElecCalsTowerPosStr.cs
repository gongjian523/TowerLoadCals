namespace TowerLoadCals.BLL.Electric
{
    //铁塔空间位置结构
    public class ElecCalsTowerPosStr
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


        public ElecCalsTowerPosStr(float span = 0, float repSpan = 0, float horiSpan = 0, float verSpan = 0)
        {
            Span = span;
            DRepresentSpan = repSpan;

            HorizontalSpan = horiSpan;
            VerticalSpan = verSpan;
        }
    }
}
