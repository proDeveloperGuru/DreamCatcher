using MaterialDesignThemes.Wpf;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace WPFUserControls.Controls
{
    /// <summary>
    /// Interaction logic for DatePicker.xaml
    /// </summary>
    public partial class DatePicker : UserControl
    {
        private static DependencyProperty SelectedDateProperty = DependencyProperty.Register("SelectedDate",
            typeof(DateTime), typeof(DatePicker), new PropertyMetadata(DateTime.Now));

        public DateTime SelectedDate
        {
            get { return (DateTime)GetValue(SelectedDateProperty); }
            set { SetValue(SelectedDateProperty, value); }
        }
        public DatePicker()
        {
            InitializeComponent();
        }

        public void CalendarDialogOpenedEventHandler(object sender, DialogOpenedEventArgs eventArgs)
            => this.Calendar.SelectedDate = SelectedDate;

        public void CalendarDialogClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if (!Equals(eventArgs.Parameter, "1")) return;

            if (!this.Calendar.SelectedDate.HasValue)
            {
                eventArgs.Cancel();
                return;
            }

            SelectedDate = this.Calendar.SelectedDate.Value;
        }

        private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
