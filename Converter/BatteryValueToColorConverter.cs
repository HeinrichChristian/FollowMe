using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace FollowMe.Converter
{
    [ValueConversion(typeof(int), typeof(Brushes))]   
    public class BatteryValueToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var batteryValue = 0;
            Int32.TryParse(value as string, out batteryValue);

            if (batteryValue < 17)
            {
                return Brushes.Red;
            }
            else if(batteryValue < 40)
            {
                return Brushes.Orange;
            }
            return Brushes.Green;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
