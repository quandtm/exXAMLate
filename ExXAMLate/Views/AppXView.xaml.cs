using Windows.UI.Xaml;

namespace ExXAMLate.Views
{
    public sealed partial class AppXView
    {
        public AppXView()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Windows.UI.ViewManagement.ApplicationView.TryUnsnap();
        }
    }
}
