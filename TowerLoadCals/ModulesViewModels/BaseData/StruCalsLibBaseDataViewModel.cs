using TowerLoadCals.BLL;
using TowerLoadCals.Mode;

namespace TowerLoadCals.Modules
{
    public class StruCalsLibBaseDataViewModel
    {
        public StruCalsLibBaseParas OverhangingTowerParas { get; set; }

        //public StruCalsLibGB50545BaseParas OverhangingTowerGB50545Paras { get { return OverhangingTowerParas.BaseParasGB50545;  } }

        //public StruCalsLibDLT5551BaseParas OverhangingTowerDLT5551Paras { get { return OverhangingTowerParas.BaseParasDLT5551; } }

        public StruCalsLibBaseParas TensionTowerParas { get; set; }

        //public StruCalsLibGB50545BaseParas TensionTowerGB50545Paras { get { return TensionTowerParas.BaseParasGB50545; } }

        //public StruCalsLibDLT5551BaseParas TensionTowerDLT5551Paras { get { return TensionTowerParas.BaseParasDLT5551; } }

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
