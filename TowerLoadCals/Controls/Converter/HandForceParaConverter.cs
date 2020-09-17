using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TowerLoadCals.Converter
{

    class HandForceParaConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((int)value == 1)
                return "两者大值";
            else if ((int)value == 2)
                return "系数法";
            else
                return "降温法";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string)value == "两者大值")
                return 1;
            if ((string)value == "系数法")
                return 2;
            else
                return 3;
        }
    }
}
