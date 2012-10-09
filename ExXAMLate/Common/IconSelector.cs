using ExXAMLate.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ExXAMLate.Common
{
    public class IconSelector : DataTemplateSelector
    {
        public DataTemplate TileTemplate { get; set; }
        public DataTemplate IconTemplate { get; set; }

        public DataTemplate SplashTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (item is AppXTile)
                return TileTemplate;
            if (item is AppXSplash)
                return SplashTemplate;

            return IconTemplate;
        }
    }
}