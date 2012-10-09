using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using ExXAMLate.Common;
using ExXAMLate.Models;
using Okra.Core;
using Okra.Navigation;
using Windows.UI.Xaml;

namespace ExXAMLate.ViewModels
{
    [System.Composition.Shared]
    public class IconViewModel : NotifyPropertyChangedBase
    {
        private readonly INavigationManager _navigation;
        private ObservableCollection<IGroup> _groups;
        private readonly ButtonGroup buttonGroup;
        private DispatcherTimer t;
        public ObservableCollection<IGroup> Groups
        {
            get { return _groups; }
            set
            {
                _groups = value;
                OnPropertyChanged();
            }
        }

        public ICommand GoBack { get; set; }
        public IconViewModel(INavigationManager navigation)
        {
            _navigation = navigation;
            GoBack = new DelegateCommand(() => _navigation.GoBack());
            Groups = new ObservableCollection<IGroup>();
            buttonGroup = new ButtonGroup("Buttons")
                {
                    Items = new ObservableCollection<string>()
                };


            Groups.Add(new ButtonAnatomyGroup());
            Groups.Add(buttonGroup);
        }

        public async Task Load()
        {
            var x = 0;
            var max = 20;
            var y = 0;

            var items = ResourceFileParser.GetKeysBasedOn(@"Common\StandardStyles.xaml", "AppBarButtonStyle");
            t = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(50) };
            t.Tick += (s, e) =>
                          {
                              t.Stop();

                              for (int index = x; index < items.Count; index++)
                              {
                                  y++;
                                  var i = items[index];
                                  buttonGroup.Items.Add(i);

                                  if (y % max == 0)
                                  {
                                      x = index;
                                      if (y < items.Count)
                                          t.Start();
                                      break;
                                  }
                              }
                          };

            t.Start();
        }
    }
}
