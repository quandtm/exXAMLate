using Windows.UI.Xaml;

namespace ExXAMLate.Views
{
    public sealed partial class HammerPantsView
    {
        public HammerPantsView()
        {
            this.InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Windows.UI.ViewManagement.ApplicationView.TryUnsnap();

        }
    }
}
