using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.DAL
{
    public class BaseDataNameTreeItem
    {
        public int ID { get; set; }

        public int ParentID { get; set; }

        public string Name { get; set; }
    }
}
