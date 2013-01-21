using System;
using Windows.UI.Xaml.Data;

namespace Capstone.Editor.Common
{
    public class IntBoolean : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value != null && value is int)
                return (int)value > 0;
            else
                return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
