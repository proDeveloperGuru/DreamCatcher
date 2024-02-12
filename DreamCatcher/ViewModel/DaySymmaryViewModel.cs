using DreamCatcher.Core.Models;
using DreamCatcher.Extensions;
using System;
using System.Windows.Input;

namespace DreamCatcher.ViewModel
{
    public class DaySymmaryViewModel
    {
        public DateTime Date { get; set; }

        public ExtendedObservableCollection<DreamViewModel>? Dreams { get; set; }
    }

    public class DreamViewModel : IData
    {
        public Dream Dream { get; set; } = null!;

        public bool HasPicture
        {
            get { return Dream.Picture != null && Dream.Picture.Length > 0; }
        }

        public ICommand? DeleteCommand { get; set; }

        public ICommand? EditCommand { get; set; }

        public Guid? Id { get { return Dream != null ? Dream.Id : null; } }
    }
}
