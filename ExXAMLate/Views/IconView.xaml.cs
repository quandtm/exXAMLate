using ExXAMLate.ViewModels;

namespace ExXAMLate.Views
{
    public sealed partial class IconView
    {
        public IconView()
        {
            InitializeComponent();
            Loaded += IconView_Loaded;
        }

        void IconView_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ((IconViewModel) DataContext).Load();
        }
    }
}
