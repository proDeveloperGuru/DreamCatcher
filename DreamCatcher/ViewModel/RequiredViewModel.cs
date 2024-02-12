using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DreamCatcher.ViewModel
{
    public class RequiredViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        private Dictionary<string, string> RequiredValueErrors
        {
            get
            {
                var type = this.GetType();
                var requiredProperties = TypeDescriptor.GetProperties(type)
                    .Cast<PropertyDescriptor>()
                    .Where(p => p.Attributes.Cast<Attribute>().Any(a => a.GetType() == typeof(RequiredAttribute))).ToList();

                var dictionary = new Dictionary<string, string>();

                foreach (var property in requiredProperties)
                {
                    if (property.PropertyType.Name == "String")
                    {
                        var value = property.GetValue(this);
                        if (value == null || value.ToString() == "")
                            dictionary.Add(property.Name, "Obligāts!");
                    }
                    else
                    {
                        var value = property.GetValue(this);
                        if (value == null)
                            dictionary.Add(property.Name, "Obligāts!");
                    }
                }

                return dictionary;
            }
        }

        public bool RequiredFieldsFilled
        {
            get
            {
                return !this.RequiredValueErrors.Any();
            }
        }

        public bool HasErrors { get; set; } = false;

        public IEnumerable? GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName) || (!HasErrors))
                return null;

            if (RequiredValueErrors.ContainsKey(propertyName))
                return new List<string>() { RequiredValueErrors[propertyName] };

            return new List<string>();
        }


        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public bool InvokeErrorChange()
        {
            if (!this.RequiredFieldsFilled)
            {
                foreach (var error in this.RequiredValueErrors)
                    ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(error.Key));
            }
            else
            {
                return true;
            }

            return false;
        }
    }
}
