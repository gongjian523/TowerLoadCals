using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TowerLoadCals.Mode;

namespace TowerLoadCals.Converter
{
    public class SmartTowerModeStringConvert
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((int)value == 0)
                return ConstVar.SmartTowerMode1Str;
            else if ((int)value == 1)
                return ConstVar.SmartTowerMode2Str;
            else
                return ConstVar.SmartTowerMode3Str;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string)value == ConstVar.SmartTowerMode1Str)
                return 0;
            else if ((string)value == ConstVar.SmartTowerMode2Str)
                return 1;
            else
                return 2;
        }
    }
}
