using System;
using System.Windows.Input;
using Okra.Core;
using Okra.Navigation;
using Windows.System;

namespace ExXAMLate.ViewModels
{
    public class ChecklistViewModel : NotifyPropertyChangedBase
    {
        private readonly INavigationManager _navigation;

        public ICommand GoBack { get; set; }
        public ICommand GoToAppX { get; set; }
        public ICommand GoToRequirements { get; set; }
        public ChecklistViewModel(INavigationManager  navigation)
        {
            _navigation = navigation;

            GoBack = new DelegateCommand(() => _navigation.GoBack());
            GoToAppX = new DelegateCommand(() => _navigation.NavigateTo("AppX"));
            GoToRequirements = new DelegateCommand(() => Launcher.LaunchUriAsync(new Uri("http://msdn.microsoft.com/en-us/library/windows/apps/hh694083.aspx", UriKind.Absolute)));
        }
    }
}
