using ExXAMLate.Models;
using ExXAMLate.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ExXAMLate.Common
{
    public class IconSelector : DataTemplateSelector
    {
        public DataTemplate TileTemplate { get; set; }
        public DataTemplate IconTemplate { get; set; }
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (item is AppXTile)
                return TileTemplate;
            return IconTemplate;
        }
    }
}