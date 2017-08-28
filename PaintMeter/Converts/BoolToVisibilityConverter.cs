using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace PaintMeter.Converts
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (null == value)
            {
                return Visibility.Hidden;
            }
            if (value is bool?)
            {
                if (value == null)
                {
                    return Visibility.Hidden;
                }

                if ((bool?)value == false)
                {
                    return Visibility.Hidden;
                }
                else
                {
                    return Visibility.Visible;
                }
            }

            Type type = value.GetType();
            throw new InvalidOperationException("Unsupported type [" + type.Name + "]");
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
