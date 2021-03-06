﻿using System.Collections.Generic;

namespace TowerLoadCals.Mode
{
    public class TowerStructure
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