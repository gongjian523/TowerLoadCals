using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerLoadCals.DAL;
using TowerLoadCals.Mode;
using static TowerLoadCals.DAL.TowerTemplateReader;

namespace TowerLoadCals.Modules
{
    public enum SettingsType { Default,Binding }

    public class Column
    {
        public string FieldName { get; set; }
        public SettingsType Settings { get; set; }
    }

    public class HeaderColumn : Column
    {
        public string Header { get; set; }
    }

    public class WorkConditionComboViewModel
    {
        public IList<WorkConditionComboSpec> WorkConditions { get; private set; }
        public ObservableCollection<Column> Columns { get; private set; }

        public WorkConditionComboViewModel()
        {

            TowerTemplateReader templateReader = new TowerTemplateReader(TowerType.LineTower);
            TowerTemplate template = templateReader.Read("D:\\00-项目\\P-200325-杆塔负荷程序\\数据资源示例\\塔库\\双回交流重冰区.dat");

            WorkConditions = templateReader.ConvertTemplateToSpec(template);

            List<Column> columns = new List<Column>();
            //columns.Add(new Column() { FieldName = "IsCalculate" });
            columns.Add(new HeaderColumn() { Settings = SettingsType.Binding, FieldName = "WorkConditionCode" , Header ="工况"});
            columns.Add(new Column() { FieldName = "WindDirectionCode" });
            columns.Add(new Column() { FieldName = "Wire" });
            columns.Add(new Column() { FieldName = "Wire2" });
            columns.Add(new Column() { FieldName = "Wire3" });
            columns.Add(new Column() { FieldName = "Wire4" });
            columns.Add(new Column() { FieldName = "Wire5" });
            //columns.Add(new Column() { FieldName = "WorkComment" });

            Columns = new ObservableCollection<Column>(columns);
        }

    }
}
