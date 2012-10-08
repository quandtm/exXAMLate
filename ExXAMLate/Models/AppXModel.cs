using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace ExXAMLate.Models
{
    public class AppXModel 
    {
        public Brush Background { get; set; }
        public string DisplayName { get; set; }
        public string Logo { get; set; }
        public BitmapSource LogoImage { get; set; }
        public BitmapSource SmallLogoImage { get; set; }
        public string SmallLogo { get; set; }
        public string WideLogo { get; set; }
        public string StoreLogo { get; set; }
    }
}