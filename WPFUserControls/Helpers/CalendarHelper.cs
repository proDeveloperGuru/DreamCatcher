using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WPFCustomControls;
using WPFUserControls.Comands;
using static WPFCustomControls.CalendarButton;
using Color = System.Windows.Media.Color;

namespace WPFUserControls.Helpers
{
    public class CalendarHelper
    {
        private List<Button>? _buttons;
        private DayOfWeek _firstDayOfTheMonth;
        private DayOfWeek _lastDayOfTheMonth;
        private int _index;
        private List<CalendarButtonLabel> _labels;

        public class CalendarStyle
        {
            public SolidColorBrush TodayBackground { get; set; } = Brushes.DeepSkyBlue;

            public SolidColorBrush TodayForeground { get; set; } = Brushes.White;

            public SolidColorBrush CalendarDayBackground { get; set; } = Brushes.White;

            public SolidColorBrush CalendarDayForeground { get; set; } = Brushes.Black;

            public SolidColorBrush PrevNextMonthDayForeground { get; set; } = Brushes.Black;

            public SolidColorBrush WeekdayForeground { get; set; } = Brushes.White;

            public SolidColorBrush WeekdayLabelBackground { get; set; } = Brushes.Transparent;

            public SolidColorBrush WeekendBackground { get; set; } = Brushes.Blue;

            public SolidColorBrush WeekendForeground { get; set; } = Brushes.Red;
        }

        public class CalendarActions
        {
            public Action<object?>? OnClick { get; set; } = null;

            public Func<int, int, List<CalendarButtonLabel>>?  YearMonthChangeCallbak { get; set; } = null;
        }

        public Dictionary<string, int> GetMonths()
        {
            Dictionary<string, int> months = new Dictionary<string, int>
            {
                { "January", 1 },
                { "February", 2 },
                { "March", 3 },
                { "April", 4 },
                { "May", 5 },
                { "June", 6 },
                { "July", 7 },
                { "August", 8 },
                { "September", 9 },
                { "October", 10 },
                { "November", 11 },
                { "December", 12 }
            };

            return months;
        }

        public void LoadCalendar(Grid grid, int year, int month, Style btnStyle, CalendarActions? actions = null, CalendarStyle? style = null)
        {
            if (actions != null) {
                if(actions.YearMonthChangeCallbak != null)
                {
                    _labels = actions.YearMonthChangeCallbak(year, month);
                }
            }

            _buttons = new List<Button>();
            ClearGrid(grid);

            SetLabels(grid, style);

            _index = 1;
            _firstDayOfTheMonth = GetFirstDay(year, month);
            _lastDayOfTheMonth = GetLastDay(year, month);

            PrevMonthDays(year, month, btnStyle, actions, style);
            _index++;

            CurMonthDays(year, month, btnStyle, actions, style);
            _index++;

            NextMonthDays(year, month, btnStyle, actions, style);

            for(var i = 0; i < _index/7; i++)
                grid.RowDefinitions.Add(new RowDefinition());

            SetButtons(grid);
        }

        private void SetButtons(Grid grid)
        {
            var c = 0;
            var r = 1;

            if(_buttons != null)
            {
                foreach (var b in _buttons)
                {
                    if (c > 6)
                    {
                        c = 0;
                        r++;
                    }

                    b.SetValue(Grid.ColumnProperty, c++);

                    b.SetValue(Grid.RowProperty, r);
                    grid.Children.Add(b);
                }
            }
        }

        private void SetLabels(Grid grid, CalendarStyle? style = null)
        {
            string[] weekDays = new string[] { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(30)});
            for (var i = 0; i <= 6; i++)
            {
                var label = new Label()
                {
                    Foreground = style != null ? style.WeekdayForeground : Brushes.White,
                    Background = style != null ? style.WeekdayLabelBackground : Brushes.Transparent,
                    Content = weekDays[i],
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(1),
                };
                
                label.SetValue(Grid.ColumnProperty, i);
                label.SetValue(Grid.RowProperty, 0);
                grid.Children.Add(label); ;
            }
                
        }

        //get day of the week of the first day of the month
        private DayOfWeek GetFirstDay(int year, int month)
        {
            DateTime date = new DateTime(year, month, 1);

            return date.DayOfWeek;
        }

        //get day of the week of the last day of the month
        private DayOfWeek GetLastDay(int year, int month)
        {
            var lastDay = DateTime.DaysInMonth(year, month);
            DateTime date = new DateTime(year, month, lastDay);

            return date.DayOfWeek;
        }

        private bool CheckIfIsWeekend(int year, int month, int day)
        {
            var date = new DateTime(year, month, day);
            return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
        }

        public int GetDayIndex(DayOfWeek day)
        {
            switch(day){
                case DayOfWeek.Sunday: return 6;
                case DayOfWeek.Monday: return 0;
                case DayOfWeek.Tuesday: return 1;
                case DayOfWeek.Wednesday: return 2;
                case DayOfWeek.Thursday: return 3;
                case DayOfWeek.Friday: return 4;
                case DayOfWeek.Saturday: return 5;
                default: return 0;         
            }
        }

