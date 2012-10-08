using System.Collections.ObjectModel;
using System.Windows.Input;
using ExXAMLate.Common;
using ExXAMLate.Models;
using Okra.Core;
using Okra.Navigation;

namespace ExXAMLate.ViewModels
{
    [System.Composition.Shared]
    public class IconViewModel : NotifyPropertyChangedBase
    {
        private readonly INavigationManager _navigation;
        public ObservableCollection<IGroup> Groups { get; set; }

        public ICommand GoBack { get; set; }
        public IconViewModel(INavigationManager navigation)
        {
            _navigation = navigation;
            GoBack = new DelegateCommand(() => _navigation.GoBack());

            var bg = new ButtonGroup("Buttons")
                {
                    Items = new ObservableCollection<string>()
                };

            var items = ResourceFileParser.GetKeysBasedOn(@"Common\StandardStyles.xaml", "AppBarButtonStyle");
            foreach (var i in items)
                bg.Items.Add(i);

            Groups = new ObservableCollection<IGroup> { new ButtonAnatomyGroup(), bg };
        }
    }
}
