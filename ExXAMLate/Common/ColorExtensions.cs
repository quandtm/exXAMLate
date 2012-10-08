using Windows.UI;

namespace ExXAMLate.Common
{
    public static class ColorExtensions
    {
        public static Color FromString(string hex)
        {
            byte a = 255;
            byte r = byte.Parse(hex.Substring(1, 2));
            byte g = byte.Parse(hex.Substring(3, 2));
            byte b = byte.Parse(hex.Substring(5, 2));
            return Color.FromArgb(a, r, g, b);
        }
    }
}