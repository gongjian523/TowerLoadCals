using System.Collections.Generic;

namespace TowerLoadCals.Mode
{
    public class Circuit
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
        /// 电流是否存在
        /// </summary>
        public bool IsCurrentExist { get; set; }

        /// <summary>
        /// 电流
        /// </summary>
        public string Current { get; set; }

        /// <summary>
        /// 电压是否存在
        /// </summary>
        public bool IsVoltageExist { get; set; }

        /// <summary>
        /// 电压
        /// </summary>
        public int Voltage { get; set; }

        /// <summary>
        /// 电压
        /// </summary>
        public List<PhaseWire> PhaseWires { get; set; }
    }
}
