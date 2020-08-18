using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Phase4App
{
    public class SizeStrConverter : IValueConverter
{
    //仕様で1～3のため、先頭に空文字
    private static readonly string[] sizeStr = {"", "Small","Middle","Large" };

    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        return sizeStr[(byte)(double)value];
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        return null;
    }
}
}
