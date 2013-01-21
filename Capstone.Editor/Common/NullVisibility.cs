﻿using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Capstone.Editor.Common
{
    public class NullVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value != null ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}