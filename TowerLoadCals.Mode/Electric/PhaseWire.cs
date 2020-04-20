using System.Collections.Generic;

namespace TowerLoadCals.Mode
{
    public class PhaseWire
    {
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<CircuitPostion> Postions { get; set; }
    }
}
