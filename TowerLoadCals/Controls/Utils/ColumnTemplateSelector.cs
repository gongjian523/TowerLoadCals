using System.Windows.Controls;
using System.Windows;


namespace TowerLoadCals.Modules
{
    public class ColumnTemplateSelector : DataTemplateSelector {
        public override DataTemplate SelectTemplate(object item, DependencyObject container) {
            Column column = (Column)item;
            return (DataTemplate)((Control)container).FindResource(column.Settings + "ColumnTemplate");
        }
    }
}