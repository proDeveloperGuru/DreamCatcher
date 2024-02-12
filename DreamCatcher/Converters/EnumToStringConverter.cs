using DreamCatcher.Properties;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;

namespace DreamCatcher.Converters
{
    [ValueConversion(typeof(Enum), typeof(string))]
    public class EnumToStringConverter : IValueConverter
    {
        #region IValueConverter Members

        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            { return null; }

            CheckSourceType(typeof(Enum), value);
            CheckTargetType(typeof(string), targetType, true);

            Type valueType = value.GetType();
            FieldInfo fieldInfo = valueType.GetField(value.ToString(), BindingFlags.Static | BindingFlags.Public);

            if (fieldInfo == null)
            { throw new ArgumentException("Bit fields are not supported!", "value"); }

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes.Length > 0)
            { return Resources.ResourceManager.GetString(attributes[0].Description); }
            else
            { return fieldInfo.Name; }
        }

        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            { return null; }

            CheckSourceType(typeof(string), value);
            CheckTargetType(typeof(Enum), targetType, false);

            string str = (string)value;

            foreach (FieldInfo fieldInfo in targetType.GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                if (fieldInfo.Name == str)
                { return fieldInfo.GetValue(null); }

                DescriptionAttribute[] attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

                foreach (DescriptionAttribute attribute in attributes)
                {
                    if (Resources.ResourceManager.GetString(attribute.Description) == str)
                    {
                        var enumValue = fieldInfo.GetValue(null);
                        return enumValue;
                    }
                }
            }

            throw new ArgumentException(string.Format("Requested value '{0}' was not found!", str), "value");
        }

        #endregion

        #region Private Methods

        private static void CheckSourceType(Type supportedSourceType, object value)
        {
            if (!supportedSourceType.IsInstanceOfType(value))
            { throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "Value must be of type '{0}'!", supportedSourceType.Name), "value"); }
        }

        private static void CheckTargetType(Type supportedTargeType, Type requestedTargetType, bool covariance)
        {
            string message = "Target type must extend '{0}'!";
            if (covariance)
            {
                if (!requestedTargetType.IsAssignableFrom(supportedTargeType))
                { throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, message, requestedTargetType.Name, supportedTargeType.Name), "targetType"); }
            }
            else
            {
                if (!supportedTargeType.IsAssignableFrom(requestedTargetType))
                { throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, message, requestedTargetType.Name, supportedTargeType.Name), "targetType"); }
            }
        }

        #endregion
    }
}
