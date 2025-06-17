using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Effects;
using WPFUserControls.Converters;
using WPFUserControls.Helpers;
using static WPFUserControls.Controls.CalendarButton;

namespace WPFUserControls.Controls
{
    /// <summary>
    /// Interaction logic for Calendar.xaml
    /// </summary>
    public partial class Calendar : UserControl
    {
        CalendarHelper _helper;
        Dictionary<string, int> _months;
        Style _btnStyle;
        EqualityConverter _equalityConverter = new EqualityConverter();

        private static DependencyProperty HoverGlowProperty = DependencyProperty.Register("HoverGlow",
            typeof(Color), typeof(Calendar), new PropertyMetadata(Colors.White));

        public Color HoverGlow
        {
            get { return (Color)GetValue(HoverGlowProperty);}
            set { SetValue(HoverGlowProperty, value);}
        }

        private static DependencyProperty ActiveGlowProperty = DependencyProperty.Register("ActiveGlow",
            typeof(Color), typeof(Calendar), new PropertyMetadata(Colors.Red));

        public Color ActiveGlow
        {
            get { return (Color)GetValue(ActiveGlowProperty); }
            set { SetValue(ActiveGlowProperty, value); }
        }

        private static DependencyProperty SelectedYearProperty = DependencyProperty.Register("SelectedYear",
            typeof(int), typeof(Calendar), new PropertyMetadata(DateTime.Now.Year));

        public int SelectedYear
        {
            get {  return (int)GetValue(SelectedYearProperty);}
            set { SetValue(SelectedYearProperty, value);}
        }

        private static DependencyProperty SelectedMonthProperty = DependencyProperty.Register("SelectedMonth",
            typeof(int), typeof(Calendar), new PropertyMetadata(DateTime.Now.Month));

        public int SelectedMonth
        {
            get { return (int)GetValue(SelectedMonthProperty); }
            set { SetValue(SelectedMonthProperty, value);}
        }

        private static DependencyProperty ActiveDateProperty = DependencyProperty.Register("ActiveDate",
            typeof(DateTime), typeof(Calendar));

        public DateTime ActiveDate
        {
            get { return (DateTime)GetValue(ActiveDateProperty); }
            set { SetValue(ActiveDateProperty, value);}
        }

        private static DependencyProperty TodayBackgroundProperty = DependencyProperty.Register("TodayBackground",
            typeof(SolidColorBrush), typeof(Calendar));

        public SolidColorBrush TodayBackground
        {
            get { return (SolidColorBrush)GetValue(TodayBackgroundProperty); }
            set { SetValue(TodayBackgroundProperty, value); }
        }

        private static DependencyProperty TodayForegroundProperty = DependencyProperty.Register("TodayForeground",
            typeof(SolidColorBrush), typeof(Calendar));

        public SolidColorBrush TodayForeground
        {
            get { return (SolidColorBrush)GetValue(TodayForegroundProperty); }
            set { SetValue(TodayForegroundProperty, value); }
        }

        private static DependencyProperty CalendarDayBackgroundProperty = DependencyProperty.Register("CalendarDayBackground",
            typeof(SolidColorBrush), typeof(Calendar), new PropertyMetadata(Brushes.White));

        public SolidColorBrush CalendarDayBackground
        {
            get { return (SolidColorBrush)GetValue(CalendarDayBackgroundProperty); }
            set { SetValue(CalendarDayBackgroundProperty, value); }
        }

        private static DependencyProperty CalendarDayForegroundProperty = DependencyProperty.Register("CalendarDayForeground",
            typeof(SolidColorBrush), typeof(Calendar), new PropertyMetadata(Brushes.Black));

        public SolidColorBrush CalendarDayForeground
        {
            get { return (SolidColorBrush)GetValue(CalendarDayForegroundProperty); }
            set { SetValue(CalendarDayForegroundProperty, value); }
        }

        private static DependencyProperty WeekdayForegroundProperty = DependencyProperty.Register("WeekdayForeground",
            typeof(SolidColorBrush), typeof(Calendar), new PropertyMetadata(Brushes.White));

        public SolidColorBrush WeekdayForeground
        {
            get { return (SolidColorBrush)GetValue(WeekdayForegroundProperty); }
            set { SetValue(WeekdayForegroundProperty, value); }
        }

        private static DependencyProperty WeekdayBackgroundProperty = DependencyProperty.Register("WeekdayBackground",
            typeof(SolidColorBrush), typeof(Calendar), new PropertyMetadata(Brushes.Transparent));

