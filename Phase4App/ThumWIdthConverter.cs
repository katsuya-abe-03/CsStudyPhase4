using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Phase4App
{
    public class ThumWidthConverter : IValueConverter
    {
        private const int thumSmallSize = 40;
        public const int thumBigSize = 40 * 3;

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //valueは、仕様で1(small)～3(big)
            return (double)value * thumSmallSize;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
