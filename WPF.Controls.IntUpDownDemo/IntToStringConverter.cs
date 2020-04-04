using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;


namespace WPF.Controls.IntUpDownDemo
{
    public class IntToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && value is int)
                return value.ToString();
            else
                return "0";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && value is string && int.TryParse((string)value, out var result))
                return result;
            else
                return 0;
        }
    }
}
