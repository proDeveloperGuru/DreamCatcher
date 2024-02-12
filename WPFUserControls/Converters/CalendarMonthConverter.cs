using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using WPFUserControls.Helpers;

namespace WPFUserControls.Converters
{
    [ValueConversion(typeof(int), typeof(String))]
    public class CalendarMonthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var helper = new CalendarHelper();
            var months = helper.GetMonths();

            return months.FirstOrDefault(x => x.Value == (int)value).Key;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var helper = new CalendarHelper();
            var months = helper.GetMonths();

            return months.FirstOrDefault(x => x.Key == (string)value).Value;
        }
    }
}
