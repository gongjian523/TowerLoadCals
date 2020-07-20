using System.Collections.ObjectModel;
using TowerLoadCals.BLL;
using TowerLoadCals.Mode;

namespace TowerLoadCals.Modules
{
    public class StruCalsLibIceCoverViewModel
    {
        public ObservableCollection<StruCalsLibIceCoverParas> IceCoverParas { get; set; }

        public StruCalsLibIceCoverViewModel()
        {
            var libParas = GlobalInfo.GetInstance().GetStruCalsLibParas();

            if (libParas == null)
                return;

            IceCoverParas = new ObservableCollection<StruCalsLibIceCoverParas>(libParas.IceCoverParas);
        }
    }
}
