using Capstone.Editor.Data;
using System;
using System.Reflection;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Capstone.Editor.Common
{
    public class ComponentPropertyTemplateSelector : DataTemplateSelector
    {
        public DataTemplate StringTemplate { get; set; }
        public DataTemplate BoolTemplate { get; set; }
        public DataTemplate NumericTemplate { get; set; }
        public DataTemplate EnumTemplate { get; set; }

        protected override Windows.UI.Xaml.DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (item is ComponentProperty && item != null)
            {
                var prop = (ComponentProperty)item;
                if (prop.DataType == typeof(string))
                    return StringTemplate;
                else if (prop.DataType == typeof(bool))
                    return BoolTemplate;
                else if (prop.DataType == typeof(int) || prop.DataType == typeof(float) ||
                         prop.DataType == typeof(double))
                    return NumericTemplate;
                else if (prop.DataType.GetRuntimeMethod("HasFlag", new[] { typeof(Enum) }) != null)
                    return EnumTemplate;
            }
            return base.SelectTemplateCore(item, container);
        }
    }
}
