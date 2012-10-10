using Windows.UI.Xaml.Media;

namespace ExXAMLate.Models
{
    public class AppXModel 
    {
        public Brush Background { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string ForegroundText { get; set; }
        public bool ShouldChangeForeground { get; set; }
    }
}