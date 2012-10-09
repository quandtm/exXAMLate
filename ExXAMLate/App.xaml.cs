using Callisto.Controls;
using ExXAMLate.Views;
using Okra.Navigation;
using Windows.ApplicationModel.Activation;
using Windows.UI;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml.Media;

namespace ExXAMLate
{
    sealed partial class App
    {
        private SettingsFlyout _flyout;
        public INavigationManager NavigationManager { get; set; }
        public App()
        {
            InitializeComponent();
            var bootstrapper = new AppBootstrapper();
            bootstrapper.Initialize();
            //bootstrapper.SearchManager.SearchPageName = "Search";
            NavigationManager = bootstrapper.NavigationManager;
            bootstrapper.ActivationManager.Activated += ActivationManagerActivated;
        }

        void ActivationManagerActivated(object sender, IActivatedEventArgs e)
        {
            //var dataTransferManager = DataTransferManager.GetForCurrentView();
            //dataTransferManager.DataRequested += ShareRequested;

            SettingsPane.GetForCurrentView().CommandsRequested += CommandsRequested;
        }

        private void CommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            args.Request.ApplicationCommands.Add(new SettingsCommand("about", "About", x =>
            {
                _flyout = new SettingsFlyout
                {
                    HeaderText = "About",
                    Content = new AboutView(),
                    IsOpen = true,
                    ContentBackgroundBrush = new SolidColorBrush(Colors.Black),
                    ContentForegroundBrush = new SolidColorBrush(Colors.White)
                };
            }));
            args.Request.ApplicationCommands.Add(new SettingsCommand("privacy", "Privacy", x =>
            {
                _flyout = new SettingsFlyout
                {
                    HeaderText = "Privacy",
                    Content = new PrivacyView(),
                    IsOpen = true,
                    ContentBackgroundBrush = new SolidColorBrush(Colors.Black),
                    ContentForegroundBrush = new SolidColorBrush(Colors.White)
                };

            }));
        }

    }
}
