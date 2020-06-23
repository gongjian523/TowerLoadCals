using System.Collections.ObjectModel;

namespace TowerLoadCals.Modules
{
    // Corresponds to a band column.  
    public class Band
    {
        // Specifies the band header. 
        public string Header { get; set; }
        public ObservableCollection<HeaderColumn> ChildColumns { get; set; }
    }
}
