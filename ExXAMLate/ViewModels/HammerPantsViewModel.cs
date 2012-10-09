using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Input;
using System.Xml.Linq;
using Okra.Core;
using Okra.Navigation;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI;

namespace ExXAMLate.ViewModels
{
    public class HammerPantsViewModel : NotifyPropertyChangedBase
    {
        private readonly INavigationManager _navigation;
        private Color _selectedColor;

        public Color SelectedColor
        {
            get { return _selectedColor; }
            set
            {
                _selectedColor = value;
                OnPropertyChanged();
            }
        }

        public ICommand GoBack { get; set; }
        public ICommand Generate { get; set; }
        public HammerPantsViewModel(INavigationManager navigation)
        {
            _navigation = navigation;
            GoBack = new DelegateCommand(() => _navigation.GoBack());
            Generate = new DelegateCommand(async () =>
                {
                    var y = XDocument.Load(@"Assets\Generic.xaml");
                    var x = new Hammer.Pants.ResourceFileParser(y);
                    var newDoc = x.Update(SelectedColor.ToString());

                    var fsp = new FileSavePicker() {SuggestedFileName = "Generic.xaml"};
                    fsp.FileTypeChoices.Add("XAML Document", new List<string> { ".xaml" });
                    var file = await fsp.PickSaveFileAsync();
                    if (file == null)
                        return;
                    var stream = await file.OpenStreamForWriteAsync();
                    newDoc.Save(stream);
                });
        }
    }
}
