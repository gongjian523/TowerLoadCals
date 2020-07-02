using System.Collections.ObjectModel;
using TowerLoadCals.BLL;
using TowerLoadCals.Mode;

namespace TowerLoadCals.Modules
{
    public class StruCalsLibExtralLoadViewModel
    {
        public ObservableCollection<StruCalsLibWireExtraLoadParas> WireExtraLoadParas { get; set; }

        public StruCalsLibExtralLoadViewModel()
        {
            var libParas = GlobalInfo.GetInstance().GetStruCalsLibParas();

            if (libParas == null)
                return;

            WireExtraLoadParas =  new ObservableCollection<StruCalsLibWireExtraLoadParas>(libParas.WireExtraLoadParas);
        }
    }
}
