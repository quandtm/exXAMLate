using System.Collections.ObjectModel;
using ExXAMLate.Models;
using Okra.Core;
using Okra.Navigation;

namespace ExXAMLate.ViewModels
{
    public class HomeViewModel
    {
        private readonly INavigationManager _navigation;
        public ObservableCollection<IGroup> Groups { get; set; }

        public HomeViewModel(INavigationManager navigation)
        {
            _navigation = navigation;
            var projects = new HomeGroup("Projects");
            projects.Items.Add(new HomeViewGroupItem { Title = "App Checklist" });
            projects.Items.Add(new HomeViewGroupItem { Title = "AppX Viewer", Clicked = new DelegateCommand(() => _navigation.NavigateTo("AppX"))});
            
            var resourceGroup = new HomeGroup("Resources");
            resourceGroup.Items.Add(new HomeViewGroupItem { Title = "Icons", Clicked = new DelegateCommand(GoToIconView) });
            resourceGroup.Items.Add(new HomeViewGroupItem { Title = "TextBlock Styles", Clicked = new DelegateCommand(GoToTextStyleView) });
            resourceGroup.Items.Add(new HomeViewGroupItem { Title = "Insta-XAML Customisation", Clicked = new DelegateCommand(() => _navigation.NavigateTo("HammerPants")) });

            Groups = new ObservableCollection<IGroup>
                {
                    projects,
                    resourceGroup
                };
        }

        public void GoToIconView()
        {
            _navigation.NavigateTo("Icon");
        }
        public void GoToTextStyleView()
        {
            _navigation.NavigateTo("TextStyle");
        }
    }
}
