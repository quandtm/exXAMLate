using System.Globalization;
using Windows.UI;

namespace ExXAMLate.Common
{
    public static class ColorExtensions
    {
        public static Color FromString(string hex)
        {
            byte a = 255;
            byte r = byte.Parse(hex.Substring(1, 2), NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(3, 2), NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(5, 2), NumberStyles.HexNumber);
            return Color.FromArgb(a, r, g, b);
        }
    }
}