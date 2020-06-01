using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerLoadCals.ModulesViewModels
{
    public interface IBaseViewModel
    {
        void Save();

        void UpDateView(string para1, string para2 = "");

        void DelSubItem(string itemName);
    }


    public interface IStruCalsBaseViewModel: IBaseViewModel
    {
        String GetTowerType();
    }
}