        //generate prev month days
        private void PrevMonthDays(int year, int month, Style btnStyle, CalendarActions? actions = null,  CalendarStyle? style = null)
        {
            int prevYear = year;
            int prevMonth = month - 1;
            if (prevMonth == 0)
            {
                prevMonth = 12;
                prevYear--;
            }
                
            var lastDay = DateTime.DaysInMonth(prevYear, prevMonth);
            var startLastMonthDay = lastDay - GetDayIndex(_firstDayOfTheMonth) + 1;

            if (_buttons != null)
            {
                for (int i = startLastMonthDay; i <= lastDay; i++)
                {
                    var button = new CalendarButton()
                    {
                        Style = btnStyle,
                        CalendarDate = new DateTime(prevYear, prevMonth, i),
                        Background = style != null ?
                            style.WeekendBackground != null ?
                                CheckIfIsWeekend(prevYear, prevMonth, i) ? Darken(style.WeekendBackground, 0.75f) : Darken(style.CalendarDayBackground, 0.75f)
                            : Darken(style.CalendarDayBackground, 0.75f) : Brushes.LightGray,
                        Foreground = style != null ?
                            style.WeekendForeground != null ?
                                CheckIfIsWeekend(prevYear, prevMonth, i) ? style.WeekendForeground : style.PrevNextMonthDayForeground
                            : style.PrevNextMonthDayForeground ?? style.CalendarDayForeground : Brushes.Black,
                    };

                    if (actions != null && actions.OnClick != null)
                    {
                        button.Command = new CalendarCommand(actions.OnClick);
                        button.CommandParameter = new DateTime(prevYear, prevMonth, i);
                    }


                    _buttons.Add(button);

                    _index++;
                }
            }
        }

        //generate days of current month
        private void CurMonthDays(int year, int month, Style btnStyle, CalendarActions? actions = null, CalendarStyle? style = null)
        {
            DateTime todayDate = DateTime.Now;
            int days = DateTime.DaysInMonth(year, month);

            if(_buttons != null)
            {
                for (int i = 1; i <= days; i++)
                {
                    var date = new DateTime(year, month, i);
                    var button = new CalendarButton() {
                        Style = btnStyle,
                        CalendarDate = date,
                        CalendarLabels = _labels != null ? 
                            _labels.Where(x => x.Date.Year == year && x.Date.Month == month && x.Date.Day == i).ToList() : new List<CalendarButtonLabel>()
                    };

                    var isWeekend = CheckIfIsWeekend(year, month, i);

                    if (month == todayDate.Month && year == todayDate.Year
                        && i == todayDate.Day)
                    {
                        button.Foreground = style != null ?
                                style.TodayForeground == null ?
                                    isWeekend ?
                                        style.WeekendForeground != null ? style.WeekendForeground : style.CalendarDayForeground
                                    : style.CalendarDayForeground
                                : style.TodayForeground
                            : Brushes.White;
                        button.Background = style != null ? 
                                style.TodayBackground == null ?
                                    isWeekend ? 
                                        style.WeekendBackground != null ? style.WeekendBackground : style.CalendarDayBackground
                                    : style.CalendarDayBackground 
                                : style.TodayBackground
                            : Brushes.DeepSkyBlue;
                    }
                    else
                    {
                        button.Background = style != null ?
                            style.WeekendBackground != null ?
                                isWeekend ? style.WeekendBackground :style.CalendarDayBackground
                            : style.CalendarDayBackground : Brushes.White;
                        button.Foreground = style != null ?
                            style.WeekendForeground != null ?
                                isWeekend ? style.WeekendForeground : style.CalendarDayForeground
                            : style.CalendarDayForeground : Brushes.Black;
                    }

                    if (actions != null && actions.OnClick != null)
                    {
                        button.Command = new CalendarCommand(actions.OnClick);
                        button.CommandParameter = date;
                    }
                        

                    _buttons.Add(button);
                    _index++;
                }
            }
        }

        //generate next month days
        private void NextMonthDays(int year, int month, Style btnStyle, CalendarActions? actions = null, CalendarStyle? style = null)
        {
            int dayNum = 1;
            var nxtY = year;
            var nxtM = month + 1;

            if(nxtM > 12)
            {
                nxtM = 1;
                nxtY++;
            }

            if(_buttons != null)
            {
                for (int i = GetDayIndex(_lastDayOfTheMonth); i < 6; i++)
                {
                    var button = new CalendarButton()
                    {
                        Style = btnStyle,
                        CalendarDate = new DateTime(nxtY, nxtM, dayNum),
                        Background = style != null ?
                            style.WeekendBackground != null ?
                                CheckIfIsWeekend(nxtY, nxtM, dayNum) ? Darken(style.WeekendBackground, 0.75f) : Darken(style.CalendarDayBackground, 0.75f)
                            : Darken(style.CalendarDayBackground, 0.75f) : Brushes.LightGray,
                        Foreground = style != null ? 
                            style.WeekendForeground != null ?
                                CheckIfIsWeekend(nxtY, nxtM, dayNum) ? style.WeekendForeground : style.PrevNextMonthDayForeground
                        : style.PrevNextMonthDayForeground ?? style.CalendarDayForeground : Brushes.Black,
                    };

                    if (actions != null && actions.OnClick != null)
                    {
                        button.Command = new CalendarCommand(actions.OnClick);
                        button.CommandParameter = new DateTime(nxtY, nxtM, dayNum);
                    }

                    _buttons.Add(button);

                    dayNum++;
                    _index++;
                }
            }
        }

        public SolidColorBrush Darken(SolidColorBrush brush, float level)
        {
            var c = brush.Color;

            var dark = Color.FromArgb(c.A,
              (byte)(c.R * level), (byte)(c.G * level), (byte)(c.B * level));

            return new SolidColorBrush(dark);
        }

        private void ClearGrid(Grid grid)
        {
            grid.Children.Clear();
            grid.RowDefinitions.Clear();
        }
    }
}
