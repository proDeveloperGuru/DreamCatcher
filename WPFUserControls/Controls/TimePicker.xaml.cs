using MaterialDesignThemes.Wpf;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace WPFUserControls.Controls
{
    public partial class TimePicker : UserControl
    {
        private static DependencyProperty TimeProperty = DependencyProperty.Register("Time",
            typeof(DateTime), typeof(TimePicker), new PropertyMetadata(DateTime.Now));

        public DateTime Time
        {
            get { return (DateTime)GetValue(TimeProperty); }
            set { SetValue(TimeProperty, value); }
        }

        private static DependencyProperty Is24HourProperty = DependencyProperty.Register("Is24Hour",
            typeof(bool), typeof(TimePicker), new PropertyMetadata(false));

        public bool Is24Hour
        {
            get { return (bool)GetValue(Is24HourProperty); }
            set { SetValue(Is24HourProperty, value); }
        }

        public TimePicker()
        {
            InitializeComponent();
        }

        public void ClockDialogOpenedEventHandler(object sender, DialogOpenedEventArgs eventArgs)
        {
            Clock.Time = Time;
        }


        public void ClockDialogClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if (Equals(eventArgs.Parameter, "1"))
                Time = Clock.Time;
        }

        private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
