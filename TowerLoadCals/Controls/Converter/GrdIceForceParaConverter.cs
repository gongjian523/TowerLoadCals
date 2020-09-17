using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TowerLoadCals.Converter
{

    class GrdIceForceParaConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((int)value == 1)
                return "+5mm冰张力";
            else
                return "正常冰张力";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string)value == "+5mm冰张力")
                return 1;
            else
                return 2;
        }
    }
}
