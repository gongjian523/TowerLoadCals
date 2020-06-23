
using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TowerLoadCals.ModulesViewModels
{
    public class AccordionItem
    {
        public AccordionItem(string _title, Action<AccordionItem> func)
        {
            Title = _title;
            Command = new DelegateCommand<AccordionItem>(func);
        }

        public string Title { get; set; }

        public ICommand Command { get; set; }
       
    }
}
