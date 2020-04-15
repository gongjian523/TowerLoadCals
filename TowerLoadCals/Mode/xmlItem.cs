using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.Mode
{
    public class NodeXml
    {
        /// <summary>
        /// 属性
        /// </summary>
        public Dictionary<string, string >  atts{ get; set;}
        
        /// <summary>
        /// 子节点
        /// </summary>
        public List<NodeXml> subNodes { get; set; }
    }
}

