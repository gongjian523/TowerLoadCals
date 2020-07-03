using System.Collections.ObjectModel;
using TowerLoadCals.BLL;
using TowerLoadCals.Mode;

namespace TowerLoadCals.Modules
{
    public class StruCalsLibIceCoverModuleVodel
    {
        public ObservableCollection<StruCalsLibIceCoverParas> IceCoverParas { get; set; }

        public StruCalsLibIceCoverModuleVodel()
        {
            var libParas = GlobalInfo.GetInstance().GetStruCalsLibParas();

            if (libParas == null)
                return;

            IceCoverParas = new ObservableCollection<StruCalsLibIceCoverParas>(libParas.IceCoverParas);
        }
    }
}
