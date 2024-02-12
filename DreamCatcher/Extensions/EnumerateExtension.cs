using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace DreamCatcher.Extensions
{
    public sealed class EnumerateExtension : MarkupExtension
    {
        public Type Type { get; set; }

        public IValueConverter Converter { get; set; }

        public CultureInfo ConverterCulture { get; set; }

        public object ConverterParameter { get; set; }

        public EnumerateExtension(Type type)
        {
            if (type == null)
            { throw new ArgumentNullException("The EnumType property is not specified!"); }

            this.Type = type;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Type == null)
            { throw new InvalidOperationException("Type no set"); }

            Type actualType = Nullable.GetUnderlyingType(Type) ?? Type;
            TypeConverter typeConverter;
            ICollection standardValues;

            if ((typeConverter = TypeDescriptor.GetConverter(actualType)) == null ||
                (standardValues = typeConverter.GetStandardValues(serviceProvider as ITypeDescriptorContext)) == null)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "The type '{0}' has no standard values!", Type), "value");
            }

            object[] items = (Type == actualType)
                ? new object[standardValues.Count]
                : new object[standardValues.Count + 1];
            int index = 0;

            if (Converter == null)
            {
                foreach (object standardValue in standardValues)
                { items[index++] = standardValue; }
            }
            else
            {
                CultureInfo culture = ConverterCulture ?? GetCulture(serviceProvider);

                foreach (object standardValue in standardValues)
                { items[index++] = Converter.Convert(standardValue, typeof(object), ConverterParameter, culture); }

                if (Type != actualType)
                { items[index] = Converter.Convert(null, typeof(object), ConverterParameter, culture); }
            }

            return items;
        }

        private static CultureInfo GetCulture(IServiceProvider serviceProvider)
        {
            if (serviceProvider != null)
            {
                IProvideValueTarget provideValueTarget = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;

                if (provideValueTarget != null)
                {
                    DependencyObject targetObject = provideValueTarget.TargetObject as DependencyObject;
                    XmlLanguage language;

                    if ((targetObject = provideValueTarget.TargetObject as DependencyObject) != null &&
                        (language = (XmlLanguage)targetObject.GetValue(FrameworkElement.LanguageProperty)) != null)
                    { return language.GetSpecificCulture(); }
                }
            }

            return null;
        }
    }
}
