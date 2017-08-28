using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Forms;

namespace PaintMeter.Converts
{
    class BoolToSolidColorBrushValueConverterToButton : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (null == value)
            {
                Color color = System.Windows.SystemColors.ControlLightBrush.Color;
                return new SolidColorBrush(color);
            }
            if (value is bool?)
            {
                Color color;
                if (value == null)
                {
                    color = System.Windows.SystemColors.ControlLightBrush.Color;
                }
                if ((bool?)value == true)
                {
                    color =  Colors.Green;
                }
                else
                {
                    color = System.Windows.SystemColors.ControlLightBrush.Color;
                }
                return new SolidColorBrush(color);
            }

            Type type = value.GetType();
            throw new InvalidOperationException("Unsupported type [" + type.Name + "]");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // If necessary, here you can convert back. Check if which brush it is (if its one),
            // get its Color-value and return it.

            throw new NotImplementedException();
        }
    }
}
