using System.Windows;
using System.Windows.Controls;

namespace WPFCustomControls
{
    public class CalendarButton : Button
    {
        public class CalendarButtonLabel
        {
            public DateTime Date { get; set; }

            public string? Text { get; set; }
        }

        private static DependencyProperty CalendarLabelsProperty = DependencyProperty.Register("CalendarLabels",
            typeof(List<CalendarButtonLabel>), typeof(CalendarButton));

        public List<CalendarButtonLabel> CalendarLabels
        {
            get { return (List<CalendarButtonLabel>)GetValue(CalendarLabelsProperty); }
            set { SetValue(CalendarLabelsProperty, value);}
        }

        private static DependencyProperty CalendarDateProperty = DependencyProperty.Register("CalendarDate",
            typeof(DateTime), typeof(CalendarButton));

        public DateTime CalendarDate
        {
            get {  return (DateTime)GetValue(CalendarDateProperty);}
            set {  SetValue(CalendarDateProperty, value); }
        }



    }
}
