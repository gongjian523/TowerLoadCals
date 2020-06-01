using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.Modules
{
    public enum SettingsType { Default, Combo, Image, Binding }

    public class Column
    {
        public string FieldName { get; set; }
        public SettingsType Settings { get; set; }
    }

    public class ComboColumn : Column
    {
        public string Header { get; set; }

        public IList Source { get; set; }
    }

    public class HeaderColumn : Column
    {
        public string Header { get; set; }
    }
}
