using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace ExXAMLate.Common
{
    public class ButtonStyleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return App.Current.Resources[(string) value] as Style;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}