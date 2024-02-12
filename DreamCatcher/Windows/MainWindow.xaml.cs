using DreamCatcher.Core.Servicees.DreamServiss;
using DreamCatcher.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using static WPFCustomControls.CalendarButton;

namespace DreamCatcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IDreamServiss _dreamServiss;
        public MainWindow(IDreamServiss dreamServiss)
        {
            _dreamServiss = dreamServiss;
            InitializeComponent();

            var now = DateTime.Now;
            var datetime = new DateTime(now.Year,now.Month,now.Day);

            this.CalendarGrid.ActiveDate = datetime;
            var page = new DaySummary(datetime,_dreamServiss);
            PageViewer.Navigate(page);
        }

        private List<CalendarButtonLabel> GetLabels(int year, int month)
        {
            return _dreamServiss.GetDreamsYearMonth(year, month)
                .Select(x => new CalendarButtonLabel() { Date = x.DateTime, Text = x.Title }).ToList();
        }

        private void OpenDay(object? obj)
        {
            if (obj != null)
            {
                var datetime = (DateTime)obj;

                this.CalendarGrid.ActiveDate = datetime;

                var page = new DaySummary(datetime, _dreamServiss);
                PageViewer.Navigate(page);
            }
        }
    }
}
