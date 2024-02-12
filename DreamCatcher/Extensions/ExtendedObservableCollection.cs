using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DreamCatcher.Extensions
{
    public interface IData
    {
        Guid? Id { get; }
    }

    public class ExtendedObservableCollection<T> : ObservableCollection<T> where T : class, IData
    {
        public ExtendedObservableCollection() : base()
        {

        }

        public ExtendedObservableCollection(IEnumerable<T> list) : base(list)
        {

        }

        public void Update(T entity)
        {
            if (entity.Id.HasValue)
            {
                var element = this.Where(x => x.Id == entity.Id).FirstOrDefault();
                if (element != null)
                {
                    var index = IndexOf(element);
                    this[index] = element;
                }
            }

            Refresh();
        }

        public void Refresh()
        {
            for (var i = 0; i < this.Count(); i++)
            {
                this.OnCollectionChanged(new System.Collections.Specialized.NotifyCollectionChangedEventArgs(System.Collections.Specialized.NotifyCollectionChangedAction.Reset));
            }
        }
    }
}