        public SolidColorBrush WeekdayBackground
        {
            get { return (SolidColorBrush)GetValue(WeekdayBackgroundProperty); }
            set { SetValue(WeekdayBackgroundProperty, value); }
        }

        private static DependencyProperty PrevNextMonthDayForegroundProperty = DependencyProperty.Register("PrevNextMonthDayForeground",
            typeof(SolidColorBrush), typeof(Calendar), new PropertyMetadata(Brushes.Transparent));

        public SolidColorBrush PrevNextMonthDayForeground
        {
            get { return (SolidColorBrush)GetValue(PrevNextMonthDayForegroundProperty); }
            set { SetValue(PrevNextMonthDayForegroundProperty, value); }
        }

        private static DependencyProperty WeekendBackgroundProperty = DependencyProperty.Register("WeekendBackground",
            typeof(SolidColorBrush), typeof(Calendar));

        public SolidColorBrush WeekendBackground
        {
            get { return (SolidColorBrush)GetValue(WeekendBackgroundProperty); }
            set { SetValue(WeekendBackgroundProperty, value); }
        }

        private static DependencyProperty WeekendForegroundProperty = DependencyProperty.Register("WeekendForeground",
            typeof(SolidColorBrush), typeof(Calendar));

        public SolidColorBrush WeekendForeground
        {
            get { return (SolidColorBrush)GetValue(WeekendForegroundProperty); }
            set { SetValue(WeekendForegroundProperty, value); }
        }

        private CalendarHelper.CalendarStyle _calendarStyle
        {
            get
            {
                return new CalendarHelper.CalendarStyle()
                {
                    TodayBackground = this.TodayBackground,
                    TodayForeground = this.TodayForeground,
                    WeekendBackground = this.WeekendBackground,
                    WeekendForeground = this.WeekendForeground,
                    CalendarDayBackground = this.CalendarDayBackground,
                    CalendarDayForeground = this.CalendarDayForeground,
                    WeekdayForeground = this.WeekdayForeground,
                    WeekdayLabelBackground = this.WeekdayBackground,
                    PrevNextMonthDayForeground = this.PrevNextMonthDayForeground,
                    ActiveGlow = this.ActiveGlow,
                    HoverGlow = this.HoverGlow,
                };
            }
        }


        private static DependencyProperty OnSelectProperty = DependencyProperty.Register("OnSelect",
            typeof(Action<object?>), typeof(Calendar), new PropertyMetadata(null));

        public Action<object?> OnSelect
        {
            get { return (Action<object?>)GetValue(OnSelectProperty); }
            set { SetValue(OnSelectProperty, value); }
        }

        private static DependencyProperty YearMonthChangeCallbakProperty = DependencyProperty.Register("YearMonthChangeCallbak",
            typeof(Func<int, int, List<CalendarButtonLabel>>), typeof(Calendar), new PropertyMetadata(null));

        public Func<int, int, List<CalendarButtonLabel>> YearMonthChangeCallbak
        {
            get { return (Func<int, int, List<CalendarButtonLabel>>)GetValue(YearMonthChangeCallbakProperty); }
            set { SetValue(YearMonthChangeCallbakProperty, value); }
        }

        private CalendarHelper.CalendarActions _calendarActions
        {
            get
            {
                return new CalendarHelper.CalendarActions()
                {
                    OnClick = OnSelect,
                    YearMonthChangeCallbak = YearMonthChangeCallbak,
                };
            }
        }


