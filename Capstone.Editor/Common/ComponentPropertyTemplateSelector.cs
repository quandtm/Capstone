using Capstone.Editor.Data;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Capstone.Editor.Common
{
    public class ComponentPropertyTemplateSelector : DataTemplateSelector
    {
        public DataTemplate StringTemplate { get; set; }
        public DataTemplate BoolTemplate { get; set; }

        protected override Windows.UI.Xaml.DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (item is ComponentProperty && item != null)
            {
                var prop = (ComponentProperty)item;
                if (prop.DataType == typeof(string))
                    return StringTemplate;
                else if (prop.DataType == typeof(bool))
                    return BoolTemplate;
            }
            return base.SelectTemplateCore(item, container);
        }
    }
}
