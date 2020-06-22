using System.Windows;
using System.Windows.Controls;

namespace TowerLoadCals.Modules
{
    public class BandTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            Band band = (Band)item;
            if (band.ChildColumns.Count == 1)
            {
                return (DataTemplate)((Control)container).FindResource("SingleColumnBandTemplate");
            }
            return (DataTemplate)((Control)container).FindResource("MultiColumnBandTemplate");
        }
    }
}
