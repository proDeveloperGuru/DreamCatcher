using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPFUserControls.Controls
{
    public class CalendarButton : Button
    {
        public class CalendarButtonLabel
        {
            public DateTime Date { get; set; }

            public string? Text { get; set; }
        }

        //private static DependencyProperty HoverGlowProperty = DependencyProperty.Register("HoverGlow",
        //    typeof(Color), typeof(Calendar), new PropertyMetadata(Colors.White));

        //public Color HoverGlow
        //{
        //    get { return (Color)GetValue(HoverGlowProperty); }
        //    set { SetValue(HoverGlowProperty, value); }
        //}

        //private static DependencyProperty ActiveGlowProperty = DependencyProperty.Register("ActiveGlow",
        //    typeof(Color), typeof(Calendar), new PropertyMetadata(Colors.Transparent));

        //public Color ActiveGlow
        //{
        //    get { return (Color)GetValue(ActiveGlowProperty); }
        //    set { SetValue(ActiveGlowProperty, value); }
        //}

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

        public CalendarButton()
        {
        }
    }
}
