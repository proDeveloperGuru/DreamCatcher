using DreamCatcher.Commands;
using DreamCatcher.Core.Servicees.DreamServiss;
using DreamCatcher.Dialogs;
using DreamCatcher.Extensions;
using DreamCatcher.ViewModel;
using MaterialDesignThemes.Wpf;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace DreamCatcher.Pages
{
    /// <summary>
    /// Interaction logic for DaySummary.xaml
    /// </summary>
    public partial class DaySummary : Page
    {
        IDreamServiss _serviss;
        public DaySummary(DateTime datetime, IDreamServiss serviss)
        {
            _serviss = serviss;
            InitializeComponent();

            var dreams = _serviss.GetDreamsByDate(datetime).Select(x => new DreamViewModel()
            {
                Dream = x,
                EditCommand = new BasicCommand(EditDream),
                DeleteCommand = new BasicCommand(DeleteDreamAsync),
            }).ToList();

            DataContext = new DaySymmaryViewModel()
            {
                Date = datetime,
                Dreams = new ExtendedObservableCollection<DreamViewModel>(dreams)
            };
        }

        private async void AddDream_Click(object sender, RoutedEventArgs e)
        {
            var model = (DaySymmaryViewModel)DataContext;
            var wnd = (MainWindow)Window.GetWindow(this);
            var editDialog = new CreateEditDreamDialog(model.Date);
            var result = await DialogHost.Show(editDialog, "RootDialog", (object obj, DialogClosingEventArgs eventArgs) =>
            {
                if (eventArgs.Parameter is bool r && r == true)
                {
                    var viewModel = editDialog.DataContext as DreamSummaryViewModel;
                    if (viewModel != null)
                    {
                        if (viewModel.RequiredFieldsFilled)
                        {
                            var dream = viewModel.ToModel();
                            dream.Picture = editDialog.Image;

                            _serviss.WriteDownDream(dream);
                            if(model.Dreams != null)
                            {
                                model.Dreams.Add(new DreamViewModel()
                                {
                                    Dream = dream,
                                    DeleteCommand = new BasicCommand(DeleteDreamAsync),
                                    EditCommand = new BasicCommand(EditDream)
                                });
                            }

                            wnd.CalendarGrid.Reload();
                        }
                        else
                        {
                            eventArgs.Cancel();
                            viewModel.HasErrors = true;
                            viewModel.InvokeErrorChange();
                        }

                    }
                }
            });
        }

        public async void DeleteDreamAsync(object? obj)
        {
            if (obj == null)
                return;

            Guid id = (Guid)obj;
            var dream = _serviss.GetById(id);

            if (dream == null)
                return;

            var wnd = (MainWindow)Window.GetWindow(this);
            var model = (DaySymmaryViewModel)DataContext;
            var confirmDialog = new ConfirmationDialog();
            confirmDialog.Header = "Delete dream";
            confirmDialog.Message = string.Format("Are you sure you want to delete the dream '{0}'?",dream.Title);
            var result = await DialogHost.Show(confirmDialog, "RootDialog");

            if(result is bool r && r == true)
            {
                _serviss.DeleteDream(id);
                if (model.Dreams != null)
                {
                    var m = model.Dreams.Where(x => x.Id == id).FirstOrDefault();
                    if (m != null)
                        model.Dreams.Remove(m);
                }

                wnd.CalendarGrid.Reload();
            }
        }

        public async void EditDream(object? obj)
        {
            if (obj == null)
                return;

            Guid id = (Guid)obj;
            var dream = _serviss.GetById(id);

            if (dream == null)
                return;

            var wnd = (MainWindow)Window.GetWindow(this);
            var model = (DaySymmaryViewModel)DataContext;
            var editDialog = new CreateEditDreamDialog(dream);
            var result = await DialogHost.Show(editDialog, "RootDialog", (object obj, DialogClosingEventArgs eventArgs) =>
            {
                if (eventArgs.Parameter is bool r && r == true)
                {
                    var viewModel = editDialog.DataContext as DreamSummaryViewModel;
                    if (viewModel != null)
                    {
                        var newDreamValues = viewModel.ToModel();
                        newDreamValues.Picture = editDialog.Image;
                        _serviss.UpdateDream(id, newDreamValues);

                        if(model.Dreams != null)
                        {
                            model.Dreams.Update(new DreamViewModel()
                            {
                                Dream = dream,
                                DeleteCommand = new BasicCommand(DeleteDreamAsync),
                                EditCommand = new BasicCommand(EditDream)
                            });
                        }

                        wnd.CalendarGrid.Reload();
                    }
                }
            });
        }
    }
}
