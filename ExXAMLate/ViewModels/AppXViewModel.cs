using System;
using System.Collections.ObjectModel;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using ExXAMLate.Common;
using ExXAMLate.Models;
using Okra.Core;
using Okra.Navigation;
using Windows.Storage;
using Windows.Storage.Pickers;
using System.IO;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace ExXAMLate.ViewModels
{
    public class AppXViewModel : NotifyPropertyChangedBase
    {
        public ObservableCollection<AppXIcon> Icons { get; set; }
        public AppXModel Model
        {
            get { return _model; }
            set
            {
                _model = value;
                OnPropertyChanged();
            }
        }

        private readonly INavigationManager _navigation;
        private AppXModel _model;
        public ICommand GoBack { get; set; }

        public AppXViewModel(INavigationManager navigation)
        {
            _navigation = navigation;
            Icons = new ObservableCollection<AppXIcon>();
            GoBack = new DelegateCommand(() => _navigation.GoBack());
            Load();
        }

        public async Task Load()
        {
            try
            {
                Windows.UI.ViewManagement.ApplicationView.TryUnsnap();

                var f = new FileOpenPicker()
                    {
                        FileTypeFilter = { ".appx" }
                    };

                var file = await f.PickSingleFileAsync();
                if (file == null)
                    return;


                var model = new AppXModel();
                var zip = new ZipArchive((await file.OpenReadAsync()).AsStream(), ZipArchiveMode.Read);
                var manifest = zip.GetEntry("AppxManifest.xml");

                XNamespace ns = "http://schemas.microsoft.com/appx/2010/manifest";
                var doc = XDocument.Load(manifest.Open());
                var package = doc.Element(ns + "Package");
                var application = package.Element(ns + "Applications").Element(ns + "Application");
                var visuals = application.Element(ns + "VisualElements");

                model.DisplayName = visuals.Attribute("DisplayName").Value;
                var logo = new AppXIcon() { Description = "The \"regular\" logo is displayed as your square tile. It measures 150x150", Title = "Logo" };
                if (visuals.Attribute("Logo") != null)
                {
                    model.Logo = visuals.Attribute("Logo").Value;
                    var logoFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(model.DisplayName + ".png", CreationCollisionOption.ReplaceExisting);
                    await zip.GetEntry(model.Logo.Replace('\\', '/')).SaveToFile(logoFile);
                    logo.Image = new BitmapImage(new Uri(logoFile.Path));
                }

                var smallLogo = new AppXIcon { Description = "The small logo is displayed in search results, \"show all apps\", and in the search charm (vertical list). It measures 30x30.", Title = "Small Logo" };
                if (visuals.Attribute("SmallLogo") != null)
                {
                    var smalllogoFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(model.DisplayName + "small.png", CreationCollisionOption.ReplaceExisting);
                    await zip.GetEntry(visuals.Attribute("SmallLogo").Value.Replace('\\', '/')).SaveToFile(smalllogoFile);
                    smallLogo.Image = new BitmapImage(new Uri(smalllogoFile.Path));
                }

                var storeLogo = new AppXTile { Description = "The store logo is visible on the Windows Store on the web and within the Windows Store app.", Title = "Store Logo", AppTitle = model.DisplayName};
                if (package.Element(ns + "Properties").Element(ns + "Logo") != null)
                {
                    var storelogoimage = await ApplicationData.Current.LocalFolder.CreateFileAsync(model.DisplayName + "store.png", CreationCollisionOption.ReplaceExisting);
                    await zip.GetEntry(package.Element(ns + "Properties").Element(ns + "Logo").Value.Replace('\\', '/')).SaveToFile(storelogoimage);
                    storeLogo.Image = new BitmapImage(new Uri(storelogoimage.Path));
                }


                //if (visuals.Attribute("WideLogoImage") != null)
                //{
                //    //model.SmallLogo = visuals.Attribute("WideLogo").Value;
                //    var widelogoimage = await ApplicationData.Current.LocalFolder.CreateFileAsync(model.DisplayName + "wide.png", CreationCollisionOption.ReplaceExisting);
                //    await zip.GetEntry(model.SmallLogo.Replace('\\', '/')).SaveToFile(widelogoimage);
                //    model.WideLogoImage = new BitmapImage(new Uri(widelogoimage.Path));
                //}

                model.Background = new SolidColorBrush(ColorExtensions.FromString(visuals.Attribute("BackgroundColor").Value));

                Model = model;
                Icons.Add(smallLogo);
                Icons.Add(storeLogo);
                Icons.Add(logo);
            }
            catch (Exception o_0)
            {

            }
        }
    }
}
