using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TowerLoadCals
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
        /// 线的类型
        /// </summary>
        public string FunctionType { get; set; }

        /// <summary>
        /// Z轴的位置
        /// </summary>
        public int Pz { get; set; }

        /// <summary>
        /// Y轴的位置
        /// </summary>
        public int Py { get; set; }

        /// <summary>
        /// X轴的位置
        /// </summary>
        public int Px { get; set; }
    }


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
        /// 电流
        /// </summary>
        public int Current{ get; set; }

        /// <summary>
        /// 电压
        /// </summary>
        public int Voltage { get; set; }

        /// <summary>
        /// 电压
        /// </summary>
        public List<PhaseWire> PhaseWires { get; set; }
    }


    public class TaStructure
    {
        /// <summary>
        /// 名字
        /// </summary>
        public string  Name { get; set; }

        /// <summary>
        /// 电路数量
        /// </summary>
        public int CircuitNum { get; set; }


        public int Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string AppearanceType { get; set; }


        /// <summary>
        ///
        /// </summary>
        public List<Circuit> CircuitSet { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int ZBaseCircuitId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int ZBasePhaseId { get; set; }
    }
}