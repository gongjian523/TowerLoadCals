using TowerLoadCals.BLL;
using TowerLoadCals.Mode;

namespace TowerLoadCals.Modules
{
    public class StruCalsLibBaseDataViewModel
    {
        public StruCalsLibBaseParas OverhangingTowerParas { get; set; }

        public StruCalsLibBaseParas TensionTowerParas { get; set; }

        public StruCalsLibBaseDataViewModel()
        {
            var libParas = GlobalInfo.GetInstance().GetStruCalsLibParas();

            if (libParas == null)
                return;

            OverhangingTowerParas = libParas.OverhangingTowerBaseParas;
            TensionTowerParas = libParas.TensionTowerBaseParas;
        }
    }
}
