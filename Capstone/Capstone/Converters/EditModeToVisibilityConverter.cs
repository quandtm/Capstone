using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Capstone.Converters
{
    public class EditModeToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var p = parameter as string;
            var expected = (EditMode)Enum.Parse(typeof(EditMode), p);
            return expected == (EditMode)value ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
