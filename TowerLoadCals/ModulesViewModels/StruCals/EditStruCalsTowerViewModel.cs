using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TowerLoadCals.Common.Utils;
using System.Windows.Input;
using TowerLoadCals.BLL;

namespace TowerLoadCals.Modules
{
    public class EditStruCalsTowerViewModel : NewStruCalsTowerViewModel
    {
        public EditStruCalsTowerViewModel()
        {

        }

        public override void  onConfirm()
        {
            //if(ProjectUtils.NewStruCalsTower(TowerName, TowerType, TemplatePath, ElectricalLoadFilePath, _fullStressTemplatePaths))
            //{
            //    close(TowerName);
            //}
        }

    }
}