        private Style SetTriggers(Style? style)
        {
            if (style == null)
                style = new Style();

            var mouseOverTrigger = new Trigger()
            {
                Property = CalendarButton.IsMouseOverProperty,
                Value = true
            };

            Binding hoverGlowBinding = new Binding("HoverGlow") {
                Source = this
            };
            Binding activeGlowBinding = new Binding("ActiveGlow") {
                Source =this
            };
            var mouseOvereffect =  new DropShadowEffect()
            {
                ShadowDepth = 0,
                Opacity = 1,
                BlurRadius = 50,
            };

            BindingOperations.SetBinding(mouseOvereffect,DropShadowEffect.ColorProperty, hoverGlowBinding);

            var activeDateEffect = new DropShadowEffect()
            {
                ShadowDepth = 0,
                Opacity = 1,
                BlurRadius = 50,
            };

            BindingOperations.SetBinding(activeDateEffect, DropShadowEffect.ColorProperty, activeGlowBinding);

            mouseOverTrigger.Setters.Add(new Setter(Panel.ZIndexProperty, 2));
            mouseOverTrigger.Setters.Add(new Setter(CalendarButton.EffectProperty, mouseOvereffect));

            var dataTrigger = new DataTrigger() {
                Value = true
            };

            MultiBinding multiBinding = new MultiBinding();
            multiBinding.Converter = _equalityConverter;

            multiBinding.Bindings.Add(new Binding("ActiveDate"));
            multiBinding.Bindings.Add(new Binding("CalendarDate") { RelativeSource = new RelativeSource(RelativeSourceMode.Self) });

            dataTrigger.Binding = multiBinding;
            dataTrigger.Setters.Add(new Setter(Panel.ZIndexProperty, 2));
            dataTrigger.Setters.Add(new Setter(CalendarButton.EffectProperty, activeDateEffect));

            style.Triggers.Add(mouseOverTrigger);
            style.Triggers.Add(dataTrigger);

            return style;
        }

        public Calendar()
        {
            InitializeComponent();
            _helper = new CalendarHelper();
            _months = _helper.GetMonths();

            object? cbtn = null;
            try {
                cbtn = FindResource("cbtn");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
            _btnStyle = SetTriggers(cbtn as Style);

            for (int i = -50; i < 50; i++)
                YearCombobox.Items.Add(DateTime.Today.AddYears(i).Year);
            YearCombobox.SelectedItem = SelectedYear;

            for (int i = 1; i <= 12; i++)
            {
                ComboBoxItem cbi = new ComboBoxItem();
                cbi.Name = "month" + i;
                cbi.Content = _months.FirstOrDefault(x => x.Value == i).Key;
                MonthCombobox.Items.Add(cbi);
            }

            MonthCombobox.SelectedIndex = _months.FirstOrDefault(x => x.Value == SelectedMonth).Value - 1;
        }

        private void PrevButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedMonth--;
            if(SelectedMonth < 1)
            {
                SelectedMonth = 12;
                SelectedYear--;
                YearCombobox.SelectedItem = SelectedYear;
            }

            MonthCombobox.SelectedIndex = _months.FirstOrDefault(x => x.Value == SelectedMonth).Value - 1;

            _helper.LoadCalendar(this, SelectedYear, SelectedMonth, _btnStyle, _calendarActions, _calendarStyle);
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedMonth++;
            if (SelectedMonth > 12)
            {
                SelectedMonth = 1;
                SelectedYear++;
                YearCombobox.SelectedItem = SelectedYear;
            }
            MonthCombobox.SelectedIndex = _months.FirstOrDefault(x => x.Value == SelectedMonth).Value - 1;

            _helper.LoadCalendar(this, SelectedYear, SelectedMonth, _btnStyle, _calendarActions, _calendarStyle);
        }

        private void YearCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cb = (ComboBox)sender;

            if (cb.SelectedIndex >= 0)
            {
                var item = cb.SelectedItem.ToString();
                if(item != null)
                {
                    var year = Int32.Parse(item);
                    SelectedYear = year;
                }
            }


            _helper.LoadCalendar(this, SelectedYear, SelectedMonth, _btnStyle, _calendarActions, _calendarStyle);
        }

        private void MonthCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cb = (ComboBox)sender;

            if (cb.SelectedIndex >= 0)
            {
                int month = cb.SelectedIndex + 1;
                SelectedMonth = month;
            }

            _helper.LoadCalendar(this, SelectedYear, SelectedMonth, _btnStyle, _calendarActions, _calendarStyle);
        }

        private void ToTodayButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedMonth = DateTime.Now.Month;
            SelectedYear = DateTime.Now.Year;

            YearCombobox.SelectedItem = SelectedYear;
            MonthCombobox.SelectedIndex = _months.FirstOrDefault(x => x.Value == SelectedMonth).Value - 1;

            _helper.LoadCalendar(this, SelectedYear, SelectedMonth, _btnStyle, _calendarActions, _calendarStyle);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _helper.LoadCalendar(this, SelectedYear, SelectedMonth, _btnStyle, _calendarActions, _calendarStyle);
        }

        public void Reload()
        {
            _helper.LoadCalendar(this, SelectedYear, SelectedMonth, _btnStyle, _calendarActions, _calendarStyle);
        }
    }
}
